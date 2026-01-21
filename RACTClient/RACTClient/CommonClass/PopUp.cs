using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;
using System.Drawing.Drawing2D;

namespace RACTClient
{
    /// <summary>
    /// 드롭다운 윈도우 클래스 입니다.
    /// </summary>
    [ToolboxItem(false)]
    public partial class MKDropDown : ToolStripDropDown
    {
        /// <summary>
        /// 드롭 다운에 표시할 자식 윈도우 입니다.
        /// </summary>
        private Control m_ChildControl;
        /// <summary>
        /// 드롭 다운에 표시할 자식 윈도우를 가져오기 합니다.
        /// </summary>
        public Control ChildControl
        {
            get { return m_ChildControl; }
        }

        /// <summary>
        /// 페이드 효과 사용여부 입니다.
        /// </summary>
        private bool m_IsFadeEffect;
        /// <summary>
        /// 페이드 효과 사용여부를 가져오거나 설정합니다.
        /// </summary>
        public bool IsFadeEffect
        {
            get { return m_IsFadeEffect; }
            set
            {
                if (m_IsFadeEffect == value) return;
                m_IsFadeEffect = value;
            }
        }

        /// <summary>
        /// 드롭다운 오픈시 자식 컨트롤에 포커스를 지정할지의 여부 입니다.
        /// </summary>
        private bool m_IsFocusOnOpen = true;
        /// <summary>
        /// 드롭다운 오픈시 자식 컨트롤에 포커스를 지정할지의 여부를 가져오거나 설정합니다.
        /// </summary>
        public bool IsFocusOnOpen
        {
            get { return m_IsFocusOnOpen; }
            set { m_IsFocusOnOpen = value; }
        }

        /// <summary>
        /// Alt키 처리 여부 입니다.
        /// </summary>
        private bool m_IsAcceptAlt = true;
        /// <summary>
        /// Alt키 처리 여부를 가져오거나 설정합니다.
        /// </summary>
        public bool IsAcceptAlt
        {
            get { return m_IsAcceptAlt; }
            set { m_IsAcceptAlt = value; }
        }

        private MKDropDown m_OwnerDropDown;
        private MKDropDown m_ChildDropDown;

        private bool _resizable;
        private bool resizable;
        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="PopupControl.Popup" /> is resizable.
        /// </summary>
        /// <value><c>true</c> if resizable; otherwise, <c>false</c>.</value>
        public bool Resizable
        {
            get { return resizable && _resizable; }
            set { resizable = value; }
        }

        private ToolStripControlHost host;

        /// <summary>
        /// 컨트롤 최소 크기 입니다.
        /// </summary>
        private Size m_MinimumSize;
        /// <summary>
        /// 컨트롤 최소 크기를 가져오거나 설정합니다.
        /// </summary>
        public new Size MinimumSize
        {
            get { return m_MinimumSize; }
            set { m_MinimumSize = value; }
        }

        /// <summary>
        /// 컨트롤 최대 크기 입니다.
        /// </summary>
        private Size m_MaximumSize;
        /// <summary>
        /// 컨트롤 최대 크기를 가져오거나 설정합니다.
        /// </summary>
        public new Size MaximumSize
        {
            get { return m_MaximumSize; }
            set { m_MaximumSize = value; }
        }

