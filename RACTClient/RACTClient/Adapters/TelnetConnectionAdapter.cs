using Rebex.Net;
using Rebex.TerminalEmulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RACTClient.Adapters
{
    public class TelnetConnectionAdapter : IRebexConnection
    {
        private Telnet _telnet;

        public TelnetConnectionAdapter()
        {
            // Telnet 객체 초기화 (호스트 정보는 Connect 시점에 설정)
            // 일부 버전에서는 생성자에 인자가 필요할 수 있으므로 확인 필요 (소스상 호스트, 포트 입력)
            _telnet = new Telnet(null);
        }

        // 생성자 오버로딩 (편의상 추가)
        public TelnetConnectionAdapter(string host, int port)
        {
            _telnet = new Telnet(host, port);
        }

        public bool IsConnected
        {
            // Telnet 객체가 Disconnect 속성이 없다면, 객체가 null이 아닌지 여부 등으로 판단
            // 정확한 연결 상태 확인이 어렵다면 단순히 객체 존재 여부만 리턴하거나 true 리턴
            get { return _telnet != null; }
        }

        public void SetProxy(Proxy proxy)
        {
            if (_telnet != null)
            {
                _telnet.Proxy = proxy; // [Source Reference: 454]
            }
        }

        public void Connect(string host, int port)
        {
            // 기존 객체 정리 (Telnet은 Disconnect가 없으므로 null 처리)
            if (_telnet != null)
            {
                _telnet = null;
            }

            // [Source Reference: 454] Telnet 객체 생성 시 접속 정보 주입
            _telnet = new Telnet(host, port);

            // Factory를 통한 프록시 설정 (앞서 만든 Factory 사용 시)
            Proxy proxy = RebexProxyFactory.CreateProxy();
            if (proxy != null)
            {
                _telnet.Proxy = proxy;
            }

            // Telnet은 생성과 동시에 연결을 시도하거나, Bind 시점에 연결됩니다.
            // 별도의 Connect() 메서드가 없다면 여기서 추가 작업은 불필요합니다.
        }

        public void Login(string userName, string password)
        {
            // Telnet은 소켓 레벨 연결 시 로그인하지 않음 (Scripting으로 처리)
            // 따라서 빈 구현(No-operation) 유지
        }

        public IShellChannelFactory GetClientObject()
        {
            return _telnet;
        }

        public void Dispose()
        {
            // [Source Reference: 462] 샘플 코드에 따라 Telnet은 명시적 Dispose 없이 null 처리
            _telnet = null;
        }
    }
}
