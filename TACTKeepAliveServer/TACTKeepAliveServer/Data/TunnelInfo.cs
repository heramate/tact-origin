using System;
using System.Collections.Generic;//List
using System.Net.NetworkInformation;
using System.Text; //TcpState, IPAddress, IPGlobalProperties

namespace TACT.KeepAliveServer
{
    /// <summary>
    /// SSH터널 정보 (터널포트, 장비IP 묶음 정보)
    /// </summary>
    [Serializable]
    public class TunnelInfo //: MarshalByRefObject, ICloneable
    {
        public static string c_TunnelServerIP = "127.0.0.1";

        /// <summary>
        /// 장비IP (터널로 연결하려는 RPCS장비)
        /// </summary>
        private string m_DeviceIP = string.Empty;
        public string DeviceIP
        {
            get { return m_DeviceIP; }
            set { m_DeviceIP = value; }
        }

        /// <summary>
        /// 터널Open 상태 정보(LISTENING)
        /// TCP    127.0.0.1:40101        0.0.0.0:0              LISTENING       15592
        /// </summary>
        private TcpConnInfo m_TcpListenerInfo = null;
        public TcpConnInfo TcpListenerInfo
        {
            get { return m_TcpListenerInfo; }
            set
            {
                if (value == null) {
                    m_TcpListenerInfo = null;
                } else {
                    m_TcpListenerInfo = new TcpConnInfo(value);
                }
            }
        }
        public bool IsListen()
        {
            return m_TcpListenerInfo != null;
        }

        /// <summary>
        /// 터널 TCP연결정보 (사용중 여부)
        /// LocalAddr=118.217.79.23:52604, RemoteAddr=118.217.79.41:43210, State=TIME_WAIT, ProcessId=0, ProcessName=Idle
        /// LocalAddr=118.217.79.23:52608, RemoteAddr=118.217.79.41:43210, State=ESTABLISHED, ProcessId=8712, ProcessName=RACTDaemonExe
        /// </summary>
        private List<TcpConnInfo> m_TcpConnectionInfos = new List<TcpConnInfo>();
        public List<TcpConnInfo> TcpConnectionInfos
        {
            get {
                return m_TcpConnectionInfos;
            }

            set {
                m_TcpConnectionInfos.Clear();
                if (value == null) return;

                for (int i = 0; i < value.Count; i++ )
                {
                    if (value[i] == null) continue;

                    TcpConnInfo newTcpConn = new TcpConnInfo(value[i]);
                    m_TcpConnectionInfos.Add(newTcpConn);
                }
            }
        }
        public bool IsConnected(){
            return m_TcpConnectionInfos.Count > 0;
        }

        /// <summary>
        /// 터널용 리모트 포트번호 (Daemon은 터널을 통해 장비접속)
        /// </summary>
        private ushort m_TunnelPort = 0;
        public ushort TunnelPort
        {
            get { return m_TunnelPort; }
            set { m_TunnelPort = value; }
        }

        /// <summary>
        /// 터널 IP (기본은 localhost지만 외부 서버가 될수도 있으므로 일단 추가)
        /// </summary>
        private string m_TunnelIP = c_TunnelServerIP;
        public string TunnelIP
        {
            get { return m_TunnelIP; }
            set { m_TunnelIP = value; }
        }


        /// <summary>
        /// 터널 상태
        /// </summary>
        private E_TunnelState m_TunnelState = E_TunnelState.Closed;
        public E_TunnelState TunnelState
        {
            get { return m_TunnelState; }
            set { m_TunnelState = value; }
        }


        public void UpdateState(E_TunnelState aTunnelState)
        {
            /// 타임아웃 체크를 위한 TimeStamp 재설정
            switch (aTunnelState)
            {
                case E_TunnelState.Closed:
                    TimeStampClosed = DateTime.Now;
                    break;

                case E_TunnelState.WaitingOpen:
                    TimeStampWaitingOpen = DateTime.Now;
                    break;
                case E_TunnelState.Opened:
                    TimeStampOpened = DateTime.Now;
                    break;
                case E_TunnelState.Connected:
                    m_TimeStampConnected= DateTime.Now;
                    break;
                case E_TunnelState.WaitingClose:
                    TimeStampWaitingClose = DateTime.Now;
                    break;

                default:
                    break;
            }

            GlobalClass.PrintLogInfo(string.Format("● 터널상태 변경: DeviceIP={0}, Port={1}, TunnelState={2}→{3}, TimeStampClosed={4}, TimeStampWaitingOpen={5}, TimeStampOpened={6}, TimeStampWaitingClose={7}",
                                                    DeviceIP, TunnelPort, TunnelState.ToString(), aTunnelState.ToString(),
                                                    Util.DateTimeToLogValue(TimeStampClosed),
                                                    Util.DateTimeToLogValue(TimeStampWaitingOpen),
                                                    Util.DateTimeToLogValue(TimeStampOpened),
                                                    Util.DateTimeToLogValue(TimeStampWaitingClose)));
            /// 터널상태변경
            TunnelState = aTunnelState;
        }

