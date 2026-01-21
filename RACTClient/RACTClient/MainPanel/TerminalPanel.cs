using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using RACTCommonClass;
using DevComponents.DotNetBar;
using RACTTerminal;
using RACTSerialProcess;
using System.Threading;
using MKLibrary.MKProcess;

namespace RACTClient
{

    /// <summary>
    /// 터미널 변경에 사용할 핸들러 입니다.
    /// </summary>
    /// <param name="aTerminalName"></param>
    public delegate void TerminalTabChangeHandler(E_TerminalStatus aStatus, string aTerminalName);
    /// <summary>
    /// 터미널 Tab 패널 입니다.
    /// </summary>
    public partial class TerminalPanel : SenderControl, IMainPanel
    {
        /// <summary>
        /// 터미널 연결 스레드 풀 입니다.
        /// </summary>
        private MKThreadPool m_TerminalConnectThreadPool;
        /// <summary>
        /// 장비 수정 이벤트 입니다.
        /// </summary>
        public event ModifyDeviceHandler OnModifyDeviceEvent;


        /// <summary>
        /// 2013-01-18 - shinyn - 수동 장비 수정 이벤트 입니다.
        /// </summary>
        public event ModifyUsrDeviceHandler OnModifyUsrDeviceEvent;

        /// <summary>
        /// 터미널 탭 변경시 발할 이벤트 입니다.
        /// </summary>
        public event TerminalTabChangeHandler OnTerminalTabChangeEvent;
        /// <summary>
        /// 활성화 중인 터미널 목록입니다.
        /// </summary>
        private List<MCTerminalEmulator> m_EmulatorList = new List<MCTerminalEmulator>();

        /// <summary>
        /// 2013-05-02- shinyn - 링크장비정보입니다.
        /// </summary>
        private DeviceInfo m_LinkDeviceInfo = null;

        /// <summary>
        /// 2013-05-16- shinyn - 빠른 연결실행할 연결 형태를 비교한다.
        /// </summary>
        private TerminalConnectInfo m_ConnectInfo = null;

        /// <summary>
        /// 2013-01-11 - shinyn - 현재 열려있는 에뮬레이터 수를 가져와서 그이상 에뮬레이터 열수 없도록 수정
        /// </summary>
        public List<MCTerminalEmulator> EmulatorList
        {
            get { return m_EmulatorList; }
        }

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public TerminalPanel()
        {
            InitializeComponent();

            m_TerminalConnectThreadPool = new MKThreadPool("Terminal Connection ThreadPool", 3);
            m_TerminalConnectThreadPool.StartThreadPool();


            Image tImage = global::RACTClient.Properties.Resources.led_green;
            imlTab.Images.Add(tImage);
            tImage = global::RACTClient.Properties.Resources.led_red;
            imlTab.Images.Add(tImage);
            tImage = global::RACTClient.Properties.Resources.led_gray;
            imlTab.Images.Add(tImage);
            tImage = global::RACTClient.Properties.Resources.led_blue;
            imlTab.Images.Add(tImage);

            ucShortenCommand.OnSendShortenCommand += new HandlerArgument1<ShortenCommandInfo>(ShortenCommand_OnSendShortenCommand);
            ucShortenScript.OnSendScript += new HandlerArgument1<Script>(ShortenScript_OnSendScript);
        }
        /// <summary>
        /// 스크립트 명령 이벤트를 처리 합니다. 
        /// </summary>
        /// <param name="aValue1"></param>
        void ShortenScript_OnSendScript(Script aValue1)
        {
            this.BringToFront();

            if (AppGlobal.s_ClientOption.ShortenCommandTaget == E_ShortenCommandTagret.ActiveTerminal)
            {
                ScriptWork(E_ScriptWorkType.Run, aValue1);
                //((SuperTabControlPanel)tabTerminal.SelectedTab.AttachedControl).Controls[0].Focus();
                //MCTerminalEmulator tTerminal = (MCTerminalEmulator)((SuperTabControlPanel)tabTerminal.SelectedTab.AttachedControl).Controls[0];
                //if (tTerminal.TerminalStatus != E_TerminalStatus.Disconnected)
                //{
                //    tTerminal.RunScript(aValue1);
                //}
            }
            else
            {
                foreach (MCTerminalEmulator tTelnet in m_EmulatorList)
                {
                    if (tTelnet.TerminalStatus != E_TerminalStatus.Disconnected)
                    {
                        tTelnet.RunScript(aValue1);
                    }
                }
            }
        }
        /// <summary>
        /// 단축 명령 이벤트를 처리 합니다.
        /// </summary>
        /// <param name="aValue1"></param>
        void ShortenCommand_OnSendShortenCommand(ShortenCommandInfo aValue1)
        {
            //if (tabTerminal.SelectedTabIndex < 0) return;
            if (AppGlobal.s_ClientOption.ShortenCommandTaget == E_ShortenCommandTagret.ActiveTerminal)
            {
                m_TerminalSelectForm.TerminalList = m_EmulatorList;
                MCTerminalEmulator tSelectedTerminal = null;
                if (m_TerminalSelectForm.ShowDialog(AppGlobal.s_ClientMainForm) == DialogResult.OK)
                {
                    tSelectedTerminal = m_TerminalSelectForm.SelectedTerminal;
                    if (tSelectedTerminal.Parent is TerminalWindows)
                    {
                        ((Form)tSelectedTerminal.Parent).BringToFront();
                    }
                    else
                    {
                        tabTerminal.SelectedTab = ((SuperTabControlPanel)tSelectedTerminal.Parent).TabItem;
                    }
                    //tSelectedTerminal.DispatchMessage(this, aValue1.Command + "\r\n");
                    tSelectedTerminal.IsLimitCmdForShortenCommand(this, aValue1.Command + "\r\n");
                }
            }
            else
            {
                foreach (MCTerminalEmulator tTelnet in m_EmulatorList)
                {
                    //tTelnet.DispatchMessage(this, aValue1.Command + "\r\n");
                    tTelnet.IsLimitCmdForShortenCommand(this, aValue1.Command + "\r\n");
                }
            }
        }

