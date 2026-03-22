using System;
using System.Collections.Generic;
using MessagePack;

namespace RACTCommonClass
{
    [Serializable]
    [MessagePackObject]
    public class AutoCompleteCmdInfo
    {
        [Key(0)] public int ModelID { get; set; } = 0;
        [Key(1)] public List<string> Command { get; set; } = new List<string>();
    }

    [Serializable]
    [MessagePackObject]
    public class AutoCompleteCmdInfoCollection
    {
        [Key(0)] public Dictionary<int, AutoCompleteCmdInfo> List { get; set; } = new Dictionary<int, AutoCompleteCmdInfo>();
        public void Add(AutoCompleteCmdInfo info) => List[info.ModelID] = info;
        public bool Contains(int id) => List.ContainsKey(id);
        [IgnoreMember] public AutoCompleteCmdInfo this[int id] => List[id];
        public AutoCompleteCmdInfoCollection DeepClone() => new AutoCompleteCmdInfoCollection { List = new Dictionary<int, AutoCompleteCmdInfo>(List) };
    }

    [Serializable]
    [MessagePackObject]
    public class DefaultCmdInfo
    {
        [Key(0)] public int ModelID { get; set; } = 0;
        [Key(1)] public int EmbargoID { get; set; } = 0;
        [Key(2)] public List<string> Command { get; set; } = new List<string>();
        [Key(3)] public List<string> Description { get; set; } = new List<string>();
    }

    [Serializable]
    [MessagePackObject]
    public class DefaultCmdInfoCollection
    {
        [Key(0)] public Dictionary<int, DefaultCmdInfo> List { get; set; } = new Dictionary<int, DefaultCmdInfo>();
        public void Add(DefaultCmdInfo info) => List[info.ModelID] = info;
        public bool Contains(int id) => List.ContainsKey(id);
        [IgnoreMember] public object SyncRoot => new object();
        [IgnoreMember] public DefaultCmdInfo this[int id] => List[id];
        public DefaultCmdInfoCollection DeepClone() => new DefaultCmdInfoCollection { List = new Dictionary<int, DefaultCmdInfo>(List) };
    }

    [Serializable]
    [MessagePackObject]
    public class LimitCmdInfo
    {
        [Key(0)] public int ModelID { get; set; } = 0;
        [Key(1)] public int EmbargoID { get; set; } = 0;
        [Key(2)] public List<EmbagoInfo> EmbagoCmd { get; set; } = new List<EmbagoInfo>();
    }

    [Serializable]
    [MessagePackObject]
    public class EmbagoInfo
    {
        [Key(0)] public string Embargo { get; set; } = string.Empty;
        [Key(1)] public bool EmbargoEnble { get; set; } = false;
    }

    [Serializable]
    [MessagePackObject]
    public class LimitCmdInfoCollection
    {
        [Key(0)] public Dictionary<int, LimitCmdInfo> List { get; set; } = new Dictionary<int, LimitCmdInfo>();
        public void Add(LimitCmdInfo info) => List[info.ModelID] = info;
        public bool Contains(int id) => List.ContainsKey(id);
        [IgnoreMember] public object SyncRoot => new object();
        [IgnoreMember] public LimitCmdInfo this[int id] => List[id];
        public LimitCmdInfoCollection DeepClone() => new LimitCmdInfoCollection { List = new Dictionary<int, LimitCmdInfo>(List) };
    }
}
