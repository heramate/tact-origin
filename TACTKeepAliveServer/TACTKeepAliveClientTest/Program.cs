using System;
//using System.Collections.Generic;
//using System.Linq;
using System.Text; //Encoding
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Collections; // BitArray
using System.Collections.Generic;
using System.Diagnostics;
using TACT.KeepAliveServer;
using RACTCommonClass;

using System.Runtime.Serialization.Formatters;//FormatterAssemblyStyle
using System.Runtime.Serialization.Formatters.Binary;//BinaryFormatter

/*
 * Keep-Alive Client 테스터 - 2018.11
 * (KAM을 보내주는 RPCS가 없는 상태에서 테스트를 위해 만든 로컬테스터)
 * 
 * ▼History
 * 2019.09.19 운영의 c-RPCS에서 Full KAM과 Min KAM 타이머가 따로 작동중으로 확인되어 동일하게 변경(timer 2개)
 * 2019.10.01 SVNCommit
 * 2019.11.xx putty profile을 이용해 지정 텔넷서버로 터널Open/Close 실행기능 추가(_ReceiveThread)
 *            putty대신 OpenSSH 이용하여 SSH터널 Open/Close하도록 변경
 */
namespace TACTKeepAliveClientTest
{
    class Program
    {
        public static string serverIP = "127.0.0.1";//"118.217.79.23";
        public static int serverPort = 40001;
        public static string deviceIP = "218.51.127.184";
        public static bool base64Apply = true;
        public static int periodKeepAliveMin = 60; //초
        public static int periodKeepAliveFull = 60*3; //초

        //"C:\\Program Files\\OpenSSH\\ssh.exe", " -R 40101:127.0.0.1 ssh-tunnel@127.0.0.1 -p 40002"
        public static string sshExeFilePath = "C:\\Program Files\\OpenSSH\\ssh.exe";
        public static string telnetServerIP = "127.0.0.1";
        public static int telnetServerPort = 23;

        // (1) UdpClient 객체 성성. 
        public static IPEndPoint localEndPoint = null;
        public static UdpClient cli = new UdpClient();
        public static Thread m_ReceiveThread = null;
        public static ManualResetEvent m_ReceiveMRE = new ManualResetEvent(false);
        public static bool m_IsStop = false;

        public static StreamWriter logWriter = null;

        //       [서버IP] [서버KAM수신포트] [장비IP] [Base64적용여부(1=적용,그외=미적용)
        // 실행: TACTKeepAliveClient.exe 127.0.0.1 40001 100.64.131.75 1
        static void Main(string[] args)
        {
            //args = new string[] { "127.0.0.1", "40001", "100.64.131.75", 1, "127.0.0.1", 23 };
            if (args.Length < 6)
            {
                Console.WriteLine("● 실행 파라미터 갯수가 다릅니다.\r\n실행파라미터: [서버IP] [서버KAM수신포트] [장비IP] [Base64적용여부(1=적용,그외=미적용)] [MinKAM전송주기(초,기본값=60초] [FullKAM전송주기(초,기본값=3분])");
                return;
            }

            // [1] 서버IP
            serverIP = args[0];
            // [2] 서버PORT
            serverPort = Convert.ToInt32(args[1]);
            // [3] 장비IP
            deviceIP = args[2];
            // [4] Base64적용 여부(1=적용,기본값=미적용)
            base64Apply = args[3].Equals("1");
            // [5] 터널을 연결할 Telnet서버IP 
            telnetServerIP = args[4];
            // [6] 터널을 연결할 Telnet서버포트
            telnetServerPort = Convert.ToInt32(args[5]);
            // [7] (옵션)Receive[Min] KAM 주기 설정 (초)
            if (args.Length > 6) { periodKeepAliveMin = Convert.ToInt32(args[6]); }
            // [8] (옵션)Receive[Full] KAM 주기 설정 (초)
            if (args.Length > 7) { periodKeepAliveFull = Convert.ToInt32(args[7]); }


            PrintLog(String.Format("● KeepAliveClient 실행! {0} \r\n  KAMSerer={1}:{2} \r\n  DeviceIP={3}, Base64={4}\r\n  Receive[Min]발송주기={5}초, Receive[Full]발송주기={6}초"
                            ,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), serverIP, serverPort
                            ,deviceIP, base64Apply ? "ON" : "OFF"
                            ,periodKeepAliveMin, periodKeepAliveFull
                            ));

