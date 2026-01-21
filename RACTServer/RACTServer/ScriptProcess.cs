using System;
using System.Collections.Generic;
using System.Text;
using RACTCommonClass;
using MKLibrary.MKData;

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
        /// 단축 명령을 수정 합니다.
        /// </summary>
        /// <param name="aClientRequest"></param>
        private static void ModifyScriptGroup(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = null;
            MKDBWorkItem tDBWI = null;
            MKDataSet tDataSet = null;
            string tQueryString = "";
            ScriptGroupRequestInfo tRequestCommandInfo;
            ScriptGroupInfo tInfo;
            try
            {
                tResultData = new ResultCommunicationData(aClientRequest);
                tRequestCommandInfo = (ScriptGroupRequestInfo)aClientRequest.RequestData;
                tInfo = tRequestCommandInfo.ScriptGroupInfo;

                tQueryString = "EXEC SP_RACT_Modify_ScriptGroup {0}, {1}, {2}, '{3}', '{4}'";

                tQueryString = string.Format(tQueryString, (int)tRequestCommandInfo.WorkType, tInfo.ID, tRequestCommandInfo.UserID, tInfo.Name, tInfo.Description);

                tDBWI = GlobalClass.m_DBPool.GetDBWorkItem();
                if (tDBWI.ExecuteQuery(tQueryString, out tDataSet) != E_DBProcessError.Success)
                {
                    System.Diagnostics.Debug.WriteLine(tDBWI.ErrorString);
                    tResultData.Error.Error = E_ErrorType.UnKnownError;
                    tResultData.Error.ErrorString = tDBWI.ErrorString;
                }
                else
                {
                    if (tDataSet != null)
                    {
                        for (int i = 0; i < tDataSet.RecordCount; i++)
                        {
                            tInfo.ID = tDataSet.GetInt32("ID");
                        }
                    }
                    tResultData.ResultData = tInfo;
                }
                GlobalClass.SendResultClient(tResultData);
            }
            catch (Exception ex)
            {
                tResultData.Error.Error = E_ErrorType.UnKnownError;
                GlobalClass.SendResultClient(tResultData);
            }
            finally
            {
                if (tDataSet != null)
                    MKOleDBClass.CloseDataSet(tDataSet);
            }
        }

        /// <summary>
        /// 단축 명령 목록을 가져오기 합니다.
        /// </summary>
        /// <param name="aClientRequest"></param>
        private static void SearchScriptGroup(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = null;
            MKDBWorkItem tDBWI = null;
            MKDataSet tDataSet = null;
            string tQueryString = "";
            ScriptGroupRequestInfo tRequestCommandInfo;
            ScriptGroupInfo tScriptGroupInfo = null;
            ScriptGroupInfoCollection tScriptGroupInfoList = new ScriptGroupInfoCollection();
            Script tScript;
           

            string tTempString = "";

            try
            {
                tResultData = new ResultCommunicationData(aClientRequest);
                tRequestCommandInfo = (ScriptGroupRequestInfo)aClientRequest.RequestData;

                tQueryString = "EXEC SP_RACT_Get_Script {0}";
                tQueryString = string.Format(tQueryString, tRequestCommandInfo.UserID);

                tDBWI = GlobalClass.m_DBPool.GetDBWorkItem();
                if (tDBWI.ExecuteQuery(tQueryString, out tDataSet) != E_DBProcessError.Success)
                {
                    System.Diagnostics.Debug.WriteLine(tDBWI.ErrorString);
                    tResultData.Error.Error = E_ErrorType.UnKnownError;
                    tResultData.Error.ErrorString = tDBWI.ErrorString;
                }
                else
                {
                    if (tDataSet != null)
                    {
                        for (int i = 0; i < tDataSet.RecordCount; i++)
                        {
                            tScriptGroupInfo = new ScriptGroupInfo();
                            tScriptGroupInfo.ID = tDataSet.GetInt32("Id");
                            tScriptGroupInfo.Name = tDataSet.GetString("Name");
                            tScriptGroupInfo.Description = tDataSet.GetString("Description");
                            tScriptGroupInfoList.Add(tScriptGroupInfo);

                            tDataSet.MoveNext();
                        }

                        tDataSet.CurrentTableIndex++;


                        for (int i = 0; i < tDataSet.RecordCount; i++)
                        {
                            if (tScriptGroupInfoList.Contains(tDataSet.GetInt32("GroupID")))
                            {
                                tScriptGroupInfo = tScriptGroupInfoList[tDataSet.GetInt32("GroupId")];

                                tScript = new Script();
                                tScript.ID = tDataSet.GetInt32("Id");
                                tScript.Name = tDataSet.GetString("Name");
                                tScript.GroupID = tDataSet.GetInt32("GroupID");
                                tScript.Description = tDataSet.GetString("Description");
                                tScript.RawScript = tDataSet.GetString("Script");
                                if (tScript.RawScript == null) tScript.RawScript = "";

                                tTempString += tScript.ID + ",";
                                tScriptGroupInfo.ScriptList.Add(tScript);
                            }
                            tDataSet.MoveNext();
                        }
                    }
                }

                tResultData.ResultData = tScriptGroupInfoList;

                GlobalClass.SendResultClient(tResultData);
            }
            catch (Exception ex)
            {
                tResultData.Error.Error = E_ErrorType.UnKnownError;
                GlobalClass.SendResultClient(tResultData);
            }
            finally
            {
                if (tDataSet != null)
                    MKOleDBClass.CloseDataSet(tDataSet);
            }
        }

        internal static void RequestScriptProcess(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = null;
            MKDBWorkItem tDBWI = null;
            MKDataSet tDataSet = null;
            string tQueryString = "";
            ScriptRequestInfo tRequestCommandInfo;
            Script tInfo;
            try
            {
                tResultData = new ResultCommunicationData(aClientRequest);
                tRequestCommandInfo = (ScriptRequestInfo)aClientRequest.RequestData;
                tInfo = tRequestCommandInfo.ScriptInfo;

                tQueryString = "EXEC SP_RACT_Modify_ScriptInfo {0}, {1}, {2}, {3}, '{4}', '{5}','{6}'";

                tQueryString = string.Format(tQueryString, (int)tRequestCommandInfo.WorkType, tInfo.ID, tInfo.GroupID, tRequestCommandInfo.UserID, tInfo.Name, tInfo.Description,tInfo.RawScript.Replace("\'","\'\'"));

                tDBWI = GlobalClass.m_DBPool.GetDBWorkItem();
                if (tDBWI.ExecuteQuery(tQueryString, out tDataSet) != E_DBProcessError.Success)
                {
                    System.Diagnostics.Debug.WriteLine(tDBWI.ErrorString);
                    tResultData.Error.Error = E_ErrorType.UnKnownError;
                    tResultData.Error.ErrorString = tDBWI.ErrorString;
                }
                else
                {
                    if (tDataSet != null)
                    {
                        for (int i = 0; i < tDataSet.RecordCount; i++)
                        {
                            tInfo.ID = tDataSet.GetInt32("ID");
                        }
                    }
                    tResultData.ResultData = tInfo;
                }
                GlobalClass.SendResultClient(tResultData);
            }
            catch (Exception ex)
            {
                tResultData.Error.Error = E_ErrorType.UnKnownError;
                GlobalClass.SendResultClient(tResultData);
            }
            finally
            {
                if (tDataSet != null)
                    MKOleDBClass.CloseDataSet(tDataSet);
            }
        }

        /// <summary>
        /// 2013-01-11 - shinyn - Cfg복원명령을 가져온다.
        /// </summary>
        /// <param name="aClientRequest"></param>
        internal static void RequestCfgRestoreCommand(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = null;
            MKDBWorkItem tDBWI = null;
            MKDataSet tDataSet = null;
            MKDataSet tDsCommand = null;
            string tQueryString = "";
            CfgRestoreCommandRequestInfo tRequestCommandInfo;
            CfgSaveInfoCollection tCfgSaveInfos = new CfgSaveInfoCollection();

            try
            {
                tResultData = new ResultCommunicationData(aClientRequest);
                tRequestCommandInfo = (CfgRestoreCommandRequestInfo)aClientRequest.RequestData;

                tDBWI = GlobalClass.m_DBPool.GetDBWorkItem();

                tQueryString = string.Format("EXEC SP_RACT_GET_CFGSAVEINFO '{0}'",
                                             tRequestCommandInfo.IPAddress);

                if (tDBWI.ExecuteQuery(tQueryString, out tDataSet) != E_DBProcessError.Success)
                {
                    System.Diagnostics.Debug.WriteLine(tDBWI.ErrorString);
                    tResultData.Error.Error = E_ErrorType.UnKnownError;
                    tResultData.Error.ErrorString = tDBWI.ErrorString;
                }
                else
                {
                    if (tDataSet != null)
                    {
                        if (tDataSet.RecordCount == 0)
                        {
                            CfgSaveInfo tCfgSaveInfo = new CfgSaveInfo();

                            tCfgSaveInfo.CfgRestoreCommands.Add(new CfgRestoreCommand());
                            tCfgSaveInfos.Add(tCfgSaveInfo);
                        }
                        else
                        {
                            tQueryString = string.Format("EXEC SP_RACT_GET_COMMANDS {0}, {1}",
                                                 tRequestCommandInfo.ModelID,
                                                 (int)tRequestCommandInfo.CommandPart);




                            if (tDBWI.ExecuteQuery(tQueryString, out tDsCommand) != E_DBProcessError.Success)
                            {
                                System.Diagnostics.Debug.WriteLine(tDBWI.ErrorString);
                                tResultData.Error.Error = E_ErrorType.UnKnownError;
                                tResultData.Error.ErrorString = tDBWI.ErrorString;
                            }
                        }

                        for (int i = 0; i < tDataSet.RecordCount; i++)
                        {
                            CfgSaveInfo tCfgSaveInfo = new CfgSaveInfo();

                            tCfgSaveInfo.Iden = tDataSet.GetInt64("Iden");
                            tCfgSaveInfo.StTime = tDataSet.GetDateTime("StTime");
                            tCfgSaveInfo.FileName = tDataSet.GetString("FileName");
                            tCfgSaveInfo.FileExtend = tDataSet.GetString("FileExtend");
                            tCfgSaveInfo.FTPServerIP = tDataSet.GetString("FTPServerIP");
                            tCfgSaveInfo.CenterFTPID = tDataSet.GetString("CenterFTPID");
                            tCfgSaveInfo.CenterFTPPW = tDataSet.GetString("CenterFTPPW");

                            tDsCommand.CurrentRowIndex = 0;

                            

                            for (int j = 0; j < tDsCommand.RecordCount; j++)
                            {
                                CfgRestoreCommand tCfgRestoreCommand = new CfgRestoreCommand();    

                                tCfgRestoreCommand.CmdSeq = tDsCommand.GetInt32("CmdSeq");
                                tCfgRestoreCommand.Cmd = tDsCommand.GetString("Cmd");
                                tCfgRestoreCommand.T_Prompt = tDsCommand.GetString("T_Prompt");

                                tCfgSaveInfo.CfgRestoreCommands.Add(tCfgRestoreCommand);

                                tDsCommand.MoveNext();
                            }

                            tCfgSaveInfos.Add(tCfgSaveInfo);

                            tDataSet.MoveNext();
                        }
                    }
                }

                tResultData.ResultData = tCfgSaveInfos;

                GlobalClass.SendResultClient(tResultData);
            }
            catch (Exception ex)
            {
                tResultData.Error.Error = E_ErrorType.UnKnownError;
                GlobalClass.SendResultClient(tResultData);
            }
            finally
            {
                if (tDataSet != null)
                    MKOleDBClass.CloseDataSet(tDataSet);
                tCfgSaveInfos.Clear();
            }


        }

        /// <summary>
        /// 2013-01-11 - shinyn - 여러대의 Cfg복원명령리스트를 가져온다.
        /// </summary>
        /// <param name="aClientRequest"></param>
        internal static void RequestDevicesCfgRestoreCommand(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = null;
            MKDBWorkItem tDBWI = null;
            MKDataSet tDataSet = null;
            MKDataSet tDsCommand = null;
            string tQueryString = "";
            CfgRestoreCommandRequestInfoCollection tRequestCommandInfos = null;
            DeviceCfgSaveInfoCollection tDeviceCfgSaveInfos = new DeviceCfgSaveInfoCollection();

            try
            {
                tResultData = new ResultCommunicationData(aClientRequest);
                tRequestCommandInfos = (CfgRestoreCommandRequestInfoCollection)aClientRequest.RequestData;


                foreach (CfgRestoreCommandRequestInfo tRequestCommandInfo in tRequestCommandInfos)
                {
                    DeviceCfgSaveInfo tDeviceCfgSaveInfo = new DeviceCfgSaveInfo();

                    tDeviceCfgSaveInfo.IPAddress = tRequestCommandInfo.IPAddress;

                    CfgSaveInfoCollection tCfgSaveInfos = new CfgSaveInfoCollection();

                    tDBWI = GlobalClass.m_DBPool.GetDBWorkItem();

                    tQueryString = string.Format("EXEC SP_RACT_GET_CFGSAVEINFO '{0}'",
                                                 tRequestCommandInfo.IPAddress);

                    if (tDBWI.ExecuteQuery(tQueryString, out tDataSet) != E_DBProcessError.Success)
                    {
                        System.Diagnostics.Debug.WriteLine(tDBWI.ErrorString);
                        tResultData.Error.Error = E_ErrorType.UnKnownError;
                        tResultData.Error.ErrorString = tDBWI.ErrorString;
                    }
                    else
                    {
                        if (tDataSet != null)
                        {

                            if (tDataSet.RecordCount == 0)
                            {
                                CfgSaveInfo tCfgSaveInfo = new CfgSaveInfo();

                                tCfgSaveInfo.CfgRestoreCommands.Add(new CfgRestoreCommand());
                                tCfgSaveInfos.Add(tCfgSaveInfo);
                            }

                            for (int i = 0; i < tDataSet.RecordCount; i++)
                            {
                                CfgSaveInfo tCfgSaveInfo = new CfgSaveInfo();

                                tCfgSaveInfo.Iden = tDataSet.GetInt64("Iden");
                                tCfgSaveInfo.StTime = tDataSet.GetDateTime("StTime");
                                tCfgSaveInfo.FileName = tDataSet.GetString("FileName");
                                tCfgSaveInfo.FileExtend = tDataSet.GetString("FileExtend");
                                tCfgSaveInfo.FTPServerIP = tDataSet.GetString("FTPServerIP");
                                tCfgSaveInfo.CenterFTPID = tDataSet.GetString("CenterFTPID");
                                tCfgSaveInfo.CenterFTPPW = tDataSet.GetString("CenterFTPPW");

                                ModelInfo tModelInfo = GlobalClass.m_ModelInfoCollection[tRequestCommandInfo.ModelID];

                                tCfgSaveInfo.CfgRestoreCommands = tModelInfo.CfgRestoreCommands;

                                tCfgSaveInfos.Add(tCfgSaveInfo);

                                tDataSet.MoveNext();
                            }                            
                        }
                    }

                    tDeviceCfgSaveInfo.CfgSaveInfoCollection = tCfgSaveInfos;                    

                    tDeviceCfgSaveInfos.Add(tDeviceCfgSaveInfo);
                }

                tResultData.ResultData = tDeviceCfgSaveInfos;
                
                GlobalClass.SendResultClient(tResultData);
            }
            catch (Exception ex)
            {
                tResultData.Error.Error = E_ErrorType.UnKnownError;
                GlobalClass.SendResultClient(tResultData);
            }
            finally
            {
                if (tDataSet != null)
                    MKOleDBClass.CloseDataSet(tDataSet);
                tDeviceCfgSaveInfos.Clear();
            }

        }
    }
}
