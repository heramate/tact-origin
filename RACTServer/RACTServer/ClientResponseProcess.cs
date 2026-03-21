using RACTCommonClass;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Dapper;

namespace RACTServer
{
    /// <summary>
    /// 클라이언트 요청에 결과를 반환하는 프로세스 입니다.
    /// </summary>
    public class ClientResponseProcess : IDisposable
    {
        private System.Collections.Concurrent.BlockingCollection<RequestCommunicationData> m_RequestQueue = null;
        private Thread[] m_RequestProcessThreads = null;

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
                int threadCount = Math.Max(2, GlobalClass.m_SystemInfo.DBConnectionCount / 3);
                m_RequestProcessThreads = new Thread[threadCount];
                for (int i = 0; i < threadCount; i++)
                {
                    m_RequestProcessThreads[i] = new Thread(new ThreadStart(ProcessClientRequest));
                    m_RequestProcessThreads[i].Start();
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
            if (m_RequestProcessThreads != null)
            {
                foreach (Thread t in m_RequestProcessThreads)
                {
                    GlobalClass.StopThread(t);
                }
            }
            if (m_RequestQueue != null) { m_RequestQueue.CompleteAdding(); m_RequestQueue.Dispose(); }
        }

        public void AddRequest(RequestCommunicationData aRequest)
        {
            if (!m_RequestQueue.IsAddingCompleted) m_RequestQueue.Add(aRequest);
        }

        private void ProcessClientRequest()
        {
            RequestCommunicationData tClientRequest = null;
            while (GlobalClass.m_IsRun)
            {
                try
                {
                    if (!m_RequestQueue.TryTake(out tClientRequest, 1000)) continue;
                    if (tClientRequest == null) continue;

                    UserInfo tUserInfo = GlobalClass.m_ClientProcess.GetUserInfo(tClientRequest.ClientID);
                    GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Info, string.Format("[클라이언트 요청 수신] ClientID={0}, UserID={1}, CommType={2} ",
                        tClientRequest.ClientID, tUserInfo != null ? tUserInfo.Account : "정보없음", tClientRequest.CommType.ToString()));

                    switch (tClientRequest.CommType)
                    {
                        case E_CommunicationType.RequestGroupInfo:
                            GroupProcess.RequestProcess(tClientRequest);
                            break;
                        case E_CommunicationType.RequestRactUserList:
                            GroupProcess.RequestRactUserListProcess(tClientRequest);
                            break;
                        case E_CommunicationType.RequestAddShareDevice:
                            GroupProcess.RequestAddShareDeviceProcess(tClientRequest);
                            break;
                        case E_CommunicationType.RequestDeviceInfo:
                            DeviceProcess.RequestProcess(tClientRequest);
                            break;
                        case E_CommunicationType.RequestModelInfo:
                            ModelInfoReceiver(tClientRequest);
                            break;
                        case E_CommunicationType.RequestOneTerminalModelInfo:
                            OneTerminalModelInfoReceiver(tClientRequest);
                            break;
                        case E_CommunicationType.RequestLimitCmdInfo:
                            LimitCmdInfoReceiver(tClientRequest);
                            break;
                        case E_CommunicationType.RequestDefaultCmdInfo:
                            DefaultCmdInfoReceiver(tClientRequest);
                            break;
                        case E_CommunicationType.RequestAutoCompleteCmd:
                            RequestAutoCompleteCmd(tClientRequest);
                            break;
                        case E_CommunicationType.RequestFactGroupInfo:
                            FactGroupInfoReceiver(tClientRequest);
                            break;
                        case E_CommunicationType.RequestShortenCommand:
                            ShortenCommandProcess.RequestProcess(tClientRequest);
                            break;
                        case E_CommunicationType.RequestShortenCommandGroup:
                            ShortenCommandProcess.RequestGroupProcess(tClientRequest);
                            break;
                        case E_CommunicationType.RequestScriptGroup:
                            ScriptProcess.RequestProcess(tClientRequest);
                            break;
                        case E_CommunicationType.RequestScriptInfo:
                            ScriptProcess.RequestScriptProcess(tClientRequest);
                            break;
                        case E_CommunicationType.RequestBatchRegisteration:
                            DeviceProcess.RequestBatchRegisteration(tClientRequest);
                            break;
                        case E_CommunicationType.RequestConnectionHistory:
                            ConnectionHistoryRequestReceiver(tClientRequest);
                            break;
                        case E_CommunicationType.RequestCommandHistory:
                            CommandHistoryRequestReceiver(tClientRequest);
                            break;
                        case E_CommunicationType.RequestFACTSearchDevice:
                            DeviceProcess.RequestFactDeviceSearchProcess(tClientRequest);
                            break;
                        case E_CommunicationType.RequestSearchDeviceForType:
                            DeviceProcess.RequestSearchDeviceForType(tClientRequest);
                            break;
                        case E_CommunicationType.RequestFACTIPSearchDevice:
                            DeviceProcess.RequestFactIPDeviceSearchProcess(tClientRequest);
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
                            ScriptProcess.RequestCfgRestoreCommand(tClientRequest);
                            break;
                        case E_CommunicationType.RequestDevicesCfgRestoreCommand:
                            ScriptProcess.RequestDevicesCfgRestoreCommand(tClientRequest);
                            break;
                        case E_CommunicationType.RequestRMSCMTSSearchDevice:
                            DeviceProcess.RequestSearchRMSCMTSDevice(tClientRequest);
                            break;
                        case E_CommunicationType.RequestSearchDeviceAuth:
                            DeviceProcess.RequestFactDeviceSearchProcess(tClientRequest, true);
                            break;
                        default:
                            GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Warning,
                                string.Format("[클라이언트 요청 수신] ClientID={0}, UserID={1}, CommType={2} : 처리되지 않은 CommunicationType 값입니다.",
                                tClientRequest.ClientID, tUserInfo != null ? tUserInfo.Account : "정보없음", tClientRequest.CommType.ToString()));
                            break;
                    }
                    tClientRequest = null;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
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
            lock (this)
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
            lock (this)
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
            lock (this)
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
            lock (this)
            {
                UseableDaemonRequestInfo tRequestInfo = (UseableDaemonRequestInfo)aClientRequest.RequestData;
                tDaemonProcessInfo = GlobalClass.s_DaemonProcessManager.GetSSHTunnelDaemonProcess();
            }
            tResultData.ResultData = new DaemonProcessInfo(tDaemonProcessInfo);
            GlobalClass.SendResultClient(tResultData);
        }

