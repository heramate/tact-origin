using System;
using System.Collections; // Queue<T>, ArrayList
using System.Collections.Generic;
using System.Threading;
using System.IO; //FileInfo
using MKLibrary.MKData;
using RACTCommonClass;
using MKLibrary.MKNetwork;  //MKUdpSocket
using System.Net; //IPAddress


namespace TACT.KeepAliveServer
{
    public class KeepAliveServer //: IDisposable
    {
        #region 변수 선언, 생성자, 소멸자
        /// <summary>
        /// KeepAlive서버를 시작하는 스레드
        /// </summary>
        private Thread m_ServerThread = null;
        /// <summary>
        /// 서버 개체를 생성합니다.
        /// </summary>
        /// <param name="aStartupPath">시작 경로입니다.</param>
        public KeepAliveServer(string aStartupPath)
        {
            GlobalClass.m_StartupPath = aStartupPath;

        }

        /// <summary>
        /// 종료 처리 합니다.
        /// </summary>
        public void Dispose()
        {
            Stop();
        }


        #endregion 변수 선언, 생성자, 소멸자 -------------------------------------

        /// <summary>
        /// 서버를 시작합니다.
        /// </summary>
        public bool Start()
        {
            try
            {
                GlobalClass.PrintLogInfo("■ 서버를 시작합니다 ---------------\r\n\r\n");
                // 파일로깅 시작
                System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(GlobalClass.m_StartupPath));
                if (GlobalClass.m_StartupPath.Length < 1) return false;

                GlobalClass.m_LogProcess = new FileLogProcess(GlobalClass.m_StartupPath + "\\Log", "TACTKeepAliveServer");
                GlobalClass.m_LogProcess.Start();
                GlobalClass.PrintLogInfo("파일로깅을 시작합니다.");

                // 환경설정 파일 로드
                if (!LoadSystemInfo()) return false;

                // DB연결
                GlobalClass.PrintLogInfo("DB연결을 시작합니다.");
                if(!DBWorker.OpenDBPool(GlobalClass.m_SystemInfo.DBConnectionCount * 2, GlobalClass.m_SystemInfo.DBServerIP,
                                         GlobalClass.m_SystemInfo.DBName, GlobalClass.m_SystemInfo.UserID,
                                         GlobalClass.m_SystemInfo.Password)) return false;

                // DB기초데이터 로딩
                //GlobalClass.PrintLogInfo("기초데이터 로딩을 시작합니다.");
                //if (!LoadBaseData()) return false;

                // DB로깅
                GlobalClass.PrintLogInfo("DB로깅을 시작합니다.");
                GlobalClass.m_DBLogProcess = new DBLogProcess(DBWorker.DBPool);

                // KeepAlive서버 스레드를 시작합니다.
                GlobalClass.PrintLogInfo("■ 서버 스레드를 시작합니다 ---------------");
                m_ServerThread = new Thread(new ThreadStart(_StartServer));
                m_ServerThread.Start();

                return true;
            }
            catch (Exception ex)
            {
                GlobalClass.PrintLogException("[KeepAliveServer.Start] :", ex);
                return false;
            }
        }

        /// <summary>
        /// 서버를 시작합니다.
        /// </summary>
        private void _StartServer()
        {
            try
            {
                GlobalClass.m_IsServerStop = false;

                GlobalClass.PrintLogInfo( "Keep-Alive 서버 스레드를 시작합니다.");
                GlobalClass.m_KeepAliveCommThread = new KeepAliveCommProcess(GlobalClass.m_SystemInfo.ServerPort);
                GlobalClass.m_KeepAliveCommThread.Start();

                GlobalClass.PrintLogInfo( "데몬통신 채널을 시작합니다.");
                GlobalClass.m_DaemonCommProcess = new DaemonCommProcess();
                GlobalClass.m_DaemonCommProcess.Start();

                GlobalClass.PrintLogInfo( "SSH터널 포트 관리자를 시작합니다.");
                GlobalClass.m_TunnelManager = new SSHTunnelManager(GlobalClass.m_SystemInfo.SSHTunnelPortMin, GlobalClass.m_SystemInfo.SSHTunnelPortMax);
                GlobalClass.m_TunnelManager.Start();

            }
            catch (Exception ex)
            {
                GlobalClass.PrintLogException("[KeepAliveServer.StartServer] :", ex);
            }
        }

