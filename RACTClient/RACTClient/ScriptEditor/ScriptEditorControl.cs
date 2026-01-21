using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using RACTCommonClass;


namespace RACTClient
{
    public partial class ScriptEditorControl : UserControl
    {
        String m_Text;
        String[] m_Lines;
        /// <summary>
        /// 선택 시작 위치 입니다.
        /// </summary>
        int m_SelectStart;
        /// <summary>
        /// 선택 종료 위치 입니다.
        /// </summary>
        int m_SelectEnd;
        bool m_Careton;
        /// <summary>
        /// 케럿 Rect 입니다.
        /// </summary>
        Rectangle m_Rcaret;
        int[] m_CharColor;
        List<int> m_Breakpoints;
        int m_LineMarker;
        /// <summary>
        /// 화면에 출력할 문자 포멧 입니다.
        /// </summary>
        StringFormat m_StringFormat;
        /// <summary>
        /// 글자 색상 입니다.
        /// </summary>
        SolidBrush m_TextBrush;
        int m_Lastcolor;
        /// <summary>
        /// 문자 넓이입니다.
        /// </summary>
        float m_CharWidth;
        /// <summary>
        /// 문자 높이 입니다.
        /// </summary>
        int m_CharHeight;
        int m_Lastx;
        /// <summary>
        /// Tab 넓이 입니다.
        /// </summary>
        float m_TabWidth;
        SyntaxColor[] m_SyntColors;
        /// <summary>
        /// Undo 목록 입니다.
        /// </summary>
        List<UndoUnit> m_Undos = new List<UndoUnit>();
        /// <summary>
        /// Undo 개수 입니다.
        /// </summary>
        int m_UndoCount;
        static String m_Search;
        /// <summary>
        /// 읽기 여부 입니다.
        /// </summary>
        bool m_Readonly;
        Timer m_Timer;

        public ScriptEditorControl()
        {
            BackColor = SystemColors.Window;
            AutoScroll = true;
            DoubleBuffered = true;
            Font = new Font("Courier New", 13, FontStyle.Regular, GraphicsUnit.Pixel);
            m_LineMarker = -1;
            m_Timer = new Timer();
            m_Timer.Interval = 500;
            m_Timer.Tick += new EventHandler(OnTimer);
            m_Text = "";
            SyntaxColors = new SyntaxColor[] 
          {
            new SyntaxColor("[Comment]", Color.Green),
            new SyntaxColor("[String]", Color.Purple),
            new SyntaxColor("using", Color.Blue),
            new SyntaxColor("new", Color.Blue),
            new SyntaxColor("typeof", Color.Blue),
            new SyntaxColor("int", Color.Blue),
            new SyntaxColor("float", Color.Blue),
            new SyntaxColor("double", Color.Blue),
            new SyntaxColor("string", Color.Blue),
            new SyntaxColor("for", Color.Blue),
            new SyntaxColor("foreach", Color.Blue),
            new SyntaxColor("if", Color.Blue),
            new SyntaxColor("break", Color.Blue),
            new SyntaxColor("continue", Color.Blue),
            new SyntaxColor("try", Color.Blue),
            new SyntaxColor("return", Color.Blue),
            new SyntaxColor("catch", Color.Blue),
            new SyntaxColor("throw", Color.Blue),
            new SyntaxColor("void", Color.Blue),
            new SyntaxColor("var", Color.Blue),
            new SyntaxColor("as", Color.Blue),
            new SyntaxColor("is", Color.Blue),
            new SyntaxColor("in", Color.Blue),
            new SyntaxColor("this", Color.Blue),
            new SyntaxColor("true", Color.Blue),
            new SyntaxColor("false", Color.Blue),
                new SyntaxColor("For", Color.Blue),
                new SyntaxColor("Next", Color.Blue),
                new SyntaxColor("To", Color.Blue),
                new SyntaxColor("Sub", Color.Blue),
                new SyntaxColor("End", Color.Blue),

          };

        }
        /// <summary>
        /// 선택 합니다.
        /// </summary>
        /// <param name="a"></param>
        public void Select(int aIndex)
        {
            Select(aIndex, aIndex);
        }
        /// <summary>
        /// 선택 합니다.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public void Select(int aStart, int aEnd)
        {
            if ((m_SelectStart == aStart) && (m_SelectEnd == aEnd)) return;
            m_SelectStart = aStart; m_SelectEnd = aEnd;
            if (!this.IsHandleCreated) return;

            bool tWar = m_Timer.Enabled;
            m_Timer.Enabled = false;
            m_Careton = true;
            m_Timer.Enabled = tWar;

            Invalidate();
            OnSelChanged();

            m_Lastx = 0;
        }
        /// <summary>
        /// 스크롤 처리 합니다.
        /// </summary>
        public void ScrollVisible()
        {
            int tHeight = 4 + LineFromPos(m_SelectEnd) * m_CharHeight;
            if (tHeight + AutoScrollPosition.Y < 0)
            {
                AutoScrollPosition = new Point(0, tHeight);
                Update();
            }
            else
            {
                if (tHeight + m_CharHeight + AutoScrollPosition.Y > Height)
                {
                    AutoScrollPosition = new Point(0, tHeight + m_CharHeight - Height);
                    Update();
                }
            }
        }
        /// <summary>
        /// 변환 처리 합니다.
        /// </summary>
        /// <param name="s"></param>
        private void Replace(String aString)
        {
            int tMin = Math.Min(m_SelectStart, m_SelectEnd);
            int tMax = Math.Max(m_SelectStart, m_SelectEnd);

            UndoUnit tUndo = new UndoUnit();
            tUndo.UndoString = !String.IsNullOrEmpty(aString) ? aString : null;
            tUndo.StartIndex = tMin;
            tUndo.Count = tMax - tMin;
            tUndo.Execute(this);

            if (m_Undos.Count > m_UndoCount)
            {
                m_Undos.RemoveRange(m_UndoCount, m_Undos.Count - m_UndoCount);
            }
            m_Undos.Add(tUndo);
            m_UndoCount++;
        }
        /// <summary>
        /// Undo 목록을 삭제 합니다.
        /// </summary>
        public void ClearUndo()
        {
            m_Undos.Clear();
            m_UndoCount = 0;
        }
        /// <summary>
        /// 읽기전용 속성을 가져오거나 설정 합니다. 입니다.
        /// </summary>
        public bool ReadOnly
        {
            get { return m_Readonly; }
            set
            {
                if (m_Readonly == value) return;
                m_Readonly = value;
                BackColor = m_Readonly ? SystemColors.Control : SystemColors.Window;
            }
        }

