using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using System.Collections;
using RACTCommonClass;
using System.IO;
using MKLibrary.MKData;
using RACTSerialProcess;
using MKLibrary.MKProcess;
using DevComponents.DotNetBar;

// yskun rnjsdud -> 지역운용자 빠른연결시 오류 사항 확인
namespace RACTClient
{
    public partial class ClientMain : SenderForm
    {
        /// <summary>
        /// 로그인 창입니다.
        /// </summary>
        public SplashControl m_SplashControl;
        /// <summary>
        /// 결과 처리에서 사용할 스레드 입니다.
        /// </summary>
        private Thread m_ProcessResultThread = null;
        /// <summary>
        /// 결과 얻기 스래드 입니다.
        /// </summary>
        private Thread m_GetResultThread = null;
        /// <summary>
        /// 결과 큐 입니다.
        /// </summary>
        private Queue m_ResultQueue = new Queue();
        /// <summary>
        /// 요청 전송 스래드 입니다.
        /// </summary>
        private Thread m_RequestSendThread = null;
        /// <summary>
        /// 그룹 수정 이벤트 입니다.
        /// </summary>
        public event ModifyGroupHandler OnModifyGroupEvent;

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public ClientMain()
        {
            InitializeComponent();
            AppGlobal.m_DirectConnect = true;

            //2016-04-06 서영응 바로가기로 실행 했을 경우 오류나는 경우 수정
            AppGlobal.s_RACTClientMode = E_RACTClientMode.Console;

            AppGlobal.terminalPanel1 = terminalPanel1;
            ShowLoginForm();
            ucCommandLine1.sTerminalApplyOption();
        }

        public ClientMain(string aIPAddress, string aID, string aPass)
        {
            InitializeComponent();
            AppGlobal.s_RACTClientMode = E_RACTClientMode.Online;
            ShowLoginForm();
            ucCommandLine1.sTerminalApplyOption();
            m_SplashControl.SetLoginInfo(aIPAddress, aID, aPass);
            timNow.Start();

            //2016-04-01 서영응 탭 포커스를 찾기위해 추가
            AppGlobal.terminalPanel1 = terminalPanel1;
        }