        /// <summary>
        /// 컨트롤 생성시의 처리 입니다.
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= NativeMethods.WS_EX_NOACTIVATE;
                return cp;
            }
        }

        /// <summary>
        /// 드롭 다운 컨트롤의 생성자 입니다.
        /// </summary>
        /// <param name="aChildControl">드롭다운 컨트롤에 표시할 자식 컨트롤 입니다.</param>
        public MKDropDown(Control aChildControl)
        {
            if (aChildControl == null)
            {
                throw new ArgumentNullException("ChildControl is null");
            }

            ((IDropDownChild)aChildControl).DropDownSelected += new EventHandler(MKDropDown_DropDownSelected);
            ((IDropDownChild)aChildControl).DropDownCanceled += new EventHandler(MKDropDown_DropDownCanceled);

            m_ChildControl = aChildControl;
            m_IsFadeEffect = SystemInformation.IsMenuAnimationEnabled && SystemInformation.IsMenuFadeEnabled;
            this._resizable = true;
            InitializeComponent();
            AutoSize = false;
            DoubleBuffered = true;
            ResizeRedraw = true;
            host = new ToolStripControlHost(aChildControl);
            Padding = Margin = host.Padding = host.Margin = Padding.Empty;
            MinimumSize = aChildControl.MinimumSize;
            aChildControl.MinimumSize = aChildControl.Size;
            MaximumSize = aChildControl.MaximumSize;
            aChildControl.MaximumSize = aChildControl.Size;
            Size = aChildControl.Size;
            aChildControl.Location = Point.Empty;
            Items.Add(host);
            aChildControl.Disposed += delegate(object sender, EventArgs e)
            {
                aChildControl = null;
                Dispose(true);
            };
            aChildControl.RegionChanged += delegate(object sender, EventArgs e)
            {
                UpdateRegion();
            };
            aChildControl.Paint += delegate(object sender, PaintEventArgs e)
            {
                PaintSizeGrip(e);
            };
            UpdateRegion();
        }

        void MKDropDown_DropDownCanceled(object sender, EventArgs e)
        {
            this.Close();
        }

        void MKDropDown_DropDownSelected(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 대화 상자 키 코드 처리 입니다.
        /// </summary>
        /// <param name="keyData">키 데이터 입니다.</param>
        /// <returns>처리 여부 입니다.</returns>
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (m_IsAcceptAlt && ((keyData & Keys.Alt) == Keys.Alt))
            {
                if ((keyData & Keys.F4) != Keys.F4)
                {
                    return false;
                }
                else
                {
                    this.Close();
                }
            }
            return base.ProcessDialogKey(keyData);
        }

        /// <summary>
        /// Updates the pop-up region.
        /// </summary>
        protected void UpdateRegion()
        {
            if (this.Region != null)
            {
                this.Region.Dispose();
                this.Region = null;
            }
            if (m_ChildControl.Region != null)
            {
                this.Region = m_ChildControl.Region.Clone();
            }
        }

        /// <summary>
        /// Shows the pop-up window below the specified control.
        /// </summary>
        /// <param name="control">The control below which the pop-up will be shown.</param>
        /// <remarks>
        /// When there is no space below the specified control, the pop-up control is shown above it.
        /// </remarks>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="control"/> is <code>null</code>.</exception>
        public void Show(Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }
            SetOwnerItem(control);
            Show(control, control.ClientRectangle);
        }

        /// <summary>
        /// Shows the pop-up window below the specified area of the specified control.
        /// </summary>
        /// <param name="control">The control used to compute screen location of specified area.</param>
        /// <param name="area">The area of control below which the pop-up will be shown.</param>
        /// <remarks>
        /// When there is no space below specified area, the pop-up control is shown above it.
        /// </remarks>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="control"/> is <code>null</code>.</exception>
        public void Show(Control control, Rectangle area)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }
            SetOwnerItem(control);
            resizableTop = resizableLeft = false;
            Point location = control.PointToScreen(new Point(area.Left, area.Top + area.Height));
            Rectangle screen = Screen.FromControl(control).WorkingArea;
            if (location.X + Size.Width > (screen.Left + screen.Width))
            {
                resizableLeft = true;
                location.X = (screen.Left + screen.Width) - Size.Width;
            }
            if (location.Y + Size.Height > (screen.Top + screen.Height))
            {
                resizableTop = true;
                location.Y -= Size.Height + area.Height;
            }
            location = control.PointToClient(location);
            Show(control, location, ToolStripDropDownDirection.BelowRight);
        }

        private const int frames = 5;
        private const int totalduration = 100;
        private const int frameduration = totalduration / frames;

        /// <summary>
        /// 드롭다운 컨프롤 표시여부 설정시의 처리 입니다.
        /// </summary>
        /// <param name="visible">표시여부 입니다.</param>
        protected override void SetVisibleCore(bool visible)
        {
            double opacity = Opacity;
            if (visible && m_IsFadeEffect && m_IsFocusOnOpen) Opacity = 0;
            base.SetVisibleCore(visible);
            if (!visible || !m_IsFadeEffect || !m_IsFocusOnOpen) return;
            for (int i = 1; i <= frames; i++)
            {
                if (i > 1)
                {
                    System.Threading.Thread.Sleep(frameduration);
                }
                Opacity = opacity * (double)i / (double)frames;
            }
            Opacity = opacity;
        }

        private bool resizableTop;
        private bool resizableLeft;

        /// <summary>
        /// 부모 아이템 설정시의 처리 입니다.
        /// </summary>
        /// <param name="control">부모 컨트롤 입니다.</param>
        private void SetOwnerItem(Control aParentControl)
        {
            if (aParentControl == null)
            {
                return;
            }
            if (aParentControl is MKDropDown)
            {
                MKDropDown popupControl = aParentControl as MKDropDown;
                m_OwnerDropDown = popupControl;
                m_OwnerDropDown.m_ChildDropDown = this;
                OwnerItem = popupControl.Items[0];
                return;
            }
            if (aParentControl.Parent != null)
            {
                SetOwnerItem(aParentControl.Parent);
            }
        }

        /// <summary>
        /// 드롭다운 컨트롤 크기 변경시의 처리 입니다.
        /// </summary>
        protected override void OnSizeChanged(EventArgs e)
        {
            m_ChildControl.MinimumSize = Size;
            m_ChildControl.MaximumSize = Size;
            m_ChildControl.Size = Size;
            m_ChildControl.Location = Point.Empty;
            base.OnSizeChanged(e);
        }

        /// <summary>
        /// 드롭다운 열기 준비시의 처리 입니다.
        /// </summary>
        protected override void OnOpening(CancelEventArgs e)
        {
            if (m_ChildControl.IsDisposed || m_ChildControl.Disposing)
            {
                e.Cancel = true;
                return;
            }
            UpdateRegion();
            base.OnOpening(e);
        }

        /// <summary>
        /// 드롭다운 오픈시의 처리 입니다.
        /// </summary>
        protected override void OnOpened(EventArgs e)
        {
            if (m_OwnerDropDown != null)
            {
                m_OwnerDropDown._resizable = false;
            }
            if (m_IsFocusOnOpen)
            {
                m_ChildControl.Focus();
            }
            base.OnOpened(e);
        }

        /// <summary>
        /// 드롭다운 닫기시의 처리 입니다.
        /// </summary>
        protected override void OnClosed(ToolStripDropDownClosedEventArgs e)
        {
            if (m_OwnerDropDown != null)
            {
                m_OwnerDropDown._resizable = true;
            }
            base.OnClosed(e);
        }

        /// <summary>
        /// Processes Windows messages.
        /// </summary>
        /// <param name="m">The Windows <see cref="T:System.Windows.Forms.Message" /> to process.</param>
        //[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            if (InternalProcessResizing(ref m, false))
            {
                return;
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// Processes the resizing messages.
        /// </summary>
        /// <param name="m">The message.</param>
        /// <returns>true, if the WndProc method from the base class shouldn't be invoked.</returns>
        //[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public bool ProcessResizing(ref Message m)
        {
            return InternalProcessResizing(ref m, true);
        }

        //[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        private bool InternalProcessResizing(ref Message m, bool contentControl)
        {
            if (m.Msg == NativeMethods.WM_NCACTIVATE && m.WParam != IntPtr.Zero && m_ChildDropDown != null && m_ChildDropDown.Visible)
            {
                m_ChildDropDown.Hide();
            }
            if (!Resizable)
            {
                return false;
            }
            if (m.Msg == NativeMethods.WM_NCHITTEST)
            {
                return OnNcHitTest(ref m, contentControl);
            }
            else if (m.Msg == NativeMethods.WM_GETMINMAXINFO)
            {
                return OnGetMinMaxInfo(ref m);
            }
            return false;
        }

        //[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        private bool OnGetMinMaxInfo(ref Message m)
        {
            NativeMethods.MINMAXINFO minmax = (NativeMethods.MINMAXINFO)Marshal.PtrToStructure(m.LParam, typeof(NativeMethods.MINMAXINFO));
            minmax.maxTrackSize = this.MaximumSize;
            minmax.minTrackSize = this.MinimumSize;
            Marshal.StructureToPtr(minmax, m.LParam, false);
            return true;
        }

        private bool OnNcHitTest(ref Message m, bool contentControl)
        {
            int x = NativeMethods.LOWORD(m.LParam);
            int y = NativeMethods.HIWORD(m.LParam);
            Point clientLocation = PointToClient(new Point(x, y));

            GripBounds gripBouns = new GripBounds(contentControl ? m_ChildControl.ClientRectangle : ClientRectangle);
            IntPtr transparent = new IntPtr(NativeMethods.HTTRANSPARENT);

            if (resizableTop)
            {
                if (resizableLeft && gripBouns.TopLeft.Contains(clientLocation))
                {
                    m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTTOPLEFT;
                    return true;
                }
                if (!resizableLeft && gripBouns.TopRight.Contains(clientLocation))
                {
                    m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTTOPRIGHT;
                    return true;
                }
                if (gripBouns.Top.Contains(clientLocation))
                {
                    m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTTOP;
                    return true;
                }
            }
            else
            {
                if (resizableLeft && gripBouns.BottomLeft.Contains(clientLocation))
                {
                    m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTBOTTOMLEFT;
                    return true;
                }
                if (!resizableLeft && gripBouns.BottomRight.Contains(clientLocation))
                {
                    m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTBOTTOMRIGHT;
                    return true;
                }
                if (gripBouns.Bottom.Contains(clientLocation))
                {
                    m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTBOTTOM;
                    return true;
                }
            }
            if (resizableLeft && gripBouns.Left.Contains(clientLocation))
            {
                m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTLEFT;
                return true;
            }
            if (!resizableLeft && gripBouns.Right.Contains(clientLocation))
            {
                m.Result = contentControl ? transparent : (IntPtr)NativeMethods.HTRIGHT;
                return true;
            }
            return false;
        }

        private VisualStyleRenderer sizeGripRenderer;
        /// <summary>
        /// Paints the sizing grip.
        /// </summary>
        /// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs" /> instance containing the event data.</param>
        public void PaintSizeGrip(PaintEventArgs e)
        {
            if (e == null || e.Graphics == null || !resizable)
            {
                return;
            }
            Size clientSize = m_ChildControl.ClientSize;
            using (Bitmap gripImage = new Bitmap(0x10, 0x10))
            {
                using (Graphics g = Graphics.FromImage(gripImage))
                {
                    if (Application.RenderWithVisualStyles)
                    {
                        if (this.sizeGripRenderer == null)
                        {
                            this.sizeGripRenderer = new VisualStyleRenderer(VisualStyleElement.Status.Gripper.Normal);
                        }
                        this.sizeGripRenderer.DrawBackground(g, new Rectangle(0, 0, 0x10, 0x10));
                    }
                    else
                    {
                        ControlPaint.DrawSizeGrip(g, m_ChildControl.BackColor, 0, 0, 0x10, 0x10);
                    }
                }
                GraphicsState gs = e.Graphics.Save();
                e.Graphics.ResetTransform();
                if (resizableTop)
                {
                    if (resizableLeft)
                    {
                        e.Graphics.RotateTransform(180);
                        e.Graphics.TranslateTransform(-clientSize.Width, -clientSize.Height);
                    }
                    else
                    {
                        e.Graphics.ScaleTransform(1, -1);
                        e.Graphics.TranslateTransform(0, -clientSize.Height);
                    }
                }
                else if (resizableLeft)
                {
                    e.Graphics.ScaleTransform(-1, 1);
                    e.Graphics.TranslateTransform(-clientSize.Width, 0);
                }
                e.Graphics.DrawImage(gripImage, clientSize.Width - 0x10, clientSize.Height - 0x10 + 1, 0x10, 0x10);
                e.Graphics.Restore(gs);
            }
        }
    }

    internal static class NativeMethods
    {
        internal const int WM_NCHITTEST = 0x0084,
                           WM_NCACTIVATE = 0x0086,
                           WS_EX_NOACTIVATE = 0x08000000,
                           HTTRANSPARENT = -1,
                           HTLEFT = 10,
                           HTRIGHT = 11,
                           HTTOP = 12,
                           HTTOPLEFT = 13,
                           HTTOPRIGHT = 14,
                           HTBOTTOM = 15,
                           HTBOTTOMLEFT = 16,
                           HTBOTTOMRIGHT = 17,
                           WM_USER = 0x0400,
                           WM_REFLECT = WM_USER + 0x1C00,
                           WM_COMMAND = 0x0111,
                           CBN_DROPDOWN = 7,
                           WM_GETMINMAXINFO = 0x0024;

        internal static int HIWORD(int n)
        {
            return (n >> 16) & 0xffff;
        }

        internal static int HIWORD(IntPtr n)
        {
            return HIWORD(unchecked((int)(long)n));
        }

        internal static int LOWORD(int n)
        {
            return n & 0xffff;
        }

        internal static int LOWORD(IntPtr n)
        {
            return LOWORD(unchecked((int)(long)n));
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct MINMAXINFO
        {
            public Point reserved;
            public Size maxSize;
            public Point maxPosition;
            public Size minTrackSize;
            public Size maxTrackSize;
        }
    }

    internal struct GripBounds
    {
        private const int GripSize = 6;
        private const int CornerGripSize = GripSize << 1;

        public GripBounds(Rectangle clientRectangle)
        {
            this.clientRectangle = clientRectangle;
        }

        private Rectangle clientRectangle;
        public Rectangle ClientRectangle
        {
            get { return clientRectangle; }
            //set { clientRectangle = value; }
        }

        public Rectangle Bottom
        {
            get
            {
                Rectangle rect = ClientRectangle;
                rect.Y = rect.Bottom - GripSize + 1;
                rect.Height = GripSize;
                return rect;
            }
        }

        public Rectangle BottomRight
        {
            get
            {
                Rectangle rect = ClientRectangle;
                rect.Y = rect.Bottom - CornerGripSize + 1;
                rect.Height = CornerGripSize;
                rect.X = rect.Width - CornerGripSize + 1;
                rect.Width = CornerGripSize;
                return rect;
            }
        }

        public Rectangle Top
        {
            get
            {
                Rectangle rect = ClientRectangle;
                rect.Height = GripSize;
                return rect;
            }
        }

        public Rectangle TopRight
        {
            get
            {
                Rectangle rect = ClientRectangle;
                rect.Height = CornerGripSize;
                rect.X = rect.Width - CornerGripSize + 1;
                rect.Width = CornerGripSize;
                return rect;
            }
        }

        public Rectangle Left
        {
            get
            {
                Rectangle rect = ClientRectangle;
                rect.Width = GripSize;
                return rect;
            }
        }

        public Rectangle BottomLeft
        {
            get
            {
                Rectangle rect = ClientRectangle;
                rect.Width = CornerGripSize;
                rect.Y = rect.Height - CornerGripSize + 1;
                rect.Height = CornerGripSize;
                return rect;
            }
        }

        public Rectangle Right
        {
            get
            {
                Rectangle rect = ClientRectangle;
                rect.X = rect.Right - GripSize + 1;
                rect.Width = GripSize;
                return rect;
            }
        }

        public Rectangle TopLeft
        {
            get
            {
                Rectangle rect = ClientRectangle;
                rect.Width = CornerGripSize;
                rect.Height = CornerGripSize;
                return rect;
            }
        }
    }
}
