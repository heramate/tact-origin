```csharp
using Serilog;
using Prometheus;
using TACT.KeepAliveServer.Services;
using TACT.KeepAliveServer.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Serilog 비동기 구조적 로깅 설정
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Async(a => a.File("logs/tact_server.log", rollingInterval: RollingInterval.Day))
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// 2. 서비스 등록
builder.Services.AddSingleton<IDBWorker, DapperDBWorker>(); // 리팩토링된 Dapper 워커
builder.Services.AddHostedService<KeepAliveReceiverService>(); // UDP 수신 백그라운드 서비스
builder.Services.AddHostedService<BatchDbUpdateService>(); // DB 배치 업데이트 서비스

var app = builder.Build();

// 3. Prometheus 메트릭 노출 (/metrics)
app.UseMetricServer();
app.UseHttpMetrics();

// 4. Minimal API 엔드포인트 (기존 데몬 통신 대체 예시)
app.MapGet("/api/status", () => Results.Ok(new { Status = "Running", Timestamp = DateTime.UtcNow }));

app.MapPost("/api/tunnel/open", async (TunnelRequest request, IDBWorker db) => {
    Log.Information("Received Tunnel Open Request for {DeviceIP}", request.DeviceIP);
    // 터널 오픈 로직 비동기 호출
    return Results.Accepted();
});

try
{
    Log.Information("Starting TACT KeepAlive Server (Powerd by Kestrel)...");
    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Server terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
```