            /// 서버 연결시도(원격호스트 설정)
            try
            {
                cli.Connect(new IPEndPoint(IPAddress.Parse(serverIP), serverPort));
            }
            catch (Exception e)
            {
                PrintLog("UdpClient.Connect fail! " + e.ToString());
                return;
            }

            if (cli.Client == null || cli.Client.LocalEndPoint == null) 
            {
                PrintLog("KAMServer에 연결이 실패했습니다.");
                return;
            }

            System.Threading.Timer timerRequestMin = null;
            System.Threading.Timer timerRequestFull = null;
            try
            {
                localEndPoint = (IPEndPoint)cli.Client.LocalEndPoint;
                m_ReceiveThread = new Thread(new ThreadStart(_ReceiveThread));
                m_ReceiveThread.Start();

                /// 대기(System.Threading.Timeout.Infinite)상태의 타이머 생성
                timerRequestMin = new System.Threading.Timer(new TimerCallback(_callback_Timer_Min), null, System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                timerRequestFull = new System.Threading.Timer(new TimerCallback(_callback_Timer_Full), null, System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);

                while (true)
                {
                    PrintLog("\r\nF1: KAM발송시작 / F2: KAM발송중지 / ELSE: 프로그램종료");
                    ConsoleKeyInfo key = Console.ReadKey();
                    if (key.Key == ConsoleKey.F1)
                    {
                        PrintLog(string.Format("[KAM발송시작] KAM발송 시작! Request[Min]={0}초 / Request[Full]={1}초마다 발송..", periodKeepAliveMin, periodKeepAliveFull));
                        timerRequestMin.Change(3*1000, periodKeepAliveMin * 1000); //N초후에 periodKeepAliveMin초 마다 실행
                        timerRequestFull.Change(10*1000, periodKeepAliveFull * 1000); //N초후에 periodKeepAliveFull초 마다 실행
                        m_ReceiveMRE.Set();
                    }
                    else if (key.Key == ConsoleKey.F2)
                    {
                        PrintLog("[KAM발송중지]");
                        timerRequestMin.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                        timerRequestFull.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                        m_ReceiveMRE.Reset();
                    }
                    else
                    {
                        m_IsStop = true;
                        m_ReceiveMRE.Reset();
                        PrintLog("program terminate!");
                        break;
                    }
                }

            }
            finally
            {
                if (timerRequestMin != null) {
                    timerRequestMin.Dispose(); timerRequestMin = null;
                }
                if (timerRequestFull != null) {
                    timerRequestFull.Dispose(); timerRequestFull = null;
                }

                // (4) UdpClient 객체 닫기
                cli.Close();
                StopThread(m_ReceiveThread);

                if (logWriter != null) logWriter.Close();
            }
        } // End of Main

        //delegate void TimerEventFiredDelegate();
        //static void callback_Timer(object state)
        //{
        //    PrintLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        //}

