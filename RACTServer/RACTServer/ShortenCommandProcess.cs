using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RACTCommonClass;
using Dapper;

namespace RACTServer
{
    /// <summary>
    /// .NET 10 기반 고성능 단축 명령 처리 프로세스 (Repository 패턴 적용)
    /// </summary>
    public class ShortenCommandProcess
    {
        internal static async Task RequestProcessAsync(RequestCommunicationData aClientRequest)
        {
            var tRequesetInfo = (ShortenCommandRequestInfo)aClientRequest.RequestData;
            if (tRequesetInfo.WorkType == E_WorkType.Search) await SearchShortenCommandAsync(aClientRequest);
            else await ModifyShortenCommandAsync(aClientRequest);
        }

        private static async Task ModifyShortenCommandAsync(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tRequest = (ShortenCommandRequestInfo)aClientRequest.RequestData;
                tRequest.ShortenCommandInfo.ID = await GlobalClass.ShortenCommandRepo.ModifyShortenCommandAsync(tRequest.WorkType, tRequest.UserID, tRequest.ShortenCommandInfo);
                tResultData.ResultData = tRequest.ShortenCommandInfo;
                GlobalClass.SendResultClient(tResultData);
            }
            catch (Exception ex)
            {
                tResultData.Error = new ErrorInfo(E_ErrorType.UnKnownError, ex.Message);
                GlobalClass.SendResultClient(tResultData);
            }
        }

        private static async Task SearchShortenCommandAsync(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tRequest = (ShortenCommandRequestInfo)aClientRequest.RequestData;
                var (groups, commands) = await GlobalClass.ShortenCommandRepo.GetShortenCommandsAsync(tRequest.UserID);

                var groupList = new ShortenCommandGroupInfoCollection();
                foreach (var g in groups) groupList.Add(g);

                foreach (var c in commands)
                {
                    var targetGroup = groupList.Cast<ShortenCommandGroupInfo>().FirstOrDefault(g => g.ID == c.GroupID);
                    targetGroup?.ShortenCommandList.Add(c);
                }

                tResultData.ResultData = groupList;
                GlobalClass.SendResultClient(tResultData);
            }
            catch (Exception ex)
            {
                tResultData.Error = new ErrorInfo(E_ErrorType.UnKnownError, ex.Message);
                GlobalClass.SendResultClient(tResultData);
            }
        }

        internal static async Task RequestGroupProcessAsync(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = new ResultCommunicationData(aClientRequest);
            try
            {
                var tRequest = (ShortenCommandGroupRequestInfo)aClientRequest.RequestData;
                tRequest.ShortenCommandGroupInfo.ID = await GlobalClass.ShortenCommandRepo.ModifyShortenCommandGroupAsync(tRequest.WorkType, tRequest.UserID, tRequest.ShortenCommandGroupInfo);
                tResultData.ResultData = tRequest.ShortenCommandGroupInfo;
                GlobalClass.SendResultClient(tResultData);
            }
            catch (Exception ex)
            {
                tResultData.Error = new ErrorInfo(E_ErrorType.UnKnownError, ex.Message);
                GlobalClass.SendResultClient(tResultData);
            }
        }
    }
}
