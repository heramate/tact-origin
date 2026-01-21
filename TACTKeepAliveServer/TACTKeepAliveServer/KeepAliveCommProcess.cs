using System;
using System.Collections.Generic; //Dictionary<>
using System.Collections;   //Queue<T>
using MKLibrary.MKNetwork;  //MKUdpSocket
using System.Threading;     //Thread
using System.Net; //IPAddress
using RACTCommonClass; // GlobalClass.PrintLog
using System.Text; //Encoding
using System.IO;
using System.Collections.Concurrent; //File

namespace TACT.KeepAliveServer
{
    public class KeepAliveCommProcess : IDisposable
    {
        #region KeepAliveMessage 수신 ─────────────────────────────────────────
        /// <summary>
        /// KeepAlive 통신을 위한 소켓
        /// </summary>
        private MKUdpSocket m_Socket = null;
        /// <summary>
        /// 로컬 IP주소 입니다.
        /// </summary>
        private string m_LocalIPAddress = IPAddress.Any.ToString(); //기본값=IPAddress.Any.ToString()
        /// <summary>
        /// 서버의 수신포트번호 입니다.
        /// </summary>
        private int m_ListenPort = 40001;

        /// <summary>
        /// KeepAlive메시지 수신 스레드
        /// </summary>
        private Thread m_ReceiveThread = null;
        /// <summary>
        /// 수신 메시지(Queue) 처리 스레드
        /// </summary>
        private Thread m_ReceiveProcessThread = null;
        /// <summary>
        /// 수신한 패킷 저장 큐
        /// </summary>
        private Queue m_ReceiveQueue = new Queue();//Item: KeepAlivePacket
        /// <summary>
        /// KAM 수신 대기 입니다.
        /// </summary>
        private ManualResetEvent m_ReceiveMRE = new ManualResetEvent(false);
        #endregion KeepAliveMessage 수신 ──────────────────────────────────────


        #region KeepAliveMessage 발송  ────────────────────────────────────────
        /// <summary>
        /// [장비IP]별로 전송(reply)할 Open/Close요청 메시지
        /// - 장비별로 전송할 메시지는 1건으로 유지한다.
        /// - 장비별 요청이 중복되는 경우 Close요청보다 Open요청을 우선시한다.
        /// </summary>
        //private Dictionary<string, KeepAliveMsg> m_SendReply = new Dictionary<string, KeepAliveMsg>();//Key=장비IP
        private ConcurrentDictionary<string, KeepAliveMsg> m_SendReply = new ConcurrentDictionary<string, KeepAliveMsg>();//Key=장비IP
        /// 보낼 메시지 처리 스레드
        /// </summary>
        private Thread m_SendProcessThread = null;
        /// <summary>
        /// KAM 송신 대기 입니다.
        /// </summary>
        private ManualResetEvent m_SendMRE = new ManualResetEvent(false);
        
        /// <summary>
        /// 장비(c-RPCS)별 최신 KAM정보
        /// </summary>
        private KeepAliveManager m_KAMManager = new KeepAliveManager();//Key: 장비IP(c-RPCS), Value: LTE_NE의 최신정보(full)

        #endregion KeepAliveMessage 발송  ─────────────────────────────────────



        ///--------------------------------------------------------------------
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public KeepAliveCommProcess(int aListenPort)
        {
            m_ListenPort = aListenPort;
        }

        /// <summary>
        /// 관련 리소스를 해제합니다.
        /// </summary>
        public void Dispose()
        {
            Stop();
        }

        /// <summary>
        /// 종료 처리 합니다.
        /// </summary>
        internal void Stop()
        {
            /// KAM(UDP) 수신 중지
            GlobalClass.StopThread(m_ReceiveProcessThread);
            lock (m_ReceiveQueue.SyncRoot)
            {
                if (m_ReceiveQueue != null) m_ReceiveQueue.Clear();
            }
            GlobalClass.StopThread(m_ReceiveThread);
            if (m_Socket != null)
            {
                m_Socket.Dispose();
                m_Socket = null;
            }
            /// KAM수신내역 삭제
            lock (m_KAMManager)
            {
                m_KAMManager.ClearAll();
            }
            
            /// KAM발송 처리 중단
            // (m_SendReply) 
            { m_SendReply.Clear(); }
            GlobalClass.StopThread(m_SendProcessThread);
        }

