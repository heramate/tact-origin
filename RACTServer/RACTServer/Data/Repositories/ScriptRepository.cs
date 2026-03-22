using Dapper;
using RACTCommonClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RACTServer.Data.Repositories
{
    public class ScriptRepository
    {
        public async Task<int> ModifyScriptGroupAsync(E_WorkType workType, int userId, ScriptGroupInfo info)
        {
            using var conn = GlobalClass.GetSqlConnection();
            var result = await conn.QueryFirstOrDefaultAsync("EXEC SP_RACT_Modify_ScriptGroup @WorkType, @ID, @UserID, @Name, @Description", new
            {
                WorkType = (int)workType,
                ID = info.ID,
                UserID = userId,
                Name = info.Name,
                Description = info.Description
            });
            return Convert.ToInt32((result as IDictionary<string, object>)?["ID"]);
        }

        public async Task<(IEnumerable<ScriptGroupInfo> groups, IEnumerable<dynamic> scripts)> GetScriptsAsync(int userId)
        {
            using var conn = GlobalClass.GetSqlConnection();
            await conn.OpenAsync();
            using var multi = await conn.QueryMultipleAsync("EXEC SP_RACT_Get_Script @UserID", new { UserID = userId });
            var groups = await multi.ReadAsync<ScriptGroupInfo>();
            var scripts = await multi.ReadAsync<dynamic>();
            return (groups, scripts);
        }

        public async Task<int> ModifyScriptInfoAsync(E_WorkType workType, int userId, Script info)
        {
            using var conn = GlobalClass.GetSqlConnection();
            var result = await conn.QueryFirstOrDefaultAsync("EXEC SP_RACT_Modify_ScriptInfo @WorkType, @ID, @GroupID, @UserID, @Name, @Description, @RawScript", new
            {
                WorkType = (int)workType,
                ID = info.ID,
                GroupID = info.GroupID,
                UserID = userId,
                Name = info.Name,
                Description = info.Description,
                RawScript = info.RawScript
            });
            return Convert.ToInt32((result as IDictionary<string, object>)?["ID"]);
        }
    }
}
