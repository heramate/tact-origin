# TACT KeepAlive Server 리팩토링 태스크

## 프로젝트 설정 및 환경 구축
- [x] .NET 9 SDK 스타일 프로젝트 파일로 전환 (`TACTKeepAliveServer.csproj`)
- [x] 필수 NuGet 패키지 식별 및 설치 (Dapper, Serilog, Prometheus, Microsoft.Data.SqlClient)

## 데이터 레이어 (Dapper) 리팩토링
- [x] `DBWorker.cs` 리팩토링 및 Dapper 통합
- [x] 비동기 데이터베이스 작업 구현
- [x] `TunnelInfo.cs` 등 데이터 모델 현대화 및 POCO 구조화

## 네트워크 및 인프라 (Kestrel) 리팩토링
- [x] `Program.cs` 설정 및 Kestrel 통합
- [x] Dependency Injection(DI) 설정
- [x] Prometheus 메트릭 수집 시스템 구축
- [x] Serilog 기반의 비동기 구조적 로깅 통합

## 주요 로직 현대화
- [x] `GlobalClass.cs` 레거시 종속성 (`MKLibrary`) 제거 및 정리
- [x] `KeepAliveReceiverService` 배경 서비스 구현 (UDP 수신)
- [x] `BatchDbUpdateService` 배경 서비스 구현 (DB 배치 업데이트)
- [x] `SSHTunnelManager.cs` 비동기 `Task` 기반으로 전환
- [x] `DaemonCommProcess.cs` 현대화 및 의존성 제거

## 최적화 및 안정화
- [x] `Span<byte>` 및 `ArrayPool`을 사용한 패킷 파싱 최적화 (`KeepAliveClass.cs`)
- [x] `Enum.cs` 통합 및 중복 정의 제거
- [x] 전체 솔루션 빌드 오류 해결 및 검증 완료
