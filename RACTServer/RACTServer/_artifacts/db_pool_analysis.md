# [gstack] /review: DB Pool 성능 병목 심층 영향도 검토

현재 RACTServer 아키텍처에서 데이터베이스 커넥션 풀(`MKDBPool`)은 성능을 결정짓는 최상위 병목점(Bottleneck)이자 전체 동시성을 제한하는 핵심 자원입니다. 이번 스레드 병렬화 작업 이후, DB 풀과 스레드 풀 간의 상호작용(Interaction)에서 발생할 수 있는 잠재적 영향도를 분석했습니다.

---

## 1. 아키텍처 초과 구독 (Over-subscription) 문제 파악
현재 서버 기동 시 `ClientCommunicationProcess`와 `ClientResponseProcess`는 각각 `(DBConnectionCount - 2)`개의 워커 스레드를 배열로 생성하도록 설정되었습니다. (앞선 1차 최적화 반영 사항)
더불어 로그인(`UserInfoReceiver`) 통신부 또한 최대 `DBConnectionCount`개의 동시성을 `SemaphoreSlim`으로 허용하고 있습니다.

이를 종합하면, **DB Pool 크기는 한정되어 있음에도 불구하고 이를 획득하려고 대기하는 활성 스레드는 DB 풀 크기의 최대 3배**에 이르게 됩니다.
- (예시) `DBConnectionCount = 30` 일 때:
  - `m_DBPool` 활성 커넥션 개수 = 30개
  - `CommunicationProcess` 워커 스레드 = 28개
  - `ResponseProcess` 워커 스레드 = 28개
  - 로그인 쇄도 임시 스레드 (`SemaphoreSlim` 통과자) = 30개
  - **합계: 최대 86개의 스레드가 30개의 커넥션을 차지하려 경쟁!**

## 2. 병목 시 파급 효과 (Impact Assessment)
만약 86개의 스레드가 동시에 `GlobalClass.m_DBPool.GetDBWorkItem()`을 호출하여 DB 연결을 요구할 경우, 30개는 정상적으로 할당받지만 나머지 56개는 대기 상태에 들어갑니다.

여기서 사용된 자체 라이브러리 `MKLibrary.MKData.MKDBPool`의 내부 구현 방식에 따라 서버의 운명이 결정됩니다.
1. **Timeout 기반 (가장 위험)**: Timeout 기능이 켜져 있다면, 일정 시간(예: 30초) 대기 후 `NullReferenceException`이나 `DBProcessError`가 터지며 56건의 클라이언트 요청이 허무하게 증발합니다.
2. **무한 대기(Block) 기반**: 락(`Monitor.Enter` / `AutoResetEvent`)을 통해 단순히 커넥션이 반환될 때까지 대기한다면, 클라이언트는 지연(Latency)은 겪겠지만 예외는 발생하지 않아 가장 안전한 상태가 유지됩니다. 
3. **Spin-Lock 기반**: `while(true) Thread.Sleep(1)` 같은 방식으로 대기한다면, 56개의 스레드가 CPU를 혹사시켜 서버 전체 응답성이 붕괴될 수 있습니다.

## 3. 극복 및 추가 최적화 권고사항 (Staff Engineer Recommendations)

이러한 DB Pool 초과 구독 및 병목 현상을 타파하여 서버 확장 능력을 안정적으로 가져가기 위해 다음 조치들을 권고합니다.

**✅ 권고 1: 워커 스레드 개수 할당 공식의 분산 조정**
스레드의 무한 증식을 막기 위해, 다음과 같이 각 프로세스의 역할을 고려하여 워커 스레드를 할당해야 합니다.
- `ClientCommunicationProcess` 쓰레드 수: `Math.Max(2, DBConnectionCount / 3)`
- `ClientResponseProcess` 쓰레드 수: `Math.Max(2, DBConnectionCount / 3)`
- 결과적으로 스레드 총합이 `DBConnectionCount`를 크게 넘지 않도록 균형을 맞춰 DB 연결 획득 경합을 물리적으로 제한합니다. (불필요한 컨텍스트 스위칭 예방)

**✅ 권고 2: DB 풀 자체의 확장 (Scale-Up)**
궁극적으로 수천 대의 클라이언트를 버티기 위해서는 물리적인 DB `DBConnectionCount` 설정값을 상향해야 합니다 (예: 100~200). `SystemInfo.xml` 등의 설정 파일에서 가용 DB 커녁션 풀 수치를 MS-SQL 스펙에 맞게 최대치로 조정하는 외부 환경 정비가 필요합니다.

---
**진행 가이드**: 위의 `권고 1(워커 스레드 공식 조정)`을 기존 소스코드에 적용하여 DB Pool 경합을 사전에 예방하는 형태로 튜닝할까요?
