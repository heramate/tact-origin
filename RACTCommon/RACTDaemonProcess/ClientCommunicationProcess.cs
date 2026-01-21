using System;
using System.Collections.Generic;
using System.Text;
using MKLibrary.MKNetwork;
using RACTCommonClass;
using System.Threading;
using System.Collections;
using TelnetProcessor;

namespace RACTDaemonProcess
{
    public class ClientCommunicationProcess
    {
        /// <summary>
        /// 클라이언트와 통신할 게이트웨이 입니다.
        /// </summary>
        private MKRemote m_RemoteGateway;
        /// <summary>
        /// 접속한 사용자 목록이 저장 됩니다.
        /// </summary>
        private UserInfoCollection m_UserInfoList = new UserInfoCollection();
        /// <summary>
        /// 클라이언트 요청을 저장할 큐 입니다.
        /// </summary>
        private Queue<RequestCommunicationData> m_RequestQueue;
        /// <summary>
        /// 요청 대기 입니다.
        /// </summary>
        private ManualResetEvent m_ReuqestMRE = new ManualResetEvent(false);
        /// <summary>
        /// 요청 처리 스레드 입니다.
        /// </summary>
        private Thread m_RequestProcessThread = null;
        /// <summary>
        /// 접속된 사용자, 데몬의 연결 상태를 확인 합니다.
        /// </summary>
        private Thread m_HelathCheckThread = null;

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public ClientCommunicationProcess()
        {
            m_RequestQueue = new Queue<RequestCommunicationData>();

            m_RequestProcessThread = new Thread(new ThreadStart(ProcessClientRequest));
            m_RequestProcessThread.Start();

            m_HelathCheckThread = new Thread(new ThreadStart(HealthCheckProcess));
            m_HelathCheckThread.Start();
        }

