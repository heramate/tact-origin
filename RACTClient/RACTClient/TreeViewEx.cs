using System;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Design;
using RACTCommonClass;

namespace RACTClient
{
    public class TreeViewEx : TreeView
    {
        /// <summary>
        /// 기본 생성자
        /// </summary>
        public TreeViewEx()
            : base()
        {
            //SetStyle(ControlStyles.DoubleBuffer , true);			
        }

        private Boolean m_UseCustomImage;
        /// <summary>
        /// UseCustomImage  속성을 가져오거나 설정합니다.
        /// </summary>
        [Category("CheckState"), DefaultValue(false)]
        public Boolean UseCustomImage
        {
            get { return m_UseCustomImage; }
            set { m_UseCustomImage = value; }
        }

        private int indexUnchecked;
        private int indexChecked;
        private int indexIndeterminate;

        [Category("CheckState")]
        [TypeConverter(typeof(TreeViewImageIndexConverter))]
        [Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor)), RefreshProperties(RefreshProperties.Repaint), DefaultValue(-1), RelatedImageList("ImageList"), Localizable(true)]
        public int CheckedImageIndex
        {
            get
            {
                if (base.ImageList == null)
                    return -1;
                if (this.indexChecked >= this.ImageList.Images.Count)
                    return Math.Max(0, this.ImageList.Images.Count - 1);
                return this.indexChecked;
            }
            set
            {
                if (value == -1)
                    value = 0;
                if (value < 0)
                    throw new ArgumentException(string.Format("Index out of bounds! ({0}) index must be equal to or greater then {1}.", value.ToString(), "0"));
                if (this.indexChecked != value)
                {
                    this.indexChecked = value;
                    if (base.IsHandleCreated)
                        base.RecreateHandle();
                }
            }
        }

        [Category("CheckState")]
        [TypeConverter(typeof(TreeViewImageIndexConverter))]
        [Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor)), RefreshProperties(RefreshProperties.Repaint), DefaultValue(-1), RelatedImageList("ImageList"), Localizable(true)]
        public int UncheckedImageIndex
        {
            get
            {
                if (base.ImageList == null)
                    return -1;
                if (this.indexUnchecked >= this.ImageList.Images.Count)
                    return Math.Max(0, this.ImageList.Images.Count - 1);
                return this.indexUnchecked;
            }
            set
            {
                if (value == -1)
                    value = 0;
                if (value < 0)
                    throw new ArgumentException(string.Format("Index out of bounds! ({0}) index must be equal to or greater then {1}.", value.ToString(), "0"));
                if (this.indexUnchecked != value)
                {
                    this.indexUnchecked = value;
                    if (base.IsHandleCreated)
                        base.RecreateHandle();
                }
            }
        }

        [Category("CheckState")]
        [TypeConverter(typeof(TreeViewImageIndexConverter))]
        [Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor)), RefreshProperties(RefreshProperties.Repaint), DefaultValue(-1), RelatedImageList("ImageList"), Localizable(true)]
        public int IndeterminateImageIndex
        {
            get
            {
                if (base.ImageList == null)
                    return -1;
                if (this.indexIndeterminate >= this.ImageList.Images.Count)
                    return Math.Max(0, this.ImageList.Images.Count - 1);
                return this.indexIndeterminate;
            }
            set
            {
                if (value == -1)
                    value = 0;
                if (value < 0)
                    throw new ArgumentException(string.Format("Index out of bounds! ({0}) index must be equal to or greater then {1}.", value.ToString(), "0"));
                if (this.indexIndeterminate != value)
                {
                    this.indexIndeterminate = value;
                    if (base.IsHandleCreated)
                        base.RecreateHandle();
                }
            }
        }
        /// <summary>
        /// Deletes the default checkboxes which are drawn when the treeview.Checkboxes property
        /// is set tot true.
        /// </summary>
        /// <param name="bounds"></param>
        /// <param name="gx"></param>
        private void ClearCheckbox(Rectangle bounds, Graphics gx)
        {
            // make sure the default checkboxes are gone.
            using (Brush brush = new SolidBrush(this.BackColor))
            {
                gx.FillRectangle(brush, bounds);
            }
        }
        /// <summary>
        /// Draws a checkbox in the desired state and style
        /// </summary>
        /// <param name="bounds">boundaries of the checkbox</param>
        /// <param name="gx">graphics context object</param>
        /// <param name="buttonState">state to draw the checkbox in</param>
        private void DrawCheckbox(Rectangle bounds, Graphics gx, ButtonState buttonState)
        {
            // if we don't have custom images, or no imagelist, draw default images
            if (!this.UseCustomImage || (this.UseCustomImage && null == this.ImageList))
            {
                ControlPaint.DrawMixedCheckBox(gx, bounds, buttonState);
                return;
            }

            // get the right image index
            int imageIndex = -1;
            if ((buttonState & ButtonState.Normal) == ButtonState.Normal)
                imageIndex = this.indexUnchecked;
            if ((buttonState & ButtonState.Checked) == ButtonState.Checked)
                imageIndex = this.indexChecked;
            if ((buttonState & ButtonState.Pushed) == ButtonState.Pushed)
                imageIndex = this.indexIndeterminate;

            if (imageIndex > -1 && imageIndex < this.ImageList.Images.Count)
            {
                // index is valid so draw the image
                this.ImageList.Draw(gx, bounds.X, bounds.Y, bounds.Width + 1, bounds.Height + 1, imageIndex);
            }
            else
            {
                // index is not valid so draw default image
                ControlPaint.DrawMixedCheckBox(gx, bounds, buttonState);
            }
        }

        /// <summary>
        /// Draws thee node lines before the checkboxes are drawn
        /// </summary>
        /// <param name="gx">Graphics context</param>
        private void DrawNodeLines(TreeNodeEx node, Rectangle bounds, Graphics gx)
        {
            // determine type of line to draw
            NodeLineType lineType = node.NodeLineType;
            if (lineType == NodeLineType.None) { return; }

            using (Pen pen = new Pen(SystemColors.ControlDark, 1))
            {
                pen.DashStyle = DashStyle.Dot;
                if (node.Parent == null)
                {
                    gx.DrawLine(pen, new Point(bounds.X - 8, bounds.Y + 8), new Point(bounds.X, bounds.Y + 8));
                }

                if (lineType == NodeLineType.WithChildren && node.IsExpanded)
                {
                    gx.DrawLine(pen, new Point(bounds.X + 8, bounds.Y + 8), new Point(bounds.X + 8, bounds.Y + 8));
                }
            }
        }

        /// <summary>
        /// Paints the node specified.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="gx"></param>
        private void PaintTreeNode(TreeNodeEx node, Graphics gx)
        {
            if (this.CheckBoxes)
            {
                // calculate boundaries
                Rectangle ncRect = new Rectangle(node.Bounds.X - 35, node.Bounds.Y, 15, 15);

                // make sure the default checkboxes are gone
                ClearCheckbox(ncRect, gx);

                // draw lines, if we are supposed to
                if (this.ShowLines)
                {
                    DrawNodeLines(node, ncRect, gx);
                }

                if (this.CheckBoxes)
                {
                    // now draw the checkboxes
                    switch (node.CheckState)
                    {
                        case CheckState.Unchecked:			// Normal
                            DrawCheckbox(ncRect, gx, ButtonState.Normal | ButtonState.Flat);
                            break;
                        case CheckState.Checked:			// Checked
                            DrawCheckbox(ncRect, gx, ButtonState.Checked | ButtonState.Flat);
                            break;
                        case CheckState.Indeterminate:		// Pushed
                            DrawCheckbox(ncRect, gx, ButtonState.Pushed | ButtonState.Flat);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 마우스 다운 처리 입니다.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                CheckNodeSelected(this.Nodes, e);
            }
             
            base.OnMouseDown(e);
        }


     
        /// <summary>
        /// 마우스에 해당하는 노드를 선택합니다.
        /// </summary>
        /// <param name="aNode"></param>
        /// <param name="e"></param>
        private void CheckNodeSelected(TreeNodeCollection aNode ,MouseEventArgs e)
        {
            for (int i = 0; i < aNode.Count; i++)
            {
                if (aNode[i].Nodes.Count > 0)
                {
                    CheckNodeSelected(aNode[i].Nodes, e);
                }

                if (aNode[i].Bounds.Contains(new Point(e.X, e.Y)))
                {
                    this.SelectedNode = aNode[i];
                    break;
                }

            }
        }

        protected override void OnAfterCheck(TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            if (node != null)
            {
                TreeNodeEx clickedNode = node as TreeNodeEx;
                if (this.CheckBoxes)
                    ToggleNodeState(clickedNode);
            }

            base.OnAfterCheck(e);
        }

        /// <summary>
        /// Toggles node state between checked & unchecked
        /// </summary>
        /// <param name="node"></param>
        private void ToggleNodeState(TreeNodeEx aNodeEx)
        {
            // no need to toggle state for non-existing node ( or non-tristatetreenode! )
            if (null == aNodeEx) return;

            // toggle state
            CheckState nextState;
            switch (aNodeEx.CheckState)
            {
                case CheckState.Unchecked:
                    nextState = CheckState.Checked;
                    break;
                default:
                    nextState = CheckState.Unchecked;
                    break;
            }

            // notify the treeview that an update is about to take place
            BeginUpdate();
            // update the node state, and dependend nodes
            aNodeEx.SetCheckStateChange(nextState);

            // force a redraw
            EndUpdate();
        }

        private int HandleNotify(Message msg)
        {
            const int NM_FIRST = 0;
            const int NM_CUSTOMDRAW = NM_FIRST - 12;

            // Drawstage
            const int CDDS_PREPAINT = 0x1;
            const int CDDS_POSTPAINT = 0x2;

            const int CDDS_ITEM = 0x10000;
            const int CDDS_ITEMPREPAINT = (CDDS_ITEM | CDDS_PREPAINT);
            const int CDDS_ITEMPOSTPAINT = (CDDS_ITEM | CDDS_POSTPAINT);

            // Custom draw return flags
            const int CDRF_DODEFAULT = 0x0;
            const int CDRF_NOTIFYPOSTPAINT = 0x10;
            const int CDRF_NOTIFYITEMDRAW = 0x20;

            NMHDR tNMHDR;
            NMTVCUSTOMDRAW tNMTVCUSTOMDRAW;
            int iResult = 0;
            object obj;
            TreeNode node;
            TreeNodeEx tsNode;

            try
            {
                if (!msg.LParam.Equals(IntPtr.Zero))
                {
                    obj = msg.GetLParam(typeof(NMHDR));
                    if (obj is NMHDR)
                    {
                        tNMHDR = (NMHDR)obj;
                        if (tNMHDR.code == NM_CUSTOMDRAW)
                        {
                            obj = msg.GetLParam(typeof(NMTVCUSTOMDRAW));
                            if (obj is NMTVCUSTOMDRAW)
                            {
                                tNMTVCUSTOMDRAW = (NMTVCUSTOMDRAW)obj;
                                switch (tNMTVCUSTOMDRAW.nmcd.dwDrawStage)
                                {
                                    case CDDS_PREPAINT:
                                        iResult = CDRF_NOTIFYITEMDRAW;
                                        break;
                                    case CDDS_ITEMPREPAINT:
                                        iResult = CDRF_NOTIFYPOSTPAINT;
                                        break;
                                    case CDDS_ITEMPOSTPAINT:
                                        node = TreeNode.FromHandle(this, tNMTVCUSTOMDRAW.nmcd.dwItemSpec);
                                        tsNode = node as TreeNodeEx;
                                        if (tsNode != null)
                                        {
                                            Graphics graph = Graphics.FromHdc(tNMTVCUSTOMDRAW.nmcd.hdc);
                                            PaintTreeNode(tsNode, graph);
                                            graph.Dispose();
                                        }
                                        iResult = CDRF_DODEFAULT;
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return iResult;
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_NOTIFY = 0x4E;

            int iResult = 0;
            bool bHandled = false;

            if (m.Msg == (0x2000 | WM_NOTIFY))
            {
                if (m.WParam.Equals(this.Handle))
                {
                    iResult = HandleNotify(m);
                    m.Result = new IntPtr(iResult);
                    bHandled = (iResult != 0);
                }
            }

            if (!bHandled)
                base.WndProc(ref m);
        }

        private struct RECT
        {
            internal int left;
            internal int top;
            internal int right;
            internal int bottom;
        }

        private struct NMHDR
        {
            internal IntPtr hwndFrom;
            internal IntPtr idFrom;
            internal int code;
        }

        private struct NMCUSTOMDRAW
        {
            internal NMHDR hdr;
            internal int dwDrawStage;
            internal IntPtr hdc;
            internal RECT rc;
            internal IntPtr dwItemSpec;
            internal int uItemState;
            internal IntPtr lItemlParam;
        }

        private struct NMTVCUSTOMDRAW
        {
            internal NMCUSTOMDRAW nmcd;
            internal int clrText;
            internal int clrTextBk;
            internal int iLevel;
        }
    }
}
