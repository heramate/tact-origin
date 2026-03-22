using System;
using System.Collections.Generic;
using System.Text;
using ACPS.CommonConfigCompareClass;
using MessagePack;

namespace RACTCommonClass
{
    #region FACTGroupInfo 클래스입니다.
    [Serializable]
    [MessagePackObject]
    public class FACTGroupInfo : ICloneableEx<FACTGroupInfo>
    {
        #region [basic generate part :: Create, ICloneable]
        public FACTGroupInfo() { }
        public FACTGroupInfo(FACTGroupInfo a) { CopyTo(a, this, false); }
        public object Clone() { return this.MemberwiseClone(); }
        public FACTGroupInfo CompactClone() { FACTGroupInfo t = new FACTGroupInfo(); CopyTo(this, t, true); return t; }
        public FACTGroupInfo DeepClone() { FACTGroupInfo t = new FACTGroupInfo(); CopyTo(this, t, false); return t; }
        private void CopyTo(FACTGroupInfo aSource, FACTGroupInfo aDest, bool aIsCompactClone)
        {
            aDest.BranchCode = aSource.BranchCode;
            aDest.BranchName = aSource.BranchName;
            aDest.CenterCode = aSource.CenterCode;
            aDest.CenterName = aSource.CenterName;
            aDest.ORG1Code = aSource.ORG1Code;
            aDest.ORG1Name = aSource.ORG1Name;
            aDest.DeviceCount = aSource.DeviceCount;
            if (aIsCompactClone)
            {
                if (aSource.ParentGroup != null) aDest.ParentGroup = aSource.ParentGroup.CompactClone();
                if (aSource.SubGroups != null) aDest.SubGroups = aSource.SubGroups.CompactClone();
            }
            else
            {
                if (aSource.ParentGroup != null) aDest.ParentGroup = aSource.ParentGroup.DeepClone();
                if (aSource.SubGroups != null) aDest.SubGroups = aSource.SubGroups.DeepClone();
            }
        }
        #endregion

        [Key(0)] public string BranchCode { get; set; } = string.Empty;
        [Key(1)] public string BranchName { get; set; } = string.Empty;
        [Key(2)] public string CenterCode { get; set; } = string.Empty;
        [Key(3)] public string CenterName { get; set; } = string.Empty;
        [Key(4)] public FACTGroupInfo ParentGroup { get; set; } = null;
        [Key(5)] public int DeviceCount { get; set; } = 0;
        [Key(6)] public FACTGroupInfoCollection SubGroups { get; set; } = null;
        [Key(7)] public string ORG1Code { get; set; } = "";
        [Key(8)] public string ORG1Name { get; set; } = "";

        public int GetToatalDeviceCount() => GetDeviceCount(this);
        private int GetDeviceCount(FACTGroupInfo group)
        {
            if (group.SubGroups == null) return group.DeviceCount;
            int total = 0;
            foreach (FACTGroupInfo sub in group.SubGroups) total += (sub.SubGroups != null) ? GetDeviceCount(sub) : sub.DeviceCount;
            return total;
        }
    }
    #endregion

    #region FACTGroupInfoCollection 클래스입니다.
    [Serializable]
    [MessagePackObject]
    public class FACTGroupInfoCollection : GenericListMarshalByRef<FACTGroupInfo>, ICloneableEx<FACTGroupInfoCollection>
    {
        public FACTGroupInfoCollection() { }
        public FACTGroupInfoCollection(FACTGroupInfoCollection a) { CopyTo(a, this, false); }
        public object Clone() { return this.MemberwiseClone(); }
        public FACTGroupInfoCollection CompactClone() { var t = new FACTGroupInfoCollection(); CopyTo(this, t, true); return t; }
        public FACTGroupInfoCollection DeepClone() { var t = new FACTGroupInfoCollection(); CopyTo(this, t, false); return t; }
        private void CopyTo(FACTGroupInfoCollection aSource, FACTGroupInfoCollection aDest, bool aIsCompactClone)
        {
            if (aDest != null && aDest.Count > 0) aDest.Clear();
            foreach (FACTGroupInfo g in aSource) { if (aIsCompactClone) aDest.Add(g.CompactClone()); else aDest.Add(g.DeepClone()); }
        }
        public new int Add(FACTGroupInfo group) => base.InnerList.Add(group);
    }
    #endregion
}
