using System;

namespace TACTRestartService
{
    public class SystemConfig
    {
        /// <summary>
        /// 서비스다운 체크 간격(기본값=15초)
        /// </summary>
        private int m_CheckIntervalSeconds = 15;
        public int CheckIntervalSeconds
        {
            get { return m_CheckIntervalSeconds; }
            set { m_CheckIntervalSeconds = value; }
        }
    }
}
