using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Ports;
using System.Threading.Tasks;
using RACTCommonClass;
using Dapper;

namespace RACTServer
{
    public class GroupProcess
    {
        private static async Task ModifyGroupInfoAsync(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tGroupRequestInfo = (GroupRequestInfo)aClientRequest.RequestData;
                var tGroupInfo = tGroupRequestInfo.GroupInfo;

                using (var conn = GlobalClass.GetSqlConnection())
                {
                    string tQuery = "EXEC SP_RACT_Modify_GroupInfo @WorkType, @UserID, @ID, @Name, @Description, @TOP_ID, @UP_ID";
                    var result = await conn.QueryFirstOrDefaultAsync(tQuery, new
                    {
                        WorkType = (int)tGroupRequestInfo.WorkType,
                        UserID = tGroupRequestInfo.UserID,
                        ID = tGroupRequestInfo.WorkType == E_WorkType.Add ? null : tGroupInfo.ID,
                        Name = tGroupInfo.Name,
                        Description = tGroupInfo.Description,
                        TOP_ID = tGroupInfo.TOP_ID,
                        UP_ID = tGroupInfo.UP_ID
                    });

                    if (result != null)
                    {
                        tGroupInfo.ID = ((IDictionary<string, object>)result)["ID"]?.ToString();
                    }
                    tResultData.ResultData = tGroupInfo;
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

        /// <summary>
        /// 그룹 정보를 전송할 비동기 메서드입니다.
        /// </summary>
        /// <param name="aClientRequest"></param>
        private static async Task GroupInfoReceiverAsync(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tGroupRequestInfo = (GroupRequestInfo)aClientRequest.RequestData;

                using (var conn = GlobalClass.GetSqlConnection())
                {
                    await conn.OpenAsync();
                    using (var multi = await conn.QueryMultipleAsync("EXEC SP_RACT_Get_GROUPINFO @UserID", new { UserID = tGroupRequestInfo.UserID }))
                    {
                        var groups = (await multi.ReadAsync<dynamic>()).ToList();
                        var devices = (await multi.ReadAsync<dynamic>()).ToList();

                        var groupInfoCollection = new GroupInfoCollection();
                        foreach (var g in groups)
                        {
                            IDictionary<string, object> dict = (IDictionary<string, object>)g;
                            var group = new GroupInfo
                            {
                                ID = dict["ID"]?.ToString(),
                                Name = dict["Name"]?.ToString(),
                                Description = dict["Description"]?.ToString(),
                                TOP_ID = dict["TOP_ID"]?.ToString(),
                                UP_ID = dict["UP_ID"]?.ToString(),
                                LEVEL = dict["LEVEL_1"] != DBNull.Value ? Convert.ToInt32(dict["LEVEL_1"]) : 0,
                                SEQ_ID = dict["SEQ_ID"] != DBNull.Value ? Convert.ToInt32(dict["SEQ_ID"]) : 0,
                                DEVICE_COUNT = dict["DEVICE_COUNT"] != DBNull.Value ? Convert.ToInt32(dict["DEVICE_COUNT"]) : 0,
                                UserID = dict["UserID"] != DBNull.Value ? Convert.ToInt32(dict["UserID"]) : 0
                            };
                            if (!groupInfoCollection.ContainsKey(group.ID))
                                groupInfoCollection.Add(group.ID, group);
                        }

                        foreach (var d in devices)
                        {
                            IDictionary<string, object> dict = (IDictionary<string, object>)d;
                            string rGroupID = dict["RACTGroupID"]?.ToString();
                            
                            GroupInfo targetGroup;
                            if (groupInfoCollection.ContainsKey(rGroupID))
                            {
                                targetGroup = groupInfoCollection[rGroupID];
                            }
                            else
                            {
                                targetGroup = new GroupInfo
                                {
                                    ID = rGroupID,
                                    Name = dict["RACTName"]?.ToString(),
                                    Description = dict["RACTDescription"]?.ToString(),
                                    TOP_ID = rGroupID
                                };
                                groupInfoCollection.Add(rGroupID, targetGroup);
                            }

                            var tDeviceInfo = new DeviceInfo
                            {
                                DeviceID = Convert.ToInt32(dict["NeID"]),
                                ModelID = Convert.ToInt32(dict["ModelID"]),
                                ModelName = dict["ModelName"]?.ToString(),
                                DevicePartCode = Convert.ToInt32(dict["ModelTypeCode"]),
                                GroupID = targetGroup.ID,
                                Name = dict["NeName"]?.ToString(),
                                IPAddress = dict["MasterIP"]?.ToString(),
                                TelnetID1 = dict["TelnetID_1"]?.ToString().Trim(),
                                TelnetPwd1 = dict["Passwd_1"]?.ToString().Trim(),
                                TelnetID2 = dict["TelnetID_2"]?.ToString().Trim(),
                                TelnetPwd2 = dict["Passwd_2"]?.ToString().Trim(),
                                ORG1Code = dict["ORG1_ID"]?.ToString(),
                                ORG1Name = dict["ORG1_Name"]?.ToString(),
                                ORG2Code = dict["ORG2_ID"]?.ToString(),
                                ORG2Name = dict["ORG2_Name"]?.ToString(),
                                CenterName = dict["CenterName"]?.ToString(),
                                TpoName = dict["TpoName"]?.ToString(),
                                DeviceType = dict["DeviceType"] != DBNull.Value ? (E_DeviceType)Convert.ToInt32(dict["DeviceType"]) : E_DeviceType.NeGroup,
                                WAIT = dict["WAIT1"]?.ToString(),
                                USERID = dict["USERID1"]?.ToString(),
                                PWD = dict["PWD1"]?.ToString(),
                                USERID2 = dict["USERID2"]?.ToString(),
                                PWD2 = dict["PWD2"]?.ToString(),
                                MoreString = dict["MoreString"]?.ToString(),
                                MoreMark = dict["MoreMark"]?.ToString(),
                                UsrName = dict["UsrName"]?.ToString(),
                                UsrID = dict["UsrID"] != DBNull.Value ? Convert.ToInt32(dict["UsrID"]) : 0,
                                Account = dict["Account"]?.ToString()
                            };
                            tDeviceInfo.TerminalConnectInfo.IPAddress = tDeviceInfo.IPAddress;
                            tDeviceInfo.TerminalConnectInfo.TelnetPort = dict["TelnetPort"] != DBNull.Value ? Convert.ToInt32(dict["TelnetPort"]) : 0;
                            tDeviceInfo.TerminalConnectInfo.ConnectionProtocol = dict["Protocol"] != DBNull.Value ? (E_ConnectionProtocol)Convert.ToInt32(dict["Protocol"]) : E_ConnectionProtocol.TELNET;
                            tDeviceInfo.TerminalConnectInfo.SerialConfig.BaudRate = dict["BaudRate"] != DBNull.Value ? Convert.ToInt32(dict["BaudRate"]) : 9600;
                            tDeviceInfo.TerminalConnectInfo.SerialConfig.DataBits = dict["DataBits"] != DBNull.Value ? Convert.ToInt32(dict["DataBits"]) : 8;
                            tDeviceInfo.TerminalConnectInfo.SerialConfig.Handshake = dict["HandShake"] != DBNull.Value ? (Handshake)Convert.ToInt32(dict["HandShake"]) : Handshake.None;
                            tDeviceInfo.TerminalConnectInfo.SerialConfig.Parity = dict["Parity"] != DBNull.Value ? (Parity)Convert.ToInt32(dict["Parity"]) : Parity.None;
                            tDeviceInfo.TerminalConnectInfo.SerialConfig.PortName = dict["PortName"]?.ToString();
                            tDeviceInfo.TerminalConnectInfo.SerialConfig.StopBits = dict["StopBits"] != DBNull.Value ? (StopBits)Convert.ToInt32(dict["StopBits"]) : StopBits.One;

                            targetGroup.DeviceList.Add(tDeviceInfo);
                        }
                        tResultData.ResultData = groupInfoCollection;
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

        /// <summary>
        /// 요청을 비동기로 처리 합니다.
        /// </summary>
        /// <param name="aClientRequest"></param>
        internal static async Task RequestProcessAsync(RequestCommunicationData aClientRequest)
        {
            GroupRequestInfo tRequesetInfo = aClientRequest.RequestData as GroupRequestInfo;

            switch (tRequesetInfo.WorkType)
            {
                case E_WorkType.Search:
                    await GroupInfoReceiverAsync(aClientRequest);
                    break;
                default:
                    await ModifyGroupInfoAsync(aClientRequest);
                    break;
            }
        }

        internal static async Task RequestRactUserListProcessAsync(RequestCommunicationData aClientRequest)
        {
            await RactUserListReceiverAsync(aClientRequest);
        }

        private static async Task RactUserListReceiverAsync(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tRequestInfo = (string[])aClientRequest.RequestData;

                using (var conn = GlobalClass.GetSqlConnection())
                {
                    var results = (await conn.QueryAsync<UserInfo>("EXEC SP_RACT_GET_USER_LIST @SearchType,@SearchValue,@DeleteUserID", 
                        new { SearchType = tRequestInfo[0], SearchValue = tRequestInfo[1], DeleteUserID = Convert.ToInt32(tRequestInfo[2]) })).ToList();
                    
                    var tUserInfos = new UserInfoCollection();
                    foreach (var u in results) tUserInfos.Add(u);
                    
                    tResultData.ResultData = tUserInfos;
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

        internal static async Task RequestAddShareDeviceProcessAsync(RequestCommunicationData aClientRequest)
        {
            await AddShareDeviceInfoReceiverAsync(aClientRequest);
        }

        private static async Task AddShareDeviceInfoReceiverAsync(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tRequestInfo = (GroupInfo)aClientRequest.RequestData;

                using (var conn = GlobalClass.GetSqlConnection())
                {
                    await conn.OpenAsync();
                    var result = await conn.QueryFirstOrDefaultAsync("EXEC SP_RACT_Modify_GroupInfo @WorkType, @UserID, @ID, @Name, @Description, '', ''", 
                        new { WorkType = (int)E_WorkType.Add, UserID = tRequestInfo.UserID, ID = (string)null, Name = tRequestInfo.Name, Description = tRequestInfo.Description });

                    if (result != null)
                    {
                        tRequestInfo.ID = ((IDictionary<string, object>)result)["ID"]?.ToString();
                    }

                    foreach (DeviceInfo aDeviceInfo in tRequestInfo.DeviceList)
                    {
                        try
                        {
                            if (aDeviceInfo.DeviceType == E_DeviceType.NeGroup)
                            {
                                await conn.ExecuteAsync("EXEC SP_RACT_MODIFY_DEVICEINFO @WorkType,@GroupID,@UserID,@DeviceID,@Protocol,@DeviceType,@TelnetPort,@PortName,@BaudRate,@DataBits,@Parity,@StopBits,@Handshake,'','','','','','','','','','','','','',@MoreString,@MoreMark", 
                                    new { WorkType = (int)E_WorkType.Add, GroupID = tRequestInfo.ID, UserID = tRequestInfo.UserID, DeviceID = aDeviceInfo.DeviceID, 
                                          Protocol = (int)aDeviceInfo.TerminalConnectInfo.ConnectionProtocol, DeviceType = (int)aDeviceInfo.DeviceType, 
                                          TelnetPort = aDeviceInfo.TerminalConnectInfo.TelnetPort, PortName = aDeviceInfo.TerminalConnectInfo.SerialConfig.PortName, 
                                          BaudRate = aDeviceInfo.TerminalConnectInfo.SerialConfig.BaudRate, DataBits = aDeviceInfo.TerminalConnectInfo.SerialConfig.DataBits, 
                                          Parity = (int)aDeviceInfo.TerminalConnectInfo.SerialConfig.Parity, StopBits = (int)aDeviceInfo.TerminalConnectInfo.SerialConfig.StopBits, 
                                          Handshake = (int)aDeviceInfo.TerminalConnectInfo.SerialConfig.Handshake, MoreString = aDeviceInfo.MoreString, MoreMark = aDeviceInfo.MoreMark });
                            }
                            else
                            {
                                await conn.ExecuteAsync("EXEC SP_RACT_MODIFY_DEVICEINFO @WorkType,@GroupID,@UserID,@DeviceID,@Protocol,@DeviceType,@TelnetPort,@PortName,@BaudRate,@DataBits,@Parity,@StopBits,@Handshake,@ModelName,@IPAddress,@TelnetID1,@TelnetPwd1,@TelnetID2,@TelnetPwd2,@Name,@TpoName,@WAIT,@USERID,@PWD,@USERID2,@PWD2,@MoreString,@MoreMark", 
                                    new { WorkType = (int)E_WorkType.Add, GroupID = tRequestInfo.ID, UserID = tRequestInfo.UserID, DeviceID = aDeviceInfo.DeviceID, 
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
                            System.Diagnostics.Debug.Write(ex.ToString());
                        }
                    }
                    tResultData.ResultData = tRequestInfo;
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

        internal static async Task<FACTGroupInfo> GetFactGroupAsync(UserInfo aUserInfo)
        {
            FACTGroupInfo tUserFACTGroupInfo = new FACTGroupInfo();
            try
            {
                string query = string.Format(SQLQuery.SelectFACTGroupInfo(), aUserInfo.GetCenterCode);
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "사용자별 FACT 그룹정보를 로드 합니다.");

                using (var conn = GlobalClass.GetSqlConnection())
                {
                    await conn.OpenAsync();
                    var results = await conn.QueryAsync(query);
                    
                    string tOldCenterCode = "", tOldBranchCode = "", tOldORG1Code = "";
                    FACTGroupInfo tCenterGroupInfo = null, tBranchGroupInfo = null, tORG1GroupInfo = null;

                    foreach (var row in results)
                    {
                        IDictionary<string, object> dict = (IDictionary<string, object>)row;
                        string branchCode = dict["org2_id"]?.ToString().Trim();
                        string centerCode = dict["CenterCode"]?.ToString().Trim();
                        string org1Code = dict["org1_id"]?.ToString();

                        if (tOldORG1Code != org1Code)
                        {
                            tOldORG1Code = org1Code;
                            tORG1GroupInfo = new FACTGroupInfo { ORG1Code = org1Code, ORG1Name = dict["org1_name"]?.ToString() };
                            if (tUserFACTGroupInfo.SubGroups == null) tUserFACTGroupInfo.SubGroups = new FACTGroupInfoCollection();
                            tUserFACTGroupInfo.SubGroups.Add(tORG1GroupInfo);
                        }

                        if (tOldBranchCode != branchCode)
                        {
                            tOldBranchCode = branchCode;
                            tBranchGroupInfo = new FACTGroupInfo { BranchCode = branchCode, BranchName = dict["org2_name"]?.ToString(), ORG1Code = org1Code, ORG1Name = dict["org1_name"]?.ToString() };
                            if (tORG1GroupInfo.SubGroups == null) tORG1GroupInfo.SubGroups = new FACTGroupInfoCollection();
                            tORG1GroupInfo.SubGroups.Add(tBranchGroupInfo);
                        }

                        if (tOldCenterCode != centerCode)
                        {
                            tOldCenterCode = centerCode;
                            tCenterGroupInfo = new FACTGroupInfo { ORG1Code = org1Code, ORG1Name = dict["org1_name"]?.ToString(), BranchCode = branchCode, BranchName = dict["org2_name"]?.ToString(), CenterCode = centerCode, CenterName = dict["CenterName"]?.ToString() };
                            if (tBranchGroupInfo.SubGroups == null) tBranchGroupInfo.SubGroups = new FACTGroupInfoCollection();
                            tBranchGroupInfo.SubGroups.Add(tCenterGroupInfo);
                        }
                    }
                }
            }
            catch (Exception) { }
            await FactCountDeviceAsync(tUserFACTGroupInfo, aUserInfo);
            return tUserFACTGroupInfo;
        }

        private static async Task FactCountDeviceAsync(FACTGroupInfo aGroupInfo, UserInfo aUserInfo)
        {
            try
            {
                using (var conn = GlobalClass.GetSqlConnection())
                {
                    await conn.OpenAsync();
                    await FactCountDeviceRecursiveAsync(aGroupInfo, aUserInfo, conn);
                }
            }
            catch (Exception) { }
        }

        private static async Task FactCountDeviceRecursiveAsync(FACTGroupInfo aGroupInfo, UserInfo aUserInfo, System.Data.SqlClient.SqlConnection conn)
        {
            if (aGroupInfo.SubGroups != null && aGroupInfo.SubGroups.Count > 0)
            {
                foreach (FACTGroupInfo tGroupInfo in aGroupInfo.SubGroups)
                {
                    await FactCountDeviceRecursiveAsync(tGroupInfo, aUserInfo, conn);
                }
            }

            if (string.IsNullOrEmpty(aGroupInfo.CenterCode)) return;

            try
            {
                var result = await conn.QueryFirstOrDefaultAsync("EXEC SP_ORG_DEVICE_Count @CenterCode", new { CenterCode = aGroupInfo.CenterCode });
                if (result != null)
                {
                    aGroupInfo.DeviceCount = Convert.ToInt32(((IDictionary<string, object>)result)["devicecount"]);
                }
            }
            catch (Exception) { }
        }
    }
}
