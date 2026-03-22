using MKLibrary.MKNetwork;
using RACTCommonClass;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Linq;

namespace RACTServer
{
    public class ClientCommunicationProcess
    {
        private MKRemote m_RemoteGateway;
        private Data.Pipeline.PipelineListener _pipelineListener;
        private UserInfoCollection m_UserInfoList = new UserInfoCollection();
        private BlockingCollection<RequestCommunicationData> m_RequestQueue;
        private ClientResponseProcess m_ClientResponseProcess = null;
        private Task[] m_RequestProcessTasks = null;
        private SemaphoreSlim m_LoginSemaphore = null;
        private Thread m_HelathCheckThread = null;
        private CancellationTokenSource _cts = new CancellationTokenSource();

        public ClientCommunicationProcess()
        {
            int semCount = Math.Max(2, GlobalClass.m_SystemInfo.DBConnectionCount / 3);
            m_LoginSemaphore = new SemaphoreSlim(semCount, semCount);
            m_RequestQueue = new BlockingCollection<RequestCommunicationData>();

            m_ClientResponseProcess = new ClientResponseProcess();
            m_ClientResponseProcess.Start();

            int taskCount = Math.Max(Environment.ProcessorCount, 8);
            m_RequestProcessTasks = new Task[taskCount];
            for (int i = 0; i < taskCount; i++)
            {
                m_RequestProcessTasks[i] = Task.Run(() => ProcessClientRequestAsync(_cts.Token));
            }

            m_HelathCheckThread = new Thread(new ThreadStart(HealthCheckProcess));
            m_HelathCheckThread.Start();
        }

        internal void Stop()
        {
            _cts.Cancel();
            _pipelineListener?.Stop();

            if (m_RemoteGateway != null) { m_RemoteGateway.Dispose(); m_RemoteGateway = null; }
            m_ClientResponseProcess.Stop();
            if (m_RequestQueue != null) m_RequestQueue.CompleteAdding();
            if (m_RequestProcessTasks != null) { try { Task.WaitAll(m_RequestProcessTasks, 3000); } catch { } }
            GlobalClass.StopThread(m_HelathCheckThread);
        }

        private void HealthCheckProcess()
        {
            /*
            while (GlobalClass.m_IsRun)
            {
                try
                {
                    for (int i = GlobalClass.s_DaemonProcessManager.DaemonProcessList.Count - 1; i > -1; i--)
                    {
                        var tProcessInfo = (DaemonProcessInfo)GlobalClass.s_DaemonProcessManager.DaemonProcessList.InnerList[i];
                        if (((TimeSpan)DateTime.Now.Subtract(tProcessInfo.LifeTime)).TotalSeconds >= 300)
                        {
                            GlobalClass.m_LogProcess.PrintLog(string.Concat("%%% 데몬 세션 삭제", tProcessInfo.IP, ":", tProcessInfo.Port));
                            GlobalClass.s_DaemonProcessManager.DaemonProcessList.RemoveAt(i);
                        }
                    }
                }
                catch (Exception ex) { GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, ex.ToString()); }
                Thread.Sleep(1000);
            }
            */
        }

