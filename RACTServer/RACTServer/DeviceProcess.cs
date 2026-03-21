using System;
using System.Collections.Generic;
using System.Linq;
using RACTCommonClass;
using Dapper;

namespace RACTServer
{
    public class DeviceProcess
    {
        /// <summary>
        /// 요청을 처리 합니다.
        /// </summary>
        /// <param name="aClientRequest"></param>
        internal static void RequestProcess(RequestCommunicationData aClientRequest)
        {
            DeviceRequestInfo tRequesetInfo = aClientRequest.RequestData as DeviceRequestInfo;

            switch (tRequesetInfo.WorkType)
            {
                case E_WorkType.Search:
                    DeviceInfoReceiver(aClientRequest);
                    break;
                default:
                    ModifyDeviceInfo(aClientRequest);
                    break;
            }
        }

        /// <summary>
        /// 장비 정보를 전송할 메서드 입니다.
        /// </summary>
        /// <param name="aClientRequest"></param>
        public static void DeviceInfoReceiver(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tUserInfo = GlobalClass.m_ClientProcess.GetUserInfo(aClientRequest.ClientID);

                using (var conn = GlobalClass.GetSqlConnection())
                {
                    var results = conn.Query("EXEC SP_RACT_GET_DEVICEINFO @CenterCode", new { CenterCode = tUserInfo.GetCenterCode });
                    var tDeviceInfoCollection = MapToDeviceInfoCollection(results);

                    if (tDeviceInfoCollection.Count > 0) 
                        tResultData.ResultData = GlobalClass.ObjectCompress(tDeviceInfoCollection);
                }
            }
            catch (Exception ex)
            {
                tResultData.Error.Error = E_ErrorType.UnKnownError;
                tResultData.Error.ErrorString = ex.Message;
            }

            GlobalClass.SendResultClient(tResultData);
        }

