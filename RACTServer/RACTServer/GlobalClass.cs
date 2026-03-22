using RACTCommonClass;
using RACTServerCommon;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

namespace RACTServer
{
    /// <summary>
    /// .NET 10 기반 전역 설정 및 유틸리티
    /// </summary>
    public static class GlobalClass
    {
        public static bool m_IsRequestToStop = false;
        public const string c_Version = "2.0.0.0 (.NET 10)"; 
        public static string m_StartupPath = string.Empty;
        public const string c_SystemConfigFileName = @"SystemInfo.xml";
        public static SystemConfig? m_SystemInfo = null;
        public static DBConnectionInfo? m_DBConnectionInfo = null;

        public static ClientCommunicationProcess? m_ClientProcess = null;
        public static ModelInfoCollection? m_ModelInfoCollection = null;
        public static LimitCmdInfoCollection? m_LimitCmdInfoCollection = null;
        public static DefaultCmdInfoCollection? m_DefaultCmdInfoCollection = null;
        public static AutoCompleteCmdInfoCollection? m_AutoCompleteCmdInfoCollection = null;

        public static FileLogProcess? m_LogProcess = null;
        public static DBLogProcess? m_DBLogProcess = null;
        public static bool m_IsRun = false;
        public static FACTGroupInfo? m_FACTGroupInfo;
        public static DaemonProcessManager? s_DaemonProcessManager;
        public static ServiceManagerCommunicationProcess? s_ServiceManagerCommunicationProcess = null;
        public static DeviceConnectionLogService? s_DeviceConnectionLogService = null;
        public static ClientResponseProcess? m_ClientResponseProcess = null;

        // Repositories
        public static Data.Repositories.DeviceRepository DeviceRepo { get; } = new();
        public static Data.Repositories.GroupRepository GroupRepo { get; } = new();
        public static Data.Repositories.ScriptRepository ScriptRepo { get; } = new();
        public static Data.Repositories.ShortenCommandRepository ShortenCommandRepo { get; } = new();

        public static readonly int s_HealthCheckTimeOut = 30;
        public static int s_UnUsedLimit = 0;

        /// <summary>
        /// .NET 10 최적화: 객체를 압축하여 CompressData 형태로 반환합니다. (Memory 효율성 개선)
        /// </summary>
        public static CompressData ObjectCompress(object aValue)
        {
            byte[] bytes = ObjectConverter.GetBytes(aValue);
            
            // .NET 10에서는 RecyclableMemoryStream 또는 ArrayPool을 고려할 수 있으나 기본 Stream 최적화 우선 적용
            using var ms = new MemoryStream();
            using (var gs = new GZipStream(ms, CompressionLevel.Optimal, true))
            {
                gs.Write(bytes, 0, bytes.Length);
            }
            
            return new CompressData(bytes.Length, new MemoryStream(ms.ToArray()));
        }

        private static string? s_CachedConnectionString = null;

        /// <summary>
        /// .NET 10 최적화: DB 연결 문자열 캐싱 및 고성능 연결 풀링 설정
        /// </summary>
        public static System.Data.SqlClient.SqlConnection GetSqlConnection()
        {
            if (m_SystemInfo == null) throw new InvalidOperationException("SystemInfo is not initialized.");

            s_CachedConnectionString ??= $"Data Source={m_SystemInfo.DBServerIP};Initial Catalog={m_SystemInfo.DBName};User ID={m_SystemInfo.UserID};Password={m_SystemInfo.Password};Max Pool Size=500;Min Pool Size=20;Connect Timeout=30;Application Name=RACTServer_NET10;";

            return new System.Data.SqlClient.SqlConnection(s_CachedConnectionString);
        }

        public static void SendResultClient(ResultCommunicationData aResult)
        {
            m_ClientProcess?.SendResultClient(aResult);
        }

        public static void StopThread(Thread? aThread)
        {
            if (aThread == null) return;
            try
            {
                if (aThread.IsAlive)
                {
                    aThread.Join(500);
                    // .NET 10에서는 Thread.Abort()가 지원되지 않으므로(PlatformNotSupported) Interrupt로 대체하거나 플래그 사용 유도
                    aThread.Interrupt(); 
                }
            }
            catch { }
        }
    }
}
