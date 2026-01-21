using System;
using System.Collections.Generic;
using System.Text;
using MKLibrary.MKData;
using System.Threading;
using System.Collections;
using RACTCommonClass;

namespace RACTServerCommon
{
    /// <summary>
    /// DB 로그처리 클래스 입니다.
    /// </summary>
    public class DBLogProcess : IDisposable
    {
        /// <summary>
        /// 로그파일의 저장 위치 입니다.
        /// </summary>
        private string m_LogPath = "";
        /// <summary>
        /// Log그를 저장할 파일로그 객체 입니다.
        /// </summary>
        public FileLogProcess m_Log = null;
        /// <summary>
        /// Snmp로그를 저장할 파일 이름 입니다.
        /// </summary>
        private const string c_LogName = "DBLogProcess";
        /// <summary>
        /// DB로그를 기록하기 위한 DBPool 객체 입니다.
        /// </summary>
        private MKOleDBPool m_DBPool = null;
        /// <summary>
        /// TMS에서 발생한 로그를 저장할 큐 입니다.
        /// </summary>
        private Queue<DBLogInfo> m_LogQueue = null;
        /// <summary>
        /// TMS에서 발생한 로그기록을 처리할  스래드 입니다.
        /// </summary>
        private Thread m_LogQueueProcessThread = null;
        /// <summary>
        /// m_LogQueueProcessThread 스래드 종료 요청 여부 입니다.
        /// </summary>
        private bool m_isLogQueueProcessThreadShutDown = false;

        /// <summary>
        /// 기본 생성자
        /// </summary>
        public DBLogProcess(MKOleDBPool aDBPool, string aStartupPath)
        {

            m_LogPath = aStartupPath + "\\log\\"; //
            m_Log = new FileLogProcess(m_LogPath, c_LogName);

            m_DBPool = aDBPool;
            m_LogQueue = new Queue<DBLogInfo>();

            m_isLogQueueProcessThreadShutDown = false;
            m_LogQueueProcessThread = new Thread(new ThreadStart(LogWriteProcess));
            m_LogQueueProcessThread.Start();

        }
        /// <summary>
        /// 데몬을 파괴 합니다.
        /// </summary>
        public void Dispose()
        {
            m_isLogQueueProcessThreadShutDown = true;

            if (m_LogQueueProcessThread != null)
            {
                //스래드가 종료 되도록 잠시 기다립니다.
                m_LogQueueProcessThread.Join(100);

                //스래드를 종료 합니다.
                if (m_LogQueueProcessThread.IsAlive)
                {
                    try
                    {
                        //트랩 처리 스래드를 강제 종료 합니다.
                        m_LogQueueProcessThread.Abort();
                    }
                    catch (Exception ex)
                    {
                        m_Log.PrintLog(E_FileLogType.Infomation, "[E]~DBLogProcess() : " + ex.ToString());
                    }
                }
            }
            m_Log.Stop();
        }


        /// <summary>
        /// 로그처리 스래드 처리 함수 입니다.
        /// </summary>
        private void LogWriteProcess()
        {
            DBLogInfo tDBLog = null;
            DBUserLogInfo tUserLogInfo = null;
            DBExecuteCommandLogInfo tExecLogInfo = null;
            string tQueryString ="";

            while (!m_isLogQueueProcessThreadShutDown)
            {
                try
                {
                    Thread.Sleep(1);
                    lock (m_LogQueue)
                    {
                        if (m_LogQueue.Count <= 0) continue;
                    }

                    tDBLog = m_LogQueue.Dequeue();
                    if (tDBLog == null) continue;

                    switch (tDBLog.LogType)
                    {
                        case E_DBLogType.SystemLog:
                            break;
                        case E_DBLogType.DeviceConnectionLog:
                            break;
                        case E_DBLogType.ExecuteCommandLog:
                            tExecLogInfo = (DBExecuteCommandLogInfo)tDBLog;
                            tQueryString = "INSERT INTO [dbo].[RACT_Log_ExcuteCommand]([DateTime],[ConnectionLogId],[Command],[IsEmbargoCmd])VALUES('{0}',{1},'{2}','{3}')";
                            tQueryString = string.Format(tQueryString, tExecLogInfo.DateTime.ToString("yyyy-MM-dd HH:mm:ss"), tExecLogInfo.ConnectionLogID, tExecLogInfo.Command , tExecLogInfo.IsLimitCmd);
                            ExcuteNoneQuery(tQueryString);

                            if (tExecLogInfo.IsLimitCmd)
                            {
                                string tQueryMessage = string.Format("update RACT_LOG_DeviceConnection set EmbargoCmdUsed = 1 where id ={0}", tExecLogInfo.ConnectionLogID);
                                ExcuteNoneQuery(tQueryMessage);
                            }
                            break;
                        case E_DBLogType.LoginLog:
                            tUserLogInfo = (DBUserLogInfo)tDBLog;
                            tQueryString = "INSERT INTO [dbo].[RACT_LOG_Login] ([DateTime],[UserID],[LogType],[Description])VALUES('{0}',{1},{2},'{3}')";
                            tQueryString = string.Format(tQueryString, tUserLogInfo.DateTime.ToString("yyyy-MM-dd HH:mm:ss"), tUserLogInfo.UserID, (int)tUserLogInfo.UserLogType, tUserLogInfo.Message);
                            ExcuteNoneQuery(tQueryString);
                            break;
                        default:
                            break;
                    }
                    tDBLog = null;

                }
                catch (Exception ex)
                {
                    m_Log.PrintLog("[E]LogWriteProcess : " + ex.ToString());
                }
            }
        }

        /// <summary>
        /// 쿼리를 실행 합니다.
        /// </summary>
        /// <param name="aQueryString"></param>
        /// <returns></returns>
        private void ExcuteNoneQuery(string aQueryString)
        {
            MKDBWorkItem tDBWI = null;
            try
            {
                tDBWI = m_DBPool.GetDBWorkItem();
                if (tDBWI.ExecuteNoneQuery(aQueryString) != E_DBProcessError.Success)
                {
                    m_Log.PrintLog("[E]Query Error : " + aQueryString);
                }
            }
            catch (Exception ex)
            {
                m_Log.PrintLog("[E]LogWriteProcess : " + ex.ToString());
            }
        }

        /// <summary>
        /// 쿼리를 실행 합니다.
        /// </summary>
        /// <param name="aQueryString"></param>
        /// <returns></returns>
        public int ExcuteQuery(string aQueryString)
        {
            MKDBWorkItem tDBWI = null;
            MKDataSet tDataSet = null;
            int tID = -1;
            try
            {
                tDBWI = m_DBPool.GetDBWorkItem();
                if (tDBWI.ExecuteQuery(aQueryString, out tDataSet) != E_DBProcessError.Success)
                {
                    m_Log.PrintLog("[E]Query Error : " + aQueryString);
                }
                else
                {
                    tID = tDataSet.GetInt32("ID");
                }
            }
            catch (Exception ex)
            {
                m_Log.PrintLog("[E]LogWriteProcess : " + ex.ToString());
            }
            return tID;
        }

        /// <summary>
        /// 로그를 저장 합니다.
        /// </summary>
        /// <param name="aLogInfo"></param>
        public void AddLog(DBLogInfo aLogInfo)
        {
            lock (m_LogQueue)
            {
                m_LogQueue.Enqueue(aLogInfo);
            }
        }

    }
}
