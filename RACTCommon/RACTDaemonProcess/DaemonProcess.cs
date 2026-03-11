using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Collections.Concurrent;
using MKLibrary.MKData;
using System.Windows.Forms;
using System.Threading;
using RACTCommonClass;
using System.IO;

namespace RACTDaemonProcess
{
    [Serializable]
    public class DaemonProcess : ISenderObject, IDisposable
    {
        private readonly CancellationTokenSource m_Cts = new CancellationTokenSource();
        private bool m_Disposed = false;

        /// <summary>
        /// 결과 전송용 스레드 입니다.
        /// </summary>
        private Thread m_TelnetResultSenderThread = null;
        /// <summary>
        /// 결과 처리에서 사용할 스레드 입니다.
        /// </summary>
        private Thread m_ProcessServerResultThread = null;
        /// <summary>
        /// 결과 큐 입니다. (BlockingCollection으로 교체)
        /// </summary>
        private BlockingCollection<byte[]> m_ResultCollection = new BlockingCollection<byte[]>();
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

        public DaemonProcess(string aServerlIP, int aServerPort, string aServerChannelName, string aLocallIP, int aLocalPort)
        {
            m_DaemonPort = aLocalPort;
        }

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
            DaemonGlobal.s_FileLogProcess = new FileLogProcess(Application.StartupPath + "\\Log\\DaemonLog\\", "DaemonSystem" + m_DaemonPort);
            DaemonGlobal.s_FileLogProcess.Start();

            DaemonGlobal.s_FileLogProcess.PrintLog("Daemon 프로세서를 시작합니다.");
            if (!LoadSystemInfo()) return false;

            DaemonGlobal.s_IsRun = true;

            m_ServerConnectionThread = new Thread(ConnectRemoteServer) { IsBackground = true };
            m_ServerConnectionThread.Start();

            DaemonGlobal.s_IsKamRun = true;
            if (DaemonGlobal.s_DaemonConfig.KAMServerConnectEnable)
            {
                DaemonGlobal.s_KamServerCommunicationProcess = new KamServerCommunicationProcess();
                if (!DaemonGlobal.s_KamServerCommunicationProcess.Start()) return false;
            }

            DaemonGlobal.s_ClientCommunicationProcess = new ClientCommunicationProcess();
            if (!DaemonGlobal.s_ClientCommunicationProcess.Start()) return false;

            DaemonGlobal.s_TelnetProcessor = new TelnetProcessor.TelnetProcessor(DaemonGlobal.s_ServerRemoteGateway);
            DaemonGlobal.s_TelnetProcessor.Start(Application.StartupPath + "\\Log\\");

            m_TelnetResultSenderThread = new Thread(SendTelnetResult) { IsBackground = true };
            m_TelnetResultSenderThread.Start();

            StartRequestSend();
            StartServerProcessResult();
            StartGetResult();

            m_DaemonStatusUpdateThread = new Thread(SendDaemonStatus) { IsBackground = true };
            m_DaemonStatusUpdateThread.Start();

            return true;
        }

        private void ConnectRemoteServer()
        {
            DaemonGlobal.s_ServerCommunicationProcess = new ServerCommunicationProcess();
            DaemonGlobal.s_ServerCommunicationProcess.Start();
        }

        public void SendDaemonLogOut()
        {
            try
            {
                if (DaemonGlobal.s_IsServerConnected && DaemonGlobal.s_ServerRemoteGateway != null)
                {
                    RemoteClientMethod tSPO = (RemoteClientMethod)DaemonGlobal.s_ServerRemoteGateway.ServerObject;
                    if (tSPO != null && DaemonGlobal.s_DaemonProcessInfo != null)
                    {
                        tSPO.CallUserLogOutMethod(DaemonGlobal.s_DaemonProcessInfo.DaemonID);
                    }
                }
            }
            catch { }
        }

        public void Stop()
        {
            SendDaemonLogOut();

            DaemonGlobal.s_IsRun = false;
            DaemonGlobal.s_IsServerConnected = false;
            m_Cts.Cancel();

            if (DaemonGlobal.s_FileLogProcess != null)
                DaemonGlobal.s_FileLogProcess.Stop();

            Dispose();
        }

