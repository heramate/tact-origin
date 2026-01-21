using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.ServiceProcess;
using System.Threading;
using System.Collections;
using MKLibrary.MKNetwork;
using RACTDaemonLauncher;
using RACTCommonClass;
using MKLibrary.MKData;
using System.IO; //FileInfo
using RACTDaemonProcess;

namespace RACTDaemonLauncherManager
{
    public partial class ServiceMain : Form
    {
        /// <summary>
        /// 로컬 서비스 컨트롤러 입니다.
        /// </summary>
        private ServiceController m_LocalServiceController = null;
        /// <summary>
        /// 서비스를 체크 합니다.
        /// </summary>
        private Thread m_CheckServiceThread = null;
        /// <summary>
        /// bool 타입 파라메터를 가지는 기본 델리게이트 입니다.
        /// </summary>
        public delegate void DefaultBoolHandler(bool aBoll);
        public delegate void RACTDaemonDisplayHandler(Hashtable aInfo);
        /// <summary>
        /// 서비스 이름 입니다.
        /// </summary>
        // 2019.02.01 KwonTaeSuk 환경설정파일 통합(DaemonConfig.xml, RACTDaemonProcess.DaemonConfig.cs)
        //private string m_LanucherServiceName = "RACS_Daemon_Launcher";
        /// <summary>
        /// Launcher 상태를 확인할 스레드 입니다.
        /// </summary>
        private Thread m_CheckLauncerStatus = null;
        /// <summary>
        /// 리모트 객체 입니다.
        /// </summary>
        private MKRemote m_RemoteGateway = null;
        /// <summary>
        /// 데몬 런처 객체
        /// </summary>
        private DaemonLauncher m_Launcher = null;
        /// <summary>
        /// 서비스 또는 데몬런처가 시작되었는지 여부
        /// </summary>
        private static bool s_IsRun = false;
        private bool m_DisposeFlag = false;

        public ServiceMain()
        {
            InitializeComponent();
            LauncherManagerAppGlobal.InitializeGridStyle(fgServerList);
            //this.Text = "RACS Daemon Launcher (Ver" + LauncherManagerAppGlobal.s_LauncherVersion + ")";
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            if (LauncherManagerAppGlobal.LoadSystemInfo())
            {
                txtLauncherIP.Text = LauncherManagerAppGlobal.s_ManagerConfig.LauncherIP;
                txtLauncherPort.Text = LauncherManagerAppGlobal.s_ManagerConfig.LauncherPort.ToString();
                txtLauncherChannelName.Text = LauncherManagerAppGlobal.s_ManagerConfig.LauncherChannelName;
                txtServiceName.Text = LauncherManagerAppGlobal.s_ManagerConfig.LauncherServiceName;
            }
            else
            {
                MessageBox.Show(string.Format("환경설정 파일 로딩에 실패했습니다({0})", DaemonConfig.s_DaemonConfigFileName));
            }
        }
      
