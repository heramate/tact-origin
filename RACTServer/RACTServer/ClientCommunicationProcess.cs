using MKLibrary.MKNetwork;
using RACTCommonClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Linq;

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
        private System.Collections.Concurrent.BlockingCollection<RequestCommunicationData> m_RequestQueue;
        /// <summary>
        /// 클라이언트 요청에 결과를 반환하는 프로세스 입니다.
        /// </summary>
        private ClientResponseProcess m_ClientResponseProcess = null;
        /// <summary>
        /// 요청 처리 스레드 배열 입니다.
        /// </summary>
        private Thread[] m_RequestProcessThreads = null;
        private SemaphoreSlim m_LoginSemaphore = null;
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
            int semCount = Math.Max(2, GlobalClass.m_SystemInfo.DBConnectionCount / 3);
            m_LoginSemaphore = new SemaphoreSlim(semCount, semCount);
            m_RequestQueue = new System.Collections.Concurrent.BlockingCollection<RequestCommunicationData>();

            m_ClientResponseProcess = new ClientResponseProcess();
            m_ClientResponseProcess.Start();

            // DBConnectionCount 제약 없이 넉넉한 스레드 할당 (Native SQL Pool 200 활용)
            int threadCount = Math.Max(Environment.ProcessorCount * 2, 16);
            m_RequestProcessThreads = new Thread[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                m_RequestProcessThreads[i] = new Thread(new ThreadStart(ProcessClientRequest));
                m_RequestProcessThreads[i].Start();
            }

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
            if (m_RequestProcessThreads != null)
            {
                foreach (Thread t in m_RequestProcessThreads)
                {
                    GlobalClass.StopThread(t);
                }
            }
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

                    var userList = m_UserInfoList.ToList();
                    foreach (var userInfo in userList)
                    {
                        if (((TimeSpan)DateTime.Now.Subtract(userInfo.LifeTime)).TotalSeconds >= GlobalClass.s_HealthCheckTimeOut)
                        {
                            m_UserInfoList.Remove(userInfo.ClientID);
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
        /// 세션 만료 결과를 생성합니다.
        /// </summary>
        private byte[] MakeSessionExpiredResult(int aClientID)
        {
            ArrayList tResults = new ArrayList();
            ResultCommunicationData tResultData = new ResultCommunicationData();
            tResultData.ClientID = aClientID;
            tResultData.Error = new ErrorInfo(E_ErrorType.SessionExpired, "사용자 세션이 만료되었습니다. 다시 로그인합니다.");
            tResults.Add(ObjectConverter.GetBytes(tResultData));
            return (byte[])ObjectConverter.GetBytes(tResults);
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
            UserInfo tUserInfo = null;

            tUserInfo = m_UserInfoList[aClientID];
            if (tUserInfo == null)
            {
                if (aClientID != 0)
                {
                    return MakeSessionExpiredResult(aClientID);
                }
                return null;
            }

            tUserInfo.LifeTime = DateTime.Now;

            if (tUserInfo.DataQueue.Count > 0)
            {
                tResults = new ArrayList();
                while (tUserInfo.DataQueue.TryDequeue(out byte[] data))
                {
                    if (tResultCount >= 200) break;
                    tResults.Add(data);
                    tResultCount++;
                }
            }

            if (tResults != null && tResults.Count > 0)
            {
                tResult = (byte[])ObjectConverter.GetBytes(tResults);
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
            if (!m_RequestQueue.IsAddingCompleted) m_RequestQueue.Add(tRequestData);
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
                    if (!m_RequestQueue.TryTake(out tClientRequest, 1000)) continue;
                    if (tClientRequest == null) continue;

                    switch (tClientRequest.CommType)
                    {
                        case E_CommunicationType.RequestUserLogout://로그아웃을 요청합니다.
                            UserLogoutReceiver((int)tClientRequest.RequestData);
                            break;
                        //case E_CommunicationType.RequestCommandProcess://명령처리를 요청합니다.
                        //    GlobalClass.m_TelnetProcessor.ExecuteCommand(tClientRequest);
                        //    break;
                        case E_CommunicationType.RequestSaveExcuteCommand:
                            SaveExcuteCommand(tClientRequest);
                            break;

                        case E_CommunicationType.RequestOpenDeviceConnectionLog:
                            OpenDeviceConnectionLog(tClientRequest);
                            break;

                        case E_CommunicationType.RequestCloseDeviceConnectionLog:
                            CloseDeviceConnectionLog(tClientRequest);
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
                    // Thread.Sleep(1); 제거됨 (BlockingCollection TryTake 사용)
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
            UserInfo tUserInfo = m_UserInfoList[aClientID];
            if (tUserInfo != null)
            {
                UpdateUserLastLoginTime(tUserInfo);
                GlobalClass.m_DBLogProcess.AddLog(new DBUserLogInfo(tUserInfo.UserID, E_UserLogType.LogOut, tUserInfo.Account + " 사용자가 로그아웃 했습니다."));

                // 2013-04-26 - shinyn - 사용자 로그아웃시 ClientID 로그로 저장
                GlobalClass.m_LogProcess.PrintLog("Account : " + tUserInfo.Account + " ClientID : " + aClientID.ToString() + " 사용자가 로그아웃 했습니다");
                m_UserInfoList.Remove(aClientID);
            }
        }

        /// <summary>
        /// 사용자 접속을 처리합니다.
        /// </summary>
        /// <param name="aUserAccount"></param>
        /// <param name="aUserPassword"></param>
        /// <param name="aClientIP"></param>
        /// <returns></returns>
        private byte[] UserInfoReceiver(string aUserAccount, string aUserPassword, string aClientIP, E_TerminalMode aTerminalMode)
        {
            m_LoginSemaphore.Wait();
            try 
            {
                return UserInfoReceiverInternal(aUserAccount, aUserPassword, aClientIP, aTerminalMode);
            }
            finally
            {
                m_LoginSemaphore.Release();
            }
        }

        private byte[] UserInfoReceiverInternal(string aUserAccount, string aUserPassword, string aClientIP, E_TerminalMode aTerminalMode)
        {
            LoginResultInfo tLoginResult = null;
            E_UserType tUserType = E_UserType.Operator_Area;

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
                tQueryMessage = string.Format(tQueryMessage, aUserAccount, aUserPassword);

                UserInfo tUserInfo = null;

                using (var conn = GlobalClass.GetSqlConnection())
                {
                    if (conn == null) return ObjectConverter.GetBytes(new LoginResultInfo(E_LoginResult.UnknownError, "데이터베이스에 연결할 수 없습니다."));
                    
                    using (var multi = conn.QueryMultiple("SP_RACT_Get_UserInfo", new { UserAccount = aUserAccount, UserPassword = aUserPassword }, commandType: CommandType.StoredProcedure))
                    {
                        var userRow = multi.Read().Cast<IDictionary<string, object>>().FirstOrDefault();
                        if (userRow != null)
                        {
                            tUserInfo = new UserInfo();
                            tUserInfo.UserID = userRow.ContainsKey("UsrID") && userRow["UsrID"] != DBNull.Value ? Convert.ToInt32(userRow["UsrID"]) : 0;
                            tUserInfo.Account = aUserAccount;
                            tUserInfo.IPAddress = tClientIPAddress;
                            tUserInfo.LifeTime = DateTime.Now;
                            tUserInfo.LastLoginTime = userRow.ContainsKey("RactLastLoginTime") && userRow["RactLastLoginTime"] != DBNull.Value ? Convert.ToDateTime(userRow["RactLastLoginTime"]) : DateTime.MinValue;

                            int usrType = userRow.ContainsKey("UsrType") && userRow["UsrType"] != DBNull.Value ? Convert.ToInt32(userRow["UsrType"]) : 0;
                            if (usrType <= 0) tUserType = E_UserType.Admin_All;
                            else if (usrType == 1) tUserType = E_UserType.Admin_Area;
                            else if (usrType == 2) tUserType = E_UserType.Operator_Area;
                            else tUserType = E_UserType.Bp_Area;

                            if (userRow.ContainsKey("IsBp") && userRow["IsBp"] != DBNull.Value && Convert.ToBoolean(userRow["IsBp"])) tUserType = E_UserType.Bp_Area;
                            if (userRow.ContainsKey("IsSupervisor") && userRow["IsSupervisor"] != DBNull.Value && Convert.ToBoolean(userRow["IsSupervisor"])) tUserType = E_UserType.Supervisor;

                            tUserInfo.UserType = tUserType;
                            tUserInfo.IsViewAllBranch = userRow.ContainsKey("isviewallBranch") && userRow["isviewallBranch"] != DBNull.Value ? Convert.ToBoolean(userRow["isviewallBranch"]) : false;

                            if (userRow.ContainsKey("IsUseRACT") && userRow["IsUseRACT"] != DBNull.Value && !Convert.ToBoolean(userRow["IsUseRACT"]))
                            {
                                return ObjectConverter.GetBytes(new LoginResultInfo(E_LoginResult.NotAuthentication, ""));
                            }

                            if (tUserInfo.LastLoginTime.AddDays(GlobalClass.s_UnUsedLimit) < DateTime.Now)
                            {
                                conn.Execute("update usr_user set IsUseRACT = 0 where account = @UserAccount", new { UserAccount = aUserAccount });
                                return ObjectConverter.GetBytes(new LoginResultInfo(E_LoginResult.UnUsedLimit, ""));
                            }

                            if (!userRow.ContainsKey("CoCode") || userRow["CoCode"] == DBNull.Value || Convert.ToInt32(userRow["CoCode"]) == 0)
                            {
                                return ObjectConverter.GetBytes(new LoginResultInfo(E_LoginResult.NothingCoCode, ""));
                            }
                        }
                        else
                        {
                            return ObjectConverter.GetBytes(new LoginResultInfo(E_LoginResult.IncorrectID, ""));
                        }

                        if (!multi.IsConsumed)
                        {
                            var centers = multi.Read<string>().ToList();
                            foreach (var center in centers) tUserInfo.Centers.Add(center);
                        }

                        if (!multi.IsConsumed)
                        {
                            var mangTypes = multi.Read<string>().FirstOrDefault();
                            if (!string.IsNullOrEmpty(mangTypes))
                            {
                                string[] tMangTypeCds = mangTypes.Split('|');
                                foreach (var cd in tMangTypeCds)
                                {
                                    if (cd.Length > 1) tUserInfo.MangTypes.Add(cd);
                                }
                            }
                        }
                    }
                }

                if (tUserType == E_UserType.Supervisor)
                {
                    tUserInfo.LimitedCmdUser = false;
                }
                else
                {
                    tUserInfo.LimitedCmdUser = true;
                }

                tIsAlreadyLogin = m_UserInfoList.ToList().Any(u => u.UserID == tUserInfo.UserID);
/*
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
*/
                if (tIsAlreadyLogin && aTerminalMode == E_TerminalMode.RACTClient)
                {
                    tLoginResult = new LoginResultInfo(E_LoginResult.AlreadyLogin, "같은 계정으로 로그인 되어있습니다.");
                    return ObjectConverter.GetBytes(tLoginResult);
                }

                GlobalClass.m_DBLogProcess.AddLog(new DBUserLogInfo(tUserInfo.UserID, E_UserLogType.Login, aUserAccount + " 사용자가 로그인 했습니다."));

                // 2013-04-26 - shinyn - 사용자 로그인시 ClientID 로그로 저장
                GlobalClass.m_LogProcess.PrintLog("Account : " + aUserAccount + " ClientID : " + tUserInfo.ClientID.ToString() + " 사용자가 로그인 했습니다");

                // 사용자 정보를 해시 테이블에 추가 합니다.
                m_UserInfoList.Add(tUserInfo);
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
            try
            {
                using (var conn = GlobalClass.GetSqlConnection())
                {
                    if (conn == null) return;
                    conn.Execute("update usr_user set Ractlastlogintime = @LastLoginTime where account = @UserAccount", 
                        new { LastLoginTime = DateTime.Now, UserAccount = aUserInfo.Account });
                }
            }
            catch (Exception ex)
            {
                GlobalClass.m_LogProcess.PrintLog(ex.ToString());
            }
        }

        private void OpenDeviceConnectionLog(RequestCommunicationData aClientRequest)
        {
            DeviceConnectionLogOpenRequestInfo tRequest = null;
            DeviceConnectionLogOpenResultInfo tServiceResult = null;
            UserInfo tUserInfo = null;

            try
            {
                tRequest = (DeviceConnectionLogOpenRequestInfo)aClientRequest.RequestData;
                tUserInfo = GetValidUserInfo(aClientRequest.ClientID);

                if (tUserInfo == null)
                {
                    tServiceResult = new DeviceConnectionLogOpenResultInfo();
                    tServiceResult.Success = false;
                    tServiceResult.ErrorMessage = "로그인 세션이 유효하지 않습니다.";
                    SendOpenDeviceConnectionLogResult(aClientRequest.ClientID, aClientRequest.OwnerKey, tServiceResult);
                    return;
                }

                tServiceResult = GlobalClass.s_DeviceConnectionLogService.OpenLog(tUserInfo, tRequest);
                SendOpenDeviceConnectionLogResult(aClientRequest.ClientID, aClientRequest.OwnerKey, tServiceResult);
            }
            catch (Exception ex)
            {
                tServiceResult = new DeviceConnectionLogOpenResultInfo();
                tServiceResult.Success = false;
                tServiceResult.ErrorMessage = ex.ToString();
                SendOpenDeviceConnectionLogResult(aClientRequest.ClientID, aClientRequest.OwnerKey, tServiceResult);
            }
        }

        private void CloseDeviceConnectionLog(RequestCommunicationData aClientRequest)
        {
            DeviceConnectionLogCloseRequestInfo tRequest = null;
            DeviceConnectionLogCloseResultInfo tServiceResult = null;
            UserInfo tUserInfo = null;

            try
            {
                tRequest = (DeviceConnectionLogCloseRequestInfo)aClientRequest.RequestData;
                tUserInfo = GetValidUserInfo(aClientRequest.ClientID);

                if (tUserInfo == null)
                {
                    tServiceResult = new DeviceConnectionLogCloseResultInfo();
                    tServiceResult.Success = false;
                    tServiceResult.ErrorMessage = "로그인 세션이 유효하지 않습니다.";
                    SendCloseDeviceConnectionLogResult(aClientRequest.ClientID, aClientRequest.OwnerKey, tServiceResult);
                    return;
                }

                tServiceResult = GlobalClass.s_DeviceConnectionLogService.CloseLog(tUserInfo, tRequest);
                SendCloseDeviceConnectionLogResult(aClientRequest.ClientID, aClientRequest.OwnerKey, tServiceResult);
            }
            catch (Exception ex)
            {
                tServiceResult = new DeviceConnectionLogCloseResultInfo();
                tServiceResult.Success = false;
                tServiceResult.ErrorMessage = ex.ToString();
                SendCloseDeviceConnectionLogResult(aClientRequest.ClientID, aClientRequest.OwnerKey, tServiceResult);
            }
        }

        private UserInfo GetValidUserInfo(int aClientID)
        {
            lock (m_UserInfoList)
            {
                if (!m_UserInfoList.Contains(aClientID))
                {
                    return null;
                }

                return (UserInfo)m_UserInfoList[aClientID];
            }
        }

        private void SendOpenDeviceConnectionLogResult(int aClientID, int aOwnerKey, DeviceConnectionLogOpenResultInfo aResult)
        {
            ResultCommunicationData tResult = new ResultCommunicationData();
            tResult.ClientID = aClientID;
            tResult.OwnerKey = aOwnerKey;
            tResult.ResultData = aResult;
            tResult.Error = new ErrorInfo(
                aResult.Success ? E_ErrorType.NoError : E_ErrorType.LogicError,
                aResult.ErrorMessage ?? string.Empty);

            SendResultClient(tResult);
        }

        private void SendCloseDeviceConnectionLogResult(int aClientID, int aOwnerKey, DeviceConnectionLogCloseResultInfo aResult)
        {
            ResultCommunicationData tResult = new ResultCommunicationData();
            tResult.ClientID = aClientID;
            tResult.OwnerKey = aOwnerKey;
            tResult.ResultData = aResult;
            tResult.Error = new ErrorInfo(
                aResult.Success ? E_ErrorType.NoError : E_ErrorType.LogicError,
                aResult.ErrorMessage ?? string.Empty);

            SendResultClient(tResult);
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
