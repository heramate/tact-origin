using RACTCommonClass;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Linq;
using Dapper;
using System.Collections;

namespace RACTServer
{
    /// <summary>
    /// .NET 10 기반 초고성능 클라이언트 요청 처리 프로세스
    /// </summary>
    public class ClientResponseProcess : IDisposable
    {
        private readonly Channel<RequestCommunicationData> _requestChannel;
        private Task[]? _workerTasks;
        private readonly CancellationTokenSource _cts = new();

        public ClientResponseProcess()
        {
            // 수천 명의 클라이언트에 대응하기 위해 고성능 Unbounded 채널 생성
            // SingleReader=false (멀티워커), SingleWriter=true (게이트웨이 스레드)
            _requestChannel = Channel.CreateUnbounded<RequestCommunicationData>(new UnboundedChannelOptions
            {
                SingleReader = false,
                SingleWriter = false // 여러 곳에서 AddRequest 호출 가능
            });
        }

        public void Dispose()
        {
            Stop();
            _cts.Dispose();
            GC.SuppressFinalize(this);
        }

        public bool Start()
        {
            try
            {
                // .NET 10 환경에서는 코어 수의 2~4배 정도의 워커가 적당함 (비동기 I/O 최적화)
                int workerCount = Math.Max(Environment.ProcessorCount * 2, 16);
                _workerTasks = new Task[workerCount];
                
                for (int i = 0; i < workerCount; i++)
                {
                    _workerTasks[i] = ProcessRequestsAsync(_cts.Token);
                }
                return true;
            }
            catch (Exception ex)
            {
                GlobalClass.m_LogProcess?.PrintLog(E_FileLogType.Error, "ClientResponseProcess Start Failed: " + ex.Message);
                return false;
            }
        }

        public void Stop()
        {
            _cts.Cancel();
            _requestChannel.Writer.TryComplete();

            if (_workerTasks != null)
            {
                try { Task.WaitAll(_workerTasks, TimeSpan.FromSeconds(3)); } catch { }
            }
        }

        public void AddRequest(RequestCommunicationData aRequest)
        {
            // 채널에 비차단 방식으로 요청 추가
            if (!_requestChannel.Writer.TryWrite(aRequest))
            {
                // 실패 시 백그라운드로 쓰기 시도 (압력 조절)
                _ = _requestChannel.Writer.WriteAsync(aRequest);
            }
        }

        private async Task ProcessRequestsAsync(CancellationToken ct)
        {
            // .NET 10 최적화: 채널 리더를 통해 비차단 방식으로 메시지 소비
            await foreach (var request in _requestChannel.Reader.ReadAllAsync(ct))
            {
                try
                {
                    await HandleRequestAsync(request);
                }
                catch (Exception ex)
                {
                    GlobalClass.m_LogProcess?.PrintLog(E_FileLogType.Error, $"Request Handling Error ({request.CommType}): {ex.Message}");
                }
            }
        }

        private async ValueTask HandleRequestAsync(RequestCommunicationData request)
        {
            // .NET 10 최적화: ValueTask를 사용하여 동기 완료 시 할당 제거
            switch (request.CommType)
            {
                case E_CommunicationType.RequestGroupInfo:
                    await GroupProcess.RequestProcessAsync(request);
                    break;
                case E_CommunicationType.RequestDeviceInfo:
                    await DeviceProcess.RequestProcessAsync(request);
                    break;
                case E_CommunicationType.RequestConnectionHistory:
                    await ConnectionHistoryRequestReceiverAsync(request);
                    break;
                case E_CommunicationType.RequestCommandHistory:
                    await CommandHistoryRequestReceiverAsync(request);
                    break;
                case E_CommunicationType.RequestShortenCommand:
                    await ShortenCommandProcess.RequestProcessAsync(request);
                    break;
                case E_CommunicationType.RequestScriptInfo:
                    await ScriptProcess.RequestScriptProcessAsync(request);
                    break;
                // ... 기타 케이스들
                default:
                    await HandleOtherRequestsAsync(request);
                    break;
            }
        }

