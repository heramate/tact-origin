using ACPS.CommonConfigCompareClass;
using System;
using System.Collections;
using MessagePack;

namespace RACTCommonClass
{
    #region DeviceInfo 클래스입니다.
    /// <summary>
    ///  장비 정보 클래스입니다.
    /// </summary>
    [Serializable]
    [MessagePackObject]
    public class DeviceInfo : ICloneableEx<DeviceInfo>
    {
        #region [basic generate part :: Create, ICloneable]
        public DeviceInfo() { TerminalConnectInfo = new TerminalConnectInfo(); }
        public DeviceInfo(DeviceInfo aDeviceInfo) { CopyTo(aDeviceInfo, this, false); }
        public object Clone() { return this.MemberwiseClone(); }
        public DeviceInfo CompactClone() { DeviceInfo t = new DeviceInfo(); CopyTo(this, t, true); return t; }
        public DeviceInfo DeepClone() { DeviceInfo t = new DeviceInfo(); CopyTo(this, t, false); return t; }
        private void CopyTo(DeviceInfo aSource, DeviceInfo aDest, bool aIsCompactClone)
        {
            aDest.DeviceID = aSource.DeviceID;
            aDest.Name = aSource.Name;
            aDest.ModelID = aSource.ModelID;
            aDest.ORG1Code = aSource.ORG1Code;
            aDest.BranchCode = aSource.BranchCode;
            aDest.CenterCode = aSource.CenterCode;
            aDest.IPAddress = aSource.IPAddress;
            aDest.TelnetID1 = aSource.TelnetID1;
            aDest.TelnetPwd1 = aSource.TelnetPwd1;
            aDest.TelnetID2 = aSource.TelnetID2;
            aDest.TelnetPwd2 = aSource.TelnetPwd2;
            aDest.ApplyDate = aSource.ApplyDate;
            aDest.GroupID = aSource.GroupID;
            aDest.ORG1Name = aSource.ORG1Name;
            aDest.ORG2Name = aSource.ORG2Name;
            aDest.CenterName = aSource.CenterName;
            aDest.TpoName = aSource.TpoName;
            aDest.ModelName = aSource.ModelName;
            aDest.DeviceType = aSource.DeviceType;
            aDest.WAIT = aSource.WAIT;
            aDest.USERID = aSource.USERID;
            aDest.PWD = aSource.PWD;
            aDest.USERID2 = aSource.USERID2;
            aDest.PWD2 = aSource.PWD2;
            aDest.MoreString = aSource.MoreString;
            aDest.MoreMark = aSource.MoreMark;
            aDest.UsrName = aSource.UsrName;
            aDest.Account = aSource.Account;
            aDest.UsrID = aSource.UsrID;
            aDest.TerminalName = aSource.TerminalName;
            aDest.TerminalConnectInfo = new TerminalConnectInfo(aSource.TerminalConnectInfo);
            if (!aIsCompactClone)
            {
                aDest.DevicePartCode = aSource.DevicePartCode;
                aDest.InputFlag = aSource.InputFlag;
                aDest.DeviceNumber = aSource.DeviceNumber;
                aDest.Version = aSource.Version;
                aDest.DeviceGroupName = aSource.DeviceGroupName;
                aDest.Description = aSource.Description;
            }
            aDest.SSHTunnelIP = aSource.SSHTunnelIP;
            aDest.SSHTunnelPort = aSource.SSHTunnelPort;
            aDest.IsRegistered = aSource.IsRegistered;
            aDest.MangTypeCd = aSource.MangTypeCd;
        }
        #endregion

