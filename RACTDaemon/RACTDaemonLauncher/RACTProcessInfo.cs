using System;
using System.Collections.Generic;
using System.Text;

namespace RACTDaemonLauncher
{
    [Serializable]
    public class RACTDaemonPocessInfo
    {
        /// <summary>
        /// 서버 IP 입니다.
        /// </summary>
        private string m_DaemonIPAddress;
        /// <summary>
        /// 서버 Port 입니다.
        /// </summary>
        private int m_DaemonPort;

        /// <summary>
        /// 프로세스 상태 입니다.
        /// </summary>
        private E_ProcessStatus m_ProcessStatus;

        /// <summary>
        /// 프로세스 상태 가져오거나 설정 합니다.
        /// </summary>
        public E_ProcessStatus ProcessStatus
        {
            get { return m_ProcessStatus; }
            set { m_ProcessStatus = value; }
        }


        public RACTDaemonPocessInfo(HealthCheckItem aItem)
        {
            m_DaemonPort = aItem.DaemonPort;
            m_DaemonIPAddress = aItem.DaemonIPAddress;
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
            get { return m_DaemonIPAddress + "#" + m_DaemonPort; }
        }
    }
}
