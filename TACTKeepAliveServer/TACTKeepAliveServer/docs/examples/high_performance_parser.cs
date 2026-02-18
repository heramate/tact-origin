```csharp
using System;
using System.Buffers;
using System.Text;

namespace TACT.KeepAliveServer.Util
{
    public static class HighPerformancePacketParser
    {
        private static readonly ArrayPool<byte> _pool = ArrayPool<byte>.Shared;

        /// <summary>
        /// Span<byte>와 ArrayPool을 사용한 고성능 TLV 디코딩 예시
        /// </summary>
        public static void DecodeWithSpan(ReadOnlySpan<byte> packet)
        {
            // 헤더 확인 (FACT)
            if (packet.Length < 4 || !packet.Slice(0, 4).SequenceEqual("FACT"u8))
            {
                return;
            }

            ReadOnlySpan<byte> payload = packet.Slice(4);
            int cursor = 0;

            while (cursor < payload.Length)
            {
                if (cursor + 2 > payload.Length) break;

                byte type = payload[cursor++];
                byte length = payload[cursor++];

                if (cursor + length > payload.Length) break;

                ReadOnlySpan<byte> value = payload.Slice(cursor, length);
                
                // 특정 타입 처리 (예: USIM)
                if (type == (byte)E_KeepAliveType.USIM)
                {
                    // 필요한 경우에만 문자열 변환 (할당 발생)
                    string usim = Encoding.ASCII.GetString(value);
                    // 처리 로직...
                }

                cursor += length;
            }
        }

        /// <summary>
        /// 수신 소켓 데이터 처리 시 ArrayPool 활용 예시
        /// </summary>
        public static void ProcessRawPacket(byte[] buffer, int length)
        {
            // 필요한 크기만큼 풀에서 대여
            byte[] rented = _pool.Rent(length);
            try
            {
                // 데이터 복사 (최소화 필요 시 소켓에서 직접 Span으로 수신 권장)
                buffer.AsSpan(0, length).CopyTo(rented);
                
                // 비즈니스 로직 처리...
                DecodeWithSpan(rented.AsSpan(0, length));
            }
            finally
            {
                // 사용 후 풀에 반납
                _pool.Return(rented);
            }
        }
    }
}
```