        public override string  Text
{
	   get { return m_Text; }
            set
            {
                ClearUndo();
                m_SelectStart = m_SelectEnd = 0;
                m_Text = value;
                Format();
                OnTextChanged();
            }
}

        public bool IsModified { get { return m_UndoCount > 0; } }
        public String GetSelectedText()
        {
            int a = Math.Min(m_SelectStart, m_SelectEnd);
            int b = Math.Max(m_SelectStart, m_SelectEnd);
            return m_Text.Substring(a, b - a);
        }
        public int SelStart { get { return m_SelectStart; } }
        public int SelEnd { get { return m_SelectEnd; } }
        public int SelMin { get { return Math.Min(m_SelectStart, m_SelectEnd); } }
        public int SelMax { get { return Math.Max(m_SelectStart, m_SelectEnd); } }
        public int LineMarker { get { return m_LineMarker; } set { if (m_LineMarker != value) { m_LineMarker = value; Invalidate(); } } }
        public List<int> BreakPoints
        {
            get { return m_Breakpoints; }
            set
            {
                m_Breakpoints = (value != null) && (value.Count > 0) ? value : null;
                Invalidate();
            }
        }

        public int Cut( )
        {
            if (m_Readonly) return 0;
            if (m_SelectStart == m_SelectEnd) return 0;
        
            Copy();
            Replace("");
            return 1;
        }
        public int Copy( )
        {
            if (m_SelectStart == m_SelectEnd) return 0x10;
       
            int a = Math.Min(m_SelectStart, m_SelectEnd), b = Math.Max(m_SelectStart, m_SelectEnd);
            Clipboard.Clear();
            Clipboard.SetText(m_Text.Substring(a, b - a));
            return 1;
        }
        public int Paste()
        {
            if (m_Readonly) return 0;
            if (!Clipboard.ContainsText()) return 0;
        
            Replace(Clipboard.GetText());
            return 1;
        }
        public int Undo( )
        {
            if (m_Readonly) return 0;
            if (m_UndoCount == 0) return 0;
        
            m_Undos[--m_UndoCount].Execute(this);
            return 1;
        }
        public int Redo( )
        {
            if (m_Readonly) return 0;
            if (m_UndoCount >= m_Undos.Count) return 0x10;
         
            m_Undos[m_UndoCount++].Execute(this);
            return 1;
        }
        public int Delete( )
        {
            if (m_Readonly) return 0;
            return 0;
        }

        public void SelectAll()
        {
            Select(0, m_Text.Length);
        }

