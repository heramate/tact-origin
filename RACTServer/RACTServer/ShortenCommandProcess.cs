using System;
using System.Collections.Generic;
using System.Text;
using RACTCommonClass;
using MKLibrary.MKData;

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
            ResultCommunicationData tResultData = null;
            MKDBWorkItem tDBWI = null;
            MKDataSet tDataSet = null;
            string tQueryString = "";
            ShortenCommandRequestInfo tRequestCommandInfo;
            ShortenCommandInfo tInfo;
            try
            {
                tResultData = new ResultCommunicationData(aClientRequest);
                tRequestCommandInfo = (ShortenCommandRequestInfo)aClientRequest.RequestData;
                tInfo = tRequestCommandInfo.ShortenCommandInfo;

                tQueryString = "EXEC SP_RACT_Modify_ShortenCommand {0}, {1}, {2}, '{3}', '{4}', '{5}','{6}'";

                tQueryString = string.Format(tQueryString, (int)tRequestCommandInfo.WorkType, tInfo.ID,tInfo.GroupID, tRequestCommandInfo.UserID, tInfo.Name, tInfo.Command, tInfo.Description);

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
                if(tDataSet != null)
                    MKOleDBClass.CloseDataSet(tDataSet);
            }

        }

        /// <summary>
        /// 단축 명령 목록을 가져오기 합니다.
        /// </summary>
        /// <param name="aClientRequest"></param>
        private static void SearchShortenCommand(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = null;
            MKDBWorkItem tDBWI = null;
            MKDataSet tDataSet = null;
            string tQueryString = "";
            ShortenCommandRequestInfo tRequestCommandInfo;
            ShortenCommandInfo tCommand;
            ShortenCommandInfoCollection tCommandList = new ShortenCommandInfoCollection();
            ShortenCommandGroupInfo tCommandGroupInfo = null;
            ShortenCommandGroupInfoCollection tGroupList = new ShortenCommandGroupInfoCollection();

            try
            {
                tResultData = new ResultCommunicationData(aClientRequest);
                tRequestCommandInfo = (ShortenCommandRequestInfo)aClientRequest.RequestData;

                tQueryString = "EXEC SP_RACT_Get_ShortenCommand {0}";

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
                            tCommandGroupInfo = new ShortenCommandGroupInfo();
                            tCommandGroupInfo.ID = tDataSet.GetInt32("ID");
                            tCommandGroupInfo.Name = tDataSet.GetString("Name");
                            tCommandGroupInfo.Description = tDataSet.GetString("Description");

                            tGroupList.Add(tCommandGroupInfo);

                            tDataSet.MoveNext();
                        }
                    }

                    tDataSet.CurrentTableIndex++;

                    for (int i = 0; i < tDataSet.RecordCount; i++)
                    {

                        tCommand = new ShortenCommandInfo();
                        tCommand.ID = tDataSet.GetInt32("Id");
                        tCommand.Name = tDataSet.GetString("Name");
                        tCommand.GroupID = tDataSet.GetInt32("GroupID");
                        tCommand.Command = tDataSet.GetString("Command");
                        tCommand.Description = tDataSet.GetString("Description");

                        tGroupList[tCommand.GroupID].ShortenCommandList.Add(tCommand);

                        tDataSet.MoveNext();
                    }

                    tResultData.ResultData = tGroupList;
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
                if(tDataSet != null)
                    MKOleDBClass.CloseDataSet(tDataSet);
                tGroupList.InnerList.Clear();
                tGroupList.Clear();
            }
        }

        /// <summary>
        /// 단축 명령 그룹을 처리 합니다.
        /// </summary>
        /// <param name="aClientRequest"></param>
        internal static void RequestGroupProcess(RequestCommunicationData aClientRequest)
        {
            ResultCommunicationData tResultData = null;
            MKDBWorkItem tDBWI = null;
            MKDataSet tDataSet = null;
            string tQueryString = "";
            ShortenCommandGroupRequestInfo tRequestCommandInfo;
            ShortenCommandGroupInfo tInfo;
            try
            {
                tResultData = new ResultCommunicationData(aClientRequest);
                tRequestCommandInfo = (ShortenCommandGroupRequestInfo)aClientRequest.RequestData;
                tInfo = tRequestCommandInfo.ShortenCommandGroupInfo;

                tQueryString = "EXEC SP_RACT_Modify_ShortenCommandGroup {0}, {1}, {2}, '{3}', '{4}'";

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
    }
}
