using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ACPS.CommonConfigCompareClass;
using RACTCommonClass;
using Dapper;
using System.Data.SqlClient;
using System.Linq;
using RACTServer.Data;

namespace RACTServer
{
    /// <summary>
    /// .NET 10 기반 기초 데이터 로드 프로세스 (SqlStatements 활용)
    /// </summary>
    public class BaseDataLoadProcess
    {
        public static bool LoadBaseData()
        {
            GlobalClass.m_LogProcess?.PrintLog(E_FileLogType.Infomation, "기초 데이터를 로드 합니다.");
            // 동기 호환성을 위해 Task.Run().GetAwaiter().GetResult() 사용
            return LoadFACTGroupInfo() && LoadModelInfo() && LoadUnUsedLimit() && LoadDefaultCmdInfoAsync().GetAwaiter().GetResult();
        }

        private static bool LoadUnUsedLimit()
        {
            try
            {
                using var conn = GlobalClass.GetSqlConnection();
                var limit = conn.ExecuteScalar<int?>(SqlStatements.System.SelectUnUsedLimit);
                if (limit.HasValue) GlobalClass.s_UnUsedLimit = limit.Value;
                return true;
            }
            catch (Exception ex)
            {
                GlobalClass.m_LogProcess?.PrintLog(E_FileLogType.Warning, "제한 날짜 로드 실패: " + ex.Message);
                return false;
            }
        }

        private static bool LoadFACTGroupInfo()
        {
            try
            {
                GlobalClass.m_FACTGroupInfo = new FACTGroupInfo { ORG1Code = "0", ORG1Name = "전국" };
                using var conn = GlobalClass.GetSqlConnection();
                conn.Open();
                var results = conn.Query(SqlStatements.Group.SelectGroupInfo).ToList();

                string tOldCenterCode = "", tOldBranchCode = "", tOldORG1Code = "";
                FACTGroupInfo? tCenterGroupInfo = null, tBranchGroupInfo = null, tORG1GroupInfo = null;

                foreach (var row in results)
                {
                    var dict = (IDictionary<string, object>)row;
                    var branchCode = (dict["org2_id"]?.ToString() ?? "").Trim();
                    var centerCode = (dict["CenterCode"]?.ToString() ?? "").Trim();
                    var org1Code = dict["org1_id"]?.ToString() ?? "";

                    if (tOldORG1Code != org1Code)
                    {
                        tOldORG1Code = org1Code;
                        tORG1GroupInfo = new FACTGroupInfo { ORG1Code = org1Code, ORG1Name = dict["org1_name"]?.ToString() ?? "" };
                        GlobalClass.m_FACTGroupInfo.SubGroups ??= new();
                        GlobalClass.m_FACTGroupInfo.SubGroups.Add(tORG1GroupInfo);
                    }

                    if (tOldBranchCode != branchCode)
                    {
                        tOldBranchCode = branchCode;
                        tBranchGroupInfo = new FACTGroupInfo { BranchCode = branchCode, BranchName = dict["org2_name"]?.ToString() ?? "", ORG1Code = org1Code };
                        tORG1GroupInfo!.SubGroups ??= new();
                        tORG1GroupInfo.SubGroups.Add(tBranchGroupInfo);
                    }

                    if (tOldCenterCode != centerCode)
                    {
                        tOldCenterCode = centerCode;
                        tCenterGroupInfo = new FACTGroupInfo { CenterCode = centerCode, CenterName = dict["CenterName"]?.ToString() ?? "", BranchCode = branchCode, ORG1Code = org1Code };
                        tBranchGroupInfo!.SubGroups ??= new();
                        tBranchGroupInfo.SubGroups.Add(tCenterGroupInfo);
                    }
                }
                return true;
            }
            catch (Exception ex) { GlobalClass.m_LogProcess?.PrintLog(E_FileLogType.Error, "LoadFACTGroupInfo Error: " + ex.Message); return false; }
        }

