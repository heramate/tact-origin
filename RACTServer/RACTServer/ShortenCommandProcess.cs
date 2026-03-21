using System;
using System.Collections.Generic;
using System.Linq;
using RACTCommonClass;
using Dapper;

namespace RACTServer
{
    public class ShortenCommandProcess
    {
        /// <summary>
        /// 요청을 처리 합니다.
        /// </summary>
        /// <param name="aClientRequest"></param>
        internal static void RequestProcess(RequestCommunicationData aClientRequest)
        {
            ShortenCommandRequestInfo tRequesetInfo = aClientRequest.RequestData as ShortenCommandRequestInfo;

            switch (tRequesetInfo.WorkType)
            {
                case E_WorkType.Search:
                    SearchShortenCommand(aClientRequest);
                    break;
                default:
                    ModifyShortenCommand(aClientRequest);
                    break;
            }
        }

        /// <summary>
        /// 단축 명령을 수정 합니다.
        /// </summary>
        /// <param name="aClientRequest"></param>
        private static void ModifyShortenCommand(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tRequestCommandInfo = (ShortenCommandRequestInfo)aClientRequest.RequestData;
                var tInfo = tRequestCommandInfo.ShortenCommandInfo;

                using (var conn = GlobalClass.GetSqlConnection())
                {
                    string tQuery = "EXEC SP_RACT_Modify_ShortenCommand @WorkType, @ID, @GroupID, @UserID, @Name, @Command, @Description";
                    var result = conn.QueryFirstOrDefault(tQuery, new
                    {
                        WorkType = (int)tRequestCommandInfo.WorkType,
                        ID = tInfo.ID,
                        GroupID = tInfo.GroupID,
                        UserID = tRequestCommandInfo.UserID,
                        Name = tInfo.Name,
                        Command = tInfo.Command,
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
        /// 단축 명령 목록을 가져오기 합니다.
        /// </summary>
        /// <param name="aClientRequest"></param>
        private static void SearchShortenCommand(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tRequestCommandInfo = (ShortenCommandRequestInfo)aClientRequest.RequestData;

                using (var conn = GlobalClass.GetSqlConnection())
                {
                    conn.Open();
                    using (var multi = conn.QueryMultiple("EXEC SP_RACT_Get_ShortenCommand @UserID", new { UserID = tRequestCommandInfo.UserID }))
                    {
                        var groups = multi.Read<ShortenCommandGroupInfo>().ToList();
                        var commands = multi.Read<ShortenCommandInfo>().ToList();

                        var groupList = new ShortenCommandGroupInfoCollection();
                        foreach (var g in groups) groupList.Add(g);

                        foreach (var c in commands)
                        {
                            if (c.GroupID >= 0 && c.GroupID < groupList.Count)
                            {
                                groupList[c.GroupID].ShortenCommandList.Add(c);
                            }
                        }

                        tResultData.ResultData = groupList;
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
        /// 단축 명령 그룹을 처리 합니다.
        /// </summary>
        /// <param name="aClientRequest"></param>
        internal static void RequestGroupProcess(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tRequestCommandInfo = (ShortenCommandGroupRequestInfo)aClientRequest.RequestData;
                var tInfo = tRequestCommandInfo.ShortenCommandGroupInfo;

                using (var conn = GlobalClass.GetSqlConnection())
                {
                    string tQuery = "EXEC SP_RACT_Modify_ShortenCommandGroup @WorkType, @ID, @UserID, @Name, @Description";
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
    }
}
