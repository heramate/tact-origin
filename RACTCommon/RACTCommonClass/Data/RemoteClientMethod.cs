using System;
using System.Collections.Generic;
using System.Text;
using MKLibrary.MKNetwork;

namespace RACTCommonClass
{
    /// <summary>
    /// 로그인에 사용할 핸들러 입니다.
    /// </summary>
    /// <param name="aUserID">id 입니다.</param>
    /// <param name="aUserPW">암호 입니다.</param>
    /// <param name="aIPAddress">IP Address 입니다.</param>
    /// <returns></returns>
    public delegate byte[] UserLoginHandler(string aUserID, string aUserPW, string aIPAddress,E_TerminalMode aMode );
    /// <summary>
    /// 로그아웃에 사용할 핸들러 입니다.
    /// </summary>
    /// <param name="aClientID"></param>
    public delegate void UserLogOutHandler(int aClientID);
    /// <summary>
    /// 데몬 접속에 사용할 핸들러 입니다.
    /// </summary>
    /// <param name="aUserInfo"></param>
    /// <returns></returns>
    public delegate byte[] UserConnectDaemonHandler(byte[] aUserInfo);
    /// <summary>
    /// 서버에 결과를 요청 합니다.
    /// </summary>
    /// <param name="aClientID">Client ID 입니다.</param>
    /// <returns></returns>
    public delegate byte[] ResultHandler(int aClientID);
    /// <summary>
    /// 서버에 Daemon 결과를 요청 합니다.
    /// </summary>
    /// <param name="aClientID">Daemon ID 입니다.</param>
    /// <returns></returns>
    public delegate byte[] DaemonResultHandler(int aDaemonID);
    /// <summary>
    /// 서버에 명령을 요청 합니다.
    /// </summary>
    /// <param name="aRequestData"></param>
    public delegate void RequestHandler(byte[] aRequestData);
    /// <summary>
    /// 데몬이 서버에 접속할때 사용할 핸들러 입니다.
    /// </summary>
    /// <param name="aClientID">Daemon Key 입니다.</param>
    /// <returns></returns>
    public delegate byte[] DaemonConnectHandler(string aIP, int aPort,string aChannelName);
    /// <summary>
    /// 텔넷에 접속하기 위해 정보를 요청 합니다.
    /// </summary>
    /// <param name="aClientID">클라이언트 ID 입니다.</param>
    /// <returns></returns>
    public delegate byte[] TelnetConnectionRequestHandler(byte[] aUseableDaemonRequestInfo);
    /// <summary>
    /// 텔넷 세션 ID 요청에 사용할 핸들러 입니다.
    /// </summary>
    /// <param name="aQuery"></param>
    /// <returns></returns>
    public delegate int TelnetSessionIDRequestHandler(string aQuery);
    /// <summary>
    /// 텔넷 접속 기록을 Update 합니다.(종료 처리)
    /// </summary>
    /// <param name="aSessionID"></param>
    /// <returns></returns>
    public delegate bool TelnetConnectionUpdateRequestHandler(int aSessionID);
    /// <summary>
    /// 데몬 상태를 Update 합니다.
    /// </summary>
    /// <param name="aRequestInfo"></param>
    public delegate void DaemonStatusUpdateRequestHandler(byte[] aRequestInfo);
    /// <summary>
    /// Health Check 요청 핸들러 입니다.
    /// </summary>
    /// <param name="aClientID"></param>
    public delegate void HealthCheckRequestHandler(int aClientID);
    /// <summary>
    /// Daemon 목록 요청 핸들러 입니다.
    /// </summary>
    /// <returns></returns>
    public delegate byte[] DaemonListRequestHandler();
    /// <summary>
    /// Daemon에 Telnet 종료 요청 핸들러 입니다.
    /// </summary>
    /// <param name="aSessionID"></param>
    public delegate void DisconnectDaemonTelnetSessionRequestHandler(int aClientID,int aSessionID);



