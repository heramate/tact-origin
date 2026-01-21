using System;
using System.Collections.Generic;
using System.Text;
using MKLibrary.MKProcess;
using RACTCommonClass;
using System.Threading;
using RACTServerCommon;
using MKLibrary.MKNetwork;
using RACTTerminal;

namespace TelnetProcessor
{
    /// <summary>
    ///  텔넷 결과 전송에 사용할 핸들러 입니다.
    /// </summary>
    /// <param name="?"></param>
    public delegate void TelnetResultHandler(TelnetCommandResultInfo aResult);
    public class TelnetProcessor
    {
        /// <summary>
        /// 접속중인 텔넷 세션입니다.
        /// </summary>
        private TelnetSessionCollection m_TelnetSessionList;
        /// <summary>
        /// 장비에 전송할 명령 저장 큐 입니다.
        /// </summary>
        private Queue<RequestCommunicationData> m_RequestQueue;
        /// <summary>
        /// 요청 대기 입니다.
        /// </summary>
        private ManualResetEvent m_RequestMRE = new ManualResetEvent(false);
        /// <summary>
        /// 명령 처리용 스레드 입니다.
        /// </summary>
        private Thread m_RequestProcessThread;
        /// <summary>
        /// 결과 저장용 큐 입니다.
        /// </summary>
        public Queue<TelnetCommandResultInfo> m_ResultQueue;
        /// <summary>
        /// 결과 대기 입니다.
        /// </summary>
        public ManualResetEvent m_ResultMRE = new ManualResetEvent(false);
        /// <summary>
        /// 실행 여부 입니다.
        /// </summary>
        private bool m_IsRun;
        /// <summary>
        /// DB Log 처리 프로세서 입니다.
        /// </summary>
        private DBLogProcess m_DBLogProcess;
        /// <summary>
        /// File Log 처리 프로세서 입니다.
        /// </summary>
        private FileLogProcess m_FileLogProcess;
        /// <summary>
        /// 세션 Thread Pool 입니다.
        /// </summary>
        private MKThreadPool m_TelnetSessinThreadPool;
        /// <summary>
        /// 서버 리모드 객체 입니다.
        /// </summary>
        private MKRemote m_ServerRemoteObject;


        

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public TelnetProcessor()
        {
      
        }

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public TelnetProcessor(MKRemote aRemoteObject)
        {
            m_ServerRemoteObject = aRemoteObject;
                  
        }

        /// <summary>
        /// 텔넷 Processor를 시작 합니다.
        /// 2013-04-26 - shinyn - Log저장 경로를 추가하고, 로그저장을 하도록 수정합니다.
        /// </summary>
        public void Start(string aPath)
        {
            m_IsRun = true;

            m_TelnetSessionList = new TelnetSessionCollection();
            m_RequestQueue = new Queue<RequestCommunicationData>();
            m_ResultQueue = new Queue<TelnetCommandResultInfo>();
			// 2019-11-10 개선사항 (로그 저장 경로 개선)
            // 2013-04-26 - shinyn - Log저장 프로세스를 추가하고, 로그저장을 하도록 수정합니다.
            m_FileLogProcess = new FileLogProcess(aPath + "TelnetProcessorLog\\", "TelnetProcessorLog");
            m_FileLogProcess.Start();

            // 세션 Pool을 50에서 100으로 늘림
            //m_TelnetSessinThreadPool = new MKThreadPool("TelnetSessionPool",50);
            m_TelnetSessinThreadPool = new MKThreadPool("TelnetSessionPool", 50);
            m_TelnetSessinThreadPool.StartThreadPool();

            m_RequestProcessThread = new Thread(new ThreadStart(ProcessRequest));
            m_RequestProcessThread.Name = "명령 처리용 스레드 입니다.";
            m_RequestProcessThread.Start();
        }

