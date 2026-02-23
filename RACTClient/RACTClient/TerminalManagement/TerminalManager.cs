using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RACTCommonClass;
using RACTTerminal;

namespace RACTClient.TerminalManagement
{
    public class TerminalManager
    {
        private readonly List<ITactTerminal> m_EmulatorList = new List<ITactTerminal>();
        private const int MaxTerminalCount = 20;

        public List<ITactTerminal> EmulatorList => m_EmulatorList;

        public event Action<ITactTerminal, E_TerminalStatus> TerminalStatusChanged;
        public event Action<E_TerminalStatus, string> TerminalTabChangeEvent;

        public bool CanAddTerminal(DeviceInfo deviceInfo, out int totalCount, IEnumerable<DeviceInfo> currentTabDevices)
        {
            totalCount = 0;
            if (m_EmulatorList.Count >= MaxTerminalCount)
            {
                return false;
            }

            int maxNumber = 0;
            foreach (var tabDeviceInfo in currentTabDevices)
            {
                bool isMatch = false;
                if (deviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET ||
                    deviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
                {
                    isMatch = tabDeviceInfo.IPAddress.Equals(deviceInfo.IPAddress);
                }
                else if (deviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SERIAL_PORT)
                {
                    isMatch = tabDeviceInfo.TerminalConnectInfo.SerialConfig.PortName.Equals(deviceInfo.TerminalConnectInfo.SerialConfig.PortName);
                }

                if (isMatch)
                {
                    // 이 로직은 탭 이름(UI)에 의존하고 있어 분리가 필요함. 
                    // 일단 관성적으로 유지하되, 전체 개수만 관리하도록 하거나 
                    // 탭 이름을 생성하는 로직을 별도로 두어야 함.
                }
            }

            return true;
        }

        public void AddEmulator(ITactTerminal emulator)
        {
            lock (m_EmulatorList)
            {
                if (!m_EmulatorList.Contains(emulator))
                {
                    m_EmulatorList.Add(emulator);
                    emulator.OnTerminalStatusChange += (s, status) => HandleTerminalStatusChange(emulator, status);
                }
            }
        }

        public void RemoveEmulator(ITactTerminal emulator)
        {
            lock (m_EmulatorList)
            {
                m_EmulatorList.Remove(emulator);
            }
        }

        private void HandleTerminalStatusChange(ITactTerminal emulator, E_TerminalStatus status)
        {
            TerminalStatusChanged?.Invoke(emulator, status);
            TerminalTabChangeEvent?.Invoke(status, emulator.Name);

            if (status == E_TerminalStatus.Disconnected)
            {
                RemoveEmulator(emulator);
            }
        }

        public void StopAll(E_TerminalSessionCloseType closeType)
        {
            if (closeType == E_TerminalSessionCloseType.All)
            {
                Parallel.ForEach(m_EmulatorList, emulator =>
                {
                    try { emulator.Disconnect(); } catch { }
                });
            }
            else
            {
                var targets = m_EmulatorList.Where(e => e.ConnectionType == ConnectionTypes.RemoteTelnet).ToList();
                foreach (var emulator in targets)
                {
                    emulator.Disconnect();
                }
            }
        }

        public void ApplyOptionToAll()
        {
            foreach (var emulator in m_EmulatorList)
            {
                emulator.ApplyOption();
            }
        }

        public void ChangeClientModeForAll()
        {
            foreach (var emulator in m_EmulatorList)
            {
                Task.Run(() => emulator.ChangeClientMode());
            }
        }
    }
}