        private void CommandHistoryRequestReceiver(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tCommandHistoryRequestInfo = (TelnetCommandHistoryRequestInfo)aClientRequest.RequestData;
                using (var conn = GlobalClass.GetSqlConnection())
                {
                    var results = conn.Query("select * from dbo.RACT_Log_ExcuteCommand where ConnectionLogID = @ID", 
                        new { ID = tCommandHistoryRequestInfo.ConnectionLogID }).ToList();
                    
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

        private void ConnectionHistoryRequestReceiver(RequestCommunicationData aClientRequest)
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
                    var results = conn.Query(tQuery, new { 
                        UserID = tConnectionHistoryRequestInfo.UserID, 
                        Start = tConnectionHistoryRequestInfo.StartTime.ToString("yyyy-MM-dd HH:mm:ss"), 
                        End = tConnectionHistoryRequestInfo.EndTime.ToString("yyyy-MM-dd HH:mm:ss") 
                    }).ToList();

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

        private void FactGroupInfoReceiver(RequestCommunicationData aClientRequest)
        {
            UserInfo tUserInfo = GlobalClass.m_ClientProcess.GetUserInfo((int)aClientRequest.RequestData);
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            if (tUserInfo.IsViewAllBranch && tUserInfo.MangTypes.Count < 1)
            {
                tResultData.ResultData = GlobalClass.m_FACTGroupInfo.DeepClone();
            }
            else
            {
                tResultData.ResultData = GroupProcess.GetFactGroup(tUserInfo);
            }
            GlobalClass.SendResultClient(tResultData);
        }

        private void ModelInfoReceiver(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            tResultData.ResultData = GlobalClass.m_ModelInfoCollection.DeepClone();
            GlobalClass.SendResultClient(tResultData);
        }

        private void OneTerminalModelInfoReceiver(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                string reqData = (string)aClientRequest.RequestData;
                using (var conn = GlobalClass.GetSqlConnection())
                {
                    var modelId = conn.QueryFirstOrDefault<int>("SELECT ModelID FROM NE_NE WHERE MasterIP = @IP and Uses = 1", new { IP = reqData });
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

        private void LimitCmdInfoReceiver(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            UserInfo tUserInfo = GlobalClass.m_ClientProcess.GetUserInfo(aClientRequest.ClientID);
            if (!BaseDataLoadProcess.LoadLimitCmdInfo(tUserInfo.UserType)) return;
            tResultData.ResultData = GlobalClass.m_LimitCmdInfoCollection.DeepClone();
            GlobalClass.SendResultClient(tResultData);
        }

        private void DefaultCmdInfoReceiver(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            if (!BaseDataLoadProcess.LoadDefaultCmdInfo()) return;
            tResultData.ResultData = GlobalClass.m_DefaultCmdInfoCollection.DeepClone();
            GlobalClass.SendResultClient(tResultData);
        }

        private void RequestAutoCompleteCmd(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            if (!BaseDataLoadProcess.LoadAutoCompleteInfo((int)aClientRequest.RequestData)) return;
            tResultData.ResultData = GlobalClass.m_AutoCompleteCmdInfoCollection.DeepClone();
            GlobalClass.SendResultClient(tResultData);
        }
    }
}
