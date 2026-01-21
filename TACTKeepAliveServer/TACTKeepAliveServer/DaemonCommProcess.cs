using System;
using System.Collections.Generic;//Queue<>
using System.Collections; //Hashtable
//using System.Linq;
//using System.Text;
using MKLibrary.MKNetwork;
using System.Threading;
using RACTCommonClass; //RemoteClientMethod

namespace TACT.KeepAliveServer
{
	/// <summary>
	/// 데몬과의 통신 처리 프로세스 입니다.
	/// </summary>
    public class DaemonCommProcess : IDisposable
    {
        #region 변수 선언, 생성자, 소멸자 부분 입니다.
        /// <summary>
        ///데몬 원격 통신을 위한 Gateway
        /// </summary>
        private MKRemote m_RemoteGateway = null;
        /// <summary>
        /// 데몬의 요청을 저장할 큐
        /// </summary>
        private Queue<RequestCommunicationData> m_RequestQueue = new Queue<RequestCommunicationData>();
        /// <summary>
        /// 데몬요청  대기 입니다.
        /// </summary>
        private ManualResetEvent m_RequestQueueMRE = new ManualResetEvent(false);
        

        /// <summary>
        /// 데몬의 요청을 처리하는 스레드
        /// </summary>
        private Thread m_RequestProcessThread = null;
        /// <summary>
        /// 데몬에 응답결과 큐
        /// </summary>
        //private Queue<ResultCommunicationData> m_ResultQueue = new Queue<ResultCommunicationData>();
        /// <summary>
        /// ClientID별 응답 데이터 큐
        /// (Key: 데몬이 TACTServer에 접속하며 발급받은 DaemonID값)
        /// </summary>
        //private Dictionary<int, Queue<ResultCommunicationData>> m_ResultQueue = null;
        //private UserInfoCollection m_UserInfoList = new UserInfoCollection();
        //private DaemonManager m_DaemonManager = new DaemonManager();
        private Dictionary<int, DaemonProcessInfo> m_DaemonResultQueue = new Dictionary<int, DaemonProcessInfo>();

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public DaemonCommProcess()
        {
            //m_RequestQueue = new Queue<RequestCommunicationData>();
            //m_ResultQueue = new Queue<ResultCommunicationData>();
            //m_ResultQueue = new Dictionary<int, Queue<ResultCommunicationData>>();
        }

        /// <summary>
        /// 관련 리소스를 해제합니다.
        /// </summary>
        public void Dispose()
        {
            Stop();
        }
        #endregion //변수 선언, 생성자, 소멸자 부분 입니다.


        /// <summary>
        /// 프로세스를 시작합니다.
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            int tCount = 0;
            string tResult = string.Empty;
            RemoteClientMethod tRemoteMethod = null;
            try
            {
                m_RemoteGateway = new MKRemote(E_RemoteType.TCPRemote, GlobalClass.m_SystemInfo.DServerIP, GlobalClass.m_SystemInfo.DServerPort, GlobalClass.m_SystemInfo.DServerChannel);
                while (tCount < 10)
                {
                    if (m_RemoteGateway.StartRemoteServer(out tResult) != E_RemoteError.Success)
                    {
                        GlobalClass.PrintLogError(string.Concat("[데몬통신자]데몬과 통신용 채널을 생성할 수 없습니다. : ", tResult));
                        Thread.Sleep(3000);
                        tCount++;
                    }
                    else
                        break;
                }

                if (m_RemoteGateway == null)
                {
                    GlobalClass.PrintLogError(string.Format("[데몬통신자]데몬 IP: {0}  PortNo: {1}  ChannelName: {2}에 연결 할 수 없습니다.", GlobalClass.m_SystemInfo.DServerIP, GlobalClass.m_SystemInfo.DServerPort, GlobalClass.m_SystemInfo.DServerChannel));
                    return false;
                }

                //
                m_RequestProcessThread = new Thread(new ThreadStart(_ProcessDaemonRequest));
                m_RequestProcessThread.Start();

                //Daemon 원격 메소드를 설정합니다.
                tRemoteMethod = new RemoteClientMethod();
                tRemoteMethod.SetRequestHandler(DaemonRequestReceiver);
                tRemoteMethod.SetResultHandler(DaemonResultSender);
                //tRemoteMethod.SetDaemonConnectHandler(DaemonConnect);
                m_RemoteGateway.ServerObject = tRemoteMethod;
                GlobalClass.PrintLogInfo(string.Format("[데몬통신자] 데몬통신용 리모트채널이 생성되었습니다. IP: {0}  Port: {1}  ChannelName: {2}", GlobalClass.m_SystemInfo.DServerIP, GlobalClass.m_SystemInfo.DServerPort, GlobalClass.m_SystemInfo.DServerChannel));

                return true;
            }
            catch (Exception ex)
            {
                GlobalClass.PrintLogException("[데몬통신자] Start() - Exception!: \r\n", ex);
                return false;
            }
        }

