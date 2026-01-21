using System;
using System.Collections.Generic;
using System.Text;

namespace RACTCommonClass
{
    [Serializable]
    public class ScriptRequestInfo
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
        private Script m_ScriptInfo;

        /// <summary>
        /// 장비정보 속성을 가져오거나 설정합니다.
        /// </summary>
        public Script ScriptInfo
        {
            get { return m_ScriptInfo; }
            set { m_ScriptInfo = value; }
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
