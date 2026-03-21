using System;
using RACTCommonClass;
using Dapper;

namespace RACTServer
{
    public class JobIDGenerator
    {
        private long m_JobID = 0;

        public bool Initialize()
        {
            try
            {
                using (var conn = GlobalClass.GetSqlConnection())
                {
                    if (conn == null) return false;
                    m_JobID = conn.QueryFirstOrDefault<long>("select JobID from SVR_JOBID where SvrID = @SvrID", new { SvrID = GlobalClass.m_SystemInfo.ServerID });

                    if (m_JobID > 0) return true;

                    conn.Execute("Insert into SVR_JOBID ( JobID, SvrID ) values (@JobID, @SvrID)", new { JobID = m_JobID, SvrID = GlobalClass.m_SystemInfo.ServerID });
                }
            }
            catch (Exception ex)
            {
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, ex.ToString());
            }
            return true;
        }

        public long GetJobID()
        {
            lock (this)
            {
                m_JobID++;
                try
                {
                    using (var conn = GlobalClass.GetSqlConnection())
                    {
                        if (conn != null)
                        {
                            conn.Execute("UPDATE SVR_JOBID SET JobID = @JobID where SvrID = @SvrID", new { JobID = m_JobID, SvrID = GlobalClass.m_SystemInfo.ServerID });
                        }
                    }
                }
                catch (Exception ex)
                {
                    GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, ex.ToString());
                }
            }
            return m_JobID;
        }
    }
}
