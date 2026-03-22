using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using MessagePack;

namespace RACTCommonClass
{
    [Serializable]
    [MessagePackObject]
    public class SerialConfig
    {
        public SerialConfig() { }
        public SerialConfig(SerialConfig a)
        {
            if (a == null) return;
            BaudRate = a.BaudRate;
            DataBits = a.DataBits;
            Handshake = a.Handshake;
            Parity = a.Parity;
            PortName = a.PortName;
            StopBits = a.StopBits;
        }

        [Key(0)] public string PortName { get; set; } = "";
        [Key(1)] public int BaudRate { get; set; } = 9600;
        [Key(2)] public int DataBits { get; set; } = 8;
        [Key(3)] public Parity Parity { get; set; } = Parity.None;
        [Key(4)] public StopBits StopBits { get; set; } = StopBits.One;
        [Key(5)] public Handshake Handshake { get; set; } = Handshake.None;
    }
}
