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
using System.Threading.Tasks;
using MKLibrary.MKProcess;
using RACTClient.Helpers;
using RACTClient.Connectivity;
using RACTClient.TerminalControl;
using RACTClient.TerminalManagement;

namespace RACTClient
{

    /// <summary>
    /// 터미널 변경에 사용할 핸들러 입니다.
    /// </summary>
    /// <param name="aTerminalName"></param>
    public delegate void TerminalTabChangeHandler(E_TerminalStatus status, string terminalName);
    /// <summary>
    /// 터미널 Tab 패널 입니다.
    /// </summary>
    public partial class TerminalPanel : SenderControl, IMainPanel
    {
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
        /// 2013-05-02- shinyn - 링크장비정보입니다.
        /// </summary>
        private DeviceInfo m_LinkDeviceInfo = null;

        /// <summary>
        /// 2013-05-16- shinyn - 빠른 연결실행할 연결 형태를 비교한다.
        /// </summary>
        private TerminalConnectInfo m_ConnectInfo = null;

        private readonly ITactTerminalFactory m_TerminalFactory = new TactTerminalFactory();
        private readonly TerminalManager m_TerminalManager = new TerminalManager();

        private TaskCompletionSource<ResultCommunicationData> m_ResponseTcs;

        /// <summary>
        /// 2013-01-11 - shinyn - 현재 열려있는 에뮬레이터 수를 가져와서 그이상 에뮬레이터 열수 없도록 수정
        /// </summary>
        public List<ITactTerminal> EmulatorList
        {
            get { return m_TerminalManager.EmulatorList; }
        }

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public TerminalPanel()
        {
            InitializeComponent();


            Image image = global::RACTClient.Properties.Resources.led_green;
            imlTab.Images.Add(image);
            image = global::RACTClient.Properties.Resources.led_red;
            imlTab.Images.Add(image);
            image = global::RACTClient.Properties.Resources.led_gray;
            imlTab.Images.Add(image);
            image = global::RACTClient.Properties.Resources.led_blue;
            imlTab.Images.Add(image);

            ucShortenCommand.OnSendShortenCommand += new HandlerArgument1<ShortenCommandInfo>(ShortenCommand_OnSendShortenCommand);
            ucShortenScript.OnSendScript += new HandlerArgument1<Script>(ShortenScript_OnSendScript);

            SshSessionPool.Instance.OnSessionDisconnected += Global_OnSharedSessionDisconnected;

            m_TerminalManager.TerminalTabChangeEvent += (status, name) => OnTerminalTabChangeEvent?.Invoke(status, name);
        }
        /// <summary>
        /// 스크립트 명령 이벤트를 처리 합니다. 
        /// </summary>
        /// <param name="aValue1"></param>
        void ShortenScript_OnSendScript(Script script)
        {
            this.BringToFront();

            if (AppGlobal.s_ClientOption.ShortenCommandTaget == E_ShortenCommandTagret.ActiveTerminal)
            {
                ScriptWork(E_ScriptWorkType.Run, script);
            }
            else
            {
                foreach (ITactTerminal emulator in m_TerminalManager.EmulatorList)
                {
                    if (emulator.TerminalStatus != E_TerminalStatus.Disconnected)
                    {
                        emulator.RunScript(script);
                    }
                }
            }
        }
        /// <summary>
        /// 단축 명령 이벤트를 처리 합니다.
        /// </summary>
        /// <param name="aValue1"></param>
        void ShortenCommand_OnSendShortenCommand(ShortenCommandInfo commandInfo)
        {
            //if (tabTerminal.SelectedTabIndex < 0) return;
            if (AppGlobal.s_ClientOption.ShortenCommandTaget == E_ShortenCommandTagret.ActiveTerminal)
            {
                m_TerminalSelectForm.TerminalList = m_TerminalManager.EmulatorList; 
                ITactTerminal selectedTerminal = null;
                if (m_TerminalSelectForm.ShowDialog(AppGlobal.s_ClientMainForm) == DialogResult.OK)
                {
                    selectedTerminal = m_TerminalSelectForm.SelectedTerminal;
                    if (selectedTerminal.Parent is TerminalWindows)
                    {
                        ((Form)selectedTerminal.Parent).BringToFront();
                    }
                    else
                    {
                        tabTerminal.SelectedTab = ((SuperTabControlPanel)selectedTerminal.Parent).TabItem;
                    }
                    selectedTerminal.IsLimitCmdForShortenCommand(this, commandInfo.Command + "\r\n");
                }
            }
            else
            {
                foreach (ITactTerminal emulator in m_TerminalManager.EmulatorList)
                {
                    emulator.IsLimitCmdForShortenCommand(this, commandInfo.Command + "\r\n");
                }
            }
        }

        /// <summary>
        /// 터미널을 추가 할 수 있는지 확인 합니다.
        /// </summary>
        /// <param name="tCount"></param>
        /// <returns></returns>
        private bool CheckTerminalCount(DeviceInfo deviceInfo, out int totalCount)
        {
            var tabDevices = tabTerminal.Tabs.Cast<SuperTabItem>()
                                .Select(t => t.Tag as DeviceInfo)
                                .Where(d => d != null);

            if (!m_TerminalManager.CanAddTerminal(deviceInfo, out totalCount, tabDevices))
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "최대 연결 개수는 20개 입니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            totalCount = CalculateTotalCountForDevice(deviceInfo, tabDevices);
            return true;
        }