        public void Search()//Strg Shift F3
        {
            MessageBox.Show("Search");
        }
        public int SearchForward(object test) //Strg F3
        {
            if ((m_Search == null) && (m_SelectStart == m_SelectEnd)) return 0x10;
            if (test != null) return 1;
            if (m_SelectStart != m_SelectEnd) m_Search = GetSelectedText();
            return SearchNext(test);
        }
        public int SearchNext(object test) //F3
        {
            if (m_Search == null) return 0x10;
            if (test != null) return 1;
            int t = Math.Max(m_SelectStart, m_SelectEnd);

            int i = m_Text.IndexOf(m_Search, t);
            if (i < 0) i = m_Text.IndexOf(m_Search, 0);

            if (i >= 0)
            {
                Select(i, i + m_Search.Length);
                ScrollVisible();
                return 1;
            }
            //MessageBeep(-1);
            return 1;
        }
        public int SearchPrev(object test) //Shift F3
        {
            if (m_Search == null) return 0x10;
            if (test != null) return 1;

            int t = Math.Min(m_SelectStart, m_SelectEnd);

            int last = -1;
            for (int i = t; ((i = m_Text.IndexOf(m_Search, i)) >= 0); ) { last = i; i += m_Search.Length; }
            for (int i = 0; ((i = m_Text.IndexOf(m_Search, i)) >= 0) && (i < t); ) { last = i; i += m_Search.Length; }

            if (last >= 0)
            {
                Select(last, last + m_Search.Length); ScrollVisible();
                return 1;
            }
            //MessageBeep(-1);
            return 1;
        }

        protected virtual void OnSelChanged() { }

        /// <summary>
        /// 라인 개수를 가져오기 합니다.
        /// </summary>
        /// <param name="aPosition"></param>
        /// <returns></returns>
        public int LineFromPos(int aPosition)
        {
            String tTempString = m_Text;
            if (aPosition > tTempString.Length)
            {
                aPosition = tTempString.Length;
            }

            int tLineCount = 0;
            for (int i = 0; i < aPosition; i++)
            {
                if (tTempString[i] == '\n')
                {
                    tLineCount++;
                }
            }
            return tLineCount;
        }

        public int GetLineStart(int line)
        {
            if (line == 0) return 0; String s = m_Text; int j = 0;
            for (int l = 0, n = s.Length; j < n; j++)
            {
                if (l == line) return j;
                if (s[j] == '\n') l++;
            }
            return j;
        }
        public int GetLineEnd(int line)
        {
            String s = m_Text; int j = 0;
            for (int l = 0, n = s.Length; j < n; j++)
            {
                if (s[j] == '\n')
                {
                    if (l == line)
                        return (j > 0) && (s[j - 1] == '\r') ? j - 1 : j;
                    l++;
                }
            }
            return j;
        }
        int PosFromPoint(Point p)
        {
            if (m_Lines == null) InitializeLine();

            Rectangle r = new Rectangle(16, AutoScrollPosition.Y + 4, 0x40000000, m_CharHeight);
            p.X = Math.Max(r.Left, p.X);
            p.Y = Math.Max(r.Top, p.Y);
            for (int i = 0, t = 0; i < m_Lines.Length; t += m_Lines[i++].Length + 2, r.Offset(0, r.Height))
                if (r.Contains(p))
                {
                    for (int l = 0; l < m_Lines[i].Length; l++)
                    {
                        int x1 = r.Left + (int)_xoffs(m_Lines[i], l + 0);
                        int x2 = r.Left + (int)_xoffs(m_Lines[i], l + 1);
                        if (x2 > p.X)
                            return t + (p.X <= ((x1 + x2) >> 1) ? l : l + 1);
                    }

                    return t + m_Lines[i].Length;
                }

            return m_Text.Length;
        }
        bool WordFromPoint(int i, ref Point ab)
        {
            if (i == m_Text.Length) return false;
            int a = i; for (; a > 0; a--) if (!IsWordChar(m_Text[a - 1])) break;
            int b = i; for (; b < m_Text.Length; b++) if (!IsWordChar(m_Text[b])) break;
            ab.X = a; ab.Y = b;
            return true;
        }
        /// <summary>
        /// 라인을 초기화 합니다.
        /// </summary>
        private void InitializeLine()
        {
            if (m_CharWidth == 0)
            {
                m_StringFormat = new StringFormat(StringFormat.GenericTypographic);
                Graphics tGraphics = this.CreateGraphics();
                SizeF tCharSize = tGraphics.MeasureString("X", this.Font, 0, m_StringFormat);
                tGraphics.Dispose();
                m_CharWidth = tCharSize.Width;
                m_CharHeight = (int)tCharSize.Height + 1;//Math.Ceiling(charsize.Height);
                m_TabWidth = 2 * m_CharWidth;
                //stringformat.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
                m_StringFormat.SetTabStops(0, new Single[] { m_TabWidth });
            }
            m_Lines = m_Text.Split(new String[] { "\r\n" }, System.StringSplitOptions.None);
        }
        float _xoffs(String s, int n)
        {
            float dx = 0;
            for (int i = 0; i < n; i++)
            {
                if ((i < s.Length) && (s[i] == '\t'))
                {
                    dx = ((int)(dx / m_TabWidth) + 1) * m_TabWidth;
                    continue;
                }
                dx += m_CharWidth;
            }
            return dx;
        }

