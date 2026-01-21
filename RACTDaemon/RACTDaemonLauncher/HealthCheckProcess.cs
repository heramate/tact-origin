using System;
using System.Collections.Generic;
using System.Text;
using MKLibrary.MKNetwork;
using System.Threading;
using System.Diagnostics;
using RACTCommonClass;

namespace RACTDaemonLauncher
{
    /// <summary>
    /// 프로스세 상태가 변경될때 사용될 핸들러 입니다.
    /// </summary>
    /// <param name="aStatus"></param>
    public delegate void ProcessStatusChangeHandelr(HealthCheckItem aCheckItem, E_ProcessStatus aStatus);
    [Serializable]
    public class HealthCheckProcess
    {
        /// <summary>
        /// 프로세스 상태가 변경되었을대 처리할 이벤트 입니다.
        /// </summary>
        public event ProcessStatusChangeHandelr OnProcessStatusChange = null;
        /// <summary>
        /// Health Check Item 입니다.
        /// </summary>
        private HealthCheckItem m_HealthCheckItem;
        /// <summary>
        /// Check 간격 입니다.
        /// </summary>
        private int m_CheckInterval = 1000;
        /// <summary>
        /// 실행 여부 입니다.
        /// </summary>
        private bool m_IsRun = false;
        /// <summary>
        /// 접속 시도 최대 수 입니다.
        /// </summary>
        private int m_MaxTryCount = 60;
        /// <summary>
        /// 리모트 객체 입니다.
        /// </summary>
        private MKRemote m_RemoteGateway;
        /// <summary>
        /// 시도 횟수입니다.
        /// </summary>
        private int m_TryCount = 0;

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        /// <param name="aCheckItem"></param>
        public HealthCheckProcess(HealthCheckItem aCheckItem)
        {
            m_HealthCheckItem = aCheckItem;
        }
      
        /// <summary>
        /// Health Check를 시작 합니다.
        /// </summary>
        /// <param name="aCheckItem"></param>
        public object StartHealthCheck(object aCheckItem)
        {
            m_TryCount = 0;
            m_IsRun = true;
            while (m_IsRun)
            {
                try
                {
                    if (m_TryCount > m_MaxTryCount)
                    {
                        Stop();
                        Console.WriteLine(m_HealthCheckItem.Key + " 에 접속 할 수 없습니다.");
                        m_HealthCheckItem.ProcessID = 0;
                        return null;
                    }
                    if (m_HealthCheckItem.AutoStart)
                    {
                        if (DaemonProcessCheck())
                        {
                            if (DaemonRemoteCheck())
                            {
                                m_TryCount = 0;
                            }
                            else
                            {
                                if (OnProcessStatusChange != null) OnProcessStatusChange(m_HealthCheckItem, E_ProcessStatus.ProcessAlive);
                            }
                        }
                        else
                        {
                            if (OnProcessStatusChange != null) OnProcessStatusChange(m_HealthCheckItem, E_ProcessStatus.ProcessKill);
                        }
                    }
                    else
                    {
                        if (OnProcessStatusChange != null) OnProcessStatusChange(m_HealthCheckItem, E_ProcessStatus.ProcessKill);
                    }
                }
                catch (Exception ex)
                {
                    AppGlobal.s_FileLog.PrintLogEnter(ex.ToString());
                }
                Thread.Sleep(m_CheckInterval);
            }

            return null;
        }
        /// <summary>
        /// Daemon 프로세스를 확인 합니다.
        /// </summary>
        /// <returns></returns>
        private bool DaemonProcessCheck()
        {
            try
            {
                if (m_HealthCheckItem.ProcessID == 0)
                {
                    LoadDaemonProcessFile();
                }

                Process[] tProcessList = Process.GetProcessesByName("RACTDaemonExe");

                if (tProcessList.Length == 0)
                {
                    m_HealthCheckItem.ProcessID = 0;
                    LoadDaemonProcessFile();

                }
                else
                {
                    foreach (Process tDaemonProcess in tProcessList)
                    {
                        if (tDaemonProcess.Id == m_HealthCheckItem.ProcessID)
                        {
                            return true;
                        }
                    }
                }
                m_TryCount++;
            }
            catch (Exception ex)
            {
                AppGlobal.s_FileLog.PrintLogEnter(ex.ToString());

            }
            return false;

        }

