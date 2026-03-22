using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using ACPS.CommonConfigCompareClass;
using MessagePack;

namespace RACTCommonClass
{
    [Serializable]
    [MessagePackObject]
    public class GroupInfo
    {
        public GroupInfo() { }
        public GroupInfo(GroupInfo a)
        {
            if (a == null) return;
            ID = a.ID; Name = a.Name; UserID = a.UserID; DeviceList = a.DeviceList;
            Description = a.Description; TOP_ID = a.TOP_ID; UP_ID = a.UP_ID;
            LEVEL = a.LEVEL; SEQ_ID = a.SEQ_ID; DEVICE_COUNT = a.DEVICE_COUNT;
        }

        [Key(0)] public string ID { get; set; }
        [Key(1)] public string Name { get; set; }
        [Key(2)] public string Description { get; set; }
        [Key(3)] public int UserID { get; set; }
        [Key(4)] public string TOP_ID { get; set; } = string.Empty;
        [Key(5)] public string UP_ID { get; set; } = string.Empty;
        [Key(6)] public int LEVEL { get; set; } = 0;
        [Key(7)] public int SEQ_ID { get; set; } = 0;
        [Key(8)] public int DEVICE_COUNT { get; set; } = 0;
        [Key(9)] public DeviceInfoCollection DeviceList { get; set; } = new DeviceInfoCollection();
    }

    [Serializable]
    [MessagePackObject]
    public class GroupInfoCollection
    {
        [Key(0)] public Dictionary<string, GroupInfo> List { get; set; } = new Dictionary<string, GroupInfo>();

        public GroupInfoCollection() { }
        public bool ContainsKey(string id) => List.ContainsKey(id);
        [IgnoreMember] public GroupInfo this[string key] { get => List[key]; set => List[key] = value; }
        public void Add(string key, GroupInfo info) => List.Add(key, info);
        [IgnoreMember] public int Count => List.Count;
        public int GetCountByGroup(string key) => List[key].DeviceList.Count;
        [IgnoreMember] public Dictionary<string, GroupInfo> InnerList => List;
        public void Remove(string key) => List.Remove(key);
    }
}
