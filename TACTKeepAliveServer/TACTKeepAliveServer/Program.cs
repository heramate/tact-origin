using System.Threading.Channels;
using Serilog;
using Prometheus;
using TACT.KeepAliveServer;
using TACT.KeepAliveServer.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Serilog 비동기 로깅 설정
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Async(a => a.File("logs/tact_server.log", rollingInterval: RollingInterval.Day))
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

// 2. 서비스 등록
builder.Services.AddSingleton(Channel.CreateUnbounded<KeepAliveMsg>());
builder.Services.AddHostedService<KeepAliveReceiverService>();
builder.Services.AddHostedService<BatchDbUpdateService>();

// DBWorker 초기화

var app = builder.Build();

// 3. Prometheus 메트릭 노출
app.UseMetricServer();
app.UseHttpMetrics();

app.MapGet("/", () => "TACT KeepAlive Server is Running (.NET 9)");
app.MapGet("/metrics", () => Results.Ok()); // prometheus-net handles this via Middleware

try
{
    Log.Information("Starting TACT KeepAlive Server...");
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
