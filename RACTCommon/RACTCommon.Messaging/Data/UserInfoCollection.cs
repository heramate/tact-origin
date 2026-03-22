using System;
using System.Collections.Generic;
using ACPS.CommonConfigCompareClass;
using System.Collections.Concurrent;
using System.Linq;
using MessagePack;

namespace RACTCommonClass
{
    [Serializable]
    [MessagePackObject]
    public class UserInfoCollection : GenericListMarshalByRef<UserInfo>, ICloneableEx<UserInfoCollection>
    {
        #region [basic generate part :: Create, ICloneable]
        public UserInfoCollection() { }
        public UserInfoCollection(UserInfoCollection a) { CopyTo(a, this, false); }
        public object Clone() { return this.MemberwiseClone(); }
        public UserInfoCollection CompactClone() { var t = new UserInfoCollection(); CopyTo(this, t, true); return t; }
        public UserInfoCollection DeepClone() { var t = new UserInfoCollection(); CopyTo(this, t, false); return t; }
        private void CopyTo(UserInfoCollection aSource, UserInfoCollection aDest, bool aIsCompactClone)
        {
            if (aDest != null && aDest.Count > 0) aDest.Clear();
            foreach (UserInfo d in aSource) { if (aIsCompactClone) aDest.Add(d.CompactClone()); else aDest.Add(d.DeepClone()); }
        }
        #endregion

        [IgnoreMember] private readonly ConcurrentDictionary<int, UserInfo> m_ClientMap = new ConcurrentDictionary<int, UserInfo>();

        public override int Add(UserInfo item)
        {
            if (item != null)
            {
                m_ClientMap[item.ClientID] = item;
                return base.Add(item);
            }
            return -1;
        }

        public override void Remove(UserInfo item)
        {
            if (item != null)
            {
                UserInfo removed;
                m_ClientMap.TryRemove(item.ClientID, out removed);
                base.Remove(item);
            }
        }

        public new void Clear() { m_ClientMap.Clear(); base.Clear(); }

        [IgnoreMember] public override UserInfo this[int id]
        {
            get => m_ClientMap.TryGetValue(id, out var user) ? user : null;
            set { if (value != null) m_ClientMap[id] = value; }
        }

        public void Remove(int id) { if (m_ClientMap.TryRemove(id, out var user)) base.Remove(user); }
        public bool Contains(int id) => m_ClientMap.ContainsKey(id);
        public List<UserInfo> ToList() => m_ClientMap.Values.ToList();
    }
}