        static void _callback_Timer_Min(object state)
        {
            //Control.BeginInvoke(new TimerEventFiredDelegate(Work));
            // (1) UdpClient 객체 성성. 
            //UdpClient cli = new UdpClient();
            System.Net.IPAddress ipAddr = IPAddress.Parse(deviceIP);
            byte[] byteIpAddr = ipAddr.GetAddressBytes();
            System.Diagnostics.Debug.Assert(byteIpAddr.Length == 4);

            byte[] keepAlivePacketMin = new byte[] 
            { 
                Convert.ToByte('F'), Convert.ToByte('A'), Convert.ToByte('C'), Convert.ToByte('T'), 
                5, 4,
                //10, 56, 35, 100,
                byteIpAddr[0], byteIpAddr[1], byteIpAddr[2], byteIpAddr[3],
                3, 7, //USIM
                Convert.ToByte('0'), Convert.ToByte('1'),Convert.ToByte('2'),Convert.ToByte('3'),Convert.ToByte('4'),
                Convert.ToByte('5'), Convert.ToByte('f')
            };

            //string msg = string.Format("Now is {0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            //byte[] datagram = Encoding.ASCII.GetBytes(msg);// Encoding.Deault: ks_c_5601-1987

            try
            {

                // (2) 데이타 송신
                byte[] sendPacket = new byte[keepAlivePacketMin.Length];
                Array.Copy(keepAlivePacketMin, 0, sendPacket, 0, keepAlivePacketMin.Length);

                if (base64Apply)
                {
                    sendPacket = Encoding.ASCII.GetBytes(Convert.ToBase64String(sendPacket));
                    //PrintLog(Encoding.ASCII.GetString(sendPacket));
                }

                int sentBytes = cli.Send(sendPacket, sendPacket.Length);
                PrintLog(string.Format("\r\n[SND(Min KAM):{0}] {1}:{2} 로 {3} 바이트 전송", DateTime.Now.ToString("MM-dd hh:mm:ss"), serverIP, serverPort, sentBytes));
                //PrintLog(string.Format(string.Format(" 발신메시지: {0} ", Encoding.ASCII.GetString(keepAlivePacketMin))));
            }
            catch (SocketException se)
            {
                PrintLog(string.Format("\r\n[Sock예외발생]: {0}", se.ToString()));
            }
            catch (Exception e)
            {
                PrintLog(string.Format("\r\n[예외발생]: {0}", e.ToString()));
            }
            finally
            {
                // (4) UdpClient 객체 닫기
                //cli.Close();

                //Console.Write("\r\npress any key ..");
                //Console.ReadLine();
            }
        } // End of method (_callback_timer)

        static int TunnelingProcessId = 0;
        static void _callback_Timer_Full(object state)
        {
            //Control.BeginInvoke(new TimerEventFiredDelegate(Work));
            // (1) UdpClient 객체 성성. 
            //UdpClient cli = new UdpClient();
            System.Net.IPAddress ipAddr = IPAddress.Parse(deviceIP);
            byte[] byteIpAddr = ipAddr.GetAddressBytes();
            System.Diagnostics.Debug.Assert(byteIpAddr.Length == 4);

            //string msg = string.Format("Now is {0}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
            //byte[] datagram = Encoding.ASCII.GetBytes(msg);// Encoding.Deault: ks_c_5601-1987
            byte[] keepAlivePacketFull = new byte[] 
            { 
                Convert.ToByte('F'), Convert.ToByte('A'), Convert.ToByte('C'), Convert.ToByte('T'), 
                1, 4,  //모델
                Convert.ToByte('V'), Convert.ToByte('1'), Convert.ToByte('0'), Convert.ToByte('0'), 
                2, 9,  // Serial Num
                Convert.ToByte('S'), Convert.ToByte('I'), Convert.ToByte('M'), Convert.ToByte('-'), Convert.ToByte('T'), 
                Convert.ToByte('I'), Convert.ToByte('2'), Convert.ToByte('B'), Convert.ToByte('1'), 
                3, 7, //USIM
                Convert.ToByte('0'), Convert.ToByte('1'),Convert.ToByte('2'),Convert.ToByte('3'),Convert.ToByte('4'),
                Convert.ToByte('5'), Convert.ToByte('f'),
                4, 15, //IMEI
                Convert.ToByte('3'), Convert.ToByte('5'),Convert.ToByte('8'),Convert.ToByte('7'),Convert.ToByte('7'),
                Convert.ToByte('7'), Convert.ToByte('0'),Convert.ToByte('7'),Convert.ToByte('0'),Convert.ToByte('0'),
                Convert.ToByte('0'), Convert.ToByte('0'),Convert.ToByte('3'),Convert.ToByte('2'),Convert.ToByte('9'),
                5, 4, //DeviceIP
                //10, 56, 35, 100,
                byteIpAddr[0], byteIpAddr[1], byteIpAddr[2], byteIpAddr[3],
                9, 6, //LTE ModuleName
                Convert.ToByte('m'), Convert.ToByte('o'),Convert.ToByte('d'),Convert.ToByte('u'),Convert.ToByte('l'),
                Convert.ToByte('e'),
                0, 0 
            };

            try
            {
                // (2) 데이타 송신
                byte[] sendPacket = new byte[keepAlivePacketFull.Length];
                Array.Copy(keepAlivePacketFull, 0, sendPacket, 0, keepAlivePacketFull.Length);

                if (base64Apply)
                {
                    sendPacket = Encoding.ASCII.GetBytes(Convert.ToBase64String(sendPacket));
                    //PrintLog(Encoding.ASCII.GetString(sendPacket));
                }

                int sentBytes = cli.Send(sendPacket, sendPacket.Length);
                PrintLog(string.Format("\r\n[SND(Full KAM):{0}] {1}:{2} 로 {3} 바이트 전송", DateTime.Now.ToString("MM-dd hh:mm:ss"), serverIP, serverPort, sentBytes));
                //PrintLog(string.Format(" 발신메시지: {0} ", Encoding.ASCII.GetString(keepAlivePacketFull)));

            }
            catch (Exception e)
            {
                PrintLog(string.Format("\r\n[예외발생]: {0}", e.ToString()));
            }

        } // End of method (_callback_timer)