        private void ShowLoginForm()
        {
            try
            {
                //전역변수에 자신을 설정합니다.
                AppGlobal.s_ClientMainForm = this;
                

                ReadOption();

                RACTClient.Helpers.UiContext.Initialize();

                if (AppGlobal.s_FileLogProcessor == null)
                    AppGlobal.s_FileLogProcessor = new RACTClient.Logging.FastLogger(AppGlobal.s_ClientOption.LogPath + "SystemLog\\", "ClientSystem");
                AppGlobal.s_FileLogProcessor.Start();
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "클라이언트를 시작 합니다.");
                m_SplashControl = new SplashControl();
                m_SplashControl.InitalizeControl();
                m_SplashControl.OnExit += new DefaultHandler(m_SplashControl_OnExit);
                AppGlobal.s_ModeChangeForm.m_SplashControl.OnExit += new DefaultHandler(ModeChange_SplashControl_OnExit);
                this.Controls.Add(m_SplashControl);
                EventProcessor.OnLoginStart += new HandlerArgument1<bool>(EventProcessor_OnLoginStart);
                m_SplashControl.BringToFront();

                this.FormBorderStyle = FormBorderStyle.None;
                this.Size = m_SplashControl.Size;
                this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - m_SplashControl.Size.Width) / 2,
                    (Screen.PrimaryScreen.WorkingArea.Height - m_SplashControl.Size.Height) / 2);
                this.SetMainFormText("TACT 클라이언트 (Ver " + AppGlobal.s_Version + ") ::: " + AppGlobal.s_RACTClientMode.ToString());

            }
            catch (Exception ex)
            {
                // AppGlobal.s_FileLog.PrintLogEnter("[E] MainForm(): " + ex.ToString());
            }
        }
        /// <summary>
        /// 클라이언트 시작 처리 스래드 입니다.
        /// </summary>
        private Thread m_StartClientThread = null;

        /// <summary>
        /// 로그인 처리를 합니다.
        /// </summary>
        /// <param name="aValue1"></param>
        void EventProcessor_OnLoginStart(bool aValue1)
        {
            if (aValue1)
            {
                if (m_StartClientThread != null && m_StartClientThread.IsAlive)
                {
                    try
                    {
                        m_StartClientThread.Abort();
                    }
                    catch { }
                }
                m_StartClientThread = new Thread(new ThreadStart(ProcessStartClient));
                m_StartClientThread.Name = "StartClient Thread";
                m_StartClientThread.Start();
            }
            else
            {
                Close();
            }
        }

        /// <summary>
        /// 클라이언트 시작을 처리 합니다.
        /// </summary>
        private void ProcessStartClient()
        {
            try
            {
                // 2013-02-20 - shinyn - 실행속도가 빨라서 로그인되지 않는 경우 발생 처리 
                System.Threading.Thread.Sleep(3000);


                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, string.Concat(AppGlobal.s_RACTClientMode.ToString(), " Mode로 시작합니다."));
                if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online)
                {

                    if (AppGlobal.s_IsModeChangeConnect)
                    {
                        AppGlobal.s_ModeChangeForm.m_SplashControl.ShowInitInfo("서버에 접속 중입니다.");
                    }
                    else
                    {
                        m_SplashControl.ShowInitInfo("서버에 접속 중입니다.");
                    }

                    if (AppGlobal.TryServerConnect() != E_ConnectError.NoError)
                    {
                        if (AppGlobal.s_IsModeChangeConnect)
                        {
                            this.Invoke(new DefaultHandler(AppGlobal.s_ModeChangeForm.m_SplashControl.ShowConsoleModeMessage));
                            return;
                        }
                        else
                        {
                            this.Invoke(new DefaultHandler(m_SplashControl.ShowConsoleModeMessage));
                            return;
                        }
                    }
                    //로그인을 처리합니다.

                    if (AppGlobal.s_IsModeChangeConnect)
                    {
                        AppGlobal.s_ModeChangeForm.m_SplashControl.ShowInitInfo("로그인 합니다.");
                    }
                    else
                    {
                        m_SplashControl.ShowInitInfo("로그인 합니다.");
                    }
                    if (!AppGlobal.LoginConnect())
                    {
                        if (AppGlobal.s_IsModeChangeConnect)
                        {
                            this.Invoke(new DefaultHandler(AppGlobal.s_ModeChangeForm.m_SplashControl.ShowConsoleModeMessage));
                            return;
                        }
                        else
                        {
                            this.Invoke(new DefaultHandler(m_SplashControl.ShowConsoleModeMessage));
                            return;
                        }
                    }
                }

                StartApplicationInit();
            }
            catch (Exception ex)
            {

            }
        }
        private void ModeChange_SplashControl_OnExit()
        {
            if (m_StartClientThread != null && m_StartClientThread.IsAlive)
            {
                try
                {
                    m_StartClientThread.Abort();
                }
                catch { }
            }
        }

        /// <summary>
        /// 종료 처리 합니다.
        /// </summary>
        void m_SplashControl_OnExit()
        {
            if (AppGlobal.s_IsModeChangeConnect) return;
            if (this.InvokeRequired)
            {
                this.Invoke(new DefaultHandler(m_SplashControl_OnExit));
                return;
            }

            this.Close();
        }

        /// <summary>
        /// 환경 정보를 로드 합니다.
        /// </summary>
        private void ReadOption()
        {
            // 2013-04-24- shinyn - 환경설정파일 저장시 오류체크


            try
            {
                FileInfo tFileInfo = new FileInfo(Application.StartupPath + "\\ClientOption.xml");
                if (!tFileInfo.Exists)
                {
                    AppGlobal.s_ClientOption = new ClientOption();
                    AppGlobal.MakeClientOption();
                }

                ArrayList tSystemInfos = null;
                E_XmlError tXmlError = E_XmlError.Success;

                tSystemInfos = MKXML.ObjectFromXML(Application.StartupPath + "\\ClientOption.xml", typeof(ClientOption), out tXmlError);
                if (tSystemInfos == null) return;
                if (tSystemInfos.Count == 0) return;
                AppGlobal.s_ClientOption = (ClientOption)tSystemInfos[0];
                // 2021-04-21 TerminalColumnCount 80으로 일단 고정
                // 2021-11-04 고정사이즈 삭제
                //AppGlobal.s_ClientOption.TerminalColumnCount = 80;
                // 2019-11-10 개선사항 (로그 저장 경로 개선)
                DirectoryInfo ScriptDinfo = new DirectoryInfo(AppGlobal.s_ClientOption.ScriptSavePath);
                if (!ScriptDinfo.Exists)
                    AppGlobal.s_ClientOption.ScriptSavePath = Application.StartupPath + "\\Script\\";

				// 2019-11-10 개선사항 (로그 저장 경로 개선)
                DirectoryInfo LogPathinfo = new DirectoryInfo(AppGlobal.s_ClientOption.LogPath);
                if (!LogPathinfo.Exists)
                    AppGlobal.s_ClientOption.LogPath = Application.StartupPath + "\\Log\\";


            }
            catch (Exception ex)
            {
                AppGlobal.s_FileLogProcessor.PrintLog("ReadOption : " + ex.Message);
            }
        }

        /// <summary>
        /// 기본 데이터를 읽기 시작 합니다.
        /// </summary>
        public bool StartApplicationInit()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DefaultHandlerReturnBool(StartApplicationInit));
                return false;
            }

            try
            {
                //시리얼 작업 프로세서를 시작 합니다.
                if (AppGlobal.s_SerialProcessor == null)
                {
                    AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "Serial 프로세서를 시작합니다.");
                    AppGlobal.s_SerialProcessor = new SerialProcess();
                    AppGlobal.s_SerialProcessor.Start();
                }

                if (AppGlobal.s_TelnetProcessor == null)
                {
                    AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "Telnet 프로세서를 시작합니다.");
                    AppGlobal.s_TelnetProcessor = new TelnetProcessor.TelnetProcessor();
                    //2013-04-26 - shinyn - Log저장 경로를 추가하고, 로그저장을 하도록 수정합니다.
                    //AppGlobal.s_TelnetProcessor.Start();
					// 2019-11-10 개선사항 (로그 저장 경로 개선)
                    AppGlobal.s_TelnetProcessor.Start(AppGlobal.s_ClientOption.LogPath);
                }

                if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online)
                {
                    AppGlobal.s_DaemonProcessList = new Dictionary<int, DaemonProcessRemoteObject>();
                    StartProcessResult();
                    StartGetResult();
                    StartRequestSend();

                    AppGlobal.MakeClientOption();
                    //기본 정보를 로드 합니다.
                    if (!LoadBaseData())
                    {
                        AppGlobal.ShowMessageBox(this, "클라이언트 실행을 위한 초기 작업에 실패 하였습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, "클라이언트 실행을 위한 초기 작업에 실패 하였습니다.");
                        Application.Exit();
                        return false;
                    }
                }
                else
                {
                    StopGetResult();
                    StopRequestSend();
                    StopProcessResult();
                }

                StartTerminalExectueLogProcess();
            }
            catch (Exception ex)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, ex.ToString());
            }

            //현재 폼을 표시합니다.
            this.Invoke(new ShowMainFormHandler(ShowMainForm));

            return true;
        }


        /// <summary>
        /// 터미널 로그 처리 프로세서를 시작 합니다.
        /// </summary>
        private void StopTerminalExectueLogProcess()
        {
            if (AppGlobal.s_TerminalExecuteLogProcess != null)
            {
                AppGlobal.s_TerminalExecuteLogProcess.Stop();
            }
        }

        /// <summary>
        /// 터미널 로그 처리 프로세서를 시작 합니다.
        /// </summary>
        private void StartTerminalExectueLogProcess()
        {
            if (AppGlobal.s_TerminalExecuteLogProcess == null)
            {
                AppGlobal.s_TerminalExecuteLogProcess = new CommandExecuteLogProcess();
                AppGlobal.s_TerminalExecuteLogProcess.Start();
            }
        }

        /// <summary>
        /// 메인폼 표시 핸들러 입니다.
        /// </summary>
        private delegate void ShowMainFormHandler();
        /// <summary>
        /// 현재 폼을 표시합니다.
        /// </summary>
        private void ShowMainForm()
        {
            this.Hide();

            try
            {
                if (AppGlobal.s_IsModeChangeConnect)
                {
                    AppGlobal.s_ModeChangeForm.Hide();
                }
                else
                {
                    if (m_SplashControl != null)
                    {
                        m_SplashControl.Visible = false;
                        m_SplashControl.Dispose();
                        m_SplashControl = null;
                    }
                }
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.Size = new Size(1064, 768);
                this.MinimumSize = new Size(1064, 768);
                this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Size.Width) / 2,
                    (Screen.PrimaryScreen.WorkingArea.Height - this.Size.Height) / 2);
                this.SetMainFormText("TACT 클라이언트 (Ver " + AppGlobal.s_Version + ") ::: " + AppGlobal.s_RACTClientMode.ToString());
                this.Show();
                AppGlobal.s_IsModeChangeConnect = false;
                InitializeControl();
                if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online)
                {
                    if (File.Exists(Application.StartupPath + AppGlobal.s_LayOutFileName))
                    {
                        dotNetBarManager1.LoadLayout(Application.StartupPath + AppGlobal.s_LayOutFileName);
                    }
					// 2019-11-10 개선사항 (로그 자동저장 설장값 옵션으로 지원 기능 추가 )
                    this.autoSaveSwitch.Value = AppGlobal.s_ClientOption.IsAutoSaveLog;
                }
            }
            catch (Exception ex)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, ex.ToString());
            }
        }

        /// <summary>
        /// 각 컨트롤을 초기화 합니다.
        /// </summary>
        private void InitializeControl()
        {
            try
            {
                trvSystemGroup.InitializeControl();
                trvGroup.InitializeControl();
                terminalPanel1.InitializeControl();
                ucCommandLine1.InitializeControl();
                ucSearchDevice1.InitializeControl();
                ucShortenCommand1.InitializeControl();
                ucShortenCommand2.InitializeControl();
                ucBatchRegisterBasePanel.initializeControl();

                // 2014-10-14 - 세션 종료시 터미널 닫히면 터미널개수 줄이도록 수정
                AppGlobal.m_TerminalPanel = terminalPanel1;

                AppGlobal.s_ClientOption.OnConnectionHistoryChange += new DefaultHandler(ConnectionHistoryChange);
                ApplyFileMenu();

                if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Console)
                {
                    tsbAddDevice.Enabled = false;
                    tsbAddGroup.Enabled = false;

                    // 2013-01-11 - shinyn - 목록열기 툴바 추가
                    tsbOpenDeviceList.Enabled = true;

                    // 2013-01-17 - shinyn - 메모장 기능 추가
                    tsbNewNotePad.Enabled = true;
                    tsbOpenNotePad.Enabled = true;
                    tabNotePads.Selected = false;

                    tabTerminal.Selected = true;
                }
                else
                {
                    if (AppGlobal.s_ClientOption.DefaultMainControl == E_ClientDefaultMainControlType.Terminal)
                    {
                        tabNotePads.Selected = false;
                        tabTerminal.Selected = true;
                    }
                    else
                    {
                        tabNotePads.Selected = false;
                        tabBatchRegister.Selected = true;
                    }
                }

                ApplyMenuEnable();
            }
            catch (Exception ex)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, ex.ToString());
            }
        }

        /// <summary>
        /// 메뉴 및 툴바를 클라이언트 모드에 따라 변경 시킵니다.
        /// </summary>
        private void ApplyMenuEnable()
        {
            //파일 --------------------------------------------------------------------------------
            mnuGroupAdd.Enabled = AppGlobal.s_RACTClientMode == E_RACTClientMode.Online;
            mnuDeviceAdd.Enabled = AppGlobal.s_RACTClientMode == E_RACTClientMode.Online;
            // 2013-01-11 - shinyn - 장비목록 불러오기 기능 추가
            mnuLoadDevice.Enabled = AppGlobal.s_RACTClientMode == E_RACTClientMode.Console;
            mnuConnectListView.Enabled = AppGlobal.s_RACTClientMode == E_RACTClientMode.Online;

            // 2013-01-17 - shinyn - 메모장 열기, 메모장 불러오기 기능 추가
            mnuNewNotePad.Enabled = true;
            mnuNewNotePad.Enabled = true;

            //스크립트
            mnuScriptManage.Enabled = AppGlobal.s_RACTClientMode == E_RACTClientMode.Online;

            //툴바
            tsbAddGroup.Enabled = AppGlobal.s_RACTClientMode == E_RACTClientMode.Online;
            tsbAddDevice.Enabled = AppGlobal.s_RACTClientMode == E_RACTClientMode.Online;

            // 2013-01-11 - shinyn - 목록열기 툴바 추가
            tsbOpenDeviceList.Enabled = AppGlobal.s_RACTClientMode == E_RACTClientMode.Console;

            tabBatchRegister.Visible = AppGlobal.s_RACTClientMode == E_RACTClientMode.Online;
            tabSystemGroup.Visible = AppGlobal.s_RACTClientMode == E_RACTClientMode.Online;

            barGroup.Visible = AppGlobal.s_RACTClientMode == E_RACTClientMode.Online;
            barShortenCommand.Visible = AppGlobal.s_RACTClientMode == E_RACTClientMode.Online;
            barDeviceSearch.Visible = AppGlobal.s_RACTClientMode == E_RACTClientMode.Online;

            if(AppGlobal.m_DirectConnect)
                mnuChangeClientMode.Visible = AppGlobal.s_RACTClientMode == E_RACTClientMode.Online;
        }

        /// <summary>
        /// 파일 메뉴 항목을 설정 합니다.
        /// </summary>
        private void ApplyFileMenu()
        {

            if (this.InvokeRequired)
            {
                this.Invoke(new DefaultHandler(ApplyFileMenu));
                return;
            }
            try
            {
                this.mnuFile.SubItems.Clear();
                // 2013-01-11 - shinyn - 파일목록 불러오기 기능 추가
                // 2013-01-17 - shinyn - 새메모장, 메모장 열기 기능 추가
                this.mnuFile.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] { 
                                                this.buttonItem9, 
                                                this.mnuGroupAdd, 
                                                this.mnuDeviceAdd, 
                                                this.mnuUsrDeviceAdd, 
                                                this.mnuLoadDevice, 
                                                this.mnuOpenCmdExecResult,
                                                this.mnuNewNotePad, this.mnuOpenNotePad});

                ButtonItem tConnectList = null;

                for (int i = AppGlobal.s_ClientOption.ConnectionHistoryList.Count - 1; i > -1; i--)
                {
                    tConnectList = new ButtonItem();
                    tConnectList.Click += new EventHandler(tConnectList_Click);
                    tConnectList.Name = ((ConnectionHistoryInfo)AppGlobal.s_ClientOption.ConnectionHistoryList[i]).DisplayName;
                    tConnectList.Text = tConnectList.Name;
                    tConnectList.Tag = AppGlobal.s_ClientOption.ConnectionHistoryList[i];
                    mnuFile.SubItems.Add(tConnectList);
                }

                // 2013-01-17 - shinyn - 새메모장, 메모장 열기 기능 추가 - 접속이력보기 수정
                if (AppGlobal.s_ClientOption.ConnectionHistoryList.Count > 0)
                {
                    this.mnuFile.SubItems[4].BeginGroup = true;
                }

                this.mnuFile.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] { this.mnuConnectListView, this.mnuExit });
            }
            catch (Exception ex)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, ex.ToString());
            }
        }

        /// <summary>
        /// 접속 목록 변경시 처리 입니다.
        /// </summary>
        void ConnectionHistoryChange()
        {
            ApplyFileMenu();
        }

        /// <summary>
        /// 서버로부터 기본 정보를 가져오기 합니다.
        /// </summary>
        /// <returns>기본 정보 로드의 성공여부 입니다.</returns>
        public bool LoadBaseData()
        {
            try
            {
                RequestCommunicationData tRequestData = null;

                tRequestData = AppGlobal.MakeDefaultRequestData();
                ShowInitInfo("FACT 그룹 정보를 로딩 합니다.");
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "FACT 그룹 정보를 로딩 합니다.");
                tRequestData.CommType = E_CommunicationType.RequestFactGroupInfo;
                tRequestData.RequestData = AppGlobal.s_LoginResult.ClientID;
                m_Result = null;
                m_MRE.Reset();

                AppGlobal.SendRequestData(this, tRequestData);
                m_MRE.WaitOne(AppGlobal.s_RequestTimeOut * 10);
                if (m_Result == null) return false;
                if (m_Result.Error.Error != E_ErrorType.NoError)
                {
                    AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, m_Result.Error.ErrorString);
                    return false;
                }

                MakeAllGroupInfo((FACTGroupInfo)m_Result.ResultData);


                // 그룹 정보및 장비정보 가져오기
                ShowInitInfo("TACT 그룹 정보를 로딩 합니다.");
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "TACT 그룹 정보를 로딩 합니다.");
                GroupRequestInfo tRequestGroupInfo = new GroupRequestInfo();
                tRequestGroupInfo.UserID = AppGlobal.s_LoginResult.UserID;
                tRequestGroupInfo.WorkType = E_WorkType.Search;

                tRequestData.CommType = E_CommunicationType.RequestGroupInfo;
                tRequestData.RequestData = tRequestGroupInfo;

                m_Result = null;
                m_MRE.Reset();

                AppGlobal.SendRequestData(this, tRequestData);
                m_MRE.WaitOne(AppGlobal.s_RequestTimeOut * 10);
                if (m_Result == null) return false;
                if (m_Result.Error.Error != E_ErrorType.NoError)
                {
                    AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, m_Result.Error.ErrorString);
                    return false;
                }

                AppGlobal.s_GroupInfoList = (GroupInfoCollection)m_Result.ResultData;

                if (AppGlobal.s_GroupInfoList == null)
                {
                    return false;
                }

                ShowInitInfo("모델 정보를 로딩 합니다.");
                tRequestData.CommType = E_CommunicationType.RequestModelInfo;
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "모델 정보를 로딩 합니다.");
                m_Result = null;
                m_MRE.Reset();
                AppGlobal.SendRequestData(this, tRequestData);
                m_MRE.WaitOne(AppGlobal.s_RequestTimeOut * 10);
                if (m_Result == null) return false;
                if (m_Result.Error.Error != E_ErrorType.NoError)
                {
                    AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, m_Result.Error.ErrorString);
                    return false;
                }
                AppGlobal.s_ModelInfoList = (ModelInfoCollection)m_Result.ResultData;
                AppGlobal.InitializeDevicePartList();

                //2015-10-30 제한 명령어 - 사용자 권한 적용.
                if (AppGlobal.s_LoginResult.UserInfo.LimitedCmdUser)
                {
                    ShowInitInfo("제한 명령어 정보를 로딩 합니다.");
                    tRequestData.CommType = E_CommunicationType.RequestLimitCmdInfo;
                    AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "제한 명령어 정보를 로딩 합니다.");
                    m_Result = null;
                    m_MRE.Reset();
                    AppGlobal.SendRequestData(this, tRequestData);
                    m_MRE.WaitOne(AppGlobal.s_RequestTimeOut * 10);
                    if (m_Result == null) return false;
                    if (m_Result.Error.Error != E_ErrorType.NoError)
                    {
                        AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, m_Result.Error.ErrorString);
                        return false;
                    }
                    AppGlobal.s_LimitCmdInfoList = (LimitCmdInfoCollection)m_Result.ResultData;
                }

                //bool tIsMatch = false;

                //foreach (LimitCmdInfo tModelInfo in AppGlobal.s_LimitCmdInfoList)
                //{
                    
                //        if (tModelInfo.ModelID.ToString().Equals("2348"))
                //        {
                //            foreach (String str in tModelInfo.EmbagoCmd)
                //            {
                //                Console.Write("2340 의 제한 명령어 " + str);
                //            }
                //            break;
                //        }
                    

                //}

                ShowInitInfo("기본 명령어 정보를 로딩 합니다.");
                tRequestData.CommType = E_CommunicationType.RequestDefaultCmdInfo;
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "기본 명령어 정보를 로딩 합니다.");
                m_Result = null;
                m_MRE.Reset();
                AppGlobal.SendRequestData(this, tRequestData);
                m_MRE.WaitOne(AppGlobal.s_RequestTimeOut * 10);
                if (m_Result == null) return false;
                if (m_Result.Error.Error != E_ErrorType.NoError)
                {
                    AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, m_Result.Error.ErrorString);
                    return false;
                }
                AppGlobal.s_DefaultCmdInfoList = (DefaultCmdInfoCollection)m_Result.ResultData;


                ShowInitInfo("접속자의 자동완성 정보를 로딩 합니다.");
                tRequestData.CommType = E_CommunicationType.RequestAutoCompleteCmd;
                tRequestData.RequestData = AppGlobal.s_LoginResult.UserID;
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "접속자의 자동완성 정보를 로딩 합니다.");
                m_Result = null;
                m_MRE.Reset();
                AppGlobal.SendRequestData(this, tRequestData);
                m_MRE.WaitOne(AppGlobal.s_RequestTimeOut * 10);
                if (m_Result == null) return false;
                if (m_Result.Error.Error != E_ErrorType.NoError)
                {
                    AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, m_Result.Error.ErrorString);
                    return false;
                }
                AppGlobal.s_AutoCompleteCmdList = (AutoCompleteCmdInfoCollection)m_Result.ResultData;
                
                
                

                // 단축 명령 정보 가져오기
                ShowInitInfo("단축 명령 정보를 로딩 합니다.");
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "단축 명령 정보를 로딩 합니다.");
                ShortenCommandRequestInfo tRequestShortenCommand = new ShortenCommandRequestInfo();
                tRequestShortenCommand.UserID = AppGlobal.s_LoginResult.UserID;
                tRequestShortenCommand.WorkType = E_WorkType.Search;

                tRequestData.CommType = E_CommunicationType.RequestShortenCommand;
                tRequestData.RequestData = tRequestShortenCommand;

                m_Result = null;
                m_MRE.Reset();

                AppGlobal.SendRequestData(this, tRequestData);
                m_MRE.WaitOne(AppGlobal.s_RequestTimeOut * 10);
                if (m_Result == null) return false;
                if (m_Result.Error.Error != E_ErrorType.NoError)
                {
                    AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, m_Result.Error.ErrorString);
                    return false;
                }

                AppGlobal.s_ShortenCommandList = (ShortenCommandGroupInfoCollection)m_Result.ResultData;


                // 스크립트 정보 가져오기
                ShowInitInfo("스크립트 정보를 로딩 합니다.");
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "스크립트 정보를 로딩 합니다.");
                ScriptGroupRequestInfo tRequestScriptGroup = new ScriptGroupRequestInfo();
                tRequestScriptGroup.UserID = AppGlobal.s_LoginResult.UserID;
                tRequestScriptGroup.WorkType = E_WorkType.Search;

                tRequestData.CommType = E_CommunicationType.RequestScriptGroup;
                tRequestData.RequestData = tRequestScriptGroup;

                m_Result = null;
                m_MRE.Reset();

                AppGlobal.SendRequestData(this, tRequestData);
                m_MRE.WaitOne(AppGlobal.s_RequestTimeOut * 10);
                if (m_Result == null) return false;
                if (m_Result.Error.Error != E_ErrorType.NoError)
                {
                    AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, m_Result.Error.ErrorString);
                    return false;
                }
                AppGlobal.s_ScriptList = (ScriptGroupInfoCollection)m_Result.ResultData;
                return true;
            }
            catch (Exception ex)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, ex.ToString());
                return false;
            }
        }
        /// <summary>
        /// 전체 그룹 정보를 생성합니다.
        /// </summary>
        private void MakeAllGroupInfo(FACTGroupInfo aAllGroupInfo)
        {
            if (AppGlobal.s_OrganizationInfo == null)
            {
                AppGlobal.s_OrganizationInfo = new OrganizationInfo();
            }

            FACTGroupInfo tAllGroupInfo = aAllGroupInfo.DeepClone();

            AppGlobal.s_OrganizationInfo.AllGroupInfo = tAllGroupInfo;

            if (AppGlobal.s_OrganizationInfo.ORG1NameByCode == null)
                AppGlobal.s_OrganizationInfo.ORG1NameByCode = new Hashtable();

            if (AppGlobal.s_OrganizationInfo.BranchNameByCode == null)
                AppGlobal.s_OrganizationInfo.BranchNameByCode = new Hashtable();

            if (AppGlobal.s_OrganizationInfo.CenterNameByCode == null)
                AppGlobal.s_OrganizationInfo.CenterNameByCode = new Hashtable();

            AppGlobal.s_OrganizationInfo.ORG1NameByCode.Clear();
            AppGlobal.s_OrganizationInfo.BranchNameByCode.Clear();
            AppGlobal.s_OrganizationInfo.CenterNameByCode.Clear();

            GetORG1List(tAllGroupInfo, AppGlobal.s_OrganizationInfo.ORG1NameByCode);
            GetBranchList(tAllGroupInfo, AppGlobal.s_OrganizationInfo.BranchNameByCode);
            GetCenterList(tAllGroupInfo, AppGlobal.s_OrganizationInfo.CenterNameByCode);
        }

        /// <summary>
        /// 실 정보를 가져옵니다.
        /// </summary>
        /// <param name="aGroupInfo"></param>
        private void GetORG1List(FACTGroupInfo aGroupInfo, Hashtable aHashtable)
        {
            if (aGroupInfo.SubGroups != null)
            {
                for (int i = 0; i < aGroupInfo.SubGroups.Count; i++)
                {
                    if (aGroupInfo.SubGroups[i].ORG1Code != "" &&
                        aGroupInfo.SubGroups[i].ORG1Code != "0")
                    {
                        lock (aHashtable.SyncRoot)
                        {
                            if (aHashtable.ContainsKey(aGroupInfo.SubGroups[i].ORG1Code) == false)
                            {
                                aHashtable.Add(aGroupInfo.SubGroups[i].ORG1Code, aGroupInfo.SubGroups[i].ORG1Name);
                            }
                        }
                    }

                    if (aGroupInfo.SubGroups[i].SubGroups != null)
                    {
                        if (aGroupInfo.SubGroups[i].SubGroups.Count > 0)
                        {
                            GetORG1List(aGroupInfo.SubGroups[i], aHashtable);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 센터 목록을 얻어 해쉬 테이블에 저장합니다.
        /// </summary>
        /// <param name="aGroupInfo"></param>
        private void GetCenterList(FACTGroupInfo aGroupInfo, Hashtable aHashtable)
        {
            if (aGroupInfo.SubGroups != null)
            {
                for (int i = 0; i < aGroupInfo.SubGroups.Count; i++)
                {
                    if (aGroupInfo.SubGroups[i].CenterCode != "" &&
                        aGroupInfo.SubGroups[i].CenterCode != "000000")
                    {
                        if (!aHashtable.ContainsKey(aGroupInfo.SubGroups[i].CenterCode))
                        {
                            aHashtable.Add(aGroupInfo.SubGroups[i].CenterCode, aGroupInfo.SubGroups[i].CenterName);
                        }
                    }

                    if (aGroupInfo.SubGroups[i].SubGroups != null)
                    {
                        if (aGroupInfo.SubGroups[i].SubGroups.Count > 0)
                        {
                            GetCenterList(aGroupInfo.SubGroups[i], aHashtable);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 팀 정보 (구.지사 정보)를 가져옵니다.
        /// </summary>
        /// <param name="aGroupInfo"></param>
        private void GetBranchList(FACTGroupInfo aGroupInfo, Hashtable aHashtable)
        {
            if (aGroupInfo.SubGroups != null)
            {
                for (int i = 0; i < aGroupInfo.SubGroups.Count; i++)
                {
                    if (aGroupInfo.SubGroups[i].BranchCode != "" &&
                        aGroupInfo.SubGroups[i].BranchCode != "0")
                    {
                        lock (aHashtable.SyncRoot)
                        {
                            if (aHashtable.ContainsKey(aGroupInfo.SubGroups[i].BranchCode) == false)
                            {
                                aHashtable.Add(aGroupInfo.SubGroups[i].BranchCode, aGroupInfo.SubGroups[i].BranchName);
                            }
                        }
                    }

                    if (aGroupInfo.SubGroups[i].SubGroups != null)
                    {
                        if (aGroupInfo.SubGroups[i].SubGroups.Count > 0)
                        {
                            GetBranchList(aGroupInfo.SubGroups[i], aHashtable);
                        }
                    }
                }
            }
        }
        //private bool m_IsWorkingForReceiveDevices = false;
        //private bool m_IsFinishedForReceiveDevices = false;
        //private Queue m_ReceivedDevicesQueue = null;
        //private Thread m_ReceiveDevicesThread = null;

        ///// <summary>
        ///// 장비 정보를 분할하여 받기를 시도합니다.
        ///// </summary>
        //private bool ReceiveDeviceInfo()
        //{
        //    ShowInitInfo("시설 정보를 로딩합니다.");
        //    AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "시설 정보를 로딩합니다.");
        //    DeviceInfoCollection tDeviceInfos = null;
        //    try
        //    {
        //        m_IsWorkingForReceiveDevices = true;
        //        m_IsFinishedForReceiveDevices = false;
        //        m_ReceivedDevicesQueue = new Queue();
        //        m_ReceiveDevicesThread = new Thread(new ThreadStart(ProcessReceiveDevices));
        //        m_ReceiveDevicesThread.Start();

        //        RequestCommunicationData tRequestData = null;

        //        while (true)
        //        {
        //            tRequestData = AppGlobal.MakeDefaultRequestData();
        //            tRequestData.CommType = E_CommunicationType.RequestDeviceInfo;
        //            DeviceRequestInfo tRequestInfo = new DeviceRequestInfo();

        //            tRequestInfo.WorkType = E_WorkType.Search;

        //            tRequestData.RequestData = tRequestInfo;

        //            m_Result = null;
        //            m_MRE.Reset();

        //            AppGlobal.SendRequestData(this, tRequestData);
        //            m_MRE.WaitOne();
        //            if (m_Result == null) return false;
        //            if (m_Result.Error.Error != E_ErrorType.NoError)
        //            {
        //                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, m_Result.Error.ErrorString);
        //                return false;
        //            }
        //            if (m_Result.ResultData == null)
        //            {
        //                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, "시설정보 결과가 NULL 입니다.");
        //                return false;
        //            }

        //            lock (m_ReceivedDevicesQueue.SyncRoot) m_ReceivedDevicesQueue.Enqueue(m_Result.ResultData);

        //            if (m_Result.CommType == E_CommunicationType.RequestDeviceInfo) break;
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, ex.ToString());
        //        return false;
        //    }
        //    finally
        //    {
        //        m_IsFinishedForReceiveDevices = true;
        //    }
        //}

        ///// <summary>
        ///// 서버에서 받은 장비 정보를 메모리에 올립니다.
        ///// </summary>
        //private void ProcessReceiveDevices()
        //{
        //    byte[] tResults = null;
        //    DeviceInfoCollection tDeviceInfos = null;
        //    CompressData tCompressData = null;
        //    try
        //    {
        //        while (true)
        //        {
        //            if (!m_IsWorkingForReceiveDevices) break;
        //            lock (m_ReceivedDevicesQueue.SyncRoot)
        //            {
        //                if (m_ReceivedDevicesQueue.Count < 1)
        //                {
        //                    if (m_IsFinishedForReceiveDevices) break;
        //                    Thread.Sleep(1);
        //                    continue;
        //                }
        //                tCompressData = (CompressData)m_ReceivedDevicesQueue.Dequeue();
        //            }
        //            if (tCompressData == null)
        //            {
        //                Thread.Sleep(1);
        //                continue;
        //            }
        //            tDeviceInfos = (DeviceInfoCollection)AppGlobal.DecompressObject(tCompressData);

        //            foreach (DeviceInfo tDeviceInfo in tDeviceInfos)
        //            {
        //                AppGlobal.s_DeviceInfoList.Add(tDeviceInfo);
        //            }
        //            tDeviceInfos = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine(ex.ToString());
        //        AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, ex.ToString());
        //    }
        //    finally
        //    {
        //        m_IsWorkingForReceiveDevices = false;
        //        m_ReceivedDevicesQueue = null;
        //    }
        //}

        /// <summary>
        /// 현재 상태를 표시 한다.
        /// </summary>
        /// <param name="aInfo"></param>
        private void ShowInitInfo(string aInfo)
        {
            if (AppGlobal.s_IsModeChangeConnect)
            {
                if (AppGlobal.s_ModeChangeForm.Visible)
                {
                    AppGlobal.s_ModeChangeForm.m_SplashControl.ShowInitInfo(aInfo);
                }
            }
            else
            {
                if (m_SplashControl != null && m_SplashControl.Visible)
                {
                    m_SplashControl.ShowInitInfo(aInfo);
                }
            }
        }


        /// <summary>
        /// 서버에 요청 스래드를 시작합니다.
        /// </summary>
        private void StartRequestSend()
        {
            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "서버 명령 전송 프로세서를 시작합니다.");
            m_RequestSendThread = new Thread(new ThreadStart(ProcessRequestSendToServer));
            m_RequestSendThread.Start();
        }

        /// <summary>
        /// 서버에 요청 스래드를 중지 합니다.
        /// </summary>
        private void StopRequestSend()
        {
            if (m_RequestSendThread == null) return;

            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "서버 명령 전송 프로세서를 종료합니다.");
            m_RequestSendThread.Join(10);
            if (m_RequestSendThread.IsAlive)
            {
                try
                {
                    m_RequestSendThread.Abort();
                }
                catch { }
            }
            m_RequestSendThread = null;

        }

        /// <summary>
        /// 서버에 요청을 전송합니다.
        /// </summary>
        private void ProcessRequestSendToServer()
        {
            RemoteClientMethod tRemoteClientMethod = null;
            object tSendObject = null;

            while (!AppGlobal.s_IsProgramShutdown)
            {
                tSendObject = null;

                lock (AppGlobal.s_RequestQueue)
                {
                    // 2013-05-02 - shinyn - 없는경우 
                    //if (AppGlobal.s_RequestQueue.Count < 1)
                    //{
                    //    AppGlobal.m_MRE.Reset();
                    //    AppGlobal.m_MRE.WaitOne();
                    //}

                    if (AppGlobal.s_RequestQueue.Count > 0)
                    {
                        tSendObject = AppGlobal.s_RequestQueue.Dequeue();
                    }
                }
                if (tSendObject != null)
                {
                    try
                    {
                        tRemoteClientMethod = (RemoteClientMethod)AppGlobal.s_RemoteGateway.ServerObject;
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
        /// 결과 받기 스래드를 시작합니다.
        /// </summary>
        private void StartGetResult()
        {
            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "서버 결과 받기 프로세서를 시작합니다.");
            m_GetResultThread = new Thread(new ThreadStart(ProcessGetResultFromServer));
            m_GetResultThread.Start();
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

            while (!AppGlobal.s_IsProgramShutdown)
            {
                if (AppGlobal.s_IsServerConnected)
                {
                    try
                    {
                        tResultData = null;
                        tRemoteClientMethod = (RemoteClientMethod)AppGlobal.s_RemoteGateway.ServerObject; ;
                        tResultDatas = tRemoteClientMethod.CallResultMethod(AppGlobal.s_LoginResult.ClientID);
                        if (tResultDatas != null) tResultData = (ArrayList)ObjectConverter.GetObject(tResultDatas);
                    }
                    catch (Exception ex)
                    {
                        //tRemoteClientMethod = null;
                        //if (AppGlobal.s_IsServerConnected)
                        //{
                        //    tResultFailCount++;
                        //    if (tResultFailCount > 3)
                        //    {
                        //        AppGlobal.s_IsServerConnected = false;
                        //        TryServerConnect();
                        //    }
                        //}
                        this.Invoke(new DefaultHandler(MainFormClose));
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
                        }
                    }

                }
                Thread.Sleep(AppGlobal.s_ServerCheckInterval);
            }
        }
        private E_ConnectError m_ConnectError;
        /// <summary>
        /// 서버와 재연결을 시도 합니다.
        /// </summary>
        private void TryServerConnect()
        {


            m_ConnectError = AppGlobal.TryServerConnect();
            if (m_ConnectError == E_ConnectError.NoError)
            {
                //서버에 다시 로그인 합니다.
                AppGlobal.LoginConnect();

            }
            Thread.Sleep(1000);


            if (m_ConnectError != E_ConnectError.NoError)
            {
                this.Invoke(new DefaultHandler(MainFormClose));
            }
        }

        /// <summary>
        /// 메인폼을 닫기 합니다.
        /// </summary>
        private void MainFormClose()
        {
            switch (m_ConnectError)
            {
                case E_ConnectError.ServerNoRun:
                    AppGlobal.ShowMessageBox(this, "서버에 연결할 수 없습니다. 서버가 정상적으로 시작되었는지 또는 FireWall이 작동중인지 확인 하십시오.\n프로그램을 종료 합니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case E_ConnectError.LinkFail:
                    AppGlobal.ShowMessageBox(this, "서버에 연결할 수 없습니다. 서버까지 네트워크가 정상적인지 확인 하십시오.\n프로그램을 종료 합니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case E_ConnectError.LocalFail:
                    AppGlobal.ShowMessageBox(this, "서버에 연결을 시도할 수 없습니다. PC의 네트워크가 정상인지 확인 하십시오.\n프로그램을 종료 합니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
            }
            if (AppGlobal.ShowMessageBox(this, "Console Mode로 변환 하시겠습니까?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ShowLoadingProgress(false);
                ChangeClientMode(E_RACTClientMode.Console);
                return;
            }

            timNow.Stop();
            AppGlobal.s_ClientMainForm.Close();
        }

        /// <summary>
        /// 결과 받기 스래드를 중지 합니다.
        /// </summary>
        private void StopGetResult()
        {
            if (m_GetResultThread != null)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "서버 결과 받기 프로세서를 종료합니다.");
                m_GetResultThread.Join(10);
                if (m_GetResultThread.IsAlive)
                {
                    try
                    {
                        m_GetResultThread.Abort();
                    }
                    catch { }
                }
                m_GetResultThread = null;
            }
        }

        /// <summary>
        /// 결과 처리 스래드를 시작합니다.
        /// </summary>
        private void StartProcessResult()
        {
            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "수신 결과 처리 프로세서를 시작 합니다.");
            m_ProcessResultThread = new Thread(new ThreadStart(ProcessServerResult));
            m_ProcessResultThread.Start();
        }

        private void StopProcessResult()
        {

            if (m_ProcessResultThread != null && m_ProcessResultThread.IsAlive)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "수신 결과 처리 프로세서를 종료 합니다.");
                try
                {
                    m_ProcessResultThread.Abort();
                }
                catch { }
                m_ProcessResultThread = null;
            }
        }

        /// <summary>
        /// 결과를 처리 합니다.
        /// </summary>
        private void ProcessServerResult()
        {
            ResultCommunicationData tResult = null;
            TelnetCommandResultInfo tWorkResult = null;
            object tObject = null;

            ISenderObject tSender = null;
            bool tIsWorked = true;

            while (!AppGlobal.s_IsProgramShutdown && AppGlobal.s_IsServerConnected)
            {
                try
                {
                    tResult = null;
                    tWorkResult = null;
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
                        else if (tObject.GetType().Equals(typeof(TelnetCommandResultInfo)))
                        {
                            tWorkResult = tObject as TelnetCommandResultInfo;
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
                            lock (AppGlobal.s_SenderList)
                            {
                                tSender = (ISenderObject)AppGlobal.s_SenderList[tResult.OwnerKey];
                            }
                            if (tSender != null)
                            {
                                tSender.ResultReceiver(tResult);
                            }
                        }

                    }
                    else if (tWorkResult != null)
                    {
                        if (tWorkResult.OwnerKey != 0)
                        {
                            tSender = null;
                            lock (AppGlobal.s_SenderList) tSender = (ISenderObject)AppGlobal.s_SenderList[tWorkResult.OwnerKey];
                            if (tSender != null) tSender.ResultReceiver(tWorkResult);
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
        /// 타이틀을 표시 합니다.
        /// </summary>
        /// <param name="aTitle"></param>
        internal void SetMainFormText(string aTitle)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument1<string>(SetMainFormText), aTitle);
                return;
            }
            this.Text = aTitle;
        }
        //private void trvGroup_OnConnectDeviceEvent(DeviceInfo aDeviceInfo)
        //{
        //    //장비에 접속 합니다.
        //    if (aDeviceInfo == null) return;

        //    aDeviceInfo.TerminalConnectInfo.IPAddress = aDeviceInfo.IPAddress;

        //    terminalPanel1.Dock = DockStyle.Fill;
        //    terminalPanel1.BringToFront();

        //    terminalPanel1.AddTerminal(aDeviceInfo, false);
        //    tabTerminal.Selected = true;
        //}
        /// <summary>
        /// 2013-05-03- shinyn - 장비접속오류를 해결하기 위해 데몬정보와 장비정보를 같이 보내서 접속한다.
        /// 이전에는 1)데몬연결 2)장비접속 순차적으로 가져오는경우,
        /// 결과값이 오지 않는 현상이 발생하여,
        /// 데몬연결하는것을 리스트로 가져와서, 장비접속할때 데몬을 장비별로 나누어주어 접속하도록 수정하니,
        /// 접속이 잘된다.
        /// </summary>
        /// <param name="aDeviceInfo"></param>
        /// <param name="aDaemonProcessInfo"></param>
        private void trvGroup_OnConnectDeviceEvent(DeviceInfo aDeviceInfo, DaemonProcessInfo aDaemonProcessInfo)
        {
            //장비에 접속 합니다.
            if (aDeviceInfo == null) return;

            aDeviceInfo.TerminalConnectInfo.IPAddress = aDeviceInfo.IPAddress;

            terminalPanel1.Dock = DockStyle.Fill;
            terminalPanel1.BringToFront();

            terminalPanel1.AddTerminal(aDeviceInfo, false, aDaemonProcessInfo);
            tabTerminal.Selected = true;
        }

        private void trvGroup_OnModifyGroupEvent(E_WorkType aWorkType, GroupInfo aGroupInfo)
        {
            if (aWorkType == E_WorkType.Delete)
            {
                DeleteGroupInfo(aGroupInfo);
            }
            else
            {
                ModifyGroupInfo tForm = new ModifyGroupInfo(aWorkType, new GroupInfo(aGroupInfo));
                tForm.initializeControl();
                tForm.ShowDialog(this);

                // 2013-08-13 - shinyn - 그룹 수정후 다시 사용자그룹을 리스트업한다.

                RequestCommunicationData tRequestData = null;
                tRequestData = AppGlobal.MakeDefaultRequestData();

                GroupRequestInfo tRequestGroupInfo = new GroupRequestInfo();
                tRequestGroupInfo.UserID = AppGlobal.s_LoginResult.UserID;
                tRequestGroupInfo.WorkType = E_WorkType.Search;

                tRequestData.CommType = E_CommunicationType.RequestGroupInfo;
                tRequestData.RequestData = tRequestGroupInfo;

                m_Result = null;
                m_MRE.Reset();

                AppGlobal.SendRequestData(this, tRequestData);
                m_MRE.WaitOne(AppGlobal.s_RequestTimeOut * 10);

                if (m_Result == null)
                {
                    AppGlobal.ShowMessageBox(this, "그룹정보를 로딩하지 못했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (m_Result.Error.Error != E_ErrorType.NoError)
                {
                    AppGlobal.ShowMessageBox(this, "그룹정보를 로딩하지 못했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, m_Result.Error.ErrorString);
                    return;
                }

                AppGlobal.s_GroupInfoList = (GroupInfoCollection)m_Result.ResultData;

                if (AppGlobal.s_GroupInfoList == null)
                {
                    AppGlobal.ShowMessageBox(this, "그룹정보를 로딩하지 못했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    trvGroup.RefreshTree();
                    return;
                }

                trvGroup.RefreshTree();
            }
        }


        /// <summary>
        /// 2013-08-14 - shinyn - 사용자 장비 공유 이벤트입니다.
        /// </summary>
        /// <param name="aGroupInfo"></param>
        private void trvGroup_OnShareDeviceEvent(GroupInfo aGroupInfo)
        {
            try
            {

                ModifyShareGroupInfo tForm = new ModifyShareGroupInfo(E_WorkType.Add);
                tForm.InitializeControl();
                tForm.ShowDialog(this);

                // 2013-08-13 - shinyn - 그룹 수정후 다시 사용자그룹을 리스트업한다.
                // 2013-09-09 - shinyn - 그룹을 넘겨 주는것으로 변경되어 재로딩 삭제
                /*
                RequestCommunicationData tRequestData = null;
                tRequestData = AppGlobal.MakeDefaultRequestData();

                GroupRequestInfo tRequestGroupInfo = new GroupRequestInfo();
                tRequestGroupInfo.UserID = AppGlobal.s_LoginResult.UserID;
                tRequestGroupInfo.WorkType = E_WorkType.Search;

                tRequestData.CommType = E_CommunicationType.RequestGroupInfo;
                tRequestData.RequestData = tRequestGroupInfo;

                m_Result = null;
                m_MRE.Reset();

                AppGlobal.SendRequestData(this, tRequestData);
                m_MRE.WaitOne(AppGlobal.s_RequestTimeOut * 10);

                if (m_Result == null)
                {
                    AppGlobal.ShowMessageBox(this, "그룹정보를 로딩하지 못했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (m_Result.Error.Error != E_ErrorType.NoError)
                {
                    AppGlobal.ShowMessageBox(this, "그룹정보를 로딩하지 못했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, m_Result.Error.ErrorString);
                    return;
                }

                AppGlobal.s_GroupInfoList = (GroupInfoCollection)m_Result.ResultData;

                if (AppGlobal.s_GroupInfoList == null)
                {
                    AppGlobal.ShowMessageBox(this, "그룹정보를 로딩하지 못했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    trvGroup.RefreshTree();
                    return;
                }

                trvGroup.RefreshTree();
                */

            }
            catch (Exception ex)
            {
            }


        }



        /// <summary>
        /// 그룹을 삭제 합니다.
        /// </summary>
        /// <param name="aGroupInfo"></param>
        private void DeleteGroupInfo(GroupInfo aGroupInfo)
        {
            if (AppGlobal.ShowMessageBox(this, "그룹을 삭제 하시겠습니까?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            //if (aGroupInfo.DeviceList.Count > 0)
            //{
            //    AppGlobal.ShowMessageBox(this, "삭제 할 수 없습니다.\n(해당 그룹에 등록된 장비가 존재합니다.)", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            RequestCommunicationData tRequestData = null;
            GroupRequestInfo tGrupRequestInfo = new GroupRequestInfo();
            tGrupRequestInfo.WorkType = E_WorkType.Delete;
            tGrupRequestInfo.GroupInfo = aGroupInfo;
            tGrupRequestInfo.UserID = AppGlobal.s_LoginResult.UserID;

            tRequestData = AppGlobal.MakeDefaultRequestData();
            tRequestData.CommType = E_CommunicationType.RequestGroupInfo;

            tRequestData.RequestData = tGrupRequestInfo;

            m_Result = null;
            m_MRE.Reset();

            AppGlobal.SendRequestData(this, tRequestData);
            m_MRE.WaitOne(AppGlobal.s_RequestTimeOut);
            if (m_Result == null)
            {
                AppGlobal.ShowMessageBox(this, "알 수 없는 에러가 발생했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (m_Result.Error.Error != E_ErrorType.NoError)
            {
                AppGlobal.ShowMessageBox(this, m_Result.Error.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            EventProcessor.Run((GroupInfo)m_Result.ResultData, E_WorkType.Delete);
        }



        private void mnuDeviceAdd_Click(object sender, EventArgs e)
        {
            trvGroup_OnModifyDeviceEvent(E_WorkType.Add, null);
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OptionConfigurationForm frmOptionConfiguration = new OptionConfigurationForm();
            frmOptionConfiguration.OnClientOptionChangeEvent += new DefaultHandler(frmOptionConfiguration_OnClientOptionChangeEvent);
            frmOptionConfiguration.ShowDialog(this);
        }

        void frmOptionConfiguration_OnClientOptionChangeEvent()
        {
            terminalPanel1.ChangeOption();
            ucCommandLine1.sTerminalApplyOption();
			// 2019-11-10 개선사항 (로그 자동저장 설장값 옵션으로 지원 기능 추가 )
            this.autoSaveSwitch.Value = AppGlobal.s_ClientOption.IsAutoSaveLog;
        }

        /// <summary>
        /// 로그아웃 합니다.
        /// </summary>
        private void SendRequestLogOut()
        {
            if (!AppGlobal.s_IsServerConnected) return;

            //RequestCommunicationData tRequestData = null;
            //tRequestData = AppGlobal.MakeDefaultRequestData();
            //tRequestData.CommType = E_CommunicationType.RequestUserLogout;
            //tRequestData.RequestData = AppGlobal.s_LoginResult.ClientID;
            //AppGlobal.SendRequestData(this, tRequestData);
            //Thread.Sleep(500);
        }


        void tConnectList_Click(object sender, EventArgs e)
        {
            ButtonItem tSender = (ButtonItem)sender;
            terminalPanel1.BringToFront();
            bar4.SelectedDockTab = 0;
            terminalPanel1.QuickConnect(((ConnectionHistoryInfo)tSender.Tag).ConnectionInfo);
        }

        /// <summary>
        /// 그룹 추가 메뉴 기능을 수행하는 함수입니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuGroupAdd_Click(object sender, EventArgs e)
        {
            trvGroup_OnModifyGroupEvent(E_WorkType.Add, null);
        }

        /// <summary>
        /// 종료 처리 합니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClientMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (AppGlobal.ShowMessageBox(this, "프로그램을 종료 하시겠습니까?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                e.Cancel = true;
                return;
            }


            if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online)
            {

                try
                {
                    dotNetBarManager1.SaveLayout(Application.StartupPath + AppGlobal.s_LayOutFileName);
                }
                catch { }

                SendLogOut();
            }

            AppGlobal.s_IsProgramShutdown = true;
            AppGlobal.s_IsServerConnected = false;
            m_MRE.Set();
            if (m_StartClientThread != null && m_StartClientThread.IsAlive)
            {
                try
                {
                    m_StartClientThread.Abort();
                }
                catch { }
            }
            SendRequestLogOut();
            AppGlobal.MakeClientOption();
            StopGetResult();
            StopRequestSend();
            StopProcessResult();
            StopTerminalExectueLogProcess();
            StopSerialProcess();
            StopTelnetProcess();
            if (AppGlobal.s_FileLogProcessor != null)
            {
                AppGlobal.s_FileLogProcessor.Stop();
            }

            foreach (DaemonProcessRemoteObject tObject in AppGlobal.s_DaemonProcessList.Values)
            {
                tObject.Stop();
            }

            try
            {
                terminalPanel1.Stop(E_TerminalSessionCloseType.All);
            }
            catch { }
        }

        private void StopTelnetProcess()
        {
            if (AppGlobal.s_TelnetProcessor != null)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "Telnet 프로세서를 종료 합니다.");
                AppGlobal.s_TelnetProcessor.Dispose();
            }
        }

        private void StopSerialProcess()
        {
            if (AppGlobal.s_SerialProcessor != null)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "Serial 프로세서를 종료 합니다.");
                AppGlobal.s_SerialProcessor.Dispose();
            }
        }

        private void trvGroup_OnModifyDeviceEvent(E_WorkType aWorkType, Object aNodeInfo)
        {

            switch (aWorkType)
            {
                case E_WorkType.Add:
                    ModifyDeviceInfo tAddDeviceInfo;
                    if (aNodeInfo == null)
                    {
                        tAddDeviceInfo = new ModifyDeviceInfo();
                    }
                    else
                    {
                        tAddDeviceInfo = new ModifyDeviceInfo(((GroupInfo)aNodeInfo).ID);
                    }

                    tAddDeviceInfo.InitializeControl();
                    tAddDeviceInfo.ShowDialog(this);
                    break;

                case E_WorkType.Modify:
                    ShowModifyDeviceForm((DeviceInfo)aNodeInfo, aWorkType);
                    break;

                case E_WorkType.Delete:
                    DeleteDevice((DeviceInfo)aNodeInfo);
                    break;
            }
        }

        /// <summary>
        /// 2013-01-18 - shinyn - 수동장비등록 이벤트 추가
        /// </summary>
        /// <param name="aWorkType"></param>
        /// <param name="aNodeInfo"></param>
        private void trvGroup_OnModifyUsrDeviceEvent(E_WorkType aWorkType, Object aNodeInfo)
        {

            switch (aWorkType)
            {
                case E_WorkType.Add:
                    ModifyUserDeviceInfo tAddDeviceInfo;
                    if (aNodeInfo == null)
                    {
                        tAddDeviceInfo = new ModifyUserDeviceInfo();
                    }
                    else
                    {
                        tAddDeviceInfo = new ModifyUserDeviceInfo(((GroupInfo)aNodeInfo).ID);
                    }

                    tAddDeviceInfo.InitializeControl();
                    tAddDeviceInfo.ShowDialog(this);
                    break;

                case E_WorkType.Modify:
                    ShowModifyUserDeviceForm((DeviceInfo)aNodeInfo, aWorkType);
                    break;

                case E_WorkType.Delete:
                    DeleteDevice((DeviceInfo)aNodeInfo);
                    break;
            }
        }

        /// <summary>
        /// 장비를 삭제 기능을 수행하는 함수입니다.
        /// </summary>
        /// <param name="aDeviceInfo"></param>
        private void DeleteDevice(DeviceInfo aDeviceInfo)
        {
            if (aDeviceInfo != null)
            {
                if (AppGlobal.ShowMessageBox(this, "장비를 삭제 하시겠습니까?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }
            }

            RequestCommunicationData tRequestData = null;
            DeviceRequestInfo tDeviceRequestInfo = new DeviceRequestInfo();
            tDeviceRequestInfo.WorkType = E_WorkType.Delete;
            tDeviceRequestInfo.DeviceInfo = aDeviceInfo;
            tDeviceRequestInfo.UserID = AppGlobal.s_LoginResult.UserID;

            tRequestData = AppGlobal.MakeDefaultRequestData();
            tRequestData.CommType = E_CommunicationType.RequestDeviceInfo;

            tRequestData.RequestData = tDeviceRequestInfo;

            m_Result = null;
            m_MRE.Reset();

            AppGlobal.SendRequestData(this, tRequestData);
            m_MRE.WaitOne(AppGlobal.s_RequestTimeOut);

            if (m_Result == null)
            {
                AppGlobal.ShowMessageBox(this, "알 수 없는 에러가 발생했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (m_Result.Error.Error != E_ErrorType.NoError)
            {
                AppGlobal.ShowMessageBox(this, m_Result.Error.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            EventProcessor.Run((DeviceInfo)m_Result.ResultData, E_WorkType.Delete);

            AppGlobal.ShowMessageBox(this, "장비를 삭제 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 장비 속성 창을 표시 합니다.
        /// </summary>
        /// <param name="aDeviceInfo">장비 정보 입니다.</param>
        /// <param name="aWorkType">Work Type 입니다.</param>
        private void ShowModifyDeviceForm(DeviceInfo aDeviceInfo, E_WorkType aWorkType)
        {
            ModifyDeviceInfo tModifyDeviceInfo = new ModifyDeviceInfo(aDeviceInfo, aWorkType);
            tModifyDeviceInfo.InitializeControl();
            tModifyDeviceInfo.ShowDialog(this);
        }

        /// <summary>
        /// 2013-01-18 - shinyn - 수동 장비 속성 창을 표시 합니다.
        /// </summary>
        /// <param name="aDeviceInfo">장비 정보 입니다.</param>
        /// <param name="aWorkType">Work Type 입니다.</param>
        private void ShowModifyUserDeviceForm(DeviceInfo aDeviceInfo, E_WorkType aWorkType)
        {
            ModifyUserDeviceInfo tModifyDeviceInfo = new ModifyUserDeviceInfo(aDeviceInfo, aWorkType);
            tModifyDeviceInfo.InitializeControl();
            tModifyDeviceInfo.ShowDialog(this);
        }

        private void terminalPanel1_OnModifyDeviceEvent(E_WorkType aWorkType, object aDeviceInfo)
        {
            ShowModifyDeviceForm((DeviceInfo)aDeviceInfo, aWorkType);
        }

        /// <summary>
        /// 2013-01-18 - shinyn - 수동장비 등록 속성창을 표시합니다.
        /// </summary>
        /// <param name="aWorkType"></param>
        /// <param name="aDeviceInfo"></param>
        private void terminalPanel1_OnModifyUsrDeviceEvent(E_WorkType aWorkType, object aDeviceInfo)
        {
            ShowModifyUserDeviceForm((DeviceInfo)aDeviceInfo, aWorkType);
        }

        private void terminalPanel1_OnTerminalTabChangeEvent(E_TerminalStatus aWorkType, string aTerminalName)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument2<E_TerminalStatus, string>(terminalPanel1_OnTerminalTabChangeEvent), new object[] { aWorkType, aTerminalName });
                return;
            }
            ucCommandLine1.TerminalChange(aWorkType, aTerminalName);
        }


        /// <summary>
        /// 2013-01-17 - shinyn - 메모장 탭 변경시 발생하는 이벤트
        /// </summary>
        /// <param name="aWorkType"></param>
        /// <param name="aNotePadName"></param>
        private void ucMainNotePads_OnNotePadTabChangeEvent(E_NotePadStatus aWorkType, string aNotePadName)
        {

        }


        private void mnuScriptManage_Click(object sender, EventArgs e)
        {
            ScriptList tListForm = new ScriptList();
            tListForm.InitializeControl();
            tListForm.ShowDialog(this);
        }

        private void mnuScriptRun_Click(object sender, EventArgs e)
        {
            if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online)
            {
                ScriptList tListForm = new ScriptList(E_ScriptFormMode.Select);
                tListForm.InitializeControl();
                if (tListForm.ShowDialog(this) == DialogResult.OK)
                {
                    terminalPanel1.ScriptWork(E_ScriptWorkType.Run, tListForm.SelectedScript);
                }
            }
            else
            {
                string tScriptText = "";
                if (AppGlobal.ShowScriptOpenDialog(out tScriptText) == DialogResult.OK)
                {
                    terminalPanel1.ScriptWork(E_ScriptWorkType.Run, new Script(tScriptText));
                }
            }
        }

        private void mnuScriptRec_Click(object sender, EventArgs e)
        {
            terminalPanel1.ScriptWork(E_ScriptWorkType.Rec);
        }

        private void mnuScriptSave_Click(object sender, EventArgs e)
        {
            terminalPanel1.ScriptWork(E_ScriptWorkType.Save);
        }

        private void mnuScriptRunCancel_Click(object sender, EventArgs e)
        {
            terminalPanel1.ScriptWork(E_ScriptWorkType.RunCancel);
        }

        private void mnuQuickConnect_Click(object sender, EventArgs e)
        {
            QuickConnect tConnectForm = new QuickConnect();
            tConnectForm.InitializeControl();
            if (tConnectForm.ShowDialog(this) == DialogResult.OK)
            {
                tabTerminal.Selected = true;
                terminalPanel1.QuickConnect(tConnectForm.QuickConnectInfo);
            }
        }

        private void mnuChangeClientMode_Click(object sender, EventArgs e)
        {
            ClientModeChange tModeChange = new ClientModeChange();
            tModeChange.InitializeControl();
            if (tModeChange.ShowDialog(this) == DialogResult.Yes)
            {
                ChangeClientMode(tModeChange.ClientMode);
            }
        }

        /// <summary>
        /// 클라이언트 모드를 변경 합니다.
        /// </summary>
        /// <param name="aMode"></param>
        private void ChangeClientMode(E_RACTClientMode aMode)
        {
            try
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "클라이언트 모드를 변경 합니다.");
                if (aMode == E_RACTClientMode.Online)
                {
                    //서버에 접속해야함
                    AppGlobal.s_IsModeChangeConnect = true;
                    AppGlobal.s_ModeChangeForm.InitializeControl();
                    AppGlobal.s_ModeChangeForm.Size = new Size(690, 410);
                    AppGlobal.s_ModeChangeForm.ShowDialog(this);

                }
                else
                {
                    terminalPanel1.ChangeClientMode();
                    SendLogOut();
                    AppGlobal.s_IsServerConnected = false;
                    AppGlobal.s_RACTClientMode = E_RACTClientMode.Console;
                    StartApplicationInit();
                }
            }
            catch (Exception ex)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, ex.ToString());
            }
        }
        /// <summary>
        /// 로그 아웃 처리 합니다.
        /// </summary>
        private void SendLogOut()
        {
            try
            {
                if (AppGlobal.s_IsServerConnected)
                {
                    if (m_ConnectError == E_ConnectError.NoError)
                    {
                        RemoteClientMethod tSPO = (RemoteClientMethod)AppGlobal.s_RemoteGateway.ServerObject;

                        tSPO.CallUserLogOutMethod(AppGlobal.s_LoginResult.ClientID);
                    }
                }
            }
            catch { }
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mnuConnectListView_Click(object sender, EventArgs e)
        {
            ConnectionHistorySearch tSearch = new ConnectionHistorySearch();
            tSearch.InitializeControl();
            tSearch.ShowDialog(this);
        }



        private void tsbQuickConnect_Click(object sender, EventArgs e)
        {
            mnuQuickConnect_Click(null, null);
        }

        private void tsbRunScript_Click(object sender, EventArgs e)
        {
            mnuScriptRun_Click(null, null);
        }

        private void mnuBatchRegister_Click(object sender, EventArgs e)
        {
            ucBatchRegisterBasePanel.BringToFront();
        }

        private void mnuTerminal_Click(object sender, EventArgs e)
        {
            terminalPanel1.BringToFront();
        }

        private void mnuScriptRecCancel_Click(object sender, EventArgs e)
        {
            terminalPanel1.ScriptWork(E_ScriptWorkType.RecCancel);
        }

        private void mnuCopy_Click(object sender, EventArgs e)
        {
            terminalPanel1.ExecTerminalScreen(E_TerminalScreenTextEditType.Copy);
        }

        private void mnuPaste_Click(object sender, EventArgs e)
        {
           
            //if(AppGlobal.bPanelFocusCheck)
            terminalPanel1.ExecTerminalScreen(E_TerminalScreenTextEditType.Paste);
        }

        private void mnuFind_Click(object sender, EventArgs e)
        {
            terminalPanel1.ExecTerminalScreen(E_TerminalScreenTextEditType.Find);
        }

        private void mnuSelectAll_Click(object sender, EventArgs e)
        {
            terminalPanel1.ExecTerminalScreen(E_TerminalScreenTextEditType.SelectAll);
        }

        private void mnuClear_Click(object sender, EventArgs e)
        {
            terminalPanel1.ExecTerminalScreen(E_TerminalScreenTextEditType.Clear);
        }

        private void ClientMain_Load(object sender, EventArgs e)
        {

        }
        //2016-01-19 서영응 기본 명령 조회 닷넷 바에 추가
        private void mnuSearchDefaultCmd_Click(object sender, EventArgs e)
        {
            terminalPanel1.ExecTerminalScreen(E_TerminalScreenTextEditType.SearchCmd);
        }

        private void trvGroup_OnConnectGroupDeviceEvent(GroupInfo aGroupInfo)
        {
            RequestCommunicationData tRequestData = null;

            tRequestData = AppGlobal.MakeDefaultRequestData();
            tRequestData.CommType = E_CommunicationType.RequestDaemonInfoList;
            tRequestData.RequestData = aGroupInfo.DeviceList.Count;
            m_Result = null;
            m_MRE.Reset();

            AppGlobal.SendRequestData(this, tRequestData);
            m_MRE.WaitOne(AppGlobal.s_RequestTimeOut);
            // m_MRE.WaitOne();
            if (m_Result == null || m_Result.Error.Error != E_ErrorType.NoError)
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "사용 가능한 Daemon 정보 로드에 실패 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "사용 가능한 Daemon 정보 로드에 실패 했습니다.");
                return;
            }

            List<DaemonProcessInfo> tDaemonList = m_Result.ResultData as List<DaemonProcessInfo>;

            for (int i = 0; i < aGroupInfo.DeviceList.Count; i++)
            {
                terminalPanel1.AddTerminal(aGroupInfo.DeviceList.InnerList[i] as DeviceInfo, false, tDaemonList[i]);
            }
            tabTerminal.Selected = true;
        }

        private void tsbScriptRunCancel_Click(object sender, EventArgs e)
        {
            mnuScriptRunCancel_Click(null, null);
        }


        // 2013-05-02-shinyn - 연결되지 않는 오류를 해결하기 위해, 데몬프로세스정보와 장비정보를 보내서 연결되도록 수정한다..
        //public void ucSearchDevice1_OnConnectDeviceEvent(DeviceInfo aDeviceInfo)
        //{
        //    trvGroup_OnConnectDeviceEvent(aDeviceInfo);
        //}
        public void ucSearchDevice1_OnConnectDeviceEvent(DeviceInfo aDeviceInfo, DaemonProcessInfo aDaemonProcessInfo)
        {
            trvGroup_OnConnectDeviceEvent(aDeviceInfo, aDaemonProcessInfo);
        }

        private void ucSearchDevice1_OnModifyDeviceEvent(E_WorkType aWorkType, object aNodeInfo)
        {
            ShowModifyDeviceForm((DeviceInfo)aNodeInfo, aWorkType);
        }

        private void superTabControl1_SelectedTabChanged(object sender, DevComponents.DotNetBar.SuperTabStripSelectedTabChangedEventArgs e)
        {

        }

        private void tsbAddGroup_Click(object sender, EventArgs e)
        {
            mnuGroupAdd_Click(null, null);
        }

        private void tsbAddDevice_Click(object sender, EventArgs e)
        {
            mnuDeviceAdd_Click(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            barDeviceSearch.Visible = !barDeviceSearch.Visible;
            barGroup.Visible = !barGroup.Visible;
            barShortenCommand.Visible = !barShortenCommand.Visible;
        }



        /// <summary>
        /// 프로그래스를 표시를 처리 합니다.
        /// </summary>
        /// <param name="aVisable"></param>
        internal void ShowLoadingProgress(bool aVisable)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument1<bool>(ShowLoadingProgress), aVisable);
                return;
            }

            progressBarX1.Visible = aVisable;
        }


        /// <summary>
        /// 탭으로 추가 합니다.
        /// </summary>
        internal void AddTerminalTab(ITerminal mCTerminalEmulator)
        {
            terminalPanel1.AddTerminal(mCTerminalEmulator);
        }


        internal void AddNotePadTab(ucNotePad aNotePad)
        {
            //tabNotePads
        }

        private void DisplayNow()
        {

            if (this.InvokeRequired)
            {
                this.Invoke(new DefaultHandler(DisplayNow));
                return;
            }


            if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online)
            {
                if (AppGlobal.s_LoginResult == null || AppGlobal.s_LoginResult.UserInfo == null) return;
                this.lblSystemInfo.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "   접속 계정 : " + AppGlobal.s_LoginResult.UserInfo.Account;
            }
            else
            {
                this.lblSystemInfo.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "    Console Mode";
            }
        }
        private void timNow_Tick(object sender, EventArgs e)
        {
            DisplayNow();
        }


        /// <summary>
        /// 2013-01-11 - shinyn - 장비 목록 파일 불러오기 기능 추가
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuLoadDevice_Click(object sender, EventArgs e)
        {
            try
            {
                DeviceInfoCollection tDeviceInfos = AppGlobal.OpenDeviceList();

                if (tDeviceInfos == null) return;

                if (tDeviceInfos.Count == 0) return;

                if (terminalPanel1.EmulatorList.Count >= 20)
                {
                    AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "터미널 연결개수는 20개까지입니다. 터미널을 닫아주세요.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                OpenDeviceList tOpenDeviceList = new OpenDeviceList();

                tOpenDeviceList.RemailTerminal = 20 - terminalPanel1.EmulatorList.Count;

                tOpenDeviceList.OpenFileDeviceList(tDeviceInfos);

                if (tOpenDeviceList.ShowDialog() != DialogResult.OK) return;

                DeviceInfoCollection tSelectDevices = tOpenDeviceList.DeviceInfos;

                foreach (DeviceInfo tDeviceInfo in tSelectDevices)
                {
                    TerminalConnectInfo tTerminalConnectInfo = new TerminalConnectInfo();

                    tTerminalConnectInfo.IPAddress = tDeviceInfo.IPAddress;
                    terminalPanel1.OpenDeviceConnect(tDeviceInfo, tTerminalConnectInfo);
                }

            }
            catch (Exception ex)
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, ex.Message.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        /// <summary>
        /// 2013-01-11 - shinyn - 장비 목록 열기 툴바 추가
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbOpenDeviceList_Click(object sender, EventArgs e)
        {
            mnuLoadDevice_Click(null, null);
        }

        /// <summary>
        /// 2013-01-17 - shinyn - 새로운 메모장 실행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuNewNotePad_Click(object sender, EventArgs e)
        {
            try
            {

                ucMainNotePads1.NewNotePad();
                tabNotePads.Selected = true;
            }
            catch (Exception ex)
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, ex.Message.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        /// <summary>
        /// 2013-01-17 - shinyn - 메모장 열기 실행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuOpenNotePad_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog tOpenFileDialog = new OpenFileDialog();

                tOpenFileDialog.Filter = "TXT Files(*.txt)|*.txt";

                if (tOpenFileDialog.ShowDialog(AppGlobal.s_ClientMainForm) != DialogResult.OK) return;

                string tFilePath = tOpenFileDialog.FileName;

                ucMainNotePads1.OpenNotePad(tFilePath);
                tabNotePads.Selected = true;

            }
            catch (Exception ex)
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, ex.Message.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //2015-09-22 명령 조회 및 자동 저장 기능.
        /// <summary>
        /// 파일 > 명령실행결과조회 메뉴 클릭시의 처리입니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuOpenCmdExecResult_Click(object sender, EventArgs e)
        {
            CmdExecResultForm tCmdExecResultForm = new CmdExecResultForm();
            tCmdExecResultForm.StartPosition = FormStartPosition.CenterParent;
            tCmdExecResultForm.ShowDialog(this);
 
            //상단에 툴바 메뉴 추가 시 
            //ex> mnuNewNotePad_Click(null, null); ..형태의 메소드 연결이 필요함.
        }


        /// <summary>
        /// 2013-01-17 - shinyn - 새로운 메모장 실행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbNewNotePad_Click(object sender, EventArgs e)
        {
            mnuNewNotePad_Click(null, null);
        }

        /// <summary>
        /// 2013-01-17 - shinyn - 메모장 열기 실행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbOpenNotePad_Click(object sender, EventArgs e)
        {
            mnuOpenNotePad_Click(null, null);
        }


        /// <summary>
        /// 2013-01-18 - shinyn - 수동 장비 등록 실행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuUsrDeviceAdd_Click(object sender, EventArgs e)
        {
            try
            {
                trvGroup_OnModifyUsrDeviceEvent(E_WorkType.Add, null);
            }
            catch (Exception ex)
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm,
                                         ex.Message.ToString(),
                                         MessageBoxButtons.OK,
                                         MessageBoxIcon.Warning);

            }
        }

        /// <summary>
        /// 2013-01-18 - shinyn - 수동장비 등록 실행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbAddUsrDevice_Click(object sender, EventArgs e)
        {
            mnuUsrDeviceAdd_Click(null, null);
        }

        private void autoSaveSwitch_ValueChanged(object sender, EventArgs e)
        {
			// 2019-11-10 개선사항 (로그 자동저장 설장값 옵션으로 지원 기능 추가 )
            //AppGlobal.s_IsAutoSaveLog = autoSaveSwitch.Value;
            AppGlobal.s_ClientOption.IsAutoSaveLog = autoSaveSwitch.Value;
   
        }

        private void mnuPasteCR_Click(object sender, EventArgs e)
        {
            terminalPanel1.ExecTerminalScreen(E_TerminalScreenTextEditType.PasteCR);
        }

        private void mnuAutoC_Click(object sender, EventArgs e)
        {
            terminalPanel1.ExecTerminalScreen(E_TerminalScreenTextEditType.AutoC);
        }
        
    }
}