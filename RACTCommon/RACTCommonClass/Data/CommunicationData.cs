using System;
using System.Collections.Generic;
using System.Text;
using MessagePack;

namespace RACTCommonClass
{
    #region CommunicationData 클래스입니다.
    /// <summary>
    ///  통신 데이터 클래스입니다.
    /// </summary>
    [Serializable]
    [MessagePackObject]
    public class CommunicationData
    {
        public CommunicationData() { }
        public CommunicationData(CommunicationData aCommunicationData) { CopyTo(aCommunicationData, this, false); }
        public object Clone() { return this.MemberwiseClone(); }
        public CommunicationData CompactClone() { CommunicationData t = new CommunicationData(); CopyTo(this, t, true); return t; }
        public CommunicationData DeepClone() { CommunicationData t = new CommunicationData(); CopyTo(this, t, false); return t; }
        private void CopyTo(CommunicationData aSource, CommunicationData aDest, bool aIsCompactClone)
        {
            aDest.JobID = aSource.JobID;
            aDest.ClientID = aSource.ClientID;
            aDest.RequestTime = aSource.RequestTime;
            aDest.UserData = aSource.UserData;
            aDest.OwnerKey = aSource.OwnerKey;
            aDest.CommType = aSource.CommType;
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
    #endregion

    #region RequestCommunicationData 클래스입니다.
    [Serializable]
    [MessagePackObject]
    public class RequestCommunicationData : CommunicationData
    {
        public RequestCommunicationData() { }
        public RequestCommunicationData(RequestCommunicationData a) { CopyTo(a, this, false); }
        public RequestCommunicationData(CommunicationData a) : base(a) { }
        public new object Clone() { return this.MemberwiseClone(); }
        public new RequestCommunicationData CompactClone() { RequestCommunicationData t = new RequestCommunicationData((CommunicationData)this); CopyTo(this, t, true); return t; }
        public new RequestCommunicationData DeepClone() { RequestCommunicationData t = new RequestCommunicationData((CommunicationData)this); CopyTo(this, t, false); return t; }
        private void CopyTo(RequestCommunicationData aSource, RequestCommunicationData aDest, bool aIsCompactClone) { aDest.RequestData = aSource.RequestData; }

        [Key(7)] public object RequestData { get; set; } = null;
    }
    #endregion

    #region ResultCommunicationData 클래스입니다.
    [Serializable]
    [MessagePackObject]
    public class ResultCommunicationData : CommunicationData
    {
        public ResultCommunicationData() { }
        public ResultCommunicationData(ResultCommunicationData a) { CopyTo(a, this, false); }
        public ResultCommunicationData(CommunicationData a) : base(a) { }
        public new object Clone() { return this.MemberwiseClone(); }
        public new ResultCommunicationData CompactClone() { ResultCommunicationData t = new ResultCommunicationData((CommunicationData)this); CopyTo(this, t, true); return t; }
        public new ResultCommunicationData DeepClone() { ResultCommunicationData t = new ResultCommunicationData((CommunicationData)this); CopyTo(this, t, false); return t; }
        private void CopyTo(ResultCommunicationData aSource, ResultCommunicationData aDest, bool aIsCompactClone)
        {
            if (aIsCompactClone) { if (aSource.Error != null) aDest.Error = aSource.Error.CompactClone(); }
            else { if (aSource.Error != null) aDest.Error = aSource.Error.DeepClone(); }
            aDest.ResultData = aSource.ResultData;
            aDest.IsCompressed = aSource.IsCompressed;
            aDest.UserData = aSource.UserData;
        }

        [Key(7)] public object ResultData { get; set; } = null;
        [Key(8)] public ErrorInfo Error { get; set; } = new ErrorInfo();
        [Key(9)] public bool IsCompressed { get; set; } = false;
    }
    #endregion

    #region CommandItemBase 클래스입니다.
    [Serializable]
    [MessagePackObject]
    public class CommandItemBase 
    {
        public CommandItemBase() { }
        public CommandItemBase(CommandItemBase a) { CopyTo(a, this, false); }
        public object Clone() { return this.MemberwiseClone(); }
        public CommandItemBase CompactClone() { CommandItemBase t = new CommandItemBase(); CopyTo(this, t, true); return t; }
        public CommandItemBase DeepClone() { CommandItemBase t = new CommandItemBase(); CopyTo(this, t, false); return t; }
        private void CopyTo(CommandItemBase aSource, CommandItemBase aDest, bool aIsCompactClone)
        {
            aDest.JobID = aSource.JobID;
            aDest.CommandResultType = aSource.CommandResultType;
            aDest.ClientID = aSource.ClientID;
            aDest.OwnerKey = aSource.OwnerKey;
        }

        [Key(0)] public long JobID { get; set; } = 0;
        [Key(1)] public E_CommandResultType CommandResultType { get; set; } = E_CommandResultType.CommandGroup;
        [Key(2)] public int ClientID { get; set; } = 0;
        [Key(3)] public int OwnerKey { get; set; } = 0;
    }
    #endregion

    #region CommandResultItem 클래스입니다.
    [Serializable]
    [MessagePackObject]
    public class CommandResultItem : CommandItemBase
    {
        public CommandResultItem() { }
        public CommandResultItem(CommandResultItem a) { CopyTo(a, this, false); }
        public CommandResultItem(CommandItemBase a) : base(a) { }
        public CommandResultItem(long j, E_CommandResultType t, object r) { base.JobID = j; base.CommandResultType = t; CommandResult = r; }
        public new object Clone() { return this.MemberwiseClone(); }
        public new CommandResultItem CompactClone() { CommandResultItem t = new CommandResultItem((CommandItemBase)this); CopyTo(this, t, true); return t; }
        public new CommandResultItem DeepClone() { CommandResultItem t = new CommandResultItem((CommandItemBase)this); CopyTo(this, t, false); return t; }
        private void CopyTo(CommandResultItem aSource, CommandResultItem aDest, bool aIsCompactClone) { aDest.CommandResult = aSource.CommandResult; aDest.UserID = aSource.UserID; }

        [Key(4)] public object CommandResult { get; set; } = null;
        [Key(5)] public int UserID { get; set; } = 0;
    }
    #endregion
}
