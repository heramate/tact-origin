using System;
using System.Collections.Generic;
using System.Collections;
using MessagePack;

namespace RACTCommonClass
{
    [Serializable]
    [MessagePackObject]
    public class CfgSaveInfo
    {
        [Key(0)] public int CmdSeq { get; set; } = 0;
        [Key(1)] public string Cmd { get; set; } = string.Empty;
        [Key(2)] public CfgRestoreCommandCollection CfgRestoreCommands { get; set; } = new CfgRestoreCommandCollection();
    }

    [Serializable]
    [MessagePackObject]
    public class CfgSaveInfoCollection : IEnumerable
    {
        [Key(0)] public List<CfgSaveInfo> InnerList { get; set; } = new List<CfgSaveInfo>();
        public void Add(CfgSaveInfo info) => InnerList.Add(info);
        public int Count => InnerList.Count;
        public IEnumerator GetEnumerator() => InnerList.GetEnumerator();
        public CfgSaveInfoCollection DeepClone() => new CfgSaveInfoCollection { InnerList = new List<CfgSaveInfo>(InnerList) };
    }

    [Serializable]
    [MessagePackObject]
    public class CfgRestoreCommand
    {
        [Key(0)] public int CmdSeq { get; set; } = 0;
        [Key(1)] public string Cmd { get; set; } = string.Empty;
        [Key(2)] public string T_Prompt { get; set; } = string.Empty;
    }

    [Serializable]
    [MessagePackObject]
    public class CfgRestoreCommandCollection : IEnumerable
    {
        [Key(0)] public List<CfgRestoreCommand> InnerList { get; set; } = new List<CfgRestoreCommand>();
        public void Add(CfgRestoreCommand info) => InnerList.Add(info);
        public int Count => InnerList.Count;
        public IEnumerator GetEnumerator() => InnerList.GetEnumerator();
        public CfgRestoreCommandCollection DeepClone() => new CfgRestoreCommandCollection { InnerList = new List<CfgRestoreCommand>(InnerList) };
    }

    [Serializable]
    [MessagePackObject]
    public class FACT_DefaultConnectionCommand
    {
        [Key(0)] public int CMDSeq { get; set; } = 0;
        [Key(1)] public string CMD { get; set; } = string.Empty;
        [Key(2)] public string Prompt { get; set; } = string.Empty;
        [Key(3)] public string ErrorString { get; set; } = string.Empty;
        [Key(4)] public string TL1_CMD { get; set; } = string.Empty;
        [Key(5)] public string TL1_Prompt { get; set; } = string.Empty;
    }

    [Serializable]
    [MessagePackObject]
    public class FACT_DefaultConnectionCommandSet
    {
        [Key(0)] public List<FACT_DefaultConnectionCommand> CommandList { get; set; } = new List<FACT_DefaultConnectionCommand>();
        public FACT_DefaultConnectionCommandSet DeepClone() => new FACT_DefaultConnectionCommandSet { CommandList = new List<FACT_DefaultConnectionCommand>(CommandList) };
    }

    [Serializable]
    [MessagePackObject]
    public class DeviceCfgSaveInfo
    {
        [Key(0)] public string IPAddress { get; set; } = "";
        [Key(1)] public CfgSaveInfoCollection CfgSaveInfoCollection { get; set; } = new();
    }

    [Serializable]
    [MessagePackObject]
    public class DeviceCfgSaveInfoCollection : IEnumerable
    {
        [Key(0)] public List<DeviceCfgSaveInfo> InnerList { get; set; } = new();
        public void Add(DeviceCfgSaveInfo info) => InnerList.Add(info);
        public IEnumerator GetEnumerator() => InnerList.GetEnumerator();
    }
}
