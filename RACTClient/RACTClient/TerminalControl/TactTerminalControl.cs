using DevComponents.DotNetBar;
using RACTClient.Adapters;
using RACTClient.Utilities.Extensions;
using RACTClient.Utilities;
using RACTCommonClass;
using RACTSerialProcess;
using RACTTerminal;
using Rebex.TerminalEmulation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using ColorScheme = Rebex.TerminalEmulation.ColorScheme;

namespace RACTClient
{
    internal class TactTerminalControl : TerminalControl, ITactTerminal, ISenderObject
    {        
        private IRebexConnection _currentConnection; // 현재 연결 객체 관리

        private DevComponents.DotNetBar.ContextMenuBar contextMenuBar1;
        private DevComponents.DotNetBar.ButtonItem cmPopUP; // 기존의 cmPopUP 역할
        private DevComponents.DotNetBar.ButtonItem mnuCopy;
        private DevComponents.DotNetBar.ButtonItem mnuPaste;
        private DevComponents.DotNetBar.ButtonItem mnuPasteE;
        private DevComponents.DotNetBar.ButtonItem mnuAutoC;
        private DevComponents.DotNetBar.ButtonItem mnuFind;
        private DevComponents.DotNetBar.ButtonItem mnuSelectAll;
        private DevComponents.DotNetBar.ButtonItem mnuClear;
        private DevComponents.DotNetBar.ButtonItem mnuSearchDefaultCmd;
        private DevComponents.DotNetBar.ButtonItem mnuBatchCmd;
        private DevComponents.DotNetBar.ButtonItem mnuStopScript; // 요청하신 스크립트 취소 메뉴
        private DevComponents.DotNetBar.ButtonItem mnuSaveTerminal;
        private DevComponents.DotNetBar.ButtonItem mnuCmdClear;
        private DevComponents.DotNetBar.ButtonItem mnuOption;

        // 동기화 객체
        private ManualResetEvent m_MRE = new ManualResetEvent(false);

        // [수정] 수신 데이터 타입 명시 (ISenderObject 호환)
        private ResultCommunicationData m_Result;

        private FACT_DefaultConnectionCommandSet m_ConnectionCommandSet;

        public TactTerminalControl()
        {
            InitScriptMenu();
            InitTerminalPalette();
        }

        private void InitTerminalPalette()
        {
            // 기본 팔레트에서 시작
            TerminalPalette palette = TerminalPalette.Ansi;

            // 알려진 색상만 안전하게 설정
            try
            {
                palette.SetColor(0, Color.Black);
                palette.SetColor(7, Color.FromArgb(0xC0, 0xC0, 0xC0));
                palette.SetColor(15, Color.White);
            }
            catch { /* 무시하고 기본값 사용 */ }

            this.Palette = palette;
            this.Refresh();
        }


        /// <summary>
        /// 터미널 상태 입니다.
        /// </summary>
        private E_TerminalStatus m_TerminalStatus = E_TerminalStatus.TryConnection;

        public Control UIControl => this; // 자기 자신(Control)을 반환

        public bool IsConnected
        {
            get
            {
                // 1. 객체가 null이면 당연히 연결 상태가 아님
                if (_currentConnection == null)
                    return false;

                try
                {
                    // 2. 어댑터 내부의 실제 클라이언트 상태와 바인딩 상태를 동시에 체크
                    // _currentConnection 내부의 실제 소켓 상태가 살아있는지 확인합니다.
                    return _currentConnection.IsConnected;
                }
                catch
                {
                    return false;
                }
            }
        }

        private DeviceInfo m_DeviceInfo; // 기존 코드에서 사용하던 변수명
        private E_TerminalMode m_TerminalMode;

        public DeviceInfo DeviceInfo
        {
            get { return m_DeviceInfo; }
            set { m_DeviceInfo = value; }
        }

        public string ToolTip { get; set; }

        public ConnectionTypes ConnectionType { get; set; }

        // 클래스 내부에 세션 ID를 보관할 변수 추가 (연결 성공 시 할당 필요)
        private int m_ConnectedSessionID = 0;

        public int ConnectedSessionID
        {
            // 연결 시 서버로부터 받은 세션 ID가 있다면 반환, 없다면 0
            get { return m_ConnectedSessionID; }
        }

        public bool IsQuickConnection => throw new NotImplementedException();

        public E_TerminalMode TerminalMode { get => m_TerminalMode; set => m_TerminalMode = value; }

        public Mode Modes { get; set; }
        public DaemonProcessInfo DaemonProcessInfo { get; set; }

        public string ComPort => throw new NotImplementedException();

               

        public event HandlerArgument2<ITactTerminal, E_TerminalStatus> OnTerminalStatusChange;
        public event DefaultHandler OnTelnetFindString;
        public event DefaultHandler CallOptionHandlerEvent;
        public event HandlerArgument3<string, eProgressItemType, bool> ProgreBarHandlerEvent;

        /// <summary>
        /// 스크립트 취소 메뉴 클릭 시 동작
        /// </summary>
        private void mnuStopScript_Click(object sender, EventArgs e)
        {
            // 스크립트 중단 로직 실행 (기존 ScriptWork 혹은 ScriptManager 직접 제어)
            ScriptWork(E_ScriptWorkType.RunCancel);

            // UI 즉시 반영을 위해 상태 변경 호출 (필요 시)
            // TerminalStatus = E_TerminalStatus.Connection;
        }

        /// <summary>
        /// 터미널 옵션(색상 및 폰트)을 장비 타입에 따라 적용합니다.
        /// </summary>
        public void ApplyOption()
        {
            Color targetBackColor;
            Color targetForeColor;
            string targetFontName;
            float targetFontSize;
            FontStyle targetFontStyle;

            // 1. 장비 중요도에 따라 설정값 분기 처리
            // 2019-11-10: 집선스위치(1), G-PON-OLT(6), NG-PON-OLT(31)는 강조 색상 사용
            if (DeviceInfo.DevicePartCode == 1 ||
                DeviceInfo.DevicePartCode == 6 ||
                DeviceInfo.DevicePartCode == 31)
            {
                // [중요 장비 설정]
                targetBackColor = AppGlobal.s_ClientOption.HighlightBackGroundColor;
                targetForeColor = AppGlobal.s_ClientOption.HighlightFontColor;
                targetFontName = AppGlobal.s_ClientOption.HighlightFontName;
                targetFontSize = AppGlobal.s_ClientOption.HighlightFontSize;
                targetFontStyle = AppGlobal.s_ClientOption.HighlightFontStyle;
            }
            else
            {
                // [일반 장비 설정]
                targetBackColor = AppGlobal.s_ClientOption.TerminalBackGroundColor;
                targetForeColor = AppGlobal.s_ClientOption.TerminalFontColor;
                targetFontName = AppGlobal.s_ClientOption.TerminalFontName;
                targetFontSize = AppGlobal.s_ClientOption.TerminalFontSize;
                targetFontStyle = AppGlobal.s_ClientOption.TerminalFontStyle;
            }

            // 2. 폰트 이름 보정 (한글 폰트명 처리)
            if (string.IsNullOrEmpty(targetFontName))
            {
                targetFontName = "Consolas"; // 기본값
            }
            else if (targetFontName.Equals("굴림") || targetFontName.Equals("돋움") ||
                     targetFontName.Equals("궁서") || targetFontName.Equals("바탕"))
            {
                targetFontName += "체";
            }

            // 3. 색상 적용 (Rebex Palette 적용)
            ApplyTerminalTheme(targetBackColor, targetForeColor);

            // 4. 폰트 적용
            // 폰트 생성 실패 방지를 위한 예외처리 포함
            try
            {
                this.Font = new Font(targetFontName, targetFontSize, targetFontStyle, GraphicsUnit.Point, ((byte)(0)));
                this.TerminalFont = new TerminalFont(targetFontName, targetFontSize);
            }
            catch (Exception)
            {
                // 폰트 생성 실패 시 기본 폰트로 대체
                this.Font = new Font("Consolas", 10, FontStyle.Regular);
                this.TerminalFont = new TerminalFont("Consolas", 10);
            }

            this.Refresh();
        }

