using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Ports;
using System.Threading.Tasks;
using RACTCommonClass;
using Dapper;

namespace RACTServer
{
    /// <summary>
    /// .NET 10 기반 고성능 그룹 처리 프로세스 (Repository 패턴 적용)
    /// </summary>
    public class GroupProcess
    {
        internal static async Task RequestProcessAsync(RequestCommunicationData aClientRequest)
        {
            var tRequesetInfo = (GroupRequestInfo)aClientRequest.RequestData;
            if (tRequesetInfo.WorkType == E_WorkType.Search) await GroupInfoReceiverAsync(aClientRequest);
            else await ModifyGroupInfoAsync(aClientRequest);
        }

        private static async Task ModifyGroupInfoAsync(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tGroupRequestInfo = (GroupRequestInfo)aClientRequest.RequestData;
                var tGroupInfo = tGroupRequestInfo.GroupInfo;

                string? newID = await GlobalClass.GroupRepo.ModifyGroupInfoAsync(tGroupRequestInfo.WorkType, tGroupRequestInfo.UserID, tGroupInfo);
                if (newID != null) tGroupInfo.ID = newID;

                tResultData.ResultData = tGroupInfo;
                GlobalClass.SendResultClient(tResultData);
            }
            catch (Exception ex)
            {
                tResultData.Error = new ErrorInfo(E_ErrorType.UnKnownError, ex.Message);
                GlobalClass.SendResultClient(tResultData);
            }
        }

