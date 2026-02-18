# KeepAlive 서버 테스트 전략

서버를 실제 장비 없이 검증하기 위한 3단계 테스트 전략입니다.

---

## 1단계: 단위 테스트 (xUnit) — 패킷 파싱 검증

**목적**: 실제 네트워크 없이 `KeepAliveMsg.Decode` 로직이 TLV 데이터를 올바르게 파싱하는지 검증합니다.

### 프로젝트 생성
```bash
dotnet new xunit -n TACTKeepAliveServer.Tests
cd TACTKeepAliveServer.Tests
dotnet add reference ..\TACTKeepAliveServer\TACTKeepAliveServer.csproj
```

### 테스트 코드 예제

```csharp
// KeepAliveMsgTests.cs
using System;
using System.Text;
using Xunit;
using TACT.KeepAliveServer;

public class KeepAliveMsgTests
{
    // TLV 패킷 직접 생성 헬퍼
    private static byte[] BuildTlvPacket(params (byte type, string value)[] items)
    {
        var bytes = new System.Collections.Generic.List<byte>();
        bytes.AddRange("FACT"u8.ToArray()); // 프리픽스
        foreach (var (type, value) in items)
        {
            var valBytes = Encoding.ASCII.GetBytes(value);
            bytes.Add(type);
            bytes.Add((byte)valBytes.Length);
            bytes.AddRange(valBytes);
        }
        return bytes.ToArray();
    }

    [Fact]
    public void Decode_ValidTlvPacket_ParsesUsimAndDeviceIP()
    {
        // Arrange: USIM(3)과 ModelName(1)이 포함된 TLV 패킷 생성
        var packet = BuildTlvPacket(
            (3, "01012345678"),   // USIM
            (1, "TACT-Model-X")  // ModelName
        );

        // Act
        var msg = new KeepAliveMsg();
        msg.Decode(packet);

        // Assert
        Assert.Equal("01012345678", msg.USIM);
        Assert.Equal("TACT-Model-X", msg.ModelName);
    }

    [Fact]
    public void Decode_Base64EncodedPacket_ParsesCorrectly()
    {
        // Arrange: TLV 패킷을 Base64로 인코딩
        var rawPacket = BuildTlvPacket((3, "01099998888"));
        var base64Bytes = Encoding.ASCII.GetBytes(Convert.ToBase64String(rawPacket));

        // Act: Base64 디코딩 후 Span 파싱
        var decoded = Convert.FromBase64String(Encoding.ASCII.GetString(base64Bytes));
        var msg = new KeepAliveMsg();
        msg.Decode(decoded);

        // Assert
        Assert.Equal("01099998888", msg.USIM);
    }

    [Fact]
    public void Decode_EmptyPacket_DoesNotThrow()
    {
        var msg = new KeepAliveMsg();
        var ex = Record.Exception(() => msg.Decode(Array.Empty<byte>()));
        Assert.Null(ex);
    }
}
```

---

## 2단계: 통합 테스트 — Channel 파이프라인 검증

**목적**: `KeepAliveReceiverService`가 패킷을 수신하여 `Channel`에 올바르게 Push하는지 검증합니다.

```csharp
// KeepAlivePipelineTests.cs
using System.Threading.Channels;
using Xunit;
using TACT.KeepAliveServer;

public class KeepAlivePipelineTests
{
    [Fact]
    public async Task Channel_WhenMessagePushed_CanBeConsumed()
    {
        // Arrange
        var channel = Channel.CreateUnbounded<KeepAliveMsg>();
        var testMsg = new KeepAliveMsg { USIM = "01011112222", DeviceIP = "192.168.1.100" };

        // Act: 생산자(Producer) 역할
        await channel.Writer.WriteAsync(testMsg);
        channel.Writer.Complete();

        // Assert: 소비자(Consumer) 역할
        var received = await channel.Reader.ReadAsync();
        Assert.Equal("01011112222", received.USIM);
        Assert.Equal("192.168.1.100", received.DeviceIP);
    }
}
```

---

## 3단계: 수동 테스트 — UDP 시뮬레이터

**목적**: 실제 서버를 실행한 상태에서 장비 역할을 하는 UDP 패킷 발송기로 End-to-End 흐름을 검증합니다.

### 실행 방법
1. 서버 실행: `dotnet run --project TACTKeepAliveServer`
2. 아래 시뮬레이터 실행: `dotnet script UdpSimulator.cs` (또는 별도 콘솔 앱으로 실행)

```csharp
// UdpSimulator.cs — 장비 역할 시뮬레이터
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

const string SERVER_IP = "127.0.0.1";
const int SERVER_PORT = 40001;

// 1. TLV 패킷 생성
var tlv = new System.Collections.Generic.List<byte>();
tlv.AddRange("FACT"u8.ToArray());
void AddTlv(byte type, string value)
{
    var b = Encoding.ASCII.GetBytes(value);
    tlv.Add(type); tlv.Add((byte)b.Length); tlv.AddRange(b);
}
AddTlv(3, "01012345678");    // USIM
AddTlv(1, "TACT-TestDevice"); // ModelName
AddTlv(2, "SN-00001");        // SerialNumber

// 2. Base64 인코딩
var base64Payload = Encoding.ASCII.GetBytes(Convert.ToBase64String(tlv.ToArray()));

// 3. UDP 전송 (5초마다 반복)
using var udp = new UdpClient();
var endpoint = new IPEndPoint(IPAddress.Parse(SERVER_IP), SERVER_PORT);

Console.WriteLine($"[Simulator] Sending Keep-Alive to {SERVER_IP}:{SERVER_PORT} every 5s...");
while (true)
{
    udp.Send(base64Payload, base64Payload.Length, endpoint);
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Packet sent ({base64Payload.Length} bytes)");
    await Task.Delay(5000);
}
```

---

## 테스트 체크리스트

| 항목 | 방법 | 기대 결과 |
| :--- | :--- | :--- |
| TLV 파싱 정확도 | xUnit 단위 테스트 | USIM, ModelName 등 필드 정상 파싱 |
| Base64 디코딩 | xUnit 단위 테스트 | 인코딩된 패킷 정상 처리 |
| 채널 파이프라인 | xUnit 통합 테스트 | 메시지 Push/Pop 정상 동작 |
| 실제 UDP 수신 | UDP 시뮬레이터 | 서버 로그에 수신 메시지 출력 확인 |
| DB 업데이트 | UDP 시뮬레이터 + DB 조회 | `LTE_NE` 테이블 `KeepAliveLastRecvDate` 갱신 확인 |
| 서버 메트릭 | 브라우저 `http://localhost:5000/metrics` | Prometheus 메트릭 정상 노출 |
