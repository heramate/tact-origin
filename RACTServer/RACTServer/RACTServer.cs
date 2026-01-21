using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using System.Collections;
using MKLibrary.MKData;

using System.Net;
using RACTCommonClass;
using System.Windows.Forms;
using RACTServerCommon;
using System.IO;

namespace RACTServer
{
    public class RACTServer
    {
		/// <summary>
		/// 서버를 시작하는 쓰레드입니다.
		/// </summary>
		private Thread m_StartServerThread = null;

        private Thread m_ReloadCheckThread = null;

		/// <summary>
		/// 타임아웃된 사용자를 자동으로 삭제처리 할지 여부 입니다. ( 디버깅시 클라이언트 세션을 자동으로 제거하면 디버깅이 안되기 때문)
		/// </summary>
		private bool m_IsTimeoutUserAutoDelete = false;
		/// <summary>
		/// 서버 개체를 생성합니다.
		/// </summary>
		/// <param name="aStartupPath">시작 경로입니다.</param>
		public RACTServer(string aStartupPath) : this(aStartupPath, true) { }
		/// <summary>
		/// 서버 개체를 생성합니다.
		/// </summary>
		/// <param name="aStartupPath"></param>
		/// <param name="aIsTimeoutUserAutoDelete"></param>
        public RACTServer(string aStartupPath, bool aIsTimeoutUserAutoDelete)
		{
			GlobalClass.m_StartupPath = aStartupPath;
			m_IsTimeoutUserAutoDelete = aIsTimeoutUserAutoDelete;
		}

       

		/// <summary>
		/// 서버를 시작합니다.
		/// </summary>
		/// <returns></returns>
		public bool Start()
		{
            try
            {
                GlobalClass.m_LogProcess = new FileLogProcess(Application.StartupPath +"\\System Log","ServerSystem");
                GlobalClass.m_LogProcess.Start();

                GlobalClass.s_DaemonProcessManager = new DaemonProcessManager();

                if (!InitializeServer())
                {
                    return false;
                }
                GlobalClass.m_IsRun = true;

                //서버 스래드를 시작합니다.
                m_StartServerThread = new Thread(new ThreadStart(StartServer));
                m_StartServerThread.Start();


                m_ReloadCheckThread = new Thread(new ThreadStart(ProcessReloadCheck));
                m_ReloadCheckThread.Start();


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
		}

        private void ProcessReloadCheck()
        {
            while (!GlobalClass.m_IsRequestToStop)
            {
                DateTime tNowTime = DateTime.Now;
                DateTime tReloadTime;

                DateTime tStartTime;
                TimeSpan tSpan;
                try
                {
                    tReloadTime = new DateTime(tNowTime.Year, tNowTime.Month, tNowTime.Day, GlobalClass.m_SystemInfo.ReloadHour, GlobalClass.m_SystemInfo.ReloadMinute, 0);
                    if (tNowTime.Hour != tReloadTime.Hour || tNowTime.Minute != tReloadTime.Minute) continue;

                    tStartTime = DateTime.Now;

                    GlobalClass.m_LogProcess.PrintLog("==============================기초 데이터를 새로 올리기 시작합니다.==============================");

                    if (!BaseDataLoadProcess.LoadBaseData())
                    {
                        GlobalClass.m_LogProcess.PrintLog("'기초 데이터 로드에 실패하였습니다.");
                    }

                    //2011-10-01 hanjiyeon 추가 Telnet 기본 접속 테스트 작업 수행.
                    // FOMS 연동 실패 조회는 SP_FOMS_DEVICE_DISCORD_INFO 를 이용하여 FOMS 연동 실패 테이블에서 직접 데이터를 얻어오고,
                    // Telnet 기본 접속 테스트 결과 실패된 장비 조회는 FACT 서버에서 시설연동 후 명령을 실행하여 결과를 DB Ne_Ne 테이블과 서버의 메모리내에 있는 장비정보에 Update 한다.

                    tSpan = DateTime.Now - tStartTime;
                    if (tSpan.Seconds < 60)
                    {
                        //데이터 갱신 작업이 60초 보다 적게 걸릴 경우 다시 RefreshData()함수가 호출되기 때문에 시간을 계산에 기다리게 처리한다.
                        Thread.Sleep((60 - tSpan.Seconds) * 1000);
                    }

                }
                catch (Exception ex)
                {
                    GlobalClass.m_LogProcess.PrintLog("ProcessReloadCheck exception : " + ex.Message.ToString());
                }
                finally
                {
                    Thread.Sleep(1000);   //1초에 한번씩 체크
                }
            }
        }


		/// <summary>
		/// 서버를 종료합니다.
		/// </summary>
		public void Stop()
		{
            GlobalClass.m_IsRun = false;

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

            if (GlobalClass.m_ClientProcess != null)
            {
                GlobalClass.m_ClientProcess.Stop();
                GlobalClass.m_ClientProcess = null;
            }

            GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "재로딩 프로세스를 종료 합니다.");
            if (m_ReloadCheckThread != null)
            {
                GlobalClass.m_IsRequestToStop = true;

                if (m_ReloadCheckThread.ThreadState == ThreadState.Running)
                {
                    m_ReloadCheckThread.Abort();
                    m_ReloadCheckThread.Join();
                }
                m_ReloadCheckThread = null;
            }


            GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "서버를 종료 합니다.");
			if (m_StartServerThread != null)
			{
				if (m_StartServerThread.ThreadState == ThreadState.Running)
				{
					m_StartServerThread.Abort();
					m_StartServerThread.Join();
				}
                m_StartServerThread = null;
			}

