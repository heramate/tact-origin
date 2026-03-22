using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using MessagePack;

namespace RACTCommonClass
{
    [Serializable]
    [MessagePackObject]
    public class ShortenCommandGroupInfo
    {
        [Key(0)] public int ID { get; set; } = 0;
        [Key(1)] public string Name { get; set; } = string.Empty;
        [Key(2)] public string Description { get; set; } = string.Empty;
        [Key(3)] public List<ShortenCommandInfo> ShortenCommandList { get; set; } = new List<ShortenCommandInfo>();
    }

    [Serializable]
    [MessagePackObject]
    public class ShortenCommandInfo
    {
        [Key(0)] public int ID { get; set; } = 0;
        [Key(1)] public int GroupID { get; set; } = 0;
        [Key(2)] public string Name { get; set; } = string.Empty;
        [Key(3)] public string Command { get; set; } = string.Empty;
        [Key(4)] public string Description { get; set; } = string.Empty;
    }

    [Serializable]
    [MessagePackObject]
    public class ShortenCommandGroupInfoCollection : IEnumerable
    {
        [Key(0)] public List<ShortenCommandGroupInfo> InnerList { get; set; } = new List<ShortenCommandGroupInfo>();
        public void Add(ShortenCommandGroupInfo info) => InnerList.Add(info);
        public IEnumerator GetEnumerator() => InnerList.GetEnumerator();
    }
}
