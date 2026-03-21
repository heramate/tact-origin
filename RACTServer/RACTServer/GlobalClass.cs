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
        public const string c_SystemConfigFileName = @"\SystemInfo.xml";
        public static SystemConfig m_SystemInfo = null;
        public static DBConnectionInfo m_DBConnectionInfo = null;
        /// <summary>
        /// JOB ID 생성 클래스 입니다.
        /// </summary>
       // public static JobIDGenerator m_JobIDGenerator = null;
        /// <summary>
        /// 클라이언트와 통신할 클래스 입니다.
        /// </summary>
        public static ClientCommunicationProcess m_ClientProcess = null;
        /// <summary>
        /// 모델 목록 입니다.
        /// </summary>
        public static ModelInfoCollection m_ModelInfoCollection = null;

        /// <summary>
        /// 15-09-10 Gunny 제한 명령어 목록 입니다.
        /// </summary>
        public static LimitCmdInfoCollection m_LimitCmdInfoCollection = null;

        /// <summary>
        /// 15-09-30 Gunny 기본 명령어 목록 입니다.
        /// </summary>
        public static DefaultCmdInfoCollection m_DefaultCmdInfoCollection = null;

        /// <summary>
        /// 15-09-30 Gunny 기본 명령어 목록 입니다.
        /// </summary>
        public static AutoCompleteCmdInfoCollection m_AutoCompleteCmdInfoCollection = null;

        /// <summary>
        /// 2013-05-02 - shinyn - 디바이스정보 목록입니다.
        /// </summary>
        //public static DeviceInfoCollection m_DeviceInfos = null;

        /// <summary>
        /// 로그 저장 프로세서 입니다.
        /// </summary>
        public static FileLogProcess m_LogProcess = null;
        /// <summary>
        /// DB 로그 저장 프로세서 입니다.
        /// </summary>
        public static DBLogProcess m_DBLogProcess = null;
        /// <summary>
        /// 서버 실행 여부 입니다.
        /// </summary>
        public static bool m_IsRun = false;
        /// <summary>
        /// FACT 그룹 정보 입니다.
        /// </summary>
        public static FACTGroupInfo m_FACTGroupInfo;
        /// <summary>
        /// 데몬 관리자 입니다.
        /// </summary>
        public static DaemonProcessManager s_DaemonProcessManager;
        /// <summary>
        /// 서버와 통신할 프로세서 입니다.
        /// </summary>
        public static ServiceManagerCommunicationProcess s_ServiceManagerCommunicationProcess = null;
        /// <summary>
        /// 세션 관리 타임 아웃 시간 입니다.
        /// </summary>
#if DEBUG
        public static readonly int s_HealthCheckTimeOut = 30;
#else
        public static readonly int s_HealthCheckTimeOut = 30;
#endif
        /// <summary>
        /// 마지막 로그인 이후 제한 기간 입니다.(Day)
        /// </summary>
        public static int s_UnUsedLimit = 0;

        public static DeviceConnectionLogService s_DeviceConnectionLogService = null;


        /// <summary>
        /// 쓰레드를 강제 종료합니다.
        /// </summary>
        /// <param name="aThread"></param>
        public static void StopThread(Thread aThread)
        {
            if (aThread != null)
            {
                aThread.Join(100);
                if (aThread.IsAlive)
                {
                    try
                    {
                        aThread.Abort();
                    }
                    catch (Exception) { }
                }
                aThread = null;
            }
        }

        /// <summary>
        /// 지정한 클라이언트에 결과를 전송합니다.
        /// </summary>		
        /// <param name="aResult">결과 데이터 입니다.</param>
        public static void SendResultClient(ResultCommunicationData aResult)
        {
            if (m_ClientProcess != null)
                m_ClientProcess.SendResultClient(aResult);
            else
                aResult = null;
        }

        /// <summary>
        /// 객체를 압축한 메모리스트림을 반환 합니다.
        /// </summary>
        /// <param name="aValue">압축할 객체 입니다.</param>
        /// <returns>메모리 스트림 입니다.</returns>
        public static CompressData ObjectCompress(object aValue)
        {
            byte[] tBytes = ObjectConverter.GetBytes(aValue);
            MemoryStream tMemoryStream = new MemoryStream();
            GZipStream tGZipStream = new GZipStream(tMemoryStream, CompressionMode.Compress, true);
            tGZipStream.Write(tBytes, 0, tBytes.Length);
            tGZipStream.Close();
            CompressData tCompressData = new CompressData(tBytes.Length, tMemoryStream);
            return tCompressData;
        }

        private static string s_CachedConnectionString = null;

        /// <summary>
        /// Native ADO.NET Connection 객체를 생성하여 반환합니다.
        /// 커넥션 풀링(Max Pool Size=200)이 자동으로 적용됩니다.
        /// </summary>
        /// <returns>SqlConnection</returns>
        public static System.Data.SqlClient.SqlConnection GetSqlConnection()
        {
            if (m_SystemInfo == null) return null;

            if (s_CachedConnectionString == null)
            {
                s_CachedConnectionString = string.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};Max Pool Size=200;Min Pool Size=10;Connect Timeout=30;",
                    m_SystemInfo.DBServerIP, m_SystemInfo.DBName, m_SystemInfo.UserID, m_SystemInfo.Password);
            }

            return new System.Data.SqlClient.SqlConnection(s_CachedConnectionString);
        }
    }
}
