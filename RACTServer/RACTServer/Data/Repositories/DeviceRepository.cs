using Dapper;
using RACTCommonClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RACTServer.Data.Repositories
{
    /// <summary>
    /// .NET 10 기반 고성능 장비 데이터 레포지토리
    /// </summary>
    public class DeviceRepository
    {
        public async Task<IEnumerable<dynamic>> GetDeviceInfoAsync(string centerCode)
        {
            using var conn = GlobalClass.GetSqlConnection();
            return await conn.QueryAsync(SqlStatements.Device.GetDeviceInfo, new { CenterCode = centerCode });
        }

        public async Task<IEnumerable<dynamic>> SearchDeviceInfoAsync(string centerCode, DeviceSearchInfo searchInfo, string mangType)
        {
            string tORG1 = "", tORG2 = "", tBranch = "";
            if (searchInfo.SelectFACTGroupInfo != null && !searchInfo.SelectFACTGroupInfo.ORG1Code.Equals("0"))
            {
                tORG1 = searchInfo.SelectFACTGroupInfo.ORG1Code;
                tORG2 = searchInfo.SelectFACTGroupInfo.BranchCode;
                tBranch = searchInfo.SelectFACTGroupInfo.CenterCode;
            }

            var param = new
            {
                CenterCode = centerCode,
                IP = searchInfo.DeviceIPAddress,
                Name = searchInfo.DeviceName,
                Part = searchInfo.DevicePart,
                Model = searchInfo.DeviceModel,
                ModelName = searchInfo.ModelName,
                Center = tBranch,
                Branch = tORG2,
                ORG1 = tORG1,
                TpoName = searchInfo.TpoName,
                IPType = searchInfo.IPTyep,
                MangType = mangType
            };

            using var conn = GlobalClass.GetSqlConnection();
            return await conn.QueryAsync(SqlStatements.Device.GetSearchDeviceInfo, param);
        }

        public async Task ExecuteModifyDeviceAsync(E_WorkType workType, int userId, DeviceInfo deviceInfo)
        {
            using var conn = GlobalClass.GetSqlConnection();
            
            var param = new
            {
                WorkType = (int)workType,
                GroupID = deviceInfo.GroupID,
                UserID = userId,
                DeviceID = deviceInfo.DeviceID,
                Protocol = (int)deviceInfo.TerminalConnectInfo.ConnectionProtocol,
                DeviceType = (int)deviceInfo.DeviceType,
                TelnetPort = deviceInfo.TerminalConnectInfo.TelnetPort,
                PortName = deviceInfo.TerminalConnectInfo.SerialConfig.PortName,
                BaudRate = deviceInfo.TerminalConnectInfo.SerialConfig.BaudRate,
                DataBits = deviceInfo.TerminalConnectInfo.SerialConfig.DataBits,
                Parity = (int)deviceInfo.TerminalConnectInfo.SerialConfig.Parity,
                StopBits = (int)deviceInfo.TerminalConnectInfo.SerialConfig.StopBits,
                Handshake = (int)deviceInfo.TerminalConnectInfo.SerialConfig.Handshake,
                ModelName = deviceInfo.DeviceType == E_DeviceType.NeGroup ? "" : deviceInfo.ModelName,
                IPAddress = deviceInfo.DeviceType == E_DeviceType.NeGroup ? "" : deviceInfo.IPAddress,
                TelnetID1 = deviceInfo.DeviceType == E_DeviceType.NeGroup ? "" : deviceInfo.TelnetID1,
                TelnetPwd1 = deviceInfo.DeviceType == E_DeviceType.NeGroup ? "" : deviceInfo.TelnetPwd1,
                TelnetID2 = deviceInfo.DeviceType == E_DeviceType.NeGroup ? "" : deviceInfo.TelnetID2,
                TelnetPwd2 = deviceInfo.DeviceType == E_DeviceType.NeGroup ? "" : deviceInfo.TelnetPwd2,
                Name = deviceInfo.DeviceType == E_DeviceType.NeGroup ? "" : deviceInfo.Name,
                TpoName = deviceInfo.DeviceType == E_DeviceType.NeGroup ? "" : deviceInfo.TpoName,
                WAIT = deviceInfo.DeviceType == E_DeviceType.NeGroup ? "" : deviceInfo.WAIT,
                USERID = deviceInfo.DeviceType == E_DeviceType.NeGroup ? "" : deviceInfo.USERID,
                PWD = deviceInfo.DeviceType == E_DeviceType.NeGroup ? "" : deviceInfo.PWD,
                USERID2 = deviceInfo.DeviceType == E_DeviceType.NeGroup ? "" : deviceInfo.USERID2,
                PWD2 = deviceInfo.DeviceType == E_DeviceType.NeGroup ? "" : deviceInfo.PWD2,
                MoreString = deviceInfo.MoreString,
                MoreMark = deviceInfo.MoreMark
            };

            await conn.ExecuteAsync(SqlStatements.Device.ModifyDeviceInfo, param);
        }

        public async Task<int> CountDeviceAsync(string centerCode)
        {
            using var conn = GlobalClass.GetSqlConnection();
            var count = await conn.ExecuteScalarAsync<int?>(SqlStatements.Device.CountDevice, new { CenterCode = centerCode });
            return count ?? 0;
        }
    }
}
