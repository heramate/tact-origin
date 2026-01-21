using System;
using System.Collections.Generic;
using System.Text;
using MKLibrary.MKNetwork;
using System.Threading;
using RACTCommonClass;
using System.Collections;

namespace RACTClient
{
    public class DaemonProcessRemoteObject
    {
        /// <summary>
        /// 데몬과 연결이 끊기면 발생합니다.
        /// </summary>
        public event DefaultHandler OnDisconnectDaemon;
        /// <summary>
        /// Daemon ID 입니다.
        /// </summary>
        private int m_DaemonID;
        /// <summary>
        /// Remote 객체 입니다.
        /// </summary>
        private MKRemote m_RemoteObject;
        /// <summary>
        /// 데몬에 명령을 전송할 스레드 입니다.
        /// </summary>
        private Thread m_RequestSendThread;
        /// <summary>
        /// 요청 큐 입니다.
        /// </summary>
        private  Queue<CommunicationData> m_DaemonRequestQueue = new Queue<CommunicationData>();
        /// <summary>
        /// 요청 대기 입니다.
        /// </summary>
        private ManualResetEvent m_RequestMRE = new ManualResetEvent(false);
        /// <summary>
        /// 결과 받기 스레드 입니다.
        /// </summary>
        private Thread m_GetResultThread;
        /// <summary>
        /// 실행 여부 입니다.
        /// </summary>
        private bool m_IsRun = false;
        /// <summary>
        /// 데몬 접속 여부 입니다.
        /// </summary>
        private bool m_IsDaemonConnected = true;
        /// <summary>
        /// 결과 큐 입니다.
        /// </summary>
        private Queue m_ResultQueue = new Queue();
        /// <summary>
        /// 결과 대기 입니다.
        /// </summary>
        private ManualResetEvent m_ResultMRE = new ManualResetEvent(false);
        /// <summary>
        /// 결과처리 스레드 입니다.
        /// </summary>
        private Thread m_ProcessResultThread;
        /// <summary>
        /// 접속 정비 목록 입니다.
        /// </summary>
        private List<int> m_ConnectedSessionDeviceIDList = new List<int>();

        /// <summary>
        /// DaemonProcessInfo 입니다.
        /// </summary>
        private DaemonProcessInfo m_ProcessInfo;



        /// <summary>
        /// DaemonProcessInfo 가져오거나 설정 합니다.
        /// </summary>
        public DaemonProcessInfo ProcessInfo
        {
            get { return m_ProcessInfo; }
            set { m_ProcessInfo = value; }
        }

        /// <summary>
        /// session 목록 가져오거나 설정 합니다.
        /// </summary>
        public List<int> ConnectedSessionIDList
        {
            get { return m_ConnectedSessionDeviceIDList; }
            set { m_ConnectedSessionDeviceIDList = value; }
        }


        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        /// <param name="aDaemonID"></param>
        /// <param name="aRemoteObject"></param>
        public DaemonProcessRemoteObject(DaemonProcessInfo aProcessInfo, MKRemote aRemoteObject)
        {
            m_ProcessInfo = aProcessInfo;
            m_DaemonID = aProcessInfo.DaemonID;
            m_RemoteObject = aRemoteObject;
            m_IsRun = true;

            StartProcessResult();
            StartGetResult();
            StartRequestSend();
        }

        public void Stop()
        {
            m_IsRun = false;
            m_DaemonRequestQueue.Clear();
            m_ResultQueue.Clear();

            m_ResultMRE.Set();
            m_RequestMRE.Set();
    //        LogOutFromDaemon();
            if (m_RemoteObject != null)
            {
                m_RemoteObject.Dispose();
            }
        }

        /// <summary>
        /// 데몬 접속 종료 합니다.
        /// </summary>
        public void DisconnectFromDaemon(int aSessionID)
        {
            if (m_RemoteObject == null) return;
            RemoteClientMethod tRemoteClientMethod = (RemoteClientMethod)m_RemoteObject.ServerObject; ;
            tRemoteClientMethod.CallDisconnectDaemonTelnetSessionRequestMethod(AppGlobal.s_LoginResult.ClientID, aSessionID);
        }