        /// <summary>
        /// KeepAlive 서버를 시작시키고 감시(listen)합니다.
        /// </summary>
        public void Start()
        {
            try
            {
                // KeepAlive수신
                m_ReceiveThread = new Thread(new ThreadStart(_Listen));
                m_ReceiveThread.Start();

                // KeepAlive수신 큐 처리
                m_ReceiveProcessThread = new Thread(new ThreadStart(_ProcessReceiveKeepAlive));
                m_ReceiveProcessThread.Start();

                // KeepAlive송신 처리
                m_SendProcessThread = new Thread(new ThreadStart(_ProcessSendKeepAliveReply));
                m_SendProcessThread.Start();

                // 데몬요청 처리 스레드
                //m_ProcessDaemonRequest = new Thread(new ThreadStart(_ProcessDaemonRequest));
                //m_ProcessDaemonRequest.Start();
            }
            catch (Exception ex)
            {
                GlobalClass.PrintLogException("[KAM처리자] KeepAliveCommProcess.StopThreads :", ex);
            }
        }

        /// <summary>
        /// KeepAlive 서버를 시작시키고 감시(listen)합니다.
        /// </summary>
        private void _Listen()
        {
            while (!GlobalClass.m_IsServerStop)
            {
                if (m_Socket != null)
                {
                    Thread.Sleep(1);
                    continue;
                }

                /// 소켓 생성 및 이벤트 처리기 연결
                E_SocketError sockError = E_SocketError.NoError;
                m_Socket = new MKUdpSocket();
                m_Socket.DataReceived += new MKDataReceivedEventHandler(m_Socket_DataReceived);
                m_Socket.DataSended += new EventHandler(m_Socket_DataSended);
                m_Socket.SocketError += new MKSocketErrorEventHandler(m_Socket_SocketError);


                /// 수신포트 Listen
                if (string.IsNullOrEmpty(m_LocalIPAddress))
                {
                    sockError = m_Socket.StartListen(m_ListenPort); //IPAddress.Any
                }
                else
                {
                    sockError = m_Socket.StartListen(m_LocalIPAddress, m_ListenPort);
                }

                if (sockError != E_SocketError.NoError)
                {
                    GlobalClass.PrintLog(E_FileLogType.Error, string.Format("[KAM처리자] KeepAlive 소켓 오픈 실행 실패! ErrorCode={0}", sockError));
                    continue;
                }

                /// 데이터 수신시작
                m_Socket.StartReceive();

                //m_ReceiveMRE.Set();       //계속
                //m_ReceiveMRE.Reset();     //정지
                //m_ReceiveMRE.WaitOne();	//신호대기 - Set()이면 true / Reset()이면 false
            } // End of while
        }


        /// <summary>
        /// 소켓 오류 이벤트 처리기
        /// </summary>
        private void m_Socket_SocketError(object sender, MKSocketErrorEventArgs e)
        {
            //System.Net.Sockets.SocketError e.SocketError
            GlobalClass.PrintLogError("[KeepAliveCommProc.m_Socket_SocketError] 소켓오류 - " + e.ToString());
        }

        /// <summary>
        /// 데이터 전송됨 이벤트 처리기
        /// </summary>
        private void m_Socket_DataSended(object sender, EventArgs e)
        {
            Console.WriteLine("[KAM처리자] m_Socket.DataSended :" + e.ToString());
        }

        /// <summary>
        /// 데이터 수신 이벤트 처리기
        /// </summary>
        private void m_Socket_DataReceived(object sender, MKDataReceivedEventArgs e)
        {
            if (e == null) return;

            try
            {
                byte[] tPacket = new byte[e.ReadCount];
                Array.Copy(e.SocketData.Buffers, tPacket, e.ReadCount);
                m_Socket.StartReceive(e.SocketData);

                GlobalClass.PrintLogInfo(string.Format("[KAM처리자] [KAM_RCV:{0}] {1}:{2} 로부터 {3} 바이트 수신", DateTime.Now.ToString("MM-dd hh:mm:ss"), e.SenderIPAddress, e.PortNumber, tPacket.Length), true);
                lock (m_ReceiveQueue.SyncRoot)
                {
                    //if (m_ReceiveQueue.Count < m_MaxQueueCount)
                    //{
                    m_ReceiveQueue.Enqueue(new KeepAlivePacket(tPacket, e.SenderIPAddress, e.PortNumber));
                    m_ReceiveMRE.Set();
                    //}
                }
            }
            catch (Exception ex)
            {
                GlobalClass.PrintLogException("[KAM처리자] KeepAliveCommProcess.m_Socket_DataReceived : ", ex);
                m_Socket.Dispose();
                m_Socket = null;
            }
        }

