using System;
using System.Collections.Generic;
using System.Net;
using Serilog;

namespace TACT.KeepAliveServer
{
    public static class GlobalClass
    {
        public static string StartupPath { get; set; } = string.Empty;
        public const string SystemConfigFileName = @"/KAMServerConfig.xml";
        public static SystemConfig m_SystemInfo { get; set; } = new SystemConfig();
        public static bool IsServerStop { get; set; } = false;

        public static void PrintLogError(string msg, bool detail = false) => Log.Error(msg);
        public static void PrintLogInfo(string msg, bool detail = false) => Log.Information(msg);
        public static void PrintLogException(string title, Exception e, bool detail = false) => Log.Error(e, title);
    }

    public static class Util
    {
        public const string DateTimeFormatMDHMS = "MM/dd HH:mm:ss";
        public const string DateTimeFormatYMDHMS = "yyyy-MM-dd HH:mm:ss";

        public static string QuotedStr(string value, string ends = "'") => $"{ends}{value}{ends}";

        public static string DateTimeToDBValue(DateTime value) => 
            value == DateTime.MinValue ? "null" : QuotedStr(value.ToString(DateTimeFormatYMDHMS));

        public static string DateTimeToLogValue(DateTime value) => 
            value == DateTime.MinValue || value == DateTime.MaxValue ? "" : value.ToString(DateTimeFormatMDHMS);

        public static bool IsValidIPAddress(string ip) => IPAddress.TryParse(ip, out _);

        public static void Assert(bool condition, string message)
        {
            if (!condition) Log.Warning("Assertion Failed: {Message}", message);
        }
    }
}