    /// <summary>
    /// 클라이언트용 원격 메소드가 정의된 클래스입니다.
    /// </summary>
    public class RemoteClientMethod : MKRemoteObject
    {
        /// <summary>
        /// Health Check 핸들러 입니다.
        /// </summary>
        private HealthCheckRequestHandler m_HealthCheckRequestHandler = null;
        /// <summary>
        /// 데몬 상태를 update 합니다.
        /// </summary>
        private DaemonStatusUpdateRequestHandler m_DaemonStatusUpdateRequestHandler;
        /// <summary>
        /// 로그아웃 핸들러 입니다.
        /// </summary>
        private UserLogOutHandler m_UserLogOutHandler;
        /// <summary>
        /// 텔넷 세션 ID 요청에 사용할 핸들러 입니다.
        /// </summary>
        private TelnetSessionIDRequestHandler m_TelnetSessionIDRequestHandler = null;
        /// <summary>
        /// 텔넷 접속 핸들러 입니다.
        /// </summary>
        private TelnetConnectionRequestHandler m_TelnetConnectionRequestHandler = null;
        /// <summary>
        /// 로그인 핸들러 입니다.
        /// </summary>
        private UserLoginHandler m_UserLoginHandler = null;
        /// <summary>
        /// 사용자가 데몬에 접속할 핸들러 입니다.
        /// </summary>
        private UserConnectDaemonHandler m_UserConnectDaemonHandler = null;
        /// <summary>
        /// 결과 처리 핸들러 입니다.
        /// </summary>
        private ResultHandler m_ResultHandler = null;
        /// <summary>
        /// 데몬 결과 처리 핸들러 입니다.
        /// </summary>
        private DaemonResultHandler m_DaemonResultHandler = null;
        /// <summary>
        /// 요청 처리 핸들러 입니다.
        /// </summary>
        private RequestHandler m_RequestHandler = null;
        /// <summary>
        /// 데몬이 로그인 할때 사용할 핸들러 입니다.
        /// </summary>
        private DaemonConnectHandler m_DaemonConnectHandler = null;
        /// <summary>
        /// 텔넷 접속 기록을 Update 합니다.(종료 처리)
        /// </summary>
        private TelnetConnectionUpdateRequestHandler m_TelnetConnectionUpdateRequestHandler = null;
        /// <summary>
        /// Daemon 목록을 가져옵니다.
        /// </summary>
        private DaemonListRequestHandler m_DaemonListRequestHandler = null;
        /// <summary>
        /// 데몬 텔넷 종료에 사용할 핸들러 입니다.
        /// </summary>
        private DisconnectDaemonTelnetSessionRequestHandler m_DisconnectDaemonTelnetSessionRequestHandler = null;

        


        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public RemoteClientMethod() { }




        /// <summary>
        /// 텔넷 종료 핸드러를 등록 합니다.
        /// </summary>
        /// <param name="aDaemonListRequestHandler"></param>
        public void SetDDisconnectDaemonTelnetSessionRequestHandler(DisconnectDaemonTelnetSessionRequestHandler aDaemonListRequestHandler)
        {
            m_DisconnectDaemonTelnetSessionRequestHandler = aDaemonListRequestHandler;
        }
        /// <summary>
        /// 텔넷 종료 메소드를 호출 합니다.
        /// </summary>
        public void CallDisconnectDaemonTelnetSessionRequestMethod(int aClientID, int aSessionID)
        {
            if (m_DisconnectDaemonTelnetSessionRequestHandler != null)
            {
                 m_DisconnectDaemonTelnetSessionRequestHandler(aClientID,aSessionID);
            }
            
        }


        /// <summary>
        /// Daemon 목록 핸들러를 등록 합니다.
        /// </summary>
        /// <param name="aUserLoginHandler"></param>
        public void SetDaemonListRequestHandler(DaemonListRequestHandler aDaemonListRequestHandler)
        {
            m_DaemonListRequestHandler = aDaemonListRequestHandler;
        }
        /// <summary>
        /// Daemon 목록 핸들러 메소드를 호출 합니다.
        /// </summary>
        public byte[] CallDaemonListRequestMethod()
        {
            if (m_DaemonListRequestHandler != null)
            {
                return m_DaemonListRequestHandler();
            }
            return null;
        }

