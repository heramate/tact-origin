using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.Text;
using System.IO;
using System.Drawing;
using System.Collections;
using RACTClient;
using RACTCommonClass;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using RACTTerminal;

namespace RACTClient
{
    
    /// <summary>
    /// 터미널 컨트롤 입니다.
    /// </summary>
    public class MCSmallTerminal : UserControl
    {
        private List<string> m_SendTerminalList = new List<string>();
            
        /// <summary>
        /// 터미널에 명령을 전송할 이벤트 입니다.
        /// </summary>
        public static event HandlerArgument2<List<string>, string> OnSendCommandToTerminalEvent = null;
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

        private event CaretOffEventHandler OnCaretOffEvent;
        private event CaretOnEventHandler OnCaretEvent;

        /// <summary>
        /// 프롬프트 입니다.
        /// </summary>
        private string m_Prompt=">";

        /// <summary>
        /// 프롬프트 가져오거나 설정 합니다.
        /// </summary>
        public string Prompt
        {
            get { return m_Prompt; }
            set { m_Prompt = value; }
        }




	
        /// <summary>
        /// 드래그 시작 위치 입니다.
        /// </summary>
        private Point m_BeginDrag;
        /// <summary>
        /// 드래그 종료 위치 입니다.
        /// </summary>
        private Point m_EndDrag;
        /// <summary>
        /// 커서의 문자 입니다.
        /// </summary>
        private string m_TextAtCursor;
        /// <summary>
        /// 마지막 표시 라인 입니다.
        /// </summary>
        private int m_LastVisibleLine;

        private bool m_XOff = false;

        private string m_OutBuffer = "";
        /// <summary>
        /// 저장될 최대 라인 수 입니다.
        /// </summary>
        private int m_ScrollbackBufferSize;
        /// <summary>
        /// 전체 받은 문자 입니다.
        /// </summary>
        private StringCollection m_ScrollbackBuffer;
        private Parser m_Parser = null;
        private TelnetParser m_NvtParser = null;
        private SmallKeyboard m_Keyboard = null;
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
        /// Type Face 입니다.
        /// </summary>
        private string m_TypeFace = FontFamily.GenericMonospace.GetName(0);
        /// <summary>
        /// Type Style 입니다.
        /// </summary>
        private FontStyle m_TypeStyle = System.Drawing.FontStyle.Regular;
        /// <summary>
        /// Type Size 입니다.
        /// </summary>
        private Int32 m_TypeSize = 8;
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
        /// Bold Color 입니다.
        /// </summary>
        private Color m_BoldColor;
        /// <summary>
        /// 비활성 색상입니다.
        /// </summary>
        private Color m_DisEnableColor = Color.Gray;
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
        private Timer timer1;
        private System.ComponentModel.IContainer components;
        private DevComponents.DotNetBar.ContextMenuBar contextMenuBar1;
        private DevComponents.DotNetBar.ButtonItem cmPopUP;
        private DevComponents.DotNetBar.ButtonItem mnuCopy;
        private DevComponents.DotNetBar.ButtonItem mnuPaste;
        private DevComponents.DotNetBar.ButtonItem mnuFind;
        private DevComponents.DotNetBar.ButtonItem mnuSelectAll;
        private DevComponents.DotNetBar.ButtonItem mnuClear;

        /// <summary>
        /// 문자 자르기 합니다.
        /// </summary>
        /// <returns></returns>
        public void ScreenScrape(int aStartColumn, int aStartRow, int aEndColumn, int aEndRow)
        {
            RichTextBox tTextBox = new RichTextBox();
            for (int tRow = aStartRow; tRow <= aEndRow; tRow++)
            {
                for (int tCos = aStartColumn; tCos <= aEndColumn; tCos++)
                {
                    tTextBox.AppendText(m_CharGrid[tRow][tCos].ToString());

                }
                tTextBox.AppendText("\r\n");
            }
            tTextBox.Text = tTextBox.Text.Substring(0, tTextBox.Text.Length - 1);

            Clipboard.SetDataObject(tTextBox.Text);
        }

