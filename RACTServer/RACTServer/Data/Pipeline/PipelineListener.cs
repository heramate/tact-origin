using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using RACTCommonClass;

namespace RACTServer.Data.Pipeline
{
    /// <summary>
    /// .NET 10 기반 고성능 TCP 리스너 (System.IO.Pipelines 활용)
    /// </summary>
    public class PipelineListener
    {
        private TcpListener _listener;
        private readonly int _port;
        private readonly Action<byte[]> _onPacketReceived;
        private CancellationTokenSource _cts;

        public PipelineListener(string ip, int port, Action<byte[]> onPacketReceived)
        {
            _port = port;
            _onPacketReceived = onPacketReceived;
            _listener = new TcpListener(IPAddress.Parse(ip), port);
        }

        public void Start()
        {
            _cts = new CancellationTokenSource();
            _ = ListenAsync(_cts.Token);
            GlobalClass.m_LogProcess?.PrintLog(E_FileLogType.Infomation, $"고성능 Pipeline 채널 시작됨 (Port: {_port})");
        }

        public void Stop()
        {
            _cts?.Cancel();
            _listener?.Stop();
            GlobalClass.m_LogProcess?.PrintLog(E_FileLogType.Infomation, "고성능 Pipeline 채널 중단됨.");
        }

        private async Task ListenAsync(CancellationToken ct)
        {
            try
            {
                // 소켓 최적화 설정
                _listener.Server.NoDelay = true;
                _listener.Server.LingerState = new LingerOption(true, 0);
                
                _listener.Start(1000); // Backlog 설정

                while (!ct.IsCancellationRequested)
                {
                    Socket socket = await _listener.AcceptSocketAsync(ct);
                    _ = HandleConnectionAsync(socket, ct);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                GlobalClass.m_LogProcess?.PrintLog(E_FileLogType.Error, $"PipelineListener Error: {ex.Message}");
            }
        }

        private async Task HandleConnectionAsync(Socket socket, CancellationToken ct)
        {
            using (socket)
            {
                var receiver = new PipelineReceiver(socket, _onPacketReceived);
                await receiver.ProcessAsync(ct);
            }
        }
    }
}
