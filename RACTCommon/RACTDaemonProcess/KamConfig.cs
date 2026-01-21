using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RACTDaemonProcess
{
    public class KamConfig
    {
        /// <summary>
        /// 데몬 환경파일 이름 입니다.
        /// </summary>
        public static string s_KamConfigFileName = "KamConfig.xml";

        /// <summary>
        /// 서버 IPAddress
        /// </summary>
        //private string m_ServerIP = "118.217.79.15";
        private bool m_KamOnOff = false;
        /// <summary>
        /// 서버 IPAddress를 가져오거나 설정합니다.
        /// </summary>
        public bool KamOnOff
        {
            get { return m_KamOnOff; }
            set { m_KamOnOff = value; }
        }

        /// <summary>
        /// 서버 IPAddress
        /// </summary>
        //private string m_ServerIP = "118.217.79.15";
        private string m_ServerIP = "10.30.6.94";
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
        private int m_ServerPort = 43215;
        /// <summary>
        /// 서버 Remote 포트를 가져오거나 설정합니다.
        /// </summary>
        public int ServerPort
        {
            get { return m_ServerPort; }
            set { m_ServerPort = value; }
        }

    }
}
