/*
 * 로컬 포트 및 프로세스 관리
 * 
 * C# Sample to list all the active TCP and UDP connections using Windows Form appl
 * 코드출처: https://code.msdn.microsoft.com/windowsdesktop/C-Sample-to-list-all-the-4817b58f/sourcecode?fileId=147562&pathId=737457011
 * Create Date: 2018.12.04
 */
using System;
using System.Collections.Generic;//List<>
using System.Linq;  //[].Any
using System.Runtime.InteropServices;//[StructLayout
using System.ComponentModel; //[DisplayName
using System.Net; //IPAddress
using System.Diagnostics; //Process
using System.Text;//StringBuilder

namespace TACT.KeepAliveServer
{
    public class SocketConnections : IDisposable
    {
        // The version of IP used by the TCP/UDP endpoint. AF_INET is used for IPv4.
        private const int AF_INET = 2; //IPv4

        // System Error Code
        private const int ERROR_SUCCESS = 0;
        private const int ERROR_INSUFFICIENT_BUFFER = 122; //0x007A: The data area passed to a system call is too small.
        private const int ERROR_INVALID_PARAMETER = 87; //0x57: The parameter is incorrect.

        /// <summary>
        /// Listening 포트 정보 (Key: localPort/listen/터널포트)
        /// </summary>
        private Dictionary<ushort, TcpConnInfo> m_AllTcpListeners = new Dictionary<ushort, TcpConnInfo>();
        /// <summary>
        /// 클라이언트측 연결정보 (Key: remotePort)
        /// </summary>
        private Dictionary<ushort, List<TcpConnInfo>> m_AllTcpConnections = new Dictionary<ushort, List<TcpConnInfo>>();


        #region 외부호출용 메소스 정의 -------------------------------------------------------

        public void Dispose()
        {
            m_AllTcpListeners.Clear();
            m_AllTcpListeners = null;

            m_AllTcpConnections.Clear();
            m_AllTcpConnections = null;
        }

        /// <summary>
        /// 특정포트가 Listen 상태인지 확인
        /// </summary>
        /// <param name="listenPort"></param>
        /// <returns></returns>
        public bool IsListen(ushort listenPort)
        {
            return m_AllTcpListeners.ContainsKey(listenPort);
        }

        /// <summary>
        /// 특정포트와의 TCP연결정보 목록 조회
        /// </summary>
        /// <param name="listenPort"></param>
        /// <returns></returns>
        public List<TcpConnInfo> GetTcpConnections(ushort listenPort)
        {
            List<TcpConnInfo> result = null;
            lock (m_AllTcpConnections)
            {
                if (m_AllTcpConnections.ContainsKey(listenPort)) {
                    result = m_AllTcpConnections[listenPort];
                }
            }
            return result;
        }


        /// <summary>
        /// LISTENING포트 TCP정보 조회
        /// </summary>
        /// <param name="listenPort"></param>
        /// <returns></returns>
        public TcpConnInfo GetTcpListener(ushort listenPort)
        {
            lock (m_AllTcpListeners)
            {
                if (m_AllTcpListeners.ContainsKey(listenPort))
                {
                    return m_AllTcpListeners[listenPort];
                }
            }
            return null;
        }

        /// <summary>
        /// 관리포트를 제외한 열린포트의 프로세스를 강제종료(close port)한다.
        /// (프로세스 정보가 없는 포트는 처리불가)
        /// </summary>
        /// <param name="managedPorts">관리포트 목록(null인 경우 모든 포트 Close)</param>
        public void KillProcesses(ushort [] managedPorts)
        {
            foreach (ushort listenPort in m_AllTcpListeners.Keys)
            {
                if (managedPorts != null && Array.Exists(managedPorts, e => e == listenPort)) continue;

                TcpConnInfo tcpConnInfo = m_AllTcpListeners[listenPort];
                if (tcpConnInfo.ProcessId == 0) 
                {
                    GlobalClass.PrintLogInfo(string.Format("[SocketConnections.KillProcesses] 프로세스 정보가 없어 종료(close port)할 수 없습니다.listenPort={0}, {1},", listenPort, tcpConnInfo.ToString()));
                    continue;
                }

                KillProcess(listenPort);
            }
        }

        /// <summary>
        /// 특정포트를 제외한 모든 TCP연결정보 조회
        /// </summary>
        /// <param name="excludedPorts">제외포트번호 목록</param>
        /// <returns></returns>
        //public List<TcpConnInfo> GetTcpConnInfos(ushort[] excludedPorts)
        //{
        //    List<TcpConnInfo> tcpConnList = new List<TcpConnInfo>();
        //    foreach (ushort listenPort in m_AllTcpListeners.Keys)
        //    {
        //        if (excludedPorts != null && Array.Exists(excludedPorts, e => e == listenPort)) continue;

