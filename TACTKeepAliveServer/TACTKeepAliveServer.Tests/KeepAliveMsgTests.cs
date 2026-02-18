using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using TACT.KeepAliveServer;

namespace TACTKeepAliveServer.Tests;

public class KeepAliveMsgTests
{
    // TLV 패킷 직접 생성 헬퍼
    private static byte[] BuildTlvPacket(params (byte type, string value)[] items)
    {
        var bytes = new List<byte>();
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
    public void Decode_ValidTlvPacket_ParsesUsimAndModelName()
    {
        // Arrange
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
    public void Decode_SerialNumberAndImei_ParsedCorrectly()
    {
        // Arrange
        var packet = BuildTlvPacket(
            (2, "SN-20250101"),  // SerialNumber
            (4, "123456789012345") // IMEI
        );

        // Act
        var msg = new KeepAliveMsg();
        msg.Decode(packet);

        // Assert
        Assert.Equal("SN-20250101", msg.SerialNumber);
        Assert.Equal("123456789012345", msg.IMEI);
    }

    [Fact]
    public void Decode_SshFields_ParsedCorrectly()
    {
        // Arrange
        var packet = BuildTlvPacket(
            (7, "ssh.example.com"),  // SSHServerDomain
            (11, "admin"),           // SSHUserID
            (12, "p@ssw0rd")         // SSHPassword
        );

        // Act
        var msg = new KeepAliveMsg();
        msg.Decode(packet);

        // Assert
        Assert.Equal("ssh.example.com", msg.SSHServerDomain);
        Assert.Equal("admin", msg.SSHUserID);
        Assert.Equal("p@ssw0rd", msg.SSHPassword);
    }

    [Fact]
    public void Decode_SshPortBytes_ParsedCorrectly()
    {
        // Arrange: SSH 포트 22 → Big-Endian 2바이트
        var bytes = new List<byte>();
        bytes.AddRange("FACT"u8.ToArray());
        bytes.Add(6); // SSHPort type
        bytes.Add(2); // length
        bytes.Add(0); // 22 = 0x0016 → high byte
        bytes.Add(22); // low byte
        var packet = bytes.ToArray();

        // Act
        var msg = new KeepAliveMsg();
        msg.Decode(packet);

        // Assert
        Assert.Equal(22, msg.SSHPort);
    }

    [Fact]
    public void Decode_EmptyPacket_DoesNotThrow()
    {
        // Arrange & Act
        var msg = new KeepAliveMsg();
        var ex = Record.Exception(() => msg.Decode(Array.Empty<byte>()));

        // Assert
        Assert.Null(ex);
    }

    [Fact]
    public void Decode_PacketWithoutFactPrefix_StillParses()
    {
        // Arrange: FACT 프리픽스 없이 TLV만 있는 경우
        var bytes = new List<byte>();
        var usimBytes = Encoding.ASCII.GetBytes("01099998888");
        bytes.Add(3); // USIM type
        bytes.Add((byte)usimBytes.Length);
        bytes.AddRange(usimBytes);
        var packet = bytes.ToArray();

        // Act
        var msg = new KeepAliveMsg();
        msg.Decode(packet);

        // Assert
        Assert.Equal("01099998888", msg.USIM);
    }

    [Fact]
    public void Decode_Base64EncodedPacket_ParsesCorrectly()
    {
        // Arrange: TLV 패킷을 Base64로 인코딩
        var rawPacket = BuildTlvPacket((3, "01099998888"), (1, "TestModel"));
        var base64Bytes = Encoding.ASCII.GetBytes(Convert.ToBase64String(rawPacket));

        // Act: Base64 디코딩 후 Span 파싱
        var decoded = Convert.FromBase64String(Encoding.ASCII.GetString(base64Bytes));
        var msg = new KeepAliveMsg();
        msg.Decode(decoded);

        // Assert
        Assert.Equal("01099998888", msg.USIM);
        Assert.Equal("TestModel", msg.ModelName);
    }

    [Fact]
    public void IsFullMessage_WithImei_ReturnsTrue()
    {
        var msg = new KeepAliveMsg { IMEI = "123456789012345" };
        Assert.True(msg.IsFullMessage());
    }

    [Fact]
    public void IsFullMessage_WithoutImeiOrSshDomain_ReturnsFalse()
    {
        var msg = new KeepAliveMsg { USIM = "01012345678" };
        Assert.False(msg.IsFullMessage());
    }

    [Fact]
    public void CopyFrom_CopiesAllFields_Correctly()
    {
        // Arrange
        var source = new KeepAliveMsg
        {
            RecvIPAddress = "10.0.0.1",
            RecvPort = 12345,
            DeviceIP = "192.168.1.100",
            USIM = "01012345678",
            IMEI = "123456789012345",
            SSHServerDomain = "ssh.example.com",
            SSHPort = 22
        };
        var target = new KeepAliveMsg();

        // Act
        target.CopyFrom(source, copyNetworkInfo: true, copyDeviceIdent: true, copySshInfo: true);

        // Assert
        Assert.Equal("10.0.0.1", target.RecvIPAddress);
        Assert.Equal("192.168.1.100", target.DeviceIP);
        Assert.Equal("01012345678", target.USIM);
        Assert.Equal("ssh.example.com", target.SSHServerDomain);
    }
}
