using System;
using System.Collections.Generic;
using System.Text;
using RACTDaemonProcess;
using System.Collections;
using System.IO;
using RACTCommonClass;
using MKLibrary.MKNetwork;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Dapper;

namespace RACTServer
{
    public class DaemonProcessManager
    {
        private MKRemote m_RemoteGateway;
        private Task m_HealthCheckTask = null;
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private DaemonProcessInfoCollection m_DaemonProcessList;
        private DaemonProcessInfo m_LastSendDaemonInfo = null;

        public DaemonProcessManager()
        {
            GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "Daemon Manager를 시작 합니다.");
            m_DaemonProcessList = new DaemonProcessInfoCollection();
        }

        public void Start()
        {
            _cts = new CancellationTokenSource();
            m_HealthCheckTask = Task.Run(() => ProcessHealthCheckAsync(_cts.Token));

            StartRemoteServer();
        }

        private async Task ProcessHealthCheckAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested && GlobalClass.m_IsRun)
            {
                try
                {
                    DateTime now = DateTime.Now;
                    var daemonList = m_DaemonProcessList.ToList();
                    foreach (var daemonInfo in daemonList)
                    {
                        if (daemonInfo == null) continue;

                        double totalSeconds = (now - daemonInfo.LifeTime).TotalSeconds;
                        if (totalSeconds >= GlobalClass.s_HealthCheckTimeOut)
                        {
                            // 한 번 더 확인 (경합 방지)
                            await Task.Delay(100, token);
                            if ((DateTime.Now - daemonInfo.LifeTime).TotalSeconds >= GlobalClass.s_HealthCheckTimeOut)
                            {
                                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, 
                                    string.Format("@@@ 데몬 세션 만료 삭제 {0}:{1} (LastLifeTime: {2}, Idle: {3:F1}s)", 
                                    daemonInfo.IP, daemonInfo.Port, daemonInfo.LifeTime.ToString("HH:mm:ss"), totalSeconds));
                                m_DaemonProcessList.Remove(daemonInfo.DaemonID);
                            }
                        }
                    }
                }
                catch (OperationCanceledException) { break; }
                catch (Exception ex)
                {
                    GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, "Daemon HealthCheck Error: " + ex.Message);
                }
                await Task.Delay(1000, token);
            }
        }

        private bool StartRemoteServer()
        {
            int count = 0;
            string result = string.Empty;
            try
            {
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "데몬 통신 채널을 생성 합니다.");
                m_RemoteGateway = new MKRemote(E_RemoteType.TCPRemote, GlobalClass.m_SystemInfo.ServerIP, GlobalClass.m_SystemInfo.DaemonUsePort, GlobalClass.m_SystemInfo.DaemonChannelName);
                
                while (count < 10)
                {
                    if (m_RemoteGateway.StartRemoteServer(out result) == E_RemoteError.Success) break;
                    GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, "데몬 채널 생성 실패: " + result);
                    Thread.Sleep(3000);
                    count++;
                }

                if (m_RemoteGateway == null) return false;

                RemoteClientMethod tRemoteMethod = new RemoteClientMethod();
                tRemoteMethod.SetDaemonResultHandler(DaemonResultSender);
                tRemoteMethod.SetDaemonConnectHandler(DaemonConnectReceiver);
                tRemoteMethod.SetTelnetSessionIDRequestHandler(TelnetSessionIdRequestReceiver);
                tRemoteMethod.SetTelnetConnectionUpdateRequestHandler(TelnetConnectionUpdateReceiver);
                tRemoteMethod.SetDaemonStatusUpdateRequestHandler(UpdateDaemonStatus);
                tRemoteMethod.SetUserLogOutHandler(DaemonLogOutReceiver);
                tRemoteMethod.SetHealthCheckRequestHandler(HealthCheckUpdateReceiver);
                m_RemoteGateway.ServerObject = tRemoteMethod;

                return true;
            }
            catch (Exception ex)
            {
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, "Daemon StartRemoteServer Error: " + ex.ToString());
                return false;
            }
        }

        private void HealthCheckUpdateReceiver(int aClientID)
        {
            var daemon = m_DaemonProcessList[aClientID];
            if (daemon != null) daemon.LifeTime = DateTime.Now;
        }

        private void DaemonLogOutReceiver(int aDaemonID)
        {
            m_DaemonProcessList.Remove(aDaemonID);
        }

        private byte[] DaemonConnectReceiver(string aIP, int aPort, string aChannelName)
        {
            try
            {
                // 중복 접속 제거
                var dupInfos = m_DaemonProcessList.ToList().Where(d => d.IP == aIP && d.Port == aPort).ToList();
                foreach (var info in dupInfos) m_DaemonProcessList.Remove(info.DaemonID);

                DaemonProcessInfo daemonInfo = new DaemonProcessInfo
                {
                    IP = aIP,
                    Port = aPort,
                    ChannelName = aChannelName + aPort,
                    LifeTime = DateTime.Now
                };

                m_DaemonProcessList.Add(daemonInfo);
                GlobalClass.m_LogProcess.PrintLog(string.Format("데몬 접속 수락: {0}:{1} ({2})", aIP, aPort, daemonInfo.ChannelName));

                return ObjectConverter.GetBytes(new DaemonLoginResultInfo 
                { 
                    DaemonInfo = daemonInfo, 
                    ClientID = daemonInfo.DaemonID, 
                    LoginResult = E_LoginResult.Success 
                });
            }
            catch
            {
                return ObjectConverter.GetBytes(new DaemonLoginResultInfo { LoginResult = E_LoginResult.UnknownError });
            }
        }

        private int TelnetSessionIdRequestReceiver(string aQuery)
        {
            return GlobalClass.m_DBLogProcess.ExcuteQuery(aQuery);
        }

        private bool TelnetConnectionUpdateReceiver(int aSessionID)
        {
            try
            {
                using (var conn = GlobalClass.GetSqlConnection())
                {
                    if (conn == null) return false;
                    conn.Execute("update RACT_LOG_DeviceConnection set ConnectLogType = 1, DisconnectTime = GETDATE() where id = @ID", new { ID = aSessionID });
                }
                return true;
            }
            catch (Exception ex)
            {
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, "TelnetConnectionUpdate Error: " + ex.Message);
                return false;
            }
        }

        private byte[] DaemonResultSender(int aDaemonID)
        {
            var daemon = m_DaemonProcessList[aDaemonID];
            if (daemon == null) return null;

            if (daemon.DataQueue.Count > 0)
            {
                ArrayList results = new ArrayList();
                while (daemon.DataQueue.TryDequeue(out byte[] data))
                {
                    results.Add(data);
                    if (results.Count >= 200) break;
                }
                return (byte[])ObjectConverter.GetBytes(results);
            }
            return null;
        }

        internal DaemonProcessInfo GetDaemonProcess(UseableDaemonRequestInfo aRequestInfo)
        {
            var daemonList = m_DaemonProcessList.ToList()
                .Where(d => !GlobalClass.m_SystemInfo.IsSSHTunnelDaemonIP(d.IP))
                .OrderBy(d => d.TelnetSessionCount)
                .ToList();

            if (daemonList.Count == 0) return null;

            // 라운드 로빈 또는 부하가 가장 적은 데몬 선택
            var target = daemonList.FirstOrDefault(d => !aRequestInfo.DisconnectDaemonList.Contains(d.DaemonID)) ?? daemonList.First();
            
            GlobalClass.m_LogProcess.PrintLog(string.Format("데몬 할당 (요청기반): {0}:{1} (Sessions: {2})", target.IP, target.Port, target.TelnetSessionCount));
            return target;
        }

        internal DaemonProcessInfo GetDaemonProcess()
        {
            var target = m_DaemonProcessList.ToList()
                .Where(d => !GlobalClass.m_SystemInfo.IsSSHTunnelDaemonIP(d.IP))
                .OrderBy(d => d.TelnetSessionCount + d.TempSessionCount)
                .FirstOrDefault();

            if (target != null)
            {
                target.TempSessionCount++;
                GlobalClass.m_LogProcess.PrintLog(string.Format("데몬 할당 (자동): {0}:{1} (Total Load: {2})", target.IP, target.Port, target.TelnetSessionCount + target.TempSessionCount));
            }
            return target;
        }

        internal DaemonProcessInfo GetSSHTunnelDaemonProcess()
        {
            var target = m_DaemonProcessList.ToList()
                .Where(d => GlobalClass.m_SystemInfo.IsSSHTunnelDaemonIP(d.IP))
                .OrderBy(d => d.TelnetSessionCount + d.TempSessionCount)
                .FirstOrDefault();

            if (target != null)
            {
                target.TempSessionCount++;
                GlobalClass.m_LogProcess.PrintLog(string.Format("SSH 터널 데몬 할당: {0}:{1}", target.IP, target.Port));
            }
            else
            {
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Warning, "사용 가능한 SSH 터널 데몬이 없습니다.");
            }
            return target;
        }

        internal void UpdateDaemonStatus(byte[] aInfo)
        {
            var tInfo = (DaemonProcessInfo)ObjectConverter.GetObject(aInfo);
            if (tInfo == null) return;

            tInfo.LifeTime = DateTime.Now;
            if (m_DaemonProcessList.Contains(tInfo.DaemonID))
            {
                m_DaemonProcessList[tInfo.DaemonID] = tInfo;
            }
            else
            {
                m_DaemonProcessList.Add(tInfo);
            }
        }

        public DaemonProcessInfoCollection DaemonProcessList
        {
            get { return m_DaemonProcessList; }
            set { m_DaemonProcessList = value; }
        }

        internal void Stop()
        {
            _cts.Cancel();
            if (m_RemoteGateway != null) { m_RemoteGateway.Dispose(); m_RemoteGateway = null; }
            
            try { m_HealthCheckTask?.Wait(3000); } catch { }
            
            GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "Daemon Manager 종료 완료.");
        }

        internal void TempConnectionListClear()
        {
            foreach (var daemon in m_DaemonProcessList.ToList()) daemon.TempSessionCount = 0;
        }
    }
}
