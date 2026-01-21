using System;
using System.Collections.Generic;
using System.Text;

namespace RACTCommonClass
{
    [Serializable]
    public class GroupRequestInfo
    {
        /// <summary>
        /// User ID 입니다.
        /// </summary>
        private int m_UserID;

        /// <summary>
        /// Work Type 입니다.
        /// </summary>
        private E_WorkType m_WorkType;

        /// <summary>
        /// GroupInfo 입니다.
        /// </summary>
        private GroupInfo m_GroupInfo;

        /// <summary>
        /// GroupInfo 가져오거나 설정 합니다.
        /// </summary>
        public GroupInfo GroupInfo
        {
            get { return m_GroupInfo; }
            set { m_GroupInfo = value; }
        }

        /// <summary>
        /// Work Type 가져오거나 설정 합니다.
        /// </summary>
        public E_WorkType WorkType
        {
            get { return m_WorkType; }
            set { m_WorkType = value; }
        }
        /// <summary>
        /// User ID 가져오거나 설정 합니다.
        /// </summary>
        public int UserID
        {
            get { return m_UserID; }
            set { m_UserID = value; }
        }


    }
}
