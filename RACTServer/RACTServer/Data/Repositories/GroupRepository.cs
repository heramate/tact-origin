using Dapper;
using RACTCommonClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RACTServer.Data.Repositories
{
    public class GroupRepository
    {
        public async Task<string?> ModifyGroupInfoAsync(E_WorkType workType, int userId, GroupInfo groupInfo)
        {
            using var conn = GlobalClass.GetSqlConnection();
            var result = await conn.QueryFirstOrDefaultAsync(SqlStatements.Group.ModifyGroupInfo, new
            {
                WorkType = (int)workType,
                UserID = userId,
                ID = workType == E_WorkType.Add ? null : groupInfo.ID,
                Name = groupInfo.Name,
                Description = groupInfo.Description,
                TOP_ID = groupInfo.TOP_ID,
                UP_ID = groupInfo.UP_ID
            });

            return (result as IDictionary<string, object>)?["ID"]?.ToString();
        }

        public async Task<(IEnumerable<dynamic> groups, IEnumerable<dynamic> devices)> GetGroupInfoWithDevicesAsync(int userId)
        {
            using var conn = GlobalClass.GetSqlConnection();
            await conn.OpenAsync();
            using var multi = await conn.QueryMultipleAsync(SqlStatements.Group.GetGroupInfoByUserId, new { UserID = userId });
            
            var groups = await multi.ReadAsync<dynamic>();
            var devices = await multi.ReadAsync<dynamic>();
            
            return (groups, devices);
        }

        public async Task<IEnumerable<UserInfo>> GetUserListAsync(string searchType, string searchValue, int deleteUserId)
        {
            using var conn = GlobalClass.GetSqlConnection();
            return await conn.QueryAsync<UserInfo>(SqlStatements.User.GetUserList, new 
            { 
                SearchType = searchType, 
                SearchValue = searchValue, 
                DeleteUserID = deleteUserId 
            });
        }

        public async Task<IEnumerable<dynamic>> GetFactGroupTreeAsync(string centerCode)
        {
            using var conn = GlobalClass.GetSqlConnection();
            return await conn.QueryAsync(SqlStatements.Group.SelectFactGroupInfo, new { CenterCode = centerCode });
        }
    }
}
