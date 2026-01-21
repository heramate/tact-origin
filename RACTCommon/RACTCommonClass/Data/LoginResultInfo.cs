using System;
using System.Collections.Generic;
using System.Text;

namespace RACTCommonClass
{
    [Serializable]
    public class LoginResultInfo
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
        /// Server ID 입니다.
        /// </summary>
        private int m_ServerID;

        /// <summary>
        /// User Type 입니다.
        /// </summary>
        private E_UserType m_UserType;
        /// <summary>
        /// User ID 입니다.
        /// </summary>
        private int m_UserID;
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public LoginResultInfo() { }

        /// <summary>
        /// UserInfo 입니다.
        /// </summary>
        private UserInfo m_UserInfo;

        /// <summary>
        /// UserInfo 가져오거나 설정 합니다.
        /// </summary>
        public UserInfo UserInfo
        {
            get { return m_UserInfo; }
            set { m_UserInfo = value; }
        }

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public LoginResultInfo(E_LoginResult aResult,string aDescription) 
        {
            m_LoginResult = aResult;
            m_Description = aDescription;
        }




      

        /// <summary>
        /// User ID 가져오거나 설정 합니다.
        /// </summary>
        public int UserID
        {
            get { return m_UserID; }
            set { m_UserID = value; }
        }

        /// <summary>
        /// User Type 가져오거나 설정 합니다.
        /// </summary>
        public E_UserType UserType
        {
            get { return m_UserType; }
            set { m_UserType = value; }
        }

        /// <summary>
        /// Server ID 가져오거나 설정 합니다.
        /// </summary>
        public int ServerID
        {
            get { return m_ServerID; }
            set { m_ServerID = value; }
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