        /// <summary>
        /// 찾은 결과를 적용 합니다.
        /// </summary>
        /// <param name="aInfo"></param>
        private void ApplyFindInformation(TelnetStringFind aInfo)
        {
            for (int i = 0; i < this.m_Rows; i++)
            {
                Array.Clear(this.m_AttribGrid[i], 0, this.m_AttribGrid[i].Length);
            }
            if (aInfo.IsMatch)
            {
                foreach (StringFindInfo tStringInfo in aInfo.FindList)
                {
                    m_AttribGrid[tStringInfo.Row][tStringInfo.Col].IsInverse = true;
                }
            }
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

            bool tFind = false;
            int tLoopRow = m_LastFindRow != 0 ? m_LastFindRow : 0;
            int tLoopCol = m_LastFindCol != 0 ? m_LastFindCol : 0;

            int tMaxRow = -1;
            int tMaxCol = -1;

            int tCharIndex = 0;

            StringFindInfo tFindInfo;
            string tBufferString = "";
            string tFindString = "";
            while (tMaxRow != m_Rows - 1 && tMaxCol != m_Cols - 1)
            {
                for (int i = tCharIndex; i < oFindInfo.FindList.Count; i++)
                {
                    tFind = false;
                    for (int tRow = tLoopRow; tRow < m_Rows; tRow++)
                    {
                        tMaxRow = tMaxRow < tRow ? tRow : tMaxRow;
                        // tLoopCol = 0;
                        for (int tCol = tLoopCol; tCol < m_Cols; tCol++)
                        {
                            tFind = false;
                            tFindInfo = oFindInfo.FindList[tCharIndex];

                            tMaxCol = tMaxCol < tCol ? tCol : tMaxCol;

                            tBufferString = m_CharGrid[tRow][tCol].ToString();
                            tFindString = tFindInfo.FindText;

                            if (aArgs.IsMatchCase)
                            {
                                tBufferString = m_CharGrid[tRow][tCol].ToString().ToUpper();
                                tFindString = tFindInfo.FindText.ToUpper();
                            }

                            if (tBufferString.Equals(tFindString))
                            {
                                tFindInfo.Row = tRow;
                                tFindInfo.Col = tCol;
                                tFind = true;
                                tLoopRow = tRow;
                                tLoopCol = tCol + 1;
                                tCharIndex++;
                                break;
                            }
                            else
                            {
                                if (oFindInfo.FindList.IndexOf(tFindInfo) > 0)
                                {
                                    oFindInfo.FindList[oFindInfo.FindList.IndexOf(tFindInfo) - 1].Row = -1;
                                    oFindInfo.FindList[oFindInfo.FindList.IndexOf(tFindInfo) - 1].Col = -1;

                                    tCharIndex = 0;
                                    tLoopCol = 0;
                                    i = 0;
                                }

                            }
                            tLoopRow = tRow;
                        }
                        if (tFind)
                        {
                            break;
                        }
                    }
                }
                if (oFindInfo.IsMatch)
                {
                    break;
                }
            }

            if (oFindInfo.IsMatch)
            {
                m_LastFindRow = oFindInfo.FindList[oFindInfo.FindList.Count - 1].Row;
                m_LastFindCol = oFindInfo.FindList[oFindInfo.FindList.Count - 1].Col + 1;
            }
            else
            {
                m_LastFindRow = 0;
                m_LastFindCol = 0;
            }

            return oFindInfo.IsMatch;
        }


        /// <summary>
        /// 화면에 표시합니다.
        /// </summary>
        /// <param name="aData">data 입니다.</param>
        /// <param name="aOffset">offset 입니다.</param>
        /// <param name="aLength">길이 입니다.</param>
        public void Write(byte[] aData, int aOffset, int aLength)
        {
            string tReceived = Encoding.ASCII.GetString(aData, aOffset, aLength);
            this.Invoke(this.OnRxdTextEvent, new string[] { string.Copy(tReceived) });
            this.Invoke(this.OnRefreshEvent);
        }
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public MCSmallTerminal()
        {

            m_ScrollbackBufferSize = 3000;
            m_ScrollbackBuffer = new StringCollection();

            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);

            m_Keyboard = new SmallKeyboard(this);
            m_Parser = new Parser();
            m_NvtParser = new TelnetParser();
            m_Caret = new Caret();
            m_Modes = new Mode();
            m_TabStops = new TabStops();
            m_SavedCarets = new System.Collections.ArrayList();

            //this.Name       = "ACK-TERM";
            //this.Text       = "ACK-TERM";

            m_Caret.Pos = new System.Drawing.Point(0, 0);
            m_CharSize = new System.Drawing.Size();

            //Font       = new System.Drawing.Font(FontFamily.GenericMonospace, 8.5F);

            //FGColor      = System.Drawing.Color.FromArgb (200, 200, 200);
#if Debug