        /// <summary>
        /// 프로세스 정보를 읽습니다.
        /// </summary>
        private void LoadDaemonProcessFile()
        {
            ProcessStartInfo tStartInfo = new ProcessStartInfo();
            tStartInfo.CreateNoWindow = false;
            tStartInfo.FileName = m_HealthCheckItem.ExecutionFilePath;
            tStartInfo.Arguments = m_HealthCheckItem.ExecutionArg;
            tStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            tStartInfo.Arguments = m_HealthCheckItem.ExecutionArg;

            Process tProcess = Process.Start(tStartInfo);
            m_HealthCheckItem.ProcessID = tProcess.Id;
        }
        /// <summary>
        /// 서버의 리모트 활성화 여부를 확인 합니다.
        /// </summary>
        /// <returns>리모트 접속 여부 입니다.</returns>
        private bool DaemonRemoteCheck()
        {
            RemoteClientMethod tSPO = null;

            DateTime tSDate = DateTime.Now;

            try
            {
                if (m_RemoteGateway == null)
                {
                    m_RemoteGateway = new MKRemote(E_RemoteType.TCPRemote, m_HealthCheckItem.DaemonIPAddress, m_HealthCheckItem.DaemonPort, m_HealthCheckItem.DaemonChannelName);
                }

                if (m_RemoteGateway == null)
                {
                    if (OnProcessStatusChange != null) OnProcessStatusChange(m_HealthCheckItem, E_ProcessStatus.RemoteKill);
                    m_TryCount++;
                    return false;
                }
              
                //else
                //{

                //    try
                //    {
                //        tSPO = (RemoteClientMethod)m_RemoteGateway.ServerObject;
                //        if (tSPO == null)
                //        {
                //            if (OnProcessStatusChange != null) OnProcessStatusChange(m_HealthCheckItem, E_ProcessStatus.RemoteKill);
                //            m_TryCount++;
                //            return false;
                //        }
                //        ObjectConverter.GetObject(tSPO.CallResultMethod(0));
                //    }
                //    catch (Exception ex)
                //    {
                //        m_TryCount++;
                //        if (OnProcessStatusChange != null) OnProcessStatusChange(m_HealthCheckItem, E_ProcessStatus.RemoteKill);
                //        Thread.Sleep(2000);
                //        return false;
                //    }
                //}
            }
            catch (Exception ex)
            {
                AppGlobal.s_FileLog.PrintLogEnter(ex.ToString());
            }
            return true;
        }

        /// <summary>
        /// 중지 처리 합니다.
        /// </summary>
        public void Stop()
        {
            m_IsRun = false;
            try
            {
                if (m_RemoteGateway != null)
                {
                    string tErrorString="";
                    m_RemoteGateway.StopServer(out tErrorString);
                    m_RemoteGateway.Dispose();
                }
                m_RemoteGateway = null;
                Process tProcess = Process.GetProcessById(m_HealthCheckItem.ProcessID);
                if (tProcess != null)
                {
                    tProcess.Kill();
                }
                m_HealthCheckItem.ProcessID = 0;
            }
            catch (Exception ex)
            {
                AppGlobal.s_FileLog.PrintLogEnter(ex.ToString());
            }
            if (OnProcessStatusChange != null) OnProcessStatusChange(m_HealthCheckItem, E_ProcessStatus.ProcessKill);
        }

        /// <summary>
        /// Health Check Item 가져오거나 설정 합니다.
        /// </summary>
        public HealthCheckItem HealthCheckItem
        {
            get { return m_HealthCheckItem; }
            set { m_HealthCheckItem = value; }
        }
        /// <summary>
        /// Check 간격 가져오거나 설정 합니다.
        /// </summary>
        public int CheckInterval
        {
            get { return m_CheckInterval; }
            set { m_CheckInterval = value; }
        }

    }
}