        private async ValueTask HandleOtherRequestsAsync(RequestCommunicationData request)
        {
            switch (request.CommType)
            {
                case E_CommunicationType.RequestRactUserList:
                    await GroupProcess.RequestRactUserListProcessAsync(request);
                    break;
                case E_CommunicationType.RequestAddShareDevice:
                    await GroupProcess.RequestAddShareDeviceProcessAsync(request);
                    break;
                case E_CommunicationType.RequestOneTerminalModelInfo:
                    await OneTerminalModelInfoReceiverAsync(request);
                    break;
                case E_CommunicationType.RequestLimitCmdInfo:
                    await LimitCmdInfoReceiverAsync(request);
                    break;
                case E_CommunicationType.RequestDefaultCmdInfo:
                    await DefaultCmdInfoReceiverAsync(request);
                    break;
                case E_CommunicationType.RequestAutoCompleteCmd:
                    await RequestAutoCompleteCmdAsync(request);
                    break;
                case E_CommunicationType.RequestFactGroupInfo:
                    await FactGroupInfoReceiverAsync(request);
                    break;
                case E_CommunicationType.RequestShortenCommandGroup:
                    await ShortenCommandProcess.RequestGroupProcessAsync(request);
                    break;
                case E_CommunicationType.RequestScriptGroup:
                    await ScriptProcess.RequestProcessAsync(request);
                    break;
                case E_CommunicationType.RequestBatchRegisteration:
                    await DeviceProcess.RequestBatchRegisterationAsync(request);
                    break;
                case E_CommunicationType.RequestFACTSearchDevice:
                    await DeviceProcess.RequestFactDeviceSearchProcessAsync(request);
                    break;
                case E_CommunicationType.RequestSearchDeviceForType:
                    await DeviceProcess.RequestSearchDeviceForTypeAsync(request);
                    break;
                case E_CommunicationType.RequestFACTIPSearchDevice:
                    await DeviceProcess.RequestFactIPDeviceSearchProcessAsync(request);
                    break;
                case E_CommunicationType.RequestCfgRestoreCommand:
                    await ScriptProcess.RequestCfgRestoreCommandAsync(request);
                    break;
                case E_CommunicationType.RequestDevicesCfgRestoreCommand:
                    await ScriptProcess.RequestDevicesCfgRestoreCommandAsync(request);
                    break;
                case E_CommunicationType.RequestRMSCMTSSearchDevice:
                    await DeviceProcess.RequestSearchRMSCMTSDeviceAsync(request);
                    break;
                case E_CommunicationType.RequestSearchDeviceAuth:
                    await DeviceProcess.RequestFactDeviceSearchProcessAsync(request, true);
                    break;
            }
        }

        // 기존 로직 비동기 래퍼들 (생략 가능하지만 호환성 유지)
        private async Task CommandHistoryRequestReceiverAsync(RequestCommunicationData aClientRequest)
        {
            var resultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var historyReq = (TelnetCommandHistoryRequestInfo)aClientRequest.RequestData;
                using var conn = GlobalClass.GetSqlConnection();
                var results = (await conn.QueryAsync("select * from dbo.RACT_Log_ExcuteCommand where ConnectionLogID = @ID", 
                    new { ID = historyReq.ConnectionLogID })).ToList();
                
                var historyList = new TelnetCommandHistoryInfoCollection();
                foreach (var row in results)
                {
                    var dict = (IDictionary<string, object>)row;
                    historyList.Add(new TelnetCommandHistoryInfo {
                        Time = dict["DateTime"] != DBNull.Value ? Convert.ToDateTime(dict["DateTime"]) : DateTime.MinValue,
                        Command = dict["Command"]?.ToString()
                    });
                }
                resultData.ResultData = historyList;
            }
            catch (Exception ex) { resultData.Error = new ErrorInfo(E_ErrorType.UnKnownError, ex.Message); }
            GlobalClass.SendResultClient(resultData);
        }

