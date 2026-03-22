using System;
using System.Buffers;
using System.IO;
using System.IO.Pipelines;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using RACTCommonClass;

namespace RACTClient
{
    /// <summary>
    /// .NET Framework 4.8 클라이언트용 고성능 Pipeline 통신 모듈
    /// </summary>
    public class PipelineClient
    {
        private Socket _socket;
        private NetworkStream _stream;
        private readonly string _ip;
        private readonly int _port;
        private CancellationTokenSource _cts;
        private Task _receiveTask;

        public event Action<byte[]> PacketReceived;

        public PipelineClient(string ip, int port)
        {
            _ip = ip;
            _port = port;
        }

        public async Task<bool> ConnectAsync()
        {
            try
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _socket.NoDelay = true;
                
                await Task.Factory.FromAsync(_socket.BeginConnect, _socket.EndConnect, _ip, _port, null);
                
                _stream = new NetworkStream(_socket, false);
                _cts = new CancellationTokenSource();
                _receiveTask = RunReceiveLoopAsync(_cts.Token);
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Disconnect()
        {
            _cts?.Cancel();
            _stream?.Dispose();
            _socket?.Close();
        }

        private async Task RunReceiveLoopAsync(CancellationToken ct)
        {
            var reader = PipeReader.Create(_stream);

            try
            {
                while (!ct.IsCancellationRequested)
                {
                    ReadResult result = await reader.ReadAsync(ct);
                    ReadOnlySequence<byte> buffer = result.Buffer;

                    while (TryReadPacket(ref buffer, out byte[] packet))
                    {
                        PacketReceived?.Invoke(packet);
                    }

                    reader.AdvanceTo(buffer.Start, buffer.End);

                    if (result.IsCompleted) break;
                }
            }
            catch (OperationCanceledException) { }
            finally
            {
                await reader.CompleteAsync();
            }
        }

        private bool TryReadPacket(ref ReadOnlySequence<byte> buffer, out byte[] packet)
        {
            packet = null;
            if (buffer.Length < 4) return false;

            byte[] lengthBytes = buffer.Slice(0, 4).ToArray();
            int packetLength = BitConverter.ToInt32(lengthBytes, 0);

            if (buffer.Length < packetLength + 4) return false;

            packet = buffer.Slice(4, packetLength).ToArray();
            buffer = buffer.Slice(buffer.GetPosition(packetLength + 4));
            return true;
        }

        public async Task SendAsync(byte[] data)
        {
            if (_stream == null) return;

            // 헤더(길이) 전송
            byte[] header = BitConverter.GetBytes(data.Length);
            await _stream.WriteAsync(header, 0, 4);
            
            // 바디 전송
            await _stream.WriteAsync(data, 0, data.Length);
            await _stream.FlushAsync();
        }
    }
}
