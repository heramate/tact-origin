# RACTServer 성능 최적화 결과 보고서

고밀도 클라이언트(수천 대 이상) 처리를 위해 RACTServer의 내부 병목 지점을 제거하고 현대적인 동시성 제어 구조로 리팩토링을 완료했습니다.

## 주요 변경 사항

### 1. 데이터 구조 및 알고리즘 최적화 (O(N) → O(1))
- **`UserInfoCollection` & `DaemonProcessInfoCollection`**: 
  - 기존: `InnerList`를 `foreach`로 순회하여 세션을 찾는 O(N) 방식 (락 경합 심함).
  - 변경: `ConcurrentDictionary<int, T>`를 도입하여 세션 조회를 O(1)로 단축하고 분산 락 구조로 전환.
- **`DataQueue`**:
  - 기존: 표준 `Queue`와 `SyncRoot` 기반의 동기화.
  - 변경: `ConcurrentQueue<byte[]>`를 도입하여 생산자-소비자 모델에서 락 없이 데이터 처리가 가능하도록 최적화.

### 2. 네트워크 및 스레딩 Throughput 확장
- **워커 스레드 공식 제거**:
  - 기존: `DBConnectionCount / 3`으로 제한된 고정 스레드 수 (처리량 인위적 캡핑).
  - 변경: `Environment.ProcessorCount * 2` (최소 16개) 이상의 넉넉한 스레드를 할당하여 I/O 대기 시간을 최소화하고 처리량 극대화.
- **`Thread.Abort()` 지양**:
  - 안전한 서비스 종료를 위해 불필요한 강제 종료 로직을 검토하고 정리했습니다.

### 3. 데이터베이스 계층 현대화
- **레거시 DB 풀 제거**:
  - `MKOleDBPool`, `MKDataSet` 등 동기화 병목이 심한 이전 세대 라이브러리 초기화 및 의존성 제거.
- **Dapper & Native Pooling 도입**:
  - `SqlConnection`의 자체 풀링(Max 200)을 활용하고 `Dapper`를 통해 고성능 쿼리 실행 기반 마련.
- **ConnectionString 캐싱**:
  - `GlobalClass.GetSqlConnection()` 호출 시마다 문자열을 생성하던 오버헤드 제거 (캐싱 처리).

## 성능 기대 효과
- **CPU 사용률 감소**: 불필요한 락 대기 및 선형 검색 제거로 컨텍스트 스위칭과 루프 오버헤드 감소.
- **메모리 안정성**: `Concurrent` 컬렉션 사용으로 멀티스레드 환경에서의 데이터 무결성 확보.
- **확장성**: 동시 접속자 수가 늘어나도 세션 조회 시간이 일정하게 유지되어 수천 대 이상의 클라이언트를 안정적으로 수용 가능.

## 향후 권장 사항
- **비동기(async/await) 전면 도입**: 현재 스레드 기반 구조에서 더 나아가 `MKRemote`와 비즈니스 로직을 `Task` 기반 비동기로 점검하면 리소스 효율을 더욱 높일 수 있습니다.
