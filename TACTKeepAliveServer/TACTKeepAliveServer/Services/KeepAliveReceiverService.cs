using System.Net;
using System.Net.Sockets;
using System.Threading.Channels;
using System.Buffers;
using Serilog;
using TACT.KeepAliveServer;

namespace TACT.KeepAliveServer.Services;

public class KeepAliveReceiverService : BackgroundService
{
    private readonly ChannelWriter<KeepAliveMsg> _channelWriter;
    private readonly IConfiguration _config;
    private const int DefaultPort = 40001;

    public KeepAliveReceiverService(Channel<KeepAliveMsg> channel, IConfiguration config)
    {
        _channelWriter = channel.Writer;
        _config = config;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int port = _config.GetValue("ServerPort", DefaultPort);
        using var udpClient = new UdpClient(port);
        
        Log.Information("UDP KeepAlive Receiver started on port {Port}", port);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = await udpClient.ReceiveAsync(stoppingToken);
                
                // 패킷 처리 작업 (별도 스레드에서 실행하여 수신 지연 방지)
                _ = ProcessPacketAsync(result);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error receiving UDP packet");
            }
        }
    }

    private async Task ProcessPacketAsync(UdpReceiveResult result)
    {
        try
        {
            // Span<byte> 최적화 디코딩 로직 호출 (KeepAliveMsg에서 구현 예정)
            var msg = DecodePacket(result.Buffer, result.RemoteEndPoint);
            if (msg != null)
            {
                await _channelWriter.WriteAsync(msg);
            }
        }
        catch (Exception ex)
        {
            Log.Debug(ex, "Failed to decode packet from {EndPoint}", result.RemoteEndPoint);
        }
    }

    private KeepAliveMsg? DecodePacket(byte[] buffer, IPEndPoint endPoint)
    {
        try 
        {
            // Span<byte>를 사용하여 불필요한 할당 없이 디코딩 수행
            ReadOnlySpan<byte> source = buffer;
            
            // Base64 디코딩에 필요한 버퍼 크기 계산 및 대여
            int decodedLength = (source.Length * 3) / 4;
            byte[] decodedBuffer = ArrayPool<byte>.Shared.Rent(decodedLength);
            
            try
            {
                if (System.Buffers.Text.Base64.DecodeFromUtf8(source, decodedBuffer, out _, out int consumed) == System.Buffers.OperationStatus.Done)
                {
                    var msg = new KeepAliveMsg
                    {
                        RecvDateTime = DateTime.Now,
                        RecvIPAddress = endPoint.Address.ToString(),
                        RecvPort = endPoint.Port
                    };
                    msg.Decode(decodedBuffer.AsSpan(0, consumed));
                    return msg;
                }
                return null;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(decodedBuffer);
            }
        }
        catch (Exception ex)
        {
            Log.Debug(ex, "Failed to decode packet via Span from {EndPoint}", endPoint);
            return null;
        }
    }
}
