using System;
using System.ComponentModel; //[DisplayName

namespace TACT.KeepAliveServer
{
    /// <summary>
    /// 터널용 새 포트를 요청한 결과
    /// </summary>
    public enum E_RequestTunnelResult
    {
        /// <summary>
        /// 기본값
        /// </summary>
        Canceled,
        /// <summary>
        /// 장비로의 터널이 이미 열려있는 경우
        /// </summary>
        Opened,
        /// <summary>
        /// 터널 요청 대기열에 추가(E_TunnelState.WaitingKeepAlive)
        /// </summary>
        RequestTunnel,
        /// <summary>
        /// 장비당 사용자세션 최대를 초과하는 경우
        /// </summary>
        UserSessionFull,

        /// <summary>
        /// 사용가능한 터널포트가 없는경우(모든 번호 사용중)
        /// </summary>
        NoUseableTunnelPort,
        /// <summary>
        /// 해당 장비에서 KeepAlive메시지가 오지 않는 경우
        /// </summary>
        NoKeepAlive,
    }

    /// <summary>
    /// SSH터널 상태 전이
    /// </summary>
    public enum E_TunnelState
    {
        /// <summary>
        /// 기본값 - KAM요청발송 대기중
        /// </summary>
        [Description("터널포트할당")]
        Closed,
        /// <summary>
        /// 연결대기 - 장비에 SSH터널Open요청을 보낸 상태
        /// </summary>
        [Description("터널Open대기")]
        WaitingOpen,
        /// <summary>
        /// 연결완료 - 포트 오픈(LISTENING, 클라이언트 연결은 없는 상태)
        /// </summary>
        [Description("터널Open완료")]
        Opened,
        /// <summary>
        /// 사용중 - 터널포트에 클라이언트가 접속하여 사용중(ESTABLISHED)
        /// </summary>
        [Description("터널접속사용중")]
        Connected,
        /// <summary>
        /// 연결해제대기 - 닫힐 포트(장비에 SSH터널Close요청이 전송대기중이거나 이미 전송된 상태)
        /// </summary>
        [Description("터널Close대기")]
        WaitingClose
    }


}
