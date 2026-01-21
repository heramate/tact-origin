using System;
using System.Collections.Generic;
using System.Text;
using MKLibrary.MKNetwork;
using RACTCommonClass;
using System.Threading;

namespace RACTDaemonProcess
{
    public class ServerCommunicationProcess
    {
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public ServerCommunicationProcess(){}


        public bool Start()
        {
            if (ProcessLoginResult(TryServerConnect()))
            {
                DaemonGlobal.s_IsServerConnected = true;
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
            int tTryCount = 0;
            RemoteClientMethod tSPO = null;
            string tErrorString = string.Empty;
            DateTime tSDate = DateTime.Now;

            if (DaemonGlobal.s_ServerRemoteGateway == null)
            {
                // 2019.01.25 KwonTaeSuk 환경설정파일 정리(DaemonLauncherConfig.xml, DaemonProcessConfig.xml)
                //DaemonGlobal.s_FileLogProcess.PrintLog(string.Concat("서버에 접속을 시도 합니다. ", DaemonGlobal.s_DaemonConfig.ServerIP, ":", DaemonGlobal.s_DaemonConfig.ServerPort, DaemonGlobal.s_DaemonConfig.ServerChannel));
                //DaemonGlobal.s_ServerRemoteGateway = new MKRemote(E_RemoteType.TCPRemote, DaemonGlobal.s_DaemonConfig.ServerIP, DaemonGlobal.s_DaemonConfig.ServerPort, DaemonGlobal.s_DaemonConfig.ServerChannel);
                DaemonGlobal.s_FileLogProcess.PrintLog(string.Concat("서버에 접속을 시도 합니다. ", DaemonGlobal.s_DaemonConfig.ServerIP, ":", DaemonGlobal.s_DaemonConfig.ServerDaemonPort, DaemonGlobal.s_DaemonConfig.ServerDaemonChannelName));
                DaemonGlobal.s_ServerRemoteGateway = new MKRemote(E_RemoteType.TCPRemote, DaemonGlobal.s_DaemonConfig.ServerIP, DaemonGlobal.s_DaemonConfig.ServerDaemonPort, DaemonGlobal.s_DaemonConfig.ServerDaemonChannelName);
            }

            if (DaemonGlobal.s_ServerRemoteGateway == null)
            {
                // s_FileLog.PrintLogEnter("IP:" + s_ServerIP + " PortNo:" + s_ServerPort + " ChannelName : " + s_ChannelName +"에 연결 할 수 없습니다.");
                return E_ConnectError.LocalFail;
            }
            else
            {
                while (DaemonGlobal.s_IsRun)
                {
                    try
                    {
                        //tTryCount++;
                        //if (tTryCount > 10)
                        //{
                        //    return E_ConnectError.ServerNoRun;
                        //}

                        if (DaemonGlobal.s_ServerRemoteGateway.ConnectServer(out tErrorString) != E_RemoteError.Success)
                        {
                            DaemonGlobal.s_FileLogProcess.PrintLog(string.Concat("서버에 접속하지 못했습니다 ", DaemonGlobal.s_DaemonConfig.ServerIP, ":", DaemonGlobal.s_DaemonConfig.ServerDaemonPort, DaemonGlobal.s_DaemonConfig.ServerDaemonChannelName));
                            // s_FileLog.PrintLogEnter(string.Concat("서버에 연결할 수 없습니다. 서버가 정상적으로 시작되었는지 또는 FireWall이 작동중인지 확인 하십시오. :", tErrorString));
                            //return E_ConnectError.LinkFail;
                        }
                        else
                        {

                            tErrorString = string.Empty;

                            tSPO = (RemoteClientMethod)DaemonGlobal.s_ServerRemoteGateway.ServerObject;
                            if (tSPO == null)
                            {
                                Thread.Sleep(3000);
                                continue;
                            }
                            ObjectConverter.GetObject(tSPO.CallDaemonResultMethod(0));
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        DaemonGlobal.s_FileLogProcess.PrintLog(string.Concat("서버에 접속하지 못했습니다 ", DaemonGlobal.s_DaemonConfig.ServerIP, ":", DaemonGlobal.s_DaemonConfig.ServerDaemonPort, DaemonGlobal.s_DaemonConfig.ServerDaemonChannelName));
                        DaemonGlobal.s_IsServerConnected = false;
                        //s_FileLog.PrintLogEnter("[E] TryServerConnect: " + ex.ToString());
                       //if (((TimeSpan)DateTime.Now.Subtract(tSDate)).TotalSeconds > 60)
                       //{
                            //return E_ConnectError.LinkFail;
                        //}
                    }
                }
                DaemonGlobal.s_FileLogProcess.PrintLog(string.Concat("서버에 접속완료 ", DaemonGlobal.s_DaemonConfig.ServerIP, ":", DaemonGlobal.s_DaemonConfig.ServerDaemonPort, DaemonGlobal.s_DaemonConfig.ServerDaemonChannelName));
                return E_ConnectError.NoError;
            }
        }

        /// <summary>
        /// 로그인 결과를 처리 합니다.
        /// </summary>
        private bool ProcessLoginResult(object tResult)
        {
            try
            {
                if ((E_ConnectError)tResult == E_ConnectError.NoError)
                {
                    if (!DaemonGlobal.LoginConnect())
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return true;
        }
    }
}
