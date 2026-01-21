using System;
using System.Collections.Generic;
using System.Text;
using RACTCommonClass;
using MKLibrary.MKNetwork;
using System.Threading;
using System.Collections;
///// 2018-10-29 KANGBONGHAN [c-RPCS 원격접속기능] LTE접속을 위해 추가된 Keep-Alive 서버와 통신
namespace RACTDaemonProcess
{
    public class KamServerCommunicationProcess 
    {
        //private MKRemote m_RemoteGateway;
        /// <summary>
        /// 요청 처리 스레드 입니다.
        /// </summary>
        private Thread m_RequestKamSendThread = null;
        //private Thread m_KamServerConnectionThread = null;
        /// <summary>
        /// 결과 처리 스레드 입니다.
        /// </summary>
        private Thread m_ProcessKamServerResultThread = null;
        /// <summary>
        /// 요청을 저장할 큐 입니다.
        /// </summary>
        private Queue<RequestCommunicationData> m_RequestQueue;
        /// <summary>
        /// 결과를 저장할 큐 입니다.
        /// </summary>
        private Queue m_ResultQueue = null;
        private ManualResetEvent m_ResultMRE = new ManualResetEvent(false);

        /// <summary>
        /// 결과 얻기 스래드 입니다.
        /// </summary>
        private Thread m_GetResultThread = null;

        public KamServerCommunicationProcess() 
        {
            m_RequestQueue = new Queue<RequestCommunicationData>();
            m_ResultQueue = new Queue();
        }

        public bool Start()
        {
            if (TryServerConnect() == E_ConnectError.NoError)
            {
                DaemonGlobal.s_IsKamServerConnected = true;


                StartKamRequestSend();
                StartKamServerProcessResult();
                StartKamGetResult();
                return true;
            }
            return false;
        }

        public void Stop()
        {
            string tErrorString = "";
            DaemonGlobal.s_KamServerRemoteGateway.StopServer(out tErrorString);
            DaemonGlobal.s_KamServerRemoteGateway.Dispose();
            m_ResultQueue.Clear();
            DaemonGlobal.s_KamServerRemoteGateway = null;

            m_ResultMRE.Set();
            DaemonGlobal.StopThread(m_RequestKamSendThread);
            DaemonGlobal.StopThread(m_ProcessKamServerResultThread);
            DaemonGlobal.StopThread(m_GetResultThread);
        }

