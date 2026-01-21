using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace RACTDaemonLauncher
{

    /// <summary>
    /// 체크할 Daemon 서버 정보 입니다.
    /// </summary>
    [Serializable]
    public class HealthCheckItem
    {
        /// <summary>
        /// 접속할 Daemon IP 입니다.
        /// </summary>
        private string m_DaemonIPAddress;
        /// <summary>
        /// 접속할 Daemon Port 입니다.
        /// </summary>
        private int m_DaemonPort;
        /// <summary>
        /// 실행 파일 위치 입니다.
        /// </summary>
        private string m_ExecutionFilePath;
        /// <summary>
        /// 파일 실행에 필요한 arg 입니다.
        /// </summary>
        private string m_ExecutionArg = "";
        /// <summary>
        /// Process ID 입니다.
        /// </summary>
        private int m_ProcessID = 0;
        /// <summary>
        /// 사용자가 중지 했는지 여부 입니다.
        /// </summary>
        private bool m_AutoStart = true;

        /// <summary>
        /// Daemon Channel Name 입니다.
        /// </summary>
        private string m_DaemonChannelName;

        /// <summary>
        /// Daemon Channel Name 가져오거나 설정 합니다.
        /// </summary>
        public string DaemonChannelName
        {
            get { return m_DaemonChannelName; }
            set { m_DaemonChannelName = value; }
        }



        /// <summary>
        /// 사용자가 중지 했는지 여부 가져오거나 설정 합니다.
        /// </summary>
        public bool AutoStart
        {
            get { return m_AutoStart; }
            set { m_AutoStart = value; }
        }
        /// <summary>
        /// Process ID 가져오거나 설정 합니다.
        /// </summary>
        public int ProcessID
        {
            get { return m_ProcessID; }
            set { m_ProcessID = value; }
        }

        public HealthCheckItem()
        {
            m_ExecutionFilePath = Application.StartupPath + "\\RACTDaemonExe.exe";
        }

        /// <summary>
        /// 파일 실행에 필요한 arg 가져오거나 설정 합니다.
        /// </summary>
        public string ExecutionArg
        {
            get { return m_ExecutionArg; }
            set { m_ExecutionArg = value; }
        }
        /// <summary>
        /// 실행 파일 위치를 설정 합니다.
        /// </summary>
        public string ExecutionFilePath
        {
            get { return m_ExecutionFilePath; }
        }
        /// <summary>
        /// 접속할 서버 Port 가져오거나 설정 합니다.
        /// </summary>
        public int DaemonPort
        {
            get { return m_DaemonPort; }
            set { m_DaemonPort = value; }
        }
        /// <summary>
        /// 접속할 서버 IP 가져오거나 설정 합니다.
        /// </summary>
        public string DaemonIPAddress
        {
            get { return m_DaemonIPAddress; }
            set { m_DaemonIPAddress = value; }
        }
        /// <summary>
        /// 키를 가져오기 합니다.
        /// </summary>
        public string Key
        {
            get { return DaemonIPAddress+"#"+DaemonPort; }
        }
    }
}