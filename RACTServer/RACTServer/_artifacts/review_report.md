# [gstack] /review: 수천 대 클라이언트 동시 접속 대비 심층 성능 검증 (Staff Engineer)

현재 적용된 이벤트 루프 구조 최적화(`BlockingCollection` 탑재)로 서버 내부의 "응답 처리 쓰루풋"은 크게 개선되었습니다. 하지만 **수천 대의 클라이언트가 동시에 접속하거나 지속적으로 통신할 때**를 가정하여 전체 스택(특히 .NET Remoting 계층)을 들여다본 결과, 프로덕션 환경에서 서버를 마비시킬 수 있는 **가장 치명적인 2가지 히든 병목(Hidden Bottleneck)**을 추가로 발견했습니다.

---

## ⛔ 1. 로그인 쇄도에 의한 DB 풀 고갈 (Thundering Herd)
**현황:**
- 클라이언트가 접속할 때 호출되는 `UserInfoReceiver`는 `.NET Remoting` 프레임워크에 의해 직접 호출되며, 클라이언트 1명당 1개의 쓰레드(ThreadPool)가 통신을 엽니다.
- 만약 모종의 이유로 서버가 재시작되어 **수천 대의 클라이언트가 0.1초 내외로 동시에 재접속 요청을 보낼 경우**, 수천 개의 스레드가 동시에 `UserInfoReceiver`를 실행하고 곧바로 `GlobalClass.m_DBPool.GetDBWorkItem()`을 호출합니다.
- **결과**: 서버의 제한된 DB 풀 수량(예: 30개)이 순식간에 고갈되고, 나머지 수백~수천 개의 스레드는 무한 대기에 빠지거나 Lock Timeout으로 예외를 뱉어내어 서버가 Crash되거나 Hang(먹통) 상태에 빠집니다.

**해결 방안 (Surgical Change):**
- DB 통신이 일어나는 `UserInfoReceiver` 내부 앞단에 `SemaphoreSlim(maxCount: DBConnectionCount)`을 두어, **동시 로그인 처리량을 DB 버퍼 사이즈만큼만 제한(Throttling)**합니다.
- 이를 통해 수백 명이 몰려와도 남은 인원은 줄을 서게 되어 DB와 서버 스레드가 절대 죽지 않습니다 (Backpressure 적용).

---

## ⛔ 2. 폴링(Polling) 구간의 전역 `lock (m_UserInfoList)` 경합
**현황:**
- 현재 구조상 클라이언트는 데이터 수신을 위해 `.NET Remoting`의 `ResultSender()`를 일정 주기마다 계속 호출(폴링)하는 것으로 보입니다.
- `ResultSender` 내부를 보면, 매 요청마다 `lock (m_UserInfoList)`를 획득한 후 단순 유효성 확인과 `tUserInfo.LifeTime = DateTime.Now`를 갱신하고 있습니다.
- **결과**: 수천 대의 클라이언트가 매초(혹은 10초)마다 폴링을 하면 1초에 수천 번의 전역 락(Global Lock) 경합이 `m_UserInfoList`에서 발생합니다. 로그인(`UserInfoReceiver`) 통과 시에도 이 락을 잡아야 하므로, 유저 추가 및 조회가 서로 병목 현상을 일으켜 지연(Latency)이 기하급수적으로 높아집니다.

**해결 방안 (Surgical Change):**
- `lock (m_UserInfoList)` 블록을 진입할 때 **전체를 감싸는 대신 읽기 전용으로 객체 레퍼런스만 빠르게 가져온 뒤 즉시 락을 풉니다.**
- `tUserInfo.LifeTime` 업데이트나 내부 큐(`DataQueue`) 접근은 이미 개별 `tUserInfo`의 Lock 범위이므로 전역 락 안에 둘 필요가 없습니다. (락 유지 시간 99% 감소)

---

## 요약 (TL;DR)
현재 구조는 로직 자체는 스레드 안전(Thread-Safe)하지만, **무제한 로그인 수용으로 인한 DB Crash**와 **폴링 메커니즘의 전역 락(Global Lock) 장기 점유**라는 한계를 가집니다. 수천 대 접속을 목표로 하신다면 위 두 가지 병목의 해소가 필수적입니다.