        /// <summary>
        /// 서버에 연결을 시도 합니다.
        /// </summary>
        /// <returns>연결 시도 성공 여부 입니다.</returns>
        public E_ConnectError TryServerConnect()
        {
            int tTryCount = 0;
            RemoteClientMethod tSPO = null;
            string tErrorString = string.Empty;
            DateTime tSDate = DateTime.Now;

            if (DaemonGlobal.s_KamServerRemoteGateway == null)
            {
                DaemonGlobal.s_FileLogProcess.PrintLog(string.Concat("장비LTE접속을 위한 Keep-Alive 서버에 접속을 시도 합니다. ", DaemonGlobal.s_DaemonConfig.KAMServerIP, ":", DaemonGlobal.s_DaemonConfig.KAMServerPort, DaemonGlobal.s_DaemonConfig.KAMServerChannel));
                DaemonGlobal.s_KamServerRemoteGateway = new MKRemote(E_RemoteType.TCPRemote, DaemonGlobal.s_DaemonConfig.KAMServerIP, DaemonGlobal.s_DaemonConfig.KAMServerPort, DaemonGlobal.s_DaemonConfig.KAMServerChannel);
            }

            if (DaemonGlobal.s_KamServerRemoteGateway == null)
            {
                // s_FileLog.PrintLogEnter("IP:" + s_ServerIP + " PortNo:" + s_ServerPort + " ChannelName : " + s_ChannelName +"에 연결 할 수 없습니다.");
                return E_ConnectError.LocalFail;
            }
            else
            {
                while (DaemonGlobal.s_IsKamRun)
                {
                    try
                    {
                        tTryCount++;
                        if (tTryCount > 5)
                        {
                            return E_ConnectError.ServerNoRun;
                        }

                        if (DaemonGlobal.s_KamServerRemoteGateway.ConnectServer(out tErrorString) != E_RemoteError.Success)
                        {
                            DaemonGlobal.s_FileLogProcess.PrintLog(string.Concat("Keep-Alive 서버에 접속하지 못했습니다 ", DaemonGlobal.s_DaemonConfig.KAMServerIP, ":", DaemonGlobal.s_DaemonConfig.KAMServerPort, DaemonGlobal.s_DaemonConfig.KAMServerChannel));
                            // s_FileLog.PrintLogEnter(string.Concat("서버에 연결할 수 없습니다. 서버가 정상적으로 시작되었는지 또는 FireWall이 작동중인지 확인 하십시오. :", tErrorString));
                            //return E_ConnectError.LinkFail;
                        }
                        else
                        {

                            tErrorString = string.Empty;

                            tSPO = (RemoteClientMethod)DaemonGlobal.s_KamServerRemoteGateway.ServerObject;
                            if (tSPO == null)
                            {
                                Thread.Sleep(3000);
                                continue;
                            }
                            ObjectConverter.GetObject(tSPO.CallDaemonResultMethod(0));
                            break;
                        }
                        Thread.Sleep(1000);
                    }
                    catch (Exception ex)
                    {
                        DaemonGlobal.s_FileLogProcess.PrintLog(string.Concat("Keep-Alive 서버에 접속하지 못했습니다 ", DaemonGlobal.s_DaemonConfig.KAMServerIP, ":", DaemonGlobal.s_DaemonConfig.KAMServerPort, DaemonGlobal.s_DaemonConfig.KAMServerChannel));
                        DaemonGlobal.s_FileLogProcess.PrintLog("[E] TryServerConnect: " + ex.ToString());
                        DaemonGlobal.s_IsServerConnected = false;
                        //if (((TimeSpan)DateTime.Now.Subtract(tSDate)).TotalSeconds > 60)
                        //{
                        //return E_ConnectError.LinkFail;
                        //}
                    }
                }
                DaemonGlobal.s_FileLogProcess.PrintLog(string.Concat("Keep-Alive 서버에 접속완료 ", DaemonGlobal.s_DaemonConfig.KAMServerIP, ":", DaemonGlobal.s_DaemonConfig.KAMServerPort, DaemonGlobal.s_DaemonConfig.KAMServerChannel));
                return E_ConnectError.NoError;
            }
        }


        private void StartKamServerProcessResult()
        {
            m_ProcessKamServerResultThread = new Thread(new ThreadStart(ProcessKamServerResult));
            m_ProcessKamServerResultThread.Start();
        }

        /// <summary>
        /// 결과를 처리 합니다.
        /// </summary>
        private void ProcessKamServerResult()
        {
            ResultCommunicationData tResult = null;
            object tObject = null;

            ISenderObject tSender = null;
            bool tIsWorked = true;

            while (DaemonGlobal.s_IsKamRun)
            {
                try
                {
                    tResult = null;
                    tObject = null;

                    if (m_ResultQueue.Count == 0)
                    {
                        m_ResultMRE.Reset();
                        m_ResultMRE.WaitOne();
                    }

                    if (m_ResultQueue.Count == 0)
                    {
                        continue;
                    }

                    lock (m_ResultQueue.SyncRoot)
                    {
                        tObject = m_ResultQueue.Dequeue();
                    }

                    if (tObject.GetType().Equals(typeof(ResultCommunicationData)))
                    {
                        tResult = tObject as ResultCommunicationData;
                    }
                    
                    
                    if (tResult != null)
                    {

                        /// 2019.10.01 KwonTaeSuk [KAMServer장비접속개선] 수신정보 파일로그 출력
                        string resultMsg = tResult.Error != null && tResult.Error.ErrorString != null ? "ResultMessage="+tResult.Error.ErrorString : string.Empty;

                        if (tResult.ResultData == null)
                        {
                            DaemonGlobal.s_FileLogProcess.PrintLog(E_FileLogType.Error, string.Format("▶[RECV:KAMServer] 응답수신 - 수신데이터(ResultData)가 null입니다. {0}", resultMsg));
                            continue;
                        }
                        System.Diagnostics.Debug.Assert(tResult.ResultData.GetType() == typeof(DeviceInfo), "수신데이터 값이 DeviceInfo가 아닙니다.");
                        DeviceInfo devInfo = ((DeviceInfo)tResult.ResultData);
                        DaemonGlobal.s_FileLogProcess.PrintLog(E_FileLogType.Info, string.Format("▶[RECV:KAMServer] {0} 장비IP={1}, SSH터널IP={2}, SSH터널포트={3}", resultMsg, devInfo.IPAddress, devInfo.SSHTunnelIP, devInfo.SSHTunnelPort));

                        /// 클라이언트에 전달
                        DaemonGlobal.SendResultClient(tResult);
                        //tResult = null;
                    }
                     
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                    DaemonGlobal.s_FileLogProcess.PrintLog(E_FileLogType.Error, string.Format("[KamServerCommunicationProc.ProcessKamServerResult] 오류발생 - {0}", ex.ToString()));
                }
            }
        }

