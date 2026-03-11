using MKLibrary.MKData;
using RACTCommonClass;
using System;

namespace RACTServer
{
    public class DeviceConnectionLogService
    {
        public DeviceConnectionLogOpenResultInfo OpenLog(UserInfo aUserInfo, DeviceConnectionLogOpenRequestInfo aRequest)
        {
            DeviceConnectionLogOpenResultInfo tResult = new DeviceConnectionLogOpenResultInfo();
            MKDBWorkItem tDBWI = null;
            MKDataSet tDataSet = null;

            try
            {
                if (aUserInfo == null)
                {
                    tResult.Success = false;
                    tResult.ErrorMessage = "사용자 세션 정보가 없습니다.";
                    return tResult;
                }

                tDBWI = GlobalClass.m_DBPool.GetDBWorkItem();
                string tQuery = MakeOpenLogQuery(aUserInfo, aRequest);

                if (tDBWI.ExecuteQuery(tQuery, out tDataSet) != E_DBProcessError.Success || tDataSet == null)
                {
                    tResult.Success = false;
                    tResult.ErrorMessage = tDBWI.ErrorString;
                    return tResult;
                }

                tResult.ConnectionLogID = tDataSet.GetInt32("ID");
                tResult.Success = tResult.ConnectionLogID > 0;
                if (!tResult.Success)
                {
                    tResult.ErrorMessage = "ConnectionLogID를 생성하지 못했습니다.";
                }
            }
            catch (Exception ex)
            {
                tResult.Success = false;
                tResult.ErrorMessage = ex.ToString();
            }
            finally
            {
                MKOleDBClass.CloseDataSet(tDataSet);
            }

            return tResult;
        }

        public DeviceConnectionLogCloseResultInfo CloseLog(UserInfo aUserInfo, DeviceConnectionLogCloseRequestInfo aRequest)
        {
            DeviceConnectionLogCloseResultInfo tResult = new DeviceConnectionLogCloseResultInfo();
            MKDBWorkItem tDBWI = null;

            try
            {
                tDBWI = GlobalClass.m_DBPool.GetDBWorkItem();
                string tQuery = MakeCloseLogQuery(aRequest);

                if (tDBWI.ExecuteNoneQuery(tQuery) != E_DBProcessError.Success)
                {
                    tResult.Success = false;
                    tResult.ErrorMessage = tDBWI.ErrorString;
                    return tResult;
                }

                tResult.Success = true;
            }
            catch (Exception ex)
            {
                tResult.Success = false;
                tResult.ErrorMessage = ex.ToString();
            }

            return tResult;
        }

        private string MakeOpenLogQuery(UserInfo aUserInfo, DeviceConnectionLogOpenRequestInfo aRequest)
        {
            return string.Format(
                "EXEC [dbo].[SP_RACT_DeviceConnectLog] {0}, {1}, {2}, '{3}', {4}",
                aUserInfo.UserID,
                aRequest.DeviceID,
                (int)E_DeviceConnectType.Connection,
                aRequest.Description.Replace("'", "''"),
                aRequest.ConnectionKind);
        }

        private string MakeCloseLogQuery(DeviceConnectionLogCloseRequestInfo aRequest)
        {
            return string.Format(
                "update RACT_LOG_DeviceConnection set ConnectLogType = 1, DisconnectTime = GETDATE() where id = {0}",
                aRequest.ConnectionLogID);
        }
    }
}