        private async Task ConnectionHistoryRequestReceiverAsync(RequestCommunicationData aClientRequest)
        {
            var resultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var historyReq = (ConnectionHistoryRequestInfo)aClientRequest.RequestData;
                using var conn = GlobalClass.GetSqlConnection();
                var results = (await conn.QueryAsync(@"select L.* ,N.NEName, N.MasterIP as IPAddress 
                                  from RACT_LOG_DeviceConnection L inner Join Ne_Ne N On N.NEID = L.NEID 
                                  where L.userid = @UserID and dateTime between @Start and @End order by L.id desc", 
                    new { UserID = historyReq.UserID, Start = historyReq.StartTime, End = historyReq.EndTime })).ToList();

                var historyList = new ConnectionHistoryInfoCollection();
                foreach (var row in results)
                {
                    var dict = (IDictionary<string, object>)row;
                    historyList.Add(new DeviceConnectionHistoryInfo {
                        ID = Convert.ToInt32(dict["ID"]),
                        DeviceID = Convert.ToInt32(dict["NEID"]),
                        DeviceName = dict["NEName"]?.ToString(),
                        ConnectionTime = Convert.ToDateTime(dict["DateTime"]),
                        ConnectionType = (E_DeviceConnectType)Convert.ToInt32(dict["ConnectLogType"]),
                        IPAddress = dict["IPAddress"]?.ToString()
                    });
                }
                resultData.ResultData = historyList;
            }
            catch (Exception ex) { resultData.Error = new ErrorInfo(E_ErrorType.UnKnownError, ex.Message); }
            GlobalClass.SendResultClient(resultData);
        }

        private async Task FactGroupInfoReceiverAsync(RequestCommunicationData aClientRequest)
        {
            var userInfo = GlobalClass.m_ClientProcess.GetUserInfo((int)aClientRequest.RequestData);
            var resultData = new ResultCommunicationData(aClientRequest);
            resultData.ResultData = (userInfo.IsViewAllBranch && userInfo.MangTypes.Count < 1) 
                ? GlobalClass.m_FACTGroupInfo.DeepClone() 
                : await GroupProcess.GetFactGroupAsync(userInfo);
            GlobalClass.SendResultClient(resultData);
        }

        private async Task OneTerminalModelInfoReceiverAsync(RequestCommunicationData aClientRequest)
        {
            var resultData = new ResultCommunicationData(aClientRequest);
            try
            {
                string ip = (string)aClientRequest.RequestData;
                using var conn = GlobalClass.GetSqlConnection();
                var modelId = await conn.QueryFirstOrDefaultAsync<int>("SELECT ModelID FROM NE_NE WHERE MasterIP = @IP and Uses = 1", new { IP = ip });
                if (modelId != 0) resultData.ResultData = GlobalClass.m_ModelInfoCollection[modelId];
            }
            catch (Exception ex) { resultData.Error = new ErrorInfo(E_ErrorType.UnKnownError, ex.Message); }
            GlobalClass.SendResultClient(resultData);
        }

        private async Task LimitCmdInfoReceiverAsync(RequestCommunicationData aClientRequest)
        {
            var resultData = new ResultCommunicationData(aClientRequest);
            var userInfo = GlobalClass.m_ClientProcess.GetUserInfo(aClientRequest.ClientID);
            if (await BaseDataLoadProcess.LoadLimitCmdInfoAsync(userInfo.UserType))
                resultData.ResultData = GlobalClass.m_LimitCmdInfoCollection.DeepClone();
            GlobalClass.SendResultClient(resultData);
        }

        private async Task DefaultCmdInfoReceiverAsync(RequestCommunicationData aClientRequest)
        {
            var resultData = new ResultCommunicationData(aClientRequest);
            if (await BaseDataLoadProcess.LoadDefaultCmdInfoAsync())
                resultData.ResultData = GlobalClass.m_DefaultCmdInfoCollection.DeepClone();
            GlobalClass.SendResultClient(resultData);
        }

        private async Task RequestAutoCompleteCmdAsync(RequestCommunicationData aClientRequest)
        {
            var resultData = new ResultCommunicationData(aClientRequest);
            if (await BaseDataLoadProcess.LoadAutoCompleteInfoAsync((int)aClientRequest.RequestData))
                resultData.ResultData = GlobalClass.m_AutoCompleteCmdInfoCollection.DeepClone();
            GlobalClass.SendResultClient(resultData);
        }
    }
}
