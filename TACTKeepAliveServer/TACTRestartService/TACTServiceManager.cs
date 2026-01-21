using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
//using System.Linq;
//using System.Text;
using RACTCommonClass;
using System.ServiceProcess;//ServiceController
using System.Diagnostics;//Process
using MKLibrary.MKData;//MKXML
using System.IO;//FileInfo

namespace TACTRestartService
{
    class TACTServiceManager : IDisposable
    {
        private Thread m_ManagerThread = null;
        private List<TACTService> m_ServiceList = new List<TACTService>();
        public static int s_ServiceTimeoutMillisecond = 1000 * 15; //15초


        public TACTServiceManager(string aStartupPath)
        {
            GlobalClass.m_StartupPath = aStartupPath;
        }

        public void Dispose()
        {
            Stop();
        }

        public void Stop()
        {
            GlobalClass.m_IsServerStop = true;

            GlobalClass.PrintLogInfo("■ 서비스 자동재기동 서버 스레드를 중지합니다 ---------------\r\n");
            //GlobalClass.PrintLogInfo("Keep-Alive 서버 스레드를 중지합니다.");
            GlobalClass.StopThread(m_ManagerThread);
        }

        public bool Start()
        {
            try
            {
                // 파일로깅 시작
                System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(GlobalClass.m_StartupPath));
                if (GlobalClass.m_StartupPath.Length < 1) return false;

                GlobalClass.m_LogProcess = new FileLogProcess(GlobalClass.m_StartupPath + "\\Log", "TACTRestartService");
                GlobalClass.m_LogProcess.Start();
                GlobalClass.PrintLogInfo("■ 서비스 자동재기동 서버를 시작합니다 ---------------");
                GlobalClass.PrintLogInfo("● 파일로깅을 시작합니다.");

                GlobalClass.PrintLogInfo("● 환경설정 값을 로드합니다.");
                _LoadSystemConfig();

                GlobalClass.PrintLogInfo("● 감시대상 서비스정보를 로드합니다.");
                if (!_LoadServiceList()) return false;

                GlobalClass.PrintLogInfo("● 감시 스레드를 시작합니다.");
                m_ManagerThread = new Thread(new ThreadStart(_ServiceMonitorThread));
                m_ManagerThread.Start();

            }
            catch (Exception ex)
            {
                GlobalClass.PrintLogException("[TACTServiceManager.Start] :", ex);
                return false;
            }

            return true;
        }

        private bool _LoadSystemConfig()
        {
            ArrayList tSystemInfos = null;
            E_XmlError tXmlError = E_XmlError.Success;
            try
            {
                FileInfo tFileInfo = new FileInfo(GlobalClass.m_StartupPath + GlobalClass.c_SystemConfigFileName);
                if (!tFileInfo.Exists)
                {
                    E_XmlError error = MKXML.ObjectToXML(tFileInfo.FullName, new SystemConfig());
                }

                tSystemInfos = MKXML.ObjectFromXML(GlobalClass.m_StartupPath + GlobalClass.c_SystemConfigFileName, typeof(SystemConfig), out tXmlError);
                if (tSystemInfos == null) return false;
                if (tSystemInfos.Count == 0) return false;
                GlobalClass.m_SystemInfo = (SystemConfig)tSystemInfos[0];

                return true;
            }
            catch (Exception ex)
            {
                GlobalClass.PrintLogException("[TACTServiceManager._LoadServiceList] ", ex);
                return false;
            }
        }

