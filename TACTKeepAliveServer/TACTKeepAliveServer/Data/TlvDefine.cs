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


}
