# RACTServer 고성능 최적화 실행 계획 (Performance Optimization Plan)

분석 보고서([performance_investigation.md](file:///C:/Users/oovka/.gemini/antigravity/brain/c8234531-fbda-4d1b-b96f-b9689312f516/performance_investigation.md))를 바탕으로, 수천 명의 동시 접속을 효율적으로 처리하기 위한 수술을 진행합니다.

## 제안된 변경 사항

### 1. 전역 세션 관리 최적화 (Lock-Free Lookup)
- **[MODIFY] [UserInfoCollection.cs](file:///d:/dev/skbb/tact-origin-refac/tact-origin/RACTCommon/RACTCommonClass/Data/UserInfoCollection.cs)**:
    - 내부 `InnerList` 검색 방식을 `Dictionary<int, UserInfo>` 기반으로 변경하거나, 가능하면 `ConcurrentDictionary`를 도입하여 `this[int aID]` 접근 시의 **O(N) 검색 병목**을 **O(1)**로 개선합니다.

### 2. 사용자별 데이터 큐 병목 제거
- **[MODIFY] [UserInfo.cs](file:///d:/dev/skbb/tact-origin-refac/tact-origin/RACTCommon/RACTCommonClass/Data/UserInfo.cs)**:
    - `private Queue m_DataQueue`를 `ConcurrentQueue<byte[]>`로 전환합니다.
- **[MODIFY] [ClientCommunicationProcess.cs](file:///d:/dev/skbb/tact-origin-refac/tact-origin/RACTServer/RACTServer/ClientCommunicationProcess.cs)**:
    - `ResultSender` 등에서 `lock (tUserInfo.DataQueue.SyncRoot)`를 사용하는 대신 `tUserInfo.DataQueue.TryDequeue`를 사용하도록 변경합니다.

### 3. 레거시 DB 풀 및 스레드 구조 정리
- **[MODIFY] [RACTServer.cs](file:///d:/dev/skbb/tact-origin-refac/tact-origin/RACTServer/RACTServer/RACTServer.cs)**:
    - `InitializeServer()`와 `Stop()`에서 `MKOleDBPool` 관련 코드를 모두 제거합니다.
- **[MODIFY] [GlobalClass.cs](file:///d:/dev/skbb/tact-origin-refac/tact-origin/RACTServer/RACTServer/GlobalClass.cs)**:
    - `m_DBPool`, `m_DBExecutePool` 필드를 제거하거나 Obsolete 처리합니다.
    - `GetSqlConnection()`의 ConnectionString 생성 부하를 줄이기 위해 정적 필드로 캐싱합니다.

### 4. 워커 스레드 확장 (Throughput 상한 해제)
- **[MODIFY] [ClientCommunicationProcess.cs](file:///d:/dev/skbb/tact-origin-refac/tact-origin/RACTServer/RACTServer/ClientCommunicationProcess.cs)**:
    - 고정된 `Thread[]` 배열 대신 `.NET ThreadPool`을 활용하거나, 워커 스레드 수를 넉넉하게 확장합니다. (현재 DBConnectionCount / 3 제약 제거)

## 검증 계획
1. **O(1) 성능 확인**: 수천 명의 사용자 세션이 있을 때 로그인/요청 처리가 지연 없이 처리되는지 로직 점검.
2. **동시성 테스트**: `ConcurrentQueue` 도입 후 데이터 누락이나 예외(InvalidOperationException)가 없는지 확인.
3. **리소스 절약**: `MKOleDBPool` 제거 후 서버 시작 시의 메모리 소모 및 DB 연결 수 감소 확인.