        /// <summary>
        /// dynamic 리스트를 DeviceInfoCollection으로 변환합니다.
        /// </summary>
        private static DeviceInfoCollection MapToDeviceInfoCollection(IEnumerable<dynamic> results)
        {
            var collection = new DeviceInfoCollection();
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
                    InputFlag = dict.ContainsKey("InputFlag") && dict["InputFlag"] != DBNull.Value ? (E_FlagType)(Convert.ToBoolean(dict["InputFlag"]).GetHashCode()) : E_FlagType.Off,
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

                if (dict.ContainsKey("DeviceType"))
                {
                   tDeviceInfo.DeviceType = (E_DeviceType)Convert.ToInt32(dict["DeviceType"]);
                }
                
                if (dict.ContainsKey("WAIT1")) tDeviceInfo.WAIT = dict["WAIT1"]?.ToString();
                if (dict.ContainsKey("USERID1")) tDeviceInfo.USERID = dict["USERID1"]?.ToString();
                if (dict.ContainsKey("PWD1")) tDeviceInfo.PWD = dict["PWD1"]?.ToString();
                if (dict.ContainsKey("USERID2")) tDeviceInfo.USERID2 = dict["USERID2"]?.ToString();
                if (dict.ContainsKey("PWD2")) tDeviceInfo.PWD2 = dict["PWD2"]?.ToString();

                collection.Add(tDeviceInfo);
            }
            return collection;
        }

        /// <summary>
        /// 단일 장비를 수정합니다.
        /// </summary>
        /// <param name="aClientRequest"></param>
        private static void ModifyDeviceInfo(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tDeviceRequestInfo = (DeviceRequestInfo)aClientRequest.RequestData;
                var tDeviceInfo = tDeviceRequestInfo.DeviceInfo;

                using (var conn = GlobalClass.GetSqlConnection())
                {
                    conn.Open();
                    ExecuteModifyDevice(tDeviceRequestInfo.WorkType, tDeviceRequestInfo.UserID, tDeviceInfo, conn);
                }

                tResultData.ResultData = tDeviceInfo;
                GlobalClass.SendResultClient(tResultData);
            }
            catch (Exception ex)
            {
                tResultData.Error.Error = E_ErrorType.UnKnownError;
                GlobalClass.SendResultClient(tResultData);
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 일괄 장비 등록 기능을 수행합니다.
        /// </summary>
        internal static void RequestBatchRegisteration(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tDeviceCollectionRequestInfo = (DeviceCollectionRequestInfo)aClientRequest.RequestData;
                var tDeviceInfoList = tDeviceCollectionRequestInfo.DeviceInfoList;

                using (var conn = GlobalClass.GetSqlConnection())
                {
                    conn.Open();
                    foreach (DeviceInfo tDeviceInfo in tDeviceInfoList)
                    {
                        tDeviceInfo.GroupID = tDeviceCollectionRequestInfo.GroupID;
                        tDeviceInfo.TerminalConnectInfo = tDeviceCollectionRequestInfo.ConnectionInfo;
                        ExecuteModifyDevice(tDeviceCollectionRequestInfo.WorkType, tDeviceCollectionRequestInfo.UserID, tDeviceInfo, conn);
                    }
                }
                tResultData.ResultData = tDeviceInfoList;
                GlobalClass.SendResultClient(tResultData);
            }
            catch (Exception ex)
            {
                tResultData.Error.Error = E_ErrorType.UnKnownError;
                GlobalClass.SendResultClient(tResultData);
                Console.WriteLine(ex.Message);
            }
        }

        private static void ExecuteModifyDevice(E_WorkType aWorkType, int aUserID, DeviceInfo aDeviceInfo, System.Data.SqlClient.SqlConnection conn)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@WorkType", (int)aWorkType);
                parameters.Add("@GroupID", aDeviceInfo.GroupID);
                parameters.Add("@UserID", aUserID);
                parameters.Add("@DeviceID", aDeviceInfo.DeviceID);
                parameters.Add("@Protocol", (int)aDeviceInfo.TerminalConnectInfo.ConnectionProtocol);
                parameters.Add("@DeviceType", (int)aDeviceInfo.DeviceType);
                parameters.Add("@TelnetPort", aDeviceInfo.TerminalConnectInfo.TelnetPort);
                parameters.Add("@PortName", aDeviceInfo.TerminalConnectInfo.SerialConfig.PortName);
                parameters.Add("@BaudRate", aDeviceInfo.TerminalConnectInfo.SerialConfig.BaudRate);
                parameters.Add("@DataBits", aDeviceInfo.TerminalConnectInfo.SerialConfig.DataBits);
                parameters.Add("@Parity", (int)aDeviceInfo.TerminalConnectInfo.SerialConfig.Parity);
                parameters.Add("@StopBits", (int)aDeviceInfo.TerminalConnectInfo.SerialConfig.StopBits);
                parameters.Add("@Handshake", (int)aDeviceInfo.TerminalConnectInfo.SerialConfig.Handshake);
                
                if (aDeviceInfo.DeviceType == E_DeviceType.NeGroup)
                {
                    conn.Execute("EXEC SP_RACT_MODIFY_DEVICEINFO @WorkType,@GroupID,@UserID,@DeviceID,@Protocol,@DeviceType,@TelnetPort,@PortName,@BaudRate,@DataBits,@Parity,@StopBits,@Handshake,'','','','','','','','','','','','','',@MoreString,@MoreMark", 
                        new { WorkType = (int)aWorkType, GroupID = aDeviceInfo.GroupID, UserID = aUserID, DeviceID = aDeviceInfo.DeviceID, 
                              Protocol = (int)aDeviceInfo.TerminalConnectInfo.ConnectionProtocol, DeviceType = (int)aDeviceInfo.DeviceType, 
                              TelnetPort = aDeviceInfo.TerminalConnectInfo.TelnetPort, PortName = aDeviceInfo.TerminalConnectInfo.SerialConfig.PortName, 
                              BaudRate = aDeviceInfo.TerminalConnectInfo.SerialConfig.BaudRate, DataBits = aDeviceInfo.TerminalConnectInfo.SerialConfig.DataBits, 
                              Parity = (int)aDeviceInfo.TerminalConnectInfo.SerialConfig.Parity, StopBits = (int)aDeviceInfo.TerminalConnectInfo.SerialConfig.StopBits, 
                              Handshake = (int)aDeviceInfo.TerminalConnectInfo.SerialConfig.Handshake, MoreString = aDeviceInfo.MoreString, MoreMark = aDeviceInfo.MoreMark });
                }
                else
                {
                    conn.Execute("EXEC SP_RACT_MODIFY_DEVICEINFO @WorkType,@GroupID,@UserID,@DeviceID,@Protocol,@DeviceType,@TelnetPort,@PortName,@BaudRate,@DataBits,@Parity,@StopBits,@Handshake,@ModelName,@IPAddress,@TelnetID1,@TelnetPwd1,@TelnetID2,@TelnetPwd2,@Name,@TpoName,@WAIT,@USERID,@PWD,@USERID2,@PWD2,@MoreString,@MoreMark", 
                        new { WorkType = (int)aWorkType, GroupID = aDeviceInfo.GroupID, UserID = aUserID, DeviceID = aDeviceInfo.DeviceID, 
                              Protocol = (int)aDeviceInfo.TerminalConnectInfo.ConnectionProtocol, DeviceType = (int)aDeviceInfo.DeviceType, 
                              TelnetPort = aDeviceInfo.TerminalConnectInfo.TelnetPort, PortName = aDeviceInfo.TerminalConnectInfo.SerialConfig.PortName, 
                              BaudRate = aDeviceInfo.TerminalConnectInfo.SerialConfig.BaudRate, DataBits = aDeviceInfo.TerminalConnectInfo.SerialConfig.DataBits, 
                              Parity = (int)aDeviceInfo.TerminalConnectInfo.SerialConfig.Parity, StopBits = (int)aDeviceInfo.TerminalConnectInfo.SerialConfig.StopBits, 
                              Handshake = (int)aDeviceInfo.TerminalConnectInfo.SerialConfig.Handshake, ModelName = aDeviceInfo.ModelName, 
                              IPAddress = aDeviceInfo.IPAddress, TelnetID1 = aDeviceInfo.TelnetID1, TelnetPwd1 = aDeviceInfo.TelnetPwd1, 
                              TelnetID2 = aDeviceInfo.TelnetID2, TelnetPwd2 = aDeviceInfo.TelnetPwd2, Name = aDeviceInfo.Name, 
                              TpoName = aDeviceInfo.TpoName, WAIT = aDeviceInfo.WAIT, USERID = aDeviceInfo.USERID, 
                              PWD = aDeviceInfo.PWD, USERID2 = aDeviceInfo.USERID2, PWD2 = aDeviceInfo.PWD2, 
                              MoreString = aDeviceInfo.MoreString, MoreMark = aDeviceInfo.MoreMark });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        internal static void RequestFactDeviceSearchProcess(RequestCommunicationData aClientRequest, bool GetMangKb = false)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tUserInfo = GlobalClass.m_ClientProcess.GetUserInfo(aClientRequest.ClientID);
                DeviceSearchInfo tSearchInfo = aClientRequest.RequestData as DeviceSearchInfo;

                string tCenterCode = tSearchInfo.IsCheckPermission ? tUserInfo.GetCenterCode : "";
                string tMangTypeCd = GetMangKb ? "" : tUserInfo.GetMangType;

                string tORG1 = "", tORG2 = "", tBranch = "";
                if (tSearchInfo.SelectFACTGroupInfo != null && !tSearchInfo.SelectFACTGroupInfo.ORG1Code.Equals("0"))
                {
                    tORG1 = tSearchInfo.SelectFACTGroupInfo.ORG1Code;
                    tORG2 = tSearchInfo.SelectFACTGroupInfo.BranchCode;
                    tBranch = tSearchInfo.SelectFACTGroupInfo.CenterCode;
                }

                using (var conn = GlobalClass.GetSqlConnection())
                {
                    var results = conn.Query("EXEC SP_RACT_GET_SearchDEVICEINFO @CenterCode,@IP,@Name,@Part,@Model,@ModelName,@Center,@Branch,@ORG1,@TpoName,@IPType,@MangType", 
                        new { CenterCode = tCenterCode, IP = tSearchInfo.DeviceIPAddress, Name = tSearchInfo.DeviceName, 
                              Part = tSearchInfo.DevicePart, Model = tSearchInfo.DeviceModel, ModelName = tSearchInfo.ModelName, 
                              Center = tBranch, Branch = tORG2, ORG1 = tORG1, TpoName = tSearchInfo.TpoName, IPType = tSearchInfo.IPTyep, MangType = tMangTypeCd });
                    
                    var tDeviceInfoCollection = MapToDeviceInfoCollection(results);
                    if (tDeviceInfoCollection.Count == 0) tDeviceInfoCollection.Add(new DeviceInfo());

                    tResultData.ResultData = GlobalClass.ObjectCompress(tDeviceInfoCollection);
                }
            }
            catch (Exception ex)
            {
                tResultData.Error.Error = E_ErrorType.UnKnownError;
                tResultData.Error.ErrorString = ex.Message;
            }

            GlobalClass.SendResultClient(tResultData);
        }

        internal static void RequestSearchDeviceForType(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                string[] tArrRequest = aClientRequest.RequestData as string[];

                using (var conn = GlobalClass.GetSqlConnection())
                {
                    var results = conn.Query("EXEC SP_RACT_SEARCH_DEVICE_FOR_TYPE @Type,@IP,@UserID", 
                        new { Type = tArrRequest[0], IP = tArrRequest[1], UserID = tArrRequest[2] });
                    
                    var tDeviceInfoCollection = MapToDeviceInfoCollection(results);
                    tResultData.ResultData = GlobalClass.ObjectCompress(tDeviceInfoCollection);
                }
            }
            catch (Exception ex)
            {
                tResultData.Error.Error = E_ErrorType.UnKnownError;
                tResultData.Error.ErrorString = ex.Message;
            }

            GlobalClass.SendResultClient(tResultData);
        }

        internal static void RequestSearchRMSCMTSDevice(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                DeviceSearchInfo tSearchInfo = aClientRequest.RequestData as DeviceSearchInfo;

                using (var conn = GlobalClass.GetSqlConnection())
                {
                    var row = conn.QueryFirstOrDefault("SELECT TOP 1 lgname, lgpwd FROM [FACT_MAIN].[dbo].[FOMS_RMS_CMTS_DEVICE] WHERE cmtsid = @IP", 
                        new { IP = tSearchInfo.DeviceIPAddress });

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
            }
            catch (Exception ex)
            {
                tResultData.Error.Error = E_ErrorType.UnKnownError;
                tResultData.Error.ErrorString = ex.Message;
            }

            GlobalClass.SendResultClient(tResultData);
        }

        internal static void RequestFactIPDeviceSearchProcess(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                string tData = aClientRequest.RequestData as string;
                string[] IPDeviceSearch = tData.Split('|');

                using (var conn = GlobalClass.GetSqlConnection())
                {
                    var results = conn.Query("EXEC SP_RACT_GET_SearchDEVICEINFO_IPList @IPList, @IPType", 
                        new { IPList = IPDeviceSearch[1], IPType = IPDeviceSearch[0] });
                    
                    var tDeviceInfoCollection = MapToDeviceInfoCollection(results);
                    tResultData.ResultData = GlobalClass.ObjectCompress(tDeviceInfoCollection);
                }
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
