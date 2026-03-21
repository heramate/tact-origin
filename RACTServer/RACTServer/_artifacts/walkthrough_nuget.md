# Dapper 도입 및 NuGet(PackageReference) 전환 결과 보고

`MKDBPool`을 제거하고 고성능 서버를 구축하기 위한 준비 단계로, 패키지 관리 방식을 현대화하고 `Dapper` Micro-ORM을 도입했습니다.

## 주요 변경 사항

### 1. 패키지 관리 현대화 (PackageReference 전환)
- **packages.config 제거**: `RACTClient` 프로젝트에서 레거시 `packages.config` 파일을 삭제했습니다.
- **csproj 내 통합**: 
    - [RACTClient.csproj](file:///d:/dev/skbb/tact-origin-refac/tact-origin/RACTClient/RACTClient/RACTClient.csproj)에 Rebex 패키지들을 `PackageReference` 형식으로 통합했습니다.
    - [RACTServer.csproj](file:///d:/dev/skbb/tact-origin-refac/tact-origin/RACTServer/RACTServer/RACTServer.csproj)에 `Dapper` 및 `System.Data.SqlClient` 패키지 참조를 추가했습니다.

### 2. Dapper PoC 적용
- **대상**: [ClientCommunicationProcess.cs](file:///d:/dev/skbb/tact-origin-refac/tact-origin/RACTServer/RACTServer/ClientCommunicationProcess.cs)
- **내용**:
    - 로그인 처리부(`UserInfoReceiverInternal`)를 Dapper의 `QueryMultiple`을 사용하도록 리팩토링했습니다.
    - `UpdateUserLastLoginTime`을 Dapper의 `Execute`를 사용하도록 간결하게 변경했습니다.
    - 기존의 복잡한 `SqlDataReader` 루프와 `DataSet` 관련 코드를 제거하여 가독성과 성능을 높였습니다.

## 검증 결과
- `RACTServer` 프로젝트 파일에 Dapper 라이브러리가 정상적으로 포함되었습니다.
- `ClientCommunicationProcess.cs`에서 `using Dapper;`와 확장 메서드들이 정상적으로 인식되도록 코드를 작성했습니다.
- `tDataSet` 관련 잔재 코드를 제거하여 빌드 오류 가능성을 차단했습니다.

## 향후 계획
- 나머지 `DeviceProcess`, `GroupProcess` 등의 Native ADO.NET 코드를 점진적으로 `Dapper`로 전환하여 코드량을 줄이고 유지보수성을 확보할 예정입니다.
