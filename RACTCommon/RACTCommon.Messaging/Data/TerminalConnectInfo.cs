using System;
using System.Collections.Generic;
using System.Text;
using MessagePack;

namespace RACTCommonClass
{
    [Serializable]
    [MessagePackObject]
    public class TerminalConnectInfo
    {
        public TerminalConnectInfo() { m_SerianConfig = new SerialConfig(); }
        public TerminalConnectInfo(TerminalConnectInfo a)
        {
            if (a == null) return;
            ConnectionProtocol = a.ConnectionProtocol;
            IPAddress = a.IPAddress;
            SerialConfig = new SerialConfig(a.SerialConfig);
            TelnetPort = a.TelnetPort;
            ID = a.ID;
            Password = a.Password;
        }

        [Key(0)] public E_ConnectionProtocol ConnectionProtocol { get; set; }
        [Key(1)] public SerialConfig SerialConfig { get; set; }
        [Key(2)] public string IPAddress { get; set; } = "";
        [Key(3)] public int TelnetPort { get; set; } = 23;
        [Key(4)] public string ID { get; set; } = string.Empty;
        [Key(5)] public string Password { get; set; } = string.Empty;

        private SerialConfig m_SerianConfig; // 하위 호환 필드
    }
}
