using System;
using System.ComponentModel;

namespace TACT.KeepAliveServer
{
    public enum E_RequestTunnelResult
    {
        Canceled,
        Opened,
        RequestTunnel,
        UserSessionFull,
        NoUseableTunnelPort,
        NoKeepAlive,
    }

    public enum E_TunnelState
    {
        [Description("터널포트할당")] Closed,
        [Description("터널Open대기")] WaitingOpen,
        [Description("터널Open완료")] Opened,
        [Description("터널접속사용중")] Connected,
        [Description("터널Close대기")] WaitingClose
    }

    public enum E_KeepAliveType : byte
    {
        EndOfData = 0,
        ModelName = 1,
        SerialNumber = 2,
        USIM = 3,
        IMEI = 4,
        IPv4Address = 5,
        SSHPort = 6,
        SSHServerDomain = 7,
        SSHTunnelCreateOption = 8,
        LTEModuleName = 9,
        SSHTunnelPort = 10,
        SSHUserID = 11,
        SSHPassword = 12
    }

    public enum E_SSHTunnelCreateOption : byte
    {
        [Description("수신확인")] Unknown = 0,
        [Description("터널Close요청")] Close = 255,
        [Description("터널Open요청")] Open = 1
    }

    public enum E_FileLogType
    {
        Info,
        Error,
        Warning,
        Debug
    }
}

