using System;
using System.Collections.Generic;
using System.Text;
using RACTCommonClass;
using MKLibrary.MKNetwork;
using MKLibrary.MKData;
using System.Threading;
using System.Collections;
using RACTDaemonProcess;

namespace RACTServer
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
        /// 클라이언트 요청에 결과를 반환하는 프로세스 입니다.
        /// </summary>
        private ClientResponseProcess m_ClientResponseProcess = null;
        /// <summary>
        /// 요청 처리 스레드 입니다.
        /// </summary>
        private Thread m_RequestProcessThread = null;
        /// <summary>
        /// 접속된 사용자, 데몬의 연결 상태를 확인 합니다.
        /// </summary>
        private Thread m_HelathCheckThread = null;
        /// <summary>
        /// 접속된 사용자, 데몬의 연결 상태를 확인 합니다.
        /// </summary>
        //private Thread m_DaemonHelathCheckThread = null;

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public ClientCommunicationProcess()
        {
            m_RequestQueue = new Queue<RequestCommunicationData>();

            m_ClientResponseProcess = new ClientResponseProcess();
            m_ClientResponseProcess.Start();

            m_RequestProcessThread = new Thread(new ThreadStart(ProcessClientRequest));
            m_RequestProcessThread.Start();

            m_HelathCheckThread = new Thread(new ThreadStart(HealthCheckProcess));
            m_HelathCheckThread.Start();

     
        }
        /// <summary>
        /// 종료 처리 합니다.
        /// </summary>
        internal void Stop()
        {
            string tErrorString = "";

            if (m_RemoteGateway != null)
            {
                m_RemoteGateway.Dispose();
                m_RemoteGateway = null;
            }
           
            m_ClientResponseProcess.Stop();
            GlobalClass.StopThread(m_RequestProcessThread);
            GlobalClass.StopThread(m_HelathCheckThread);
        }

        /// <summary>
        /// 접속된 사용자, 데몬의 연결 상태를 확인 합니다.
        /// </summary>
        private void HealthCheckProcess()
        {
            UserInfo tUserInfo;
            while (GlobalClass.m_IsRun)
            {
                try
                {

                    for(int i = m_UserInfoList.Count-1; i >-1; i--)
                    {
                        tUserInfo =(UserInfo) m_UserInfoList.InnerList[i];
                        if (((TimeSpan)DateTime.Now.Subtract(tUserInfo.LifeTime)).TotalSeconds >= GlobalClass.s_HealthCheckTimeOut)
                        {
                            m_UserInfoList.RemoveAt(i);
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
        /// <summary>
        /// 접속된 사용자, 데몬의 연결 상태를 확인 합니다.
        /// </summary>
        private void DaemonHealthCheckProcess()
        {
            DaemonProcessInfo tProcessInfo;
            while (GlobalClass.m_IsRun)
            {
                try
                {
                    for (int i = GlobalClass.s_DaemonProcessManager.DaemonProcessList.Count - 1; i > -1; i--)
                    {
                        tProcessInfo = (DaemonProcessInfo)GlobalClass.s_DaemonProcessManager.DaemonProcessList.InnerList[i];
                        if (((TimeSpan)DateTime.Now.Subtract(tProcessInfo.LifeTime)).TotalSeconds >= 300)
                        {
                            GlobalClass.m_LogProcess.PrintLog(string.Concat("%%% 데몬 세션 삭제", tProcessInfo.IP, ":", tProcessInfo.Port));
                            GlobalClass.s_DaemonProcessManager.DaemonProcessList.RemoveAt(i);
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
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "클라이언트 채널을 생성 합니다.");
                m_RemoteGateway = new MKRemote(E_RemoteType.TCPRemote, GlobalClass.m_SystemInfo.ServerIP, GlobalClass.m_SystemInfo.ServerPort, GlobalClass.m_SystemInfo.ServerChannel);
                while (tCount < 10)
                {
                    if (m_RemoteGateway.StartRemoteServer(out tResult) != E_RemoteError.Success)
                    {
                        GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, string.Concat("클라이언트 채널을 생성할 수 없습니다. : ", tResult));
                        Thread.Sleep(3000);
                        tCount++;
                    }
                    else
                        break;
                }

                if (m_RemoteGateway == null)
                {
                    GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, string.Format("클라이언트 IP: {0}  PortNo: {1}  ChannelName: {2}에 연결 할 수 없습니다.", GlobalClass.m_SystemInfo.ServerIP, GlobalClass.m_SystemInfo.ServerPort, GlobalClass.m_SystemInfo.ServerChannel));
                    return false;
                }

                //client 원격 메소드를 설정합니다.
                tRemoteMethod = new RemoteClientMethod();
                tRemoteMethod.SetUserLoginHandler(UserInfoReceiver);
                tRemoteMethod.SetUserLogOutHandler(UserLogoutReceiver);
                tRemoteMethod.SetRequestHandler(RequestReceiver);
                tRemoteMethod.SetResultHandler(ResultSender);
               // tRemoteMethod.SetTelnetConnectionRequestHandler(TelnetConnectionRequestReceiver);

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
        /// 텔넷 접속 정보를 요청 합니다.
        /// </summary>
        /// <param name="aClientID"></param>
        /// <returns></returns>
        private byte[] TelnetConnectionRequestReceiver(byte[] aTelnetConnectionRequestHandler)
        {
            lock (this)
            {
                UseableDaemonRequestInfo tRequestInfo = (UseableDaemonRequestInfo)ObjectConverter.GetObject(aTelnetConnectionRequestHandler);
                DaemonProcessInfo tProcessInfo;
                tProcessInfo = GlobalClass.s_DaemonProcessManager.GetDaemonProcess(tRequestInfo);
                if (tProcessInfo != null)
                {
                    return ObjectConverter.GetBytes(new DaemonProcessInfo(tProcessInfo));
                }

                return null;
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
                tUserInfo.LifeTime = DateTime.Now;
                if (tUserInfo == null) return null;

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
            lock (m_RequestQueue) m_RequestQueue.Enqueue(tRequestData);
        }

        /// <summary>
        /// 클라이어트 요청을 처리 합니다.
        /// </summary>
        private void ProcessClientRequest()
        {
            RequestCommunicationData tClientRequest = null;
            while (GlobalClass.m_IsRun)
            {
                try
                {
                    lock (m_RequestQueue)
                    {
                        if (m_RequestQueue.Count < 1) continue;
                        tClientRequest =m_RequestQueue.Dequeue();
                    }
                    if (tClientRequest == null) continue;

                    switch (tClientRequest.CommType)
                    {
                        case E_CommunicationType.RequestUserLogout://로그아웃을 요청합니다.
                            UserLogoutReceiver((int)tClientRequest.RequestData);
                            break;
                        //case E_CommunicationType.RequestCommandProcess://명령처리를 요청합니다.
                        //    GlobalClass.m_TelnetProcessor.ExecuteCommand(tClientRequest);
                        //    break;
                        case E_CommunicationType.RequestSaveExcuteCommand :
                            SaveExcuteCommand(tClientRequest);
                            break;
                        default:
                            m_ClientResponseProcess.AddRequest(tClientRequest);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    
                }
                finally
                {
                    Thread.Sleep(1);
                }
            }
        }
        /// <summary>
        /// 로그 저장 요청을 처리 합니다.
        /// </summary>
        /// <param name="tClientRequest"></param>
        private void SaveExcuteCommand(RequestCommunicationData tClientRequest)
        {
            GlobalClass.m_DBLogProcess.AddLog((DBExecuteCommandLogInfo)tClientRequest.RequestData);
        }

        /// <summary>
        /// 로그 아웃 처리 합니다.
        /// </summary>
        /// <param name="tClientRequest"></param>
        private void UserLogoutReceiver(int aClientID)
        {
            lock (m_UserInfoList)
            {
                if (m_UserInfoList.Contains(aClientID))
                {
                    UserInfo tUserInfo = m_UserInfoList[aClientID];
                    UpdateUserLastLoginTime(tUserInfo);
                    GlobalClass.m_DBLogProcess.AddLog(new DBUserLogInfo(tUserInfo.UserID, E_UserLogType.LogOut, tUserInfo.Account + " 사용자가 로그아웃 했습니다."));

                    // 2013-04-26 - shinyn - 사용자 로그아웃시 ClientID 로그로 저장
                    GlobalClass.m_LogProcess.PrintLog("Account : " + tUserInfo.Account + " ClientID : " + aClientID.ToString() + " 사용자가 로그아웃 했습니다");
                    m_UserInfoList.Remove(aClientID);
                }
            }
        }

        /// <summary>
        /// 사용자 접속을 처리합니다.
        /// </summary>
        /// <param name="aUserAccount"></param>
        /// <param name="aUserPassword"></param>
        /// <param name="aClientIP"></param>
        /// <returns></returns>
        private byte[] UserInfoReceiver(string aUserAccount, string aUserPassword, string aClientIP,E_TerminalMode aTerminalMode )
        {
            LoginResultInfo tLoginResult = null;
            E_UserType tUserType = E_UserType.Operator_Area;

            MKDBWorkItem tDBWI = GlobalClass.m_DBPool.GetDBWorkItem();
            MKDataSet tDataSet = null;

            string[] tClientIPAddressList = aClientIP.Split('/');
            string tClientIPAddress = tClientIPAddressList[0];

            try
            {
                string tQueryMessage = "";

                if (aUserPassword.Equals("DASAN_HRKIM"))
                {
                    aUserPassword = "";
                }

                tQueryMessage = "EXEC SP_RACT_Get_UserInfo '{0}', '{1}'";
                tQueryMessage = string.Format(tQueryMessage, aUserAccount,aUserPassword);

                tDBWI.ExecuteQuery(tQueryMessage, out tDataSet);

               

                if (tDataSet == null)
                {
                    return ObjectConverter.GetBytes(new LoginResultInfo(E_LoginResult.UnknownError, "데이터베이스에 연결할 수 없습니다."));
                }

                UserInfo tUserInfo = null;

                if (tDataSet.RecordCount > 0)
                {
                    tUserInfo = new UserInfo();
                    tUserInfo.UserID = tDataSet.GetInt32("UsrID");
                    tUserInfo.Account = aUserAccount;
                    tUserInfo.IPAddress = tClientIPAddress;
                    tUserInfo.LifeTime = DateTime.Now;
                    tUserInfo.LastLoginTime = tDataSet.GetDateTime("RactLastLoginTime");
                    
                    //tUserType = (E_UserType)tDataSet.GetInt32("UsrType");
                    if (tDataSet.GetInt32("UsrType") <= 0)
                    {
                        tUserType = E_UserType.Admin_All;
                    }
                    else if (tDataSet.GetInt32("UsrType") == 1)
                    {
                        tUserType = E_UserType.Admin_Area;
                    }
                    else if (tDataSet.GetInt32("UsrType") == 2)
                    {
                        tUserType = E_UserType.Operator_Area;
                    }
                    else
                    {
                        tUserType = E_UserType.Bp_Area;
                    }

                    if (tDataSet.GetBool("IsBp") == true)
                    {
                        tUserType = E_UserType.Bp_Area;
                    }

                    if (tDataSet.GetBool("IsSupervisor") == true)
                    {
                        tUserType = E_UserType.Supervisor;
                    }

                    tUserInfo.UserType = tUserType;

                    tUserInfo.IsViewAllBranch = tDataSet.GetBool("isviewallBranch");

                    if (!tDataSet.GetBool("IsUseRACT"))
                    {
                        return ObjectConverter.GetBytes(new LoginResultInfo(E_LoginResult.NotAuthentication, ""));
                    }

                    if (tUserInfo.LastLoginTime.AddDays(GlobalClass.s_UnUsedLimit) < DateTime.Now)
                    {
                        tDBWI.ExecuteNoneQuery(string.Format("update usr_user set IsUseRACT = 0 where account ='{0}'", aUserAccount));
                        return ObjectConverter.GetBytes(new LoginResultInfo(E_LoginResult.UnUsedLimit, ""));
                    }
                }
                else
                {
                   // m_FileLog.PrintLogEnter("ID가 존재하지 않습니다.");
                    return ObjectConverter.GetBytes(new LoginResultInfo(E_LoginResult.IncorrectID, ""));
                }

                tDataSet.CurrentTableIndex++;

                if (tDataSet.RecordCount > 0)
                {
                    for (int i = 0; i < tDataSet.RecordCount; i++)
                    {
                        tUserInfo.Centers.Add(tDataSet.GetString("centerCode"));
                        tDataSet.MoveNext();
                    }
                }
                //2019-03-25 KangBongHan 제한명령어 명령어별 권한 변경건 수정
                //2015-10-30 제한명령어 권한 적용.
                //LimitedCmdUser의 기본값은 false 임. (총괄사용자 등은 제한 없음.)
                //Supervisor는 제한 없이 사용 가능 그외 
                if (tUserType == E_UserType.Supervisor)
                {
                    tUserInfo.LimitedCmdUser = false;
                }
                else
                {
                    tUserInfo.LimitedCmdUser = true;
                }


                //웹에서 설정하는 3개의 권한에 대해서만 체크함.
                /*
                tDataSet = null;
                //tQueryMessage = "SELECT * FROM dbo.RACT_USR_AUTH_DEF WHERE MenuTypeID=1";
                tQueryMessage = "SELECT MAX(CAST(GlobalAdmin AS INT)) AS GlobalAdmin, "+
                                "MAX(CAST(LocalAdmin AS INT)) AS LocalAdmin, " +
                                "MAX(CAST(CenterAdmin AS INT)) AS CenterAdmin, " +
                                "MAX(CAST(BpAdmin AS INT)) AS BpAdmin " +
                                "FROM dbo.RACT_NE_EMBAGO_CMD ";
                if (tDBWI.ExecuteQuery(tQueryMessage, out tDataSet) == E_DBProcessError.Success)
                {
                    if (tUserType <= E_UserType.Admin_All)
                    {
                        if (tDataSet.GetBool("GlobalAdmin") == true)
                        {
                            tUserInfo.LimitedCmdUser = true;
                        }
                    }
                    else if (tUserType == E_UserType.Admin_Area)
                    {
                        if (tDataSet.GetBool("LocalAdmin") == true)
                        {
                            tUserInfo.LimitedCmdUser = true;
                        }
                    }
                    else if (tUserType == E_UserType.Operator_Area)
                    {
                        if (tDataSet.GetBool("CenterAdmin") == true)
                        {
                            tUserInfo.LimitedCmdUser = true;
                        }
                    }
                      else if (tUserType == E_UserType.Bp_Area)
                    {
                        if (tDataSet.GetBool("BpAdmin") == true)
                        {
                            tUserInfo.LimitedCmdUser = true;
                        }
                    }
                }
                */
                MKOleDBClass.CloseDataSet(tDataSet);

                bool tIsAlreadyLogin = false;
                lock (m_UserInfoList)
                {
                    foreach (UserInfo tmpUserInfo in m_UserInfoList)
                    {
                        if (tmpUserInfo.UserID == tUserInfo.UserID)
                        {
                            tIsAlreadyLogin = true;
                            break;
                        }
                    }
                }

                
                if (tIsAlreadyLogin && aTerminalMode == E_TerminalMode.RACTClient)
                {
                    tLoginResult = new LoginResultInfo(E_LoginResult.AlreadyLogin, "같은 계정으로 로그인 되어있습니다.");
                    return ObjectConverter.GetBytes(tLoginResult);
                }

                GlobalClass.m_DBLogProcess.AddLog(new DBUserLogInfo(tUserInfo.UserID,E_UserLogType.Login,aUserAccount +" 사용자가 로그인 했습니다."));

                // 2013-04-26 - shinyn - 사용자 로그인시 ClientID 로그로 저장
                GlobalClass.m_LogProcess.PrintLog("Account : " + aUserAccount + " ClientID : " + tUserInfo.ClientID.ToString() + " 사용자가 로그인 했습니다");

                // 사용자 정보를 해시 테이블에 추가 합니다.
                lock (m_UserInfoList)
                {
                    m_UserInfoList.Add(tUserInfo);
                }
                tLoginResult = new LoginResultInfo();
                tLoginResult.UserID = tUserInfo.UserID;
                tLoginResult.ClientID = tUserInfo.ClientID;
                tLoginResult.ServerID = GlobalClass.m_SystemInfo.ServerID;
                tLoginResult.UserType = tUserType;
                tLoginResult.UserInfo = tUserInfo;
                tLoginResult.LoginResult = E_LoginResult.Success;

                //접속 제한 날짜를 업데이트 합니다.
                tUserInfo.LastLoginTime = DateTime.Now;

                UpdateUserLastLoginTime(tUserInfo);
               
                return ObjectConverter.GetBytes(tLoginResult);

            }
            catch (Exception ex)
            {
                MKOleDBClass.CloseDataSet(tDataSet);
                tLoginResult = new LoginResultInfo(E_LoginResult.UnknownError, ex.ToString());
                return ObjectConverter.GetBytes(tLoginResult);
            }
        }
        /// <summary>
        /// 마지막 접속날짜를 변경 합니다.
        /// </summary>
        /// <param name="aUserInfo"></param>
        private void UpdateUserLastLoginTime(UserInfo aUserInfo)
        {
            MKDBWorkItem tDBWI = GlobalClass.m_DBPool.GetDBWorkItem();
            MKDataSet tDataSet = null;

            try
            {
                tDBWI.ExecuteNoneQuery(string.Format("update usr_user set Ractlastlogintime = '{0}' where account ='{1}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), aUserInfo.Account));
            }
            catch (Exception ex)
            {
                GlobalClass.m_LogProcess.PrintLog(ex.ToString());
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