        /// <summary>
        /// Rebex 터미널의 팔레트와 옵션을 수정하여 지정된 배경/글자색을 적용합니다.
        /// (기존 ApplyCustomColors 개선)
        /// </summary>
        /// <param name="backColor">적용할 배경색</param>
        /// <param name="foreColor">적용할 글자색</param>
        private void ApplyTerminalTheme(Color backColor, Color foreColor)
        {
            // 1. [옵션 준비] 현재 옵션 가져오기
            TerminalOptions options = this.Options;
            options.ColorScheme = ColorScheme.Custom;

            // 2. [팔레트 생성]
            TerminalPalette newPalette = new TerminalPalette();

            // 3. [기본 색상 채우기] 0~13번은 표준 ANSI 색상 유지 (명령어 출력 등 가독성 보장)
            newPalette.SetColor(0, Color.Black);
            newPalette.SetColor(1, Color.Maroon);
            newPalette.SetColor(2, Color.Green);
            newPalette.SetColor(3, Color.Olive);
            newPalette.SetColor(4, Color.Navy);
            newPalette.SetColor(5, Color.Purple);
            newPalette.SetColor(6, Color.Teal);
            newPalette.SetColor(7, Color.Silver);
            newPalette.SetColor(8, Color.Gray);
            newPalette.SetColor(9, Color.Red);
            newPalette.SetColor(10, Color.Lime);
            newPalette.SetColor(11, Color.Yellow);
            newPalette.SetColor(12, Color.Blue);
            newPalette.SetColor(13, Color.Magenta);

            // 4. [사용자 색상 검증]
            // 색상이 설정되지 않았거나(Empty) 투명한 경우 기본값 처리
            if (backColor == Color.Empty || backColor == Color.Transparent) backColor = Color.White;
            if (foreColor == Color.Empty || foreColor == Color.Transparent) foreColor = Color.Black;

            // 5. [핵심] 14번을 배경, 15번을 글자색으로 정의
            newPalette.SetColor(14, backColor); // 14번 슬롯: 배경
            newPalette.SetColor(15, foreColor); // 15번 슬롯: 글자

            // 6. [옵션 연결] 터미널에게 14번과 15번을 쓰라고 지시
            options.SetColorIndex(SchemeColorName.Background, 14);
            options.SetColorIndex(SchemeColorName.Foreground, 15);

            // 7. [최종 적용] 
            this.Palette = newPalette;
            this.Options = options;

            // 8. WinForm 컨트롤 배경색 동기화 (여백 처리)
            this.BackColor = backColor;
            this.ForeColor = foreColor; // WinForm 컨트롤 속성도 맞춰줌

            this.Invalidate();
        }

        /// <summary>
        /// 터미널에 SecureCRT 스타일(연한 베이지 배경 + 회색 글자) 테마를 적용합니다.
        /// </summary>
        private void ApplySecureCrtTheme()
        {
            // 1. [폰트] Consolas 10pt (없으면 Courier New)
            try { this.Font = new Font("Consolas", 10, FontStyle.Regular); }
            catch { this.Font = new Font("Courier New", 10, FontStyle.Regular); }

            // 2. [커서] 블록 스타일 & 깜빡임
            this.CursorStyle = Rebex.TerminalEmulation.CursorStyle.Block;
            this.CursorBlinkingInterval = 500;

            // 3. [이벤트] 커서 색상 핸들러 연결 (중복 방지)
            this.CursorColor -= TerminalControl_CursorColor;
            //this.CursorColor += TerminalControl_CursorColor;

            // 4. [팔레트] SecureCRT 색상 정의
            TerminalOptions options = this.Options;
            options.ColorScheme = ColorScheme.Custom;
            TerminalPalette newPalette = new TerminalPalette();

            // 0~7번 (Normal)
            newPalette.SetColor(0, Color.FromArgb(64, 64, 64));      // 0: 진한 회색 (커서 배경)
            newPalette.SetColor(1, Color.FromArgb(187, 0, 0));       // 1: Red
            newPalette.SetColor(2, Color.FromArgb(0, 187, 0));       // 2: Green
            newPalette.SetColor(3, Color.FromArgb(187, 187, 0));     // 3: Yellow
            newPalette.SetColor(4, Color.FromArgb(0, 0, 187));       // 4: Blue
            newPalette.SetColor(5, Color.FromArgb(187, 0, 187));     // 5: Magenta
            newPalette.SetColor(6, Color.FromArgb(0, 187, 187));     // 6: Cyan
            newPalette.SetColor(7, Color.FromArgb(255, 255, 238));   // 7: 베이지 (기본 배경 & 커서 글자)

            // 8~15번 (Bright) - 8번은 Bold용 검정
            newPalette.SetColor(8, Color.Black);
            newPalette.SetColor(9, Color.Red);
            newPalette.SetColor(10, Color.Lime);
            newPalette.SetColor(11, Color.FromArgb(255, 255, 0));
            newPalette.SetColor(12, Color.Blue);
            newPalette.SetColor(13, Color.Magenta);
            newPalette.SetColor(14, Color.Cyan);
            newPalette.SetColor(15, Color.White);

            // 5. [적용] 팔레트 교체
            this.Palette = newPalette;

            // 6. [옵션] 색상 매핑
            options.SetColorIndex(SchemeColorName.Foreground, 0); // 기본 글자 -> 0 (회색)
            options.SetColorIndex(SchemeColorName.Background, 7); // 기본 배경 -> 7 (베이지)
            options.SetColorIndex(SchemeColorName.Bold, 8);       // 강조 글자 -> 8 (검정)

            // 7. [완료] 적용 및 갱신
            this.Options = options;
            this.BackColor = newPalette.GetColor(7); // 여백 색상 일치

            this.CursorStyle = Rebex.TerminalEmulation.CursorStyle.Block; // 1. 모양: 블록
            this.CursorBlinkingInterval = 500; // 2. 속도: 500ms (0.5초) - 설정 안하면 안 깜빡일 수 있음
            this.Invalidate();
        }