        /// <summary>
        /// 수신큐의 Keep-Alive패킷을 꺼내서 처리합니다.
        /// </summary>
        private void _ProcessReceiveKeepAlive()
        {
            KeepAlivePacket tKAMPacket = null;

            while (!GlobalClass.m_IsServerStop)
            {
                try
                {
                    if (m_ReceiveQueue.Count == 0)
                    {
                        m_ReceiveMRE.Reset();
                        m_ReceiveMRE.WaitOne();
                    }

                    lock (m_ReceiveQueue.SyncRoot)
                    {
                        tKAMPacket = (m_ReceiveQueue.Count > 0) ? (KeepAlivePacket)m_ReceiveQueue.Dequeue() : null;
                    }
                    if (tKAMPacket == null) continue;

                    //------------------------------------------------------------------
                    // 패킷 디코딩 (Base64 적용 및 TLV 파싱)
                    KeepAliveMsg recvKeepAlive = null;
                    recvKeepAlive = DecodeKeepAlivePacket(tKAMPacket);
                    if (recvKeepAlive == null) continue;

                    GlobalClass.PrintLogInfo(recvKeepAlive.ToString(string.Format("◀[RECV:KAM{0}", recvKeepAlive.IsFullMessage() ? "(Full)" : "(Min)")));
                    //if (recvKeepAlive.IsFullMessage())
                     //   GlobalClass.PrintLogInfo(recvKeepAlive.ToString(string.Format("◀[RECV:KAM{0}", "(Full)")));
                    //[디버깅용] 수신한 바이트데이터 file export
                    //System.IO.File.WriteAllBytes(String.Format("KeepAliveCommProcess_RCV_{0}.log", DateTime.Now.ToString("yyyyMMdd_hhmmss")), tKAPacket.Packet);


                    ///-----------------------------------------------------------------
                    /// KAM수신 DB로그 저장
                    GlobalClass.m_DBLogProcess.AddLog(new DBLogKeepAlive(E_DBLogType.KeepAliveLog, recvKeepAlive));

                    ///-----------------------------------------------------------------
                    /// 장비별 KAM 정보 DB업데이트 및 최신 정보 조회
                    bool isFullKAM = recvKeepAlive.IsFullMessage();
                    DBWorker.Update_LTE_NE(ref recvKeepAlive);
                    
                    ///-----------------------------------------------------------------
                    /// 장비별 KAM 정보 업데이트 (LTE세션정보)
                    /// - DBWorker.Update_LTE_NE 실행후 
                    m_KAMManager.Add(recvKeepAlive);

                    ///-----------------------------------------------------------------
                    /// Request[Full] KAM이면 응답 추가
                    if (isFullKAM)
                    {
                        AddKeepAliveReply(recvKeepAlive.DeviceIP, E_SSHTunnelCreateOption.Unknown, 0);
                    }
                }
                catch (Exception e)
                {
                    GlobalClass.PrintLogException("[KeepAliveCommProcess._ProcessReceiveKeepAlive] 오류발생: ", e);
                }

            } // End of while
        }