        /// <summary>
        /// Health Check 핸들러를 등록 합니다.
        /// </summary>
        /// <param name="aUserLoginHandler"></param>
        public void SetHealthCheckRequestHandler(HealthCheckRequestHandler aHealthCheckRequestHandler)
        {
            m_HealthCheckRequestHandler = aHealthCheckRequestHandler;
        }
        /// <summary>
        /// Health Check 핸들러 메소드를 호출 합니다.
        /// </summary>
        public void CallHealthCheckRequestMethod(int aClientID)
        {
            if (m_HealthCheckRequestHandler != null)
            {
                m_HealthCheckRequestHandler(aClientID);
            }
        }

        /// <summary>
        /// 텔넷 접속 기록을 Update 합니다.(종료 처리)
        /// </summary>
        /// <param name="aUserLoginHandler"></param>
        public void SetDaemonStatusUpdateRequestHandler(DaemonStatusUpdateRequestHandler aDaemonStatusUpdateRequestHandler)
        {
            m_DaemonStatusUpdateRequestHandler = aDaemonStatusUpdateRequestHandler;
        }
        /// <summary>
        /// 텔넷 접속 기록을 Update 메소드를 호출 합니다.(종료 처리)
        /// </summary>
        public void CallDaemonStatusUpdateRequestMethod(byte[] aDaemonInfo)
        {
            if (m_DaemonStatusUpdateRequestHandler != null)
            {
                 m_DaemonStatusUpdateRequestHandler(aDaemonInfo);
            }
        }

        /// <summary>
        /// 텔넷 접속 기록을 Update 합니다.(종료 처리)
        /// </summary>
        /// <param name="aUserLoginHandler"></param>
        public void SetTelnetConnectionUpdateRequestHandler(TelnetConnectionUpdateRequestHandler aTelnetConnectionUpdateRequestHandler)
        {
            m_TelnetConnectionUpdateRequestHandler = aTelnetConnectionUpdateRequestHandler;
        }
        /// <summary>
        /// 텔넷 접속 기록을 Update 메소드를 호출 합니다.(종료 처리)
        /// </summary>
        public bool CallTelnetConnectionUpdateRequestMethod(int aSessionID)
        {
            if (m_TelnetConnectionUpdateRequestHandler != null)
            {
                return m_TelnetConnectionUpdateRequestHandler(aSessionID);
            }
            return false;
        }

        /// <summary>
        /// 텔넷 접속 요청에 사용할 핸들러를 등록 합니다.
        /// </summary>
        /// <param name="aUserLoginHandler"></param>
        public void SetUserLogOutHandler(UserLogOutHandler aUserLogOutHandler)
        {
            m_UserLogOutHandler = aUserLogOutHandler;
        }
        /// <summary>
        /// 텔넷 접속 요청에 사용할 메소드를 호출 합니다.
        /// </summary>
        public void CallUserLogOutMethod(int aClientID)
        {
            if (m_UserLogOutHandler != null)
            {
                m_UserLogOutHandler(aClientID);
            }
        }

        /// <summary>
        /// 텔넷 접속 요청에 사용할 핸들러를 등록 합니다.
        /// </summary>
        /// <param name="aUserLoginHandler"></param>
        public void SetTelnetSessionIDRequestHandler(TelnetSessionIDRequestHandler aTelnetSessionIDRequestHandler)
        {
            m_TelnetSessionIDRequestHandler = aTelnetSessionIDRequestHandler;
        }
        /// <summary>
        /// 텔넷 접속 요청에 사용할 메소드를 호출 합니다.
        /// </summary>
        public int CallTelnetSessionIDRequestHandler(string aQuery)
        {
            if (m_TelnetSessionIDRequestHandler != null)
            {
                return m_TelnetSessionIDRequestHandler(aQuery);
            }
            return -1;
        }

        /// <summary>
        /// 텔넷 접속 요청에 사용할 핸들러를 등록 합니다.
        /// </summary>
        /// <param name="aUserLoginHandler"></param>
        public void SetTelnetConnectionRequestHandler(TelnetConnectionRequestHandler aTelnetConnectionRequestHandler)
        {
            m_TelnetConnectionRequestHandler = aTelnetConnectionRequestHandler;
        }
        /// <summary>
        /// 텔넷 접속 요청에 사용할 메소드를 호출 합니다.
        /// </summary>
        public byte[] CallTelnetConnectionRequestHandler(byte[] aUseableDaemonRequestInfo)
        {
            if (m_TelnetConnectionRequestHandler != null)
            {
                return m_TelnetConnectionRequestHandler(aUseableDaemonRequestInfo);
            }

            return null;
        }