        void _synaxcolor()
        {
            String tTempString = m_Text;
            int tStringLength = tTempString.Length;
            Array.Resize(ref m_CharColor, tStringLength + 1);

            if (m_SyntColors == null) return;

            int tColorComment = 0;
            int tColorString = 0;

            for (int i = 0; i < m_SyntColors.Length; i++)
            {
                SyntaxColor tSyntaxColor = m_SyntColors[i];
                if (String.IsNullOrEmpty(tSyntaxColor.SyntaxString)) continue;
                if (tSyntaxColor.SyntaxString[0] != '[') continue;
                if (tSyntaxColor.SyntaxString == "[String]") tColorString = tSyntaxColor.Color.ToArgb();
                if (tSyntaxColor.SyntaxString == "[Comment]") tColorComment = tSyntaxColor.Color.ToArgb();
            }

            for (int j = 0; j < tStringLength - 1; j++)
            {
                m_CharColor[j] = 0;

                if ((tTempString[j] == '/') && (tTempString[j + 1] == '*'))
                {
                    for (int t = 0; j < tStringLength; j++, t++)
                    {
                        m_CharColor[j] = tColorComment;//(0,128,0);
                        if ((t > 2) && (tTempString[j - 1] == '*') && (tTempString[j] == '/')) break;
                    }
                    continue;
                }

                //        if ((s[j] == '/') && (s[j + 1] == '/'))
                if ((tTempString[j] == '\''))
                {
                    for (; j < tStringLength; j++)
                    {
                        if ((tTempString[j] == 13) || (tTempString[j] == 10)) break;
                        m_CharColor[j] = tColorComment;
                    }
                    continue;
                }

                if ((tTempString[j] == '"') || (tTempString[j] == '\''))
                    for (int t = j + 1; t < tStringLength; t++)
                    {
                        if (tTempString[t] == '\\') continue;
                        if ((tTempString[t] == 13) || (tTempString[t] == 10)) break;
                        if (tTempString[t] == tTempString[j])
                        {
                            for (; j <= t; j++) m_CharColor[j] = tColorString; j--;
                            break;
                        }
                    }

                if (char.IsLetter(tTempString[j]))
                    if ((j == 0) || !char.IsLetter(tTempString[j - 1]))
                    {
                        int t = j + 1; for (; t < tStringLength; t++) if (!IsWordChar(tTempString[t])) break;

                        int color = 0;

                        for (int x = 0; x < m_SyntColors.Length; x++)
                            if (!String.IsNullOrEmpty(m_SyntColors[x].SyntaxString) && (m_SyntColors[x].SyntaxString[0] != '['))
                                if (m_SyntColors[x].SyntaxString.Length == t - j)
                                    if (String.Compare(m_SyntColors[x].SyntaxString, 0, tTempString, j, t - j) == 0)
                                    {
                                        color = m_SyntColors[x].Color.ToArgb();
                                        break;
                                    }

                        if (color != 0)
                        {
                            for (; j < t; j++) m_CharColor[j] = color; j--;
                            continue;
                        }
                    }

            }

            if (tStringLength != 0) m_CharColor[tStringLength - 1] = tStringLength > 1 ? m_CharColor[tStringLength - 2] : 0;
        }
        void _setcolor(int c)
        {
            if ((m_TextBrush != null) && (m_Lastcolor == c)) return;
            if (m_TextBrush != null) m_TextBrush.Dispose();
            m_Lastcolor = c;
            m_TextBrush = new SolidBrush(Color.FromArgb(c | unchecked((int)0xff000000)));
        }
        static bool IsWordChar(char c)
        {
            return char.IsLetterOrDigit(c) || (c == '_');
        }
        /// <summary>
        /// Syntax Color 구조체 입니다.
        /// </summary>
        public struct SyntaxColor
        {
            /// <summary>
            /// 문자 입니다.
            /// </summary>
            private String m_SyntaxString;
            /// <summary>
            /// 색상 입니다.
            /// </summary>
            private Color m_Color;

