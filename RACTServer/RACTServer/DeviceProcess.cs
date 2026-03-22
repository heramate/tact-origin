using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RACTCommonClass;
using Dapper;

namespace RACTServer
{
    /// <summary>
    /// .NET 10 기반 고성능 장비 처리 프로세스 (Repository 패턴 적용)
    /// </summary>
    public class DeviceProcess
    {
        internal static async Task RequestProcessAsync(RequestCommunicationData aClientRequest)
        {
            DeviceRequestInfo tRequesetInfo = aClientRequest.RequestData as DeviceRequestInfo;

            switch (tRequesetInfo.WorkType)
            {
                case E_WorkType.Search:
                    await DeviceInfoReceiverAsync(aClientRequest);
                    break;
                default:
                    await ModifyDeviceInfoAsync(aClientRequest);
                    break;
            }
        }

        public static async Task DeviceInfoReceiverAsync(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tUserInfo = GlobalClass.m_ClientProcess.GetUserInfo(aClientRequest.ClientID);
                var results = await GlobalClass.DeviceRepo.GetDeviceInfoAsync(tUserInfo.GetCenterCode);
                var tDeviceInfoCollection = MapToDeviceInfoCollection(results);

                if (tDeviceInfoCollection.Count > 0) 
                    tResultData.ResultData = GlobalClass.ObjectCompress(tDeviceInfoCollection);
            }
            catch (Exception ex)
            {
                tResultData.Error.Error = E_ErrorType.UnKnownError;
                tResultData.Error.ErrorString = ex.Message;
            }

            GlobalClass.SendResultClient(tResultData);
        }

        private static DeviceInfoCollection MapToDeviceInfoCollection(IEnumerable<dynamic> results)
        {
            var collection = new DeviceInfoCollection();
            if (results == null) return collection;

            foreach (var row in results)
            {
                IDictionary<string, object> dict = (IDictionary<string, object>)row;
                var tDeviceInfo = new DeviceInfo
                {
                    DeviceID = Convert.ToInt32(dict["NeID"]),
                    ModelID = Convert.ToInt32(dict["ModelID"]),
                    Name = dict["NeName"]?.ToString(),
                    ORG1Code = dict["org1_id"]?.ToString(),
                    ORG2Code = dict["org2_id"]?.ToString(),
                    BranchCode = dict["org2_id"]?.ToString(),
                    CenterCode = dict["CenterCode"]?.ToString(),
                    IPAddress = dict["MasterIP"]?.ToString(),
                    InputFlag = dict.ContainsKey("InputFlag") && dict["InputFlag"] != DBNull.Value ? (E_FlagType)(Convert.ToBoolean(dict["InputFlag"]).GetHashCode()) : E_FlagType.FORMS,
                    DeviceNumber = dict["devicenum"]?.ToString(),
                    DevicePartCode = dict.ContainsKey("ModelTypeCode") ? Convert.ToInt32(dict["ModelTypeCode"]) : 0,
                    Version = dict["OsVersion"]?.ToString(),
                    TelnetID1 = dict["TelnetID_1"]?.ToString().Trim(),
                    TelnetID2 = dict["TelnetID_2"]?.ToString().Trim(),
                    TelnetPwd1 = dict["Passwd_1"]?.ToString().Trim(),
                    TelnetPwd2 = dict["Passwd_2"]?.ToString().Trim(),
                    ORG1Name = dict.ContainsKey("ORG1_Name") ? dict["ORG1_Name"]?.ToString() : "",
                    ORG2Name = dict.ContainsKey("ORG2_Name") ? dict["ORG2_Name"]?.ToString() : "",
                    TpoName = dict.ContainsKey("TpoName") ? dict["TpoName"]?.ToString() : "",
                    CenterName = dict.ContainsKey("BizPlsName") ? dict["BizPlsName"]?.ToString() : (dict.ContainsKey("CenterName") ? dict["CenterName"]?.ToString() : ""),
                    ModelName = dict.ContainsKey("ModelName") ? dict["ModelName"]?.ToString() : "",
                    MangTypeCd = dict.ContainsKey("MangTypeCd") ? dict["MangTypeCd"]?.ToString() : "",
                    DeviceGroupName = dict.ContainsKey("GroupName") ? dict["GroupName"]?.ToString() : ""
                };

                string gID = dict["GroupID"]?.ToString();
                tDeviceInfo.GroupID = string.IsNullOrEmpty(gID) ? "-1" : gID;
                tDeviceInfo.TerminalConnectInfo.IPAddress = tDeviceInfo.IPAddress;

                if (dict.ContainsKey("DeviceType")) tDeviceInfo.DeviceType = (E_DeviceType)Convert.ToInt32(dict["DeviceType"]);
                if (dict.ContainsKey("WAIT1")) tDeviceInfo.WAIT = dict["WAIT1"]?.ToString();
                if (dict.ContainsKey("USERID1")) tDeviceInfo.USERID = dict["USERID1"]?.ToString();
                if (dict.ContainsKey("PWD1")) tDeviceInfo.PWD = dict["PWD1"]?.ToString();
                if (dict.ContainsKey("USERID2")) tDeviceInfo.USERID2 = dict["USERID2"]?.ToString();
                if (dict.ContainsKey("PWD2")) tDeviceInfo.PWD2 = dict["PWD2"]?.ToString();

                collection.Add(tDeviceInfo);
            }
            return collection;
        }

