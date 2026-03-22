using System;
using System.Collections.Generic;
using System.Text;
using RACTCommonClass;
using System.Threading;
using MKLibrary.MKNetwork;

namespace RACTServer
{
    /// <summary>
    /// RACTServerServiceManager와 통신하며 서버 상태(데몬 목록 등)를 보고하는 프로세스입니다.
    /// </summary>
    public class ServiceManagerCommunicationProcess
    {
        private MKRemote m_RemoteGateway;

        internal bool Start()
        {
            int count = 0;
            string result = string.Empty;
            try
            {
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "Service Manager 통신 채널을 생성 합니다.");
                m_RemoteGateway = new MKRemote(E_RemoteType.TCPRemote, GlobalClass.m_SystemInfo.ServerIP, GlobalClass.m_SystemInfo.ServiceManagerUsePort, GlobalClass.m_SystemInfo.ServiceManagerChannelName);
                
                while (count < 10)
                {
                    if (m_RemoteGateway.StartRemoteServer(out result) == E_RemoteError.Success) break;
                    GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, "Service Manager 채널 생성 실패: " + result);
                    Thread.Sleep(3000);
                    count++;
                }

                if (m_RemoteGateway == null) return false;

                RemoteClientMethod tRemoteMethod = new RemoteClientMethod();
                tRemoteMethod.SetDaemonListRequestHandler(OnDaemonListRequested);
                m_RemoteGateway.ServerObject = tRemoteMethod;

                return true;
            }
            catch (Exception ex)
            {
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, "ServiceManager Start Error: " + ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// 서비스 매니저로부터 데몬 목록 요청이 왔을 때 처리합니다.
        /// </summary>
        private byte[] OnDaemonListRequested()
        {
            try
            {
                var daemonList = GlobalClass.s_DaemonProcessManager?.DaemonProcessList;
                if (daemonList != null)
                {
                    // 복사본을 생성하여 직렬화 전송 (동시성 안전)
                    return ObjectConverter.GetBytes(new DaemonProcessInfoCollection(daemonList));
                }
            }
            catch (Exception ex)
            {
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, "OnDaemonListRequested Error: " + ex.Message);
            }
            return null;
        }

        internal void Stop()
        {
            if (m_RemoteGateway != null)
            {
                m_RemoteGateway.Dispose();
                m_RemoteGateway = null;
            }
            GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "Service Manager 통신 프로세스 종료.");
        }
    }
}