        /// <summary>
        /// 터널포트Close 대기 시작
        /// </summary>
        private DateTime m_TimeStampClosed = DateTime.MinValue;
        public DateTime TimeStampClosed
        {
            get { return m_TimeStampClosed; }
            private set { m_TimeStampClosed = value; }
        }

        /// <summary>
        /// 터널포트Open 대기 시작 (장비에 터널Open 요청발송)
        /// </summary>
        private DateTime m_TimeStampWaitingOpen = DateTime.MinValue;
        public DateTime TimeStampWaitingOpen
        {
            get { return m_TimeStampWaitingOpen; }
            private set { m_TimeStampWaitingOpen = value; }
        }

        /// <summary>
        /// 터널포트에 클라이언트 접속
        /// </summary>
        private DateTime m_TimeStampConnected = DateTime.MinValue;
        public DateTime TimeStampConnected
        {
            get { return m_TimeStampConnected; }
            private set { m_TimeStampConnected = value; }
        }


        /// <summary>
        /// 터널포트Open(LISTENING) 시작
        /// </summary>
        private DateTime m_TimeStampOpened = DateTime.MinValue;
        public DateTime TimeStampOpened
        {
            get { return m_TimeStampOpened; }
            private set { m_TimeStampOpened = value; }
        }

        /// <summary>
        /// 터널포트Close 대기 시작 (장비에 터널Close 요청발송)
        /// </summary>
        private DateTime m_TimeStampWaitingClose = DateTime.MinValue;
        public DateTime TimeStampWaitingClose
        {
            get { return m_TimeStampWaitingClose; }
            private set { m_TimeStampWaitingClose = value; }
        }


        /// <summary>
        /// 포트 실 상태값 (System.Net.Networkinformation.TcpState)
        /// </summary>
        #region [참고] 포트상태 설명 ----------------------------------------------
        //정보출처: http://ktdsoss.tistory.com/282
        //public enum System.Net.Networkinformation.TcpState
        //{
        //    Unknown = 0,
        //    // 완전히 연결이 종료된 상태
        //    Closed = 1,
        //    // 서버의 데몬이 떠 있어서 클라이언트의 접속 요청을 기다리고 있는 상태(Windows에선 LISTENING)
        //    Listen = 2,
        //    // 클라이언트가 서버에게 연결을 요청한 상태
        //    SynSent = 3,
        //    // 서버가 클라이언트로부터 접속 요구(SYN)을 받아 클라이언트에게 응답(SYN/ACK)을 하였지만, 
        //    // 아직 클라이언트에게 확인 메시지(ACK)는 받지 못한 상태
        //    SynReceived = 4,
        //    // 서버와 클라이언트 간에 세션 연결이 성립되어 통신이 이루어지고 있는 상태
        //    // (클라이언트가 서버의 SYN을 받아서 세션이 연결된 상태)
        //    Established = 5,
        //    // 클라이언트가 서버에게 연결을 끊고자 요청하는 상태(FIN을 보낸 상태)
        //    FinWait1 = 6,
        //    // 서버가 클라이언트로부터 연결 종료 응답을 기다리는 상태
        //    //(서버가 클라이언트로부터 최초로 FIN을 받은 후, 클라이언트에게 ACK을 주었을 때)
        //    FinWait2 = 7,
        //    // TCP 연결이 상위 응용프로그램 레벨로부터 연결 종료를 기다리는 상태
        //    // :Passive Close 하는 쪽에서 프로그램이 소켓을 종료시키는 것을 기다리기 위한 상태. 
        //    // 가령, 소켓 프로그래밍 시 TCP connection 을 close 함수로 명시적으로 끊어주지 않으면 
        //    // CLOSE_WAIT 상태로 영원히 남을 수 있고 이는 resource leak 으로 이어짐
        //    CloseWait = 8,
        //    // 흔하지 않으나 주로 확인 메시지가 전송 도중 유실된 상태
        //    Closing = 9,
        //    // 호스트가 원격지 호스트의 연결 종료 요구 승인을 기다리는 상태
        //    // (서버가 클라이언트에게 FIN을 보냈을 때의 상태)
        //    LastAck = 10,
        //    // 연결은 종결되었지만 (분실되었을지 모를 느린 세그먼트를 위해) 당분간 소켓을 열어놓은 상태. 
        //    // 기본값은 120(초). Active Close 하는 쪽의 마지막 ACK가 유실되었을 때, 
        //    // Passive Close 하는 쪽은 자신이 보낸 FIN에 대한 응답을 받지 못했으므로 FIN을 재전송함. 
        //    // 이 때 TCP는 connection 정보를 유지하고 있고 ACK를 다시 보낼 수 있음
        //    TimeWait = 11,
        //    // The transmission control buffer (TCB) for the TCP connection is being deleted.
        //    DeleteTcb = 12
        //}
        //▼ 특수 IP 정보
        //IPAddress.Any = 0.0.0.0  /  IPAddress.IPv6Any = ::
        //IPAddress.None = 255.255.255.255  /  IPAddress.IPv6None = ::
        //IPAddress.Loopback = 127.0.0.1  /  IPAddress.IPv6Loopback = ::1
        //IPAddress.Broadcast = 255.255.255.255
        #endregion [참고] 포트상태 설명 ----------------------------------------------
        //[DefaultValue(E_TunnelPortStatus.WaitingOpen)]
        //public E_TunnelPortStatus PortStatus { get; set; }
        //[DefaultValue(TcpState.Unknown)]
        //public TcpState PortState { get; set; }