        /// <summary>
        /// 종료 처리 합니다.
        /// </summary>
        public void Dispose()
        {
            m_IsRun = false;

            try
            {
                if (m_FileLogProcess != null)
                {
                    m_FileLogProcess.Stop();
                }
                foreach (TelnetSessionInfo tSession in m_TelnetSessionList)
                {
                    tSession.Dispose();
                }

                if (m_RequestProcessThread.IsAlive)
                {
                    m_RequestProcessThread.Abort();
                }

                if (m_TelnetSessinThreadPool != null)
                {
                    m_TelnetSessinThreadPool.StopThreadPool();
                    m_TelnetSessinThreadPool.Dispose();
                }
            }
            catch { }
        }
        /// <summary>
        /// Telnet 명령 요청을 처리 합니다.
        /// </summary>
        private void ProcessRequest()
        {
            RequestCommunicationData tRequestData = null;
            while (m_IsRun)
            {
                tRequestData = null;

                if (m_RequestQueue.Count == 0)
                {
                    m_RequestMRE.Reset();
                    m_RequestMRE.WaitOne();
                }

                lock (m_RequestQueue)
                {
                    tRequestData = m_RequestQueue.Dequeue();
                }

                if (tRequestData != null)
                {
                    ProcessTelnetCommand(tRequestData);

                    tRequestData = null;
                }
            }
        }

        private void ProcessTelnetCommand(RequestCommunicationData aRequestCommunicationData)
        {
            m_TelnetSessinThreadPool.ExecuteWork(new MKWorkItem(new WorkItemParmeterMethod(ExecuteTelnetCommand), aRequestCommunicationData));  
        }

        private object ExecuteTelnetCommand(object tRequestCommunicationData)
        {
            RequestCommunicationData aRequestCommunicationData =(RequestCommunicationData) tRequestCommunicationData;

            TelnetCommandInfo tCommandInfo = null;
            TelnetSessionInfo tSessionInfo = null;
            string tLog = "";