        /// <summary>
        /// 장비(Cat.M1)에 요청을 전송합니다.
        /// 2019.11.11 터널Close요청 재발송 기능 추가 - 장비측 요청(11/8판교회의) / 설정값 추가:TunnelRequestSendPeriodSeconds, TunnelRequestCount
        /// </summary>
        private void _ProcessSendKeepAliveReply()
        {
            List<String> removeList = new List<String>();

            List<KeepAliveMsg> removeLists = new List<KeepAliveMsg>();

            KeepAliveMsg removeMsg = null;

            while (!GlobalClass.m_IsServerStop)
            {
                try
                {
                    if (m_SendReply.Count == 0)// || m_KAMManager.GetCount() == 0)
                    {
                        m_SendMRE.Reset();
                        m_SendMRE.WaitOne();
                    }
                    GlobalClass.PrintLogInfo(string.Format("[KeepAliveCommProcess._ProcessSendKeepAliveReply TEST_______0] "));
                    lock (m_SendReply)
                    {
                        if (m_SendReply.Count <= 0)
                            continue;

                        foreach (string deviceIP in m_SendReply.Keys)
                        {
                            KeepAliveMsg sendMsg = m_SendReply[deviceIP];

                            /// Close요청인 경우 발송주기 체크
                            if (sendMsg.SSHTunnelCreateOption == E_SSHTunnelCreateOption.Close
                                && ((TimeSpan)DateTime.Now.Subtract(sendMsg.SentDateTime)).TotalSeconds < GlobalClass.m_SystemInfo.TunnelRequestSendPeriodSeconds)
                            {
                                GlobalClass.PrintLogInfo(string.Format("[KeepAliveCommProcess._ProcessSendKeepAliveReply TEST_______1] {0}, {1}. {2}. {3}", deviceIP, ((TimeSpan)DateTime.Now.Subtract(sendMsg.SentDateTime)).TotalSeconds, sendMsg.SSHTunnelCreateOption, sendMsg.SentCount));
                                continue;
                            }

                            /// 최신 KAM정보(발신처)를 가져온다.
                            KeepAliveMsg replyMsg = m_KAMManager.CreateKeepAliveReplyMessage(sendMsg.DeviceIP, sendMsg.SSHTunnelCreateOption, sendMsg.SSHTunnelPort);
                            if (replyMsg == null)
                            {
                                /// 수신한 KAM이 없으면 대기
                                /// Full,Open,Close 메시지 1분이상(6000 * 10) 시도하면 m_SendReply에서 삭제하도록 처리
                                
                                if (sendMsg.SentCount > 6000)
                                {
                                    m_SendReply.TryRemove(deviceIP, out removeMsg);
                                    GlobalClass.PrintLogInfo(string.Format("▶[SEND:KAM({0})] KAM 1분이상 미수신 장비에 발송 요청 메시지 삭제 IP:{1} ", EnumUtil.GetDescription(sendMsg.SSHTunnelCreateOption), deviceIP));
                                }
                                sendMsg.SentCount++;
                                
                                //GlobalClass.PrintLogInfo(string.Format("▶[SEND:KAM({0})]KAM 미수신 장비에 발송 요청 메시지 삭제 IP:{1} ", EnumUtil.GetDescription(sendMsg.SSHTunnelCreateOption), deviceIP));
                                //m_SendReply.TryRemove(deviceIP, out removeMsg);

                                continue;
                            }

                            /// KAM발송
                            byte[] byteDatas = KeepAliveReplyToByteDatas(replyMsg);
                            if (byteDatas == null) continue;
                            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(replyMsg.SendIPAddress), replyMsg.SendPort);
                            m_Socket.SendDataTo(byteDatas, (EndPoint)remoteEP);
                            sendMsg.SentDateTime = DateTime.Now;
                            replyMsg.SentDateTime = DateTime.Now;
                            sendMsg.SentCount++;
                            GlobalClass.PrintLogInfo(replyMsg.ToString(string.Format("▶[SEND:KAM({0})] ", EnumUtil.GetDescription(replyMsg.SSHTunnelCreateOption))));
                            
                            /// 터널상태 갱신
                            if (replyMsg.SSHTunnelCreateOption == E_SSHTunnelCreateOption.Open)
                            {
                                GlobalClass.PrintLogInfo(string.Format("[KeepAliveCommProcess._ProcessSendKeepAliveReply TEST_______2] {0}, {1}. {2}. {3}", deviceIP, ((TimeSpan)DateTime.Now.Subtract(sendMsg.SentDateTime)).TotalSeconds, sendMsg.SSHTunnelCreateOption, sendMsg.SentCount));
                                //GlobalClass.m_TunnelManager.UpdateState(replyMsg.SSHTunnelPort, E_TunnelState.WaitingOpen);
                                GlobalClass.m_TunnelManager.AddTunnelPortState(replyMsg.SSHTunnelPort, E_TunnelState.WaitingOpen);
                            }
                            else if (replyMsg.SSHTunnelCreateOption == E_SSHTunnelCreateOption.Close)
                            {
                                GlobalClass.PrintLogInfo(string.Format("[KeepAliveCommProcess._ProcessSendKeepAliveReply TEST_______3] {0}, {1}. {2}. {3}", deviceIP, ((TimeSpan)DateTime.Now.Subtract(sendMsg.SentDateTime)).TotalSeconds, sendMsg.SSHTunnelCreateOption, sendMsg.SentCount));
                                //GlobalClass.m_TunnelManager.UpdateState(replyMsg.SSHTunnelPort, E_TunnelState.WaitingClose);
                                GlobalClass.m_TunnelManager.AddTunnelPortState(replyMsg.SSHTunnelPort, E_TunnelState.WaitingClose);
                            }

                            /// 발송완료한 메시지는 삭제한다. 단,Close요청은 발송 횟수 체크
                            if (replyMsg.SSHTunnelCreateOption != E_SSHTunnelCreateOption.Close
                                || sendMsg.SentCount >= GlobalClass.m_SystemInfo.TunnelRequestCount)
                            {
                                removeList.Add(deviceIP);
                            }

                            /// KAM발송 DB로그 저장
                            GlobalClass.m_DBLogProcess.AddLog(new DBLogKeepAlive(E_DBLogType.KeepAliveLog, replyMsg));
                            GlobalClass.PrintLogInfo(string.Format("[KeepAliveCommProcess._ProcessSendKeepAliveReply TEST_______4] {0}", deviceIP));
                        } // End of foreach (m_SendReply)

                        /// 처리완료 건은 삭제
                        //foreach (String deviceIP in removeList) m_SendReply.Remove(deviceIP);

                        //KeepAliveMsg removeMsg = null;
                        foreach (String deviceIP in removeList) 
                        {
                            m_SendReply.TryRemove(deviceIP, out removeMsg);
                        }

                        removeList.Clear();
                        GlobalClass.PrintLogInfo(string.Format("[KeepAliveCommProcess._ProcessSendKeepAliveReply TEST_______5] "));
                    } // End of lock (m_SendReply)
                }
                catch (Exception e)
                {
                    GlobalClass.PrintLogException("[KeepAliveCommProcess._ProcessSendKeepAliveReply]", e);
                }
                Thread.Sleep(10);
            } // End of while (!GlobalClass.m_IsServerStop)
        }