        /// <summary>
        /// 데몬 접속 요청을 처리 합니다.
        /// </summary>
        /// <param name="aClientID"></param>
        //private byte[] DaemonConnect(string aIP, int aPort, string aChannelName)
        //{
        //    GlobalClass.PrintLogInfo(string.Format("[데몬통신자] 데몬 접속 요청 수신 DaemonAddress={0}:{1}, ChannelName={2}"));

        //    DaemonLoginResultInfo tLoginResult = new DaemonLoginResultInfo();
        //    DaemonProcessInfo tAlreadyLoginInfo = null;
        //    try
        //    {
        //        KAMDaemon tDaemonInfo = new KAMDaemon(aIP, aPort, aChannelName + aPort.ToString());
        //        tDaemonInfo.LifeTime = DateTime.Now;
        //        bool tIsAlreadyLogin = false;
        //        lock (m_DaemonManager)
        //        {
        //            foreach (int daemonID in m_DaemonManager.Keys)
        //            {
        //                m_DaemonManager[daemonID]
        //                if (tmpUserInfo.IP == aIP && tmpUserInfo.Port == aPort)
        //                {
        //                    tIsAlreadyLogin = true;
        //                    tAlreadyLoginInfo = tmpUserInfo;
        //                    break;
        //                }
        //            }
        //        }

        //        if (tIsAlreadyLogin)
        //        {
        //            GlobalClass.s_DaemonProcessManager.DaemonProcessList.Remove(tAlreadyLoginInfo);
        //        }


        //        // 사용자 정보를 해시 테이블에 추가 합니다.
        //        lock (GlobalClass.s_DaemonProcessManager.DaemonProcessList)
        //        {
        //            GlobalClass.m_LogProcess.PrintLog(string.Concat("데몬 접속 ", tDaemonInfo.IP, ":", tDaemonInfo.Port, " ", tDaemonInfo.ChannelName));
        //            GlobalClass.s_DaemonProcessManager.DaemonProcessList.Add(tDaemonInfo);
        //        }
        //        tLoginResult.DaemonInfo = tDaemonInfo;
        //        tLoginResult.ClientID = tDaemonInfo.DaemonID;
        //        tLoginResult.LoginResult = E_LoginResult.Success;
        //    }
        //    catch (Exception ex)
        //    {
        //        tLoginResult.LoginResult = E_LoginResult.UnknownError;
        //        return ObjectConverter.GetBytes(tLoginResult);
        //    }

        //    return ObjectConverter.GetBytes(tLoginResult);
        //}

        /// <summary>
        /// 수신한 요청을 큐에 저장
        /// </summary>
        /// <param name="aData"></param>
        private void DaemonRequestReceiver(byte[] aData)
        {
            if (aData == null || aData.Length == 0) return;

            try
            {
                RequestCommunicationData tRequestData = (RequestCommunicationData)ObjectConverter.GetObject(aData);

                lock (m_RequestQueue)
                {
                    m_RequestQueue.Enqueue(tRequestData);
                    m_RequestQueueMRE.Set();
                }
            }
            catch (Exception e)
            {
                GlobalClass.PrintLogException(string.Format("[데몬통신자] [DaemonCommProcess.RequestReceiver] "), e);
            }
        }

