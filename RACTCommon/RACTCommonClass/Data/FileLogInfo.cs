using System;
using System.Collections.Generic;
using System.Text;

namespace RACTCommonClass
{
    public class FileLogInfo
    {
        /// <summary>
        /// Log Type 입니다.
        /// </summary>
        private E_FileLogType m_LogType = E_FileLogType.Infomation;
        /// <summary>
        /// Log 메시지 입니다.
        /// </summary>
        private string m_Message;

        /// <summary>
        /// 기본 생성자입니다.
        /// </summary>
        public FileLogInfo() { }

        /// <summary>
        /// 기본 생성자입니다.
        /// </summary>
        public FileLogInfo(E_FileLogType aLogType, string aMessage)
        {
            m_LogType = aLogType;
            m_Message = aMessage;
        }

        /// <summary>
        /// Log 메시지 가져오거나 설정 합니다.
        /// </summary>
        public string Message
        {
            get { return m_Message; }
            set { m_Message = value; }
        }


        /// <summary>
        /// Log Type 가져오거나 설정 합니다.
        /// </summary>
        public E_FileLogType LogType
        {
            get { return m_LogType; }
            set { m_LogType = value; }
        }
    }
}
