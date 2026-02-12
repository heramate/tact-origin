using Rebex.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RACTClient.Adapters
{
    /// <summary>
    /// 클라이언트 옵션(ClientOption)을 기반으로 Rebex Proxy 객체를 생성하는 팩토리 클래스입니다.
    /// </summary>
    public static class RebexProxyFactory
    {
        /// <summary>
        /// 현재 AppGlobal 설정에 맞는 Proxy 객체를 생성하여 반환합니다.
        /// 프록시를 사용하지 않는 경우 null을 반환합니다.
        /// </summary>
        /// <returns>Rebex.Net.Proxy 객체 또는 null</returns>
        public static Proxy CreateProxy()
        {
            // 1. 옵션 유효성 검사
            if (AppGlobal.s_ClientOption == null || !AppGlobal.s_ClientOption.UseProxy)
            {
                return null;
            }

            // 2. 프록시 타입 매핑 (Legacy 옵션값 -> Rebex Enum)
            ProxyType rebexProxyType = GetRebexProxyType(AppGlobal.s_ClientOption.ProxyType);

            // 3. Proxy 객체 생성 (호스트 및 포트 설정)
            // [Source Reference: TerminalClientWinForm_MainForm.cs, Line 451]
            Proxy proxy = new Proxy(
                rebexProxyType,
                AppGlobal.s_ClientOption.ProxyHost,
                AppGlobal.s_ClientOption.ProxyPort
            );

            // 4. 인증 정보 설정 (ID/PW가 있는 경우)
            if (!string.IsNullOrEmpty(AppGlobal.s_ClientOption.ProxyUser))
            {
                // [Source Reference: TerminalClientWinForm_MainForm.cs, Line 452]
                proxy.AuthenticationMethod = ProxyAuthentication.Basic;
                proxy.Credentials = new NetworkCredential(
                    AppGlobal.s_ClientOption.ProxyUser,
                    AppGlobal.s_ClientOption.ProxyPass
                );
            }

            return proxy;
        }

        /// <summary>
        /// 기존 시스템의 프록시 타입(int/enum)을 Rebex ProxyType으로 변환합니다.
        /// </summary>
        private static ProxyType GetRebexProxyType(int legacyType)
        {
            // AppGlobal.s_ClientOption.ProxyType의 정의에 따라 매핑 수정 필요
            // 예시: 0=Socks4, 1=Socks5, 2=HttpConnect
            switch (legacyType)
            {
                case 1:
                    return ProxyType.Socks5;
                case 2:
                    return ProxyType.HttpConnect; // HTTP Tunneling
                case 0:
                default:
                    return ProxyType.Socks4;
            }
        }
    }
}
