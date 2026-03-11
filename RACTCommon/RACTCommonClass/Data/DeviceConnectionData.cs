using System;

namespace RACTCommonClass
{
    [Serializable]
    public class DeviceConnectionLogOpenRequestInfo
    {
        public int ClientID { get; set; }
        public int UserID { get; set; }
        public int DeviceID { get; set; }
        public string DeviceIP { get; set; }
        public int ConnectionKind { get; set; }
        public string Description { get; set; }
    }

    [Serializable]
    public class DeviceConnectionLogOpenResultInfo
    {
        public bool Success { get; set; }
        public int ConnectionLogID { get; set; }
        public string ErrorMessage { get; set; }
    }

    [Serializable]
    public class DeviceConnectionLogCloseRequestInfo
    {
        public int ClientID { get; set; }
        public int UserID { get; set; }
        public int ConnectionLogID { get; set; }
        public int SessionID { get; set; }
        public string DisconnectReason { get; set; }
    }

    [Serializable]
    public class DeviceConnectionLogCloseResultInfo
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }

}