        private int CalculateTotalCountForDevice(DeviceInfo deviceInfo, IEnumerable<DeviceInfo> tabDevices)
        {
            int maxNumber = 0;
            int currentCount = 0;

            foreach (var tabDeviceInfo in tabDevices)
            {
                if (IsSameDeviceConnection(deviceInfo, tabDeviceInfo))
                {
                    currentCount = 1;
                    int tabIndex = ParseTabIndexFromTabName(tabDeviceInfo);
                    if (tabIndex > maxNumber) maxNumber = tabIndex;
                }
            }

            return (maxNumber > 0 || currentCount > 0) ? Math.Max(currentCount, maxNumber + 1) : 0;
        }

        private bool IsSameDeviceConnection(DeviceInfo deviceInfo, DeviceInfo tabDeviceInfo)
        {
            if (deviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SERIAL_PORT)
            {
                return tabDeviceInfo.TerminalConnectInfo.SerialConfig.PortName.Equals(deviceInfo.TerminalConnectInfo.SerialConfig.PortName);
            }
            return tabDeviceInfo.IPAddress.Equals(deviceInfo.IPAddress);
        }

        private int ParseTabIndexFromTabName(DeviceInfo deviceInfo)
        {
            var tab = tabTerminal.Tabs.Cast<SuperTabItem>().FirstOrDefault(t => t.Tag == deviceInfo);
            if (tab == null) return 0;

            string tabName = tab.Text;
            if (tabName.Contains("(") && tabName.Contains(")"))
            {
                try
                {
                    int start = tabName.IndexOf("(") + 1;
                    int length = tabName.IndexOf(")") - start;
                    return int.Parse(tabName.Substring(start, length));
                }
                catch { }
            }
            return 0;
        }
        /// <summary>
        /// 터미널을 생성 합니다.
        /// </summary>
        private ITactTerminal MakeEmulator(DeviceInfo deviceInfo, bool isQuickConnection)
        {
            ITactTerminal emulator = m_TerminalFactory.CreateTerminal(deviceInfo, isQuickConnection);
            
            emulator.OnTerminalStatusChange += new HandlerArgument2<object, E_TerminalStatus>(tEmulator_OnTerminalStatusChange);
            emulator.OnTelnetFindString += new DefaultHandler(tEmulator_OnTelnetFindString);
            emulator.CallOptionHandlerEvent += new DefaultHandler(tEmulator_CallOptionHandlerEvent);
            emulator.ProgreBarHandlerEvent += new HandlerArgument3<string, eProgressItemType, bool>(tEmulator_ProgreBarHandlerEvent);

            m_TerminalManager.AddEmulator(emulator);

            return emulator;
        }
        /// <summary>
        /// 패널을 생성 합니다.
        /// </summary>
        private SuperTabItem MakeTabPanel(ITactTerminal emulator, int count)
        {
            SuperTabControlPanel tabControlPanel;
            SuperTabItem tabItem;
            tabItem = new DevComponents.DotNetBar.SuperTabItem();
            tabControlPanel = new DevComponents.DotNetBar.SuperTabControlPanel();
            tabItem.MouseUp += new MouseEventHandler(TabItem_MouseUp);
            tabTerminal.Controls.Add(tabControlPanel);
            tabControlPanel.Controls.Add((Control)emulator);
            tabItem.AttachedControl = tabControlPanel;
            tabItem.GlobalItem = false;

            if (emulator.DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET)
            {
                if (AppGlobal.s_ClientOption.TerminalDisplayNameType == E_TerminalDisplayNameType.IPAddress)
                {
                    tabItem.Name = count > 0 ? emulator.DeviceInfo.IPAddress + "(" + count.ToString() + ")" : emulator.DeviceInfo.IPAddress;
                }
                else
                {
                    tabItem.Name = count > 0 ? emulator.DeviceInfo.Name + "(" + count.ToString() + ")" : emulator.DeviceInfo.Name;
                }
            }
            else if (emulator.DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
            {
                if (AppGlobal.s_ClientOption.TerminalDisplayNameType == E_TerminalDisplayNameType.IPAddress)
                {
                    tabItem.Name = count > 0 ? emulator.DeviceInfo.IPAddress + "(" + count.ToString() + ")" : emulator.DeviceInfo.IPAddress;
                }
                else
                {
                    tabItem.Name = count > 0 ? emulator.DeviceInfo.Name + "(" + count.ToString() + ")" : emulator.DeviceInfo.Name;
                }
            }
            else
            {
                tabItem.Name = count > 0 ? "Serial-" + emulator.DeviceInfo.TerminalConnectInfo.SerialConfig.PortName + "(" + count + ")" : "Serial-" + emulator.DeviceInfo.TerminalConnectInfo.SerialConfig.PortName;
            }
            tabItem.Text = tabItem.Name;
            emulator.DeviceInfo.TerminalName = tabItem.Name;
            tabItem.Image = (Image)global::RACTClient.Properties.Resources.TryConnect;
            ((Control)emulator).Name = tabItem.Name;

            tabItem.Tag = emulator.DeviceInfo;
            tabItem.Tooltip = emulator.ToolTip;
            tabControlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControlPanel.Location = new System.Drawing.Point(0, 26);
            tabControlPanel.Name = "superTabControlPanel1";
            tabControlPanel.Size = new System.Drawing.Size(150, 124);
            tabControlPanel.TabIndex = 1;
            tabControlPanel.TabItem = tabItem;

            ((Control)emulator).Name = tabItem.Text;

            return tabItem;
        }

        internal void AddTerminal(ITactTerminal emulator)
        {
            int totalCount = 0;

            if (!CheckTerminalCount(emulator.DeviceInfo, out totalCount)) return;

            SuperTabItem tabItem = MakeTabPanel(emulator, totalCount);
            tabTerminal.Tabs.AddRange(new DevComponents.DotNetBar.BaseItem[] { tabItem });
            tabTerminal.ReorderTabsEnabled = true;
            emulator.TerminalStatus = emulator.TerminalStatus;
            tabTerminal.SelectedTab = tabItem;

        }

        /// <summary>
        /// 터미널을 추가 합니다.
        /// </summary>
        /// <param name="deviceInfo">장비 정보 입니다.</param>
        /// <param name="isQuickConnection">빠른 연결 여부</param>
        public void AddTerminal(DeviceInfo deviceInfo, bool isQuickConnection, DaemonProcessInfo daemonProcessInfo)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument3<DeviceInfo, bool, DaemonProcessInfo>(AddTerminal), new object[] { deviceInfo, isQuickConnection, daemonProcessInfo });
                return;
            }

