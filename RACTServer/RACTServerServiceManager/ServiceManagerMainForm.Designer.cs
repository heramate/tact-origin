namespace RACTServerServiceManager
{
    partial class ServiceManagerMainForm
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

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServiceManagerMainForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new MKLibrary.Controls.MKButton(this.components);
            this.superTabControl1 = new DevComponents.DotNetBar.SuperTabControl();
            this.superTabControlPanel2 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.pnlDaemonDataList = new RACTServerServiceManager.ucDaemonDataListPanel();
            this.tabDaemonDataList = new DevComponents.DotNetBar.SuperTabItem();
            this.superTabControlPanel1 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.pnlConnectionData = new RACTServerServiceManager.ucConnectionDataPanel();
            this.tabConnectionData = new DevComponents.DotNetBar.SuperTabItem();
            this.noiRACT = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuStart = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuStop = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuClose = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.superTabControl1)).BeginInit();
            this.superTabControl1.SuspendLayout();
            this.superTabControlPanel2.SuspendLayout();
            this.superTabControlPanel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 381);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(406, 44);
            this.panel1.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackgroundImageDisable = null;
            this.btnClose.BackgroundImageHover = null;
            this.btnClose.BackgroundImageNormal = null;
            this.btnClose.BackgroundImageSelect = null;
            this.btnClose.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnClose.BorderEdgeRadius = 3;
            this.btnClose.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnClose.ButtonImageCenter = null;
            this.btnClose.ButtonImageLeft = null;
            this.btnClose.ButtonImageRight = null;
            this.btnClose.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnClose, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnClose.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnClose.ColorDepthFocus = 2;
            this.btnClose.ColorDepthHover = 2;
            this.btnClose.ColorDepthShadow = 2;
            this.btnClose.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnClose.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnClose.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnClose.IconIndexDisable = -1;
            this.btnClose.IconIndexHover = -1;
            this.btnClose.IconIndexNormal = -1;
            this.btnClose.IconIndexSelect = -1;
            this.btnClose.Image = null;
            this.btnClose.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnClose.ImageIndent = 0;
            this.btnClose.ImageList = null;
            this.btnClose.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnClose.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnClose.Location = new System.Drawing.Point(317, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(72, 25);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "닫기";
            this.btnClose.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnClose.TextIndent = 0;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // superTabControl1
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.superTabControl1.ControlBox.CloseBox.Name = "";
            // 
            // 
            // 
            this.superTabControl1.ControlBox.MenuBox.Name = "";
            this.superTabControl1.ControlBox.Name = "";
            this.superTabControl1.ControlBox.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.superTabControl1.ControlBox.MenuBox,
            this.superTabControl1.ControlBox.CloseBox});
            this.superTabControl1.Controls.Add(this.superTabControlPanel1);
            this.superTabControl1.Controls.Add(this.superTabControlPanel2);
            this.superTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControl1.Location = new System.Drawing.Point(0, 0);
            this.superTabControl1.Name = "superTabControl1";
            this.superTabControl1.ReorderTabsEnabled = true;
            this.superTabControl1.SelectedTabFont = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.superTabControl1.SelectedTabIndex = 0;
            this.superTabControl1.Size = new System.Drawing.Size(406, 381);
            this.superTabControl1.TabFont = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.superTabControl1.TabIndex = 1;
            this.superTabControl1.Tabs.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.tabConnectionData,
            this.tabDaemonDataList});
            this.superTabControl1.Text = "superTabControl1";
            // 
            // superTabControlPanel2
            // 
            this.superTabControlPanel2.Controls.Add(this.pnlDaemonDataList);
            this.superTabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel2.Location = new System.Drawing.Point(0, 0);
            this.superTabControlPanel2.Name = "superTabControlPanel2";
            this.superTabControlPanel2.Size = new System.Drawing.Size(406, 381);
            this.superTabControlPanel2.TabIndex = 0;
            this.superTabControlPanel2.TabItem = this.tabDaemonDataList;
            // 
            // pnlDaemonDataList
            // 
            this.pnlDaemonDataList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDaemonDataList.Location = new System.Drawing.Point(0, 0);
            this.pnlDaemonDataList.Name = "pnlDaemonDataList";
            this.pnlDaemonDataList.Size = new System.Drawing.Size(406, 381);
            this.pnlDaemonDataList.TabIndex = 0;
            // 
            // tabDaemonDataList
            // 
            this.tabDaemonDataList.AttachedControl = this.superTabControlPanel2;
            this.tabDaemonDataList.GlobalItem = false;
            this.tabDaemonDataList.Name = "tabDaemonDataList";
            this.tabDaemonDataList.Text = "데몬 상태 정보";
            // 
            // superTabControlPanel1
            // 
            this.superTabControlPanel1.Controls.Add(this.pnlConnectionData);
            this.superTabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel1.Location = new System.Drawing.Point(0, 28);
            this.superTabControlPanel1.Name = "superTabControlPanel1";
            this.superTabControlPanel1.Size = new System.Drawing.Size(406, 353);
            this.superTabControlPanel1.TabIndex = 1;
            this.superTabControlPanel1.TabItem = this.tabConnectionData;
            // 
            // pnlConnectionData
            // 
            this.pnlConnectionData.BackColor = System.Drawing.SystemColors.Control;
            this.pnlConnectionData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlConnectionData.Location = new System.Drawing.Point(0, 0);
            this.pnlConnectionData.Name = "pnlConnectionData";
            this.pnlConnectionData.Padding = new System.Windows.Forms.Padding(10);
            this.pnlConnectionData.Size = new System.Drawing.Size(406, 353);
            this.pnlConnectionData.TabIndex = 0;
            this.pnlConnectionData.Load += new System.EventHandler(this.pnlConnectionData_Load);
            // 
            // tabConnectionData
            // 
            this.tabConnectionData.AttachedControl = this.superTabControlPanel1;
            this.tabConnectionData.GlobalItem = false;
            this.tabConnectionData.Name = "tabConnectionData";
            this.tabConnectionData.Text = "접속 정보 설정";
            // 
            // noiRACT
            // 
            this.noiRACT.ContextMenuStrip = this.contextMenuStrip1;
            this.noiRACT.Icon = ((System.Drawing.Icon)(resources.GetObject("noiRACT.Icon")));
            this.noiRACT.Text = "RACT Server Service Manager";
            this.noiRACT.Visible = true;
            this.noiRACT.MouseClick += new System.Windows.Forms.MouseEventHandler(this.noiRACT_MouseClick);
            this.noiRACT.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.noiRACT_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuOpen,
            this.mnuStart,
            this.mnuStop,
            this.mnuClose});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(139, 92);
            // 
            // mnuOpen
            // 
            this.mnuOpen.Name = "mnuOpen";
            this.mnuOpen.Size = new System.Drawing.Size(138, 22);
            this.mnuOpen.Text = "열기";
            this.mnuOpen.Click += new System.EventHandler(this.mnuOpen_Click);
            // 
            // mnuStart
            // 
            this.mnuStart.Name = "mnuStart";
            this.mnuStart.Size = new System.Drawing.Size(138, 22);
            this.mnuStart.Text = "서비스 시작";
            this.mnuStart.Click += new System.EventHandler(this.mnuStart_Click);
            // 
            // mnuStop
            // 
            this.mnuStop.Name = "mnuStop";
            this.mnuStop.Size = new System.Drawing.Size(138, 22);
            this.mnuStop.Text = "서비스 중지";
            this.mnuStop.Click += new System.EventHandler(this.mnuStop_Click);
            // 
            // mnuClose
            // 
            this.mnuClose.Name = "mnuClose";
            this.mnuClose.Size = new System.Drawing.Size(138, 22);
            this.mnuClose.Text = "종료";
            this.mnuClose.Click += new System.EventHandler(this.mnuClose_Click);
            // 
            // ServiceManagerMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 425);
            this.Controls.Add(this.superTabControl1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ServiceManagerMainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Server Service Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServiceManagerMainForm_FormClosing);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.superTabControl1)).EndInit();
            this.superTabControl1.ResumeLayout(false);
            this.superTabControlPanel2.ResumeLayout(false);
            this.superTabControlPanel1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private MKLibrary.Controls.MKButton btnClose;
        private DevComponents.DotNetBar.SuperTabControl superTabControl1;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel2;
        private DevComponents.DotNetBar.SuperTabItem tabDaemonDataList;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel1;
        private DevComponents.DotNetBar.SuperTabItem tabConnectionData;
        private ucConnectionDataPanel pnlConnectionData;
        private ucDaemonDataListPanel pnlDaemonDataList;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuStart;
        private System.Windows.Forms.ToolStripMenuItem mnuStop;
        private System.Windows.Forms.ToolStripMenuItem mnuClose;
        private System.Windows.Forms.NotifyIcon noiRACT;
        private System.Windows.Forms.ToolStripMenuItem mnuOpen;
    }
}