        /// <summary>
        /// 서버의 리모트 활성화 여부를 확인 합니다.
        /// </summary>
        /// <returns>리모트 접속 여부 입니다.</returns>
        private void _CheckLauncherStatusProcess()
        {
            DaemonLauncherPartObject tSPO = null;
            DateTime tSDate = DateTime.Now;
            int tTryCount = 0;
            string tResult = string.Empty;

            while (s_IsRun)
            {
                if (m_RemoteGateway == null)
                {
                    // 2019.02.01 KwonTaeSuk 환경설정파일 통합(DaemonConfig.xml, RACTDaemonProcess.DaemonConfig.cs)
                    //m_RemoteGateway = new MKRemote(E_RemoteType.TCPRemote, LauncherManagerAppGlobal.s_ManagerConfig.LauncherIP, LauncherManagerAppGlobal.s_ManagerConfig.LauncherPort, AppGlobal.s_ServiceRemoteChannelName);
                    m_RemoteGateway = new MKRemote(E_RemoteType.TCPRemote, LauncherManagerAppGlobal.s_ManagerConfig.LauncherIP, LauncherManagerAppGlobal.s_ManagerConfig.LauncherPort, LauncherManagerAppGlobal.s_ManagerConfig.LauncherChannelName);
                    while (tTryCount < 10)
                    {
                        if (m_RemoteGateway.ConnectServer(out tResult) != E_RemoteError.Success)
                        {
                            Console.WriteLine(string.Format("[시도횟수:{0}/10] 데몬런처 채널에 연결이 실패했습니다{1}:{2} {3}\r\nErrMsg={4}", tTryCount, LauncherManagerAppGlobal.s_ManagerConfig.LauncherIP, LauncherManagerAppGlobal.s_ManagerConfig.LauncherPort, LauncherManagerAppGlobal.s_ManagerConfig.LauncherChannelName, tResult));
                            Thread.Sleep(3000);
                            tTryCount++;
                        }
                        else
                            break;
                    }
                }

                if (m_RemoteGateway != null)
                {

                    try
                    {
                        tSPO = (DaemonLauncherPartObject)m_RemoteGateway.ServerObject;
                        if (tSPO != null)
                        {
                            Hashtable tHealthCheckProcessCollection = (Hashtable)ObjectConverter.GetObject(tSPO.CallGetRACTDaemonListHandlerMethod());
                            if (tHealthCheckProcessCollection != null)
                            {
                                this.Invoke(new RACTDaemonDisplayHandler(ApplyServiceStatus), new object[] { tHealthCheckProcessCollection });
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        //m_RemoteGateway = null;
                    }
                }
                Thread.Sleep(1000);
            }
        }

        
        private void btnServiceStart_Click(object sender, EventArgs e)
        {
            if (s_IsRun)
            {
                MessageBox.Show("DaemonLauncher가 이미 실행중이어서 서비스를 시작할 수 없습니다.\r\n중지후 시작해주십시오.");
                return;
            }
            btnServiceStart.Enabled = false;

            string tResult;
            if (!StartService(out tResult))
            {
                MessageBox.Show("서비스 실행에 실패했습니다.\r\n" + tResult);
                btnServiceStart.Enabled = true;
                return;
            }

            /// 서비스/데몬상태 체크 스레드 실행
            s_IsRun = true;
            StartCheckServiceProcess();
        }

        /// <summary>
        /// 서버 시작을 처리합니다.
        /// </summary>
        public bool StartService(out string oResultMsg)
        {
            oResultMsg = string.Empty;

            try
            {
                int tCount = 0;
                if (m_LocalServiceController == null)
                {
                    GetLocalService();
                }
                if (m_LocalServiceController == null)
                {
                    oResultMsg = "서비스 객체를 찾지 못했습니다 [ServiceName: " + LauncherManagerAppGlobal.s_ManagerConfig.LauncherServiceName + "]";
                    return false;
                }

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
                Console.WriteLine(ex.ToString());
                oResultMsg = ex.ToString();
            }
            return false;
        }

        /// <summary>
        /// 체크 서비스 프로세스를 중지 합니다.
        /// </summary>
        private void StopCheckServiceProcess()
        {
            LauncherManagerAppGlobal.StopThread(m_CheckServiceThread);
            LauncherManagerAppGlobal.StopThread(m_CheckLauncerStatus);
        }

        /// <summary>
        /// 체크 서비스 프로세스를 시작 합니다.
        /// </summary>
        private void StartCheckServiceProcess()
        {
            if (m_CheckServiceThread == null)
            {
                m_CheckServiceThread = new Thread(new ThreadStart(_CheckServiceProcess));
                m_CheckServiceThread.Name = "DaemonLauncher Service Monitor";
                m_CheckServiceThread.Start();
            }

            if (m_CheckLauncerStatus == null)
            {
                m_CheckLauncerStatus = new Thread(new ThreadStart(_CheckLauncherStatusProcess));
                m_CheckLauncerStatus.Name = "DaemonProcess Status Monitor";
                m_CheckLauncerStatus.Start();
            }
        }

        /// <summary>
        /// 서비스가 실행 중인지 확인 합니다.
        /// </summary>
        private void _CheckServiceProcess()
        {
            try
            {
                //this.Invoke(new DefaultBoolHandler(IsServiceProcessing), new object[] { false });
                while (s_IsRun)
                {
                    try
                    {
                        if (m_LocalServiceController == null)
                        {
                            GetLocalService();
                        }

                        if (m_LocalServiceController != null)
                        {
                            this.Invoke(new DefaultBoolHandler(EnabledChange), new object[] { CheckService(ServiceControllerStatus.Running) });
                        }
                        Thread.Sleep(1000);
                    }
                    catch{}
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// 서버 중지를 처리합니다.
        /// </summary>
        public void StopService()
        {
            if (m_LocalServiceController == null)
            {
                Console.WriteLine("서비스가 실행되고 있지 않습니다.");
                return;
            }

            s_IsRun = false;
            bool tResult = false;
            int tCount = 0;
            //pnlServer.Enabled = false;

           
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
                Console.WriteLine(ex.ToString());
            }
            
            if (!tResult)
            {
                DialogResult tDialogResult = MessageBox.Show(this, "RACS Daemon Service를 중지되지 않았습니다. 재 시도 하시겠습니까?", "RACS Daemon Service", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question);
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

            fgServerList.Rows.Count = 1;
        }

        private void IsServiceProcessing(bool abool)
        {
            btnServiceStart.Enabled = !abool;

            if (!abool)
            {
                fgServerList.Rows.Count = 1;
            }
            pnlServer.Enabled = abool;
            btnServiceStop.Enabled = abool;
           
        }

        /// <summary>
        /// 서비스 상태를 확인 합니다.
        /// </summary>
        /// <returns>실행 여부 입니다.</returns>
        private bool CheckService(ServiceControllerStatus aCheckStatus)
        {
            if (m_LocalServiceController == null) return false;
            m_LocalServiceController.Refresh();
            return (m_LocalServiceController.Status == aCheckStatus);
        }

        /// <summary>
        /// 로컬 PC에서 서비스 정보를 얻기 합니다.
        /// </summary>
        /// <returns>로컬 PC에 서비스가 존재하는지의 여부 입니다.</returns>
        private bool GetLocalService()
        {
            m_LocalServiceController = GetService(LauncherManagerAppGlobal.s_ManagerConfig.LauncherServiceName);
            Console.WriteLine(string.Format("■ {0} DaemonLauncher 서비스{1}", LauncherManagerAppGlobal.s_ManagerConfig.LauncherServiceName, m_LocalServiceController != null ? "를 찾았습니다." : "가 없습니다."));
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
        /// 서비스 중지 후 컨트롤를 변경 시켜 줍니다.
        /// </summary>
        private void EnabledChange(bool aAlive)
        {
            try
            {
                if (m_LocalServiceController != null)
                {
                    lblServiceStatus.Text = m_LocalServiceController.Status.ToString();
                }

                //btnServiceStart.Enabled = !aAlive;
                //btnServiceStop.Enabled = aAlive;

                //mnuStart.Enabled = !aAlive;
                //mnuStop.Enabled = aAlive;
                //if (!aAlive)
                //{
                //    fgServerList.Rows.Count = 1;
                //}
                //pnlServer.Enabled = aAlive;

                //txtLauncherIP.Enabled = !aAlive;
                //txtLauncherPort.Enabled = !aAlive;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void 닫기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "RACS Daemon Manager를 종료 하시겠습니까?", "RACS Daemon Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // 2019.02.01 KwonTaeSuk 환경설정파일 통합(DaemonConfig.xml, RACTDaemonProcess.DaemonConfig.cs)
                //MKXML.ObjectToXML(Application.StartupPath + "\\" + LauncherManagerAppGlobal.s_LauncherConfigName, LauncherManagerAppGlobal.s_ManagerConfig);
                s_IsRun = false;
                StopCheckServiceProcess();
                m_DisposeFlag = true;
                Dispose();
            }
        }

        private void ApplyServiceStatus(Hashtable aInfo)
        {
            try
            {
                fgServerList.Rows.Count = aInfo.Count + 1;

                int tRow = 1;
                string tStatus = "";
                foreach (RACTDaemonPocessInfo tProcess in aInfo.Values)
                {
                    switch (tProcess.ProcessStatus)
                    {
                        case E_ProcessStatus.LoginAlive:
                            tStatus = "정상";
                            break;
                        case E_ProcessStatus.LoginKill:
                            tStatus = "로그인오류";
                            break;
                        case E_ProcessStatus.ProcessKill:
                            tStatus = "중지";
                            break;
                        case E_ProcessStatus.RemoteKill:
                            tStatus = "통신오류";
                            break;
                        case E_ProcessStatus.ProcessAlive:
                            tStatus = "프로세스실행";
                            break;
                    }

                    fgServerList[tRow, "NO"] = tRow;
                    fgServerList[tRow, "ServerIP"] = tProcess.DaemonIPAddress;
                    fgServerList[tRow, "ServerPort"] = tProcess.DaemonPort;
                    fgServerList[tRow, "Status"] = tStatus;
                    fgServerList.Rows[tRow].UserData = tProcess;
                    tRow++;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        //private void fgServerList_SelChange(object sender, EventArgs e)
        //{
        //    if (fgServerList.RowSel < 1 ) 
        //    {
        //        lblServerIP.Text = "";
        //        lblServerPort.Text = "";

        //        return;
        //    }

        //    RACTDaemonPocessInfo tProcessInfo = fgServerList.Rows[fgServerList.RowSel].UserData as RACTDaemonPocessInfo;
        //    if (tProcessInfo == null) return;

        //    lblServerIP.Text = tProcessInfo.DaemonIPAddress;
        //    lblServerPort.Text = tProcessInfo.DaemonPort.ToString();

        //    btnServerStart.Enabled = true;
        //    btnServerStop.Enabled = true;
        //    //if (tProcessInfo.ProcessStatus == E_ProcessStatus.LoginAlive)
        //    //{
        //    //    btnServerStart.Enabled = false;
        //    //    btnServerStop.Enabled = true;
        //    //}
        //    //else
        //    //{
        //    //    btnServerStart.Enabled = true;
        //    //    btnServerStop.Enabled = false;
        //    //}
        //}

        private void btnServiceStop_Click(object sender, EventArgs e)
        {
            s_IsRun = false;
            StopCheckServiceProcess();
            StopService();
            btnServiceStart.Enabled = true;
        }

        private void ServiceMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            btnServiceStop_Click(null, null);
            btnLauncherStop_Click(null, null);

            if (m_Launcher != null)
            {
                m_Launcher.Stop();
                m_Launcher = null;
            }
            /// 서비스/데몬상태 체크 스레드 중지
            s_IsRun = false;
            StopCheckServiceProcess();

            this.Hide();
        }

        private void mnuInfo_Click(object sender, EventArgs e)
        {
            this.Activate();
            this.Show();
            this.Refresh();
        }

        private void noiAMAS_DoubleClick(object sender, EventArgs e)
        {
            this.Activate();
            this.Show();
            this.Refresh();
        }
/*
 2019.02.01 KwonTaeSuk 데몬단위 시작/중지 기능은 필요시 다시 점검할 것
        /// <summary>
        /// 선택한 DaemonProcess를 실행한다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnServerStart_Click(object sender, EventArgs e)
        {
            if (fgServerList.RowSel >= 1)
            {
                RACTDaemonPocessInfo tProcessInfo = fgServerList.Rows[fgServerList.RowSel].UserData as RACTDaemonPocessInfo;
                if (tProcessInfo == null)
                {
                    MessageBox.Show("프로세스 정보가 없음");
                    return;
                }

                if (tProcessInfo.ProcessStatus != E_ProcessStatus.LoginAlive)
                {
                    DaemonLauncherPartObject tSPO = (DaemonLauncherPartObject)m_RemoteGateway.ServerObject;
                    if (tSPO != null)
                    {
                        if (MessageBox.Show(this, "실행 하시겠습니까?", "RACS Daemon Service", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            if (tSPO.CallSetStartServerMethod(tProcessInfo.Key, true))
                            {
                                btnServerStop.Enabled = true;
                                btnServerStart.Enabled = false;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("리모트 에러");
                    }
                }
                else
                {
                    MessageBox.Show("프로세스가 정상 상태가 아님");
                }

            }
        }

        private void btnServerStop_Click(object sender, EventArgs e)
        {
            if (fgServerList.RowSel >= 1)
            {
                RACTDaemonPocessInfo tProcessInfo = fgServerList.Rows[fgServerList.RowSel].UserData as RACTDaemonPocessInfo;
                if (tProcessInfo == null)
                {
                    MessageBox.Show("프로세스 정보가 OS에 존재 하지 않습니다.");
                    return;
                }
                if (tProcessInfo.ProcessStatus == E_ProcessStatus.LoginAlive)
                    //|| tProcessInfo.ProcessStatus == E_ProcessStatus.ProcessAlive)
                {
                    DaemonLauncherPartObject tSPO = (DaemonLauncherPartObject)m_RemoteGateway.ServerObject;
                    if (tSPO != null)
                    {
                        if (MessageBox.Show(this, "프로세스 종료시 이후 자동 시작되지 않습니다.\n종료 하시겠습니까?", "RACT Daemon Launcher", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            if (tSPO.CallSetStartServerMethod(tProcessInfo.Key, false))
                            {
                                btnServerStop.Enabled = false;
                                btnServerStart.Enabled = true;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("리모트 에러 발생");
                    }
                }
                else
                {
                    MessageBox.Show("프로세스 상태 이상 발생");
                }
            }
        }
*/
        /// <summary>
        /// 서비스를 거치지 않고 데몬런처를 바로 실행 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLauncherStart_Click(object sender, EventArgs e)
        {
            if (m_Launcher != null) {
                MessageBox.Show("DaemonLauncher가 이미 실행중입니다.\r\n중지후 시작해주십시오.");
                return;
            }

            try
            {
                m_Launcher = new DaemonLauncher();
                if (m_Launcher.Start())
                {
                    btnLauncherStart.Enabled = false;

                    /// 서비스/데몬상태 체크 스레드 실행
                    s_IsRun = true;
                    StartCheckServiceProcess();
                }
                else
                {
                    MessageBox.Show("데몬런처 실행이 실패했습니다.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[btnLauncherStart_Click] " + ex.ToString());
            }
        }

        /// <summary>
        /// 데몬런처 중지(no service)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLauncherStop_Click(object sender, EventArgs e)
        {
            try
            {
                /// 서비스/데몬상태 체크 스레드 중지
                s_IsRun = false;
                StopCheckServiceProcess();

                if (m_Launcher != null)
                {
                    m_Launcher.Stop();
                    m_Launcher = null;
                }

                btnLauncherStart.Enabled = true;
                fgServerList.Rows.Count = 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[btnLauncherStop_Click] " + ex.ToString());
            }
        }

        private void _EnabledButtons(bool aEnabled)
        {
            btnServiceStart.Enabled = aEnabled;
            btnServiceStop.Enabled = aEnabled;

            btnLauncherStart.Enabled = aEnabled;
            btnLauncherStop.Enabled = aEnabled;
        }
   }
}