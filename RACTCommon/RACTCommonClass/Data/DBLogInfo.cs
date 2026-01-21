using System;
using System.Collections.Generic;
using System.Text;

namespace RACTCommonClass
{
    [Serializable]
    public class DBLogInfo
    {

        /// <summary>
        /// 로그 타입 입니다. 입니다.
        /// </summary>
        protected E_DBLogType m_LogType;

        /// <summary>
        /// 메시지 입니다. 입니다.
        /// </summary>
        protected string m_Message="";

        /// <summary>
        /// 시간 입니다.
        /// </summary>
        protected DateTime m_DateTime;

       
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public DBLogInfo() 
        {
            m_DateTime = DateTime.Now;
        }

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public DBLogInfo(E_DBLogType aLogType, string aMessage)
        {
            m_LogType = aLogType;
            m_Message = aMessage;
            m_DateTime = DateTime.Now;
        }

        /// <summary>
        /// 메시지 입니다. 가져오거나 설정 합니다.
        /// </summary>
        public string Message
        {
            get { return m_Message; }
            set { m_Message = value; }
        }


        /// <summary>
        /// 로그 타입 입니다. 가져오거나 설정 합니다.
        /// </summary>
        public E_DBLogType LogType
        {
            get { return m_LogType; }
            set { m_LogType = value; }
        }
        /// <summary>
        /// 시간 가져오기 합니다.
        /// </summary>
        public DateTime DateTime
        {
            get { return m_DateTime; }
        }


    }
    /// <summary>
    /// 명령 실행 로그 정보 입니다.
    /// </summary>
    [Serializable]
    public class DBExecuteCommandLogInfo : DBLogInfo
    {
        /// <summary>
        /// Connection Log ID 입니다.
        /// </summary>
        private int m_ConnectionLogID;
        /// <summary>
        /// Command 입니다.
        /// </summary>
        private string m_Command;
        /// <summary>
        /// Device Info 입니다.
        /// </summary>
        private DeviceInfo m_DeviceInfo;

        /// <summary>
        /// Gunny 2015고도화
        /// 제한명령어 T / F 입니다.
        /// </summary>
        private bool m_IsLimitCmd;

        /// <summary>
        /// Device Info 가져오거나 설정 합니다.
        /// </summary>
        public bool IsLimitCmd
        {
            get { return m_IsLimitCmd; }
            set { m_IsLimitCmd = value; }
        }

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public DBExecuteCommandLogInfo():base(E_DBLogType.ExecuteCommandLog,"") {}
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        /// <param name="aConnectionLogID"></param>
        /// <param name="aCommand"></param>
        public DBExecuteCommandLogInfo(int aConnectionLogID, string aCommand) : base(E_DBLogType.ExecuteCommandLog, "") 
        {
            m_ConnectionLogID = aConnectionLogID;
            m_Command = aCommand;
        }



        /// <summary>
        /// Device Info 가져오거나 설정 합니다.
        /// </summary>
        public DeviceInfo DeviceInfo
        {
            get { return m_DeviceInfo; }
            set { m_DeviceInfo = value; }
        }

        /// <summary>
        /// Command 가져오거나 설정 합니다.
        /// </summary>
        public string Command
        {
            get { return m_Command; }
            set { m_Command = value; }
        }
        /// <summary>
        /// Connection Log ID 가져오거나 설정 합니다.
        /// </summary>
        public int ConnectionLogID
        {
            get { return m_ConnectionLogID; }
            set { m_ConnectionLogID = value; }
        }

    }
    /// <summary>
    /// 사용자 로그인/로그 아웃 로그 정보 입니다.
    /// </summary>
    [Serializable]
    public class DBUserLogInfo : DBLogInfo
    {
        /// <summary>
        /// 사용자 ID 입니다.
        /// </summary>
        private int m_UserID;
        /// <summary>
        /// 로그 타입 입니다.
        /// </summary>
        private E_UserLogType m_UserLogType;
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public DBUserLogInfo() : base(E_DBLogType.LoginLog, "") { }
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        /// <param name="aUserID">사용자 ID 입니다.</param>
        /// <param name="aUserLogType">로그인/ 로그아웃 타입 입니다.</param>
        public DBUserLogInfo(int aUserID, E_UserLogType aUserLogType) : base(E_DBLogType.LoginLog, "") 
        {
            m_UserID = aUserID;
            m_UserLogType = aUserLogType;
        }
        
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        /// <param name="aUserID">사용자 ID 입니다.</param>
        /// <param name="aUserLogType">로그인/ 로그아웃 타입 입니다.</param>
        public DBUserLogInfo(int aUserID, E_UserLogType aUserLogType, string aMessage) : base(E_DBLogType.LoginLog, aMessage) 
        {
            m_UserID = aUserID;
            m_UserLogType = aUserLogType;
        }


        /// <summary>
        /// 로그 타입 가져오거나 설정 합니다.
        /// </summary>
        public E_UserLogType UserLogType
        {
            get { return m_UserLogType; }
            set { m_UserLogType = value; }
        }

        /// <summary>
        /// 사용자 ID 가져오거나 설정 합니다.
        /// </summary>
        public int UserID
        {
            get { return m_UserID; }
            set { m_UserID = value; }
        }
    }

    /// <summary>
    /// 로그 타입 입니다.
    /// </summary>
    [Serializable]
    public enum E_UserLogType
    {
        /// <summary>
        /// 로그인 입니다.
        /// </summary>
        Login = 0,
        /// <summary>
        /// 로그 아웃 입니다.
        /// </summary>
        LogOut =1
    }

    /// <summary>
    /// 장비 연결 로그 타입 입니다.
    /// </summary>
    [Serializable]
    public enum E_DeviceConnectType
    {
        /// <summary>
        /// 접속 입니다.
        /// </summary>
        Connection = 0,
        /// <summary>
        /// 접속 끊김 입니다.
        /// </summary>
        DisConnection =1
    }

    /// <summary>
    /// 2013-01-18 -shinyn - 장비 타입 0:NE등록된 장비 1:사용자NE등록된 장비
    /// </summary>
    [Serializable]
    public enum E_DeviceType
    {
        /// <summary>
        /// NE_NE등록된 장비
        /// </summary>
        NeGroup = 1,
        /// <summary>
        /// RACT_USR_NE_NE 등록된 장비
        /// </summary>
        UserNeGroup = 2,
    }
}
