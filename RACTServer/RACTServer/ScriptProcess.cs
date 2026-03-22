using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RACTCommonClass;
using Dapper;

namespace RACTServer
{
    /// <summary>
    /// .NET 10 기반 고성능 스크립트 처리 프로세스 (Repository 패턴 적용)
    /// </summary>
    public class ScriptProcess
    {
        internal static async Task RequestProcessAsync(RequestCommunicationData aClientRequest)
        {
            var tRequesetInfo = (ScriptGroupRequestInfo)aClientRequest.RequestData;
            if (tRequesetInfo.WorkType == E_WorkType.Search) await SearchScriptGroupAsync(aClientRequest);
            else await ModifyScriptGroupAsync(aClientRequest);
        }

        private static async Task ModifyScriptGroupAsync(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tRequest = (ScriptGroupRequestInfo)aClientRequest.RequestData;
                tRequest.ScriptGroupInfo.ID = await GlobalClass.ScriptRepo.ModifyScriptGroupAsync(tRequest.WorkType, tRequest.UserID, tRequest.ScriptGroupInfo);
                tResultData.ResultData = tRequest.ScriptGroupInfo;
                GlobalClass.SendResultClient(tResultData);
            }
            catch (Exception ex)
            {
                tResultData.Error = new ErrorInfo(E_ErrorType.UnKnownError, ex.Message);
                GlobalClass.SendResultClient(tResultData);
            }
        }

        private static async Task SearchScriptGroupAsync(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tRequest = (ScriptGroupRequestInfo)aClientRequest.RequestData;
                var (groups, scripts) = await GlobalClass.ScriptRepo.GetScriptsAsync(tRequest.UserID);

                var scriptGroupList = new ScriptGroupInfoCollection();
                foreach (var g in groups) scriptGroupList.Add(g);

                foreach (var s in scripts)
                {
                    int groupId = (int)s.GroupID;
                    if (scriptGroupList.Contains(groupId))
                    {
                        scriptGroupList[groupId].ScriptList.Add(new Script {
                            ID = (int)s.Id, Name = s.Name?.ToString(), GroupID = groupId,
                            Description = s.Description?.ToString(), RawScript = s.Script?.ToString()
                        });
                    }
                }
                tResultData.ResultData = scriptGroupList;
                GlobalClass.SendResultClient(tResultData);
            }
            catch (Exception ex)
            {
                tResultData.Error = new ErrorInfo(E_ErrorType.UnKnownError, ex.Message);
                GlobalClass.SendResultClient(tResultData);
            }
        }

        internal static async Task RequestScriptProcessAsync(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tRequest = (ScriptRequestInfo)aClientRequest.RequestData;
                tRequest.ScriptInfo.ID = await GlobalClass.ScriptRepo.ModifyScriptInfoAsync(tRequest.WorkType, tRequest.UserID, tRequest.ScriptInfo);
                tResultData.ResultData = tRequest.ScriptInfo;
                GlobalClass.SendResultClient(tResultData);
            }
            catch (Exception ex)
            {
                tResultData.Error = new ErrorInfo(E_ErrorType.UnKnownError, ex.Message);
                GlobalClass.SendResultClient(tResultData);
            }
        }

        internal static async Task RequestCfgRestoreCommandAsync(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tRequest = (CfgRestoreCommandRequestInfo)aClientRequest.RequestData;
                using var conn = GlobalClass.GetSqlConnection();
                var saveInfos = (await conn.QueryAsync<CfgSaveInfo>("EXEC SP_RACT_GET_CFGSAVEINFO @IPAddress", new { IPAddress = tRequest.IPAddress })).ToList();

                if (saveInfos.Count == 0)
                {
                    var info = new CfgSaveInfo();
                    info.CfgRestoreCommands.Add(new CfgRestoreCommand());
                    tResultData.ResultData = new CfgSaveInfoCollection { info };
                }
                else
                {
                    var cmds = (await conn.QueryAsync<CfgRestoreCommand>("EXEC SP_RACT_GET_COMMANDS @ModelID, @PartID", new { ModelID = tRequest.ModelID, PartID = (int)tRequest.CommandPart })).ToList();
                    var collection = new CfgSaveInfoCollection();
                    foreach (var s in saveInfos) { foreach (var c in cmds) s.CfgRestoreCommands.Add(c); collection.Add(s); }
                    tResultData.ResultData = collection;
                }
                GlobalClass.SendResultClient(tResultData);
            }
            catch (Exception ex)
            {
                tResultData.Error = new ErrorInfo(E_ErrorType.UnKnownError, ex.Message);
                GlobalClass.SendResultClient(tResultData);
            }
        }

        internal static async Task RequestDevicesCfgRestoreCommandAsync(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var requests = (CfgRestoreCommandRequestInfoCollection)aClientRequest.RequestData;
                var deviceConfigs = new DeviceCfgSaveInfoCollection();
                using var conn = GlobalClass.GetSqlConnection();

                foreach (CfgRestoreCommandRequestInfo req in requests)
                {
                    var devCfg = new DeviceCfgSaveInfo { IPAddress = req.IPAddress };
                    var saves = (await conn.QueryAsync<CfgSaveInfo>("EXEC SP_RACT_GET_CFGSAVEINFO @IPAddress", new { IPAddress = req.IPAddress })).ToList();

                    if (saves.Count > 0)
                    {
                        foreach (var s in saves)
                        {
                            if (GlobalClass.m_ModelInfoCollection.Contains(req.ModelID)) s.CfgRestoreCommands = GlobalClass.m_ModelInfoCollection[req.ModelID].CfgRestoreCommands;
                            devCfg.CfgSaveInfoCollection.Add(s);
                        }
                    }
                    else
                    {
                        var s = new CfgSaveInfo(); s.CfgRestoreCommands.Add(new CfgRestoreCommand());
                        devCfg.CfgSaveInfoCollection.Add(s);
                    }
                    deviceConfigs.Add(devCfg);
                }
                tResultData.ResultData = deviceConfigs;
                GlobalClass.SendResultClient(tResultData);
            }
            catch (Exception ex)
            {
                tResultData.Error = new ErrorInfo(E_ErrorType.UnKnownError, ex.Message);
                GlobalClass.SendResultClient(tResultData);
            }
        }
    }
}