        /// <summary>
        /// 터미널을 추가 할 수 있는지 확인 합니다.
        /// </summary>
        /// <param name="tCount"></param>
        /// <returns></returns>
        private bool CheckTerminalCount(DeviceInfo aDeviceInfo, out int tCount)
        {
            tCount = 0;

            if (m_EmulatorList.Count >= 20)
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "최대 연결 개수는 20개 입니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            DeviceInfo tDeviceInfo;
            string tTabName = "";
            int tMaxNumber = 0;
            int tTempNumber = 0;
            for (int i = 0; i < tabTerminal.Tabs.Count; i++)
            {
                tDeviceInfo = ((SuperTabItem)tabTerminal.Tabs[i]).Tag as DeviceInfo;
                tTabName = ((SuperTabItem)tabTerminal.Tabs[i]).Text;

                if (aDeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET)
                {
                    if (tDeviceInfo.IPAddress.Equals(aDeviceInfo.IPAddress))
                    {
                        if (tTabName.Contains("("))
                        {
                            // 2013-05-02 - 아이피가 겹치는 경우 뒤에 이름 추가
                            try
                            {
                                tTempNumber = int.Parse(tTabName.Substring(tTabName.IndexOf("(") + 1, tTabName.Length - tTabName.IndexOf(")")));
                            }
                            catch (Exception ex)
                            {
                                tTempNumber = tMaxNumber + 1;
                            }

                            if (tTempNumber > tMaxNumber)
                            {
                                tMaxNumber = tTempNumber;
                            }
                        }
                        else
                        {
                            tCount++;
                        }
                    }
                }
                else if (aDeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
                {

                    // 2013-03-06 - shinyn - SSH텔넷인경우 분기 처리 추가
                    if (tDeviceInfo.IPAddress.Equals(aDeviceInfo.IPAddress))
                    {
                        if (tTabName.Contains("("))
                        {
                            tTempNumber = int.Parse(tTabName.Substring(tTabName.IndexOf("(") + 1, tTabName.Length - tTabName.IndexOf(")")));
                            if (tTempNumber > tMaxNumber)
                            {
                                tMaxNumber = tTempNumber;
                            }
                        }
                        else
                        {
                            tCount++;
                        }
                    }
                }
                else
                {
                    if (tDeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SERIAL_PORT)
                    {
                        if (tDeviceInfo.TerminalConnectInfo.SerialConfig.PortName.Equals(aDeviceInfo.TerminalConnectInfo.SerialConfig.PortName))
                        {
                            if (tTabName.Contains("("))
                            {
                                tTempNumber = int.Parse(tTabName.Substring(tTabName.IndexOf("(") + 1, tTabName.Length - tTabName.IndexOf(")")));
                                if (tTempNumber > tMaxNumber)
                                {
                                    tMaxNumber = tTempNumber;
                                }
                            }
                            else
                            {
                                tCount++;
                            }
                        }
                    }
                }
            }

            if (tMaxNumber > 0)
            {
                tCount = tMaxNumber + 1;
            }

            return true;
        }
        /// <summary>
        /// 터미널을 생성 합니다.
        /// </summary>
        private MCTerminalEmulator MakeEmulator(DeviceInfo aDeviceInfo, bool aIsQuickConnection)
        {
            MCTerminalEmulator tEmulator = new MCTerminalEmulator(aIsQuickConnection);
            tEmulator.OnTelnetFindString += new DefaultHandler(Emulator_OnTelnetFindString);
            tEmulator.OnTerminalStatusChange += new HandlerArgument2<MCTerminalEmulator, E_TerminalStatus>(tEmulator_OnTerminalStatusChange);
            tEmulator.Modes = new Mode();
            tEmulator.Modes.Flags = tEmulator.Modes.Flags |= Mode.s_AutoWrap;
            tEmulator.Dock = DockStyle.Fill;
            tEmulator.DeviceInfo = aDeviceInfo;

            AppGlobal.s_ClientOption.AddConnectionHistory(aDeviceInfo);

            if (aDeviceInfo.DevicePartCode == 1 || /* 집선스위치 */
                aDeviceInfo.DevicePartCode == 6 || /* G-PON-OLT */
                aDeviceInfo.DevicePartCode == 31 /* NG-PON-OLT */ )
            {
				// 2019-11-10 개선사항 (옵션 설정 개선)
                tEmulator.ApplyOption();
                    //tEmulator.SetFontColor(AppGlobal.s_ClientOption.HighlightFontColor);
                    //tEmulator.SetBackGroundColor(AppGlobal.s_ClientOption.HighlightBackGroundColor);

            }

            m_EmulatorList.Add(tEmulator);

            return tEmulator;
        }
        /// <summary>
        /// 패널을 생성 합니다.
        /// </summary>
        private SuperTabItem MakeTabPanel(MCTerminalEmulator aMCTerminalEmulator, int aCount)
        {

            SuperTabControlPanel tTabPanel;
            SuperTabItem tTabItem;
            tTabItem = new DevComponents.DotNetBar.SuperTabItem();
            tTabPanel = new DevComponents.DotNetBar.SuperTabControlPanel();
            tTabItem.MouseUp += new MouseEventHandler(TabItem_MouseUp);
            tabTerminal.Controls.Add(tTabPanel);
            tTabPanel.Controls.Add(aMCTerminalEmulator);
            tTabItem.AttachedControl = tTabPanel;
            tTabItem.GlobalItem = false;


            if (aMCTerminalEmulator.DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET)
            {
                if (AppGlobal.s_ClientOption.TerminalDisplayNameType == E_TerminalDisplayNameType.IPAddress)
                {
                    tTabItem.Name = aCount > 0 ? aMCTerminalEmulator.DeviceInfo.IPAddress + "(" + aCount.ToString() + ")" : aMCTerminalEmulator.DeviceInfo.IPAddress;
                }
                else
                {
                    tTabItem.Name = aCount > 0 ? aMCTerminalEmulator.DeviceInfo.Name + "(" + aCount.ToString() + ")" : aMCTerminalEmulator.DeviceInfo.Name;
                }
            }
            else if (aMCTerminalEmulator.DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
            {
                // 2013-03-06 - shinyn - SSH텔넷인 경우 분기처리 추가
                if (AppGlobal.s_ClientOption.TerminalDisplayNameType == E_TerminalDisplayNameType.IPAddress)
                {
                    tTabItem.Name = aCount > 0 ? aMCTerminalEmulator.DeviceInfo.IPAddress + "(" + aCount.ToString() + ")" : aMCTerminalEmulator.DeviceInfo.IPAddress;
                }
                else
                {
                    tTabItem.Name = aCount > 0 ? aMCTerminalEmulator.DeviceInfo.Name + "(" + aCount.ToString() + ")" : aMCTerminalEmulator.DeviceInfo.Name;
                }
            }
            else
            {
                tTabItem.Name = aCount > 0 ? "Serial-" + aMCTerminalEmulator.DeviceInfo.TerminalConnectInfo.SerialConfig.PortName + "(" + aCount + ")" : "Serial-" + aMCTerminalEmulator.DeviceInfo.TerminalConnectInfo.SerialConfig.PortName;
            }
            tTabItem.Text = tTabItem.Name;
            aMCTerminalEmulator.DeviceInfo.TerminalName = tTabItem.Name;
            tTabItem.Image = (Image)global::RACTClient.Properties.Resources.TryConnect;
            aMCTerminalEmulator.Name = tTabItem.Name;

            tTabItem.Tag = aMCTerminalEmulator.DeviceInfo;
            tTabItem.Tooltip = aMCTerminalEmulator.ToolTip;
            tTabPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            tTabPanel.Location = new System.Drawing.Point(0, 26);
            tTabPanel.Name = "superTabControlPanel1";
            tTabPanel.Size = new System.Drawing.Size(150, 124);
            tTabPanel.TabIndex = 1;
            tTabPanel.TabItem = tTabItem;

            aMCTerminalEmulator.Name = tTabItem.Text;


            return tTabItem;

        }

        internal void AddTerminal(MCTerminalEmulator mCTerminalEmulator)
        {
            int tCount = 0;

            if (!CheckTerminalCount(mCTerminalEmulator.DeviceInfo, out tCount)) return;

            SuperTabItem tTabItem = MakeTabPanel(mCTerminalEmulator, tCount);
            tabTerminal.Tabs.AddRange(new DevComponents.DotNetBar.BaseItem[] { tTabItem });
            tabTerminal.ReorderTabsEnabled = true;
            mCTerminalEmulator.TerminalStatus = mCTerminalEmulator.TerminalStatus;
            tabTerminal.SelectedTab = tTabItem;

        }

        /// <summary>
        /// 터미널을 추가 합니다.
        /// </summary>
        /// <param name="aDeviceInfo">장비 정보 입니다.</param>
        /// <param name="aIsQuickConnection">빠른 연결 여부</param>
        public void AddTerminal(DeviceInfo aDeviceInfo, bool aIsQuickConnection, DaemonProcessInfo aDaemonProcessInfo)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument3<DeviceInfo, bool, DaemonProcessInfo>(AddTerminal), new object[] { aDeviceInfo, aIsQuickConnection, aDaemonProcessInfo });
                return;
            }

            if (aDaemonProcessInfo == null)
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "사용 가능한 Daemon 정보 로드에 실패 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "사용 가능한 Daemon 정보 로드에 실패 했습니다.");
                return;
            }
            int tCount = 0;
            if (!CheckTerminalCount(aDeviceInfo, out tCount)) return;

