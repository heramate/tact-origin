using System;
using System.Collections.Generic;
using System.Text;
using RACTCommonClass;

namespace RACTClient
{
    [Serializable]
    public class ConnectionHistoryInfo
    {
        /// <summary>
        /// 접속 시간 입니다.
        /// </summary>
        private DateTime m_ConncetTime;

        /// <summary>
        /// 접속 정보 입니다.
        /// </summary>
        private TerminalConnectInfo m_ConnectionInfo;
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public ConnectionHistoryInfo() { }
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        /// <param name="aInfo"></param>
        public ConnectionHistoryInfo(TerminalConnectInfo aInfo)
        {
            m_ConncetTime = DateTime.Now;
            m_ConnectionInfo = aInfo;
        }
        /// <summary>
        /// 표시 이름을 가져오기 합니다.
        /// </summary>
        public string DisplayName
        {
            get
            {
                
                if (ConnectionInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET)
                {
                    return ConnectionInfo.IPAddress;
                }
                else if (ConnectionInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
                {
                    // 2013-03-06 - shinyn - SSH텔넷인 경우 분기처리 추가
                    return ConnectionInfo.IPAddress;
                }
                else
                {
                    return ConnectionInfo.SerialConfig.PortName;
                }
            }
        }
        /// <summary>
        /// 접속 정보 가져오거나 설정 합니다.
        /// </summary>
        public TerminalConnectInfo ConnectionInfo
        {
            get { return m_ConnectionInfo; }
            set { m_ConnectionInfo = value; }
        }

        /// <summary>
        /// 접속 시간 가져오거나 설정 합니다.
        /// </summary>
        public DateTime ConncetTime
        {
            get { return m_ConncetTime; }
            set { m_ConncetTime = value; }
        }

    }
}
