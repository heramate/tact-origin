using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RACTCommonClass;
using RACTClient;
using DevComponents.DotNetBar;
using RACTTerminal;
using MKLibrary.MKProcess;
using System.Threading;
using System.Management;
using System.Collections;
using System.Net;
using System.IO;
using MKLibrary.MKData;
using RACTSerialProcess;
using DevComponents.DotNetBar.Controls;

namespace RACTOneTerminal
{
    public partial class ucOneTerminal : SenderForm
    {
        /// <summary>
        /// 로그인 처리 스래드 입니다.
        /// </summary>
        private Thread m_LoginThread = null;
        /// <summary>
        /// 클라이언트 시작 처리 스래드 입니다.
        /// </summary>
        private Thread m_StartClientThread = null;
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
        /// 터미널 연결 스레드 풀 입니다.
        /// </summary>
      //  private MKThreadPool m_TerminalConnectThreadPool;
        private DeviceInfo m_DeviceInfo;
        /// <summary>
        /// 장비 속성을 가져오거나 설정합니다.
        /// </summary>
        public DeviceInfo DeviceInfo
        {
            get { return m_DeviceInfo; }
            set { m_DeviceInfo = value; }
        }

        public ucOneTerminal()
        {
            InitializeComponent();
        }

        public ucOneTerminal(string aServerIP, string aUserID, string aUserPW,string aDeviceIP) : this()
        {
            TerminalConnectInfo tConnectionInfo = new TerminalConnectInfo();
            tConnectionInfo.IPAddress = aDeviceIP;
            tConnectionInfo.ConnectionProtocol = E_ConnectionProtocol.TELNET;

            m_DeviceInfo = new DeviceInfo();
            m_DeviceInfo.IPAddress = aDeviceIP;
            m_DeviceInfo.TerminalConnectInfo.IPAddress = aDeviceIP;

            ReadOption();

            InitializeControl();

            //AppGlobal.s_ClientMainForm = new ClientMain();
            //AppGlobal.s_ClientMainForm.Name = "One Terminal";
            AppGlobal.s_Caller = E_TerminalMode.QuickClient;
            //AppGlobal.s_Caller = E_TerminalMode.RACTClient;

            AppGlobal.s_ServerIP = aServerIP;
            AppGlobal.s_UserAccount = aUserID;
            AppGlobal.s_Password = aUserPW;

            RACTClient.Helpers.UiContext.Initialize();

			// 2019-11-10  簡 ׻ ( α  浵 渷  簡)
            AppGlobal.s_FileLogProcessor = new RACTClient.Logging.FastLogger(AppGlobal.s_ClientOption.LogPath + "SystemLog\\", "ClientSystem");
            AppGlobal.s_FileLogProcessor.Start();
            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "클라이언트를 시작 합니다.");

            EventProcessor.OnLoginStart += new HandlerArgument1<bool>(EventProcessor_OnLoginStart);