        [Key(0)] public bool IsRegistered { get; set; } = true;
        [Key(1)] public TerminalConnectInfo TerminalConnectInfo { get; set; }
        [Key(2)] public int DeviceID { get; set; } = 0;
        [Key(3)] public string Name { get; set; } = string.Empty;
        [Key(4)] public int ModelID { get; set; } = 0;
        [Key(5)] public string ModelName { get; set; } = string.Empty;
        [Key(6)] public string BranchCode { get; set; } = string.Empty;
        [Key(7)] public string CenterCode { get; set; } = string.Empty;
        [Key(8)] public string TpoName { get; set; } = string.Empty;
        [Key(9)] public string IPAddress { get; set; } = string.Empty;
        [Key(10)] public string TerminalName { get; set; } = string.Empty;
        [Key(11)] public int DevicePartCode { get; set; } = 0;
        [Key(12)] public E_FlagType InputFlag { get; set; } = E_FlagType.FORMS;
        [Key(13)] public string TelnetID1 { get; set; } = string.Empty;
        [Key(14)] public string TelnetPwd1 { get; set; } = string.Empty;
        [Key(15)] public string TelnetID2 { get; set; } = string.Empty;
        [Key(16)] public string TelnetPwd2 { get; set; } = string.Empty;
        [Key(17)] public string Version { get; set; } = string.Empty;
        [Key(18)] public string DeviceNumber { get; set; } = string.Empty;
        [Key(19)] public string GroupID { get; set; } = "0";
        [Key(20)] public DateTime ApplyDate { get; set; } = DateTime.Now;
        [Key(21)] public string DeviceGroupName { get; set; } = string.Empty;
        [Key(22)] public string Description { get; set; } = string.Empty;
        [Key(23)] public string ORG1Code { get; set; } = string.Empty;
        [Key(24)] public string ORG1Name { get; set; } = string.Empty;
        [Key(25)] public string ORG2Code { get; set; } = string.Empty;
        [Key(26)] public string ORG2Name { get; set; } = string.Empty;
        [Key(27)] public string CenterName { get; set; } = string.Empty;
        [IgnoreMember] public string Location => DeviceType == E_DeviceType.NeGroup ? $"{ORG1Name.Trim()}>{ORG2Name.Trim()}>{CenterName.Trim()}>{TpoName.Trim()}" : TpoName.Trim();
        [Key(28)] public E_DeviceType DeviceType { get; set; } = E_DeviceType.NeGroup;
        [Key(29)] public CfgSaveInfoCollection CfgSaveInfos { get; set; } = new CfgSaveInfoCollection();
        [Key(30)] public string WAIT { get; set; } = string.Empty;
        [Key(31)] public string USERID { get; set; } = string.Empty;
        [Key(32)] public string PWD { get; set; } = string.Empty;
        [Key(33)] public string USERID2 { get; set; } = string.Empty;
        [Key(34)] public string PWD2 { get; set; } = string.Empty;
        [Key(35)] public string MoreString { get; set; } = string.Empty;
        [Key(36)] public string MoreMark { get; set; } = string.Empty;
        [Key(37)] public string UsrName { get; set; } = string.Empty;
        [Key(38)] public string Account { get; set; } = string.Empty;
        [Key(39)] public int UsrID { get; set; } = 0;
        [Key(40)] public string SSHTunnelIP { get; set; } = string.Empty;
        [Key(41)] public int SSHTunnelPort { get; set; } = 0;
        [Key(42)] public string MangTypeCd { get; set; } = string.Empty;
    }
    #endregion

    #region DeviceInfoCollection 클래스입니다.
    [Serializable]
    [MessagePackObject]
    public class DeviceInfoCollection : GenericListMarshalByRef<DeviceInfo>, ICloneableEx<DeviceInfoCollection>
    {
        public DeviceInfoCollection() { }
        public DeviceInfoCollection(DeviceInfoCollection a) { CopyTo(a, this, false); }
        public object Clone() { return this.MemberwiseClone(); }
        public DeviceInfoCollection CompactClone() { var t = new DeviceInfoCollection(); CopyTo(this, t, true); return t; }
        public DeviceInfoCollection DeepClone() { var t = new DeviceInfoCollection(); CopyTo(this, t, false); return t; }
        private void CopyTo(DeviceInfoCollection aSource, DeviceInfoCollection aDest, bool aIsCompactClone)
        {
            if (aDest != null && aDest.Count > 0) aDest.Clear();
            foreach (DeviceInfo d in aSource) { if (aIsCompactClone) aDest.Add(d.CompactClone()); else aDest.Add(d.DeepClone()); }
        }