        //        TcpConnInfo tcpConnInfo = m_AllTcpListeners[listenPort];
        //        tcpConnList.Add(tcpConnInfo);
        //    }
        //    return tcpConnList;
        //}

        /// <summary>
        /// 해당포트의 프로세스를 종료합니다.
        /// (프로세스 정보가 없는 포트는 처리불가)
        /// </summary>
        /// <param name="listenPort">종료(close)할 포트번호</param>
        public void KillProcess(ushort listenPort)
        {
            try
            {
                if (!m_AllTcpListeners.ContainsKey(listenPort)) return;

                TcpConnInfo tcpConnInfo = m_AllTcpListeners[listenPort];
                if (tcpConnInfo == null)
                {
                    GlobalClass.PrintLogInfo(string.Format("[SocketConnections.KillProcess] 포트연결정보가 없어 종료(close port)할 수 없습니다.listenPort={0}", listenPort));
                    return;
                }

                if (tcpConnInfo.ProcessId <= 0)
                {
                    GlobalClass.PrintLogInfo(string.Format("[SocketConnections.KillProcess] 프로세스 정보가 없어 종료(close port)할 수 없습니다.listenPort={0}", listenPort));
                    return;
                }

                try
                {
                    Process proc = Process.GetProcessById(tcpConnInfo.ProcessId);
                    if (proc != null)
                    {
                        GlobalClass.PrintLogInfo(string.Format("[SocketConnections.KillProcess] 수신포트 {0}의 프로세스를 강제종료 합니다. {1}", listenPort, tcpConnInfo._ToString()));
                        proc.Kill();
                    }
                }
                catch (ArgumentException ae)
                {
                    GlobalClass.PrintLogException(string.Format("[SocketConnections.KillProcess] 수신포트 {0}의 프로세스가 실행되고 있지 않습니다. {1}", listenPort, tcpConnInfo._ToString()), ae);
                }
            }
            catch (Exception e)
            {
                GlobalClass.PrintLogException("[SocketConnections.KillProcess] ", e);
            }
        }

