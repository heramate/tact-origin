using Rebex.Net;
using Rebex.TerminalEmulation;
using System;

namespace RACTClient.Adapters
{
    public class TelnetConnectionAdapter : IRebexConnection
    {
        private Telnet _telnet;

        public TelnetConnectionAdapter()
        {
        }

        public bool IsConnected => _telnet != null;

        public void SetProxy(Proxy proxy)
        {
            if (_telnet != null) _telnet.Proxy = proxy;
        }

        public void Connect(string host, int port)
        {
            _telnet = new Telnet(host, port);
        }

        // 프록시를 적용하는 메서드 (연결은 이후 Bind 시점에 자동 수행됨)
        public void ConnectWithProxy(string host, int port, Proxy proxy)
        {
            // 1. 객체 생성 (연결 설정만 준비)
            _telnet = new Telnet(host, port);

            // 2. SSH 점프 서버가 열어준 SOCKS5 프록시 주입
            if (proxy != null)
            {
                _telnet.Proxy = proxy;
            }
        }

        public void Login(string userName, string password) { }

        public IShellChannelFactory GetClientObject()
        {
            return _telnet;
        }

        public void Dispose()
        {
            // Dispose 메서드가 없으므로 참조만 해제하여 GC(가비지 컬렉터)에 맡김
            _telnet = null;
        }
    }
}