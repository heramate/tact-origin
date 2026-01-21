using System;
using System.Collections.Generic;
using System.Text;

namespace RACTCommonClass
{
    [Serializable]
    public class TelnetCommandHistoryRequestInfo
    {

        /// <summary>
        /// Connection Log ID 입니다.
        /// </summary>
        private int m_ConnectionLogID;

        /// <summary>
        /// Connection Log ID 가져오거나 설정 합니다.
        /// </summary>
        public int ConnectionLogID
        {
            get { return m_ConnectionLogID; }
            set { m_ConnectionLogID = value; }
        }

    }
}
