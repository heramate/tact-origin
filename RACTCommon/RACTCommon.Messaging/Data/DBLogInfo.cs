using System;
using System.Collections.Generic;
using System.Text;
using MessagePack;

namespace RACTCommonClass
{
    [Serializable]
    [MessagePackObject]
    public class DBLogInfo
    {
        public DBLogInfo() { DateTime = DateTime.Now; }
        public DBLogInfo(E_DBLogType type, string msg) { LogType = type; Message = msg; DateTime = DateTime.Now; }

        [Key(0)] public string Message { get; set; } = "";
        [Key(1)] public E_DBLogType LogType { get; set; }
        [Key(2)] public DateTime DateTime { get; set; }
    }

    [Serializable]
    [MessagePackObject]
    public class DBExecuteCommandLogInfo : DBLogInfo
    {
        public DBExecuteCommandLogInfo() : base(E_DBLogType.ExecuteCommandLog, "") { }
        public DBExecuteCommandLogInfo(int connID, string cmd) : base(E_DBLogType.ExecuteCommandLog, "") { ConnectionLogID = connID; Command = cmd; }

        [Key(3)] public bool IsLimitCmd { get; set; }
        [Key(4)] public DeviceInfo DeviceInfo { get; set; }
        [Key(5)] public string Command { get; set; }
        [Key(6)] public int ConnectionLogID { get; set; }
    }

    [Serializable]
    [MessagePackObject]
    public class DBUserLogInfo : DBLogInfo
    {
        public DBUserLogInfo() : base(E_DBLogType.LoginLog, "") { }
        public DBUserLogInfo(int userID, E_UserLogType type) : base(E_DBLogType.LoginLog, "") { UserID = userID; UserLogType = type; }
        public DBUserLogInfo(int userID, E_UserLogType type, string msg) : base(E_DBLogType.LoginLog, msg) { UserID = userID; UserLogType = type; }

        [Key(3)] public E_UserLogType UserLogType { get; set; }
        [Key(4)] public int UserID { get; set; }
    }

    [Serializable] public enum E_UserLogType { Login = 0, LogOut = 1 }
    [Serializable] public enum E_DeviceConnectType { Connection = 0, DisConnection = 1 }
    [Serializable] public enum E_DeviceType { NeGroup = 1, UserNeGroup = 2 }
}