        [DllImport("iphlpapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern uint GetExtendedTcpTable(IntPtr pTcpTable, ref int pdwSize,
            bool bOrder, int ulAf, TcpTableClass tableClass, uint reserved = 0);


        /// <summary>
        /// 로컬의 TCP연결정보를 조회해서 업데이트한다.
        /// </summary>
        /// <param name="aMinPort">조회대상포트 Min포트값</param>
        /// <param name="aMaxPort">조회대상포트 Max포트값</param>
        public void UpdateAllTcpConnections(ushort aMinPort = 0, ushort aMaxPort = ushort.MaxValue)
        {
            m_AllTcpListeners.Clear();
            m_AllTcpConnections.Clear();

            int bufferSize = 0;
            uint result = GetExtendedTcpTable(IntPtr.Zero, ref bufferSize, true, AF_INET, TcpTableClass.TCP_TABLE_OWNER_PID_ALL);

            IntPtr tcpProcessListPtr = Marshal.AllocHGlobal(bufferSize);

            try
            {
                result = GetExtendedTcpTable(tcpProcessListPtr, ref bufferSize, true, AF_INET, TcpTableClass.TCP_TABLE_OWNER_PID_ALL);

                if (result != ERROR_SUCCESS)
                {
                    //GlobalClass.PrintLogError(string.Format("■Error2! [SocketConnections.GetAllTcpConnections] resultCode = {0}", result));
                    return;
                }


                MIB_TCPTABLE_OWNER_PID tcpRecordsTable = (MIB_TCPTABLE_OWNER_PID)
                                        Marshal.PtrToStructure(tcpProcessListPtr, typeof(MIB_TCPTABLE_OWNER_PID));
                IntPtr tableRowPtr = (IntPtr)((long)tcpProcessListPtr +
                                        Marshal.SizeOf(tcpRecordsTable.dwNumEntries));

                for (int row = 0; row < tcpRecordsTable.dwNumEntries; row++)
                {
                    MIB_TCPROW_OWNER_PID tcpRow = (MIB_TCPROW_OWNER_PID)Marshal.PtrToStructure(tableRowPtr, typeof(MIB_TCPROW_OWNER_PID));


                    tableRowPtr = (IntPtr)((long)tableRowPtr + Marshal.SizeOf(tcpRow));

                    ushort localPort = BitConverter.ToUInt16(new byte[2] { tcpRow.localPort[1], tcpRow.localPort[0] }, 0);
                    ushort remotePort = BitConverter.ToUInt16(new byte[2] { tcpRow.remotePort[1], tcpRow.remotePort[0] }, 0);

                    /// 로컬포트 또는 리모트포트가 지정 포트 범위내 인것만 처리한다.
                    if ((localPort < aMinPort || localPort > aMaxPort)
                        && (remotePort < aMinPort || remotePort > aMaxPort)) continue;

                    TcpConnInfo tcpConnInfo = new TcpConnInfo(new IPAddress(tcpRow.localAddr).ToString(),
                                                              new IPAddress(tcpRow.remoteAddr).ToString(),
                                                              localPort,
                                                              remotePort,
                                                              tcpRow.owningPid,
                                                              tcpRow.state);
                    
                    //GlobalClass.PrintLogInfo(string.Format("●TCP포트조회: {0}", tcpConnInfo._ToString()), true);
                    /// LISTEN
                    //Local           ┃Remote          ┃State        ┃ProcessId ┃ProcessName
                    //0.0.0.0:40101	  ┃0.0.0.0:0	    ┃LISTENING	   ┃16536	   ┃sshd (40101포트로 터널Open)
                    if (localPort >= aMinPort && localPort <= aMaxPort
                        && tcpRow.state == MibTcpState.LISTENING)
                    {
                        m_AllTcpListeners[localPort] = tcpConnInfo;
                    }
                    /// CLIENT CONNECTION
                    //127.0.0.1:37523 ┃127.0.0.1:40101	┃ESTABLISHED  ┃9796	   ┃TACTDaemonExe (데몬→터널에 접속)
                    else if (remotePort >= aMinPort && remotePort <= aMaxPort)
                    {
                        if (!m_AllTcpConnections.ContainsKey(remotePort))
                        {
                            List<TcpConnInfo> tcpConnList = new List<TcpConnInfo>();
                            m_AllTcpConnections[remotePort] = tcpConnList;
                        }

                        m_AllTcpConnections[remotePort].Add(tcpConnInfo);
                    }
                    else
                    {
                        /// 무시되는 연결정보
                        //127.0.0.1:40101 ┃127.0.0.1:37523 ┃ESTABLISHED  ┃16536	   ┃OpenSSH
                        tcpConnInfo = null;
                    }
                } // End of for

            }
            catch (OutOfMemoryException outOfMemoryException)
            {
                GlobalClass.PrintLogException("[SocketConnections.UpdateAllTcpConnections] Out Of Memory: ", outOfMemoryException);
            }
            catch (Exception e)
            {
                GlobalClass.PrintLogException("[SocketConnections.UpdateAllTcpConnections] Exception : ", e);
            }
            finally
            {
                Marshal.FreeHGlobal(tcpProcessListPtr);
            }
        }

        #endregion 외부호출용 메소스 정의 ----------------------------------------------------

        #region 정보 출력용 ToString() 대체 메소드 -------------------------------------------
        /// <summary>
        /// TcpListeners 정보를 string으로 얻는다 (정보출력용)
        /// </summary>
        /// <returns></returns>
        public string TcpListenersToString()
        {
            if (m_AllTcpListeners.Count == 0) return "조회결과 없음.";
            StringBuilder sb = new StringBuilder();
            sb.Append("▼ GetAllTcpConnections ----------------\r\n");
            sb.Append("Local\t ┃State\t ┃ProcessId\t ┃ProcessName\r\n");
            foreach (ushort listenPort in m_AllTcpListeners.Keys)
            {
                TcpConnInfo tcpConnInfo = m_AllTcpListeners[listenPort];
                sb.Append(string.Format("{0}:{1}\t ┃{2}\t ┃{3} ┃{4}\r\n",
                    tcpConnInfo.LocalAddress.ToString(), tcpConnInfo.LocalPort,
                    //tcpConnInfo.RemoteAddress.ToString(), tcpConnInfo.RemotePort,
                    tcpConnInfo.State.ToString(),
                    tcpConnInfo.ProcessId,
                    tcpConnInfo.ProcessName
                    ));
            }
            return sb.ToString();
        }

        /// <summary>
        /// tcp 연결정보를 string으로 얻는다 (정보출력용)
        /// </summary>
        public string TcpConnectionsToString()
        {
            if (m_AllTcpConnections.Count == 0) return "조회결과 없음.";
            StringBuilder sb = new StringBuilder();
            sb.Append("▼ GetAllTcpConnections ----------------\r\n");
            sb.Append("Local\t ┃Remote\t ┃State\t ┃ProcessId ┃ProcessName\r\n");
            foreach (ushort remotePort in m_AllTcpConnections.Keys)
            {
                List<TcpConnInfo> tcpConnList = m_AllTcpConnections[remotePort];
                foreach (TcpConnInfo tcpConnInfo in tcpConnList)
                {
                    sb.Append(string.Format("{0}:{1}\t ┃{2}:{3}\t ┃{4} ┃{5}\t ┃{6}\r\n",
                        tcpConnInfo.LocalAddress.ToString(), tcpConnInfo.LocalPort,
                        tcpConnInfo.RemoteAddress.ToString(), tcpConnInfo.RemotePort,
                        tcpConnInfo.State.ToString(),
                        tcpConnInfo.ProcessId,
                        tcpConnInfo.ProcessName
                        ));
                }
            }
            return sb.ToString();
        }
        #endregion 정보 출력용 ToString() 대체 메소드 ----------------------------------------

    } // End of class SocketConnections

