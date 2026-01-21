using System;
using System.Collections.Generic;
using System.Text;
using RACTCommonClass;
using System.Threading;
using MKLibrary.MKNetwork;

namespace RACTServer
{
    public class ServiceManagerCommunicationProcess
    {
        /// <summary>
        /// daemon과 통신할 게이트웨이 입니다.
        /// </summary>
        private MKRemote m_RemoteGateway;
        /// <summary>
        /// 클라이언트 요청을 저장할 큐 입니다.
        /// </summary>
        private Queue<RequestCommunicationData> m_RequestQueue;
        /// <summary>
        /// 요청 처리 스레드 입니다.
        /// </summary>
        private Thread m_RequestProcessThread = null;
        /// <summary>
        /// 접속된 사용자, 데몬의 연결 상태를 확인 합니다.
        /// </summary>
        private Thread m_DaemonHelathCheckThread = null;
        /// <summary>
        /// Daemon 목록 입니다.
        /// </summary>
        private DaemonProcessInfoCollection m_DaemonProcessList;
        /// <summary>
        /// Daemon 목록 가져오거나 설정 합니다.
        /// </summary>
        public DaemonProcessInfoCollection DaemonProcessList
        {
            get { return m_DaemonProcessList; }
            set { m_DaemonProcessList = value; }
        }

        internal bool Start()
        {
            int tCount = 0;
            string tResult = string.Empty;
            RemoteClientMethod tRemoteMethod = null;
            try
            {
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Infomation, "Server Service Manager 채널을 생성 합니다.");
                m_RemoteGateway = new MKRemote(E_RemoteType.TCPRemote, GlobalClass.m_SystemInfo.ServerIP, GlobalClass.m_SystemInfo.ServiceManagerUsePort, GlobalClass.m_SystemInfo.ServiceManagerChannelName);
                while (tCount < 10)
                {
                    // server의 Remote Server를 엽니다
                    if (m_RemoteGateway.StartRemoteServer(out tResult) != E_RemoteError.Success)
                    {
                        GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, string.Concat("Server Service Manager 채널을 생성할 수 없습니다. : ", tResult));
                        Thread.Sleep(3000);
                        tCount++;
                    }
                    else
                        break;
                }

                if (m_RemoteGateway == null)
                {
                    GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, string.Format("Server Service Manager IP: {0}  PortNo: {1}  ChannelName: {2}에 연결 할 수 없습니다.", GlobalClass.m_SystemInfo.ServerIP, GlobalClass.m_SystemInfo.ServiceManagerUsePort, GlobalClass.m_SystemInfo.ServiceManagerChannelName));
                    return false;
                }

                //Daemon 원격 메소드를 설정합니다.
                tRemoteMethod = new RemoteClientMethod();
                tRemoteMethod.SetDaemonListRequestHandler(DaemonListRequest);
                m_RemoteGateway.ServerObject = tRemoteMethod;

                return true;
            }
            catch (Exception ex)
            {
                GlobalClass.m_LogProcess.PrintLog(E_FileLogType.Error, ex.ToString());
                return false;
            }
        }

        /// <summary>
        /// Server에서 동작중인 Daemon들의 정보 목록을 요청합니다.
        /// </summary>
        /// <returns></returns>
        private byte[] DaemonListRequest()
        {
            DaemonProcessInfoCollection tDaemonList = GlobalClass.s_DaemonProcessManager.DaemonProcessList;

            if (tDaemonList != null)
            {
                return ObjectConverter.GetBytes(new DaemonProcessInfoCollection(tDaemonList));
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
            GlobalClass.StopThread(m_RequestProcessThread);
            GlobalClass.StopThread(m_DaemonHelathCheckThread);
        }
    }
}