            try
            {
                tCommandInfo = (TelnetCommandInfo)aRequestCommunicationData.RequestData;
                if (tCommandInfo.WorkTyp == E_TelnetWorkType.Connect)
                {

                    // 2013-04-26 - shinyn - 클라이언트에서 요청 받아서 세션 실행 로그 저장
                    string tRequestTime = aRequestCommunicationData.RequestTime.Year.ToString("0000") + "-" +
                                          aRequestCommunicationData.RequestTime.Month.ToString("00") + "-" +
                                          aRequestCommunicationData.RequestTime.Day.ToString("00") + " " +
                                          aRequestCommunicationData.RequestTime.Hour.ToString("00") + ":" +
                                          aRequestCommunicationData.RequestTime.Minute.ToString("00") + ":" +
                                          aRequestCommunicationData.RequestTime.Second.ToString("00");

                    DateTime tNow = DateTime.Now;
                    string tNowTime = tNow.Year.ToString("0000") + "-" +
                                      tNow.Month.ToString("00") + "-" +
                                      tNow.Day.ToString("00") + " " +
                                      tNow.Hour.ToString("00") + ":" +
                                      tNow.Minute.ToString("00") + ":" +
                                      tNow.Second.ToString("00");
                    tLog = "ProcessTelnetCommand : 클라이언트 세션 요청에 따른 세션 연결 : ClientID : " + aRequestCommunicationData.ClientID.ToString() +
                                  "요청시간 : " + tRequestTime + " 세션연결시간 : " + tNowTime;
                    m_FileLogProcess.PrintLog(tLog);

                    tSessionInfo = new TelnetSessionInfo(aRequestCommunicationData.ClientID, tCommandInfo.DeviceInfo);
                    tSessionInfo.Sender = tCommandInfo.Sender;
                    tSessionInfo.OnTelnetResultEvent += new TelnetResultHandler(tSessionInfo_OnTelnetResultEvent);
                    tSessionInfo.OnSessionDisConnectEvent += new SessionDisConnectHandler(tSessionInfo_OnSessionDisConnectEvent);
                    //tSessionInfo.OnTelnetCommandSaveEvent += new TelnetCommandSaveHandler(tSessionInfo_OnTelnetCommandSaveEvent);
                    if (tSessionInfo.Connect(aRequestCommunicationData))
                    {
                        if (tCommandInfo.DeviceInfo.IsRegistered)
                        {

                            //2017.06.21 - NoSeungPil - RCCS 로그인 기능추가
                            int ConnectionKind = 1; //Telnet
                            if (tCommandInfo.DeviceInfo.TerminalConnectInfo.SerialConfig.PortName == "RCCSPort") ConnectionKind = 2; //RCCS
							if (tCommandInfo.DeviceInfo.TerminalConnectInfo.SerialConfig.PortName == "RPCSPort") ConnectionKind = 3; //RPCS
                            if (tCommandInfo.DeviceInfo.TerminalConnectInfo.SerialConfig.PortName == "RPCSLTE") ConnectionKind = 4; //RPCSLTE

                            //string tQueryString = "EXEC [dbo].[SP_RACT_DeviceConnectLog] '{0}', {1}, {2}, {3}, '{4}', {5}";
                            //tQueryString = string.Format(tQueryString, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), tCommandInfo.UserID, tCommandInfo.DeviceInfo.DeviceID, (int)E_DeviceConnectType.Connection, tCommandInfo.DeviceInfo.IPAddress + " 장비에 연결 합니다.", ConnectionKind);
                            string tQueryString = "EXEC [dbo].[SP_RACT_DeviceConnectLog] {0}, {1}, {2}, '{3}', {4}";
                            tQueryString = string.Format(tQueryString, tCommandInfo.UserID, tCommandInfo.DeviceInfo.DeviceID, (int)E_DeviceConnectType.Connection, tCommandInfo.DeviceInfo.IPAddress + " 장비에 연결 합니다.", ConnectionKind);
                            tSessionInfo.ConnectionSessionID = GetTelnetSessionID(tQueryString);

                            try
                            {
                                tLog = "ProcessTelnetCommand : 클라이언트 최초 접속 : ClientID : " + aRequestCommunicationData.ClientID.ToString() +
                                      " LogID = " + tSessionInfo.ConnectionSessionID.ToString() +
                                      " 장비아이피 : " + tCommandInfo.DeviceInfo.IPAddress + " 에 연결합니다.";
                                m_FileLogProcess.PrintLog(tLog);
                            }
                            catch (Exception e)
                            {
                                m_FileLogProcess.PrintLog("ExecuteTelnetCommand 최초접속 오류 Exception : " + e.Message.ToString());
                            }

                        }
                        else
                        {
                            tSessionInfo.ConnectionSessionID = tSessionInfo.GetHashCode();
                        }
                        if (tSessionInfo.ConnectionSessionID > 0)
                        {
                            m_TelnetSessionList.Add(tSessionInfo);
                        }

                    }

                }
                else if (tCommandInfo.WorkTyp == E_TelnetWorkType.Disconnect)
                {
                    if (m_TelnetSessionList.Contains(tCommandInfo.SessionID))
                    {
                        tSessionInfo = m_TelnetSessionList[tCommandInfo.SessionID];
                        tSessionInfo.Disconnect(aRequestCommunicationData);
                    }
                    // tSessionInfo_OnSessionDisConnectEvent(tCommandInfo.SessionID);
                }
                else
                {
                    if (m_TelnetSessionList.Contains(tCommandInfo.SessionID))
                    {
                        tSessionInfo = m_TelnetSessionList[tCommandInfo.SessionID];
                        tSessionInfo.AddCommand(aRequestCommunicationData);

                        try
                        {
                            if (tCommandInfo.Command.Length > 2)
                            {
                                tLog = "ProcessTelnetCommand : 클라이언트 명령어 실행 : ClientID : " + aRequestCommunicationData.ClientID.ToString() +
                              " LogID = " + tSessionInfo.ConnectionSessionID.ToString() +
                              " 장비아이피 : " + tCommandInfo.DeviceInfo.IPAddress + " 에 명령수행" +
                              " 명령어 : " + tCommandInfo.Command;
                                m_FileLogProcess.PrintLog(tLog);
                            }

                            
                        }
                        catch (Exception e)
                        {
                            m_FileLogProcess.PrintLog("ExecuteTelnetCommand 클라이언트 명령어 실행 오류 Exception : " + e.Message.ToString());
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                m_FileLogProcess.PrintLog("ExecuteTelnetCommand exception : " + ex.Message.ToString());
                //System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            return null;
        }

        public int GetTelnetSessionCount
        {
            get { return m_TelnetSessionList.Count; }
        }
        /// <summary>
        /// 접속 정보를 추가하고 ID를 가져오기합니다.
        /// </summary>
        /// <param name="aQuery"></param>
        /// <returns></returns>
        private int GetTelnetSessionID(string aQuery)
        {
             RemoteClientMethod tRemoteClientMethod = null;
           
            int tSessionID = -1;

            int tResultFailCount = 0;

            while (true)
            {

                try
                {
                    tRemoteClientMethod = (RemoteClientMethod)m_ServerRemoteObject.ServerObject;
                    tSessionID = tRemoteClientMethod.CallTelnetSessionIDRequestHandler(aQuery);

                    if (tSessionID > 0)
                    {
                        break;
                    }

                    if (tResultFailCount > 3)
                    {
                        break;
                    }
                }
                catch
                {
                    tResultFailCount++;
                }
                Thread.Sleep(100);
            }

            return tSessionID;
        }

        /// <summary>
        /// 접속 정보를 Update 합니다.
        /// </summary>
        private bool UpdateTelnetConnection(int aSessinoID)
        {
            if (m_ServerRemoteObject == null) return false;
            RemoteClientMethod tRemoteClientMethod = null;

            tRemoteClientMethod = (RemoteClientMethod)m_ServerRemoteObject.ServerObject; ;
            return tRemoteClientMethod.CallTelnetConnectionUpdateRequestMethod(aSessinoID);
        }

        /// <summary>
        /// 텔넷 로그를 저장 합니다.
        /// </summary>
        /// <param name="aSessionKey"></param>
        /// <param name="aCommand"></param>
        void tSessionInfo_OnTelnetCommandSaveEvent(DBExecuteCommandLogInfo aLogInfo)
        {
            m_DBLogProcess.AddLog(aLogInfo);
        }

        /// <summary>
        /// 세션 종료 처리 합니다.
        /// </summary>
        /// <param name="aKey"></param>
        void tSessionInfo_OnSessionDisConnectEvent(int aKey)
        {
            //2015-10-20 hanjiyeon 개체 참조 오류 관련 예외처리 추가.
            try
            {
                lock (m_TelnetSessionList)
                {
                    if (m_TelnetSessionList.Contains(aKey))
                    {
                        if (m_TelnetSessionList[aKey].Sender == null)
                        {
                            UpdateTelnetConnection(aKey);
                        }

                        // 2013-04-26 - shinyn - 장비세션을 종료하고 세션을 지웁니다.
                        m_FileLogProcess.PrintLog(" tSessionInfo_OnSessionDisConnectEvent : 장비세션 종료 실행 : ClientID : " + m_TelnetSessionList[aKey].ClientID.ToString() +
                                                  " Session ID : " + m_TelnetSessionList[aKey].ConnectionSessionID.ToString());

                        m_TelnetSessionList[aKey].Dispose();
                        m_TelnetSessionList.Remove(aKey);

                        // 2013-04-26 - shinyn - 장비세션을 종료하고 세션을 지웁니다.
                        m_FileLogProcess.PrintLog(" tSessionInfo_OnSessionDisConnectEvent : 장비세션 종료 실행 : Session Count : " + m_TelnetSessionList.Count.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
      
        /// <summary>
        /// 텔넷 결과 처리 입니다.
        /// </summary>
        /// <param name="aResult"></param>
        void tSessionInfo_OnTelnetResultEvent(TelnetCommandResultInfo aResult)
        {
            lock (m_ResultQueue)
            {
                m_ResultQueue.Enqueue(aResult);
                m_ResultMRE.Set();
            }
        }
        /// <summary>
        /// 텔넷 요청 처리 입니다.
        /// </summary>
        /// <param name="aRequestData"></param>
        public void ExecuteCommand(RequestCommunicationData aRequestData)
        {
            lock (m_RequestQueue)
            {
                m_RequestQueue.Enqueue(aRequestData);
                m_RequestMRE.Set();
            }
        }

        /// <summary>
        /// 사용자 세션을 삭제 합니다.
        /// </summary>
        /// <param name="p"></param>
        public void RemoveUserSession(int aClientID)
        {
            TelnetSessionInfo tSessIon;
            try
            {
                for (int i = m_TelnetSessionList.Count - 1; i > -1; i--)
                {
                    tSessIon = (TelnetSessionInfo)m_TelnetSessionList.InnerList[i];
                    if (tSessIon.ClientID == aClientID)
                    {
                        tSessionInfo_OnSessionDisConnectEvent(tSessIon.ConnectionSessionID);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

       


        public void DisconnectSession(int aClientID, int aSessionID)
        {
            TelnetSessionInfo tSessionInfo;

            if (m_TelnetSessionList.Contains(aSessionID))
            {
                tSessionInfo = m_TelnetSessionList[aSessionID];
                tSessionInfo.Disconnect();
            }
        }

        public TelnetSessionCollection RunTelnetSessionList
        {
            get { return m_TelnetSessionList; }
        }
    }
}
