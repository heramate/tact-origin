using System;
using System.Collections.Generic;
using System.Linq;
using RACTCommonClass;
using Dapper;

namespace RACTServer
{
    public class ScriptProcess
    {
        /// <summary>
        /// 요청을 처리 합니다.
        /// </summary>
        /// <param name="aClientRequest"></param>
        internal static void RequestProcess(RequestCommunicationData aClientRequest)
        {
            ScriptGroupRequestInfo tRequesetInfo = aClientRequest.RequestData as ScriptGroupRequestInfo;

            switch (tRequesetInfo.WorkType)
            {
                case E_WorkType.Search:
                    SearchScriptGroup(aClientRequest);
                    break;
                default:
                    ModifyScriptGroup(aClientRequest);
                    break;
            }
        }

        /// <summary>
        /// 스크립트 그룹을 수정 합니다.
        /// </summary>
        /// <param name="aClientRequest"></param>
        private static void ModifyScriptGroup(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tRequestCommandInfo = (ScriptGroupRequestInfo)aClientRequest.RequestData;
                var tInfo = tRequestCommandInfo.ScriptGroupInfo;

                using (var conn = GlobalClass.GetSqlConnection())
                {
                    string tQuery = "EXEC SP_RACT_Modify_ScriptGroup @WorkType, @ID, @UserID, @Name, @Description";
                    var result = conn.QueryFirstOrDefault(tQuery, new
                    {
                        WorkType = (int)tRequestCommandInfo.WorkType,
                        ID = tInfo.ID,
                        UserID = tRequestCommandInfo.UserID,
                        Name = tInfo.Name,
                        Description = tInfo.Description
                    });

                    if (result != null)
                    {
                        tInfo.ID = Convert.ToInt32(((IDictionary<string, object>)result)["ID"]);
                    }
                }

                tResultData.ResultData = tInfo;
                GlobalClass.SendResultClient(tResultData);
            }
            catch (Exception ex)
            {
                tResultData.Error.Error = E_ErrorType.UnKnownError;
                tResultData.Error.ErrorString = ex.Message;
                GlobalClass.SendResultClient(tResultData);
            }
        }

        /// <summary>
        /// 스크립트 그룹 목록을 가져오기 합니다.
        /// </summary>
        /// <param name="aClientRequest"></param>
        private static void SearchScriptGroup(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tRequestCommandInfo = (ScriptGroupRequestInfo)aClientRequest.RequestData;

                using (var conn = GlobalClass.GetSqlConnection())
                {
                    conn.Open();
                    using (var multi = conn.QueryMultiple("EXEC SP_RACT_Get_Script @UserID", new { UserID = tRequestCommandInfo.UserID }))
                    {
                        var groups = multi.Read<ScriptGroupInfo>().ToList();
                        var scripts = multi.Read<dynamic>().ToList();

                        var scriptGroupList = new ScriptGroupInfoCollection();
                        foreach (var g in groups) scriptGroupList.Add(g);

                        foreach (var s in scripts)
                        {
                            int groupId = (int)s.GroupID;
                            if (scriptGroupList.Contains(groupId))
                            {
                                var script = new Script
                                {
                                    ID = (int)s.Id,
                                    Name = s.Name?.ToString(),
                                    GroupID = groupId,
                                    Description = s.Description?.ToString(),
                                    RawScript = s.Script?.ToString()
                                };
                                scriptGroupList[groupId].ScriptList.Add(script);
                            }
                        }
                        tResultData.ResultData = scriptGroupList;
                    }
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

        internal static void RequestScriptProcess(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tRequestCommandInfo = (ScriptRequestInfo)aClientRequest.RequestData;
                var tInfo = tRequestCommandInfo.ScriptInfo;

                using (var conn = GlobalClass.GetSqlConnection())
                {
                    string tQuery = "EXEC SP_RACT_Modify_ScriptInfo @WorkType, @ID, @GroupID, @UserID, @Name, @Description, @RawScript";
                    var result = conn.QueryFirstOrDefault(tQuery, new
                    {
                        WorkType = (int)tRequestCommandInfo.WorkType,
                        ID = tInfo.ID,
                        GroupID = tInfo.GroupID,
                        UserID = tRequestCommandInfo.UserID,
                        Name = tInfo.Name,
                        Description = tInfo.Description,
                        RawScript = tInfo.RawScript
                    });

                    if (result != null)
                    {
                        tInfo.ID = Convert.ToInt32(((IDictionary<string, object>)result)["ID"]);
                    }
                }

                tResultData.ResultData = tInfo;
                GlobalClass.SendResultClient(tResultData);
            }
            catch (Exception ex)
            {
                tResultData.Error.Error = E_ErrorType.UnKnownError;
                tResultData.Error.ErrorString = ex.Message;
                GlobalClass.SendResultClient(tResultData);
            }
        }

        /// <summary>
        /// Cfg복원명령을 가져온다.
        /// </summary>
        /// <param name="aClientRequest"></param>
        internal static void RequestCfgRestoreCommand(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tRequestCommandInfo = (CfgRestoreCommandRequestInfo)aClientRequest.RequestData;
                var tCfgSaveInfos = new CfgSaveInfoCollection();

                using (var conn = GlobalClass.GetSqlConnection())
                {
                    conn.Open();
                    var tempSaveInfos = conn.Query<CfgSaveInfo>("EXEC SP_RACT_GET_CFGSAVEINFO @IPAddress", new { IPAddress = tRequestCommandInfo.IPAddress }).ToList();

                    if (tempSaveInfos.Count == 0)
                    {
                        var tCfgSaveInfo = new CfgSaveInfo();
                        tCfgSaveInfo.CfgRestoreCommands.Add(new CfgRestoreCommand());
                        tCfgSaveInfos.Add(tCfgSaveInfo);
                    }
                    else
                    {
                        var commands = conn.Query<CfgRestoreCommand>("EXEC SP_RACT_GET_COMMANDS @ModelID, @PartID", 
                                                                   new { ModelID = tRequestCommandInfo.ModelID, PartID = (int)tRequestCommandInfo.CommandPart }).ToList();

                        foreach (var saveInfo in tempSaveInfos)
                        {
                            foreach (var cmdInfo in commands)
                            {
                                saveInfo.CfgRestoreCommands.Add(new CfgRestoreCommand
                                {
                                    CmdSeq = cmdInfo.CmdSeq,
                                    Cmd = cmdInfo.Cmd,
                                    T_Prompt = cmdInfo.T_Prompt
                                });
                            }
                            tCfgSaveInfos.Add(saveInfo);
                        }
                    }
                }

                tResultData.ResultData = tCfgSaveInfos;
                GlobalClass.SendResultClient(tResultData);
            }
            catch (Exception ex)
            {
                tResultData.Error.Error = E_ErrorType.UnKnownError;
                tResultData.Error.ErrorString = ex.Message;
                GlobalClass.SendResultClient(tResultData);
            }
        }