        private bool _LoadServiceList()
        {
            m_ServiceList.Clear();

            //string serviceName = "TACS_KeepAliveServer";
            //string processName = "TACTKeepAliveServerService";
            //string description = "[2018,c-RPCS] LTE접속을 위한 Keep-Alive Server";
            //TACTService serviceInfo = new TACTService(serviceName, processName, description);
            //GlobalClass.PrintLogInfo(string.Format("{0} 감시대상서비스정보 : {1}", m_ServiceList.Count+1, serviceInfo._ToString()));
            //m_ServiceList.Add(serviceInfo);

            ArrayList tSystemInfos = null;
            E_XmlError tXmlError = E_XmlError.Success;
            try
            {
                FileInfo tFileInfo = new FileInfo(GlobalClass.m_StartupPath + GlobalClass.c_ServiceListFileName);
                if (!tFileInfo.Exists)
                {
                    E_XmlError error = MKXML.ObjectToXML(tFileInfo.FullName, new TACTServiceList());
                }

                tSystemInfos = MKXML.ObjectFromXML(GlobalClass.m_StartupPath + GlobalClass.c_ServiceListFileName, typeof(TACTServiceList), out tXmlError);
                if (tSystemInfos == null) return false;
                if (tSystemInfos.Count == 0) return false;
                foreach (TACTService tInfo in (TACTServiceList)tSystemInfos[0])
                {
                    m_ServiceList.Add(tInfo);
                    GlobalClass.PrintLogInfo(string.Format("[{0}] 감시대상서비스정보 : {1}", m_ServiceList.Count, tInfo._ToString()));
                }

                return true;
            }
            catch (Exception ex)
            {
                GlobalClass.PrintLogException("[TACTServiceManager._LoadServiceList] ", ex);
                return false;
            }
        }

        /// <summary>
        /// [Thread] 감시대상 서비스가 중단되었는지 체크한다.
        /// </summary>
        protected void _ServiceMonitorThread()
        {
            while (!GlobalClass.m_IsServerStop)
            {
                var services = ServiceController.GetServices();

                foreach (ServiceController service in services)
                {
                    TACTService tServiceInfo = GetTACTService(service.ServiceName);
                    if (tServiceInfo == null) continue;

                    /// 중지되었으면 서비스를 다시 시작한다.
                    if (service.Status.Equals(ServiceControllerStatus.Stopped))
                    {
                        GlobalClass.PrintLogInfo(string.Format("중단된 서비스를 감지하였습니다. 서비스명={0}", service.ServiceName));
                        StartService(tServiceInfo);
                    }
                } // End of foreach (ServiceController[])

                Thread.Sleep(1000*GlobalClass.m_SystemInfo.CheckIntervalSeconds); //N초 후에 체크
            } // End of while
        }

        /// <summary>
        /// 감시대상 서비스 정보를 얻는다.
        /// </summary>
        /// <param name="aServiceName"></param>
        /// <returns></returns>
        public TACTService GetTACTService(string aServiceName)
        {
            foreach (TACTService tService in m_ServiceList)
            {
                if (tService.ServiceName.Equals(aServiceName))
                {
                    return tService;
                }
            }
            return null;
        }

        /// <summary>
        /// 서비스를 시작한다.
        /// </summary>
        /// <param name="aSeviceInfo"></param>
        public static void StartService(TACTService aSeviceInfo)
        {
            ServiceController service = new ServiceController(aSeviceInfo.ServiceName);
            try
            {
                GlobalClass.PrintLogInfo("서비스 시작 : " + aSeviceInfo._ToString());

                TimeSpan timeout = TimeSpan.FromMilliseconds(s_ServiceTimeoutMillisecond);
                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch (Exception ex)
            {
                GlobalClass.PrintLogException("StartService Exception : ", ex);
            }
        }
/*
        public static bool KillProcess(TACTService aServiceInfo)
        {
            try
            {
                Process[] procList = Process.GetProcessesByName(aServiceInfo.ProcessName);
                if (procList.Length > 0)
                {
                    procList[0].Kill();
                    procList[0].WaitForExit(s_ServiceTimeoutMillisecond);

                    GlobalClass.PrintLogInfo(string.Format("서비스 프로세스 중지 : 프로세스명={1}, 프로세스ID={2}, 프로세스시작시각={3}"
                                            , aServiceInfo.ServiceName, procList[0].ProcessName, procList[0].Id, procList[0].StartTime.ToString("yyyy-MM-dd HH:mm:ss")));
                    return true;
                }
            }
            catch (Exception ex)
            {
                GlobalClass.PrintLogException("KillProcess Exception1 : ", ex);
            }

            return false;
        }

        public static void StopService(TACTService aServiceInfo)
        {
            ServiceController service = new ServiceController(aServiceInfo.ServiceName);


            try
            {
                TimeSpan timeout = TimeSpan.FromMilliseconds(s_ServiceTimeoutMillisecond);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                GlobalClass.PrintLogInfo("서비스 중지 : " + aServiceInfo._ToString());
            }
            catch (Exception ex)
            {
                GlobalClass.PrintLogException("StopService Exception2 : ", ex);
            }
        }
*/
    } // End of class TACTServiceMonitor
}
