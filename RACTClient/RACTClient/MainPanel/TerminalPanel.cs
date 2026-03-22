using DevComponents.DotNetBar;
using RACTClient.Services;
using RACTClient.Utilities;
using RACTCommonClass;
using RACTTerminal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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
    public partial class TerminalPanel : UserControl, IMainPanel, ISenderObject
    {
        public event ModifyDeviceHandler OnModifyDeviceEvent;
        public event ModifyUsrDeviceHandler OnModifyUsrDeviceEvent;
        public event TerminalTabChangeHandler OnTerminalTabChangeEvent;
        /// <summary>
        /// 활성화 중인 터미널 목록입니다.
        /// </summary>
        private List<ITactTerminal> m_EmulatorList = new List<ITactTerminal>();

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
        public List<ITactTerminal> EmulatorList
        {
            get { return m_EmulatorList; }
        }

        private readonly TerminalConnectionService _connectionService;

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public TerminalPanel()
        {
            InitializeComponent();




            // 0. 서비스 초기화
            _connectionService = new TerminalConnectionService();

            Image tImage = global::RACTClient.Properties.Resources.led_green;
            imlTab.Images.Add(tImage);
            tImage = global::RACTClient.Properties.Resources.led_red;
            imlTab.Images.Add(tImage);
            tImage = global::RACTClient.Properties.Resources.led_gray;
            imlTab.Images.Add(tImage);
            tImage = global::RACTClient.Properties.Resources.led_blue;
            imlTab.Images.Add(tImage);

            ucShortenCommand.OnSendShortenCommand += new HandlerArgument1<ShortenCommandInfo>(ShortenCommand_OnSendShortenCommand);
            ucShortenScript.OnSendScript -= new HandlerArgument1<Script>(ShortenScript_OnSendScript);
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
                foreach (ITactTerminal tTelnet in m_EmulatorList)
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
            if (m_EmulatorList.Count == 0) return;

            if (AppGlobal.s_ClientOption.ShortenCommandTaget == E_ShortenCommandTagret.ActiveTerminal)
            {
                // m_TerminalSelectForm.TerminalList expects List<ITerminal> or similar?
                // Assuming TerminalSelectForm is updated or handles ITerminal generic list
                m_TerminalSelectForm.TerminalList = m_EmulatorList;
                ITactTerminal tSelectedTerminal = null;
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
                foreach (ITactTerminal tTelnet in m_EmulatorList)
                {
                    //tTelnet.DispatchMessage(this, aValue1.Command + "\r\n");
                    tTelnet.IsLimitCmdForShortenCommand(this, aValue1.Command + "\r\n");
                }
            }
        }

        #region 고성능 비동기 통신 사용 예시 (.NET 10 대응)

        /// <summary>
        /// [.NET 10 고성능 서버 대응 예시]
        /// 현재 활성화된 터미널의 장비 정보를 서버로부터 비동기로 다시 가져옵니다.
        /// </summary>
        public async Task RefreshDeviceInfoAsync()
        {
            if (tabTerminal.SelectedTab == null) return;
            
            DeviceInfo currentDevice = tabTerminal.SelectedTab.Tag as DeviceInfo;
            if (currentDevice == null) return;

            try
            {
                // 1. 요청 패킷 구성 (기존 DTO 사용)
                var request = new RequestCommunicationData
                {
                    CommType = E_CommunicationType.RequestDeviceInfo,
                    RequestData = new DeviceRequestInfo 
                    { 
                        WorkType = E_WorkType.Search, 
                        DeviceInfo = currentDevice 
                    }
                };

                // 2. 고성능 채널(Pipelines + Channels)을 통한 비동기 요청 및 대기
                // UI 스레드를 차단하지 않고 서버의 응답을 기다립니다.
                ResultCommunicationData result = await AppGlobal.SendRequestAsync(request);

                if (result != null && result.Error.Error == E_ErrorType.NoError)
                {
                    // 3. 결과 역직렬화 (MessagePack 내부 처리됨) 및 UI 반영
                    // 컴축된 데이터인 경우 해제 후 반영
                    DeviceInfo updatedDevice = result.ResultData as DeviceInfo;
                    if (updatedDevice != null)
                    {
                        UpdateTerminalUI(updatedDevice);
                        AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, $"[고성능 채널] {updatedDevice.Name} 정보 새로고침 완료");
                    }
                }
            }
            catch (Exception ex)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, "장비 정보 새로고침 실패: " + ex.Message);
            }
        }

        /// <summary>
        /// 비동기 결과를 UI에 안전하게 반영합니다.
        /// </summary>
        private void UpdateTerminalUI(DeviceInfo device)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdateTerminalUI(device)));
                return;
            }

            if (tabTerminal.SelectedTab != null)
            {
                tabTerminal.SelectedTab.Text = device.Name;
                tabTerminal.SelectedTab.Tag = device;
                // 기타 UI 컨트롤 갱신 로직...
            }
        }

        #endregion

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

            int tMaxNumber = 0;
            foreach (SuperTabItem tab in tabTerminal.Tabs)
            {
                var tDeviceInfo = tab.Tag as DeviceInfo;
                if (tDeviceInfo == null) continue;

                bool isSameDevice = false;
                var protocol = aDeviceInfo.TerminalConnectInfo.ConnectionProtocol;

                if (protocol == E_ConnectionProtocol.TELNET || protocol == E_ConnectionProtocol.SSHTelnet)
                {
                    isSameDevice = tDeviceInfo.IPAddress.Equals(aDeviceInfo.IPAddress);
                }
                else if (protocol == E_ConnectionProtocol.SERIAL_PORT)
                {
                    isSameDevice = tDeviceInfo.TerminalConnectInfo.SerialConfig.PortName.Equals(aDeviceInfo.TerminalConnectInfo.SerialConfig.PortName);
                }

                if (isSameDevice)
                {
                    tCount++;
                    string tTabName = tab.Text;
                    if (tTabName.Contains("(") && tTabName.Contains(")"))
                    {
                        try
                        {
                            int start = tTabName.LastIndexOf("(") + 1;
                            int end = tTabName.LastIndexOf(")");
                            if (int.TryParse(tTabName.Substring(start, end - start), out int tTempNumber))
                            {
                                if (tTempNumber > tMaxNumber) tMaxNumber = tTempNumber;
                            }
                        }
                        catch { /* 무시 */ }
                    }
                }
            }

            if (tMaxNumber > 0) tCount = tMaxNumber + 1;
            return true;
        }
        /// <summary>
        /// 터미널을 생성 합니다.
        /// </summary>
        private ITactTerminal MakeEmulator(DeviceInfo aDeviceInfo, bool aIsQuickConnection)
        {
            ITactTerminal tEmulator = null;

            tEmulator = new TerminalView();

            tEmulator.OnTelnetFindString += new DefaultHandler(Emulator_OnTelnetFindString);
            tEmulator.OnTerminalStatusChange += new HandlerArgument2<ITactTerminal, E_TerminalStatus>(tEmulator_OnTerminalStatusChange);
            tEmulator.Modes = new Mode();
            tEmulator.Modes.Flags = tEmulator.Modes.Flags |= Mode.s_AutoWrap;
            ((Control)tEmulator).Dock = DockStyle.Fill;
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
        private SuperTabItem MakeTabPanel(ITactTerminal aMCTerminalEmulator, int aCount)
        {

            SuperTabControlPanel tTabPanel;
            SuperTabItem tTabItem;
            tTabItem = new DevComponents.DotNetBar.SuperTabItem();
            tTabPanel = new DevComponents.DotNetBar.SuperTabControlPanel();
            tTabItem.MouseUp += new MouseEventHandler(TabItem_MouseUp);
            tabTerminal.Controls.Add(tTabPanel);
            tTabPanel.Controls.Add(aMCTerminalEmulator.UIControl);
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
            ((Control)aMCTerminalEmulator).Name = tTabItem.Name;

            tTabItem.Tag = aMCTerminalEmulator.DeviceInfo;
            tTabItem.Tooltip = aMCTerminalEmulator.ToolTip;
            tTabPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            tTabPanel.Location = new System.Drawing.Point(0, 26);
            tTabPanel.Name = "superTabControlPanel1";
            tTabPanel.Size = new System.Drawing.Size(150, 124);
            tTabPanel.TabIndex = 1;
            tTabPanel.TabItem = tTabItem;

            ((Control)aMCTerminalEmulator).Name = tTabItem.Text;


            return tTabItem;

        }

        internal void AddTerminal(ITactTerminal mCTerminalEmulator)
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
        /// <param name="aDaemonProcessInfo">데몬 프로세스 정보 (선택 사항)</param>
        public async void AddTerminal(DeviceInfo aDeviceInfo, bool aIsQuickConnection, DaemonProcessInfo aDaemonProcessInfo = null)
        {
            int tCount = 0;
            if (!CheckTerminalCount(aDeviceInfo, out tCount)) return;

            ITactTerminal tEmulator = MakeEmulator(aDeviceInfo, aIsQuickConnection);
            tEmulator.DaemonProcessInfo = aDaemonProcessInfo;

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
                tEmulator.UIControl.Dock = DockStyle.Fill;
                tForm.AddTerminalControl(tEmulator);
                tForm.Size = new Size(AppGlobal.s_ClientOption.PopupSizeWidth, AppGlobal.s_ClientOption.PopupSizeHeight);

                string displayName;
                if (aDeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SERIAL_PORT)
                {
                    displayName = "Serial-" + aDeviceInfo.TerminalConnectInfo.SerialConfig.PortName;
                }
                else if (AppGlobal.s_ClientOption.TerminalDisplayNameType == E_TerminalDisplayNameType.IPAddress)
                {
                    displayName = aDeviceInfo.IPAddress;
                }
                else
                {
                    displayName = aDeviceInfo.Name;
                }

                if (tCount > 0) displayName += $"({tCount})";

                tEmulator.Name = displayName;
                aDeviceInfo.TerminalName = displayName;
                tForm.Text = displayName;
                tForm.MaximizeBox = false;
                tForm.Show();
            }

            try
            {
                // [리팩토링] 레거시 IsChangeMode 기능을 서비스 레이어(TerminalPanel)에서 재구현
                // 1차 접속 시도
                if (!await tEmulator.ConnectDeviceAsync(aDeviceInfo))
                {
                    // 접속 실패 시 RPCS 모델이고 현재 유선 모드(Mode 3 아님)인 경우 무선 전환 의사 확인
                    if (AppGlobal.s_ConnectionMode != 3 && AppGlobal.IsRpcsDevice(aDeviceInfo.ModelID))
                    {
                        var confirmMsg = "유선 접속이 실패 하였습니다. 무선 접속 하시겠습니까?\r\n" +
                                       "RPCS(무선) 장비 접속 시 LTE망 과금이 발생합니다.\r\n접속 하시겠습니까?";

                        if (MessageBox.Show(confirmMsg, "무선 접속 전환", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            // 무선 모드(3)로 임시 변경 후 2차 접속 시도
                            int originalMode = AppGlobal.s_ConnectionMode;
                            try
                            {
                                AppGlobal.s_ConnectionMode = 3;
                                await tEmulator.ConnectDeviceAsync(aDeviceInfo);
                            }
                            finally
                            {
                                // 다른 연결에 영향을 주지 않도록 원래 모드로 복구 (또는 전역 유지 정책에 따름)
                                // 레거시는 전역적으로 사용하므로 여기서는 유지하거나 복구하는 정책 결정 필요
                                // 일단 레거시 정합성을 위해 유지하지 않고 개별 접속 세션에만 적용되도록 함
                                AppGlobal.s_ConnectionMode = originalMode;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.SafeInvoke(() =>
                {
                    AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, $"장비 연결 중 예외 발생 ({aDeviceInfo.IPAddress}): {ex.Message}");
                });
            }

            OnTerminalTabChangeEvent?.Invoke(E_TerminalStatus.Add, tEmulator.Name);
        }

        /// <summary>
        /// 2014-10-14 - 신윤남 - 창닫히는 경우 에뮬레이터 리스트에서 삭제
        /// </summary>
        /// <param name="aValue1"></param>
        /// <param name="aValue2"></param>
        public void tEmulator_OnTerminalStatusChange(ITactTerminal aValue1, E_TerminalStatus aValue2)
        {
            try
            {
                if (OnTerminalTabChangeEvent != null) OnTerminalTabChangeEvent(aValue2, aValue1.Name);

                lock (m_EmulatorList)
                {
                    if (aValue2 == E_TerminalStatus.Disconnected)
                    {
                        m_EmulatorList.Remove(aValue1);
                    }
                    else if (aValue2 == E_TerminalStatus.TryConnection || aValue2 == E_TerminalStatus.Connection)
                    {
                        if (!m_EmulatorList.Contains(aValue1))
                        {
                            m_EmulatorList.Add(aValue1);
                        }
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
        /// <param name="aConnectInfo"></param>
        internal async Task QuickConnect(TerminalConnectInfo aConnectInfo)
        {
            m_ConnectInfo = aConnectInfo;

            if (aConnectInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET ||
                aConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
            {
                if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online)
                {
                    try
                    {
                        // 서버에 사용할 수 있는 장비인지 비동기로 확인합니다.
                        var tRequestData = AppGlobal.MakeDefaultRequestData();
                        tRequestData.CommType = E_CommunicationType.RequestFACTSearchDevice;
                        tRequestData.RequestData = new DeviceSearchInfo
                        {
                            DeviceIPAddress = aConnectInfo.IPAddress,
                            UserID = AppGlobal.s_LoginResult.UserID,
                            IsCheckPermission = false
                        };

                        var result = await _connectionService.SendRequestAsync(this, tRequestData);
                        if (result != null && result.ResultData != null)
                        {
                            var deviceCollection = AppGlobal.DecompressObject((CompressData)result.ResultData) as DeviceInfoCollection;

                            DeviceInfo tDeviceInfo = null;
                            if (deviceCollection != null && deviceCollection.Contains(aConnectInfo.IPAddress))
                            {
                                tDeviceInfo = deviceCollection[aConnectInfo.IPAddress];

                                if (AppGlobal.s_LoginResult.UserInfo.Centers.Count > 0)
                                {
                                    if (!AppGlobal.s_LoginResult.UserInfo.Centers.Contains(tDeviceInfo.CenterCode))
                                    {
                                        AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "해당 장비에 접속 권한이 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return;
                                    }
                                }

                                // 20260227 ShinMyungsu User별 장비접속권한(MangTypeCd 망구분 추가)
                                if (AppGlobal.s_LoginResult.UserInfo.MangTypes.Count > 0)
                                {
                                    if (!AppGlobal.s_LoginResult.UserInfo.MangTypes.Contains(tDeviceInfo.MangTypeCd))
                                    {
                                        AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "해당 장비에 접속 권한이 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return;
                                    }
                                }
                                
                                tDeviceInfo.TerminalConnectInfo = aConnectInfo;
                            }
                            else
                            {
                                tDeviceInfo = new DeviceInfo
                                {
                                    IsRegistered = false,
                                    IPAddress = aConnectInfo.IPAddress,
                                    TerminalConnectInfo = aConnectInfo
                                };

                                if (aConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
                                {
                                    tDeviceInfo.TelnetID1 = aConnectInfo.ID;
                                    tDeviceInfo.TelnetPwd1 = aConnectInfo.Password;
                                }
                            }

                            AddTerminal(tDeviceInfo, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, $"장비 조회 중 오류 발생: {ex.Message}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    var tDeviceInfo = new DeviceInfo
                    {
                        IsRegistered = false,
                        IPAddress = m_ConnectInfo.IPAddress,
                        TerminalConnectInfo = m_ConnectInfo
                    };
                    AddTerminal(tDeviceInfo, true);
                }
            }
            else if (aConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SERIAL_PORT)
            {
                var tDeviceInfo = new DeviceInfo
                {
                    IsRegistered = false,
                    IPAddress = m_ConnectInfo.IPAddress,
                    TerminalConnectInfo = m_ConnectInfo
                };
                AddTerminal(tDeviceInfo, true);
            }
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

        void TabItem_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                SuperTabItem tBaseItem = (SuperTabItem)sender;
                tabTerminal.SelectedTab = tBaseItem;
                SuperTabControlPanel tPanel = (SuperTabControlPanel)tBaseItem.AttachedControl;
                ITactTerminal tEmulator = (ITactTerminal)tPanel.Controls[0];
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
        private ITactTerminal m_SelectedEmulator = null;
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
        // private MCTerminalEmulator m_CloseEmulator = null; // 제거됨
        /// <summary>
        /// 탭 닫기 처리 합니다.
        /// </summary>
        private void tabTerminal_TabItemClose(object sender, SuperTabStripTabItemCloseEventArgs e)
        {
            if (((SuperTabItem)e.Tab).AttachedControl.Controls.Count == 0) return;

            ITactTerminal tCloseEmulator = ((SuperTabItem)e.Tab).AttachedControl.Controls[0] as ITactTerminal;

            // 2014-08-19 - 신윤남 - 종료 클릭시에는 상위 Parent를 종료하지 않도록 한다.
            tCloseEmulator.UIControl.Tag = "TabItemClose";

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
            ITactTerminal tEmulator = (ITactTerminal)((SuperTabControlPanel)tabTerminal.SelectedTab.AttachedControl).Controls[0];
            m_SelectedEmulator = tEmulator;
            if (OnTerminalTabChangeEvent != null) OnTerminalTabChangeEvent(tEmulator.TerminalStatus, tabTerminal.SelectedTab.Name);
        }
        /// <summary>
        /// 재 접속 처리 합니다.
        /// </summary>
        private async void mnuReConnect_Click(object sender, EventArgs e)
        {
            if (m_SelectedEmulator == null) return;
            if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Console)
            {
                m_SelectedEmulator.DeviceInfo.IsRegistered = false;
            }
            // 재연결 작업을 별도 태스크로 실행
            try
            {
                await m_SelectedEmulator.ConnectDeviceAsync(m_SelectedEmulator.DeviceInfo);
            }
            catch (Exception ex)
            {
                this.SafeInvoke(() =>
                {
                    AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, $"재연결 중 오류 ({m_SelectedEmulator.DeviceInfo.IPAddress}): {ex.Message}");
                });
            }

            /*
            if (m_SelectedEmulator is TerminalView view)
            {
                view.CloseSession();

                try
                {
                    var targetInfo = MapToTargetInfo(m_SelectedEmulator.DeviceInfo);
                    var context = await _bastionService.OpenSessionAsync(targetInfo);
                    view.AttachSession(context);

                    m_EmulatorList.Add(m_SelectedEmulator);

                    // 재연결시 바로 터미널에서 키보드 입력할 수 있도록 포커스 처리
                    m_SelectedEmulator.UIControl.Focus();
                }
                catch (Exception ex)
                {
                    view.LogMessage($"Error: {ex.Message}");
                }
            }
            */


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
            foreach (ITactTerminal tEmulator in m_EmulatorList)
            {
                tEmulator.ApplyOption();
            }
        }
        /// <summary>
        /// 컨트롤을 초기화 시킵니다.
        /// </summary>
        public void InitializeControl()
        {
            m_EmulatorList = new List<ITactTerminal>();
        }
        /// <summary>
        /// 탭 이름 변경 처리 합니다.
        /// </summary>
        /// <summary>
        /// 탭 이름 변경 처리 및 TerminalView의 Name 동기화
        /// </summary>
        private void mnuReName_Click(object sender, EventArgs e)
        {
            // 1. 현재 선택된 탭 아이템 가져오기
            var selectedTab = tabTerminal.SelectedTab as DevComponents.DotNetBar.SuperTabItem;
            if (selectedTab == null) return;

            // 2. 이름 변경 다이얼로그 띄우기
            TerminalReName tReName = new TerminalReName(selectedTab.Text);
            tReName.InitializeContro();

            if (tReName.ShowDialog(this) == DialogResult.OK)
            {
                string newName = tReName.GetNewTabName;

                // 3. 탭의 표시 텍스트 변경
                selectedTab.Text = newName;

                // 4. [핵심] 탭에 붙어있는 TerminalView 컨트롤의 Name 속성 변경
                // SuperTabItem.AttachedControl은 탭 페이지 내부의 Panel(컨트롤)을 가리킵니다.
                if (selectedTab.AttachedControl != null)
                {
                    // AttachedControl 내부에 TerminalView가 직접 로드되어 있는 경우
                    var terminalView = selectedTab.AttachedControl.Controls[0] as TerminalView;

                    // 만약 TerminalView가 다른 패널 등에 Dock되어 있다면 FindForm이나 Controls 탐색이 필요할 수 있으나,
                    // 일반적인 구조라면 아래와 같이 처리됩니다.
                    if (terminalView != null)
                    {
                        terminalView.UpdateTerminalName(newName);
                        // 이제부터 WriteClog() 호출 시 terminalView.Name이 newName이므로 
                        // 새 이름의 .clog 파일이 생성됩니다.
                    }
                }
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
            ITactTerminal tSelectedTerminal = null;
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
            ITactTerminal tSelectedTerminal = null;
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
            if (aCloseType == E_TerminalSessionCloseType.All)
            {
                for (int i = m_EmulatorList.Count - 1; i > -1; i--)
                {
                    m_EmulatorList[i].Disconnect();
                }
                Thread.Sleep(500);
            }
            else
            {
                for (int i = m_EmulatorList.Count - 1; i > -1; i--)
                {
                    var tTelnet = m_EmulatorList[i];
                    if (tTelnet.ConnectionType == ConnectionTypes.RemoteTelnet)
                    {
                        tTelnet.Disconnect();
                    }
                }
            }
        }

        internal async Task StopAsync(E_TerminalSessionCloseType aCloseType)
        {
            var terminals = m_EmulatorList.ToArray();

            if (aCloseType == E_TerminalSessionCloseType.All)
            {
                for (int i = terminals.Length - 1; i > -1; i--)
                {
                    await terminals[i].DisconnectAsync();
                }
            }
            else
            {
                for (int i = terminals.Length - 1; i > -1; i--)
                {
                    var tTelnet = terminals[i];
                    if (tTelnet.ConnectionType == ConnectionTypes.RemoteTelnet)
                    {
                        await tTelnet.DisconnectAsync();
                    }
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
            // }
            var terminals = m_EmulatorList.ToArray();
            for (int i = 0; i < terminals.Length; i++)
            {
                terminals[i].ChangeClientMode();
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
            ((Control)tabTerminal.SelectedTab.AttachedControl).Controls[0].Focus();
            ((ITactTerminal)((SuperTabControlPanel)tabTerminal.SelectedTab.AttachedControl).Controls[0]).ExecTerminalScreen(aEditType);
        }

        internal void ExecTerminalScreenTest(ITactTerminal m, E_TerminalScreenTextEditType aEditType)
        {

            m.ExecTerminalScreen(aEditType);
        }

        private void mnuCloseOther_Click(object sender, EventArgs e)
        {
            if (tabTerminal.SelectedTab == null) return;

            var selectedTab = tabTerminal.SelectedTab;

            // 역순으로 순회하며 선택된 탭을 제외하고 모두 닫기
            for (int i = tabTerminal.Tabs.Count - 1; i >= 0; i--)
            {
                var tab = tabTerminal.Tabs[i] as SuperTabItem;
                if (tab != null && tab != selectedTab)
                {
                    tab.Close();
                }
            }

            this.Invalidate();
        }

        private void mnuCloseAll_Click(object sender, EventArgs e)
        {
            for (int i = tabTerminal.Tabs.Count - 1; i >= 0; i--)
            {
                var tab = tabTerminal.Tabs[i] as SuperTabItem;
                if (tab != null)
                {
                    tab.Close();
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

            m_SelectedEmulator.UIControl.Dock = DockStyle.Fill;

            tForm.AddTerminalControl(m_SelectedEmulator);
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
            m_SelectedEmulator.DeviceInfo.TerminalName = m_SelectedEmulator.Name;
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
        private async void mnuRestoreCfgCmd_Click(object sender, EventArgs e)
        {
            if (m_SelectedEmulator == null) return;

            try
            {
                if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online)
                {
                    if (AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "Config복원명령은 로그인후 root 경로에서 실행됩니다. \r\nConfig복원명령을 실행하시겠습니까?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    {
                        return;
                    }

                    var tRequest = new CfgRestoreCommandRequestInfo
                    {
                        CommandPart = E_CommandPart.ConfigBRRestore,
                        IPAddress = m_SelectedEmulator.DeviceInfo.IPAddress,
                        ModelID = m_SelectedEmulator.DeviceInfo.ModelID
                    };

                    var tRequestData = AppGlobal.MakeDefaultRequestData();
                    tRequestData.CommType = E_CommunicationType.RequestCfgRestoreCommand;
                    tRequestData.RequestData = tRequest;

                    var result = await _connectionService.SendRequestAsync(this, tRequestData);
                    if (result != null && result.ResultData != null)
                    {
                        if (result.Error.Error != E_ErrorType.NoError)
                        {
                            AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "오류 발생:" + result.Error.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        var tCfgSaveInfos = result.ResultData as CfgSaveInfoCollection;
                        if (tCfgSaveInfos == null || tCfgSaveInfos.Count == 0)
                        {
                            AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "최근 Config 복원 파일이 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        var tCfgSaveInfo = tCfgSaveInfos.InnerList[0] as CfgSaveInfo;
                        if (string.IsNullOrEmpty(tCfgSaveInfo?.FileName))
                        {
                            AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "최근 Config 복원 파일이 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        var tOpenRestoreCommand = new OpenRestoreCommand();
                        tOpenRestoreCommand.OpenOnlineRestoreList(tCfgSaveInfos);

                        if (tOpenRestoreCommand.ShowDialog() != DialogResult.OK) return;

                        var tSelectCfgSaveInfos = tOpenRestoreCommand.CfgSaveInfos;
                        SetTelnetReservedString(tSelectCfgSaveInfos);

                        var tSelectCfgSaveInfo = tSelectCfgSaveInfos.InnerList[0] as CfgSaveInfo;
                        var tScript = new Script(AppGlobal.GetScript(tSelectCfgSaveInfo.CfgRestoreCommands));

                        AppGlobal.s_ClientOption.ShortenCommandTaget = E_ShortenCommandTagret.ActiveTerminal;
                        m_SelectedEmulator.RunScript(tScript);
                    }
                }
                else if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Console)
                {
                    if (AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "Config복원명령은 로그인후 root 경로에서 실행됩니다. \r\nConfig복원명령을 실행하시겠습니까?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    {
                        return;
                    }

                    var tDeviceInfo = m_SelectedEmulator.DeviceInfo;
                    if (tDeviceInfo.CfgSaveInfos.Count == 0)
                    {
                        AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "장비 Config 복원 명령이 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var tCfgSaveInfo = tDeviceInfo.CfgSaveInfos.InnerList[0] as CfgSaveInfo;
                    if (string.IsNullOrEmpty(tCfgSaveInfo?.FullFileName))
                    {
                        AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "장비 Config 복원 명령이 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var tOpenRestoreCommand = new OpenRestoreCommand();
                    tOpenRestoreCommand.OpenConsoleRestoreList(tDeviceInfo.CfgSaveInfos);

                    if (tOpenRestoreCommand.ShowDialog() != DialogResult.OK) return;

                    var tSelectCfgSaveInfos = tOpenRestoreCommand.CfgSaveInfos;
                    var tSelectCfgSaveInfo = tSelectCfgSaveInfos.InnerList[0] as CfgSaveInfo;
                    var tScript = new Script(tSelectCfgSaveInfo.CfgRestoreScript);

                    AppGlobal.s_ClientOption.ShortenCommandTaget = E_ShortenCommandTagret.ActiveTerminal;
                    m_SelectedEmulator.RunScript(tScript);
                }
            }
            catch (Exception ex)
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        private async void mnuLinkConnect_Click(object sender, EventArgs e)
        {
            var tSearch = new SearchLinkDevice();
            if (tSearch.ShowDialog(this) != DialogResult.OK) return;

            var tDeviceInfos = tSearch.SelectedDeviceList;
            var tDeviceInfo = tDeviceInfos.InnerList.Count > 0 ? tDeviceInfos.InnerList[0] as DeviceInfo : null;

            if (tDeviceInfo == null) return;

            try
            {
                var tRequestData = AppGlobal.MakeDefaultRequestData();
                tRequestData.CommType = E_CommunicationType.RequestDefaultConnectionCommand;
                tRequestData.RequestData = tDeviceInfo;

                var result = await _connectionService.SendRequestAsync(this, tRequestData);
                if (result != null && result.ResultData != null)
                {
                    // 비동기 결과 처리 후 필요한 로직 실행 (예: 스크립트 실행 또는 연결)
                    if (result.Error.Error != E_ErrorType.NoError)
                    {
                        AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "TerminalPanel : mnuLinkConnect_Click : 기본 접속 명령 정보 로드에 실패 했습니다. IP : " + tDeviceInfo.IPAddress);
                        MessageBox.Show("접속정보를 가져오지 못했습니다.");
                        return;
                    }

                    if (result.ResultData.GetType().Equals(typeof(FACT_DefaultConnectionCommandSet)))
                    {
                        FACT_DefaultConnectionCommandSet tCommandSet = result.ResultData as FACT_DefaultConnectionCommandSet;
                        Script tScript = new Script(AppGlobal.GetTelnetScript(tDeviceInfo, tCommandSet));
                        m_SelectedEmulator.RunScript(tScript);
                    }
                }
            }
            catch (Exception ex)
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 2013-05-02 - shinyn - 기본접속 명령을 요청후 스크립트를 실행한다.
        /// </summary>
        /// <param name="vResult"></param>
        // ISenderObject 인터페이스 구현을 위해 유지합니다.
        public void ResultReceiver(ResultCommunicationData result)
        {
            if (result == null) return;

            // [핵심 수정] 
            // 수신된 결과를 비동기 서비스(TerminalConnectionService)로 전달합니다.
            // 서비스 내부의 딕셔너리에서 JobID를 찾아 해당 Task를 완료시킵니다.
            bool handled = _connectionService.HandleResult(result);

            if (!handled)
            {
                // 만약 대기 중인 Task가 없다면(이미 타임아웃 되었거나 등) 로그를 남깁니다.
                System.Diagnostics.Debug.WriteLine($"[Warning] 수신된 JobID({result.JobID})에 매칭되는 대기 요청이 없습니다.");
            }
        }

        public void ResultReceiver(CommandResultItem vResult)
        {
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


