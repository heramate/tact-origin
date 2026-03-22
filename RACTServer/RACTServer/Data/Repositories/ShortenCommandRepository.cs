using Dapper;
using RACTCommonClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RACTServer.Data.Repositories
{
    public class ShortenCommandRepository
    {
        public async Task<int> ModifyShortenCommandAsync(E_WorkType workType, int userId, ShortenCommandInfo info)
        {
            using var conn = GlobalClass.GetSqlConnection();
            var result = await conn.QueryFirstOrDefaultAsync("EXEC SP_RACT_Modify_ShortenCommand @WorkType, @ID, @GroupID, @UserID, @Name, @Command, @Description", new
            {
                WorkType = (int)workType,
                ID = info.ID,
                GroupID = info.GroupID,
                UserID = userId,
                Name = info.Name,
                Command = info.Command,
                Description = info.Description
            });
            return Convert.ToInt32((result as IDictionary<string, object>)?["ID"]);
        }

        public async Task<(IEnumerable<ShortenCommandGroupInfo> groups, IEnumerable<ShortenCommandInfo> commands)> GetShortenCommandsAsync(int userId)
        {
            using var conn = GlobalClass.GetSqlConnection();
            await conn.OpenAsync();
            using var multi = await conn.QueryMultipleAsync("EXEC SP_RACT_Get_ShortenCommand @UserID", new { UserID = userId });
            var groups = await multi.ReadAsync<ShortenCommandGroupInfo>();
            var commands = await multi.ReadAsync<ShortenCommandInfo>();
            return (groups, commands);
        }

        public async Task<int> ModifyShortenCommandGroupAsync(E_WorkType workType, int userId, ShortenCommandGroupInfo info)
        {
            using var conn = GlobalClass.GetSqlConnection();
            var result = await conn.QueryFirstOrDefaultAsync("EXEC SP_RACT_Modify_ShortenCommandGroup @WorkType, @ID, @UserID, @Name, @Description", new
            {
                WorkType = (int)workType,
                ID = info.ID,
                UserID = userId,
                Name = info.Name,
                Description = info.Description
            });
            return Convert.ToInt32((result as IDictionary<string, object>)?["ID"]);
        }
    }
}