        public bool Start()
        {
            int tCount = 0;
            string tResult = string.Empty;
            try
            {
                m_RemoteGateway = new MKRemote(E_RemoteType.TCPRemote, GlobalClass.m_SystemInfo.ServerIP, GlobalClass.m_SystemInfo.ServerPort, GlobalClass.m_SystemInfo.ServerChannel);
                while (tCount < 10)
                {
                    if (m_RemoteGateway.StartRemoteServer(out tResult) == E_RemoteError.Success) break;
                    GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, "클라이언트 채널 생성 실패: " + tResult);
                    Thread.Sleep(3000);
                    tCount++;
                }

                if (m_RemoteGateway == null) return false;

                RemoteClientMethod tRemoteMethod = new RemoteClientMethod();
                tRemoteMethod.SetUserLoginHandler(UserInfoReceiver);
                tRemoteMethod.SetUserLogOutHandler(UserLogoutReceiver);
                tRemoteMethod.SetRequestHandler(RequestReceiver);
                tRemoteMethod.SetResultHandler(ResultSender);
                m_RemoteGateway.ServerObject = tRemoteMethod;

                // 고성능 Pipeline 채널 병행 가동 (기존 포트 + 1)
                _pipelineListener = new Data.Pipeline.PipelineListener(GlobalClass.m_SystemInfo.ServerIP, GlobalClass.m_SystemInfo.ServerPort + 1, RequestReceiver);
                _pipelineListener.Start();

                return true;
                }
                catch (Exception ex) { GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, ex.ToString()); return false; }
                }

                private void RequestReceiver(byte[] aRequestData)
                {
                if (aRequestData == null) return;
                var tRequest = (RequestCommunicationData)ObjectConverter.GetObject(aRequestData);
                if (tRequest != null && !m_RequestQueue.IsAddingCompleted) m_RequestQueue.Add(tRequest);
                }

        private async Task ProcessClientRequestAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested && !m_RequestQueue.IsCompleted)
            {
                try
                {
                    if (m_RequestQueue.TryTake(out RequestCommunicationData tClientRequest, 1000, cancellationToken))
                    {
                        if (tClientRequest == null) continue;
                        switch (tClientRequest.CommType)
                        {
                            case E_CommunicationType.RequestOpenDeviceConnectionLog:
                                await OpenDeviceConnectionLogAsync(tClientRequest);
                                break;
                            case E_CommunicationType.RequestCloseDeviceConnectionLog:
                                await CloseDeviceConnectionLogAsync(tClientRequest);
                                break;
                            default:
                                m_ClientResponseProcess.AddRequest(tClientRequest);
                                break;
                        }
                    }
                }
                catch (OperationCanceledException) { break; }
                catch (Exception ex) { GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, "ProcessClientRequestAsync Error: " + ex.ToString()); }
            }
        }

        private void UserLogoutReceiver(int aClientID)
        {
            try
            {
                lock (m_UserInfoList)
                {
                    if (m_UserInfoList.Contains(aClientID))
                    {
                        UserInfo tUserInfo = m_UserInfoList[aClientID];
                        m_UserInfoList.Remove(aClientID);
                        GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, string.Format("%%% 클라이언트 로그아웃 계정: {0} IP: {1}", tUserInfo.Account, tUserInfo.IPAddress));
                    }
                }
            }
            catch (Exception ex) { GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, ex.ToString()); }
        }

        private byte[] UserInfoReceiver(string aUserAccount, string aUserPassword, string aClientIP, E_TerminalMode aTerminalMode)
        {
            string tClientIP = aClientIP.Split('/')[0];
            try
            {
                m_LoginSemaphore.Wait();
                using (var conn = GlobalClass.GetSqlConnection())
                {
                    var user = conn.QueryFirstOrDefault("select * from usr_user where account = @Account", new { Account = aUserAccount });
                    if (user == null) return ObjectConverter.GetBytes(new LoginResultInfo { LoginResult = E_LoginResult.IncorrectID });
                    if (user.password?.ToString() != aUserPassword) return ObjectConverter.GetBytes(new LoginResultInfo { LoginResult = E_LoginResult.IncorrectPassword });

                    UserInfo tUserInfo = new UserInfo
                    {
                        Account = aUserAccount,
                        IPAddress = tClientIP,
                        UserType = (E_UserType)Convert.ToInt32(user.UserTypeCode),
                        UserID = Convert.ToInt32(user.usr_id),
                        BranchCode = user.org2_id?.ToString(),
                        CenterCode = user.CenterCode?.ToString(),
                        LimitedCmdUser = Convert.ToBoolean(user.LimitedCmdUser)
                    };

                    lock (m_UserInfoList) { m_UserInfoList.Add(tUserInfo); }
                    
                    var tLoginResult = new LoginResultInfo { ServerID = GlobalClass.m_SystemInfo.ServerID, UserType = tUserInfo.UserType, UserInfo = tUserInfo, LoginResult = E_LoginResult.Success };
                    _ = UpdateLastLoginTimeAsync(tUserInfo);
                    GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, string.Format("%%% 클라이언트 로그인 성공 계정: {0} IP: {1}", aUserAccount, tClientIP));
                    return ObjectConverter.GetBytes(tLoginResult);
                }
            }
            catch (Exception ex) { GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, ex.ToString()); return ObjectConverter.GetBytes(new LoginResultInfo { LoginResult = E_LoginResult.UnknownError }); }
            finally { m_LoginSemaphore.Release(); }
        }

        private async Task UpdateLastLoginTimeAsync(UserInfo aUserInfo)
        {
            try
            {
                using (var conn = GlobalClass.GetSqlConnection())
                {
                    if (conn == null) return;
                    await conn.ExecuteAsync("update usr_user set Ractlastlogintime = @LastLoginTime where account = @UserAccount", new { LastLoginTime = DateTime.Now, UserAccount = aUserInfo.Account });
                }
            }
            catch (Exception ex) { GlobalClass.m_LogProcess.PrintLog(ex.ToString()); }
        }

        private async Task OpenDeviceConnectionLogAsync(RequestCommunicationData aClientRequest)
        {
            try
            {
                var tRequest = (DeviceConnectionLogOpenRequestInfo)aClientRequest.RequestData;
                var tUserInfo = GetValidUserInfo(aClientRequest.ClientID);
                if (tUserInfo == null) { SendOpenDeviceConnectionLogResult(aClientRequest.ClientID, aClientRequest.OwnerKey, new DeviceConnectionLogOpenResultInfo { Success = false, ErrorMessage = "로그인 세션 만료" }); return; }
                var tServiceResult = await GlobalClass.s_DeviceConnectionLogService.OpenLogAsync(tUserInfo, tRequest);
                SendOpenDeviceConnectionLogResult(aClientRequest.ClientID, aClientRequest.OwnerKey, tServiceResult);
            }
            catch (Exception ex) { SendOpenDeviceConnectionLogResult(aClientRequest.ClientID, aClientRequest.OwnerKey, new DeviceConnectionLogOpenResultInfo { Success = false, ErrorMessage = ex.Message }); }
        }

        private async Task CloseDeviceConnectionLogAsync(RequestCommunicationData aClientRequest)
        {
            try
            {
                var tRequest = (DeviceConnectionLogCloseRequestInfo)aClientRequest.RequestData;
                var tUserInfo = GetValidUserInfo(aClientRequest.ClientID);
                if (tUserInfo == null) { SendCloseDeviceConnectionLogResult(aClientRequest.ClientID, aClientRequest.OwnerKey, new DeviceConnectionLogCloseResultInfo { Success = false, ErrorMessage = "로그인 세션 만료" }); return; }
                var tServiceResult = await GlobalClass.s_DeviceConnectionLogService.CloseLogAsync(tUserInfo, tRequest);
                SendCloseDeviceConnectionLogResult(aClientRequest.ClientID, aClientRequest.OwnerKey, tServiceResult);
            }
            catch (Exception ex) { SendCloseDeviceConnectionLogResult(aClientRequest.ClientID, aClientRequest.OwnerKey, new DeviceConnectionLogCloseResultInfo { Success = false, ErrorMessage = ex.Message }); }
        }

        private UserInfo GetValidUserInfo(int aClientID) { lock (m_UserInfoList) { return m_UserInfoList.Contains(aClientID) ? (UserInfo)m_UserInfoList[aClientID] : null; } }

        private void SendOpenDeviceConnectionLogResult(int aClientID, int aOwnerKey, DeviceConnectionLogOpenResultInfo aResult)
        {
            SendResultClient(new ResultCommunicationData { ClientID = aClientID, OwnerKey = aOwnerKey, ResultData = aResult, Error = new ErrorInfo(aResult.Success ? E_ErrorType.NoError : E_ErrorType.LogicError, aResult.ErrorMessage ?? string.Empty) });
        }

        private void SendCloseDeviceConnectionLogResult(int aClientID, int aOwnerKey, DeviceConnectionLogCloseResultInfo aResult)
        {
            SendResultClient(new ResultCommunicationData { ClientID = aClientID, OwnerKey = aOwnerKey, ResultData = aResult, Error = new ErrorInfo(aResult.Success ? E_ErrorType.NoError : E_ErrorType.LogicError, aResult.ErrorMessage ?? string.Empty) });
        }

        private byte[] ResultSender(int aClientID)
        {
            UserInfo tUserInfo = null;
            lock (m_UserInfoList) { if (m_UserInfoList.Contains(aClientID)) tUserInfo = m_UserInfoList[aClientID]; }
            if (tUserInfo == null) return aClientID != 0 ? MakeSessionExpiredResult(aClientID) : null;
            tUserInfo.LifeTime = DateTime.Now;
            if (tUserInfo.DataQueue.Count > 0)
            {
                ArrayList tResults = new ArrayList();
                while (tUserInfo.DataQueue.TryDequeue(out byte[] data)) { tResults.Add(data); if (tResults.Count >= 200) break; }
                if (tResults.Count > 0) return (byte[])ObjectConverter.GetBytes(tResults);
            }
            return null;
        }

        private byte[] MakeSessionExpiredResult(int aClientID)
        {
            var tResult = new ResultCommunicationData { ClientID = aClientID, Error = new ErrorInfo(E_ErrorType.SessionExpired, "Session Expired") };
            return (byte[])ObjectConverter.GetBytes(new ArrayList { ObjectConverter.GetBytes(tResult) });
        }

        public void SendResultClient(ResultCommunicationData aResultData)
        {
            try { lock (m_UserInfoList) { if (m_UserInfoList.Contains(aResultData.ClientID)) m_UserInfoList[aResultData.ClientID].DataQueue.Enqueue(ObjectConverter.GetBytes(aResultData)); } } catch { }
        }

        public void SendResultClient(int aClientID, ArrayList aResults)
        {
            try { lock (m_UserInfoList) { if (m_UserInfoList.Contains(aClientID)) { var user = m_UserInfoList[aClientID]; foreach (byte[] res in aResults) user.DataQueue.Enqueue(res); } } } catch { }
        }

        internal UserInfo GetUserInfo(int tClientID) { return m_UserInfoList[tClientID]; }
    }
}
