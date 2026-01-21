using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MKLibrary.MKData;
using System.ServiceProcess;
using System.Threading;
using RACTCommonClass;
using System.Collections;
using MKLibrary.MKNetwork;
using System.IO;
using RACTServer;

namespace RACTServerServiceManager
{
    public partial class ServiceManagerMainForm : Form
    {
        #region
        /// <summary>
        /// 로컬 서비스 컨트롤러 입니다.
        /// </summary>
        private ServiceController m_LocalServiceController = null;
        /// <summary>
        /// 서비스를 체크 합니다.
        /// </summary>
        private Thread m_CheckServiceThread;
        /// <summary>
        /// 서버에 접속 처리할 스레드 입니다.
        /// </summary>
        private Thread m_ServerConnectionThread = null;
        /// <summary>
        /// 결과 처리에서 사용할 스레드 입니다.
        /// </summary>
        private Thread m_ProcessServerResultThread = null;
        /// <summary>
        /// 서버 요청 전송용 스레드입니다.
        /// </summary>
        private Thread m_RequestSendThread = null;
        /// <summary>
        /// 결과 얻기 스래드 입니다.
        /// </summary>
        private Thread m_GetResultThread = null;
        /// <summary>
        /// 결과 큐 입니다.
        /// </summary>
        private Queue m_ResultQueue = new Queue();
        /// <summary>
        /// 서비스 매니저 실행 여부 입니다.
        /// </summary>
        private bool m_ISManagerStart = false;
        /// <summary>
        /// 서비스 실행 여부 입니다.
        /// </summary>
        private bool m_IsServiceStart = false;
        /// <summary>
        /// 서비스 이름 입니다.
        /// </summary>
        private string m_ServiceName = "RACS_Server";

        #endregion

        public ServiceManagerMainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes the control.
        /// </summary>
        public void InitializeControl()
        {
            m_ISManagerStart = true;
            ServiceManagerGlobal.m_ServerStartupPath = Application.StartupPath;
            this.Text = "ServiceManager  v." + GlobalClass.c_Version;//버전 표시

            if (Start())
            {
                ServiceManagerGlobal.s_MainForm = this;
                pnlConnectionData.InitializeControl();
                pnlDaemonDataList.InitializeControl();

                m_CheckServiceThread = new Thread(new ThreadStart(CheckServiceRunning));
                m_CheckServiceThread.Start();

                m_ServerConnectionThread = new Thread(new ThreadStart(ConnectRemoteServer));
                m_ServerConnectionThread.Start();
            }
        }


