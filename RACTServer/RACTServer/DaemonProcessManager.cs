using System;
using System.Collections.Generic;
using System.Text;
using RACTDaemonProcess;
using System.Collections;
using MKLibrary.MKData;
using System.IO;
using RACTCommonClass;
using MKLibrary.MKNetwork;
using System.Threading;

namespace RACTServer
{
    public class DaemonProcessManager
    {
        /// <summary>
        /// daemon과 통신할 게이트웨이 입니다.
        /// </summary>
        private MKRemote m_RemoteGateway;
        /// <summary>
        /// 클라이언트 요청을 저장할 큐 입니다.
        /// </summary>
        private Queue<RequestCommunicationData> m_RequestQueue;
        /// <summary>
        /// 요청 처리 스레드 입니다.
        /// </summary>
        private Thread m_RequestProcessThread = null;
        /// <summary>
        /// 접속된 사용자, 데몬의 연결 상태를 확인 합니다.
        /// </summary>
        private Thread m_DaemonHelathCheckThread = null;
        /// <summary>
        /// Daemon 목록 입니다.
        /// </summary>
        private DaemonProcessInfoCollection m_DaemonProcessList;
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public DaemonProcessManager()
        {
            GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "Daemon Manager를 시작 합니다.");
            m_DaemonProcessList = new DaemonProcessInfoCollection();

            m_DaemonHelathCheckThread = new Thread(new ThreadStart(HealthCheckProcess));
            m_DaemonHelathCheckThread.Start();
        }

        private void HealthCheckProcess()
        {
            DaemonProcessInfo tDaemonInfo;
            double tTotalSeconds = 0;
            DateTime tNow = DateTime.Now;
            while (GlobalClass.m_IsRun)
            {
                try
                {
                    tNow = DateTime.Now;
                    lock (m_DaemonProcessList)
                    {
                        for (int i = m_DaemonProcessList.Count - 1; i > -1; i--)
                        {
                            tDaemonInfo = null;
                            tTotalSeconds = 0;


                            tDaemonInfo = (DaemonProcessInfo)m_DaemonProcessList.InnerList[i];

                            if (tDaemonInfo != null)
                            {
                                tTotalSeconds = ((TimeSpan)tNow.Subtract(tDaemonInfo.LifeTime)).TotalSeconds;
                                if (tTotalSeconds >= GlobalClass.s_HealthCheckTimeOut)
                                {
                                    Thread.Sleep(100);
                                    tTotalSeconds = ((TimeSpan)tNow.Subtract(tDaemonInfo.LifeTime)).TotalSeconds;
                                    if (tTotalSeconds >= GlobalClass.s_HealthCheckTimeOut)
                                    {
                                        // 2013-04-26-shinyn - 데몬 연결이 끊겼을 경우 로그 저장
                                        GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, string.Concat("@@@", tDaemonInfo.IP, ":", tDaemonInfo.Port, " => Before Life Time  :", tDaemonInfo.LifeTime.ToString("yyyy-MM-dd HH:mm:ss"), " Total Seconds :", tTotalSeconds));
                                        m_DaemonProcessList.RemoveAt(i);
                                    }
                                }

                                // 2013-03-07 - shinyn - 데몬이 세션 타임아웃을 체크하고, 데몬을 삭제한다. 데몬 체크하는 로그를 계속 보여준다.
                                System.Diagnostics.Debug.WriteLine(string.Concat("@@@", tDaemonInfo.IP, ":", tDaemonInfo.Port, " => Before Life Time  :", tDaemonInfo.LifeTime.ToString("yyyy-MM-dd HH:mm:ss"), " Total Seconds :", tTotalSeconds));
                                tDaemonInfo = null;
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, ex.ToString());
                }
                Thread.Sleep(1000);
            }
        }

        internal bool Start()
        {
            //DaemonProcess tDaemonProcess;

            //GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, GlobalClass.m_SystemInfo.DefaultDaemonRunCount.ToString() +"개의 Default Daemon을 실행 합니다.");
            //for (int i = 0; i < GlobalClass.m_SystemInfo.DefaultDaemonRunCount; i++)
            //{
            //    tDaemonProcess = new DaemonProcess(GlobalClass.m_SystemInfo.ServerIP, GlobalClass.m_SystemInfo.ServerPort, GlobalClass.m_SystemInfo.ServerChannel, GlobalClass.m_SystemInfo.ServerIP, GlobalClass.m_SystemInfo.DaemonUsePort + i);
            //    if (!tDaemonProcess.StartDaemon())
            //    {
            //        tDaemonProcess.Stop();
            //    }
            //}


            int tCount = 0;
            string tResult = string.Empty;
            RemoteClientMethod tRemoteMethod = null;
            try
            {
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "데몬 채널을 생성 합니다.");
                m_RemoteGateway = new MKRemote(E_RemoteType.TCPRemote, GlobalClass.m_SystemInfo.ServerIP, GlobalClass.m_SystemInfo.DaemonUsePort, GlobalClass.m_SystemInfo.DaemonChannelName);
                while (tCount < 10)
                {
                    if (m_RemoteGateway.StartRemoteServer(out tResult) != E_RemoteError.Success)
                    {
                        GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, string.Concat("데몬 채널을 생성할 수 없습니다. : ", tResult));
                        Thread.Sleep(3000);
                        tCount++;
                    }
                    else
                        break;
                }

                if (m_RemoteGateway == null)
                {
                    GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, string.Format("데몬 IP: {0}  PortNo: {1}  ChannelName: {2}에 연결 할 수 없습니다.", GlobalClass.m_SystemInfo.ServerIP, GlobalClass.m_SystemInfo.DaemonUsePort, GlobalClass.m_SystemInfo.DaemonChannelName));
                    return false;
                }

