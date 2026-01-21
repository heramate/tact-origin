using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using MKLibrary.MKData;
using System.Windows.Forms;
using System.Threading;
using RACTCommonClass;
using System.IO;

namespace RACTDaemonProcess
{
    [Serializable]
    public class DaemonProcess : SenderObject
    {
        /// <summary>
        /// 결과 전송용 스레드 입니다.
        /// </summary>
        private Thread m_TelnetResultSenderThread= null;
        /// <summary>
        /// 결과 처리에서 사용할 스레드 입니다.
        /// </summary>
        private Thread m_ProcessServerResultThread = null;
        /// <summary>
        /// 결과 큐 입니다.
        /// </summary>
        private Queue m_ResultQueue = new Queue();
        /// <summary>
        /// 결과 대기 입니다.
        /// </summary>
        private ManualResetEvent m_ResultMRE = new ManualResetEvent(false);
        /// <summary>
        /// 결과 얻기 스래드 입니다.
        /// </summary>
        private Thread m_GetResultThread = null;
        /// <summary>
        /// 서버의 데몬 상태를 업데이트 합니다.
        /// </summary>
        private Thread m_DaemonStatusUpdateThread = null;
        /// <summary>
        /// 서버 요청 전송용 스레드입니다.
        /// </summary>
        private Thread m_RequestSendThread = null;
        /// <summary>
        /// 서버에 접속 처리할 스레드 입니다.
        /// </summary>
        private Thread m_ServerConnectionThread = null;
        /// <summary>
        /// 데몬 접속 포트
        /// </summary>
        private int m_DaemonPort = 0;

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        //public DaemonProcess(){}
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public DaemonProcess(string aServerlIP, int aServerPort, string aServerChannelName,string aLocallIP, int aLocalPort)
        {
            m_DaemonPort = aLocalPort;
/*          // 2019.01.25 KwonTaeSuk 환경설정파일 정리(DaemonLauncherConfig.xml, DaemonProcessConfig.xml)
            DaemonGlobal.s_DaemonConfig = new DaemonConfig();
            DaemonGlobal.s_DaemonConfig.ServerIP = aServerlIP;
            //DaemonGlobal.s_DaemonConfig.ServerPort = aServerPort;
            //DaemonGlobal.s_DaemonConfig.ServerChannel = aServerChannelName;
            DaemonGlobal.s_DaemonConfig.ServerDaemonPort = aServerPort;
            DaemonGlobal.s_DaemonConfig.ServerDaemonChannelName = aServerChannelName;

            DaemonGlobal.s_DaemonConfig.DaemonIP = aLocallIP;
            DaemonGlobal.s_DaemonConfig.DaemonPort = aLocalPort;
*/
        }
        /// <summary>
        /// 데몬을 시작합니다.
        /// </summary>
        /// <returns></returns>
        public bool StartDaemon()
        {
            if (StartLocalProcess())
            {
                return true;
            }

            Stop();
            return false;

        }
        private bool StartLocalProcess()
        {
            //System.Diagnostics.Debug.Assert(DaemonGlobal.s_DaemonConfig != null, "환경설정 파일을 먼저 읽어야합니다.");
            DaemonGlobal.s_FileLogProcess = new FileLogProcess(Application.StartupPath + "\\Log\\DaemonLog\\", "DaemonSystem" + m_DaemonPort);
            DaemonGlobal.s_FileLogProcess.Start();

            DaemonGlobal.s_FileLogProcess.PrintLog("Daemon 프로세서를 시작합니다.");
            //if (DaemonGlobal.s_DaemonConfig == null)
            //{
                if (!LoadSystemInfo()) return false;
            //}

            
            DaemonGlobal.s_IsRun = true;

            m_ServerConnectionThread = new Thread(new ThreadStart(ConnectRemoteServer));
            m_ServerConnectionThread.Start();

            DaemonGlobal.s_IsKamRun = true;
            //m_KamServerConnectionThread = new Thread(new ThreadStart(ConnectKamServer));
            //m_KamServerConnectionThread.Start();
            if (DaemonGlobal.s_DaemonConfig.KAMServerConnectEnable)
            {
                DaemonGlobal.s_KamServerCommunicationProcess = new KamServerCommunicationProcess();
                if (!DaemonGlobal.s_KamServerCommunicationProcess.Start()) return false;
            }

            DaemonGlobal.s_ClientCommunicationProcess = new ClientCommunicationProcess();
            if (!DaemonGlobal.s_ClientCommunicationProcess.Start()) return false;

          

            DaemonGlobal.s_TelnetProcessor = new TelnetProcessor.TelnetProcessor(DaemonGlobal.s_ServerRemoteGateway);
			// 2019-11-10 개선사항 (로그 저장 경로 개선 Client 경로 지정과는 다른 값을 사용하여 별도 수정 )
            // 2013-04-26 - shinyn - 텔넷프로세서 시작시 실행경로를 추가하여, 로그를 저장하도록 수정
            //DaemonGlobal.s_TelnetProcessor.Start();
            DaemonGlobal.s_TelnetProcessor.Start(Application.StartupPath+ "\\Log\\");

            m_TelnetResultSenderThread = new Thread(new ThreadStart(SendTelnetResult));
            m_TelnetResultSenderThread.Start();

            StartRequestSend();
            StartServerProcessResult();
            StartGetResult();

            m_DaemonStatusUpdateThread = new Thread(new ThreadStart(SendDaemonStatus));
            m_DaemonStatusUpdateThread.Start();

            return true;
        }
        /// <summary>
        /// 서버 접속을 처리합니다.
        /// 서버에 접속이 안 되어도 데몬은 실행이 되도록...
        /// </summary>
        private void ConnectRemoteServer()
        {
            DaemonGlobal.s_ServerCommunicationProcess = new ServerCommunicationProcess();
            DaemonGlobal.s_ServerCommunicationProcess.Start();
        }

