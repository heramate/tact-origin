using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TACT.KeepAliveServer
{
    /// <summary>
    /// 서버 설정 정보입니다.
    /// </summary>
    public class SystemConfig
    {
        /// <summary>
        /// 서버 ID 입니다.
        /// </summary>
        private int m_ServerID = 0;
        /// <summary>
        /// 서버 ID 속성을 가져오거나 설정합니다.
        /// </summary>
        public int ServerID
        {
            get { return m_ServerID; }
            set { m_ServerID = value; }
        }

        /// <summary>
        /// 서버 IPAddress
        /// </summary>
        private string m_ServerIP = string.Empty;
        /// <summary>
        /// 서버 IPAddress를 가져오거나 설정합니다.
        /// </summary>
        public string ServerIP
        {
            get { return m_ServerIP; }
            set { m_ServerIP = value; }
        }

        /// <summary>
        /// Remote 서버 채널
        /// </summary>
        private string m_ServerChannel = "RemoteKAMServer";
        /// <summary>
        /// 서버 Remote 채널을 가져오거나 설정합니다.
        /// </summary>
        public string ServerChannel
        {
            get { return m_ServerChannel; }
            set { m_ServerChannel = value; }
        }
        /// <summary>
        /// Remote 서버 포트(KAM수신 UDP포트)
        /// </summary>
        private int m_ServerPort = 40001;
        /// <summary>
        /// 서버 수신 포트를 가져오거나 설정합니다.
        /// </summary>
        public int ServerPort
        {
            get { return m_ServerPort; }
            set { m_ServerPort = value; }
        }

        /// <summary>
        /// DB Server IPAddress 입니다.
        /// </summary>
        private string m_DBServerIP = string.Empty;
        /// <summary>
        /// DB Server IPAddress 속성을 가져오거나 설정합니다.
        /// </summary>
        public string DBServerIP
        {
            get { return m_DBServerIP; }
            set { m_DBServerIP = value; }
        }
        /// <summary>
        /// DB 명 입니다.
        /// </summary>
        private string m_DBName = string.Empty;
        /// <summary>
        /// DB 명 속성을 가져오거나 설정합니다.
        /// </summary>
        public string DBName
        {
            get { return m_DBName; }
            set { m_DBName = value; }
        }
        /// <summary>
        /// DB 사용자 계정 입니다.
        /// </summary>
        private string m_UserID = string.Empty;
        /// <summary>
        /// DB 사용자 계정 속성을 가져오거나 설정합니다.
        /// </summary>
        public string UserID
        {
            get { return m_UserID; }
            set { m_UserID = value; }
        }
        /// <summary>
        /// DB 사용자 패스워드 입니다.
        /// </summary>
        private string m_Password = string.Empty;
        /// <summary>
        /// DB 사용자 패스워드 속성을 가져오거나 설정합니다.
        /// </summary>
        public string Password
        {
            get { return m_Password; }
            set { m_Password = value; }
        }
        /// <summary>
        /// DB 연결 개수 입니다.
        /// </summary>
        private int m_DBConnectionCount = 3;
        /// <summary>
        /// DB 연결 개수 속성을 가져오거나 설정합니다.
        /// </summary>
        public int DBConnectionCount
        {
            get { return m_DBConnectionCount; }
            set { m_DBConnectionCount = value; }
        }

        /// <summary>
        /// 데몬Remote 서버 IP
        /// </summary>
        private string m_DServerIP = "";
        /// <summary>
        /// 데몬Remote 서버를 가져오거나 설정합니다.
        /// </summary>
        public string DServerIP
        {
            get { return m_DServerIP; }
            set { m_DServerIP = value; }
        }
        /// <summary>
        /// 데몬Remote 서버 채널
        /// </summary>
        private string m_DServerChannel = "";
        /// <summary>
        /// 데몬Remote 서버 채널을 가져오거나 설정합니다.
        /// </summary>
        public string DServerChannel
        {
            get { return m_DServerChannel; }
            set { m_DServerChannel = value; }
        }
        /// <summary>
        /// 데몬Remote 서버 포트
        /// </summary>
        private int m_DServerPort = 0;
        /// <summary>
        /// 데몬Remote 서버 포트를 가져오거나 설정합니다.
        /// </summary>
        public int DServerPort
        {
            get { return m_DServerPort; }
            set { m_DServerPort = value; }
        }

        /// <summary>
        /// SSH Server IP
        /// </summary>
        private string m_SSHServerIP = string.Empty;
        /// <summary>
        /// SSH Server IP get/set
        /// </summary>
        public string SSHServerIP
        {
            get { return m_SSHServerIP; }
            set { m_SSHServerIP = value; }
        }        

        /// <summary>
        /// SSH Server 접속포트
        /// </summary>
        private int m_SSHServerPort = 0;
        /// <summary>
        /// SSH Server 접속포트 get/set
        /// </summary>
        public int SSHServerPort
        {
            get { return m_SSHServerPort; }
            set { m_SSHServerPort = value; }
        }        

        /// <summary>
        /// SSH Server 접속ID
        /// </summary>
        private string m_SSHUserID = string.Empty;
        /// <summary>
        /// SSH Server 접속ID get/set
        /// </summary>
        public string SSHUserID
        {
            get { return m_SSHUserID; }
            set { m_SSHUserID = value; }
        }

        /// <summary>
        /// SSH Server 접속 패스워드
        /// </summary>
        private string m_SSHPassword = string.Empty;
        /// <summary>
        /// SSH Server 접속 패스워드 get/set
        /// </summary>
        public string SSHPassword
        {
            get { return m_SSHPassword; }
            set { m_SSHPassword = value; }
        }

        /// <summary>
        /// SSH Tunnel용 할당 포트 범위(min)
        /// </summary>
        private ushort m_SSHTunnelPortMin = 0;
        public ushort SSHTunnelPortMin
        {
            get { return m_SSHTunnelPortMin; }
            set { m_SSHTunnelPortMin = value; }
        }

        /// <summary>
        /// SSH Tunnel용 할당 포트 범위(max)
        /// </summary>
        private ushort m_SSHTunnelPortMax = 0;
        public ushort SSHTunnelPortMax
        {
            get { return m_SSHTunnelPortMax; }
            set { m_SSHTunnelPortMax = value; }
        }

        /// <summary>
        /// SSH Tunnel용 할당 포트 범위
        /// </summary>
        private string m_SSHTunnelPortRange = "";
        /// <summary>
        /// SSH Tunnel용 할당 포트 범위 get/set
        /// </summary>
        public string SSHTunnelPortRange
        {
            get { return m_SSHTunnelPortRange; }
            set { 
                m_SSHTunnelPortRange = value;

                if (m_SSHTunnelPortRange.Length > 3 && m_SSHTunnelPortRange.IndexOf(',') >= 0)
                {
                    string[] ports = m_SSHTunnelPortRange.Split(',');
                    try
                    {
                        if (ports.Length > 0 && !string.IsNullOrEmpty(ports[0])) SSHTunnelPortMin = Convert.ToUInt16(ports[0]);
                        if (ports.Length > 1 && !string.IsNullOrEmpty(ports[1])) SSHTunnelPortMax = Convert.ToUInt16(ports[1]);
                    }
                    catch (Exception ex)
                    {
                        GlobalClass.PrintLogException("[SystemConfig.SSHTunnelPortRange.set] SSH터널용 할당 포트 설정값은 N,M 형식이어야합니다.", ex);
                    }
                }
            }
        }

        /// <summary>
        /// SSH터널 사용여부 판단 Timeout (초)
        /// (0이면 Timeout체크 안함)
        /// </summary>
        private UInt32 m_SSHTunnelUseTimeoutSeconds = 0;
        public UInt32 SSHTunnelUseTimeoutSeconds
        {
            get { return m_SSHTunnelUseTimeoutSeconds; }
            set { m_SSHTunnelUseTimeoutSeconds = value; }
        }

        /// <summary>
        /// KeepAlive 메시지 Base64 인코딩 적용 여부
        /// (TLV(Type-Length-Value)중 prefix인 'FACT'를 포함한 전체 Value 인코딩)
        /// </summary>
        private bool m_KeepAliveBase64Encode = true;
        public bool KeepAliveBase64Encode
        {
            get { return m_KeepAliveBase64Encode; }
            set { m_KeepAliveBase64Encode = value; }
        }

        /// <summary>
        /// 데몬요청 처리 Timeout 
        /// (KeepAlive메시지가 오지 않아서 요청을 처리하지 못하는 경우)
        /// </summary>
        private UInt32 m_DaemonRequestTimeoutSeconds = 5 * 60;
        public UInt32 DaemonRequestTimeoutSeconds
        {
            get { return m_DaemonRequestTimeoutSeconds; }
            set { m_DaemonRequestTimeoutSeconds = value; }
        }

        /// <summary>
        /// 장비에 터널Open/Close 요청하였으나 반응(포트상태 변화)없음
        /// </summary>
        private UInt32 m_TunnelRequestTimeoutSeconds = 0;
        public UInt32 TunnelRequestTimeoutSeconds
        {
            get { return m_TunnelRequestTimeoutSeconds; }
            set { m_TunnelRequestTimeoutSeconds = value; }
        }

        /// <summary>
        /// 파일로그에 모든 정보를 출력할지 여부 (GlobalClass.PrintLogXXX() 에서 플래그값 사용)
        /// </summary>
        private bool m_FileLogDetailYN = false;
        public bool FileLogDetailYN
        {
            get { return m_FileLogDetailYN; }
            set { m_FileLogDetailYN = value; }
        }

    #region 2019.11.11 터널Close요청 재발송 기능 추가 - 장비측 요청(11/8판교회의)
        /// <summary>
        /// [터널Close요청] 재발송 주기(초)
        /// </summary>
        private UInt32 m_TunnelRequestSendPeriodSeconds = 10;
        public UInt32 TunnelRequestSendPeriodSeconds
        {
            get { return m_TunnelRequestSendPeriodSeconds; }
            set { m_TunnelRequestSendPeriodSeconds = value; }
        }

        /// <summary>
        /// [터널Close요청] 재발송 횟수 (기본은 1회 발송)
        /// </summary>
        private UInt32 m_TunnelRequestCount = 1;
        public UInt32 TunnelRequestCount
        {
            get { return m_TunnelRequestCount; }
            set { m_TunnelRequestCount = value; }
        }
    #endregion //2019.11.11 터널Close요청 재발송 기능 추가

    } // End of class (SystemConfig)
}