        /// <summary>
        /// 여러대의 Cfg복원명령리스트를 가져온다.
        /// </summary>
        /// <param name="aClientRequest"></param>
        internal static void RequestDevicesCfgRestoreCommand(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tRequestCommandInfos = (CfgRestoreCommandRequestInfoCollection)aClientRequest.RequestData;
                var tDeviceCfgSaveInfos = new DeviceCfgSaveInfoCollection();

                using (var conn = GlobalClass.GetSqlConnection())
                {
                    conn.Open();
                    foreach (CfgRestoreCommandRequestInfo tRequestCommandInfo in tRequestCommandInfos)
                    {
                        var tDeviceCfgSaveInfo = new DeviceCfgSaveInfo { IPAddress = tRequestCommandInfo.IPAddress };
                        var tCfgSaveInfos = new CfgSaveInfoCollection();

                        var saveRows = conn.Query<CfgSaveInfo>("EXEC SP_RACT_GET_CFGSAVEINFO @IPAddress", new { IPAddress = tRequestCommandInfo.IPAddress }).ToList();

                        if (saveRows.Count > 0)
                        {
                            foreach (var tCfgSaveInfo in saveRows)
                            {
                                if (GlobalClass.m_ModelInfoCollection.Contains(tRequestCommandInfo.ModelID))
                                {
                                    tCfgSaveInfo.CfgRestoreCommands = GlobalClass.m_ModelInfoCollection[tRequestCommandInfo.ModelID].CfgRestoreCommands;
                                }
                                tCfgSaveInfos.Add(tCfgSaveInfo);
                            }
                        }
                        else
                        {
                            var tCfgSaveInfo = new CfgSaveInfo();
                            tCfgSaveInfo.CfgRestoreCommands.Add(new CfgRestoreCommand());
                            tCfgSaveInfos.Add(tCfgSaveInfo);
                        }

                        tDeviceCfgSaveInfo.CfgSaveInfoCollection = tCfgSaveInfos;
                        tDeviceCfgSaveInfos.Add(tDeviceCfgSaveInfo);
                    }
                }

                tResultData.ResultData = tDeviceCfgSaveInfos;
                GlobalClass.SendResultClient(tResultData);
            }
            catch (Exception ex)
            {
                tResultData.Error.Error = E_ErrorType.UnKnownError;
                tResultData.Error.ErrorString = ex.Message;
                GlobalClass.SendResultClient(tResultData);
            }
        }
    }
}