        /// <summary>
        /// 서버를 종료합니다.
        /// </summary>
        public void Stop()
        {
            GlobalClass.m_IsServerStop = true;

            GlobalClass.PrintLogInfo("■ 서버 스레드를 중지합니다 ---------------\r\n");
            GlobalClass.PrintLogInfo( "Keep-Alive 서버 스레드를 중지합니다.");
            GlobalClass.m_KeepAliveCommThread.Stop();
            GlobalClass.m_KeepAliveCommThread = null;

            GlobalClass.PrintLogInfo( "데몬통신 채널을 중지합니다.");
            GlobalClass.m_DaemonCommProcess.Stop();
            GlobalClass.m_DaemonCommProcess = null;

            GlobalClass.PrintLogInfo( "SSH터널 포트 관리자를 중지합니다.");
            GlobalClass.m_TunnelManager.Stop();
            GlobalClass.m_TunnelManager = null;

            GlobalClass.PrintLogInfo("DB로깅을 중지합니다.");
            if (GlobalClass.m_DBLogProcess != null) 
            {
                GlobalClass.m_DBLogProcess.Dispose();
                GlobalClass.m_DBLogProcess = null;
            }
            GlobalClass.StopThread(m_ServerThread);


            GlobalClass.PrintLogInfo("DB연결을 중지합니다.");
            DBWorker.StopDBPool();

            //GlobalClass.PrintLogInfo("기초데이터를 삭제합니다.");
            //GlobalClass.m_KeepAliveInfoCollection.Clear();

            GlobalClass.PrintLogInfo("파일로깅을 중지합니다.");
            if (GlobalClass.m_LogProcess != null)
            {
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
            GlobalClass.PrintLogInfo("환경설정 파일(SystemInfo.xml) 로드 합니다.");

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
                GlobalClass.m_FileLogDetailYN = GlobalClass.m_SystemInfo.FileLogDetailYN;

                return true;
            }
            catch (Exception ex)
            {
                GlobalClass.PrintLogException("[KeepAliveServer.LoadSystemInfo] ", ex);
                return false;
            }
        }

        public bool LoadBaseData()
        {
            //if (!DBWorker.SelectKeepAliveCollections(out GlobalClass.m_KeepAliveInfoCollection))
            //{
            //    GlobalClass.PrintLogInfo("기초데이터(LTE장치정보) 로딩에 실패했습니다.");
            //    return false;
            //}

            return true;
        }

        //데몬이 SSH터널 요청을 보낸 상황을 만든다.
        public void _Test1(string aDeviceIP)
        {
            GlobalClass.m_DaemonCommProcess._AddRequestTest(aDeviceIP);
        }

        /// <summary>
        /// SSH터널 대기중인 장비 목록 정보 얻기
        /// </summary>
        /// <returns>KeepAliveMsg 리스트</returns>
        public ArrayList GetKeepAliveWatingList()
        {
            return GlobalClass.m_KeepAliveCommThread.GetKeepAliveWatingList();
        }

        /// <summary>
        /// SSH터널 요청 삭제
        /// </summary>
        /// <returns></returns>
        public void ClearAllRequest()
        {
            GlobalClass.m_KeepAliveCommThread.ClearAllRequest();
            GlobalClass.m_TunnelManager.ClearAll();
        }

        /// <summary>
        /// 감시중인 터널포트 정보
        /// </summary>
        /// <returns></returns>
        public ArrayList GetTunnelPortInfoList()
        {
            return GlobalClass.m_TunnelManager.GetTunnelPortList();
        }

        
    } // End of class KeepAliveServer
}