        /// <summary>
        /// 발송KAM 을 byte array데이터로 변경한다.
        /// </summary>
        /// <param name="replyMsg"></param>
        /// <returns></returns>
        private byte[] KeepAliveReplyToByteDatas(KeepAliveMsg replyMsg)
        {
            byte[] byteDatas = null;
            switch (replyMsg.SSHTunnelCreateOption)
            {
                case E_SSHTunnelCreateOption.Unknown:
                    byteDatas = replyMsg.Encode(TlvDef.s_ReplyStructureMin, GlobalClass.m_SystemInfo.KeepAliveBase64Encode);
                    break;
                case E_SSHTunnelCreateOption.Open:
                case E_SSHTunnelCreateOption.Close:
                    byteDatas = replyMsg.Encode(TlvDef.s_ReplyStructureFull, GlobalClass.m_SystemInfo.KeepAliveBase64Encode);
                    break;
                default:
                    Util.Assert(false, string.Format("[KAM처리자] _ProcessSendKeepAlive : 지원하지 않는 터널링옵션입니다. SSHTunnelCreateOption={0}", EnumUtil.GetDescription(replyMsg.SSHTunnelCreateOption)));
                    break;
            }

            return byteDatas;
        }

        /// <summary>
        /// 처리중인 데몬요청을 모두 삭제한다.
        /// </summary>
        public void ClearAllRequest()
        {
            m_SendReply.Clear();
            m_KAMManager.ClearAll();
        }