        private void ApplyCustomColors()
        {
            // 1. [옵션 준비] 현재 옵션 가져오기
            TerminalOptions options = this.Options;
            options.ColorScheme = ColorScheme.Custom;

            // 2. [팔레트 생성] 수정 가능한 새 팔레트 통을 만듭니다.
            TerminalPalette newPalette = new TerminalPalette();

            // 3. [중요] 표준 색상(0~13번) 채우기
            // 이 과정이 없으면 기본 명령어나 디렉토리 색상이 검정색으로 나와서 안 보입니다.
            // System.Drawing.Color를 사용하므로 오류가 나지 않습니다.
            newPalette.SetColor(0, Color.Black);
            newPalette.SetColor(1, Color.Maroon);
            newPalette.SetColor(2, Color.Green);
            newPalette.SetColor(3, Color.Olive);
            newPalette.SetColor(4, Color.Navy);
            newPalette.SetColor(5, Color.Purple);
            newPalette.SetColor(6, Color.Teal);
            newPalette.SetColor(7, Color.Silver);
            newPalette.SetColor(8, Color.Gray);
            newPalette.SetColor(9, Color.Red);
            newPalette.SetColor(10, Color.Lime);
            newPalette.SetColor(11, Color.Yellow);
            newPalette.SetColor(12, Color.Blue);
            newPalette.SetColor(13, Color.Magenta);
            // (14, 15번은 사용자 색상을 위해 비워둡니다)

            // 4. [사용자 색상 주입] AppGlobal에서 색상 가져오기
            Color userBackColor = AppGlobal.s_ClientOption.HighlightBackGroundColor; // 배경색 설정값
            Color userForeColor = AppGlobal.s_ClientOption.HighlightFontColor; // 글자색 설정값 (변수명 확인 필요)

            // 만약 글자색 설정 변수가 따로 없다면 임시로 지정 (예: 흰색 or 반전색)
            if (userForeColor == Color.Empty) userForeColor = Color.White;

            // 5. [핵심] 14번을 배경, 15번을 글자색으로 정의
            newPalette.SetColor(14, userBackColor); // 14번 슬롯: 배경
            newPalette.SetColor(15, userForeColor); // 15번 슬롯: 글자

            // 6. [옵션 연결] 터미널에게 14번과 15번을 쓰라고 지시
            options.SetColorIndex(SchemeColorName.Background, 14); // 배경 -> 14번
            options.SetColorIndex(SchemeColorName.Foreground, 15); // 글자 -> 15번

            // 7. [최종 적용] 
            // 순서 중요: 팔레트를 먼저 갈아끼우고 옵션을 적용합니다.
            this.Palette = newPalette;
            this.Options = options;

            // 8. 화면 갱신 및 커서 색상 동기화
            this.BackColor = userBackColor; // 터미널 여백 색상도 맞춤
            this.Invalidate();
        }

        // 커서 색상 이벤트 핸들러 (최종 수정)
        private void TerminalControl_CursorColor(object sender, Rebex.TerminalEmulation.CursorColorEventArgs e)
        {
            // [SecureCRT 스타일 커서 로직]
            // 1. e.BackColor: 커서 블록(네모)의 색상 -> 진한 회색 (Index 0)
            // 2. e.ForeColor: 커서 안의 글자 색상 -> 연한 베이지 (Index 7)

            // 이렇게 설정하면 커서가 글자를 덮을 때, 
            // 글자는 베이지색, 배경은 회색이 되어 가독성이 완벽해집니다.
            e.BackColor = 0;
            e.ForeColor = 7;

            // (참고)
            // e.CellForeColor / e.CellBackColor는 "원래 글자의 색"을 알려주는 읽기 전용 속성일 가능성이 큽니다.
            // 예: "원래 글자가 빨간색일 때만 커서를 빨갛게 하라" 같은 로직을 짤 때 사용합니다.
            // 우리는 테마를 고정할 것이므로 위처럼 0과 7로 덮어쓰면 됩니다.
        }

        public void ChangeClientMode()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 장비 접속을 수행합니다. (Rebex 통합 최종 구현)
        /// </summary>
        public object ConnectDevice(object aDeviceInfo)
        {
            DeviceInfo = new DeviceInfo((DeviceInfo)aDeviceInfo);

            // [추가] 재연결 시 이전 터미널 화면을 깨끗하게 비움
            this.SafeInvoke(() => {
                this.Screen.Clear(); // 전체 화면 지움
                this.Screen.SetCursorPosition(0, 0); // 커서를 좌측 상단으로 이동
            });

            m_ConnectionCommandSet = null;

            // [Legacy Logic] 2023-06-13 VOIP AGW PORT 2001 치환
            if (m_DeviceInfo.DevicePartCode == 13)
            {
                if (m_DeviceInfo.ModelID != 3727)
                    m_DeviceInfo.TerminalConnectInfo.TelnetPort = 2001;
            }

            // [Legacy Logic] 터미널 로그 시작 (파일 저장 등)
            try
            {
                // StartTerminalLog는 TactTerminalControl 또는 부모 클래스에 정의되어 있어야 함
                StartTerminalLog(m_DeviceInfo);
            }
            catch (Exception ex)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, "Log Start Failed: " + ex.Message);
            }


            // 1. 기존 연결 종료 및 상태 초기화
            Disconnect();
            TerminalStatus = E_TerminalStatus.TryConnection;

