using RACTCommonClass;
using Rebex.Net;
using Rebex.TerminalEmulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RACTClient.Adapters
{
    /// <summary>
    /// Rebex Ssh 객체를 관리하고 IRebexConnection 인터페이스를 구현하는 어댑터 클래스입니다.
    /// </summary>
    public class SshConnectionAdapter : IRebexConnection
    {
        /// <summary>
        /// 실제 통신을 담당하는 Rebex SSH 객체
        /// </summary>
        private Ssh _ssh;

        /// <summary>
        /// 생성자: Ssh 객체를 초기화합니다.
        /// </summary>
        public SshConnectionAdapter()
        {
            _ssh = new Ssh();

            // [Source Reference: TerminalClientWinForm_MainForm.cs Line 434]
            // 필요한 경우 SSH 설정(Timeout, Encryption Algorithm 등)을 여기서 초기화할 수 있습니다.
            _ssh.Timeout = AppGlobal.s_RequestTimeOut * 2; // 예: 기존 타임아웃의 2배 적용
        }

        /// <summary>
        /// 연결 여부를 확인합니다.
        /// </summary>
        public bool IsConnected
        {
            get { return _ssh != null && _ssh.IsConnected; }
        }

        /// <summary>
        /// 외부에서 생성된 프록시 객체를 설정합니다.
        /// (RebexProxyFactory를 사용하지 않고 직접 설정할 경우 사용)
        /// </summary>
        public void SetProxy(Proxy proxy)
        {
            if (_ssh != null)
            {
                _ssh.Proxy = proxy;
            }
        }

        /// <summary>
        /// 서버에 소켓 연결을 수행합니다.
        /// 이 단계에서는 프록시 설정을 자동으로 로드하고 연결만 수립하며, 인증은 하지 않습니다.
        /// </summary>
        /// <param name="host">대상 호스트 IP 또는 도메인</param>
        /// <param name="port">SSH 포트</param>
        public void Connect(string host, int port)
        {
            if (_ssh == null) _ssh = new Ssh();
            _ssh.Connect(host, port);
        }

        /// <summary>
        /// 연결된 세션에 대해 인증(Login)을 수행합니다.
        /// </summary>
        /// <param name="userName">사용자 계정</param>
        /// <param name="password">비밀번호</param>
        public void Login(string userName, string password)
        {
            if (_ssh == null || !_ssh.IsConnected)
            {
                throw new InvalidOperationException("SSH session is not connected. Call Connect() first.");
            }

            // [Source Reference: TerminalClientWinForm_MainForm.cs Line 454]
            // 키 파일 인증이 필요한 경우 AppGlobal.s_ClientOption 등을 참조하여 
            // SshPrivateKey 로직을 추가할 수 있습니다. 현재는 ID/PW 방식을 기본으로 구현합니다.
            _ssh.Login(userName, password);
        }

        /// <summary>
        /// TerminalControl.Bind() 메서드에 전달할 실제 클라이언트 객체를 반환합니다.
        /// </summary>
        public IShellChannelFactory GetClientObject()
        {
            return _ssh;
        }

        /// <summary>
        /// 리소스를 해제하고 연결을 종료합니다.
        /// </summary>
        public void Dispose()
        {
            if (_ssh != null)
            {
                try
                {
                    if (_ssh.IsConnected)
                    {
                        _ssh.Disconnect();
                    }
                    _ssh.Dispose();
                }
                catch (Exception ex)
                {
                    // 해제 과정의 오류는 로깅만 남기고 무시 (AppGlobal 로거 사용)
                    // [Source Reference: AppGlobal.cs Line 42]
                    AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, "SshAdapter Dispose Error: " + ex.Message);
                }
                finally
                {
                    _ssh = null;
                }
            }
        }
    }
}
