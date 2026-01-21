using System;
using System.Collections.Generic;
using System.Text;

namespace RACTCommonClass
{
    [Serializable]
    public class ShortenCommandGroupRequestInfo
    {

        /// <summary>
        /// User ID 입니다.
        /// </summary>
        private int m_UserID;
        /// <summary>
        /// 작업타입 입니다.
        /// </summary>
        private E_WorkType m_WorkType;

        /// <summary>
        /// 장비 정보 입니다,.
        /// </summary>
        private ShortenCommandGroupInfo m_ShortenCommandGroupInfo;

        /// <summary>
        /// 장비정보 속성을 가져오거나 설정합니다.
        /// </summary>
        public ShortenCommandGroupInfo ShortenCommandGroupInfo
        {
            get { return m_ShortenCommandGroupInfo; }
            set { m_ShortenCommandGroupInfo = value; }
        }

        /// <summary>
        /// 작업타입 속성을 가져오거나 설정합니다.
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
