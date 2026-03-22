using System;
using System.Collections.Generic;
using System.Collections;
using MessagePack;

namespace RACTCommonClass
{
    [Serializable]
    [MessagePackObject]
    public class DeviceConnectionHistoryInfo
    {
        [Key(0)] public int ID { get; set; } = 0;
        [Key(1)] public int DeviceID { get; set; } = 0;
        [Key(2)] public string DeviceName { get; set; } = string.Empty;
        [Key(3)] public DateTime ConnectionTime { get; set; } = DateTime.MinValue;
        [Key(4)] public DateTime EndTime { get; set; } = DateTime.MinValue;
        [Key(5)] public E_DeviceConnectType ConnectionType { get; set; } = E_DeviceConnectType.Connection;
        [Key(6)] public string Description { get; set; } = string.Empty;
        [Key(7)] public string IPAddress { get; set; } = string.Empty;
    }

    [Serializable]
    [MessagePackObject]
    public class ConnectionHistoryInfoCollection : IEnumerable
    {
        [Key(0)] public List<DeviceConnectionHistoryInfo> InnerList { get; set; } = new();
        public void Add(DeviceConnectionHistoryInfo info) => InnerList.Add(info);
        public IEnumerator GetEnumerator() => InnerList.GetEnumerator();
        public ConnectionHistoryInfoCollection DeepClone() => new ConnectionHistoryInfoCollection { InnerList = new List<DeviceConnectionHistoryInfo>(InnerList) };
    }

    [Serializable]
    [MessagePackObject]
    public class TelnetCommandHistoryInfo
    {
        [Key(0)] public DateTime Time { get; set; } = DateTime.MinValue;
        [Key(1)] public string Command { get; set; } = string.Empty;
    }

    [Serializable]
    [MessagePackObject]
    public class TelnetCommandHistoryInfoCollection : IEnumerable
    {
        [Key(0)] public List<TelnetCommandHistoryInfo> InnerList { get; set; } = new();
        public void Add(TelnetCommandHistoryInfo info) => InnerList.Add(info);
        public IEnumerator GetEnumerator() => InnerList.GetEnumerator();
        public TelnetCommandHistoryInfoCollection DeepClone() => new TelnetCommandHistoryInfoCollection { InnerList = new List<TelnetCommandHistoryInfo>(InnerList) };
    }
}
