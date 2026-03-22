using RACTCommonClass;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Dapper;
using System.Collections;

namespace RACTServer
{
    /// <summary>
    /// 클라이언트 요청에 결과를 반환하는 프로세스 입니다.
    /// </summary>
    public class ClientResponseProcess : IDisposable
    {
        private System.Collections.Concurrent.BlockingCollection<RequestCommunicationData> m_RequestQueue = null;
        private Task[] m_RequestProcessTasks = null;
        private CancellationTokenSource _cts = new CancellationTokenSource();

        public ClientResponseProcess()
        {
            m_RequestQueue = new System.Collections.Concurrent.BlockingCollection<RequestCommunicationData>();
        }

        public void Dispose()
        {
            Stop();
        }

        public bool Start()
        {
            try
            {
                // DB 연결 수에 비례하여 워커 태스크 생성 (비동기이므로 스레드보다 적은 수로도 충분하지만 병렬성 유지)
                int taskCount = Math.Max(Environment.ProcessorCount, GlobalClass.m_SystemInfo.DBConnectionCount / 4);
                m_RequestProcessTasks = new Task[taskCount];
                
                for (int i = 0; i < taskCount; i++)
                {
                    m_RequestProcessTasks[i] = Task.Run(() => ProcessClientRequestAsync(_cts.Token));
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Stop()
        {
            _cts.Cancel();
            
            if (m_RequestQueue != null && !m_RequestQueue.IsAddingCompleted)
            {
                m_RequestQueue.CompleteAdding();
            }

            if (m_RequestProcessTasks != null)
            {
                try
                {
                    Task.WaitAll(m_RequestProcessTasks, 3000);
                }
                catch (AggregateException) { }
            }

            if (m_RequestQueue != null) { m_RequestQueue.Dispose(); m_RequestQueue = null; }
        }

        public void AddRequest(RequestCommunicationData aRequest)
        {
            if (m_RequestQueue != null && !m_RequestQueue.IsAddingCompleted) 
                m_RequestQueue.Add(aRequest);
        }

        private async Task ProcessClientRequestAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested && !m_RequestQueue.IsCompleted)
            {
                RequestCommunicationData tClientRequest = null;
                try
                {
                    if (!m_RequestQueue.TryTake(out tClientRequest, 1000, cancellationToken)) continue;
                    if (tClientRequest == null) continue;

                    UserInfo tUserInfo = GlobalClass.m_ClientProcess.GetUserInfo(tClientRequest.ClientID);
                    GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Info, string.Format("[클라이언트 요청 수신] ClientID={0}, UserID={1}, CommType={2} ",
                        tClientRequest.ClientID, tUserInfo != null ? tUserInfo.Account : "정보없음", tClientRequest.CommType.ToString()));

                    switch (tClientRequest.CommType)
                    {
                        case E_CommunicationType.RequestGroupInfo:
                            await GroupProcess.RequestProcessAsync(tClientRequest);
                            break;
                        case E_CommunicationType.RequestRactUserList:
                            await GroupProcess.RequestRactUserListProcessAsync(tClientRequest);
                            break;
                        case E_CommunicationType.RequestAddShareDevice:
                            await GroupProcess.RequestAddShareDeviceProcessAsync(tClientRequest);
                            break;
                        case E_CommunicationType.RequestDeviceInfo:
                            await DeviceProcess.RequestProcessAsync(tClientRequest);
                            break;
                        case E_CommunicationType.RequestModelInfo:
                            ModelInfoReceiver(tClientRequest);
                            break;
                        case E_CommunicationType.RequestOneTerminalModelInfo:
                            await OneTerminalModelInfoReceiverAsync(tClientRequest);
                            break;
                        case E_CommunicationType.RequestLimitCmdInfo:
                            await LimitCmdInfoReceiverAsync(tClientRequest);
                            break;
                        case E_CommunicationType.RequestDefaultCmdInfo:
                            await DefaultCmdInfoReceiverAsync(tClientRequest);
                            break;
                        case E_CommunicationType.RequestAutoCompleteCmd:
                            await RequestAutoCompleteCmdAsync(tClientRequest);
                            break;
                        case E_CommunicationType.RequestFactGroupInfo:
                            await FactGroupInfoReceiverAsync(tClientRequest);
                            break;
                        case E_CommunicationType.RequestShortenCommand:
                            await ShortenCommandProcess.RequestProcessAsync(tClientRequest);
                            break;
                        case E_CommunicationType.RequestShortenCommandGroup:
                            await ShortenCommandProcess.RequestGroupProcessAsync(tClientRequest);
                            break;
                        case E_CommunicationType.RequestScriptGroup:
                            await ScriptProcess.RequestProcessAsync(tClientRequest);
                            break;
                        case E_CommunicationType.RequestScriptInfo:
                            await ScriptProcess.RequestScriptProcessAsync(tClientRequest);
                            break;
                        case E_CommunicationType.RequestBatchRegisteration:
                            await DeviceProcess.RequestBatchRegisterationAsync(tClientRequest);
                            break;
                        case E_CommunicationType.RequestConnectionHistory:
                            await ConnectionHistoryRequestReceiverAsync(tClientRequest);
                            break;
                        case E_CommunicationType.RequestCommandHistory:
                            await CommandHistoryRequestReceiverAsync(tClientRequest);
                            break;
                        case E_CommunicationType.RequestFACTSearchDevice:
                            await DeviceProcess.RequestFactDeviceSearchProcessAsync(tClientRequest);
                            break;
                        case E_CommunicationType.RequestSearchDeviceForType:
                            await DeviceProcess.RequestSearchDeviceForTypeAsync(tClientRequest);
                            break;
                        case E_CommunicationType.RequestFACTIPSearchDevice:
                            await DeviceProcess.RequestFactIPDeviceSearchProcessAsync(tClientRequest);
                            break;
                        case E_CommunicationType.RequestDaemonInfo:
                            DaemonProcessInfoRequestReceiver(tClientRequest);
                            break;
                        case E_CommunicationType.RequestDaemonInfoList:
                            DaemonProcessInfoListRequestReceiver(tClientRequest);
                            break;
                        case E_CommunicationType.RequestSSHDaemonInfoList:
                            SSHTunnelDaemonProcessInfoListRequestReceiver(tClientRequest);
                            break;
                        case E_CommunicationType.RequestSSHDaemonInfo:
                            SSHTunnelDaemonProcessInfoRequestReceiver(tClientRequest);
                            break;
                        case E_CommunicationType.RequestDefaultConnectionCommand:
                            DefaultConnectionCommand(tClientRequest);
                            break;
                        case E_CommunicationType.RequestCfgRestoreCommand:
                            await ScriptProcess.RequestCfgRestoreCommandAsync(tClientRequest);
                            break;
                        case E_CommunicationType.RequestDevicesCfgRestoreCommand:
                            await ScriptProcess.RequestDevicesCfgRestoreCommandAsync(tClientRequest);
                            break;
                        case E_CommunicationType.RequestRMSCMTSSearchDevice:
                            await DeviceProcess.RequestSearchRMSCMTSDeviceAsync(tClientRequest);
                            break;
                        case E_CommunicationType.RequestSearchDeviceAuth:
                            await DeviceProcess.RequestFactDeviceSearchProcessAsync(tClientRequest, true);
                            break;
                        default:
                            GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Warning,
                                string.Format("[클라이언트 요청 수신] ClientID={0}, UserID={1}, CommType={2} : 처리되지 않은 CommunicationType 값입니다.",
                                tClientRequest.ClientID, tUserInfo != null ? tUserInfo.Account : "정보없음", tClientRequest.CommType.ToString()));
                            break;
                    }
                }
                catch (OperationCanceledException) { break; }
                catch (Exception ex)
                {
                    GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, "ProcessClientRequestAsync Error: " + ex.ToString());
                }
                finally
                {
                    tClientRequest = null;
                }
            }
        }

        private void DefaultConnectionCommand(RequestCommunicationData tClientRequest)
        {
            DefaultConnectionCommandProcess.GetCommand(tClientRequest);
        }

        private void DaemonProcessInfoRequestReceiver(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            lock (GlobalClass.s_DaemonProcessManager)
            {
                UseableDaemonRequestInfo tRequestInfo = (UseableDaemonRequestInfo)aClientRequest.RequestData;
                DaemonProcessInfo tProcessInfo;
                GlobalClass.m_LogProcess.PrintLog("DaemonProcessInfoRequestReceiver : 데몬요청 ClientID : " + tRequestInfo.ClientID.ToString());
                tProcessInfo = GlobalClass.s_DaemonProcessManager.GetDaemonProcess(tRequestInfo);
                if (tProcessInfo != null)
                {
                    tResultData.ResultData = new DaemonProcessInfo(tProcessInfo);
                }
            }
            GlobalClass.SendResultClient(tResultData);
        }

        private void DaemonProcessInfoListRequestReceiver(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            List<DaemonProcessInfo> tDaemonList = new List<DaemonProcessInfo>();
            lock (GlobalClass.s_DaemonProcessManager)
            {
                int tRequestCount = (int)aClientRequest.RequestData;
                GlobalClass.s_DaemonProcessManager.TempConnectionListClear();
                for (int i = 0; i < tRequestCount; i++)
                {
                    tDaemonList.Add(GlobalClass.s_DaemonProcessManager.GetDaemonProcess());
                }
            }
            tResultData.ResultData = tDaemonList;
            GlobalClass.SendResultClient(tResultData);
        }

        private void SSHTunnelDaemonProcessInfoListRequestReceiver(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            List<DaemonProcessInfo> tDaemonList = new List<DaemonProcessInfo>();
            lock (GlobalClass.s_DaemonProcessManager)
            {
                DeviceInfoCollection tDeviceList = (DeviceInfoCollection)aClientRequest.RequestData;
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, string.Format("[LTE연결을 위한 데몬요청 수신] 장비={0}건, ", tDeviceList.Count));

                foreach (DeviceInfo tDeviceInfo in tDeviceList)
                {
                    if (tDeviceInfo == null) continue;
                    var tProcessInfo = GlobalClass.s_DaemonProcessManager.GetSSHTunnelDaemonProcess();
                    tDaemonList.Add(tProcessInfo);
                }
            }
            tResultData.ResultData = tDaemonList;
            GlobalClass.SendResultClient(tResultData);
        }

        private void SSHTunnelDaemonProcessInfoRequestReceiver(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            DaemonProcessInfo tDaemonProcessInfo = null;
            lock (GlobalClass.s_DaemonProcessManager)
            {
                UseableDaemonRequestInfo tRequestInfo = (UseableDaemonRequestInfo)aClientRequest.RequestData;
                tDaemonProcessInfo = GlobalClass.s_DaemonProcessManager.GetSSHTunnelDaemonProcess();
            }
            tResultData.ResultData = new DaemonProcessInfo(tDaemonProcessInfo);
            GlobalClass.SendResultClient(tResultData);
        }

        private async Task CommandHistoryRequestReceiverAsync(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tCommandHistoryRequestInfo = (TelnetCommandHistoryRequestInfo)aClientRequest.RequestData;
                using (var conn = GlobalClass.GetSqlConnection())
                {
                    var results = (await conn.QueryAsync("select * from dbo.RACT_Log_ExcuteCommand where ConnectionLogID = @ID", 
                        new { ID = tCommandHistoryRequestInfo.ConnectionLogID })).ToList();
                    
                    var tHistoryList = new TelnetCommandHistoryInfoCollection();
                    foreach (var row in results)
                    {
                        var dict = (IDictionary<string, object>)row;
                        tHistoryList.Add(new TelnetCommandHistoryInfo
                        {
                            Time = dict["DateTime"] != DBNull.Value ? Convert.ToDateTime(dict["DateTime"]) : DateTime.MinValue,
                            Command = dict["Command"]?.ToString()
                        });
                    }
                    tResultData.ResultData = tHistoryList;
                }
                GlobalClass.SendResultClient(tResultData);
            }
            catch (Exception ex)
            {
                tResultData.Error.Error = E_ErrorType.UnKnownError;
                tResultData.Error.ErrorString = ex.Message;
                GlobalClass.SendResultClient(tResultData);
            }
        }

        private async Task ConnectionHistoryRequestReceiverAsync(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tConnectionHistoryRequestInfo = (ConnectionHistoryRequestInfo)aClientRequest.RequestData;
                string tQuery = @"select L.* ,N.NEName, N.MasterIP as IPAddress 
                                  from RACT_LOG_DeviceConnection L 
                                  inner Join Ne_Ne N On N.NEID = L.NEID 
                                  where L.userid = @UserID 
                                  and dateTime between @Start and @End 
                                  order by L.id desc";

                using (var conn = GlobalClass.GetSqlConnection())
                {
                    var results = (await conn.QueryAsync(tQuery, new { 
                        UserID = tConnectionHistoryRequestInfo.UserID, 
                        Start = tConnectionHistoryRequestInfo.StartTime.ToString("yyyy-MM-dd HH:mm:ss"), 
                        End = tConnectionHistoryRequestInfo.EndTime.ToString("yyyy-MM-dd HH:mm:ss") 
                    })).ToList();

                    var tHistoryList = new ConnectionHistoryInfoCollection();
                    foreach (var row in results)
                    {
                        var dict = (IDictionary<string, object>)row;
                        var tHistoryInfo = new DeviceConnectionHistoryInfo
                        {
                            ID = dict["ID"] != DBNull.Value ? Convert.ToInt32(dict["ID"]) : 0,
                            DeviceID = dict["NEID"] != DBNull.Value ? Convert.ToInt32(dict["NEID"]) : 0,
                            DeviceName = dict["NEName"]?.ToString(),
                            ConnectionTime = dict["DateTime"] != DBNull.Value ? Convert.ToDateTime(dict["DateTime"]) : DateTime.MinValue,
                            ConnectionType = dict["ConnectLogType"] != DBNull.Value ? (E_DeviceConnectType)Convert.ToInt32(dict["ConnectLogType"]) : E_DeviceConnectType.Connection,
                            Description = dict["Description"]?.ToString(),
                            IPAddress = dict["IPAddress"]?.ToString()
                        };
                        if (tHistoryInfo.ConnectionType == E_DeviceConnectType.DisConnection)
                        {
                            tHistoryInfo.EndTime = dict["DisconnectTime"] != DBNull.Value ? Convert.ToDateTime(dict["DisconnectTime"]) : DateTime.MinValue;
                        }
                        tHistoryList.Add(tHistoryInfo);
                    }
                    tResultData.ResultData = tHistoryList;
                    GlobalClass.SendResultClient(tResultData);
                }
            }
            catch (Exception ex)
            {
                tResultData.Error.Error = E_ErrorType.UnKnownError;
                tResultData.Error.ErrorString = ex.Message;
                GlobalClass.SendResultClient(tResultData);
            }
        }

        private async Task FactGroupInfoReceiverAsync(RequestCommunicationData aClientRequest)
        {
            UserInfo tUserInfo = GlobalClass.m_ClientProcess.GetUserInfo((int)aClientRequest.RequestData);
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            if (tUserInfo.IsViewAllBranch && tUserInfo.MangTypes.Count < 1)
            {
                tResultData.ResultData = GlobalClass.m_FACTGroupInfo.DeepClone();
            }
            else
            {
                tResultData.ResultData = await GroupProcess.GetFactGroupAsync(tUserInfo);
            }
            GlobalClass.SendResultClient(tResultData);
        }

        private void ModelInfoReceiver(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            tResultData.ResultData = GlobalClass.m_ModelInfoCollection.DeepClone();
            GlobalClass.SendResultClient(tResultData);
        }

        private async Task OneTerminalModelInfoReceiverAsync(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                string reqData = (string)aClientRequest.RequestData;
                using (var conn = GlobalClass.GetSqlConnection())
                {
                    var modelId = await conn.QueryFirstOrDefaultAsync<int>("SELECT ModelID FROM NE_NE WHERE MasterIP = @IP and Uses = 1", new { IP = reqData });
                    if (modelId != 0)
                        tResultData.ResultData = GlobalClass.m_ModelInfoCollection[modelId];
                }
                GlobalClass.SendResultClient(tResultData);
            }
            catch (Exception ex)
            {
                tResultData.Error.Error = E_ErrorType.UnKnownError;
                tResultData.Error.ErrorString = ex.Message;
                GlobalClass.SendResultClient(tResultData);
            }
        }

        private async Task LimitCmdInfoReceiverAsync(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            UserInfo tUserInfo = GlobalClass.m_ClientProcess.GetUserInfo(aClientRequest.ClientID);
            if (!await BaseDataLoadProcess.LoadLimitCmdInfoAsync(tUserInfo.UserType)) return;
            tResultData.ResultData = GlobalClass.m_LimitCmdInfoCollection.DeepClone();
            GlobalClass.SendResultClient(tResultData);
        }

        private async Task DefaultCmdInfoReceiverAsync(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            if (!await BaseDataLoadProcess.LoadDefaultCmdInfoAsync()) return;
            tResultData.ResultData = GlobalClass.m_DefaultCmdInfoCollection.DeepClone();
            GlobalClass.SendResultClient(tResultData);
        }

        private async Task RequestAutoCompleteCmdAsync(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            if (!await BaseDataLoadProcess.LoadAutoCompleteInfoAsync((int)aClientRequest.RequestData)) return;
            tResultData.ResultData = GlobalClass.m_AutoCompleteCmdInfoCollection.DeepClone();
            GlobalClass.SendResultClient(tResultData);
        }
    }
}
