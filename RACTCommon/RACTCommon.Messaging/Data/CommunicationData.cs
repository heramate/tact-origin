using System;
using System.Collections.Generic;
using System.Text;
using MessagePack;

namespace RACTCommonClass
{
    [Serializable]
    [MessagePackObject]
    public class CommunicationData
    {
        public CommunicationData() { }
        public CommunicationData(CommunicationData aSource) { CopyTo(aSource, this, false); }
        private void CopyTo(CommunicationData aSource, CommunicationData aDest, bool aIsCompactClone)
        {
            aDest.JobID = aSource.JobID; aDest.ClientID = aSource.ClientID;
            aDest.RequestTime = aSource.RequestTime; aDest.UserData = aSource.UserData;
            aDest.OwnerKey = aSource.OwnerKey; aDest.CommType = aSource.CommType;
            aDest.IsLimitCmd = aSource.IsLimitCmd;
        }

        [Key(0)] public E_CommunicationType CommType { get; set; } = E_CommunicationType.UnKnown;
        [Key(1)] public long JobID { get; set; } = 0;
        [Key(2)] public int ClientID { get; set; } = 0;
        [Key(3)] public DateTime RequestTime { get; set; } = DateTime.Now;
        [Key(4)] public object UserData { get; set; } = null;
        [Key(5)] public int OwnerKey { get; set; } = 0;
        [Key(6)] public bool IsLimitCmd { get; set; } = false;
    }

    [Serializable]
    [MessagePackObject]
    public class RequestCommunicationData : CommunicationData
    {
        public RequestCommunicationData() { }
        public RequestCommunicationData(CommunicationData a) : base(a) { }
        [Key(7)] public object RequestData { get; set; } = null;
    }

    [Serializable]
    [MessagePackObject]
    public class ResultCommunicationData : CommunicationData
    {
        public ResultCommunicationData() { }
        public ResultCommunicationData(CommunicationData a) : base(a) { }
        [Key(7)] public object ResultData { get; set; } = null;
        [Key(8)] public ErrorInfo Error { get; set; } = new ErrorInfo();
        [Key(9)] public bool IsCompressed { get; set; } = false;
    }

    [Serializable]
    [MessagePackObject]
    public class CommandItemBase 
    {
        [Key(0)] public long JobID { get; set; } = 0;
        [Key(1)] public E_CommandResultType CommandResultType { get; set; } = E_CommandResultType.CommandGroup;
        [Key(2)] public int ClientID { get; set; } = 0;
        [Key(3)] public int OwnerKey { get; set; } = 0;
    }

    [Serializable]
    [MessagePackObject]
    public class CommandResultItem : CommandItemBase
    {
        [Key(4)] public object CommandResult { get; set; } = null;
        [Key(5)] public int UserID { get; set; } = 0;
    }
}
