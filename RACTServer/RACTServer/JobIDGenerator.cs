using System;
using System.Collections.Generic;
using System.Text;

using MKLibrary.MKData;
using RACTCommonClass;

namespace RACTServer
{
    public class JobIDGenerator
    {
        /// <summary>
        /// 현재의 JobID 입니다.
        /// </summary>
        private long m_JobID = 0;

        /// <summary>
        /// JobID생성 객체를 초기화 합니다.
        /// </summary>
        /// <returns>초기화 작업의 성공여부 입니다.</returns>
        public bool Initialize()
        {

            DateTime tSystemDate = DateTime.Now;
            MKDBWorkItem tDBWI = null;
            string tQueryMessage = "select JobID from SVR_JOBID where SvrID = " + GlobalClass.m_SystemInfo.ServerID.ToString();
            MKDataSet tDataSet = null;

            try
            {
                tQueryMessage = string.Concat("select JobID from SVR_JOBID where SvrID = ", GlobalClass.m_SystemInfo.ServerID);

                tDBWI = GlobalClass.m_DBPool.GetDBWorkItem();
                tDBWI.ExecuteQuery(tQueryMessage, out tDataSet);

                if (tDataSet.RecordCount > 0)
                {
                    m_JobID = long.Parse(tDataSet["JobID"].ToString());
                }
                if (tDataSet != null) MKOleDBClass.CloseDataSet(tDataSet);

                if (m_JobID > 0) return true;

                tDBWI = GlobalClass.m_DBExecutePool.GetDBWorkItem();
                tDBWI.ExecuteNoneQuery(string.Format("Insert into SVR_JOBID ( JobID, SvrID ) values ({0},{1});", m_JobID, GlobalClass.m_SystemInfo.ServerID));
            }
            catch (Exception ex)
            {
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, ex.ToString());
            }
            finally
            {
                if (tDataSet != null) MKOleDBClass.CloseDataSet(tDataSet);
            }
            return true;
        }

        /// <summary>
        /// Job ID를 얻기 합니다.
        /// </summary>
        /// <returns></returns>
        public long GetJobID()
        {
            //long tID = 0;
            MKDBWorkItem tDBWI = GlobalClass.m_DBExecutePool.GetDBWorkItem();

            lock (this)
            {
                m_JobID++;
                //tID = m_JobID;
                tDBWI.ExecuteNoneQuery(string.Format("UPDATE SVR_JOBID SET JobID ={1} where SvrID = {0};", GlobalClass.m_SystemInfo.ServerID, m_JobID));
            }
            return m_JobID;
        }
    }
}