        /// <summary>
        /// 로그 아웃 처리 합니다.
        /// </summary>
        public void SendDaemonLogOut()
        {
            try
            {
                if (DaemonGlobal.s_IsServerConnected)
                {
                    RemoteClientMethod tSPO = (RemoteClientMethod)DaemonGlobal.s_ServerRemoteGateway.ServerObject;
                    tSPO.CallUserLogOutMethod(DaemonGlobal.s_DaemonProcessInfo.DaemonID);
                }
            }
            catch { }
        }

        public void Stop()
        {

            SendDaemonLogOut();

            DaemonGlobal.s_IsRun = false;
            DaemonGlobal.s_IsServerConnected = false;

            DaemonGlobal.s_FileLogProcess.Stop();

            DaemonGlobal.StopThread(m_TelnetResultSenderThread);
            DaemonGlobal.StopThread(m_ProcessServerResultThread);
            m_ResultQueue.Clear();
            DaemonGlobal.StopThread(m_GetResultThread);
            DaemonGlobal.StopThread(m_DaemonStatusUpdateThread);
            DaemonGlobal.StopThread(m_RequestSendThread);


            if (DaemonGlobal.s_TelnetProcessor != null)
            {
                DaemonGlobal.s_TelnetProcessor.Dispose();
            }
            if (DaemonGlobal.s_ClientCommunicationProcess != null)
            {
                DaemonGlobal.s_ClientCommunicationProcess.Stop();
            }
            if (DaemonGlobal.s_KamServerCommunicationProcess != null)
            {
                DaemonGlobal.s_KamServerCommunicationProcess.Stop();
            }


            if (DaemonGlobal.s_ServerRemoteGateway != null)
            {
                DaemonGlobal.s_ServerRemoteGateway.Dispose();
            }
            if (DaemonGlobal.s_RequestQueue != null)
            {
                DaemonGlobal.s_RequestQueue.Clear();
            }


            DaemonGlobal.s_TelnetProcessor = null;
            DaemonGlobal.s_ClientCommunicationProcess = null;
            DaemonGlobal.s_KamServerCommunicationProcess = null;
            DaemonGlobal.s_ServerRemoteGateway = null;
        }

