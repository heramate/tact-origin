using System;
using System.Collections.Generic;
using System.Text;

namespace RACTCommonClass
{
    /// <summary>
    /// 빠른연결 접속 정보입니다.
    /// </summary>
    [Serializable]
    public class TerminalConnectInfo
    {

        /// <summary>
        /// 연결 타입 입니다.
        /// </summary>
        private E_ConnectionProtocol m_ConnectionProtocol;

        /// <summary>
        /// Serial Config 입니다.
        /// </summary>
        private SerialConfig m_SerianConfig;
        /// <summary>
        /// IP Address 입니다.
        /// </summary>
        private string m_IPAddress="";

        /// <summary>
        /// Telnet Port 입니다.
        /// </summary>
        private int m_TelnetPort=23;

        /// <summary>
        /// 2013-01-28 SSH 연결 아이디
        /// </summary>
        private string m_ID = string.Empty;

        public string ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        /// <summary>
        /// 2013-01-28 SSH 연결 비밀번호
        /// </summary>
        private string m_Password = string.Empty;

        public string Password
        {
            get { return m_Password; }
            set { m_Password = value; }
        }



        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public TerminalConnectInfo() 
        {
            m_SerianConfig = new SerialConfig();
        }
        /// <summary>
        /// 2013-01-28 - shinyn - SSH 아이디, 비밀번호 추가 복사 생성자 입니다.
        /// </summary>
        /// <param name="aInfo"></param>
        public TerminalConnectInfo(TerminalConnectInfo aInfo)
        {
            m_ConnectionProtocol = aInfo.m_ConnectionProtocol;
            m_IPAddress = aInfo.m_IPAddress;
            m_SerianConfig = new SerialConfig(aInfo.m_SerianConfig);
            m_TelnetPort = aInfo.m_TelnetPort;
            m_ID = aInfo.m_ID;
            m_Password = aInfo.m_Password;
        }

        /// <summary>
        /// Telnet Port 가져오거나 설정 합니다.
        /// </summary>
        public int TelnetPort
        {
            get { return m_TelnetPort; }
            set { m_TelnetPort = value; }
        }

        /// <summary>
        /// IP Address 가져오거나 설정 합니다.
        /// </summary>
        public string IPAddress
        {
            get { return m_IPAddress; }
            set { m_IPAddress = value; }
        }


        /// <summary>
        /// Serial Config 가져오거나 설정 합니다.
        /// </summary>
        public SerialConfig SerialConfig
        {
            get { return m_SerianConfig; }
            set { m_SerianConfig = value; }
        }

        /// <summary>
        /// 연결 타입 가져오거나 설정 합니다.
        /// </summary>
        public E_ConnectionProtocol ConnectionProtocol
        {
            get { return m_ConnectionProtocol; }
            set { m_ConnectionProtocol = value; }
        }



    }
}