        /// <summary>
        /// 데몬 요청을 처리 합니다.
        /// </summary>
        private void _ProcessDaemonRequest()
        {
            RequestCommunicationData tRequest = null;
            while (!GlobalClass.m_IsServerStop)
            {
                try
                {
                    if (m_RequestQueue.Count == 0)
                    {
                        m_RequestQueueMRE.Reset();
                        m_RequestQueueMRE.WaitOne();
                    }

                    tRequest = null;
                    lock (m_RequestQueue)
                    {
                        if (m_RequestQueue.Count > 0)
                            tRequest = m_RequestQueue.Dequeue();
                    }
                    if (tRequest == null || tRequest.RequestData == null) continue;


                    /// 데몬과 서버시각이 다르므로 현 서버시각으로 업데이트 (요청처리 Timeout 체크용)
                    tRequest.RequestTime = DateTime.Now;
                    DaemonProcessInfo tDaemonInfo = (DaemonProcessInfo)tRequest.UserData;

                    /// 요청데이터 유효성 체크
                    DeviceInfo devInfo = (DeviceInfo)tRequest.RequestData;
                    GlobalClass.PrintLogInfo(string.Format("◁[RECV:데몬요청] DeviceIP={0}, ClientID={1}, OwnerKey={2}, CommType={3}, 요청수신시각={4}",
                    tRequest.CommType.ToString(), tRequest.ClientID, tRequest.OwnerKey, tRequest.RequestTime.ToShortTimeString(), devInfo.IPAddress));

                    if (!Util.IsValidIPAddress(devInfo.IPAddress)) {
                        Util.Assert(false, string.Format("└[데몬통신자][데몬요청수신] 장비IP 정보가 올바르지 않습니다.IP={0} 요청을 무시합니다.", devInfo.IPAddress));
                        continue;
                    }

                    /// 데몬 요청을 터널관리자(SSHTunnelManager) 또는 KAM처리자(KeepAliveCommProcess)로 전달
                    DispatchDaemonRequest(tRequest);
                }
                catch (Exception ex)
                {
                    GlobalClass.PrintLogException("[데몬통신자] [DaemonCommProcess._ProcessRequest] ", ex);
                }
            }
        }

        /// <summary>
        /// 데몬으로 부터 받은 요청을 다른 모듈에 분배
        /// </summary>
        /// <param name="aRequest"></param>
        private void DispatchDaemonRequest(RequestCommunicationData aRequest)
        {
            if (aRequest == null) return;
            DeviceInfo deviceInfo = (DeviceInfo)aRequest.RequestData;
            if (deviceInfo == null) return;

            switch (aRequest.CommType)
            {
                // 장비 터널Open 요청
                case E_CommunicationType.RequestSSHTunnelOpen:
                    GlobalClass.m_TunnelManager.AddDaemonRequest(aRequest);
                    GlobalClass.PrintLogInfo(string.Format("└[데몬통신자] 터널관리자(SSHTunnelManager)에 요청 전달(장비IP={0}, 요청수신시각={1})", deviceInfo.IPAddress, Util.DateTimeToLogValue(aRequest.RequestTime)));
                    break;

                default:
                    GlobalClass.PrintLogError(string.Format("└[데몬통신자] DispatchDaemonRequest - 지원하지 않는 통신타입(E_CommunicationType)입니다. CommType={0} ", aRequest.CommType.ToString()));
                    break;
            } // End of switch
        }