        [IgnoreMember] public override DeviceInfo this[int aID]
        {
            get { lock (base.InnerList.SyncRoot) { foreach (DeviceInfo d in base.InnerList) if (d.DeviceID == aID) return d; } return null; }
            set { lock (base.InnerList.SyncRoot) { for (int i = 0; i < base.InnerList.Count; i++) if (((DeviceInfo)base.InnerList[i]).DeviceID == aID) { base.InnerList[i] = value; break; } } }
        }

        [IgnoreMember] public DeviceInfo this[string ip]
        {
            get { lock (base.InnerList.SyncRoot) { foreach (DeviceInfo d in base.InnerList) if (d.IPAddress == ip) return d; } return null; }
            set { lock (base.InnerList.SyncRoot) { for (int i = 0; i < base.InnerList.Count; i++) if (((DeviceInfo)base.InnerList[i]).IPAddress == ip) { base.InnerList[i] = value; break; } } }
        }

        public void Remove(int id) { lock (base.InnerList.SyncRoot) { foreach (DeviceInfo d in base.InnerList) if (d.DeviceID == id) { base.Remove(d); break; } } }
        public void Remove(DeviceInfo deviceInfo) { lock (base.InnerList.SyncRoot) { foreach (DeviceInfo d in base.InnerList) if (d == deviceInfo) { base.Remove(d); break; } } }
        public bool Contains(int id) => this[id] != null;
        public bool Contains(string ip) => this[ip] != null;

        public DeviceIDList GetIDs() { var list = new DeviceIDList(); foreach (DeviceInfo d in base.InnerList) list.Add(d.DeviceID); return list; }
    }
    #endregion

    #region DeviceIDList 클래스입니다.
    [Serializable]
    [MessagePackObject]
    public class DeviceIDList : MarshalByRefObject, ICloneable, ICollection
    {
        [Key(0)] public ArrayList IDs { get; set; } = new ArrayList();
        public DeviceIDList() { }
        public DeviceIDList(DeviceIDList v) { if (v != null) foreach (var id in v.IDs) IDs.Add(id); }
        public int Add(int id) => IDs.Add(id);
        public void AddRange(ICollection v) => IDs.AddRange(v);
        public void Remove(int id) => IDs.Remove(id);
        public bool Contains(int id) => IDs.Contains(id);
        [IgnoreMember] public int Count => IDs.Count;
        [IgnoreMember] public object SyncRoot => IDs.SyncRoot;
        [IgnoreMember] public bool IsSynchronized => IDs.IsSynchronized;
        public void CopyTo(Array array, int index) => IDs.CopyTo(array, index);
        public IEnumerator GetEnumerator() => IDs.GetEnumerator();
        object ICloneable.Clone() => new DeviceIDList(this);
        [IgnoreMember] public int this[int index] { get => (int)IDs[index]; set => IDs[index] = value; }
    }
    #endregion

    #region DeviceCollection 클래스입니다.
    [Serializable]
    [MessagePackObject]
    public class DeviceCollection : GenericListMarshalByRef<DeviceInfo>, ICloneableEx<DeviceCollection>
    {
        public DeviceCollection() { }
        public DeviceCollection(DeviceCollection a) { CopyTo(a, this, false); }
        public object Clone() { return this.MemberwiseClone(); }
        public DeviceCollection CompactClone() { var t = new DeviceCollection(); CopyTo(this, t, true); return t; }
        public DeviceCollection DeepClone() { var t = new DeviceCollection(); CopyTo(this, t, false); return t; }
        private void CopyTo(DeviceCollection aSource, DeviceCollection aDest, bool aIsCompactClone)
        {
            if (aDest != null && aDest.Count > 0) aDest.Clear();
            foreach (DeviceInfo d in aSource) { if (aIsCompactClone) aDest.Add(d.CompactClone()); else aDest.Add(d.DeepClone()); }
        }
    }
    #endregion
}
