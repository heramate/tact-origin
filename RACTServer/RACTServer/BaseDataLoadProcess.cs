using System;
using System.Collections.Generic;
using System.Text;

using System.Threading;
using ACPS.CommonConfigCompareClass;
using RACTCommonClass;
using Dapper;
using System.Data.SqlClient;
using System.Linq;

namespace RACTServer
{
    public class BaseDataLoadProcess
    {
        /// <summary>
        /// 서버의 초기정보를 Load하는 클래스 입니다.
        /// </summary>
        /// <returns>데이터 로드의 성공 요부 입니다.</returns>
        public static bool LoadBaseData()
        {
            GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "기초 데이터를 로드 합니다.");
            if (!LoadFACTGroupInfo()) return false;
            if (!LoadModelInfo()) return false;
            if (!LoadUnUsedLimit()) return false;
            /* 2019-01-16 제한명령어 명령어별 권한 변경건 수정 */
            //if (!LoadLimitCmdInfo()) return false;
            if (!LoadDefaultCmdInfo()) return false;
            // 2013-05-02- shinyn - 장비정보를 로드합니다.
            //if (!LoadDeviceInfo()) return false;


            return true;

        }

        /// <summary>
        /// 제한 날짜를 로드 합니다.
        /// </summary>
        /// <returns></returns>
        private static bool LoadUnUsedLimit()
        {
            try
            {
                using (var conn = GlobalClass.GetSqlConnection())
                {
                    string tQuery = "select UnUsedLimit from RACT_Manage";
                    GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "제한 날짜를 로드 합니다.");
                    var limit = conn.ExecuteScalar<int?>(tQuery);
                    if (limit.HasValue)
                    {
                        GlobalClass.s_UnUsedLimit = limit.Value;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Warning, "제한 날짜를 로드에 실패 했습니다. " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 그룹 정보를 로드 합니다.
        /// </summary>
        /// <returns>그룹 정보 로드의 성공 여부 입니다.</returns>
        private static bool LoadFACTGroupInfo()
        {
            FACTGroupInfo tCenterGroupInfo = null;
            FACTGroupInfo tBranchGroupInfo = null;
            FACTGroupInfo tORG1GroupInfo = null;
            FACTGroupInfo tGroupInfo = null;

            string tOldCenterCode = "";
            string tOldBranchCode = "";
            string tOldORG1Code = "";
            try
            {
                GlobalClass.m_FACTGroupInfo = new FACTGroupInfo();
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "FACT 그룹정보를 로드 합니다.");

                using (var conn = GlobalClass.GetSqlConnection())
                {
                    conn.Open();
                    var results = conn.Query(SQLQuery.SelectGroupInfo()).ToList();

                    if (results.Count > 0)
                    {
                        GlobalClass.m_FACTGroupInfo.ORG1Code = "0";
                        GlobalClass.m_FACTGroupInfo.ORG1Name = "전국";

                        foreach (var row in results)
                        {
                            IDictionary<string, object> dict = (IDictionary<string, object>)row;
                            tGroupInfo = new FACTGroupInfo();
                            tGroupInfo.BranchCode = (dict["org2_id"]?.ToString() ?? "").Trim();
                            tGroupInfo.BranchName = dict["org2_name"]?.ToString() ?? "";
                            tGroupInfo.CenterCode = (dict["CenterCode"]?.ToString() ?? "").Trim();
                            tGroupInfo.CenterName = dict["CenterName"]?.ToString() ?? "";
                            tGroupInfo.ORG1Code = dict["org1_id"]?.ToString() ?? "";
                            tGroupInfo.ORG1Name = dict["org1_name"]?.ToString() ?? "";

                            if (tOldORG1Code != tGroupInfo.ORG1Code)
                            {
                                tOldORG1Code = tGroupInfo.ORG1Code;
                                tORG1GroupInfo = new FACTGroupInfo();
                                tORG1GroupInfo.ORG1Code = tGroupInfo.ORG1Code;
                                tORG1GroupInfo.ORG1Name = tGroupInfo.ORG1Name;

                                if (GlobalClass.m_FACTGroupInfo.SubGroups == null)
                                    GlobalClass.m_FACTGroupInfo.SubGroups = new FACTGroupInfoCollection();
                                
                                GlobalClass.m_FACTGroupInfo.SubGroups.Add(tORG1GroupInfo);
                            }

                            if (tOldBranchCode != tGroupInfo.BranchCode)
                            {
                                tOldBranchCode = tGroupInfo.BranchCode;
                                tBranchGroupInfo = new FACTGroupInfo();
                                tBranchGroupInfo.BranchCode = tGroupInfo.BranchCode;
                                tBranchGroupInfo.BranchName = tGroupInfo.BranchName;
                                tBranchGroupInfo.ORG1Code = tGroupInfo.ORG1Code;
                                tBranchGroupInfo.ORG1Name = tGroupInfo.ORG1Name;

                                if (tORG1GroupInfo.SubGroups == null)
                                    tORG1GroupInfo.SubGroups = new FACTGroupInfoCollection();
                                
                                tORG1GroupInfo.SubGroups.Add(tBranchGroupInfo);
                            }

                            if (tOldCenterCode != tGroupInfo.CenterCode)
                            {
                                tOldCenterCode = tGroupInfo.CenterCode;
                                tCenterGroupInfo = new FACTGroupInfo();
                                tCenterGroupInfo.ORG1Code = tGroupInfo.ORG1Code;
                                tCenterGroupInfo.ORG1Name = tGroupInfo.ORG1Name;
                                tCenterGroupInfo.BranchCode = tGroupInfo.BranchCode;
                                tCenterGroupInfo.BranchName = tGroupInfo.BranchName;
                                tCenterGroupInfo.CenterCode = tGroupInfo.CenterCode;
                                tCenterGroupInfo.CenterName = tGroupInfo.CenterName;

                                if (tBranchGroupInfo.SubGroups == null)
                                    tBranchGroupInfo.SubGroups = new FACTGroupInfoCollection();
                                
                                tBranchGroupInfo.SubGroups.Add(tCenterGroupInfo);
                            }
                        }
                        
                        FactCountDevice(GlobalClass.m_FACTGroupInfo, conn);
                        return true;
                    }
                    else
                    {
                        GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "FACT 그룹 정보가 존재하지 않습니다.");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, ex.ToString());
                return false;
            }
        }

        private static int s_TotalDeviceCount = 0;
        private static void FactCountDevice(FACTGroupInfo aGroupInfo, SqlConnection conn)
        {
            try
            {
                if (aGroupInfo.SubGroups != null && aGroupInfo.SubGroups.Count > 0)
                {
                    foreach (FACTGroupInfo tGroupInfo in aGroupInfo.SubGroups)
                    {
                        FactCountDevice(tGroupInfo, conn);
                    }
                }

                if (string.IsNullOrEmpty(aGroupInfo.CenterCode)) return;

                var count = conn.ExecuteScalar<int?>("exec SP_ORG_DEVICE_Count @CenterCode", new { CenterCode = aGroupInfo.CenterCode });
                if (count.HasValue)
                {
                    aGroupInfo.DeviceCount = count.Value;
                    s_TotalDeviceCount += aGroupInfo.DeviceCount;
                }
            }
            catch (Exception) { }
        }

        /// <summary>
		/// 장비 모델 정보를 로드 합니다.
		/// </summary>
		/// <returns>장비 모델 정보 로드의 성공 여부 입니다.</returns>
		private static bool LoadModelInfo()
		{
			try
			{
				GlobalClass.m_ModelInfoCollection = new ModelInfoCollection();
				GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "모델 정보를 로드 합니다.");

				using (var conn = GlobalClass.GetSqlConnection())
				{
					conn.Open();
					string tQuery = @"select m.modeltypecode, mt.modeltypename, m.modelid, m.modelname, ms.morestring, m.SlotCount, m.Divergence, ms.moremark, 
                                      m.portcnt, mt.factmaxaccesscnt, m.uses, ec.EmbargoCmd, m.IpTypeCd 
                                      from ne_model m 
                                      left outer join ne_morestring ms on m.modelID=ms.modelID 
                                      left outer join NE_EMBAGO_CMD ec on m.modelid=ec.modelid 
                                      inner join ne_modeltype mt on m.modeltypecode=mt.modeltypecode 
                                      where mt.Uses=1 and (m.Uses = 1 OR m.Uses = 3) 
                                      order by mt.modeltypecode, m.modelid";

					var modelRows = conn.Query(tQuery).ToList();

					foreach (var row in modelRows)
					{
						IDictionary<string, object> dict = (IDictionary<string, object>)row;
						int modelID = Convert.ToInt32(dict["modelid"]);
						
						ModelInfo tModelInfo;
						lock (GlobalClass.m_ModelInfoCollection.SyncRoot)
						{
							if (GlobalClass.m_ModelInfoCollection.Contains(modelID))
							{
								tModelInfo = GlobalClass.m_ModelInfoCollection[modelID];
							}
							else
							{
								tModelInfo = new ModelInfo();
								tModelInfo.ModelID = modelID;
								tModelInfo.ModelName = dict["modelname"]?.ToString();
								tModelInfo.PortCount = Convert.ToInt32(dict["portcnt"]);
								tModelInfo.ModelTypeCode = Convert.ToInt32(dict["modeltypecode"]);
								tModelInfo.ModelTypeName = dict["modeltypename"]?.ToString();
								tModelInfo.MoreMark = dict["moremark"]?.ToString();
								tModelInfo.MoreString = dict["morestring"]?.ToString();
								tModelInfo.SlotCount = Convert.ToInt32(dict["SlotCount"]);
								tModelInfo.Divergence = Convert.ToInt32(dict["Divergence"]);
								tModelInfo.IpTypeCd = Convert.ToInt32(dict["IpTypeCd"]);
								GlobalClass.m_ModelInfoCollection.Add(tModelInfo);

                                // Load CfgRestoreCommands
                                var cfgCmds = conn.Query("EXEC SP_RACT_GET_COMMANDS @ModelID, @PartID", 
                                                        new { ModelID = modelID, PartID = (int)E_CommandPart.ConfigBRRestore });
                                foreach (var cfgRow in cfgCmds)
                                {
                                    IDictionary<string, object> cDict = (IDictionary<string, object>)cfgRow;
                                    var cmd = new CfgRestoreCommand
                                    {
                                        CmdSeq = Convert.ToInt32(cDict["CmdSeq"]),
                                        Cmd = cDict["Cmd"]?.ToString(),
                                        T_Prompt = cDict["T_Prompt"]?.ToString()
                                    };
                                    tModelInfo.CfgRestoreCommands.Add(cmd);
                                }

                                // Load DefaultConnectionCommadSet
                                string defQuery = @"select cmd_cmd.* 
                                                    from cmd_cmd WITH(NOLOCK),
                                                    cmd_cmdset cmdset WITH(NOLOCK)
                                                    inner join CMD_CMDMASTER cmdmaster WITH(NOLOCK) on cmdmaster.cmdsetid = cmdset.cmdsetid
                                                    where cmdset.cmdpartid = 65
                                                    and cmdset.modelid = @ModelID 
                                                    and cmdmaster.cmdgrpid = cmd_cmd.cmdgrpid";
                                var defCmds = conn.Query(defQuery, new { ModelID = modelID });
                                foreach (var defRow in defCmds)
                                {
                                    IDictionary<string, object> dDict = (IDictionary<string, object>)defRow;
                                    var cmd = new FACT_DefaultConnectionCommand
                                    {
                                        CMDSeq = Convert.ToInt32(dDict["cmdseq"]),
                                        CMD = dDict["cmd"]?.ToString(),
                                        Prompt = dDict["t_prompt"]?.ToString(),
                                        ErrorString = dDict["t_ErrStr"]?.ToString()
                                    };
                                    tModelInfo.DefaultConnectionCommadSet.CommandList.Add(cmd);
                                }
							}
						}

						if (dict.ContainsKey("EmbargoCmd") && dict["EmbargoCmd"] != DBNull.Value && dict["EmbargoCmd"] != null)
						{
							tModelInfo.EmbagoCmd.Add(dict["EmbargoCmd"].ToString());
						}
					}
				}
				return true;
			}
			catch (Exception ex)
			{
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Warning, "모델 정보 로드에 실패 했습니다. " + ex.Message);
				return false;
			}
		}