                //Daemon 원격 메소드를 설정합니다.
                tRemoteMethod = new RemoteClientMethod();
                tRemoteMethod.SetDaemonResultHandler(DaemonResultSender);
                tRemoteMethod.SetDaemonConnectHandler(DaemonConnectReceiver);
                tRemoteMethod.SetTelnetSessionIDRequestHandler(TelnetSessionIdRequestReceiver);
                tRemoteMethod.SetTelnetConnectionUpdateRequestHandler(TelnetConnectionUpdateReceiver);
                tRemoteMethod.SetDaemonStatusUpdateRequestHandler(UpdateDaemonStatus);
                tRemoteMethod.SetUserLogOutHandler(DaemonLogOutReceiver);
                tRemoteMethod.SetHealthCheckRequestHandler(HealthCheckUpdateReceiver);
                m_RemoteGateway.ServerObject = tRemoteMethod;

                return true;
            }
            catch (Exception ex)
            {
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, ex.ToString());
                return false;
            }

        }

        /// <summary>
        /// Life Time을 변경 합니다.
        /// </summary>
        /// <param name="aClientID"></param>
        private void HealthCheckUpdateReceiver(int aClientID)
        {
            lock (m_DaemonProcessList)
            {
                if (m_DaemonProcessList.Contains(aClientID))
                {
                    System.Diagnostics.Debug.WriteLine(string.Concat("###", m_DaemonProcessList[aClientID].IP, ":", m_DaemonProcessList[aClientID].Port, " => Before Life Time  :", m_DaemonProcessList[aClientID].LifeTime.ToString("yyyy-MM-dd HH:mm:ss"), " After Life Time :", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    m_DaemonProcessList[aClientID].LifeTime = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// 데몬 종료 처리 합니다.
        /// </summary>
        /// <param name="aDaemonID"></param>
        private void DaemonLogOutReceiver(int aDaemonID)
        {
            lock (GlobalClass.s_DaemonProcessManager.DaemonProcessList)
            {
                DaemonProcessInfo tDaemonInfo = GlobalClass.s_DaemonProcessManager.DaemonProcessList[aDaemonID];
                if (tDaemonInfo != null)
                {
                    GlobalClass.s_DaemonProcessManager.DaemonProcessList.Remove(tDaemonInfo);
                }
            }
        }
        /// <summary>
        /// 데몬 접속 요청을 처리 합니다.
        /// </summary>
        /// <param name="aClientID"></param>
        private byte[] DaemonConnectReceiver(string aIP, int aPort, string aChannelName)
        {
            DaemonLoginResultInfo tLoginResult = new DaemonLoginResultInfo();
            DaemonProcessInfo tAlreadyLoginInfo = null;
            try
            {
                DaemonProcessInfo tDaemonInfo = null;
                tDaemonInfo = new DaemonProcessInfo();
                tDaemonInfo.IP = aIP;
                tDaemonInfo.Port = aPort;
                tDaemonInfo.ChannelName = aChannelName + aPort;
                tDaemonInfo.LifeTime = DateTime.Now;
                bool tIsAlreadyLogin = false;
                lock (GlobalClass.s_DaemonProcessManager)
                {
                    foreach (DaemonProcessInfo tmpUserInfo in GlobalClass.s_DaemonProcessManager.DaemonProcessList)
                    {
                        if (tmpUserInfo.IP == aIP && tmpUserInfo.Port == aPort)
                        {
                            tIsAlreadyLogin = true;
                            tAlreadyLoginInfo = tmpUserInfo;
                            break;
                        }
                    }
                }

                if (tIsAlreadyLogin)
                {
                    GlobalClass.s_DaemonProcessManager.DaemonProcessList.Remove(tAlreadyLoginInfo);
                }


                // 사용자 정보를 해시 테이블에 추가 합니다.
                lock (GlobalClass.s_DaemonProcessManager.DaemonProcessList)
                {
                    GlobalClass.m_LogProcess.PrintLog(string.Concat("데몬 접속 ", tDaemonInfo.IP, ":", tDaemonInfo.Port, " ", tDaemonInfo.ChannelName));
                    GlobalClass.s_DaemonProcessManager.DaemonProcessList.Add(tDaemonInfo);
                }
                tLoginResult.DaemonInfo = tDaemonInfo;
                tLoginResult.ClientID = tDaemonInfo.DaemonID;
                tLoginResult.LoginResult = E_LoginResult.Success;
                return ObjectConverter.GetBytes(tLoginResult);

            }
            catch (Exception ex)
            {
                tLoginResult.LoginResult = E_LoginResult.UnknownError;
                return ObjectConverter.GetBytes(tLoginResult);
            }
        }
        /// <summary>
        /// 텔넷 세션 ID 요청을 처리 합니다.
        /// </summary>
        /// <param name="aQuery"></param>
        /// <returns></returns>
        private int TelnetSessionIdRequestReceiver(string aQuery)
        {
            return GlobalClass.m_DBLogProcess.ExcuteQuery(aQuery);
        }

        /// <summary>
        /// DB 접속 정보를 Update 합니다.(종료 처리)
        /// </summary>
        /// <param name="aSessionID"></param>
        /// <returns></returns>
        private bool TelnetConnectionUpdateReceiver(int aSessionID)
        {
            MKDBWorkItem tDBWI = GlobalClass.m_DBPool.GetDBWorkItem();
            try
            {
                //string tQueryMessage = string.Format("update RACT_LOG_DeviceConnection set ConnectLogType = 1, DisconnectTime = '{0}' where id ={1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), aSessionID);
                string tQueryMessage = string.Format("update RACT_LOG_DeviceConnection set ConnectLogType = 1, DisconnectTime = GETDATE() where id ={0}", aSessionID);
                if (tDBWI.ExecuteNoneQuery(tQueryMessage) == E_DBProcessError.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 데몬 결과 요청을 처리합니다.
        /// </summary>
        /// <param name="aClientID"></param>
        private byte[] DaemonResultSender(int aDaemonID)
        {
            byte[] tResult = null;
            ArrayList tResults = null;
            int tResultCount = 0;
            DaemonProcessInfo tDaemonProcessInfo;


            if (!m_DaemonProcessList.Contains(aDaemonID)) return null;

            lock (m_DaemonProcessList)
            {
                tDaemonProcessInfo = (DaemonProcessInfo)GlobalClass.s_DaemonProcessManager.DaemonProcessList[aDaemonID];
            }
            if (tDaemonProcessInfo == null) return null;

            lock (tDaemonProcessInfo.DataQueue.SyncRoot)
            {
                if (tDaemonProcessInfo.DataQueue.Count > 0)
                {
                    tResults = new ArrayList();
                    while (tDaemonProcessInfo.DataQueue.Count > 0)
                    {
                        if (tResultCount >= 200) break;
                        tResults.Add(tDaemonProcessInfo.DataQueue.Dequeue());
                        tResultCount++;
                    }
                    tResult = (byte[])ObjectConverter.GetBytes(tResults);
                }
            }
            return tResult;
        }

        internal bool ContainsKey(int aKey)
        {
            return m_DaemonProcessList.Contains(aKey);
        }

        /// <summary>
        /// 마지막으로 전송한 데몬 접속 정보 입니다.
        /// </summary>
        private DaemonProcessInfo m_LastSendDaemonInfo = null;

        /// <summary>
        /// 사용할 수 있는 데몬을 가져오기 합니다.
        /// </summary>
        /// <returns></returns>
        internal DaemonProcessInfo GetDaemonProcess(UseableDaemonRequestInfo aRequestInfo)
        {

            lock (m_DaemonProcessList)
            {
                if (m_DaemonProcessList.Count == 0) return null;

                DaemonProcessInfo tInfo = null;
                if (m_LastSendDaemonInfo != null)
                {
                    if (!m_DaemonProcessList.Contains(m_LastSendDaemonInfo.DaemonID))
                    {
                        m_LastSendDaemonInfo = null;
                    }
                }
                for (int i = m_DaemonProcessList.InnerList.Count - 1; i > -1; i--)
                {

                    tInfo = (DaemonProcessInfo)m_DaemonProcessList.InnerList[i];

                    // SSH터널링용 데몬서버는 대상에서 제외한다.
                    if (GlobalClass.m_SystemInfo.IsSSHTunnelDaemonIP(tInfo.IP)) continue;

                    if (m_LastSendDaemonInfo == null)
                    {
                        if (!aRequestInfo.DisconnectDaemonList.Contains(tInfo.DaemonID))
                        {
                            m_LastSendDaemonInfo = tInfo;
                        }
                    }

                    if (m_LastSendDaemonInfo.DaemonID != tInfo.DaemonID && !aRequestInfo.DisconnectDaemonList.Contains(tInfo.DaemonID))
                    {
                        if (m_LastSendDaemonInfo.TelnetSessionCount > tInfo.TelnetSessionCount)
                        {
                            m_LastSendDaemonInfo = tInfo;
                        }
                    }
                }
            }
            GlobalClass.m_LogProcess.PrintLog(string.Concat("접속 데몬 정보 전송", m_LastSendDaemonInfo.IP, ":", m_LastSendDaemonInfo.Port));
            return m_LastSendDaemonInfo;

            //lock (m_DaemonProcessList)
            //{
            //    if (m_DaemonProcessList.Count == 0) return null;

            //    DaemonProcessInfo tInfo = null;
            //    if(m_LastSendDaemonInfo != null)
            //    {
            //        if (!m_DaemonProcessList.Contains(m_LastSendDaemonInfo.DaemonID))
            //        {
            //            m_LastSendDaemonInfo = null;
            //        }
            //    }
            //    /*
            //    for(int i = m_DaemonProcessList.InnerList.Count -1; i > -1;i--)
            //    {
                    
            //        tInfo =(DaemonProcessInfo) m_DaemonProcessList.InnerList[i];
            //        if (m_LastSendDaemonInfo == null)
            //        {
            //            if (!aRequestInfo.DisconnectDaemonList.Contains(tInfo.DaemonID))
            //            {
            //                m_LastSendDaemonInfo = tInfo;
            //            }
            //        }

            //        if (m_LastSendDaemonInfo.DaemonID != tInfo.DaemonID && !aRequestInfo.DisconnectDaemonList.Contains(tInfo.DaemonID))
            //        {
            //            if (m_LastSendDaemonInfo.TelnetSessionCount > tInfo.TelnetSessionCount)
            //            {
            //                m_LastSendDaemonInfo = tInfo;
            //            }
            //        }
            //    }
            //    */
            //    /*
            //    for (int i = m_DaemonProcessList.InnerList.Count - 1; i > -1; i--)
            //    {

            //        tInfo = (DaemonProcessInfo)m_DaemonProcessList.InnerList[i];
            //        if (m_LastSendDaemonInfo == null)
            //        {
            //            if (!aRequestInfo.DisconnectDaemonList.Contains(tInfo.DaemonID))
            //            {
            //                m_LastSendDaemonInfo = tInfo;
            //            }
            //        }
            //        // 2013-03-08 - shinyn - null이 아닌경우에만 비교하도록 수정
            //        if (!aRequestInfo.DisconnectDaemonList.Contains(tInfo.DaemonID))
            //        {
            //            if (m_LastSendDaemonInfo.DaemonID != tInfo.DaemonID)
            //            {
            //                if (m_LastSendDaemonInfo.TelnetSessionCount > tInfo.TelnetSessionCount)
            //                {
            //                    m_LastSendDaemonInfo = tInfo;
            //                }
            //            }
            //        }

                    
            //    }
            //    */
            //    // 2013-04-26- shinyn - 데몬세션이 잘 열리지 않는 원인을 찾기위해 로그를 저장하도록 수정
            //    for (int i = m_DaemonProcessList.InnerList.Count - 1; i > -1; i--)
            //    {

            //        tInfo = (DaemonProcessInfo)m_DaemonProcessList.InnerList[i];
            //        if (m_LastSendDaemonInfo == null)
            //        {
            //            if (!aRequestInfo.DisconnectDaemonList.Contains(tInfo.DaemonID))
            //            {
            //                m_LastSendDaemonInfo = tInfo;
            //            }
            //        }
            //        // 2013-03-08 - shinyn - null이 아닌경우에만 비교하도록 수정
            //        if (!aRequestInfo.DisconnectDaemonList.Contains(tInfo.DaemonID))
            //        {
            //            if (m_LastSendDaemonInfo.DaemonID != tInfo.DaemonID)
            //            {
            //                if (m_LastSendDaemonInfo.TelnetSessionCount > tInfo.TelnetSessionCount)
            //                {
            //                    m_LastSendDaemonInfo = tInfo;
            //                }
            //            }
            //        }
            //    }

            //    // 2013-04-26- shinyn - 데몬세션이 잘 열리지 않는 원인을 찾기위해 로그를 저장하도록 수정
            //    string tDaemonLog = "GetDaemonProcess Daemon IP : " + m_LastSendDaemonInfo.IP +
            //                       " Daemon Port : " + m_LastSendDaemonInfo.Port.ToString() +
            //                       " Telnet Session Count : " + m_LastSendDaemonInfo.TelnetSessionCount.ToString();
            //    System.Diagnostics.Debug.WriteLine(tDaemonLog);
            //    GlobalClass.m_LogProcess.PrintLog(tDaemonLog);



            //}
            //GlobalClass.m_LogProcess.PrintLog(string.Concat("접속 데몬 정보 전송", m_LastSendDaemonInfo.IP, ":", m_LastSendDaemonInfo.Port));
            //return m_LastSendDaemonInfo;
        }
        /// <summary>
        /// 사용할 수 있는 데몬을 가져오기 합니다.
        /// </summary>
        /// <returns></returns>
        internal DaemonProcessInfo GetDaemonProcess()
        {
            lock (m_DaemonProcessList)
            {
                if (m_DaemonProcessList.Count == 0) return null;

                DaemonProcessInfo tInfo = null;
                for (int i = m_DaemonProcessList.InnerList.Count - 1; i > -1; i--)
                {
                    tInfo = (DaemonProcessInfo)m_DaemonProcessList.InnerList[i];

                    // SSH터널링용 데몬서버는 대상에서 제외한다.
                    if (GlobalClass.m_SystemInfo.IsSSHTunnelDaemonIP(tInfo.IP)) continue;

                    if (m_LastSendDaemonInfo == null)
                    {
                        m_LastSendDaemonInfo = tInfo;
                    }

                    if (m_LastSendDaemonInfo.DaemonID != tInfo.DaemonID)
                    {
                        if (m_LastSendDaemonInfo.TelnetSessionCount + m_LastSendDaemonInfo.TempSessionCount > tInfo.TelnetSessionCount + tInfo.TempSessionCount)
                        {
                            m_LastSendDaemonInfo = tInfo;
                        }
                    }
                }
            }
            m_LastSendDaemonInfo.TempSessionCount++;
            return m_LastSendDaemonInfo;
        }

        /// <summary>
        /// KwonTaeSuk, 2018.12, [c-RPCS과제] 
        /// 사용할 수 있는 SSH터널링 데몬정보를 얻습니다.
        /// 2019.09.25 KwonTaeSuk LTE데몬할당된 다음 일반 데몬 선택시 LTE데몬 할당되는 문제: m_LastSendDaemonInfo 미사용 처리
        /// </summary>
        /// <returns>데몬정보</returns>
        internal DaemonProcessInfo GetSSHTunnelDaemonProcess()
        {
            string resultStr = string.Empty;
            DaemonProcessInfo tDaemonInfo = null;

            lock (m_DaemonProcessList)
            {
                if (m_DaemonProcessList.Count == 0)
                {
                    GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Warning, "[DaemonProcessManager.GetSSHTunnelDaemonProcess] 사용가능한 데몬프로세스가 없습니다.\r\n데몬의 기동여부 확인이 필요합니다.");
                    return tDaemonInfo;
                }

                if (GlobalClass.m_SystemInfo.GetSSHTunnelDaemonIPCount() == 0)
                {
                    GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Warning, "[DaemonProcessManager.GetSSHTunnelDaemonProcess] SSH터널링 데몬서버IP가 설정되지 않았습니다.\r\n환경설정(System.xml)파일의 <SSHTunnelDaemonIP>에 터널링 서버IP를 추가해야합니다(여러개인 경우 쉼표(,)로 연결)");
                    return tDaemonInfo;
                }

                DaemonProcessInfo tTempDaemon = null;
                for (int i = m_DaemonProcessList.InnerList.Count - 1; i > -1; i--)
                {
                    tTempDaemon = (DaemonProcessInfo)m_DaemonProcessList.InnerList[i];

                    /// SSH터널링 데몬서버IP가 아니면 skip
                    if (!GlobalClass.m_SystemInfo.IsSSHTunnelDaemonIP(tTempDaemon.IP)) continue;

                    if (tDaemonInfo == null)
                    {
                        tDaemonInfo = tTempDaemon;
                        continue;
                    }

                    /// 접속세션 수가 적은 데몬을 선택
                    if (tDaemonInfo.TelnetSessionCount + tDaemonInfo.TempSessionCount > tTempDaemon.TelnetSessionCount + tTempDaemon.TempSessionCount)
                    {
                        tDaemonInfo = tTempDaemon;
                    }
                } // End of for

                if (tDaemonInfo != null) tDaemonInfo.TempSessionCount++;
            }

            if (tDaemonInfo == null) {
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Warning, string.Format("[DaemonProcessManager.GetSSHTunnelDaemonProcess]SSH터널링을 위한 데몬이 없습니다[ServerIP={0}]", GlobalClass.m_SystemInfo.SSHTunnelDaemonIP));
            }
            
            return tDaemonInfo;
        }

        /// <summary>
        /// 데몬 상태를 갱신 합니다.
        /// </summary>
        /// <param name="DaemonProcessInfo"></param>
        internal void UpdateDaemonStatus(byte[] aInfo)
        {
            DaemonProcessInfo tInfo = (DaemonProcessInfo)ObjectConverter.GetObject(aInfo);
            if (tInfo == null) return;
            tInfo.LifeTime = DateTime.Now;

            lock (m_DaemonProcessList)
            { 
                if (m_DaemonProcessList.Contains(tInfo.DaemonID))
                {
                    System.Diagnostics.Debug.WriteLine(string.Concat("###", tInfo.IP, ":", tInfo.Port, " => User Count :", tInfo.ConnectUsercount, " Session Count :", tInfo.TelnetSessionCount));
                    // System.Diagnostics.Debug.WriteLine(string.Concat("###", m_DaemonProcessList[tInfo.DaemonID].IP, ":", m_DaemonProcessList[tInfo.DaemonID].Port, " => Before Life Time  :", m_DaemonProcessList[tInfo.DaemonID].LifeTime.ToString("yyyy-MM-dd HH:mm:ss"), " After Life Time :", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    m_DaemonProcessList[tInfo.DaemonID] = tInfo;
                    if (m_LastSendDaemonInfo != null && m_LastSendDaemonInfo.DaemonID == tInfo.DaemonID)
                    {
                        m_LastSendDaemonInfo = tInfo;
                    }
                }
                else
                {
                    m_DaemonProcessList.Add(tInfo);
                }
            }
        }

        /// <summary>
        /// Daemon 목록 가져오거나 설정 합니다.
        /// </summary>
        public DaemonProcessInfoCollection DaemonProcessList
        {
            get { return m_DaemonProcessList; }
            set { m_DaemonProcessList = value; }
        }

        internal void Stop()
        {
            if (m_RemoteGateway != null)
            {
                string tstring="";
                
                m_RemoteGateway.Dispose();

                m_RemoteGateway = null;
            }
            if (m_RequestQueue != null)
            {
                lock (m_RequestQueue)
                {
                    m_RequestQueue.Clear();
                }
            }

            if (m_RequestProcessThread != null && m_RequestProcessThread.IsAlive)
            {
                try
                {
                    m_RequestProcessThread.Abort();
                }
                catch { }
            }
            if (m_DaemonHelathCheckThread != null && m_DaemonHelathCheckThread.IsAlive)
            {
                try
                {
                    m_DaemonHelathCheckThread.Abort();
                }
                catch { }
            }
        }

        /// <summary>
        /// 임시 접속 개수를 초기화 합니다.
        /// </summary>
        internal void TempConnectionListClear()
        {
            for (int i = m_DaemonProcessList.InnerList.Count - 1; i > -1; i--)
            {
                ((DaemonProcessInfo)m_DaemonProcessList.InnerList[i]).TempSessionCount = 0;
            }
        }
    }
}
    

