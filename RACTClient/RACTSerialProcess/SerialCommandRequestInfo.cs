using System;
using System.Collections.Generic;
using System.Text;

namespace RACTSerialProcess
{
    public class SerialCommandRequestInfo
    {
         /// <summary>
        /// ComPort 입니다.
        /// </summary>
        private string m_ComPort;


        /// <summary>
        /// Received Data 입니다.
        /// </summary>
        private string m_RequestData;
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        /// <param name="aComPort"></param>
        /// <param name="aData"></param>
        public SerialCommandRequestInfo(string aComPort, string aData)
        {
            m_ComPort = aComPort;
            m_RequestData = aData;
        }

        /// <summary>
        /// Received Data 가져오거나 설정 합니다.
        /// </summary>
        public string RequestData
        {
            get { return m_RequestData; }
            set { m_RequestData = value; }
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
