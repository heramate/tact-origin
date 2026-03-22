using System;
using System.Collections.Generic;
using System.Text;
using MessagePack;

namespace RACTCommonClass
{
    [Serializable]
    [MessagePackObject]
    public class DeviceConnectionLogOpenRequestInfo
    {
        [Key(0)] public int ClientID { get; set; }
        [Key(1)] public int UserID { get; set; }
        [Key(2)] public int DeviceID { get; set; }
        [Key(3)] public string DeviceIP { get; set; }
        [Key(4)] public string Description { get; set; } = "";
        [Key(5)] public int ConnectionKind { get; set; } = 0;
    }

    [Serializable]
    [MessagePackObject]
    public class DeviceConnectionLogOpenResultInfo
    {
        [Key(0)] public bool Success { get; set; }
        [Key(1)] public int ConnectionLogID { get; set; }
        [Key(2)] public string ErrorMessage { get; set; } = "";
    }

    [Serializable]
    [MessagePackObject]
    public class DeviceConnectionLogCloseRequestInfo
    {
        [Key(0)] public int ConnectionLogID { get; set; }
    }

    [Serializable]
    [MessagePackObject]
    public class DeviceConnectionLogCloseResultInfo
    {
        [Key(0)] public bool Success { get; set; }
        [Key(1)] public string ErrorMessage { get; set; } = "";
    }
}