            if (daemonProcessInfo == null)
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "사용 가능한 Daemon 정보 로드에 실패 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "사용 가능한 Daemon 정보 로드에 실패 했습니다.");
                return;
            }
            int totalCount = 0;
            if (!CheckTerminalCount(deviceInfo, out totalCount)) return;

            ITactTerminal emulator = MakeEmulator(deviceInfo, isQuickConnection);
            // Daemon 관련 추가 로직이 필요하다면 여기에 작성

            if (AppGlobal.s_ClientOption.TerminalWindowsPopupType == E_DefaultTerminalPopupType.Tab)
            {
                SuperTabItem tabItem = MakeTabPanel(emulator, totalCount);
                tabTerminal.Tabs.AddRange(new DevComponents.DotNetBar.BaseItem[] { tabItem });
                tabTerminal.ReorderTabsEnabled = true;
                tabTerminal.SelectedTab = tabItem;
            }

            if (OnTerminalTabChangeEvent != null) OnTerminalTabChangeEvent(E_TerminalStatus.Add, emulator.Name);
        }


        /// <summary>
        /// 터미널을 추가 합니다.
        /// </summary>
        /// <param name="aDeviceInfo">장비 정보 입니다.</param>
        /// <param name="aIsQuickConnection">빠른 연결 여부</param>
        public void AddTerminal(DeviceInfo deviceInfo, bool isQuickConnection)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument2<DeviceInfo, bool>(AddTerminal), new object[] { deviceInfo, isQuickConnection });
                return;
            }

            int totalCount = 0;
            if (!CheckTerminalCount(deviceInfo, out totalCount)) return;

            ITactTerminal emulator = MakeEmulator(deviceInfo, isQuickConnection);
            
            DeviceInfo jumpHost = GetJumpHostForDevice(deviceInfo);
            emulator.JumpHost = jumpHost;

            if (AppGlobal.s_ClientOption.TerminalWindowsPopupType == E_DefaultTerminalPopupType.Tab)
            {
                SuperTabItem tabItem = MakeTabPanel(emulator, totalCount);
                tabTerminal.Tabs.AddRange(new DevComponents.DotNetBar.BaseItem[] { tabItem });
                tabTerminal.ReorderTabsEnabled = true;
                tabTerminal.SelectedTab = tabItem;
            }
            else
            {
                TerminalWindows window = new TerminalWindows();
                ((Control)emulator).Dock = DockStyle.Fill;
                window.AddTerminalControl(emulator);
                window.Size = new Size(AppGlobal.s_ClientOption.PopupSizeWidth, AppGlobal.s_ClientOption.PopupSizeHeight);

                if (emulator.DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET)
                {
                    emulator.Name = emulator.DeviceInfo.IPAddress;
                }
                else if (emulator.DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
                {
                    emulator.Name = emulator.DeviceInfo.IPAddress;
                }
                else
                {
                    emulator.Name = "Serial-" + emulator.DeviceInfo.TerminalConnectInfo.SerialConfig.PortName;
                }
                window.Text = emulator.Name;
                window.MaximizeBox = false;
                window.Show();
            }

            Task.Run(() =>
            {
                try
                {
                    AppGlobal.s_FileLogProcessor?.PrintLog(E_FileLogType.Infomation, "AddTerminal : Task Start : " + emulator.DeviceInfo.IPAddress);
                    emulator.ConnectDevice(emulator.DeviceInfo, emulator.JumpHost);
                }
                catch (Exception ex)
                {
                    AppGlobal.s_FileLogProcessor?.PrintLog(E_FileLogType.Error, "AddTerminal Error: " + ex.Message);
                }
            });

            if (OnTerminalTabChangeEvent != null) OnTerminalTabChangeEvent(E_TerminalStatus.Add, emulator.Name);
        }

        private DeviceInfo GetJumpHostForDevice(DeviceInfo device)
        {
            // TODO: DB  나  설정에서  장비의 Zone  또는  망  구분별  중계  서버  정보를  조회하는  로직  구현
            return null;
        }

        /// <summary>
        /// 2014-10-14 - 신윤남 - 창닫히는 경우 에뮬레이터 리스트에서 삭제
        /// </summary>
        /// <param name="aValue1"></param>
        /// <param name="aValue2"></param>
        public void tEmulator_OnTerminalStatusChange(object sender, E_TerminalStatus status)
        {
            // 이 로직은 이제 TerminalManager에서 이벤트로 처리하거나 
            // Manager가 상태 변경을 추적하고 있으므로 
            // Panel은 UI 업데이트 위주로 수행함.
            // (이미 생성자에서 m_TerminalManager.TerminalTabChangeEvent를 구독 중)
        }

        /// <summary>
        /// 빠른 연결 처리 합니다.
        /// </summary>
        /// <param name="quickConnectInfo"></param>
        internal async Task QuickConnect(TerminalConnectInfo connectInfo)
        {
            DeviceInfo deviceInfo = null;

            m_ConnectInfo = connectInfo;

            if (connectInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET)
            {
                if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online)
                {
                    RequestCommunicationData requestData = null;
                    DeviceSearchInfo searchInfo = new DeviceSearchInfo();

                    searchInfo.DeviceIPAddress = connectInfo.IPAddress;
                    searchInfo.UserID = AppGlobal.s_LoginResult.UserID;
                    requestData = AppGlobal.MakeDefaultRequestData();
                    searchInfo.IsCheckPermission = false;
                    requestData.CommType = E_CommunicationType.RequestFACTSearchDevice;

                    requestData.RequestData = searchInfo;

                    m_Result = null;
                    m_ResponseTcs = new TaskCompletionSource<ResultCommunicationData>();

                    AppGlobal.SendRequestData(this, requestData);
                    
                    var completedTask = await Task.WhenAny(m_ResponseTcs.Task, Task.Delay(AppGlobal.s_RequestTimeOut));
                    if (completedTask != m_ResponseTcs.Task)
                    {
                        AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "타임 아웃 발생했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    if (deviceInfo == null)
                        deviceInfo = new DeviceInfo();
                    deviceInfo.IsRegistered = false;
                    deviceInfo.IPAddress = m_ConnectInfo.IPAddress;
                    deviceInfo.TerminalConnectInfo = m_ConnectInfo;

                    AddTerminal(deviceInfo, true);
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
            else if (connectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
            {
                if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online)
                {
                    RequestCommunicationData requestData = null;
                    DeviceSearchInfo searchInfo = new DeviceSearchInfo();

                    searchInfo.DeviceIPAddress = connectInfo.IPAddress;
                    searchInfo.UserID = AppGlobal.s_LoginResult.UserID;
                    requestData = AppGlobal.MakeDefaultRequestData();
                    searchInfo.IsCheckPermission = false;
                    requestData.CommType = E_CommunicationType.RequestFACTSearchDevice;

                    requestData.RequestData = searchInfo;

                    m_Result = null;
                    m_ResponseTcs = new TaskCompletionSource<ResultCommunicationData>();

                    AppGlobal.SendRequestData(this, requestData);

                    var completedTask = await Task.WhenAny(m_ResponseTcs.Task, Task.Delay(AppGlobal.s_RequestTimeOut));
                    if (completedTask != m_ResponseTcs.Task)
                    {
                        AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "타임 아웃 발생하였습니다.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    if (deviceInfo == null)
                        deviceInfo = new DeviceInfo();
                    deviceInfo.IsRegistered = false;
                    deviceInfo.IPAddress = m_ConnectInfo.IPAddress;
                    deviceInfo.TerminalConnectInfo = m_ConnectInfo;

                    AddTerminal(deviceInfo, true);
                }
            }
            else if (connectInfo.ConnectionProtocol == E_ConnectionProtocol.SERIAL_PORT)
            {
                if (deviceInfo == null)
                    deviceInfo = new DeviceInfo();
                deviceInfo.IsRegistered = false;
                deviceInfo.IPAddress = m_ConnectInfo.IPAddress;
                deviceInfo.TerminalConnectInfo = m_ConnectInfo;

                AddTerminal(deviceInfo, true);
            }
            

            //AddTerminal(tDeviceInfo, true);
        }

        /// <summary>
        /// 2013-01-11 - shinyn - 목록불러오기로 장비연결시 사용됨.
        /// </summary>
        /// <param name="aDeviceInfo"></param>
        /// <param name="aConnectInfo"></param>
        internal void OpenDeviceConnect(DeviceInfo deviceInfo, TerminalConnectInfo connectInfo)
        {
            if (connectInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET)
            {
                deviceInfo.IsRegistered = false;
                deviceInfo.TerminalConnectInfo = connectInfo;
            }
            else if (connectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
            {
                deviceInfo.IsRegistered = false;
                deviceInfo.TerminalConnectInfo = connectInfo;
            }
            else
            {
                deviceInfo.IsRegistered = false;
                deviceInfo.TerminalConnectInfo = connectInfo;
            }
            AddTerminal(deviceInfo, true);
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
                SuperTabItem baseItem = (SuperTabItem)sender;
                SuperTabControlPanel panel = (SuperTabControlPanel)baseItem.AttachedControl;
                ITactTerminal emulator = panel.Controls[0] as ITactTerminal;
                if (emulator == null) return;

                m_SelectedEmulator = emulator;
                mnuModifyDevice.Enabled = emulator.DeviceInfo.IsRegistered;

                mnuRestoreCfgCmd.Enabled = emulator.IsConnected;
                mnuDisConnect.Enabled = emulator.IsConnected;
                mnuLinkConnect.Enabled = emulator.IsConnected;
                mnuReConnect.Enabled = !emulator.IsConnected;

                ShowContextMenu(cmTabPopup);
            }
        }
        /// <summary>
        /// 선택된 Emulator 입니다.
        /// </summary>
        private ITactTerminal m_SelectedEmulator = null;
        //public MCTerminalEmulator m_SelectedEmulator = null;
        /// <summary>
        /// 팝업 메뉴를 표시 합니다.
        /// </summary>
        /// <param name="cm"></param>
        private void ShowContextMenu(ButtonItem popup)
        {
            popup.Popup(MousePosition);
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

            ITactTerminal emulator = tabTerminal.SelectedTab.AttachedControl.Controls[0] as ITactTerminal;
            if (emulator != null) emulator.FindForm_Close();
        }
        /// <summary>
        /// 찾기 처리 합니다.
        /// </summary>
        void TelnetFindForm_OnTelnetStringFind(TelnetStringFindHandlerArgs args)
        {
            if (tabTerminal.Tabs.Count == 0) return;

            ITactTerminal emulator = tabTerminal.SelectedTab.AttachedControl.Controls[0] as ITactTerminal;
            if (emulator != null) emulator.FindForm_OnTelnetStringFind(args);
        }
        /// <summary>
        /// 닫기 처리된 컨트롤 입니다.
        /// </summary>
        private ITactTerminal m_CloseEmulator = null;
        private void tabTerminal_TabItemClose(object sender, SuperTabStripTabItemCloseEventArgs e)
        {
            if (((SuperTabItem)e.Tab).AttachedControl.Controls.Count == 0) return;

            ITactTerminal closeEmulator = ((SuperTabItem)e.Tab).AttachedControl.Controls[0] as ITactTerminal;

            if (closeEmulator != null && closeEmulator.IsConnected)
            {
                closeEmulator.Disconnect();
                closeEmulator.TerminalStatus = E_TerminalStatus.Disconnected;
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
            ITactTerminal emulator = ((SuperTabControlPanel)tabTerminal.SelectedTab.AttachedControl).Controls[0] as ITactTerminal;
            if (emulator != null)
            {
                m_SelectedEmulator = emulator;
                if (OnTerminalTabChangeEvent != null) OnTerminalTabChangeEvent(emulator.TerminalStatus, tabTerminal.SelectedTab.Name);
            }
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

            m_TerminalManager.AddEmulator(m_SelectedEmulator);

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
            DeviceInfo deviceInfo = m_SelectedEmulator.DeviceInfo;

            switch (deviceInfo.DeviceType)
            {
                case E_DeviceType.NeGroup:
                    if (OnModifyDeviceEvent != null)
                    {
                        OnModifyDeviceEvent(E_WorkType.Modify, deviceInfo);
                    }
                    break;
                case E_DeviceType.UserNeGroup:
                    if (OnModifyUsrDeviceEvent != null)
                    {
                        OnModifyUsrDeviceEvent(E_WorkType.Modify, deviceInfo);
                    }
                    break;
            }
        }
        /// <summary>
        /// 터미널 개수를 가져오기 합니다.
        /// </summary>
        public int TerminalCount
        {
            get { return m_TerminalManager.EmulatorList.Count; }
        }
        /// <summary>
        /// 변경된 환경 정보를 적용합니다.
        /// </summary>
        internal void ChangeOption()
        {
            m_TerminalManager.ApplyOptionToAll();
        }
        /// <summary>
        /// 컨트롤을 초기화 시킵니다.
        /// </summary>
        public void InitializeControl()
        {
            // TerminalManager 내부 초기화 필요 시 호출
        }
        /// <summary>
        /// 탭 이름 변경 처리 합니다.
        /// </summary>
        private void mnuReName_Click(object sender, EventArgs e)
        {
            TerminalReName renameForm = new TerminalReName(tabTerminal.SelectedTab.Text);
            renameForm.InitializeContro();
            if (renameForm.ShowDialog(this) == DialogResult.OK)
            {
                tabTerminal.SelectedTab.Text = renameForm.GetNewTabName;
            }
        }
        /// <summary>
        /// 터미널 선택 화면 입니다.
        /// </summary>
        private SelectTargetTerminal m_TerminalSelectForm = new SelectTargetTerminal();
        /// <summary>
        /// 스크립트 처리 합니다.
        /// </summary>
        internal void ScriptWork(E_ScriptWorkType scriptWorkType)
        {
            if (m_TerminalManager.EmulatorList.Count == 0) return;

            m_TerminalSelectForm.TerminalList = m_TerminalManager.EmulatorList;
            ITactTerminal selectedTerminal = null;
            if (m_TerminalSelectForm.ShowDialog(AppGlobal.s_ClientMainForm) == DialogResult.OK)
            {
                selectedTerminal = m_TerminalSelectForm.SelectedTerminal;
                if (selectedTerminal.Parent is TerminalWindows)
                {
                    ((Form)selectedTerminal.Parent).BringToFront();
                }
                else
                {
                    tabTerminal.SelectedTab = ((SuperTabControlPanel)selectedTerminal.Parent).TabItem;
                }
                selectedTerminal.ScriptWork(scriptWorkType);
            }
        }
        /// <summary>
        /// 스크립트 처리 합니다.
        /// </summary>
        internal void ScriptWork(E_ScriptWorkType scriptWorkType, Script script)
        {
            if (m_TerminalManager.EmulatorList.Count == 0) return;

            m_TerminalSelectForm.TerminalList = m_TerminalManager.EmulatorList;
            ITactTerminal selectedTerminal = null;
            if (m_TerminalSelectForm.ShowDialog(AppGlobal.s_ClientMainForm) == DialogResult.OK)
            {

                selectedTerminal = m_TerminalSelectForm.SelectedTerminal;
                if (selectedTerminal.Parent is TerminalWindows)
                {
                    ((Form)selectedTerminal.Parent).BringToFront();
                }
                else
                {
                    tabTerminal.SelectedTab = ((SuperTabControlPanel)selectedTerminal.Parent).TabItem;
                }
                m_TerminalSelectForm.SelectedTerminal.RunScript(script);
            }
        }

        /// <summary>
        /// 종료 처리 합니다.
        /// </summary>
        internal void Stop(E_TerminalSessionCloseType closeType)
        {
            m_TerminalManager.StopAll(closeType);
        }
        /// <summary>
        /// 클라이언트 모드 변경을 적용 합니다.
        /// </summary>
        public void ChangeClientMode()
        {
            m_TerminalManager.ChangeClientModeForAll();
        }
        /// <summary>
        /// 메인화면의 편집 처리 합니다.
        /// </summary>
        /// <param name="aEditType"></param>
        internal void ExecTerminalScreen(E_TerminalScreenTextEditType editType)
        {
            if (tabTerminal.SelectedTabIndex < 0) 
                return;
            ((Control)tabTerminal.SelectedTab.AttachedControl).Controls[0].Focus();
            ((ITactTerminal)((SuperTabControlPanel)tabTerminal.SelectedTab.AttachedControl).Controls[0]).ExecTerminalScreen(editType);
        }

        internal void ExecTerminalScreenTest(ITactTerminal terminal, E_TerminalScreenTextEditType editType)
        {
            terminal.ExecTerminalScreen(editType);
        }

        private void mnuCloseOther_Click(object sender, EventArgs e)
        {
            if (m_SelectedEmulator == null) return;

            var tabsToClose = new List<SuperTabItem>();
            foreach (SuperTabItem tab in tabTerminal.Tabs)
            {
                ITactTerminal emulator = tab.AttachedControl.Controls[0] as ITactTerminal;
                if (emulator != null && emulator.ConnectedSessionID != m_SelectedEmulator.ConnectedSessionID)
                {
                    tabsToClose.Add(tab);
                }
            }

            foreach (var tab in tabsToClose)
            {
                tab.Close();
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

            TerminalWindows window = new TerminalWindows();

            m_SelectedEmulator.TerminalMode = E_TerminalMode.QuickClient;

            m_SelectedEmulator.Dock = DockStyle.Fill;

            window.Controls.Add(m_SelectedEmulator);
            window.Size = new Size(AppGlobal.s_ClientOption.PopupSizeWidth, AppGlobal.s_ClientOption.PopupSizeHeight);

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
            window.Text = m_SelectedEmulator.Name;
            tabTerminal.SelectedTab.Close();
            window.MaximizeBox = false;
            window.BringToFront();
            window.Show();
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

                    RequestCommunicationData requestData = null;
                    CfgRestoreCommandRequestInfo request = new CfgRestoreCommandRequestInfo();

                    DeviceInfo deviceInfo = (DeviceInfo)tabTerminal.SelectedTab.Tag;

                    request.CommandPart = E_CommandPart.ConfigBRRestore;
                    request.IPAddress = deviceInfo.IPAddress;
                    request.ModelID = deviceInfo.ModelID;

                    requestData = AppGlobal.MakeDefaultRequestData();
                    requestData.CommType = E_CommunicationType.RequestCfgRestoreCommand;
                    requestData.RequestData = request;

                    m_Result = null;
                    m_MRE.Reset();

                    AppGlobal.SendRequestData(this, requestData);
                    m_MRE.WaitOne(AppGlobal.s_RequestTimeOut);
                }
                else if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Console)
                {
                    if (AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "Config복원명령은 로그인후 root 경로에서 실행됩니다. \r\nConfig복원명령을 실행하시겠습니까?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    {
                        return;
                    }

                    DeviceInfo deviceInfo = (DeviceInfo)tabTerminal.SelectedTab.Tag;

                    if (deviceInfo.CfgSaveInfos.Count == 0)
                    {
                        AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "장비 Config 복원 명령이 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    CfgSaveInfo cfgSaveInfo = (CfgSaveInfo)deviceInfo.CfgSaveInfos.InnerList[0];

                    if (cfgSaveInfo.FullFileName == "")
                    {
                        AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "장비 Config 복원 명령이 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    OpenRestoreCommand restoreForm = new OpenRestoreCommand();
                    restoreForm.OpenConsoleRestoreList(deviceInfo.CfgSaveInfos);

                    if (restoreForm.ShowDialog() != DialogResult.OK) return;

                    CfgSaveInfoCollection selectedCfgSaveInfos = restoreForm.CfgSaveInfos;

                    CfgSaveInfo selectedCfgSaveInfo = (CfgSaveInfo)selectedCfgSaveInfos.InnerList[0];
                    Script script = new Script(selectedCfgSaveInfo.CfgRestoreScript);

                    AppGlobal.s_ClientOption.ShortenCommandTaget = E_ShortenCommandTagret.ActiveTerminal;
                    m_SelectedEmulator.RunScript(script);
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
        private void SetTelnetReservedString(CfgSaveInfoCollection selectedCfgSaveInfos)
        {
            string configFileName = string.Empty;

            foreach (CfgSaveInfo cfgSaveInfo in selectedCfgSaveInfos)
            {
                foreach (CfgRestoreCommand restoreCommand in cfgSaveInfo.CfgRestoreCommands)
                {
                    if (restoreCommand.Cmd.IndexOf(TelnetReservedString.c_FTPIP) > -1)
                    {
                        restoreCommand.Cmd = restoreCommand.Cmd.Replace(TelnetReservedString.c_FTPIP, cfgSaveInfo.FTPServerIP);
                    }
                    else if (restoreCommand.Cmd.IndexOf(TelnetReservedString.c_FTPUSER) > -1)
                    {
                        restoreCommand.Cmd = restoreCommand.Cmd.Replace(TelnetReservedString.c_FTPUSER, cfgSaveInfo.CenterFTPID);
                    }
                    else if (restoreCommand.Cmd.IndexOf(TelnetReservedString.c_FTPPASSEORD) > -1)
                    {
                        restoreCommand.Cmd = restoreCommand.Cmd.Replace(TelnetReservedString.c_FTPPASSEORD, cfgSaveInfo.CenterFTPPW);
                    }
                    else if (restoreCommand.Cmd.IndexOf(TelnetReservedString.c_CONFIGFILENAME) > -1)
                    {
                        configFileName = cfgSaveInfo.FileName;
                        restoreCommand.Cmd = restoreCommand.Cmd.Replace(TelnetReservedString.c_CONFIGFILENAME, configFileName);
                    }
                    else if (restoreCommand.Cmd.IndexOf(TelnetReservedString.c_CONFIGFILENAMEEXT) > -1)
                    {
                        configFileName = cfgSaveInfo.FileName;
                        if (cfgSaveInfo.FileExtend != "")
                        {
                            configFileName += "." + cfgSaveInfo.FileExtend;
                        }
                        restoreCommand.Cmd = restoreCommand.Cmd.Replace(TelnetReservedString.c_CONFIGFILENAMEEXT, configFileName);
                    }
                    else if (restoreCommand.Cmd.IndexOf(TelnetReservedString.c_CONFIGFILENAME16) > -1)
                    {
                        configFileName = cfgSaveInfo.FileName;
                        restoreCommand.Cmd = restoreCommand.Cmd.Replace(TelnetReservedString.c_CONFIGFILENAME16, configFileName);
                    }
                    else if (restoreCommand.Cmd.IndexOf(TelnetReservedString.c_CONFIGFILENAMEEXT16) > -1)
                    {
                        configFileName = cfgSaveInfo.FileName;
                        if (cfgSaveInfo.FileExtend != "")
                        {
                            configFileName += "." + cfgSaveInfo.FileExtend;
                        }
                        restoreCommand.Cmd = restoreCommand.Cmd.Replace(TelnetReservedString.c_CONFIGFILENAMEEXT16, configFileName);
                    }
                    else if (restoreCommand.Cmd.IndexOf(TelnetReservedString.c_CONFIGFULLFILENAME) > -1)
                    {
                        configFileName = cfgSaveInfo.FileName;
                        if (cfgSaveInfo.FileExtend != "")
                        {
                            configFileName += "." + cfgSaveInfo.FileExtend;
                        }
                        restoreCommand.Cmd = restoreCommand.Cmd.Replace(TelnetReservedString.c_CONFIGFULLFILENAME, configFileName);
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
            SearchLinkDevice searchForm = new SearchLinkDevice();

            if (searchForm.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }

            DeviceInfoCollection deviceInfos = searchForm.SelectedDeviceList;
            DeviceInfo deviceInfo = null;

            foreach (DeviceInfo item in deviceInfos)
            {
                deviceInfo = item;
            }

            m_LinkDeviceInfo = deviceInfo;

            StartSendThread(new ThreadStart(RequestConnectionCommand));
        }

        private void RequestConnectionCommand()
        {
            RequestCommunicationData requestData = null;

            requestData = AppGlobal.MakeDefaultRequestData();
            requestData.CommType = E_CommunicationType.RequestDefaultConnectionCommand;
            requestData.RequestData = m_LinkDeviceInfo;
            m_Result = null;
            m_MRE.Reset();

            AppGlobal.SendRequestData(this, requestData);
            m_MRE.WaitOne(AppGlobal.s_RequestTimeOut);
        }

        /// <summary>
        /// 2013-05-02 - shinyn - 기본접속 명령을 요청후 스크립트를 실행한다.
        /// </summary>
        public override void ResultReceiver(ResultCommunicationData result)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument1<ResultCommunicationData>(ResultReceiver), result);
                return;
            }

            base.ResultReceiver(result);

            m_ResponseTcs?.TrySetResult(result);

            if (m_Result == null || m_Result.Error.Error != E_ErrorType.NoError)
            {
                HandleErrorResult(result);
                return;
            }

            object resultData = m_Result.ResultData;
            if (resultData == null) return;

            if (resultData is FACT_DefaultConnectionCommandSet commandSet)
            {
                HandleDefaultConnectionCommand(commandSet);
            }
            else if (resultData is CompressData compressData)
            {
                HandleCompressedDeviceData(compressData);
            }
            else if (resultData is CfgSaveInfoCollection cfgSaveInfos)
            {
                HandleConfigRestoreData(cfgSaveInfos);
            }
        }

        private void HandleErrorResult(ResultCommunicationData result)
        {
            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "TerminalPanel : ResultReceiver : 명령 정보 로드 실패. CommType: " + result.CommType);
            if (result.CommType == E_CommunicationType.RequestDefaultConnectionCommand)
            {
                MessageBox.Show("접속정보를 가져오지 못했습니다.");
            }
        }

        private void HandleDefaultConnectionCommand(FACT_DefaultConnectionCommandSet commandSet)
        {
            Script script = new Script(AppGlobal.GetTelnetScript(m_LinkDeviceInfo, commandSet));
            m_SelectedEmulator.RunScript(script);
        }

        private void HandleCompressedDeviceData(CompressData compressData)
        {
            object decompressedObject = AppGlobal.DecompressObject(compressData);
            if (decompressedObject is DeviceInfoCollection deviceInfos)
            {
                DeviceInfo deviceInfo = ResolveDeviceInfo(deviceInfos);
                if (deviceInfo != null)
                {
                    AddTerminal(deviceInfo, true);
                }
            }
        }

        private DeviceInfo ResolveDeviceInfo(DeviceInfoCollection deviceInfos)
        {
            if (m_ConnectInfo == null) return null;

            if (!deviceInfos.Contains(m_ConnectInfo.IPAddress))
            {
                return new DeviceInfo
                {
                    IsRegistered = false,
                    IPAddress = m_ConnectInfo.IPAddress,
                    TerminalConnectInfo = m_ConnectInfo
                };
            }

            DeviceInfo deviceInfo = deviceInfos[m_ConnectInfo.IPAddress];
            if (IsAccessDenied(deviceInfo))
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "해당 장비에 접속 권한이 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            deviceInfo.TerminalConnectInfo = m_ConnectInfo;
            if (m_ConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
            {
                deviceInfo.TelnetID1 = m_ConnectInfo.ID;
                deviceInfo.TelnetPwd1 = m_ConnectInfo.Password;
            }

            return deviceInfo;
        }

        private bool IsAccessDenied(DeviceInfo deviceInfo)
        {
            return AppGlobal.s_RACTClientMode == E_RACTClientMode.Online &&
                   AppGlobal.s_LoginResult.UserInfo.Centers.Count > 0 &&
                   !AppGlobal.s_LoginResult.UserInfo.Centers.Contains(deviceInfo.CenterCode);
        }

        private void HandleConfigRestoreData(CfgSaveInfoCollection cfgSaveInfos)
        {
            if (cfgSaveInfos.Count == 0 || string.IsNullOrEmpty(((CfgSaveInfo)cfgSaveInfos.InnerList[0]).FileName))
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "Config 복원 파일이 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            OpenRestoreCommand restoreForm = new OpenRestoreCommand();
            restoreForm.OpenOnlineRestoreList(cfgSaveInfos);

            if (restoreForm.ShowDialog() == DialogResult.OK)
            {
                CfgSaveInfoCollection selectedCfgSaveInfos = restoreForm.CfgSaveInfos;
                SetTelnetReservedString(selectedCfgSaveInfos);

                CfgSaveInfo selectedCfgSaveInfo = (CfgSaveInfo)selectedCfgSaveInfos.InnerList[0];
                Script script = new Script(AppGlobal.GetScript(selectedCfgSaveInfo.CfgRestoreCommands));

                AppGlobal.s_ClientOption.ShortenCommandTaget = E_ShortenCommandTagret.ActiveTerminal;
                m_SelectedEmulator.RunScript(script);
            }
        }
        }


        private void Global_OnSharedSessionDisconnected(string disconnectedKey)
        {
            UiContext.RunAsync(() =>
            {
                if (this.IsDisposed) return;

                AppGlobal.s_FileLogProcessor?.PrintLog(E_FileLogType.Warning, $" 장비 세션 연결 단절됨 : {disconnectedKey}");

                var targets = m_TerminalManager.EmulatorList.Where(e => e.JumpHost != null).ToList();

                foreach (ITactTerminal emulator in targets)
                {
                    string emulatorKey = SshSessionPool.Instance.GetKey(emulator.JumpHost);
                    if (emulatorKey == disconnectedKey)
                    {
                        ExecuteAutoReconnect(emulator);
                    }
                }
            });
        }

        private void ExecuteAutoReconnect(ITactTerminal emulator)
        {
            emulator.TerminalStatus = E_TerminalStatus.Disconnected;

            string emulatorName = emulator.Name;
            DeviceInfo targetInfo = emulator.DeviceInfo;
            DeviceInfo jumpHostInfo = emulator.JumpHost;

            Task.Run(() =>
            {
                try
                {
                    AppGlobal.s_FileLogProcessor?.PrintLog(E_FileLogType.Infomation, $"[  տ   ]  հ   õ   : {emulatorName}");
                    emulator.ConnectDevice(targetInfo, jumpHostInfo);
                    AppGlobal.s_FileLogProcessor?.PrintLog(E_FileLogType.Infomation, $"[  տ   ]  հ   ߽   : {emulatorName}");
                }
                catch (Exception ex)
                {
                    AppGlobal.s_FileLogProcessor?.PrintLog(E_FileLogType.Error, $"[  տ   ] {emulatorName}  հ   н : {ex.Message}");
                }
            });
        }

    }
}