        private void StartKamRequestSend()
        {
            m_RequestKamSendThread = new Thread(new ThreadStart(ProcessRequestKamSendToServer));
            m_RequestKamSendThread.Start();
        }

        private void StopKamRequestSend()
        {
            if (m_RequestKamSendThread == null) return;

            m_RequestKamSendThread.Join(10);
            if (m_RequestKamSendThread.IsAlive)
            {
                try
                {
                    m_RequestKamSendThread.Abort();
                }
                catch { }
            }
            m_RequestKamSendThread = null;

        }

        /// <summary>
        /// 서버에 요청을 전송합니다.
        /// </summary>
        private void ProcessRequestKamSendToServer()
        {
            RemoteClientMethod tRemoteClientMethod = null;
            object tSendObject = null;

            while (DaemonGlobal.s_IsKamRun)
            {
                tSendObject = null;

                lock (m_RequestQueue)
                {
                    if (m_RequestQueue.Count > 0)
                    {
                        tSendObject = m_RequestQueue.Dequeue();
                    }
                }
                if (tSendObject != null)
                {
                    try
                    {
                        tRemoteClientMethod = (RemoteClientMethod)DaemonGlobal.s_KamServerRemoteGateway.ServerObject;
                        tRemoteClientMethod.CallRequestMethod(ObjectConverter.GetBytes(tSendObject));

                        RequestCommunicationData tReq = (RequestCommunicationData)tSendObject;
                        System.Diagnostics.Debug.Assert(tReq.RequestData.GetType() == typeof(DeviceInfo), "RequestData 타입이 DeviceInfo가 아님!");
                        DaemonGlobal.s_FileLogProcess.PrintLog(E_FileLogType.Error, string.Format("▶[SEND:KAMServer] LTE접속요청 장비IP={0}, 장비모델={1}({2})", ((DeviceInfo)tReq.RequestData).IPAddress, ((DeviceInfo)tReq.RequestData).ModelName, ((DeviceInfo)tReq.RequestData).ModelID));
                    }
                    catch (Exception ex)
                    {
                        DaemonGlobal.s_FileLogProcess.PrintLog(E_FileLogType.Error, string.Format("[KamServerCommunicationProc.ProcessRequestKamSendToServer] 오류발생 -", ex.ToString()));
                    }
                }
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// 결과 받기 스래드를 시작합니다.
        /// </summary>
        private void StartKamGetResult()
        {
            m_GetResultThread = new Thread(new ThreadStart(ProcessGetResultFromServer));
            m_GetResultThread.Start();
        }
        /// <summary>
        /// 서버로부터 결과 받음을 처리 합니다.
        /// </summary>
        private void ProcessGetResultFromServer()
        {
            RemoteClientMethod tRemoteClientMethod = null;
            ArrayList tResultList = null; //Item: ResultCommunicationData
            byte[] tResultDatas = null;

            int tResultFailCount = 0;

            while (DaemonGlobal.s_IsKamRun)
            {
                /// 2019.10.01 KwonTaeSuk [KAMServer장비접속개선] TACTServer에 접속되어 있어야 통신가능(서버발급 DaemonID값을 데몬구분Key로 사용)
                if (DaemonGlobal.s_DaemonProcessInfo == null) continue;

                if (DaemonGlobal.s_IsKamServerConnected)
                {
                    try
                    {
                        tResultList = null;
                        tRemoteClientMethod = (RemoteClientMethod)DaemonGlobal.s_KamServerRemoteGateway.ServerObject; ;
                        tResultDatas = tRemoteClientMethod.CallResultMethod(DaemonGlobal.s_DaemonProcessInfo.DaemonID);
                        /// 2019.10.01 KwonTaeSuk [KAMServer장비접속개선] 여러 데몬요청 처리(ResultCommunicationData → ArrayList)
                        if (tResultDatas != null) tResultList = (ArrayList)ObjectConverter.GetObject(tResultDatas);
                    }
                    catch (Exception ex)
                    {
                        DaemonGlobal.s_FileLogProcess.PrintLog(E_FileLogType.Error, string.Format("[KamServerCommunicationProc.ProcessGetResultFromServer] 오류발생 - {0}", ex.ToString()));
                        tRemoteClientMethod = null;
                        if (DaemonGlobal.s_IsKamServerConnected)
                        {
                            tResultFailCount++;
                            if (tResultFailCount > 3)
                            {
                                DaemonGlobal.s_IsKamServerConnected = false;
                                if (TryServerConnect() == E_ConnectError.NoError)
                                    tResultFailCount = 0;
                            }
                        }
                    }

                    if (tResultList != null && tResultList.Count > 0)
                    {
                        try
                        {
                            lock (m_ResultQueue.SyncRoot)
                            {
                                for (int i = 0; i < tResultList.Count; i++)
                                {
                                    m_ResultQueue.Enqueue(tResultList[i]);
                                }
                                //DaemonGlobal.s_FileLogProcess.PrintLog(E_FileLogType.Error, string.Format("[KamServerCommunicationProc.ProcessGetResultFromServer] {0}건 수신", tResultList.Count));
                            }
                            m_ResultMRE.Set();
                        }
                        catch (Exception ex)
                        {
                            // AppGlobal.s_FileLog.PrintLogEnter("ProcessGetResultFromServer-Data: " + ex.ToString());
                        }
                    }

                }
                else
                {

                    tResultFailCount++;
                    if (tResultFailCount > 3)
                    {
                        DaemonGlobal.s_IsKamServerConnected = false;
                        if(TryServerConnect() == E_ConnectError.NoError)
                            tResultFailCount = 0;
                    }

                }
                Thread.Sleep(100);
            }

        }

        /// <summary>
        /// KAMServer에 요청
        /// 2019.10.01 KwonTaeSuk [KAMServer장비접속개선] 요청시 데몬정보 포함(TACTServer접속시 받은 DaemonID를 KAMServer에서 데몬구분Key로 사용)
        /// </summary>
        /// <param name="vCommunicationData"></param>
        
        public void SendKamRequestData(RequestCommunicationData vCommunicationData)
        {
            if (!DaemonGlobal.s_IsKamRun) return;

            m_ResultMRE.Reset();

            if (DaemonGlobal.s_DaemonProcessInfo == null)
            {
                DaemonGlobal.s_FileLogProcess.PrintLog(E_FileLogType.Error, "[KamServerCommunicationProc.SendKamRequestData] TACTServer에 먼저 접속해야 합니다(통신시 서버발급 DaemonID값 사용)");
                return;
            }
            vCommunicationData.UserData = DaemonGlobal.s_DaemonProcessInfo;

            lock (m_RequestQueue)
            {
                m_RequestQueue.Enqueue(vCommunicationData);
            }
            m_ResultMRE.Set();
        }

  
    }
}