            StartLogin();

           
        }
        MCTerminalEmulator m_Emulator;
        private void ConnectDevice()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DefaultHandler(ConnectDevice));
                return;
            }
            //m_TerminalConnectThreadPool = new MKThreadPool("Terminal Connection ThreadPool", 1);
            //m_TerminalConnectThreadPool.StartThreadPool();

			// 2019-11-10 개선사항 (로그 저장 경로 개선)
			// 2019-11-10 개선사항 (옵션 설정 개선)
			// 2019-11-10 개선사항 (OneTerminal 접속시 진행 상태UI 기능)
            m_Emulator = new MCTerminalEmulator(true, E_TerminalMode.QuickClient);
            //m_Emulator.TerminalMode = E_TerminalMode.QuickClient;
            m_Emulator.OnTelnetFindString += new DefaultHandler(Emulator_OnTelnetFindString);
            m_Emulator.CallOptionHandlerEvent += new DefaultHandler(m_Emulator_CallOptionHandlerEvent);
            m_Emulator.ProgreBarHandlerEvent += new HandlerArgument3<string, eProgressItemType, bool>(m_Emulator_ProgreBarHandlerEvent);
            m_Emulator.Modes = new Mode();
            m_Emulator.Modes.Flags = m_Emulator.Modes.Flags |= Mode.s_AutoWrap;
            m_Emulator.Dock = DockStyle.Fill;
            m_Emulator.DeviceInfo = m_DeviceInfo;
			
			if (m_DeviceInfo.DevicePartCode == 1 || /* 집선스위치 */
                m_DeviceInfo.DevicePartCode == 6 || /* G-PON-OLT */
                m_DeviceInfo.DevicePartCode == 31 /* NG-PON-OLT */ )
                
                {
                    m_Emulator.ExtApplyOption();
                    //m_Emulator.SetFontColor(Color.Red);
                    //m_Emulator.SetBackGroundColor(Color.Black);

                }
                
            //2019-09-25 자동저장 로그 파일명으로 사용할 TerminalName 정보 설정
            if (AppGlobal.s_ClientOption.TerminalDisplayNameType == E_TerminalDisplayNameType.IPAddress)
            {
                m_Emulator.DeviceInfo.TerminalName = m_DeviceInfo.IPAddress; 
            }
            else
            {
                m_Emulator.DeviceInfo.TerminalName = m_DeviceInfo.Name;
            }

            this.Controls.Add(panel);
            panel.Controls.Add(m_Emulator);
          //  m_TerminalConnectThreadPool.ExecuteWork(new MKWorkItem(new WorkItemDefaultMethod(m_Emulator.ConnectDevice)));

            // 2013-05-02 - shinyn - 터미널 연결시 연결안되는 오류를 수정하기 위해, 텔넷 연결시 WorkItem을 같이 보내서 연결하도록 수정합니다.
            //m_Emulator.ConnectDevice();
			// 2019-11-10 개선사항 (OneTerminal 접속시 진행 상태UI 기능)
            ProgressBarText(progressBarX1, "디바이스에 연결 중입니다.");
            m_Emulator.ConnectDevice(m_Emulator.DeviceInfo);

            switch(m_Emulator.TerminalStatus)
            {
                case E_TerminalStatus.Connection:
                    ProgressBarText(progressBarX1, "디바이스에 연결 중입니다."); 
                    ProgressBarType(progressBarX1, eProgressItemType.Standard);
                    Thread.Sleep(1000);
                    ProgressBarVisble(panel, false);//panel1.Visible = false;
                    break;
                case E_TerminalStatus.Disconnected:
                    ProgressBarText(progressBarX1, "디바이스에 연결이 종료 또는 연결 되지 않았습니다."); 
                    ProgressBarType(progressBarX1, eProgressItemType.Standard);
                    break;
                    
            }
            

            //필요한 코드???
            //
            //progressBarX1 컨트롤을 제어 할 수 있는 코드.
        }

        void m_Emulator_ProgreBarHandlerEvent(string aValue1, eProgressItemType aValue2, bool aValue3)
        {
            if (!aValue1.Equals(""))
                ProgressBarText(progressBarX1, aValue1);//progressBarX1.Text = aValue1;

            ProgressBarType(progressBarX1, aValue2);//progressBarX1.ProgressType = aValue2;

            ProgressBarVisble(panel1, aValue3);//panel1.Visible = aValue3;


        }


        void m_Emulator_CallOptionHandlerEvent()
        {
            OptionConfigurationForm frmOptionConfiguration = new OptionConfigurationForm();
            frmOptionConfiguration.OnClientOptionChangeEvent += new DefaultHandler(frmOptionConfiguration_OnClientOptionChangeEvent);
            frmOptionConfiguration.ShowDialog(this);
        }

        void frmOptionConfiguration_OnClientOptionChangeEvent()
        {
            m_Emulator.ExtApplyOption();
        }

        private void InitializeControl()
        {
            
            if (AppGlobal.s_ClientOption == null)
            {
                ReadOption();
            }

            if (AppGlobal.s_ClientOption == null)
            {
                //Text = "TACT v" + AppGlobal.s_Version + " (" + m_DeviceInfo.Name + ")";
                Text = "TACT v" + AppGlobal.s_Version + " (" + m_DeviceInfo.Name + ")" + "               단축키 : 기본 명령 조회(F1), 자동 완성(F2)";
            }
            else
            {
                if (AppGlobal.s_ClientOption.TerminalDisplayNameType == E_TerminalDisplayNameType.IPAddress)
                {
                    //Text = "TACT v" + AppGlobal.s_Version + " (" + m_DeviceInfo.IPAddress + ")";
                    Text = "TACT v" + AppGlobal.s_Version + " (" + m_DeviceInfo.IPAddress + ")" + "               단축키 : 기본 명령 조회(F1), 자동 완성(F2)";
                }
                else
                {
                    //Text = "TACT v" + AppGlobal.s_Version + " (" + m_DeviceInfo.Name + ")";
                    Text = "TACT v" + AppGlobal.s_Version + " (" + m_DeviceInfo.Name + ")" + "               단축키 : 기본 명령 조회(F1), 자동 완성(F2)";
                }
            }
        }


        /// <summary>
        /// 환경 정보를 로드 합니다.
        /// </summary>
        private void ReadOption()
        {
            
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
				// 2019-11-10 개선사항 (로그 저장 경로 개선)
                DirectoryInfo ScriptDinfo = new DirectoryInfo(AppGlobal.s_ClientOption.ScriptSavePath);
                if (!ScriptDinfo.Exists)
                    AppGlobal.s_ClientOption.ScriptSavePath = Application.StartupPath + "\\Script\\";
				// 2019-11-10 개선사항 (로그 저장 경로 개선)
                //CommandLog Path Option
                DirectoryInfo CommandDinfo = new DirectoryInfo(AppGlobal.s_ClientOption.LogPath);
                if (!CommandDinfo.Exists)
                    AppGlobal.s_ClientOption.LogPath = Application.StartupPath + "\\Log\\";
            }
            catch (Exception ex)
            {
                AppGlobal.s_FileLogProcessor.PrintLog("OneTerminal ReadOption : " + ex.Message.ToString());
            }
        }

        /// <summary>
        /// 찾기 창을 표시 합니다.
        /// </summary>
        void Emulator_OnTelnetFindString()
        {
            //if (AppGlobal.s_TelnetFindForm == null)
            //{
            //    AppGlobal.s_TelnetFindForm = new TelnetFindForm();
            //    AppGlobal.s_TelnetFindForm.OnTelnetStringFind += new TelnetStringFindHandler(TelnetFindForm_OnTelnetStringFind);
            //    AppGlobal.s_TelnetFindForm.FormClosing += new FormClosingEventHandler(s_TelnetFindForm_FormClosing);
            //    AppGlobal.s_TelnetFindForm.Show(this);
            //    AppGlobal.s_TelnetFindForm.Activate();
            //}
            //else if (!AppGlobal.s_TelnetFindForm.Visible)
            //{
            //    AppGlobal.s_TelnetFindForm.Visible = true;
            //    //  AppGlobal.s_TelnetFindForm.Activate();
            //    //  AppGlobal.s_TelnetFindForm.Focus();
            //}
            //else
            //{
            //    // AppGlobal.s_TelnetFindForm.Activate();
            //    // AppGlobal.s_TelnetFindForm.Focus();
            //}
        }

        /// <summary>
        /// 찾기 처리 합니다.
        /// </summary>
        void TelnetFindForm_OnTelnetStringFind(TelnetStringFindHandlerArgs aStringArgs)
        {
            ((MCTerminalEmulator)panel.Controls[0]).FindForm_OnTelnetStringFind(aStringArgs);
        }

        void s_TelnetFindForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ((MCTerminalEmulator)panel.Controls[0]).FindForm_Close();
        }

        private void ucOneTerminal_FormClosed(object sender, FormClosedEventArgs e)
        {
            

            
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
        /// 로그아웃 합니다.
        /// </summary>
        private void SendRequestLogOut()
        {
            if (!AppGlobal.s_IsServerConnected) return;

            RequestCommunicationData tRequestData = null;
            tRequestData = AppGlobal.MakeDefaultRequestData();
            tRequestData.CommType = E_CommunicationType.RequestUserLogout;
            tRequestData.RequestData = AppGlobal.s_LoginResult.ClientID;
            AppGlobal.SendRequestData(this, tRequestData);
            Thread.Sleep(500);
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

        private void Login(string aID, string aPW, string aServerIP)
        {
            AppGlobal.s_RACTClientMode = E_RACTClientMode.Online;

            AppGlobal.s_ServerIP = aServerIP;
            AppGlobal.s_UserAccount = aID;
            AppGlobal.s_Password = aPW;

            StartLogin();
        }
        /// <summary>
        /// 로그인을 처리하는 부분입니다.
        /// </summary>			
        public void StartLogin()
        {
            try
            {
                IPHostEntry tIPhe = Dns.Resolve(SystemInformation.ComputerName);
                IPAddress tIP = tIPhe.AddressList[0];

                AppGlobal.s_ClientIP = "";
                for (int i = 0; i < tIPhe.AddressList.Length; i++)
                {
                    AppGlobal.s_ClientIP += tIPhe.AddressList[i].ToString() + "/";
                }

                AppGlobal.s_ClientIP = AppGlobal.s_ClientIP.Trim('/');

                GetMACAddress();

            }
            catch (Exception ex)
            {
                
            }
            m_LoginThread = new Thread(new ThreadStart(ProcessLogin));
            m_LoginThread.Start();
        }

        private void GetMACAddress()
        {
            ManagementObjectSearcher query = new ManagementObjectSearcher
                ("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled='TRUE'");
            ManagementObjectCollection queryCol = query.Get();

            string tMACAddress = "";

            foreach (ManagementObject mo in queryCol)
            {
                tMACAddress = (string)mo["MACAddress"];
            }

            AppGlobal.s_MacAddress = tMACAddress;
        }

        /// <summary>
        /// 로그인을 처리 합니다.
        /// </summary>
        private void ProcessLogin()
        {
            EventProcessor.LoginStarting(true);
        }

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
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, string.Concat(AppGlobal.s_RACTClientMode.ToString(), " Mode로 시작합니다."));
                if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online)
                {

                    if (AppGlobal.TryServerConnect() != E_ConnectError.NoError)
                    {
                        MessageBox.Show("서버에 연결 할 수 없습니다.");
                        // 2019-11-10 개선사항 (OneTerminal 접속시 진행 상태UI 기능)
                        ProgressBarText(progressBarX1,"서버에 연결 할 수 없습니다.");
                        ProgressBarType(progressBarX1, eProgressItemType.Standard);
                        return;
                    }
                   
                    if (!AppGlobal.LoginConnect())
                    {
                        // 2019-11-10 개선사항 (OneTerminal 접속시 진행 상태UI 기능)
                        ProgressBarText(progressBarX1, "서버에 로그인을 실패 했습니다.");
                        ProgressBarType(progressBarX1, eProgressItemType.Standard);
                        return;
                    }


                }

                 StartApplicationInit();
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 기본 데이터를 읽기 시작 합니다.
        /// </summary>
        public bool StartApplicationInit()
        {
            try
            {
				// 2019-11-10 개선사항 (OneTerminal 접속시 진행 상태UI 기능)
                ProgressBarText(progressBarX1, "프로그램 기본 정보를 로드 하는 중 입니다.");
                AppGlobal.s_DaemonProcessList = new Dictionary<int, DaemonProcessRemoteObject>();
                StartProcessResult();
                StartGetResult();
                StartRequestSend();

                loadDefaultData();
          
                StartTerminalExectueLogProcess();

                if (LoadDeviceInfo())
                {
                    ConnectDevice();
                }
            }
            catch (Exception ex)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, ex.ToString());
            }

            //현재 폼을 표시합니다.
            //this.Invoke(new ShowMainFormHandler(ShowMainForm));

            return true;
        }


        private bool LoadDeviceInfo()
        {
            DeviceInfo tDeviceInfo = new DeviceInfo();
            //서버에 사용할 수 있는 장비인지 확인합니다.

            RequestCommunicationData tRequestData = null;
            DeviceSearchInfo tSearchInfo = new DeviceSearchInfo();

            //2017.06.21 - NoSeungPil - RCCS 로그인 기능추가
            bool isFACTDevice = true;

            tSearchInfo.DeviceIPAddress = m_DeviceInfo.IPAddress;
            tSearchInfo.UserID = AppGlobal.s_LoginResult.UserID;
            tRequestData = AppGlobal.MakeDefaultRequestData();
            tSearchInfo.IsCheckPermission = false;
            tRequestData.CommType = E_CommunicationType.RequestFACTSearchDevice;

            tRequestData.RequestData = tSearchInfo;

            m_Result = null;
            m_MRE.Reset();

            AppGlobal.SendRequestData(this, tRequestData);
            m_MRE.WaitOne(AppGlobal.s_RequestTimeOut);


            if (m_Result == null)
            {
                //타임 아웃 처리 콘솔 모드로 변경 해야 하나?
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "타임 아웃 발생했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                tDeviceInfo = ((DeviceInfoCollection)AppGlobal.DecompressObject((CompressData)m_Result.ResultData))[m_DeviceInfo.IPAddress];

                if (tDeviceInfo == null)
                {
                    tDeviceInfo = new DeviceInfo();
                    tDeviceInfo.IsRegistered = false;
                    tDeviceInfo.IPAddress = m_DeviceInfo.IPAddress;
                    tDeviceInfo.TerminalConnectInfo = m_DeviceInfo.TerminalConnectInfo;

                    //2017.06.21 - NoSeungPil - RCCS 로그인 기능추가
                    isFACTDevice = false;
                }
                else
                {
                    //2017.06.21 - NoSeungPil - RCCS 로그인 기능추가
                    if (AppGlobal.s_ConnectionMode != 1)
                    {
                        if (AppGlobal.s_LoginResult.UserInfo.Centers.Count > 0)
                        {
                            if (!AppGlobal.s_LoginResult.UserInfo.Centers.Contains(tDeviceInfo.CenterCode))
                            {
                                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "해당 장비에 접속 권한이 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return false;
                            }
                        }
                    }

                    tDeviceInfo.TerminalConnectInfo = m_DeviceInfo.TerminalConnectInfo;
                }
            }

            //2017.06.21 - NoSeungPil - RCCS 로그인 기능추가
            if (AppGlobal.s_ConnectionMode == 1)
            {
                tDeviceInfo.IsRegistered = true;
                
                tDeviceInfo.IPAddress = AppGlobal.s_RCCSIP;
                tDeviceInfo.TerminalConnectInfo.TelnetPort = AppGlobal.s_RCCSPort;

                tDeviceInfo.TerminalConnectInfo.SerialConfig.PortName = "RCCSPort";

                if (!isFACTDevice)
                {
                    tSearchInfo.DeviceIPAddress = m_DeviceInfo.IPAddress;
                    tRequestData = AppGlobal.MakeDefaultRequestData();
                    tRequestData.CommType = E_CommunicationType.RequestRMSCMTSSearchDevice;

                    tRequestData.RequestData = tSearchInfo;

                    m_Result = null;
                    m_MRE.Reset();

                    AppGlobal.SendRequestData(this, tRequestData);
                    m_MRE.WaitOne(AppGlobal.s_RequestTimeOut);

                    if (m_Result == null)
                    {
                        //타임 아웃 처리 콘솔 모드로 변경 해야 하나?
                        AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "타임 아웃 발생했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else
                    {
                        DeviceInfo tDeviceInfoRMS = ((DeviceInfoCollection)AppGlobal.DecompressObject((CompressData)m_Result.ResultData))[m_DeviceInfo.IPAddress];

                        if (tDeviceInfoRMS == null)
                        {
                            AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "RCCS 로그인 : 미등록 장비입니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            if ((tDeviceInfoRMS.TelnetID1 == "") || (tDeviceInfoRMS.TelnetPwd1 == ""))
                            {
                                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "RCCS 로그인 : 로그인 정보가 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                tDeviceInfo.TelnetID1 = tDeviceInfoRMS.TelnetID1;
                                tDeviceInfo.TelnetPwd1 = tDeviceInfoRMS.TelnetPwd1;
                            }
                        }
                    }
                }

            }
			else if (AppGlobal.s_ConnectionMode == 2)
            {
                tDeviceInfo.IsRegistered = true;

                tDeviceInfo.IPAddress = AppGlobal.s_RPCSIP;
                tDeviceInfo.TerminalConnectInfo.TelnetPort = AppGlobal.s_RPCSPort;

                tDeviceInfo.TerminalConnectInfo.SerialConfig.PortName = "RPCSPort";
                //tDeviceInfo.TerminalConnectInfo.ConnectionProtocol = E_ConnectionProtocol.TELNET;

                // console(uart) 접속시  해당 장비가 Fact에 등록 되지 않은 장비는 
                //해당 정보를 재조회를 한다. 이때 제공해주는 주체가 있어야 하는데.
                //현재 정해진 것이 없음 2018-11-19
                //우선 2019-06-19 NE_NE 없으면 RMSCMT 테이블 조회 하도록 수정
                if (!isFACTDevice)
                {
                    tSearchInfo.DeviceIPAddress = m_DeviceInfo.IPAddress;
                    tRequestData = AppGlobal.MakeDefaultRequestData();
                    tRequestData.CommType = E_CommunicationType.RequestRMSCMTSSearchDevice;

                    tRequestData.RequestData = tSearchInfo;

                    m_Result = null;
                    m_MRE.Reset();

                    AppGlobal.SendRequestData(this, tRequestData);
                    m_MRE.WaitOne(AppGlobal.s_RequestTimeOut);

                    if (m_Result == null)
                    {
                        //타임 아웃 처리 콘솔 모드로 변경 해야 하나?
                        AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "타임 아웃 발생했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else
                    {
                        DeviceInfo tDeviceInfoRPCS = ((DeviceInfoCollection)AppGlobal.DecompressObject((CompressData)m_Result.ResultData))[m_DeviceInfo.IPAddress];

                        if (tDeviceInfoRPCS == null)
                        {
                            AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "RPCS 로그인 : 미등록 장비입니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            if ((tDeviceInfoRPCS.TelnetID1 == "") || (tDeviceInfoRPCS.TelnetPwd1 == ""))
                            {
                                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "RPCS 로그인 : 로그인 정보가 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                tDeviceInfo.TelnetID1 = tDeviceInfoRPCS.TelnetID1;
                                tDeviceInfo.TelnetPwd1 = tDeviceInfoRPCS.TelnetPwd1;
                            }
                        }
                    }
                }

            }
            else if (AppGlobal.s_ConnectionMode == 3)
            {
                tDeviceInfo.IsRegistered = true;

                //tDeviceInfo.IPAddress = AppGlobal.s_RPCSIP;
                //tDeviceInfo.TerminalConnectInfo.TelnetPort = AppGlobal.s_RPCSPort;

                tDeviceInfo.TerminalConnectInfo.SerialConfig.PortName = "RPCSLTE";

                
            }

            m_DeviceInfo = tDeviceInfo;
            return true;

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
                        tRemoteClientMethod = null;
                        if (AppGlobal.s_IsServerConnected)
                        {
                            tResultFailCount++;
                            if (tResultFailCount > 3)
                            {
                                AppGlobal.s_IsServerConnected = false;
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

            for (int i = 0; i < 3; i++)
            {
                m_ConnectError = AppGlobal.TryServerConnect();
                if (m_ConnectError == E_ConnectError.NoError)
                {
                    //서버에 다시 로그인 합니다.
                    AppGlobal.LoginConnect();
                    break;
                }
                Thread.Sleep(5000);
            }

            if (m_ConnectError != E_ConnectError.NoError)
            {
                this.Invoke(new DefaultHandler(MainFormClose));
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
            AppGlobal.s_ClientMainForm.Close();
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

        private void ucOneTerminal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (AppGlobal.s_TerminalExecuteLogProcess != null)
            {
                AppGlobal.s_TerminalExecuteLogProcess.Dispose();
            }
            if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online)
            {
                SendLogOut();
            }

            if (m_Emulator != null)
            {
                m_Emulator.Disconnect();
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

            
            //20170818 - NoSeungPil - RCCS 로그인의 경우 종료시 강제로 ctrl + d 전송
            if (AppGlobal.s_ConnectionMode == 1)
            {
                StringBuilder tSendString = new StringBuilder();

                tSendString.Append("Sub Main \r\n");
                tSendString.Append("\t" + Script.s_Send + " &chr(17)&chr(100)\r\n");
                /*
                tSendString.Append("\t" + Script.s_Send + " &chr(13)\r\n");
                tSendString.Append("\t" + Script.s_WaitForString + " \"" + "#" + "\"\r\n");
                tSendString.Append("\t" + Script.s_Send + " \"" + "exit" + "\"&chr(13)\r\n");
                tSendString.Append("\t" + Script.s_WaitForString + " \"" + ":" + "\"\r\n");
                tSendString.Append("\t" + Script.s_Send + " &chr(17)&chr(100)\r\n");
                 */

                tSendString.Append("End Sub");
                Script tScript = new Script(tSendString.ToString());

                MCTerminalEmulator m_SelectedEmulator =  new MCTerminalEmulator(true);

                m_SelectedEmulator.DeviceInfo = m_DeviceInfo;

                m_SelectedEmulator.RunScriptRCCS(tScript);

            }
            

            StopGetResult();
            StopRequestSend();
            StopProcessResult();
            StopTerminalExectueLogProcess();

            if (AppGlobal.s_FileLogProcessor != null)
            {
                AppGlobal.s_FileLogProcessor.Stop();
            }

            foreach (DaemonProcessRemoteObject tObject in AppGlobal.s_DaemonProcessList.Values)
            {
                tObject.Stop();
            }
        }

        public bool loadDefaultData()
        {
            RequestCommunicationData tRequestData = null;

            tRequestData = AppGlobal.MakeDefaultRequestData();


            tRequestData.CommType = E_CommunicationType.RequestOneTerminalModelInfo;
            tRequestData.RequestData = m_DeviceInfo.IPAddress;
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

            if (m_Result.ResultData != null)
            { 
                ModelInfo tModelInfo = (ModelInfo)m_Result.ResultData;
                AppGlobal.s_ModelInfoList.Add(tModelInfo);
            }

            if (AppGlobal.s_LoginResult.UserInfo.LimitedCmdUser)
            {

                tRequestData.CommType = E_CommunicationType.RequestLimitCmdInfo;

                //AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "제한 명령어 정보를 로딩 합니다.");

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

            return true;
        }

		// 2019-11-10 개선사항 (OneTerminal 접속시 진행 상태UI 기능)
        /// <summary>
        /// 크로스 스레드 대비 ProgressBar 컨트롤 함수
        /// </summary>
        public void ProgressBarText(ProgressBarX progressBarX, String sText)
        {
            if (progressBarX.InvokeRequired)
            {
                progressBarX.Invoke(new HandlerArgument2<ProgressBarX, String>(ProgressBarText), new object[] { progressBarX, sText });
                return;
            }

            if (progressBarX != null)
            {
                progressBarX.Text = sText;
            }
        }
        /// <summary>
        /// 크로스 스레드 대비 ProgressBar 컨트롤 함수
        /// </summary>
        public void ProgressBarVisble(Panel progressPanel, Boolean Value)
        {
            if (progressPanel.InvokeRequired)
            {
                progressPanel.Invoke(new HandlerArgument2<Panel, Boolean>(ProgressBarVisble), new object[] { progressPanel, Value });
                return;
            }

            if (progressPanel != null)
            {
                progressPanel.Visible = Value;
            }
        }
        /// <summary>
        /// 크로스 스레드 대비 ProgressBar 컨트롤 함수
        /// </summary>
        public void ProgressBarType(ProgressBarX progressBarX, eProgressItemType Value)
        {
            if (progressBarX.InvokeRequired)
            {
                progressBarX.Invoke(new HandlerArgument2<ProgressBarX, eProgressItemType>(ProgressBarType), new object[] { progressBarX, Value });
                return;
            }

            if (progressBarX != null)
            {
                progressBarX.ProgressType = Value;
            }
        }
        
    }

}