        private static async Task GroupInfoReceiverAsync(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tGroupRequestInfo = (GroupRequestInfo)aClientRequest.RequestData;
                var (groups, devices) = await GlobalClass.GroupRepo.GetGroupInfoWithDevicesAsync(tGroupRequestInfo.UserID);

                var groupInfoCollection = MapToGroupInfoCollection(groups, devices);
                tResultData.ResultData = groupInfoCollection;
                GlobalClass.SendResultClient(tResultData);
            }
            catch (Exception ex)
            {
                tResultData.Error = new ErrorInfo(E_ErrorType.UnKnownError, ex.Message);
                GlobalClass.SendResultClient(tResultData);
            }
        }

        private static GroupInfoCollection MapToGroupInfoCollection(IEnumerable<dynamic> groups, IEnumerable<dynamic> devices)
        {
            var collection = new GroupInfoCollection();
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
                    LEVEL = dict.ContainsKey("LEVEL_1") ? Convert.ToInt32(dict["LEVEL_1"]) : 0,
                    SEQ_ID = dict.ContainsKey("SEQ_ID") ? Convert.ToInt32(dict["SEQ_ID"]) : 0,
                    DEVICE_COUNT = dict.ContainsKey("DEVICE_COUNT") ? Convert.ToInt32(dict["DEVICE_COUNT"]) : 0,
                    UserID = dict.ContainsKey("UserID") ? Convert.ToInt32(dict["UserID"]) : 0
                };
                if (!collection.ContainsKey(group.ID)) collection.Add(group.ID, group);
            }

            foreach (var d in devices)
            {
                IDictionary<string, object> dict = (IDictionary<string, object>)d;
                string rGroupID = dict["RACTGroupID"]?.ToString() ?? "0";
                
                if (!collection.ContainsKey(rGroupID))
                {
                    collection.Add(rGroupID, new GroupInfo { ID = rGroupID, Name = dict["RACTName"]?.ToString(), TOP_ID = rGroupID });
                }

                var targetGroup = collection[rGroupID];
                var tDeviceInfo = new DeviceInfo
                {
                    DeviceID = Convert.ToInt32(dict["NeID"]),
                    ModelID = Convert.ToInt32(dict["ModelID"]),
                    ModelName = dict["ModelName"]?.ToString(),
                    DevicePartCode = Convert.ToInt32(dict["ModelTypeCode"]),
                    GroupID = targetGroup.ID,
                    Name = dict["NeName"]?.ToString(),
                    IPAddress = dict["MasterIP"]?.ToString(),
                    DeviceType = dict.ContainsKey("DeviceType") ? (E_DeviceType)Convert.ToInt32(dict["DeviceType"]) : E_DeviceType.NeGroup
                };
                targetGroup.DeviceList.Add(tDeviceInfo);
            }
            return collection;
        }

        internal static async Task RequestRactUserListProcessAsync(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tRequest = (string[])aClientRequest.RequestData;
                var users = await GlobalClass.GroupRepo.GetUserListAsync(tRequest[0], tRequest[1], Convert.ToInt32(tRequest[2]));
                
                var collection = new UserInfoCollection();
                foreach (var u in users) collection.Add(u);
                
                tResultData.ResultData = collection;
                GlobalClass.SendResultClient(tResultData);
            }
            catch (Exception ex)
            {
                tResultData.Error = new ErrorInfo(E_ErrorType.UnKnownError, ex.Message);
                GlobalClass.SendResultClient(tResultData);
            }
        }

        internal static async Task RequestAddShareDeviceProcessAsync(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var groupInfo = (GroupInfo)aClientRequest.RequestData;
                string? newID = await GlobalClass.GroupRepo.ModifyGroupInfoAsync(E_WorkType.Add, groupInfo.UserID, groupInfo);
                if (newID != null)
                {
                    groupInfo.ID = newID;
                    foreach (DeviceInfo device in groupInfo.DeviceList)
                    {
                        device.GroupID = newID;
                        await GlobalClass.DeviceRepo.ExecuteModifyDeviceAsync(E_WorkType.Add, groupInfo.UserID, device);
                    }
                }
                tResultData.ResultData = groupInfo;
                GlobalClass.SendResultClient(tResultData);
            }
            catch (Exception ex)
            {
                tResultData.Error = new ErrorInfo(E_ErrorType.UnKnownError, ex.Message);
                GlobalClass.SendResultClient(tResultData);
            }
        }

        internal static async Task<FACTGroupInfo> GetFactGroupAsync(UserInfo aUserInfo)
        {
            FACTGroupInfo root = new FACTGroupInfo();
            try
            {
                var results = await GlobalClass.GroupRepo.GetFactGroupTreeAsync(aUserInfo.GetCenterCode);
                MapToFactTree(root, results);
            }
            catch { }
            await FactCountDeviceAsync(root);
            return root;
        }

        private static void MapToFactTree(FACTGroupInfo root, IEnumerable<dynamic> results)
        {
            string tOldCenterCode = "", tOldBranchCode = "", tOldORG1Code = "";
            FACTGroupInfo? tCenterGroupInfo = null, tBranchGroupInfo = null, tORG1GroupInfo = null;

            foreach (var row in results)
            {
                IDictionary<string, object> dict = (IDictionary<string, object>)row;
                string org1Code = dict["org1_id"]?.ToString() ?? "";
                string branchCode = dict["org2_id"]?.ToString() ?? "";
                string centerCode = dict["CenterCode"]?.ToString() ?? "";

                if (tOldORG1Code != org1Code)
                {
                    tOldORG1Code = org1Code;
                    tORG1GroupInfo = new FACTGroupInfo { ORG1Code = org1Code, ORG1Name = dict["org1_name"]?.ToString() ?? "" };
                    root.SubGroups ??= new FACTGroupInfoCollection();
                    root.SubGroups.Add(tORG1GroupInfo);
                }

                if (tOldBranchCode != branchCode)
                {
                    tOldBranchCode = branchCode;
                    tBranchGroupInfo = new FACTGroupInfo { BranchCode = branchCode, BranchName = dict["org2_name"]?.ToString() ?? "", ORG1Code = org1Code };
                    tORG1GroupInfo!.SubGroups ??= new FACTGroupInfoCollection();
                    tORG1GroupInfo.SubGroups.Add(tBranchGroupInfo);
                }

                if (tOldCenterCode != centerCode)
                {
                    tOldCenterCode = centerCode;
                    tCenterGroupInfo = new FACTGroupInfo { CenterCode = centerCode, CenterName = dict["CenterName"]?.ToString() ?? "", BranchCode = branchCode, ORG1Code = org1Code };
                    tBranchGroupInfo!.SubGroups ??= new FACTGroupInfoCollection();
                    tBranchGroupInfo.SubGroups.Add(tCenterGroupInfo);
                }
            }
        }

        private static async Task FactCountDeviceAsync(FACTGroupInfo group)
        {
            if (group.SubGroups != null)
            {
                foreach (FACTGroupInfo sub in group.SubGroups) await FactCountDeviceAsync(sub);
            }
            if (!string.IsNullOrEmpty(group.CenterCode))
            {
                group.DeviceCount = await GlobalClass.DeviceRepo.CountDeviceAsync(group.CenterCode);
            }
        }
    }
}
