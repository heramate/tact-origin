using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;

namespace TACT.KeepAliveServer
{
    public static class DBWorker
    {
        private static string? _connectionString;

        public static void Initialize(string server, string database, string user, string password)
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = server,
                InitialCatalog = database,
                UserID = user,
                Password = password,
                TrustServerCertificate = true,
                MaxPoolSize = 100
            };
            _connectionString = builder.ConnectionString;
        }

        public static async Task<List<KeepAliveMsg>> SelectKeepAliveCollectionsAsync()
        {
            if (string.IsNullOrEmpty(_connectionString)) return new List<KeepAliveMsg>();

            using var connection = new SqlConnection(_connectionString);
            const string query = @"SELECT USIM, IMEI, SerialNumber, LTEModuleName, 
                                          NeID, DeviceIP, DeviceModelName, KeepAliveLastRecvDate 
                                   FROM LTE_NE WITH(NOLOCK)";

            var results = await connection.QueryAsync<KeepAliveMsg>(query);
            return results.AsList();
        }

        public static async Task Update_LTE_NE_Async(KeepAliveMsg msg)
        {
            if (string.IsNullOrEmpty(_connectionString)) return;
            if (string.IsNullOrEmpty(msg.USIM)) return;

            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@USIM", msg.USIM);
            parameters.Add("@IMEI", msg.IsFullMessage() ? msg.IMEI : null);
            parameters.Add("@SerialNumber", msg.IsFullMessage() ? msg.SerialNumber : null);
            parameters.Add("@LTEModuleName", msg.IsFullMessage() ? msg.LTEModuleName : null);
            parameters.Add("@DeviceIP", msg.DeviceIP);
            parameters.Add("@DeviceModelName", msg.IsFullMessage() ? msg.ModelName : null);
            parameters.Add("@KeepAliveRecvDate", msg.RecvDateTime);

            try
            {
                var result = await connection.QueryFirstOrDefaultAsync<dynamic>(
                    "P_LTE_KeepAlive_UPDATE_LTE_NE",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                if (result != null && result.ResultCode == "OK")
                {
                    msg.LTEModuleName = result.LTEModuleName;
                    msg.DeviceIP = result.DeviceIP;
                    msg.IMEI = result.IMEI;
                    msg.RecvDateTime = result.KeepAliveLastRecvDate;
                    msg.ModelName = result.DeviceModelName;
                    msg.SerialNumber = result.SerialNumber;
                    msg.NeId = result.NeID;
                }
            }
            catch (Exception ex)
            {
                // 로깅 로직은 Serilog 도입 후 처리
                Console.WriteLine($"[DB Error] {ex.Message}");
            }
        }
    }
}