            MCTerminalEmulator tEmulator = MakeEmulator(aDeviceInfo, aIsQuickConnection);
            if (AppGlobal.s_ClientOption.TerminalWindowsPopupType == E_DefaultTerminalPopupType.Tab)
            {
                tEmulator.DaemonProcessInfo = aDaemonProcessInfo;
                SuperTabItem tTabItem = MakeTabPanel(tEmulator, tCount);
                tabTerminal.Tabs.AddRange(new DevComponents.DotNetBar.BaseItem[] { tTabItem });
                tabTerminal.ReorderTabsEnabled = true;

                tabTerminal.SelectedTab = tTabItem;
            }
            else
            {
                TerminalWindows tForm = new TerminalWindows();
                tEmulator.Dock = DockStyle.Fill;
                tForm.AddTerminalControl(tEmulator);

                tForm.Size = new Size(AppGlobal.s_ClientOption.PopupSizeWidth, AppGlobal.s_ClientOption.PopupSizeHeight);


                if (tEmulator.DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET)
                {
                    tEmulator.Name = tEmulator.DeviceInfo.IPAddress;
                    if (tCount > 0)
                    {
                        tEmulator.Name += "(" + tCount.ToString() + ")";
                    }
                }
                else if (tEmulator.DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
                {
                    // 2013-03-06 - shinyn - SSH텔넷인 경우 분기처리 추가
                    tEmulator.Name = tEmulator.DeviceInfo.IPAddress;
                    if (tCount > 0)
                    {
                        tEmulator.Name += "(" + tCount.ToString() + ")";
                    }
                }
                else
                {
                    tEmulator.Name = "Serial-" + tEmulator.DeviceInfo.TerminalConnectInfo.SerialConfig.PortName;
                }



                tForm.Text = tEmulator.Name;
                tForm.MaximizeBox = false;

                tForm.Show();
            }

            // 2013-05-02 - shinyn - 터미널 연결시 연결안되는 오류를 수정하기 위해, 텔넷 연결시 WorkItem을 같이 보내서 연결하도록 수정합니다.
            //m_TerminalConnectThreadPool.ExecuteWork(new MKWorkItem(new WorkItemDefaultMethod(tEmulator.ConnectDevice)));

            MKWorkItem tWorkItem = new MKWorkItem(new WorkItemParmeterMethod(tEmulator.ConnectDevice), new DeviceInfo(tEmulator.DeviceInfo));
            m_TerminalConnectThreadPool.ExecuteWork(tWorkItem);


            if (OnTerminalTabChangeEvent != null) OnTerminalTabChangeEvent(E_TerminalStatus.Add, tEmulator.Name);
        }


