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
    public partial class CssEditor : UserControl
    {
        private string m_Text ="";
        private String[] m_Lines;
        private int m_Selstart, m_Selend;
        private bool m_Careton;
        private Rectangle m_Rcaret;
        private int[] m_Charcolor;
        private List<int> m_Breakpoints;
        private int m_Linemarker;
        private StringFormat m_Stringformat;
        private SolidBrush m_Textbrush;
        private int m_Lastcolor;
        private float m_Chardx;
        private int m_Chardy;
        private int m_Lastx;
        private float m_Tabdx;
        private SyntaxColor[] m_Syntcolors;
        private List<UndoUnit> m_Undos = new List<UndoUnit>();
        private int m_Iundo;
        private static String m_Search;
        private bool m_Readonly;
        private Timer m_Timer;

        public CssEditor()
        {
            BackColor = SystemColors.Window;
            AutoScroll = true;
            DoubleBuffered = true;
            Font = new Font("Courier New", 13, FontStyle.Regular, GraphicsUnit.Pixel);
            m_Linemarker = -1;
            m_Timer = new Timer();
            m_Timer.Interval = 500;
            m_Timer.Tick += new EventHandler(OnTimer);
            m_Text = "";

            m_Syntcolors = new SyntaxColor[] {
            new SyntaxColor("next", Color.Blue),
            new SyntaxColor("Sub", Color.Blue),
            new SyntaxColor("End", Color.Blue),
            new SyntaxColor("dim", Color.Blue),
            new SyntaxColor("For", Color.Blue),
            new SyntaxColor("To", Color.Blue),
            new SyntaxColor(Script.s_Send, Color.GreenYellow),
            new SyntaxColor(Script.s_WaitForString, Color.Brown),
          };
        }

        public void Select(int a)
        {
            Select(a, a);
        }
        public void Select(int a, int b)
        {
            if ((m_Selstart == a) && (m_Selend == b)) return;
            m_Selstart = a; m_Selend = b;
            if (!this.IsHandleCreated) return;

            bool war = m_Timer.Enabled;
            m_Timer.Enabled = false;
            m_Careton = true;
            m_Timer.Enabled = war;

            Invalidate();
            OnSelChanged();

            m_Lastx = 0;
        }
        public void ScrollVisible()
        {
            int y = 4 + LineFromPos(m_Selend) * m_Chardy;
            if (y + AutoScrollPosition.Y < 0)
            {
                AutoScrollPosition = new Point(0, y);
                Update();
            }
            else
                if (y + m_Chardy + AutoScrollPosition.Y > Height)
                {
                    AutoScrollPosition = new Point(0, y + m_Chardy - Height);
                    Update();
                }
        }
        void Replace(String s)
        {
            int a = Math.Min(m_Selstart, m_Selend);
            int b = Math.Max(m_Selstart, m_Selend);

            UndoUnit undo = new UndoUnit();
            undo.s = !String.IsNullOrEmpty(s) ? s : null;
            undo.i = a;
            undo.n = b - a;
            undo.Execute(this);

            if (m_Undos.Count > m_Iundo)
                m_Undos.RemoveRange(m_Iundo, m_Undos.Count - m_Iundo);
            m_Undos.Add(undo); m_Iundo++;
        }
        public void ClearUndo()
        {
            m_Undos.Clear(); m_Iundo = 0;
        }

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

        public String EditText
        {
            get { return m_Text; }
            set
            {
                ClearUndo(); m_Selstart = m_Selend = 0;
                m_Text = value;
                Format(); OnTextChanged();
            }
        }

        public bool IsModified { get { return m_Iundo > 0; } }
        public String GetSelectedText()
        {
            int a = Math.Min(m_Selstart, m_Selend);
            int b = Math.Max(m_Selstart, m_Selend);
            return m_Text.Substring(a, b - a);
        }
        public int SelStart { get { return m_Selstart; } }
        public int SelEnd { get { return m_Selend; } }
        public int SelMin { get { return Math.Min(m_Selstart, m_Selend); } }
        public int SelMax { get { return Math.Max(m_Selstart, m_Selend); } }
        public int LineMarker { get { return m_Linemarker; } set { if (m_Linemarker != value) { m_Linemarker = value; Invalidate(); } } }
        public List<int> BreakPoints
        {
            get { return m_Breakpoints; }
            set
            {
                m_Breakpoints = (value != null) && (value.Count > 0) ? value : null;
                Invalidate();
            }
        }

        public int Cut(object test)
        {
            if (m_Readonly) return 0;
            if (m_Selstart == m_Selend) return 0;
            if (test != null) return 1;
            Copy(test);
            Replace("");
            return 1;
        }
        public int Copy(object test)
        {
            if (m_Selstart == m_Selend) return 0x10;
            if (test != null) return 1;
            int a = Math.Min(m_Selstart, m_Selend), b = Math.Max(m_Selstart, m_Selend);
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
        public int Undo(object test)
        {
            if (m_Readonly) return 0;
            if (m_Iundo == 0) return 0;
            if (test != null) return 1;
            m_Undos[--m_Iundo].Execute(this);
            return 1;
        }
        public int Redo(object test)
        {
            if (m_Readonly) return 0;
            if (m_Iundo >= m_Undos.Count) return 0x10;
            if (test != null) return 1;
            m_Undos[m_Iundo++].Execute(this);
            return 1;
        }
        public int Delete(object test)
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
            if ((m_Search == null) && (m_Selstart == m_Selend)) return 0x10;
            if (test != null) return 1;
            if (m_Selstart != m_Selend) m_Search = GetSelectedText();
            return SearchNext(test);
        }
        public int SearchNext(object test) //F3
        {
            if (m_Search == null) return 0x10;
            if (test != null) return 1;
            int t = Math.Max(m_Selstart, m_Selend);

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

            int t = Math.Min(m_Selstart, m_Selend);

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

        public int LineFromPos(int pos)
        {
            String s = m_Text; if (pos > s.Length) pos = s.Length;
            int l = 0; for (int i = 0; i < pos; i++) if (s[i] == '\n') l++;
            return l;
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
            if (m_Lines == null) _initlines();

            Rectangle r = new Rectangle(16, AutoScrollPosition.Y + 4, 0x40000000, m_Chardy);
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

        void _initlines()
        {
            if (m_Chardx == 0)
            {
                m_Stringformat = new StringFormat(StringFormat.GenericTypographic);
                Graphics graphics = this.CreateGraphics();
                SizeF charsize = graphics.MeasureString("X", this.Font, 0, m_Stringformat);
                graphics.Dispose();
                m_Chardx = charsize.Width;
                m_Chardy = (int)charsize.Height + 1;//Math.Ceiling(charsize.Height);
                m_Tabdx = 2 * m_Chardx;
                //stringformat.FormatFlags = StringFormatFlags.MeasureTrailingSpaces;
                m_Stringformat.SetTabStops(0, new Single[] { m_Tabdx });
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
                    dx = ((int)(dx / m_Tabdx) + 1) * m_Tabdx;
                    continue;
                }
                dx += m_Chardx;
            }
            return dx;
        }
        void _synaxcolor()
        {
            String s = m_Text; int n = s.Length;
            Array.Resize(ref m_Charcolor, n + 1);

            if (m_Syntcolors == null)
                return;

            int colorcomment = 0;
            int colorstring = 0;

            for (int t = 0; t < m_Syntcolors.Length; t++)
            {
                SyntaxColor c = m_Syntcolors[t];
                if (String.IsNullOrEmpty(c.s)) continue;
                if (c.s[0] != '[') continue;
                if (c.s == "[String]") colorstring = c.c.ToArgb();
                if (c.s == "[Comment]") colorcomment = c.c.ToArgb();
            }

            for (int j = 0; j < n - 1; j++)
            {
                m_Charcolor[j] = 0;

                if ((s[j] == '/') && (s[j + 1] == '*'))
                {
                    for (int t = 0; j < n; j++, t++)
                    {
                        m_Charcolor[j] = colorcomment;//(0,128,0);
                        if ((t > 2) && (s[j - 1] == '*') && (s[j] == '/')) break;
                    }
                    continue;
                }

                if ((s[j] == '/') && (s[j + 1] == '/'))
                {
                    for (; j < n; j++)
                    {
                        if ((s[j] == 13) || (s[j] == 10)) break;
                        m_Charcolor[j] = colorcomment;
                    }
                    continue;
                }

                if ((s[j] == '"') || (s[j] == '\''))
                    for (int t = j + 1; t < n; t++)
                    {
                        if (s[t] == '\\') continue;
                        if ((s[t] == 13) || (s[t] == 10)) break;
                        if (s[t] == s[j])
                        {
                            for (; j <= t; j++) m_Charcolor[j] = colorstring; j--;
                            break;
                        }
                    }

                if (char.IsLetter(s[j]))
                    if ((j == 0) || !char.IsLetter(s[j - 1]))
                    {
                        int t = j + 1; for (; t < n; t++) if (!IsWordChar(s[t])) break;

                        int color = 0;

                        for (int x = 0; x < m_Syntcolors.Length; x++)
                            if (!String.IsNullOrEmpty(m_Syntcolors[x].s) && (m_Syntcolors[x].s[0] != '['))
                                if (m_Syntcolors[x].s.Length == t - j)
                                    if (String.Compare(m_Syntcolors[x].s, 0, s, j, t - j) == 0)
                                    {
                                        color = m_Syntcolors[x].c.ToArgb();
                                        break;
                                    }

                        if (color != 0)
                        {
                            for (; j < t; j++) m_Charcolor[j] = color; j--;
                            continue;
                        }
                    }

            }

            if (n != 0) m_Charcolor[n - 1] = n > 1 ? m_Charcolor[n - 2] : 0;
        }
        void _setcolor(int c)
        {
            if ((m_Textbrush != null) && (m_Lastcolor == c)) return;
            if (m_Textbrush != null) m_Textbrush.Dispose();
            m_Lastcolor = c;
            m_Textbrush = new SolidBrush(Color.FromArgb(c | unchecked((int)0xff000000)));
        }
        static bool IsWordChar(char c)
        {
            return char.IsLetterOrDigit(c) || (c == '_');
        }

        public struct SyntaxColor
        {
            public override string ToString()
            {
                return Token + " " + Color.ToString(); // GetType().Name;
            }
            public SyntaxColor(String s, Color c)
            {
                this.s = s;
                this.c = c;
            }
            public String s;
            public Color c;
            public String Token
            {
                get { return s; }
                set { s = value; }
            }
            public Color Color
            {
                get { return c; }
                set { c = value; }
            }
        };
        public virtual SyntaxColor[] SyntaxColors
        {
            get { return m_Syntcolors; }
            set
            {
                m_Syntcolors = value;
                if (m_Charcolor == null) return;
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

        class UndoUnit
        {
            public void Execute(CssEditor ctrl)
            {
                String os = n != 0 ? ctrl.m_Text.Substring(i, n) : null;
                ctrl.replace(i, n, s != null ? s : "");
                n = s != null ? s.Length : 0;
                s = os;
                ctrl.Select(i + n); ctrl.ScrollVisible();

            }
            public int i, n; public String s;
        };
        void Format()
        {
            _initlines();
            m_Selstart = Math.Min(m_Selstart, m_Text.Length);
            m_Selend = Math.Min(m_Selend, m_Text.Length);
            AutoScrollMinSize = new Size(0, m_Lines.Length * m_Chardy + m_Chardy);
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
            Graphics graphics = e.Graphics;
            String text = this.m_Text;
            m_Rcaret.Width = 0;
            RectangleF rcc = graphics.ClipBounds;
            Rectangle rc = new Rectangle((int)rcc.Left, (int)rcc.Top, (int)rcc.Width + 1, (int)rcc.Height + 1);

            if (m_Lines == null) _initlines();

            int a = Math.Min(m_Selstart, m_Selend);
            int b = Math.Max(m_Selstart, m_Selend);

            //graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            Rectangle r = new Rectangle(16, AutoScrollPosition.Y + 4, 0x40000000, m_Chardy);
            for (int i = 0, t = 0; i < m_Lines.Length; t += m_Lines[i++].Length + 2, r.Offset(0, r.Height))
            {
                String line = m_Lines[i];
                if (rc.IntersectsWith(r))
                {
                    if (m_Charcolor != null)
                    {
                        int n = line.Length;
                        for (int aa = 0, bb = 0; ; bb++)
                            if ((m_Charcolor[t + aa] != m_Charcolor[t + bb]) || (bb == n))
                            {
                                _setcolor(m_Charcolor[t + aa]);
                                if ((aa == 0) && (bb == n))
                                {
                                    graphics.DrawString(line, this.Font, m_Textbrush, r, m_Stringformat);
                                }
                                else
                                {
                                    //Rectangle rr=r; rr.X = (int)Math.Round(rr.X+_xoffs(line,a));
                                    //graphics.DrawString(line.Substring(a,b-a),this.Font,textbrush,rr,stringformat);

                                    Rectangle rr = Rectangle.FromLTRB(r.Left + (int)_xoffs(line, aa) + 0, r.Top, r.Left + (int)_xoffs(line, bb) + 1, r.Bottom);
                                    graphics.SetClip(rr);
                                    graphics.DrawString(line, this.Font, m_Textbrush, r, m_Stringformat);
                                    graphics.ResetClip();
                                }
                                if (bb == n) break;
                                aa = bb;
                            }
                    }
                    else
                    {
                        graphics.DrawString(line, this.Font, Brushes.Black, r, m_Stringformat);
                    }

                    if (m_Breakpoints != null)
                        for (int tt = 0; tt < m_Breakpoints.Count; tt++)
                            if (m_Breakpoints[tt] == i)
                            {
                                graphics.FillEllipse(Brushes.DarkRed, new Rectangle(3, r.Top + 3, 10, 10));
                                break;
                            }

                    if (m_Linemarker == i)
                    {
                        Point[] pp = new Point[] { new Point(6, r.Top + 2), new Point(6 + 7, r.Top + 2 + 6), new Point(6, r.Top + 2 + 12) };
                        graphics.FillPolygon(Brushes.Green, pp);
                    }
                }

                if (b >= t)
                    if (a <= t + line.Length)
                    {
                        int i1 = Math.Max(a - t, 0);
                        int i2 = Math.Min(b - t, line.Length);

                        Rectangle rr = Rectangle.FromLTRB(r.Left + (int)_xoffs(line, i1) + 0, r.Top, r.Left + (int)_xoffs(line, i2) + 1, r.Bottom);

                        if ((a != b) && (b > t))
                            if (rc.IntersectsWith(r))
                            {
                                if ((i2 >= line.Length) && ((t + i2) != m_Selend)) rr.Width += 8;

                                graphics.FillRectangle(Focused ? SystemBrushes.Highlight : SystemBrushes.GradientInactiveCaption, rr);

                                graphics.SetClip(rr);
                                graphics.DrawString(line, this.Font, Focused ? SystemBrushes.HighlightText : Brushes.Black, r, m_Stringformat);
                                graphics.ResetClip();
                            }

                        if (((t + i1) == m_Selend) || ((t + i2) == m_Selend))
                        {
                            m_Rcaret = rr;
                            if ((t + i2) == m_Selend) m_Rcaret.X = m_Rcaret.Right;
                            m_Rcaret.Width = 1;
                            m_Rcaret.X--;
                            if (Focused && m_Careton)
                                graphics.FillRectangle(Brushes.Black, m_Rcaret);
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
                    Select(m_Selstart, i);
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
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    if (m_Selstart == m_Selend)
                    {
                        int i = m_Selend;
                        if (i >= m_Text.Length) return;
                        i++; if ((i < m_Text.Length) && (m_Text[i] == '\n')) i++;
                        Select(m_Selstart, i); ScrollVisible();
                    }
                    if (m_Readonly) return;
                    Replace("");
                    break;

                case Keys.Left:
                    if (m_Selstart == m_Selend)
                    {
                        int i = m_Selstart;
                        if (i == 0) break;
                        i--; if (m_Text[i] == '\n') i--;
                        Select(i); ScrollVisible();
                    }
                    else
                    {
                        Select(Math.Min(m_Selstart, m_Selend)); ScrollVisible();
                    }
                    break;
                case Keys.Right:
                    if (m_Selstart == m_Selend)
                    {
                        int i = m_Selstart;
                        if (i >= m_Text.Length) break;
                        i++; if ((i < m_Text.Length) && (m_Text[i] == '\n')) i++;
                        Select(i); ScrollVisible();
                    }
                    else
                    {
                        Select(Math.Max(m_Selstart, m_Selend)); ScrollVisible();
                    }
                    break;

                case Keys.Up:
                    {
                        Point P = m_Rcaret.Location; P.Y -= m_Chardy; if (m_Lastx != 0) P.X = m_Lastx;
                        Select(PosFromPoint(P)); ScrollVisible();
                        m_Lastx = P.X;
                    }
                    break;

                case Keys.Down:
                    {
                        Point P = m_Rcaret.Location; P.Y += m_Chardy; if (m_Lastx != 0) P.X = m_Lastx;
                        Select(PosFromPoint(P)); ScrollVisible();
                        m_Lastx = P.X;
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
                String s = m_Text;
                int a = SelMin; for (; (a > 0) && (s[a - 1] != '\n'); a--) ;
                int b = a; for (; (b < s.Length) && ((s[b] == ' ') || (s[b] == '\t')); b++) ;
                Replace("\r\n" + s.Substring(a, b - a));
                return;
            }
            if (e.KeyChar == (char)8)
            {
                if (m_Selstart == m_Selend)
                {
                    if (m_Selstart == 0) return;
                    m_Selstart--;
                    if (m_Text[m_Selstart] == '\n')
                        m_Selstart--;
                }
                Replace("");
                return;
            }
            if (e.KeyChar == '\t')
            {
                int l1 = LineFromPos(SelMin), l2 = LineFromPos(SelMax);
                if (l1 != l2)
                {
                    int i1 = GetLineStart(l1);
                    int i2 = GetLineEnd(l2);
                    bool bout = (ModifierKeys & Keys.Shift) == Keys.Shift;

                    String s = m_Text.Substring(i1, i2 - i1), r = "";
                    Select(i1, i2);
                    String[] ss = s.Split(new String[] { "\r\n" }, System.StringSplitOptions.None);
                    for (int i = 0; i < ss.Length; i++)
                    {
                        if (i != 0) r += "\r\n";
                        if (bout)
                            r += (ss[i].Length > 0) && ((ss[i][0] == '\t') || (ss[i][0] == ' ')) ? ss[i].Substring(1, ss[i].Length - 1) : ss[i];
                        else
                            r += "\t" + ss[i];
                    }
                    if (r == s) return;
                    Replace(r);
                    Select(i1, i1 + r.Length);
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
