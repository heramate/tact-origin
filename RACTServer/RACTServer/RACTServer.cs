using RACTCommonClass;
using RACTServerCommon;
using System;
using System.Collections;
using System.IO;
using MKLibrary.MKData;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RACTServer
{
    public class RACTServer
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private Task _reloadCheckTask = null;
        private Task _startServerTask = null;

        /// <summary>
        /// 타임아웃된 사용자를 자동으로 삭제처리 할지 여부 입니다. ( 디버깅시 클라이언트 세션을 자동으로 제거하면 디버깅이 안되기 때문)
        /// </summary>
        private bool m_IsTimeoutUserAutoDelete = false;

        public RACTServer(string aStartupPath) : this(aStartupPath, true) { }
        public RACTServer(string aStartupPath, bool aIsTimeoutUserAutoDelete)
        {
            GlobalClass.m_StartupPath = aStartupPath;
            m_IsTimeoutUserAutoDelete = aIsTimeoutUserAutoDelete;
        }

        /// <summary>
        /// 서버를 시작합니다.
        /// </summary>
        public bool Start()
        {
            try
            {
                GlobalClass.m_LogProcess = new FileLogProcess(Path.Combine(Application.StartupPath, "System Log"), "ServerSystem");
                GlobalClass.m_LogProcess.Start();

                GlobalClass.s_DeviceConnectionLogService = new DeviceConnectionLogService();

                if (!InitializeServer()) return false;

                GlobalClass.m_IsRun = true;
                _cts = new CancellationTokenSource();

                // 서버 구동 백그라운드 태스크
                _startServerTask = Task.Run(() => StartServerAsync());

                // 주기적 데이터 갱신 태스크
                _reloadCheckTask = Task.Run(() => ProcessReloadCheckAsync(_cts.Token));

                return true;
            }
            catch (Exception ex)
            {
                GlobalClass.m_LogProcess?.PrintLog(E_FileLogType.Error, "Server Start Failed: " + ex.Message);
                return false;
            }
        }

        private async Task ProcessReloadCheckAsync(CancellationToken token)
        {
            GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "데이터 재로딩 체크 프로세스를 시작합니다.");
            
            while (!token.IsCancellationRequested && GlobalClass.m_IsRun)
            {
                try
                {
                    DateTime now = DateTime.Now;
                    if (now.Hour == GlobalClass.m_SystemInfo.ReloadHour && now.Minute == GlobalClass.m_SystemInfo.ReloadMinute)
                    {
                        GlobalClass.m_LogProcess.PrintLog("==============================기초 데이터를 새로 올리기 시작합니다.==============================");
                        
                        if (!BaseDataLoadProcess.LoadBaseData())
                        {
                            GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Warning, "기초 데이터 로드에 실패하였습니다.");
                        }

                        // 1분 동안 중복 실행 방지
                        await Task.Delay(60000, token);
                    }
                }
                catch (OperationCanceledException) { break; }
                catch (Exception ex)
                {
                    GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, "ProcessReloadCheckAsync exception : " + ex.Message);
                }
                
                await Task.Delay(1000, token);
            }
        }

        /// <summary>
        /// 서버를 종료합니다.
        /// </summary>
        public void Stop()
        {
            GlobalClass.m_IsRun = false;
            _cts.Cancel();

            GlobalClass.m_LogProcess?.PrintLog(E_FileLogType.Infomation, "서버 종료 절차를 시작합니다.");

            /*
            if (GlobalClass.s_ServiceManagerCommunicationProcess != null)
            {
                GlobalClass.s_ServiceManagerCommunicationProcess.Stop();
                GlobalClass.s_ServiceManagerCommunicationProcess = null;
            }

            if (GlobalClass.s_DaemonProcessManager != null)
            {
                GlobalClass.s_DaemonProcessManager.Stop();
                GlobalClass.s_DaemonProcessManager = null;
            }
            */

            if (GlobalClass.m_ClientProcess != null)
            {
                GlobalClass.m_ClientProcess.Stop();
                GlobalClass.m_ClientProcess = null;
            }

            try
            {
                // 실행 중인 태스크들 대기
                Task.WaitAll(new[] { _reloadCheckTask, _startServerTask }.FilterNotNull(), 3000);
            }
            catch { }

            if (GlobalClass.m_DBLogProcess != null)
            {
                GlobalClass.m_DBLogProcess.Dispose();
                GlobalClass.m_DBLogProcess = null;
            }

            if (GlobalClass.m_LogProcess != null)
            {
                GlobalClass.m_LogProcess.Stop();
                GlobalClass.m_LogProcess = null;
            }
        }

        public bool LoadSystemInfo()
        {
            try
            {
                string configPath = Path.Combine(GlobalClass.m_StartupPath, GlobalClass.c_SystemConfigFileName.TrimStart('\\'));
                FileInfo tFileInfo = new FileInfo(configPath);
                
                if (!tFileInfo.Exists) MKXML.ObjectToXML(tFileInfo.FullName, new SystemConfig());

                ArrayList tSystemInfos = MKXML.ObjectFromXML(tFileInfo.FullName, typeof(SystemConfig), out E_XmlError tXmlError);
                if (tSystemInfos == null || tSystemInfos.Count == 0) return false;

                GlobalClass.m_SystemInfo = (SystemConfig)tSystemInfos[0];
                GlobalClass.m_DBConnectionInfo = new DBConnectionInfo
                {
                    DBServerIP = GlobalClass.m_SystemInfo.DBServerIP,
                    DBName = GlobalClass.m_SystemInfo.DBName,
                    UserID = GlobalClass.m_SystemInfo.UserID,
                    Password = GlobalClass.m_SystemInfo.Password,
                    DBConnectionCount = GlobalClass.m_SystemInfo.DBConnectionCount
                };

                return true;
            }
            catch (Exception ex)
            {
                GlobalClass.m_LogProcess?.PrintLog(E_FileLogType.Error, "LoadSystemInfo Failed: " + ex.Message);
                return false;
            }
        }

        public bool MakeSystemInfo()
        {
            try
            {
                IPAddress[] addressList = Dns.GetHostEntry(Environment.MachineName).AddressList;
                SystemConfig tSystemInfo = new SystemConfig
                {
                    ServerID = 0,
                    ServerIP = addressList.Length > 0 ? addressList[0].ToString() : "127.0.0.1",
                    DBServerIP = Environment.MachineName + ",43218\\FACT_TEST",
                    DBName = "FACT_TEST",
                    UserID = "sa",
                    Password = "factskB~2012",
                    ServerPort = 54321,
                    ServerChannel = "RemoteClient"
                };

                string configPath = Path.Combine(GlobalClass.m_StartupPath, GlobalClass.c_SystemConfigFileName.TrimStart('\\'));
                return MKXML.ObjectToXML(configPath, tSystemInfo) == E_XmlError.Success;
            }
            catch (Exception ex)
            {
                GlobalClass.m_LogProcess?.PrintLog(E_FileLogType.Error, "MakeSystemInfo Failed: " + ex.Message);
                return false;
            }
        }

        private bool InitializeServer()
        {
            GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "서버 초기화를 진행합니다.");
            if (!LoadSystemInfo()) return false;
            GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "Dapper 기반 고성능 비동기 파이프라인이 활성화되었습니다.");
            return true;
        }

        private void StartServerAsync()
        {
            if (!BaseDataLoadProcess.LoadBaseData())
            {
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, "기초 정보 로드 실패로 서버 구동을 중단합니다.");
                return;
            }

            RemoteServerStart();
            GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "서버 서비스가 성공적으로 시작되었습니다.");
        }

        private void RemoteServerStart()
        {
            try
            {
                GlobalClass.m_ClientProcess = new ClientCommunicationProcess();
                GlobalClass.m_ClientProcess.Start();

                //GlobalClass.s_DaemonProcessManager = new DaemonProcessManager();
                //GlobalClass.s_DaemonProcessManager.Start();

                //GlobalClass.s_ServiceManagerCommunicationProcess = new ServiceManagerCommunicationProcess();
                //GlobalClass.s_ServiceManagerCommunicationProcess.Start();
            }
            catch (Exception ex)
            {
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, "RemoteServerStart Error: " + ex.Message);
            }
        }
    }

    internal static class TaskExtensions
    {
        public static Task[] FilterNotNull(this Task[] tasks)
        {
            return Array.FindAll(tasks, t => t != null);
        }
    }
}
