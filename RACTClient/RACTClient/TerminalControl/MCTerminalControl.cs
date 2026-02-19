using System;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using RACTClient;
using RACTCommonClass;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using DevComponents.DotNetBar;
using MKLibrary.MKNetwork;
using RACTSerialProcess;
using RACTTerminal;
using System.Threading;

using System.Diagnostics;
using System.Management;
using System.Timers;
using RACTClient.SubForm;
using System.Text.RegularExpressions;

namespace RACTClient
{
    /// <summary>
    /// 터미널 컨트롤 입니다.
    /// </summary>
    /// <remarks>
    /// [2017/08/04] VScroll/Resize시 연관값 정리(HScroll은 미작동):
    /// - m_ScrollbackBuffer.Count  : 모든 문자열 보관 [0 .. N]
    /// - m_Cols  (=고정값, AppGlobal.s_ClientOption.TerminalColumnCount/PopupSizeWidth)
    ///   : 화면의 컬럼 수 (좌우스크롤 미지원, 코드는 있으나 미사용)
    /// - m_Rows : 화면의 줄 수 [0 .. m_Rows-1]
    /// - m_CharGrid[m_Rows][col] 
    ///   : 화면의 문자열(m_ScrollbackBuffer 의 일부 + 커서줄 문자열 포함)
    /// - m_AttribGrid[m_Rows][col]  (m_CharGrid와 동일 사이즈)
    ///   : 화면의 문자별 폰트속성 (예: IsInverse = true 이면 형광표시)
    /// - m_VertScrollBar [-1 .. N]
    ///   m_VertScrollBar.Minimum = 0 (가끔 -1로 설정되는 경우 no scroll상태로 처리)
    ///   m_VertScrollBar.Maximum = m_ScrollbackBuffer.Count - m_Rows + 1
    ///   m_ScrollbackBuffer.Index = m_VertScrollBar.Value + m_Rows - 2  (m_BeginRow/m_EndRow)
    ///   m_LastVisibleLine = (m_Rows - m_ScrollbackBuffer.Count - 1) .. 0
    ///   m_LastVisibleLine = m_VertScrollBar.Value - m_VertScrollBar.Maximum
    /// </remarks>
    public class MCTerminalEmulator : SenderControl, ISerialEmulator, ITelnetEmulator, ITerminal
    {
        private System.ComponentModel.IContainer components;
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
        
        /// <summary>
        /// Input Output Boolean 
        /// </summary>
        private bool m_IsOutPut = false;
        /// <summary>
        /// 자동저장 관련 명령어 읽을 타이밍 관련 boolean
        /// </summary>
        private bool m_IsAutoLogSaver = false;
        /// <summary>
        /// 명령어 확인하는 값
        /// </summary>
        private string lineCommendBuffer = "";

        /// <summary>
        /// 명령어 입력 결과 확인하는 값
        /// </summary>
        private string lineRunningBuffer = "";

        
        /// <summary>
        /// 자동완성인지 확인
        /// </summary>
        private bool FromAutoCmd = false;
        /// <summary>
        /// 명령어 입력 후인지 확인하는 값
        /// </summary>
        private bool isAfterCmd = false;
        /// <summary>
        /// 수신 대기 스크립트를 저장했는지 여부 입니다.
        /// </summary>
        private bool m_IsSaveWaitScript = false;
        /// <summary>
        /// 프롬프트를 확인했는지 여부 입니다.
        /// </summary>
        private bool m_IsCheckPrompt = false;
        /// <summary>
        /// 터미널 상태 변경 이벤트 입니다.
        /// </summary>
        public event HandlerArgument2<object, E_TerminalStatus> OnTerminalStatusChange;
        /// <summary>
        /// 새로고침 이벤트 입니다.
        /// </summary>
        private event RefreshEventHandler OnRefreshEvent;
        /// <summary>
        /// 수신 이벤트 입니다.
        /// </summary>
        private event RxdTextEventHandler OnRxdTextEvent;
        /// <summary>
        /// 찾기 이벤트 입니다.
        /// </summary>
        public event DefaultHandler OnTelnetFindString;
        /// <summary>
        /// 커서 끄기 이벤트 입니다.
        /// </summary>
        private event CaretOffEventHandler OnCaretOffEvent;
        /// <summary>
        /// 커서 켜기 이벤트 입니다.
        /// </summary>
        private event CaretOnEventHandler OnCaretEvent;
        /// <summary>
        /// Caret 표시 여부 입니다.
        /// </summary>
        private bool m_IsShowCaret = true;
        /// <summary>
        ///  ߰  ü  
        /// </summary>
        private DeviceInfo m_JumpHost = null;

        public DeviceInfo JumpHost
        {
            get { return m_JumpHost; }
            set { m_JumpHost = value; }
        }

        /// <summary>
        ///  ͹̳   ¿  
        /// </summary>
        private E_TerminalStatus m_TerminalStatus = E_TerminalStatus.TryConnection;
        /// <summary>
        /// 스크립트 생성자 입니다.
        /// </summary>
        public ScriptGenerator m_ScriptGenerator = new ScriptGenerator();
        /// <summary>
        /// 연결 타입 입니다.
        /// </summary>
        private ConnectionTypes m_ConnectionType;
        /// <summary>
        /// Host 이름 입니다.
        /// </summary>
        private string m_Hostname;
        /// <summary>
        /// 터미널 실행 모드 입니다.
        /// </summary>
        private E_TerminalMode m_TerminalMode = E_TerminalMode.RACTClient;
        /// <summary>
        /// 드래그 시작 위치 입니다.
        /// </summary>
        private Point m_BeginDrag;
        /// <summary>
        /// 드래그 종료 위치 입니다.
        /// </summary>
        private Point m_EndDrag;

        /// <summary>
        /// 명령어 문자 입니다.
        /// </summary>
        private String strCmd;

        /// <summary>
        /// 순차적 명령처리 중인지 파악.
        /// </summary>
        private bool isBatchCmdRunning = false;

        /// <summary>
        /// 순차적 명령처리 Count
        /// </summary>
        private int BatchCmdCount = 0;

        /// <summary>
        /// 순차적 명령 배열
        /// </summary>
        private string[] BatchCmdArray;

        /// <summary>
        /// 순차적 명령 타이머
        /// </summary>
        private System.Windows.Forms.Timer timer;

        // 2014-07-02 - 신윤남 - 스크롤 후 값 복사하는 기능 추가
        private int m_BeginRow;
        private int m_BeginCol;
        private int m_EndRow;
        private int m_EndCol;
        private StringBuilder m_CopyValue;

        /// <summary>
        /// 커서의 문자 입니다.
        /// </summary>
        private string m_TextAtCursor = "";
        /// <summary>
        /// 마지막 표시 라인 입니다.
        /// </summary>
        private int m_LastVisibleLine;
        
        // 2015-06-01 - 신윤남 - 마지막 column 입니다.
        private int m_LastVisibleCol;
        /// <summary>
        /// 장비 접속 여부 입니다.
        /// </summary>
        private bool m_IsConnected;
        /// <summary>
        /// 엔터 누름 입니다.
        /// </summary>
        private bool m_IsPressEnter = false;
        /// <summary>
        /// 데몬 객체 입니다.
        /// </summary>
        private DaemonProcessRemoteObject m_DaemonProcessRemoteObject;
        private bool m_XOff = false;
        /// <summary>
        /// 임시 버퍼 입니다.
        /// </summary>
        private string m_OutBuffer = "";
        /// <summary>
        /// 저장될 최대 라인 수 입니다.
        /// </summary>
        private int m_ScrollbackBufferSize;
        /// <summary>
        /// 전체 받은 문자 입니다.
        /// </summary>
        private StringCollection m_ScrollbackBuffer;
        /// <summary>
        /// 
        /// </summary>
        private Parser m_Parser = null;
        private TelnetParser m_NvtParser = null;
        private Keyboard m_Keyboard = null;
        private TabStops m_TabStops = null;
        /// <summary>
        /// Erase Buffer 입니다.
        /// </summary>
        private Bitmap m_EraseBitmap = null;
        private Graphics m_EraseBuffer = null;
        /// <summary>
        /// 문자가 저장될 그리드 입니다.
        /// </summary>
        private Char[][] m_CharGrid = null;
        /// <summary>
        /// 문자 속성정보가 저장됩니다.
        /// </summary>
        private CharAttribStruct[][] m_AttribGrid = null;
        private CharAttribStruct m_CharAttribs;
        /// <summary>
        /// 열 갯수 입니다.
        /// </summary>
        private Int32 m_Cols;
        /// <summary>
        /// 행 갯수 입니다.
        /// </summary>
        private Int32 m_Rows;
        /// <summary>
        /// Top Margin 입니다.
        /// </summary>
        private Int32 m_TopMargin;
        /// <summary>
        /// Bottom Margin 입니다.
        /// </summary>
        private Int32 m_BottomMargin;
        /// <summary>
        /// 문자 크기 입니다.
        /// </summary>
        private Size m_CharSize;
        /// <summary>
        /// 밑줄 위치 입니다.
        /// </summary>
        private Int32 m_UnderlinePos;
        /// <summary>
        /// 커서 입니다.
        /// </summary>
        private Caret m_Caret;
        private ArrayList m_SavedCarets;
        /// <summary>
        /// 폰트 시작 위치 입니다.
        /// </summary>
        private Point m_DrawstringOffset;
        /// <summary>
        /// Fg Color 입니다.
        /// </summary>
        private Color m_FGColor;
        /// <summary>
        /// Bold Color 입니다.
        /// </summary>
        private Color m_BoldColor;
        /// <summary>
        /// Blink Color 입니다.
        /// </summary>
        private Color m_BlinkColor;
        private Chars m_G0;
        private Chars m_G1;
        private Chars m_G2;
        private Chars m_G3;
        private Mode m_Modes;
        /// <summary>
        /// 마지막 찾은 Row 입니다.
        /// </summary>
        private int m_LastFindRow = 0;
        /// <summary>
        /// 마지막 찾은 Col 입니다.
        /// </summary>
        private int m_LastFindCol = 0;
        /// <summary>
        /// Caret 표시용 타이머 입니다.
        /// </summary>
        private System.Windows.Forms.Timer timer1;

        // 스크롤바
        private VertScrollBar m_VertScrollBar;
        private HorzScrollBar m_HorzScrollBar;
        
        /// <summary>
        /// 스크립트 관리자 입니다.
        /// </summary>
        private ScriptManager m_ScriptManager;
        /// <summary>
        /// 터미널 연결 타입 입니다.
        /// </summary>
        private E_ConnectionProtocol m_ConnectionProtocolType = E_ConnectionProtocol.TELNET;
        /// <summary>
        /// 빠른 연결 처리 여부 입니다.
        /// </summary>
        private bool m_IsQuickConnection = false;
        /// <summary>
        /// 빠른 연결인지 여부를 가져오기 합니다.
        /// </summary>
        public bool IsQuickConnection
        {
            get { return m_IsQuickConnection; }
        }
        /// <summary>
        /// 라인 번호 표시 여부 입니다.
        /// </summary>
        private bool m_IsShowLineNumber = false;
		// 2019-11-10 ???? (?? ?? ??)
        /// <summary>
        /// 옵션 창을 호출 하기 위한 이벤트 입니다.
        /// </summary>
        public event DefaultHandler CallOptionHandlerEvent;
		// 2019-11-10 ???? (OneTerminal ??? ?? ??UI ??)
        /// <summary>
        /// Oneterminal 접시 프로그래스바를 제어하기 위한 이벤트 입니다.
        /// </summary
        public event HandlerArgument3<String, eProgressItemType, bool> ProgreBarHandlerEvent;

        //2019-11-18 전송지연 옵션 처리
        public bool m_IsConected = false;
        private Thread m_CmdControlThread = null;
        //public Queue<RequestCommunicationData> m_CmdQueue;
        public Queue<String> m_CmdQueue;

        //2022-11-28 유선접속에서 무선접속 전환시 문제점 개선
        /// <summary>
        /// CatM1 연결 처리 여부 입니다.
        /// </summary>
        private bool m_ChangeMode = false;
        /// <summary>
        /// CatM1 연결인지 여부를 가져오기 합니다.
        /// </summary>
        public bool IsChangeMode
        {
            get { return m_ChangeMode; }
            set { m_ChangeMode = value; }
        }
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public MCTerminalEmulator() : this(false) { }

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public MCTerminalEmulator(bool aIsQuickConnection)
        {
            string tTempFont = AppGlobal.s_ClientOption.TerminalFontName;
            if (tTempFont.Equals("굴림")
                || tTempFont.Equals("돋움")
                || tTempFont.Equals("궁서")
                || tTempFont.Equals("바탕"))
            {
                tTempFont += "체";
            }
            this.Font = new Font(tTempFont, AppGlobal.s_ClientOption.TerminalFontSize, AppGlobal.s_ClientOption.TerminalFontStyle, GraphicsUnit.Point, ((byte)(0))); ;

            m_IsQuickConnection = aIsQuickConnection;
            this.AutoScroll = true;
            DoubleBuffered = true;
            //m_ScrollbackBufferSize = 3000;
            //2015-11-12 hanjiyeon 버퍼사이즈 증가시킴. (show tech 등의 결과가 긴 경우 모두 표시 안되는 문제 보완)
            m_ScrollbackBufferSize = 20000;
            m_ScrollbackBuffer = new StringCollection();


            m_ScriptManager = new ScriptManager(this);
            m_ScriptManager.OnRunScriptComplete += new DefaultHandler(m_ScriptManager_OnRunScriptComplete);

            m_VertScrollBar = new VertScrollBar();
            m_VertScrollBar.OnChangeScrollValue += new ChangeScrollValue(m_VertScrollBar_OnChangeScrollValue);

            m_VertScrollBar.Layout += new LayoutEventHandler(m_VertScrollBar.sb2_Layout);
            m_VertScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            m_VertScrollBar.Visible = false;
            m_VertScrollBar.Enabled = false;

            Controls.Add(m_VertScrollBar);

            InitializeComponent();

            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);

            m_Keyboard = new Keyboard(this);
            m_Parser = new Parser();
            m_NvtParser = new TelnetParser();
            m_Caret = new Caret();
            m_Modes = new Mode();
            m_TabStops = new TabStops();
            m_SavedCarets = new System.Collections.ArrayList();


            m_Caret.Pos = new System.Drawing.Point(0, 0);
            m_CharSize = new System.Drawing.Size();

            if (AppGlobal.s_ClientOption == null)
            {
                m_FGColor = Color.FromArgb(255, 173, 255, 47);
                BackColor = Color.FromArgb(255, 0, 0, 0);
                m_BoldColor = Color.FromArgb(255, 255, 255, 255);
                m_BlinkColor = Color.FromArgb(255, 255, 0, 0);
            }
            else
            {
                m_FGColor = AppGlobal.s_ClientOption.TerminalFontColor;
                BackColor = AppGlobal.s_ClientOption.TerminalBackGroundColor;
                m_BoldColor = AppGlobal.s_ClientOption.TerminalBoldColor;
                m_BlinkColor = AppGlobal.s_ClientOption.TerminalBlinkColor;
            }

            m_G0 = new Chars(Chars.Sets.ASCII);
            m_G1 = new Chars(Chars.Sets.ASCII);
            m_G2 = new Chars(Chars.Sets.DECSG);
            m_G3 = new Chars(Chars.Sets.DECSG);

            m_CharAttribs.GL = m_G0;
            m_CharAttribs.GR = m_G2;
            m_CharAttribs.GS = null;

            GetFontInfo();

            MakeContextMenu();

            // 2015-06-01 - 신윤남 - Terminal 사이즈 늘리기
            this.SetSize(24, AppGlobal.s_ClientOption.TerminalColumnCount);

            m_Parser.OnParserEvent += new ParserEventHandler(CommandRouter);
            
            // 키보드 입력 이벤트
            m_Keyboard.OnKeyboardEvent          +=      new KeyboardEventHandler(DispatchMessage);
            // 컨트롤 키 입력 이벤트
            m_Keyboard.OnControlKeyBoardEvent   +=      new ControlKeyboardEventHandler(DispatchControlMessage);

            m_NvtParser.NvtParserEvent          +=      new NegotiateParserEventHandler(TelnetInterpreter);
            OnRefreshEvent                      +=      new RefreshEventHandler(ShowBuffer);
            OnCaretOffEvent                     +=      new CaretOffEventHandler(CaretOff);
            OnCaretEvent                        +=      new CaretOnEventHandler(CaretOn);
            OnRxdTextEvent                      +=      new RxdTextEventHandler(m_NvtParser.Parsestring);

            m_BeginDrag = new System.Drawing.Point();
            m_EndDrag = new System.Drawing.Point();

            m_CopyValue = new StringBuilder();

            timer1 = new System.Windows.Forms.Timer();
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            timer1.Start();
  
            MCSmallTerminal.OnSendCommandToTerminalEvent += new HandlerArgument2<List<string>, string>(AppGlobal_OnSendCommandToTerminalEvent);

            //2019-11-18 전송지연 옵션 처리
            m_CmdQueue = new Queue<String>();
            m_CmdControlThread = new Thread(new ThreadStart(SendTelnetCommand));
            m_CmdControlThread.Start();

            m_IsCheckPrompt = false;

            // 영역선택 취소
            Deselect();
        }
		
		// 2019-11-10 ???? (OneTerminal ??? ?? ??UI ??)
        public MCTerminalEmulator(bool aIsQuickConnection, E_TerminalMode eTerminalMode)
        {
            string tTempFont = AppGlobal.s_ClientOption.TerminalFontName;
            if (tTempFont.Equals("굴림")
                || tTempFont.Equals("돋움")
                || tTempFont.Equals("궁서")
                || tTempFont.Equals("바탕"))
            {
                tTempFont += "체";
            }
            this.Font = new Font(tTempFont, AppGlobal.s_ClientOption.TerminalFontSize, AppGlobal.s_ClientOption.TerminalFontStyle, GraphicsUnit.Point, ((byte)(0))); ;

            m_IsQuickConnection = aIsQuickConnection;
            this.AutoScroll = true;
            DoubleBuffered = true;
            //m_ScrollbackBufferSize = 3000;
            //2015-11-12 hanjiyeon 버퍼사이즈 증가시킴. (show tech 등의 결과가 긴 경우 모두 표시 안되는 문제 보완)
            m_ScrollbackBufferSize = 20000;
            m_ScrollbackBuffer = new StringCollection();


            m_ScriptManager = new ScriptManager(this);
            m_ScriptManager.OnRunScriptComplete += new DefaultHandler(m_ScriptManager_OnRunScriptComplete);

            m_VertScrollBar = new VertScrollBar();
            m_VertScrollBar.OnChangeScrollValue += new ChangeScrollValue(m_VertScrollBar_OnChangeScrollValue);

            m_VertScrollBar.Layout += new LayoutEventHandler(m_VertScrollBar.sb2_Layout);
            m_VertScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            m_VertScrollBar.Visible = false;
            m_VertScrollBar.Enabled = false;

            Controls.Add(m_VertScrollBar);

            InitializeComponent();

            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);

            m_Keyboard = new Keyboard(this);
            m_Parser = new Parser();
            m_NvtParser = new TelnetParser();
            m_Caret = new Caret();
            m_Modes = new Mode();
            m_TabStops = new TabStops();
            m_SavedCarets = new System.Collections.ArrayList();


            m_Caret.Pos = new System.Drawing.Point(0, 0);
            m_CharSize = new System.Drawing.Size();

            if (AppGlobal.s_ClientOption == null)
            {
                m_FGColor = Color.FromArgb(255, 173, 255, 47);
                BackColor = Color.FromArgb(255, 0, 0, 0);
                m_BoldColor = Color.FromArgb(255, 255, 255, 255);
                m_BlinkColor = Color.FromArgb(255, 255, 0, 0);
            }
            else
            {
                m_FGColor = AppGlobal.s_ClientOption.TerminalFontColor;
                BackColor = AppGlobal.s_ClientOption.TerminalBackGroundColor;
                m_BoldColor = AppGlobal.s_ClientOption.TerminalBoldColor;
                m_BlinkColor = AppGlobal.s_ClientOption.TerminalBlinkColor;
            }

            m_G0 = new Chars(Chars.Sets.ASCII);
            m_G1 = new Chars(Chars.Sets.ASCII);
            m_G2 = new Chars(Chars.Sets.DECSG);
            m_G3 = new Chars(Chars.Sets.DECSG);

            m_CharAttribs.GL = m_G0;
            m_CharAttribs.GR = m_G2;
            m_CharAttribs.GS = null;

            GetFontInfo();

            TerminalMode = eTerminalMode;

            MakeContextMenu();

            // 2015-06-01 - 신윤남 - Terminal 사이즈 늘리기
            this.SetSize(24, AppGlobal.s_ClientOption.TerminalColumnCount);

            m_Parser.OnParserEvent += new ParserEventHandler(CommandRouter);

            // 키보드 입력 이벤트
            m_Keyboard.OnKeyboardEvent += new KeyboardEventHandler(DispatchMessage);
            // 컨트롤 키 입력 이벤트
            m_Keyboard.OnControlKeyBoardEvent += new ControlKeyboardEventHandler(DispatchControlMessage);

            m_NvtParser.NvtParserEvent += new NegotiateParserEventHandler(TelnetInterpreter);
            OnRefreshEvent += new RefreshEventHandler(ShowBuffer);
            OnCaretOffEvent += new CaretOffEventHandler(CaretOff);
            OnCaretEvent += new CaretOnEventHandler(CaretOn);
            OnRxdTextEvent += new RxdTextEventHandler(m_NvtParser.Parsestring);

            m_BeginDrag = new System.Drawing.Point();
            m_EndDrag = new System.Drawing.Point();

            m_CopyValue = new StringBuilder();

            timer1 = new System.Windows.Forms.Timer();
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            timer1.Start();

            MCSmallTerminal.OnSendCommandToTerminalEvent += new HandlerArgument2<List<string>, string>(AppGlobal_OnSendCommandToTerminalEvent);

            //2019-11-18 전송지연 옵션 처리
            m_CmdQueue = new Queue<String>();
            m_CmdControlThread = new Thread(new ThreadStart(SendTelnetCommand));
            m_CmdControlThread.Start();
			
            m_IsCheckPrompt = false;