        static void _ReceiveThread()
        {
            // (3) 데이타 수신
            while (!m_IsStop)
            {
                m_ReceiveMRE.WaitOne();
                try
                {
                    //IPEndPoint epRemote = new IPEndPoint(IPAddress.Any, 0);
                    if (localEndPoint == null) continue;

                    IPEndPoint remoteEndPoint = new IPEndPoint(localEndPoint.Address, localEndPoint.Port);
                    byte[] bytes = cli.Receive(ref remoteEndPoint);
                    PrintLog(string.Format("\r\n[RECV:{0}] {1} 로부터 {2} 바이트 수신", DateTime.Now.ToString("MM-dd hh:mm:ss"), remoteEndPoint.ToString(), bytes.Length));

                    KeepAliveMsg recvKeepAlivMsg = null;
                    try
                    {
                        if (base64Apply)
                        {
                            //PrintLog("└ 수신메시지:" + Convert.FromBase64String(Encoding.ASCII.GetString(bytes)));
                            byte[] byteDatas = Convert.FromBase64String(Encoding.ASCII.GetString(bytes));
                            recvKeepAlivMsg = new KeepAliveMsg(byteDatas);
                        }
                        else
                        {
                            //PrintLog("└ 수신메시지: " + Encoding.ASCII.GetString(bytes));//Encoding.UTF8.GetString(bytes));
                            recvKeepAlivMsg = new KeepAliveMsg(bytes);
                        }
                    }
                    catch (Exception e)
                    {
                        PrintLog(string.Format("└[KAM수신] 수신메시지분석 오류발생 - {0}", e.ToString()));
                        continue;
                    }
                    if (recvKeepAlivMsg == null) continue;
                    
                    /// 
                    recvKeepAlivMsg.RecvDateTime = DateTime.Now;
                    recvKeepAlivMsg.RecvIPAddress = remoteEndPoint.Address.ToString();
                    recvKeepAlivMsg.RecvPort = remoteEndPoint.Port;
                    PrintLog(recvKeepAlivMsg.ToString("[KAM수신:" + EnumUtil.GetDescription(recvKeepAlivMsg.SSHTunnelCreateOption) + "]"));

                    /// 프로세스ID값 업데이트
                    Process proc = null;
                    if (TunnelingProcessId > 0)
                    {
                        try
                        {
                            proc = Process.GetProcessById(TunnelingProcessId);
                        }
                        catch (ArgumentException ae)
                        {
                            System.Diagnostics.Debug.Write("[_ReceiveThread()] " + ae.ToString());
                            TunnelingProcessId = 0;
                            proc = null;
                        }
                    }

                    /// 터널Open/Close 요청이 있으면
                    switch (recvKeepAlivMsg.SSHTunnelCreateOption)
                    {
                        case E_SSHTunnelCreateOption.Open:
                            //PrintLog("└ Open요청을 받았으나 아무것도 안하기");
                            /// SSH터널링(putty)이 실행안된상태면 실행
                            if (TunnelingProcessId == 0)
                            {
                                PrintLog(string.Format(" └ SSH터널Open 실행중(port={0}) ...", recvKeepAlivMsg.SSHTunnelPort));
                                // putty 프로파일(local40101) 설정후 테스트할 것: TACTKeepAliveClient_putty설정_local40101_xx.png 캡춰 참고
                                // 터널Open 확인법> netstat -ano |findstr ":40"
                                //TunnelingProcessId = RunProcess("C:\\Program Files\\PuTTY0.71\\putty.exe", "-load \"local40101\" -l ssh-tunnel -pw aug@201908");
                                //TunnelingProcessId = RunProcess("C:\\Program Files\\OpenSSH\\ssh.exe", " -R 40101:127.0.0.1:23 ssh-tunnel@127.0.0.1 -p 40002");
                                TunnelingProcessId = RunProcess("C:\\Program Files\\OpenSSH\\ssh.exe", " -R " + recvKeepAlivMsg.SSHTunnelPort + ":" + telnetServerIP + ":" + telnetServerPort + " " + recvKeepAlivMsg.SSHUserID + "@" + recvKeepAlivMsg.SSHServerDomain + " -p " + recvKeepAlivMsg.SSHPort);
                            }
                            else
                            {
                                PrintLog(string.Format(" ssh가 이미 실행중..ProcessId={0}", TunnelingProcessId, proc.ProcessName));
                            }
                            break;

                        case E_SSHTunnelCreateOption.Close:
                            //PrintLog("└ Close요청을 받았으나 아무것도 안하기");
                            /// SSH터널링(putty)이 실행안된상태면 실행
                            if (TunnelingProcessId == 0)
                            {
                                PrintLog("└ ssh 미실행중..");
                            }
                            else
                            {
                                PrintLog(string.Format("└ ssh 강제종료 - 터널Close [ProcessId={0}]", TunnelingProcessId));
                                Process tunnelProc = Process.GetProcessById(TunnelingProcessId);
                                if (tunnelProc != null)
                                {
                                    tunnelProc.Kill();
                                }
                            }
                            break;

                        default:
                            break;
                    } // End of switch


                    // 수신데이타 파일로그 생성
                    //File.WriteAllBytes(String.Format("UDPEchoClient_RCV_{0}.log", DateTime.Now.ToString("yyyyMMdd_hhmmss")), bytes);
                }
                catch (SocketException e)
                {
                    PrintLog(string.Format("\r\n[Sock예외발생]: {0}", e.ToString()));
                    System.Diagnostics.Debug.WriteLine(string.Format("\r\n[Sock예외발생]: {0}", e.ToString()));
                }
                catch (Exception e)
                {
                    PrintLog(string.Format("\r\n[예외발생]: {0}", e.ToString()));
                    System.Diagnostics.Debug.WriteLine(string.Format("\r\n[Sock예외발생]: {0}", e.ToString()));
                }

                Thread.Sleep(500);
            } // End of while

        }
        
