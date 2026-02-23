using RACTCommonClass;

namespace RACTClient.TerminalControl
{
    public interface ITactTerminalFactory
    {
        ITactTerminal CreateTerminal(DeviceInfo deviceInfo, bool isQuickConnection);
    }
}