			if (GlobalClass.m_DBPool != null)
			{
				lock (GlobalClass.m_DBPool)
				{
					try
					{
						GlobalClass.m_DBPool.StopDBPool();
					}
					catch (Exception) { }
					GlobalClass.m_DBPool.Dispose();
					GlobalClass.m_DBPool = null;
				}
			}

			if (GlobalClass.m_DBExecutePool != null)
			{
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "데이터베이스 DBPool 접속을 종료 합니다.");
				lock (GlobalClass.m_DBExecutePool)
				{
					try
					{
						GlobalClass.m_DBExecutePool.StopDBPool();
					}
					catch (Exception) { }
					GlobalClass.m_DBExecutePool.Dispose();
					GlobalClass.m_DBExecutePool = null;
				}
			}
            if (GlobalClass.m_DBLogProcess != null)
            {
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "DB로그 프로세서를 종료 합니다.");
                GlobalClass.m_DBLogProcess.Dispose();
                GlobalClass.m_DBLogProcess = null;
            }


            if (GlobalClass.m_LogProcess != null)
            {
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "로그 프로세서를 종료 합니다.");
                GlobalClass.m_LogProcess.Stop();
                GlobalClass.m_LogProcess = null;
            }

	
		}

		/// <summary>
		/// 서버 설정 정보를 로드합니다.
		/// </summary>
		/// <returns></returns>
		public bool LoadSystemInfo()
		{
			ArrayList tSystemInfos = null;			
			E_XmlError tXmlError = E_XmlError.Success;
			try
			{
                FileInfo tFileInfo = new FileInfo(GlobalClass.m_StartupPath + GlobalClass.c_SystemConfigFileName);
                if (!tFileInfo.Exists) MKXML.ObjectToXML(tFileInfo.FullName, new SystemConfig());

				tSystemInfos = MKXML.ObjectFromXML(GlobalClass.m_StartupPath + GlobalClass.c_SystemConfigFileName, typeof(SystemConfig), out tXmlError);
				if (tSystemInfos == null) return false;
				if (tSystemInfos.Count == 0) return false;
				GlobalClass.m_SystemInfo = (SystemConfig)tSystemInfos[0];

				GlobalClass.m_DBConnectionInfo = new DBConnectionInfo();
				GlobalClass.m_DBConnectionInfo.DBServerIP = GlobalClass.m_SystemInfo.DBServerIP;
				GlobalClass.m_DBConnectionInfo.DBName = GlobalClass.m_SystemInfo.DBName;
				GlobalClass.m_DBConnectionInfo.UserID = GlobalClass.m_SystemInfo.UserID;
				GlobalClass.m_DBConnectionInfo.Password = GlobalClass.m_SystemInfo.Password;
				GlobalClass.m_DBConnectionInfo.DBConnectionCount = GlobalClass.m_SystemInfo.DBConnectionCount;

				return true;
			}
			catch (Exception ex)
			{
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, ex.ToString());
				return false;
			}
		}

       


		/// <summary>
		/// 환경 정보를 생성합니다.
		/// </summary>
		/// <returns></returns>
		public bool MakeSystemInfo()
		{
			IPAddress[] tIPAddress = null;
			E_XmlError tXmlError = E_XmlError.Success;
			SystemConfig tSystemInfo = null;
			try
			{
				tIPAddress = Dns.GetHostEntry(Environment.MachineName).AddressList;
				tSystemInfo = new SystemConfig();

				tSystemInfo.ServerID = 0;
				if (tIPAddress.Length > 0)
				{
					tSystemInfo.ServerIP = tIPAddress[0].ToString();
					
				}
                // 2013-01-11 - shinyn - 테스트인경우 FACT_TEST에서 하도록 수정
                tSystemInfo.DBServerIP = Environment.MachineName + ",43218" + "\\FACT_TEST";
                tSystemInfo.DBName = "FACT_TEST";
                tSystemInfo.UserID = "sa";
                tSystemInfo.Password = "factskB~2012";
				//tSystemInfo.DBServerIP = Environment.MachineName + ",43218" + "\\FACT_MAIN";
                //tSystemInfo.DBName = "FACT_TEST";
                //tSystemInfo.UserID = "factdev";
                //tSystemInfo.Password = "factdev";

                tSystemInfo.ServerPort = 54321;
				tSystemInfo.ServerChannel = "RemoteClient";
				
				

				tXmlError = MKXML.ObjectToXML(string.Concat(GlobalClass.m_StartupPath, GlobalClass.c_SystemConfigFileName), tSystemInfo);

				if (tXmlError == E_XmlError.Success)
					return true;
				else
					return false;
			}
			catch (Exception ex)
			{
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, ex.ToString());
				return false;
			}
		}


		/// <summary>
		/// 서버를 초기화합니다.
		/// </summary>
		/// <returns></returns>
		private bool InitializeServer()
		{
			try
			{
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "서버를 초기화 합니다.");
				if (!LoadSystemInfo()) return false;

				GlobalClass.m_DBPool = new MKOleDBPool(E_DatabaseServerType.MSSqlServer, 10);
				GlobalClass.m_DBPool.StartDBPool();

				E_DBProcessError tError = E_DBProcessError.Success;
				tError = GlobalClass.m_DBPool.OpenDatabase(GlobalClass.m_SystemInfo.DBConnectionCount, GlobalClass.m_SystemInfo.DBServerIP, GlobalClass.m_SystemInfo.DBName, GlobalClass.m_SystemInfo.UserID, GlobalClass.m_SystemInfo.Password);
				if (tError != E_DBProcessError.Success)
				{
                    GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, "데이터베이스 열기를 실패하였습니다." + " " + tError.ToString());
					return false;
				}
				else
				{
					GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "데이터베이스 DBPool에 접속하였습니다.");
				}

				GlobalClass.m_DBExecutePool = new MKOleDBPool(E_DatabaseServerType.MSSqlServer, 10);
				GlobalClass.m_DBExecutePool.StartDBPool();

                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, GlobalClass.m_SystemInfo.DBConnectionCount + " " + GlobalClass.m_SystemInfo.DBServerIP + " " + GlobalClass.m_SystemInfo.DBName + " " + GlobalClass.m_SystemInfo.UserID + " " + GlobalClass.m_SystemInfo.Password);				
				tError = GlobalClass.m_DBExecutePool.OpenDatabase(GlobalClass.m_SystemInfo.DBConnectionCount, GlobalClass.m_SystemInfo.DBServerIP, GlobalClass.m_SystemInfo.DBName, GlobalClass.m_SystemInfo.UserID, GlobalClass.m_SystemInfo.Password);
				if (tError != E_DBProcessError.Success)
				{
                    GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Warning, "데이터베이스 열기를 실패하였습니다." + " " + tError.ToString());
					return false;
				}
				else
				{
                    GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "데이터베이스 DBPool에 접속하였습니다.");
				}

				GlobalClass.m_DBLogProcess = new DBLogProcess(GlobalClass.m_DBPool, GlobalClass.m_StartupPath);

				return true;
			}
			catch (Exception ex)
			{
				GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, ex.ToString());
				return false;
			}
		}

		/// <summary>
		/// 서버를 시작합니다.
		/// </summary>
		private void StartServer()
		{
            if (!BaseDataLoadProcess.LoadBaseData())
            {
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, "기초 정보 로드에 실패 했습니다.");
                return;
            }
			RemoteServerStart();

            GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "서버 초기화가 완료 되었습니다.");
		}

		/// <summary>
		/// 원격 채널을 시작합니다.
		/// </summary>
		private void RemoteServerStart()
		{			
			try
			{
				//Jbo ID생성자를 초기화 합니다.
                //GlobalClass.m_JobIDGenerator = new JobIDGenerator();
                //GlobalClass.m_JobIDGenerator.Initialize();

                GlobalClass.m_ClientProcess = new ClientCommunicationProcess();
                GlobalClass.m_ClientProcess.Start();

                GlobalClass.s_DaemonProcessManager = new DaemonProcessManager();
                GlobalClass.s_DaemonProcessManager.Start();

                GlobalClass.s_ServiceManagerCommunicationProcess = new ServiceManagerCommunicationProcess();
                GlobalClass.s_ServiceManagerCommunicationProcess.Start();

                //GlobalClass.m_TelnetProcessor = new TelnetProcessor.TelnetProcessor(GlobalClass.m_DBLogProcess,GlobalClass.m_LogProcess);
                //GlobalClass.m_TelnetProcessor.Start();
			}
			catch (Exception ex)
			{
				
			}
		}

       
    }
}
