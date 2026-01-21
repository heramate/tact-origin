using System;
using System.Collections.Generic;
using System.Text;

namespace RACTCommonClass
{
    /// <summary>
    /// 접속 기록 요청 정보 입니다.
    /// </summary>
    [Serializable]
    public class ConnectionHistoryRequestInfo
    {
        /// <summary>
        /// Start Time 입니다.
        /// </summary>
        private DateTime m_StartTime;

        /// <summary>
        /// End Time 입니다.
        /// </summary>
        private DateTime m_EndTime;

        /// <summary>
        /// UserID 입니다.
        /// </summary>
        private int m_UserID;

        /// <summary>
        /// UserID 가져오거나 설정 합니다.
        /// </summary>
        public int UserID
        {
            get { return m_UserID; }
            set { m_UserID = value; }
        }


        /// <summary>
        /// End Time 가져오거나 설정 합니다.
        /// </summary>
        public DateTime EndTime
        {
            get { return m_EndTime; }
            set { m_EndTime = value; }
        }


        /// <summary>
        /// Start Time 가져오거나 설정 합니다.
        /// </summary>
        public DateTime StartTime
        {
            get { return m_StartTime; }
            set { m_StartTime = value; }
        }

    }
}