        /// <summary>
        /// 결과 받기 스래드를 중지 합니다.
        /// </summary>
        private void StopGetResult()
        {
            if (m_GetResultThread != null)
            {
                m_GetResultThread.Join(10);
                if (m_GetResultThread.IsAlive)
                {
                    try
                    {
                        m_GetResultThread.Abort();
                    }
                    catch { }
                }
                m_GetResultThread = null;
            }
        }

        /// <summary>
        /// 결과 처리 스래드를 시작합니다.
        /// </summary>
        private void StartProcessResult()
        {
            m_ProcessResultThread = new Thread(new ThreadStart(ProcessServerResult));
            m_ProcessResultThread.Start();
        }

        private void StopProcessResult()
        {
            try
            {
                if (m_ProcessResultThread != null)
                {
                    m_ProcessResultThread.Join(10);
                    if (m_ProcessResultThread.IsAlive)
                    {

                        m_ProcessResultThread.Abort();

                    }
                    m_ProcessResultThread = null;
                }
            }
            catch { }
        }

        /// <summary>
        /// 결과를 처리 합니다.
        /// </summary>
        private void ProcessServerResult()
        {
            ResultCommunicationData tResult = null;
            TelnetCommandResultInfo tWorkResult = null;
            object tObject = null;

            ISenderObject tSender = null;
            bool tIsWorked = true;

            while (m_IsRun)
            {
                try
                {

                    tResult = null;
                    tWorkResult = null;
                    tObject = null;


                    if (m_ResultQueue.Count == 0)
                    {
                        m_ResultMRE.Reset();
                        m_ResultMRE.WaitOne();
                    }

                    if (m_ResultQueue.Count == 0) continue;
                    lock (m_ResultQueue.SyncRoot)
                    {
                        tObject = ObjectConverter.GetObject((byte[])m_ResultQueue.Dequeue());

                        if (tObject.GetType().Equals(typeof(ResultCommunicationData)))
                        {
                            tResult = tObject as ResultCommunicationData;
                        }
                        else if (tObject.GetType().Equals(typeof(TelnetCommandResultInfo)))
                        {
                            tWorkResult = tObject as TelnetCommandResultInfo;
                        }
                    }

                    if (tResult != null)
                    {
                        if (tResult.OwnerKey != 0)
                        {
                            tSender = null;
                            lock (AppGlobal.s_SenderList)
                            {
                                tSender = (ISenderObject)AppGlobal.s_SenderList[tResult.OwnerKey];
                            }
                            if (tSender != null)
                            {
                                tSender.ResultReceiver(tResult);
                            }
                        }

                    }
                    else if (tWorkResult != null)
                    {
                        if (tWorkResult.OwnerKey != 0)
                        {
                            tSender = null;
                            lock (AppGlobal.s_SenderList) tSender = (ISenderObject)AppGlobal.s_SenderList[tWorkResult.OwnerKey];
                            if (tSender != null) tSender.ResultReceiver(tWorkResult);
                        }
                    }

                    tResult = null;
                    tWorkResult = null;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }
        }

        /// <summary>
        /// 결과 받기 스래드를 시작합니다.
        /// </summary>
        private void StartGetResult()
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
            ArrayList tResultData = null;
            byte[] tResultDatas = null;

            int tResultFailCount = 0;

            while (m_IsRun)
            {

                try
                {
                    tResultData = null;
                    tRemoteClientMethod = (RemoteClientMethod)m_RemoteObject.ServerObject; ;
                    tResultDatas = tRemoteClientMethod.CallResultMethod(AppGlobal.s_LoginResult.ClientID);
                    if (tResultDatas != null) tResultData = (ArrayList)ObjectConverter.GetObject(tResultDatas);
                }
                catch (Exception ex)
                {
                    tRemoteClientMethod = null;
                    m_IsDaemonConnected = false;
                    if (TryServerConnect() != E_ConnectError.NoError)
                    {
                        if (OnDisconnectDaemon != null) OnDisconnectDaemon();
                        break;
                    }
                }

                if (tResultData != null)
                {
                    try
                    {
                        if (tResultData.Count > 0)
                        {

                            lock (m_ResultQueue.SyncRoot)
                            {
                                for (int i = 0; i < tResultData.Count; i++)
                                {
                                    m_ResultQueue.Enqueue(tResultData[i]);
                                    m_ResultMRE.Set();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.ToString());
                        // AppGlobal.s_FileLog.PrintLogEnter("ProcessGetResultFromServer-Data: " + ex.ToString());
                    }
                }


                Thread.Sleep(AppGlobal.s_ServerCheckInterval);
            }
        }
      //  private E_ConnectError m_ConnectError;
        /// <summary>
        /// 서버와 재연결을 시도 합니다.
        /// </summary>
        private E_ConnectError TryServerConnect()
        {
            E_ConnectError tConnectError = E_ConnectError.LinkFail;

            if (m_RemoteObject != null)
            {
                m_RemoteObject.Dispose();
                m_RemoteObject = null;
            }

            
            tConnectError = AppGlobal.TryDaemonConnect(m_ProcessInfo.IP, m_ProcessInfo.Port, m_ProcessInfo.ChannelName, out m_RemoteObject);

            if (tConnectError == E_ConnectError.NoError)
            {
                m_IsDaemonConnected = true;
            }

            return tConnectError;
        }
        /// <summary>
        /// 데몬 접속 여부 입니다.
        /// </summary>
        public bool IsDaemonConnected
        {
            get { return m_IsDaemonConnected; }
        }

        /// <summary>
        /// 서버에 요청 스래드를 시작합니다.
        /// </summary>
        private void StartRequestSend()
        {
            m_RequestSendThread = new Thread(new ThreadStart(ProcessRequestSendToServer));
            m_RequestSendThread.Start();
        }

        /// <summary>
        /// 서버에 요청 스래드를 중지 합니다.
        /// </summary>
        public void StopRequestSend()
        {
            if (m_RequestSendThread == null) return;

            m_RequestSendThread.Join(10);
            if (m_RequestSendThread.IsAlive)
            {
                try
                {
                    m_RequestSendThread.Abort();
                }
                catch { }
            }
            m_RequestSendThread = null;

        }

        /// <summary>
        /// 서버에 요청을 전송합니다.
        /// </summary>
        private void ProcessRequestSendToServer()
        {
            RemoteClientMethod tRemoteClientMethod = null;
            object tSendObject = null;

            while (m_IsRun)
            {
                tSendObject = null;


                if (m_DaemonRequestQueue.Count == 0)
                {
                    m_RequestMRE.Reset();
                    m_RequestMRE.WaitOne();
                }
                lock (m_DaemonRequestQueue)
                {
                    if (m_DaemonRequestQueue.Count > 0)
                    {
                        tSendObject = m_DaemonRequestQueue.Dequeue();
                    }
                }
                if (tSendObject != null)
                {
                    try
                    {
                        tRemoteClientMethod = (RemoteClientMethod)m_RemoteObject.ServerObject;
                        tRemoteClientMethod.CallRequestMethod(ObjectConverter.GetBytes(tSendObject));
                    }
                    catch (Exception ex)
                    {
                    }

                    tSendObject = null;
                }
            }
        }

        /// <summary>
        /// 요청 데이터를 전송합니다.
        /// </summary>
        /// <param name="vSender">전송자 입니다.</param>
        /// <param name="vCommunicationData">전송 데이터 입니다.</param>
        public void SendDaemonRequestData(ISenderObject vSender, CommunicationData vCommunicationData)
        {
            if (!m_IsRun) return;

            if (vSender != null)
            {
                AppGlobal.AddSender(vSender);
                vCommunicationData.OwnerKey = vSender.GetHashCode();
            }
            lock (m_DaemonRequestQueue)
            {
                m_DaemonRequestQueue.Enqueue(vCommunicationData);
            }
            m_RequestMRE.Set();
        }

        /// <summary>
        /// Daemon ID 가져오거나 설정 합니다.
        /// </summary>
        public int DaemonID
        {
            get { return m_DaemonID; }
            set { m_DaemonID = value; }
        }



        /// <summary>
        /// Remote 객체 가져오거나 설정 합니다.
        /// </summary>
        public MKRemote RemoteObject
        {
            get { return m_RemoteObject; }
            set { m_RemoteObject = value; }
        }
    }
}
