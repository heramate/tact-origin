using RACTCommonClass;
using System;
using System.Threading.Tasks;
using Dapper;

namespace RACTServer
{
    public class DeviceConnectionLogService
    {
        public async Task<DeviceConnectionLogOpenResultInfo> OpenLogAsync(UserInfo aUserInfo, DeviceConnectionLogOpenRequestInfo aRequest)
        {
            DeviceConnectionLogOpenResultInfo tResult = new DeviceConnectionLogOpenResultInfo();

            try
            {
                if (aUserInfo == null)
                {
                    tResult.Success = false;
                    tResult.ErrorMessage = "사용자 세션 정보가 없습니다.";
                    return tResult;
                }

                using (var conn = GlobalClass.GetSqlConnection())
                {
                    if (conn == null) throw new Exception("DBConnection Failed");
                    
                    var sql = "EXEC [dbo].[SP_RACT_DeviceConnectLog] @UserID, @DeviceID, @ConnectType, @Description, @ConnectionKind";
                    var param = new
                    {
                        UserID = aUserInfo.UserID,
                        DeviceID = aRequest.DeviceID,
                        ConnectType = (int)E_DeviceConnectType.Connection,
                        Description = aRequest.Description,
                        ConnectionKind = aRequest.ConnectionKind
                    };

                    tResult.ConnectionLogID = await conn.QueryFirstOrDefaultAsync<int>(sql, param);
                }

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

            return tResult;
        }

        public async Task<DeviceConnectionLogCloseResultInfo> CloseLogAsync(UserInfo aUserInfo, DeviceConnectionLogCloseRequestInfo aRequest)
        {
            DeviceConnectionLogCloseResultInfo tResult = new DeviceConnectionLogCloseResultInfo();

            try
            {
                using (var conn = GlobalClass.GetSqlConnection())
                {
                    if (conn == null) throw new Exception("DBConnection Failed");
                    await conn.ExecuteAsync("update RACT_LOG_DeviceConnection set ConnectLogType = 1, DisconnectTime = GETDATE() where id = @ID", new { ID = aRequest.ConnectionLogID });
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
    }
}
