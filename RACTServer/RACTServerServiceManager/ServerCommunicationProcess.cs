using System;
using System.Collections.Generic;
using System.Text;
using MKLibrary.MKNetwork;
using RACTCommonClass;
using System.Threading;

namespace RACTServerServiceManager
{
    class ServerCommunicationProcess
    {
        
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public ServerCommunicationProcess() { }


        public bool Start()
        {
            if ((E_ConnectError)TryServerConnect() == E_ConnectError.NoError)
            {
                ServiceManagerGlobal.s_IsServerConnected = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 서버에 연결을 시도 합니다.
        /// </summary>
        /// <returns>연결 시도 성공 여부 입니다.</returns>
        public E_ConnectError TryServerConnect()
        {
            //int tTryCount = 0;
            RemoteClientMethod tSPO = null;
            string tErrorString = string.Empty;
            DateTime tSDate = DateTime.Now;

            if (ServiceManagerGlobal.s_ServerRemoteGateway == null)
            {
                ServiceManagerGlobal.s_ServerRemoteGateway = new MKRemote(E_RemoteType.TCPRemote, ServiceManagerGlobal.m_SystemInfo.ServerIP,
                    ServiceManagerGlobal.m_SystemInfo.ServiceManagerUsePort, ServiceManagerGlobal.m_SystemInfo.ServiceManagerChannelName);
            }

            if (ServiceManagerGlobal.s_ServerRemoteGateway == null)
            {
                // s_FileLog.PrintLogEnter("IP:" + s_ServerIP + " PortNo:" + s_ServerPort + " ChannelName : " + s_ChannelName +"에 연결 할 수 없습니다.");
                return E_ConnectError.LocalFail;
            }
            else
            {
                while (ServiceManagerGlobal.s_IsRun)
                {
                    try
                    {
                        //tTryCount++;
                        //if (tTryCount > 10)
                        //{
                        //    return E_ConnectError.ServerNoRun;
                        //}

                        if (ServiceManagerGlobal.s_ServerRemoteGateway.ConnectServer(out tErrorString) != E_RemoteError.Success)
                        {
                            // s_FileLog.PrintLogEnter(string.Concat("서버에 연결할 수 없습니다. 서버가 정상적으로 시작되었는지 또는 FireWall이 작동중인지 확인 하십시오. :", tErrorString));
                            //return E_ConnectError.LinkFail;
                        }
                        else
                        {

                            //tErrorString = string.Empty;

                            //tSPO = (RemoteClientMethod)ServiceManagerGlobal.s_ServerRemoteGateway.ServerObject;
                            //if (tSPO == null)
                            //{
                            //    Thread.Sleep(3000);
                            //    continue;
                            //}

                            //ServiceManagerGlobal.s_RunningDaemonList = (DaemonProcessInfoCollection)ObjectConverter.GetObject(tSPO.CallDaemonListRequestMethod());
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        ServiceManagerGlobal.s_IsServerConnected = false;
                        //s_FileLog.PrintLogEnter("[E] TryServerConnect: " + ex.ToString());
                        if (((TimeSpan)DateTime.Now.Subtract(tSDate)).TotalSeconds > 60)
                        {
                            return E_ConnectError.LinkFail;
                        }
                    }
                }
                return E_ConnectError.NoError;
            }
        }
    }
}