            /// <summary>
            /// 문자열 변환 합니다.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return SyntaxString + " " + Color.ToString(); // GetType().Name;
            }
            /// <summary>
            /// 기본 생성자 입니다.
            /// </summary>
            /// <param name="aString"></param>
            /// <param name="aColor"></param>
            public SyntaxColor(String aString, Color aColor)
            {
                this.m_SyntaxString = aString;
                this.m_Color = aColor;
            }
           
            public String SyntaxString
            {
                get { return m_SyntaxString; }
                set { m_SyntaxString = value; }
            }
            public Color Color
            {
                get { return m_Color; }
                set { m_Color = value; }
            }
        };
        public virtual SyntaxColor[] SyntaxColors
        {
            get { return m_SyntColors; }
            set
            {
                m_SyntColors = value;
                if (m_CharColor == null) return;
                _synaxcolor();
                Invalidate();
            }
        }

        void replace(int i, int n, string s)
        {
            if (m_Breakpoints != null)
                for (int k = 0; k < m_Breakpoints.Count; k++)
                    m_Breakpoints[k] = GetLineStart(m_Breakpoints[k]);

            m_Text = m_Text.Substring(0, i) + s + m_Text.Substring(i + n, m_Text.Length - (i + n));

            if (m_Breakpoints != null)
                for (int k = 0; k < m_Breakpoints.Count; k++)
                {
                    int t = m_Breakpoints[k];
                    if (t >= i)
                    {
                        if (t < i + n)
                        {
                            m_Breakpoints.RemoveAt(k--);
                            continue;
                        }
                        t -= n;
                        t += s.Length;
                    }
                    m_Breakpoints[k] = LineFromPos(t);
                }
            Format();
            OnTextChanged();
        }

        /// <summary>
        /// Undo 클래스 입니다.
        /// </summary>
        private class UndoUnit
        {
            /// <summary>
            /// 시작 위치 입니다.
            /// </summary>
            private int m_StartIndex;
            /// <summary>
            /// 개수 입니다.
            /// </summary>
            private int m_Count;
            /// <summary>
            /// Undo 문자 입니다.
            /// </summary>
            private String m_UndoString;

            /// <summary>
            /// 실행 합니다.
            /// </summary>
            /// <param name="ctrl"></param>
            public void Execute(ScriptEditorControl aEditor)
            {
                String tString = m_Count != 0 ? aEditor.m_Text.Substring(m_StartIndex, m_Count) : null;
                aEditor.replace(m_StartIndex, m_Count, m_UndoString != null ? m_UndoString : "");
                m_Count = m_UndoString != null ? UndoString.Length : 0;
                m_UndoString = tString;
                aEditor.Select(m_StartIndex + m_Count); aEditor.ScrollVisible();
            }

            /// <summary>
            /// 시작 위치를 가져오거나 설정 합니다.
            /// </summary>
            public int StartIndex
            {
                get { return m_StartIndex; }
                set { m_StartIndex = value; }
            }
            /// <summary>
            /// 개수를 가져오거나 설정 합니다.
            /// </summary>
            public int Count
            {
                get { return m_Count; }
                set { m_Count = value; }
            }
            /// <summary>
            /// Undo 문자를 가져오거나 설정 합니다.
            /// </summary>
            public String UndoString
            {
                get { return m_UndoString; }
                set { m_UndoString = value; }
            }
            
        };

        private void Format()
        {
            InitializeLine();
            m_SelectStart = Math.Min(m_SelectStart, m_Text.Length);
            m_SelectEnd = Math.Min(m_SelectEnd, m_Text.Length);
            AutoScrollMinSize = new Size(0, m_Lines.Length * m_CharHeight + m_CharHeight);
            _synaxcolor();
            Invalidate();
        }
        protected virtual void OnTextChanged()
        {
            if (EditTextChanged != null) EditTextChanged(this, EventArgs.Empty);
        }
        public event EventHandler EditTextChanged;

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics tGraphics = e.Graphics;
            String tScriptText = this.m_Text;
            m_Rcaret.Width = 0;
            RectangleF tClipBounds = tGraphics.ClipBounds;
            Rectangle tRect = new Rectangle((int)tClipBounds.Left, (int)tClipBounds.Top, (int)tClipBounds.Width + 1, (int)tClipBounds.Height + 1);

            if (m_Lines == null) InitializeLine();

            int tMin = Math.Min(m_SelectStart, m_SelectEnd);
            int tMax = Math.Max(m_SelectStart, m_SelectEnd);