        /// <summary>
        /// 데몬 상태 갱신을 전송 합니다.
        /// </summary>
        public void SendDaemonStatus()
        {
            RemoteClientMethod tRemoteClientMethod;
            DaemonProcessInfo tRequestProcess ;
            while (DaemonGlobal.s_IsRun)
            {
                if (DaemonGlobal.s_DaemonProcessInfo != null)
                {
                    try
                    {
                        if (DaemonGlobal.s_IsServerConnected)
                        {
                            DaemonGlobal.s_DaemonProcessInfo.ConnectUsercount = DaemonGlobal.s_ClientCommunicationProcess.GetConnectionUserCount;
                            DaemonGlobal.s_DaemonProcessInfo.TelnetSessionCount = DaemonGlobal.s_TelnetProcessor.GetTelnetSessionCount;
                            tRequestProcess = new DaemonProcessInfo(DaemonGlobal.s_DaemonProcessInfo);
                            System.Diagnostics.Debug.WriteLine(string.Concat("##데몬 상태 갱신 ", tRequestProcess.IP, ":", tRequestProcess.Port));
                            // 2013-04-23 - shinyn - 데몬 세션 연결수 표시
                            System.Diagnostics.Debug.WriteLine(" ##세션 연결 갱신 Daemon ID : " + DaemonGlobal.s_DaemonProcessInfo.DaemonID.ToString() +
                                                               " Daemon SessionCount : " + DaemonGlobal.s_DaemonProcessInfo.TelnetSessionCount.ToString() +
                                                               " IP " + tRequestProcess.IP + " : " + tRequestProcess.Port.ToString());
                            tRemoteClientMethod = (RemoteClientMethod)DaemonGlobal.s_ServerRemoteGateway.ServerObject;
                            tRemoteClientMethod.CallDaemonStatusUpdateRequestMethod(ObjectConverter.GetBytes(tRequestProcess));
                        }
                    }
                    catch (Exception ex)
                    {
                        DaemonGlobal.s_FileLogProcess.PrintLog("SendDaemonStatus 오류 Exception : " + ex.Message.ToString());
                    }
                }
                Thread.Sleep(2000);
            }
        }

