using ACPS.CommonConfigCompareClass;
using System;
using System.Collections;
using System.Collections.Concurrent;
using MessagePack;

namespace RACTCommonClass
{
    /// <summary>
    ///  사용자 정보 클래스입니다.
    /// </summary>
    [Serializable]
    [MessagePackObject]
    public class UserInfo : ICloneableEx<UserInfo>
    {
        #region [basic generate part :: Create, ICloneable]
        public UserInfo() { this.ClientID = this.GetHashCode(); }
        public UserInfo(UserInfo a) { this.ClientID = this.GetHashCode(); CopyTo(a, this, false); }
        public object Clone() { return this.MemberwiseClone(); }
        public UserInfo CompactClone() { var t = new UserInfo(); CopyTo(this, t, true); return t; }
        public UserInfo DeepClone() { var t = new UserInfo(); CopyTo(this, t, false); return t; }
        private void CopyTo(UserInfo aSource, UserInfo aDest, bool aIsCompactClone)
        {
            aDest.ClientID = aSource.ClientID;
            aDest.UserID = aSource.UserID;
            aDest.Account = aSource.Account;
            aDest.IPAddress = aSource.IPAddress;
            aDest.MacIPAddress = aSource.MacIPAddress;
            aDest.LastLoginTime = aSource.LastLoginTime;
            aDest.BranchCode = aSource.BranchCode;
            aDest.CenterCode = aSource.CenterCode;
            aDest.DevicePartCode = aSource.DevicePartCode;
            aDest.DevicePartCodes = aSource.DevicePartCodes;
            aDest.UserType = aSource.UserType;
            aDest.IsSupervisor = aSource.IsSupervisor;
            aDest.FACTORGType = aSource.FACTORGType;
            aDest.ORG1Code = aSource.ORG1Code;
            aDest.Centers = (ArrayList)aSource.Centers.Clone();
            aDest.ReceivedDeviceCount = aSource.ReceivedDeviceCount;
            aDest.LifeTime = aSource.LifeTime;
            aDest.GroupInfos = aSource.GroupInfos;
            aDest.LimitedCmdUser = aSource.LimitedCmdUser;
            aDest.MangTypes = (ArrayList)aSource.MangTypes.Clone();
            aDest.Name = aSource.Name;
            aDest.IsViewAllBranch = aSource.IsViewAllBranch;
        }
        #endregion

        [Key(0)] public int ClientID { get; set; } = 0;
        [Key(1)] public int UserID { get; set; } = 0;
        [Key(2)] public string Account { get; set; } = string.Empty;
        [Key(3)] public string Name { get; set; } = string.Empty;
        [Key(4)] public string IPAddress { get; set; } = string.Empty;
        [Key(5)] public string MacIPAddress { get; set; } = string.Empty;
        [Key(6)] public DateTime LastLoginTime { get; set; } = DateTime.Now;
        [Key(7)] public string BranchCode { get; set; } = string.Empty;
        [Key(8)] public string CenterCode { get; set; } = string.Empty;
        [Key(9)] public int DevicePartCode { get; set; } = -1;
        [Key(10)] public string DevicePartCodes { get; set; } = string.Empty;
        [Key(11)] public E_UserType UserType { get; set; } = E_UserType.Admin_All;
        [Key(12)] public bool IsSupervisor { get; set; } = false;
        [Key(13)] public bool IsViewAllBranch { get; set; } = false;
        [Key(14)] public ArrayList Centers { get; set; } = new ArrayList();
        [Key(15)] public bool FACTORGType { get; set; } = false;
        [Key(16)] public string ORG1Code { get; set; } = string.Empty;
        [Key(17)] public int ReceivedDeviceCount { get; set; } = 0;
        [Key(18)] public DateTime LifeTime { get; set; } = DateTime.Now;
        [Key(19)] public GroupInfoCollection GroupInfos { get; set; } = new GroupInfoCollection();
        [Key(20)] public bool LimitedCmdUser { get; set; } = false;
        [Key(21)] public ArrayList MangTypes { get; set; } = new ArrayList();

        [IgnoreMember] public ConcurrentQueue<byte[]> DataQueue { get; set; } = new ConcurrentQueue<byte[]>();

        [IgnoreMember] public string GetCenterCode
        {
            get
            {
                string t = "";
                foreach (var c in Centers) t += $"{c}''" + "," + "''";
                return t.Length > 0 ? t.Substring(0, t.Length - 5) : t;
            }
        }

        [IgnoreMember] public string GetMangType
        {
            get
            {
                string t = "";
                foreach (var m in MangTypes) t += $"{m}''" + "," + "''";
                return t.Length > 0 ? t.Substring(0, t.Length - 5) : t;
            }
        }
    }
}