        private static bool LoadModelInfo()
        {
            try
            {
                GlobalClass.m_ModelInfoCollection = new ModelInfoCollection();
                using var conn = GlobalClass.GetSqlConnection();
                conn.Open();
                var modelRows = conn.Query(SqlStatements.System.SelectModelInfo).ToList();

                foreach (var row in modelRows)
                {
                    var dict = (IDictionary<string, object>)row;
                    int modelID = Convert.ToInt32(dict["modelid"]);
                    
                    if (!GlobalClass.m_ModelInfoCollection.Contains(modelID))
                    {
                        var info = new ModelInfo {
                            ModelID = modelID, ModelName = dict["modelname"]?.ToString(),
                            PortCount = Convert.ToInt32(dict["portcnt"]), ModelTypeCode = Convert.ToInt32(dict["modeltypecode"]),
                            ModelTypeName = dict["modeltypename"]?.ToString(), MoreMark = dict["moremark"]?.ToString(),
                            MoreString = dict["morestring"]?.ToString(), SlotCount = Convert.ToInt32(dict["SlotCount"]),
                            Divergence = Convert.ToInt32(dict["Divergence"]), IpTypeCd = Convert.ToInt32(dict["IpTypeCd"])
                        };
                        GlobalClass.m_ModelInfoCollection.Add(info);
                    }
                }
                return true;
            }
            catch (Exception ex) { GlobalClass.m_LogProcess?.PrintLog(E_FileLogType.Error, "LoadModelInfo Error: " + ex.Message); return false; }
        }

        public static async Task<bool> LoadLimitCmdInfoAsync(E_UserType userType)
        {
            try
            {
                GlobalClass.m_LimitCmdInfoCollection = new();
                using var conn = GlobalClass.GetSqlConnection();
                var results = await conn.QueryAsync("EXEC SP_RACT_Get_EmbargoCmdInfo @UserType", new { UserType = (int)userType });

                foreach (var row in results)
                {
                    var dict = (IDictionary<string, object>)row;
                    int modelID = Convert.ToInt32(dict["ModelID"]);
                    if (!GlobalClass.m_LimitCmdInfoCollection.Contains(modelID))
                        GlobalClass.m_LimitCmdInfoCollection.Add(new LimitCmdInfo { ModelID = modelID, EmbargoID = Convert.ToInt32(dict["EmbargoID"]) });
                    
                    if (dict["EmbargoCmd"] != null)
                        GlobalClass.m_LimitCmdInfoCollection[modelID].EmbagoCmd.Add(new EmbagoInfo { Embargo = dict["EmbargoCmd"].ToString()!, EmbargoEnble = Convert.ToBoolean(dict["mAdmin"]) });
                }
                return true;
            }
            catch { return false; }
        }

        public static async Task<bool> LoadDefaultCmdInfoAsync()
        {
            try
            {
                GlobalClass.m_DefaultCmdInfoCollection = new();
                using var conn = GlobalClass.GetSqlConnection();
                var results = await conn.QueryAsync("SELECT ID, ModelID, Command, Description FROM fact_main.dbo.RACT_AutoCommandGuide ORDER BY ModelID, ID");
                foreach (var row in results)
                {
                    var dict = (IDictionary<string, object>)row;
                    int mID = Convert.ToInt32(dict["ModelID"]);
                    if (!GlobalClass.m_DefaultCmdInfoCollection.Contains(mID))
                        GlobalClass.m_DefaultCmdInfoCollection.Add(new DefaultCmdInfo { ModelID = mID, EmbargoID = Convert.ToInt32(dict["ID"]) });
                    GlobalClass.m_DefaultCmdInfoCollection[mID].Command.Add(dict["Command"]!.ToString()!);
                }
                return true;
            }
            catch { return false; }
        }

        public static async Task<bool> LoadAutoCompleteInfoAsync(int userID)
        {
            try
            {
                GlobalClass.m_AutoCompleteCmdInfoCollection = new();
                using var conn = GlobalClass.GetSqlConnection();
                var results = await conn.QueryAsync("SELECT distinct LTRIM(RTRIM(A.[Command])) as Command, C.ModelID FROM [FACT_MAIN].[dbo].[RACT_Log_ExcuteCommand] A Inner Join [FACT_MAIN].dbo.RACT_LOG_DeviceConnection B On A.ConnectionLogId= B.ID Inner Join [FACT_MAIN].dbo.Ne_NE C On B.NEID = C.NEID WHERE B.UserID=@UserID ORDER BY Command DESC", new { UserID = userID });
                foreach (var row in results)
                {
                    var dict = (IDictionary<string, object>)row;
                    int mID = Convert.ToInt32(dict["ModelID"]);
                    if (!GlobalClass.m_AutoCompleteCmdInfoCollection.Contains(mID))
                        GlobalClass.m_AutoCompleteCmdInfoCollection.Add(new AutoCompleteCmdInfo { ModelID = mID });
                    GlobalClass.m_AutoCompleteCmdInfoCollection[mID].Command.Add(dict["Command"]!.ToString()!);
                }
                return true;
            }
            catch { return false; }
        }
    }
}
