using System;
using System.Collections.Generic;
using System.Text;

namespace RACTCommonClass
{
    [Serializable]
    public class UseableDaemonRequestInfo
    {
        /// <summary>
        /// Client ID 입니다.
        /// </summary>
        private int m_ClientID;

        /// <summary>
        /// 접속 하지 못한 Daemon ID 목록 입니다.
        /// </summary>
        private List<int> m_DisconnectDaemonList;

        public UseableDaemonRequestInfo(int aClientID, List<int> aDaemonList)
        {
            m_ClientID = aClientID;
            m_DisconnectDaemonList = aDaemonList;
        }

        /// <summary>
        /// 접속 하지 못한 Daemon ID 목록 가져오거나 설정 합니다.
        /// </summary>
        public List<int> DisconnectDaemonList
        {
            get { return m_DisconnectDaemonList; }
            set { m_DisconnectDaemonList = value; }
        }
        /// <summary>
        /// Client ID 가져오거나 설정 합니다.
        /// </summary>
        public int ClientID
        {
            get { return m_ClientID; }
            set { m_ClientID = value; }
        }

    }
}