        /// <summary>
        /// Process Kill
        /// 참고: http://www.gisdeveloper.co.kr/?p=2181
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="Args"></param>
        /// <returns>프로세스ID</returns>
        private static int RunProcess(String FileName, String Args)
        {
            Process p = new Process();

            p.StartInfo.FileName = FileName;
            p.StartInfo.Arguments = Args;

            p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;

            if (p.Start())
            {
                return p.Id; // ProcessId
            }
            else
            {
                return 0;
            }
            //return p.Start();
            //p.WaitForExit();
            //return p.ExitCode;
        }

        /// <summary>
        /// 쓰레드를 강제 종료합니다.
        /// </summary>
        /// <param name="aThread"></param>
        public static void StopThread(Thread aThread)
        {
            if (aThread == null) return;

            //종료대기
            aThread.Join(100);
            if (aThread.IsAlive)
            {
                try
                {
                    //강제종료
                    aThread.Abort();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.Write("[StopThread()] " + e.ToString());
                    Thread.ResetAbort();
                }
            }
            aThread = null;
        }

        public static void PrintLog(string aText)
        {
            if (logWriter == null)
            {
                string LogFilePath = System.IO.Directory.GetCurrentDirectory() + "\\Log";
                if (!Directory.Exists(LogFilePath))
                {
                    //로그 저장 폴더가 없으면 생성합니다.
                    Directory.CreateDirectory(LogFilePath);
                }

                logWriter = System.IO.File.CreateText(LogFilePath + "\\TACTKeepAliveClient_" + DateTime.Now.ToString("yyyyMMdd_HHMM") + ".log");
            }

            Console.WriteLine(aText);
            logWriter.WriteLine(aText);
        }
      

    } // End of class

}