             m_FGColor = Color.GreenYellow;
                BackColor = Color.Black;
                m_BoldColor = Color.Blue;
                m_BlinkColor = Color.Red;

            
#endif
            if (AppGlobal.s_ClientOption != null)
            {
                ForeColor = AppGlobal.s_ClientOption.TerminalFontColor;
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

            ChangePopupType();

            //m_VertScrollBar = new VertScrollBar();
            //m_VertScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.HandleScroll);
            ////m_VertScrollBar.ValueChanged += new EventHandler(m_VertScrollBar_ValueChanged);

            //m_VertScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            Font = new System.Drawing.Font(m_TypeFace, m_TypeSize, m_TypeStyle);
            //Controls.Add(m_VertScrollBar);

            this.SetSize(10, 80);

            m_Parser.OnParserEvent += new ParserEventHandler(CommandRouter);
            m_Keyboard.OnKeyboardEvent += new KeyboardEventHandler(DispatchMessage);
            m_Keyboard.OnControlKeyBoardEvent += new ControlKeyboardEventHandler(DispatchControlMessage);
            m_NvtParser.NvtParserEvent += new NegotiateParserEventHandler(TelnetInterpreter);
            OnRefreshEvent += new RefreshEventHandler(ShowBuffer);
            OnCaretOffEvent += new CaretOffEventHandler(CaretOff);
            OnCaretEvent += new CaretOnEventHandler(CaretOn);
            OnRxdTextEvent += new RxdTextEventHandler(m_NvtParser.Parsestring);

            m_BeginDrag = new System.Drawing.Point();
            m_EndDrag = new System.Drawing.Point();
            timer1 = new Timer();
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            timer1.Start();
            DisplayCommand(m_Prompt);
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
            this.mnuFind = new DevComponents.DotNetBar.ButtonItem();
            this.mnuSelectAll = new DevComponents.DotNetBar.ButtonItem();
            this.mnuClear = new DevComponents.DotNetBar.ButtonItem();
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
            this.cmPopUP.AutoExpandOnClick = true;
            this.cmPopUP.Name = "cmPopUP";
            this.cmPopUP.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.mnuCopy,
            this.mnuPaste});
          ///  this.mnuFind,
           // this.mnuSelectAll,
           // this.mnuClear
            this.cmPopUP.Text = "buttonItem1";
            // 
            // buttonItem2
            // 
            this.mnuCopy.Name = "buttonItem2";
            this.mnuCopy.Text = "복사(&C)";
            mnuCopy.Click += new EventHandler(mnuCopy_Click);
            // 
            // buttonItem3
            // 
            this.mnuPaste.Name = "buttonItem3";
            this.mnuPaste.Text = "붙여넣기(&V)";
            mnuPaste.Click += new EventHandler(mnuPaste_Click);
            // 
            // buttonItem4
            // 
            this.mnuFind.BeginGroup = true;
            this.mnuFind.Name = "buttonItem4";
            this.mnuFind.Text = "찾기(&F)";
            mnuFind.Click += new EventHandler(mnuFind_Click);
            // 
            // buttonItem5
            // 
            this.mnuSelectAll.Name = "buttonItem5";
            this.mnuSelectAll.Text = "모두선택(&A)";
            mnuSelectAll.Click += new EventHandler(mnuSelectAll_Click);
            // 
            // buttonItem6
            // 
            this.mnuClear.BeginGroup = true;
            this.mnuClear.Name = "buttonItem6";
            this.mnuClear.Text = "화면지움(&R)";
            mnuClear.Click += new EventHandler(mnuClear_Click);
            // 
            // MCTerminalEmulator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.Controls.Add(this.contextMenuBar1);
            this.Name = "MCTerminalEmulator";
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).EndInit();
            this.ResumeLayout(false);
        }

        /// <summary>
        /// 팝업 타입을 설정 합니다.
        /// </summary>
        private void ChangePopupType()
        {
            //switch (AppGlobal.s_ClientOption.TerminalPopupType)
            //{
            //    case E_TerminalPopupType.PopUp:
            //        this.ContextMenu = m_TerminalContextMenu;
            //        break;
            //    default:
            //        this.ContextMenu = null;
            //        break;
            //}
        }

        /// <summary>
        /// 종료 처리 합니다.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {

            timer1.Stop();
            base.Dispose(disposing);

        }
        /// <summary>
        /// 크기 변경 이벤트 처리 입니다.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(System.EventArgs e)
        {
            this.Font = new Font(m_TypeFace, m_TypeSize, m_TypeStyle);

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

            SetSize(tRows, tColumns);

            StringCollection tVisiblebuffer = new StringCollection();
            for (int i = m_ScrollbackBuffer.Count - 1; i >= 0; i--)
            {
                tVisiblebuffer.Insert(0, m_ScrollbackBuffer[i]);
                if (tVisiblebuffer.Count >= tRows - 1) break;
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
            Refresh();

            base.OnResize(e);
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighSpeed;
            e.Graphics.TextRenderingHint = TextRenderingHint.SystemDefault;
            e.Graphics.TextContrast = 0;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;

            WipeScreen(e.Graphics);
            Redraw(e.Graphics);
            if (m_IsShowCaret && this.Enabled && this.Focused)
            {
                ShowCaret(e.Graphics);
            }
        }

        protected override void WndProc(ref Message m)
        {
            
            switch (m.Msg)
            {
                case WMCodes.WM_KEYDOWN:
                case WMCodes.WM_SYSKEYDOWN:
                case WMCodes.WM_KEYUP:
                case WMCodes.WM_SYSKEYUP:
                case WMCodes.WM_SYSCHAR:
                case WMCodes.WM_CHAR:
                        this.m_Keyboard.KeyDown(m);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool tReturn = false;
            if ((msg.Msg == WMCodes.WM_KEYDOWN) || (msg.Msg == WMCodes.WM_SYSKEYDOWN))
            {
                switch (keyData)
                {
                    case Keys.Tab:
                        string tTempString = "";
                        char tCurChar;
                        for (int x = 1; x < this.m_Cols; x++)
                        {
                            tCurChar = this.m_CharGrid[this.m_Caret.Pos.Y][x];
                            if (tCurChar == '\0')
                            {
                                continue;
                            }
                            tTempString = tTempString + Convert.ToString(tCurChar);
                        }
                        if (m_SendTerminalList.Count > 0)
                        {
                            if (OnSendCommandToTerminalEvent != null)
                            {
                                OnSendCommandToTerminalEvent(m_SendTerminalList, tTempString);
                                OnSendCommandToTerminalEvent(m_SendTerminalList, "\t");
                                this.CarriageReturn();
                                this.LineFeed();
                            }
                        }
                        tReturn = true;
                        break;
                    case Keys.Left:
                    case Keys.Right:
                    case Keys.Up:
                    case Keys.Down:
                        tReturn = true;
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

            m_EndDrag.X = CurArgs.X;
            m_EndDrag.Y = CurArgs.Y;

            int tEndCol = m_EndDrag.X / m_CharSize.Width;
            int tEndRow = m_EndDrag.Y / m_CharSize.Height;

            int tBegCol = m_BeginDrag.X / m_CharSize.Width;
            int tBegRow = m_BeginDrag.Y / m_CharSize.Height;


            for (int iRow = 0; iRow < this.m_Rows; iRow++)
            {
                for (int iCol = 0; iCol < this.m_Cols; iCol++)
                {
                    this.m_AttribGrid[iRow][iCol].IsInverse = false;
                }
            }


            if (tEndRow < tBegRow)
            {
                int i = tEndRow;
                tEndRow = tBegRow;
                tBegRow = i;
                for (int tCurRow = tBegRow; tCurRow <= tEndRow; tCurRow++)
                {
                    if (tCurRow <= 0) continue;

                    for (int tCurCol = 0; tCurCol < this.m_Cols; tCurCol++)
                    {
                        if (this.m_CharGrid[tCurRow][tCurCol] == '\0') continue;

                        if (tCurRow == tBegRow && tCurCol < tEndCol) continue;

                        if (tCurRow == tEndRow && tCurCol == tBegCol)
                        {
                            m_AttribGrid[tCurRow][tCurCol].IsInverse = true;
                            break;
                        }
                        m_AttribGrid[tCurRow][tCurCol].IsInverse = true;
                    }
                }
                this.Refresh();
                return;
            }

            if (tEndCol < tBegCol && tBegRow == tEndRow)
            {
                int tTemp = tEndCol;
                tEndCol = tBegCol;
                tBegCol = tTemp;
            }

            for (int tCurRow = tBegRow; tCurRow <= tEndRow; tCurRow++)
            {
                if (tCurRow >= this.m_Rows) break;

                for (int tCurCol = 0; tCurCol < this.m_Cols; tCurCol++)
                {
                    if (this.m_CharGrid[tCurRow][tCurCol] == '\0') continue;

                    if (tCurRow == tBegRow && tCurCol < tBegCol) continue;

                    if (tCurRow == tEndRow && tCurCol == tEndCol)
                    {
                        m_AttribGrid[tCurRow][tCurCol].IsInverse = true;
                        break;
                    }
                    m_AttribGrid[tCurRow][tCurCol].IsInverse = true;
                }
            }
            this.Refresh();
        }

        /// <summary>
        /// 마우스 up 처리 합니다.
        /// </summary>
        /// <param name="CurArgs"></param>
        protected override void OnMouseUp(MouseEventArgs CurArgs)
        {
            if (CurArgs.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (this.m_BeginDrag.X == CurArgs.X && this.m_BeginDrag.Y == CurArgs.Y)
                {
                    for (int tRow = 0; tRow < this.m_Rows; tRow++)
                    {
                        for (int tCol = 0; tCol < this.m_Cols; tCol++)
                        {
                            m_AttribGrid[tRow][tCol].IsInverse = false;
                        }
                    }
                }
                this.Refresh();
            }

        }
        /// <summary>
        /// 마우스 다운 처리 입니다.
        /// </summary>
        /// <param name="CurArgs"></param>
        protected override void OnMouseDown(MouseEventArgs aCurArgs)
        {
            this.Focus();
            if (!this.Enabled) return;

            Font tFont = new Font(FontFamily.GenericMonospace, 8.5F);
            Graphics tGraphics = this.CreateGraphics();

            if (aCurArgs.Button == MouseButtons.Right)
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
            else if (aCurArgs.Button == MouseButtons.Left)
            {
                this.m_BeginDrag.X = aCurArgs.X;
                this.m_BeginDrag.Y = aCurArgs.Y;
            }
            base.OnMouseDown(aCurArgs);
        }


        /// <summary>
        /// 모두 삭제 처리 입니다.
        /// </summary>
        void mnuClear_Click(object sender, EventArgs e)
        {
            //초기화 합니다.
            for (int i = 0; i < this.m_Rows; i++)
            {
                Array.Clear(this.m_CharGrid[i], 0, this.m_CharGrid[i].Length);
                Array.Clear(this.m_AttribGrid[i], 0, this.m_AttribGrid[i].Length);
            }
            CaretToAbs(0, 0);
        }


        /// <summary>
        /// 모두 선택 처리 입니다.
        /// </summary>
        void mnuSelectAll_Click(object sender, EventArgs e)
        {
            for (int tRow = 0; tRow < this.m_Rows; tRow++)
            {
                for (int tCol = 0; tCol < this.m_Cols; tCol++)
                {
                    m_AttribGrid[tRow][tCol].IsInverse = true;
                }
            }
            this.Refresh();
        }

       
        /// <summary>
        /// 찾기 처리 입니다.
        /// </summary>
        void mnuFind_Click(object sender, EventArgs e)
        {
            if (OnTelnetFindString != null) OnTelnetFindString();
        }

        /// <summary>
        /// 터미널에서 해당 문자를 찾습니다.
        /// </summary>
        /// <param name="aString">찾을 문자 입니다.</param>
        public void FindForm_OnTelnetStringFind(TelnetStringFindHandlerArgs aArgs)
        {
            TelnetStringFind tFindInfo;
            if (FindString(aArgs, out tFindInfo))
            {
                System.Diagnostics.Debug.WriteLine("찾음");
            }
            else
            {
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

            AppGlobal.ShowMessage(AppGlobal.s_ClientMainForm, "'" + aArgs.FindString + "'을(를) 찾을 수 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        /// <summary>
        /// 복사 처리 합니다.
        /// </summary>
        private void mnuCopy_Click(object sender, System.EventArgs e)
        {
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
        private void mnuPaste_Click(object sender, System.EventArgs e)
        {
            IDataObject tClipboard = Clipboard.GetDataObject();

            if (tClipboard.GetDataPresent(DataFormats.Text))
            {
                if (tClipboard.GetData(DataFormats.Text) != null)
                {
                    this.DispatchMessage(this, tClipboard.GetData(DataFormats.Text).ToString());
                }
            }
        }



        /// <summary>
        /// 스크롤 처리 합니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="aScrollEventArgs"></param>
        private void HandleScroll(Object sender, ScrollEventArgs aScrollEventArgs)
        {
            if (!this.m_Caret.IsOff)
            {
                m_TextAtCursor = "";
                char tCurrentChar;
                for (int i = 0; i < this.m_Cols; i++)
                {
                    tCurrentChar = this.m_CharGrid[this.m_Caret.Pos.Y][i];

                    if (tCurrentChar == '\0') continue;

                    m_TextAtCursor = m_TextAtCursor + Convert.ToString(tCurrentChar);
                }
            }


            m_LastVisibleLine += (aScrollEventArgs.OldValue * m_CharSize.Height + aScrollEventArgs.NewValue * m_CharSize.Height) / m_CharSize.Height;


            switch (aScrollEventArgs.Type)
            {
                case ScrollEventType.SmallIncrement:
                    m_LastVisibleLine += 1;
                    break;
                case ScrollEventType.SmallDecrement:
                    m_LastVisibleLine += -1;
                    break;
                // case ScrollEventType.ThumbPosition:
                case ScrollEventType.ThumbTrack:

                    int tValue = aScrollEventArgs.NewValue - aScrollEventArgs.OldValue;
                    if (tValue < 0)
                    {
                        m_LastVisibleLine += -5;
                    }
                    else if (tValue > 0)
                    {
                        m_LastVisibleLine += 5;
                    }
                    break;
                default:
                    return;
            }


            if (m_LastVisibleLine > 0)
            {
                m_LastVisibleLine = 0;
            }

            if (m_LastVisibleLine < (0 - m_ScrollbackBuffer.Count) + (m_Rows))
            {
                m_LastVisibleLine = (0 - m_ScrollbackBuffer.Count) + (m_Rows) - 1;
            }


            int tColumns = m_Cols;
            int tRows = m_Rows;

            SetSize(tRows, tColumns);

            StringCollection tVisiblebuffer = new StringCollection();

            for (int i = m_ScrollbackBuffer.Count - 1 + m_LastVisibleLine; i >= 0; i--)
            {
                if (m_ScrollbackBuffer.Count < i) return;
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
            }

            this.Refresh();
        }
        

        /// <summary>
        /// 받음 처리 합니다.
        /// </summary>
        /// <param name="aResult"></param>
        private void OnReceivedData(string aResult)
        {
            System.Diagnostics.Debug.WriteLine(aResult);
            this.Invoke(this.OnRxdTextEvent, new string[] { string.Copy(aResult) });
            this.Invoke(this.OnRefreshEvent);
            this.Focus();
        }
        /// <summary>
        /// 엔터 누름 입니다.
        /// </summary>
        private bool m_IsPressEnter = false;
        /// <summary>
        /// 컨트롤 메시지를 전송 합니다.
        /// </summary>
        /// <param name="aSender"></param>
        /// <param name="aKeyMap"></param>
        void DispatchControlMessage(object aSender, KeyInfo aKeyInfo)
        {
            Console.WriteLine(aKeyInfo.Outstring);
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

                //DisplayCommand(aKeyInfo.Outstring);
                switch (aKeyInfo.ScanCode)
                {
                    case 14:
                        if (m_Caret.Pos.X <= 1) return;
                        DisplayCommand("\b \b");
                        break;
                    case 28:
                        DisplayCommand("\r\n");
                        if (m_SendTerminalList.Count == 0) return;
                        if (OnSendCommandToTerminalEvent != null) OnSendCommandToTerminalEvent(m_SendTerminalList, m_ScrollbackBuffer[m_ScrollbackBuffer.Count - 1] + "\x0D");
                        break;
                    case 75:
                        if (m_Caret.Pos.X <= 1) return;
                        DisplayCommand("\b");
                        break;
                    case 83:
                        if (m_Caret.Pos.X <= 1) return;
                        DisplayCommand("");
                        break;
                    case 71:
                        CaretToAbs(1,m_Caret.Pos.Y);
                        break;
                    case 72:
                        if (m_ScrollbackBuffer.Count == 0) return;
                        if (m_CommandHistoryIndex < 1) return;
                        CaretToAbs(1,m_Caret.Pos.Y);
                        ClearRight(0);
                        m_CommandHistoryIndex--;
                        DisplayCommand(m_ScrollbackBuffer[m_CommandHistoryIndex]);
                        
                        break;
                    case 80:
                        if (m_ScrollbackBuffer.Count == 0) return;
                        if (m_CommandHistoryIndex >= m_ScrollbackBuffer.Count -1) return;
                        CaretToAbs(1, m_Caret.Pos.Y);
                        ClearRight(0);
                        m_CommandHistoryIndex++;
                        DisplayCommand(m_ScrollbackBuffer[m_CommandHistoryIndex]);
                        break;
                    default:
                        DisplayCommand(aKeyInfo.Outstring);
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
        private int m_CommandHistoryIndex = 0;
        /// <summary>
        /// 메시지를 전송 합니다.
        /// </summary>
        /// <param name="aSender"></param>
        /// <param name="aText"></param>
        private void DispatchMessage(Object aSender, string aText)
        {
            // Console.WriteLine(aText);
            if (this.m_XOff == true)
            {
                m_OutBuffer += aText;
                return;
            }

            try
            {
                System.Byte[] smk = new System.Byte[aText.Length];

                if (m_OutBuffer != "")
                {
                    aText = m_OutBuffer + aText;
                    m_OutBuffer = "";
                }
                DisplayCommand(aText);
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

        private void AssignColors(CharAttribStruct aCurAttribs, ref Color aCurFGColor, ref Color aCurBGColor)
        {

            aCurFGColor = this.Enabled ? this.ForeColor : m_DisEnableColor;
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

            Color tCurFGColor = System.Drawing.Color.White;
            Color tCurBGColor = System.Drawing.Color.Black;

            AssignColors(aCurAttribs, ref tCurFGColor, ref tCurBGColor);

            if ((tCurBGColor != this.BackColor && (this.m_Modes.Flags & Mode.s_LightBackground) == 0) ||
                (tCurBGColor != this.ForeColor && (this.m_Modes.Flags & Mode.s_LightBackground) > 0))
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
                if (AppGlobal.s_ClientOption == null)
                {
                    aCurGraphics.Clear(Color.GreenYellow);
                }
                else
                {
                    aCurGraphics.Clear(AppGlobal.s_ClientOption.TerminalFontColor);
                }
            }
            else
            {
                if (AppGlobal.s_ClientOption == null)
                {
                    aCurGraphics.Clear(Color.Black);
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
            CaretToAbs(0, this.m_Caret.Pos.Y);
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
            this.m_ScrollbackBuffer.Add(tTempString.Substring(m_Prompt.Length));
            m_CommandHistoryIndex = m_ScrollbackBuffer.Count;
            
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

            this.CaretDown();
            m_IsCompleteInitialize = false;
            DisplayCommand(m_Prompt);
            m_IsCompleteInitialize = true;
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

        private void CaretOff()
        {
            if (this.m_Caret.IsOff == true)
            {
                return;
            }

            this.m_Caret.IsOff = true;
        }

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

                    if (Param > 0) this.SetSize(Param, this.m_Cols);

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
                        this.SetSize(this.m_Rows, 132);
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
                        this.SetSize(this.m_Rows, 80);
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
                    this.m_XOff = true;
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
        private void SetSize(System.Int32 Rows, System.Int32 Columns)
        {
            this.m_Rows = Rows;
            this.m_Cols = Columns;

            this.m_TopMargin = 0;
            this.m_BottomMargin = Rows - 1;

            //this.ClientSize = new System.Drawing.Size (
            //	System.Convert.ToInt32 (this.CharSize.Width  * this.Columns + 2) + this.VertScrollBar.Width,
            //	System.Convert.ToInt32 (this.CharSize.Height * this.Rows    + 2));

            // create the character grid (rows by columns) this is a shadow of what's displayed
            this.m_CharGrid = new System.Char[Rows][];

            this.m_Caret.Pos.X = 0;
            this.m_Caret.Pos.Y = 0;

            for (int i = 0; i < this.m_CharGrid.Length; i++)
            {
                this.m_CharGrid[i] = new System.Char[Columns];
            }

            this.m_AttribGrid = new CharAttribStruct[Rows][];

            for (int i = 0; i < this.m_AttribGrid.Length; i++)
            {
                this.m_AttribGrid[i] = new CharAttribStruct[Columns];
            }
        }

        /// <summary>
        /// 폰트 정보를 가져오기 합니다.
        /// </summary>
        private void GetFontInfo()
        {
            Graphics tmpGraphics = this.CreateGraphics();

            this.m_DrawstringOffset = this.GetDrawstringOffset(tmpGraphics, 0, 0, 'A');

            System.Drawing.Point tmpPoint = this.GetCharSize(tmpGraphics);

            this.m_CharSize.Width = tmpPoint.X; // 
            this.m_CharSize.Height = tmpPoint.Y;

            tmpGraphics.Dispose();

            this.m_UnderlinePos = this.m_CharSize.Height - 2;

            this.m_Caret.Bitmap = new System.Drawing.Bitmap(this.m_CharSize.Width, this.m_CharSize.Height);
            this.m_Caret.Buffer = System.Drawing.Graphics.FromImage(this.m_Caret.Bitmap);
            this.m_Caret.Buffer.Clear(System.Drawing.Color.FromArgb(255, 181, 106));
            this.m_EraseBitmap = new System.Drawing.Bitmap(this.m_CharSize.Width, this.m_CharSize.Height);
            this.m_EraseBuffer = System.Drawing.Graphics.FromImage(this.m_EraseBitmap);


        }

        private void OnClickFont(System.Object Sender, System.EventArgs e)
        {
            System.Windows.Forms.FontDialog fontDialog = new System.Windows.Forms.FontDialog();

            fontDialog.FixedPitchOnly = true;
            fontDialog.ShowEffects = false;
            fontDialog.Font = this.Font;

          
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
        /// Mode를 가져오거나 설정 합니다.
        /// </summary>
        public Mode Modes
        {
            get { return m_Modes; }
            set { m_Modes = value; }
        }



        /// <summary>
        /// 결과를 표시 합니다.
        /// </summary>
        /// <param name="aResult"></param>
        public void DisplayCommand(string aCommand)
        {
           // System.Diagnostics.Debug.WriteLine(aCommand);
            OnReceivedData(aCommand);
        }
        /// <summary>
        /// Caret 표시 여부 입니다.
        /// </summary>
        private bool m_IsShowCaret = true;
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
        /// 옵션 정보를 적용 합니다.
        /// </summary>
        internal void ApplyOption()
        {
            ForeColor = AppGlobal.s_ClientOption.TerminalFontColor;
            BackColor = AppGlobal.s_ClientOption.TerminalBackGroundColor;
            this.Refresh();
        }

        #region ITelnetEmulator 멤버

        public Mode GetMode()
        {
            return Modes;
        }

        #endregion

        private bool m_IsCompleteInitialize = false;
        internal void Status(bool aStatus)
        {
            this.Enabled = aStatus;    
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MCSmallTerminal
            // 
            this.Name = "MCSmallTerminal";
            this.Leave += new System.EventHandler(this.MCSmallTerminal_Leave);
            this.ResumeLayout(false);

        }

        private void MCSmallTerminal_Leave(object sender, EventArgs e)
        {

        }

        internal void SetReceiveList(List<string> aCheckList)
        {
            m_SendTerminalList = aCheckList;
        }
    }

    public class SmallKeyboard
    {
        /// <summary>
        /// 키보드 이벤트 입니다.
        /// </summary>
        public event KeyboardEventHandler OnKeyboardEvent;
        /// <summary>
        /// 컨트롤키 이벤트 입니다.
        /// </summary>
        public event ControlKeyboardEventHandler OnControlKeyBoardEvent;
        /// <summary>
        /// 마지막 키 눌렀는지 여부 입니다.
        /// </summary>
        private bool m_LastKeyDownSent = false;
        /// <summary>
        /// Alt 키 누름 여부 입니다.
        /// </summary>
        private bool m_AltIsDown = false;
        /// <summary>
        /// Shift 키 누름 여부 입니다.
        /// </summary>
        private bool m_ShiftIsDown = false;
        /// <summary>
        /// Ctrl 키 누름 여부 입니다.
        /// </summary>
        private bool m_CtrlIsDown = false;
        /// <summary>
        /// 부모 입니다.
        /// </summary>
        private MCSmallTerminal m_Parent;
        /// <summary>
        /// Key Map 입니다.
        /// </summary>
        private SmallKeyMap KeyMap = new SmallKeyMap();

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        /// <param name="aParent"></param>
        public SmallKeyboard(MCSmallTerminal aParent)
        {
            this.m_Parent = aParent;
        }
        /// <summary>
        /// 키보드 Down 처리 입니다.
        /// </summary>
        /// <param name="KeyMess"></param>
        public void KeyDown(System.Windows.Forms.Message aKeyMessage)
        {
            Byte[] tLPBytes;
            Byte[] tWPBytes;
            UInt16 tKeyValue = 0;
            UInt16 tRepeatCount = 0;
            Byte tScanCode = 0;
            Byte tAnsiChar = 0;
            UInt16 tUniChar = 0;
            Byte tFlags = 0;


            tLPBytes = BitConverter.GetBytes(aKeyMessage.LParam.ToInt64());
            tWPBytes = BitConverter.GetBytes(aKeyMessage.WParam.ToInt64());
            tRepeatCount = System.BitConverter.ToUInt16(tLPBytes, 0);
            tScanCode = tLPBytes[2];
            tFlags = tLPBytes[3];


            if (aKeyMessage.Msg == WMCodes.WM_SYSKEYDOWN || aKeyMessage.Msg == WMCodes.WM_KEYDOWN)
            {
                tKeyValue = System.BitConverter.ToUInt16(tWPBytes, 0);

                switch (tKeyValue)
                {
                    case 16:  // Shift Keys
                    case 160: // L Shift Key
                    case 161: // R Shift Keys
                        this.m_ShiftIsDown = true;
                        return;

                    case 17:  // Ctrl Keys
                    case 162: // L Ctrl Key
                    case 163: // R Ctrl Key
                        this.m_CtrlIsDown = true;
                        return;

                    case 18:  // Alt Keys (Menu)
                    case 164: // L Alt Key
                    case 165: // R Ctrl Key
                        this.m_AltIsDown = true;
                        return;
                }

                string tModifier = "Key";

                if (m_ShiftIsDown)
                {
                    tModifier = "Shift";
                }
                else if (m_CtrlIsDown)
                {
                    tModifier = "Ctrl";
                }
                else if (m_AltIsDown)
                {
                    tModifier = "Alt";
                }


                KeyInfo tControlKey = KeyMap.Find(tScanCode, Convert.ToBoolean(tFlags & 0x01), tModifier, m_Parent.Modes.Flags);

                this.m_LastKeyDownSent = false;

                if (tControlKey != null)
                {
                    this.m_LastKeyDownSent = true;
                    OnControlKeyBoardEvent(this, tControlKey);
                }

            }
            else if (aKeyMessage.Msg == WMCodes.WM_SYSKEYUP || aKeyMessage.Msg == WMCodes.WM_KEYUP)
            {
                tKeyValue = System.BitConverter.ToUInt16(tWPBytes, 0);

                switch (tKeyValue)
                {
                    case 16:  // Shift Keys
                    case 160: // L Shift Key
                    case 161: // R Shift Keys
                        this.m_ShiftIsDown = false;
                        break;

                    case 17:  // Ctrl Keys
                    case 162: // L Ctrl Key
                    case 163: // R Ctrl Key
                        this.m_CtrlIsDown = false;
                        break;

                    case 18:  // Alt Keys (Menu)
                    case 164: // L Alt Key
                    case 165: // R Ctrl Key
                        this.m_AltIsDown = false;
                        break;
                    case 37: //Left Keys
                    case 38: // UP Keys
                    case 39: //Right Keys
                    case 40://Down Keys
                        tAnsiChar = tWPBytes[0];
                        tUniChar = System.BitConverter.ToUInt16(tWPBytes, 0);

                        KeyInfo tControlKey = KeyMap.Find(tScanCode, Convert.ToBoolean(tFlags & 0x01), "Key", m_Parent.Modes.Flags);

                        this.m_LastKeyDownSent = false;

                        if (tControlKey != null)
                        {
                            this.m_LastKeyDownSent = true;
                            OnControlKeyBoardEvent(this, tControlKey);
                        }
                        break;
                    default:
                        break;
                }
            }

            else if (aKeyMessage.Msg == WMCodes.WM_SYSCHAR || aKeyMessage.Msg == WMCodes.WM_CHAR)
            {
                tAnsiChar = tWPBytes[0];
                tUniChar = System.BitConverter.ToUInt16(tWPBytes, 0);


                if (this.m_LastKeyDownSent == false)
                {
                    OnKeyboardEvent(this, System.Convert.ToChar(tAnsiChar).ToString());
                }
            }
        }
        /// <summary>
        /// Tab을 전송 합니다.
        /// </summary>
        public void SendTab()
        {
            OnKeyboardEvent(this, "\t");
        }
    }
}





