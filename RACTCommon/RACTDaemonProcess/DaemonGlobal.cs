using System;
using System.Collections.Generic;
using System.Text;
using MKLibrary.MKNetwork;
using RACTCommonClass;
using System.Threading;

namespace RACTDaemonProcess
{
    public static class DaemonGlobal
    {
        /// <summary>
        /// 데몬 정보 입니다.
        /// </summary>
        public static DaemonConfig s_DaemonConfig = null;
        /// <summary>
        /// 데몬 실행 여부 입니다.
        /// </summary>
        public static bool s_IsRun = false;
        /// <summary>
        /// Telnet 명령 처리할 클래스 입니다.
        /// </summary>
        public static TelnetProcessor.TelnetProcessor s_TelnetProcessor = null;
        /// <summary>
        /// 클라이언트와 통신할 프로세서 입니다.
        /// </summary>
        public static ClientCommunicationProcess s_ClientCommunicationProcess = null;
        /// <summary>
        /// 서버와 통신할 프로세서 입니다.
        /// </summary>
        public static ServerCommunicationProcess s_ServerCommunicationProcess = null;
        /// <summary>
        /// 리모트통신을 위한 원격객체입니다.
        /// </summary>
        public static MKRemote s_ServerRemoteGateway = null;
        /// <summary>
        /// 요청 큐 입니다.
        /// </summary>
        public static Queue<CommunicationData> s_RequestQueue = new Queue<CommunicationData>();
        /// <summary>
        /// 요청자 목록 입니다.
        /// </summary>
        public static Dictionary<int, ISenderObject> s_SenderList = new Dictionary<int, ISenderObject>();
        /// <summary>
        /// 서버에 접속 되었는지 여부 입니다.
        /// </summary>
        public static bool s_IsServerConnected;
        /// <summary>
        /// 데몬 정보 입니다.
        /// </summary>
        public static DaemonProcessInfo s_DaemonProcessInfo = null;
        /// <summary>
        /// 로그 저장용 프로세서 입니다.
        /// </summary>
        public static FileLogProcess s_FileLogProcess = null;
        /// <summary>
        /// 세션 관리 타임 아웃 시간 입니다.
        /// </summary>
        public static readonly int s_HealthCheckTimeOut = 25;
        /// <summary>
        /// 2018-10-29 KANGBONGHAN
        /// Kam서버와 통신할 프로세서 입니다.
        /// </summary>
        public static KamServerCommunicationProcess s_KamServerCommunicationProcess = null;
		/// <summary>
        /// Kam서버와 리모트통신을 위한 원격객체입니다.
        /// </summary>
        public static MKRemote s_KamServerRemoteGateway = null;
        /// <summary>
        /// KamServerCommunication Thread 실행 여부  
        /// </summary>
        public static bool s_IsKamRun = false;
        /// <summary>
        /// KAM 큐 입니다.
        /// </summary>
        public static Queue<CommunicationData> s_RequestKamQueue = new Queue<CommunicationData>();
        /// <summary>
        /// KAM 서버에 접속 되었는지 여부 입니다.
        /// </summary>
        public static bool s_IsKamServerConnected;

        /// <summary>
        /// 지정한 클라이언트에 결과를 전송합니다.
        /// </summary>		
        /// <param name="aResult">결과 데이터 입니다.</param>
        public static void SendResultClient(ResultCommunicationData aResult)
        {
            if (s_ClientCommunicationProcess != null)
            {
                s_ClientCommunicationProcess.SendResultClient(aResult);
            }
            else
            {
                aResult = null;
            }
        }

        /// <summary>
        /// 터널링 요청 결과 반환 후 클라이언트의 요청을 전송 합니다.
        /// </summary>		
        /// <param name="aResult">결과 데이터 입니다.</param>
        public static void SendRequsetSSHTunnel(RequestCommunicationData aResult)
        {
            if (s_ClientCommunicationProcess != null)
            {
                s_ClientCommunicationProcess.RequsetSSHTunnelReceiver(aResult);
            }
            else
            {
                aResult = null;
            }
        }

        /// <summary>
        /// 요청 데이터를 전송합니다.
        /// </summary>
        /// <param name="vSender">전송자 입니다.</param>
        /// <param name="vCommunicationData">전송 데이터 입니다.</param>
        public static void SendDaemonRequestData(ISenderObject vSender, CommunicationData vCommunicationData)
        {
            if (vSender != null)
            {
                AddSender(vSender);
                vCommunicationData.OwnerKey = vSender.GetHashCode();
            }
            lock (s_RequestQueue)
            {
                s_RequestQueue.Enqueue(vCommunicationData);
            }
        }
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
        /// 요청 전송자를 추가 합니다.
        /// </summary>
        /// <param name="vSender">전송자 입니다.</param>
        public static void AddSender(ISenderObject vSender)
        {
            lock (s_SenderList)
            {
                if (!s_SenderList.ContainsKey(vSender.GetHashCode()))
                {
                    s_SenderList.Add(vSender.GetHashCode(), vSender);
                }
            }
        }

        /// <summary>
        /// 로그인 및 서버 연결을 처리 합니다.
        /// </summary>
        /// <param name="vID">사용자 아이디 입니다.</param>
        /// <param name="vPwd">사용자 패스워드 입니다.</param>
        /// <param name="vIPAddress">사용자 아이피 주소 입니다.</param>
        public static bool LoginConnect()
        {
            string tLogMessage = "";

            try
            {
                RemoteClientMethod tSPO = (RemoteClientMethod)DaemonGlobal.s_ServerRemoteGateway.ServerObject;

                // 2019.01.25 KwonTaeSuk 환경설정파일 정리(DaemonLauncherConfig.xml, DaemonProcessConfig.xml)
                //DaemonLoginResultInfo tConnectResult = (DaemonLoginResultInfo)ObjectConverter.GetObject(tSPO.CallDaemonConnectHandler(DaemonGlobal.s_DaemonConfig.DaemonIP, DaemonGlobal.s_DaemonConfig.DaemonPort,DaemonConfig.s_DaemonRemoteChannelName));
                DaemonLoginResultInfo tConnectResult = (DaemonLoginResultInfo)ObjectConverter.GetObject(tSPO.CallDaemonConnectHandler(DaemonGlobal.s_DaemonConfig.DaemonIP, DaemonGlobal.s_DaemonConfig.DaemonPort, DaemonGlobal.s_DaemonConfig.DaemonChannelName));
                if (tConnectResult.LoginResult != E_LoginResult.Success)
                {
                    return false;
                }
                else
                {
                    //AppGlobal.s_FileLog.PrintLogEnter("로그인에 " + s_LoginResult.Result.ToString() + "했습니다. " + DateTime.Now.ToString());
                    // AppGlobal.s_FileLog.PrintLogEnter("[Login] : " + tLogMessage);
                    DaemonGlobal.s_DaemonProcessInfo = tConnectResult.DaemonInfo;
                    return true;
                }
            }
            catch (Exception ex)
            {
                //AppGlobal.ShowMessage(AppGlobal.s_ClientMainForm, "서버와 연결할 수 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // AppGlobal.s_FileLog.PrintLogEnter(ex.ToString() + DateTime.Now.ToString());
            }
            return false;
        }
    }
}
