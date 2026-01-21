namespace RACTClient
{
    partial class ucBatchDeviceSelectPanel
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucBatchDeviceSelectPanel));
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.ucDeviceSearch = new RACTClient.ucDeviceSearch();
            this.fgDeviceList = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAdd = new MKLibrary.Controls.MKButton(this.components);
            this.btnAddAll = new MKLibrary.Controls.MKButton(this.components);
            this.btnRemove = new MKLibrary.Controls.MKButton(this.components);
            this.btnRemoveAll = new MKLibrary.Controls.MKButton(this.components);
            this.lblCheckDeviceCount = new System.Windows.Forms.Label();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fgDeviceList)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.ucDeviceSearch);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.splitContainer.Panel2.Controls.Add(this.fgDeviceList);
            this.splitContainer.Panel2.Controls.Add(this.panel1);
            this.splitContainer.Panel2.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.splitContainer.Size = new System.Drawing.Size(815, 514);
            this.splitContainer.SplitterDistance = 278;
            this.splitContainer.TabIndex = 0;
            // 
            // ucDeviceSearch
            // 
            this.ucDeviceSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.ucDeviceSearch.DeviceName = null;
            this.ucDeviceSearch.DevicePartCode = null;
            this.ucDeviceSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucDeviceSearch.IPAddress = null;
            this.ucDeviceSearch.Location = new System.Drawing.Point(0, 0);
            this.ucDeviceSearch.ModelInfo = null;
            this.ucDeviceSearch.Name = "ucDeviceSearch";
            this.ucDeviceSearch.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.ucDeviceSearch.Size = new System.Drawing.Size(815, 278);
            this.ucDeviceSearch.TabIndex = 0;
            // 
            // fgDeviceList
            // 
            this.fgDeviceList.BackColor = System.Drawing.SystemColors.Window;
            this.fgDeviceList.ColumnInfo = resources.GetString("fgDeviceList.ColumnInfo");
            this.fgDeviceList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fgDeviceList.ForeColor = System.Drawing.SystemColors.WindowText;
            this.fgDeviceList.Location = new System.Drawing.Point(3, 36);
            this.fgDeviceList.Name = "fgDeviceList";
            this.fgDeviceList.Rows.Count = 1;
            this.fgDeviceList.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.ListBox;
            this.fgDeviceList.Size = new System.Drawing.Size(809, 196);
            this.fgDeviceList.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("fgDeviceList.Styles"));
            this.fgDeviceList.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.panel1.Controls.Add(this.lblCheckDeviceCount);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.btnAddAll);
            this.panel1.Controls.Add(this.btnRemove);
            this.panel1.Controls.Add(this.btnRemoveAll);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(809, 36);
            this.panel1.TabIndex = 2;
            // 
            // btnAdd
            // 
            this.btnAdd.BackgroundImageDisable = null;
            this.btnAdd.BackgroundImageHover = null;
            this.btnAdd.BackgroundImageNormal = null;
            this.btnAdd.BackgroundImageSelect = null;
            this.btnAdd.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnAdd.BorderEdgeRadius = 3;
            this.btnAdd.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnAdd.ButtonImageCenter = null;
            this.btnAdd.ButtonImageLeft = null;
            this.btnAdd.ButtonImageRight = null;
            this.btnAdd.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnAdd, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnAdd.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnAdd.ColorDepthFocus = 2;
            this.btnAdd.ColorDepthHover = 2;
            this.btnAdd.ColorDepthShadow = 2;
            this.btnAdd.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnAdd.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnAdd.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnAdd.IconIndexDisable = -1;
            this.btnAdd.IconIndexHover = -1;
            this.btnAdd.IconIndexNormal = -1;
            this.btnAdd.IconIndexSelect = -1;
            this.btnAdd.Image = null;
            this.btnAdd.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnAdd.ImageIndent = 0;
            this.btnAdd.ImageList = null;
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnAdd.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnAdd.Location = new System.Drawing.Point(8, 6);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(62, 23);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "선택추가";
            this.btnAdd.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnAdd.TextIndent = 0;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnAddAll
            // 
            this.btnAddAll.BackgroundImageDisable = null;
            this.btnAddAll.BackgroundImageHover = null;
            this.btnAddAll.BackgroundImageNormal = null;
            this.btnAddAll.BackgroundImageSelect = null;
            this.btnAddAll.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnAddAll.BorderEdgeRadius = 3;
            this.btnAddAll.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnAddAll.ButtonImageCenter = null;
            this.btnAddAll.ButtonImageLeft = null;
            this.btnAddAll.ButtonImageRight = null;
            this.btnAddAll.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnAddAll, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnAddAll.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnAddAll.ColorDepthFocus = 2;
            this.btnAddAll.ColorDepthHover = 2;
            this.btnAddAll.ColorDepthShadow = 2;
            this.btnAddAll.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnAddAll.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnAddAll.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnAddAll.IconIndexDisable = -1;
            this.btnAddAll.IconIndexHover = -1;
            this.btnAddAll.IconIndexNormal = -1;
            this.btnAddAll.IconIndexSelect = -1;
            this.btnAddAll.Image = null;
            this.btnAddAll.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnAddAll.ImageIndent = 0;
            this.btnAddAll.ImageList = null;
            this.btnAddAll.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnAddAll.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnAddAll.Location = new System.Drawing.Point(76, 6);
            this.btnAddAll.Name = "btnAddAll";
            this.btnAddAll.Size = new System.Drawing.Size(62, 23);
            this.btnAddAll.TabIndex = 1;
            this.btnAddAll.Text = "전체추가";
            this.btnAddAll.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnAddAll.TextIndent = 0;
            this.btnAddAll.Click += new System.EventHandler(this.btnAddAll_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.BackgroundImageDisable = null;
            this.btnRemove.BackgroundImageHover = null;
            this.btnRemove.BackgroundImageNormal = null;
            this.btnRemove.BackgroundImageSelect = null;
            this.btnRemove.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnRemove.BorderEdgeRadius = 3;
            this.btnRemove.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnRemove.ButtonImageCenter = null;
            this.btnRemove.ButtonImageLeft = null;
            this.btnRemove.ButtonImageRight = null;
            this.btnRemove.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnRemove, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnRemove.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnRemove.ColorDepthFocus = 2;
            this.btnRemove.ColorDepthHover = 2;
            this.btnRemove.ColorDepthShadow = 2;
            this.btnRemove.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnRemove.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnRemove.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnRemove.IconIndexDisable = -1;
            this.btnRemove.IconIndexHover = -1;
            this.btnRemove.IconIndexNormal = -1;
            this.btnRemove.IconIndexSelect = -1;
            this.btnRemove.Image = null;
            this.btnRemove.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnRemove.ImageIndent = 0;
            this.btnRemove.ImageList = null;
            this.btnRemove.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnRemove.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnRemove.Location = new System.Drawing.Point(673, 6);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(62, 23);
            this.btnRemove.TabIndex = 1;
            this.btnRemove.Text = "선택삭제";
            this.btnRemove.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnRemove.TextIndent = 0;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnRemoveAll
            // 
            this.btnRemoveAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveAll.BackgroundImageDisable = null;
            this.btnRemoveAll.BackgroundImageHover = null;
            this.btnRemoveAll.BackgroundImageNormal = null;
            this.btnRemoveAll.BackgroundImageSelect = null;
            this.btnRemoveAll.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnRemoveAll.BorderEdgeRadius = 3;
            this.btnRemoveAll.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnRemoveAll.ButtonImageCenter = null;
            this.btnRemoveAll.ButtonImageLeft = null;
            this.btnRemoveAll.ButtonImageRight = null;
            this.btnRemoveAll.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnRemoveAll, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnRemoveAll.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnRemoveAll.ColorDepthFocus = 2;
            this.btnRemoveAll.ColorDepthHover = 2;
            this.btnRemoveAll.ColorDepthShadow = 2;
            this.btnRemoveAll.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnRemoveAll.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnRemoveAll.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnRemoveAll.IconIndexDisable = -1;
            this.btnRemoveAll.IconIndexHover = -1;
            this.btnRemoveAll.IconIndexNormal = -1;
            this.btnRemoveAll.IconIndexSelect = -1;
            this.btnRemoveAll.Image = null;
            this.btnRemoveAll.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnRemoveAll.ImageIndent = 0;
            this.btnRemoveAll.ImageList = null;
            this.btnRemoveAll.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnRemoveAll.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnRemoveAll.Location = new System.Drawing.Point(741, 6);
            this.btnRemoveAll.Name = "btnRemoveAll";
            this.btnRemoveAll.Size = new System.Drawing.Size(62, 23);
            this.btnRemoveAll.TabIndex = 1;
            this.btnRemoveAll.Text = "전체삭제";
            this.btnRemoveAll.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnRemoveAll.TextIndent = 0;
            this.btnRemoveAll.Click += new System.EventHandler(this.btnRemoveAll_Click);
            // 
            // lblCheckDeviceCount
            // 
            this.lblCheckDeviceCount.AutoSize = true;
            this.lblCheckDeviceCount.Location = new System.Drawing.Point(574, 12);
            this.lblCheckDeviceCount.Name = "lblCheckDeviceCount";
            this.lblCheckDeviceCount.Size = new System.Drawing.Size(93, 12);
            this.lblCheckDeviceCount.TabIndex = 182;
            this.lblCheckDeviceCount.Text = "선택 장비대수 : ";
            // 
            // ucBatchDeviceSelectPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.Controls.Add(this.splitContainer);
            this.Name = "ucBatchDeviceSelectPanel";
            this.Size = new System.Drawing.Size(815, 514);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fgDeviceList)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private C1.Win.C1FlexGrid.C1FlexGrid fgDeviceList;
        private ucDeviceSearch ucDeviceSearch;
        private MKLibrary.Controls.MKButton btnAddAll;
        private MKLibrary.Controls.MKButton btnRemoveAll;
        private MKLibrary.Controls.MKButton btnAdd;
        private MKLibrary.Controls.MKButton btnRemove;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblCheckDeviceCount;

    }
}
