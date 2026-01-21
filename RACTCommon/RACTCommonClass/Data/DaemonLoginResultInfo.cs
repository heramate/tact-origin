using System;
using System.Collections.Generic;
using System.Text;

namespace RACTCommonClass
{
    [Serializable]
    public class DaemonLoginResultInfo
    {
        /// <summary>
        /// 로그인 결과 입니다.
        /// </summary>
        private E_LoginResult m_LoginResult = E_LoginResult.UnknownError;

        /// <summary>
        /// 설명 입니다.
        /// </summary>
        private string m_Description;

        /// <summary>
        /// Client ID 입니다.
        /// </summary>
        private int m_ClientID;

        /// <summary>
        /// Daemon Process Info 입니다.
        /// </summary>
        private DaemonProcessInfo m_DaemonInfo;

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public DaemonLoginResultInfo() { }
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        /// <param name="aResult"></param>
        public DaemonLoginResultInfo(E_LoginResult aResult)
        {
            m_LoginResult = aResult;
        }

        /// <summary>
        /// Daemon Process Info 가져오거나 설정 합니다.
        /// </summary>
        public DaemonProcessInfo DaemonInfo
        {
            get { return m_DaemonInfo; }
            set { m_DaemonInfo = value; }
        }

        /// <summary>
        /// Client ID 가져오거나 설정 합니다.
        /// </summary>
        public int ClientID
        {
            get { return m_ClientID; }
            set { m_ClientID = value; }
        }

        /// <summary>
        /// 설명 가져오거나 설정 합니다.
        /// </summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }

        /// <summary>
        /// 로그인 결과 가져오거나 설정 합니다.
        /// </summary>
        public E_LoginResult LoginResult
        {
            get { return m_LoginResult; }
            set { m_LoginResult = value; }
        }
    }
}
