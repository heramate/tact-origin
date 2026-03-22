using DevComponents.DotNetBar;
using RACTClient.Models;
using RACTClient.Services;
using RACTClient.Utilities;
using RACTClient.Utilities.Extensions;
using RACTCommonClass;
using RACTTerminal;
using Rebex.Net;
using Rebex.TerminalEmulation;
using System;
using System.Collections;
using System.Collections.Concurrent; // BlockingCollection 사용을 위해 추가
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RACTClient
{
    // 커스텀 스크롤 기능을 위한 확장 클래스
    public class MyTerminalControl : TerminalControl
    {
        public void ScrollToRow(int targetRow)
        {
            int currentPos = this.ScreenPosition;
            int rowsToScroll = currentPos - targetRow;

            if (rowsToScroll != 0)
            {
                this.Scroll(rowsToScroll);
            }
        }

        public void SelectAll()
        {
            // 히스토리 시작(-HistoryLength)부터 화면 끝(Screen.Rows)까지 선택
            this.SetSelection(0, -this.HistoryLength, this.Screen.Columns, this.Screen.Rows);
        }
    }

    public partial class TerminalView : UserControl, ITactTerminal, ISenderObject
    {
        #region Fields & Properties

        private MyTerminalControl _terminal;
        private ContextMenuStrip _contextMenu;
        private readonly SynchronizationContext _uiContext;

        // 상태 및 연결 정보
        private E_TerminalStatus _terminalStatus = E_TerminalStatus.Disconnected;
        private bool _isConnected = false;
        private bool _isQuickConnection = false;
        private bool _isBatchCmdRunning = false;
        private E_TerminalMode _terminalMode;
        private bool _isBastionManagedSession = false;
        private ConnectionTypes _connectionType;
        private DeviceInfo _deviceInfo;
        private SessionContext _context;
        private FACT_DefaultConnectionCommandSet _connectionCommandSet;
        private ScriptGenerator _scriptGenerator = new ScriptGenerator();

        public SessionContext Context { get => _context; private set => _context = value; }
        public bool IsConnected => _isConnected;
        public bool IsQuickConnection => _isQuickConnection;
        public E_TerminalMode TerminalMode { get => _terminalMode; set => _terminalMode = value; }
        public string ToolTip
        {
            get
            {
                if (_deviceInfo == null || _deviceInfo.TerminalConnectInfo == null) return string.Empty;

                if (string.IsNullOrEmpty(_deviceInfo.TpoName))
                {
                    if (_deviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET ||
                        _deviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
                    {
                        return _deviceInfo.IPAddress?.Trim() ?? string.Empty;
                    }

                    if (_deviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SERIAL_PORT)
                    {
                        return "Serial-" + _deviceInfo.TerminalConnectInfo.SerialConfig.PortName;
                    }

                    return string.Empty;
                }

                if (AppGlobal.s_ClientOption.TerminalDisplayNameType == E_TerminalDisplayNameType.IPAddress)
                {
                    return "[" + (_deviceInfo.Name?.Trim() ?? string.Empty) + "]";
                }

                return "[" + (_deviceInfo.IPAddress?.Trim() ?? string.Empty) + "]";
            }
        }
        public string ComPort => throw new NotImplementedException();
        private int _connectedSessionID = 0;
        public int ConnectedSessionID => _connectedSessionID;

        public ConnectionTypes ConnectionType
        {
            get => _connectionType;
            set => _connectionType = value;
        }

        // 테마 및 UI 요소
        public Color BackgroundColor { get; private set; } = Color.Black;
        public Color ForegroundColor { get; private set; } = Color.Gainsboro;

        private Panel _searchPanel;
        private TextBox _txtSearch;
        private Label _lblLogStatus;
        private int _lastFoundRow = int.MinValue;
        private int _lastFoundCol = 0;

        // 검색 및 로깅 관련
        private FileStream _logStream;
        private bool _isLogging = false;
        private StringBuilder _commandBuffer = new StringBuilder();
        private static readonly string[] _forbiddenCommands = { "rm -rf", "reboot", "shutdown", "poweroff", "init 0", "init 6" };

        // DotNetBar UI Components
        private DevComponents.DotNetBar.ContextMenuBar contextMenuBar1;
        private DevComponents.DotNetBar.ButtonItem cmPopUP;
        private DevComponents.DotNetBar.ButtonItem mnuCopy;
        private DevComponents.DotNetBar.ButtonItem mnuPaste;
        private DevComponents.DotNetBar.ButtonItem mnuPasteE;
        private DevComponents.DotNetBar.ButtonItem mnuAutoC;
        private DevComponents.DotNetBar.ButtonItem mnuFind;
        private DevComponents.DotNetBar.ButtonItem mnuSelectAll;
        private DevComponents.DotNetBar.ButtonItem mnuClear;
        private DevComponents.DotNetBar.ButtonItem mnuSearchDefaultCmd;
        private DevComponents.DotNetBar.ButtonItem mnuBatchCmd;
        private DevComponents.DotNetBar.ButtonItem mnuStopScript;
        private DevComponents.DotNetBar.ButtonItem mnuSaveTerminal;
        private DevComponents.DotNetBar.ButtonItem mnuCmdClear;
        private DevComponents.DotNetBar.ButtonItem mnuOption;

        // 1. 필드 선언 (이미 있다면 선언부 확인)
        private readonly TerminalConnectionService _connectionService;
        private readonly ITerminalAutoResponder _autoResponder;
        private CancellationTokenSource _scriptCts;
        private string _lastRecordedWaitText = "";
        //clog 분리
        // 로그 엔트리 구조체: 데이터와 당시의 상태 캡처 
        private struct LogEntry
        {
            public string Content;
            public string TabName;
            public string SessionTime;
            public bool IsCommand;
            public bool WriteClog;
            public bool WriteNormalLog;
        }
        private string _sessionStartTimeStamp = string.Empty;
        private BlockingCollection<LogEntry> _logQueue = new BlockingCollection<LogEntry>();
        private Task _logWriterTask;
        private CancellationTokenSource _logCts;

        #endregion

        #region Constructors

        public TerminalView() : this(false) { }

        public TerminalView(bool quickConnection)
        {
            _isQuickConnection = quickConnection;
            _uiContext = SynchronizationContext.Current ?? new WindowsFormsSynchronizationContext();
            _connectionService = new TerminalConnectionService();

            _autoResponder = new TerminalAutoResponder();
            // 기본 규칙 추가 (필요 시 DB 또는 설정을 통해 로드하도록 확장 가능)
            _autoResponder.AddRule(new AutoResponseRule("--More--", " ", false, "Standard More Prompt"));

            InitializeComponent();
            InitializeSearchPanel();
            InitializeContextMenu();
            InitializeTerminalEvents();
            MCSmallTerminal.OnSendCommandToTerminalEvent += new HandlerArgument2<List<string>, string>(AppGlobal_OnSendCommandToTerminalEvent);
        }

        #endregion

        #region ITactTerminal & Connection Management

        public async void AttachSession(SessionContext context)
        {
            if (Context != null)
            {
                await CloseSessionAsync();
            }

            Context = context;
            ClearTerminal();

            TerminalStatus = E_TerminalStatus.TryConnection;

            if (context.TerminalSession is Ssh ssh)
            {
                _terminal.Bind(ssh);
            }
            else if (context.TerminalSession is Telnet telnet)
            {
                _terminal.Bind(telnet);
            }
            else if (context.TerminalSession is SerialPortChannel serial)
            {
                _terminal.Bind(serial);
            }

            _isConnected = true;
            _connectedSessionID = context.SessionID;
            TerminalStatus = E_TerminalStatus.Connection;

            // 접속 성공 시 자동으로 로그 기록 시작 (옵션 관계없이 상시 저장)
            StartTerminalLog(_deviceInfo);

            ChangeStatusIcon();

            _terminal.Disconnected += OnSessionDisconnected;
            _terminal.Focus();
        }

        public void CloseSession()
        {
            _isConnected = false;

            if (Context != null)
            {
                if (_isBastionManagedSession)
                {
                    Task.Run(async () => await BastionManager.Instance.ReleaseSessionAsync(Context)).Wait();
                }
                else
                {
                    Context.Dispose();
                }

                Context = null;
                _isBastionManagedSession = false;
            }
        }

        private async void OnSessionDisconnected(object sender, EventArgs e)
        {
            await CloseSessionAsync();
            TerminalStatus = E_TerminalStatus.Disconnected;

            await _uiContext.PostAsync(() =>
            {
                if (this.Parent is TabPage tab)
                    tab.Text += " (Disconnected)";
            });

        }

        private void FinalizeConnectionLog(int logIdToClose, int sessionIdToClose)
        {
            DeviceConnectionLogCloseResultInfo tCloseResult = RequestCloseDeviceConnectionLog(logIdToClose, sessionIdToClose);
            if (tCloseResult != null && !tCloseResult.Success)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, tCloseResult.ErrorMessage);
            }
        }

        public async Task<bool> ConnectDeviceAsync(DeviceInfo info)
        {
            _deviceInfo = info;
            int connectionMode = AppGlobal.s_ConnectionMode;

            //2023-06-13 VOIP AGW PORT 2001 치환 
            if (_deviceInfo.DevicePartCode == 13)
            {
                if (_deviceInfo.ModelID != 3727)
                    _deviceInfo.TerminalConnectInfo.TelnetPort = 2001;
            }


            try
            {
                TerminalStatus = E_TerminalStatus.TryConnection;
                ClearTerminal();

                TargetInfo target = null;
                SessionContext context = null;

                // ---------------------------------------------------------
                // 1. 엔드포인트 확정 (Endpoint Resolution)
                // ---------------------------------------------------------

                // Case A: Reverse SSH (Mode 3 & Online)
                if (connectionMode == 3 && AppGlobal.s_RACTClientMode == E_RACTClientMode.Online)
                {
                    LogMessage("▶ [Mode 3] SSH 터널링 매핑 정보 조회 중 (RMI)...");
                    if (!await RequestSSHTunnelInfoAsync())
                    {
                        throw new Exception("SSH 터널링 정보 획득 실패. (KamServer/LTE 접속 불가)");
                    }

                    LogMessage($"▶ SSH 터널 매핑 성공: {_deviceInfo.IPAddress}:{_deviceInfo.TerminalConnectInfo.TelnetPort}");
                    target = MapToTargetInfo(_deviceInfo);
                    target.UseBastion = false; // 터널링 보조 데몬을 이미 거쳤으므로 Bastion 중복 사용 지양
                }
                // Case B: 일반 접속 (Console 포함)
                else
                {
                    // Console 모드인 경우 Bastion 비활성화 (Standalone)
                    AppGlobal.s_ClientOption.UseBastion = (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online);
                    target = MapToTargetInfo(info);
                }

                // ---------------------------------------------------------
                // 2. 공통 접속 처리 (Common Connection logic)
                // ---------------------------------------------------------
                if (target == null) return false;                // 2.1 Bastion 경유
                if (target.UseBastion)
                {
                    //LogMessage("▶ Bastion 터널 할당 중...");
                    context = await BastionManager.Instance.CreateTunnelAsync(target);
                    ConnectionType = ConnectionTypes.RemoteTelnet;
                    _isBastionManagedSession = true;
                }
                // 2.2 직접 접속
                else
                {
                    _isBastionManagedSession = false;
                    LogMessage($"▶ 직접 연결 시도 중 ({target.Host}:{target.Port})...");

                    if (target.Protocol == E_ConnectionProtocol.SSHTelnet)
                    {
                        var ssh = new Ssh();
                        ssh.Settings.SshParameters.Compression = true;
                        await ssh.ConnectAsync(target.Host, target.Port);
                        await ssh.LoginAsync(target.Username, target.Password);
                        context = new SessionContext(null, ssh);
                        ConnectionType = ConnectionTypes.LocalTelnet;
                    }
                    else if (target.Protocol == E_ConnectionProtocol.SERIAL_PORT)
                    {
                        LogMessage($"▶ 시리얼 직접 연결 시도 중 ({target.PortName})...");

                        var serial = new SerialPortChannel(
                            target.PortName,
                            target.BaudRate,
                            target.Parity,
                            target.DataBits,
                            target.StopBits);

                        context = new SessionContext(null, serial);
                        ConnectionType = ConnectionTypes.Serial;
                    }
                    else
                    {
                        var telnet = new Telnet(target.Host, target.Port);
                        context = new SessionContext(null, telnet);
                        ConnectionType = ConnectionTypes.LocalTelnet;
                    }
                }

                // ---------------------------------------------------------
                // 3. 세션 바인딩
                // ---------------------------------------------------------
                if (context != null)
                {
                    // 세션 ID 할당 (0인 경우 임의의 값 할당)
                    if (context.SessionID == 0)
                    {
                        context.SessionID = new Random().Next(1, int.MaxValue);
                    }

                    AttachSession(context);
                    TerminalStatus = E_TerminalStatus.Connection;

                    // Wake-up sequence: 세션 바인딩 후 \n 송신 (장비 반응 유도)
                    if (context.TerminalSession is SerialPortChannel)
                    {
                        _terminal.Scripting.Send("\n");
                    }

                    // [설계 반영] Online 모드일 때만 DB 접속 로그 생성 (비동기)
                    if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online)
                    {
                        // 실제 접속에 영향을 주지 않도록 별도 스레드에서 실행
                        _ = Task.Run(() => InitConnectionLog());
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                // InnerException을 포함하여 상세 정보를 구성
                string errorMessage = ex.InnerException != null
                    ? $"Connection failed: {ex.Message} {Environment.NewLine}(Inner Exception: {ex.InnerException.Message})"
                    : $"Connection failed: {ex.Message}";
                errorMessage = $"Connection failed: {ex.Message}";
                LogMessage(errorMessage);
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, $"ConnectDeviceAsync Failed ({info.IPAddress}): {ex}");
                TerminalStatus = E_TerminalStatus.Disconnected;
            }
            return false;
        }

        private bool InitConnectionLog()
        {
            DeviceConnectionLogOpenResultInfo tOpenResult = RequestOpenDeviceConnectionLog(_deviceInfo);
            if (tOpenResult == null || !tOpenResult.Success)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, "접속 로그를 생성하지 못했습니다.");
                return false;
            }

            _connectionLogID = tOpenResult.ConnectionLogID;
            return true;
        }

        int _connectionLogID;

        private DeviceConnectionLogCloseResultInfo RequestCloseDeviceConnectionLog(int logIdToClose, int sessionIdToClose)
        {
            DeviceConnectionLogCloseRequestInfo tRequest = new DeviceConnectionLogCloseRequestInfo();
            tRequest.ClientID = AppGlobal.s_LoginResult.ClientID;
            tRequest.UserID = AppGlobal.s_LoginResult.UserID;
            tRequest.ConnectionLogID = logIdToClose;
            tRequest.SessionID = sessionIdToClose;
            tRequest.DisconnectReason = "ClientDisconnect";

            return AppGlobal.s_DeviceConnectionLogClient.CloseLog(tRequest);
        }


        private TargetInfo MapToTargetInfo(DeviceInfo info)
        {
            var target = new TargetInfo
            {
                Host = info.IPAddress,
                Port = info.TerminalConnectInfo.TelnetPort,
                Protocol = info.TerminalConnectInfo.ConnectionProtocol,
                Username = string.IsNullOrEmpty(info.TerminalConnectInfo.ID) ? info.TelnetID1 : info.TerminalConnectInfo.ID,
                Password = string.IsNullOrEmpty(info.TerminalConnectInfo.Password) ? info.TelnetPwd1 : info.TerminalConnectInfo.Password,
                UseBastion = (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online)
            };

            // 시리얼 설정 매핑
            if (info.TerminalConnectInfo.SerialConfig != null)
            {
                target.PortName = info.TerminalConnectInfo.SerialConfig.PortName;
                target.BaudRate = info.TerminalConnectInfo.SerialConfig.BaudRate;
                target.DataBits = info.TerminalConnectInfo.SerialConfig.DataBits;
                target.Parity = info.TerminalConnectInfo.SerialConfig.Parity;
                target.StopBits = info.TerminalConnectInfo.SerialConfig.StopBits;
                target.Handshake = info.TerminalConnectInfo.SerialConfig.Handshake;
            }

            return target;
        }

        private void AppGlobal_OnSendCommandToTerminalEvent(List<string> targetTerminalNames, string commandText)
        {
            if (!_isConnected || targetTerminalNames == null || !targetTerminalNames.Contains(this.Name))
                return;

            string[] lineDelimiters = { "\r\n", "\r", "\n" };
            string[] individualCommands = commandText.Split(lineDelimiters, StringSplitOptions.RemoveEmptyEntries);

            // [성능 최적화] 연산 및 DB 로깅은 UI를 잠그지 않는 Task에서 수행하고, 터미널 명령 전송만 SafeInvoke 처리
            Task.Run(() =>
            {
                if (Context == null || _terminal.Scripting == null) return;

                foreach (string currentCmd in individualCommands)
                {
                    bool isLimit = IsLimitCmd(currentCmd);

                    if (!isLimit)
                    {
                        this.SafeInvoke(() => 
                        {
                            if (Context == null || _terminal.Scripting == null) return;
                            _terminal.Scripting.Send(currentCmd + "\r");
                        });

                        WriteClog(currentCmd, true); // .clog 명령어 기록 (비동기 큐)
                        SaveCommandLog(currentCmd, false); // DB 처리 (백그라운드 스레드 진행)
                    }
                    else
                    {
                        SaveCommandLog(currentCmd, true);
                        this.SafeInvoke(() => LogMessage($"[Security] Blocked command: {currentCmd}"));
                    }

                    // CPU 스파이크 방지 및 UI 스레드와의 경합 제어를 위한 미세 딜레이
                    Thread.Sleep(3);
                }
            });
        }

        // [성능 최적화] Regex 캐시: ModelID -> 변환된 정규표현식 및 매핑 딕셔너리
        private class LimitCmdCacheItem
        {
            public System.Text.RegularExpressions.Regex CompiledRegex { get; set; }
            public Dictionary<string, EmbagoInfo> RulesMap { get; set; }
        }
        private static System.Collections.Concurrent.ConcurrentDictionary<int, LimitCmdCacheItem> _limitCmdRegexCache = new System.Collections.Concurrent.ConcurrentDictionary<int, LimitCmdCacheItem>();

        public bool IsLimitCmd(string lineCmd)
        {
            // 1. 기본 통과 조건
            if (string.IsNullOrWhiteSpace(lineCmd)) return false;
            if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Console) return false;
            if (AppGlobal.s_LoginResult?.UserInfo == null || !AppGlobal.s_LoginResult.UserInfo.LimitedCmdUser) return false;

            // 2. 명령어 정규화 (대소문자 무시, 연속된 공백을 하나로)
            string processedCmd = lineCmd.ToSingleSpace().Trim();

            // 3. DB 기반 금지어 리스트 확인
            if (_deviceInfo != null)
            {
                LimitCmdInfo limitCmdInfo = null;
                if (AppGlobal.s_LimitCmdInfoList.Contains(_deviceInfo.ModelID))
                {
                    limitCmdInfo = AppGlobal.s_LimitCmdInfoList[_deviceInfo.ModelID];
                }

                if (limitCmdInfo != null && limitCmdInfo.EmbagoCmd != null && limitCmdInfo.EmbagoCmd.Count > 0)
                {
                    // Regex 캐시 구성 (초기 1회만 생성, 이후 O(1) 정규식 매칭)
                    LimitCmdCacheItem cacheItem = _limitCmdRegexCache.GetOrAdd(_deviceInfo.ModelID, (modelId) =>
                    {
                        var item = new LimitCmdCacheItem
                        {
                            RulesMap = new Dictionary<string, EmbagoInfo>(StringComparer.OrdinalIgnoreCase)
                        };
                        List<string> escapePatterns = new List<string>();

                        foreach (EmbagoInfo info in limitCmdInfo.EmbagoCmd)
                        {
                            string embargoTrigger = info.Embargo?.Trim();
                            if (!string.IsNullOrEmpty(embargoTrigger))
                            {
                                item.RulesMap[embargoTrigger] = info;
                                escapePatterns.Add(System.Text.RegularExpressions.Regex.Escape(embargoTrigger));
                            }
                        }

                        if (escapePatterns.Count > 0)
                        {
                            string combinedPattern = string.Join("|", escapePatterns);
                            item.CompiledRegex = new System.Text.RegularExpressions.Regex(combinedPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Compiled);
                        }
                        return item;
                    });

                    // 정규식 매칭 여부 스캔
                    if (cacheItem.CompiledRegex != null)
                    {
                        System.Text.RegularExpressions.Match match = cacheItem.CompiledRegex.Match(processedCmd);
                        if (match.Success)
                        {
                            // 매칭된 정확한 금지어 식별
                            string matchedText = match.Value;
                            if (cacheItem.RulesMap.TryGetValue(matchedText, out EmbagoInfo targetEmbago))
                            {
                                // [A] 즉시 차단 모드 (EmbargoEnble == true)
                                if (targetEmbago.EmbargoEnble)
                                {
                                    // 크로스 스레드 데드락 방지를 위해 ShowMessage (Invoke 자동 처리 함수) 사용
                                    AppGlobal.ShowMessage(AppGlobal.s_ClientMainForm,
                                        $"\"{targetEmbago.Embargo}\"는 금지된 명령어입니다.\n해당 계정은 이 명령어를 사용할 수 없습니다.",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return true; // 차단됨
                                }
                                // [B] 경고 후 선택 모드 (EmbargoEnble == false)
                                else
                                {
                                    if (AppGlobal.ShowMessage(AppGlobal.s_ClientMainForm,
                                        $"\"{targetEmbago.Embargo}\"는 제한된 명령어입니다.\n실행하시겠습니까?",
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                                    {
                                        return true; // 차단으로 간주
                                    }
                                }
                            }
                            else
                            {
                                return true; // 안전을 위해 폴백 차단
                            }
                        }
                    }
                }
            }

            return false; // 통과
        }

        private void InitializeComponent()
        {
            _terminal = new MyTerminalControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 10),
                HistoryMaxLength = 5000,
                MousePasteEnabled = false,
                AutoAdjustTerminalSize = true
            };

            _terminal.DataReceived += _terminal_DataReceived;
            _terminal.KeyDown += _terminal_KeyDown;
            this.Controls.Add(_terminal);

            _lblLogStatus = new Label
            {
                Text = "● REC",
                ForeColor = Color.Red,
                BackColor = Color.Transparent,
                Font = new Font("Arial", 8, FontStyle.Bold),
                Visible = false,
                Parent = this
            };

            _lblLogStatus.BringToFront();
            this.Size = new Size(800, 600);
        }

        public void ApplyTheme(Font font, Color bg, Color fg)
        {
            BackgroundColor = bg;
            ForegroundColor = fg;

            _terminal.Font = font;

            TerminalPalette palette = new TerminalPalette();
            palette.SetColor(TerminalColor.LightCyan, bg);
            palette.SetColor(TerminalColor.White, fg);
            palette.SetColor(TerminalColor.LightGray, fg);

            _terminal.Palette = palette;
            _terminal.Options.ColorScheme = Rebex.TerminalEmulation.ColorScheme.Custom;
            // _terminal.Options.SetColorIndex(SchemeColorName.Background, TerminalColor.Black);
            _terminal.Options.SetColorIndex(SchemeColorName.Background, TerminalColor.LightCyan);
            _terminal.Options.SetColorIndex(SchemeColorName.Foreground, TerminalColor.White);

            _terminal.CursorBlinkingInterval = 500;

            _terminal.Refresh();
        }

        private void InitializeSearchPanel()
        {
            _searchPanel = new Panel { Dock = DockStyle.Top, Height = 35, BackColor = Color.LightGray, Visible = false };

            var lblFind = new Label { Text = "Find:", Location = new Point(10, 10), AutoSize = true };
            _txtSearch = new TextBox { Location = new Point(45, 7), Width = 150 };
            _txtSearch.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (e.Shift) FindPrev(_txtSearch.Text);
                    else FindNext(_txtSearch.Text);
                }
            };

            var btnPrev = new Button { Text = "Prev", Location = new Point(205, 5), Width = 50 };
            btnPrev.Click += (s, e) => FindPrev(_txtSearch.Text);

            var btnNext = new Button { Text = "Next", Location = new Point(260, 5), Width = 50 };
            btnNext.Click += (s, e) => FindNext(_txtSearch.Text);

            var btnClose = new Button { Text = "X", Location = new Point(315, 5), Width = 30 };
            btnClose.Click += (s, e) => ToggleSearchBar(false);

            _searchPanel.Controls.Add(lblFind);
            _searchPanel.Controls.Add(_txtSearch);
            _searchPanel.Controls.Add(btnPrev);
            _searchPanel.Controls.Add(btnNext);
            _searchPanel.Controls.Add(btnClose);

            this.Controls.Add(_searchPanel);
            _searchPanel.BringToFront();
        }

        private void InitializeTerminalEvents()
        {
            _terminal.ImeMode = ImeMode.Disable;

            // 자동 메뉴 팝업을 막기 위해 null로 설정하거나 이벤트를 직접 핸들링합니다.
            _terminal.ContextMenuStrip = null;
            _terminal.MouseClick += _terminal_MouseClick;
            _terminal.Enter += (s, e) => _terminal.ImeMode = ImeMode.Disable;
        }

        private void _terminal_MouseClick(object sender, MouseEventArgs e)
        {
            // 우클릭이 아니면 무시
            if (e.Button != MouseButtons.Right) return;

            var popupType = AppGlobal.s_ClientOption.TerminalPopupType;

            switch (popupType)
            {
                case E_TerminalPopupType.CopyPaste:
                    // [CopyPaste 모드]: 메뉴를 띄우지 않고 즉시 실행
                    HandleSmartCopyPaste();
                    break;

                case E_TerminalPopupType.None:
                    // [None 모드]: 설정 창 표시
                    using (var configForm = new TerminalPopupType())
                    {
                        configForm.ShowDialog(this);
                    }
                    break;

                default:
                    // [Normal 또는 기타]: 전체 메뉴 표시
                    // 커서 위치(Cursor.Position)에 메뉴를 띄웁니다.
                    //_contextMenu.Show(Cursor.Position);
                    cmPopUP.Popup(MousePosition);
                    break;
            }
        }

        private void HandleSmartCopyPaste()
        {
            string selectedText = _terminal.GetSelectedText();

            if (!string.IsNullOrEmpty(selectedText))
            {
                // 텍스트가 선택되어 있다면 -> 복사
                Clipboard.SetText(selectedText);
            }
            else
            {
                // 선택된 게 없다면 -> 붙여넣기
                PasteText();
            }
        }

        #endregion

        #region Search & Find Logic

        public void ToggleSearchBar(bool show)
        {
            _searchPanel.Visible = show;
            if (show)
            {
                _txtSearch.Focus();
                _txtSearch.SelectAll();
            }
            else
            {
                _terminal.Focus();
            }
        }

        public void FindNext(string text)
        {
            if (string.IsNullOrEmpty(text)) return;

            int historyLen = _terminal.HistoryLength;
            int screenRows = _terminal.Screen.Rows;
            int cols = _terminal.Screen.Columns;

            int totalStartRow = -historyLen;
            int totalEndRow = screenRows;

            int startRow = (_lastFoundRow == int.MinValue) ? totalStartRow : _lastFoundRow;
            int startCol = (_lastFoundRow == int.MinValue) ? 0 : _lastFoundCol + 1;

            if (startCol >= cols)
            {
                startCol = 0;
                startRow++;
            }

            bool found = false;

            if (SearchForward(text, startRow, startCol, totalEndRow, cols, out int foundRow, out int foundCol))
            {
                HighlightAndScroll(foundRow, foundCol, text.Length);
                found = true;
            }
            else if (SearchForward(text, totalStartRow, 0, startRow + 1, cols, out foundRow, out foundCol))
            {
                HighlightAndScroll(foundRow, foundCol, text.Length);
                found = true;
            }

            if (!found)
            {
                MessageBox.Show($"Text '{text}' not found (Forward).", "Find", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _lastFoundRow = int.MinValue;
                _lastFoundCol = 0;
            }
        }

        public void FindPrev(string text)
        {
            if (string.IsNullOrEmpty(text)) return;

            int historyLen = _terminal.HistoryLength;
            int screenRows = _terminal.Screen.Rows;
            int cols = _terminal.Screen.Columns;

            int totalStartRow = -historyLen;
            int totalEndRow = screenRows;

            int startRow = (_lastFoundRow == int.MinValue) ? totalEndRow - 1 : _lastFoundRow;
            int startCol = (_lastFoundRow == int.MinValue) ? cols - 1 : _lastFoundCol - 1;

            if (startCol < 0)
            {
                startCol = cols - 1;
                startRow--;
            }

            bool found = false;

            if (SearchBackward(text, startRow, startCol, totalStartRow, cols, out int foundRow, out int foundCol))
            {
                HighlightAndScroll(foundRow, foundCol, text.Length);
                found = true;
            }
            else if (SearchBackward(text, totalEndRow - 1, cols - 1, startRow - 1, cols, out foundRow, out foundCol))
            {
                HighlightAndScroll(foundRow, foundCol, text.Length);
                found = true;
            }

            if (!found)
            {
                MessageBox.Show($"Text '{text}' not found (Backward).", "Find", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _lastFoundRow = int.MinValue;
                _lastFoundCol = 0;
            }
        }

        private bool SearchForward(string text, int startRow, int startCol, int endRow, int cols, out int foundRow, out int foundCol)
        {
            foundRow = -1;
            foundCol = -1;
            const int chunkSize = 100; // [성능 최적화] 텍스트를 청크 단위로 가져와 RPC 호출 횟수 감소

            for (int r = startRow; r < endRow; r += chunkSize)
            {
                int currentChunkSize = Math.Min(chunkSize, endRow - r);
                string[] lines = _terminal.Screen.GetRegionText(0, r, cols, currentChunkSize);
                if (lines == null) continue;

                for (int i = 0; i < lines.Length; i++)
                {
                    int currentRow = r + i;
                    string lineText = lines[i];
                    int currentStartCol = (currentRow == startRow) ? startCol : 0;

                    if (currentStartCol >= lineText.Length) continue;

                    int c = lineText.IndexOf(text, currentStartCol, StringComparison.OrdinalIgnoreCase);

                    if (c >= 0)
                    {
                        foundRow = currentRow;
                        foundCol = c;
                        return true;
                    }
                }
            }
            return false;
        }

        private bool SearchBackward(string text, int startRow, int startCol, int endRow, int cols, out int foundRow, out int foundCol)
        {
            foundRow = -1;
            foundCol = -1;
            const int chunkSize = 100; // [성능 최적화] 역방향 검색도 청크 단위 적용

            for (int r = startRow; r >= endRow; r -= chunkSize)
            {
                int chunkStartRow = Math.Max(endRow, r - chunkSize + 1);
                int currentChunkSize = r - chunkStartRow + 1;

                string[] lines = _terminal.Screen.GetRegionText(0, chunkStartRow, cols, currentChunkSize);
                if (lines == null) continue;

                // 역방향 검색이므로 청크 내에서도 뒤에서부터 검사
                for (int i = lines.Length - 1; i >= 0; i--)
                {
                    int currentRow = chunkStartRow + i;
                    string lineText = lines[i];

                    int c;
                    if (currentRow == startRow)
                    {
                        int limit = Math.Min(startCol, lineText.Length - 1);
                        if (limit < 0) continue;
                        c = lineText.LastIndexOf(text, limit, StringComparison.OrdinalIgnoreCase);
                    }
                    else
                    {
                        c = lineText.LastIndexOf(text, StringComparison.OrdinalIgnoreCase);
                    }

                    if (c >= 0)
                    {
                        foundRow = currentRow;
                        foundCol = c;
                        return true;
                    }
                }
            }
            return false;
        }

        private void HighlightAndScroll(int row, int col, int length)
        {
            _terminal.SetSelection(col, row, col + length, row);

            int screenPos = _terminal.ScreenPosition;
            int screenRows = _terminal.Screen.Rows;

            if (row < screenPos || row >= screenPos + screenRows)
            {
                _terminal.ScrollToRow(row);
            }

            _lastFoundRow = row;
            _lastFoundCol = col;
            _terminal.Focus();
        }

        #endregion


        #region
        private void InitializeContextMenu()
        {
            /*
            _contextMenu = new ContextMenuStrip();
            _contextMenu.Items.Add("Copy", null, (s, e) =>
            {
                string selected = _terminal.GetSelectedText();
                if (!string.IsNullOrEmpty(selected))
                {
                    Clipboard.SetText(selected);
                }
            });
            _contextMenu.Items.Add("Paste", null, (s, e) => PasteText());
            _contextMenu.Items.Add("Select All", null, (s, e) => _terminal.SelectAll());
            _contextMenu.Items.Add(new ToolStripSeparator());
            _contextMenu.Items.Add("Clear Input", null, (s, e) => ClearInput());
            _contextMenu.Items.Add(new ToolStripSeparator());
            _contextMenu.Items.Add("Find...", null, (s, e) => ToggleSearchBar(true));
            _contextMenu.Items.Add("Settings...", null, (s, e) => ShowSettings());
            var itemLog = new ToolStripMenuItem("Start Logging...");
            itemLog.Click += (s, e) => ToggleLogging();
            _contextMenu.Items.Add(itemLog);
            _contextMenu.Opening += (s, e) => itemLog.Text = _isLogging ? "Stop Logging" : "Start Logging...";
            _terminal.ContextMenuStrip = _contextMenu;
            */

            MakeContextMenu();
        }

        private void MakeContextMenu()
        {
            this.contextMenuBar1 = new DevComponents.DotNetBar.ContextMenuBar();
            this.cmPopUP = new DevComponents.DotNetBar.ButtonItem();

            // 메뉴 객체들 초기화
            this.mnuCopy = new ButtonItem { Name = "mnuCopy", Text = "복사(&Y)" };
            this.mnuPaste = new ButtonItem { Name = "mnuPaste", Text = "붙여넣기(&P)" };
            this.mnuPasteE = new ButtonItem { Name = "mnuPasteE", Text = "<CR>붙여넣기(&B)" };
            this.mnuAutoC = new ButtonItem { Name = "mnuAutoC", Text = "자동완성(F2)" };
            this.mnuFind = new ButtonItem { Name = "mnuFind", Text = "찾기(&F)", BeginGroup = true };
            this.mnuSelectAll = new ButtonItem { Name = "mnuSelectAll", Text = "모두선택(&A)" };
            this.mnuClear = new ButtonItem { Name = "mnuClear", Text = "화면지움(&R)", BeginGroup = true };
            this.mnuCmdClear = new ButtonItem { Name = "mnuCmdClear", Text = "입력 명령 지움(&U)", BeginGroup = true };
            this.mnuSearchDefaultCmd = new ButtonItem { Name = "mnuSearchDefaultCmd", Text = "기본 명령 조회 (F1)", BeginGroup = true };
            this.mnuBatchCmd = new ButtonItem { Name = "mnuBatchCmd", Text = "일괄 명령실행", BeginGroup = true };

            // [핵심] 스크립트 취소 메뉴 (요청 사항)
            this.mnuStopScript = new ButtonItem
            {
                Name = "mnuStopScript",
                Text = "스크립트 취소",
                BeginGroup = true,
                ImageSmall = (Image)global::RACTClient.Properties.Resources.run_cancel
            };

            this.mnuSaveTerminal = new ButtonItem { Name = "mnuSaveTerminal", Text = "결과저장", BeginGroup = true };
            this.mnuOption = new ButtonItem { Name = "mnuOption", Text = "옵션", BeginGroup = true };

            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).BeginInit();
            this.SuspendLayout();

            // ContextMenuBar 설정
            this.contextMenuBar1.AntiAlias = true;
            this.contextMenuBar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] { this.cmPopUP });
            this.contextMenuBar1.Location = new System.Drawing.Point(64, 27);
            this.contextMenuBar1.Name = "contextMenuBar1";
            this.contextMenuBar1.Size = new System.Drawing.Size(75, 25);
            this.contextMenuBar1.Stretch = true;
            this.contextMenuBar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;

            // 메인 팝업 설정 및 서브 아이템 구성
            this.cmPopUP.AutoExpandOnClick = true;
            this.cmPopUP.Text = "TerminalMenu";

            var subItems = new List<BaseItem> {
        mnuCopy, mnuPaste, mnuPasteE, mnuAutoC, mnuFind, mnuSelectAll,
        mnuClear, mnuCmdClear, mnuSearchDefaultCmd, mnuBatchCmd,
        mnuStopScript, mnuSaveTerminal
    };

            if (TerminalMode == E_TerminalMode.QuickClient)
            {
                subItems.Add(mnuOption);
            }

            this.cmPopUP.SubItems.AddRange(subItems.ToArray());

            // 단축키 및 리소스/이벤트 연결 (기존 로직 적용)
            SetupMenuDetails();

            this.Controls.Add(this.contextMenuBar1);
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).EndInit();
            this.ResumeLayout(false);
        }

        private void SetupMenuDetails()
        {
            // 복사
            this.mnuCopy.Shortcuts.Add(eShortcut.CtrlY);
            this.mnuCopy.ImageSmall = (Image)global::RACTClient.Properties.Resources.copy;
            this.mnuCopy.Click += mnuCopy_Click_Event;

            // 붙여넣기
            this.mnuPaste.Shortcuts.Add(eShortcut.CtrlP);
            this.mnuPaste.ImageSmall = (Image)global::RACTClient.Properties.Resources.paste;
            this.mnuPaste.Click += mnuPaste_Click_Event;

            // 기타 설정
            this.mnuPasteE.Shortcuts.Add(eShortcut.CtrlB);
            this.mnuPasteE.Click += mnuPasteCR_Click_Event;
            this.mnuAutoC.Shortcuts.Add(eShortcut.F2);
            this.mnuAutoC.Click += mnuAutoC_Click_Event;
            this.mnuFind.Shortcuts.Add(eShortcut.CtrlF);
            this.mnuFind.Click += mnuFind_Click_Event;
            this.mnuSelectAll.Shortcuts.Add(eShortcut.CtrlA);
            this.mnuSelectAll.Click += mnuSelectAll_Click_Event;
            this.mnuClear.Shortcuts.Add(eShortcut.CtrlR);
            this.mnuClear.Click += mnuClear_Click_Event;
            this.mnuCmdClear.Shortcuts.Add(eShortcut.CtrlU);
            this.mnuCmdClear.Click += mnuCmdClear_Click_Event;
            this.mnuSearchDefaultCmd.Shortcuts.Add(eShortcut.F1);
            this.mnuSearchDefaultCmd.Click += mnuSearchDefaultCmd_Click_Event;
            this.mnuBatchCmd.Click += mnuBatchCmd_Click;

            // 스크립트 취소 이벤트
            this.mnuStopScript.Click += mnuStopScript_Click;

            this.mnuSaveTerminal.Click += mnuSaveTerminal_Click;
            this.mnuOption.Click += mnuOption_Click;
        }

        private void mnuOption_Click(object sender, EventArgs e)
        {
            CallOptionHandlerEvent?.Invoke();
        }

        private void mnuSaveTerminal_Click(object sender, EventArgs e)
        {
            WriteTerminalLog();
        }

        private void mnuStopScript_Click(object sender, EventArgs e)
        {
            // 1. 녹화 중인 경우 취소 처리
            if (_terminalStatus == E_TerminalStatus.Recording)
            {
                ScriptWork(E_ScriptWorkType.RecCancel);
                this.LogInfo("스크립트 녹화가 취소되었습니다.");
                return;
            }

            // 2. 실행 중인 비동기 스크립트 중단
            if (_scriptCts != null)
            {
                _scriptCts.Cancel();
                // TerminalStatus 복구 및 로깅은 RunScriptAsync 의 finally/catch에서 처리됨
            }
        }

        private void mnuBatchCmd_Click(object sender, EventArgs e)
        {
            BatchCmdForm tForm = new BatchCmdForm(this);
            tForm.StartPosition = FormStartPosition.CenterParent;
            tForm.SendBatchCommandFunction += new BatchCmdForm.SendBatchCommand(RunBatchCommnad);
            tForm.ShowDialog(this.ParentForm);
        }

        public void RunBatchCommnad(string batchCmd, decimal cycleTime)
        {
            if (_isBatchCmdRunning) return;

            Task.Run(async () =>
            {
                try
                {
                    _isBatchCmdRunning = true;
                    string filteredBatch = string.Empty;

                    var lines = batchCmd.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    List<string> safeCommands = new List<string>();

                    // 1. 순수 연산 & 로깅 구간 (UI 스레드 독립, RunSync 외부에서 수행)
                    foreach (var line in lines)
                    {
                        if (IsLimitCmd(line))
                        {
                            this.SafeInvoke(() => LogWarning($"[Batch Skip] 금지 명령어가 포함되어 실행을 건너뜁니다: {line}"));
                            continue;
                        }
                        safeCommands.Add(line);
                        WriteClog(line, true);
                        SaveCommandLog(line, false);
                    }

                    if (safeCommands.Count == 0) return;
                    filteredBatch = string.Join("\r\n", safeCommands);

                    // 2. 터미널 상태 제어 및 아이콘 변경 (짧은 UI Sync)
                    this.RunSync(() =>
                    {
                        if (this.IsDisposed) return;
                        _terminal.SetDataProcessingMode(DataProcessingMode.None);
                        _terminal.UserInputEnabled = false;
                        TerminalStatus = E_TerminalStatus.RunScript;
                        try { ChangeStatusIcon(); } catch { }
                        try { mnuStopScript.Enabled = true; } catch { }
                    });

                    // 3. 엔진 배치 커맨드 실행
                    int timeoutMs = (cycleTime > 0) ? (int)(cycleTime * 1000) : 10000;
                    _terminal.Scripting.ExecuteBatch(filteredBatch, null, (msg) => this.SafeInvoke(() => LogInfo(msg)), timeoutMs);
                    this.SafeInvoke(() => LogInfo("배치 명령 실행 완료."));
                }
                catch (Exception ex)
                {
                    this.SafeInvoke(() => LogError("Batch Execution Failed: " + ex.Message));
                }
                finally
                {
                    _isBatchCmdRunning = false;
                    // Restore UI state
                    this.RunSync(() =>
                    {
                        if (this.IsDisposed) return;
                        _terminal.SetDataProcessingMode(DataProcessingMode.Automatic);
                        _terminal.UserInputEnabled = true;

                        try { mnuStopScript.Enabled = false; } catch { }

                        TerminalStatus = IsConnected ? E_TerminalStatus.Connection : E_TerminalStatus.Disconnected;
                        try { ChangeStatusIcon(); } catch { }
                    });
                }
            });
        }

        public string IsLimitCmdByBatch(string lineCmd)
        {
            // 기존의 중복 로직을 제거하고 IsLimitCmd를 호출
            // true가 반환되면 차단된 것이므로, 에러 메시지 처리를 위해 명령어 이름을 반환하거나 빈 값을 반환
            if (IsLimitCmd(lineCmd))
            {
                return "Blocked"; // 구체적인 명령어 매칭이 필요하면 IsLimitCmd 내부에서 out 변수를 쓰도록 확장 가능
            }
            return string.Empty;
        }

        private void mnuSearchDefaultCmd_Click_Event(object sender, EventArgs e)
        {
            if (TerminalMode == E_TerminalMode.QuickClient)
                mnuSearchDefaultCmd_Click(sender, e);
            else
                AppGlobal.terminalPanel1.ExecTerminalScreen(E_TerminalScreenTextEditType.SearchCmd);
            //ShowNotImplementedMessage("기본 명령 조회");

        }

        private void mnuSearchDefaultCmd_Click(object sender, EventArgs e)
        {
            using (var tForm = new SearchDefaultCmdForm())
            {
                tForm.txtSearch.Text = GetCmd().TrimStart();
                tForm.modelID = _deviceInfo?.ModelID ?? 0;
                tForm.SendCmd += (cmd) => SetAutoCompleteCmd(cmd);
                tForm.StartPosition = FormStartPosition.CenterParent;
                tForm.ShowDialog(this.ParentForm);
            }
        }

        private void mnuCmdClear_Click_Event(object sender, EventArgs e)
        {
            //ShowNotImplementedMessage("입력 명령 지움");
            if (TerminalMode == E_TerminalMode.QuickClient)
                mnuCmdClear_Click(sender, e);
            else
                AppGlobal.terminalPanel1.ExecTerminalScreen(E_TerminalScreenTextEditType.CmdClear);
        }

        private void mnuCmdClear_Click(object sender, EventArgs e)
        {
            // 1. 공통 입력 지우기 함수 호출
            ClearInput();

            // 2. UI 로그 기록 (선택 사항)
            LogInfo("입력 명령 줄을 초기화했습니다.");
        }

        private void mnuClear_Click_Event(object sender, EventArgs e)
        {
            //ShowNotImplementedMessage("화면 지움");
            if (TerminalMode == E_TerminalMode.QuickClient)
                mnuClear_Click(sender, e);
            else
                AppGlobal.terminalPanel1.ExecTerminalScreen(E_TerminalScreenTextEditType.Clear);
        }

        private void mnuClear_Click(object sender, EventArgs e)
        {
            ClearTerminal();
        }

        private void mnuSelectAll_Click_Event(object sender, EventArgs e)
        {
            if (TerminalMode == E_TerminalMode.QuickClient)
                mnuSelectAll_Click(sender, e);
            else
                AppGlobal.terminalPanel1.ExecTerminalScreen(E_TerminalScreenTextEditType.SelectAll);

        }

        private void mnuSelectAll_Click(object sender, EventArgs e)
        {
            // ShowNotImplementedMessage("모두 선택"); // 필요 시 주석 해제
            _terminal.SelectAll();
        }

        private void mnuFind_Click_Event(object sender, EventArgs e)
        {
            if (TerminalMode == E_TerminalMode.QuickClient)
                mnuFind_Click(sender, e);
            else
                AppGlobal.terminalPanel1.ExecTerminalScreen(E_TerminalScreenTextEditType.Find);

        }

        private void mnuFind_Click(object sender, EventArgs e)
        {
            ToggleSearchBar(true);
        }

        private void mnuAutoC_Click_Event(object sender, EventArgs e)
        {
            if (TerminalMode == E_TerminalMode.QuickClient)
                mnuAutoC_Click(sender, e);
            else
                AppGlobal.terminalPanel1.ExecTerminalScreen(E_TerminalScreenTextEditType.AutoC);


        }

        private void mnuAutoC_Click(object sender, EventArgs e)
        {
            string lineCmd = GetCmd();
            if (string.IsNullOrEmpty(lineCmd)) return;

            var autoCForm = new RACTClient.SubForm.AutoCompleteKey
            {
                keyText = lineCmd,
                modelID = _deviceInfo?.ModelID ?? 0,
                StartPosition = FormStartPosition.Manual
            };

            // 커서 위치 계산 (Rebex 좌표 활용)
            int x = _terminal.Screen.CursorLeft * (int)Math.Ceiling(_terminal.Font.Size);
            int y = (_terminal.Screen.CursorTop + 1) * _terminal.Font.Height;
            Point screenPoint = _terminal.PointToScreen(new Point(x, y));
            autoCForm.Location = screenPoint;

            autoCForm.SetAutoCompleteKey += (cmd) => SetAutoCompleteCmd(cmd);
            autoCForm.ShowDialog(this.ParentForm);
        }

        private void SetAutoCompleteCmd(string autoCKeyCmd)
        {
            if (string.IsNullOrEmpty(autoCKeyCmd)) return;

            string currentCmd = GetCmd();
            ClearInput(); // _commandBuffer 및 추적 초기화

            // 기존 입력만큼 백스페이스 전송
            if (!string.IsNullOrEmpty(currentCmd))
            {
                _terminal.Scripting?.Send(new string('\b', currentCmd.Length));
            }

            _terminal.Scripting?.Send(autoCKeyCmd);
        }

        private void mnuPasteCR_Click_Event(object sender, EventArgs e)
        {
            //ShowNotImplementedMessage("<CR> 붙여넣기"); 
            if (TerminalMode == E_TerminalMode.QuickClient)
                mnuPasteCR_Click(sender, e);
            else
                AppGlobal.terminalPanel1.ExecTerminalScreen(E_TerminalScreenTextEditType.PasteCR);
        }

        private void mnuPasteCR_Click(object sender, EventArgs e)
        {
            // true 인자를 전달하여 CR을 추가하도록 호출
            PasteText(true);
        }

        private void mnuPaste_Click_Event(object sender, EventArgs e)
        {
            if (TerminalMode == E_TerminalMode.QuickClient)
                mnuPaste_Click(sender, e);
            else
                AppGlobal.terminalPanel1.ExecTerminalScreen(E_TerminalScreenTextEditType.Paste);
        }

        private void mnuPaste_Click(object sender, EventArgs e)
        {
            PasteText();
        }

        private void mnuCopy_Click_Event(object sender, EventArgs e)
        {
            if (TerminalMode == E_TerminalMode.QuickClient)
                mnuCopy_Click(sender, e);
            else
                AppGlobal.terminalPanel1.ExecTerminalScreen(E_TerminalScreenTextEditType.Copy);

        }

        private void mnuCopy_Click(object sender, EventArgs e)
        {
            string selected = _terminal.GetSelectedText();
            if (!string.IsNullOrEmpty(selected))
                Clipboard.SetText(selected);
        }

        private void ClearInput()
        {
            if (_terminal.Scripting != null)
            {
                // 1. 현재 입력된 명령어의 길이를 파악합니다.
                // GetCmd()는 버퍼와 화면을 모두 체크하여 현재 입력 중인 문자열을 반환합니다.
                string currentCmd = GetCmd();
                int cmdLength = currentCmd.Length;

                if (cmdLength > 0)
                {
                    // 2. 백스페이스 시퀀스 생성
                    // \b : 커서를 왼쪽으로 한 칸 이동
                    // ' ' : 해당 칸을 공백으로 덮어씀 (삭제 효과)
                    // \b : 다시 왼쪽으로 한 칸 이동 (다음 삭제를 위해 커서 위치 고정)
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < cmdLength; i++)
                    {
                        sb.Append("\b \b");
                    }

                    // 터미널에 전송하여 시각적으로 삭제
                    _terminal.Scripting.Send(sb.ToString());
                }

                // 3. Ctrl+U (\x15)도 보조적으로 전송 (지원하는 장비는 즉시 삭제됨)
                _terminal.Scripting.Send("\x15");

                // 4. 로컬 소프트웨어 버퍼 초기화
                _commandBuffer.Clear();
            }
        }

        public static bool IsCommandSafe(string text, out string forbiddenKeyword)
        {
            forbiddenKeyword = null;
            if (string.IsNullOrEmpty(text)) return true;

            foreach (var cmd in _forbiddenCommands)
            {
                if (text.IndexOf(cmd, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    forbiddenKeyword = cmd;
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 실행된 명령어를 로그에 저장합니다.
        /// </summary>
        /// <param name="command">명령어</param>
        /// <param name="isLimitCmd">제한/금지된 명령어 여부</param>
        private void SaveCommandLog(string command, bool isLimitCmd)
        {
            if (string.IsNullOrWhiteSpace(command)) return;
            if (_deviceInfo == null) return;

            try
            {
                var logInfo = new DBExecuteCommandLogInfo
                {
                    DeviceInfo = _deviceInfo,
                    Command = command,
                    IsLimitCmd = isLimitCmd,
                    ConnectionLogID = _connectedSessionID,
                    LogType = E_DBLogType.ExecuteCommandLog
                };

                AppGlobal.s_TerminalExecuteLogProcess?.AddTerminalExecuteLog(logInfo);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SaveCommandLog Failed: {ex.Message}");
            }
        }

        #endregion

        #region Terminal Input Handling

        private void PasteText(bool appendCR = false)
        {
            if (!Clipboard.ContainsText()) return;

            string text = Clipboard.GetText();

            // [핵심] DB 기반 금지어 체크 함수 호출 (팝업 처리 포함)
            // IsLimitCmd가 true를 반환하면 '금지' 또는 '사용자가 아니오 선택' 상태임
            if (IsLimitCmd(text))
            {
                SaveCommandLog(text, true);
                return; // 전송 중단
            }

            if (appendCR)
            {
                if (!text.EndsWith("\r") && !text.EndsWith("\n")) text += "\r";
            }

            if (_isConnected && _terminal.Scripting != null)
            {
                _terminal.Scripting.Send(text);

                // 실행된 명령어 로그 저장 (여러 줄일 경우 분할 저장)
                var lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    WriteClog(line, true); // .clog 명령어 기록
                    SaveCommandLog(line, false);
                }
            }
        }

        public void SendText(string text)
        {
            if (string.IsNullOrEmpty(text)) return;
            if (IsLimitCmd(text))
            {
                return;
            }
            if (_isConnected && _terminal.Scripting != null)
            {
                _terminal.Scripting.Send(text);
                WriteClog(text, true); // .clog 명령어 기록
                SaveCommandLog(text, false);
            }
        }

        public void SendRccsDisconnectSequence()
        {
            this.SafeInvoke(() =>
            {
                if (!_isConnected || _terminal?.Scripting == null) return;

                // Legacy RCCS 종료 시 사용하던 chr(17)&chr(100) 전송 시퀀스를 그대로 유지합니다.
                _terminal.Scripting.Send("\x11d");
            });
        }

        public void ClearTerminal()
        {
            this.SafeInvoke(() =>
            {
                if (_terminal?.Screen != null)
                {
                    _terminal.Screen.Clear();
                    _terminal.Screen.SetCursorPosition(0, 0);

                    // 만약 히스토리(스크롤 바)까지 모두 비우고 싶다면 아래 코드를 추가하세요.
                    _terminal.HistoryMaxLength = 0;
                    _terminal.HistoryMaxLength = 5000; // 다시 원래대로 설정

                    _terminal.Refresh();
                }
            });
        }

        #endregion

        #region Logging

        private void ToggleLogging()
        {
            if (_isLogging) { StopLogging(); }
            else
            {
                using (var sfd = new SaveFileDialog { Filter = "Log|*.log", FileName = $"Log_{DateTime.Now:HHmmss}.log" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK) StartLogging(sfd.FileName);
                }
            }
        }

        /// <summary>
        /// 디바이스 정보를 기반으로 경로를 생성하고 StartLogging 함수를 호출합니다.
        /// </summary>
        private void StartTerminalLog(DeviceInfo aDeviceInfo)
        {
            if (aDeviceInfo == null) return;

            try
            {
                // 1. 기존 로그 정리 (이미 로깅 중이면 중단)
                if (_isLogging)
                {
                    StopLogging();
                }

                // 비동기 통합 큐가 참조할 세션 로깅용 타임스탬프 고정
                _sessionStartTimeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                // 2. 공통 StartLogging 함수 호출
                StartLogging("");
                // clog 및 log 통합용 워커 구동
                StartLogWriter();
            }
            catch (Exception ex)
            {
                LogError("StartTerminalLog Failed: " + ex.Message);
            }
        }

        /// <summary>
        /// 실제 FileStream을 생성하고 로깅 상태를 활성화합니다.
        /// </summary>
        private void StartLogging(string path)
        {
            try
            {
                // 기존 동기 FileStream 제거 및 비동기 상태 활성화
                _isLogging = true;

                // UI 업데이트
                this.SafeInvoke(() =>
                {
                    if (_lblLogStatus != null)
                    {
                        _lblLogStatus.Text = "● REC";
                        //_lblLogStatus.Visible = AppGlobal.s_ClientOption.IsAutoSaveLog;
                    }
                    LogInfo($"백그라운드 터미널 로깅 시작 (비동기 큐 이용)");
                });
            }
            catch (Exception ex)
            {
                _isLogging = false;
                throw new Exception($"로깅을 시작할 수 없습니다: {ex.Message}");
            }
        }

        private void StopLogging()
        {
            _isLogging = false;
            this.SafeInvoke(() =>
            {
                if (_lblLogStatus != null) _lblLogStatus.Visible = false;
            });
            // 탭 종료 시에만 StopLogWriter()를 호출하므로 여기서 워커를 닫지 않습니다.
        }

        #region .clog AutoSave Logic (Legacy Compatibility)

        /// <summary>
        /// .clog 파일에 로그를 기록합니다. (명령어는 |&|...|| 로 감쌉니다)
        /// 파일명은 현재 탭(컨트롤)의 이름에 따라 동적으로 결정됩니다.
        /// </summary>
        private void WriteClog0(string data, bool isCommand = false)
        {
            try
            {
                // 1. 자동 저장 옵션 확인
                if (!AppGlobal.s_ClientOption.IsAutoSaveLog) return;
                if (_deviceInfo == null) return;

                // 2. 현재 탭 이름 가져오기 
                // (this.Name 혹은 부모 탭의 Text를 사용할 수 있습니다. 여기서는 명확성을 위해 Name을 우선 사용합니다.)
                string currentTabName = string.IsNullOrEmpty(this.Name) ? _deviceInfo.Name : this.Name;

                // 3. 파일명에서 사용할 수 없는 문자 제거 (윈도우 파일 시스템 제한)
                string safeFileName = string.Join("_", currentTabName.Split(Path.GetInvalidFileNameChars()));

                // 4. 경로 설정: Log\AutoSaveLogs\yyyy-MM-dd\
                string tLogPath = Path.Combine(AppGlobal.s_ClientOption.LogPath, "AutoSaveLogs", DateTime.Now.ToString("yyyy-MM-dd"));

                if (!Directory.Exists(tLogPath))
                    Directory.CreateDirectory(tLogPath);

                // 5. 최종 파일 경로 (.clog)
                string tFileName = safeFileName + ".clog";
                string tFullPath = Path.Combine(tLogPath, tFileName);

                // 6. 데이터 구성 (명령어일 경우 구분자 추가)
                string content = isCommand ? $"|&|{data}||{Environment.NewLine}" : data;

                // 7. UTF-8로 추가 기록 (Append)
                // 파일명이 바뀌면 자동으로 해당 이름의 새 파일이 생성되거나 기존 파일에 추가됩니다.
                File.AppendAllText(tFullPath, content, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                // 로그 기록 실패 시 디버그 출력 (I/O 충돌 방지)
                System.Diagnostics.Debug.WriteLine($".clog Write Failed [{this.Name}]: {ex.Message}");
            }
        }

        private void WriteClog(string data, bool isCommand = false)
        {
            if (!AppGlobal.s_ClientOption.IsAutoSaveLog || _deviceInfo == null) return;

            _logQueue.Add(new LogEntry
            {
                Content = data,
                TabName = string.IsNullOrEmpty(this.Name) ? _deviceInfo.Name : this.Name,
                SessionTime = _sessionStartTimeStamp,
                IsCommand = isCommand,
                WriteClog = true,
                WriteNormalLog = false
            });
        }
        private void StartLogWriter()
        {
            if (_logWriterTask != null) return;
            _logCts = new CancellationTokenSource();

            _logWriterTask = Task.Factory.StartNew(() =>
            {
                foreach (var entry in _logQueue.GetConsumingEnumerable(_logCts.Token))
                {
                    try { WriteToFile(entry); }
                    catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.Message); }
                }
            }, _logCts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        // Dispose 또는 세션 종료 시 호출
        private void StopLogWriter()
        {
            _logQueue.CompleteAdding();
            _logCts?.Cancel();
            try { _logWriterTask?.Wait(1000); } catch { }
        }

        private void WriteToFile(LogEntry entry)
        {
            if (string.IsNullOrEmpty(entry.TabName)) return;

            // 최신 옵션 경로 및 파일명 생성 문자 캡처 (실시간 반영)
            string baseLogPath = AppGlobal.s_ClientOption.LogPath;
            if (string.IsNullOrEmpty(baseLogPath)) return;
            string safeTabName = string.Join("_", entry.TabName.Split(Path.GetInvalidFileNameChars()));

            try 
            {
                // 1. .log 처리 (TerminalLogs)
                if (entry.WriteNormalLog && !string.IsNullOrEmpty(entry.SessionTime))
                {
                    string tDirPath = Path.Combine(baseLogPath, "TerminalLogs");
                    if (!Directory.Exists(tDirPath)) Directory.CreateDirectory(tDirPath);

                    string logFileName = $"{safeTabName}_{entry.SessionTime}.log";
                    File.AppendAllText(Path.Combine(tDirPath, logFileName), entry.Content, Encoding.UTF8);
                }

                // 2. .clog 처리 (AutoSaveLogs)
                if (entry.WriteClog)
                {
                    string tClogPath = Path.Combine(baseLogPath, "AutoSaveLogs", DateTime.Now.ToString("yyyy-MM-dd"));
                    if (!Directory.Exists(tClogPath)) Directory.CreateDirectory(tClogPath);

                    string clogFileName = safeTabName + ".clog";
                    string finalContent = entry.IsCommand
                        ? $"{Environment.NewLine}|&|{entry.Content}||{Environment.NewLine}"
                        : entry.Content;

                    File.AppendAllText(Path.Combine(tClogPath, clogFileName), finalContent, Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"WriteToFile I/O Exception: {ex.Message}");
            }
        }

        #endregion

        // 탭 이름이나 컨트롤 이름을 변경하는 예시 메서드
        public void UpdateTerminalName(string newName)
        {
            this.Name = newName;
            // 이후 WriteClog 호출 시 자동으로 'newName.clog'로 저장됨
            LogInfo($"터미널 이름이 {newName}(으)로 변경되어 로그 파일명이 업데이트되었습니다.");
        }

        private void _terminal_DataReceived(object sender, DataReceivedEventArgs e)
        {
            string rawDataStr = e.RawData;
            if (string.IsNullOrEmpty(rawDataStr)) return;

            // 통합 비동기 큐로 .log 및 .clog 기록 위임
            _logQueue.Add(new LogEntry
            {
                Content = rawDataStr,
                TabName = string.IsNullOrEmpty(this.Name) ? (_deviceInfo?.Name ?? "") : this.Name,
                SessionTime = _sessionStartTimeStamp,
                IsCommand = false,
                WriteClog = AppGlobal.s_ClientOption.IsAutoSaveLog,
                WriteNormalLog = _isLogging
            });

            if (AppGlobal.s_ClientOption.IsUseTerminalAutoMoreString)
            {
                HandleAutoMore(e.StrippedData);
            }
        }

        private void HandleAutoMore(string receivedText)
        {
            if (string.IsNullOrEmpty(receivedText)) return;

            // 1. 장비별 특화된 More 설정 반영 (동적으로 규칙 업데이트)
            UpdateAutoResponderRules();

            // 2. 자동 응답기 실행
            string response = _autoResponder.Process(receivedText);

            if (!string.IsNullOrEmpty(response))
            {
                if (_isConnected && _terminal.Scripting != null)
                {
                    _terminal.Scripting.Send(response);
                }
            }
        }

        // [성능 최적화] AutoResponder 캐싱 상태 변수
        private int _cachedDeviceModelId = -1;
        private string _cachedMoreString = null;
        private string _cachedMoreMark = null;

        private void UpdateAutoResponderRules()
        {
            if (_deviceInfo == null) return;

            string tMoreString = "";
            string tMoreMark = "";

            if (_deviceInfo.DeviceType == E_DeviceType.UserNeGroup)
            {
                tMoreString = _deviceInfo.MoreString;
                tMoreMark = _deviceInfo.MoreMark;
            }
            else 
            {
                // 캐시 히트(Hit) 체크 - 이전에 스캔한 이력이 있다면 전역 컬렉션 스캔 회피
                if (_cachedDeviceModelId == _deviceInfo.ModelID && _cachedMoreString != null)
                {
                    tMoreString = _cachedMoreString;
                    tMoreMark = _cachedMoreMark;
                }
                else if (AppGlobal.s_ModelInfoList != null && AppGlobal.s_ModelInfoList.Contains(_deviceInfo.ModelID))
                {
                    ModelInfo tModelInfo = AppGlobal.s_ModelInfoList[_deviceInfo.ModelID];
                    tMoreString = tModelInfo.MoreString;
                    tMoreMark = tModelInfo.MoreMark;
                    
                    // 스캔 결과 캐싱
                    _cachedDeviceModelId = _deviceInfo.ModelID;
                    _cachedMoreString = tMoreString;
                    _cachedMoreMark = tMoreMark;
                }
            }

            if (!string.IsNullOrEmpty(tMoreString))
            {
                string keyToSend = " ";
                if (tMoreMark != null)
                {
                    if (tMoreMark.Contains("${SPACE}")) keyToSend = " ";
                    else if (tMoreMark.Contains("${ENTER}")) keyToSend = "\r";
                }

                // [성능 최적화] 기존 규칙과 동일하면 갱신하지 않음
                if (_autoResponder.HasRule(tMoreString, keyToSend)) return;

                // 기존 규칙을 클리어하고 새로 설정 (장비가 바뀔 수 있으므로)
                _autoResponder.ClearRules();
                _autoResponder.AddRule(new AutoResponseRule(tMoreString, keyToSend));
            }
        }

        private void SaveWaitScript()
        {
            if (_terminalStatus != E_TerminalStatus.Recording) return;
            if (_terminal?.Screen == null) return;

            int row = _terminal.Screen.CursorTop;
            int cols = _terminal.Screen.Columns;
            string[] lines = _terminal.Screen.GetRegionText(0, row, cols, 1);
            if (lines != null && lines.Length > 0)
            {
                string lineText = lines[0];
                // 프롬프트 기호(>, $, #, ])를 기준으로 추출 시도
                int promptIndex = lineText.LastIndexOfAny(new char[] { '>', '$', '#', ']' });

                string waitText = "";
                if (promptIndex >= 0)
                {
                    // 기호 발견 시 기호 포함 앞부분을 프롬프트로 간주
                    waitText = lineText.Substring(0, promptIndex + 1).Trim();
                }
                else
                {
                    // 기호가 없는 경우 커서 앞부분까지만 추출 (임시 프롬프트)
                    int cursorLeft = _terminal.Screen.CursorLeft;
                    if (cursorLeft > 0 && cursorLeft <= lineText.Length)
                        waitText = lineText.Substring(0, cursorLeft).Trim();
                }

                // 유의미한 텍스트이고 이전과 다른 경우에만 기록 (중복 방지)
                if (!string.IsNullOrEmpty(waitText) && waitText != _lastRecordedWaitText)
                {
                    _scriptGenerator.AddWait(new TerminalScriptKeyInfo(waitText, E_TerminalScriptKeyType.Normal));
                    _lastRecordedWaitText = waitText;
                }
            }
        }

        private bool IsLoginPrompt()
        {
            if (_terminal?.Screen == null) return false;
            try
            {
                int row = _terminal.Screen.CursorTop;
                int cols = _terminal.Screen.Columns;
                string[] lines = _terminal.Screen.GetRegionText(0, row, cols, 1);
                if (lines != null && lines.Length > 0)
                {
                    string lineText = lines[0];
                    string[] keywords = { "login:", "password:", "id:", "pw:" };
                    foreach (string keyword in keywords)
                    {
                        if (lineText.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                            return true;
                    }
                }
            }
            catch { }
            return false;
        }

        private void _terminal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
            {
                ToggleSearchBar(true);
                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }

            if (_terminalStatus == E_TerminalStatus.Recording)
            {
                SaveWaitScript();

                string keyData = "";
                E_TerminalScriptKeyType keyType = E_TerminalScriptKeyType.Normal;

                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab || e.KeyCode == Keys.Back)
                {
                    keyData = ((int)e.KeyCode).ToString();
                    keyType = E_TerminalScriptKeyType.Control;
                }
                else
                {
                    char c = GetCharFromKey(e.KeyCode, e.Shift);
                    if (c != '\0') keyData = c.ToString();
                }

                if (!string.IsNullOrEmpty(keyData)) _scriptGenerator.AddSend(new TerminalScriptKeyInfo(keyData, keyType));
            }

            if (e.KeyCode == Keys.Enter)
            {
                if (IsLoginPrompt())
                {
                    _commandBuffer.Clear();
                    return;
                }

                string command = GetCmd();

                // .clog에 명령어 기록 (명령어 구분자 포함)
                WriteClog(command, true);

                if (IsLimitCmd(command))
                {
                    SaveCommandLog(command, true); // 차단된 명령어 로그 저장
                    ClearInput();
                    e.Handled = true;
                    e.SuppressKeyPress = true;

                    LogWarning($"[Security] 금지 명령어가 감지되어 입력을 초기화했습니다: {command}");
                    return;
                }

                SaveCommandLog(command, false); // 정상 실행 명령어 로그 저장
                _commandBuffer.Clear();
            }
            else if (e.KeyCode == Keys.Back)
            {
                if (_commandBuffer.Length > 0) _commandBuffer.Length--;
            }
            else
            {
                char c = GetCharFromKey(e.KeyCode, e.Shift);
                if (c != '\0' && !char.IsControl(c))
                {
                    _commandBuffer.Append(c);
                }
            }
        }

        private char GetCharFromKey(Keys key, bool shift)
        {
            if (key >= Keys.A && key <= Keys.Z) return (char)((shift ? 'A' : 'a') + (key - Keys.A));
            if (key >= Keys.D0 && key <= Keys.D9) return (char)('0' + (key - Keys.D0));
            if (key == Keys.Space) return ' ';
            if (key == Keys.OemMinus) return shift ? '_' : '-';
            return '\0';
        }

        public string GetCmd()
        {
            // 1. 우선적으로 키보드 입력 버퍼 확인
            string cmdBufferText = _commandBuffer.ToString().Trim();
            if (!string.IsNullOrEmpty(cmdBufferText)) return cmdBufferText;

            // 2. 버퍼가 비어있을 경우 화면에서 현재 라인 텍스트 추출
            if (_terminal?.Screen != null)
            {
                int cursorRow = _terminal.Screen.CursorTop;
                int cols = _terminal.Screen.Columns;
                string[] lines = _terminal.Screen.GetRegionText(0, cursorRow, cols, 1);

                if (lines != null && lines.Length > 0)
                {
                    string lineText = lines[0];
                    int cursorLeft = _terminal.Screen.CursorLeft;

                    // 커서 위치까지의 텍스트만 취함
                    if (cursorLeft >= 0 && cursorLeft <= lineText.Length)
                    {
                        lineText = lineText.Substring(0, cursorLeft);
                    }

                    // 프롬프트 기호(>, $, #, ]) 이후의 문자열을 명령어로 간주
                    int promptIndex = lineText.LastIndexOfAny(new char[] { '>', '$', '#', ']' });
                    if (promptIndex >= 0 && promptIndex < lineText.Length - 1)
                    {
                        return lineText.Substring(promptIndex + 1).Trim();
                    }
                    return lineText.Trim();
                }
            }
            return string.Empty;
        }

        #endregion


        #region
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing) { StopLogging(); Context?.Dispose(); _terminal?.Dispose(); }
        //    base.Dispose(disposing);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                StopLogging();

                if (Context != null)
                {
                    // Dispose 내에서 비동기 호출을 기다려야 할 경우
                    Task.Run(async () => await BastionManager.Instance.ReleaseSessionAsync(Context)).Wait();
                    Context = null;
                }

                _terminal?.Dispose();
            }
            base.Dispose(disposing);
        }

        public void LogMessage(string msg) => _terminal.Screen.Write(msg + "\r\n");

        public void FocusTerminal()
        {
            _terminal.Focus();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (_lblLogStatus != null)
            {
                _lblLogStatus.Location = new Point(this.Width - 60, 5);
                _lblLogStatus.BringToFront();
            }
            _terminal?.Invalidate();
        }

        public object ConnectDevice(object deviceInfo)
        {
            _deviceInfo = new DeviceInfo((DeviceInfo)deviceInfo);

            this.SafeInvoke(() =>
            {
                _terminal?.Screen.Clear();
                _terminal?.Screen.SetCursorPosition(0, 0);
            });

            return null;
        }

        public async void Disconnect()
        {
            await DisconnectAsync();
        }
        public async Task DisconnectAsync()
        {
            try
            {
                if (_isConnected)
                {
                    _terminal?.Unbind();
                    await CloseSessionAsync();
                    TerminalStatus = E_TerminalStatus.Disconnected;
                }
            }
            catch (Exception ex)
            {
                LogMessage($"Disconnect error: {ex.Message}");
            }
        }
        public async Task CloseSessionAsync()
        {
            _isConnected = false;

            // [설계 반영] 세션 종료 로그 기록 (Online 모드 전용)
            if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online && _connectionLogID != 0)
            {
                int logIdToClose = _connectionLogID;
                int sessionIdToClose = _connectedSessionID;
                _connectionLogID = 0; // 중복 호출 방지
                await Task.Run(() => FinalizeConnectionLog(logIdToClose, sessionIdToClose)).ConfigureAwait(false);
            }

            if (Context != null)
            {
                if (_isBastionManagedSession)
                {
                    await BastionManager.Instance.ReleaseSessionAsync(Context);
                }
                else
                {
                    Context.Dispose();
                }

                Context = null;
                _isBastionManagedSession = false;
            }
        }

        public void DisplayResult(int sessionID, string result)
        {
            System.Diagnostics.Debug.WriteLine($"DisplayResult (ID:{sessionID}): {result}");
        }

        public void RunScript(Script script)
        {
            if (script == null) return;

            // UI 스레드에서 비동기 메서드 호출 (Fire and Forget 패턴 사용 시 컨텍스트 주의)
            _ = RunScriptAsync(script);
        }

        private string ExtractQuotedContent(string text)
        {
            int firstQuote = text.IndexOf('"');
            int lastQuote = text.LastIndexOf('"');
            if (firstQuote >= 0 && lastQuote > firstQuote)
            {
                return text.Substring(firstQuote + 1, lastQuote - firstQuote - 1);
            }
            return string.Empty;
        }
        public async Task RunScriptAsync(Script script)
        {
            if (script == null || string.IsNullOrEmpty(script.RawScript)) return;

            _scriptCts?.Cancel();
            _scriptCts = new CancellationTokenSource();
            var ct = _scriptCts.Token;

            TerminalStatus = E_TerminalStatus.RunScript;
            this.LogInfo("스크립트 실행을 시작합니다...");

            try
            {
                // 1. 프롬프트 자동 감지 단계
                string detectedPrompt = "";
                await Task.Run(async () =>
                {
                    _terminal.Scripting.Send("\r");
                    await Task.Delay(500);
                    this.RunSync(() => { detectedPrompt = DetectCurrentPrompt(); });
                }, ct);

                this.RunSync(() =>
                {
                    if (this.IsDisposed) return;
                    _terminal.SetDataProcessingMode(DataProcessingMode.None);
                    _terminal.UserInputEnabled = false;
                });

                await Task.Run(async () =>
                {
                    string[] lines = script.RawScript.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                    foreach (var line in lines)
                    {
                        ct.ThrowIfCancellationRequested();

                        string trimmed = line.Trim();
                        // 주석 및 불필요한 라인 스킵
                        if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith("'")) continue;
                        if (trimmed.Equals("Sub Main", StringComparison.OrdinalIgnoreCase) ||
                            trimmed.Equals("End Sub", StringComparison.OrdinalIgnoreCase)) continue;

                        // --- [명령어 전송 처리 (Send)] ---
                        if (trimmed.StartsWith(Script.s_Send))
                        {
                            string content = ExtractQuotedContent(trimmed);

                            // [개선 포인트 1] 따옴표가 없는 단독 chr(13) 또는 &chr(13) 처리
                            // TACT.Send chr(13) 혹은 TACT.Send "sh ver"&chr(13) 형태 대응
                            if (trimmed.Contains("chr(13)") || trimmed.Contains("char(13)"))
                            {
                                // 만약 따옴표 내용이 없더라도 chr(13)이 있으면 엔터로 간주
                                if (string.IsNullOrEmpty(content)) content = "\r";
                                else if (!content.EndsWith("\r")) content += "\r";
                            }

                            // [Step 1] 명령어 라벨 로그 기록 (이전 데이터 수신 완료를 위해 미세 대기 후 기록)
                            await Task.Delay(50, ct);
                            WriteClog(content, true);

                            // [Step 2] 실제 명령어 전송
                            _terminal.Scripting.Send(content);

                            // [Step 3] 동기화 보장: 명령어 전송 후 장비 프롬프트를 대기함으로 로그 순서 강제 교정
                            // 단순히 chr(13)만 보낸 경우에도 장비가 다음 프롬프트를 띄울 시간을 줍니다.
                            try
                            {
                                // [Step 3] 동기화 보장 (Rebex 표준 방식 적용)
                                // 정규식 이벤트 생성
                                var promptEvent = ScriptEvent.FromRegex(@".*[>#\]]\s*$");
                                // 2초 타임아웃으로 현재 장비의 프롬프트(> 또는 #)가 나타날 때까지 대기
                                _terminal.Scripting.WaitFor(promptEvent | ScriptEvent.Delay(2000));
                            }
                            catch (Exception)
                            {
                                // 타임아웃 발생 시 장비가 응답이 느린 것이므로 강제 딜레이 부여
                                await Task.Delay(200, ct);
                            }

                            SaveCommandLog(content, false);
                        }
                        // --- [대기 처리 (WaitForString)] ---
                        else if (trimmed.StartsWith(Script.s_WaitForString))
                        {
                            string waitStr = ExtractQuotedContent(trimmed);

                            // 호환성 로직: 스크립트의 고정 프롬프트 대신 실제 감지된 프롬프트 사용
                            bool isPromptType = waitStr.EndsWith("# ") || waitStr.EndsWith("> ") || waitStr.EndsWith("] ");
                            if (isPromptType && !string.IsNullOrEmpty(detectedPrompt))
                            {
                                if (waitStr != detectedPrompt)
                                {
                                    System.Diagnostics.Debug.WriteLine($"[호환성 모드] '{waitStr}' -> '{detectedPrompt}' 대기 변경");
                                    waitStr = detectedPrompt;
                                }
                            }

                            if (!string.IsNullOrEmpty(waitStr))
                            {
                                // Regex 특수문자 이스케이프 처리하여 대기
                                var targetEvent = ScriptEvent.FromRegex(Regex.Escape(waitStr));

                                // 해당 문자열이 오거나, 최대 5초가 지날 때까지 대기
                                _terminal.Scripting.WaitFor(targetEvent | ScriptEvent.Delay(5000));
                            }
                        }

                        // 명령어 라인 간의 최소 휴식 시간
                        await Task.Delay(100, ct);
                    }
                }, ct);

                this.LogInfo("스크립트 실행이 완료되었습니다.");
            }
            catch (OperationCanceledException)
            {
                this.LogInfo("스크립트 실행이 사용자로부터 취소되었습니다.");
            }
            catch (Exception ex)
            {
                this.LogError($"스크립트 실행 중 오류: {ex.Message}");
            }
            finally
            {
                _scriptCts = null;
                this.RunSync(() =>
                {
                    if (this.IsDisposed) return;
                    _terminal.SetDataProcessingMode(DataProcessingMode.Automatic);
                    _terminal.UserInputEnabled = true;
                    TerminalStatus = IsConnected ? E_TerminalStatus.Connection : E_TerminalStatus.Disconnected;
                });
            }
        }

        /// <summary>
        /// 프롬프트 감지 헬퍼 메서드
        /// </summary>
        /// <returns></returns>
        private string DetectCurrentPrompt()
        {
            if (_terminal?.Screen == null) return string.Empty;

            try
            {
                // 현재 커서가 위치한 줄의 텍스트를 가져옴
                int row = _terminal.Screen.CursorTop;
                int cols = _terminal.Screen.Columns;
                string[] lines = _terminal.Screen.GetRegionText(0, row, cols, 1);

                if (lines != null && lines.Length > 0)
                {
                    string lineText = lines[0];
                    int cursorLeft = _terminal.Screen.CursorLeft;

                    // 커서 위치까지만 자름 (입력 중인 텍스트 제외 목적)
                    if (cursorLeft > 0 && cursorLeft <= lineText.Length)
                        lineText = lineText.Substring(0, cursorLeft);

                    // 프롬프트 기호(>, #, $, ])의 마지막 위치를 찾음
                    int promptIndex = lineText.LastIndexOfAny(new char[] { '>', '#', '$', ']' });
                    if (promptIndex >= 0)
                    {
                        // 기호 포함 앞부분 추출 후, 스크립트 관례에 따라 뒤에 스페이스 하나 추가
                        return lineText.Substring(0, promptIndex + 1) + " ";
                    }
                }
            }
            catch { }
            return string.Empty;
        }
        public void WriteTerminalLog()
        {
            try
            {
                using (var sfd = new SaveFileDialog
                {
                    // 텍스트 및 로그 파일 포맷만 필터로 설정
                    Filter = "Log Files (*.log)|*.log|Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                    DefaultExt = "log",
                    Title = "터미널 결과 저장"
                })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        // 직접 파일을 쓰지 않고, Rebex Terminal의 Save 메서드를 호출합니다.
                        // 포맷은 TerminalCaptureFormat.Text로 고정합니다.
                        _terminal.Save(
                            sfd.FileName,
                            TerminalCaptureFormat.Text,
                            TerminalCaptureOptions.None // 필요 시 SaveTerminalResolution 등 옵션 추가 가능
                        );

                        AppGlobal.ShowMessageBox(
                            AppGlobal.s_ClientMainForm,
                            "화면 내용을 텍스트 파일로 저장했습니다.",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                LogError("WriteTerminalLog Error: " + ex.Message);
                AppGlobal.ShowMessageBox(
                    AppGlobal.s_ClientMainForm,
                    "파일 저장 중 오류가 발생했습니다.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
            }
        }

        public void ApplyOption()
        {
            if (_deviceInfo == null) return;

            // 1. UI 표시 상태만 옵션에 따라 동기화 (로깅은 건드리지 않음)
            if (_lblLogStatus != null)
            {
                //_lblLogStatus.Visible = AppGlobal.s_ClientOption.IsAutoSaveLog;
            }

            Color targetBackColor;
            Color targetForeColor;
            string targetFontName;
            float targetFontSize;
            FontStyle targetFontStyle;

            if (_deviceInfo.DevicePartCode == 1 || _deviceInfo.DevicePartCode == 6 || _deviceInfo.DevicePartCode == 31)
            {
                targetBackColor = AppGlobal.s_ClientOption.HighlightBackGroundColor;
                targetForeColor = AppGlobal.s_ClientOption.HighlightFontColor;
                targetFontName = AppGlobal.s_ClientOption.HighlightFontName;
                targetFontSize = AppGlobal.s_ClientOption.HighlightFontSize;
                targetFontStyle = AppGlobal.s_ClientOption.HighlightFontStyle;
            }
            else
            {
                targetBackColor = AppGlobal.s_ClientOption.TerminalBackGroundColor;
                targetForeColor = AppGlobal.s_ClientOption.TerminalFontColor;
                targetFontName = AppGlobal.s_ClientOption.TerminalFontName;
                targetFontSize = AppGlobal.s_ClientOption.TerminalFontSize;
                targetFontStyle = AppGlobal.s_ClientOption.TerminalFontStyle;
            }

            if (string.IsNullOrEmpty(targetFontName)) targetFontName = "Consolas";
            else if (targetFontName.Equals("굴림") || targetFontName.Equals("돋움") || targetFontName.Equals("궁서") || targetFontName.Equals("바탕"))
                targetFontName += "체";

            ApplyTheme(new Font(targetFontName, targetFontSize, targetFontStyle), targetBackColor, targetForeColor);

            try
            {
                this.Font = new Font(targetFontName, targetFontSize, targetFontStyle, GraphicsUnit.Point, ((byte)(0)));
                _terminal.TerminalFont = new TerminalFont(targetFontName, targetFontSize);
            }
            catch
            {
                this.Font = new Font("Consolas", 10, FontStyle.Regular);
                _terminal.TerminalFont = new TerminalFont("Consolas", 10);
            }

            this.Refresh();
        }

        public void FindForm_Close() { }

        public void FindForm_OnTelnetStringFind(TelnetStringFindHandlerArgs args) => ShowNotImplementedMessage("문자열 찾기");

        public void IsLimitCmdForShortenCommand(object sender, string text)
        {
            // 신규 엔진의 SendText는 내부적으로 IsLimitCmd 필터링을 수행함
            SendText(text);
        }

        public void ScriptWork(E_ScriptWorkType scriptWorkType)
        {
            switch (scriptWorkType)
            {
                case E_ScriptWorkType.Rec:
                    TerminalStatus = E_TerminalStatus.Recording;
                    _scriptGenerator.Reset();
                    _lastRecordedWaitText = ""; // 녹화 시작 시 초기화
                    break;

                case E_ScriptWorkType.RecCancel:
                    TerminalStatus = E_TerminalStatus.Connection;
                    _scriptGenerator.Reset();
                    break;

                case E_ScriptWorkType.Save:
                    TerminalStatus = E_TerminalStatus.Connection;

                    Script tScript = new Script();
                    tScript.RawScript = _scriptGenerator.MakeScript();
                    ModifyScript tSave = new ModifyScript(E_WorkType.Add, tScript);
                    tSave.InitializeControl();
                    tSave.ShowDialog(AppGlobal.s_ClientMainForm);
                    break;

                case E_ScriptWorkType.RunCancel:
                    TerminalStatus = E_TerminalStatus.Connection;
                    mnuStopScript_Click(null, null);
                    break;

                case E_ScriptWorkType.RecLog:
                    TerminalStatus = E_TerminalStatus.RecLog;
                    break;
            }
        }

        public void ChangeClientMode()
        {
            if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Console)
            {
                if (ConnectionType == ConnectionTypes.RemoteTelnet)
                {
                    Disconnect();
                    TerminalStatus = E_TerminalStatus.Disconnected;
                }
            }
        }

        public void ExecTerminalScreen(E_TerminalScreenTextEditType editType)
        {
            switch (editType)
            {
                case E_TerminalScreenTextEditType.Copy: mnuCopy_Click(null, null); break;
                case E_TerminalScreenTextEditType.Clear: mnuClear_Click(null, null); break;
                case E_TerminalScreenTextEditType.CmdClear: mnuCmdClear_Click(null, null); break;
                case E_TerminalScreenTextEditType.Find: mnuFind_Click(null, null); break;
                case E_TerminalScreenTextEditType.Paste: mnuPaste_Click(null, null); break;
                case E_TerminalScreenTextEditType.PasteCR: mnuPasteCR_Click(null, null); break;
                case E_TerminalScreenTextEditType.SelectAll: mnuSelectAll_Click(null, null); break;
                case E_TerminalScreenTextEditType.AutoC: mnuAutoC_Click(null, null); break;
                case E_TerminalScreenTextEditType.SearchCmd: mnuSearchDefaultCmd_Click(null, null); break;
            }
        }

        private void ShowNotImplementedMessage(string featureName)
        {
            MessageBox.Show($"'{featureName}' 기능은 현재 구현 예정입니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion



        #region ITactTerminalControl 멤버 구현 (ITactTerminal이 Control 역할도 겸함)

        // 기존 이벤트 및 속성 구현
        public event DefaultHandler OnTelnetFindString;
        public event HandlerArgument2<ITactTerminal, E_TerminalStatus> OnTerminalStatusChange;
        public event DefaultHandler CallOptionHandlerEvent;
        public event HandlerArgument3<string, DevComponents.DotNetBar.eProgressItemType, bool> ProgreBarHandlerEvent;

        public Mode Modes { get; set; } = new Mode();
        public DeviceInfo DeviceInfo { get => _deviceInfo; set => _deviceInfo = value; }
        public DaemonProcessInfo DaemonProcessInfo { get; set; }
        public Control UIControl => this;

        public E_TerminalStatus TerminalStatus
        {
            get => _terminalStatus;
            set
            {
                if (_terminalStatus != value)
                {
                    E_TerminalStatus oldStatus = _terminalStatus;
                    _terminalStatus = value;
                    OnTerminalStatusChange?.Invoke(this, value);

                    if (oldStatus == E_TerminalStatus.TryConnection &&
                        _terminalStatus == E_TerminalStatus.Connection &&
                        AppGlobal.s_ClientOption.IsUseTerminalAutoLogin)
                    {
                        StartLoginProcess();
                    }

                    // 스크립트 실행/녹화 상태에 따른 메뉴 활성화 제어
                    UpdateScriptMenuStatus();
                }
                ChangeStatusIcon();
            }
        }

        private void UpdateScriptMenuStatus()
        {
            if (mnuStopScript == null) return;

            // 녹화 중이거나 스크립트 실행 중일 때만 '스크립트 취소' 메뉴 활성화
            mnuStopScript.Enabled = (_terminalStatus == E_TerminalStatus.Recording ||
                                    _terminalStatus == E_TerminalStatus.RunScript ||
                                    _terminalStatus == E_TerminalStatus.RecLog);
        }

        private bool _isAutoLoginProcessing = false;

        private void StartLoginProcess()
        {
            Task.Run(async () =>
            {
                try
                {
                    if (_isAutoLoginProcessing) return;
                    _isAutoLoginProcessing = true;

                    var protocol = _deviceInfo.TerminalConnectInfo.ConnectionProtocol;

                    if ((protocol == E_ConnectionProtocol.TELNET || protocol == E_ConnectionProtocol.SSHTelnet) &&
                        AppGlobal.s_RACTClientMode == E_RACTClientMode.Online)
                    {
                        LogInfo($"자동 로그인을 시도합니다. (Target: {_deviceInfo.IPAddress})");

                        if (_connectionCommandSet == null)
                        {
                            if (!await RequestConnectionCommandSetAsync())
                            {
                                LogError("접속 명령 세트를 로드하지 못했습니다.");
                                return;
                            }
                        }

                        if (protocol == E_ConnectionProtocol.SSHTelnet) return;

                        ExecuteTelnetLoginScript();
                    }
                }
                catch (Exception ex)
                {
                    LogError("StartLoginProcess Error: " + ex.Message);
                }
                finally
                {
                    _isAutoLoginProcessing = false;
                }
            });
        }

        private void LogInfo(string msg) => this.SafeInvoke(() => AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, msg));
        private void LogWarning(string msg) => this.SafeInvoke(() => AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, msg));
        private void LogError(string msg) => this.SafeInvoke(() => AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, msg));



        private async Task<bool> RequestConnectionCommandSetAsync()
        {
            try
            {
                RequestCommunicationData requestData = AppGlobal.MakeDefaultRequestData();
                requestData.CommType = E_CommunicationType.RequestDefaultConnectionCommand;
                requestData.RequestData = _deviceInfo;

                if (_deviceInfo.TerminalConnectInfo.TelnetPort == 1023) requestData.UserData = "TL1";

                LogInfo($"기본 접속 명령 로드 시도... (IP: {_deviceInfo.IPAddress})");

                // [변경] 서비스의 SendRequestAsync를 사용하여 응답을 직접 기다립니다.
                // TResult는 예상되는 데이터 타입(FACT_DefaultConnectionCommandSet)입니다.
                var response = await _connectionService.SendRequestAsync(
                    this,
                    requestData,
                    AppGlobal.s_RequestTimeOut
                );

                if (response == null || response.Error.Error != E_ErrorType.NoError)
                {
                    LogWarning($"기본 접속 명령 로드 실패. (IP: {_deviceInfo.IPAddress})");
                    return false;
                }

                _connectionCommandSet = response.ResultData as FACT_DefaultConnectionCommandSet;

                if (_connectionCommandSet == null || _connectionCommandSet.CommandList.Count == 0)
                {
                    LogWarning("접속 명령 정보가 비어 있습니다.");
                    return false;
                }

                foreach (var cmd in _connectionCommandSet.CommandList)
                {
                    cmd.ErrorString = "Login incorrect";
                }

                return true;
            }
            catch (TaskCanceledException)
            {
                LogWarning($"기본 접속 명령 로드 시간 초과. (IP: {_deviceInfo.IPAddress})");
                return false;
            }
            catch (Exception ex)
            {
                LogError("RequestConnectionCommandSet Exception: " + ex.Message);
                return false;
            }
        }




        /// <summary>
        /// LTE/Reverse SSH 접속을 위해 서버에 터널 개방을 요청하고 매핑된 정보를 가져옵니다.
        /// (MCTerminalControl.cs의 RequestSSHTunnelOpen 로직 이식)
        /// 관리서버에 직접 요청하도록 수정됨.
        /// </summary>
        private async Task<bool> RequestSSHTunnelInfoAsync()
        {
            try
            {
                RequestCommunicationData requestData = AppGlobal.MakeDefaultRequestData();
                requestData.CommType = E_CommunicationType.RequestSSHTunnelOpen;
                requestData.RequestData = _deviceInfo;

                LogInfo($"SSH 터널 개방 요청 중... (관리서버 직접 요청, IP: {_deviceInfo.IPAddress})");

                // [변경] LTE/Reverse SSH 환경이므로 타임아웃을 넉넉하게 설정(30초)
                // 관리서버에 직접 요청
                var response = await _connectionService.SendRequestAsync(
                    this,
                    requestData,
                    AppGlobal.s_RequestTimeOut * 6
                );

                if (response == null || response.Error.Error != E_ErrorType.NoError)
                {
                    LogWarning($"{_deviceInfo.IPAddress} 장비에 접속 할 수 없습니다. 터널링 요청 실패");
                    if (response != null)
                        LogWarning($"{_deviceInfo.IPAddress} ErrorString = {response.Error.ErrorString}");
                    return false;
                }

                var tunnelInfo = response.ResultData as DeviceInfo;
                if (tunnelInfo == null) return false;

                if (string.IsNullOrEmpty(tunnelInfo.SSHTunnelIP) || tunnelInfo.SSHTunnelPort == 0)
                {
                    LogWarning($"SSH Tunnel 정보가 유효하지 않습니다. (IP: {tunnelInfo.SSHTunnelIP}, Port: {tunnelInfo.SSHTunnelPort})");
                    return false;
                }

                // 터널 주소로 디바이스 정보 업데이트
                _deviceInfo.IPAddress = tunnelInfo.SSHTunnelIP;
                _deviceInfo.TerminalConnectInfo.TelnetPort = tunnelInfo.SSHTunnelPort;

                return true;
            }
            catch (TaskCanceledException)
            {
                LogWarning($"SSH 터널 개방 요청 시간 초과. (IP: {_deviceInfo.IPAddress})");
                return false;
            }
            catch (Exception ex)
            {
                LogError("RequestSSHTunnelInfo Exception: " + ex.Message);
                return false;
            }
        }

        private void ExecuteTelnetLoginScript()
        {
            Task.Run(async () =>
            {
                bool loginSucceeded = false;
                try
                {
                    if (this.IsDisposed) return;
                    _terminal.SetDataProcessingMode(DataProcessingMode.None);
                    _terminal.UserInputEnabled = false;
                    TerminalStatus = E_TerminalStatus.RunScript;

                    AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "UI Mode set to None (Sequential)");

                    _terminal.Scripting.ExecuteConnectionCommand(_connectionCommandSet, _deviceInfo);

                    loginSucceeded = true;
                    LogInfo("자동 로그인 완료.");
                }
                catch (Exception ex)
                {
                    LogError("Script Failed: " + ex.Message);
                    await DisconnectAsync();
                }
                finally
                {
                    this.RunSync(() =>
                    {
                        if (this.IsDisposed) return;
                        _terminal.SetDataProcessingMode(DataProcessingMode.Automatic);
                        _terminal.UserInputEnabled = true;
                        TerminalStatus = loginSucceeded && IsConnected
                            ? E_TerminalStatus.Connection
                            : E_TerminalStatus.Disconnected;
                    });
                }
            });
        }

        private void ChangeStatusIcon()
        {
            try
            {
                if (AppGlobal.s_IsProgramShutdown) return;

                switch (_terminalStatus)
                {
                    case E_TerminalStatus.Disconnected:
                        SaveDeviceLog("연결 종료 했습니다.");

                        if (ProgreBarHandlerEvent != null)
                            ProgreBarHandlerEvent("디바이스에 연결 종료 되었습니다.", eProgressItemType.Standard, true);

                        AppGlobal.m_TerminalPanel?.tEmulator_OnTerminalStatusChange(this, E_TerminalStatus.Disconnected);

                        if (_terminalMode == E_TerminalMode.QuickClient && AppGlobal.s_ClientOption.IsUseTerminalClose)
                        {
                            if (!(this.Parent is SuperTabControlPanel) && !(this.Parent is TerminalWindows))
                            {
                                //System.Diagnostics.Process.GetCurrentProcess().Kill();
                                Application.Exit();
                                return;
                            }
                        }

                        if (Parent == null || _terminalMode != E_TerminalMode.RACTClient) break;

                        UpdateParentUI(global::RACTClient.Properties.Resources.Disconnect,
                                       global::RACTClient.Properties.Resources.racs_32_disconnect);
                        break;

                    case E_TerminalStatus.Connection:
                        SaveDeviceLog("연결 했습니다.");
                        mnuStopScript.Enabled = false;

                        if (Parent != null && _terminalMode == E_TerminalMode.RACTClient)
                        {
                            UpdateParentUI(global::RACTClient.Properties.Resources.Connect, null);
                        }
                        break;

                    case E_TerminalStatus.RunScript:
                        SaveDeviceLog("스크립트 실행 합니다.");
                        mnuStopScript.Enabled = true;

                        if (Parent != null && _terminalMode == E_TerminalMode.RACTClient)
                        {
                            UpdateParentUI(global::RACTClient.Properties.Resources.ScriptRun, null);
                        }
                        break;

                    case E_TerminalStatus.Recording:
                        SaveDeviceLog("스크립트 저장 합니다.");

                        if (Parent != null && _terminalMode == E_TerminalMode.RACTClient)
                        {
                            UpdateParentUI(global::RACTClient.Properties.Resources.Recoding, null);
                        }
                        break;
                    default:
                        mnuStopScript.Enabled = false;
                        break;
                }

                OnTerminalStatusChange?.Invoke(this, _terminalStatus);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        private void SaveDeviceLog(string log)
        {
            if (_deviceInfo == null) return;

            var connectInfo = _deviceInfo.TerminalConnectInfo;
            if (connectInfo == null) return;

            if (connectInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET ||
                connectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
            {
                AppGlobal.s_FileLogProcessor.PrintLog($"[Telnet] {_deviceInfo.IPAddress}:{connectInfo.TelnetPort} {log}");
            }
            else if (connectInfo.ConnectionProtocol == E_ConnectionProtocol.SERIAL_PORT)
            {
                string portName = connectInfo.SerialConfig != null ? connectInfo.SerialConfig.PortName : "Unknown";
                AppGlobal.s_FileLogProcessor.PrintLog($"[Serial] {portName} {log}");
            }
            else
            {
                AppGlobal.s_FileLogProcessor.PrintLog($"[Device] {_deviceInfo.IPAddress} {log}");
            }
        }



        /// <summary>
        /// 부모 컨트롤(Tab 또는 Window)의 아이콘/이미지를 업데이트하는 헬퍼 함수
        /// </summary>
        private void UpdateParentUI(Image tabImage, Icon windowIcon)
        {
            if (this.Parent is SuperTabControlPanel tabParent)
            {
                if (_terminalStatus == E_TerminalStatus.Disconnected &&
                    AppGlobal.s_ClientOption.IsUseTerminalClose && this.Tag as string != "TabItemClose")
                {
                    tabParent.Dispose();
                }
                else
                {
                    tabParent.TabItem.Image = tabImage;
                }
            }
            else if (this.Parent is TerminalWindows winParent)
            {
                if (_terminalStatus == E_TerminalStatus.Disconnected && AppGlobal.s_ClientOption.IsUseTerminalClose)
                {
                    winParent.Dispose();
                }
                else if (windowIcon != null)
                {
                    winParent.Icon = windowIcon;
                }
            }
        }

        private DeviceConnectionLogOpenResultInfo RequestOpenDeviceConnectionLog(DeviceInfo aDeviceInfo)
        {
            DeviceConnectionLogOpenRequestInfo tRequest = new DeviceConnectionLogOpenRequestInfo();
            tRequest.ClientID = AppGlobal.s_LoginResult.ClientID;
            tRequest.UserID = AppGlobal.s_LoginResult.UserID;
            tRequest.DeviceID = aDeviceInfo.DeviceID;
            tRequest.DeviceIP = aDeviceInfo.IPAddress;
            tRequest.ConnectionKind = GetConnectionKind(aDeviceInfo);
            tRequest.Description = aDeviceInfo.IPAddress + " 장비에 연결 합니다.";

            return AppGlobal.s_DeviceConnectionLogClient.OpenLog(tRequest);
        }


        private int GetConnectionKind(DeviceInfo aDeviceInfo)
        {
            int tConnectionKind = 1;

            if (aDeviceInfo.TerminalConnectInfo.SerialConfig.PortName == "RCCSPort") tConnectionKind = 2;
            if (aDeviceInfo.TerminalConnectInfo.SerialConfig.PortName == "RPCSPort") tConnectionKind = 3;
            if (aDeviceInfo.TerminalConnectInfo.SerialConfig.PortName == "RPCSLTE") tConnectionKind = 4;

            return tConnectionKind;
        }


        #endregion

        #region ISenderObject Implementation

        /// <summary>
        /// AppGlobal(서버)로부터 응답이 왔을 때 호출되는 전역 콜백입니다.
        /// </summary>
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

        /// <summary>
        /// CommandResultItem 수신용 (인터페이스 요구사항, 사용 안 함)
        /// </summary>
        public void ResultReceiver(CommandResultItem result)
        {
        }

        #endregion
    }
}