        public void RemoveKeepAliveReply(string aDeviceIP)
        {
            Util.Assert(!string.IsNullOrEmpty(aDeviceIP), "[KeepAliveCommProcess.AddKeepAliveReply] 장비IP 가 없습니다.");
            KeepAliveMsg removeMsg = null;
            lock (m_SendReply)
            {
                /*
                if (m_SendReply.ContainsKey(aDeviceIP))
                {
                    m_SendReply.Remove(aDeviceIP);
                }
                */
                m_SendReply.TryRemove(aDeviceIP, out removeMsg);
            }
        }


        /// <summary>
        /// KeepAlive 발송(응답)할 메시지에 추가
        /// (KAM발송종류=터널Close요청 > 터널Open요청 > 발송KAM(수신확인) - 발송우선순위별)
        /// </summary>
        /// <param name="aDeviceIP">장비IP</param>
        /// <param name="aTunnelOption">요청종류(터널Open/Close)</param>
        /// <param name="aTunnelPort">터널포트(터널Close요청시 값 사용)</param>
        public void AddKeepAliveReply(string aDeviceIP, E_SSHTunnelCreateOption aTunnelOption, int aTunnelPort = 0)
        {
            Util.Assert(!string.IsNullOrEmpty(aDeviceIP), "[KeepAliveCommProcess.AddKeepAliveReply] 장비IP 가 없습니다.");
            if (string.IsNullOrEmpty(aDeviceIP)) return;

            lock (m_SendReply)
            {
                KeepAliveMsg newReplyMsg = new KeepAliveMsg(aDeviceIP, aTunnelOption, aTunnelPort);

                bool bAddReply = false;
                KeepAliveMsg oldReplyMsg = null;

                if (!m_SendReply.TryGetValue(aDeviceIP, out oldReplyMsg) || oldReplyMsg == null)
                {
                    bAddReply = true;
                }
                else
                {
                    if (oldReplyMsg.SSHTunnelCreateOption == E_SSHTunnelCreateOption.Unknown)
                    {
                        bAddReply = true;
                    }
                    else
                    {
                        /// Close요청이 대기중, Open요청 추가인 경우 Open요청 (Open요청이 상위 우선순위)
                        if (newReplyMsg.SSHTunnelCreateOption != oldReplyMsg.SSHTunnelCreateOption
                            && newReplyMsg.SSHTunnelCreateOption == E_SSHTunnelCreateOption.Open)
                        {
                            bAddReply = true;
                        }
                        else
                        {
                            // 동일한 요청이 이미 대기중이면
                            // 또는 Open요청이 대기중인 상태에서 Close요청이 온경우 추가안함 (Open요청이 상위 우선순위)
                            bAddReply = false;
                        }
                    }
                }

                if (bAddReply)
                {
                    GlobalClass.PrintLogInfo(string.Format("[KAM처리자({0})] 발송메시지 추가: DeviceIP={1}, Open/Close요청={2}, 터널포트={3}", 
                                            EnumUtil.GetDescription(aTunnelOption), aDeviceIP, aTunnelOption.ToString(), aTunnelPort));

                    newReplyMsg.TimestampWaiting = DateTime.Now; //발송대기 시작시각
                    m_SendReply[aDeviceIP] = newReplyMsg; //새 요청 추가 or 업데이트
                    m_SendMRE.Set();
                }
            } // End of lock (m_SendReply)
        }

        /// <summary>
        /// KeepAlive 발송(응답)할 메시지에 삭제
        /// </summary>
        /// <param name="aDeviceIP"></param>
        /// <param name="aTunnelOption"></param>
        /// <param name="aTunnelPort"></param>
        public void RemoveKeepAliveReply(string aDeviceIP, E_SSHTunnelCreateOption aTunnelOption, int aTunnelPort = 0)
        {
            Util.Assert(!string.IsNullOrEmpty(aDeviceIP), "[KeepAliveCommProcess.RemoveKeepAliveReply] 장비IP 가 없습니다.");
            if (string.IsNullOrEmpty(aDeviceIP)) return;

            lock (m_SendReply)
            {
                KeepAliveMsg oldReplyMsg = null;
                KeepAliveMsg removeMsg = null;
                if (m_SendReply.TryGetValue(aDeviceIP, out oldReplyMsg))
                {
                    Util.Assert(oldReplyMsg != null, "[KeepAliveCommProcess.RemoveKeepAliveReply] KeekAliveMsg is null.");
                    if (oldReplyMsg.SSHTunnelPort == aTunnelPort && oldReplyMsg.SSHTunnelCreateOption == aTunnelOption)
                    {
                        GlobalClass.PrintLogInfo(string.Format("[KAM처리자({0})] 발송메시지 삭제: DeviceIP={1}, Open/Close요청={2}, 터널포트={3}",
                                                EnumUtil.GetDescription(aTunnelOption), aDeviceIP, aTunnelOption.ToString(), aTunnelPort));
                        //m_SendReply.Remove(aDeviceIP);
                        m_SendReply.TryRemove(aDeviceIP, out removeMsg);
                    }
                }
            }
        }

