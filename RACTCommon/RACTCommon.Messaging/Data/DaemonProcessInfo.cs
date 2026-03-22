using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Collections.Concurrent;
using System.Linq;
using ACPS.CommonConfigCompareClass;
using MessagePack;

namespace RACTCommonClass
{
    [Serializable]
    [MessagePackObject]
    public class DaemonProcessInfo : ICloneableEx<DaemonProcessInfo>
    {
        #region [basic generate part :: Create, ICloneable]
        public DaemonProcessInfo() { DaemonID = this.GetHashCode(); }
        public DaemonProcessInfo(DaemonProcessInfo a) { CopyTo(a, this, false); }
        public object Clone() { return this.MemberwiseClone(); }
        public DaemonProcessInfo CompactClone() { var t = new DaemonProcessInfo(); CopyTo(this, t, true); return t; }
        public DaemonProcessInfo DeepClone() { var t = new DaemonProcessInfo(); CopyTo(this, t, false); return t; }
        private void CopyTo(DaemonProcessInfo aSource, DaemonProcessInfo aDest, bool aIsCompactClone)
        {
            aDest.DaemonID = aSource.DaemonID;
            aDest.ChannelName = aSource.ChannelName;
            aDest.ConnectUsercount = aSource.ConnectUsercount;
            aDest.TelnetSessionCount = aSource.TelnetSessionCount;
            aDest.IP = aSource.IP;
            aDest.Port = aSource.Port;
            aDest.LifeTime = aSource.LifeTime;
            aDest.TempSessionCount = aSource.TempSessionCount;
        }
        #endregion

        [Key(0)] public int DaemonID { get; set; } = 0;
        [Key(1)] public string IP { get; set; } = string.Empty;
        [Key(2)] public int Port { get; set; } = 0;
        [Key(3)] public string ChannelName { get; set; } = string.Empty;
        [Key(4)] public int ConnectUsercount { get; set; } = 0;
        [Key(5)] public int TelnetSessionCount { get; set; } = 0;
        [Key(6)] public int TempSessionCount { get; set; } = 0;
        [Key(7)] public DateTime LifeTime { get; set; } = DateTime.Now;

        [IgnoreMember] public ConcurrentQueue<byte[]> DataQueue { get; set; } = new ConcurrentQueue<byte[]>();
    }

    [Serializable]
    [MessagePackObject]
    public class DaemonProcessInfoCollection : GenericListMarshalByRef<DaemonProcessInfo>, ICloneableEx<DaemonProcessInfoCollection>
    {
        public DaemonProcessInfoCollection() { }
        public DaemonProcessInfoCollection(DaemonProcessInfoCollection a) { CopyTo(a, this, false); }
        public object Clone() { return this.MemberwiseClone(); }
        public DaemonProcessInfoCollection CompactClone() { var t = new DaemonProcessInfoCollection(); CopyTo(this, t, true); return t; }
        public DaemonProcessInfoCollection DeepClone() { var t = new DaemonProcessInfoCollection(); CopyTo(this, t, false); return t; }
        private void CopyTo(DaemonProcessInfoCollection aSource, DaemonProcessInfoCollection aDest, bool aIsCompactClone)
        {
            if (aDest != null && aDest.Count > 0) aDest.Clear();
            foreach (DaemonProcessInfo d in aSource) { if (aIsCompactClone) aDest.Add(d.CompactClone()); else aDest.Add(d.DeepClone()); }
        }

        [IgnoreMember] private readonly ConcurrentDictionary<int, DaemonProcessInfo> m_ClientMap = new ConcurrentDictionary<int, DaemonProcessInfo>();

        public override int Add(DaemonProcessInfo item)
        {
            if (item != null) { m_ClientMap[item.DaemonID] = item; return base.Add(item); }
            return -1;
        }

        public override void Remove(DaemonProcessInfo item)
        {
            if (item != null) { m_ClientMap.TryRemove(item.DaemonID, out _); base.Remove(item); }
        }

        public new void Clear() { m_ClientMap.Clear(); base.Clear(); }

        [IgnoreMember] public override DaemonProcessInfo this[int id]
        {
            get => m_ClientMap.TryGetValue(id, out var info) ? info : null;
            set { if (value != null) m_ClientMap[id] = value; }
        }

        public void Remove(int id) { if (m_ClientMap.TryRemove(id, out var info)) base.Remove(info); }
        public bool Contains(int id) => m_ClientMap.ContainsKey(id);
        public List<DaemonProcessInfo> ToList() => m_ClientMap.Values.ToList();
    }
}
