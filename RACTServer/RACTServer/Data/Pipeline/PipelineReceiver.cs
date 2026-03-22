using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace RACTServer.Data.Pipeline
{
    /// <summary>
    /// .NET 10 기반 Pipeline 고성능 수신 처리기
    /// 클라이언트 수정 없이 서버 내부 수신 버퍼링을 최적화합니다.
    /// </summary>
    public class PipelineReceiver
    {
        private readonly Socket _socket;
        private readonly Action<byte[]> _onPacketReceived;

        public PipelineReceiver(Socket socket, Action<byte[]> onPacketReceived)
        {
            _socket = socket;
            _onPacketReceived = onPacketReceived;
        }

        public async Task ProcessAsync(CancellationToken ct)
        {
            var reader = PipeReader.Create(new NetworkStream(_socket, false));

            try
            {
                while (true)
                {
                    ReadResult result = await reader.ReadAsync(ct);
                    ReadOnlySequence<byte> buffer = result.Buffer;

                    while (TryReadPacket(ref buffer, out byte[]? packet))
                    {
                        if (packet != null) _onPacketReceived(packet);
                    }

                    reader.AdvanceTo(buffer.Start, buffer.End);

                    if (result.IsCompleted) break;
                }
            }
            finally
            {
                await reader.CompleteAsync();
            }
        }

        private bool TryReadPacket(ref ReadOnlySequence<byte> buffer, out byte[]? packet)
        {
            packet = null;

            // 헤더 정보(예: 4바이트 길이 정보)를 읽을 수 있는지 확인
            if (buffer.Length < 4) return false;

            // 길이 정보 추출 (Little Endian 가정)
            Span<byte> lengthBytes = stackalloc byte[4];
            buffer.Slice(0, 4).CopyTo(lengthBytes);
            int packetLength = BitConverter.ToInt32(lengthBytes);

            // 전체 패킷(헤더+바디)이 버퍼에 모두 들어왔는지 확인
            if (buffer.Length < packetLength + 4) return false;

            // 패킷 바디를 추출 (기존 로직 호환을 위해 byte[]로 변환)
            packet = buffer.Slice(4, packetLength).ToArray();

            // 읽은 만큼 버퍼 위치 전진
            buffer = buffer.Slice(buffer.GetPosition(packetLength + 4));
            return true;
        }
    }
}