        /// <summary>
        /// KeepAlive를 기다리는 터널요청(Open/Close)건 정보
        /// </summary>
        /// <returns>KeepAliveMsg리스트</returns>
        public ArrayList GetKeepAliveWatingList()
        {
            ArrayList replyList = new ArrayList();

            foreach (string deviceIP in m_SendReply.Keys)
            {
                KeepAliveMsg replyMsg = new KeepAliveMsg(m_SendReply[deviceIP]);
                //if (replyMsg.SSHTunnelCreateOption != E_SSHTunnelCreateOption.Open) continue;
                replyList.Add(replyMsg);
            }

            return replyList;
        }

        /// <summary>
        /// 수신 Keep-Alive 메시지데이터에 Base64 적용 
        /// </summary>
        /// <param name="tKAPacket"></param>
        /// <returns>처리결과 true(성공)=정상패킷,false(실패)=비정상패킷으로 진행불가</returns>
        private KeepAliveMsg DecodeKeepAlivePacket(KeepAlivePacket tKAMPacket)
        {
            KeepAliveMsg result = null;
            if (GlobalClass.m_SystemInfo.KeepAliveBase64Encode)
            {
                if (tKAMPacket == null || tKAMPacket.Packet == null || tKAMPacket.Packet.Length < 1)
                {
                    throw new Exception("[KAM처리자] [KAM수신] 수신한 KeepAlive 데이터가 null입니다.");
                }

                //byte[] byteDatas = Routrek.Toolkit.Base64.Decode(tKAPacket.Packet);
                byte[] byteDatas = Convert.FromBase64String(Encoding.ASCII.GetString(tKAMPacket.Packet));
                //GlobalClass.PrintLogInfo(string.Format("■ Base64 디코딩 처리({0}→{1} bytes) : {2} → {3}",
                //                            tKAMPacket.Packet.Length, byteDatas.Length,
                //                            Encoding.ASCII.GetString(tKAMPacket.Packet), Encoding.ASCII.GetString(byteDatas)), false);
                if (byteDatas == null || byteDatas.Length < 1)
                {
                    throw new Exception(String.Format("[KAM처리자] [KeepAliveCommProcess.DecodeKeepAlivePacket] Base64디코딩 실패(byteDatas is null) Sender={0}:{1}", tKAMPacket.SenderAddress, tKAMPacket.PortNumber));
                }

                result = new KeepAliveMsg(byteDatas, tKAMPacket.Time, tKAMPacket.SenderAddress, tKAMPacket.PortNumber);
            }
            else
            {
                result = new KeepAliveMsg(tKAMPacket.Packet, tKAMPacket.Time, tKAMPacket.SenderAddress, tKAMPacket.PortNumber);
            }
            return result;
        }

        /// <summary>
        /// 데몬에 보낼 응답 추가
        /// </summary>
        /// <param name="aReq">데몬에서 받은 요청데이터</param>
        /// <param name="aTunnelIP">터널IP(장비LTE접속을 위한 IP)</param>
        /// <param name="aTunnelPort">터널포트(장비LTE접속을 위한 포트)</param>
        /// <param name="aErrorInfo">데몬요청 처리오류시 오류정보</param>
        private void ResponseToDaemon(RequestCommunicationData aReq, string aTunnelIP, ushort aTunnelPort, ErrorInfo aErrorInfo)
        {
            GlobalClass.m_DaemonCommProcess.AddResult(aReq, aTunnelIP, aTunnelPort, aErrorInfo);
        }

    } // End of class KeepAliveCommProcess
} // End of namespace