        /// <summary>
        /// 접속된 사용자, 데몬의 연결 상태를 확인 합니다.
        /// </summary>
        private void HealthCheckProcess()
        {
            UserInfo tUserInfo;
            double tTotalSeconds = 0;
            DateTime tNow = DateTime.Now;
            List<int> tUserIDList = new List<int>();
            bool tExist = false;
            while (DaemonGlobal.s_IsRun)
            {
                try
                {
                    tUserInfo = null;
                    tTotalSeconds = 0;
                    tNow = DateTime.Now;
                    tUserIDList.Clear();

                    // 2013-04-26- shinyn - 사용자 세션 타임아웃시 세션 제거한다.
                    for (int i = m_UserInfoList.Count - 1; i > -1; i--)
                    {

                        lock (m_UserInfoList)
                        {
                            tUserInfo = (UserInfo)m_UserInfoList.InnerList[i];
                        }
                        if (tUserInfo == null) continue;
                        tTotalSeconds = ((TimeSpan)tNow.Subtract(tUserInfo.LifeTime)).TotalSeconds;
                        if (tTotalSeconds >= DaemonGlobal.s_HealthCheckTimeOut)
                        {
                            // 2013-04-26 - shinyn - 유저 세션 타임아웃 걸린경우 로그를 저장한다.
                            string tNowString = tNow.Year.ToString("0000") + "-" + 
                                                tNow.Month.ToString("00") + "-" +
                                                tNow.Day.ToString("00") + " " +
                                                tNow.Hour.ToString("00") + ":" +
                                                tNow.Minute.ToString("00") + ":"+
                                                tNow.Second.ToString("00");
                            string tCheckTime = tUserInfo.LifeTime.Year.ToString("0000") + "-" +
                                                tUserInfo.LifeTime.Month.ToString("00") + "-" +
                                                tUserInfo.LifeTime.Day.ToString("00") + " " +
                                                tUserInfo.LifeTime.Hour.ToString("00") + ":" +
                                                tUserInfo.LifeTime.Minute.ToString("00") + ":" +
                                                tUserInfo.LifeTime.Second.ToString("00");

                            DaemonGlobal.s_FileLogProcess.PrintLog(" HealthCheckProcess : 유저 세션 Disconnect : ClientID : "+ tUserInfo.ClientID.ToString() +
                                                                   " Account : " + tUserInfo.Account + 
                                                                   " TimeOut : " + DaemonGlobal.s_HealthCheckTimeOut.ToString() + 
                                                                   " 현재시간 : " + tNowString + 
                                                                   " 체크시간 : " + tCheckTime);

                            RemoveUserSession(tUserInfo);
                            continue;
                        }


                    }

                    // 2013-04-26- shinyn - Disconnect된 사용자세션에 해당하는 장비 세션을 제거한다.
                    if (DaemonGlobal.s_TelnetProcessor != null)
                    {
                        lock (DaemonGlobal.s_TelnetProcessor.RunTelnetSessionList)
                        {
                            foreach (TelnetSessionInfo tSessionInfo in DaemonGlobal.s_TelnetProcessor.RunTelnetSessionList)
                            {
                                tUserIDList.Add(tSessionInfo.ClientID);
                            }
                        }

                        foreach (int tClientID in tUserIDList)
                        {
                            tExist = false;
                            foreach (UserInfo tCheckUser in m_UserInfoList)
                            {
                                if (tCheckUser.ClientID == tClientID)
                                {
                                    tExist = true;
                                    break;
                                }
                            }

                            if (!tExist)
                            {
                                // 2013-04-26 - shinyn - Disconnect된 사용자세션에 해당하는 장비세션 Disconnect로그 저장
                                DaemonGlobal.s_FileLogProcess.PrintLog(" HealthCheckProcess : 사용자 세션 Disconnect에 따른 장비 세션 Disconnect : ClientID:" + tClientID.ToString());
                                DaemonGlobal.s_TelnetProcessor.RemoveUserSession(tClientID);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    DaemonGlobal.s_FileLogProcess.PrintLog(E_FileLogType.Error, "1 : " + ex.ToString());
                    DaemonGlobal.s_FileLogProcess.PrintLog(E_FileLogType.Error, "2 : " + ex.Source);
                    DaemonGlobal.s_FileLogProcess.PrintLog(E_FileLogType.Error, "3 : " + ex.StackTrace);
                    DaemonGlobal.s_FileLogProcess.PrintLog(E_FileLogType.Error, "4 : " + ex.Message);
                }
                Thread.Sleep(1000);
            }
        }
        /// <summary>
        /// 사용자 세션을 삭제 합니다.
        /// </summary>
        /// <param name="aUserInfo"></param>
        private void RemoveUserSession(UserInfo aUserInfo)
        {           
            try
            {
                DaemonGlobal.s_TelnetProcessor.RemoveUserSession(aUserInfo.ClientID);
            }
            catch (Exception ex)
            {
                DaemonGlobal.s_FileLogProcess.PrintLog(E_FileLogType.Error, ex.ToString());
            }

            //2015-10-20 hanjiyeon 세션 삭제 중 예외 오류 발생. 분리 처리.
            try
            {
                lock (m_UserInfoList)
                {
                    m_UserInfoList.Remove(aUserInfo.ClientID);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }            
        }

        /// <summary>
        /// 종료 처리 합니다.
        /// </summary>
        public void Stop()
        {
            string tErrorString="";
            m_RemoteGateway.StopServer(out tErrorString);
            m_RemoteGateway.Dispose();
            m_RequestQueue.Clear();
            m_RemoteGateway = null;

            DaemonGlobal.StopThread(m_RequestProcessThread);
            DaemonGlobal.StopThread(m_HelathCheckThread);
        }
        /// <summary>
        /// 클라이언트 프로세스를 시작합니다.
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            int tCount = 0;
            string tResult = string.Empty;
            RemoteClientMethod tRemoteMethod = null;
            try
            {
                // 2019.01.25 KwonTaeSuk 환경설정파일 정리(DaemonLauncherConfig.xml, DaemonProcessConfig.xml)
                string tDaemonChannelName = DaemonGlobal.s_DaemonConfig.DaemonChannelName + DaemonGlobal.s_DaemonConfig.DaemonPort;
                //DaemonGlobal.s_FileLogProcess.PrintLog(E_FileLogType.Infomation, string.Concat(DaemonGlobal.s_DaemonConfig.DaemonIP,":",DaemonGlobal.s_DaemonConfig.DaemonPort,DaemonConfig.s_DaemonRemoteChannelName + DaemonGlobal.s_DaemonConfig.DaemonPort, "  데몬->클라이언트 연결되는 채널을 생성 합니다."));
                //m_RemoteGateway = new MKRemote(E_RemoteType.TCPRemote, DaemonGlobal.s_DaemonConfig.DaemonIP, DaemonGlobal.s_DaemonConfig.DaemonPort, DaemonConfig.s_DaemonRemoteChannelName+DaemonGlobal.s_DaemonConfig.DaemonPort);
                DaemonGlobal.s_FileLogProcess.PrintLog(E_FileLogType.Infomation, string.Concat(DaemonGlobal.s_DaemonConfig.DaemonIP, ":", DaemonGlobal.s_DaemonConfig.DaemonPort, tDaemonChannelName, "  데몬->클라이언트 연결되는 채널을 생성 합니다."));
                m_RemoteGateway = new MKRemote(E_RemoteType.TCPRemote, DaemonGlobal.s_DaemonConfig.DaemonIP, DaemonGlobal.s_DaemonConfig.DaemonPort, tDaemonChannelName);

                while (tCount < 10)
                {
                    DaemonGlobal.s_FileLogProcess.PrintLog(E_FileLogType.Error, string.Format("데몬 IP: {0}  PortNo: {1}  ChannelName: {2}에 생성 합니다.", DaemonGlobal.s_DaemonConfig.DaemonIP, DaemonGlobal.s_DaemonConfig.DaemonPort, tDaemonChannelName));
                    if (m_RemoteGateway.StartRemoteServer(out tResult) != E_RemoteError.Success)
                    {
                        DaemonGlobal.s_FileLogProcess.PrintLog(E_FileLogType.Error, string.Concat("데몬 채널을 생성할 수 없습니다. : ", tResult));
                        Thread.Sleep(3000);
                        tCount++;
                    }
                    else
                        break;
                }

                if (m_RemoteGateway == null)
                {
                    DaemonGlobal.s_FileLogProcess.PrintLog(E_FileLogType.Error, string.Format("데몬 IP: {0}  PortNo: {1}  ChannelName: {2}에 생성 할 수 없습니다.", DaemonGlobal.s_DaemonConfig.DaemonIP, DaemonGlobal.s_DaemonConfig.DaemonPort, tDaemonChannelName));
                    return false;
                }

                //원격 메소드를 설정합니다.
                tRemoteMethod = new RemoteClientMethod();
                tRemoteMethod.SetUserConnectDaemonHandler(UserInfoReceiver);
                tRemoteMethod.SetRequestHandler(RequestReceiver);
                tRemoteMethod.SetResultHandler(ResultSender);
                tRemoteMethod.SetUserLogOutHandler(UserLogoutReceiver);
                tRemoteMethod.SetDDisconnectDaemonTelnetSessionRequestHandler(DisconnectTelnetSession);
                m_RemoteGateway.ServerObject = tRemoteMethod;

                return true;
            }
            catch (Exception ex)
            {
                DaemonGlobal.s_FileLogProcess.PrintLog(E_FileLogType.Error,ex.ToString());
                return false;
            }
        }
        /// <summary>
        /// 텔넷 세션 종료 처리 합니다.
        /// </summary>
        /// <param name="aClientID"></param>
        /// <param name="aSessionID"></param>
        private void DisconnectTelnetSession(int aClientID, int aSessionID)
        {
            DaemonGlobal.s_TelnetProcessor.DisconnectSession(aClientID, aSessionID);
            //if (DaemonGlobal.s_TelnetProcessor.CheckRemoveUserSession(aClientID))
            //{
            //    m_UserInfoList.Remove(aClientID);
            //}
        }
        /// <summary>
        /// 로그아웃 처리 합니다.
        /// </summary>
        /// <param name="aClientID"></param>
        private void UserLogoutReceiver(int aClientID)
        {
            lock (m_UserInfoList)
            {
                if (m_UserInfoList.Contains(aClientID))
                {
                    RemoveUserSession(m_UserInfoList[aClientID]);
                    //m_UserInfoList.Remove(aClientID);
                }
            }
        }
   
        /// <summary>
        /// 클라이언트의 결과 요청을 처리합니다.
        /// </summary>
        /// <param name="aClientID"></param>
        private byte[] ResultSender(int aClientID)
        {
            byte[] tResult = null;
            ArrayList tResults = null;
            int tResultCount = 0;
            lock (m_UserInfoList)
            {
                if (!m_UserInfoList.Contains(aClientID)) return null;

                UserInfo tUserInfo = (UserInfo)m_UserInfoList[aClientID];
                if (tUserInfo == null) return null;
                tUserInfo.LifeTime = DateTime.Now;
                lock (tUserInfo.DataQueue.SyncRoot)
                {
                    if (tUserInfo.DataQueue.Count > 0)
                    {
                        tResults = new ArrayList();
                        while (tUserInfo.DataQueue.Count > 0)
                        {
                            if (tResultCount >= 200) break;
                            tResults.Add(tUserInfo.DataQueue.Dequeue());
                            tResultCount++;
                        }
                        tResult = (byte[])ObjectConverter.GetBytes(tResults);
                    }
                }
            }
            return tResult;
        }

        /// <summary>
        /// 클라이언트 요청을 처리합니다.
        /// </summary>
        /// <param name="aData"></param>
        private void RequestReceiver(byte[] aData)
        {
            RequestCommunicationData tRequestData = (RequestCommunicationData)ObjectConverter.GetObject(aData);
            lock (m_RequestQueue)
            {
                m_RequestQueue.Enqueue(tRequestData);
                m_ReuqestMRE.Set();
            }
        }

        public void RequsetSSHTunnelReceiver(RequestCommunicationData aData)
        {
            lock (m_RequestQueue)
            {
                m_RequestQueue.Enqueue(aData);
                m_ReuqestMRE.Set();
            }
        }

        /// <summary>
        /// 클라이어트 요청을 처리 합니다.
        /// </summary>
        private void ProcessClientRequest()
        {
            RequestCommunicationData tClientRequest = null;
            while (DaemonGlobal.s_IsRun)
            {
                try
                {
                    if (m_RequestQueue.Count == 0)
                    {
                        m_ReuqestMRE.Reset();
                        m_ReuqestMRE.WaitOne();
                    }
                    lock (m_RequestQueue)
                    {
                        tClientRequest = m_RequestQueue.Dequeue();
                    }
                    if (tClientRequest != null)
                    {
                        switch (tClientRequest.CommType)
                        {
                            case E_CommunicationType.RequestCommandProcess://명령처리를 요청합니다.
                                DaemonGlobal.s_TelnetProcessor.ExecuteCommand(tClientRequest);
                                break;
                            case E_CommunicationType.RequestSSHTunnelOpen:
                                DaemonGlobal.s_KamServerCommunicationProcess.SendKamRequestData(tClientRequest);
                                break;
                            default:
                                break;
                        }
                        tClientRequest = null;
                    }
                }
                catch (Exception ex)
                {
                    DaemonGlobal.s_FileLogProcess.PrintLog(ex.ToString());
                }
            }
        }
        /// <summary>
        /// 로그 저장 요청을 처리 합니다.
        /// </summary>
        /// <param name="tClientRequest"></param>
        private void SaveExcuteCommand(RequestCommunicationData tClientRequest)
        {
           // GlobalClass.m_DBLogProcess.AddLog((DBExecuteCommandLogInfo)tClientRequest.RequestData);
        }

        /// <summary>
        ///// 로그 아웃 처리 합니다.
        ///// </summary>
        ///// <param name="tClientRequest"></param>
        //private void UserLogoutReceiver(int aClientID)
        //{
        //    lock (m_UserInfoList)
        //    {
        //        if (m_UserInfoList.Contains(aClientID))
        //        {
        //            UserInfo tUserInfo = m_UserInfoList[aClientID];
        //          //  GlobalClass.m_DBLogProcess.AddLog(new DBUserLogInfo(tUserInfo.UserID, E_UserLogType.LogOut, tUserInfo.Account + " 사용자가 로그아웃 했습니다."));
        //        }
        //    }
        //}

        /// <summary>
        /// 사용자 접속을 처리합니다.
        /// </summary>
        /// <param name="aUserAccount"></param>
        /// <param name="aUserPassword"></param>
        /// <param name="aClientIP"></param>
        /// <returns></returns>
        private byte[] UserInfoReceiver(byte[] aUserInfo)
        {
            LoginResultInfo tLoginResult;
            try
            {

                UserInfo tUserInfo = (UserInfo)ObjectConverter.GetObject(aUserInfo);

                bool tIsAlreadyLogin = false;
                tUserInfo.LifeTime = DateTime.Now;
                lock (m_UserInfoList)
                {
                    tIsAlreadyLogin = m_UserInfoList.Contains(tUserInfo.ClientID);
                }

                if (tIsAlreadyLogin)
                {
                    m_UserInfoList.Remove(tUserInfo.ClientID);
                }
	
               // GlobalClass.m_DBLogProcess.AddLog(new DBUserLogInfo(tUserInfo.UserID, E_UserLogType.Login, aUserAccount + " 사용자가 로그인 했습니다."));
                // 사용자 정보를 해시 테이블에 추가 합니다.
                lock (m_UserInfoList)
                {
                    m_UserInfoList.Add(tUserInfo);
                }
    
                tLoginResult = new LoginResultInfo();
                tLoginResult.UserID = tUserInfo.UserID;
                tLoginResult.ClientID = tUserInfo.ClientID;
                
                //tLoginResult.ServerID = DaemonGlobal.s_DaemonConfig.DaemonID;
                tLoginResult.LoginResult = E_LoginResult.Success;


                return ObjectConverter.GetBytes(tLoginResult);

            }
            catch (Exception ex)
            {
                tLoginResult = new LoginResultInfo(E_LoginResult.UnknownError, ex.ToString());
                return ObjectConverter.GetBytes(tLoginResult);
            }
        }

        /// <summary>
        /// 결과 데이터를 클라이언트의 전송 목록에 저장합니다.
        /// </summary>
        /// <param name="aResultData"></param>
        public void SendResultClient(ResultCommunicationData aResultData)
        {
            try
            {
                lock (m_UserInfoList)
                {
                    if (m_UserInfoList.Contains(aResultData.ClientID))
                    {
                        UserInfo tUserInfo = m_UserInfoList[aResultData.ClientID];
                        lock (tUserInfo.DataQueue.SyncRoot) tUserInfo.DataQueue.Enqueue(ObjectConverter.GetBytes(aResultData));
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 접속된 사용자 개수를 가져오기 합니다.
        /// </summary>
        public int GetConnectionUserCount
        {
            get { return m_UserInfoList.Count; }
        }

        /// <summary>
        /// 결과 데이터를 클라이언트의 전송 목록에 저장 합니다.
        /// </summary>
        /// <param name="aClientID"></param>
        /// <param name="aResults"></param>
        public void SendResultClient(int aClientID, ArrayList aResults)
        {
            try
            {
                lock (m_UserInfoList)
                {
                    if (m_UserInfoList.Contains(aClientID))
                    {
                        UserInfo tUserInfo = m_UserInfoList[aClientID];
                        lock (tUserInfo.DataQueue.SyncRoot)
                        {
                            foreach (byte[] tResult in aResults)
                            {
                                tUserInfo.DataQueue.Enqueue(tResult);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 사용자 정보를 가져오기 합니다.
        /// </summary>
        /// <param name="tClientID">Client ID 입니다.</param>
        /// <returns></returns>
        internal UserInfo GetUserInfo(int tClientID)
        {
            return m_UserInfoList[tClientID];
        }
    }
}
