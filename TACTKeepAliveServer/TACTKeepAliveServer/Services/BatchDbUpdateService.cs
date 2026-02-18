using System.Threading.Channels;
using Serilog;
using TACT.KeepAliveServer;

namespace TACT.KeepAliveServer.Services;

public class BatchDbUpdateService : BackgroundService
{
    private readonly ChannelReader<KeepAliveMsg> _channelReader;
    private const int BatchSize = 100;
    private const int BatchIntervalMs = 5000;

    public BatchDbUpdateService(Channel<KeepAliveMsg> channel)
    {
        _channelReader = channel.Reader;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Log.Information("Batch DB Update Service started.");
        
        var batch = new List<KeepAliveMsg>(BatchSize);
        var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(BatchIntervalMs));

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // 채널에서 데이터 읽기 (비동기)
                if (await _channelReader.WaitToReadAsync(stoppingToken))
                {
                    while (_channelReader.TryRead(out var msg))
                    {
                        batch.Add(msg);
                        if (batch.Count >= BatchSize)
                        {
                            await FlushBatchAsync(batch);
                        }
                    }
                }

                // 주기적 플러시 (BatchSize 미달 시에도 시간 기반으로 처리)
                if (batch.Count > 0)
                {
                    await FlushBatchAsync(batch);
                }

                await timer.WaitForNextTickAsync(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error in Batch DB Update process");
            }
        }
    }

    private async Task FlushBatchAsync(List<KeepAliveMsg> batch)
    {
        try
        {
            Log.Information("Flushing {Count} KAM records to DB", batch.Count);
            
            // 병렬 또는 순차 비동기 업데이트 실행
            var tasks = batch.Select(msg => DBWorker.Update_LTE_NE_Async(msg));
            await Task.WhenAll(tasks);
            
            batch.Clear();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to flush KAM batch to DB");
        }
    }
}
