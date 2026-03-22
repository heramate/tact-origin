using System;
using System.Collections.Generic;
using MessagePack;

namespace RACTCommonClass
{
    [Serializable]
    [MessagePackObject]
    public class ScriptGroupInfo
    {
        [Key(0)] public int ID { get; set; } = 0;
        [Key(1)] public string Name { get; set; } = string.Empty;
        [Key(2)] public string Description { get; set; } = string.Empty;
        [Key(3)] public List<Script> ScriptList { get; set; } = new List<Script>();
    }

    [Serializable]
    [MessagePackObject]
    public class Script
    {
        [Key(0)] public int ID { get; set; } = 0;
        [Key(1)] public string Name { get; set; } = string.Empty;
        [Key(2)] public string Description { get; set; } = string.Empty;
        [Key(3)] public string RawScript { get; set; } = string.Empty;
        [Key(4)] public int GroupID { get; set; } = 0;
    }

    [Serializable]
    [MessagePackObject]
    public class ScriptGroupInfoCollection
    {
        [Key(0)] public Dictionary<int, ScriptGroupInfo> List { get; set; } = new Dictionary<int, ScriptGroupInfo>();
        public void Add(ScriptGroupInfo info) => List[info.ID] = info;
        public bool Contains(int id) => List.ContainsKey(id);
        [IgnoreMember] public ScriptGroupInfo this[int id] => List[id];
    }
}
