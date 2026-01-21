using System;
using System.Collections.Generic;
using System.Text;
using RACTDaemonProcess;

namespace RACTServer
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
        private string m_ServerIP = "";
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
        private string m_ServerChannel = "RemoteClient";
        /// <summary>
        /// 서버 Remote 채널을 가져오거나 설정합니다.
        /// </summary>
        public string ServerChannel
        {
            get { return m_ServerChannel; }
            set { m_ServerChannel = value; }
        }

        /// <summary>
        /// Remote 서버 포트
        /// </summary>
        private int m_ServerPort = 54321;
        /// <summary>
        /// 서버 Remote 포트를 가져오거나 설정합니다.
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
        /// 기본 데몬 실행 개수 입니다.
        /// </summary>
        private int m_DefaultDaemonRunCount = 0;

        /// <summary>
        /// 기본 데몬 실행 개수 가져오거나 설정 합니다.
        /// </summary>
        public int DefaultDaemonRunCount
        {
            get { return m_DefaultDaemonRunCount; }
            set { m_DefaultDaemonRunCount = value; }
        }

        /// <summary>
        /// Daemon Use Default Port 입니다.
        /// </summary>
        private int m_DaemonUsePort= 43211;

        /// <summary>
        /// Daemon Use Default Port 가져오거나 설정 합니다.
        /// </summary>
        public int DaemonUsePort
        {
            get { return m_DaemonUsePort; }
            set { m_DaemonUsePort = value; }
        }

        /// <summary>
        /// Daemon Connection Channel Name 입니다.
        /// </summary>
        private string m_DaemonChannelName = "RemoteDaemon";

        /// <summary>
        /// Daemon Connection Channel Name 가져오거나 설정 합니다.
        /// </summary>
        public string DaemonChannelName
        {
            get { return m_DaemonChannelName; }
            set { m_DaemonChannelName = value; }
        }
        /// <summary>
        /// ServiceManager가 사용하는 Port 입니다.
        /// </summary>
        private int m_ServiceManagerUsePort = 23451;
        /// <summary>
        /// ServiceManager 속성을 가져오거나 설정합니다.
        /// </summary>
        public int ServiceManagerUsePort
        {
            get { return m_ServiceManagerUsePort; }
            set { m_ServiceManagerUsePort = value; }
        }
        /// <summary>
        /// ServiceManager 이름 입니다.
        /// </summary>
        private string m_ServiceManagerChannelName = "RemoteServiceManager";
        /// <summary>
        /// ServiceManager 이름 속성을 가져오거나 설정합니다.
        /// </summary>
        public string ServiceManagerChannelName
        {
            get { return m_ServiceManagerChannelName; }
            set { m_ServiceManagerChannelName = value; }
        }

        private int m_ReloadHour = 0;

        public int ReloadHour
        {
            get { return m_ReloadHour; }
            set { m_ReloadHour = value; }
        }

        private int m_ReloadMinute = 0;

        public int ReloadMinute
        {
            get { return m_ReloadMinute; }
            set { m_ReloadMinute = value; }
        }

        #region [2018,c-RPCS과제] LTE Cat.M1을 통한 LTE장비접속 지원 기능 ---------------------------
        /// <summary>
        /// SSH터널링 가능한 데몬의 서버IP 목록
        /// (해당 서버에서만 RPCS장비에 LTE접속을 위한 SSH터널링 포트를 생성할 수 있다)
        /// </summary>
        private List<string> m_SSHTunnelDaemonIPList = new List<string>();
        /// <summary>
        /// SSH터널링 가능한 데몬의 서버IP 목록
        /// (해당 서버에서만 RPCS장비에 LTE접속을 위한 SSH터널을 생성할 수 있다)
        /// </summary>
        //public List<string> SSHTunnelDaemonIPList { get; set; }
        public int GetSSHTunnelDaemonIPCount()
        {
            return m_SSHTunnelDaemonIPList.Count;
        }
        public bool IsSSHTunnelDaemonIP(string ip)
        {
            return m_SSHTunnelDaemonIPList.IndexOf(ip) > -1;
        }

        private string m_SSHTunnelDaemonIP = string.Empty;
        public string SSHTunnelDaemonIP 
        { 
            get { return m_SSHTunnelDaemonIP;  }

            set { 
                m_SSHTunnelDaemonIP = value;
                m_SSHTunnelDaemonIPList.Clear();

                if (!string.IsNullOrEmpty(m_SSHTunnelDaemonIP))
                {
                    string[] ipList = m_SSHTunnelDaemonIP.Split(',');
                    try
                    {
                        foreach (string daemonIP in ipList)
                        {
                            if (string.IsNullOrEmpty(daemonIP)) continue;
                            m_SSHTunnelDaemonIPList.Add(daemonIP);
                            System.Diagnostics.Debug.WriteLine(string.Format("[SystemConfig][환경설정 로드] SSH터널링 서버IP {0}추가", daemonIP));
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("[E][SystemConfig.SSHTunnelPortRange.set] " + ex.ToString());
                    }
                }
            }
        }

        #endregion [2018,c-RPCS과제] LTE Cat.M1을 통한 LTE장비접속 지원 기능 --------------------
    }
}
