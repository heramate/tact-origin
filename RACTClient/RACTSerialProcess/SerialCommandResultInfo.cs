using System;
using System.Collections.Generic;
using System.Text;

namespace RACTSerialProcess
{
    public class SerialCommandResultInfo
    {

        /// <summary>
        /// ComPort 입니다.
        /// </summary>
        private string m_ComPort;
        /// <summary>
        /// Received Data 입니다.
        /// </summary>
        private string m_ReceivedData;

        /// <summary>
        /// Session ID 입니다.
        /// </summary>
        private int m_SessionID;

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        /// <param name="aComPort"></param>
        /// <param name="aData"></param>
        public SerialCommandResultInfo(string aComPort, string aData,int aSessionID)
        {
            m_ComPort = aComPort;
            m_ReceivedData = aData;
            m_SessionID = aSessionID;
        }

        /// <summary>
        /// Received Data 가져오거나 설정 합니다.
        /// </summary>
        public string ReceivedData
        {
            get { return m_ReceivedData; }
            set { m_ReceivedData = value; }
        }

        /// <summary>
        /// Session ID 가져오거나 설정 합니다.
        /// </summary>
        public int SessionID
        {
            get { return m_SessionID; }
            set { m_SessionID = value; }
        }

        /// <summary>
        /// ComPort 가져오거나 설정 합니다.
        /// </summary>
        public string ComPort
        {
            get { return m_ComPort; }
            set { m_ComPort = value; }
        }

    }
}