        /// <summary>
        /// 데몬에 전송할 응답을 추가합니다.
        /// </summary>
        /// <param name="aRequest"></param>
        public void AddResult(RequestCommunicationData aReq, string aTunnelIP, ushort aTunnelPort, ErrorInfo aErrorInfo)
        {
            if (aReq == null) return;
            DaemonProcessInfo tDaemonInfo = null;
            DeviceInfo reqDeviceInfo = null;

            try
            {
                lock (m_DaemonResultQueue)
                {
                    tDaemonInfo = (DaemonProcessInfo)aReq.UserData;
                    if (tDaemonInfo == null) return;

                    if (!m_DaemonResultQueue.ContainsKey(tDaemonInfo.DaemonID))
                    {
                        m_DaemonResultQueue.Add(tDaemonInfo.DaemonID, new DaemonProcessInfo(tDaemonInfo));
                        //GlobalClass.PrintLogError(string.Format("[데몬통신자] AddResult - 해당 데몬의 연결정보가 없습니다. RemoteClientID={0}, DaemonAddress={1}:{2}, DaemonID={3}"
                        //                                        ,aReq.ClientID, tKAMDaemon.DaemonIP, tKAMDaemon.DaemonPort, tKAMDaemon.DaemonID));
                    }

                    reqDeviceInfo = (DeviceInfo)aReq.RequestData;
                    DeviceInfo resDeviceInfo = new DeviceInfo();
                    resDeviceInfo.IPAddress = reqDeviceInfo.IPAddress;
                    resDeviceInfo.SSHTunnelIP = aTunnelIP;
                    resDeviceInfo.SSHTunnelPort = aTunnelPort;

                    ResultCommunicationData tResult = new ResultCommunicationData(aReq);
                    tResult.ResultData = resDeviceInfo;
                    //tResult.UserData = null;
                    if (aErrorInfo != null) tResult.Error = aErrorInfo;

                    m_DaemonResultQueue[tDaemonInfo.DaemonID].DataQueue.Enqueue(tResult);

                    GlobalClass.PrintLogInfo(string.Format("[DaemonComm.AddResult] 데몬에 응답 추가: DaemonID={0}, DaemonAddress={1}:{2}, ClientID={3}, CommType={4}, 장비IP={5}, TunnelPort={6}, Error={7}"
                                            , tDaemonInfo.DaemonID, tDaemonInfo.IP, tDaemonInfo.Port, tResult.ClientID
                                            , tResult.CommType.ToString(), resDeviceInfo.IPAddress, resDeviceInfo.SSHTunnelPort, tResult.Error == null ? string.Empty : tResult.Error.ErrorString));
                } // End of lock
            }
            catch(Exception e)
            {
                GlobalClass.PrintLogException(string.Format("[DaemonCommProcess.AddResult] DaemonAddress={0}:{1}, 요청장비IP={2}, 터널정보={3}:{4}, 전달메시지={5}"
                                , tDaemonInfo != null ? tDaemonInfo.IP : null
                                , tDaemonInfo != null ? tDaemonInfo.Port.ToString() : null
                                , reqDeviceInfo != null ? reqDeviceInfo.IPAddress : null
                                , aTunnelIP, aTunnelPort
                                , aErrorInfo != null ? aErrorInfo.ErrorString : null), e);
            }
        }

        /// <summary>
        /// 요청 정보를 추가합니다.
        /// </summary>
        /// <param name="aRequest"></param>
        //public void AddResult(ResultCommunicationData aResult)
        //{
        //    Util.Assert(aResult != null && aResult.ResultData != null, "");
        //    if (aResult == null || aResult.ResultData == null) return;

