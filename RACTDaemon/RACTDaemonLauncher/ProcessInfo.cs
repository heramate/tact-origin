using System;
using System.Collections.Generic;
using System.Text;

namespace RACTDaemonLauncher
{
    [Serializable]
    public class ProcessInfo
    {

        /// <summary>
        /// Process ID 입니다.
        /// </summary>
        private int m_ProcessID = 0;
        /// <summary>
        /// 사용자가 중지 했는지 여부 입니다.
        /// </summary>
        private bool m_AutoStart = true;
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public ProcessInfo() { }

        /// <summary>
        /// Key 입니다.
        /// </summary>
        private string m_Key;

        /// <summary>
        /// Key 가져오거나 설정 합니다.
        /// </summary>
        public string Key
        {
            get { return m_Key; }
            set { m_Key = value; }
        }

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        /// <param name="aProcessID"></param>
        public ProcessInfo(int aProcessID)
        {
            m_ProcessID = aProcessID;
        }
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        /// <param name="aProcessID"></param>
        public ProcessInfo(string aKey, int aProcessID, bool aManualStop)
        {
            m_Key = aKey;
            m_ProcessID = aProcessID;
            m_AutoStart = aManualStop;
        }
        /// <summary>
        /// 사용자가 중지 했는지 여부 가져오거나 설정 합니다.
        /// </summary>
        public bool AutoStart
        {
            get { return m_AutoStart; }
            set { m_AutoStart = value; }
        }


        /// <summary>
        /// Process ID 가져오거나 설정 합니다.
        /// </summary>
        public int ProcessID
        {
            get { return m_ProcessID; }
            set { m_ProcessID = value; }
        }

    }
}