    [StructLayout(LayoutKind.Sequential)]
    public class TcpConnInfo
    {
        [DisplayName("Updated DateTime")]
        public DateTime UpdateTime {get; set;}

        [DisplayName("Local Address")]
        [DefaultValue("")]//IPAddress.Any
        public string LocalAddress { get; set; }

        [DisplayName("Local Port")]
        [DefaultValue(0)]
        public int LocalPort { get; set; }

        [DisplayName("Remote Address")]
        [DefaultValue("")]//IPAddress.Any
        public string RemoteAddress { get; set; }

        [DisplayName("Remote Port")]
        [DefaultValue(0)]
        public int RemotePort { get; set; }

        [DisplayName("State")]
        [DefaultValue(MibTcpState.NONE)]
        public MibTcpState State { get; set; }

        [DisplayName("Process ID")]
        [DefaultValue(0)]
        public int ProcessId { get; set; }

        [DisplayName("Process Name")]
        [DefaultValue("")]
        public string ProcessName { get; set; }

        private TcpConnInfo() { }
        public TcpConnInfo(string localIp, string remoteIp, int localPort,
                              int remotePort, int pId, MibTcpState state)
        {
            UpdateTime = DateTime.Now;
            LocalAddress = localIp;
            RemoteAddress = remoteIp;
            LocalPort = localPort;
            RemotePort = remotePort;
            State = state;
            ProcessId = pId;

            if (Process.GetProcesses().Any(process => process.Id == pId)) {
                ProcessName = Process.GetProcessById(ProcessId).ProcessName;
            }
        }
        public TcpConnInfo(TcpConnInfo aSource)
        {
            this.UpdateTime = aSource.UpdateTime;
            this.LocalAddress = aSource.LocalAddress;
            this.RemoteAddress = aSource.RemoteAddress;
            this.LocalPort = aSource.LocalPort;
            this.RemotePort = aSource.RemotePort;
            this.State = aSource.State;
            this.ProcessId = aSource.ProcessId;
            this.ProcessName = aSource.ProcessName;
        }

        public string _ToString()
        {
            return string.Format("LocalAddr={0}:{1}, RemoteAddr={2}:{3}, State={4}, ProcessId={5}, ProcessName={6}", 
                                    this.LocalAddress, this.LocalPort, 
                                    this.RemoteAddress, this.RemotePort, 
                                    this.State.ToString(), 
                                    this.ProcessId, this.ProcessName);
        }

        /// <summary>
        /// List<TcpConnInfo> 복사 메서드
        /// </summary>
        /// <param name="aTcpConnList"></param>
        /// <returns></returns>
        public static List<TcpConnInfo> CopyTcpConnInfoList(List<TcpConnInfo> aTcpConnList)
        {
            List<TcpConnInfo> newConnList = new List<TcpConnInfo>();
            if (aTcpConnList == null) return newConnList;

            for (int i = 0 ; i < aTcpConnList.Count ; i++)
            {
                if (aTcpConnList[i] == null) continue;
                newConnList.Add(new TcpConnInfo(aTcpConnList[i]));
            }
            return newConnList;
        }
    }

