using System;
using System.Collections.Generic;
using System.Text;
using MKLibrary.MKNetwork;

namespace RACTCommonClass
{
    public delegate byte[] UserLoginHandler(string aUserID, string aUserPW, string aIPAddress, E_TerminalMode aMode);
    public delegate void UserLogOutHandler(int aClientID);
    public delegate void RequestHandler(byte[] aRequestData);
    public delegate byte[] ResultHandler(int aClientID);

    public delegate byte[] DaemonResultHandler(int aDaemonID);
    public delegate byte[] DaemonConnectHandler(string aIP, int aPort, string aChannelName);
    public delegate int TelnetSessionIDRequestHandler(string aQuery);
    public delegate bool TelnetConnectionUpdateRequestHandler(int aSessionID);
    public delegate void DaemonStatusUpdateRequestHandler(byte[] aInfo);
    public delegate byte[] DaemonListRequestHandler();
    public delegate void HealthCheckRequestHandler(int aClientID);

    // 데몬 프로젝트 전용 핸들러들
    public delegate byte[] DaemonUserConnectHandler(byte[] aUserInfo);
    public delegate void DaemonDisconnectTelnetSessionHandler(int aClientID, int aSessionID);

    public class RemoteClientMethod : MKRemoteObject
    {
        private UserLoginHandler m_UserLoginHandler;
        private UserLogOutHandler m_UserLogOutHandler;
        private RequestHandler m_RequestHandler;
        private ResultHandler m_ResultHandler;

        private DaemonResultHandler m_DaemonResultHandler;
        private DaemonConnectHandler m_DaemonConnectHandler;
        private TelnetSessionIDRequestHandler m_TelnetSessionIDRequestHandler;
        private TelnetConnectionUpdateRequestHandler m_TelnetConnectionUpdateRequestHandler;
        private DaemonStatusUpdateRequestHandler m_DaemonStatusUpdateRequestHandler;
        private DaemonListRequestHandler m_DaemonListRequestHandler;
        private HealthCheckRequestHandler m_HealthCheckRequestHandler;

        private DaemonUserConnectHandler m_DaemonUserConnectHandler;
        private DaemonDisconnectTelnetSessionHandler m_DaemonDisconnectTelnetSessionHandler;

        public void SetUserLoginHandler(UserLoginHandler a) => m_UserLoginHandler = a;
        public void SetUserLogOutHandler(UserLogOutHandler a) => m_UserLogOutHandler = a;
        public void SetRequestHandler(RequestHandler a) => m_RequestHandler = a;
        public void SetResultHandler(ResultHandler a) => m_ResultHandler = a;
        public void SetDaemonResultHandler(DaemonResultHandler a) => m_DaemonResultHandler = a;
        public void SetDaemonConnectHandler(DaemonConnectHandler a) => m_DaemonConnectHandler = a;
        public void SetTelnetSessionIDRequestHandler(TelnetSessionIDRequestHandler a) => m_TelnetSessionIDRequestHandler = a;
        public void SetTelnetConnectionUpdateRequestHandler(TelnetConnectionUpdateRequestHandler a) => m_TelnetConnectionUpdateRequestHandler = a;
        public void SetDaemonStatusUpdateRequestHandler(DaemonStatusUpdateRequestHandler a) => m_DaemonStatusUpdateRequestHandler = a;
        public void SetDaemonListRequestHandler(DaemonListRequestHandler a) => m_DaemonListRequestHandler = a;
        public void SetHealthCheckRequestHandler(HealthCheckRequestHandler a) => m_HealthCheckRequestHandler = a;

        // 데몬 프로젝트 호환 오버로드
        public void SetUserConnectDaemonHandler(DaemonUserConnectHandler a) => m_DaemonUserConnectHandler = a;
        public void SetDDisconnectDaemonTelnetSessionRequestHandler(DaemonDisconnectTelnetSessionHandler a) => m_DaemonDisconnectTelnetSessionHandler = a;

        public byte[] CallUserLoginMethod(string id, string pw, string ip, E_TerminalMode mode) => m_UserLoginHandler?.Invoke(id, pw, ip, mode);
        public void CallUserLogOutMethod(int id) => m_UserLogOutHandler?.Invoke(id);
        public void CallRequestMethod(byte[] data) => m_RequestHandler?.Invoke(data);
        public byte[] CallResultRequestMethod(int id) => m_ResultHandler?.Invoke(id);
        public byte[] CallResultMethod(int id) => m_ResultHandler?.Invoke(id);

        public byte[] CallDaemonResultRequestMethod(int id) => m_DaemonResultHandler?.Invoke(id);
        public byte[] CallDaemonResultMethod(int id) => m_DaemonResultHandler?.Invoke(id);

        public byte[] CallDaemonConnectMethod(string ip, int port, string name) => m_DaemonConnectHandler?.Invoke(ip, port, name);
        public byte[] CallDaemonConnectHandler(string ip, int port, string name) => m_DaemonConnectHandler?.Invoke(ip, port, name);
        public byte[] CallUserConnectDaemonMethod(byte[] data) => m_DaemonUserConnectHandler?.Invoke(data);

        public int CallTelnetSessionIDRequestHandler(string query) => m_TelnetSessionIDRequestHandler?.Invoke(query) ?? -1;
        public bool CallTelnetConnectionUpdateRequestMethod(int id) => m_TelnetConnectionUpdateRequestHandler?.Invoke(id) ?? false;
        public void CallDaemonStatusUpdateRequestMethod(byte[] info) => m_DaemonStatusUpdateRequestHandler?.Invoke(info);
        public byte[] CallDaemonListRequestMethod() => m_DaemonListRequestHandler?.Invoke();
        public void CallHealthCheckRequestMethod(int id) => m_HealthCheckRequestHandler?.Invoke(id);
        public void CallDisconnectDaemonTelnetSessionRequestMethod(int clientID, int sessionID) => m_DaemonDisconnectTelnetSessionHandler?.Invoke(clientID, sessionID);
    }
}