        /// <summary>
        /// 서버 설정 정보를 로드합니다.
        /// </summary>
        /// <returns></returns>
        public bool LoadSystemInfo()
        {
            DaemonGlobal.s_FileLogProcess.PrintLog("Daemon 설정 정보를 로드 합니다.");
            ArrayList tSystemInfos = null;
            E_XmlError tXmlError = E_XmlError.Success;
            try
            {
                FileInfo tConfigFile =new  FileInfo(Application.StartupPath +"\\"+ DaemonConfig.s_DaemonConfigFileName);
                if (!tConfigFile.Exists)
                {
                    MKXML.ObjectToXML(tConfigFile.FullName, new DaemonConfig());
                    DaemonGlobal.s_FileLogProcess.PrintLog(E_FileLogType.Info, string.Format("환경정보 파일이 없어 새로 생성하였습니다({0})", tConfigFile.FullName));
                    return false;
                }
                tSystemInfos = MKXML.ObjectFromXML(Application.StartupPath + "\\" + DaemonConfig.s_DaemonConfigFileName, typeof(DaemonConfig), out tXmlError);
                if (tSystemInfos == null) return false;
                if (tSystemInfos.Count == 0) return false;
                DaemonGlobal.s_DaemonConfig = (DaemonConfig)tSystemInfos[0];

                // 데몬접속 포트 설정값
                DaemonGlobal.s_DaemonConfig.DaemonPort = m_DaemonPort;
                if (m_DaemonPort == 0)
                {
                    DaemonGlobal.s_FileLogProcess.PrintLog(E_FileLogType.Warning, string.Format("[환경설정 오류] 데몬 접속 포트가 설정되지 않았습니다.(DaemonPort={0})", m_DaemonPort));
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                DaemonGlobal.s_FileLogProcess.PrintLog(E_FileLogType.Error,ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 텔넷 결과를 전송 합니다.
        /// </summary>
        private void SendTelnetResult()
        {
            TelnetCommandResultInfo tResult = null;
            ResultCommunicationData tResultCommunication = null;

            while (DaemonGlobal.s_IsRun)
            {
                if (DaemonGlobal.s_TelnetProcessor.m_ResultQueue.Count == 0)
                {
                    DaemonGlobal.s_TelnetProcessor.m_ResultMRE.Reset();
                    DaemonGlobal.s_TelnetProcessor.m_ResultMRE.WaitOne();

                }
                lock (DaemonGlobal.s_TelnetProcessor.m_ResultQueue)
                {
                    tResult = DaemonGlobal.s_TelnetProcessor.m_ResultQueue.Dequeue();
                }
             
                if (tResult != null)
                {
                    DaemonGlobal.SendResultClient(tResult);
                    tResult = null;
                }
            }
        }

        /// <summary>
        /// 결과 처리 스래드를 시작합니다.
        /// </summary>
        private void StartServerProcessResult()
        {
            m_ProcessServerResultThread = new Thread(new ThreadStart(ProcessServerResult));
            m_ProcessServerResultThread.Start();
        }

        /// <summary>
        /// 결과를 처리 합니다.
        /// </summary>
        private void ProcessServerResult()
        {
            ResultCommunicationData tResult = null;
            object tObject = null;

            ISenderObject tSender = null;
            bool tIsWorked = true;

            while (DaemonGlobal.s_IsRun)
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
                    lock (m_ResultQueue.SyncRoot)
                    {
                        tObject = ObjectConverter.GetObject((byte[])m_ResultQueue.Dequeue());
                    }
                    Console.WriteLine("[III]ProcessServerResult : " + tObject.GetType().ToString());

                    if (tObject.GetType().Equals(typeof(ResultCommunicationData)))
                    {
                        tResult = tObject as ResultCommunicationData;
                    }


                    if (tResult != null)
                    {
                        if (tResult.OwnerKey != 0)
                        {
                            tSender = null;
                            lock (DaemonGlobal.s_SenderList)
                            {
                                tSender = (ISenderObject)DaemonGlobal.s_SenderList[tResult.OwnerKey];
                            }
                            if (tSender != null)
                            {
                                tSender.ResultReceiver(tResult);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }
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
        private void StopRequestSend()
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

            while (DaemonGlobal.s_IsRun)
            {
                tSendObject = null;

                lock (DaemonGlobal.s_RequestQueue)
                {
                    if (DaemonGlobal.s_RequestQueue.Count > 0)
                    {
                        tSendObject = DaemonGlobal.s_RequestQueue.Dequeue();
                    }
                }
                if (tSendObject != null)
                {
                    try
                    {
                        tRemoteClientMethod = (RemoteClientMethod)DaemonGlobal.s_ServerRemoteGateway.ServerObject;
                        tRemoteClientMethod.CallRequestMethod(ObjectConverter.GetBytes(tSendObject));
                    }
                    catch (Exception ex)
                    {
                    }
                }
                Thread.Sleep(1);
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

            while (DaemonGlobal.s_IsRun)
            {
                if (DaemonGlobal.s_IsServerConnected)
                {
                    try
                    {
                        tResultData = null;
                        tRemoteClientMethod = (RemoteClientMethod)DaemonGlobal.s_ServerRemoteGateway.ServerObject; ;
                        tResultDatas = tRemoteClientMethod.CallDaemonResultMethod(DaemonGlobal.s_DaemonProcessInfo.DaemonID);
                        if (tResultDatas != null) tResultData = (ArrayList)ObjectConverter.GetObject(tResultDatas);
                    }
                    catch (Exception ex)
                    {
                        tRemoteClientMethod = null;
                        if (DaemonGlobal.s_IsServerConnected)
                        {
                            tResultFailCount++;
                            if (tResultFailCount > 3)
                            {
                                DaemonGlobal.s_IsServerConnected = false;
                                TryServerConnect();
                            }
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
                            // AppGlobal.s_FileLog.PrintLogEnter("ProcessGetResultFromServer-Data: " + ex.ToString());
                        }
                    }

                }
                else
                {
                   
                        tResultFailCount++;
                        if (tResultFailCount > 3)
                        {
                            DaemonGlobal.s_IsServerConnected = false;
                            TryServerConnect();
                        }
                    
                }
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// 서버와 재연결을 시도 합니다.
        /// </summary>
        private void TryServerConnect()
        {

            for (int i = 0; i < 3; i++)
            {
                if (DaemonGlobal.s_ServerCommunicationProcess.Start())
                {
                    break;
                }
                Thread.Sleep(5000);
            }

           
        }
    }
}
