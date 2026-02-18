using System.Threading.Channels;
using Xunit;
using TACT.KeepAliveServer;

namespace TACTKeepAliveServer.Tests;

public class KeepAlivePipelineTests
{
    [Fact]
    public async Task Channel_WhenMessagePushed_CanBeConsumed()
    {
        // Arrange
        var channel = Channel.CreateUnbounded<KeepAliveMsg>();
        var testMsg = new KeepAliveMsg { USIM = "01011112222", DeviceIP = "192.168.1.100" };

        // Act
        await channel.Writer.WriteAsync(testMsg);
        channel.Writer.Complete();

        // Assert
        var received = await channel.Reader.ReadAsync();
        Assert.Equal("01011112222", received.USIM);
        Assert.Equal("192.168.1.100", received.DeviceIP);
    }

    [Fact]
    public async Task Channel_MultipleMessages_ReceivedInOrder()
    {
        // Arrange
        var channel = Channel.CreateUnbounded<KeepAliveMsg>();
        var usims = new[] { "01011111111", "01022222222", "01033333333" };

        // Act: 여러 메시지 순서대로 Push
        foreach (var usim in usims)
            await channel.Writer.WriteAsync(new KeepAliveMsg { USIM = usim });
        channel.Writer.Complete();

        // Assert: FIFO 순서 보장 확인
        int idx = 0;
        await foreach (var msg in channel.Reader.ReadAllAsync())
        {
            Assert.Equal(usims[idx++], msg.USIM);
        }
        Assert.Equal(3, idx);
    }

    [Fact]
    public async Task Channel_BoundedCapacity_BlocksWhenFull()
    {
        // Arrange: 용량 1짜리 채널
        var channel = Channel.CreateBounded<KeepAliveMsg>(new BoundedChannelOptions(1)
        {
            FullMode = BoundedChannelFullMode.Wait
        });

        // Act: 첫 번째 메시지 Push (즉시 성공)
        await channel.Writer.WriteAsync(new KeepAliveMsg { USIM = "01011111111" });

        // 두 번째 Push는 소비 전까지 대기 → TryWrite로 실패 확인
        bool written = channel.Writer.TryWrite(new KeepAliveMsg { USIM = "01022222222" });

        // Assert
        Assert.False(written); // 채널이 꽉 찼으므로 실패해야 함
    }

    [Fact]
    public async Task Channel_WhenCompleted_ReadAllAsyncEnds()
    {
        // Arrange
        var channel = Channel.CreateUnbounded<KeepAliveMsg>();
        await channel.Writer.WriteAsync(new KeepAliveMsg { USIM = "01099999999" });
        channel.Writer.Complete();

        // Act
        var count = 0;
        await foreach (var _ in channel.Reader.ReadAllAsync())
            count++;

        // Assert
        Assert.Equal(1, count);
    }
}