    // Enum to define the set of values used to indicate the type of table returned by 
    // calls made to the function 'GetExtendedTcpTable'.
    public enum TcpTableClass
    {
        TCP_TABLE_BASIC_LISTENER,
        TCP_TABLE_BASIC_CONNECTIONS,
        TCP_TABLE_BASIC_ALL,
        TCP_TABLE_OWNER_PID_LISTENER,
        TCP_TABLE_OWNER_PID_CONNECTIONS,
        TCP_TABLE_OWNER_PID_ALL,
        TCP_TABLE_OWNER_MODULE_LISTENER,
        TCP_TABLE_OWNER_MODULE_CONNECTIONS,
        TCP_TABLE_OWNER_MODULE_ALL
    }

    // Enum for different possible states of TCP connection
    public enum MibTcpState
    {
        /// <summary>
        /// 완전히 연결이 종료된 상태
        /// </summary>
        CLOSED = 1,
        /// <summary>
        /// 서버의 데몬이 떠 있어서 클라이언트의 접속 요청을 기다리고 있는 상태(Windows에선 LISTENING)
        /// </summary>
        LISTENING = 2,
        /// <summary>
        /// 클라이언트가 서버에게 연결을 요청한 상태
        /// </summary>
        SYN_SENT = 3,
        /// <summary>
        /// 서버가 클라이언트로부터 접속 요구(SYN)을 받아 클라이언트에게 응답(SYN/ACK)을 하였지만, 
        //  아직 클라이언트에게 확인 메시지(ACK)는 받지 못한 상태
        /// </summary>
        SYN_RCVD = 4,
        /// <summary>
        /// 서버와 클라이언트 간에 세션 연결이 성립되어 통신이 이루어지고 있는 상태
        //  (클라이언트가 서버의 SYN을 받아서 세션이 연결된 상태)
        /// </summary>
        ESTABLISHED = 5,
        /// <summary>
        /// 클라이언트가 서버에게 연결을 끊고자 요청하는 상태(FIN을 보낸 상태)
        /// </summary>
        FIN_WAIT1 = 6,
        /// <summary>
        /// 서버가 클라이언트로부터 연결 종료 응답을 기다리는 상태
        /// (서버가 클라이언트로부터 최초로 FIN을 받은 후, 클라이언트에게 ACK을 주었을 때)
        /// </summary>
        FIN_WAIT2 = 7,
        /// <summary>
        /// TCP 연결이 상위 응용프로그램 레벨로부터 연결 종료를 기다리는 상태
        /// :Passive Close 하는 쪽에서 프로그램이 소켓을 종료시키는 것을 기다리기 위한 상태. 
        /// 가령, 소켓 프로그래밍 시 TCP connection 을 close 함수로 명시적으로 끊어주지 않으면 
        /// CLOSE_WAIT 상태로 영원히 남을 수 있고 이는 resource leak 으로 이어짐
        /// </summary>
        CLOSE_WAIT = 8,
        /// <summary>
        /// 흔하지 않으나 주로 확인 메시지가 전송 도중 유실된 상태
        /// </summary>
        CLOSING = 9,
        /// <summary>
        /// 호스트가 원격지 호스트의 연결 종료 요구 승인을 기다리는 상태
        /// (서버가 클라이언트에게 FIN을 보냈을 때의 상태)
        /// </summary>
        LAST_ACK = 10,
        /// <summary>
        /// 연결은 종결되었지만 (분실되었을지 모를 느린 세그먼트를 위해) 당분간 소켓을 열어놓은 상태. 
        /// 기본값은 120(초). Active Close 하는 쪽의 마지막 ACK가 유실되었을 때, 
        /// Passive Close 하는 쪽은 자신이 보낸 FIN에 대한 응답을 받지 못했으므로 FIN을 재전송함. 
        /// 이 때 TCP는 connection 정보를 유지하고 있고 ACK를 다시 보낼 수 있음
        /// </summary>
        TIME_WAIT = 11,
        /// <summary>
        /// The transmission control buffer (TCB) for the TCP connection is being deleted.
        /// </summary>
        DELETE_TCB = 12,
        NONE = 0
    }

    /// <summary>
    /// The structure contains a table of process IDs (PIDs) and the IPv4 TCP links that 
    /// are context bound to these PIDs.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MIB_TCPTABLE_OWNER_PID
    {
        public uint dwNumEntries;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct,
            SizeConst = 1)]
        public MIB_TCPROW_OWNER_PID[] table;
    }

    /// <summary>
    /// The structure contains information that describes an IPv4 TCP connection with 
    /// IPv4 addresses, ports used by the TCP connection, and the specific process ID
    /// (PID) associated with connection.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MIB_TCPROW_OWNER_PID
    {
        public MibTcpState state;
        public uint localAddr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] localPort;
        public uint remoteAddr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] remotePort;
        public int owningPid;
    }
}