        /// <summary>
        /// 서버를 시작합니다.
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            try
            {
                ServiceManagerGlobal.m_LogProcess = new FileLogProcess(Application.StartupPath + "\\System Log", "ServerServiceManager");
                ServiceManagerGlobal.m_LogProcess.Start();
                ServiceManagerGlobal.m_LogProcess.PrintLog("서버를 시작합니다.");
                if (!InitializeServer())
                {
                    return false;
                }
                //ServiceManagerGlobal.m_IsRun = true;

                ///TODO 나중에 스레드 만들어어어어어
                //서버 스래드를 시작합니다.
                //m_StartServiceThread = new Thread(new ThreadStart(StartServiceManager));
                //m_StartServiceThread.Start();

                DisplayBaseData();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool InitializeServer()
        {
            if (!LoadSystemInfo())
                return false;
            else
                return true;
        }

        /// <summary>
        /// 서버 설정 정보를 로드합니다.
        /// </summary>
        /// <returns></returns>
        public bool LoadSystemInfo()
        {
            ServiceManagerGlobal.m_LogProcess.PrintLog("서버 설정 정보를 로드 합니다.");
            ArrayList tSystemInfos = null;
            E_XmlError tXmlError = E_XmlError.Success;
            try
            {
                // 참조할 Config 파일 정보
                FileInfo tFileInfo = new FileInfo(ServiceManagerGlobal.m_ServerStartupPath + ServiceManagerGlobal.c_SystemConfigFileName);
                if (!tFileInfo.Exists) MKXML.ObjectToXML(tFileInfo.FullName, new SystemConfig());

                // 파일로 부터 Config객체 형태로 변환
                tSystemInfos = MKXML.ObjectFromXML(ServiceManagerGlobal.m_ServerStartupPath + ServiceManagerGlobal.c_SystemConfigFileName, 
                    typeof(SystemConfig), out tXmlError);

                if (tSystemInfos == null) return false;
                if (tSystemInfos.Count == 0) return false;
                // System 정보 저장
                ServiceManagerGlobal.SetSystemInfo((SystemConfig)tSystemInfos[0]);

                return true;
            }
            catch (Exception ex)
            {
                ServiceManagerGlobal.m_LogProcess.PrintLog(E_FileLogType.Error, ex.ToString());
                return false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        /// <summary>
        /// Loads the config info.
        /// </summary>
        private void LoadConfigInfo()
        {
           
            if (!Start())
            {
                MessageBox.Show("에러 발생");
            }
            else
            {
                DisplayBaseData();
            }
        }

        /// <summary>
        /// 기본 접속 정보를 폼에 표시합니다.
        /// </summary>
        private void DisplayBaseData()
        {
            pnlConnectionData.DisplayBaseData();
        }

        /// <summary>
        /// 서비스를 시작합니다.
        /// </summary>
        public bool StartService()
        {
            MKXML.ObjectToXML(ServiceManagerGlobal.m_ServerStartupPath + ServiceManagerGlobal.c_SystemConfigFileName, ServiceManagerGlobal.m_SystemInfo);



            try
            {
                int tCount = 0;
                if (m_LocalServiceController == null) return false;
                // m_LocalServiceController.Stop();
                if (m_LocalServiceController.Status == ServiceControllerStatus.Stopped)
                {
                    m_LocalServiceController.Start();
                }

                while (tCount < 180)
                {
                    tCount++;
                    Thread.Sleep(500);

                    m_LocalServiceController.Refresh();
                    if (m_LocalServiceController.Status == ServiceControllerStatus.Running)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
           
            return m_IsServiceStart;
        }

        /// <summary>
        /// 서비스를 중지 시킵니다.
        /// </summary>
        public void StopService()
        {
            if (MessageBox.Show(this, "RACT Daemon Launcher 서비스를 종료 하시겠습니까?", "AMAS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                bool tResult = false;
                int tCount = 0;

                try
                {
                    if (m_LocalServiceController != null)
                    {
                        m_LocalServiceController.Stop();
                        while (true)
                        {
                            tCount++;
                            Thread.Sleep(1000);
                            m_LocalServiceController.Refresh();
                            if (m_LocalServiceController.Status == ServiceControllerStatus.Stopped)
                            {
                                tResult = true;
                                break;
                            }

                            if (tCount > 30)
                            {
                                tResult = false;
                                break;
                            }
                            Thread.Sleep(1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }

                if (!tResult)
                {
                    DialogResult tDialogResult = MessageBox.Show(this,
                        "Server Service Manager가 중지되지 않았습니다. 재 시도 하시겠습니까?",
                        "Server Service Manager", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question);

                    if (tDialogResult == DialogResult.Cancel)
                    {
                        return;
                    }
                    else if (tDialogResult == DialogResult.Retry)
                    {
                        StopService();
                        return;
                    }
                }

                pnlDaemonDataList.SetGridRowCount(1);
                ServiceManagerGlobal.s_IsRun = false;
            }
        }

        /// <summary>
        /// 로컬 PC에서 서비스 정보를 얻기 합니다.
        /// </summary>
        /// <returns>로컬 PC에 서비스가 존재하는지의 여부 입니다.</returns>
        private bool GetLocalService()
        {
            m_LocalServiceController = GetService(m_ServiceName);
            if (ServiceManagerGlobal.m_LogProcess != null) ServiceManagerGlobal.m_LogProcess.PrintLog(E_FileLogType.Error, string.Format("서비스목록에서 [{0}]를 찾을 수 없습니다.", m_ServiceName));
            return m_LocalServiceController != null;
        }

        /// <summary>
        /// 로컬 PC에 지정한 이름의 서비스가 존재하는지 확인 합니다.
        /// </summary>
        /// <param name="aServiceName">서비스 이름 입니다.</param>
        /// <returns>서비스 컨트롤 객체입니다.</returns>
        public ServiceController GetService(string aServiceName)
        {
            ServiceController[] tServices = ServiceController.GetServices();

            for (int i = 0; i < tServices.Length; i++)
            {
                if (tServices[i].ServiceName.ToUpper() == aServiceName.ToUpper())
                {
                    return tServices[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 서비스의 현재 실행 유무를 가져옵니다.
        /// </summary>
        /// <returns></returns>
        private bool ServiceIsStarted()
        {
            m_LocalServiceController = GetService(m_ServiceName);
            if (m_LocalServiceController == null)
            {
                if (ServiceManagerGlobal.m_LogProcess != null) ServiceManagerGlobal.m_LogProcess.PrintLog(E_FileLogType.Error, string.Format("서비스목록에서 [{0}]를 찾을 수 없습니다.", m_ServiceName));
                return false;
            }
           

            return m_LocalServiceController.Status == ServiceControllerStatus.Running ? true : false;
        }

        /// <summary>
        /// Service가 현재 실행 중인지 체크합니다.
        /// </summary>
        private void CheckServiceRunning()
        {
            // TODO : 매니저 종료하면 m_ISManagerStart = false 시켜줘야 
            //ServiceManagerGlobal.s_IsRun = true;
            while (m_ISManagerStart)
            {
                // 서비스가 실행중이면
                if (ServiceIsStarted())
                {
                    ServiceManagerGlobal.s_IsRun = true;
                    m_IsServiceStart = true;
                    pnlConnectionData.EnableButton(false);
                }
                // 서비스가 현재 중지중이면
                else
                {
                    ServiceManagerGlobal.s_IsRun = false;
                    m_IsServiceStart = false;
                    pnlConnectionData.EnableButton(true);
                    Thread.Sleep(1000);
                }
            }
        }

        /// <summary>
        /// 서버 접속을 처리합니다.
        /// </summary>
        private void ConnectRemoteServer()
        {
            ServiceManagerGlobal.s_ServerCommunicationProcess = new ServerCommunicationProcess();

            // 서비스매니저가 실행중인 동안
            while (m_ISManagerStart)
            {
                try
                {
                    // 서비스가 실행중인 동안
                    if (ServiceManagerGlobal.s_IsRun)
                    {
                        // 서버가 멈췄으면
                        if (!ServiceManagerGlobal.s_IsServerConnected)
                        {
                            // 서버에 접속 시도
                            if (ServiceManagerGlobal.s_ServerCommunicationProcess.Start())
                            {
                                StopRequestSend();

                                StartRequestSend();
                                StartServerProcessResult();
                                StartGetResult();
                            }
                        }
                        else
                        {
                            CheckDaemonList();
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());

                }
                Thread.Sleep(1000);
            }
        }

        private void CheckDaemonList()
        {
            try
            {
                RemoteClientMethod tSPO = (RemoteClientMethod)ServiceManagerGlobal.s_ServerRemoteGateway.ServerObject;
                ServiceManagerGlobal.s_RunningDaemonList = (DaemonProcessInfoCollection)ObjectConverter.GetObject(tSPO.CallDaemonListRequestMethod());

                pnlDaemonDataList.DisplayList();
            }
            catch (Exception ex)
            {
                ServiceManagerGlobal.s_IsServerConnected = false;
            }
        }

        /// <summary>
        /// 서버에 요청 스래드를 시작합니다.
        /// </summary>
        private void StartRequestSend()
        {

            m_RequestSendThread = new Thread(new ThreadStart(ProcessRequestSendToServer));
            m_RequestSendThread.Start();
        }

        private void StopRequestSend()
        {
            if (m_RequestSendThread != null && m_RequestSendThread.IsAlive)
            {
                try
                {
                    m_RequestSendThread.Abort();
                }
                catch { }
                m_RequestSendThread = null;
            }
        }

        /// <summary>
        /// 결과 처리 스래드를 시작합니다.
        /// </summary>
        private void StartServerProcessResult()
        {
            m_ProcessServerResultThread = new Thread(new ThreadStart(ProcessServerResult));
            m_ProcessServerResultThread.Start();
        }

        /// <summary>
        /// 결과 받기 스래드를 시작합니다.
        /// </summary>
        private void StartGetResult()
        {
            m_GetResultThread = new Thread(new ThreadStart(ProcessGetResultFromServer));
            m_GetResultThread.Start();
        }

        /// <summary>
        /// 서버에 요청을 전송합니다.
        /// </summary>
        private void ProcessRequestSendToServer()
        {
            RemoteClientMethod tRemoteClientMethod = null;
            object tSendObject = null;

            while (ServiceManagerGlobal.s_IsRun)
            {
                tSendObject = null;

                lock (ServiceManagerGlobal.s_RequestQueue)
                {
                    if (ServiceManagerGlobal.s_RequestQueue.Count > 0)
                    {
                        tSendObject = ServiceManagerGlobal.s_RequestQueue.Dequeue();
                    }
                }
                if (tSendObject != null)
                {
                    try
                    {
                        tRemoteClientMethod = (RemoteClientMethod)ServiceManagerGlobal.s_ServerRemoteGateway.ServerObject;
                        tRemoteClientMethod.CallRequestMethod(ObjectConverter.GetBytes(tSendObject));
                    }
                    catch (Exception ex)
                    {
                    }
                }
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// 결과를 처리 합니다.
        /// </summary>
        private void ProcessServerResult()
        {
            ResultCommunicationData tResult = null;
            object tObject = null;

            ISenderObject tSender = null;
            bool tIsWorked = true;

            while (ServiceManagerGlobal.s_IsRun)
            {
                try
                {
                    tResult = null;
                    tObject = null;
                    tIsWorked = true;

                    lock (m_ResultQueue.SyncRoot)
                    {
                        if (m_ResultQueue.Count < 1)
                        {
                            tIsWorked = false;
                            continue;
                        }

                        tObject = ObjectConverter.GetObject((byte[])m_ResultQueue.Dequeue());

                        Console.WriteLine("[III]ProcessServerResult : " + tObject.GetType().ToString());

                        if (tObject.GetType().Equals(typeof(ResultCommunicationData)))
                        {
                            tResult = tObject as ResultCommunicationData;
                        }
                        else
                        {
                            tIsWorked = false;
                            continue;
                        }
                    }

                    if (tResult != null)
                    {
                        if (tResult.OwnerKey != 0)
                        {
                            tSender = null;
                            lock (ServiceManagerGlobal.s_SenderList)
                            {
                                tSender = (ISenderObject)ServiceManagerGlobal.s_SenderList[tResult.OwnerKey];
                            }
                            if (tSender != null)
                            {
                                tSender.ResultReceiver(tResult);
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
                finally
                {
                    if (!tIsWorked) Thread.Sleep(1);
                }
            }
        }

        /// <summary>
        /// 서버로부터 결과 받음을 처리 합니다.
        /// </summary>
        private void ProcessGetResultFromServer()
        {
            RemoteClientMethod tRemoteClientMethod = null;
            ArrayList tResultData = null;
            byte[] tResultDatas = null;

            int tResultFailCount = 0;

            while (ServiceManagerGlobal.s_IsRun)
            {
                if (ServiceManagerGlobal.s_IsServerConnected)
                {
                    try
                    {
                        tResultData = null;
                        tRemoteClientMethod = (RemoteClientMethod)ServiceManagerGlobal.s_ServerRemoteGateway.ServerObject; ;
                        //tResultDatas = tRemoteClientMethod.CallServiceManagerResultMethod(ServiceManagerGlobal.s_DaemonProcessInfo.DaemonID);
                        if (tResultDatas != null) tResultData = (ArrayList)ObjectConverter.GetObject(tResultDatas);
                    }
                    catch (Exception ex)
                    {
                        tRemoteClientMethod = null;
                        if (ServiceManagerGlobal.s_IsServerConnected)
                        {
                            tResultFailCount++;
                            if (tResultFailCount > 3)
                            {
                                ServiceManagerGlobal.s_IsServerConnected = false;
                                TryServerConnect();
                            }
                        }
                    }

                    if (tResultData != null)
                    {
                        try
                        {
                            if (tResultData.Count > 0)
                            {

                                lock (m_ResultQueue.SyncRoot)
                                {
                                    for (int i = 0; i < tResultData.Count; i++)
                                    {
                                        m_ResultQueue.Enqueue(tResultData[i]);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // AppGlobal.s_FileLog.PrintLogEnter("ProcessGetResultFromServer-Data: " + ex.ToString());
                        }
                    }

                }
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// 서버에 연결을 시도 합니다.
        /// </summary>
        /// <returns>연결 시도 성공 여부 입니다.</returns>
        public E_ConnectError TryServerConnect()
        {
            RemoteClientMethod tSPO = null;
            string tErrorString = string.Empty;
            DateTime tSDate = DateTime.Now;

            if (ServiceManagerGlobal.s_ServerRemoteGateway == null)
            {
                ServiceManagerGlobal.s_ServerRemoteGateway = new MKRemote(E_RemoteType.TCPRemote, ServiceManagerGlobal.m_SystemInfo.ServerIP, ServiceManagerGlobal.m_SystemInfo.ServiceManagerUsePort, ServiceManagerGlobal.m_SystemInfo.ServiceManagerChannelName);
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

                            tErrorString = string.Empty;

                            tSPO = (RemoteClientMethod)ServiceManagerGlobal.s_ServerRemoteGateway.ServerObject;
                            if (tSPO == null)
                            {
                                Thread.Sleep(3000);
                                continue;
                            }
                            ObjectConverter.GetObject(tSPO.CallDaemonListRequestMethod());
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        ServiceManagerGlobal.s_IsServerConnected = false;
                        //s_FileLog.PrintLogEnter("[E] TryServerConnect: " + ex.ToString());
                        //if (((TimeSpan)DateTime.Now.Subtract(tSDate)).TotalSeconds > 60)
                        //{
                        //return E_ConnectError.LinkFail;
                        //}
                    }
                }
                return E_ConnectError.NoError;
            }
        }

        private void OnShowForm()
        {
            this.Activate();
            this.Show();
            this.Refresh();
        }

        private void noiRACT_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            OnShowForm();
        }

        private void ServiceManagerMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_ISManagerStart)
            {
                e.Cancel = true;
                this.Hide();
                noiRACT.Visible = true;
            }
        }

        private void noiRACT_MouseClick(object sender, MouseEventArgs e)
        {
            mnuStart.Enabled = !ServiceManagerGlobal.s_IsRun;
            mnuStop.Enabled = ServiceManagerGlobal.s_IsRun;
        }

        private void mnuStart_Click(object sender, EventArgs e)
        {
            pnlConnectionData.StartService();
        }

        private void mnuStop_Click(object sender, EventArgs e)
        {
            pnlConnectionData.StopService();
        }

        private void mnuOpen_Click(object sender, EventArgs e)
        {
            OnShowForm();
        }

        private void mnuClose_Click(object sender, EventArgs e)
        {
            m_ISManagerStart = false;
            ServiceManagerGlobal.m_LogProcess.Stop();
            this.Close();
        }

        private void pnlConnectionData_Load(object sender, EventArgs e)
        {
         
        }
    }
}