        private static async Task ModifyDeviceInfoAsync(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tDeviceRequestInfo = (DeviceRequestInfo)aClientRequest.RequestData;
                await GlobalClass.DeviceRepo.ExecuteModifyDeviceAsync(tDeviceRequestInfo.WorkType, tDeviceRequestInfo.UserID, tDeviceRequestInfo.DeviceInfo);

                tResultData.ResultData = tDeviceRequestInfo.DeviceInfo;
                GlobalClass.SendResultClient(tResultData);
            }
            catch (Exception ex)
            {
                tResultData.Error.Error = E_ErrorType.UnKnownError;
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, "ModifyDeviceInfoAsync Error: " + ex.Message);
                GlobalClass.SendResultClient(tResultData);
            }
        }

        internal static async Task RequestBatchRegisterationAsync(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tDeviceCollectionRequestInfo = (DeviceCollectionRequestInfo)aClientRequest.RequestData;
                foreach (DeviceInfo tDeviceInfo in tDeviceCollectionRequestInfo.DeviceInfoList)
                {
                    tDeviceInfo.GroupID = tDeviceCollectionRequestInfo.GroupID;
                    tDeviceInfo.TerminalConnectInfo = tDeviceCollectionRequestInfo.ConnectionInfo;
                    await GlobalClass.DeviceRepo.ExecuteModifyDeviceAsync(tDeviceCollectionRequestInfo.WorkType, tDeviceCollectionRequestInfo.UserID, tDeviceInfo);
                }
                tResultData.ResultData = tDeviceCollectionRequestInfo.DeviceInfoList;
                GlobalClass.SendResultClient(tResultData);
            }
            catch (Exception ex)
            {
                tResultData.Error.Error = E_ErrorType.UnKnownError;
                GlobalClass.SendResultClient(tResultData);
            }
        }

        internal static async Task RequestFactDeviceSearchProcessAsync(RequestCommunicationData aClientRequest, bool GetMangKb = false)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tUserInfo = GlobalClass.m_ClientProcess.GetUserInfo(aClientRequest.ClientID);
                DeviceSearchInfo tSearchInfo = aClientRequest.RequestData as DeviceSearchInfo;

                string tCenterCode = tSearchInfo.IsCheckPermission ? tUserInfo.GetCenterCode : "";
                string tMangTypeCd = GetMangKb ? "" : tUserInfo.GetMangType;

                var results = await GlobalClass.DeviceRepo.SearchDeviceInfoAsync(tCenterCode, tSearchInfo, tMangTypeCd);
                var tDeviceInfoCollection = MapToDeviceInfoCollection(results);
                
                if (tDeviceInfoCollection.Count == 0) tDeviceInfoCollection.Add(new DeviceInfo());
                tResultData.ResultData = GlobalClass.ObjectCompress(tDeviceInfoCollection);
            }
            catch (Exception ex)
            {
                tResultData.Error.Error = E_ErrorType.UnKnownError;
                tResultData.Error.ErrorString = ex.Message;
            }

            GlobalClass.SendResultClient(tResultData);
        }

        internal static async Task RequestSearchDeviceForTypeAsync(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                string[] tArrRequest = aClientRequest.RequestData as string[];
                using var conn = GlobalClass.GetSqlConnection();
                var results = await conn.QueryAsync("EXEC SP_RACT_SEARCH_DEVICE_FOR_TYPE @Type,@IP,@UserID", new { Type = tArrRequest[0], IP = tArrRequest[1], UserID = tArrRequest[2] });
                tResultData.ResultData = GlobalClass.ObjectCompress(MapToDeviceInfoCollection(results));
            }
            catch (Exception ex)
            {
                tResultData.Error.Error = E_ErrorType.UnKnownError;
                tResultData.Error.ErrorString = ex.Message;
            }
            GlobalClass.SendResultClient(tResultData);
        }

        internal static async Task RequestSearchRMSCMTSDeviceAsync(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                DeviceSearchInfo tSearchInfo = aClientRequest.RequestData as DeviceSearchInfo;
                using var conn = GlobalClass.GetSqlConnection();
                var row = await conn.QueryFirstOrDefaultAsync("SELECT TOP 1 lgname, lgpwd FROM [FACT_MAIN].[dbo].[FOMS_RMS_CMTS_DEVICE] WHERE cmtsid = @IP", new { IP = tSearchInfo.DeviceIPAddress });

                var tDeviceInfoCollection = new DeviceInfoCollection();
                var tDeviceInfo = new DeviceInfo { IPAddress = tSearchInfo.DeviceIPAddress };
                if (row != null)
                {
                    tDeviceInfo.TelnetID1 = row.lgname?.ToString().Trim();
                    tDeviceInfo.TelnetPwd1 = row.lgpwd?.ToString().Trim();
                }
                tDeviceInfoCollection.Add(tDeviceInfo);
                tResultData.ResultData = GlobalClass.ObjectCompress(tDeviceInfoCollection);
            }
            catch (Exception ex)
            {
                tResultData.Error.Error = E_ErrorType.UnKnownError;
                tResultData.Error.ErrorString = ex.Message;
            }
            GlobalClass.SendResultClient(tResultData);
        }

        internal static async Task RequestFactIPDeviceSearchProcessAsync(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                string tData = aClientRequest.RequestData as string;
                string[] IPDeviceSearch = tData.Split('|');
                using var conn = GlobalClass.GetSqlConnection();
                var results = await conn.QueryAsync("EXEC SP_RACT_GET_SearchDEVICEINFO_IPList @IPList, @IPType", new { IPList = IPDeviceSearch[1], IPType = IPDeviceSearch[0] });
                tResultData.ResultData = GlobalClass.ObjectCompress(MapToDeviceInfoCollection(results));
            }
            catch (Exception ex)
            {
                tResultData.Error.Error = E_ErrorType.UnKnownError;
                tResultData.Error.ErrorString = ex.Message;
            }
            GlobalClass.SendResultClient(tResultData);
        }
    }
}
