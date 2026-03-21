# [gstack] /plan-eng-review: MKDBPool -> Dapper 마이그레이션 전략

> **역할**: Eng Manager
> **주요 목표**: 아키텍처 통신 레이어(.NET Remoting, 큐)는 100% 무결하게 유지하면서, 서버 내부의 악성 병목인 `MKDBPool`을 가장 모던하고 빠른 `Dapper`로 우회(Bypass/Surge)하는 수술 계획 수립.

## 1. 아키텍처 결정 (Architecture Lock-in)
### 1.1. Native Connection Pooling의 활용
- 기존에는 제한된 개수의 `MKDBWorkItem`을 순회하며 락을 걸고 할당받는 커스텀 풀(`MKDBPool`)을 사용했습니다.
- 이제는 `System.Data.SqlClient.SqlConnection`을 직접 생성하여 사용합니다. 이는 .NET 내부의 C/C++ 레벨 최적화된 Native Connection Pool을 타게 되므로 커넥션 할당/반환에 드는 CPU 병목(Spin-lock 등)이 완벽히 사라집니다.

### 1.2. Micro-ORM (Dapper) 도입
- 기존의 무거운 `MKDataSet`과 `OleDbAdapter` 대신, `Dapper`의 `Query()`, `Execute()` 확장 메서드를 사용하여 데이터베이스 호출과 마이핑(Mapping) 오버헤드를 제로(0)에 가깝게 만듭니다.

### 1.3. 스레드 인터페이스 유지 (Surgical Approach)
- 네트워크 계층 모델(`.NET Remoting`)의 한계상 `UserInfoReceiver` 등의 콜백은 동기식(`byte[]`)으로 묶여 있습니다.
- 따라서 내부를 무리하게 `async/await`로 변경하지 않고, 기존 워커 스레드나 풀 내에서 동기식 `Dapper.Query()`를 실행하되 쿼리 실행 자체의 폭발적인 속도 향상과 락 프리(Lock-Free) 이점을 취합니다.

## 2. 작업 상세 대상 및 순서 (Proposed Changes)

### 1. 공통 유틸리티 팩토리 구축
#### [NEW] `d:\dev\skbb\tact-origin-refac\tact-origin\RACTServer\RACTServer\DBConnectionFactory.cs` (또는 GlobalClass에 편입)
- `SystemInfo`의 접속 정보를 취합하여 `ConnectionString`을 생성하고, `GetSqlConnection()`을 반환하는 정적(Static) 유틸리티를 하나 개설합니다. (Max Pool Size = 200 등 넉넉하게 지정)

### 2. Dapper 라이브러리 추가
#### [MODIFY] `d:\dev\skbb\tact-origin-refac\tact-origin\RACTServer\RACTServer\RACTServer.csproj`
- 패키지 참조 추가 (`System.Data.SqlClient` 및 `Dapper`).

### 3. 로그인 통신부 전환 (PoC 수술)
#### [MODIFY] `d:\dev\skbb\tact-origin-refac\tact-origin\RACTServer\RACTServer\ClientCommunicationProcess.cs`
- 대규모 덩어리인 `UserInfoReceiverInternal` 내부의 `GlobalClass.m_DBPool.GetDBWorkItem()` 부분을 삭제.
- `using (var conn = GlobalClass.GetSqlConnection()) { var userRow = conn.QuerySingleOrDefault(...) }` 형태로 Dapper 전환.

## 3. 검증 계획 (Verification Plan)
- PoC(Proof of Concept) 수술 부위인 `UserInfoReceiver`가 정상적으로 빌드되는지 확인.
- Dapper 적용 후 커넥션 누사(Leak)가 없는지 `using ()` 블럭 생명주기 검증.
- 성공 확인 후 나머지 Process (Device, Group 등)으로 일괄 전파.