        public void SendDaemonStatus()
        {
            while (DaemonGlobal.s_IsRun && !m_Cts.Token.IsCancellationRequested)
            {
                if (DaemonGlobal.s_DaemonProcessInfo != null)
                {
                    try
                    {
                        if (DaemonGlobal.s_IsServerConnected && DaemonGlobal.s_ServerRemoteGateway != null)
                        {
                            DaemonGlobal.s_DaemonProcessInfo.ConnectUsercount = DaemonGlobal.s_ClientCommunicationProcess.GetConnectionUserCount;
                            DaemonGlobal.s_DaemonProcessInfo.TelnetSessionCount = DaemonGlobal.s_TelnetProcessor.GetTelnetSessionCount;
                            var tRequestProcess = new DaemonProcessInfo(DaemonGlobal.s_DaemonProcessInfo);
                            
                            RemoteClientMethod tRemoteClientMethod = (RemoteClientMethod)DaemonGlobal.s_ServerRemoteGateway.ServerObject;
                            if (tRemoteClientMethod != null)
                            {
                                tRemoteClientMethod.CallDaemonStatusUpdateRequestMethod(ObjectConverter.GetBytes(tRequestProcess));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        DaemonGlobal.s_FileLogProcess.PrintLog("SendDaemonStatus 오류 Exception : " + ex.Message);
                    }
                }
                
                if (m_Cts.Token.WaitHandle.WaitOne(2000)) break;
            }
        }

        public bool LoadSystemInfo()
        {
            DaemonGlobal.s_FileLogProcess.PrintLog("Daemon 설정 정보를 로드 합니다.");
            try
            {
                FileInfo tConfigFile = new FileInfo(Application.StartupPath + "\\" + DaemonConfig.s_DaemonConfigFileName);
                if (!tConfigFile.Exists)
                {
                    MKXML.ObjectToXML(tConfigFile.FullName, new DaemonConfig());
                    DaemonGlobal.s_FileLogProcess.PrintLog(E_FileLogType.Info, string.Format("환경정보 파일이 없어 새로 생성하였습니다({0})", tConfigFile.FullName));
                    return false;
                }
                ArrayList tSystemInfos = MKXML.ObjectFromXML(tConfigFile.FullName, typeof(DaemonConfig), out E_XmlError tXmlError);
                if (tSystemInfos == null || tSystemInfos.Count == 0) return false;
                
                DaemonGlobal.s_DaemonConfig = (DaemonConfig)tSystemInfos[0];
                DaemonGlobal.s_DaemonConfig.DaemonPort = m_DaemonPort;
                
                if (m_DaemonPort == 0)
                {
                    DaemonGlobal.s_FileLogProcess.PrintLog(E_FileLogType.Warning, "[환경설정 오류] 데몬 접속 포트가 설정되지 않았습니다.");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                DaemonGlobal.s_FileLogProcess.PrintLog(E_FileLogType.Error, ex.ToString());
                return false;
            }
        }

        private void SendTelnetResult()
        {
            while (DaemonGlobal.s_IsRun && !m_Cts.Token.IsCancellationRequested)
            {
                TelnetCommandResultInfo tResult = null;
                try
                {
                    if (DaemonGlobal.s_TelnetProcessor.m_ResultQueue.Count == 0)
                    {
                        DaemonGlobal.s_TelnetProcessor.m_ResultMRE.Reset();
                        // 대기 중 취소 체크
                        if (WaitHandle.WaitAny(new[] { DaemonGlobal.s_TelnetProcessor.m_ResultMRE, m_Cts.Token.WaitHandle }) == 1) break;
                    }

                    lock (DaemonGlobal.s_TelnetProcessor.m_ResultQueue)
                    {
                        if (DaemonGlobal.s_TelnetProcessor.m_ResultQueue.Count > 0)
                            tResult = DaemonGlobal.s_TelnetProcessor.m_ResultQueue.Dequeue();
                    }

                    if (tResult != null)
                    {
                        DaemonGlobal.SendResultClient(tResult);
                    }
                }
                catch (OperationCanceledException) { break; }
                catch (Exception ex)
                {
                    DaemonGlobal.s_FileLogProcess.PrintLog(E_FileLogType.Error, "SendTelnetResult 오류: " + ex.Message);
                }
            }
        }

        private void StartServerProcessResult()
        {
            m_ProcessServerResultThread = new Thread(ProcessServerResult) { IsBackground = true };
            m_ProcessServerResultThread.Start();
        }

        private void ProcessServerResult()
        {
            try
            {
                foreach (byte[] tData in m_ResultCollection.GetConsumingEnumerable(m_Cts.Token))
                {
                    try
                    {
                        object tObject = ObjectConverter.GetObject(tData);
                        if (tObject is ResultCommunicationData tResult)
                        {
                            if (tResult.OwnerKey != 0)
                            {
                                ISenderObject tSender = null;
                                lock (DaemonGlobal.s_SenderList)
                                {
                                    if (DaemonGlobal.s_SenderList.ContainsKey(tResult.OwnerKey))
                                        tSender = (ISenderObject)DaemonGlobal.s_SenderList[tResult.OwnerKey];
                                }
                                tSender?.ResultReceiver(tResult);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("ProcessServerResult 분석 오류: " + ex.Message);
                    }
                }
            }
            catch (OperationCanceledException) { /* 종료 정상 단계 */ }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ProcessServerResult 메인 루프 오류: " + ex.ToString());
            }
        }

        private void StartRequestSend()
        {
            m_RequestSendThread = new Thread(ProcessRequestSendToServer) { IsBackground = true };
            m_RequestSendThread.Start();
        }

        private void StopRequestSend()
        {
            if (m_RequestSendThread == null) return;
            if (!m_RequestSendThread.Join(100))
            {
                // 강제 종료 지양 (Cts로 제어 유도)
            }
            m_RequestSendThread = null;
        }

        private void ProcessRequestSendToServer()
        {
            while (DaemonGlobal.s_IsRun && !m_Cts.Token.IsCancellationRequested)
            {
                object tSendObject = null;
                try
                {
                    lock (DaemonGlobal.s_RequestQueue)
                    {
                        if (DaemonGlobal.s_RequestQueue.Count > 0)
                        {
                            tSendObject = DaemonGlobal.s_RequestQueue.Dequeue();
                        }
                    }

                    if (tSendObject != null && DaemonGlobal.s_ServerRemoteGateway != null)
                    {
                        RemoteClientMethod tRemoteClientMethod = (RemoteClientMethod)DaemonGlobal.s_ServerRemoteGateway.ServerObject;
                        tRemoteClientMethod?.CallRequestMethod(ObjectConverter.GetBytes(tSendObject));
                    }
                }
                catch (Exception) { }

                if (m_Cts.Token.WaitHandle.WaitOne(1)) break;
            }
        }

        private void StartGetResult()
        {
            m_GetResultThread = new Thread(ProcessGetResultFromServer) { IsBackground = true };
            m_GetResultThread.Start();
        }

        private void ProcessGetResultFromServer()
        {
            int tResultFailCount = 0;
            while (DaemonGlobal.s_IsRun && !m_Cts.Token.IsCancellationRequested)
            {
                if (DaemonGlobal.s_IsServerConnected && DaemonGlobal.s_ServerRemoteGateway != null)
                {
                    try
                    {
                        RemoteClientMethod tRemoteClientMethod = (RemoteClientMethod)DaemonGlobal.s_ServerRemoteGateway.ServerObject;
                        if (tRemoteClientMethod != null && DaemonGlobal.s_DaemonProcessInfo != null)
                        {
                            byte[] tResultDatas = tRemoteClientMethod.CallDaemonResultMethod(DaemonGlobal.s_DaemonProcessInfo.DaemonID);
                            if (tResultDatas != null)
                            {
                                ArrayList tResultData = (ArrayList)ObjectConverter.GetObject(tResultDatas);
                                if (tResultData != null && tResultData.Count > 0)
                                {
                                    foreach (object item in tResultData)
                                    {
                                        if (item is byte[] packet)
                                            m_ResultCollection.Add(packet, m_Cts.Token);
                                    }
                                }
                                tResultFailCount = 0;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        tResultFailCount++;
                        if (tResultFailCount > 3)
                        {
                            DaemonGlobal.s_IsServerConnected = false;
                            TryServerConnect();
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

                if (m_Cts.Token.WaitHandle.WaitOne(100)) break;
            }
        }

        private void TryServerConnect()
        {
            for (int i = 0; i < 3; i++)
            {
                if (m_Cts.Token.IsCancellationRequested) break;
                if (DaemonGlobal.s_ServerCommunicationProcess != null && DaemonGlobal.s_ServerCommunicationProcess.Start())
                {
                    break;
                }
                if (m_Cts.Token.WaitHandle.WaitOne(5000)) break;
            }
        }

        #region ISenderObject 구현
        public void ResultReceiver(ResultCommunicationData vResult) { /* DaemonProcess는 분배자 역할 */ }
        public void ResultReceiver(CommandResultItem vResult) { /* DaemonProcess는 분배자 역할 */ }
        #endregion

        #region IDisposable 구현
        public void Dispose()
        {
            if (m_Disposed) return;

            m_Cts.Cancel();

            try
            {
                m_ResultCollection.CompleteAdding();

                DaemonGlobal.StopThread(m_TelnetResultSenderThread);
                DaemonGlobal.StopThread(m_ProcessServerResultThread);
                DaemonGlobal.StopThread(m_GetResultThread);
                DaemonGlobal.StopThread(m_DaemonStatusUpdateThread);
                DaemonGlobal.StopThread(m_RequestSendThread);

                if (DaemonGlobal.s_TelnetProcessor != null) DaemonGlobal.s_TelnetProcessor.Dispose();
                if (DaemonGlobal.s_ClientCommunicationProcess != null) DaemonGlobal.s_ClientCommunicationProcess.Stop();
                if (DaemonGlobal.s_KamServerCommunicationProcess != null) DaemonGlobal.s_KamServerCommunicationProcess.Stop();
                if (DaemonGlobal.s_ServerRemoteGateway != null) DaemonGlobal.s_ServerRemoteGateway.Dispose();
                
                while (m_ResultCollection.Count > 0) m_ResultCollection.TryTake(out _);
                m_ResultCollection.Dispose();
                m_Cts.Dispose();
            }
            catch { }

            m_Disposed = true;
        }
        #endregion
    }
}