            // 영역선택 취소
            Deselect();
        }

        /// <summary>
        /// 스크립트 종료처리 합니다.
        /// </summary>
        void m_ScriptManager_OnRunScriptComplete()
        {
            try
            {
                TerminalStatus = E_TerminalStatus.Connection;
                if (ProgreBarHandlerEvent != null)
                    ProgreBarHandlerEvent("디바이스에 연결 되었습니다.", eProgressItemType.Standard, false);
                if (isBatchCmdRunning) return;

                m_ScriptManager.Stop();
            }
            catch (Exception e) {}
            finally
            {
                //2019-11-21
                //ScriptManager.Stop 시 ThreadAbortException가 catch되지 안는 현상이 있어 
                //try catch finally 구문으로 CheckPrompt가 수행 될 수 있도록 변경
                CheckPrompt();
                AppGlobal.s_MultipleCmd = 20;
            }
        }


        /// <summary>
        /// 문자 자르기 합니다.
        /// </summary>
        /// <returns></returns>
        public void ScreenScrape(int aStartColumn, int aStartRow, int aEndColumn, int aEndRow)
        {
            StringBuilder tTextBox = new StringBuilder();
            string tTempString = "";
            int tStartColumn = 0;
            int tEndColumn = 0;
            for (int tRow = aStartRow; tRow <= aEndRow; tRow++)
            {
                tTempString = "";
                for (int tCos = tRow == aStartRow ? aStartColumn : 0; tCos <= (tRow == aEndRow ? aEndColumn : m_Cols - 1); tCos++)
                {
                    //System.Diagnostics.Debug.WriteLine(m_CharGrid[tRow][tCos].ToString());
                    if (m_CharGrid[tRow][tCos].Equals('\0'))
                    {
                        continue;
                    }

                    tTempString += m_CharGrid[tRow][tCos].ToString();

                }

                tTextBox.AppendLine(tTempString);
            }
            Clipboard.SetDataObject(tTextBox.ToString());
        }

        /// <summary>
        /// 찾은 결과를 적용 합니다.
        /// </summary>
        /// <param name="aInfo"></param>
        private void ApplyFindInformation(TelnetStringFind aInfo)
        {

            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument1<TelnetStringFind>(ApplyFindInformation), aInfo);
                return;
            }

            for (int i = 0; i < this.m_Rows; i++)
            {
                Array.Clear(this.m_AttribGrid[i], 0, this.m_AttribGrid[i].Length);
            }

            if (aInfo.IsMatch)
            {
                //System.Diagnostics.Debug.WriteLine("찾은 라인 : " + (aInfo.FindList[0].Row + 1));


                if (aInfo.FindList[0].Row >= NowDrawStart && aInfo.FindList[0].Row <= NowDrawEnd)
                {
                    //스크롤 안해도 되면 할 거 없나????????????
                }
                else
                {
                    m_VertScrollBar.IsChangeValue = true;
                    DisplayScrollLast(aInfo.FindList[0].Row - 1);
                }

                foreach (StringFindInfo tStringInfo in aInfo.FindList)
                {
                    if (NowDrawStart > 0)
                    {
                        m_AttribGrid[tStringInfo.Row - NowDrawStart - 1][tStringInfo.Col].IsInverse = true;
                    }
                    else
                    {
                        m_AttribGrid[tStringInfo.Row][tStringInfo.Col].IsInverse = true;
                    }
                }
            }
            this.Refresh();
        }

        /// <summary>
        /// 현재 표시하고 있는 Row의 시작 위치를 가져오기 합니다.
        /// </summary>
        private int NowDrawStart
        {
            get
            {
                int tNowDrawLineStart = m_ScrollbackBuffer.Count + m_LastVisibleLine - m_Rows;
                tNowDrawLineStart = tNowDrawLineStart < 0 ? 0 : tNowDrawLineStart;

                return tNowDrawLineStart;
            }
        }
        /// <summary>
        /// 현재 표시하고 있는 Row의 종료 위치를 가져오기 합니다.
        /// </summary>
        private int NowDrawEnd
        {
            get { return m_ScrollbackBuffer.Count + m_LastVisibleLine; }
        }

        /// <summary>
        /// 문자열 검색을 합니다.
        /// </summary>
        /// <param name="aString">찾을 문자 입니다.</param>
        /// <param name="aOption">찾기 옵션 입니다.</param>
        /// <param name="oFindInfo">찾기 결과 입니다.</param>
        /// <returns>찾음 여부입니다.</returns>
        private bool FindString(TelnetStringFindHandlerArgs aArgs, out TelnetStringFind oFindInfo)
        {
            oFindInfo = new TelnetStringFind(aArgs.FindString);


            string tSearchSource = "";
            char tCurrentChar;

            for (int i = m_LastFindRow; i < m_ScrollbackBuffer.Count; i++)
            {
                tSearchSource = "";

                tSearchSource = m_ScrollbackBuffer[i];

                if (m_LastFindCol != 0)
                {
                    tSearchSource = tSearchSource.Substring(m_LastFindCol, tSearchSource.Length - m_LastFindCol);
                }
                if (tSearchSource.Contains(aArgs.FindString))
                {
                    //if (tSearchSource.IndexOf(aArgs.FindString) + aArgs.FindString.Length == m_LastFindCol)
                    //{
                    //    m_LastFindCol = 0;
                    //    continue;
                    //}
                    //else
                    //{
                    for (int tResult = 0; tResult < oFindInfo.FindList.Count; tResult++)// tSearchSource.IndexOf(aArgs.FindString); tResult < aArgs.FindString.Length; tResult++)
                    {
                        oFindInfo.FindList[tResult].Row = i;
                        oFindInfo.FindList[tResult].Col = m_LastFindCol + tSearchSource.IndexOf(aArgs.FindString) + tResult;
                    }
                    //}
                    m_LastFindRow = i;
                    m_LastFindCol = oFindInfo.FindList[oFindInfo.FindList.Count - 1].Col + 1;
                    return true;
                }
                m_LastFindCol = 0;
            }
            m_LastFindRow = 0;
            m_LastFindCol = 0;
            return false;

            return oFindInfo.IsMatch;
        }

        /// <summary>
        /// 외부에서 명령이 들어 왔을 경우 처리 입니다.
        /// </summary>
        /// <param name="aList"></param>
        /// <param name="aValue1"></param>
        void AppGlobal_OnSendCommandToTerminalEvent(List<string> aList, string aValue1)
        {
            if (m_IsConnected && aList.Contains(this.Name))
            {
                //1.String 명령어단위(줄바꿈)로 문자열 배열생성
                String[] SepStrs = { "\r" };
                String[] CmdStr = aValue1.Split(SepStrs, StringSplitOptions.RemoveEmptyEntries);

                //단일 명령일때 처리 
                if (CmdStr.Length == 1)
                {
                    if (IsLimitCmd(aValue1))
                    {
                        return;
                    }
                    this.DispatchMessage(this, aValue1);

                }
                else
                {
                    //2. 제한 명령어 확인 
                    for (int i = 0; i < CmdStr.Length; i++)
                    {
                        String CurrentCmd = CmdStr[i].ToString();

                        //if (SepCnt > 0)
                        {
                            if (CurrentCmd.Length > 0)
                            {
                                if (IsLimitCmd(CurrentCmd))
                                {
                                    return;
                                }
                            }
                        }
                    }
                    //3.스크립트로 해당 명령어 OR 명령어들 수행
                    Script tCommandScript = null;

                    //스크립트 타임아웃 값 설정 명령어당 적절한 수치를 설정하기 애매함.
                    //Cmd 당 기본 30으로 더함. 추후 옵션 메뉴에서 따로 설정하도록 기능 지원하면.. 사용자 편의 제공.
                    AppGlobal.s_MultipleCmd = 60 + (30 * CmdStr.Length);

                    tCommandScript = ScriptGenerator.MakeBatchCommand(aValue1, m_Prompt);

                    //tCommandScript.ScriptType = E_ScriptType.WaitScript;
                    RunScript(tCommandScript);
                }

                //4.수행한 명령어 로그 저장
                for (int i = 0; i < CmdStr.Length; i++)
                {
                    String CurrentCmd = CmdStr[i].ToString();
               
                    if (CurrentCmd.Length > 0)
                    {
                        SavePasteCommandLog(false, CurrentCmd);
                    }
                }

            }
        }

        /// <summary>
        /// 메뉴를 구성 합니다.
        /// </summary>
        private void MakeContextMenu()
        {
            this.contextMenuBar1 = new DevComponents.DotNetBar.ContextMenuBar();
            this.cmPopUP = new DevComponents.DotNetBar.ButtonItem();
            this.mnuCopy = new DevComponents.DotNetBar.ButtonItem();
            this.mnuPaste = new DevComponents.DotNetBar.ButtonItem();
            this.mnuPasteE = new DevComponents.DotNetBar.ButtonItem();
            this.mnuAutoC = new DevComponents.DotNetBar.ButtonItem();
            this.mnuFind = new DevComponents.DotNetBar.ButtonItem();
            this.mnuSelectAll = new DevComponents.DotNetBar.ButtonItem();
            this.mnuClear = new DevComponents.DotNetBar.ButtonItem();
            this.mnuSearchDefaultCmd = new DevComponents.DotNetBar.ButtonItem();
            this.mnuBatchCmd = new DevComponents.DotNetBar.ButtonItem();
            this.mnuStopScript = new DevComponents.DotNetBar.ButtonItem();
            this.mnuSaveTerminal = new DevComponents.DotNetBar.ButtonItem();
            this.mnuCmdClear = new ButtonItem();
            this.mnuOption = new ButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuBar1
            // 
            this.contextMenuBar1.AntiAlias = true;
            this.contextMenuBar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.cmPopUP});
            this.contextMenuBar1.Location = new System.Drawing.Point(64, 27);
            this.contextMenuBar1.Name = "contextMenuBar1";
            this.contextMenuBar1.Size = new System.Drawing.Size(75, 25);
            this.contextMenuBar1.Stretch = true;
            this.contextMenuBar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.contextMenuBar1.TabIndex = 0;
            this.contextMenuBar1.TabStop = false;
            this.contextMenuBar1.Text = "contextMenuBar1";
            // 
            // cmPopUP
            // 
            if (TerminalMode == E_TerminalMode.QuickClient)
            {
                this.cmPopUP.AutoExpandOnClick = true;
                this.cmPopUP.Name = "cmPopUP";
                this.cmPopUP.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
                this.mnuCopy,
                this.mnuPaste,
                this.mnuPasteE,
                this.mnuAutoC,
                this.mnuFind,
                this.mnuSelectAll,
                this.mnuClear,
                this.mnuCmdClear,
                this.mnuSearchDefaultCmd,
                this.mnuBatchCmd,
                this.mnuStopScript,
                this.mnuSaveTerminal,
                this.mnuOption
                });
                this.cmPopUP.Text = "buttonItem1";
            }else
            {
                this.cmPopUP.AutoExpandOnClick = true;
                this.cmPopUP.Name = "cmPopUP";
                this.cmPopUP.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
                this.mnuCopy,
                this.mnuPaste,
                this.mnuPasteE,
                this.mnuAutoC,
                this.mnuFind,
                this.mnuSelectAll,
                this.mnuClear,
                this.mnuCmdClear,
                this.mnuSearchDefaultCmd,
                this.mnuBatchCmd,
                this.mnuStopScript,
                this.mnuSaveTerminal
                });
                this.cmPopUP.Text = "buttonItem1";
            }
            // 
            // buttonItem2
            // 
            this.mnuCopy.Name = "buttonItem2";
            this.mnuCopy.Text = "복사(&Y)";
            //2016-03-31 서영응 단축키 변경
            //this.mnuCopy.Shortcuts.Add(eShortcut.CtrlC);
            this.mnuCopy.Shortcuts.Add(eShortcut.CtrlY);
            this.mnuCopy.ImageSmall = (Image)global::RACTClient.Properties.Resources.copy;

            //2016-04-01 서영응 단축키 이벤트 변경
            //mnuCopy.Click += new EventHandler(mnuCopy_Click);
            mnuCopy.Click += new EventHandler(mnuCopy_Click_Event);
            
            // 
            // buttonItem3
            // 
            this.mnuPaste.Name = "buttonItem3";
            this.mnuPaste.Text = "붙여넣기(&P)";

            //2016-03-31 서영응 단축키 변경
            //this.mnuPaste.Shortcuts.Add(eShortcut.CtrlV);
            this.mnuPaste.Shortcuts.Add(eShortcut.CtrlP);
            this.mnuPaste.ImageSmall = (Image)global::RACTClient.Properties.Resources.paste;

            //2016-04-01 서영응 단축키 이벤트 변경
            //mnuPaste.Click += new EventHandler(mnuPaste_Click);
            mnuPaste.Click += new EventHandler(mnuPaste_Click_Event);
            
            // 
            // buttonItem3
            // 
            this.mnuPasteE.Name = "buttonItem9";
            this.mnuPasteE.Text = "<CR>붙여넣기(&B)";
            this.mnuPasteE.Shortcuts.Add(eShortcut.CtrlB);
            this.mnuPasteE.ImageSmall = (Image)global::RACTClient.Properties.Resources.paste;

            //2016-04-01 서영응 단축키 이벤트 변경
            //mnuPasteE.Click += new EventHandler(mnuPasteCR_Click);
            mnuPasteE.Click += new EventHandler(mnuPasteCR_Click_Event);


            // buttonItem3
            // 
            this.mnuAutoC.Name = "buttonItem10";
            //2016-01-19 서영응 단축키 Ctrl+E -> F2로 변경
            this.mnuAutoC.Text = "자동완성(F2)";
            this.mnuAutoC.Shortcuts.Add(eShortcut.F2);
            //this.mnuAutoC.Text = "자동완성(&E)";

            //2016-04-01 서영응 단축키 이벤트 변경
            //mnuAutoC.Click += new EventHandler(mnuAutoC_Click);
            mnuAutoC.Click += new EventHandler(mnuAutoC_Click_Event);

            // 
            // buttonItem4
            // 
            this.mnuFind.BeginGroup = true;
            this.mnuFind.Name = "buttonItem4";
            this.mnuFind.Text = "찾기(&F)";
            this.mnuFind.Shortcuts.Add(eShortcut.CtrlF);
            this.mnuFind.ImageSmall = (Image)global::RACTClient.Properties.Resources.find;

            //2016-04-01 서영응 단축키 이벤트 변경
            //mnuFind.Click += new EventHandler(mnuFind_Click);
            mnuFind.Click += new EventHandler(mnuFind_Click_Event);

            // 
            // buttonItem5
            // 
            this.mnuSelectAll.Name = "buttonItem5";
            this.mnuSelectAll.Text = "모두선택(&A)";
            this.mnuSelectAll.Shortcuts.Add(eShortcut.CtrlA);
            this.mnuSelectAll.ImageSmall = (Image)global::RACTClient.Properties.Resources.select_all;

            //2016-04-01 서영응 단축키 이벤트 변경
            //mnuSelectAll.Click += new EventHandler(mnuSelectAll_Click);
            mnuSelectAll.Click += new EventHandler(mnuSelectAll_Click_Event);
            // 
            // buttonItem6
            // 
            this.mnuClear.BeginGroup = true;
            this.mnuClear.Name = "buttonItem6";
            this.mnuClear.Text = "화면지움(&R)";
            this.mnuClear.Shortcuts.Add(eShortcut.CtrlR);
            this.mnuClear.ImageSmall = (Image)global::RACTClient.Properties.Resources.Clear;

            //2016-04-01 서영응 단축키 이벤트 변경
            //mnuClear.Click += new EventHandler(mnuClear_Click);
            mnuClear.Click += new EventHandler(mnuClear_Click_Event);

            this.mnuCmdClear.BeginGroup = true;
            this.mnuCmdClear.Name = "btnCmdClear";
            this.mnuCmdClear.Text = "입력 명령 지움(&U)";
            this.mnuCmdClear.Shortcuts.Add(eShortcut.CtrlU);
            this.mnuCmdClear.ImageSmall = (Image)global::RACTClient.Properties.Resources.Clear;
            mnuCmdClear.Click += new EventHandler(mnuCmdClear_Click_Event);

            this.mnuSearchDefaultCmd.BeginGroup = true;
            this.mnuSearchDefaultCmd.Name = "buttonItemDefaultCmd";
            this.mnuSearchDefaultCmd.Text = "기본 명령 조회 (F1)";

            this.mnuSearchDefaultCmd.Shortcuts.Add(eShortcut.F1);

            //2016-04-01 서영응 단축키 이벤트 변경
            //mnuSearchDefaultCmd.Click += new EventHandler(this.mnuSearchDefaultCmd_Click);
            mnuSearchDefaultCmd.Click += new EventHandler(this.mnuSearchDefaultCmd_Click_Event);
            
            this.mnuBatchCmd.BeginGroup = true;
            this.mnuBatchCmd.Name = "buttonItemBatchCmd";
            this.mnuBatchCmd.Text = "일괄 명령실행";
 
            this.mnuBatchCmd.ImageSmall = (Image)global::RACTClient.Properties.Resources.Clear;
            this.mnuBatchCmd.Click += new EventHandler(mnuBatchCmd_Click);


            this.mnuStopScript.BeginGroup = true;
            this.mnuStopScript.Name = "buttonItem7";
            this.mnuStopScript.Text = "스크립트 취소";
            this.mnuStopScript.ImageSmall = (Image)global::RACTClient.Properties.Resources.run_cancel;
            mnuStopScript.Click += new EventHandler(mnuStopScript_Click);

            this.mnuSaveTerminal.BeginGroup = true;
            this.mnuSaveTerminal.Name = "buttonItem8";
            this.mnuSaveTerminal.Text = "결과저장";
            this.mnuSaveTerminal.ImageSmall = (Image)global::RACTClient.Properties.Resources.run_cancel;
            mnuSaveTerminal.Click += new EventHandler(mnuSaveTerminal_Click);

            this.mnuOption.BeginGroup = true;
            this.mnuOption.Name = "buttonItem11";
            this.mnuOption.Text = "옵션";
            this.mnuOption.ImageSmall = (Image)global::RACTClient.Properties.Resources.run_cancel;
            mnuOption.Click += new EventHandler(mnuOption_Click);
            // 
            // MCTerminalEmulator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.Controls.Add(this.contextMenuBar1);
            this.Name = "MCTerminalEmulator";
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).EndInit();
            this.ResumeLayout(false);
        }
                     

        void mnuSaveTerminal_Click(object sender, EventArgs e)
        {
            WriteTerminalLog();
        }
		// 2019-11-10 ???? (?? ?? ??)
        void mnuOption_Click(object sender, EventArgs e)
        {
            CallOptionHandlerEvent();
        }

        /// <summary>
        /// 종료 처리 합니다.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {

            timer1.Stop();
            SendTelnetStop(); //2019-11-18 전송지연 옵션 처리
            base.Dispose(disposing);

        }
        /// <summary>
        /// 크기 변경 이벤트 처리 입니다.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(System.EventArgs e)
        {
            if (ClientSize.Width == 0 || m_CharSize.Width == 0) return;
            // this.Font = new Font(m_TypeFace, m_TypeSize, m_TypeStyle);

            string tTextAtCursor = "";
            char tCurChar;
            for (int i = 0; i < m_Cols; i++)
            {
                tCurChar = m_CharGrid[m_Caret.Pos.Y][i];

                if (tCurChar == '\0')
                {
                    continue;
                }
                tTextAtCursor = tTextAtCursor + Convert.ToString(tCurChar);
            }

            // 2015-06-01 - 신윤남 - 컬럼사이즈 변경
            int tColumns = ClientSize.Width / m_CharSize.Width - 1;
            int tRows = ClientSize.Height / m_CharSize.Height;

            if (tRows < 5)
            {
                tRows = 5;
                Height = m_CharSize.Height * tRows;
            }

            if (tColumns < 5)
            {
                tColumns = 5;
                Width = m_CharSize.Width * tColumns;
            }

            if (Parent != null)
            {
                if (Bottom > Parent.ClientSize.Height)
                {
                    Height = Parent.ClientSize.Height - Top;
                }
            }

            // 2015-06-01 - 신윤남 - Terminal 사이즈 늘리기
            SetSize(tRows, AppGlobal.s_ClientOption.TerminalColumnCount);

            StringCollection tVisiblebuffer = new StringCollection();
            for (int i = m_ScrollbackBuffer.Count - 1; i >= 0; i--)
            {
                tVisiblebuffer.Insert(0, m_ScrollbackBuffer[i]);
                if (tVisiblebuffer.Count > tRows - 2) break;
            }

            int tLastline = 0;
            for (int i = 0; i < tVisiblebuffer.Count; i++)
            {
                for (int tColumnIndex = 0; tColumnIndex < tColumns; tColumnIndex++)
                {
                    if (tColumnIndex > tVisiblebuffer[i].Length - 1) continue;
                    m_CharGrid[i][tColumnIndex] = tVisiblebuffer[i].ToCharArray()[tColumnIndex];
                }
                tLastline = i;
            }

            for (int tColumnIndex = 0; tColumnIndex < tColumns; tColumnIndex++)
            {
                if (tColumnIndex > tTextAtCursor.Length - 1) continue;
                m_CharGrid[tLastline + 1][tColumnIndex] = tTextAtCursor.ToCharArray()[tColumnIndex];
            }

            CaretToAbs(tTextAtCursor.Length, tLastline + 1);

            base.OnResize(e);

            // 스크롤 재계산(OnResize하면 초기화되어 추가함)
            SetScrollBarValues();
            UpdateAttribGridInverse();
            Refresh();
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighSpeed;
            e.Graphics.TextRenderingHint = TextRenderingHint.SystemDefault;
            e.Graphics.TextContrast = 0;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;

            WipeScreen(e.Graphics);
            Redraw(e.Graphics);
            if (m_IsShowCaret && m_IsConnected && this.Focused)
            {
                ShowCaret(e.Graphics);
            }
        }

        protected override void WndProc(ref Message m)
        {
             //System.Diagnostics.Debug.WriteLine("############             " + m.Msg);
            switch (m.Msg)
            {
                case WMCodes.WM_KEYDOWN:
                case WMCodes.WM_SYSKEYDOWN:
                case WMCodes.WM_KEYUP:
                case WMCodes.WM_SYSKEYUP:
                case WMCodes.WM_SYSCHAR:
                case WMCodes.WM_CHAR:
                    if (m_IsConnected && m_TerminalStatus != E_TerminalStatus.RunScript)
                    {
                        this.m_Keyboard.KeyDown(m);
                    }
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        bool tReturn = false;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            tReturn = false;
            if ((msg.Msg == WMCodes.WM_KEYDOWN) || (msg.Msg == WMCodes.WM_SYSKEYDOWN))
            {
                switch (keyData)
                {
                    case Keys.Tab:
                        if (m_IsConnected && m_TerminalStatus != E_TerminalStatus.RunScript)
                        {
                            this.m_Keyboard.SendTab();
                        }
                        tReturn = true;
                        if (m_TerminalStatus == E_TerminalStatus.Recording)
                        {
                            SaveWaitScript();
                            m_ScriptGenerator.AddSend(new TerminalScriptKeyInfo(Convert.ToString((int)keyData), E_TerminalScriptKeyType.Control));
                        }
                        break;
                    case Keys.Enter:
                        //SaveCommandLog();
                        //m_IsAutoLogSaver = true;
                        if (m_TerminalStatus == E_TerminalStatus.Recording)
                        {
                            SaveWaitScript();
                            m_ScriptGenerator.AddSend(new TerminalScriptKeyInfo(Convert.ToString((int)keyData), E_TerminalScriptKeyType.Control));
                        }
                        break;
                    case Keys.Back:
                        
                        if (m_TerminalStatus == E_TerminalStatus.Recording)
                        {
                            SaveWaitScript();
                            m_ScriptGenerator.AddSend(new TerminalScriptKeyInfo(Convert.ToString((int)keyData), E_TerminalScriptKeyType.Control));
                        }
                        break;

                    default:

                        break;
                }


            }
            return tReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CurArgs"></param>
        protected override void OnMouseMove(MouseEventArgs CurArgs)
        {
            if (CurArgs.Button != MouseButtons.Left) return;
            if (TerminalStatus == E_TerminalStatus.RunScript) return;

            // 2014-08-19 - 신윤남 - 마우스 이동시 좌표값 오류 발생시 오류 로그 저장한다.
            try
            {
                // 영역선택 중인지 상태체크
                if (!IsSelectMode()) return;

                m_EndDrag.X = CurArgs.X;
                m_EndDrag.Y = CurArgs.Y;

                int tEndCol = m_EndDrag.X / m_CharSize.Width;
                int tEndRow = m_EndDrag.Y / m_CharSize.Height;
                int tBegCol = m_BeginDrag.X / m_CharSize.Width;
                int tBegRow = m_BeginDrag.Y / m_CharSize.Height;

                // 상하단 드래그시 자동 스크롤
                if (m_VertScrollBar.Minimum == 0 && m_VertScrollBar.Maximum > 0)
                {
                    if (m_EndDrag.Y > m_BeginDrag.Y)
                    {
                        if (tEndRow > m_Rows - 1 && (CurArgs.Y % m_CharSize.Height > m_CharSize.Height * .5))
                        {
                            m_VertScrollBar.IsChangeValue = true;
                            DisplayScrollLast(m_VertScrollBar.Value + 1);
                        }
                    }
                    else if (m_EndDrag.Y < m_BeginDrag.Y)
                    {
                        if (tEndRow == 0 && (CurArgs.Y < m_CharSize.Height * .7))
                        {
                            m_VertScrollBar.IsChangeValue = true;
                            DisplayScrollLast(m_VertScrollBar.Value - 1);
                        }
                    }
                }

                this.m_EndRow = _MousePointToRow(CurArgs.Y);
                this.m_EndCol = CurArgs.X / m_CharSize.Width;

                UpdateAttribGridInverse();
                this.Refresh();
            }
            catch (Exception ex)
            {
                AppGlobal.s_FileLogProcessor.PrintLog("OnMouseMove exception : " + ex.Message.ToString());
            }
        }

        /// <summary>
        /// 마우스 up 처리 합니다.
        /// </summary>
        /// <param name="CurArgs"></param>
        protected override void OnMouseUp(MouseEventArgs CurArgs)
        {
            if (CurArgs.Button == System.Windows.Forms.MouseButtons.Left)
            {
                // 영역선택 중인지 상태체크
                if (!IsSelectMode()) return;

                if (this.m_BeginDrag.X == CurArgs.X && this.m_BeginDrag.Y == CurArgs.Y)
                {
                    Deselect();
                    UpdateAttribGridInverse();
                    this.Refresh();
                }
                else
                {
                    // 2014-07-02 - 신윤남 - 스크롤 후 값 복사하는 기능 추가
                    int tRow = 0;
                    int tCol = 0;

                    this.m_EndRow = _MousePointToRow(CurArgs.Y);
                    this.m_EndCol = CurArgs.X / m_CharSize.Width;


                    int tEndCol = 0;
                    int tEndRow = 0;

                    int tBegCol = 0;
                    int tBegRow = 0;

                    //string result = 

                    //string[] arrayResult = result.Split(new char[]{','});

                    int i = 0;
                    List<char[]> list = new List<char[]>();

                    for (i = 0; i < m_ScrollbackBuffer.Count; i++)
                    {
                        char[] array = m_ScrollbackBuffer[i].ToCharArray();
                        list.Add(array);
                    }

                    if (m_BeginRow == m_EndRow && m_BeginCol < m_EndCol)
                    {
                        if (m_VertScrollBar.Maximum == 0)
                        {
                            tBegRow = m_BeginRow - 1;
                            tBegCol = m_BeginCol;

                            tEndRow = m_EndRow - 1;
                            tEndCol = m_EndCol;
                        }
                        else
                        {
                            tBegRow = m_BeginRow;
                            tBegCol = m_BeginCol;

                            tEndRow = m_EndRow;
                            tEndCol = m_EndCol;
                        }

                    }
                    else if (m_BeginRow == m_EndRow && m_BeginCol > m_EndCol)
                    {
                        if (m_VertScrollBar.Maximum == 0)
                        {
                            tBegRow = m_BeginRow - 1;
                            tBegCol = m_EndCol;

                            tEndRow = m_EndRow - 1;
                            tEndCol = m_BeginCol;
                        }
                        else
                        {
                            tBegRow = m_BeginRow;
                            tBegCol = m_EndCol;

                            tEndRow = m_EndRow;
                            tEndCol = m_BeginCol;
                        }

                    }
                    else if (m_BeginRow < m_EndRow)
                    {
                        if (m_VertScrollBar.Maximum == 0)
                        {
                            tBegRow = m_BeginRow - 1;
                            tBegCol = m_BeginCol;

                            tEndRow = m_EndRow - 1;
                            tEndCol = m_EndCol;
                        }
                        else
                        {
                            tBegRow = m_BeginRow;
                            tBegCol = m_BeginCol;

                            tEndRow = m_EndRow;
                            tEndCol = m_EndCol;
                        }


                    }
                    else if (m_BeginRow > m_EndRow)
                    {
                        if (m_VertScrollBar.Maximum == 0)
                        {
                            tBegRow = m_EndRow - 1;
                            tBegCol = m_EndCol;

                            tEndRow = m_BeginRow - 1;
                            tEndCol = m_BeginCol;
                        }
                        else
                        {
                            tBegRow = m_EndRow;
                            tBegCol = m_EndCol;

                            tEndRow = m_BeginRow;
                            tEndCol = m_BeginCol;
                        }
                    }


                    for (tRow = tBegRow; tRow <= tEndRow; tRow++)
                    {

                        if (tRow < list.Count && tRow >= 0)
                        {
                            char[] result = list[tRow];
                            if (tRow == tBegRow && tRow == tEndRow)
                            {

                                if (tBegCol > result.GetLength(0))
                                {
                                    tBegCol = result.GetLength(0) - 1;
                                    if (tBegCol == -1) tBegCol = 0;
                                }
                                /*
                                for (tCol = tBegCol; tCol <= result.GetLength(0); tCol++)
                                {
                                    if (tCol < 0) tCol = 0;
                                    if (tCol < result.GetLength(0))
                                    {
                                        m_CopyValue.Append(result[tCol]);
                                    }
                                }
                                */
                             
                                for (tCol = tBegCol; tCol <= tEndCol; tCol++)
                                {
                                    if (tCol < 0) tCol = 0;
                                    if (tCol < result.GetLength(0))
                                    {

                                        m_CopyValue.Append(result[tCol]);
                                    }
                                }

                                
                            }
                            else if (tRow == tBegRow && tRow != tEndRow)
                            {
                                for (tCol = tBegCol; tCol <= result.GetLength(0); tCol++)
                                {
                                    if (tCol < 0) tCol = 0;
                                    if (tCol < result.GetLength(0))
                                    {
                                        m_CopyValue.Append(result[tCol]);
                                    }
                                }
                            }
                            else if (tRow != tBegRow && tRow == tEndRow)
                            {
                                for (tCol = 0; tCol <= tEndCol; tCol++)
                                {
                                    if (tCol < result.GetLength(0))
                                    {
                                        if (tCol < 0) tCol = 0;
                                        m_CopyValue.Append(result[tCol]);
                                    }
                                }
                            }
                            else
                            {
                                for (tCol = 0; tCol <= result.GetLength(0); tCol++)
                                {
                                    if (tCol < result.GetLength(0))
                                    {
                                        if (tCol < 0) tCol = 0;
                                        m_CopyValue.Append(result[tCol]);
                                    }

                                }
                            }

                            //2016-03-31 서영응 선택(드레그) Copy 시 엔터값 넣음

                            if (tEndRow > tRow)
                             m_CopyValue.Append("\r\n");
                        }

                    }

                    //System.Diagnostics.Debug.WriteLine(m_CopyValue);

                    list.Clear();
                    list = null;
                }

            }

        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            base.OnScroll(se);
        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            int tTextLinesToMove = e.Delta * SystemInformation.MouseWheelScrollLines / 120;

            if (tTextLinesToMove == 0) return;

            m_VertScrollBar.IsChangeValue = true;

            if (tTextLinesToMove < 0)
            {
                if (m_VertScrollBar.Value + (tTextLinesToMove * -1) >= m_VertScrollBar.Maximum)
                {
                    m_VertScrollBar.Value = m_VertScrollBar.Maximum;
                }
                else
                {
                    m_VertScrollBar.Value += (tTextLinesToMove * -1);
                }
                //  HandleScroll(m_VertScrollBar, new ScrollEventArgs(ScrollEventType.LargeIncrement, m_VertScrollBar.Value, (tTextLinesToMove * -1)));

            }
            else
            {
                if (m_VertScrollBar.Value + (tTextLinesToMove * -1) <= m_VertScrollBar.Minimum)
                {
                    m_VertScrollBar.Value = m_VertScrollBar.Minimum;
                }
                else
                {
                    m_VertScrollBar.Value += (tTextLinesToMove * -1);
                }
                //  HandleScroll(m_VertScrollBar, new ScrollEventArgs(ScrollEventType.LargeDecrement, m_VertScrollBar.Value, (tTextLinesToMove * -1)));
            }

            this.Invalidate();

            base.OnMouseWheel(e);
        }

        /// <summary>
        /// 마우스 다운 처리 입니다.
        /// </summary>
        /// <param name="CurArgs"></param>
        protected override void OnMouseDown(MouseEventArgs aCurArgs)
        {
            this.Focus();

            Font tFont = new Font(FontFamily.GenericMonospace, 8.5F);
            Graphics tGraphics = this.CreateGraphics();

            if (aCurArgs.Button == MouseButtons.Right)
            {
                if (this.Parent is TerminalWindows)
                {
                    cmPopUP.Popup(MousePosition);
                }
                else
                {
                    if (AppGlobal.s_ClientOption.TerminalPopupType == E_TerminalPopupType.None)
                    {
                        TerminalPopupType tType = new TerminalPopupType();
                        tType.ShowDialog(this);
                    }

                    if (AppGlobal.s_ClientOption.TerminalPopupType == E_TerminalPopupType.CopyPaste)
                    {
                        mnuCopy_Click(null, null);
                        mnuPaste_Click(null, null);
                    }
                    else
                    {
                        cmPopUP.Popup(MousePosition);
                        //ShowPopManu(aCurArgs);
                    }
                }
            }
            else if (aCurArgs.Button == MouseButtons.Left)
            {
                if (m_ScrollbackBuffer.Count < 1)
                {
                    Deselect();
                }
                else
                {
                    this.m_BeginDrag.X = aCurArgs.X;
                    this.m_BeginDrag.Y = aCurArgs.Y;

                    // 2014-07-02 - 신윤남 - 스크롤 후 값 복사하는 기능 추가
                    if (m_CopyValue.Length > 0)
                    {
                        m_CopyValue.Remove(0, m_CopyValue.Length);
                    }

                    this.m_BeginRow = _MousePointToRow(aCurArgs.Y);
                    this.m_BeginCol = aCurArgs.X / m_CharSize.Width;
                }
            }
            base.OnMouseDown(aCurArgs);
        }


        /// <summary>
        /// 모두 삭제 처리 입니다.
        /// </summary>
        void mnuClear_Click(object sender, EventArgs e)
        {
            Deselect();
            m_ScrollbackBuffer.Clear();
            SetScrollBarValues();

            //초기화 합니다.
            for (int i = 0; i < this.m_Rows; i++)
            {
                Array.Clear(this.m_CharGrid[i], 0, this.m_CharGrid[i].Length);
                Array.Clear(this.m_AttribGrid[i], 0, this.m_AttribGrid[i].Length);
            }
            CaretToAbs(0, 0);
                
            //m_ScrollbackBuffer.Clear();
            //m_VertScrollBar.Visible = false;
            //m_VertScrollBar.Value = 0;
            //m_VertScrollBar.Maximum = 0;
            //m_VertScrollBar.Minimum = -1;
            ////초기화 합니다.
            //for (int i = 0; i < this.m_Rows; i++)
            //{
            //    Array.Clear(this.m_CharGrid[i], 0, this.m_CharGrid[i].Length);
            //    Array.Clear(this.m_AttribGrid[i], 0, this.m_AttribGrid[i].Length);
            //}
            //CaretToAbs(0, 0);
        }
		// 2019-11-10 ???? (??? ??? ??? ?? ??_???  )
        /// <summary>
        /// 모두 삭제 처리 입니다.
        /// </summary>
        void mnuCmdClear_Click(object sender, EventArgs e)
        {
            if (m_TerminalStatus == E_TerminalStatus.RunScript) return;
            String backCnt = "";

            for (int j = 0; j < GetCmd().Length + 1; j++)
            {
                backCnt += "\b";
            }
            this.DispatchMessage(this, backCnt);

        }

        //2015-09-30
        /// <summary>
        /// 기본 명령 조회.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void mnuSearchDefaultCmd_Click(object sender, EventArgs e)
        {
            using (SearchDefaultCmdForm tForm = new SearchDefaultCmdForm())
            {
                // Console.WriteLine("MODE : " + this.TerminalMode);
                tForm.txtSearch.Text = GetCmd().TrimStart();
                tForm.modelID = m_DeviceInfo.ModelID;
                tForm.SendCmd += defaultFrom_Command;
                tForm.StartPosition = FormStartPosition.CenterParent;
                tForm.ShowDialog(this.ParentForm);
            }
               
        }

        
        //2015-10-07
        /// <summary>
        /// 일괄 명령실행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void mnuBatchCmd_Click(object sender, EventArgs e)
        {
            BatchCmdForm tForm = new BatchCmdForm(this);           
            tForm.StartPosition = FormStartPosition.CenterParent;
            tForm.SendBatchCommandFunction += new BatchCmdForm.SendBatchCommand(RunBatchCommnad);
            tForm.ShowDialog(this.ParentForm);
        }


        //2015-09-30
        /// <summary>
        /// [ Gunny ] 선택된 기본 명령어 표시
        /// </summary>
        /// <param name="DefaultCmd"></param>
        void defaultFrom_Command(string DefaultCmd)
        {
            if (GetCmd().Length > 0)
            {
                int i = 0;
                while (GetCmd().Length != i)
                {
                    this.DispatchMessage(this, "\b");
                    i++;
                }
            }
                this.DispatchMessage(this, DefaultCmd);
        }


        //2015-09-30
        /// <summary>
        /// [ Gunny ] 자동 명령어 표시
        /// </summary>
        /// <param name="DefaultCmd"></param>
        void SetAutoCompleteCmd(string AutoCKeyCmd)
        {
            FromAutoCmd = true;
            this.DispatchMessage(this, ""+AutoCKeyCmd);
        }
        


        //2015-10-07
        /// <summary>
        /// [ Gunny ] 순차적 명령어 실행
        /// </summary>
        /// <param name="DefaultCmd"></param>
        void RunBatchCommnad(string BatchCmd , decimal CycleTime)
        {
            //DispatchMessage(this, BatchCmd);
            if (isBatchCmdRunning)
            {
                return;
            }
            isBatchCmdRunning = true;
            BatchCmdArray = BatchCmd.Split('\n');

            timer = new System.Windows.Forms.Timer();
            this.timer.Interval = (int)((double)CycleTime * 1000);
            this.timer.Tick += new System.EventHandler(this.Work);
            Work(null, null);
            timer.Start();
            
        }

        private void Work(object sender, EventArgs e)
        {

            if (BatchCmdCount == BatchCmdArray.Length)
            {
                BatchCmdCount = 0;
                timer.Stop();
                isBatchCmdRunning = false;
                return;

            }

                            
            DispatchMessage(this, BatchCmdArray.GetValue(BatchCmdCount).ToString() + "\r\n");
            SaveCommandLog(false);
            m_IsPressEnter = true;
            BatchCmdCount++;
        }



        private static DateTime Delay(int MS)
        {

            DateTime ThisMoment = DateTime.Now;

            TimeSpan duration = new TimeSpan(0, 0, 0, 0, MS);

            DateTime AfterWards = ThisMoment.Add(duration);

            while (AfterWards >= ThisMoment)
            {

                System.Windows.Forms.Application.DoEvents();

                ThisMoment = DateTime.Now;

            }

            return DateTime.Now;

        }

        /// <summary>
        /// 스크립트 멈춤
        /// </summary>
        void mnuStopScript_Click(object sender, EventArgs e)
        {
            ScriptWork(E_ScriptWorkType.RunCancel);
        }


        /// <summary>
        /// 모두 선택 처리 입니다.
        /// </summary>
        void mnuSelectAll_Click(object sender, EventArgs e)
        {
            // 선택영역 설정
            m_BeginRow = 0;
            m_BeginCol = 0;
            m_EndRow = m_ScrollbackBuffer.Count - 1;
            m_EndCol = m_Cols;

            UpdateAttribGridInverse();


            // 내용 Copy
            List<char[]> list = new List<char[]>();

            for (int i = 0; i < m_ScrollbackBuffer.Count; i++)
            {
                char[] array = m_ScrollbackBuffer[i].ToCharArray();
                list.Add(array);
            }

            if (m_CopyValue.Length > 0)
            {
                m_CopyValue.Remove(0, m_CopyValue.Length);
            }
            

            for (int i = 0; i < list.Count; i++)
            {
                char[] result = list[i];
                for(int j=0;j<result.Length;j++)
                {
                    m_CopyValue.Append(result[j]);    
                }
                m_CopyValue.Append("\r\n");
            }
            list.Clear();
            list = null;


            this.Refresh();
        }

        /// <summary>
        /// 찾기 폼 입니다.
        /// </summary>
        private TelnetFindForm m_FindForm = null;
        /// <summary>
        /// 찾기 처리 입니다.
        /// </summary>
        void mnuFind_Click(object sender, EventArgs e)
        {
            if (m_FindForm == null)
            {
                m_FindForm = new TelnetFindForm();
                m_FindForm.OnTelnetStringFind += new TelnetStringFindHandler(TelnetFindForm_OnTelnetStringFind);
                m_FindForm.FormClosing += new FormClosingEventHandler(s_TelnetFindForm_FormClosing);
                if (this.Parent is TerminalWindows)
                {
                    m_FindForm.ShowDialog((Form)this.Parent);
                }
                else
                {
                    m_FindForm.Show(AppGlobal.s_ClientMainForm);
                }
            }
            else if (!m_FindForm.Visible)
            {
                m_FindForm.Visible = true;
            }

            if (OnTelnetFindString != null) OnTelnetFindString();
                
            
            

            //if (m_FindForm == null)
            //{
            //    m_FindForm = new TelnetFindForm();
            //    m_FindForm.OnTelnetStringFind += new TelnetStringFindHandler(TelnetFindForm_OnTelnetStringFind);
            //    m_FindForm.FormClosing += new FormClosingEventHandler(s_TelnetFindForm_FormClosing);
            //    if (this.Parent is TerminalWindows)
            //    {
            //        m_FindForm.ShowDialog((Form)this.Parent);
            //    }
            //    else
            //    {
            //        m_FindForm.Show(AppGlobal.s_ClientMainForm);
            //    }
            //}
            //else if (!m_FindForm.Visible)
            //{
            //    m_FindForm.Visible = true;
            //}

            //if (OnTelnetFindString != null) OnTelnetFindString();
        }

        void s_TelnetFindForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            FindForm_Close();
        }
        /// <summary>
        /// 찾기 처리 합니다.
        /// </summary>
        void TelnetFindForm_OnTelnetStringFind(TelnetStringFindHandlerArgs aStringArgs)
        {


            FindForm_OnTelnetStringFind(aStringArgs);
        }

        /// <summary>
        /// 터미널에서 해당 문자를 찾습니다.
        /// </summary>
        /// <param name="aString">찾을 문자 입니다.</param>
        public void FindForm_OnTelnetStringFind(TelnetStringFindHandlerArgs aArgs)
        {
            TelnetStringFind tFindInfo;
            if (!FindString(aArgs, out tFindInfo))
            {
                //찾지 못함
                this.Invoke(new TelnetStringFindHandler(ShowNotFindMessage), new object[] { aArgs });
            }
            ApplyFindInformation(tFindInfo);
            this.Invoke(this.OnRefreshEvent);
        }

        /// <summary>
        /// 못 찾았다고 표시한다.
        /// </summary>
        private void ShowNotFindMessage(TelnetStringFindHandlerArgs aArgs)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new TelnetStringFindHandler(ShowNotFindMessage), new object[] { aArgs });
                return;
            }

            AppGlobal.ShowMessageBox(m_FindForm, "'" + aArgs.FindString + "'을(를) 찾을 수 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        /// <summary>
        /// 복사 처리 합니다.
        /// </summary>
        private void mnuCopy_Click(object sender, System.EventArgs e)
        {
            if (m_CopyValue.Length < 1) 
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "복사할 내용이 없습니다.\r\n다시 선택해주십시오.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Clipboard.SetDataObject(m_CopyValue.ToString());
            //Console.WriteLine("MODE : " + this.TerminalMode);

            // 2014-07-02 - 신윤남 - 스크롤 후 값 복사하는 기능 추가
            //Clipboard.SetDataObject(m_CopyValue.ToString());

            /*
            Point tStartPoint = new Point();
            Point tStopPoint = new Point();
            bool tFoundStart = false;
            bool tFoundStop = false;

            for (int aRow = 0; aRow < m_Rows; aRow++)
            {
                for (int aCol = 0; aCol < m_Cols; aCol++)
                {
                    if (!tFoundStart && m_AttribGrid[aRow][aCol].IsInverse)
                    {
                        tStartPoint.X = aCol;
                        tStartPoint.Y = aRow;
                        tFoundStart = true;
                    }

                    if (tFoundStart && !tFoundStop && !m_AttribGrid[aRow][aCol].IsInverse && m_CharGrid[aRow][aCol] != '\0')
                    {
                        tStopPoint.X = aCol - 1;
                        tStopPoint.Y = aRow;
                        tFoundStop = true;

                        if (aCol == 0)
                        {
                            aRow--;
                            while (this.m_CharGrid[aRow][0] == '\0')
                            {
                                aRow--;
                            }

                            for (aCol = 0; aCol < m_Cols; aCol++)
                            {
                                if (this.m_CharGrid[aRow][aCol] == '\0')
                                {
                                    tStopPoint.X = aCol - 1;
                                    tStopPoint.Y = aRow;
                                }
                            }
                        }
                        break;
                    }

                    if (tFoundStop && tFoundStart)
                    {
                        break;
                    }
                }

                if (tFoundStart && !tFoundStop && aRow == m_Rows - 1)
                {
                    for (int tCol = 0; tCol < this.m_Cols; tCol++)
                    {
                        if (this.m_CharGrid[aRow][tCol] == '\0')
                        {
                            tStopPoint.X = tCol - 1;
                            tStopPoint.Y = aRow;
                        }
                    }
                }
            }
            Console.WriteLine("start.X " + Convert.ToString(tStartPoint.X) +
                             " start.Y " + Convert.ToString(tStartPoint.Y) +
                             " stop.X " + Convert.ToString(tStopPoint.X) +
                             " stop.Y " + Convert.ToString(tStopPoint.Y));

            this.ScreenScrape(tStartPoint.X, tStartPoint.Y, tStopPoint.X, tStopPoint.Y);
            */
            //StringCollection tStringCollection = this.ScreenScrape(tStartPoint.X, tStartPoint.Y,tStopPoint.X, tStopPoint.Y);
            //foreach (string tString in tStringCollection)
            //{
            //    Console.WriteLine(tString);
            //}

        }
        /// <summary>
        /// 붙이기처리 합니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        //20190307 KangBonghan
        //제한 명령어 입력시 오동작 이슈
        //메모장이나 파일에서 \n 포함되게 단일 명령어 또는 복수 명령어를 드래그 앤 카피 후 붙여넣기시 제한명령어를 체크하시 않고
        //장비에서 명령어를 실행하게됨.
        //1. \n\r으로 명령 문자열을 구분
        //2. 구분한 명령어별로 DispatchMessage 를 실행하도록 수정
        //3. \n\r으로 구분하였는지 구분한 명령어에 \n\r를 포함하여야 하는지 체크하는 변수 필요(\r 만전송)
        //4. 복수 명령어를 드래그 앤 카피 시 사전에 입력된 값이 있는지 체크 후 처리
        //5. \r 포함된 명령어 라인은 DB로그 저장이 안되는 현상이 있어 따로 저장 할 수 있도록 처리
        //6. mnuPasteCR_Click함수에는 강제 \r이 붙기때문에 DB로그 저장 처리 하도록 수정
        String[] SepStrs = {"\r\n"};
        String SepStr = "\r\n";
        private void mnuPaste_Click(object sender, System.EventArgs e)
        {
            /*
            if (m_TerminalStatus == E_TerminalStatus.RunScript) return;

            IDataObject tClipboard = Clipboard.GetDataObject();

            if (tClipboard.GetDataPresent(DataFormats.Text))
            {
                if (tClipboard.GetData(DataFormats.Text) != null)
                {
                    String cmd = tClipboard.GetData(DataFormats.Text).ToString();

                    this.Invoke(this.OnRxdTextEvent, new string[] { string.Copy(cmd) });
                    this.Invoke(this.OnRefreshEvent);
                }
            }

            */

            //CheckPrompt();

            if (false)
            {
                if (m_TerminalStatus == E_TerminalStatus.RunScript) return;
                String backCnt = "";
                String PreCmd = GetCmd();

                IDataObject tClipboard = Clipboard.GetDataObject();

                if (tClipboard.GetDataPresent(DataFormats.Text))
                {
                    if (tClipboard.GetData(DataFormats.Text) != null)
                    {
                        String Cmd = tClipboard.GetData(DataFormats.Text).ToString();
                        int SepCnt = Regex.Matches(Cmd, SepStr).Count;
                        String[] CmdStr = Cmd.Split(SepStrs, StringSplitOptions.None);

                        for (int i = 0; i < CmdStr.Length; i++)
                        {
                            String CurrentCmd = CmdStr[i].ToString();

                            if (SepCnt > 0)
                            {
                                if (CurrentCmd.Length > 0)
                                {
                                    if (IsLimitCmd(PreCmd + CurrentCmd))
                                    {
                                        SavePasteCommandLog(true, PreCmd + CurrentCmd);
                                        backCnt = "";
                                        for (int j = 0; j < PreCmd.Length + 1; j++)
                                        {
                                            backCnt += "\b";
                                        }
                                        this.DispatchMessage(this, backCnt);

                                    }
                                    else
                                    {
                                        m_IsPressEnter = true;
                                        //this.DispatchMessage(this, CurrentCmd + "\r");
                                        SendCommand(CurrentCmd + "\r");
                                        SavePasteCommandLog(false, PreCmd + CurrentCmd);
                                    }
                                    PreCmd = "";
                                }
                                //SepCnt--;
                            }
                            else
                            {
                                if (CurrentCmd.Length > 0)
                                {
                                    this.DispatchMessage(this, CurrentCmd);
                                    //SaveCommandLog(false);
                                }
                            }


                            /*
                            if (IsLimitCmd(CurrentCmd))
                            {

                                if (SepCnt > 0)
                                {
                                    SavePasteCommandLog(true, CurrentCmd);
                                    SepCnt--;
                                }
                                else
                                {
                                    this.DispatchMessage(this, CurrentCmd);
                                }
                                continue;
                            }
                            else
                            {
                                if (SepCnt > 0)
                                {
                                    this.DispatchMessage(this, CurrentCmd + "\r");
                                    SepCnt--;
                                }
                                else
                                {
                                    this.DispatchMessage(this, CurrentCmd);
                                }
                                SaveCommandLog(false);
                            }
                            */

                        }

                    }
                }
            }
            else
            {
                if (m_TerminalStatus == E_TerminalStatus.RunScript) return;

                String PreCmd = GetCmd();

                IDataObject tClipboard = Clipboard.GetDataObject();

                if (tClipboard.GetDataPresent(DataFormats.Text))
                {
                    if (tClipboard.GetData(DataFormats.Text) != null)
                    {
                        String Cmd = tClipboard.GetData(DataFormats.Text).ToString();
                        int SepCnt = Regex.Matches(Cmd, SepStr).Count;
                        String[] CmdStr = Cmd.Split(SepStrs, StringSplitOptions.RemoveEmptyEntries);

                        //단일 명령일때 처리 
                        if (CmdStr.Length == 1)
                        {
                            if (IsLimitCmd(Cmd))
                            {
                                return;
                            }
                            this.DispatchMessage(this, Cmd);

                        }
                        else
                        {

                            //2. 제한 명령어 확인 
                            for (int i = 0; i < CmdStr.Length; i++)
                            {
                                String CurrentCmd = CmdStr[i].ToString();

                                //if (SepCnt > 0)
                                {
                                    if (CurrentCmd.Length > 0)
                                    {
                                        if (IsLimitCmd(CurrentCmd))
                                        {
                                            return;
                                        }
                                    }
                                }
                            }
                            //3.스크립트로 해당 명령어 OR 명령어들 수행
                            Script tCommandScript = null;

                            //스크립트 타임아웃 값 설정 명령어당 적절한 수치를 설정하기 애매함.
                            //Cmd 당 기본 30으로 더함. 추후 옵션 메뉴에서 따로 설정하도록 기능 지원하면.. 사용자 편의 제공.
                            AppGlobal.s_MultipleCmd = 60 + (30 * CmdStr.Length);
                            tCommandScript = ScriptGenerator.MakeBatchCommand(Cmd.Replace("\r\n","\r"), m_Prompt);

                            //tCommandScript.ScriptType = E_ScriptType.WaitScript;
                            RunScript(tCommandScript);

                            //4. 수행한 명령어 로그 저장(복수 명령어 일때만 저장)
                            for (int i = 0; i < CmdStr.Length; i++)
                            {
                                String CurrentCmd = CmdStr[i].ToString();

                                if (CurrentCmd.Length > 0)
                                {
                                    SavePasteCommandLog(false, CurrentCmd);
                                }
                            }

                        }
                        
                    }
                }
            }
    
            
            /*
            if (m_TerminalStatus == E_TerminalStatus.RunScript) return;

            IDataObject tClipboard = Clipboard.GetDataObject();

            if (tClipboard.GetDataPresent(DataFormats.Text))
            {
                if (tClipboard.GetData(DataFormats.Text) != null)
                {
                    String Cmd = tClipboard.GetData(DataFormats.Text).ToString();
                    int SepCnt = Regex.Matches(Cmd, SepStr).Count;
                    String[] CmdStr = Cmd.Split(SepStrs, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < CmdStr.Length; i++)
                    {
                        if (SepCnt > 0)
                        {
                            foreach (Char chStr in CmdStr[i])
                            {
                                //PrintChar(chStr);
                                CheckPrompt();
                                Console.WriteLine("==============================DispatchMessage==============================");
                                this.DispatchMessage(this, chStr.ToString());
                                //Thread.Sleep(100);

                                System.Diagnostics.Debug.WriteLine(chStr.ToString());
                            }
                            //PrintChar('\r');
                            //PrintChar('\n');
                             CheckPrompt();
                             KeyInfo tControlKey = m_Keyboard.EenterKeyinfo();
                             Thread.Sleep(3000);
                             Console.WriteLine("==============================DispatchControlMessage==============================");
                             this.DispatchControlMessage(this, tControlKey);
                            //m_IsPressEnter = true;
                            SepCnt--;
                        }
                        else
                        {
                            foreach (Char chStr in CmdStr[i])
                            {
                                //PrintChar(chStr);
                                CheckPrompt();
                                this.DispatchMessage(this, chStr.ToString());
                                System.Diagnostics.Debug.WriteLine(chStr.ToString());
                            }
                        }                       
                    }
                }
            }*/
            
        }



        /// <summary>
        /// 자동완성 합니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuAutoC_Click(object sender, System.EventArgs e)
        {

            m_Keyboard.CtrlIsDown = false;

            string lineCmd = GetCmd();

            if (lineCmd == "")
            {
                return;
            }

            AutoCompleteKey tForm = new AutoCompleteKey();
            tForm.keyText = lineCmd;
            tForm.modelID = m_DeviceInfo.ModelID;
            tForm.SetAutoCompleteKey += new AutoCompleteKey.SetAutoCompleteCmd(SetAutoCompleteCmd);
            tForm.StartPosition = FormStartPosition.CenterParent;
            tForm.Location = new Point(m_Caret.Pos.X * m_CharSize.Width, m_Caret.Pos.Y * m_CharSize.Height);
            tForm.ShowDialog(this.ParentForm);

            //m_Keyboard.CtrlIsDown= false;

            //string lineCmd = GetCmd();
                       
            //if (lineCmd == "")
            //{
            //    return;
            //}

            //AutoCompleteKey tForm = new AutoCompleteKey();
            //tForm.keyText = lineCmd;
            //tForm.modelID = m_DeviceInfo.ModelID;
            //tForm.SetAutoCompleteKey += new AutoCompleteKey.SetAutoCompleteCmd(SetAutoCompleteCmd);
            //tForm.StartPosition = FormStartPosition.CenterParent;
            //tForm.Location = new Point(m_Caret.Pos.X * m_CharSize.Width, m_Caret.Pos.Y * m_CharSize.Height);
            //tForm.ShowDialog(this.ParentForm);
            
    
        }
        
        /// <summary>
        /// 붙이기처리 합니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuPasteCR_Click(object sender, System.EventArgs e)
        {

            if (false)
            {
                //CheckPrompt();
                if (m_TerminalStatus == E_TerminalStatus.RunScript) return;
                String backCnt = "";
                String PreCmd = GetCmd();

                IDataObject tClipboard = Clipboard.GetDataObject();

                if (tClipboard.GetDataPresent(DataFormats.Text))
                {
                    if (tClipboard.GetData(DataFormats.Text) != null)
                    {
                        String Cmd = tClipboard.GetData(DataFormats.Text).ToString();
                        int SepCnt = Regex.Matches(Cmd, SepStr).Count;
                        String[] CmdStr = Cmd.Split(SepStrs, StringSplitOptions.None);

                        for (int i = 0; i < CmdStr.Length; i++)
                        {
                            String CurrentCmd = CmdStr[i].ToString();

                            if (SepCnt > 0)
                            {
                                if (CurrentCmd.Length > 0)
                                {
                                    if (IsLimitCmd(PreCmd + CurrentCmd))
                                    {
                                        SavePasteCommandLog(true, PreCmd + CurrentCmd);
                                        backCnt = "";
                                        for (int j = 0; j < PreCmd.Length + 1; j++)
                                        {
                                            backCnt += "\b";
                                        }
                                        this.DispatchMessage(this, backCnt);
                                    }
                                    else
                                    {
                                        this.DispatchMessage(this, CurrentCmd + "\r");
                                        SavePasteCommandLog(false, PreCmd + CurrentCmd);
                                        m_IsPressEnter = true;
                                    }
                                    PreCmd = "";
                                }
                                SepCnt--;
                            }
                            else
                            {
                                //강제 CR을 보내면 제한명령어 체크가 되지 않음.
                                //해서 제한명령어를 먼저 체크를 하고 아닐 경우에만 CR을 보낼 수 있도록 한다.
                                if (CurrentCmd.Length > 0)
                                {
                                    if (IsLimitCmd(PreCmd + CurrentCmd))
                                    {
                                        SavePasteCommandLog(true, PreCmd + CurrentCmd);
                                        backCnt = "";
                                        for (int j = 0; j < PreCmd.Length + 1; j++)
                                        {
                                            backCnt += "\b";
                                        }
                                        this.DispatchMessage(this, backCnt);
                                    }
                                    else
                                    {
                                        this.DispatchMessage(this, CurrentCmd + "\r");
                                        SavePasteCommandLog(false, PreCmd + CurrentCmd);
                                        m_IsPressEnter = true;
                                    }
                                    PreCmd = "";
                                }
                            }

                        }

                    }
                }
            }
            else
            {
                if (m_TerminalStatus == E_TerminalStatus.RunScript) return;
                String backCnt = "";
                String PreCmd = GetCmd();

                IDataObject tClipboard = Clipboard.GetDataObject();

                if (tClipboard.GetDataPresent(DataFormats.Text))
                {
                    if (tClipboard.GetData(DataFormats.Text) != null)
                    {
                        String Cmd = tClipboard.GetData(DataFormats.Text).ToString();
                        int SepCnt = Regex.Matches(Cmd, SepStr).Count;
                        String[] CmdStr = Cmd.Split(SepStrs, StringSplitOptions.None);

                        if (CmdStr.Length == 1)
                        {
                            if (IsLimitCmd(Cmd))
                            {
                                return;
                            }
                            this.DispatchMessage(this, Cmd + "\r");                       
                            SavePasteCommandLog(false, Cmd);
                            m_IsPressEnter = true;

                        }
                        else
                        {

                            //2. 제한 명령어 확인 
                            for (int i = 0; i < CmdStr.Length; i++)
                            {
                                String CurrentCmd = CmdStr[i].ToString();

                                //if (SepCnt > 0)
                                {
                                    if (CurrentCmd.Length > 0)
                                    {
                                        if (IsLimitCmd(CurrentCmd))
                                        {
                                            return;
                                        }
                                    }
                                }
                            }
                            //3.스크립트로 해당 명령어 OR 명령어들 수행
                            Script tCommandScript = null;

                            //스크립트 타임아웃 값 설정 명령어당 적절한 수치를 설정하기 애매함.
                            //Cmd 당 기본 30으로 더함. 추후 옵션 메뉴에서 따로 설정하도록 기능 지원하면.. 사용자 편의 제공.
                            AppGlobal.s_MultipleCmd = 60 + (30 * CmdStr.Length);
                            tCommandScript = ScriptGenerator.MakeBatchCommand(Cmd.Replace("\r\n", "\r"), m_Prompt);

                            //tCommandScript.ScriptType = E_ScriptType.WaitScript;
                            RunScript(tCommandScript);

                            //4. 수행한 명령어 로그 저장(복수 명령어 일때만 저장)
                            for (int i = 0; i < CmdStr.Length; i++)
                            {
                                String CurrentCmd = CmdStr[i].ToString();

                                if (CurrentCmd.Length > 0)
                                {
                                    SavePasteCommandLog(false, CurrentCmd);
                                }
                            }

                        }

                    }
                }
            }


            /*
            if (m_TerminalStatus == E_TerminalStatus.RunScript) return;

            IDataObject tClipboard = Clipboard.GetDataObject();

            if (tClipboard.GetDataPresent(DataFormats.Text))
            {
                if (tClipboard.GetData(DataFormats.Text) != null)
                {
                    String cmd = tClipboard.GetData(DataFormats.Text).ToString();
                    if (IsLimitCmd(cmd))
                    {
                        SaveCommandLog(true);
                        return;
                    }
                    else
                    {
                        this.DispatchMessage(this, tClipboard.GetData(DataFormats.Text).ToString() + "\r\n");
                        SaveCommandLog(false);
                        m_IsPressEnter = true;
                    }
                }
            }
            */
            //if (m_TerminalStatus == E_TerminalStatus.RunScript) return;

            //IDataObject tClipboard = Clipboard.GetDataObject();

            //if (tClipboard.GetDataPresent(DataFormats.Text))
            //{
            //    if (tClipboard.GetData(DataFormats.Text) != null)
            //    {
            //        String cmd = tClipboard.GetData(DataFormats.Text).ToString();
            //        if (IsLimitCmd(cmd))
            //        {
            //            SaveCommandLog(true);
            //            return;
            //        }
            //        else
            //        {
            //                this.DispatchMessage(this, tClipboard.GetData(DataFormats.Text).ToString() + "\r\n");
            //                SaveCommandLog(false);
            //                m_IsPressEnter = true;
            //        }
            //    }
            //}
        }


        void m_VertScrollBar_OnChangeScrollValue(int aValue)
        {
            if (!m_VertScrollBar.Visible) return;
            if (!m_VertScrollBar.IsChangeValue) return;
            lock (this)
            {
                if (!this.m_Caret.IsOff)
                {
                    m_TextAtCursor = "";
                    char tCurrentChar;
                    for (int i = 0; i < this.m_Cols; i++)
                    {
                        tCurrentChar = this.m_CharGrid[this.m_Caret.Pos.Y][i];
                        if (tCurrentChar == '\0')
                        {
                            continue;
                        }
                        m_TextAtCursor = m_TextAtCursor + Convert.ToString(tCurrentChar);
                    }
                    CaretOff();
                }


                if (m_VertScrollBar.Value == m_VertScrollBar.Maximum)
                {
                    m_LastVisibleLine = 0;
                }
                else
                {
                    m_LastVisibleLine += aValue;
                }

                if (m_LastVisibleLine > 0)
                {
                    m_LastVisibleLine = 0;
                }

                if (m_LastVisibleLine < 0 - m_ScrollbackBuffer.Count + m_Rows - 1)
                {
                    m_LastVisibleLine = 0 - m_ScrollbackBuffer.Count + m_Rows - 1;
                }


                int tColumns = m_Cols;
                int tRows = m_Rows;

                //2015-06-01 - 신윤남 - 컬럼사이즈 변경
                //this.SetSize(tRows, tColumns);
                this.SetSize(tRows, AppGlobal.s_ClientOption.TerminalColumnCount);

                StringCollection tVisiblebuffer = new StringCollection();

                for (int i = m_ScrollbackBuffer.Count - 1 + m_LastVisibleLine; i >= 0; i--)
                {
                    tVisiblebuffer.Insert(0, this.m_ScrollbackBuffer[i]);

                    if (tVisiblebuffer.Count >= tRows - 1)
                    {
                        break;
                    }
                }

                // 문자 표시
                for (int i = 0; i < tVisiblebuffer.Count; i++)
                {
                    for (int tColumn = 0; tColumn < tColumns; tColumn++)
                    {
                        if (tColumn > tVisiblebuffer[i].Length - 1) continue;
                        m_CharGrid[i][tColumn] = tVisiblebuffer[i].ToCharArray()[tColumn];
                    }
                }

                // Cursor라인 줄 표시
                if (m_LastVisibleLine == 0)
                {
                    CaretOn();

                    for (int column = 0; column < this.m_Cols; column++)
                    {
                        if (column > this.m_TextAtCursor.Length - 1)
                        {
                            continue;
                        }
                        this.m_CharGrid[this.m_Rows - 1][column] = this.m_TextAtCursor.ToCharArray()[column];
                    }
                    this.CaretToAbs(this.m_TextAtCursor.Length, this.m_Rows - 1);
                }
                else
                {
                    this.CaretOff();
                }

                UpdateAttribGridInverse();

                //System.Diagnostics.Debug.WriteLine(string.Concat("m_VertScrollBar = " + m_VertScrollBar.Value + "[" + m_VertScrollBar.Minimum + ", " + m_VertScrollBar.Maximum + "]"));

                this.Refresh();
            }
        }


        void m_HorzScrollBar_OnChangeScrollValue(int aValue)
        {
            if (!m_HorzScrollBar.Visible) return;
            if (!m_HorzScrollBar.IsChangeValue) return;
            
            lock (this)
            {
                
                if (!this.m_Caret.IsOff)
                {
                    m_TextAtCursor = "";
                    char tCurrentChar;
                    for (int i = 0; i < this.m_Cols; i++)
                    {
                        tCurrentChar = this.m_CharGrid[this.m_Caret.Pos.Y][i];
                        if (tCurrentChar == '\0')
                        {
                            continue;
                        }
                        m_TextAtCursor = m_TextAtCursor + Convert.ToString(tCurrentChar);
                    }
                    CaretOff();
                }


                if (m_HorzScrollBar.Value == m_HorzScrollBar.Maximum)
                {
                    m_LastVisibleCol = AppGlobal.s_ClientOption.TerminalColumnCount;
                }
                else
                {
                    m_LastVisibleCol += aValue;
                }

                if (m_LastVisibleCol > AppGlobal.s_ClientOption.TerminalColumnCount)
                {
                    m_LastVisibleCol = AppGlobal.s_ClientOption.TerminalColumnCount;
                }
                
                //if (m_LastVisibleCol <= m_Cols)
                //{
                //    m_LastVisibleCol = m_Cols - 1;
                //}
                

                int tColumns = AppGlobal.s_ClientOption.TerminalColumnCount;
                int tRows = m_Rows;

                //2015-06-01 - 신윤남 - 컬럼사이즈 변경
                //this.SetSize(tRows, tColumns);
                this.SetSize(tRows, AppGlobal.s_ClientOption.TerminalColumnCount);

                StringCollection tVisiblebuffer = new StringCollection();

                for (int i = m_ScrollbackBuffer.Count - 1 + m_LastVisibleLine; i >= 0; i--)
                {
                    tVisiblebuffer.Insert(0, this.m_ScrollbackBuffer[i]);

                    if (tVisiblebuffer.Count >= tRows - 1)
                    {
                        break;
                    }
                }


                for (int i = 0; i < tVisiblebuffer.Count; i++)
                {
                    for (int tColumn = 0; tColumn < tColumns; tColumn++)
                    {
                        if (tColumn > tVisiblebuffer[i].Length - 1)
                        {
                            continue;
                        }
                        m_CharGrid[i][tColumn] = tVisiblebuffer[i].ToCharArray()[tColumn];

                    }
                }

                if (m_LastVisibleLine == 0)
                {
                    CaretOn();

                    for (int column = 0; column < this.m_Cols; column++)
                    {
                        if (column > this.m_TextAtCursor.Length - 1)
                            continue;
                        this.m_CharGrid[this.m_Rows - 1][column] = this.m_TextAtCursor.ToCharArray()[column];
                    }
                    this.CaretToAbs(this.m_TextAtCursor.Length, this.m_Rows - 1);
                }
                else
                {
                    this.CaretOff();
                }


                //System.Diagnostics.Debug.WriteLine("###### m_LastVisibleLine : " + m_LastVisibleLine);
                
                this.Refresh();
            }
        }


        /// <summary>
        /// 스크롤 값입니다.
        /// </summary>
        private int m_ScrollValue = 0;

        /// <summary>
        /// 스크롤중 새로운 데이터가 들어 왔을경우 마지막 출력을 보여준다.
        /// </summary>
        private void DisplayScrollLast(int aStartRow)
        {
            if (!m_VertScrollBar.Visible)
            {
                return;
            }
            try
            {
                m_VertScrollBar.MoveLine(aStartRow);
               //System.Diagnostics.Debug.WriteLine("###### m_LastVisibleLine : " + m_LastVisibleLine);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }


        /// <summary>
        /// 스크롤바 값을 조절 합니다.
        /// </summary>
        private void SetScrollBarValues()
        {
            try
            {
                if (m_ScrollbackBuffer.Count > m_Rows - 1)
                {
                    m_VertScrollBar.Visible = true;
                    m_VertScrollBar.Enabled = true;
                    m_VertScrollBar.Minimum = 0;
                    m_VertScrollBar.Maximum = m_ScrollbackBuffer.Count - m_Rows + 1;
                    m_VertScrollBar.OldValue = m_VertScrollBar.Maximum;
                    m_VertScrollBar.Value = m_VertScrollBar.Maximum;

                    m_LastVisibleLine = m_VertScrollBar.Value - m_VertScrollBar.Maximum;
                }
                // 초기화
                else
                {
                    m_VertScrollBar.Visible = false;
                    m_VertScrollBar.Enabled = false;
                    m_VertScrollBar.Minimum = 0;
                    //m_VertScrollBar.Maximum = 0;
                    m_VertScrollBar.Maximum = 1;    // 2019-01-18 Edit_LMW 수정 - 복사이벤트 동작시 m_VertScrollBar.Maximum값이 0이면 좌표값의 Row - 1로 작동되어 윗줄이 복사되는 문제가 있어 임시로 강제적으로 1로 할당.
                    m_VertScrollBar.Value = 0;
                    m_VertScrollBar.OldValue = 0;

                    m_LastVisibleLine = 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }


        /// 2015-09-23 자동저장 및 조회기능 Gunny
        /// <summary>
        /// 로그 파일 내용 추가.
        /// </summary>
        /// <param name="str"></param>
        private void FileWrite(string str)
        {

            string tFileName = m_DeviceInfo.TerminalName.ToString()+".clog";
			// 2019-11-10 ???? (?? ?? ?? ??)
            string tDirPath = AppGlobal.s_ClientOption.LogPath + @"AutoSaveLogs\" + GetDate() + "\\";
            string tFilePath = tDirPath + tFileName;

            DirectoryInfo di = new DirectoryInfo(tDirPath);
            if (di.Exists != true) Directory.CreateDirectory(tDirPath);

            FileStream fs = new FileStream(tFilePath, FileMode.Append, FileAccess.Write);

            //FileMode중 append는 이어쓰기. 파일이 없으면 만든다.

            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);

            sw.WriteLine(str);

            sw.Flush();

            sw.Close();

            fs.Close();

        }


        /// <summary>
        /// 받음 처리 합니다.
        /// </summary>
        /// <param name="aResult"></param>
        private void OnReceivedData(string aResult)
        {           
            try
            {
                // 영역선택 취소(초기화)
                Deselect();

                aResult = aResult.Replace("\t", "    ");


                if (this.InvokeRequired)
                {
                    this.Invoke(new HandlerArgument1<string>(OnReceivedData), aResult);
                    return;
                }

                //Console.WriteLine("aResult   : "+aResult);              
                //aResult =  aResult.Replace("\a", "");
                // 2015-05-30 - 신윤남 - 데이타 받아서 client화면에 보여주는 부분
                //Console.WriteLine("Display :" + aResult);

                //2015-11-12 hanjiyeon 분기문 추가 - show tech 결과 표시 속도 개선.
                if (aResult.Length > 100)
                {
                    this.SetVisibleCore(false);
                    
                    this.Invoke(this.OnRxdTextEvent, new string[] { string.Copy(aResult) });
                    this.Invoke(this.OnRefreshEvent);
                    
                    this.SetVisibleCore(true);
                    this.Focus();
                }
                else
                {
                    this.Invoke(this.OnRxdTextEvent, new string[] { string.Copy(aResult) });
                    this.Invoke(this.OnRefreshEvent);
                }

            }
            catch {}
            //catch (Exception ex)
            //{
            //    System.Diagnostics.Debug.WriteLine("[OnReceivedData] " + ex.ToString());
            //}

        }

        
        /// <summary>
        /// Gunny 현재 명령어 받음 
        /// </summary>
        public String GetCmd()
        {
            if (m_Prompt.Length == 0) return "";
            string tTempString = "";
            char tCurChar;           
            for (int x = 0; x < this.m_Cols; x++)
            {
                    tCurChar = this.m_CharGrid[this.m_Caret.Pos.Y][x];
                if (tCurChar == '\0')
                {
                    continue;
                }
                tTempString = tTempString + Convert.ToString(tCurChar);
            }
            
            tTempString = tTempString.TrimEnd();

            //Console.WriteLine("tTempString : " + tTempString);

            //Console.WriteLine("m_Prompt : " + m_Prompt);

            if (tTempString.IndexOf(m_Prompt) < 0) return "";
     
            tTempString = tTempString.Replace(m_Prompt, "");

            
            return tTempString.TrimStart();
        }

        //닷넷 하위 버전에 Join이 없어 임의로 만듬
        //해당 함수는 ' ' (공백) 를 문자 사이에 넣기 위한 기능으로만 사용.
        public String StrJoin(String separator, String[] values)
        {
            StringBuilder resultStr = new StringBuilder();

            foreach (String str in values)
            {
                resultStr.Append(str);
                resultStr.Append(separator);
            }

            return resultStr.ToString().Trim();
        }

        /// <summary>
        /// Gunny [제한명령어 인지 여부 리턴] 현재 명령어 받음 
        /// </summary>
        public bool IsLimitCmd(String lineCmd)
        {
            bool result = false;

            if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Console)
                return result;

            //2015-10-30 제한 명령어 - 사용자 권한 적용.
            
            if (!AppGlobal.s_LoginResult.UserInfo.LimitedCmdUser)
            return result;
            //lineCmd에 스페이스 공백이 여러개인경우
            // EX) clear  ip ospf process -> 제한 체크에 걸리지 않음.
            // 
            String[] SpliteStr = lineCmd.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            String ResultCmd = StrJoin(" ", SpliteStr);

             if (AppGlobal.s_LimitCmdInfoList.Contains(m_DeviceInfo.ModelID))
             {
                LimitCmdInfo tLimitCmdInfo = AppGlobal.s_LimitCmdInfoList[m_DeviceInfo.ModelID];

                ArrayList limitCmdList = tLimitCmdInfo.EmbagoCmd;
               /*
                 foreach (String cmd in limitCmdList)
                {
                    if (lineCmd.Contains(cmd))
                    {
                        result = true;
                        lineCmd = "";
                        AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "\"" + cmd + "\" 는 제한 명령어 입니다.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                 */
                foreach (EmbagoInfo embagoInfo in limitCmdList)
                {

                    if (ResultCmd.Contains(embagoInfo.Embargo))
                        {
                            if (embagoInfo.EmbargoEnble == true)
                            {
                                result = true;
                                ResultCmd = "";
                                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "\"" + embagoInfo.Embargo + "\" 금지 명령어를 포함 하고 있습니다. \n\r해당 사용자는 금지 명령어를 사용 할 수 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return result;
                            }
                            else
                            {
                                if (AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "\"" + embagoInfo.Embargo + "\" 제한 명령어를 포함 하고 입니다. \n\r사용 하시겠습니까?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                                {
                                    result = true;
                                    ResultCmd = "";
                                    return result;
                                }
                            }
                        }
                }
            }
            return result;
        }

        /// <summary>
        /// Gunny [제한명령어]를 확인 후 제한 명령어가 있으면 제한 명령어를 리턴.
        /// </summary>
        public string IsLimitCmdByBatch(String lineCmd)
        {
            bool result = false;

            String resultCmd = "";

            //2015-10-30 제한 명령어 - 사용자 권한 적용.
            if (!AppGlobal.s_LoginResult.UserInfo.LimitedCmdUser)
            return resultCmd;
          
            if (AppGlobal.s_LimitCmdInfoList.Contains(m_DeviceInfo.ModelID))
            {
                LimitCmdInfo tLimitCmdInfo = AppGlobal.s_LimitCmdInfoList[m_DeviceInfo.ModelID];

                ArrayList limitCmdList = tLimitCmdInfo.EmbagoCmd;
                /*
                foreach (String cmd in limitCmdList)
                {
                    if (lineCmd.Contains(cmd))
                    {
                        lineCmd = "";
                        resultCmd = cmd;
                        AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "\"" + cmd + "\" 는 제한 명령어 입니다.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return resultCmd;
                    }
                }
                */
                foreach (EmbagoInfo embagoInfo in limitCmdList)
                {
                    
                        if (lineCmd.Contains(embagoInfo.Embargo))
                        {
                            if (embagoInfo.EmbargoEnble == true)
                            {
                                lineCmd = "";
                                resultCmd = embagoInfo.Embargo;
                                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "\"" + embagoInfo.Embargo + "\" 는 제한 명령어 입니다.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return resultCmd;
                            }
                        }

                }
            }
            return resultCmd;
        }

        /// <summary>
        /// 컨트롤 메시지를 전송 합니다.
        /// </summary>
        /// <param name="aSender"></param>
        /// <param name="aKeyMap"></param>
        void DispatchControlMessage(object aSender, KeyInfo aKeyInfo)
        {

            if (this.m_XOff == true)
            {
                m_OutBuffer += aKeyInfo.Outstring;
                return;
            }

            try
            {
                System.Byte[] smk = new System.Byte[aKeyInfo.Outstring.Length];

                if (m_OutBuffer != "")
                {
                    aKeyInfo.Outstring = m_OutBuffer + aKeyInfo.Outstring;
                    m_OutBuffer = "";
                }



                if (m_DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET)
                {
                    if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online )//&& m_DeviceInfo.IsRegistered) 모든 장비를 데몬을 통한 통신으로 변경, 등록된 장비 여부 체크 제외
                    {
                        if (m_DaemonProcessRemoteObject == null) return;

                        RequestCommunicationData tRequestData = null;
                        TelnetCommandInfo tCommandInfo = new TelnetCommandInfo();
                        tCommandInfo.DeviceInfo = m_DeviceInfo;
                        tCommandInfo.UserID = AppGlobal.s_LoginResult.UserID;
                        tCommandInfo.WorkTyp = E_TelnetWorkType.Execute;
                        tCommandInfo.Command = aKeyInfo.Outstring;
                        tCommandInfo.SessionID = m_ConnectedSessionID;
                        tCommandInfo.KeyInfo = aKeyInfo;
                       if (aKeyInfo.ScanCode == 28 || aKeyInfo.ScanCode == 15)
                        {
                            if (!EnterPressFunction())
                                return;
                        }

                        tRequestData = AppGlobal.MakeDefaultRequestData();
                        tRequestData.CommType = E_CommunicationType.RequestCommandProcess;
                        tRequestData.RequestData = tCommandInfo;

                        m_Result = null;
                        m_MRE.Reset();

                        m_DaemonProcessRemoteObject.SendDaemonRequestData(this, tRequestData);
                        
                    }
                    else
                    {
                        RequestCommunicationData tRequestData = null;
                        TelnetCommandInfo tCommandInfo = new TelnetCommandInfo();
                        tCommandInfo.DeviceInfo = m_DeviceInfo;
                        // tCommandInfo.UserID = AppGlobal.s_LoginResult.UserID;
                        tCommandInfo.WorkTyp = E_TelnetWorkType.Execute;
                        tCommandInfo.Command = aKeyInfo.Outstring;
                        tCommandInfo.SessionID = m_ConnectedSessionID;
                        tCommandInfo.KeyInfo = aKeyInfo;

                        if (aKeyInfo.ScanCode == 28 || aKeyInfo.ScanCode == 15)
                        {
                            if (!EnterPressFunction())
                                return;
                        }

                        tCommandInfo.Sender = this;
                        tRequestData = new RequestCommunicationData();
                        tRequestData.CommType = E_CommunicationType.RequestCommandProcess;
                        tRequestData.RequestData = tCommandInfo;

                        AppGlobal.s_TelnetProcessor.ExecuteCommand(tRequestData);
                        m_IsOutPut = false;
                    }
                }
                else if (m_DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
                {
                    // 2013-03-06 - shinyn - SSH텔넷기능인 경우 분기처리 추가
                    if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online )//&& m_DeviceInfo.IsRegistered) 모든 장비를 데몬을 통한 통신으로 변경, 등록된 장비 여부 체크 제외
                    {

                        if (m_DaemonProcessRemoteObject == null) return;

                        RequestCommunicationData tRequestData = null;
                        TelnetCommandInfo tCommandInfo = new TelnetCommandInfo();
                        tCommandInfo.DeviceInfo = m_DeviceInfo;
                        tCommandInfo.UserID = AppGlobal.s_LoginResult.UserID;
                        tCommandInfo.WorkTyp = E_TelnetWorkType.Execute;
                        tCommandInfo.Command = aKeyInfo.Outstring;
                        tCommandInfo.SessionID = m_ConnectedSessionID;
                        tCommandInfo.KeyInfo = aKeyInfo;
                        if (aKeyInfo.ScanCode == 28 || aKeyInfo.ScanCode == 15)
                        {
                            if (!EnterPressFunction())
                                return;
                        }

                        tRequestData = AppGlobal.MakeDefaultRequestData();
                        tRequestData.CommType = E_CommunicationType.RequestCommandProcess;
                        tRequestData.RequestData = tCommandInfo;

                        m_Result = null;
                        m_MRE.Reset();

                        m_DaemonProcessRemoteObject.SendDaemonRequestData(this, tRequestData);

                    }
                    else
                    {
                        RequestCommunicationData tRequestData = null;
                        TelnetCommandInfo tCommandInfo = new TelnetCommandInfo();
                        tCommandInfo.DeviceInfo = m_DeviceInfo;
                        // tCommandInfo.UserID = AppGlobal.s_LoginResult.UserID;
                        tCommandInfo.WorkTyp = E_TelnetWorkType.Execute;
                        tCommandInfo.Command = aKeyInfo.Outstring;
                        tCommandInfo.SessionID = m_ConnectedSessionID;
                        tCommandInfo.KeyInfo = aKeyInfo;

                        if (aKeyInfo.ScanCode == 28 || aKeyInfo.ScanCode == 15)
                        {
                            if (!EnterPressFunction())
                                return;
                        }

                        tCommandInfo.Sender = this;
                        tRequestData = new RequestCommunicationData();
                        tRequestData.CommType = E_CommunicationType.RequestCommandProcess;
                        tRequestData.RequestData = tCommandInfo;

                        AppGlobal.s_TelnetProcessor.ExecuteCommand(tRequestData);
                    }
                }
                else
                {
                    if (aKeyInfo.ScanCode == 28 || aKeyInfo.ScanCode == 15)
                    {
                        if (!EnterPressFunction())
                            return;
                    }
                    AppGlobal.s_SerialProcessor.SendRequest(this, aKeyInfo.Outstring);
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        



        private bool EnterPressFunction()
        {
            String cmd = GetCmd();
            
            if (IsLimitCmd(cmd))
            {
                string backCnt = "";
                for (int i = 0; i < cmd.Length + 1; i++)
                {
                    backCnt += "\b";
                }
                this.DispatchMessage(this, backCnt);
                SaveCommandLog(true);

                return false;
            }
            else
            {
                SaveCommandLog(false);
                m_IsPressEnter = true;
                return true;

            }
        }


        /// <summary>
        /// [ - 제한명령어 - Gunny ]단축 명령어의 내용을 검수 합니다.
        /// </summary>
        /// <param name="aSender"></param>
        /// <param name="aText"></param>
        public void IsLimitCmdForShortenCommand(Object aSender, string aText)
        {
            if (IsLimitCmd(aText))
            {
                return;
            }
            else
            {
                DispatchMessage(aSender, aText);
            }

        }


        /// <summary>
        /// 메시지를 전송 합니다.
        /// </summary>
        /// <param name="aSender"></param>
        /// <param name="aText"></param>
        public void DispatchMessage(Object aSender, string aText)
       {

           if (aSender.ToString().Equals("RACTClient.Keyboard"))
            {                
                byte[] ascii = Encoding.ASCII.GetBytes(aText);
                bool isCtrlKey = false;
                foreach (byte b in ascii)
                {
                    if (string.Format("0x{0:x}", b).Equals("0x16"))
                        isCtrlKey = true;
                }
                if (isCtrlKey)
                {
                    return;
                }
            }

            if (this.m_XOff == true)
            {
                m_OutBuffer += aText;
                return;
            }

            try
            {
                //System.Byte[] smk = new System.Byte[aText.Length];

                if (m_OutBuffer != "")
                {
                    aText = m_OutBuffer + aText;
                    m_OutBuffer = "";
                }


                if (m_DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET)
                {
                    TelnetCommandInfo tCommandInfo = new TelnetCommandInfo();
                    tCommandInfo.DeviceInfo = m_DeviceInfo;
                    tCommandInfo.SessionID = m_ConnectedSessionID;

                    tCommandInfo.WorkTyp = E_TelnetWorkType.Execute;
                    tCommandInfo.Command = aText;
                    RequestCommunicationData tRequestData = null;

                    if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online )//&& m_DeviceInfo.IsRegistered) 모든 장비를 데몬을 통한 통신으로 변경, 등록된 장비 여부 체크 제외
                    {
                        tCommandInfo.UserID = AppGlobal.s_LoginResult.UserID;
                        tRequestData = AppGlobal.MakeDefaultRequestData();
                        tRequestData.CommType = E_CommunicationType.RequestCommandProcess;
                        tRequestData.RequestData = tCommandInfo;



                        if (m_DaemonProcessRemoteObject == null) return;
                        m_Result = null;
                        m_MRE.Reset();
                        m_DaemonProcessRemoteObject.SendDaemonRequestData(this, tRequestData);
                        if (m_IsPressEnter == true)
                        {
                            //System.Diagnostics.Debug.WriteLine("DispatchMessage !!!!!!!!!!!!!!!!!!!!! m_IsPressEnter !!!!!!!!!!!!!!!!!!! [=" + m_IsPressEnter.ToString());
                            Thread.Sleep(AppGlobal.s_ClientOption.SendDelay);
                        }
                    }
                    else
                    {
                        tCommandInfo.Sender = this;
                        tRequestData = new RequestCommunicationData();
                        tRequestData.CommType = E_CommunicationType.RequestCommandProcess;
                        tRequestData.RequestData = tCommandInfo;
                        AppGlobal.s_TelnetProcessor.ExecuteCommand(tRequestData);
                    }
                }
                else if (m_DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
                {
                    // 2013-03-06 - shinyn - SSH텔넷기능인 경우 분기처리 추가
                    TelnetCommandInfo tCommandInfo = new TelnetCommandInfo();
                    tCommandInfo.DeviceInfo = m_DeviceInfo;
                    tCommandInfo.SessionID = m_ConnectedSessionID;

                    tCommandInfo.WorkTyp = E_TelnetWorkType.Execute;
                    tCommandInfo.Command = aText;
                    RequestCommunicationData tRequestData = null;

                    if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online )//&& m_DeviceInfo.IsRegistered) 모든 장비를 데몬을 통한 통신으로 변경, 등록된 장비 여부 체크 제외
                    {
                        tCommandInfo.UserID = AppGlobal.s_LoginResult.UserID;
                        tRequestData = AppGlobal.MakeDefaultRequestData();
                        tRequestData.CommType = E_CommunicationType.RequestCommandProcess;
                        tRequestData.RequestData = tCommandInfo;



                        if (m_DaemonProcessRemoteObject == null) return;
                        m_Result = null;
                        m_MRE.Reset();
                        m_DaemonProcessRemoteObject.SendDaemonRequestData(this, tRequestData);
                    }
                    else
                    {
                        tCommandInfo.Sender = this;
                        tRequestData = new RequestCommunicationData();
                        tRequestData.CommType = E_CommunicationType.RequestCommandProcess;
                        tRequestData.RequestData = tCommandInfo;
                        AppGlobal.s_TelnetProcessor.ExecuteCommand(tRequestData);
                    }
                }
                else
                {
                    AppGlobal.s_SerialProcessor.SendRequest(this, aText);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }


        private void PrintChar(Char aCurChar)
        {
            if (m_Caret.EOL == true)
            {
                if ((m_Modes.Flags & Mode.s_AutoWrap) == Mode.s_AutoWrap)
                {
                    LineFeed();
                    CarriageReturn();
                    m_Caret.EOL = false;
                }
            }

            Int32 X = m_Caret.Pos.X;
            Int32 Y = m_Caret.Pos.Y;

            m_AttribGrid[Y][X] = m_CharAttribs;

            if (m_CharAttribs.GS != null)
            {
                aCurChar = Chars.Get(aCurChar, m_AttribGrid[Y][X].GS.Set, m_AttribGrid[Y][X].GR.Set);

                if (m_CharAttribs.GS.Set == Chars.Sets.DECSG) m_AttribGrid[Y][X].IsDECSG = true;

                m_CharAttribs.GS = null;
            }
            else
            {
                aCurChar = Chars.Get(aCurChar, m_AttribGrid[Y][X].GL.Set, m_AttribGrid[Y][X].GR.Set);

                if (m_CharAttribs.GL.Set == Chars.Sets.DECSG) m_AttribGrid[Y][X].IsDECSG = true;
            }

            m_CharGrid[Y][X] = aCurChar;

            CaretRight();
        }
        /// <summary>
        /// 시작 위치를 가져 오기 합니다.
        /// </summary>
        /// <param name="aCurGraphics"></param>
        /// <param name="aX"></param>
        /// <param name="aY"></param>
        /// <param name="aCurChar"></param>
        /// <returns></returns>
        private System.Drawing.Point GetDrawstringOffset(Graphics aCurGraphics, Int32 aX, Int32 aY, Char aCurChar)
        {
            CharacterRange[] tCharacterRanges ={ new System.Drawing.CharacterRange(0, 1) };
            RectangleF tLayoutRect = new RectangleF(aX, aY, 100, 100);
            StringFormat tStringFormat = new StringFormat();

            tStringFormat.SetMeasurableCharacterRanges(tCharacterRanges);

            Region[] tStringRegions = new Region[1];

            tStringRegions = aCurGraphics.MeasureCharacterRanges(aCurChar.ToString(), this.Font, tLayoutRect, tStringFormat);

            RectangleF tMeasureRect = tStringRegions[0].GetBounds(aCurGraphics);

            return new Point((int)(tMeasureRect.X + 0.5), (int)(tMeasureRect.Y + 0.5));
        }

        /// <summary>
        /// Char Size를 구합니다.
        /// </summary>
        /// <param name="aCurGraphics"></param>
        /// <returns></returns>
        private Point GetCharSize(Graphics aCurGraphics)
        {
            CharacterRange[] tCharacterRanges ={ new CharacterRange(0, 1) };
            RectangleF tLayoutRect = new RectangleF(0, 0, 100, 100);
            StringFormat tStringFormat = new StringFormat();
            Region[] tRtringRegions = new Region[1];

            tStringFormat.SetMeasurableCharacterRanges(tCharacterRanges);
            tRtringRegions = aCurGraphics.MeasureCharacterRanges("A", this.Font, tLayoutRect, tStringFormat);
            RectangleF tMeasureRect = tRtringRegions[0].GetBounds(aCurGraphics);

            return new Point((int)(tMeasureRect.Width + 0.5), (int)(tMeasureRect.Height + 0.5));
        }
        /// <summary>
        /// 색상을 적용 합니다.
        /// </summary>
        private void AssignColors(CharAttribStruct aCurAttribs, ref Color aCurFGColor, ref Color aCurBGColor)
        {

            aCurFGColor = this.m_FGColor;
            aCurBGColor = this.BackColor;

            if (aCurAttribs.IsBlinking)
            {
                aCurFGColor = this.m_BlinkColor;
            }

            if (aCurAttribs.IsBold)
            {
                aCurFGColor = this.m_BoldColor;
            }

            if (aCurAttribs.UseAltColor)
            {
                aCurFGColor = aCurAttribs.AltColor;
            }

            if (aCurAttribs.UseAltBGColor)
            {
                aCurBGColor = aCurAttribs.AltBGColor;
            }

            if (aCurAttribs.IsInverse)
            {
                Color tTmpColor = aCurBGColor;

                aCurBGColor = aCurFGColor;
                aCurFGColor = tTmpColor;
            }

            if ((this.m_Modes.Flags & Mode.s_LightBackground) > 0 && aCurAttribs.UseAltColor == false && aCurAttribs.UseAltBGColor == false)
            {
                Color TmpColor = aCurBGColor;

                aCurBGColor = aCurFGColor;
                aCurFGColor = TmpColor;
            }
        }

        /// <summary>
        /// 화면에 문자를 표시 합니다.
        /// </summary>
        /// <param name="aCurGraphics"></param>
        /// <param name="aCurChar"></param>
        /// <param name="aY"></param>
        /// <param name="aX"></param>
        /// <param name="aCurAttribs"></param>
        private void ShowChar(Graphics aCurGraphics, Char aCurChar, Int32 aX, Int32 aY, CharAttribStruct aCurAttribs)
        {
            if (aCurChar == '\0')
            {
                return;
            }

            Color tCurFGColor = Color.White;
            Color tCurBGColor = Color.Black;

            AssignColors(aCurAttribs, ref tCurFGColor, ref tCurBGColor);

            if ((tCurBGColor != this.BackColor && (this.m_Modes.Flags & Mode.s_LightBackground) == 0) ||
                (tCurBGColor != this.m_FGColor && (this.m_Modes.Flags & Mode.s_LightBackground) > 0))
            {
                m_EraseBuffer.Clear(tCurBGColor);
                aCurGraphics.DrawImageUnscaled(m_EraseBitmap, aX, aY);
            }

            if (aCurAttribs.IsUnderscored)
            {
                aCurGraphics.DrawLine(new Pen(tCurFGColor, 1), aX, aY + m_UnderlinePos, aX + m_CharSize.Width, aY + m_UnderlinePos);
            }

            if ((aCurAttribs.IsDECSG == true) &&
                (aCurChar == 'l' ||
                aCurChar == 'q' ||
                aCurChar == 'w' ||
                aCurChar == 'k' ||
                aCurChar == 'x' ||
                aCurChar == 't' ||
                aCurChar == 'n' ||
                aCurChar == 'u' ||
                aCurChar == 'm' ||
                aCurChar == 'v' ||
                aCurChar == 'j' ||
                aCurChar == '`'))
            {
                this.ShowSpecialChar(aCurGraphics, aCurChar, aY, aX, tCurFGColor, tCurBGColor);
                return;
            }
            
            aCurGraphics.DrawString(aCurChar.ToString(), this.Font, new SolidBrush(tCurFGColor), aX - m_DrawstringOffset.X, aY - m_DrawstringOffset.Y);
        }

        private void ShowSpecialChar(Graphics aCurGraphics, Char aCurChar, Int32 aY, Int32 aX, Color aCurFGColor, Color aCurBGColor)
        {
            if (aCurChar == '\0')
            {
                return;
            }

            switch (aCurChar)
            {
                case '`': // diamond
                    Point[] tCurPoints = new Point[4];

                    tCurPoints[0] = new Point(aX + m_CharSize.Width / 2, aY + m_CharSize.Height / 6);
                    tCurPoints[1] = new Point(aX + 5 * m_CharSize.Width / 6, aY + m_CharSize.Height / 2);
                    tCurPoints[2] = new Point(aX + m_CharSize.Width / 2, aY + 5 * m_CharSize.Height / 6);
                    tCurPoints[3] = new Point(aX + m_CharSize.Width / 6, aY + m_CharSize.Height / 2);

                    aCurGraphics.FillPolygon(new SolidBrush(aCurFGColor), tCurPoints);
                    break;

                case 'l': // top left bracket
                    aCurGraphics.DrawLine(new Pen(aCurFGColor, 1), aX + m_CharSize.Width / 2 - 1, aY + m_CharSize.Height / 2, aX + m_CharSize.Width, aY + m_CharSize.Height / 2);
                    aCurGraphics.DrawLine(new Pen(aCurFGColor, 1), aX + m_CharSize.Width / 2, aY + m_CharSize.Height / 2, aX + m_CharSize.Width / 2, aY + m_CharSize.Height);
                    break;

                case 'q': // horizontal line
                    aCurGraphics.DrawLine(new Pen(aCurFGColor, 1), aX, aY + m_CharSize.Height / 2, aX + m_CharSize.Width, aY + m_CharSize.Height / 2);
                    break;

                case 'w': // top tee-piece
                    aCurGraphics.DrawLine(new Pen(aCurFGColor, 1), aX, aY + m_CharSize.Height / 2, aX + m_CharSize.Width, aY + m_CharSize.Height / 2);
                    aCurGraphics.DrawLine(new Pen(aCurFGColor, 1), aX + m_CharSize.Width / 2, aY + m_CharSize.Height / 2, aX + m_CharSize.Width / 2, aY + m_CharSize.Height);
                    break;

                case 'k': // top right bracket
                    aCurGraphics.DrawLine(new Pen(aCurFGColor, 1), aX, aY + m_CharSize.Height / 2, aX + m_CharSize.Width / 2, aY + m_CharSize.Height / 2);
                    aCurGraphics.DrawLine(new Pen(aCurFGColor, 1), aX + m_CharSize.Width / 2, aY + m_CharSize.Height / 2, aX + m_CharSize.Width / 2, aY + m_CharSize.Height);
                    break;

                case 'x': // vertical line
                    aCurGraphics.DrawLine(new Pen(aCurFGColor, 1), aX + m_CharSize.Width / 2, aY, aX + m_CharSize.Width / 2, aY + m_CharSize.Height);
                    break;

                case 't': // left hand tee-piece
                    aCurGraphics.DrawLine(new Pen(aCurFGColor, 1), aX + m_CharSize.Width / 2, aY, aX + m_CharSize.Width / 2, aY + m_CharSize.Height);
                    aCurGraphics.DrawLine(new Pen(aCurFGColor, 1), aX + m_CharSize.Width / 2, aY + m_CharSize.Height / 2, aX + m_CharSize.Width, aY + m_CharSize.Height / 2);
                    break;

                case 'n': // cross piece
                    aCurGraphics.DrawLine(new Pen(aCurFGColor, 1), aX + m_CharSize.Width / 2, aY, aX + m_CharSize.Width / 2, aY + m_CharSize.Height);
                    aCurGraphics.DrawLine(new Pen(aCurFGColor, 1), aX, aY + m_CharSize.Height / 2, aX + m_CharSize.Width, aY + m_CharSize.Height / 2);
                    break;

                case 'u': // right hand tee-piece
                    aCurGraphics.DrawLine(new Pen(aCurFGColor, 1), aX, aY + m_CharSize.Height / 2, aX + m_CharSize.Width / 2, aY + m_CharSize.Height / 2);
                    aCurGraphics.DrawLine(new Pen(aCurFGColor, 1), aX + m_CharSize.Width / 2, aY, aX + m_CharSize.Width / 2, aY + m_CharSize.Height);
                    break;

                case 'm': // bottom left bracket
                    aCurGraphics.DrawLine(new Pen(aCurFGColor, 1), aX + m_CharSize.Width / 2, aY + m_CharSize.Height / 2, aX + m_CharSize.Width, aY + m_CharSize.Height / 2);
                    aCurGraphics.DrawLine(new Pen(aCurFGColor, 1), aX + m_CharSize.Width / 2, aY, aX + m_CharSize.Width / 2, aY + m_CharSize.Height / 2);
                    break;

                case 'v': // bottom tee-piece
                    aCurGraphics.DrawLine(new Pen(aCurFGColor, 1), aX, aY + m_CharSize.Height / 2, aX + m_CharSize.Width, aY + m_CharSize.Height / 2);
                    aCurGraphics.DrawLine(new Pen(aCurFGColor, 1), aX + m_CharSize.Width / 2, aY, aX + m_CharSize.Width / 2, aY + m_CharSize.Height / 2);
                    break;

                case 'j': // bottom right bracket
                    aCurGraphics.DrawLine(new Pen(aCurFGColor, 1), aX, aY + m_CharSize.Height / 2, aX + m_CharSize.Width / 2, aY + m_CharSize.Height / 2);
                    aCurGraphics.DrawLine(new Pen(aCurFGColor, 1), aX + m_CharSize.Width / 2, aY, aX + m_CharSize.Width / 2, aY + m_CharSize.Height / 2);
                    break;
                default:
                    break;
            }

        }

        private void WipeScreen(Graphics aCurGraphics)
        {
            if ((this.m_Modes.Flags & Mode.s_LightBackground) > 0)
            {
				// 2019-11-10 ???? (?? ?? ?? ?? )
                if (m_DeviceInfo.DevicePartCode == 1 || /* 집선스위치 */
                    m_DeviceInfo.DevicePartCode == 6 || /* G-PON-OLT */
                    m_DeviceInfo.DevicePartCode == 31 /* NG-PON-OLT */ )
                {
                    aCurGraphics.Clear(AppGlobal.s_ClientOption.HighlightFontColor);
                }
                else
                {
                    aCurGraphics.Clear(AppGlobal.s_ClientOption.TerminalFontColor);
                }
            }
            else
            {
                if (m_DeviceInfo.DevicePartCode == 1 || /* 집선스위치 */
                    m_DeviceInfo.DevicePartCode == 6 || /* G-PON-OLT */
                    m_DeviceInfo.DevicePartCode == 31 /* NG-PON-OLT */ )
                {
                    aCurGraphics.Clear(AppGlobal.s_ClientOption.HighlightBackGroundColor);
                }
                else
                {
                    aCurGraphics.Clear(AppGlobal.s_ClientOption.TerminalBackGroundColor);
                }

            }
        }
        /// <summary>
        /// 아래쪽을 삭제 합니다.
        /// </summary>
        /// <param name="aParam"></param>
        private void ClearDown(Int32 aParam)
        {
            Int32 tX = this.m_Caret.Pos.X;
            Int32 tY = this.m_Caret.Pos.Y;

            switch (aParam)
            {
                case 0: // from cursor to bottom inclusive
                    Array.Clear(this.m_CharGrid[tY], tX, this.m_CharGrid[tY].Length - tX);
                    Array.Clear(this.m_AttribGrid[tY], tX, this.m_AttribGrid[tY].Length - tX);

                    for (int i = tY + 1; i < this.m_Rows; i++)
                    {
                        Array.Clear(this.m_CharGrid[i], 0, this.m_CharGrid[i].Length);
                        Array.Clear(this.m_AttribGrid[i], 0, this.m_AttribGrid[i].Length);
                    }
                    break;

                case 1: // from top to cursor inclusive
                    Array.Clear(this.m_CharGrid[tY], 0, tX + 1);
                    Array.Clear(this.m_AttribGrid[tY], 0, tX + 1);

                    for (int i = 0; i < tY; i++)
                    {
                        Array.Clear(this.m_CharGrid[i], 0, this.m_CharGrid[i].Length);
                        Array.Clear(this.m_AttribGrid[i], 0, this.m_AttribGrid[i].Length);
                    }
                    break;

                case 2: // entire screen
                    for (int i = 0; i < this.m_Rows; i++)
                    {
                        Array.Clear(this.m_CharGrid[i], 0, this.m_CharGrid[i].Length);
                        Array.Clear(this.m_AttribGrid[i], 0, this.m_AttribGrid[i].Length);
                    }
                    break;

                default:
                    break;
            }
        }
        /// <summary>
        /// 오른쪽을 삭제 합니다.
        /// </summary>
        /// <param name="aParam"></param>
        private void ClearRight(Int32 aParam)
        {
            Int32 tX = this.m_Caret.Pos.X;
            Int32 tY = this.m_Caret.Pos.Y;

            switch (aParam)
            {
                case 0: // from cursor to end of line inclusive
                    Array.Clear(this.m_CharGrid[tY], tX, this.m_CharGrid[tY].Length - tX);
                    Array.Clear(this.m_AttribGrid[tY], tX, this.m_AttribGrid[tY].Length - tX);
                    break;

                case 1: // from beginning to cursor inclusive
                    Array.Clear(this.m_CharGrid[tY], 0, tX + 1);
                    Array.Clear(this.m_AttribGrid[tY], 0, tX + 1);
                    break;

                case 2: // entire line
                    //m_IsSaveWaitScript = false;
                    m_Prompt = "";
                    SaveWaitScript();
                    //SaveCommandLog();
                    Array.Clear(this.m_CharGrid[tY], 0, this.m_CharGrid[tY].Length);
                    Array.Clear(this.m_AttribGrid[tY], 0, this.m_AttribGrid[tY].Length);

                    break;

                default:
                    break;
            }
        }

        private void ShowBuffer()
        {
            this.Invalidate();
        }

        private void Redraw(System.Drawing.Graphics tCurGraphics)
        {
            Point tCurPoint;
            Char tCurChar;

            // refresh the screen
            for (Int32 Y = 0; Y < m_Rows; Y++)
            {
                for (Int32 X = 0; X < m_Cols; X++)
                {
                    tCurChar = m_CharGrid[Y][X];
                    
                    if (tCurChar == '\0')
                    {
                        continue;
                    }

                    tCurPoint = new Point(X * m_CharSize.Width, Y * m_CharSize.Height);
                    this.ShowChar(tCurGraphics, tCurChar, tCurPoint.X, tCurPoint.Y, this.m_AttribGrid[Y][X]);
                }
            }
        }

        private void TelnetInterpreter(object Sender, NegotiateParserEventArgs e)
        {
            switch (e.Action)
            {
                case E_NegotiateActions.SendUp:
                    this.m_Parser.Parsestring(e.CurChar.ToString());
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 행 바꿈 처리 합니다.
        /// </summary>
        private void CarriageReturn()
        {
            m_IsSaveWaitScript = false;
            if(!tReturn)
                m_IsCheckPrompt = false;
            CaretToAbs(0, this.m_Caret.Pos.Y);
            SetScrollBarValues();

        }

        /// <summary>
        /// 탭을 띄웁니다.
        /// </summary>
        private void Tab()
        {
            for (Int32 i = 0; i < m_TabStops.Columns.Length; i++)
            {
                if (i > m_Caret.Pos.X && m_TabStops.Columns[i] == true)
                {
                    CaretToAbs(i, this.m_Caret.Pos.Y);
                    return;
                }
            }

            this.CaretToAbs(this.m_Cols - 1, this.m_Caret.Pos.Y);
            return;
        }

        private void TabSet()
        {
            m_TabStops.Columns[this.m_Caret.Pos.X] = true;
        }

        private void ClearTabs(Params aCurParams) // TBC 
        {
            Int32 Param = 0;

            if (aCurParams.Count() > 0)
            {
                Param = System.Convert.ToInt32(aCurParams.Elements[0]);
            }

            switch (Param)
            {
                case 0: // Current Position
                    this.m_TabStops.Columns[this.m_Caret.Pos.X] = false;
                    break;

                case 3: // All Tabs
                    for (int i = 0; i < this.m_TabStops.Columns.Length; i++)
                    {
                        this.m_TabStops.Columns[i] = false;
                    }
                    break;

                default:
                    break;
            }
        }

        private void ReverseLineFeed()
        {
            if (this.m_Caret.Pos.Y == this.m_TopMargin)
            {
                int i;

                for (i = this.m_BottomMargin; i > this.m_TopMargin; i--)
                {
                    this.m_CharGrid[i] = this.m_CharGrid[i - 1];
                    this.m_AttribGrid[i] = this.m_AttribGrid[i - 1];
                }

                this.m_CharGrid[this.m_TopMargin] = new System.Char[this.m_Cols];

                this.m_AttribGrid[this.m_TopMargin] = new CharAttribStruct[this.m_Cols];
            }

            this.CaretUp();
        }
        /// <summary>
        /// 라인을 추가 합니다.
        /// </summary>
        /// <param name="aCurParams"></param>
        private void InsertLine(Params aCurParams)
        {

            if (this.m_Caret.Pos.Y < this.m_TopMargin ||
                this.m_Caret.Pos.Y > this.m_BottomMargin)
            {
                return;
            }

            Int32 tNbrOff = 1;

            if (aCurParams.Count() > 0)
            {
                tNbrOff = Convert.ToInt32(aCurParams.Elements[0]);
            }

            while (tNbrOff > 0)
            {
                for (int i = m_BottomMargin; i > m_Caret.Pos.Y; i--)
                {
                    m_CharGrid[i] = m_CharGrid[i - 1];
                    m_AttribGrid[i] = m_AttribGrid[i - 1];
                }

                m_CharGrid[m_Caret.Pos.Y] = new Char[m_Cols];
                m_AttribGrid[m_Caret.Pos.Y] = new CharAttribStruct[m_Cols];

                tNbrOff--;
            }
        }
        /// <summary>
        /// 라인을 삭제 합니다.
        /// </summary>
        /// <param name="aCurParams"></param>
        private void DeleteLine(Params aCurParams)
        {
            if (this.m_Caret.Pos.Y < this.m_TopMargin ||
                this.m_Caret.Pos.Y > this.m_BottomMargin)
            {
                return;
            }

            Int32 tNbrOff = 1;

            if (aCurParams.Count() > 0)
            {
                tNbrOff = Convert.ToInt32(aCurParams.Elements[0]);
            }

            while (tNbrOff > 0)
            {
                for (int i = this.m_Caret.Pos.Y; i < this.m_BottomMargin; i++)
                {
                    this.m_CharGrid[i] = this.m_CharGrid[i + 1];
                    this.m_AttribGrid[i] = this.m_AttribGrid[i + 1];
                }

                this.m_CharGrid[this.m_BottomMargin] = new System.Char[this.m_Cols];
                this.m_AttribGrid[this.m_BottomMargin] = new CharAttribStruct[this.m_Cols];

                tNbrOff--;
            }
        }
        /// <summary>
        /// 줄바꿈 처리 합니다.
        /// </summary>
        private void LineFeed()
        {      
            if (this.m_ScrollbackBuffer.Count > this.m_ScrollbackBufferSize)
            {
                this.m_ScrollbackBuffer.RemoveAt(0);
            }

            string tTempString = "";
            char tCurChar;
            for (int x = 0; x < this.m_Cols; x++)
            {
                tCurChar = this.m_CharGrid[this.m_Caret.Pos.Y][x];
                if (tCurChar == '\0')
                {
                    continue;
                }
                tTempString = tTempString + Convert.ToString(tCurChar);
            }
            if (m_IsShowLineNumber)
            {
                this.m_ScrollbackBuffer.Add(m_ScrollbackBuffer.Count + 1 + " " + tTempString);
            }
            else
            {
                this.m_ScrollbackBuffer.Add(tTempString);
            }
            if (m_IsPressEnter)
            {
				// 2019-11-10 ???? (?? ???? ??? ???? ?? ?? ?? )
                if (AppGlobal.s_ClientOption.IsAutoSaveLog)
                {
                    string lineCommand = GetCmd();
                    if (lineCommand.Length > 0)
                    {
                        FileWrite("|&|" + GetCmd() + "||");
                    }
                }

                m_IsPressEnter = false;
            }
            if (this.m_Caret.Pos.Y == this.m_BottomMargin || this.m_Caret.Pos.Y == this.m_Rows - 1)
            {

                int i;

                for (i = this.m_TopMargin; i < this.m_BottomMargin; i++)
                {
                    this.m_CharGrid[i] = this.m_CharGrid[i + 1];
                    this.m_AttribGrid[i] = this.m_AttribGrid[i + 1];
                }

                this.m_CharGrid[i] = new System.Char[this.m_Cols];
                this.m_AttribGrid[i] = new CharAttribStruct[this.m_Cols];

            }

            SetScrollBarValues();
            this.CaretDown();
            this.Refresh();

        }

        private void Index(Int32 Param)
        {
            if (Param == 0) Param = 1;

            for (int i = 0; i < Param; i++)
            {
                this.LineFeed();
            }
        }

        private void ReverseIndex(Int32 Param)
        {
            if (Param == 0) Param = 1;

            for (int i = 0; i < Param; i++)
            {
                this.ReverseLineFeed();
            }
        }
        /// <summary>
        /// 커서 끄기 처리 입니다.
        /// </summary>
        private void CaretOff()
        {
            if (this.m_Caret.IsOff == true)
            {
                return;
            }

            this.m_Caret.IsOff = true;
        }
        /// <summary>
        /// 커서 표시 처리 입니다.
        /// </summary>
        private void CaretOn()
        {
            if (!m_Caret.IsOff)
            {
                return;
            }
            m_Caret.IsOff = false;
        }
        /// <summary>
        /// Caret을 표시 합니다.
        /// </summary>
        /// <param name="CurGraphics"></param>
        private void ShowCaret(Graphics aCurGraphics)
        {
            Int32 tX = this.m_Caret.Pos.X;
            Int32 tY = this.m_Caret.Pos.Y;

            if (m_Caret.IsOff == true)
            {
                return;
            }

            aCurGraphics.DrawImageUnscaled(this.m_Caret.Bitmap, tX * (int)this.m_CharSize.Width, tY * (int)this.m_CharSize.Height);

            if (this.m_CharGrid[tY][tX] == '\0')
            {
                return;
            }

            CharAttribStruct tCurAttribs = new CharAttribStruct();

            tCurAttribs.UseAltColor = true;

            tCurAttribs.GL = this.m_AttribGrid[tY][tX].GL;
            tCurAttribs.GR = this.m_AttribGrid[tY][tX].GR;
            tCurAttribs.GS = this.m_AttribGrid[tY][tX].GS;

            if (this.m_AttribGrid[tY][tX].UseAltBGColor == false)
            {
                tCurAttribs.AltColor = this.BackColor;
            }
            else if (this.m_AttribGrid[tY][tX].UseAltBGColor == true)
            {
                tCurAttribs.AltColor = this.m_AttribGrid[tY][tX].AltBGColor;
            }

            tCurAttribs.IsUnderscored = this.m_AttribGrid[tY][tX].IsUnderscored;
            tCurAttribs.IsDECSG = this.m_AttribGrid[tY][tX].IsDECSG;

            this.ShowChar(aCurGraphics, this.m_CharGrid[tY][tX], m_Caret.Pos.X * this.m_CharSize.Width, m_Caret.Pos.Y * this.m_CharSize.Height, tCurAttribs);
        }

        /// <summary>
        /// Caret 위치를 위로 이동합니다.
        /// </summary>
        private void CaretUp()
        {
            this.m_Caret.EOL = false;

            if ((this.m_Caret.Pos.Y > 0 && (this.m_Modes.Flags & Mode.s_OriginRelative) == 0) ||
                (this.m_Caret.Pos.Y > this.m_TopMargin && (this.m_Modes.Flags & Mode.s_OriginRelative) > 0))
            {
                this.m_Caret.Pos.Y -= 1;
            }
        }

        /// <summary>
        /// Caret 위치를 아래로 이동합니다.
        /// </summary>
        private void CaretDown()
        {
            this.m_Caret.EOL = false;

            if ((this.m_Caret.Pos.Y < this.m_Rows - 1 && (this.m_Modes.Flags & Mode.s_OriginRelative) == 0) ||
                (this.m_Caret.Pos.Y < this.m_BottomMargin && (this.m_Modes.Flags & Mode.s_OriginRelative) > 0))
            {
                this.m_Caret.Pos.Y += 1;
            }


        }
        /// <summary>
        /// Caret 위치를 왼쪽으로 이동합니다.
        /// </summary>
        private void CaretLeft()
        {
            this.m_Caret.EOL = false;

            if (this.m_Caret.Pos.X > 0)
            {
                this.m_Caret.Pos.X -= 1;
            }
            else
            {
                //2023-02-23 AutoWrap이후 백스페이스 수행시 윗 라인으로 이동이 안되어 코드 추가 
                this.m_Caret.Pos.X = this.m_Cols - 1;
                this.m_Caret.Pos.Y -= 1;
            }
        }
        /// <summary>
        /// Caret 위치를 오른쪽으로 이동합니다.
        /// </summary>
        private void CaretRight()
        {
            if (this.m_Caret.Pos.X < this.m_Cols - 1)
            {
                this.m_Caret.Pos.X += 1;
                this.m_Caret.EOL = false;
            }
            else
            {
                this.m_Caret.EOL = true;
            }
        }

        private void CaretToRel(System.Int32 aY, System.Int32 aX)
        {

            this.m_Caret.EOL = false;


            if ((this.m_Modes.Flags & Mode.s_OriginRelative) == 0)
            {
                this.CaretToAbs(aX, aY);
                return;
            }

            aY += this.m_TopMargin;

            if (aX < 0)
            {
                aX = 0;
            }

            if (aX > this.m_Cols - 1)
            {
                aX = this.m_Cols - 1;
            }

            if (aY < this.m_TopMargin)
            {
                aY = this.m_TopMargin;
            }

            if (aY > this.m_BottomMargin)
            {
                aY = this.m_BottomMargin;
            }

            this.m_Caret.Pos.Y = aY;
            this.m_Caret.Pos.X = aX;
        }

        /// <summary>
        /// Caret 위치를 옮깁니다.
        /// </summary>
        /// <param name="aY"></param>
        /// <param name="aX"></param>
        private void CaretToAbs(Int32 aX, Int32 aY)
        {
            this.m_Caret.EOL = false;

            if (aX < 0)
            {
                aX = 0;
            }

            if (aX > this.m_Cols - 1)
            {
                aX = this.m_Cols - 1;
            }

            if (aY < 0 && (this.m_Modes.Flags & Mode.s_OriginRelative) == 0)
            {
                aY = 0;
            }

            if (aY < this.m_TopMargin && (this.m_Modes.Flags & Mode.s_OriginRelative) > 0)
            {
                aY = this.m_TopMargin;
            }

            if (aY > this.m_Rows - 1 && (this.m_Modes.Flags & Mode.s_OriginRelative) == 0)
            {
                aY = this.m_Rows - 1;
            }

            if (aY > this.m_BottomMargin && (this.m_Modes.Flags & Mode.s_OriginRelative) > 0)
            {
                aY = this.m_BottomMargin;
            }

            this.m_Caret.Pos.Y = aY;
            this.m_Caret.Pos.X = aX;
        }

        private void CommandRouter(object Sender, ParserEventArgs e)
        {

            switch (e.Action)
            {
                case E_Actions.Print:
                    this.PrintChar(e.CurChar);
                    break;

                case E_Actions.Execute:
  
                    this.ExecuteChar(e.CurChar);
                    break;

                case E_Actions.Dispatch:
                    break;

                default:
                    break;
            }

            Int32 Param = 0;

            Int32 tIncrement = 1; // increment

            switch (e.CurSequence)
            {
                case "":
                    break;

                case "\x1b" + "7": //DECSC Save Cursor position and attributes
                    this.m_SavedCarets.Add(new CaretAttribs(this.m_Caret.Pos, this.m_G0.Set, this.m_G1.Set, this.m_G2.Set, this.m_G3.Set, this.m_CharAttribs));
                    break;

                case "\x1b" + "8": //DECRC Restore Cursor position and attributes
                    this.m_Caret.Pos = ((CaretAttribs)this.m_SavedCarets[this.m_SavedCarets.Count - 1]).Pos;
                    this.m_CharAttribs = ((CaretAttribs)this.m_SavedCarets[this.m_SavedCarets.Count - 1]).Attribs;

                    this.m_G0.Set = ((CaretAttribs)this.m_SavedCarets[this.m_SavedCarets.Count - 1]).G0Set;
                    this.m_G1.Set = ((CaretAttribs)this.m_SavedCarets[this.m_SavedCarets.Count - 1]).G1Set;
                    this.m_G2.Set = ((CaretAttribs)this.m_SavedCarets[this.m_SavedCarets.Count - 1]).G2Set;
                    this.m_G3.Set = ((CaretAttribs)this.m_SavedCarets[this.m_SavedCarets.Count - 1]).G3Set;

                    this.m_SavedCarets.RemoveAt(this.m_SavedCarets.Count - 1);

                    break;

                case "\x1b~": //LS1R Locking Shift G1 -> GR
                    this.m_CharAttribs.GR = m_G1;
                    break;

                case "\x1bn": //LS2 Locking Shift G2 -> GL
                    this.m_CharAttribs.GL = m_G2;
                    break;

                case "\x1b}": //LS2R Locking Shift G2 -> GR
                    this.m_CharAttribs.GR = m_G2;
                    break;

                case "\x1bo": //LS3 Locking Shift G3 -> GL
                    this.m_CharAttribs.GL = m_G3;
                    break;

                case "\x1b|": //LS3R Locking Shift G3 -> GR
                    this.m_CharAttribs.GR = m_G3;
                    break;

                case "\x1b#8": //DECALN
                    e.CurParams.Elements.Add("1");
                    e.CurParams.Elements.Add(this.m_Rows.ToString());
                    this.SetScrollRegion(e.CurParams);

                    for (int tRow = 0; tRow < this.m_Rows; tRow++)
                    {
                        this.CaretToAbs(0, tRow);

                        for (int tCol = 0; tCol < this.m_Cols; tCol++)
                        {
                            this.PrintChar('E');
                        }
                    }
                    break;

                case "\x1b=": // Keypad to Application mode
                    this.m_Modes.Flags = this.m_Modes.Flags | Mode.s_KeypadAppln;
                    break;

                case "\x1b>": // Keypad to Numeric mode
                    this.m_Modes.Flags = this.m_Modes.Flags ^ Mode.s_KeypadAppln;
                    break;

                case "\x1b[B": // CUD

                    if (e.CurParams.Count() > 0)
                    {
                        tIncrement = Convert.ToInt32(e.CurParams.Elements[0]);
                    }

                    if (tIncrement == 0) tIncrement = 1;

                    this.CaretToAbs(this.m_Caret.Pos.X, this.m_Caret.Pos.Y + tIncrement);
                    break;

                case "\x1b[A": // CUU

                    if (e.CurParams.Count() > 0)
                    {
                        tIncrement = Convert.ToInt32(e.CurParams.Elements[0]);
                    }

                    if (tIncrement == 0) tIncrement = 1;

                    this.CaretToAbs(this.m_Caret.Pos.X, this.m_Caret.Pos.Y - tIncrement);
                    break;

                case "\x1b[C": // CUF

                    if (e.CurParams.Count() > 0)
                    {
                        tIncrement = Convert.ToInt32(e.CurParams.Elements[0]);
                    }

                    if (tIncrement == 0) tIncrement = 1;

                    this.CaretToAbs(this.m_Caret.Pos.X + tIncrement, this.m_Caret.Pos.Y);
                    break;

                case "\x1b[D": // CUB

                    if (e.CurParams.Count() > 0)
                    {
                        tIncrement = Convert.ToInt32(e.CurParams.Elements[0]);
                    }

                    if (tIncrement == 0) tIncrement = 1;

                    this.CaretToAbs(this.m_Caret.Pos.X - tIncrement, this.m_Caret.Pos.Y);
                    break;

                case "\x1b[H": // CUP
                case "\x1b[f": // HVP

                    System.Int32 X = 0;
                    System.Int32 Y = 0;

                    if (e.CurParams.Count() > 0)
                    {
                        Y = Convert.ToInt32(e.CurParams.Elements[0]) - 1;
                    }

                    if (e.CurParams.Count() > 1)
                    {
                        X = Convert.ToInt32(e.CurParams.Elements[1]) - 1;
                    }

                    this.CaretToRel(Y, X);
                    break;

                case "\x1b[J":

                    if (e.CurParams.Count() > 0)
                    {
                        Param = Convert.ToInt32(e.CurParams.Elements[0]);
                    }

                    this.ClearDown(Param);
                    break;

                case "\x1b[K":

                    if (e.CurParams.Count() > 0)
                    {
                        Param = Convert.ToInt32(e.CurParams.Elements[0]);
                    }

                    this.ClearRight(Param);
                    break;

                case "\x1b[L": // INSERT LINE
                    this.InsertLine(e.CurParams);
                    break;

                case "\x1b[M": // DELETE LINE
                    this.DeleteLine(e.CurParams);
                    break;

                case "\x1bN": // SS2 Single Shift (G2 -> GL)
                    this.m_CharAttribs.GS = this.m_G2;
                    break;

                case "\x1bO": // SS3 Single Shift (G3 -> GL)
                    this.m_CharAttribs.GS = this.m_G3;
                    //System.Console.WriteLine ("SS3: GS = {0}", this.CharAttribs.GS);
                    break;

                case "\x1b[m":
                    this.SetCharAttribs(e.CurParams);
                    break;

                case "\x1b[?h":
                    this.SetqmhMode(e.CurParams);
                    break;

                case "\x1b[?l":
                    this.SetqmlMode(e.CurParams);
                    break;

                case "\x1b[c": // DA Device Attributes
                    //                    this.DispatchMessage (this, "\x1b[?64;1;2;6;7;8;9c");
                    this.DispatchMessage(this, "\x1b[?6c");
                    break;

                case "\x1b[g":
                    this.ClearTabs(e.CurParams);
                    break;

                case "\x1b[h":
                    this.SethMode(e.CurParams);
                    break;

                case "\x1b[l":
                    this.SetlMode(e.CurParams);
                    break;

                case "\x1b[r": // DECSTBM Set Top and Bottom Margins
                    this.SetScrollRegion(e.CurParams);
                    break;

                case "\x1b[t": // DECSLPP Set Lines Per Page

                    if (e.CurParams.Count() > 0)
                    {
                        Param = System.Convert.ToInt32(e.CurParams.Elements[0]);
                    }

                    if (Param > 0)
                    {
                        // 2015-06-01 - 신윤남 - 컬럼 사이즈 변경
                        //this.SetSize(Param, this.m_Cols);
                        this.SetSize(Param, AppGlobal.s_ClientOption.TerminalColumnCount);
                    }

                    break;

                case "\x1b" + "D": // IND

                    if (e.CurParams.Count() > 0)
                    {
                        Param = System.Convert.ToInt32(e.CurParams.Elements[0]);
                    }

                    this.Index(Param);
                    break;

                case "\x1b" + "E": // NEL
                    this.LineFeed();
                    this.CarriageReturn();
                    break;

                case "\x1bH": // HTS
                    this.TabSet();
                    break;

                case "\x1bM": // RI
                    if (e.CurParams.Count() > 0)
                    {
                        Param = System.Convert.ToInt32(e.CurParams.Elements[0]);
                    }

                    this.ReverseIndex(Param);
                    break;

                default:
                    //System.Console.Write ("unsupported VT sequence {0} happened\n", e.CurSequence);
                    break;

            }

            if (e.CurSequence.StartsWith("\x1b("))
            {
                this.SelectCharSet(ref this.m_G0.Set, e.CurSequence.Substring(2));
            }
            else if (e.CurSequence.StartsWith("\x1b-") ||
                e.CurSequence.StartsWith("\x1b)"))
            {
                this.SelectCharSet(ref this.m_G1.Set, e.CurSequence.Substring(2));
            }
            else if (e.CurSequence.StartsWith("\x1b.") ||
                e.CurSequence.StartsWith("\x1b*"))
            {
                this.SelectCharSet(ref this.m_G2.Set, e.CurSequence.Substring(2));
            }
            else if (e.CurSequence.StartsWith("\x1b/") ||
                e.CurSequence.StartsWith("\x1b+"))
            {
                this.SelectCharSet(ref this.m_G3.Set, e.CurSequence.Substring(2));
            }

            SetScrollBarValues();
        }

        private void SelectCharSet(ref Chars.Sets CurTarget, string Curstring)
        {
            switch (Curstring)
            {
                case "B":
                    CurTarget = Chars.Sets.ASCII;
                    break;

                case "%5":
                    CurTarget = Chars.Sets.DECS;
                    break;

                case "0":
                    CurTarget = Chars.Sets.DECSG;
                    break;

                case ">":
                    CurTarget = Chars.Sets.DECTECH;
                    break;

                case "<":
                    CurTarget = Chars.Sets.DECSG;
                    break;

                case "A":
                    if ((this.m_Modes.Flags & Mode.s_National) == 0)
                    {
                        CurTarget = Chars.Sets.ISOLatin1S;
                    }
                    else
                    {
                        CurTarget = Chars.Sets.NRCUK;
                    }
                    break;

                case "4":
                    //                    CurTarget = uc_Chars.Sets.NRCDutch;
                    break;

                case "5":
                    CurTarget = Chars.Sets.NRCFinnish;
                    break;

                case "R":
                    CurTarget = Chars.Sets.NRCFrench;
                    break;

                case "9":
                    CurTarget = Chars.Sets.NRCFrenchCanadian;
                    break;

                case "K":
                    CurTarget = Chars.Sets.NRCGerman;
                    break;

                case "Y":
                    CurTarget = Chars.Sets.NRCItalian;
                    break;

                case "6":
                    CurTarget = Chars.Sets.NRCNorDanish;
                    break;

                case "'":
                    CurTarget = Chars.Sets.NRCNorDanish;
                    break;

                case "%6":
                    CurTarget = Chars.Sets.NRCPortuguese;
                    break;

                case "Z":
                    CurTarget = Chars.Sets.NRCSpanish;
                    break;

                case "7":
                    CurTarget = Chars.Sets.NRCSwedish;
                    break;

                case "=":
                    CurTarget = Chars.Sets.NRCSwiss;
                    break;

                default:
                    break;
            }
        }

        private void SetqmhMode(Params CurParams) // set mode for ESC?h command
        {
            System.Int32 OptInt = 0;

            foreach (string CurOption in CurParams.Elements)
            {
                try
                {
                    OptInt = System.Convert.ToInt32(CurOption);
                }
                catch (System.Exception CurException)
                {
                    //System.Console.WriteLine (CurException.Message);
                    MessageBox.Show(CurException.Message);
                }

                switch (OptInt)
                {
                    case 1: // set cursor keys to application mode
                        this.m_Modes.Flags = this.m_Modes.Flags | Mode.s_CursorAppln;
                        break;

                    case 2: // lock the keyboard
                        this.m_Modes.Flags = this.m_Modes.Flags | Mode.s_Locked;
                        break;

                    case 3: // set terminal to 132 column mode
                        //this.SetSize(this.m_Rows, 132);
                        // 2015-06-01- 신윤남 - 컬럼 사이즈 변경
                        this.SetSize(this.m_Rows, AppGlobal.s_ClientOption.TerminalColumnCount);
                        break;

                    case 5: // Light Background Mode
                        this.m_Modes.Flags = this.m_Modes.Flags | Mode.s_LightBackground;
                        this.OnRefreshEvent();
                        break;

                    case 6: // Origin Mode Relative
                        this.m_Modes.Flags = this.m_Modes.Flags | Mode.s_OriginRelative;
                        this.CaretToRel(0, 0);
                        break;

                    case 7: // Autowrap On
                        this.m_Modes.Flags = this.m_Modes.Flags | Mode.s_AutoWrap;
                        break;

                    case 8: // AutoRepeat On
                        this.m_Modes.Flags = this.m_Modes.Flags | Mode.s_Repeat;
                        break;

                    case 42: // DECNRCM Multinational Charset
                        this.m_Modes.Flags = this.m_Modes.Flags | Mode.s_National;
                        break;

                    case 66: // Numeric Keypad Application Mode On
                        this.m_Modes.Flags = this.m_Modes.Flags | Mode.s_KeypadAppln;
                        break;

                    default:
                        break;
                }
            }
        }

        private void SetqmlMode(Params CurParams) // set mode for ESC?l command
        {
            System.Int32 OptInt = 0;

            foreach (string CurOption in CurParams.Elements)
            {
                try
                {
                    OptInt = System.Convert.ToInt32(CurOption);
                }
                catch (System.Exception CurException)
                {
                    //System.Console.WriteLine (CurException.Message);
                    MessageBox.Show(CurException.Message);
                }

                switch (OptInt)
                {
                    case 1: // set cursor keys to normal cursor mode
                        this.m_Modes.Flags = this.m_Modes.Flags & ~Mode.s_CursorAppln;
                        break;

                    case 2: // unlock the keyboard
                        this.m_Modes.Flags = this.m_Modes.Flags & ~Mode.s_Locked;
                        break;

                    case 3: // set terminal to 80 column mode
                        //this.SetSize(this.m_Rows, 80);
                        // 2015-06-01-신윤남 - 컬럼 사이즈 변경
                        this.SetSize(this.m_Rows, AppGlobal.s_ClientOption.TerminalColumnCount);
                        break;

                    case 5: // Dark Background Mode
                        this.m_Modes.Flags = this.m_Modes.Flags & ~Mode.s_LightBackground;
                        this.OnRefreshEvent();
                        break;

                    case 6: // Origin Mode Absolute
                        this.m_Modes.Flags = this.m_Modes.Flags & ~Mode.s_OriginRelative;
                        this.CaretToAbs(0, 0);
                        break;

                    case 7: // Autowrap Off
                        this.m_Modes.Flags = this.m_Modes.Flags & ~Mode.s_AutoWrap;
                        break;

                    case 8: // AutoRepeat Off
                        this.m_Modes.Flags = this.m_Modes.Flags & ~Mode.s_Repeat;
                        break;

                    case 42: // DECNRCM National Charset
                        this.m_Modes.Flags = this.m_Modes.Flags & ~Mode.s_National;
                        break;

                    case 66: // Numeric Keypad Application Mode On
                        this.m_Modes.Flags = this.m_Modes.Flags & ~Mode.s_KeypadAppln;
                        break;

                    default:
                        break;
                }
            }
        }

        private void SethMode(Params CurParams) // set mode for ESC?h command
        {
            System.Int32 OptInt = 0;

            foreach (string CurOption in CurParams.Elements)
            {
                try
                {
                    OptInt = System.Convert.ToInt32(CurOption);
                }
                catch (System.Exception CurException)
                {
                    //System.Console.WriteLine (CurException.Message);
                    MessageBox.Show(CurException.Message);
                }

                switch (OptInt)
                {
                    case 1: // set local echo off
                        this.m_Modes.Flags = this.m_Modes.Flags | Mode.s_LocalEchoOff;
                        break;

                    default:
                        break;
                }
            }
        }

        private void SetlMode(Params CurParams) // set mode for ESC?l command
        {
            System.Int32 OptInt = 0;

            foreach (string CurOption in CurParams.Elements)
            {
                try
                {
                    OptInt = System.Convert.ToInt32(CurOption);
                }
                catch (System.Exception CurException)
                {
                    //System.Console.WriteLine (CurException.Message);
                    MessageBox.Show(CurException.Message);
                }

                switch (OptInt)
                {
                    case 1: // set LocalEcho on
                        this.m_Modes.Flags = this.m_Modes.Flags & ~Mode.s_LocalEchoOff;
                        break;

                    default:
                        break;
                }
            }
        }

        private void SetScrollRegion(Params CurParams)
        {
            if (CurParams.Count() > 0)
            {
                this.m_TopMargin = System.Convert.ToInt32(CurParams.Elements[0]) - 1;
            }

            if (CurParams.Count() > 1)
            {
                this.m_BottomMargin = System.Convert.ToInt32(CurParams.Elements[1]) - 1;
            }

            if (this.m_BottomMargin == 0)
            {
                this.m_BottomMargin = this.m_Rows - 1;
            }

            if (this.m_TopMargin < 0)
            {
                this.m_BottomMargin = 0;
            }
        }

        private void ClearCharAttribs()
        {
            this.m_CharAttribs.IsBold = false;
            this.m_CharAttribs.IsDim = false;
            this.m_CharAttribs.IsUnderscored = false;
            this.m_CharAttribs.IsBlinking = false;
            this.m_CharAttribs.IsInverse = false;
            this.m_CharAttribs.IsPrimaryFont = false;
            this.m_CharAttribs.IsAlternateFont = false;
            this.m_CharAttribs.UseAltBGColor = false;
            this.m_CharAttribs.UseAltColor = false;
            this.m_CharAttribs.AltColor = System.Drawing.Color.White;
            this.m_CharAttribs.AltBGColor = System.Drawing.Color.Black;
        }

        private void SetCharAttribs(Params CurParams)
        {
            if (CurParams.Count() < 1)
            {
                this.ClearCharAttribs();
                return;
            }

            for (int i = 0; i < CurParams.Count(); i++)
            {
                switch (System.Convert.ToInt32(CurParams.Elements[i]))
                {
                    case 0:
                        this.ClearCharAttribs();
                        break;

                    case 1:
                        this.m_CharAttribs.IsBold = true;
                        break;

                    case 4:
                        this.m_CharAttribs.IsUnderscored = true;
                        break;

                    case 5:
                        this.m_CharAttribs.IsBlinking = true;
                        break;

                    case 7:
                        this.m_CharAttribs.IsInverse = true;
                        break;

                    case 22:
                        this.m_CharAttribs.IsBold = false;
                        break;

                    case 24:
                        this.m_CharAttribs.IsUnderscored = false;
                        break;

                    case 25:
                        this.m_CharAttribs.IsBlinking = false;
                        break;

                    case 27:
                        this.m_CharAttribs.IsInverse = false;
                        break;

                    case 30:
                        this.m_CharAttribs.UseAltColor = true;
                        this.m_CharAttribs.AltColor = System.Drawing.Color.Black;
                        break;

                    case 31:
                        this.m_CharAttribs.UseAltColor = true;
                        this.m_CharAttribs.AltColor = System.Drawing.Color.Red;
                        break;

                    case 32:
                        this.m_CharAttribs.UseAltColor = true;
                        this.m_CharAttribs.AltColor = System.Drawing.Color.Green;
                        break;

                    case 33:
                        this.m_CharAttribs.UseAltColor = true;
                        this.m_CharAttribs.AltColor = System.Drawing.Color.Yellow;
                        break;

                    case 34:
                        this.m_CharAttribs.UseAltColor = true;
                        this.m_CharAttribs.AltColor = System.Drawing.Color.Blue;
                        break;

                    case 35:
                        this.m_CharAttribs.UseAltColor = true;
                        this.m_CharAttribs.AltColor = System.Drawing.Color.Magenta;
                        break;

                    case 36:
                        this.m_CharAttribs.UseAltColor = true;
                        this.m_CharAttribs.AltColor = System.Drawing.Color.Cyan;
                        break;

                    case 37:
                        this.m_CharAttribs.UseAltColor = true;
                        this.m_CharAttribs.AltColor = System.Drawing.Color.White;
                        break;

                    case 40:
                        this.m_CharAttribs.UseAltBGColor = true;
                        this.m_CharAttribs.AltBGColor = System.Drawing.Color.Black;
                        break;

                    case 41:
                        this.m_CharAttribs.UseAltBGColor = true;
                        this.m_CharAttribs.AltBGColor = System.Drawing.Color.Red;
                        break;

                    case 42:
                        this.m_CharAttribs.UseAltBGColor = true;
                        this.m_CharAttribs.AltBGColor = System.Drawing.Color.Green;
                        break;

                    case 43:
                        this.m_CharAttribs.UseAltBGColor = true;
                        this.m_CharAttribs.AltBGColor = System.Drawing.Color.Yellow;
                        break;

                    case 44:
                        this.m_CharAttribs.UseAltBGColor = true;
                        this.m_CharAttribs.AltBGColor = System.Drawing.Color.Blue;
                        break;

                    case 45:
                        this.m_CharAttribs.UseAltBGColor = true;
                        this.m_CharAttribs.AltBGColor = System.Drawing.Color.Magenta;
                        break;

                    case 46:
                        this.m_CharAttribs.UseAltBGColor = true;
                        this.m_CharAttribs.AltBGColor = System.Drawing.Color.Cyan;
                        break;

                    case 47:
                        this.m_CharAttribs.UseAltBGColor = true;
                        this.m_CharAttribs.AltBGColor = System.Drawing.Color.White;
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 수신한 문자를 처리 합니다.
        /// </summary>
        /// <param name="aCurrentChar"></param>
        private void ExecuteChar(Char aCurrentChar)
        {
            switch (aCurrentChar)
            {
                case '\x05': // ENQ request for the answerback message
                    this.DispatchMessage(this, "vt220");
                    break;

                case '\x07': // BEL ring my bell
                    // this.BELL;
                    break;

                case '\x08': // BS back space
                    this.CaretLeft();
                    break;

                case '\x09': // HT Horizontal Tab
                    this.Tab();
                    break;

                case '\x0A': // LF  LineFeed
                case '\x0B': // VT  VerticalTab
                case '\x0C': // FF  FormFeed
                case '\x84': // IND Index
                    this.LineFeed();
                    break;

                case '\x0D': // CR CarriageReturn
                    this.CarriageReturn();
                    break;

                case '\x0E': // SO maps G1 into GL
                    this.m_CharAttribs.GL = this.m_G1;
                    break;

                case '\x0F': // SI maps G0 into GL
                    this.m_CharAttribs.GL = this.m_G0;
                    break;

                case '\x11': // DC1/XON continue sending characters
                    this.m_XOff = false;
                    this.DispatchMessage(this, "");
                    break;

                case '\x13': // DC3/XOFF stop sending characters
                    //this.m_XOff = true;
                    break;

                case '\x85': // NEL Next line
                    this.LineFeed();
                    this.CaretToAbs(0, this.m_Caret.Pos.Y);
                    break;

                case '\x88': // HTS Horizontal tab set 
                    this.TabSet();
                    break;

                case '\x8D': // RI Reverse Index 
                    this.ReverseLineFeed();
                    break;

                case '\x8E': // SS2 Single Shift (G2 -> GL)
                    this.m_CharAttribs.GS = this.m_G2;
                    break;

                case '\x8F': // SS3 Single Shift (G3 -> GL)
                    this.m_CharAttribs.GS = this.m_G3;
                    break;

                default:
                    break;
            }
        }
        /// <summary>
        /// 터미널 크기를 설정 합니다.
        /// </summary>
        /// <param name="Rows"></param>
        /// <param name="Columns"></param>
        private void SetSize(Int32 aRows, Int32 aColumns)
        {
            this.m_Rows = aRows;
            this.m_Cols = aColumns;

            this.m_TopMargin = 0;
            this.m_BottomMargin = aRows - 1;
            /*
            this.ClientSize = new System.Drawing.Size (
            	System.Convert.ToInt32 (this.CharSize.Width  * this.Columns + 2) + this.VertScrollBar.Width,
            	System.Convert.ToInt32 (this.CharSize.Height * this.Rows    + 2));
            */
            // create the character grid (rows by columns) this is a shadow of what's displayed
            this.m_CharGrid = new Char[aRows][];

            //this.m_Caret.Pos.X = 0;
            //this.m_Caret.Pos.Y = 0;

            for (int i = 0; i < this.m_CharGrid.Length; i++)
            {
                this.m_CharGrid[i] = new Char[aColumns];
            }

            this.m_AttribGrid = new CharAttribStruct[aRows][];

            for (int i = 0; i < this.m_AttribGrid.Length; i++)
            {
                this.m_AttribGrid[i] = new CharAttribStruct[aColumns];
            }
        }

        /// <summary>
        /// 폰트 정보를 가져오기 합니다.
        /// </summary>
        private void GetFontInfo()
        {
            Graphics tmpGraphics = this.CreateGraphics();

            this.m_DrawstringOffset = this.GetDrawstringOffset(tmpGraphics, 0, 0, 'A');

            Point tmpPoint = this.GetCharSize(tmpGraphics);

            this.m_CharSize.Width = tmpPoint.X; // 
            this.m_CharSize.Height = tmpPoint.Y;

            //tmpGraphics.Dispose();

            this.m_UnderlinePos = this.m_CharSize.Height - 2;

            this.m_Caret.Bitmap = new Bitmap(this.m_CharSize.Width, this.m_CharSize.Height);
            this.m_Caret.Buffer = Graphics.FromImage(this.m_Caret.Bitmap);
            this.m_Caret.Buffer.Clear(Color.FromArgb(255, 181, 106));
            this.m_EraseBitmap = new Bitmap(this.m_CharSize.Width, this.m_CharSize.Height);
            this.m_EraseBuffer = Graphics.FromImage(this.m_EraseBitmap);

        }



        /// <summary>
        /// Rows를 가져오기 합니다.
        /// </summary>
        public int Rows
        {
            get { return this.m_Rows; }
        }
        /// <summary>
        /// Columns을 가져오기 합니다.
        /// </summary>
        public int Columns
        {
            get { return this.m_Cols; }
        }
        /// <summary>
        /// 접속 타입을 가져오거나 설정 합니다.
        /// </summary>
        public ConnectionTypes ConnectionType
        {
            get { return this.m_ConnectionType; }
            set { this.m_ConnectionType = value; }
        }
        /// <summary>
        /// Host Name을 가져오기 합니다.
        /// </summary>
        public string Hostname
        {
            get { return this.m_Hostname; }
            set { this.m_Hostname = value; }
        }
        /// <summary>
        /// Mode를 가져오거나 설정 합니다.
        /// </summary>
        public Mode Modes
        {
            get { return m_Modes; }
            set { m_Modes = value; }
        }
        /// <summary>
        /// Device Info 입니다.
        /// </summary>
        private DeviceInfo m_DeviceInfo;

        /// <summary>
        /// Device Info 가져오거나 설정 합니다.
        /// </summary>
        public DeviceInfo DeviceInfo
        {
            get { return m_DeviceInfo; }
            set { m_DeviceInfo = value; }
        }



        /// <summary>
        /// 접속할 데몬 정보 입니다.
        /// </summary>
        private DaemonProcessInfo m_DaemonProcessInfo;
        /// <summary>
        /// 접속할 데몬 정보 속성을 가져오거나 설정합니다.
        /// </summary>
        public DaemonProcessInfo DaemonProcessInfo
        {
            get { return m_DaemonProcessInfo; }
            set { m_DaemonProcessInfo = value; }
        }

        /// <summary>
        /// 장비 연결을 시작 합니다
        /// </summary>
        //public void ConnectDevice()
        //{
        //    mnuClear_Click(null, null);
        //    m_ConnectionCommandSet = null;
        //    DaemonProcessInfo tDaemonProcessInfo;
        //    try
        //    {
        //        // 2013-04-26 - shinyn - 장비연결요청 --> 연결가능한 데몬있는지 확인 --> 데몬으로 통해 장비 연결
        //        if (m_DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET)
        //        {
        //            if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online && m_DeviceInfo.IsRegistered)
        //            {
        //                m_ConnectionType = ConnectionTypes.RemoteTelnet;
        //                List<int> tDisconnectDaemonList = new List<int>();
        //                bool tIsDaemonConnect = false;
        //                while (true)
        //                {
        //                    if (m_DaemonProcessInfo == null)
        //                    {
        //                        AppGlobal.s_FileLogProcessor.PrintLog("사용 가능한 Daemon 정보를 로드합니다.");

        //                        UseableDaemonRequestInfo tDaemonRequestInfo = new UseableDaemonRequestInfo(AppGlobal.s_LoginResult.ClientID, tDisconnectDaemonList);
        //                        RequestCommunicationData tRequestData = null;

        //                        tRequestData = AppGlobal.MakeDefaultRequestData();
        //                        tRequestData.CommType = E_CommunicationType.RequestDaemonInfo;
        //                        tRequestData.RequestData = tDaemonRequestInfo;
        //                        m_Result = null;
        //                        m_MRE.Reset();

        //                        AppGlobal.SendRequestData(this, tRequestData);
        //                        m_MRE.WaitOne(AppGlobal.s_RequestTimeOut);


        //                        // 2013-03-07 - shinyn - 사용가능한 Daemon정보가 없으면 메시지 보이고, 로그저장
        //                        if (m_Result == null)
        //                        {
        //                            // 2013-04-26- shinyn- 사용가능한 Daemon정보가 있는지 로그 정보 보이도록 하여 수정
        //                            System.Diagnostics.Debug.WriteLine("사용가능한 Daemon 정보가 없습니다.");
        //                            // 2013-04-26- shinyn- 크로스 스레드 에러나는 부분 수정
        //                            //AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "사용 가능한 Daemon 정보 로드에 실패 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                            MessageBox.Show("사용 가능한 Daemon 정보 로드에 실패 했습니다.");
        //                            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "사용 가능한 Daemon 정보 로드에 실패 했습니다.");
        //                            break;
        //                        }
        //                        else if (m_Result.Error.Error != E_ErrorType.NoError || m_Result.ResultData == null)
        //                        {
        //                            // 2013-04-26- shinyn- 크로스 스레드 에러나는 부분 수정
        //                            //AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "사용 가능한 Daemon 정보 로드에 실패 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                            MessageBox.Show("사용 가능한 Daemon 정보 로드에 실패 했습니다.");
        //                            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "사용 가능한 Daemon 정보 로드에 실패 했습니다.");
        //                            break;
        //                        }
        //                        tDaemonProcessInfo = m_Result.ResultData as DaemonProcessInfo;
        //                        if (tDaemonProcessInfo == null)
        //                        {
        //                            TerminalStatus = E_TerminalStatus.Disconnected;
        //                            // 2013-04-26- shinyn- 크로스 스레드 에러나는 부분 수정
        //                            //AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "사용 가능한 Daemon 정보 로드에 실패 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                            MessageBox.Show("사용 가능한 Daemon 정보 로드에 실패 했습니다.");
        //                            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "사용 가능한 Daemon 정보 로드에 실패 했습니다.");
        //                            break;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        tDaemonProcessInfo = m_DaemonProcessInfo;
        //                    }

        //                    if (ConnectTelnetDaemon(tDaemonProcessInfo))
        //                    {
        //                        tIsDaemonConnect = true;
        //                        break;
        //                    }
        //                    else
        //                    {
        //                        tDisconnectDaemonList.Add(tDaemonProcessInfo.DaemonID);
        //                    }
        //                }
        //                // return tIsDaemonConnect;
        //            }
        //            else
        //            {
        //                m_ConnectionType = ConnectionTypes.LocalTelnet;
        //                RequestCommunicationData tRequestData = null;
        //                TelnetCommandInfo tCommandInfo = new TelnetCommandInfo();
        //                tCommandInfo.Sender = this;
        //                tCommandInfo.DeviceInfo = m_DeviceInfo;

        //                //tCommandInfo.UserID = AppGlobal.s_LoginResult.UserID;
        //                tCommandInfo.WorkTyp = E_TelnetWorkType.Connect;

        //                // tRequestData = AppGlobal.MakeDefaultRequestData();
        //                tRequestData = new RequestCommunicationData();
        //                tRequestData.CommType = E_CommunicationType.RequestCommandProcess;
        //                tRequestData.RequestData = tCommandInfo;
        //                TerminalStatus = E_TerminalStatus.TryConnection;

        //                AppGlobal.s_TelnetProcessor.ExecuteCommand(tRequestData);
        //            }
        //        }
        //        else if (m_DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
        //        {
        //            // 2013-03-06 - shinyn - SSH텔넷인 경우 분기처리 추가
        //            if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online && m_DeviceInfo.IsRegistered)
        //            {
        //                m_ConnectionType = ConnectionTypes.RemoteTelnet;
        //                List<int> tDisconnectDaemonList = new List<int>();
        //                bool tIsDaemonConnect = false;
        //                while (true)
        //                {
        //                    if (m_DaemonProcessInfo == null)
        //                    {
        //                        AppGlobal.s_FileLogProcessor.PrintLog("사용 가능한 Daemon 정보를 로드합니다.");

        //                        UseableDaemonRequestInfo tDaemonRequestInfo = new UseableDaemonRequestInfo(AppGlobal.s_LoginResult.ClientID, tDisconnectDaemonList);
        //                        RequestCommunicationData tRequestData = null;

        //                        tRequestData = AppGlobal.MakeDefaultRequestData();
        //                        tRequestData.CommType = E_CommunicationType.RequestDaemonInfo;
        //                        tRequestData.RequestData = tDaemonRequestInfo;
        //                        m_Result = null;
        //                        m_MRE.Reset();

        //                        AppGlobal.SendRequestData(this, tRequestData);
        //                        m_MRE.WaitOne(AppGlobal.s_RequestTimeOut);
        //                        if (m_Result == null || m_Result.Error.Error != E_ErrorType.NoError || m_Result.ResultData == null)
        //                        {
        //                            // 2013-04-26- shinyn- 크로스 스레드 에러나는 부분 수정
        //                            //AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "사용 가능한 Daemon 정보 로드에 실패 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                            MessageBox.Show("사용 가능한 Daemon 정보 로드에 실패 했습니다.");
        //                            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "사용 가능한 Daemon 정보 로드에 실패 했습니다.");
        //                            break;
        //                        }
        //                        tDaemonProcessInfo = m_Result.ResultData as DaemonProcessInfo;
        //                        if (tDaemonProcessInfo == null)
        //                        {
        //                            TerminalStatus = E_TerminalStatus.Disconnected;
        //                            // 2013-04-26- shinyn- 크로스 스레드 에러나는 부분 수정
        //                            //AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "사용 가능한 Daemon 정보 로드에 실패 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                            MessageBox.Show("사용 가능한 Daemon 정보 로드에 실패 했습니다.");
        //                            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "사용 가능한 Daemon 정보 로드에 실패 했습니다.");
        //                            break;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        tDaemonProcessInfo = m_DaemonProcessInfo;
        //                    }

        //                    if (ConnectTelnetDaemon(tDaemonProcessInfo))
        //                    {
        //                        tIsDaemonConnect = true;
        //                        break;
        //                    }
        //                    else
        //                    {
        //                        tDisconnectDaemonList.Add(tDaemonProcessInfo.DaemonID);
        //                    }
        //                }
        //                // return tIsDaemonConnect;
        //            }
        //            else
        //            {
        //                m_ConnectionType = ConnectionTypes.LocalTelnet;
        //                RequestCommunicationData tRequestData = null;
        //                TelnetCommandInfo tCommandInfo = new TelnetCommandInfo();
        //                tCommandInfo.Sender = this;
        //                tCommandInfo.DeviceInfo = m_DeviceInfo;

        //                // 2013-01-28 - shinyn - SSH인경우 아이디와 비밀번호가 있어야 하므로 넣어줌
        //                if(AppGlobal.s_ClientOption.IsUseTerminalAutoLogin == true)
        //                {
        //                    tCommandInfo.DeviceInfo.TelnetID1 = m_DeviceInfo.TerminalConnectInfo.ID;
        //                    tCommandInfo.DeviceInfo.TelnetPwd1 = m_DeviceInfo.TerminalConnectInfo.Password;
        //                }
        //                //tCommandInfo.UserID = AppGlobal.s_LoginResult.UserID;
        //                tCommandInfo.WorkTyp = E_TelnetWorkType.Connect;

        //                // tRequestData = AppGlobal.MakeDefaultRequestData();
        //                tRequestData = new RequestCommunicationData();
        //                tRequestData.CommType = E_CommunicationType.RequestCommandProcess;
        //                tRequestData.RequestData = tCommandInfo;
        //                TerminalStatus = E_TerminalStatus.TryConnection;

        //                AppGlobal.s_TelnetProcessor.ExecuteCommand(tRequestData);
        //            }
        //        }
        //        else
        //        {
        //            m_ConnectionType = ConnectionTypes.Serial;
        //            if (!AppGlobal.s_SerialProcessor.ConnectDevice(this, m_DeviceInfo.TerminalConnectInfo.SerialConfig))
        //            {
        //                TerminalStatus = E_TerminalStatus.Disconnected;
        //                // 2013-04-26- shinyn- 크로스 스레드 에러나는 부분 수정
        //                //AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, m_DeviceInfo.TerminalConnectInfo.SerialConfig.PortName + " 을 사용 할 수 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                MessageBox.Show(m_DeviceInfo.TerminalConnectInfo.SerialConfig.PortName + " 을 사용 할 수 없습니다.");
        //                // return false;
        //            }
        //            else
        //            {
        //                TerminalStatus = E_TerminalStatus.Connection;
        //                m_IsConnected = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, ex.ToString());
        //    }
        //    // return true;

        //}

        // 2015-04-16 - 신윤남 - 터미널 결과를 파일에 저장합니다.
        private LogWriter m_TerminalLog = null;

        private void StartTerminalLog(DeviceInfo aDeviceInfo)
        {

            try
            {
                if (m_TerminalLog != null)
                {
                    m_TerminalLog.Delete();
                }

                m_TerminalLog = null;
            }
            catch (Exception ex)
            {
            }

            try
            {
				// 2019-11-10 ???? (?? ?? ?? ??)
                string tFileName = aDeviceInfo.IPAddress.ToString() + "_" + GetDateTime() + ".log";
                string tDirPath = AppGlobal.s_ClientOption.LogPath + @"TerminalLogs";
                string tFilePath = AppGlobal.s_ClientOption.LogPath + @"TerminalLogs\" + tFileName;

                m_TerminalLog = new LogWriter(tDirPath, tFilePath);
            }
            catch (Exception ex)
            {
            }
        }

        private string GetDateTime()
        {
            DateTime NowDate = DateTime.Now;
            return NowDate.ToString("yyyyMMddHHmmss");
        }

        private string GetDate()
        {
            DateTime NowDate = DateTime.Now;
            return NowDate.ToString("yyyyMMdd");
        }



        public void ConnectDevice(DeviceInfo target, DeviceInfo jumpHost)
        {
            this.JumpHost = jumpHost;
            ConnectDevice((object)target);
        }

        public object ConnectDevice(object aDeviceInfo)
        {
            m_DeviceInfo = new DeviceInfo((DeviceInfo)aDeviceInfo);
            if (!AppGlobal.s_ClientOption.IsTerminalTextClear)
                mnuClear_Click(null, null);
            m_ConnectionCommandSet = null;
            DaemonProcessInfo tDaemonProcessInfo;

            int ConnectionMode = AppGlobal.s_ConnectionMode;

            //2023-06-13 VOIP AGW PORT 2001 치환 
            if (m_DeviceInfo.DevicePartCode == 13)
                m_DeviceInfo.TerminalConnectInfo.TelnetPort = 2001;

            // 2015-04-16 - 신윤남 - Terminal로그를 생성합니다.
            try
            {
                StartTerminalLog(m_DeviceInfo);   
            }
            catch (Exception ex)
            {
            }

            try
            {
                // 2013-04-26 - shinyn - 장비연결요청 --> 연결가능한 데몬있는지 확인 --> 데몬으로 통해 장비 연결
                if (m_DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET)
                {
                    if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online)//&& m_DeviceInfo.IsRegistered) 모든 장비를 데몬을 통한 통신으로 변경, 등록된 장비 여부 체크 제외 
                    {

                        m_ConnectionType = ConnectionTypes.RemoteTelnet;
                        List<int> tDisconnectDaemonList = new List<int>();
                        bool tIsDaemonConnect = false;
                        while (true)
                        {

                            if (m_DaemonProcessInfo == null || IsChangeMode)
                            {
                                AppGlobal.s_FileLogProcessor.PrintLog("사용 가능한 Daemon 정보를 로드합니다.");

                                UseableDaemonRequestInfo tDaemonRequestInfo = new UseableDaemonRequestInfo(AppGlobal.s_LoginResult.ClientID, tDisconnectDaemonList);
                                RequestCommunicationData tRequestData = null;

                                tRequestData = AppGlobal.MakeDefaultRequestData();
                                tRequestData.CommType = E_CommunicationType.RequestDaemonInfo;
                                if (ConnectionMode == 3)
                                    tRequestData.CommType = E_CommunicationType.RequestSSHDaemonInfo;
                                tRequestData.RequestData = tDaemonRequestInfo;
                                m_Result = null;

                                m_MRE.Reset();

                                AppGlobal.SendRequestData(this, tRequestData);

                                // 2013-05-02 - shinyn - 데몬 정보 요청 하는 것을 로그에 저장
                                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "ConnectDevice : 사용 가능한 Daemon정보를 요청했습니다. IP : " + m_DeviceInfo.IPAddress);

                                m_MRE.WaitOne(AppGlobal.s_RequestTimeOut);


                                // 2013-03-07 - shinyn - 사용가능한 Daemon정보가 없으면 메시지 보이고, 로그저장
                                if (m_Result == null)
                                {
                                    // 2013-04-26- shinyn- 사용가능한 Daemon정보가 있는지 로그 정보 보이도록 하여 수정
                                    // 2013-05-02 - shinyn - 사용가능한 Daemon정보가 있는지 로그 정보에 장비 아이피를 보이도록 수정
                                    //System.Diagnostics.Debug.WriteLine("사용가능한 Daemon 정보가 없습니다. IP : " + m_DeviceInfo.IPAddress);

                                    // 2013-04-26- shinyn- 크로스 스레드 에러나는 부분 수정
                                    TerminalStatus = E_TerminalStatus.Disconnected;
                                    AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "사용 가능한 Daemon 정보 로드에 실패 했습니다. IP : " + m_DeviceInfo.IPAddress);
                                    break;
                                }
                                else if (m_Result.Error.Error != E_ErrorType.NoError || m_Result.ResultData == null)
                                {
                                    // 2013-04-26- shinyn- 크로스 스레드 에러나는 부분 수정
                                    // 2013-05-02 - shinyn - 사용가능한 Daemon정보가 있는지 로그 정보에 장비 아이피를 보이도록 수정
                                    TerminalStatus = E_TerminalStatus.Disconnected;
                                    //System.Diagnostics.Debug.WriteLine("사용가능한 Daemon 정보가 없습니다. IP : " + m_DeviceInfo.IPAddress);
                                    AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "사용 가능한 Daemon 정보 로드에 실패 했습니다. IP : " + m_DeviceInfo.IPAddress);
                                    break;
                                }

                                tDaemonProcessInfo = m_Result.ResultData as DaemonProcessInfo;
                                if (tDaemonProcessInfo == null)
                                {
                                    TerminalStatus = E_TerminalStatus.Disconnected;
                                    // 2013-04-26- shinyn- 크로스 스레드 에러나는 부분 수정
                                    // 2013-05-02 - shinyn - 사용가능한 Daemon정보가 있는지 로그 정보에 장비 아이피를 보이도록 수정
                                    //System.Diagnostics.Debug.WriteLine("사용가능한 Daemon 정보가 없습니다. IP : " + m_DeviceInfo.IPAddress);
                                    AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "사용 가능한 Daemon 정보 로드에 실패 했습니다. + IP : " + m_DeviceInfo.IPAddress);
                                    break;
                                }
                            }
                            else
                            {
                                tDaemonProcessInfo = m_DaemonProcessInfo;
                            }

                            if (ConnectTelnetDaemon(tDaemonProcessInfo, ref ConnectionMode))
                            {
                                tIsDaemonConnect = true;
                                IsChangeMode = false;
                                break;
                            }
                            else
                            {
                                tDisconnectDaemonList.Add(tDaemonProcessInfo.DaemonID);
                                
                                if (ConnectionMode == 3)
                                {
                                    TerminalStatus = E_TerminalStatus.Disconnected;
                                    AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "Daemon(터널링) 또는 장비 연결에 실패 했습니다. IP : " + m_DeviceInfo.IPAddress);
                                    break;
                                }

                                if (IsChangeMode)
                                    ConnectionMode = 3;

                            }
                        }
                        // return tIsDaemonConnect;
                    }
                    else
                    {
                        m_ConnectionType = ConnectionTypes.LocalTelnet;
                        RequestCommunicationData tRequestData = null;
                        TelnetCommandInfo tCommandInfo = new TelnetCommandInfo();
                        tCommandInfo.Sender = this;
                        tCommandInfo.DeviceInfo = m_DeviceInfo;

                        //tCommandInfo.UserID = AppGlobal.s_LoginResult.UserID;
                        tCommandInfo.WorkTyp = E_TelnetWorkType.Connect;

                        // tRequestData = AppGlobal.MakeDefaultRequestData();
                        tRequestData = new RequestCommunicationData();
                        tRequestData.CommType = E_CommunicationType.RequestCommandProcess;
                        tRequestData.RequestData = tCommandInfo;
                        TerminalStatus = E_TerminalStatus.TryConnection;

                        AppGlobal.s_TelnetProcessor.ExecuteCommand(tRequestData);
                    }
                }
                else if (m_DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
                {
                    // 2013-03-06 - shinyn - SSH텔넷인 경우 분기처리 추가
                    if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online )//&& m_DeviceInfo.IsRegistered) 모든 장비를 데몬을 통한 통신으로 변경, 등록된 장비 여부 체크 제외
                    {
                        m_ConnectionType = ConnectionTypes.RemoteTelnet;
                        List<int> tDisconnectDaemonList = new List<int>();
                        bool tIsDaemonConnect = false;
                        while (true)
                        {
                            if (m_DaemonProcessInfo == null)
                            {
                                AppGlobal.s_FileLogProcessor.PrintLog("사용 가능한 Daemon 정보를 로드합니다.");

                                UseableDaemonRequestInfo tDaemonRequestInfo = new UseableDaemonRequestInfo(AppGlobal.s_LoginResult.ClientID, tDisconnectDaemonList);
                                RequestCommunicationData tRequestData = null;

                                tRequestData = AppGlobal.MakeDefaultRequestData();
                                tRequestData.CommType = E_CommunicationType.RequestDaemonInfo;
                                tRequestData.RequestData = tDaemonRequestInfo;
                                m_Result = null;
                                m_MRE.Reset();

                                AppGlobal.SendRequestData(this, tRequestData);
                                m_MRE.WaitOne(AppGlobal.s_RequestTimeOut);
                                if (m_Result == null || m_Result.Error.Error != E_ErrorType.NoError || m_Result.ResultData == null)
                                {
                                    // 2013-04-26- shinyn- 크로스 스레드 에러나는 부분 수정
                                    //AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "사용 가능한 Daemon 정보 로드에 실패 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    MessageBox.Show("사용 가능한 Daemon 정보 로드에 실패 했습니다.");
									TerminalStatus = E_TerminalStatus.Disconnected;
                                    AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "사용 가능한 Daemon 정보 로드에 실패 했습니다.");
                                    break;
                                }
                                tDaemonProcessInfo = m_Result.ResultData as DaemonProcessInfo;
                                if (tDaemonProcessInfo == null)
                                {
                                    TerminalStatus = E_TerminalStatus.Disconnected;
                                    // 2013-04-26- shinyn- 크로스 스레드 에러나는 부분 수정
                                    //AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "사용 가능한 Daemon 정보 로드에 실패 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    MessageBox.Show("사용 가능한 Daemon 정보 로드에 실패 했습니다.");
                                    AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "사용 가능한 Daemon 정보 로드에 실패 했습니다.");
                                    break;
                                }
                            }
                            else
                            {
                                tDaemonProcessInfo = m_DaemonProcessInfo;
                            }

                            if (ConnectTelnetDaemon(tDaemonProcessInfo, ref ConnectionMode))
                            {
                                tIsDaemonConnect = true;
                                break;
                            }
                            else
                            {
                                tDisconnectDaemonList.Add(tDaemonProcessInfo.DaemonID);
                            }
                        }
                        // return tIsDaemonConnect;
                    }
                    else
                    {
                        m_ConnectionType = ConnectionTypes.LocalTelnet;
                        RequestCommunicationData tRequestData = null;
                        TelnetCommandInfo tCommandInfo = new TelnetCommandInfo();
                        tCommandInfo.Sender = this;
                        tCommandInfo.DeviceInfo = m_DeviceInfo;

                        // 2013-01-28 - shinyn - SSH인경우 아이디와 비밀번호가 있어야 하므로 넣어줌
                        if (AppGlobal.s_ClientOption.IsUseTerminalAutoLogin == true)
                        {
                            tCommandInfo.DeviceInfo.TelnetID1 = m_DeviceInfo.TerminalConnectInfo.ID;
                            tCommandInfo.DeviceInfo.TelnetPwd1 = m_DeviceInfo.TerminalConnectInfo.Password;
                        }
                        //tCommandInfo.UserID = AppGlobal.s_LoginResult.UserID;
                        tCommandInfo.WorkTyp = E_TelnetWorkType.Connect;

                        // tRequestData = AppGlobal.MakeDefaultRequestData();
                        tRequestData = new RequestCommunicationData();
                        tRequestData.CommType = E_CommunicationType.RequestCommandProcess;
                        tRequestData.RequestData = tCommandInfo;
                        TerminalStatus = E_TerminalStatus.TryConnection;

                        AppGlobal.s_TelnetProcessor.ExecuteCommand(tRequestData);
                    }
                }
                else
                {
                    m_ConnectionType = ConnectionTypes.Serial;
                    if (!AppGlobal.s_SerialProcessor.ConnectDevice(this, m_DeviceInfo.TerminalConnectInfo.SerialConfig))
                    {
                        TerminalStatus = E_TerminalStatus.Disconnected;
                        // 2013-04-26- shinyn- 크로스 스레드 에러나는 부분 수정
                        //AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, m_DeviceInfo.TerminalConnectInfo.SerialConfig.PortName + " 을 사용 할 수 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        MessageBox.Show(m_DeviceInfo.TerminalConnectInfo.SerialConfig.PortName + " 을 사용 할 수 없습니다.");
                        // return false;
                    }
                    else
                    {
                        TerminalStatus = E_TerminalStatus.Connection;
						// 2019-11-10 ???? (OneTerminal ??? ?? ??UI ??)
                        if (ProgreBarHandlerEvent!= null)
                            ProgreBarHandlerEvent("디바이스에 연결 되었습니다.", eProgressItemType.Standard, false);
                        m_IsConnected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, ex.ToString());
            }
            return null;

        }

        private int m_TelnetDaemonID = -1;
        /// <summary>
        /// 할당된 데몬에 접속을 합니다.
        /// </summary>
        /// <param name="aDaemonProcessInfo"></param>
        /// <returns></returns>
        private bool ConnectTelnetDaemon(DaemonProcessInfo aDaemonProcessInfo, ref int ConnectionMode)
        {
            RemoteClientMethod tDaemonMethod = null;

            MKRemote tRemoteObject = null;
            DateTime tSDate = DateTime.Now;

            m_TelnetDaemonID = aDaemonProcessInfo.DaemonID;
            if (AppGlobal.s_DaemonProcessList.ContainsKey(aDaemonProcessInfo.DaemonID))
            {
                m_DaemonProcessRemoteObject = AppGlobal.s_DaemonProcessList[aDaemonProcessInfo.DaemonID];

                // 2013-07-26 - 이미 연결된 데몬을 가져왔을경우 상태 체크를 하여 재연결 하도록 한다.
                //  m_DaemonProcessRemoteObject.OnDisconnectDaemon += new DefaultHandler(m_DaemonProcessRemoteObject_OnDisconnectDaemon);
            }
            else
            {

                if (AppGlobal.TryDaemonConnect(aDaemonProcessInfo.IP, aDaemonProcessInfo.Port, aDaemonProcessInfo.ChannelName, out tRemoteObject) != E_ConnectError.NoError)
                {
                    return false;
                }

                m_DaemonProcessRemoteObject = new DaemonProcessRemoteObject(aDaemonProcessInfo, tRemoteObject);
                m_DaemonProcessRemoteObject.OnDisconnectDaemon += new DefaultHandler(m_DaemonProcessRemoteObject_OnDisconnectDaemon);
                RemoteClientMethod tRemoteClientMethod = (RemoteClientMethod)m_DaemonProcessRemoteObject.RemoteObject.ServerObject;
                tRemoteClientMethod.CallUserConnectDaemonHandler(ObjectConverter.GetBytes(AppGlobal.s_LoginResult.UserInfo));

                if (!AppGlobal.s_DaemonProcessList.ContainsKey(aDaemonProcessInfo.DaemonID))
                {
                    AppGlobal.s_DaemonProcessList.Add(m_DaemonProcessRemoteObject.DaemonID, m_DaemonProcessRemoteObject);
                }
            }

            /*===============================================================================*/
            /*===============================================================================*/
            //mnuClear_Click(null, null);
            if (ConnectionMode == 3)
            {

                RequestCommunicationData ttRequestData = null;
                DeviceInfo tDeviceInfo = new DeviceInfo();

                tDeviceInfo = m_DeviceInfo;

                ttRequestData = AppGlobal.MakeDefaultRequestData();
                ttRequestData.CommType = E_CommunicationType.RequestSSHTunnelOpen;
                ttRequestData.RequestData = tDeviceInfo;

                m_Result = null;
                m_MRE.Reset();

                m_DaemonProcessRemoteObject.SendDaemonRequestData(this, ttRequestData);

                m_MRE.WaitOne(AppGlobal.s_RequestTimeOut * 12);

                if (m_Result == null || m_Result.Error.Error != E_ErrorType.NoError)
                {
                    AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, m_DeviceInfo.IPAddress + " 장비에 접속 할 수 없습니다. 터널링 요청 실패");
                    if (m_Result != null)
                        AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, m_DeviceInfo.IPAddress + " ErrorString = " + m_Result.Error.ErrorString );
                    return false;
                }
                else
                {
                    if ( (((DeviceInfo)m_Result.ResultData).SSHTunnelIP == null) || (((DeviceInfo)m_Result.ResultData).SSHTunnelPort == 0) )
                        return false;
                    m_DeviceInfo.IPAddress = ((DeviceInfo)m_Result.ResultData).SSHTunnelIP;
                    m_DeviceInfo.TerminalConnectInfo.TelnetPort = ((DeviceInfo)m_Result.ResultData).SSHTunnelPort;
                }
            }
    
            /*===============================================================================*/
            /*===============================================================================*/
            if (!AppGlobal.s_ClientOption.IsTerminalTextClear)
                mnuClear_Click(null, null);
            RequestCommunicationData tRequestData = null;
            TelnetCommandInfo tCommandInfo = new TelnetCommandInfo();
            tCommandInfo.DeviceInfo = m_DeviceInfo;
            tCommandInfo.UserID = AppGlobal.s_LoginResult.UserID;
            tCommandInfo.WorkTyp = E_TelnetWorkType.Connect;

            tRequestData = AppGlobal.MakeDefaultRequestData();
            tRequestData.CommType = E_CommunicationType.RequestCommandProcess;
            tRequestData.RequestData = tCommandInfo;
            TerminalStatus = E_TerminalStatus.TryConnection;


            m_Result = null;
            m_MRE.Reset();

            m_DaemonProcessRemoteObject.SendDaemonRequestData(this, tRequestData);

            m_MRE.WaitOne(AppGlobal.s_RequestTimeOut);
            if (m_Result == null || m_Result.Error.Error != E_ErrorType.NoError)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, tCommandInfo.DeviceInfo.IPAddress + " 장비에 접속 할 수 없습니다.");
                if (ConnectionMode != 3)
                {
                    if (AppGlobal.IsRpcsDevice(m_DeviceInfo.ModelID))
                    {
                        if (MessageBox.Show("유선 접속이 실패 하였습니다. 무선 접속 하시겠습니까? \r\nRPCS(무선) 장비 접속시 LTE망 과금이 발생합니다.\r\n접속 하시겠습니까?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            IsChangeMode = true;
                            //ConnectionMode = 3;
                            return false;
                        }
                        
                    }
                    return false;
                }
            }
            else
            {
                if (ConnectionMode != 3)
                {
                    if (AppGlobal.IsRpcsDevice(m_DeviceInfo.ModelID))
                    {
                        TelnetCommandResultInfo tTelnetCommandResultInfo = m_Result as TelnetCommandResultInfo;
                        if (tTelnetCommandResultInfo.ReslutType == E_TelnetReslutType.DisConnected)
                        {

                            if (MessageBox.Show("유선 접속이 실패 하였습니다. 무선 접속 하시겠습니까? \r\nRPCS(무선) 장비 접속시 LTE망 과금이 발생합니다.\r\n접속 하시겠습니까?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                IsChangeMode = true;
                                //ConnectionMode = 3;
                                return false;
                            }
                        }
                    }
                }
            }
            
            m_IsConnected = true;

            return m_IsConnected;
        }


        /// <summary>
        /// 데몬이랑 연결 종료 되었을때 처리 입니다.
        /// </summary>
        void m_DaemonProcessRemoteObject_OnDisconnectDaemon()
        {

            Disconnect();
        }



        /// <summary>
        /// 접속 세션 ID 입니다.
        /// </summary>
        private int m_ConnectedSessionID = 0;

        public int ConnectedSessionID
        {
            get { return m_ConnectedSessionID; }
        }



        /// <summary>
        /// 결과를 표시 합니다.
        /// </summary>
        /// <param name="aResult"></param>
        public override void DisplayResult(ResultCommunicationData aResult)
        {

            //결과표시
            //if (aResult == null) return;

            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument1<ResultCommunicationData>(DisplayResult), aResult);
                return;
            }

            DisplayScrollLast(m_ScrollbackBuffer.Count - (m_Rows -1));// 스크롤 업다운시 입력값 첫 문자 누락되는 현상 
            TelnetCommandResultInfo tTelnetResultInfo = (TelnetCommandResultInfo)aResult;
            if (tTelnetResultInfo.SessionID != 0)
            {
                m_ConnectedSessionID = tTelnetResultInfo.SessionID;

                if (!m_DaemonProcessRemoteObject.ConnectedSessionIDList.Contains(m_ConnectedSessionID))
                {
                    m_DaemonProcessRemoteObject.ConnectedSessionIDList.Add(m_ConnectedSessionID);
                }

                if (tTelnetResultInfo.ReslutType == E_TelnetReslutType.Data)
                {
                    m_IsConnected = true;
                    if (TerminalStatus == E_TerminalStatus.TryConnection)
                    {
                        TerminalStatus = E_TerminalStatus.Connection;
						// 2019-11-10 ???? (OneTerminal ??? ?? ??UI ??)
                        if (ProgreBarHandlerEvent != null)
                            ProgreBarHandlerEvent("디바이스에 연결 되었습니다.", eProgressItemType.Standard, false);
                    }

                }
            }

            if (tTelnetResultInfo.ReslutType == E_TelnetReslutType.DisConnected)
            {
                TerminalStatus = E_TerminalStatus.Disconnected;
                m_IsConnected = false;
            }
            else
            {
                // 받은 문자열이 많이 오는 경우 멈춰버리는 오류 수정
                // Thread.Sleep(20);
                
                OnReceivedData(aResult.ResultData.ToString());
                
                if (this.Parent is SuperTabControlPanel)
                {
                    SuperTabControlPanel tPanel = (SuperTabControlPanel)this.Parent;
                }
                else if (this.Parent is TerminalWindows)
                {
                    TerminalWindows tWindow = (TerminalWindows)this.Parent;
                }

                //System.Diagnostics.Debug.WriteLine("ResultString : " + aResult.ResultData.ToString());                
                // 2015-04-16 - 신윤남 - 터미널 로그를 저장합니다.
                
                // Gunny 로그 변경시 참조 -
                if (m_TerminalLog != null)
                {
                   m_TerminalLog.Log(aResult.ResultData.ToString());
                }

                // 2013-08-08 - shinyn -  More String 온경우 SPACE스크립트 실행
                // 모델별로 -- More -- 처리하는 리스트 받아서 처리하기.

                // 클라이언트 옵션에 MoreString 자동스크롤 사용인경우에만 실행
                if (AppGlobal.s_ClientOption.IsUseTerminalAutoMoreString == true)
                {
                    string tMoreString = "";
                    string tMoreMark = "";

                    if (m_DeviceInfo.DeviceType == E_DeviceType.UserNeGroup)
                    {
                        tMoreString = m_DeviceInfo.MoreString;
                        tMoreMark = m_DeviceInfo.MoreMark;
                    }
                    else
                    {
                        // 모델리스트에 모델이 있는경우
                        if (AppGlobal.s_ModelInfoList.Contains(m_DeviceInfo.ModelID))
                        {
                            ModelInfo tModelInfo = AppGlobal.s_ModelInfoList[m_DeviceInfo.ModelID];

                            tMoreString = tModelInfo.MoreString;
                            tMoreMark = tModelInfo.MoreMark;
                        }
                    }

                    // More문자와 MoreMark가 있어야만 자동스크롤되도록 한다.
                    if (tMoreString != "" && tMoreMark != "")
                    {

                        if (aResult.ResultData.ToString().IndexOf(tMoreString) >= 0)
                        {
                            // MoreString
                            if (tMoreMark.IndexOf("${SPACE}") >= 0)
                            {
                                StringBuilder tSendString = new StringBuilder();

                                tSendString.Append("Sub Main \r\n");
                                tSendString.Append("\t" + Script.s_Send + " \" \"\r\n");
                                tSendString.Append("End Sub");
                                Script tScript = new Script(tSendString.ToString());
                                DispatchMessage(this, " ");
                                //RunScript(tScript);
                            }
                            else if (tMoreMark.IndexOf("${ENTER}") >= 0)
                            {
                                //System.Diagnostics.Debug.WriteLine(aResult.ResultData.ToString());
                                StringBuilder tSendString = new StringBuilder();

                                tSendString.Append("Sub Main \r\n");
                                tSendString.Append("\t" + Script.s_Send + " chr(13)\r\n");
                                tSendString.Append("End Sub");
                                Script tScript = new Script(tSendString.ToString());
                                DispatchMessage(this, "\r");
                                //RunScript(tScript);
                            }
                            else
                            {
                                //System.Diagnostics.Debug.WriteLine(aResult.ResultData.ToString());
                                StringBuilder tSendString = new StringBuilder();

                                tSendString.Append("Sub Main \r\n");
                                tSendString.Append("\t" + Script.s_Send + " \" \"&chr(13)\r\n");
                                tSendString.Append("End Sub");
                                Script tScript = new Script(tSendString.ToString());
                                DispatchMessage(this, " \r");
                                //RunScript(tScript);
                            }
                        }

                    }


                }
                
                m_ScriptManager.CheckWait(aResult.ResultData.ToString());
				// 2019-11-10 ???? (?? ???? ??? ???? ?? ?? ?? )
                if (AppGlobal.s_ClientOption.IsAutoSaveLog)
                {
                    
                    //if (aResult.ResultData.ToString().Length > 1)
                    //2016-01-20 서영응 자동 저장 기능 체크하는 부분 변경 (엔터를 체크하여 엔터값이 있을 경우에만 자동 저장)
                    int nEnterCheck = aResult.ResultData.ToString().IndexOf("\n");

                    if(nEnterCheck >= 0)
                    { 
                        FileWrite(aResult.ResultData.ToString());
                    }
                    
                }
                //FileWrite(aResult.ResultData.ToString());
                //dsConsole.Write(aResult.ResultData.ToString());
                //FileWrite(aResult.ResultData.ToString());
                //if (AppGlobal.s_IsAutoSaveLog)
                //{
                //    if (m_IsAutoLogSaver)
                //    {

                //        FileWrite("|&|" + lineCommendBuffer.Replace(m_Prompt, "") + "||");
                //        lineCommendBuffer = "";
                //        m_IsAutoLogSaver = false;
                //    }
                //    else
                //    {
                //        if ("\b \b".Equals(aResult.ResultData.ToString()) && lineCommendBuffer.Length > 0)
                //        {
                //            lineCommendBuffer = lineCommendBuffer.Substring(0, lineCommendBuffer.Length - 1);
                //        }
                //        else
                //        {
                //            if (aResult.ResultData.ToString().Length < 3)
                //            {
                //                lineCommendBuffer += aResult.ResultData.ToString();
                //            }
                //            else
                //            {
                //                FileWrite(aResult.ResultData.ToString());
                //            }
                //        }
                //    }
                //}
               }
        }
        /// <summary>
        /// 상태에 따른 아이콘을 변경 합니다.
        /// </summary>
        private void ChangeStatusIcon()
        {
            try
            {
                if (AppGlobal.s_IsProgramShutdown) return;

                switch (m_TerminalStatus)
                {
                    case E_TerminalStatus.Disconnected:
                        m_IsConnected = false;
                        if (m_DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET)
                        {
                            //if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online && m_DeviceInfo.IsRegistered)
                            //{
                            if (m_DaemonProcessRemoteObject != null && m_DaemonProcessRemoteObject.IsDaemonConnected)
                            {
                                lock (m_DaemonProcessRemoteObject.ConnectedSessionIDList)
                                {
                                    if (m_DaemonProcessRemoteObject.ConnectedSessionIDList.Contains(m_ConnectedSessionID))
                                    {
                                        m_DaemonProcessRemoteObject.ConnectedSessionIDList.Remove(m_ConnectedSessionID);
                                        if (m_DaemonProcessRemoteObject.ConnectedSessionIDList.Count == 0)
                                        {
                                            int tDaemonID = m_DaemonProcessRemoteObject.DaemonID;
                                            AppGlobal.s_DaemonProcessList.Remove(tDaemonID);
                                            m_DaemonProcessRemoteObject.Stop();
                                            m_DaemonProcessRemoteObject = null;
                                        }
                                        m_ConnectedSessionID = 0;
                                    }
                                    else
                                    {
                                        System.Diagnostics.Debug.WriteLine("zz");
                                    }
                                }
                            }
                        }
                        else if (m_DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
                        {
                            // 2013-03-06 - shinyn - SSH텔넷기능인 경우 분기처리 추가
                            //if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online && m_DeviceInfo.IsRegistered)
                            //{
                            if (m_DaemonProcessRemoteObject != null && m_DaemonProcessRemoteObject.IsDaemonConnected)
                            {
                                lock (m_DaemonProcessRemoteObject.ConnectedSessionIDList)
                                {
                                    if (m_DaemonProcessRemoteObject.ConnectedSessionIDList.Contains(m_ConnectedSessionID))
                                    {
                                        m_DaemonProcessRemoteObject.ConnectedSessionIDList.Remove(m_ConnectedSessionID);
                                        if (m_DaemonProcessRemoteObject.ConnectedSessionIDList.Count == 0)
                                        {
                                            int tDaemonID = m_DaemonProcessRemoteObject.DaemonID;
                                            AppGlobal.s_DaemonProcessList.Remove(tDaemonID);
                                            m_DaemonProcessRemoteObject.Stop();
                                            m_DaemonProcessRemoteObject = null;
                                        }
                                        m_ConnectedSessionID = 0;
                                    }
                                    else
                                    {
                                        System.Diagnostics.Debug.WriteLine("zz");
                                    }
                                }
                            }
                            //}
                        }
                        SaveDeviceLog("연결 종료 했습니다.");
                        if(ProgreBarHandlerEvent != null)
                            ProgreBarHandlerEvent("디바이스에 연결 종료 되었습니다.", eProgressItemType.Standard, true);

                        // 2014-10-14 - 신윤남 - 터미널창 닫을 경우 터미널리스트에서 삭제하도록 처리합니다.
                        if (AppGlobal.m_TerminalPanel != null)
                        {
                            AppGlobal.m_TerminalPanel.tEmulator_OnTerminalStatusChange(this, E_TerminalStatus.Disconnected);
                        }
                        

                        // 2014-07-03 - 신윤남 - 원터미널에서 연결끊기면, 종료하도록 수정
                        if (m_TerminalMode == E_TerminalMode.QuickClient &&
                            AppGlobal.s_ClientOption.IsUseTerminalClose == true)
                        {
                            if (this.Parent.GetType() == typeof(SuperTabControlPanel))
                            {
                            }
                            else if (this.Parent.GetType() == typeof(TerminalWindows))
                            {
                            }
                            else
                            {
                                //AppGlobal.m_TerminalPanel.tEmulator_OnTerminalStatusChange(this, E_TerminalStatus.Disconnected);
                                Process.GetCurrentProcess().Kill();
                                return;
                            }

                        }


                        if (Parent == null) return;
                        if (m_TerminalMode != E_TerminalMode.RACTClient) return;
                        if (this.Parent is SuperTabControlPanel)
                        {
                            if (AppGlobal.s_ClientOption.IsUseTerminalClose == true)
                            {
                                //SuperTabControlPanel tabPrent = ((SuperTabControlPanel)this.Parent);
                                // 2014-08-19 - 신윤남 - 종료 클릭시에는 상위 Parent를 종료하면 Client까지 종료되므로 종료되지 않도록 한다.
                                if (this.Tag != "TabItemClose")
                                {
                                    //AppGlobal.m_TerminalPanel.EmulatorList.RemoveAt(
                                    this.Parent.Dispose();

                                    //SuperTabControlPanel tabParent = ((SuperTabControlPanel)this.Parent);
                                    
                                    //AppGlobal.m_TerminalPanel.l

                                    //System.Console.WriteLine(((SuperTabControlPanel)this.Parent).Parent.GetType().ToString());
                                }
                                return;
                            }
                            else
                            {
                                ((SuperTabControlPanel)this.Parent).TabItem.Image = (Image)global::RACTClient.Properties.Resources.Disconnect;
                            }

                        }

                        if (this.Parent is TerminalWindows)
                        {
                            if (AppGlobal.s_ClientOption.IsUseTerminalClose == true)
                            {
                                if (this.Parent.GetType() == typeof(TerminalWindows))
                                {
                                    TerminalWindows winParent = ((TerminalWindows)this.Parent);
                                    winParent.Dispose();
                                }
                                //TerminalWindows winPrent = ((TerminalWindows)this.Parent);
                                //winPrent.Dispose();
                                return;
                            }
                            else
                            {
                                ((TerminalWindows)this.Parent).Icon = global::RACTClient.Properties.Resources.racs_32_disconnect;
                            }
                        }
                        break;
                    case E_TerminalStatus.Connection:
                        SaveDeviceLog("연결 했습니다.");
                        mnuStopScript.Enabled = false;
                        if (Parent == null) return;
                        if (m_TerminalMode != E_TerminalMode.RACTClient) return;
                        if (this.Parent is SuperTabControlPanel)
                        {
                            ((SuperTabControlPanel)this.Parent).TabItem.Image = (Image)global::RACTClient.Properties.Resources.Connect;
                        }
                        m_IsConnected = true;
                        break;
                    case E_TerminalStatus.RunScript:
                        SaveDeviceLog("스크립트 실행 합니다.");
                        mnuStopScript.Enabled = true;
                        if (Parent == null) return;
                        if (m_TerminalMode != E_TerminalMode.RACTClient) return;
                        if (this.Parent is SuperTabControlPanel)
                        {
                            ((SuperTabControlPanel)this.Parent).TabItem.Image = (Image)global::RACTClient.Properties.Resources.ScriptRun;
                        }
                        break;
                    case E_TerminalStatus.Recording:
                        SaveDeviceLog("스크립트 저장 합니다.");
                        if (Parent == null) return;
                        if (m_TerminalMode != E_TerminalMode.RACTClient) return;
                        if (this.Parent is SuperTabControlPanel)
                        {
                            ((SuperTabControlPanel)this.Parent).TabItem.Image = (Image)global::RACTClient.Properties.Resources.Recoding;
                        }
                        break;
                }
                if (OnTerminalStatusChange != null) OnTerminalStatusChange(this, m_TerminalStatus);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
        /*
        private Process GetParentProcess()
        {
            Process parentProcess = null;

            using (Process currentProcess = Process.GetCurrentProcess())
            {
                string filter = string.Format("ProcessId={0}", currentProcess.Id);
                SelectQuery query = new SelectQuery("Win32_Process", filter);

                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
                using (ManagementObjectCollection results = searcher.Get())
                {

                    if (results.Count > 0)
                    {
                        if (results.Count > 1)
                            throw new InvalidOperationException();
                        IEnumerator resultEnumerator = results.GetEnumerator();
                        bool fMoved = resultEnumerator.MoveNext();

                        using (ManagementObject wmiProcess = (ManagementObject)resultEnumerator.Current)
                        {
                            PropertyData parentProcessId = wmiProcess.Properties["ParentProcessId"];
                            uint pid = (uint)parentProcessId.Value;

                            parentProcess = Process.GetProcessById((int)pid);

                        }

                    }

                }

            }

            return parentProcess;

        }
        */

        private void SaveDeviceLog(string aLog)
        {

            if (m_DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(string.Concat("[Telnet] ", m_DeviceInfo.IPAddress, ":", m_DeviceInfo.TerminalConnectInfo.TelnetPort, " ", aLog));
            }
            else if (m_DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
            {
                // 2013-03-06 - shinyn - SSH텔넷기능인 경우 분기처리 추가
                AppGlobal.s_FileLogProcessor.PrintLog(string.Concat("[Telnet] ", m_DeviceInfo.IPAddress, ":", m_DeviceInfo.TerminalConnectInfo.TelnetPort, " ", aLog));
            }
            else
            {
                AppGlobal.s_FileLogProcessor.PrintLog(string.Concat("[Serial] ", m_DeviceInfo.TerminalConnectInfo.SerialConfig.PortName, " ", aLog));
            }
        }

        /// <summary>
        /// Caret을 표시 합니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            m_IsShowCaret = !m_IsShowCaret;
            this.Invalidate();
        }

        /// <summary>
        /// 접속 종료 합니다.
        /// </summary>
        public void Disconnect()
        {

            // 2015-04-16 - 신윤남 - Terminal로그를 삭제합니다.
            try
            {
                if (m_TerminalLog != null)
                {
                    m_TerminalLog.Delete();
                }

                m_TerminalLog = null;
            }
            catch (Exception ex)
            {
            }

            try
            {
                switch (TerminalStatus)
                {
                    case E_TerminalStatus.Recording:
                        if (!AppGlobal.s_IsProgramShutdown)
                        {
                            if (AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "스크립트 레코딩을 취소 하시겠습니까?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            {
                                return;
                            }
                        }
                        break;
                    case E_TerminalStatus.RunScript:
                        if (!AppGlobal.s_IsProgramShutdown)
                        {
                            if (AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "실행중인 스크립트 취소 하시겠습니까?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                            {
                                return;
                            }
                        }
                        break;
                }



                if (m_DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET)
                {
                    if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online )//&& m_DeviceInfo.IsRegistered) 모든 장비를 데몬을 통한 통신으로 변경, 등록된 장비 여부 체크 제외
                    {
                        DisconnectDaemonTelnetSession();
                    }
                    else
                    {
                        DisconnectLocalTelnetSession();
                    }
                }
                else if (m_DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
                {
                    // 2013-03-06 - shinyn - SSH텔넷기능인 경우 분기처리 추가
                    if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online )//&& m_DeviceInfo.IsRegistered)
                    {
                        DisconnectDaemonTelnetSession();
                    }
                    else
                    {
                        DisconnectLocalTelnetSession();
                    }
                }
                else
                {
                    AppGlobal.s_SerialProcessor.DisconnectDevice(this);
                    TerminalStatus = E_TerminalStatus.Disconnected;
                }

                if (m_ScriptManager != null)
                {
                    m_ScriptManager.Stop();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
        /// <summary>
        /// 로컬 텔넷 프로세서를 종료 합니다.
        /// </summary>
        private void DisconnectLocalTelnetSession()
        {
            RequestCommunicationData tRequestData = null;
            TelnetCommandInfo tCommandInfo = new TelnetCommandInfo();
            tCommandInfo.Sender = this;
            tCommandInfo.SessionID = m_ConnectedSessionID;
            tCommandInfo.DeviceInfo = m_DeviceInfo;
            tCommandInfo.WorkTyp = E_TelnetWorkType.Disconnect;
            tRequestData = new RequestCommunicationData();
            tRequestData.CommType = E_CommunicationType.RequestCommandProcess;
            tRequestData.RequestData = tCommandInfo;
            // TerminalStatus = E_TerminalStatus.TryConnection;

            AppGlobal.s_TelnetProcessor.ExecuteCommand(tRequestData);
        }
        /// <summary>
        /// 데몬에 연결된 세션을 종료 합니다.
        /// </summary>
        private void DisconnectDaemonTelnetSession()
        {
            if (m_DaemonProcessRemoteObject == null) return;
            if (!m_DaemonProcessRemoteObject.IsDaemonConnected)
            {
                TerminalStatus = E_TerminalStatus.Disconnected;
                return;
            }


            m_DaemonProcessRemoteObject.DisconnectFromDaemon(m_ConnectedSessionID);

            //RequestCommunicationData tRequestData = null;
            //TelnetCommandInfo tCommandInfo = new TelnetCommandInfo();
            //tCommandInfo.DeviceInfo = m_DeviceInfo;
            //tCommandInfo.SessionID = m_ConnectedSessionID;
            //tCommandInfo.UserID = AppGlobal.s_LoginResult.UserID;
            //tCommandInfo.WorkTyp = E_TelnetWorkType.Disconnect;
            //tRequestData = AppGlobal.MakeDefaultRequestData();
            //tRequestData.CommType = E_CommunicationType.RequestBatchRegisteration;
            //tRequestData.RequestData = tCommandInfo;

            //m_Result = null;
            //m_MRE.Reset();
            //m_DaemonProcessRemoteObject.SendDaemonRequestData(this, tRequestData);


            TerminalStatus = E_TerminalStatus.Disconnected;
            m_IsConnected = false;

        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MCTerminalEmulator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(10, 10);
            this.AutoSize = true;
            this.Name = "MCTerminalEmulator";
            this.ResumeLayout(false);

        }


        /// <summary>
        /// 옵션 정보를 적용 합니다.
        /// </summary>
        public void ApplyOption()
        {
			// 2019-11-10 ???? (?? ?? ?? ?? ?? ??)
            if (m_DeviceInfo.DevicePartCode == 1 || /* 집선스위치 */
                m_DeviceInfo.DevicePartCode == 6 || /* G-PON-OLT */
                m_DeviceInfo.DevicePartCode == 31 /* NG-PON-OLT */ )
            {
                m_FGColor = AppGlobal.s_ClientOption.HighlightFontColor;
                this.BackColor = AppGlobal.s_ClientOption.HighlightBackGroundColor;
                string tTempFont = AppGlobal.s_ClientOption.HighlightFontName;
                if (tTempFont.Equals("굴림")
                    || tTempFont.Equals("돋움")
                    || tTempFont.Equals("궁서")
                    || tTempFont.Equals("바탕"))
                {
                    tTempFont += "체";
                }

                this.Font = new Font(tTempFont, AppGlobal.s_ClientOption.HighlightFontSize, AppGlobal.s_ClientOption.HighlightFontStyle, GraphicsUnit.Point, ((byte)(0)));
            }
            else
            {
                m_FGColor = AppGlobal.s_ClientOption.TerminalFontColor;
                this.BackColor = AppGlobal.s_ClientOption.TerminalBackGroundColor;
                string tTempFont = AppGlobal.s_ClientOption.TerminalFontName;
                if (tTempFont.Equals("굴림")
                    || tTempFont.Equals("돋움")
                    || tTempFont.Equals("궁서")
                    || tTempFont.Equals("바탕"))
                {
                    tTempFont += "체";
                }

                this.Font = new Font(tTempFont, AppGlobal.s_ClientOption.TerminalFontSize, AppGlobal.s_ClientOption.TerminalFontStyle, GraphicsUnit.Point, ((byte)(0)));
            }
            //2014-08-19 - 신윤남 -  null인경우에는 FontInfo를 가져오면 오류 발생하여 없앤다.
            if (this.components != null)
            {
                GetFontInfo();
            }


            this.Refresh();
        }

        public void ExtApplyOption()
        {
            ApplyOption();
        }


        /// <summary>
        /// 스크립트 작업을 처리 합니다. 
        /// </summary>
        /// <param name="aScriptWorkType"></param>
        public void ScriptWork(E_ScriptWorkType aScriptWorkType, object aScript)
        {
             // Overload for ITerminal interface compatibility
             ScriptWork(aScriptWorkType);
        }

        internal void ScriptWork(E_ScriptWorkType aScriptWorkType)
        {
            switch (aScriptWorkType)
            {
                case E_ScriptWorkType.Rec:
                    // ((SuperTabItem)this.Parent.Parent).Image = (Image)global::RACTClient.Properties.Resources.Recoding;
                    TerminalStatus = E_TerminalStatus.Recording;
                    m_ScriptGenerator.Reset();
                    break;

                case E_ScriptWorkType.RecCancel:
                    // ((SuperTabItem)this.Parent.Parent).Image = (Image)global::RACTClient.Properties.Resources.Connect;
                    TerminalStatus = E_TerminalStatus.Connection;
                    m_ScriptGenerator.Reset();
                    break;

                case E_ScriptWorkType.Save:
                    // ((SuperTabItem)this.Parent.Parent).Image = (Image)global::RACTClient.Properties.Resources.Connect;
                    TerminalStatus = E_TerminalStatus.Connection;

                    Script tScript = new Script();
                    tScript.RawScript = m_ScriptGenerator.MakeScript();
                    ModifyScript tSave = new ModifyScript(E_WorkType.Add, tScript);
                    tSave.InitializeControl();
                    tSave.ShowDialog(AppGlobal.s_ClientMainForm);
                    break;
                case E_ScriptWorkType.RunCancel:
                    // ((SuperTabItem)this.Parent.Parent).Image = (Image)global::RACTClient.Properties.Resources.Connect;
                    TerminalStatus = E_TerminalStatus.Connection;
                    m_ScriptManager.Stop();
                    break;
                case E_ScriptWorkType.RecLog:
                    // ((SuperTabItem)this.Parent.Parent).Image = (Image)global::RACTClient.Properties.Resources.Connect;
                    TerminalStatus = E_TerminalStatus.RecLog;
                    break;
            }

            //ChangeStatusIcon();

        }
        /// <summary>
        /// 프롬프트 입니다.
        /// </summary>
        private string m_Prompt = "";
        /// <summary>
        /// 명령을 저장 합니다.
        /// </summary>
        private void SaveCommandLog(bool isLimitCmd)
        {
            if (m_Prompt.Length == 0) return;
            string tTempString = "";
            char tCurChar;
            for (int x = 0; x < this.m_Cols; x++)
            {
                tCurChar = this.m_CharGrid[this.m_Caret.Pos.Y][x];
                if (tCurChar == '\0')
                {
                    continue;
                }
                tTempString = tTempString + Convert.ToString(tCurChar);
            }
            if (tTempString.IndexOf(m_Prompt) < 0) return;
            tTempString = tTempString.Replace(m_Prompt, "");
            if (tTempString.Length == 0) return;

            m_CommandLogInfo = new DBExecuteCommandLogInfo();
            m_CommandLogInfo.DeviceInfo = m_DeviceInfo;
            m_CommandLogInfo.Command = tTempString;
            m_CommandLogInfo.IsLimitCmd = isLimitCmd;
            m_CommandLogInfo.ConnectionLogID = m_ConnectedSessionID;
            m_CommandLogInfo.LogType = E_DBLogType.ExecuteCommandLog;

            AppGlobal.s_TerminalExecuteLogProcess.AddTerminalExecuteLog(m_CommandLogInfo);
        }

        /// <summary>
        /// 붙여넣기한 명령을 저장 합니다.
        /// </summary>
        private void SavePasteCommandLog(bool isLimitCmd, String Cmd)
        {
            m_CommandLogInfo = new DBExecuteCommandLogInfo();
            m_CommandLogInfo.DeviceInfo = m_DeviceInfo;
            m_CommandLogInfo.Command = Cmd;
            m_CommandLogInfo.IsLimitCmd = isLimitCmd;
            m_CommandLogInfo.ConnectionLogID = m_ConnectedSessionID;
            m_CommandLogInfo.LogType = E_DBLogType.ExecuteCommandLog;

            AppGlobal.s_TerminalExecuteLogProcess.AddTerminalExecuteLog(m_CommandLogInfo);
        }

        /// <summary>
        /// 로그 정보 입니다.
        /// </summary>
        DBExecuteCommandLogInfo m_CommandLogInfo;
        internal void CheckPrompt()
        {
            
            if (m_IsCheckPrompt) return;

            string tTempString = "";
            char tCurChar;
            for (int x = 0; x < this.m_Cols; x++)
            {
                tCurChar = this.m_CharGrid[this.m_Caret.Pos.Y][x];
                if (tCurChar == '\0')
                {
                    continue;
                }
                tTempString = tTempString + Convert.ToString(tCurChar);
            }
          
            if (tTempString.Length == 0) return;
            

            //2016-04-01 서영응 명령 프롬프트에 스페이스가 없는 경우에 정상 동작이 안되어서 스페이스를 강제로 넣어주는 부분 제거
            //m_Prompt = tTempString.TrimEnd()+" ";
            m_Prompt = tTempString.TrimEnd();

            m_IsCheckPrompt = true;


        }
        /// <summary>
        /// 수신 대기 스크립트를 저장 합니다.
        /// </summary>
        internal void SaveWaitScript()
        {
            if (m_IsSaveWaitScript) return;

            string tTempString = "";
            char tCurChar;
            for (int x = 0; x < this.m_Cols; x++)
            {
                tCurChar = this.m_CharGrid[this.m_Caret.Pos.Y][x];
                if (tCurChar == '\0')
                {
                    continue;
                }
                tTempString = tTempString + Convert.ToString(tCurChar);
            }
            m_ScriptGenerator.AddWait(new TerminalScriptKeyInfo(tTempString, E_TerminalScriptKeyType.Normal));

            for (int i = 0; i < m_MoreStringList.Length; i++)
            {
                if (m_MoreStringList[i].Equals(tTempString))
                {
                    System.Diagnostics.Debug.WriteLine("zz");
                }
            }

            m_IsSaveWaitScript = true;
        }
        /// <summary>
        /// More 문자열 입니다.
        /// </summary>
        private string[] m_MoreStringList = new string[] { "--More--", "--more--", "(q)uit", "-- more --", "-- More --" };



        /// <summary>
        /// 스크립트를 실행 합니다.
        /// </summary>
        /// <param name="aScript"></param>
        public void RunScript(Script aScript)
        {
            if (m_TerminalStatus != E_TerminalStatus.Connection)
            {
                if (IsLimitCmd(aScript.RawScript))
                {
                    return;
                }
            }

            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument1<Script>(RunScript), aScript);
                return;
            }
            if (m_TerminalStatus == E_TerminalStatus.RunScript)
            {
                if (AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "현재 '" + m_ScriptManager.Script.Name + "' 스크립트가 실행 중입니다.\n강제 종료 후 실행하시겠습니까?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
            }

            if (m_TerminalStatus == E_TerminalStatus.Recording)
            {
                if (AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "현재 스크립트 기록 실행 중입니다.\n강제 종료 후 실행하시겠습니까?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
                m_ScriptGenerator.Reset();
            }

            m_ScriptManager.Stop();
            m_ScriptManager.Script = aScript;
            TerminalStatus = E_TerminalStatus.RunScript;
            m_ScriptManager.Run();
        }

        //20170818 - NoSeungPil - RCCS 로그인의 경우 종료시 강제로 ctrl + d 전송
        /// <summary>
        /// 스크립트를 실행 합니다.
        /// </summary>
        /// <param name="aScript"></param>
        public void RunScriptRCCS(Script aScript)
        {
            if (m_TerminalStatus != E_TerminalStatus.Connection)
            {
                if (IsLimitCmd(aScript.RawScript))
                {
                    return;
                }
            }

            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument1<Script>(RunScript), aScript);
                return;
            }
            if (m_TerminalStatus == E_TerminalStatus.RunScript)
            {
                if (AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "현재 '" + m_ScriptManager.Script.Name + "' 스크립트가 실행 중입니다.\n강제 종료 후 실행하시겠습니까?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
            }

            if (m_TerminalStatus == E_TerminalStatus.Recording)
            {
                if (AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "현재 스크립트 기록 실행 중입니다.\n강제 종료 후 실행하시겠습니까?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
                m_ScriptGenerator.Reset();
            }

            m_ScriptManager.Stop();
            m_ScriptManager.Script = aScript;
            TerminalStatus = E_TerminalStatus.RunScript;
            m_ScriptManager.Run();
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
        /// <summary>
        /// 자동 로그인 명령세트 입니다.
        /// </summary>
        private FACT_DefaultConnectionCommandSet m_ConnectionCommandSet;
        /// <summary>
        /// 자동 로그인 명령세트를 가져오기 합니다.
        /// </summary>
        public FACT_DefaultConnectionCommandSet ConnectioncommandSet
        {
            get { return m_ConnectionCommandSet; }
        }

        /// <summary>
        /// 로그인 중인지 파악
        /// </summary>
        private bool m_NowLogin;

        /// <summary>
        /// 로그인 중인지 파악
        /// </summary>
        public bool NowLogin
        {
            get { return m_NowLogin; }
            set { m_NowLogin = value; }
        }


        /// <summary>
        /// 자동 로그인 처리 시작 합니다.
        /// </summary>
        private void StartLoginProcess()
        {
            try
            {
                if (m_DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET)
                {
                    if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online && m_DeviceInfo.IsRegistered && m_ConnectionCommandSet == null) //자동 로그인 동작에서는 등록여부 유지
                    {
                        AppGlobal.s_FileLogProcessor.PrintLog("기본 접속 정보를 로드합니다.");

                        RequestCommunicationData tRequestData = null;

                        tRequestData = AppGlobal.MakeDefaultRequestData();
                        tRequestData.CommType = E_CommunicationType.RequestDefaultConnectionCommand;
                        //2013-05-02- shinyn - 수동장비인 경우 기본접속 정보는 DeviceInfo에 있으므로 DeviceInfo를 보내고 기본접속 정보를 로드한다.
                        //tRequestData.RequestData = m_DeviceInfo.DeviceID;
                        tRequestData.RequestData = m_DeviceInfo;

                        //2015-09-18 hanjiyeon 추가 - 1023 port 접속 시 기본명령어 로드 코드 수정.
                        if (m_DeviceInfo.TerminalConnectInfo.TelnetPort == 1023)
                        {
                            tRequestData.UserData = "TL1";
                        }

                        m_Result = null;
                        m_MRE.Reset();

                        AppGlobal.SendRequestData(this, tRequestData);
                        m_MRE.WaitOne(AppGlobal.s_RequestTimeOut);

                        if (m_Result == null || m_Result.Error.Error != E_ErrorType.NoError)
                        {
                            // 2013-05-02 - shinyn - 기본접속 명령 로드 실패시 장비 아이피 로그에 저장
                            //AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "기본 접속 명령 로드에 실패 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            //TerminalStatus = E_TerminalStatus.Disconnected;
                            //AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "기본 접속 명령 정보 로드에 실패 했습니다.");
                            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "StartLoginProcess : 기본 접속 명령 정보 로드에 실패 했습니다. IP : " + m_DeviceInfo.IPAddress);
                            TerminalStatus = E_TerminalStatus.Disconnected;
                            return;
                        }

                        m_ConnectionCommandSet = m_Result.ResultData as FACT_DefaultConnectionCommandSet;


                        // 2013-03-07 - shinyn - 수동장비등록인 경우 기본접속 명령은 자체적으로 만들어서 스크립트 실행하도록 한다.


                        if (m_ConnectionCommandSet == null && m_ConnectionCommandSet.CommandList.Count == 0)
                        {
                            // 2013-05-02 - shinyn - 기본접속 명령 로드 실패시 장비 아이피 로그에 저장
                            // AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "기본 접속 명령 정보 로드에 실패 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            // AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "기본 접속 명령 정보 로드에 실패 했습니다.");
                            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "StartLoginProcess : 기본 접속 명령 정보 로드에 실패 했습니다. IP : " + m_DeviceInfo.IPAddress);
                        }
                        else
                        {
                            for (int i = 0; i < m_ConnectionCommandSet.CommandList.Count; i++)
                            {
                                m_ConnectionCommandSet.CommandList[i].ErrorString = "Login incorrect";
                            }


                            Script tLoginCommandScript = null;

                            //2015-09-18 hanjiyeon 추가 - 1023 port 접속 시 접속 메소드 추가 및 분기 처리.
                            if (m_DeviceInfo.TerminalConnectInfo.TelnetPort == 1023)
                            {
                                tLoginCommandScript = ScriptGenerator.MakeDefaultConnectionCommand_TL1(m_ConnectionCommandSet, m_DeviceInfo);
                            }
                            else
                            {
                                tLoginCommandScript = ScriptGenerator.MakeDefaultConnectionCommand(m_ConnectionCommandSet, m_DeviceInfo);
                            }
                            tLoginCommandScript.ScriptType = E_ScriptType.WaitScript;
                            RunScript(tLoginCommandScript);
                        }
                    }
                }
                else if (m_DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
                {
                    // 2013-03-06 - shinyn - SSH텔넷기능인 경우 분기처리 추가
                    if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online && m_DeviceInfo.IsRegistered && m_ConnectionCommandSet == null) //자동 로그인 동작에서는 등록여부 유지
                    {
                        AppGlobal.s_FileLogProcessor.PrintLog("기본 접속 정보를 로드합니다.");

                        RequestCommunicationData tRequestData = null;

                        tRequestData = AppGlobal.MakeDefaultRequestData();
                        tRequestData.CommType = E_CommunicationType.RequestDefaultConnectionCommand;
                        //2013-05-02- shinyn - 수동장비인 경우 기본접속 정보는 DeviceInfo에 있으므로 DeviceInfo를 보내고 기본접속 정보를 로드한다.
                        //tRequestData.RequestData = m_DeviceInfo.DeviceID;
                        tRequestData.RequestData = m_DeviceInfo;
                        m_Result = null;
                        m_MRE.Reset();

                        AppGlobal.SendRequestData(this, tRequestData);
                        m_MRE.WaitOne(AppGlobal.s_RequestTimeOut);

                        if (m_Result == null || m_Result.Error.Error != E_ErrorType.NoError)
                        {
                            //AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "기본 접속 명령 로드에 실패 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "기본 접속 명령 정보 로드에 실패 했습니다.");
                            TerminalStatus = E_TerminalStatus.Disconnected;
                            return;
                        }

                        m_ConnectionCommandSet = m_Result.ResultData as FACT_DefaultConnectionCommandSet;

                        if (m_ConnectionCommandSet == null && m_ConnectionCommandSet.CommandList.Count == 0)
                        {
                            //AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "StartLoginProcess : 기본 접속 명령 정보 로드에 실패 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "StartLoginProcess : 기본 접속 명령 정보 로드에 실패 했습니다.");
                        }
                        else
                        {
                            for (int i = 0; i < m_ConnectionCommandSet.CommandList.Count; i++)
                            {
                                m_ConnectionCommandSet.CommandList[i].ErrorString = "Login incorrect";
                            }
                            Script tLoginCommandScript = ScriptGenerator.MakeDefaultConnectionCommand(m_ConnectionCommandSet, m_DeviceInfo);
                            tLoginCommandScript.ScriptType = E_ScriptType.WaitScript;
                            //2013-03-06 - shinyn - SSH텔넷기능인 경우 로그인 스크립트는 실행하지 않도록한다.
                            //RunScript(tLoginCommandScript);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                AppGlobal.s_FileLogProcessor.PrintLog("기본 접속 정보를 로드 할 수 없습니다. 정보가 없거나 일시적인 실패 입니다.");
            }
            
        }


        /// <summary>
        /// 사용중인 Comport를 가져오기 합니다.
        /// </summary>
        public string ComPort
        {
            get { return m_DeviceInfo.TerminalConnectInfo.SerialConfig.PortName; }
        }
        /// <summary>
        /// 시리얼 결과를 처리 합니다.
        /// </summary>
        /// <param name="aResult"></param>
        public void DisplayResult(SerialCommandResultInfo aResult)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument1<SerialCommandResultInfo>(DisplayResult), aResult);
                return;
            }
            DisplayScrollLast(m_ScrollbackBuffer.Count - (m_Rows - 1));
            OnReceivedData(aResult.ReceivedData.ToString());
            m_ScriptManager.CheckWait(aResult.ReceivedData.ToString());
            m_ConnectedSessionID = aResult.SessionID;
            if (TerminalStatus == E_TerminalStatus.TryConnection)
            {
                TerminalStatus = E_TerminalStatus.Connection;
                m_IsConnected = true;
            }

        }

        /// <summary>
        /// Serial Config 가져오거나 설정 합니다.
        /// </summary>
        public SerialConfig SerialConfig
        {
            get { return m_DeviceInfo.TerminalConnectInfo.SerialConfig; }
            set { m_DeviceInfo.TerminalConnectInfo.SerialConfig = value; }
        }


        /// <summary>
        /// 터미널 연결 타입 가져오거나 설정 합니다.
        /// </summary>
        public E_ConnectionProtocol ConnectionProtocolType
        {
            get { return m_ConnectionProtocolType; }
            set { m_ConnectionProtocolType = value; }
        }

        /// <summary>
        /// 결과를 처리 합니다.
        /// </summary>
        /// <param name="aSessionID"></param>
        /// <param name="aResult"></param>
        public void DisplayResult(int aSessionID, string aResult)
        {
            m_ConnectedSessionID = aSessionID;
            if (TerminalStatus == E_TerminalStatus.TryConnection)
            {
                TerminalStatus = E_TerminalStatus.Connection;
				// 2019-11-10 ???? (OneTerminal ??? ?? ??UI ??)
                if (ProgreBarHandlerEvent != null)
                    ProgreBarHandlerEvent("디바이스에 연결 되었습니다.", eProgressItemType.Standard, false);
                m_IsConnected = true;
            }

            OnReceivedData(aResult);
            m_ScriptManager.CheckWait(aResult);
        }
        /// <summary>
        /// 장비 접속 여부 가져오거나 설정 합니다.
        /// </summary>
        public bool IsConnected
        {
            get { return m_IsConnected; }
            set { m_IsConnected = value; }
        }

        #region ITelnetEmulator 멤버

        public void DisplayResult(TelnetCommandResultInfo aResult)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument1<TelnetCommandResultInfo>(DisplayResult), aResult);
                return;
            }
            m_ConnectedSessionID = aResult.SessionID;
            DisplayScrollLast(m_ScrollbackBuffer.Count - (m_Rows - 1));
            if (aResult.ReslutType == E_TelnetReslutType.DisConnected)
            {
                TerminalStatus = E_TerminalStatus.Disconnected;
                m_IsConnected = false;
            }
            else
            {
                if (TerminalStatus == E_TerminalStatus.TryConnection)
                {
                    TerminalStatus = E_TerminalStatus.Connection;
					// 2019-11-10 ???? (OneTerminal ??? ?? ??UI ??)
                    if (ProgreBarHandlerEvent!= null)
                            ProgreBarHandlerEvent("디바이스에 연결 되었습니다.", eProgressItemType.Standard, false);
                    m_IsConnected = true;
                }

                OnReceivedData(aResult.ResultData.ToString());

                m_ScriptManager.CheckWait(aResult.ResultData.ToString());
            }
        }

        #endregion
        /// <summary>
        /// 클라이언트 모드 변경을 처리 합니다.
        /// </summary>
        public void ChangeClientMode()
        {
            if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Console)
            {
                if (m_ConnectionType == ConnectionTypes.RemoteTelnet)
                {
                    DisconnectDaemonTelnetSession();
                    TerminalStatus = E_TerminalStatus.Disconnected;
                }
            }
        }

        /// <summary>
        /// 편집 처리 합니다.
        /// </summary>
        /// <param name="aEditType"></param>
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
        /// <summary>
        /// 찾기창 닫기 처리합니다.
        /// </summary>
        public void FindForm_Close()
        {
            m_LastFindRow = 0;
            m_LastFindCol = 0;
        }



        /// <summary>
        /// ToolTip 속성을 가져오거나 설정합니다.
        /// </summary>
        public string ToolTip
        {
            get
            {
                string tToolTip = "";
                if (m_DeviceInfo.TpoName.Length == 0)
                {
                    if (m_DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET)
                    {
                        return m_DeviceInfo.IPAddress.Trim();
                    }
                    else if (m_DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
                    {
                        // 2013-03-06 - shinyn - SSH텔넷인경우 분기처리 추가
                        return m_DeviceInfo.IPAddress.Trim();
                    }
                    else
                    {
                        return "Serial-" + m_DeviceInfo.TerminalConnectInfo.SerialConfig.PortName;
                    }
                }
                else
                {
                    if (AppGlobal.s_ClientOption.TerminalDisplayNameType == E_TerminalDisplayNameType.IPAddress)
                    {
                        return "[" + m_DeviceInfo.Name.Trim() + "]";
                    }
                    else
                    {
                        return "[" + m_DeviceInfo.IPAddress.Trim() + "]";
                    }

                }
            }
        }

        public void WriteTerminalLog()
        {
            try
            {
                SaveFileDialog tOpenDialog = new SaveFileDialog();
                tOpenDialog.DefaultExt = "tacts";
                tOpenDialog.Filter = "TACT Log Files (*.log)|*.log|All Files (*.*)|*.*";

                if (tOpenDialog.ShowDialog(AppGlobal.s_ClientMainForm) == DialogResult.OK)
                {

                    // 2015-04-16 - 신윤남 - 저장된 터미널 로그를 저장합니다.
                    string tString = "";

                    if (m_TerminalLog != null)
                    {
                        File.AppendAllText(tOpenDialog.FileName, m_TerminalLog.GetAllText());
                    }
                    else
                    {
                        
                        File.AppendAllText(tOpenDialog.FileName, "");
                        foreach (string tTerminalLog in m_ScrollbackBuffer)
                        {

                            File.AppendAllText(tOpenDialog.FileName, tTerminalLog + Environment.NewLine);
                        }


                        if (!this.m_Caret.IsOff)
                        {
                            m_TextAtCursor = "";
                            char tCurrentChar;
                            for (int i = 0; i < this.m_Cols; i++)
                            {
                                tCurrentChar = this.m_CharGrid[this.m_Caret.Pos.Y][i];
                                if (tCurrentChar == '\0')
                                {
                                    continue;
                                }
                                m_TextAtCursor = m_TextAtCursor + Convert.ToString(tCurrentChar);
                            }
                        }

                        File.AppendAllText(tOpenDialog.FileName, m_TextAtCursor + Environment.NewLine);
                    }

                    AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "화면을 파일을 저장 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    /*
                    string tString = "";
                    File.AppendAllText(tOpenDialog.FileName, "");
                    foreach (string tTerminalLog in m_ScrollbackBuffer)
                    {

                        File.AppendAllText(tOpenDialog.FileName, tTerminalLog + Environment.NewLine);
                    }


                    if (!this.m_Caret.IsOff)
                    {
                        m_TextAtCursor = "";
                        char tCurrentChar;
                        for (int i = 0; i < this.m_Cols; i++)
                        {
                            tCurrentChar = this.m_CharGrid[this.m_Caret.Pos.Y][i];
                            if (tCurrentChar == '\0')
                            {
                                continue;
                            }
                            m_TextAtCursor = m_TextAtCursor + Convert.ToString(tCurrentChar);
                        }
                    }

                    File.AppendAllText(tOpenDialog.FileName, m_TextAtCursor + Environment.NewLine);
                    AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "화면을 파일을 저장 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    */
                }
                
            }
            catch
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "화면을 파일 저장 실패 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 터미널 실행 모드 속성을 가져오거나 설정합니다.
        /// </summary>
        public E_TerminalMode TerminalMode
        {
            get { return m_TerminalMode; }
            set { m_TerminalMode = value; }
        }

        void mnuCopy_Click_Event(object sender, EventArgs e)
        {
          
            if (TerminalMode == E_TerminalMode.QuickClient)
                mnuCopy_Click(sender, e);
            else
                AppGlobal.terminalPanel1.ExecTerminalScreen(E_TerminalScreenTextEditType.Copy);
             
            
        }

        void mnuPaste_Click_Event(object sender, EventArgs e)
        {
            if (TerminalMode == E_TerminalMode.QuickClient)
                mnuPaste_Click(sender, e);
            else
                AppGlobal.terminalPanel1.ExecTerminalScreen(E_TerminalScreenTextEditType.Paste);
        }

        void mnuPasteCR_Click_Event(object sender, EventArgs e)
        {
            if (TerminalMode == E_TerminalMode.QuickClient)
                mnuPasteCR_Click(sender, e);
            else
                AppGlobal.terminalPanel1.ExecTerminalScreen(E_TerminalScreenTextEditType.PasteCR);
        }

        void mnuAutoC_Click_Event(object sender, EventArgs e)
        {
            if (TerminalMode == E_TerminalMode.QuickClient)
                mnuAutoC_Click(sender, e);
            else
                AppGlobal.terminalPanel1.ExecTerminalScreen(E_TerminalScreenTextEditType.AutoC);
        }

        void mnuFind_Click_Event(object sender, EventArgs e)
        {
            if (TerminalMode == E_TerminalMode.QuickClient)
                mnuFind_Click(sender, e);
            else
                AppGlobal.terminalPanel1.ExecTerminalScreen(E_TerminalScreenTextEditType.Find);
        }

        void mnuSelectAll_Click_Event(object sender, EventArgs e)
        {
            if (TerminalMode == E_TerminalMode.QuickClient)
                mnuSelectAll_Click(sender, e);
            else
                AppGlobal.terminalPanel1.ExecTerminalScreen(E_TerminalScreenTextEditType.SelectAll);
        }

        void mnuClear_Click_Event(object sender, EventArgs e)
        {
            if (TerminalMode == E_TerminalMode.QuickClient)
                mnuClear_Click(sender, e);
            else
                AppGlobal.terminalPanel1.ExecTerminalScreen(E_TerminalScreenTextEditType.Clear);
        }
		// 2019-11-10 ???? (??? ??? ??? ?? ??_???  )
        void mnuCmdClear_Click_Event(object sender, EventArgs e)
        {
            if (TerminalMode == E_TerminalMode.QuickClient)
                mnuCmdClear_Click(sender, e);
            else
                AppGlobal.terminalPanel1.ExecTerminalScreen(E_TerminalScreenTextEditType.CmdClear);
           }

        void mnuSearchDefaultCmd_Click_Event(object sender, EventArgs e)
        {
            if (TerminalMode == E_TerminalMode.QuickClient)
                mnuSearchDefaultCmd_Click(sender, e);
            else
                AppGlobal.terminalPanel1.ExecTerminalScreen(E_TerminalScreenTextEditType.SearchCmd);
        }

        /// <summary>
        /// 두 int값을 서로의 값으로 변경합니다.
        /// </summary>
        /// <param name="mouseY"></param>
        public void _Swap(ref int a, ref int b)
        {
            var temp = a;
            a = b;
            b = temp;
        }

        /// <summary>
        /// 마우스 Y위치에 해당하는 m_ScrollbackBuffer의 index를 계산한다. 
        /// </summary>
        /// <param name="mouseY"></param>
        int _MousePointToRow(int mouseY)
        {
            int nRows = mouseY / m_CharSize.Height;

            if (m_VertScrollBar.Minimum == 0 && m_VertScrollBar.Maximum > 0)
            {
                nRows += m_VertScrollBar.Value;
            }

            // Boundary 유효성 체크
            if (nRows < 0) nRows = 0;
            else if (nRows > m_ScrollbackBuffer.Count - 1) nRows = m_ScrollbackBuffer.Count - 1;

            return nRows;
        }

        /// <summary>
        /// 선택영역 표시(Inverse)값 설정
        /// </summary>
        protected void UpdateAttribGridInverse()
        {
            // 초기화
            for (int iRow = 0; iRow < m_AttribGrid.Length; iRow++)
            {
                if (m_AttribGrid[iRow] == null) continue;
                for (int iCol = 0; iCol < m_AttribGrid[iRow].Length; iCol++)
                {
                    m_AttribGrid[iRow][iCol].IsInverse = false;
                }
            }

            // 선택영역이 있으면
            if (!IsSelectMode()) return;

            int tBegRow = m_BeginRow, tEndRow = m_EndRow;
            int tBegCol = m_BeginCol, tEndCol = m_EndCol;

            if (tBegRow > tEndRow)
            {
                _Swap(ref tBegRow, ref tEndRow);
                _Swap(ref tBegCol, ref tEndCol);
            }
            else if (tBegRow == tEndRow)
            {
                if (tBegCol > tEndCol)
                    _Swap(ref tBegCol, ref tEndCol);
            }

            // OutOfIndexBoundary 예외 경우처리
            if (tBegRow < 0) tBegRow = 0;
            if (tEndRow > m_ScrollbackBuffer.Count - 1) tEndRow = m_ScrollbackBuffer.Count - 1;

            bool isInverse = false;
            for (int iRow = 0; iRow < m_AttribGrid.Length; iRow++)
            {
                if (m_AttribGrid[iRow] == null) continue;

                int iCurRow = iRow + (m_VertScrollBar.Maximum > 0 ? m_VertScrollBar.Value : 0);
                if (iCurRow < tBegRow || iCurRow > tEndRow) continue;

                // value range
                if (iCurRow < 0) iCurRow = 0;
                else if (iCurRow > m_ScrollbackBuffer.Count - 1) iCurRow = m_ScrollbackBuffer.Count - 1;

                for (int iCol = 0; iCol < m_AttribGrid[iRow].Length; iCol++)
                {
                    isInverse = false;

                    if (tBegRow == tEndRow)
                    {
                        if (iCol >= tBegCol && iCol <= tEndCol)
                            isInverse = true;
                    }
                    else
                    {
                        if (iCurRow == tBegRow)
                            isInverse = (iCol >= tBegCol);
                        else if (iCurRow == tEndRow)
                            isInverse = (iCol <= tEndCol);
                        else
                            isInverse = true;
                    }

                    m_AttribGrid[iRow][iCol].IsInverse = isInverse;
                } // End of for (iCol)
            } // End of for (iRow)
        }

        /// <summary>
        /// 선택영역 취소(초기화)
        /// </summary>
        protected void Deselect()
        {
            if (!IsSelectMode()) return;

            m_BeginRow = m_EndRow = -1;
            m_BeginCol = m_EndCol = -1;

            if (m_CopyValue.Length > 0)
                m_CopyValue.Remove(0, m_CopyValue.Length);

            UpdateAttribGridInverse();
        }

        /// <summary>
        /// 선택영역(Drag)이 있는지 또는 선택중인지 상태 체크
        /// </summary>
        /// <param name="mouseY"></param>
        protected bool IsSelectMode()
        {
            return (m_BeginRow > -1);
        }

		// 2019-11-10 ???? (?? ?? ?? ?? ?? ?? ?? ?? )
        public void SetFontColor(Color mColor)
        {
            m_FGColor = mColor;
        }

        public void SetBackGroundColor(Color mColor)
        {
            BackColor = mColor;
        }

        /// <summary>
        /// Command 를 전송 합니다.
        /// </summary>
        private void SendTelnetCommand()
        {
            //RequestCommunicationData CmdData = null;
            String CmdData = String.Empty;

            TelnetCommandInfo tCommandInfo = new TelnetCommandInfo();
            RequestCommunicationData tRequestData = null;

            while (true)
            {
                Thread.Sleep(100);

                if (!m_IsConnected)
                    continue;

                if (m_CmdQueue.Count == 0)
                    continue;

                lock (m_CmdQueue)
                {
                    CmdData = m_CmdQueue.Dequeue();
                }

                if (CmdData != null)
                {
                    if (m_DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET)
                    {

                        //TelnetCommandInfo tCommandInfo = new TelnetCommandInfo();
                        tCommandInfo.DeviceInfo = m_DeviceInfo;
                        tCommandInfo.SessionID = m_ConnectedSessionID;

                        tCommandInfo.WorkTyp = E_TelnetWorkType.Execute;
                        tCommandInfo.Command = CmdData;
                        //if (m_ScriptManager.IsRunScript != true)
                        //    tCommandInfo.CmdSendDelay = AppGlobal.s_ClientOption.SendDelay;
                        //RequestCommunicationData tRequestData = null;

                        if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online )// && m_DeviceInfo.IsRegistered) 모든 장비를 데몬을 통한 통신으로 변경, 등록된 장비 여부 체크 제외 
                        {

                            tCommandInfo.UserID = AppGlobal.s_LoginResult.UserID;
                            tRequestData = AppGlobal.MakeDefaultRequestData();
                            tRequestData.CommType = E_CommunicationType.RequestCommandProcess;
                            tRequestData.RequestData = tCommandInfo;


                            if (m_DaemonProcessRemoteObject == null) return;
                            m_Result = null;
                            m_MRE.Reset();

                            m_DaemonProcessRemoteObject.SendDaemonRequestData(this, tRequestData);
                        }
                        else
                        {
                            tCommandInfo.Sender = this;
                            tRequestData = new RequestCommunicationData();
                            tRequestData.CommType = E_CommunicationType.RequestCommandProcess;
                            tRequestData.RequestData = tCommandInfo;
                            AppGlobal.s_TelnetProcessor.ExecuteCommand(tRequestData);
                        }
                    }
                    else if (m_DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
                    {
                        tCommandInfo.DeviceInfo = m_DeviceInfo;
                        tCommandInfo.SessionID = m_ConnectedSessionID;

                        tCommandInfo.WorkTyp = E_TelnetWorkType.Execute;
                        tCommandInfo.Command = CmdData;
                        //if (m_ScriptManager.IsRunScript != true)
                        //    tCommandInfo.CmdSendDelay = AppGlobal.s_ClientOption.SendDelay;
                        //RequestCommunicationData tRequestData = null;

                        if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online)// && m_DeviceInfo.IsRegistered) 모든 장비를 데몬을 통한 통신으로 변경, 등록된 장비 여부 체크 제외 
                        {
                            //사용 유무 확인 안됨
                            tCommandInfo.UserID = AppGlobal.s_LoginResult.UserID;
                            tRequestData = AppGlobal.MakeDefaultRequestData();
                            tRequestData.CommType = E_CommunicationType.RequestCommandProcess;
                            tRequestData.RequestData = tCommandInfo;


                            if (m_DaemonProcessRemoteObject == null) return;
                            m_Result = null;
                            m_MRE.Reset();

                            m_DaemonProcessRemoteObject.SendDaemonRequestData(this, tRequestData);
                        }
                        else
                        {
                            tCommandInfo.Sender = this;
                            tRequestData = new RequestCommunicationData();
                            tRequestData.CommType = E_CommunicationType.RequestCommandProcess;
                            tRequestData.RequestData = tCommandInfo;
                            AppGlobal.s_TelnetProcessor.ExecuteCommand(tRequestData);
                        }
                    }
                    else
                    {
                        AppGlobal.s_SerialProcessor.SendRequest(this, CmdData);
                    }
					
                    CmdData = null;
                    tRequestData = null;
                    //System.Diagnostics.Debug.WriteLine("SendTelnetCommand !!!!!!!!!!!!!!!!!!!!! SendDelay Start!!!!!!!!!!!!!!!!!!! [=");
                    //System.Diagnostics.Debug.WriteLine("SendTelnetCommand !!!!!!!!!!!!!!!!!!!!! SendDelay m_IsPressEnter!!!!!!!!!!!!!!!!!!! [=" + m_IsPressEnter.ToString());
                    //if (m_IsPressEnter)
                    //    Thread.Sleep(AppGlobal.s_ClientOption.SendDelay);
                    //System.Diagnostics.Debug.WriteLine("SendTelnetCommand !!!!!!!!!!!!!!!!!!!!! SendDelay End!!!!!!!!!!!!!!!!!!! ");
                }

                Thread.Sleep(AppGlobal.s_ClientOption.SendDelay);
            }
            //System.Diagnostics.Debug.WriteLine("SendTelnetCommand !!!!!!!!!!!!!!!!!!!!!  End!!!!!!!!!!!!!!!!!!! ");
        }

        //private void SendCommand(RequestCommunicationData Command)
        private void SendCommand(String Command)    
        {
            lock (m_CmdQueue)
            {
                m_CmdQueue.Enqueue(Command);
                //System.Diagnostics.Debug.WriteLine("SendTelnetCommand !!!!!!!!!!!!!!!!!!!!! Enqueue !!!!!!!!!!!!!!!!!!! [=" + Command);
            }
        }

        //
        private void SendTelnetStop()
        {
            m_CmdQueue.Clear();

            if (m_CmdControlThread != null)
            {
                m_CmdControlThread.Join(100);
                if (m_CmdControlThread.IsAlive)
                {
                    try
                    {
                        m_CmdControlThread.Abort();
                    }
                    catch (Exception) { }
                }
                m_CmdControlThread = null;
            }
        }

    } // End of class (MCTerminalEmulator)
 
}



