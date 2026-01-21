namespace RACTClient
{
    partial class ucSearchDevice
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucSearchDevice));
            this.btnSearch = new MKLibrary.Controls.MKButton(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.contextMenuBar1 = new DevComponents.DotNetBar.ContextMenuBar();
            this.ctmPopup = new DevComponents.DotNetBar.ButtonItem();
            this.mnuQuickConnect = new DevComponents.DotNetBar.ButtonItem();
            this.mnuTL1Connect = new DevComponents.DotNetBar.ButtonItem();
            this.mnuAddDevice = new DevComponents.DotNetBar.ButtonItem();
            this.grdDeviceList = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.cboSearch = new MKLibrary.Controls.MKComboBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.mnuRpcsConnect = new DevComponents.DotNetBar.ButtonItem();
            this.mnuCatm1Connect = new DevComponents.DotNetBar.ButtonItem();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdDeviceList)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackgroundImageDisable = null;
            this.btnSearch.BackgroundImageHover = null;
            this.btnSearch.BackgroundImageNormal = null;
            this.btnSearch.BackgroundImageSelect = null;
            this.btnSearch.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnSearch.BorderEdgeRadius = 3;
            this.btnSearch.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnSearch.ButtonImageCenter = null;
            this.btnSearch.ButtonImageLeft = null;
            this.btnSearch.ButtonImageRight = null;
            this.btnSearch.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnSearch, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnSearch.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnSearch.ColorDepthFocus = 2;
            this.btnSearch.ColorDepthHover = 2;
            this.btnSearch.ColorDepthShadow = 2;
            this.btnSearch.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnSearch.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnSearch.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnSearch.IconIndexDisable = -1;
            this.btnSearch.IconIndexHover = -1;
            this.btnSearch.IconIndexNormal = -1;
            this.btnSearch.IconIndexSelect = -1;
            this.btnSearch.Image = null;
            this.btnSearch.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnSearch.ImageIndent = 0;
            this.btnSearch.ImageList = null;
            this.btnSearch.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnSearch.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnSearch.Location = new System.Drawing.Point(168, 30);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(55, 21);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "검색";
            this.btnSearch.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnSearch.TextIndent = 0;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.contextMenuBar1);
            this.panel1.Controls.Add(this.grdDeviceList);
            this.panel1.Location = new System.Drawing.Point(5, 57);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(219, 201);
            this.panel1.TabIndex = 4;
            // 
            // contextMenuBar1
            // 
            this.contextMenuBar1.AntiAlias = true;
            this.contextMenuBar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.ctmPopup});
            this.contextMenuBar1.Location = new System.Drawing.Point(49, 65);
            this.contextMenuBar1.Name = "contextMenuBar1";
            this.contextMenuBar1.Size = new System.Drawing.Size(75, 25);
            this.contextMenuBar1.Stretch = true;
            this.contextMenuBar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.contextMenuBar1.TabIndex = 1;
            this.contextMenuBar1.TabStop = false;
            this.contextMenuBar1.Text = "contextMenuBar1";
            // 
            // ctmPopup
            // 
            this.ctmPopup.AutoExpandOnClick = true;
            this.ctmPopup.Name = "ctmPopup";
            this.ctmPopup.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.mnuQuickConnect,
            this.mnuRpcsConnect,
            this.mnuTL1Connect,
            this.mnuAddDevice,
            this.mnuCatm1Connect});
            this.ctmPopup.Text = "buttonItem1";
            // 
            // mnuQuickConnect
            // 
            this.mnuQuickConnect.Name = "mnuQuickConnect";
            this.mnuQuickConnect.Text = "빠른 연결";
            this.mnuQuickConnect.Click += new System.EventHandler(this.mnuQuickConnect_Click);
            // 
            // mnuTL1Connect
            // 
            this.mnuTL1Connect.Name = "mnuTL1Connect";
            this.mnuTL1Connect.Text = "TL1 연결";
            this.mnuTL1Connect.Click += new System.EventHandler(this.mnuTL1Connect_Click);
            // 
            // mnuAddDevice
            // 
            this.mnuAddDevice.Name = "mnuAddDevice";
            this.mnuAddDevice.Text = "장비 추가";
            this.mnuAddDevice.Click += new System.EventHandler(this.mnuAddDevice_Click);
            // 
            // mnuCatm1Connect
            // 
            this.mnuCatm1Connect.Name = "mnuCatm1Connect";
            this.mnuCatm1Connect.Text = "CAT.M1 연결";
            this.mnuCatm1Connect.Click += new System.EventHandler(this.mnuCatm1Connect_Click);
            // 
            // grdDeviceList
            // 
            this.grdDeviceList.AllowEditing = false;
            this.grdDeviceList.BackColor = System.Drawing.SystemColors.Window;
            this.grdDeviceList.ColumnInfo = resources.GetString("grdDeviceList.ColumnInfo");
            this.grdDeviceList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdDeviceList.ForeColor = System.Drawing.SystemColors.WindowText;
            this.grdDeviceList.Location = new System.Drawing.Point(0, 0);
            this.grdDeviceList.Name = "grdDeviceList";
            this.grdDeviceList.Rows.Count = 1;
            this.grdDeviceList.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.ListBox;
            this.grdDeviceList.Size = new System.Drawing.Size(219, 201);
            this.grdDeviceList.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("grdDeviceList.Styles"));
            this.grdDeviceList.TabIndex = 0;
            this.grdDeviceList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdDeviceList_MouseDown);
            // 
            // cboSearch
            // 
            this.cboSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboSearch.BackColor = System.Drawing.SystemColors.Window;
            this.cboSearch.BackColorSelected = System.Drawing.Color.Orange;
            this.cboSearch.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(201)))), ((int)(((byte)(185)))));
            this.cboSearch.BorderEdgeRadius = 3;
            this.cboSearch.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.cboSearch.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.Fixed;
            this.cboSearch.BoxBorderColor = System.Drawing.Color.Gray;
            this.cboSearch.ColorBoxSize = new System.Drawing.Size(12, 8);
            this.cboSearch.ComboBoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSearch.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboSearch.ForeColor = System.Drawing.Color.Black;
            this.cboSearch.ImageList = null;
            this.cboSearch.ItemHeight = 14;
            this.cboSearch.Location = new System.Drawing.Point(7, 30);
            this.cboSearch.MaxDorpDownWidth = 500;
            this.cboSearch.Name = "cboSearch";
            this.cboSearch.SelectedIndex = -1;
            this.cboSearch.ShowColorBox = false;
            this.cboSearch.Size = new System.Drawing.Size(155, 21);
            this.cboSearch.TabIndex = 171;
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtSearch.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtSearch.Location = new System.Drawing.Point(7, 3);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(216, 21);
            this.txtSearch.TabIndex = 173;
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            this.txtSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSearch_KeyPress);
            this.txtSearch.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtSearch_MouseDown);
            // 
            // mnuRpcsConnect
            // 
            this.mnuRpcsConnect.Name = "mnuRpcsConnect";
            this.mnuRpcsConnect.Text = "빠른 연결";
            this.mnuRpcsConnect.Click += new System.EventHandler(this.mnuRpcsConnect_Click);
            // 
            // ucSearchDevice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.cboSearch);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnSearch);
            this.Name = "ucSearchDevice";
            this.Size = new System.Drawing.Size(228, 261);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdDeviceList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MKLibrary.Controls.MKButton btnSearch;
        private System.Windows.Forms.Panel panel1;
        private C1.Win.C1FlexGrid.C1FlexGrid grdDeviceList;
        private MKLibrary.Controls.MKComboBox cboSearch;
        private DevComponents.DotNetBar.ContextMenuBar contextMenuBar1;
        private DevComponents.DotNetBar.ButtonItem ctmPopup;
        private DevComponents.DotNetBar.ButtonItem mnuQuickConnect;
        private DevComponents.DotNetBar.ButtonItem mnuAddDevice;
        private System.Windows.Forms.TextBox txtSearch;
        private DevComponents.DotNetBar.ButtonItem mnuTL1Connect;
        private DevComponents.DotNetBar.ButtonItem mnuRpcsConnect;
        private DevComponents.DotNetBar.ButtonItem mnuCatm1Connect;
    }
}