        /// <summary>
        /// 기본생성자 미사용
        /// </summary>
        private TunnelInfo() { }
        public TunnelInfo(string aTunnelIP, ushort aTunnelPort, string aDeviceIP, E_TunnelState aTunnelState)
        {
            TunnelIP = aTunnelIP;
            TunnelPort = aTunnelPort;
            DeviceIP = aDeviceIP;
            UpdateState(aTunnelState);
        }

        public TunnelInfo(ushort aTunnelPort, E_TunnelState aTunnelState)
        {
            //TunnelIP = aTunnelIP;
            TunnelPort = aTunnelPort;
            //DeviceIP = aDeviceIP;
            TunnelState = aTunnelState;
        }

        /// <summary>
        /// 터널을 이용중인 세션 수
        /// </summary>
        /// <returns></returns>
        public int GetTunnelConnectCount()
        {
            return (TcpConnectionInfos == null ? 0 : TcpConnectionInfos.Count);
        }

        public string _ToString(bool _bIncludeTcpConn = false)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("\t터널포트={0}, 장비IP={1}, 터널상태={2}, 터널Open대기시작={3}, 터널Open시작={4}, 터널Close대기시작={5}"
                            , this.m_TunnelPort, this.m_DeviceIP
                            , this.m_TunnelState.ToString()
                            , Util.DateTimeToLogValue(this.TimeStampWaitingOpen)
                            , Util.DateTimeToLogValue(this.TimeStampOpened)
                            , Util.DateTimeToLogValue(this.TimeStampWaitingClose)));

            if (_bIncludeTcpConn)
            {
                if (m_TcpConnectionInfos == null || m_TcpConnectionInfos.Count == 0)
                {
                    sb.AppendLine("\t└터널에 접속정보 없음.");
                }
                else 
                {
                    sb.AppendLine("\t└로컬주소      \t┃리모트주소     \t┃포트상태\t┃프로세스정보\t┃정보갱신시각");
                    for (int i = 0 ; i < m_TcpConnectionInfos.Count ; i++)
                    {
                        TcpConnInfo tcpConn = m_TcpConnectionInfos[i];
                        if (m_TcpConnectionInfos[i] == null) continue;
                        sb.AppendLine(string.Format("\t{0}:{1}\t{2}:{3}\t{4}\t{5}:{6}\t{7}"
                        , tcpConn.LocalAddress, tcpConn.LocalPort, tcpConn.RemoteAddress, tcpConn.RemotePort, tcpConn.State.ToString()
                        , tcpConn.ProcessId, tcpConn.ProcessName, tcpConn.UpdateTime.ToString("MM/dd HH:mm:ss")));
                    }
                }
            }

            return sb.ToString();
        }

    } // End of class TunnelPort


}
