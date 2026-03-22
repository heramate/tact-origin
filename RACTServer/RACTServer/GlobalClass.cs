using RACTCommonClass;
using RACTServerCommon;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading;

namespace RACTServer
{
    public static class GlobalClass
    {
        public static bool m_IsRequestToStop = false;
        public const string c_Version = "1.1.0.0"; 
        public static string m_StartupPath = string.Empty;
        public const string c_SystemConfigFileName = @"SystemInfo.xml";
        public static SystemConfig m_SystemInfo = null;
        public static DBConnectionInfo m_DBConnectionInfo = null;

        public static ClientCommunicationProcess m_ClientProcess = null;
        public static ModelInfoCollection m_ModelInfoCollection = null;
        public static LimitCmdInfoCollection m_LimitCmdInfoCollection = null;
        public static DefaultCmdInfoCollection m_DefaultCmdInfoCollection = null;
        public static AutoCompleteCmdInfoCollection m_AutoCompleteCmdInfoCollection = null;

        public static FileLogProcess m_LogProcess = null;
        public static DBLogProcess m_DBLogProcess = null;
        public static bool m_IsRun = false;
        public static FACTGroupInfo m_FACTGroupInfo;
        public static DaemonProcessManager s_DaemonProcessManager;
        public static ServiceManagerCommunicationProcess s_ServiceManagerCommunicationProcess = null;
        public static DeviceConnectionLogService s_DeviceConnectionLogService = null;

        public static readonly int s_HealthCheckTimeOut = 30;
        public static int s_UnUsedLimit = 0;

        /// <summary>
        /// 쓰레드를 안전하게 대기 후 종료를 시도합니다.
        /// </summary>
        public static void StopThread(Thread aThread)
        {
            if (aThread == null) return;
            try
            {
                if (aThread.IsAlive)
                {
                    aThread.Join(500);
                    if (aThread.IsAlive) aThread.Abort();
                }
            }
            catch { }
            finally { aThread = null; }
        }

        public static void SendResultClient(ResultCommunicationData aResult)
        {
            if (m_ClientProcess != null)
                m_ClientProcess.SendResultClient(aResult);
        }

        /// <summary>
        /// 객체를 압축하여 CompressData 형태로 반환합니다.
        /// </summary>
        public static CompressData ObjectCompress(object aValue)
        {
            byte[] tBytes = ObjectConverter.GetBytes(aValue);
            using (MemoryStream ms = new MemoryStream())
            {
                using (GZipStream gs = new GZipStream(ms, CompressionMode.Compress, true))
                {
                    gs.Write(tBytes, 0, tBytes.Length);
                }
                return new CompressData(tBytes.Length, new MemoryStream(ms.ToArray()));
            }
        }

        private static string s_CachedConnectionString = null;

        /// <summary>
        /// DB 연결 객체를 생성합니다. (Max Pool Size=200 적용)
        /// </summary>
        public static System.Data.SqlClient.SqlConnection GetSqlConnection()
        {
            if (m_SystemInfo == null) return null;

            if (s_CachedConnectionString == null)
            {
                s_CachedConnectionString = string.Format(
                    "Data Source={0};Initial Catalog={1};User ID={2};Password={3};Max Pool Size=200;Min Pool Size=10;Connect Timeout=30;",
                    m_SystemInfo.DBServerIP, m_SystemInfo.DBName, m_SystemInfo.UserID, m_SystemInfo.Password);
            }

            return new System.Data.SqlClient.SqlConnection(s_CachedConnectionString);
        }
    }
}