            // 3. Rebex 연결 (SSH / TELNET) - 비동기 실행으로 UI Freezing 방지
            Task.Run(() =>
            {

                try
                {
                    // 3-1. 프로토콜에 따른 어댑터 생성
                    if (DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
                    {
                        ConnectionType = ConnectionTypes.RemoteTelnet; // Enum 확인 필요 why ssh is RemoteTelnet?
                        _currentConnection = new SshConnectionAdapter();
                    }
                    else
                    {
                        ConnectionType = ConnectionTypes.RemoteTelnet;
                        _currentConnection = new TelnetConnectionAdapter(
                            DeviceInfo.IPAddress,
                            DeviceInfo.TerminalConnectInfo.TelnetPort
                        );
                    }


                    // 3-2. 연결 수행 (내부적으로 RebexProxyFactory를 통해 프록시 자동 적용)
                    // [Source Reference: Connect 메서드 내부에서 Factory 호출]
                    int port = (DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
                               ? 22
                               : DeviceInfo.TerminalConnectInfo.TelnetPort;

                    _currentConnection.Connect(DeviceInfo.IPAddress, port);

                    // 3-3. SSH 로그인 수행 (Telnet은 Scripting으로 후처리)
                    if (_currentConnection is SshConnectionAdapter)
                    {
                        _currentConnection.Login(DeviceInfo.USERID, DeviceInfo.PWD);
                    }

                    // 3-4. UI 스레드에서 터미널 바인딩 (SafeInvoke 사용)
                    this.SafeInvoke(() =>
                    {
                        // 연결된 객체(Ssh/Telnet)를 Rebex TerminalControl에 바인딩
                        this.Bind(_currentConnection.GetClientObject());
                        
                        // [추가] 서버측 종료 감지를 위한 이벤트 핸들러 연결
                        this.Disconnected += TactTerminalControl_Disconnected;

                        //IsConnected = true;
                        TerminalStatus = E_TerminalStatus.Connection;

                        ChangeStatusIcon(); // [Source Reference: Line 382]

                        // 로그 기록
                        AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation,
                            $"Connected to {DeviceInfo.IPAddress} ({ConnectionType})");
                    });

                    // 3-5. Telnet 자동 로그인 스크립트 실행
                    if (_currentConnection is TelnetConnectionAdapter)
                    {
                        // 바인딩이 완료된 후 실행해야 하므로 UI 업데이트가 끝날 때까지 기다리거나
                        // SafeInvoke 내부 로직이 완료된 시점에 호출되어야 함.
                        // SafeInvoke는 비동기(Post)이므로, 순차 보장을 위해 별도 메서드 호출
                        //PerformTelnetLogin(DeviceInfo.TelnetID1, DeviceInfo.TelnetPwd1);
                    }
                }
                catch (Exception ex)
                {
                    // 연결 실패 처리
                    this.SafeInvoke(() =>
                    {
                        TerminalStatus = E_TerminalStatus.Disconnected;
                        AppGlobal.ShowMessage(AppGlobal.s_ClientMainForm,
                            "접속 실패: " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);

                        AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error,
                            $"Connection Error ({DeviceInfo.IPAddress}): {ex}");
                    });

                    // 자원 해제
                    if (_currentConnection != null) _currentConnection.Dispose();
                }
            });

