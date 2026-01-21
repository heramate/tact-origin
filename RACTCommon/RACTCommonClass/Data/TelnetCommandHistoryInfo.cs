using System;
using System.Collections.Generic;
using System.Text;

namespace RACTCommonClass
{
    [Serializable]
    public class TelnetCommandHistoryInfo
    {

        /// <summary>
        /// Time 입니다.
        /// </summary>
        private DateTime m_Time;

        /// <summary>
        /// Command 입니다.
        /// </summary>
        private string m_Command;

        /// <summary>
        /// Command 가져오거나 설정 합니다.
        /// </summary>
        public string Command
        {
            get { return m_Command; }
            set { m_Command = value; }
        }

        /// <summary>
        /// Time 가져오거나 설정 합니다.
        /// </summary>
        public DateTime Time
        {
            get { return m_Time; }
            set { m_Time = value; }
        }

    }
    [Serializable]
    public class TelnetCommandHistoryInfoCollection : List<TelnetCommandHistoryInfo>
    {
    }
}