        // 15-09-10
        // 제한 명령어 정보를 수집합니다.
        // Gunny
        public static bool LoadLimitCmdInfo(E_UserType tUserType)
        {
            try
            {
                GlobalClass.m_LimitCmdInfoCollection = new LimitCmdInfoCollection();
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "제한 명령어를 로드 합니다.");

                using (var conn = GlobalClass.GetSqlConnection())
                {
                    string tQuery = "SP_RACT_Get_EmbargoCmdInfo @UserType";
                    var results = conn.Query(tQuery, new { UserType = (int)tUserType });

                    foreach (var row in results)
                    {
                        IDictionary<string, object> dict = (IDictionary<string, object>)row;
                        int modelID = Convert.ToInt32(dict["ModelID"]);
                        
                        LimitCmdInfo tLimitCmdInfo;
                        lock (GlobalClass.m_LimitCmdInfoCollection.SyncRoot)
                        {
                            if (GlobalClass.m_LimitCmdInfoCollection.Contains(modelID))
                            {
                                tLimitCmdInfo = GlobalClass.m_LimitCmdInfoCollection[modelID];
                            }
                            else
                            {
                                tLimitCmdInfo = new LimitCmdInfo { ModelID = modelID, EmbargoID = Convert.ToInt32(dict["EmbargoID"]) };
                                GlobalClass.m_LimitCmdInfoCollection.Add(tLimitCmdInfo);
                            }
                        }

                        if (dict["EmbargoCmd"] != DBNull.Value && dict["EmbargoCmd"] != null)
                        {
                            var tEmbagoInfo = new EmbagoInfo
                            {
                                Embargo = dict["EmbargoCmd"].ToString(),
                                EmbargoEnble = Convert.ToBoolean(dict["mAdmin"])
                            };
                            tLimitCmdInfo.EmbagoCmd.Add(tEmbagoInfo);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Warning, "제한 명령어 로드에 실패 했습니다. " + ex.Message);
                return false;
            }
        }

        // 15-09-30
        // 기본 명령어 정보를 수집합니다.
        // Gunny
        public static bool LoadDefaultCmdInfo()
        {
            try
            {
                GlobalClass.m_DefaultCmdInfoCollection = new DefaultCmdInfoCollection();
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "장비별 기본 명령어를 로드 합니다.");

                using (var conn = GlobalClass.GetSqlConnection())
                {
                    string tQuery = "SELECT ID , ModelID , Command , Description , UserID from fact_main.dbo.RACT_AutoCommandGuide order by ModelID , ID";
                    var results = conn.Query(tQuery);

                    foreach (var row in results)
                    {
                        IDictionary<string, object> dict = (IDictionary<string, object>)row;
                        int modelID = Convert.ToInt32(dict["ModelID"]);

                        DefaultCmdInfo tDefaultCmdInfo;
                        lock (GlobalClass.m_DefaultCmdInfoCollection.SyncRoot)
                        {
                            if (GlobalClass.m_DefaultCmdInfoCollection.Contains(modelID))
                            {
                                tDefaultCmdInfo = GlobalClass.m_DefaultCmdInfoCollection[modelID];
                            }
                            else
                            {
                                tDefaultCmdInfo = new DefaultCmdInfo { ModelID = modelID, EmbargoID = Convert.ToInt32(dict["ID"]) };
                                GlobalClass.m_DefaultCmdInfoCollection.Add(tDefaultCmdInfo);
                            }
                        }

                        if (dict["Command"] != DBNull.Value && dict["Command"] != null)
                        {
                            tDefaultCmdInfo.Command.Add(dict["Command"].ToString());
                            tDefaultCmdInfo.Description.Add(dict["Description"]?.ToString());
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Warning, "기본 명령어 로드에 실패 했습니다. " + ex.Message);
                return false;
            }
        }

        // 15-09-30
        // 기본 명령어 정보를 수집합니다.
        // Gunny
        public static bool LoadAutoCompleteInfo(int userID)
        {
            try
            {
                GlobalClass.m_AutoCompleteCmdInfoCollection = new AutoCompleteCmdInfoCollection();
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "자동완성을 로드 합니다.");

                using (var conn = GlobalClass.GetSqlConnection())
                {
                    string tQuery = @"SELECT distinct 
                                    LTRIM(RTRIM(A.[Command])) as Command, B.UserID, C.ModelID 
                                    FROM [FACT_MAIN].[dbo].[RACT_Log_ExcuteCommand] A 
                                    Inner Join [FACT_MAIN].dbo.RACT_LOG_DeviceConnection B On A.ConnectionLogId= B.ID 
                                    Inner Join [FACT_MAIN].dbo.Ne_NE C On B.NEID = C.NEID 
                                    where B.UserID=@UserID 
                                    order by Command desc";

                    var results = conn.Query(tQuery, new { UserID = userID });

                    foreach (var row in results)
                    {
                        IDictionary<string, object> dict = (IDictionary<string, object>)row;
                        int modelID = Convert.ToInt32(dict["ModelID"]);

                        AutoCompleteCmdInfo tAutoCompleteCmdInfo;
                        lock (GlobalClass.m_AutoCompleteCmdInfoCollection.SyncRoot)
                        {
                            if (GlobalClass.m_AutoCompleteCmdInfoCollection.Contains(modelID))
                            {
                                tAutoCompleteCmdInfo = GlobalClass.m_AutoCompleteCmdInfoCollection[modelID];
                            }
                            else
                            {
                                tAutoCompleteCmdInfo = new AutoCompleteCmdInfo { ModelID = modelID };
                                GlobalClass.m_AutoCompleteCmdInfoCollection.Add(tAutoCompleteCmdInfo);
                            }
                        }

                        if (dict["Command"] != DBNull.Value && dict["Command"] != null)
                        {
                            tAutoCompleteCmdInfo.Command.Add(dict["Command"].ToString());
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Warning, "자동완성을 로드하는데 실패 했습니다. " + ex.Message);
                return false;
            }
        }
    }
}