            return null;
        }

        private void StartTerminalLog(DeviceInfo info)
        {
            // 기존 소스 코드에 있는 로직을 그대로 사용하거나,
            // AppGlobal.s_TerminalExecuteLogProcess 등을 호출
            // 예: AppGlobal.s_FileLogProcessor.StartLog(info.IPAddress);
        }

        /// <summary>
        /// 터미널 상태 가져오거나 설정 합니다.
        /// </summary>
        public E_TerminalStatus TerminalStatus
        {
            get { return m_TerminalStatus; }
            set
            {
                if (m_TerminalStatus != value)
                {
                    m_TerminalStatus = value;
                    if (m_TerminalStatus == E_TerminalStatus.Connection && AppGlobal.s_ClientOption.IsUseTerminalAutoLogin)
                    {
                        StartLoginProcess();
                    }
                }
                ChangeStatusIcon();
            }
        }

        // =============================================================
        // 1. ISenderObject 인터페이스 구현 (이름 수정됨: SetCommunicationResult -> ResultReceiver)
        // =============================================================

        /// <summary>
        /// AppGlobal(서버)로부터 응답이 왔을 때 호출되는 콜백입니다.
        /// </summary>
        public void ResultReceiver(ResultCommunicationData vResult)
        {
            this.m_Result = vResult;

            // 대기 중인 스레드(WaitOne)를 깨웁니다.
            try
            {
                m_MRE.Set();
            }
            catch { }
        }

        /// <summary>
        /// CommandResultItem 수신용 (인터페이스 요구사항, 사용 안 함)
        /// </summary>
        public void ResultReceiver(CommandResultItem vResult)
        {
            // 필요 시 구현, 현재는 비워둠
        }

        // =============================================================
        // 2. StartLoginProcess (메인 로직)
        // =============================================================

        private void StartLoginProcess()
        {
            Task.Run(() =>
            {
                try
                {
                    var protocol = m_DeviceInfo.TerminalConnectInfo.ConnectionProtocol;

                    // Telnet 또는 SSH 접속이고, 자동 로그인이 필요한 상황인지 체크
                    if ((protocol == E_ConnectionProtocol.TELNET || protocol == E_ConnectionProtocol.SSHTelnet) &&
                        AppGlobal.s_RACTClientMode == E_RACTClientMode.Online &&
                        m_DeviceInfo.IsRegistered &&
                        m_ConnectionCommandSet == null)
                    {
                        LogInfo("기본 접속 정보를 로드합니다.");

                        // [수정] 분리된 헬퍼 메서드 호출
                        if (!RequestConnectionCommandSet())
                        {
                            return; // 로드 실패 시 중단
                        }

                        // SSH는 정보만 로드하고 스크립트 실행은 안 함 (SshConnectionAdapter에서 처리)
                        if (protocol == E_ConnectionProtocol.SSHTelnet) return;

                        // TELNET 스크립트 실행
                        ExecuteTelnetLoginScript();
                    }
                }
                catch (Exception ex)
                {
                    LogError("StartLoginProcess Error: " + ex.Message);
                }
            });
        }

        // =============================================================
        // 3. RequestConnectionCommandSet (통신 및 대기 로직 분리)
        // =============================================================

        private bool RequestConnectionCommandSet()
        {
            try
            {
                // 1. 요청 패킷 생성
                RequestCommunicationData tRequestData = AppGlobal.MakeDefaultRequestData();
                tRequestData.CommType = E_CommunicationType.RequestDefaultConnectionCommand;
                tRequestData.RequestData = m_DeviceInfo;

                // TL1 포트(1023) 특수 처리
                if (m_DeviceInfo.TerminalConnectInfo.TelnetPort == 1023)
                {
                    tRequestData.UserData = "TL1";
                }

                // 2. 초기화 (결과 비우기, 신호등 빨간불)
                m_Result = null;
                m_MRE.Reset();

                // 3. 요청 전송 (this는 ISenderObject이므로 에러 없음)
                AppGlobal.SendRequestData(this, tRequestData);

                // 4. 대기 (타임아웃 적용)
                if (!m_MRE.WaitOne(AppGlobal.s_RequestTimeOut))
                {
                    LogWarning($"기본 접속 명령 로드 시간 초과. (IP: {m_DeviceInfo.IPAddress})");
                    return false;
                }

                // 5. 결과 유효성 검사
                if (m_Result == null || m_Result.Error.Error != E_ErrorType.NoError)
                {
                    LogWarning($"기본 접속 명령 로드 실패. (IP: {m_DeviceInfo.IPAddress})");
                    return false;
                }

                // 6. 데이터 캐스팅 및 저장
                m_ConnectionCommandSet = m_Result.ResultData as FACT_DefaultConnectionCommandSet;

                if (m_ConnectionCommandSet == null || m_ConnectionCommandSet.CommandList.Count == 0)
                {
                    LogWarning("접속 명령 정보가 비어 있습니다.");
                    return false;
                }

                // 7. 에러 메시지 초기화 (기존 로직 유지)
                foreach (var cmd in m_ConnectionCommandSet.CommandList)
                {
                    cmd.ErrorString = "Login incorrect";
                }

                return true;
            }
            catch (Exception ex)
            {
                LogError("RequestConnectionCommandSet Exception: " + ex.Message);
                return false;
            }
        }

        // =============================================================
        // 4. ExecuteTelnetLoginScript (스크립트 실행 로직)
        // =============================================================

        private void ExecuteTelnetLoginScript()
        {
            // UI 제어 (화면 갱신 중지, 입력 차단)
            this.RunSync(() =>
            {
                this.SetDataProcessingMode(DataProcessingMode.None);
                this.UserInputEnabled = false;
                TerminalStatus = E_TerminalStatus.RunScript;
            });

            try
            {
                // [확장 메서드 사용] RebexScriptingExtensions에 구현된 로직 호출
                // 1023 포트 여부는 내부에서 m_DeviceInfo를 통해 판단됨
                this.Scripting.ExecuteConnectionCommand(m_ConnectionCommandSet, m_DeviceInfo);

                LogInfo("자동 로그인 완료.");
            }
            catch (Exception ex)
            {
                LogError("Script Execution Failed: " + ex.Message);
            }
            finally
            {
                // UI 복구
                this.SafeInvoke(() =>
                {
                    if (!this.IsDisposed)
                    {
                        this.SetDataProcessingMode(DataProcessingMode.Automatic);
                        this.UserInputEnabled = true;
                        TerminalStatus = E_TerminalStatus.Connection;
                        this.Focus();
                    }
                });
            }
        }

        // ---------------------------------------------------------------------
        // 로그 헬퍼 메서드
        // ---------------------------------------------------------------------
        private void LogInfo(string msg)
        {
            this.SafeInvoke(() => AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, msg));
        }

        private void LogWarning(string msg)
        {
            this.SafeInvoke(() => {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, msg);
                // 필요 시 연결 종료 처리
                // TerminalStatus = E_TerminalStatus.Disconnected; 
            });
        }

        private void LogError(string msg)
        {
            this.SafeInvoke(() => AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, msg));
        }

        private void ChangeStatusIcon()
        {
            try
            {
                if (AppGlobal.s_IsProgramShutdown) return;

                // 신규 코드의 IsConnected 프로퍼티가 실시간 상태를 반환하므로 
                // m_IsConnected 필드를 직접 제어하기보다 현재 상태를 로그나 UI에 반영하는 데 집중합니다.

                switch (m_TerminalStatus)
                {
                    case E_TerminalStatus.Disconnected:
                        // [수정] 신규 IsConnected 로직을 따르므로 m_IsConnected = false 할당은 불필요할 수 있으나,
                        // 하위 호환성을 위해 유지하거나 필요 시 제거 가능합니다.

                        SaveDeviceLog("연결 종료 했습니다.");

                        if (ProgreBarHandlerEvent != null)
                            ProgreBarHandlerEvent("디바이스에 연결 종료 되었습니다.", eProgressItemType.Standard, true);

                        // 터미널 패널 상태 업데이트 알림
                        AppGlobal.m_TerminalPanel?.tEmulator_OnTerminalStatusChange(this, E_TerminalStatus.Disconnected);

                        // QuickClient 모드 및 자동 종료 로직
                        if (m_TerminalMode == E_TerminalMode.QuickClient && AppGlobal.s_ClientOption.IsUseTerminalClose)
                        {
                            if (!(this.Parent is SuperTabControlPanel) && !(this.Parent is TerminalWindows))
                            {
                                System.Diagnostics.Process.GetCurrentProcess().Kill();
                                return;
                            }
                        }

                        if (Parent == null || m_TerminalMode != E_TerminalMode.RACTClient) break;

                        // UI 아이콘 업데이트 (Tab/Window)
                        UpdateParentUI(global::RACTClient.Properties.Resources.Disconnect,
                                       global::RACTClient.Properties.Resources.racs_32_disconnect);
                        break;

                    case E_TerminalStatus.Connection:
                        SaveDeviceLog("연결 했습니다.");
                        mnuStopScript.Enabled = false;

                        if (Parent != null && m_TerminalMode == E_TerminalMode.RACTClient)
                        {
                            UpdateParentUI(global::RACTClient.Properties.Resources.Connect, null);
                        }
                        break;

                    case E_TerminalStatus.RunScript:
                        SaveDeviceLog("스크립트 실행 합니다.");
                        mnuStopScript.Enabled = true;

                        if (Parent != null && m_TerminalMode == E_TerminalMode.RACTClient)
                        {
                            UpdateParentUI(global::RACTClient.Properties.Resources.ScriptRun, null);
                        }
                        break;

                    case E_TerminalStatus.Recording:
                        SaveDeviceLog("스크립트 저장 합니다.");

                        if (Parent != null && m_TerminalMode == E_TerminalMode.RACTClient)
                        {
                            UpdateParentUI(global::RACTClient.Properties.Resources.Recoding, null);
                        }
                        break;
                    default:
                        // 그 외 모든 상태에서는 비활성화
                        mnuStopScript.Enabled = false;
                        // ... 기존 UI 처리 로직 ...
                        break;
                }

                // 상태 변경 이벤트 전파
                OnTerminalStatusChange?.Invoke(this, m_TerminalStatus);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// 부모 컨트롤(Tab 또는 Window)의 아이콘/이미지를 업데이트하는 헬퍼 함수
        /// </summary>
        private void UpdateParentUI(Image tabImage, Icon windowIcon)
        {
            if (this.Parent is SuperTabControlPanel tabParent)
            {
                if (m_TerminalStatus == E_TerminalStatus.Disconnected &&
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
                if (m_TerminalStatus == E_TerminalStatus.Disconnected && AppGlobal.s_ClientOption.IsUseTerminalClose)
                {
                    winParent.Dispose();
                }
                else if (windowIcon != null)
                {
                    winParent.Icon = windowIcon;
                }
            }
        }

        private void InitializeRebexMode()
        {
            throw new NotImplementedException();
        }

        private void InitializeLegacyMode()
        {
            throw new NotImplementedException();
        }

        private bool IsLegacyTarget(DeviceInfo aDeviceInfo)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Telnet 접속 시 스크립트를 이용해 자동으로 로그인합니다.
        /// </summary>
        public void PerformTelnetLogin(string userId, string password)
        {
            // [중요 1] 자동 화면 갱신 일시 중지 (Source: TerminalScripting_MainForm.cs [3])
            // 이를 설정하지 않으면 Scripting 객체가 데이터를 수신하기 전에 터미널 화면이 데이터를 소비해버립니다.
            // 1. RunSync를 사용하여 모드 변경이 "완료될 때까지" 대기
            // (Race Condition 방지)
            this.RunSync(() =>
            {
                this.SetDataProcessingMode(DataProcessingMode.None);
                this.UserInputEnabled = false; // 스크립트 실행 중 사용자 키보드 입력 차단
            });

            // [중요 2] 스크립트는 Blocking 방식이므로 반드시 백그라운드 스레드에서 실행 (UI Freezing 방지)
            Task.Run(() =>
            {
                try
                {
                    // Rebex Scripting 객체 가져오기
                    var scripting = this.Scripting;

                    // 기본 타임아웃 설정 (ms)
                    //int timeout = 5000;

                    // 1. 로그인 프롬프트 대기 ("login:", "Login:", "User Name:" 등을 포괄하기 위해 "ogin:" 사용)
                    // (Source: TerminalScripting_MainForm.cs [4] "ogin:")
                    this.Scripting.WaitFor(ScriptEvent.FromString("ogin:"));

                    // 2. ID 전송
                    scripting.SendCommand(userId);

                    // 3. 비밀번호 프롬프트 대기 ("Password:", "password:" 등을 포괄하기 위해 "assword:" 사용)
                    // (Source: TerminalScripting_MainForm.cs [4] "assword:")
                    scripting.WaitFor(ScriptEvent.FromString("assword:"));

                    // 4. 비밀번호 전송
                    scripting.SendCommand(password);

                    // 5. (선택사항) 로그인 성공 여부 확인을 위해 프롬프트 감지 시도
                    // scripting.DetectPrompt(); 
                }
                catch (TerminalException ex) // [Source: 508] Rebex 터미널 관련 예외 처리
                {
                    this.SafeInvoke(() =>
                    {
                        AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, "Telnet Login Script TerminalError: " + ex.Message);
                    });
                }
                catch (Exception ex) // 타임아웃 등 기타 예외 처리
                {
                    this.SafeInvoke(() =>
                    {
                        AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, "Telnet Login Script Error: " + ex.Message);
                    });
                }
                finally
                {
                    // [중요] 스크립트 종료 후 반드시 화면 갱신 모드 복구 [Source: 495]
                    this.SafeInvoke(() =>
                    {
                        if (!this.IsDisposed)
                        {
                            this.SetDataProcessingMode(DataProcessingMode.Automatic);
                            this.UserInputEnabled = true;
                            this.Focus();

                            // 로그인 완료 후 터미널 상태 갱신 필요 시 호출
                            // ChangeStatusIcon(); 
                        }
                    });
                }
            });
        }

        public void Disconnect()
        {
            try
            {
                // 이벤트 핸들러 제거 (중복 처리 방지)
                this.Disconnected -= TactTerminalControl_Disconnected;

                // [추가] 수동 종료 시 메시지 출력
                if (this.IsConnected)
                {
                    this.Screen.Write("\r\n[RACT] Session disconnected by user.\r\n");
                }

                this.Unbind();

                if (_currentConnection != null)
                {
                    _currentConnection.Dispose();
                    _currentConnection = null;
                }

                m_TerminalStatus = E_TerminalStatus.Disconnected;

                this.SafeInvoke(() => {
                    ChangeStatusIcon();
                });
            }
            catch (Exception ex)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, $"Disconnect Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Rebex 터미널 엔진에서 연결 종료(exit 등)를 감지했을 때 호출됩니다.
        /// </summary>
        /// <summary>
        /// Rebex 터미널 엔진에서 연결 종료(exit 등)를 감지했을 때 호출됩니다.
        /// </summary>
        private void TactTerminalControl_Disconnected(object sender, EventArgs e)
        {
            this.Disconnected -= TactTerminalControl_Disconnected;

            this.SafeInvoke(() =>
            {
                // 1. 터미널 화면에 종료 메시지 출력 (사용자 알림)
                // \r\n을 사용하여 새로운 라인에 출력합니다.
                this.Screen.Write("\r\n[RACT] Connection closed by foreign host.\r\n");

                // 2. 내부 상태 업데이트
                TerminalStatus = E_TerminalStatus.Disconnected;

                // 3. 어댑터 자원 정리
                if (_currentConnection != null)
                {
                    _currentConnection.Dispose();
                    _currentConnection = null;
                }

                SaveDeviceLog("서버에 의해 연결이 종료되었습니다.");

                // 4. 언바인딩 처리 (Rebex UI 스트림 연결 해제)
                this.Unbind();
            });
        }

        public void DisplayResult(int aSessionID, string aResult)
        {
            throw new NotImplementedException();
        }

        public void DisplayResult(SerialCommandResultInfo aResult)
        {
            throw new NotImplementedException();
        }

        public void ExecTerminalScreen(E_TerminalScreenTextEditType aEditType)
        {
            switch (aEditType)
            {
                case E_TerminalScreenTextEditType.Copy:
                    mnuCopy_Click(null, null);
                    break;
                case E_TerminalScreenTextEditType.Clear:
                    mnuClear_Click(null, null);
                    break;
                // 2019-11-10 ???? (??? ??? ??? ?? ??_???  )
                case E_TerminalScreenTextEditType.CmdClear:
                    mnuCmdClear_Click(null, null);
                    break;
                case E_TerminalScreenTextEditType.Find:
                    mnuFind_Click(null, null);
                    break;
                case E_TerminalScreenTextEditType.Paste:
                    mnuPaste_Click(null, null);
                    break;
                case E_TerminalScreenTextEditType.PasteCR:
                    mnuPasteCR_Click(null, null);
                    break;
                case E_TerminalScreenTextEditType.SelectAll:
                    mnuSelectAll_Click(null, null);
                    break;
                case E_TerminalScreenTextEditType.AutoC:
                    mnuAutoC_Click(null, null);
                    break;
                case E_TerminalScreenTextEditType.SearchCmd:
                    mnuSearchDefaultCmd_Click(null, null);
                    break;
            }
        }

        public void FindForm_Close()
        {
            throw new NotImplementedException();
        }

        public void FindForm_OnTelnetStringFind(TelnetStringFindHandlerArgs aArgs)
        {
            throw new NotImplementedException();
        }

        public void IsLimitCmdForShortenCommand(object aSender, string aText)
        {
            throw new NotImplementedException();
        }

        public void RunScript(Script aScript)
        {
            throw new NotImplementedException();
        }

        public void ScriptWork(E_ScriptWorkType aScriptWorkType)
        {
            throw new NotImplementedException();
        }

        public void WriteTerminalLog()
        {
            throw new NotImplementedException();
        }

        private void SaveDeviceLog(string aLog)
        {
            // m_DeviceInfo가 null이면 로그를 남길 수 없으므로 방어 코드 추가
            if (m_DeviceInfo == null) return;

            string logPrefix = string.Empty;
            var connectInfo = m_DeviceInfo.TerminalConnectInfo;

            if (connectInfo == null) return;

            // 기존 로직 구조 유지
            if (connectInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET ||
                connectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
            {
                // IP와 Port 정보를 조합하여 출력
                AppGlobal.s_FileLogProcessor.PrintLog(string.Concat("[Telnet] ", m_DeviceInfo.IPAddress, ":", connectInfo.TelnetPort, " ", aLog));
            }
            else if (connectInfo.ConnectionProtocol == E_ConnectionProtocol.SERIAL_PORT)
            {
                // Serial의 경우 PortName 출력 (SerialConfig null 체크 추가)
                string portName = connectInfo.SerialConfig != null ? connectInfo.SerialConfig.PortName : "Unknown";
                AppGlobal.s_FileLogProcessor.PrintLog(string.Concat("[Serial] ", portName, " ", aLog));
            }
            else
            {
                // 그 외 기본 로그
                AppGlobal.s_FileLogProcessor.PrintLog(string.Concat("[Device] ", m_DeviceInfo.IPAddress, " ", aLog));
            }
        }

        /// <summary>
        /// 스크립트 제어 관련 메뉴 항목을 초기화합니다.
        /// </summary>
        private void InitScriptMenu()
        {
            MakeContextMenu();
            if (this.cmPopUP == null) return;

            // 1. 메뉴 객체 생성 및 설정
            this.mnuStopScript = new DevComponents.DotNetBar.ButtonItem
            {
                Name = "mnuStopScript",
                Text = "스크립트 취소",
                BeginGroup = true, // 메뉴 그룹 구분선 생성
                Enabled = false    // 기본적으로는 비활성 상태
            };

            // 2. 아이콘 설정 (리소스 확인 필요)
            try
            {
                this.mnuStopScript.ImageSmall = (Image)global::RACTClient.Properties.Resources.run_cancel;
            }
            catch
            {
                // 리소스가 없을 경우 대비
                System.Diagnostics.Debug.WriteLine("Warning: run_cancel resource not found.");
            }

            // 3. 이벤트 핸들러 연결
            this.mnuStopScript.Click += new EventHandler(mnuStopScript_Click);

            // 4. 컨텍스트 메뉴(cmPopUP)에 추가
            // mnuSearchDefaultCmd(보통 뒤에서 3~4번째) 위치를 고려하여 추가하거나 단순 Add 처리
            this.cmPopUP.SubItems.Add(this.mnuStopScript);
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

        private void mnuSaveTerminal_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void mnuOption_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void mnuBatchCmd_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #region Context Menu Event Handlers

        private void mnuCopy_Click_Event(object sender, EventArgs e)
        {
            if (TerminalMode == E_TerminalMode.QuickClient)
                mnuCopy_Click(sender, e); // 내부 복사 로직 호출
            else
                AppGlobal.terminalPanel1?.ExecTerminalScreen(E_TerminalScreenTextEditType.Copy);
        }

        private void mnuCopy_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void mnuPaste_Click_Event(object sender, EventArgs e)
        {
            if (TerminalMode == E_TerminalMode.QuickClient)
                mnuPaste_Click(sender, e);
            else
                AppGlobal.terminalPanel1?.ExecTerminalScreen(E_TerminalScreenTextEditType.Paste);
        }

        private void mnuPasteCR_Click_Event(object sender, EventArgs e)
        {
            if (TerminalMode == E_TerminalMode.QuickClient)
                mnuPasteCR_Click(sender, e);
            else
                AppGlobal.terminalPanel1?.ExecTerminalScreen(E_TerminalScreenTextEditType.PasteCR);
        }

        private void mnuPasteCR_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void mnuAutoC_Click_Event(object sender, EventArgs e)
        {
            if (TerminalMode == E_TerminalMode.QuickClient)
                mnuAutoC_Click(sender, e);
            else
                AppGlobal.terminalPanel1?.ExecTerminalScreen(E_TerminalScreenTextEditType.AutoC);
        }

        private void mnuAutoC_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void mnuFind_Click_Event(object sender, EventArgs e)
        {
            if (TerminalMode == E_TerminalMode.QuickClient)
                mnuFind_Click(sender, e);
            else
                AppGlobal.terminalPanel1?.ExecTerminalScreen(E_TerminalScreenTextEditType.Find);
        }

        private void mnuFind_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void mnuSelectAll_Click_Event(object sender, EventArgs e)
        {
            if (TerminalMode == E_TerminalMode.QuickClient)
                mnuSelectAll_Click(sender, e);
            else
                AppGlobal.terminalPanel1?.ExecTerminalScreen(E_TerminalScreenTextEditType.SelectAll);
        }

        private void mnuSelectAll_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void mnuClear_Click_Event(object sender, EventArgs e)
        {
            if (TerminalMode == E_TerminalMode.QuickClient)
                mnuClear_Click(sender, e);
            else
                AppGlobal.terminalPanel1?.ExecTerminalScreen(E_TerminalScreenTextEditType.Clear);
        }

        private void mnuClear_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void mnuCmdClear_Click_Event(object sender, EventArgs e)
        {
            if (TerminalMode == E_TerminalMode.QuickClient)
                mnuCmdClear_Click(sender, e);
            else
                AppGlobal.terminalPanel1?.ExecTerminalScreen(E_TerminalScreenTextEditType.CmdClear);
        }

        private void mnuCmdClear_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void mnuSearchDefaultCmd_Click_Event(object sender, EventArgs e)
        {
            if (TerminalMode == E_TerminalMode.QuickClient)
                mnuSearchDefaultCmd_Click(sender, e);
            else
                AppGlobal.terminalPanel1?.ExecTerminalScreen(E_TerminalScreenTextEditType.SearchCmd);
        }

        private void mnuSearchDefaultCmd_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Command Restriction & Paste Logic

        /// <summary>
        /// 제한 명령어 체크 결과 구조체
        /// </summary>
        private struct LimitCheckResult
        {
            public bool IsLimited;     // 차단 여부
            public string EmbargoCmd;  // 발견된 제한 명령어
            public bool IsFatal;       // 절대 금지 여부 (EmbargoEnble == true)
        }

        /// <summary>
        /// [통합 로직 1] 공통 제한 명령어 체크 엔진
        /// </summary>
        private LimitCheckResult CheckCommandRestriction(string lineCmd)
        {
            LimitCheckResult result = new LimitCheckResult { IsLimited = false, EmbargoCmd = string.Empty };

            if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Console) return result;
            if (AppGlobal.s_LoginResult?.UserInfo?.LimitedCmdUser != true) return result;
            if (string.IsNullOrWhiteSpace(lineCmd)) return result;

            // 공백 정규화
            string normalizedCmd = string.Join(" ", lineCmd.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

            if (AppGlobal.s_LimitCmdInfoList.Contains(m_DeviceInfo.ModelID))
            {
                var limitInfo = AppGlobal.s_LimitCmdInfoList[m_DeviceInfo.ModelID] as LimitCmdInfo;
                if (limitInfo?.EmbagoCmd == null) return result;

                foreach (EmbagoInfo info in limitInfo.EmbagoCmd)
                {
                    if (string.IsNullOrEmpty(info.Embargo)) continue;

                    // 대소문자 무시 검색
                    if (normalizedCmd.IndexOf(info.Embargo, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        return new LimitCheckResult { IsLimited = true, EmbargoCmd = info.Embargo, IsFatal = info.EmbargoEnble };
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// [통합 로직 2] 외부 노출용 제한 명령어 확인 함수
        /// </summary>
        public bool IsLimitCmd(string lineCmd)
        {
            var check = CheckCommandRestriction(lineCmd);
            if (!check.IsLimited) return false;

            if (check.IsFatal)
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm,
                    $"\"{check.EmbargoCmd}\" 금지 명령어를 포함하고 있습니다.\n해당 사용자는 이 명령어를 사용할 수 없습니다.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
            else
            {
                var dialogResult = AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm,
                    $"\"{check.EmbargoCmd}\" 제한 명령어를 포함하고 있습니다.\n사용 하시겠습니까?",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                return dialogResult == DialogResult.No;
            }
        }

        /// <summary>
        /// [통합 로직 3] 정제된 붙여넣기 함수
        /// </summary>
        private void mnuPaste_Click(object sender, EventArgs e)
        {
            if (m_TerminalStatus == E_TerminalStatus.RunScript) return;

            IDataObject tClipboard = Clipboard.GetDataObject();
            if (tClipboard == null || !tClipboard.GetDataPresent(DataFormats.Text)) return;

            string fullCmd = tClipboard.GetData(DataFormats.Text) as string;
            if (string.IsNullOrWhiteSpace(fullCmd)) return;

            // 모든 줄바꿈 기호 대응 분리
            string[] cmdLines = fullCmd.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            if (cmdLines.Length == 1)
            {
                // 단일 명령 처리
                string singleCmd = cmdLines[0];

                // 1. 제한 명령어 체크
                if (IsLimitCmd(singleCmd)) return;

                // 2. Rebex 세션을 통한 명령 전송
                // DispatchMessage 대신 Rebex의 SendCommand 또는 Write 사용
                try
                {
                    if (this.IsConnected)
                    {
                        // SendCommand는 자동으로 끝에 NewLine(\r 또는 \r\n)을 붙여 전송합니다.
                        // 만약 이미 \r이 포함되어 있다면 Write를 사용해야 합니다.
                        //this.Scripting.SendCommand(singleCmd);
                        this.SafeInvoke(() => {
                            // 기존 내용을 지우고 싶다면 백스페이스 등을 보낼 수 있으나, 
                            // Rebex에서는 직접 가상 텍스트를 입력하는 방식을 권장합니다.
                            this.Screen.Write(singleCmd);
                            this.Focus();
                        });

                        // 실행 로그 저장
                        SavePasteCommandLog(false, singleCmd);
                    }
                }
                catch (Exception ex)
                {
                    AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, $"Single Paste Error: {ex.Message}");
                }
            }
            else
            {
                // 복수 명령 처리: 선제적 전체 보안 검사
                foreach (string line in cmdLines)
                {
                    if (IsLimitCmd(line)) return;
                }

                // 스크립트 배치 실행
                try
                {
                    Script tCommandScript = ScriptGenerator.MakeBatchCommand(fullCmd.Replace("\r\n", "\r"), m_Prompt + "|#|>");
                    AppGlobal.s_MultipleCmd = 60 + (30 * cmdLines.Length);
                    RunScript(tCommandScript);

                    // 실행 로그 저장
                    foreach (string line in cmdLines)
                    {
                        SavePasteCommandLog(false, line);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Paste Error: {ex.Message}");
                }
            }
        }

        private string m_Prompt = "";
        private bool m_IsCheckPrompt = false;

        /// <summary>
        /// 현재 커서가 위치한 라인에서 프롬프트 문자열을 추출하여 설정합니다.
        /// </summary>
        internal void CheckPrompt()
        {
            // 이미 프롬프트를 확인했다면 스킵 (필요 시 로직에 따라 초기화 후 재호출)
            if (m_IsCheckPrompt) return;

            try
            {
                // 1. Rebex Screen에서 현재 커서가 있는 행(Row)의 텍스트를 가져옴
                // Screen.Cursor.Row는 현재 커서의 행 번호입니다.
                // 현재 커서 위치 확인
                int cursorRow = this.Screen.CursorTop;
                int cursorCol = this.Screen.CursorLeft;
                int screenWidth = this.Screen.Columns;  // 또는 Width

                // 현재 row 전체 텍스트 가져오기 (0, cursorRow, 전체 너비, 1행)
                string[] currentRowLines = this.Screen.GetRegionText(0, cursorRow, screenWidth, 1);
                string currentRowText = currentRowLines[0].TrimEnd();

                if (string.IsNullOrWhiteSpace(currentRowText)) return;

                // 2. 텍스트 정제 (TrimEnd를 통해 우측 공백 제거)
                // 기존 2016-04-01 수정사항 반영: 강제 스페이스 추가 없이 그대로 사용
                string detectedPrompt = currentRowText.TrimEnd();

                if (detectedPrompt.Length > 0)
                {
                    m_Prompt = detectedPrompt;
                    m_IsCheckPrompt = true;

                    // 디버그 로그 (필요 시)
                    // System.Diagnostics.Debug.WriteLine($"[Prompt Detected] : {m_Prompt}");
                }
            }
            catch (Exception ex)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, $"CheckPrompt Error: {ex.Message}");
            }
        }

        /// <summary>
        /// 붙여넣기 또는 실행한 명령어를 DB 로그에 저장합니다.
        /// </summary>
        /// <param name="isLimitCmd">제한 명령어 여부</param>
        /// <param name="cmd">실행된 명령어 문자열</param>
        private void SavePasteCommandLog(bool isLimitCmd, string cmd)
        {
            try
            {
                // 1. 로그 정보를 담을 객체 생성
                DBExecuteCommandLogInfo m_CommandLogInfo = new DBExecuteCommandLogInfo
                {
                    DeviceInfo = m_DeviceInfo,
                    Command = cmd,
                    IsLimitCmd = isLimitCmd,
                    // ConnectedSessionID는 현재 클래스에서 구현된 세션 ID 값을 사용
                    // (구현되지 않았다면 0 또는 적절한 ID 변수 매핑 필요)
                    ConnectionLogID = this.ConnectedSessionID,
                    LogType = E_DBLogType.ExecuteCommandLog
                };

                // 2. 글로벌 로그 프로세서를 통해 DB 저장
                // AppGlobal에 해당 객체가 정적으로 선언되어 있어야 합니다.
                AppGlobal.s_TerminalExecuteLogProcess?.AddTerminalExecuteLog(m_CommandLogInfo);
            }
            catch (Exception ex)
            {
                // 로그 저장 실패 시 디버그 출력 (흐름에 지장을 주지 않기 위함)
                System.Diagnostics.Debug.WriteLine("SavePasteCommandLog Error: " + ex.Message);
            }
        }

        #endregion

    }
}
