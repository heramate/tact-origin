using System.Windows.Forms;
using RACTCommonClass;
using RACTTerminal;

namespace RACTClient.TerminalControl
{
    public class TactTerminalFactory : ITactTerminalFactory
    {
        public ITactTerminal CreateTerminal(DeviceInfo deviceInfo, bool isQuickConnection)
        {
            ITactTerminal emulator;

            // 프로토콜에 따른 분기 (향후 확장 가능)
            if (deviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET ||
                deviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet ||
                deviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SERIAL_PORT)
            {
                emulator = new RebexTerminalControl();
            }
            else
            {
                // 기본값 또는 레거시 대응
                emulator = new RebexTerminalControl();
            }

            ((Control)emulator).Dock = DockStyle.Fill;
            emulator.DeviceInfo = deviceInfo;

            return emulator;
        }
    }
}
