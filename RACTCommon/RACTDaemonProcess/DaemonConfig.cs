/// 2019.02.01 KwonTaeSuk 환경설정파일 통합(DaemonConfig.xml, RACTDaemonProcess.DaemonConfig.cs)
using System;
using System.Collections.Generic;
using System.Text;

namespace RACTDaemonProcess
{
    /// <summary>
    /// 서버 설정 정보입니다.
    /// </summary>
    public class DaemonConfig
    {
        /// <summary>
        /// 데몬 통합 config 입니다.
        /// </summary>
        public static readonly string s_DaemonConfigFileName = "DaemonConfig.xml";

        #region TACT Server 관련 설정값 ---------------------------------------------
        /// <summary>
        /// 서버 IPAddress
        /// </summary>
        private string m_ServerIP = "118.217.79.41";

        /// <summary>
        /// 서버 IPAddress를 가져오거나 설정합니다.
        /// </summary>
        public string ServerIP
        {
            get { return m_ServerIP; }
            set { m_ServerIP = value; }
        }
        #endregion TACT Server 관련 설정값 ------------------------------------------

        #region RACTDaemonProcess 관련 설정값 ---------------------------------------

        /// <summary>
        /// 데몬 IP
        /// </summary>
        private string m_DaemonIP = "118.217.79.41";
        /// <summary>
        /// 데몬 포트
        /// </summary>
        private int m_DaemonPort = 43216;
        /// <summary>
        /// 데몬 실행 갯수
        /// </summary>
        private int m_DaemonStartCount = 1;
        /// <summary>
        /// 데몬 리모트 채널명 PRIFIX 입니다.
        /// (실 채널명은 [DaemonChannelName+DaemonPort~데몬포트]로 구성, ex:RACTDaemonChannel43216, RACTDaemonChannel43217))
        /// </summary>
        private string m_DaemonChannelName = "RACTDaemonChannel";

        /// <summary>
        /// Daemon IPAddress를 가져오거나 설정합니다.
        /// </summary>
        public string DaemonIP
        {
            get { return m_DaemonIP; }
            set { m_DaemonIP = value; }
        }
        /// <summary>
        /// Daemon Remote 포트를 가져오거나 설정합니다.
        /// </summary>
        public int DaemonPort
        {
            get { return m_DaemonPort; }
            set { m_DaemonPort = value; }
        }
        /// <summary>
        /// 실행 개수 가져오거나 설정 합니다.
        /// </summary>
        public int DaemonStartCount
        {
            get { return m_DaemonStartCount; }
            set { m_DaemonStartCount = value; }
        }
        /// <summary>
        /// 데몬 리모트 채널명 PRIFIX 가져오거나 설정 합니다.
        /// </summary>
        public string DaemonChannelName
        {
            get { return m_DaemonChannelName; }
            set { m_DaemonChannelName = value; }
        }
        #endregion RACTDaemonProcess 관련 설정값 ------------------------------------


        #region RACTDaemonLauncher 관련 설정값 --------------------------------------

        /// <summary>
        /// 데몬런처 IP
        /// </summary>
        private string m_LauncherIP = "10.30.5.140";
        /// <summary>
        /// Launcher Port
        /// </summary>
        private int m_LauncherPort = 43218;
        /// <summary>
        /// Launcher Channel name 입니다.
        /// </summary>
        private string m_LauncherChannelName = "RactLauncherChannel";
        /// <summary>
        /// 데몬 런처 윈도우서비스명 (ServiceController 검색용)
        /// </summary>
        private string m_LauncherServiceName = "RACS_Daemon_Launcher";
        

        /// <summary>
        /// 데몬런처 IP get/set
        /// </summary>
        public string LauncherIP
        {
            get { return m_LauncherIP; }
            set { m_LauncherIP = value; }
        }
        /// <summary>
        /// Launcher Channel name  get/set
        /// </summary>
        public string LauncherChannelName
        {
            get { return m_LauncherChannelName; }
            set { m_LauncherChannelName = value; }
        }
        /// <summary>
        /// 데몬런처 포트 get/set
        /// </summary>
        public int LauncherPort
        {
            get { return m_LauncherPort; }
            set { m_LauncherPort = value; }
        }
        /// <summary>
        /// Launcher Channel name  get/set
        /// </summary>
        public string LauncherServiceName
        {
            get { return m_LauncherServiceName; }
            set { m_LauncherServiceName = value; }
        }
        
        #endregion RACTDaemonLauncher 관련 설정값 -----------------------------------

        /// <summary>
        /// Server Daemon Channel Name 입니다.
        /// </summary>
        private string m_ServerDaemonChannelName = "RemoteDaemon";
        /// <summary>
        /// Server Daemon Port 입니다.
        /// </summary>
        private int m_ServerDaemonPort = 65431;

        /// <summary>
        /// Server Daemon Channel Name  get/set
        /// </summary>
        public string ServerDaemonChannelName
        {
            get { return m_ServerDaemonChannelName; }
            set { m_ServerDaemonChannelName = value; }
        }
        /// <summary>
        /// Server Daemon Port 가져오거나 설정 합니다.
        /// </summary>
        public int ServerDaemonPort
        {
            get { return m_ServerDaemonPort; }
            set { m_ServerDaemonPort = value; }
        }


        #region [18고도화(c-RPCS원격접속)] KAMServer(Keep-Alive Message Server, Cat.M1을 통한 LTE장비접속 기능개발) ---

        /// <summary>
        /// KAMServer접속여부(데몬의 LTE 연결 기능 On/Off)
        /// </summary>
        private bool m_KAMServerConnectEnable = false;
        /// <summary>
        /// KAMServer 접속여부(데몬의 LTE 연결 기능 on/off)
        /// </summary>
        public bool KAMServerConnectEnable
        {
            get { return m_KAMServerConnectEnable; }
            set { m_KAMServerConnectEnable = value; }
        }

        /// <summary>
        /// KAMServer IP
        /// </summary>
        private string m_KAMServerIP = "118.217.79.23";
        /// <summary>
        /// KAMServer IPAddress를 가져오거나 설정합니다.
        /// </summary>
        public string KAMServerIP
        {
            get { return m_KAMServerIP; }
            set { m_KAMServerIP = value; }
        }

        /// <summary>
        /// KAMServer 원격 포트
        /// </summary>
        private int m_KAMServerPort = 40009;
        /// <summary>
        /// KAMServer 원격 포트를 가져오거나 설정합니다.
        /// </summary>
        public int KAMServerPort
        {
            get { return m_KAMServerPort; }
            set { m_KAMServerPort = value; }
        }

        /// <summary>
        /// KAMServer 원격 채널명
        /// </summary>
        private string m_KAMServerChannel = "RemoteKAMServer";
        /// <summary>
        /// KAMServer 원격 채널을 가져오거나 설정합니다.
        /// </summary>
        public string KAMServerChannel
        {
            get { return m_KAMServerChannel; }
            set { m_KAMServerChannel = value; }
        }

        #endregion [18고도화(c-RPCS원격접속)] KAMServer(Keep-Alive Message Server, Cat.M1을 통한 LTE장비접속 기능개발) ---


    }
}
