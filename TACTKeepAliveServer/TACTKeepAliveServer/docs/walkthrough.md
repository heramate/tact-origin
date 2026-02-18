# TACT KeepAlive Server .NET 9 리팩토링 워크스루

레거시 C# 프로젝트를 최신 .NET 9 아키텍처로 성공적으로 전환했습니다. 주요 변경 사항과 성과는 다음과 같습니다.

## 🚀 주요 성과

- **성공적인 빌드**: 모든 레거시 종속성(`MKLibrary`, `RACTCommonClass`)을 제거하고 .NET 9 SDK 환경에서 빌드 성공을 확인했습니다.
- **Dapper 통합**: 기존 ADO.NET 호출을 제거하고 Dapper를 사용한 비동기 DB 레이어로 전환하여 처리 효율을 높였습니다.
- **Kestrel & Minimal APIs**: 서버 실행 모델을 Kestrel 기반으로 현대화하고, 상태 체크를 위한 엔드포인트를 추가했습니다.
- **고성능 Span 기반 파싱**: 
    - `KeepAliveClass.cs`: `Span<byte>`를 사용하여 메모 할당 없이 TLV 데이터를 파싱합니다.
    - `KeepAliveReceiverService.cs`: `Base64.DecodeFromUtf8`과 `ArrayPool`을 사용하여 수신 데이터부터 파싱까지 전 과정을 `Span` 기반으로 최적화했습니다. (문자열 변환 단계 제거)
- **현대적 관찰성**: Serilog(비동기 로깅)와 Prometheus(메트릭 수집)를 통합하여 실시간 모니터링 기능을 강화했습니다.

## 🛠 아키텍처 변화

````mermaid
graph TD
    UDP["UDP 수신 (Port 40001)"] --> Receiver["KeepAliveReceiverService"]
    Receiver -- "Span<byte> 파싱" --> Channel["System.Threading.Channels"]
    Channel --> DBUpdate["BatchDbUpdateService"]
    DBUpdate --> Dapper["Dapper (DB 연동)"]
    
    Kestrel["Kestrel Server"] --> Metrics["Prometheus Metrics /metrics"]
    Kestrel --> Health["Health Check /"]
    
    Logging["Serilog (Async)"] <==> All["전체 컴포넌트"]
````

## 📂 리팩토링된 주요 파일

- **[Program.cs](file:///d:/dev/skbb/tact-origin/TACTKeepAliveServer/TACTKeepAliveServer/Program.cs)**: Kestrel, DI, Serilog, Prometheus 통합의 중심점
- **[KeepAliveClass.cs](file:///d:/dev/skbb/tact-origin/TACTKeepAliveServer/TACTKeepAliveServer/Data/KeepAliveClass.cs)**: `Span<byte>` 기반의 최적화된 패킷 디코더
- **[KeepAliveReceiverService.cs](file:///d:/dev/skbb/tact-origin/TACTKeepAliveServer/TACTKeepAliveServer/Services/KeepAliveReceiverService.cs)**: `Base64.DecodeFromUtf8`을 사용한 고성능 수신 서비스

## ✅ 검증 결과

- **Build Status**: `Succeeded` (0 Errors, 0 Critical Warnings)
- **Decoding Performance**: 불필요한 `String` 할당 및 `byte[]` 복사 제거 완료
