using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RACTCommonClass;
using Dapper;

namespace RACTServer
{
    public class ShortenCommandProcess
    {
        /// <summary>
        /// 요청을 비동기로 처리 합니다.
        /// </summary>
        /// <param name="aClientRequest"></param>
        internal static async Task RequestProcessAsync(RequestCommunicationData aClientRequest)
        {
            ShortenCommandRequestInfo tRequesetInfo = aClientRequest.RequestData as ShortenCommandRequestInfo;

            switch (tRequesetInfo.WorkType)
            {
                case E_WorkType.Search:
                    await SearchShortenCommandAsync(aClientRequest);
                    break;
                default:
                    await ModifyShortenCommandAsync(aClientRequest);
                    break;
            }
        }

        /// <summary>
        /// 단축 명령을 비동기로 수정 합니다.
        /// </summary>
        /// <param name="aClientRequest"></param>
        private static async Task ModifyShortenCommandAsync(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tRequestCommandInfo = (ShortenCommandRequestInfo)aClientRequest.RequestData;
                var tInfo = tRequestCommandInfo.ShortenCommandInfo;

                using (var conn = GlobalClass.GetSqlConnection())
                {
                    string tQuery = "EXEC SP_RACT_Modify_ShortenCommand @WorkType, @ID, @GroupID, @UserID, @Name, @Command, @Description";
                    var result = await conn.QueryFirstOrDefaultAsync(tQuery, new
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
        /// 단축 명령 목록을 비동기로 가져오기 합니다.
        /// </summary>
        /// <param name="aClientRequest"></param>
        private static async Task SearchShortenCommandAsync(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tRequestCommandInfo = (ShortenCommandRequestInfo)aClientRequest.RequestData;

                using (var conn = GlobalClass.GetSqlConnection())
                {
                    await conn.OpenAsync();
                    using (var multi = await conn.QueryMultipleAsync("EXEC SP_RACT_Get_ShortenCommand @UserID", new { UserID = tRequestCommandInfo.UserID }))
                    {
                        var groups = (await multi.ReadAsync<ShortenCommandGroupInfo>()).ToList();
                        var commands = (await multi.ReadAsync<ShortenCommandInfo>()).ToList();

                        var groupList = new ShortenCommandGroupInfoCollection();
                        foreach (var g in groups) groupList.Add(g);

                        foreach (var c in commands)
                        {
                            // GroupID는 groupList의 Index가 아닐 수 있으므로 정확한 매칭 로직 필요 (기존 로직 유지하되 안전성 확보)
                            var targetGroup = groupList.Cast<ShortenCommandGroupInfo>().FirstOrDefault(g => g.ID == c.GroupID);
                            if (targetGroup != null)
                            {
                                targetGroup.ShortenCommandList.Add(c);
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
        /// 단축 명령 그룹을 비동기로 처리 합니다.
        /// </summary>
        /// <param name="aClientRequest"></param>
        internal static async Task RequestGroupProcessAsync(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tRequestCommandInfo = (ShortenCommandGroupRequestInfo)aClientRequest.RequestData;
                var tInfo = tRequestCommandInfo.ShortenCommandGroupInfo;

                using (var conn = GlobalClass.GetSqlConnection())
                {
                    string tQuery = "EXEC SP_RACT_Modify_ShortenCommandGroup @WorkType, @ID, @UserID, @Name, @Description";
                    var result = await conn.QueryFirstOrDefaultAsync(tQuery, new
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
