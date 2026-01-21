using System;
using System.ComponentModel;

namespace TACT.KeepAliveServer
{
    /// <summary>
    /// 0.원격전원콘솔제어장치_기술규격서(20181114)_v01.docx 문서 참고
    /// (3.2 장치와 FACT간 LTE Cat.M1 통신 규격)
    /// 
    /// - TLV는 순서무관
    /// </summary>
    [Serializable]
    public static class TlvDef
    {
        #region KeepAlive TLV(Tag,Length,Value) 통신 규격 정의 ----------------------

        public static int c_TypeBytes = 1; // byte(s)
        public static int c_LengthBytes = 1; // byte(s)
        public static string c_LoopbackIPAddr = "127.0.0.255"; //장비(RPCS)와 매핑안된 KAM의 장비IP값

        /// <summary>
        /// Keep-Alive요청메시지(RPCS→KAMServer) 규약 (MIN, 1분 단위)
        /// </summary>
        public static E_KeepAliveType[] s_ReceiveStructureMin = 
        { 
            E_KeepAliveType.IPv4Address,            //RPCS 관리IPv4 (MIN)
            E_KeepAliveType.USIM,                   //USIM
            E_KeepAliveType.EndOfData
        };
        /// <summary>
        /// Keep-Alive요청메시지(RPCS→KAMServer) 규약 (Full메시지, 1시간 단위)
        /// </summary>
        public static E_KeepAliveType[] s_ReceiveStructureFull = 
        {
            E_KeepAliveType.ModelName,              //모델명 (FULL)
            E_KeepAliveType.IPv4Address,            //RPCS 관리IPv4 (FULL)
            E_KeepAliveType.USIM,                   //USIM
            E_KeepAliveType.IMEI,                   //IMEI
            E_KeepAliveType.SerialNumber,           //LTE SerialNumber
            E_KeepAliveType.LTEModuleName,          //LTE ModuleName
            E_KeepAliveType.EndOfData
        };

        /// <summary>
        /// 수신확인 응답(KAMServer→RPCS)메시지 규약 
        /// </summary>
        public static E_KeepAliveType[] s_ReplyStructureMin = 
        { 
            E_KeepAliveType.IPv4Address,            //RPCS 관리IPv4 (MIN)
            E_KeepAliveType.USIM,                   //USIM
            E_KeepAliveType.EndOfData
        };
        public static string s_ReplyStructureMinDBColumns = "DeviceIP, USIM"; //TBL: LTE_NE

        /// <summary>
        /// 응답메시지 full포맷(SSH터널 Open/Close요청)
        /// </summary>
        public static E_KeepAliveType[] s_ReplyStructureFull = 
        {
            E_KeepAliveType.ModelName,              //모델명 (FULL)
            E_KeepAliveType.IPv4Address,            //RPCS 관리IPv4 (FULL)
            E_KeepAliveType.USIM,                   //USIM
            E_KeepAliveType.SSHTunnelCreateOption,  //SSH 터널 Open/Close요청 플래그
            E_KeepAliveType.SSHServerDomain,        //SSH 접속 도메인(IP) (SSH)
            E_KeepAliveType.SSHPort,                //SSH 접속 PORT (SSH)
            E_KeepAliveType.SSHTunnelPort,          //SSH 터널용 PORT (SSH)
            E_KeepAliveType.SSHUserID,              //SSH 접속 계정 (SSH)
            E_KeepAliveType.SSHPassword,            //SSH 접속 PWD (SSH)
            E_KeepAliveType.EndOfData
        };
        public static string s_ReplyStructurFullDBColumns = 
        "DeviceModelName, DeviceIP, USIM, "; //TBL: LTE_NE

        #endregion KeepAlive TLV(Tag,Length,Value) 통신 규격 정의 -------------------
    }

    [Serializable]
    public enum E_KeepAliveType : byte
    {
        /// <summary>
        /// End of data
        /// </summary>
        EndOfData = 0,
        /// <summary>
        /// 모델명, 가변길이 (string format, without null termination)
        /// </summary>
        ModelName = 1,
        /// <summary>
        /// Cat.M1 Serial number, 가변길이 (string format, without null termination)
        /// </summary>
        SerialNumber = 2,
        /// <summary>
        /// Cat.M1 USIM 정보, 가변길이 (string format, without null termination)
        /// </summary>
        USIM = 3, //Universal Subscriber Identity Module, 가입자 구별번호
        /// <summary>
        /// Cat.M1 IMEI 정보, 가변길이 (string format, without null termination)
        /// </summary>
        IMEI = 4, //International Mobile Equipment Identity, 단말기 고유 일련번호(단말기 제조사+모델등의 정보를 포함)
        /// <summary>
        /// 장치본체 IPv4 정보, 4 bytes (htonl) - 장비구분Key
        /// </summary>
        IPv4Address = 5,

        /// <summary>
        /// SSH 서버 포트, 2 bytes (htons)
        /// </summary>
        SSHPort = 6,
        /// <summary>
        /// SSH 서버 도메인(or IP), 가변길이 (string format, without null termination)
        /// </summary>
        SSHServerDomain = 7,
        /// <summary>
        /// SSH Tunnel 생성요청 여부, 1 bytes (0xFF: close, 0x01: create)
        /// </summary>
        SSHTunnelCreateOption = 8,
        /// <summary>
        /// LTE Cat.M1 모듈명, 가변길이 (string format, without null termination)
        /// </summary>
        LTEModuleName = 9,
        /// <summary>
        /// SSH remote port, 2 bytes (htons)
        /// </summary>
        SSHTunnelPort = 10,
        /// <summary>
        /// SSH 서버 접속 계정, 가변길이 (string format, without null termination)
        /// </summary>
        SSHUserID = 11,
        /// <summary>
        /// SSH 서버 접속 패스워드, 가변길이 (string format, without null termination)
        /// </summary>
        SSHPassword = 12,

        /// <summary>
        /// 값 유효범위 체크용
        /// </summary>
        MaxValue = SSHPassword //= Math.Pow(2, TLVKeepAlive.c_TagBitLength)
    }

    [Serializable]
    public enum E_SSHTunnelCreateOption : byte
    {
        /// <summary>
        /// 기본값(송신메시지 일때는 Full Keep-Alive에 대한 reply를 의미)
        /// </summary>
        [Description("수신확인")]
        Unknown = 0,

        /// <summary>
        /// SSH터널 Close 요청
        /// </summary>
        [Description("터널Close요청")]
        Close = 255, //0xFF

        /// <summary>
        /// SSH터널 Open 요청
        /// </summary>
        [Description("터널Open요청")]
        Open = 1   // 0x01
    }

}