        /// <summary>
        /// 데몬이 로그인 할때 사용할 핸들러를 등록 합니다.
        /// </summary>
        /// <param name="aUserLoginHandler"></param>
        public void SetDaemonConnectHandler(DaemonConnectHandler aDaemonConnectHandler)
        {
            m_DaemonConnectHandler = aDaemonConnectHandler;
        }
        /// <summary>
        /// 데몬이 로그인 할때 사용할 메소드를 호출 합니다.
        /// </summary>
        public byte[] CallDaemonConnectHandler(string aIP,int aPort,string aChannelName)
        {
            if (m_DaemonConnectHandler != null)
            {
                return m_DaemonConnectHandler(aIP, aPort, aChannelName);
            }

            return null;
        }
        /// <summary>
        /// 사용자가 데몬에 로그인할 핸들러를 등록 합니다.
        /// </summary>
        /// <param name="aUserLoginHandler"></param>
        public void SetUserConnectDaemonHandler(UserConnectDaemonHandler aUserLoginHandler)
        {
            m_UserConnectDaemonHandler = aUserLoginHandler;
        }
        /// <summary>
        /// 사용자가 데몬에 로그인할 메소드를 호출 합니다.
        /// </summary>
        public byte[] CallUserConnectDaemonHandler(byte[] aUserInfo)
        {
            if (m_UserConnectDaemonHandler != null)
            {
                return m_UserConnectDaemonHandler(aUserInfo);
            }

            return null;
        }

        /// <summary>
        /// 로그인 핸들러를 등록 합니다.
        /// </summary>
        /// <param name="aUserLoginHandler"></param>
        public void SetUserLoginHandler(UserLoginHandler aUserLoginHandler)
        {
            m_UserLoginHandler = aUserLoginHandler;
        }
        /// <summary>
        /// 로그인 메소드를 호출 합니다.
        /// </summary>
        public byte[] CallUserLoginMethod(string aUserID, string aUserPW, string aIPAddress,E_TerminalMode aMode)
        {
            if (m_UserLoginHandler != null)
            {
                return m_UserLoginHandler(aUserID, aUserPW, aIPAddress, aMode);
            }

            return null;
        }
        /// <summary>
        /// Daemon 결과 요청 핸들러를 등록 합니다.
        /// </summary>
        public void SetDaemonResultHandler(DaemonResultHandler aResultHandler)
        {
            m_DaemonResultHandler = aResultHandler;
        }

        /// <summary>
        /// 결과 요청 메소드를 호출 합니다.
        /// </summary>
        /// <param name="aClientID">Client ID 입니다.</param>
        public byte[] CallDaemonResultMethod(int aDaemonID)
        {
            if (m_DaemonResultHandler != null)
            {
                return m_DaemonResultHandler(aDaemonID);
            }
            return null;
        }
        /// <summary>
        /// 결과 요청 핸들러를 등록 합니다.
        /// </summary>
        public void SetResultHandler(ResultHandler aResultHandler)
        {
            m_ResultHandler = aResultHandler;
        }

        /// <summary>
        /// 결과 요청 메소드를 호출 합니다.
        /// </summary>
        /// <param name="aClientID">Client ID 입니다.</param>
        public byte[] CallResultMethod(int aClientID)
        {
            if (m_ResultHandler != null)
            {
                return m_ResultHandler(aClientID);
            }
            return null;
        }
        /// <summary>
        /// 요청 처리 메소드를 등록합니다.
        /// </summary>
        /// <param name="aRequestHandler"></param>
        public void SetRequestHandler(RequestHandler aRequestHandler)
        {
            m_RequestHandler = aRequestHandler;
        }
        /// <summary>
        /// 요청 처리 메소드를 호출 합니다.
        /// </summary>
        /// <param name="aRequestData"></param>
        public void CallRequestMethod(byte[] aRequestData)
        {
            if (m_RequestHandler != null)
            {
                m_RequestHandler(aRequestData);
            }
        }
    }
}
