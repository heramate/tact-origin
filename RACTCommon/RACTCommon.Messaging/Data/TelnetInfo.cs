using System;
using System.Collections.Generic;
using MessagePack;

namespace RACTCommonClass
{
    [Serializable]
    [MessagePackObject]
    public class TelnetCommandInfo
    {
        [Key(0)] public DeviceInfo DeviceInfo { get; set; }
        [Key(1)] public string Command { get; set; } = string.Empty;
        [Key(2)] public E_TelnetWorkType WorkTyp { get; set; } = E_TelnetWorkType.Connect;
        [Key(3)] public int UserID { get; set; } = 0;
        [Key(4)] public object KeyInfo { get; set; } // Stub: object로 대체하여 종속성 제거
        [Key(5)] public int SessionID { get; set; } = 0;
        [IgnoreMember] public ITelnetEmulator Sender { get; set; }
    }

    [Serializable] public enum E_TelnetWorkType { Connect, Disconnect, Execute }

    [Serializable]
    [MessagePackObject]
    public class TelnetCommandResultInfo : ResultCommunicationData
    {
        public TelnetCommandResultInfo() { }
        public TelnetCommandResultInfo(RequestCommunicationData aRequestInfo) : base(aRequestInfo) { }

        [Key(10)] public E_TelnetReslutType ReslutType { get; set; } = E_TelnetReslutType.Data;
        [Key(11)] public int SessionID { get; set; } = 0;
    }

    [Serializable] public enum E_TelnetReslutType { TryConnect, Connected, DisConnected, Data }

    public interface ITelnetEmulator
    {
        void DisplayResult(TelnetCommandResultInfo tResult);
    }
}
