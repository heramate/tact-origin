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
    /// Rebex Ssh 및 Telnet 연결을 추상화하는 공통 인터페이스입니다.
    /// </summary>
    public interface IRebexConnection : IDisposable
    {
        /// <summary>
        /// 연결 여부를 반환합니다.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// 프록시 서버 설정을 적용합니다.
        /// </summary>
        /// <param name="proxy">Rebex Proxy 객체</param>
        void SetProxy(Proxy proxy);

        /// <summary>
        /// 서버에 접속합니다. (Socket 연결)
        /// </summary>
        /// <param name="host">호스트 주소</param>
        /// <param name="port">포트 번호</param>
        void Connect(string host, int port);

        /// <summary>
        /// 로그인 인증을 수행합니다.
        /// SSH는 이 단계에서 인증을 수행하며, Telnet은 구현에 따라 스크립트로 처리하거나 무시합니다.
        /// </summary>
        /// <param name="userName">사용자 ID</param>
        /// <param name="password">비밀번호</param>
        void Login(string userName, string password);

        /// <summary>
        /// TerminalControl.Bind()에 전달할 채널 팩토리 객체를 반환합니다.
        /// </summary>
        /// <returns>IShellChannelFactory (Ssh 또는 Telnet 객체)</returns>
        IShellChannelFactory GetClientObject();
    }
}