        //    DeviceInfo deviceInfo = (DeviceInfo)aResult.ResultData;
        //    GlobalClass.PrintLogInfo(string.Format("[DaemonComm]▶데몬에전달: ClientID={0}, CommType={1}, DeviceIP={2}, TunnelPort={3}, Error={4}"
        //                            , aResult.ClientID, aResult.CommType.ToString(), deviceInfo.IPAddress, deviceInfo.SSHTunnelPort, aResult.Error == null ? "" : aResult.Error.ErrorString));
        //    lock (m_ResultQueue)
        //    {
        //        //if (!m_ResultQueue.ContainsKey(aResult.ClientID)) {   
        //        //    m_ResultQueue[aResult.ClientID] = new Queue<ResultCommunicationData>();
        //        //}
        //        //m_ResultQueue[aResult.ClientID].Enqueue(aResult);
        //        m_ResultQueue.Enqueue(aResult);
        //    }
        //}
        

        /// <summary>
        /// 데몬에 응답을 처리합니다.
        /// </summary>
        /// <param name="aClientID">요청한 ClientID</param>
        private byte[] DaemonResultSender(int aDaemonID)
        {
            byte[] tByteResult = null;
            ArrayList tResultList = new ArrayList();

            try
            {
                DaemonProcessInfo tDaemon = null;
                lock (m_DaemonResultQueue)
                {
                    if (!m_DaemonResultQueue.TryGetValue(aDaemonID, out tDaemon)) return null; 
                    if (tDaemon == null) return null;
                }
                
                lock(tDaemon.DataQueue.SyncRoot)
                {
                    while (tDaemon.DataQueue.Count > 0)
                    {
                        ResultCommunicationData tResult = (ResultCommunicationData)tDaemon.DataQueue.Dequeue();
                        if (tResult == null) continue;

                        DeviceInfo devInfo = (DeviceInfo)tResult.ResultData;
                        GlobalClass.PrintLogInfo(string.Format("▷[SEND:데몬] DaemonID={0}, DaemonAddress={1}:{2}, ClientID={3}, CommType={4}, DeviceIP={5}, TunnelPort={6}, Error={7}, ErrorStr={8}"
                                                , tDaemon.DaemonID, tDaemon.IP, tDaemon.Port, tResult.ClientID
                                                , tResult.CommType.ToString(), devInfo.IPAddress, devInfo.SSHTunnelPort, tResult.Error.Error.ToString(), tResult.Error.ErrorString));

                        tResultList.Add(tResult);
                    }

                    if (tResultList.Count > 0)
                    {
                        tByteResult = (byte[])ObjectConverter.GetBytes(tResultList);
                    }
                } // End of lock
            } 
            catch(Exception e)
            {
                GlobalClass.PrintLogException(string.Format("[데몬통신자] [DaemonCommProcess.DaemonResultSender] aDaemonID={0}", aDaemonID), e);
            }

            return tByteResult;
        }

        /// <summary>
        /// 프로세스를 종료합니다.
        /// </summary>
        public void Stop()
        {
            string tErrorString = string.Empty;
            if (m_RemoteGateway != null)
            {
                m_RemoteGateway.StopServer(out tErrorString);
                m_RemoteGateway.Dispose();
                m_RemoteGateway = null;
            }

            lock (m_RequestQueue)
            {
                if (m_RequestQueue != null) m_RequestQueue.Clear();
            }
            GlobalClass.StopThread(m_RequestProcessThread);

            if (m_DaemonResultQueue != null)
            {
                m_DaemonResultQueue.Clear();
            }
        }

        // Daemon에서 장비접속을 위한 SSH터널 요청이 왔다고 가정
        public void _AddRequestTest(string aDeviceIP)
        {
            //TODO: 즉석테스트) 터널요청 후 포트 열린것 확인하고 데몬에 결과 전송
            RequestCommunicationData tRequest = new RequestCommunicationData();
            tRequest.CommType = E_CommunicationType.RequestSSHTunnelOpen;
            tRequest.ClientID = 0;
            DeviceInfo devInfo = new DeviceInfo();
            devInfo.IPAddress = aDeviceIP;// "218.51.127.184";
            tRequest.RequestData = devInfo;
            lock (m_RequestQueue)
            {
                m_RequestQueue.Enqueue(tRequest);
                m_RequestQueueMRE.Set();
            }            
        }
    }
}