        /// <summary>
        /// 터미널을 추가 합니다.
        /// </summary>
        /// <param name="aDeviceInfo">장비 정보 입니다.</param>
        /// <param name="aIsQuickConnection">빠른 연결 여부</param>
        public void AddTerminal(DeviceInfo aDeviceInfo, bool aIsQuickConnection)
        {
            // 2013-05-02 - shinyn - 빠른연결 처리시 재시도 제외처리
            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument2<DeviceInfo, bool>(AddTerminal), new object[] { aDeviceInfo, aIsQuickConnection });
                return;
            }

            int tCount = 0;
            if (!CheckTerminalCount(aDeviceInfo, out tCount)) return;

            MCTerminalEmulator tEmulator = MakeEmulator(aDeviceInfo, aIsQuickConnection);
            if (AppGlobal.s_ClientOption.TerminalWindowsPopupType == E_DefaultTerminalPopupType.Tab)
            {
                SuperTabItem tTabItem = MakeTabPanel(tEmulator, tCount);
                tabTerminal.Tabs.AddRange(new DevComponents.DotNetBar.BaseItem[] { tTabItem });
                tabTerminal.ReorderTabsEnabled = true;
                tabTerminal.SelectedTab = tTabItem;
            }
            else
            {
                TerminalWindows tForm = new TerminalWindows();
                tEmulator.Dock = DockStyle.Fill;
                tForm.AddTerminalControl(tEmulator);
                tForm.Size = new Size(AppGlobal.s_ClientOption.PopupSizeWidth, AppGlobal.s_ClientOption.PopupSizeHeight);


                if (tEmulator.DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET)
                {
                    tEmulator.Name = tEmulator.DeviceInfo.IPAddress;
                }
                else if (tEmulator.DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
                {
                    // 2013-03-06 - shinyn - SSH텔넷인 경우 분기처리 추가
                    tEmulator.Name = tEmulator.DeviceInfo.IPAddress;
                }
                else
                {
                    tEmulator.Name = "Serial-" + tEmulator.DeviceInfo.TerminalConnectInfo.SerialConfig.PortName;
                }
                tForm.Text = tEmulator.Name;
                tForm.MaximizeBox = false;
                tForm.Show();
            }

            // 2013-05-02 - shinyn - 터미널 연결시 연결안되는 오류를 수정하기 위해, 텔넷 연결시 WorkItem을 같이 보내서 연결하도록 수정합니다.
            //m_TerminalConnectThreadPool.ExecuteWork(new MKWorkItem(new WorkItemDefaultMethod(tEmulator.ConnectDevice)));

            MKWorkItem tWorkItem = new MKWorkItem(new WorkItemParmeterMethod(tEmulator.ConnectDevice), new DeviceInfo(tEmulator.DeviceInfo));


            m_TerminalConnectThreadPool.ExecuteWork(tWorkItem);

            //2013-05-02 - shinyn - 터미널 연결 스레드 풀요청과 받는 것을 제대로 받았는지 체크한다.
            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "AddTerminal : ThreadPool 시작 : " + tEmulator.DeviceInfo.IPAddress);



            if (OnTerminalTabChangeEvent != null) OnTerminalTabChangeEvent(E_TerminalStatus.Add, tEmulator.Name);
        }

        /// <summary>
        /// 2014-10-14 - 신윤남 - 창닫히는 경우 에뮬레이터 리스트에서 삭제
        /// </summary>
        /// <param name="aValue1"></param>
        /// <param name="aValue2"></param>
        public void tEmulator_OnTerminalStatusChange(MCTerminalEmulator aValue1, E_TerminalStatus aValue2)
        {
            try
            {
                if (OnTerminalTabChangeEvent != null) OnTerminalTabChangeEvent(aValue2, aValue1.Name);

                if (aValue2 == E_TerminalStatus.Disconnected)
                {
                    lock (m_EmulatorList)
                    {
                        m_EmulatorList.Remove(aValue1);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// 빠른 연결 처리 합니다.
        /// </summary>
        /// <param name="quickConnectInfo"></param>
        internal void QuickConnect(TerminalConnectInfo aConnectInfo)
        {
            DeviceInfo tDeviceInfo = null;

            m_ConnectInfo = aConnectInfo;

            if (aConnectInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET)
            {
                if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online)
                {
                    //서버에 사용할 수 있는 장비인지 확인합니다.

                    RequestCommunicationData tRequestData = null;
                    DeviceSearchInfo tSearchInfo = new DeviceSearchInfo();

                    tSearchInfo.DeviceIPAddress = aConnectInfo.IPAddress;
                    tSearchInfo.UserID = AppGlobal.s_LoginResult.UserID;
                    tRequestData = AppGlobal.MakeDefaultRequestData();
                    tSearchInfo.IsCheckPermission = false;
                    tRequestData.CommType = E_CommunicationType.RequestFACTSearchDevice;

                    tRequestData.RequestData = tSearchInfo;

                    m_Result = null;
                    m_MRE.Reset();

                    AppGlobal.SendRequestData(this, tRequestData);
                    m_MRE.WaitOne(AppGlobal.s_RequestTimeOut);
                }
                else
                {
                    if (tDeviceInfo == null)

                        tDeviceInfo = new DeviceInfo();
                    tDeviceInfo.IsRegistered = false;
                    tDeviceInfo.IPAddress = m_ConnectInfo.IPAddress;
                    tDeviceInfo.TerminalConnectInfo = m_ConnectInfo;

                    AddTerminal(tDeviceInfo, true);

                }
                /*
                if (m_Result == null)
                {
                    //타임 아웃 처리 콘솔 모드로 변경 해야 하나?
                    AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "타임 아웃 발생했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                    //tDeviceInfo = new DeviceInfo();
                    //tDeviceInfo.IsRegistered = false;
                    //tDeviceInfo.IPAddress = aConnectInfo.IPAddress;
                    //tDeviceInfo.TerminalConnectInfo = aConnectInfo;
                }
                else
                {
                    tDeviceInfo = ((DeviceInfoCollection)AppGlobal.DecompressObject((CompressData)m_Result.ResultData))[aConnectInfo.IPAddress];

                    if (tDeviceInfo == null)
                    {
                        tDeviceInfo = new DeviceInfo();
                        tDeviceInfo.IsRegistered = false;
                        tDeviceInfo.IPAddress = aConnectInfo.IPAddress;
                        tDeviceInfo.TerminalConnectInfo = aConnectInfo;
                    }
                    else
                    {
                        if (AppGlobal.s_LoginResult.UserInfo.Centers.Count > 0)
                        {
                            if (!AppGlobal.s_LoginResult.UserInfo.Centers.Contains(tDeviceInfo.CenterCode))
                            {
                                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "해당 장비에 접속 권한이 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        tDeviceInfo.TerminalConnectInfo = aConnectInfo;
                    }
                }
            }

                
            */
            }
            else if (aConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
            {
                // 2013-03-06 - shinyn - SSH텔넷인 경우 분기처리 추가
                if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online)
                {
                    //서버에 사용할 수 있는 장비인지 확인합니다.

                    RequestCommunicationData tRequestData = null;
                    DeviceSearchInfo tSearchInfo = new DeviceSearchInfo();

                    tSearchInfo.DeviceIPAddress = aConnectInfo.IPAddress;
                    tSearchInfo.UserID = AppGlobal.s_LoginResult.UserID;
                    tRequestData = AppGlobal.MakeDefaultRequestData();
                    tSearchInfo.IsCheckPermission = false;
                    tRequestData.CommType = E_CommunicationType.RequestFACTSearchDevice;

                    tRequestData.RequestData = tSearchInfo;

                    m_Result = null;
                    m_MRE.Reset();

                    AppGlobal.SendRequestData(this, tRequestData);
                    m_MRE.WaitOne(AppGlobal.s_RequestTimeOut);
                }
                else
                {
                    if (tDeviceInfo == null)

                        tDeviceInfo = new DeviceInfo();
                    tDeviceInfo.IsRegistered = false;
                    tDeviceInfo.IPAddress = m_ConnectInfo.IPAddress;
                    tDeviceInfo.TerminalConnectInfo = m_ConnectInfo;

                    AddTerminal(tDeviceInfo, true);

                }
                /*
                if (m_Result == null)
                {
                    //타임 아웃 처리 콘솔 모드로 변경 해야 하나?
                    AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "타임 아웃 발생했습니다.",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    tDeviceInfo = ((DeviceInfoCollection)AppGlobal.DecompressObject((CompressData)m_Result.ResultData))[aConnectInfo.IPAddress];

                    if (tDeviceInfo == null)
                    {
                        tDeviceInfo = new DeviceInfo();
                        tDeviceInfo.IsRegistered = false;
                        tDeviceInfo.IPAddress = aConnectInfo.IPAddress;
                        tDeviceInfo.TerminalConnectInfo = aConnectInfo;
                    }
                    else
                    {
                        if (AppGlobal.s_LoginResult.UserInfo.Centers.Count > 0)
                        {
                            if (!AppGlobal.s_LoginResult.UserInfo.Centers.Contains(tDeviceInfo.CenterCode))
                            {
                                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "해당 장비에 접속 권한이 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        tDeviceInfo.TerminalConnectInfo = aConnectInfo;
                    }
                }
                    
            }

            if (tDeviceInfo == null)
            {
                tDeviceInfo = new DeviceInfo();
                tDeviceInfo.IsRegistered = false;
                tDeviceInfo.IPAddress = aConnectInfo.IPAddress;
                tDeviceInfo.TerminalConnectInfo =  aConnectInfo ;
            }
            */
            }
            else if (aConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SERIAL_PORT)
            {//2020-10-05 TACT 기능개선 Console 접속 시 터미널(COM-PORT)접속 실행 불가 수정 
                if (tDeviceInfo == null)
                    tDeviceInfo = new DeviceInfo();
                tDeviceInfo.IsRegistered = false;
                tDeviceInfo.IPAddress = m_ConnectInfo.IPAddress;
                tDeviceInfo.TerminalConnectInfo = m_ConnectInfo;

                AddTerminal(tDeviceInfo, true);
            }
            

            //AddTerminal(tDeviceInfo, true);
        }

        /// <summary>
        /// 2013-01-11 - shinyn - 목록불러오기로 장비연결시 사용됨.
        /// </summary>
        /// <param name="aDeviceInfo"></param>
        /// <param name="aConnectInfo"></param>
        internal void OpenDeviceConnect(DeviceInfo aDeviceInfo, TerminalConnectInfo aConnectInfo)
        {


            if (aConnectInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET)
            {
                aDeviceInfo.IsRegistered = false;
                aDeviceInfo.TerminalConnectInfo = aConnectInfo;
            }
            else if (aConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
            {
                // 2013-03-06 - shinyn - SSH텔넷 기능인 경우 분기처리 추가
                aDeviceInfo.IsRegistered = false;
                aDeviceInfo.TerminalConnectInfo = aConnectInfo;
            }
            else
            {
                aDeviceInfo.IsRegistered = false;
                aDeviceInfo.TerminalConnectInfo = aConnectInfo;
            }
            AddTerminal(aDeviceInfo, true);
        }

        /// <summary>
        /// 탭 마우스 up 처리 합니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TabItem_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                SuperTabItem tBaseItem = (SuperTabItem)sender;
                SuperTabControlPanel tPanel = (SuperTabControlPanel)tBaseItem.AttachedControl;
                MCTerminalEmulator tEmulator = (MCTerminalEmulator)tPanel.Controls[0];
                m_SelectedEmulator = tEmulator;
                mnuModifyDevice.Enabled = tEmulator.DeviceInfo.IsRegistered;

                // 2013-01-11 - shinyn - 복원 명령 실행 메뉴 추가
                mnuRestoreCfgCmd.Enabled = tEmulator.IsConnected;

                mnuDisConnect.Enabled = tEmulator.IsConnected;
                //2013-05-02 - shinyn - 링크 장비 연결은 연결된 상태에서 해야함.
                mnuLinkConnect.Enabled = tEmulator.IsConnected;
                mnuReConnect.Enabled = !tEmulator.IsConnected;

                ShowContextMenu(cmTabPopup);
            }
        }
        /// <summary>
        /// 선택된 Emulator 입니다.
        /// </summary>
        private MCTerminalEmulator m_SelectedEmulator = null;
        //public MCTerminalEmulator m_SelectedEmulator = null;
        /// <summary>
        /// 팝업 메뉴를 표시 합니다.
        /// </summary>
        /// <param name="cm"></param>
        private void ShowContextMenu(ButtonItem aPopup)
        {
            aPopup.Popup(MousePosition);
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
            //    AppGlobal.s_TelnetFindForm.Show(AppGlobal.s_ClientMainForm);
            //    AppGlobal.s_TelnetFindForm.Activate();
            //}
            //else if (!AppGlobal.s_TelnetFindForm.Visible)
            //{
            //    AppGlobal.s_TelnetFindForm.Visible = true;
            //  //  AppGlobal.s_TelnetFindForm.Activate();
            //  //  AppGlobal.s_TelnetFindForm.Focus();
            //}
            //else
            //{
            //   // AppGlobal.s_TelnetFindForm.Activate();
            //   // AppGlobal.s_TelnetFindForm.Focus();
            //}
        }

        void s_TelnetFindForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tabTerminal.Tabs.Count == 0) return;

            ((MCTerminalEmulator)(tabTerminal.SelectedTab.AttachedControl.Controls[0])).FindForm_Close();
        }
        /// <summary>
        /// 찾기 처리 합니다.
        /// </summary>
        void TelnetFindForm_OnTelnetStringFind(TelnetStringFindHandlerArgs aStringArgs)
        {
            if (tabTerminal.Tabs.Count == 0) return;

            ((MCTerminalEmulator)(tabTerminal.SelectedTab.AttachedControl.Controls[0])).FindForm_OnTelnetStringFind(aStringArgs);
        }
        /// <summary>
        /// 닫기 처리된 컨트롤 입니다.
        /// </summary>
        private MCTerminalEmulator m_CloseEmulator = null;
        /// <summary>
        /// 탭 닫기 처리 합니다.
        /// </summary>
        private void tabTerminal_TabItemClose(object sender, SuperTabStripTabItemCloseEventArgs e)
        {
            if (((SuperTabItem)e.Tab).AttachedControl.Controls.Count == 0) return;

            MCTerminalEmulator tCloseEmulator = ((SuperTabItem)e.Tab).AttachedControl.Controls[0] as MCTerminalEmulator;

            // 2014-08-19 - 신윤남 - 종료 클릭시에는 상위 Parent를 종료하지 않도록 한다.
            tCloseEmulator.Tag = "TabItemClose";

            if (tCloseEmulator.IsConnected)
            {
                tCloseEmulator.Disconnect();
                tCloseEmulator.TerminalStatus = E_TerminalStatus.Disconnected;
            }
        }
        /// <summary>
        /// 탭 선택 변경 처리 합니다.
        /// </summary>
        private void tabTerminal_SelectedTabChanged(object sender, SuperTabStripSelectedTabChangedEventArgs e)
        {
            if (tabTerminal.SelectedTabIndex < 0)
            {
                m_SelectedEmulator = null;
                return;
            }

            if (((SuperTabControlPanel)tabTerminal.SelectedTab.AttachedControl).Controls.Count == 0) return;

            ((SuperTabControlPanel)tabTerminal.SelectedTab.AttachedControl).Controls[0].Focus();
            MCTerminalEmulator tEmulator = (MCTerminalEmulator)((SuperTabControlPanel)tabTerminal.SelectedTab.AttachedControl).Controls[0];
            m_SelectedEmulator = tEmulator;
            if (OnTerminalTabChangeEvent != null) OnTerminalTabChangeEvent(tEmulator.TerminalStatus, tabTerminal.SelectedTab.Name);
        }
        /// <summary>
        /// 재 접속 처리 합니다.
        /// </summary>
        private void mnuReConnect_Click(object sender, EventArgs e)
        {
            if (m_SelectedEmulator == null) return;
            if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Console)
            {
                m_SelectedEmulator.DeviceInfo.IsRegistered = false;
            }
            //MCTerminalEmulator tMCTerminalEmulator = (MCTerminalEmulator)((SuperTabControlPanel)tabTerminal.SelectedTab.AttachedControl).Controls[0];
            // 2013-05-02 - shinyn - 터미널 연결시 연결안되는 오류를 수정하기 위해, 텔넷 연결시 WorkItem을 같이 보내서 연결하도록 수정합니다.
            //m_SelectedEmulator.ConnectDevice();
            m_SelectedEmulator.ConnectDevice(m_SelectedEmulator.DeviceInfo);

            m_EmulatorList.Add(m_SelectedEmulator);

            // 재연결시 바로 터미널에서 키보드 입력할 수 있도록 포커스 처리
            m_SelectedEmulator.Focus();
        }
        /// <summary>
        /// 접속 종료 처리 합니다.
        /// </summary>
        private void mnuDisConnect_Click(object sender, EventArgs e)
        {
            if (m_SelectedEmulator == null) return;
            //MCTerminalEmulator tMCTerminalEmulator = (MCTerminalEmulator)((SuperTabControlPanel)tabTerminal.SelectedTab.AttachedControl).Controls[0];
            m_SelectedEmulator.Disconnect();
        }
        /// <summary>
        /// 새로운 탭 연결 처리 합니다.
        /// </summary>
        private void mnuNewTab_Click(object sender, EventArgs e)
        {
            //MCTerminalEmulator tMCTerminalEmulator = (MCTerminalEmulator)((SuperTabControlPanel)tabTerminal.SelectedTab.AttachedControl).Controls[0];
            if (m_SelectedEmulator == null) return;
            AddTerminal(m_SelectedEmulator.DeviceInfo, m_SelectedEmulator.IsQuickConnection);
        }
        /// <summary>
        /// 장비 수정 처리 합니다.
        /// </summary>
        private void mnuModifyDevice_Click(object sender, EventArgs e)
        {
            if (m_SelectedEmulator == null) return;
            DeviceInfo tDeviceInfo = m_SelectedEmulator.DeviceInfo;

            // 2013-01-18 - shinyn - 수동, 일반장비 수정합니다.
            switch (tDeviceInfo.DeviceType)
            {
                case E_DeviceType.NeGroup:
                    if (OnModifyDeviceEvent != null)
                    {
                        OnModifyDeviceEvent(E_WorkType.Modify, tDeviceInfo);
                    }
                    break;
                case E_DeviceType.UserNeGroup:
                    if (OnModifyUsrDeviceEvent != null)
                    {
                        OnModifyUsrDeviceEvent(E_WorkType.Modify, tDeviceInfo);
                    }
                    break;
            }
        }
        /// <summary>
        /// 터미널 개수를 가져오기 합니다.
        /// </summary>
        public int TerminalCount
        {
            get { return m_EmulatorList.Count; }
        }
        /// <summary>
        /// 변경된 환경 정보를 적용합니다.
        /// </summary>
        internal void ChangeOption()
        {
            foreach (MCTerminalEmulator tEmulator in m_EmulatorList)
            {
                tEmulator.ApplyOption();
            }
        }
        /// <summary>
        /// 컨트롤을 초기화 시킵니다.
        /// </summary>
        public void InitializeControl()
        {
            m_EmulatorList = new List<MCTerminalEmulator>();
        }
        /// <summary>
        /// 탭 이름 변경 처리 합니다.
        /// </summary>
        private void mnuReName_Click(object sender, EventArgs e)
        {
            TerminalReName tReName = new TerminalReName(tabTerminal.SelectedTab.Text);
            tReName.InitializeContro();
            if (tReName.ShowDialog(this) == DialogResult.OK)
            {
                tabTerminal.SelectedTab.Text = tReName.GetNewTabName;
            }
        }
        /// <summary>
        /// 터미널 선택 화면 입니다.
        /// </summary>
        private SelectTargetTerminal m_TerminalSelectForm = new SelectTargetTerminal();
        /// <summary>
        /// 스크립트 처리 합니다.
        /// </summary>
        internal void ScriptWork(E_ScriptWorkType aScriptWorkType)
        {
            if (m_EmulatorList.Count == 0) return;

            m_TerminalSelectForm.TerminalList = m_EmulatorList;
            MCTerminalEmulator tSelectedTerminal = null;
            if (m_TerminalSelectForm.ShowDialog(AppGlobal.s_ClientMainForm) == DialogResult.OK)
            {
                tSelectedTerminal = m_TerminalSelectForm.SelectedTerminal;
                if (tSelectedTerminal.Parent is TerminalWindows)
                {
                    ((Form)tSelectedTerminal.Parent).BringToFront();
                }
                else
                {
                    tabTerminal.SelectedTab = ((SuperTabControlPanel)tSelectedTerminal.Parent).TabItem;
                }
                tSelectedTerminal.ScriptWork(aScriptWorkType);
            }
        }
        /// <summary>
        /// 스크립트 처리 합니다.
        /// </summary>
        internal void ScriptWork(E_ScriptWorkType e_ScriptWorkType, Script aScript)
        {
            //if (tabTerminal.SelectedTabIndex < 0) return;
            ////(Image)global::RACTClient.Properties.Resources.;
            //((SuperTabControlPanel)tabTerminal.SelectedTab.AttachedControl).Controls[0].Focus();
            //((MCTerminalEmulator)((SuperTabControlPanel)tabTerminal.SelectedTab.AttachedControl).Controls[0]).RunScript(aScript);

            if (m_EmulatorList.Count == 0) return;

            m_TerminalSelectForm.TerminalList = m_EmulatorList;
            MCTerminalEmulator tSelectedTerminal = null;
            if (m_TerminalSelectForm.ShowDialog(AppGlobal.s_ClientMainForm) == DialogResult.OK)
            {

                tSelectedTerminal = m_TerminalSelectForm.SelectedTerminal;
                if (tSelectedTerminal.Parent is TerminalWindows)
                {
                    ((Form)tSelectedTerminal.Parent).BringToFront();
                }
                else
                {
                    tabTerminal.SelectedTab = ((SuperTabControlPanel)tSelectedTerminal.Parent).TabItem;
                }
                m_TerminalSelectForm.SelectedTerminal.RunScript(aScript);
            }
        }

        /// <summary>
        /// 종료 처리 합니다.
        /// </summary>
        internal void Stop(E_TerminalSessionCloseType aCloseType)
        {
            MCTerminalEmulator tTelnet;
            if (aCloseType == E_TerminalSessionCloseType.All)
            {
                for (int i = m_EmulatorList.Count - 1; i > -1; i--)
                {
                    tTelnet = m_EmulatorList[i];
                    if (m_TerminalConnectThreadPool != null)
                    {
                        m_TerminalConnectThreadPool.ExecuteWork(new MKWorkItem(new WorkItemDefaultMethod(tTelnet.Disconnect)));
                    }
                }

                Thread.Sleep(500);

            }
            else
            {
                for (int i = m_EmulatorList.Count - 1; i > -1; i--)
                {
                    tTelnet = m_EmulatorList[i];
                    if (tTelnet.ConnectionType != ConnectionTypes.RemoteTelnet)
                    {
                        continue;
                    }
                    tTelnet.Disconnect();
                }
            }

            if (AppGlobal.s_IsProgramShutdown)
            {

                if (m_TerminalConnectThreadPool != null)
                {
                    m_TerminalConnectThreadPool.StopThreadPool();
                    m_TerminalConnectThreadPool.Dispose();
                    m_TerminalConnectThreadPool = null;
                }
            }
        }
        /// <summary>
        /// 클라이언트 모드 변경을 적용 합니다.
        /// </summary>
        public void ChangeClientMode()
        {
            // MCTerminalEmulator tTelnet;
            //for (int i = 0; i < tabTerminal.Tabs.Count; i++)
            // {
            //     tTelnet = (MCTerminalEmulator)((SuperTabControlPanel)((SuperTabItem)tabTerminal.Tabs[i]).AttachedControl).Controls[0];//.ChangeClientMode();
            //     m_TerminalConnectThreadPool.ExecuteWork(new MKWorkItem(new WorkItemDefaultMethod(tTelnet.ChangeClientMode)));

            // }
            for (int i = 0; i < m_EmulatorList.Count; i++)
            {
                m_TerminalConnectThreadPool.ExecuteWork(new MKWorkItem(new WorkItemDefaultMethod(m_EmulatorList[i].ChangeClientMode)));
            }
        }
        /// <summary>
        /// 메인화면의 편집 처리 합니다.
        /// </summary>
        /// <param name="aEditType"></param>
        internal void ExecTerminalScreen(E_TerminalScreenTextEditType aEditType)
        {
            if (tabTerminal.SelectedTabIndex < 0) 
                return;
            ((SuperTabControlPanel)tabTerminal.SelectedTab.AttachedControl).Controls[0].Focus();
            ((MCTerminalEmulator)((SuperTabControlPanel)tabTerminal.SelectedTab.AttachedControl).Controls[0]).ExecTerminalScreen(aEditType);
        }

        internal void ExecTerminalScreenTest(MCTerminalEmulator m,E_TerminalScreenTextEditType aEditType)
        {

            m.ExecTerminalScreen(aEditType);
        }

        private void mnuCloseOther_Click(object sender, EventArgs e)
        {
            if (m_SelectedEmulator == null) return;

            for (int i = tabTerminal.Tabs.Count - 1; i > -1; i--)
            {
                if (((MCTerminalEmulator)((SuperTabControlPanel)((SuperTabItem)tabTerminal.Tabs[i]).AttachedControl).Controls[0]).ConnectedSessionID != m_SelectedEmulator.ConnectedSessionID)
                {
                    m_TerminalConnectThreadPool.ExecuteWork(new MKWorkItem(new WorkItemDefaultMethod(((SuperTabItem)tabTerminal.Tabs[i]).Close)));
                }
            }
            this.Invalidate();
        }

        /// <summary>
        /// 탭 분리 입니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuChangeStatus_Click(object sender, EventArgs e)
        {
            if (m_SelectedEmulator == null) return;

            TerminalWindows tForm = new TerminalWindows();

            m_SelectedEmulator.TerminalMode = E_TerminalMode.QuickClient;

            m_SelectedEmulator.Dock = DockStyle.Fill;

            tForm.Controls.Add(m_SelectedEmulator);
            tForm.Size = new Size(AppGlobal.s_ClientOption.PopupSizeWidth, AppGlobal.s_ClientOption.PopupSizeHeight);

            if (m_SelectedEmulator.DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET)
            {
                switch (AppGlobal.s_ClientOption.TerminalDisplayNameType)
                {
                    case E_TerminalDisplayNameType.IPAddress:
                        m_SelectedEmulator.Name = m_SelectedEmulator.DeviceInfo.IPAddress;
                        break;
                    case E_TerminalDisplayNameType.TID:
                        m_SelectedEmulator.Name = m_SelectedEmulator.DeviceInfo.Name;
                        break;
                }

            }
            else if (m_SelectedEmulator.DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
            {
                // 2013-03-06 - shinyn - SSH텔넷인경우 분기처리 추가
                switch (AppGlobal.s_ClientOption.TerminalDisplayNameType)
                {
                    case E_TerminalDisplayNameType.IPAddress:
                        m_SelectedEmulator.Name = m_SelectedEmulator.DeviceInfo.IPAddress;
                        break;
                    case E_TerminalDisplayNameType.TID:
                        m_SelectedEmulator.Name = m_SelectedEmulator.DeviceInfo.Name;
                        break;
                }
            }
            else
            {
                m_SelectedEmulator.Name = "Serial-" + m_SelectedEmulator.DeviceInfo.TerminalConnectInfo.SerialConfig.PortName;
            }
            tForm.Text = m_SelectedEmulator.Name;
            tabTerminal.SelectedTab.Close();
            tForm.MaximizeBox = false;
            tForm.BringToFront();
            tForm.Show();
            


        }

        private void mnuSaveTerminalLog_Click(object sender, EventArgs e)
        {
            if (m_SelectedEmulator == null) return;
            m_SelectedEmulator.WriteTerminalLog();
        }


        /// <summary>
        /// 2013-01-11 - shinyn - 복원명령 실행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuRestoreCfgCmd_Click(object sender, EventArgs e)
        {
            try
            {
                if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online)
                {

                    if (AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "Config복원명령은 로그인후 root 경로에서 실행됩니다. \r\nConfig복원명령을 실행하시겠습니까?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    {
                        return;
                    }

                    //서버에 사용할 수 있는 장비인지 확인합니다.
                    RequestCommunicationData tRequestData = null;
                    CfgRestoreCommandRequestInfo tRequest = new CfgRestoreCommandRequestInfo();

                    DeviceInfo tDeviceInfo = (DeviceInfo)tabTerminal.SelectedTab.Tag;

                    tRequest.CommandPart = E_CommandPart.ConfigBRRestore;
                    tRequest.IPAddress = tDeviceInfo.IPAddress;
                    tRequest.ModelID = tDeviceInfo.ModelID;


                    tRequestData = AppGlobal.MakeDefaultRequestData();
                    tRequestData.CommType = E_CommunicationType.RequestCfgRestoreCommand;

                    tRequestData.RequestData = tRequest;

                    m_Result = null;
                    m_MRE.Reset();

                    AppGlobal.SendRequestData(this, tRequestData);
                    m_MRE.WaitOne(AppGlobal.s_RequestTimeOut);


                    //if (m_Result == null)
                    //{
                    //    //타임 아웃 처리 콘솔 모드로 변경 해야 하나?
                    //    AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "타임 아웃 발생했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    return;
                    //}
                    //else
                    //{
                    //    if (m_Result.Error.Error != E_ErrorType.NoError)
                    //    {
                    //        AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "오류 발생:" + m_Result.Error.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //        return;
                    //    }

                    //    CfgSaveInfoCollection tCfgSaveInfos = null;

                    //    tCfgSaveInfos = (CfgSaveInfoCollection)m_Result.ResultData;

                    //    if (tCfgSaveInfos == null)
                    //    {
                    //        AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "복원할 CFG 바이너리 파일이 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //        return;
                    //    }
                    //    else
                    //    {

                    //        if (tCfgSaveInfos.Count == 0)
                    //        {
                    //            AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "최근 Config 복원 파일이 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //            return;
                    //        }



                    //        CfgSaveInfo tCfgSaveInfo = (CfgSaveInfo)tCfgSaveInfos.InnerList[0];

                    //        if (tCfgSaveInfo.FileName == "")
                    //        {
                    //            AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "최근 Config 복원 파일이 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //            return;
                    //        }
                    //        else
                    //        {
                    //            OpenRestoreCommand tOpenRestoreCommand = new OpenRestoreCommand();

                    //            tOpenRestoreCommand.OpenOnlineRestoreList(tCfgSaveInfos);

                    //            if (tOpenRestoreCommand.ShowDialog() != DialogResult.OK) return;

                    //            // 선택결과에 대해서 CFG 복원명령 예약어를 매핑 처리
                    //            CfgSaveInfoCollection tSelectCfgSaveInfos = tOpenRestoreCommand.CfgSaveInfos;

                    //            string tConfigFileName = string.Empty;

                    //            SetTelnetReservedString(tSelectCfgSaveInfos);

                    //            // 스크립트 명령 실행
                    //            CfgSaveInfo tSelectCfgSaveInfo = (CfgSaveInfo)tSelectCfgSaveInfos.InnerList[0];
                    //            Script tScript = new Script(AppGlobal.GetScript(tSelectCfgSaveInfo.CfgRestoreCommands));

                    //            AppGlobal.s_ClientOption.ShortenCommandTaget = E_ShortenCommandTagret.ActiveTerminal;
                    //            m_SelectedEmulator.RunScript(tScript);
                    //        }
                    //    }
                    //}
                }
                else if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Console)
                {

                    if (AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "Config복원명령은 로그인후 root 경로에서 실행됩니다. \r\nConfig복원명령을 실행하시겠습니까?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    {
                        return;
                    }

                    DeviceInfo tDeviceInfo = (DeviceInfo)tabTerminal.SelectedTab.Tag;

                    if (tDeviceInfo.CfgSaveInfos.Count == 0)
                    {
                        AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "장비 Config 복원 명령이 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    CfgSaveInfo tCfgSaveInfo = (CfgSaveInfo)tDeviceInfo.CfgSaveInfos.InnerList[0];

                    if (tCfgSaveInfo.FullFileName == "")
                    {
                        AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "장비 Config 복원 명령이 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    OpenRestoreCommand tOpenRestoreCommand = new OpenRestoreCommand();

                    tOpenRestoreCommand.OpenConsoleRestoreList(tDeviceInfo.CfgSaveInfos);

                    if (tOpenRestoreCommand.ShowDialog() != DialogResult.OK) return;

                    // 선택결과에 대해서 CFG 복원명령 예약어를 매핑 처리
                    CfgSaveInfoCollection tSelectCfgSaveInfos = tOpenRestoreCommand.CfgSaveInfos;

                    // 스크립트 명령실행
                    CfgSaveInfo tSelectCfgSaveInfo = (CfgSaveInfo)tSelectCfgSaveInfos.InnerList[0];
                    Script tScript = new Script(tSelectCfgSaveInfo.CfgRestoreScript);

                    AppGlobal.s_ClientOption.ShortenCommandTaget = E_ShortenCommandTagret.ActiveTerminal;
                    m_SelectedEmulator.RunScript(tScript);

                }


            }
            catch (Exception ex)
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, ex.Message.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 2013-01-11 - shinyn - 텔넷 예약어 매칭처리
        /// </summary>
        /// <param name="tSelectCfgSaveInfos"></param>
        private void SetTelnetReservedString(CfgSaveInfoCollection tSelectCfgSaveInfos)
        {
            string tConfigFileName = string.Empty;

            foreach (CfgSaveInfo tCfgSaveInfo in tSelectCfgSaveInfos)
            {
                foreach (CfgRestoreCommand tCfgRestoreCommand in tCfgSaveInfo.CfgRestoreCommands)
                {
                    if (tCfgRestoreCommand.Cmd.IndexOf(TelnetReservedString.c_FTPIP) > -1)
                    {
                        tCfgRestoreCommand.Cmd = tCfgRestoreCommand.Cmd.Replace(TelnetReservedString.c_FTPIP, tCfgSaveInfo.FTPServerIP);
                    }
                    else if (tCfgRestoreCommand.Cmd.IndexOf(TelnetReservedString.c_FTPUSER) > -1)
                    {
                        tCfgRestoreCommand.Cmd = tCfgRestoreCommand.Cmd.Replace(TelnetReservedString.c_FTPUSER, tCfgSaveInfo.CenterFTPID);
                    }
                    else if (tCfgRestoreCommand.Cmd.IndexOf(TelnetReservedString.c_FTPPASSEORD) > -1)
                    {
                        tCfgRestoreCommand.Cmd = tCfgRestoreCommand.Cmd.Replace(TelnetReservedString.c_FTPPASSEORD, tCfgSaveInfo.CenterFTPPW);
                    }
                    else if (tCfgRestoreCommand.Cmd.IndexOf(TelnetReservedString.c_CONFIGFILENAME) > -1)
                    {
                        tConfigFileName = tCfgSaveInfo.FileName;
                        tCfgRestoreCommand.Cmd = tCfgRestoreCommand.Cmd.Replace(TelnetReservedString.c_CONFIGFILENAME, tConfigFileName);
                    }
                    else if (tCfgRestoreCommand.Cmd.IndexOf(TelnetReservedString.c_CONFIGFILENAMEEXT) > -1)
                    {
                        tConfigFileName = tCfgSaveInfo.FileName;
                        if (tCfgSaveInfo.FileExtend != "")
                        {
                            tConfigFileName += "." + tCfgSaveInfo.FileExtend;
                        }
                        tCfgRestoreCommand.Cmd = tCfgRestoreCommand.Cmd.Replace(TelnetReservedString.c_CONFIGFILENAMEEXT, tConfigFileName);
                    }
                    else if (tCfgRestoreCommand.Cmd.IndexOf(TelnetReservedString.c_CONFIGFILENAME16) > -1)
                    {
                        tConfigFileName = tCfgSaveInfo.FileName;
                        tCfgRestoreCommand.Cmd = tCfgRestoreCommand.Cmd.Replace(TelnetReservedString.c_CONFIGFILENAME16, tConfigFileName);
                    }
                    else if (tCfgRestoreCommand.Cmd.IndexOf(TelnetReservedString.c_CONFIGFILENAMEEXT16) > -1)
                    {
                        tConfigFileName = tCfgSaveInfo.FileName;
                        if (tCfgSaveInfo.FileExtend != "")
                        {
                            tConfigFileName += "." + tCfgSaveInfo.FileExtend;
                        }
                        tCfgRestoreCommand.Cmd = tCfgRestoreCommand.Cmd.Replace(TelnetReservedString.c_CONFIGFILENAMEEXT16, tConfigFileName);
                    }
                    else if (tCfgRestoreCommand.Cmd.IndexOf(TelnetReservedString.c_CONFIGFULLFILENAME) > -1)
                    {
                        tConfigFileName = tCfgSaveInfo.FileName;
                        if (tCfgSaveInfo.FileExtend != "")
                        {
                            tConfigFileName += "." + tCfgSaveInfo.FileExtend;
                        }
                        tCfgRestoreCommand.Cmd = tCfgRestoreCommand.Cmd.Replace(TelnetReservedString.c_CONFIGFULLFILENAME, tConfigFileName);
                    }
                }
            }
        }

        /// <summary>
        /// 2013-05-02 - shinyn - 링크장비연결 접속
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuLinkConnect_Click(object sender, EventArgs e)
        {
            SearchLinkDevice tSearch = new SearchLinkDevice();

            if (tSearch.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            DeviceInfoCollection tDeviceInfos = tSearch.SelectedDeviceList;
            DeviceInfo tDeviceInfo = null;

            foreach (DeviceInfo tItem in tDeviceInfos)
            {
                tDeviceInfo = tItem;
            }

            m_LinkDeviceInfo = tDeviceInfo;

            StartSendThread(new ThreadStart(RequestConnectionCommand));



        }

        private void RequestConnectionCommand()
        {
            // 사용자등록장비인 경우 기본접속 명령을 DeviceInfo에 같이 가지고 있음.
            RequestCommunicationData tRequestData = null;

            tRequestData = AppGlobal.MakeDefaultRequestData();
            tRequestData.CommType = E_CommunicationType.RequestDefaultConnectionCommand;
            //2013-05-02- shinyn - 수동장비인 경우 기본접속 정보는 DeviceInfo에 있으므로 DeviceInfo를 보내고 기본접속 정보를 로드한다.
            tRequestData.RequestData = m_LinkDeviceInfo;
            m_Result = null;
            m_MRE.Reset();

            AppGlobal.SendRequestData(this, tRequestData);
            m_MRE.WaitOne(AppGlobal.s_RequestTimeOut);
        }

        /// <summary>
        /// 2013-05-02 - shinyn - 기본접속 명령을 요청후 스크립트를 실행한다.
        /// </summary>
        /// <param name="vResult"></param>
        public override void ResultReceiver(ResultCommunicationData vResult)
        {

            // 2013-05-02- shinyn - 결과값 올때까지 계속 실행한다.
            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument1<ResultCommunicationData>(ResultReceiver), vResult);
                return;
            }

            base.ResultReceiver(vResult);

            if (m_Result == null || m_Result.Error.Error != E_ErrorType.NoError)
            {
                // 2013-05-02 - shinyn - 기본접속 명령 로드 실패시 장비 아이피 로그에 저장
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "TerminalPanel : ResultReceiver : 기본 접속 명령 정보 로드에 실패 했습니다. IP : " + m_LinkDeviceInfo.IPAddress);
                if (vResult.CommType == E_CommunicationType.RequestDefaultConnectionCommand)
                {
                    MessageBox.Show("접속정보를 가져오지 못했습니다.");
                }
                return;
            }

            object tResult = m_Result.ResultData;

            if (tResult.GetType().Equals(typeof(FACT_DefaultConnectionCommandSet)))
            {
                FACT_DefaultConnectionCommandSet tCommandSet = m_Result.ResultData as FACT_DefaultConnectionCommandSet;

                Script tScript = new Script(AppGlobal.GetTelnetScript(m_LinkDeviceInfo, tCommandSet));
                m_SelectedEmulator.RunScript(tScript);
            }
            else if (tResult.GetType().Equals(typeof(CompressData)))
            {
                object tObject = AppGlobal.DecompressObject((CompressData)tResult);
                DeviceInfo tDeviceInfo = null;

                // 장비연결인 경우 연결 실행하도록 수정
                if (tObject.GetType().Equals(typeof(DeviceInfoCollection)))
                {
                    if (m_ConnectInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET)
                    {
                        if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online)
                        {

                            DeviceInfoCollection tDeviceInfos = (DeviceInfoCollection)tObject;

                            if (!tDeviceInfos.Contains(m_ConnectInfo.IPAddress))
                            {
                                tDeviceInfo = new DeviceInfo();
                                tDeviceInfo.IsRegistered = false;
                                tDeviceInfo.IPAddress = m_ConnectInfo.IPAddress;
                                tDeviceInfo.TerminalConnectInfo = m_ConnectInfo;
                            }
                            else
                            {

                                tDeviceInfo = tDeviceInfos[m_ConnectInfo.IPAddress];

                                if (AppGlobal.s_LoginResult.UserInfo.Centers.Count > 0)
                                {
                                    if (!AppGlobal.s_LoginResult.UserInfo.Centers.Contains(tDeviceInfo.CenterCode))
                                    {
                                        AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "해당 장비에 접속 권한이 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return;
                                    }
                                }
                                tDeviceInfo.TerminalConnectInfo = m_ConnectInfo;
                            }


                        }

                        if (tDeviceInfo == null)
                        {
                            tDeviceInfo = new DeviceInfo();
                            tDeviceInfo.IsRegistered = false;
                            tDeviceInfo.IPAddress = m_ConnectInfo.IPAddress;
                            tDeviceInfo.TerminalConnectInfo = m_ConnectInfo;
                        }

                    }
                    else if (m_ConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
                    {
                        if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online)
                        {

                            DeviceInfoCollection tDeviceInfos = (DeviceInfoCollection)tObject;

                            if (!tDeviceInfos.Contains(m_ConnectInfo.IPAddress))
                            {
                                tDeviceInfo = new DeviceInfo();
                                tDeviceInfo.IsRegistered = false;
                                tDeviceInfo.IPAddress = m_ConnectInfo.IPAddress;
                                tDeviceInfo.TerminalConnectInfo = m_ConnectInfo;

                                tDeviceInfo.TelnetID1 = m_ConnectInfo.ID;
                                tDeviceInfo.TelnetPwd1 = m_ConnectInfo.Password;
                            }
                            else
                            {
                                tDeviceInfo = tDeviceInfos[m_ConnectInfo.IPAddress];

                                if (AppGlobal.s_LoginResult.UserInfo.Centers.Count > 0)
                                {
                                    if (!AppGlobal.s_LoginResult.UserInfo.Centers.Contains(tDeviceInfo.CenterCode))
                                    {
                                        AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "해당 장비에 접속 권한이 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return;
                                    }
                                }
                                tDeviceInfo.TerminalConnectInfo = m_ConnectInfo;
                            }

                        }

                        if (tDeviceInfo == null)
                        {
                            tDeviceInfo = new DeviceInfo();
                            tDeviceInfo.IsRegistered = false;
                            tDeviceInfo.IPAddress = m_ConnectInfo.IPAddress;
                            tDeviceInfo.TerminalConnectInfo = m_ConnectInfo;
                        }
                    }
                    else
                    {
                        tDeviceInfo = new DeviceInfo();
                        tDeviceInfo.IsRegistered = false;
                        tDeviceInfo.TerminalConnectInfo = m_ConnectInfo;
                    }

                    AddTerminal(tDeviceInfo, true);
                }
            }
            else if (tResult.GetType().Equals(typeof(CfgSaveInfoCollection)))
            {
                CfgSaveInfoCollection tCfgSaveInfos = null;

                tCfgSaveInfos = (CfgSaveInfoCollection)m_Result.ResultData;

                if (tCfgSaveInfos == null)
                {
                    AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "복원할 CFG 바이너리 파일이 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {

                    if (tCfgSaveInfos.Count == 0)
                    {
                        AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "최근 Config 복원 파일이 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }



                    CfgSaveInfo tCfgSaveInfo = (CfgSaveInfo)tCfgSaveInfos.InnerList[0];

                    if (tCfgSaveInfo.FileName == "")
                    {
                        AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "최근 Config 복원 파일이 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        OpenRestoreCommand tOpenRestoreCommand = new OpenRestoreCommand();

                        tOpenRestoreCommand.OpenOnlineRestoreList(tCfgSaveInfos);

                        if (tOpenRestoreCommand.ShowDialog() != DialogResult.OK) return;

                        // 선택결과에 대해서 CFG 복원명령 예약어를 매핑 처리
                        CfgSaveInfoCollection tSelectCfgSaveInfos = tOpenRestoreCommand.CfgSaveInfos;

                        string tConfigFileName = string.Empty;

                        SetTelnetReservedString(tSelectCfgSaveInfos);

                        // 스크립트 명령 실행
                        CfgSaveInfo tSelectCfgSaveInfo = (CfgSaveInfo)tSelectCfgSaveInfos.InnerList[0];
                        Script tScript = new Script(AppGlobal.GetScript(tSelectCfgSaveInfo.CfgRestoreCommands));

                        AppGlobal.s_ClientOption.ShortenCommandTaget = E_ShortenCommandTagret.ActiveTerminal;
                        m_SelectedEmulator.RunScript(tScript);
                    }
                }
            }
        }

        private void tabTerminal_Enter(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Enter");
            //AppGlobal.bPanelFocusCheck = true;
        }

        private void tabTerminal_Leave(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Leave");
            //AppGlobal.bPanelFocusCheck = false;
        }

    }
}