            //graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            Rectangle tRectScroll = new Rectangle(16, AutoScrollPosition.Y + 4, 0x40000000, m_CharHeight);
            for (int i = 0, t = 0; i < m_Lines.Length; t += m_Lines[i++].Length + 2, tRectScroll.Offset(0, tRectScroll.Height))
            {
                String tLine = m_Lines[i];
                if (tRect.IntersectsWith(tRectScroll))
                {
                    if (m_CharColor != null)
                    {
                        int tLength = tLine.Length;
                        for (int aa = 0, bb = 0; ; bb++)
                            if ((m_CharColor[t + aa] != m_CharColor[t + bb]) || (bb == tLength))
                            {
                                _setcolor(m_CharColor[t + aa]);
                                if ((aa == 0) && (bb == tLength))
                                {
                                    tGraphics.DrawString(tLine, this.Font, m_TextBrush, tRectScroll, m_StringFormat);
                                }
                                else
                                {
                                    //Rectangle rr=r; rr.X = (int)Math.Round(rr.X+_xoffs(line,a));
                                    //graphics.DrawString(line.Substring(a,b-a),this.Font,textbrush,rr,stringformat);

                                    Rectangle rr = Rectangle.FromLTRB(tRectScroll.Left + (int)_xoffs(tLine, aa) + 0, tRectScroll.Top, tRectScroll.Left + (int)_xoffs(tLine, bb) + 1, tRectScroll.Bottom);
                                    tGraphics.SetClip(rr);
                                    tGraphics.DrawString(tLine, this.Font, m_TextBrush, tRectScroll, m_StringFormat);
                                    tGraphics.ResetClip();
                                }
                                if (bb == tLength) break;
                                aa = bb;
                            }
                    }
                    else
                    {
                        tGraphics.DrawString(tLine, this.Font, Brushes.Black, tRectScroll, m_StringFormat);
                    }

                    if (m_Breakpoints != null)
                        for (int tt = 0; tt < m_Breakpoints.Count; tt++)
                            if (m_Breakpoints[tt] == i)
                            {
                                tGraphics.FillEllipse(Brushes.DarkRed, new Rectangle(3, tRectScroll.Top + 3, 10, 10));
                                break;
                            }

                    if (m_LineMarker == i)
                    {
                        Point[] pp = new Point[] { new Point(6, tRectScroll.Top + 2), new Point(6 + 7, tRectScroll.Top + 2 + 6), new Point(6, tRectScroll.Top + 2 + 12) };
                        tGraphics.FillPolygon(Brushes.Green, pp);
                    }
                }

                if (tMax >= t)
                    if (tMin <= t + tLine.Length)
                    {
                        int i1 = Math.Max(tMin - t, 0);
                        int i2 = Math.Min(tMax - t, tLine.Length);

                        Rectangle rr = Rectangle.FromLTRB(tRectScroll.Left + (int)_xoffs(tLine, i1) + 0, tRectScroll.Top, tRectScroll.Left + (int)_xoffs(tLine, i2) + 1, tRectScroll.Bottom);

                        if ((tMin != tMax) && (tMax > t))
                            if (tRect.IntersectsWith(tRectScroll))
                            {
                                if ((i2 >= tLine.Length) && ((t + i2) != m_SelectEnd)) rr.Width += 8;

                                tGraphics.FillRectangle(Focused ? SystemBrushes.Highlight : SystemBrushes.GradientInactiveCaption, rr);

                                tGraphics.SetClip(rr);
                                tGraphics.DrawString(tLine, this.Font, Focused ? SystemBrushes.HighlightText : Brushes.Black, tRectScroll, m_StringFormat);
                                tGraphics.ResetClip();
                            }

                        if (((t + i1) == m_SelectEnd) || ((t + i2) == m_SelectEnd))
                        {
                            m_Rcaret = rr;
                            if ((t + i2) == m_SelectEnd) m_Rcaret.X = m_Rcaret.Right;
                            m_Rcaret.Width = 1;
                            m_Rcaret.X--;
                            if (Focused && m_Careton)
                                tGraphics.FillRectangle(Brushes.Black, m_Rcaret);
                        }
                    }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            int i = PosFromPoint(e.Location); //System.Diagnostics.Debug.WriteLine(i);
            if (Capture)
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    Select(m_SelectStart, i);
                    ScrollVisible();
                }
            }
            else
            {
                /*
                Point ab; 
                if(WordFromPoint(i,ab)) 
                {
                  String s=Text.Substring(ab.X,ab.Y-ab.X);
                  if(this.toolTip1.GetToolTip(this)!=s)
                  {
                    this.toolTip1.SetToolTip(this,s);
                  }
                }
                */
            }
            Cursor = Cursors.IBeam;
            base.OnMouseMove(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                Select(PosFromPoint(e.Location));
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            Point ab = new Point(); if (WordFromPoint(PosFromPoint(e.Location), ref ab)) Select(ab.X, ab.Y);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            Invalidate();
            m_Careton = true;
            m_Timer.Enabled = true;
            base.OnGotFocus(e);
        }
        protected override void OnLostFocus(EventArgs e)
        {
            Invalidate();
            m_Timer.Enabled = false;
            base.OnLostFocus(e);
        }
        private bool m_IsPressShift = false;
        private int m_ShiftPosition = 0;
        private int m_ShiftEndPosition = 0;

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.ShiftKey:
                    m_IsPressShift = false;
                    m_ShiftEndPosition = 0;
                    m_ShiftPosition = 0;
                    break;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            int tCurrentPosition;
            switch (e.KeyCode)
            {
                case Keys.Shift:
                    break;
                case Keys.ShiftKey:
                    m_ShiftPosition = m_SelectStart;
                    m_IsPressShift = true;
                    break;
                case Keys.Home:
                    if (m_IsPressShift)
                    {
                        tCurrentPosition = m_SelectStart;
                        while (true)
                        {
                            tCurrentPosition--;
                            if (m_Text[tCurrentPosition] == '\n')
                            {
                                tCurrentPosition++;
                                break;
                            }
                        }
                        Select(tCurrentPosition, m_ShiftPosition); ScrollVisible();
                    }
                    else
                    {
                        if (m_SelectStart == m_SelectEnd)
                        {
                            tCurrentPosition = m_SelectStart;
                            if (tCurrentPosition == 0) break;
                            while (true)
                            {
                                tCurrentPosition--;
                                if (m_Text[tCurrentPosition] == '\n')
                                {
                                    tCurrentPosition++;
                                    break;
                                }
                            }

                            Select(tCurrentPosition); ScrollVisible();
                        }
                    }
                   
                    break;
                case Keys.End:
                    if (m_IsPressShift)
                    {
                        tCurrentPosition = m_SelectStart;
                        while (true)
                        {
                            tCurrentPosition++;
                            if (m_Text[tCurrentPosition] == '\n')
                            {
                                tCurrentPosition--;
                                break;
                            }
                        }
                        Select(tCurrentPosition, m_ShiftPosition); ScrollVisible();
                    }
                    else
                    {
                        if (m_SelectStart == m_SelectEnd)
                        {
                            tCurrentPosition = m_SelectStart;
                            while (true)
                            {
                                if (m_Text[tCurrentPosition] == '\n')
                                {
                                    tCurrentPosition--;
                                    break;
                                }
                                tCurrentPosition++;
                            }

                            Select(tCurrentPosition); ScrollVisible();
                        }
                    }
                    break;
                case Keys.Delete:
                    if (m_SelectStart == m_SelectEnd)
                    {
                        tCurrentPosition = m_SelectEnd;
                        if (tCurrentPosition >= m_Text.Length) return;
                        tCurrentPosition++; if ((tCurrentPosition < m_Text.Length) && (m_Text[tCurrentPosition] == '\n')) tCurrentPosition++;
                        Select(m_SelectStart, tCurrentPosition); ScrollVisible();
                    }
                    if (m_Readonly) return;
                    Replace("");
                    break;

                case Keys.Left:
                    if (m_IsPressShift)
                    {
                        System.Diagnostics.Debug.WriteLine(string.Concat("## ",m_SelectStart-1,"  ",m_ShiftPosition));

                        Select(m_SelectStart - 1);
                        Select(m_SelectStart -1,m_ShiftPosition); ScrollVisible();
                    }
                    else
                    {
                        if (m_SelectStart == m_SelectEnd)
                        {
                            tCurrentPosition = m_SelectStart;
                            if (tCurrentPosition == 0) break;
                            tCurrentPosition--; if (m_Text[tCurrentPosition] == '\n') tCurrentPosition--;
                            Select(tCurrentPosition); ScrollVisible();
                        }
                        else
                        {
                            Select(Math.Min(m_SelectStart, m_SelectEnd)); ScrollVisible();
                        }
                    }
                    break;
                case Keys.Right:
                    if (m_IsPressShift)
                    {
                        System.Diagnostics.Debug.WriteLine(string.Concat("## ", m_ShiftPosition, "  ", m_SelectStart + 1));
                        Select(m_SelectStart + 1, m_ShiftPosition); ScrollVisible();
                    }
                    else
                    {
                        if (m_SelectStart == m_SelectEnd)
                        {
                            tCurrentPosition = m_SelectStart;
                            if (tCurrentPosition >= m_Text.Length) break;
                            tCurrentPosition++; if ((tCurrentPosition < m_Text.Length) && (m_Text[tCurrentPosition] == '\n')) tCurrentPosition++;
                            Select(tCurrentPosition); ScrollVisible();
                        }
                        else
                        {
                            Select(Math.Max(m_SelectStart, m_SelectEnd)); ScrollVisible();
                        }
                    }
                    break;

                case Keys.Up:
                   
                        if (m_IsPressShift)
                        {
                           

                            Point tPoint = m_Rcaret.Location;
                            tPoint.Y -= m_CharHeight;
                            if (m_Lastx != 0) tPoint.X = m_Lastx;
                            m_Rcaret.Location = tPoint;
                            System.Diagnostics.Debug.WriteLine(string.Concat("## ", PosFromPoint(tPoint), "  ", m_ShiftPosition));
                            Select(PosFromPoint(tPoint),m_ShiftPosition);
                            ScrollVisible();
                            m_Lastx = tPoint.X;
                        }
                        else
                        {
                            Point tPoint = m_Rcaret.Location; tPoint.Y -= m_CharHeight; if (m_Lastx != 0) tPoint.X = m_Lastx;
                            Select(PosFromPoint(tPoint)); ScrollVisible();
                            m_Lastx = tPoint.X;
                        }
                 
                    break;

                case Keys.Down:
                   
                        if (m_IsPressShift)
                        {
                            Point tPoint = m_Rcaret.Location; tPoint.Y += m_CharHeight; if (m_Lastx != 0) tPoint.X = m_Lastx;
                            Select(PosFromPoint(tPoint), m_ShiftPosition); ScrollVisible();
                            m_Lastx = tPoint.X;
                        }
                        else
                        {
                            Point tPoint = m_Rcaret.Location; tPoint.Y += m_CharHeight; if (m_Lastx != 0) tPoint.X = m_Lastx;
                            Select(PosFromPoint(tPoint)); ScrollVisible();
                            m_Lastx = tPoint.X;
                        }
               
                    break;
            }

        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            if (m_Readonly) return;

            if (e.KeyChar == (char)13)
            {
                String tText = m_Text;
                int tSelectMin = SelMin; for (; (tSelectMin > 0) && (tText[tSelectMin - 1] != '\n'); tSelectMin--) ;
                int tBackupSelectMin = tSelectMin; for (; (tBackupSelectMin < tText.Length) && ((tText[tBackupSelectMin] == ' ') || (tText[tBackupSelectMin] == '\t')); tBackupSelectMin++) ;
                Replace("\r\n" + tText.Substring(tSelectMin, tBackupSelectMin - tSelectMin));
                return;
            }
            if (e.KeyChar == (char)8)
            {
                if (m_SelectStart == m_SelectEnd)
                {
                    if (m_SelectStart == 0) return;
                    m_SelectStart--;
                    if (m_Text[m_SelectStart] == '\n')
                        m_SelectStart--;
                }
                Replace("");
                return;
            }
            if (e.KeyChar == '\t')
            {
                int tMinLineCount = LineFromPos(SelMin), tMaxLineCount = LineFromPos(SelMax);
                if (tMinLineCount != tMaxLineCount)
                {
                    int tLineStart = GetLineStart(tMinLineCount);
                    int tLineEnd = GetLineEnd(tMaxLineCount);
                    bool bout = (ModifierKeys & Keys.Shift) == Keys.Shift;

                    String tSelectString = m_Text.Substring(tLineStart, tLineEnd - tLineStart);
                    string tTabString = "";
                    Select(tLineStart, tLineEnd);
                    String[] tSplit = tSelectString.Split(new String[] { "\r\n" }, System.StringSplitOptions.None);
                    for (int i = 0; i < tSplit.Length; i++)
                    {
                        if (i != 0) tTabString += "\r\n";
                        if (bout)
                            tTabString += (tSplit[i].Length > 0) && ((tSplit[i][0] == '\t') || (tSplit[i][0] == ' ')) ? tSplit[i].Substring(1, tSplit[i].Length - 1) : tSplit[i];
                        else
                            tTabString += "\t" + tSplit[i];
                    }
                    if (tTabString == tSelectString) return;
                    Replace(tTabString);
                    Select(tLineStart, tLineStart + tTabString.Length);
                    return;
                }
                Replace(e.KeyChar.ToString());
                return;
            }

            if (!char.IsControl(e.KeyChar))
                Replace(e.KeyChar.ToString());

        }

        protected override bool IsInputKey(Keys keyData)
        {
            return true;
        }

        private void OnTimer(Object sender, EventArgs e)
        {
            m_Careton ^= true;// careton ? false : true;
            Invalidate(m_Rcaret);
        }

  


    }

}
