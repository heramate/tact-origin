namespace RACTClient
{
    partial class TerminalPanel
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
            this.tabTerminal = new DevComponents.DotNetBar.SuperTabControl();
            this.contextMenuBar1 = new DevComponents.DotNetBar.ContextMenuBar();
            this.cmTabPopup = new DevComponents.DotNetBar.ButtonItem();
            this.mnuReName = new DevComponents.DotNetBar.ButtonItem();
            this.mnuCloseOther = new DevComponents.DotNetBar.ButtonItem();
            this.mnuReConnect = new DevComponents.DotNetBar.ButtonItem();
            this.mnuDisConnect = new DevComponents.DotNetBar.ButtonItem();
            this.mnuLinkConnect = new DevComponents.DotNetBar.ButtonItem();
            this.mnuNewTab = new DevComponents.DotNetBar.ButtonItem();
            this.mnuModifyDevice = new DevComponents.DotNetBar.ButtonItem();
            this.mnuRestoreCfgCmd = new DevComponents.DotNetBar.ButtonItem();
            this.mnuChangeStatus = new DevComponents.DotNetBar.ButtonItem();
            this.mnuSaveTerminalLog = new DevComponents.DotNetBar.ButtonItem();
            this.imlTab = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.tabTerminal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabTerminal
            // 
            this.tabTerminal.BackColor = System.Drawing.Color.White;
            this.tabTerminal.CloseButtonOnTabsVisible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tabTerminal.ControlBox.CloseBox.Name = "";
            // 
            // 
            // 
            this.tabTerminal.ControlBox.MenuBox.Name = "";
            this.tabTerminal.ControlBox.Name = "";
            this.tabTerminal.ControlBox.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.tabTerminal.ControlBox.MenuBox,
            this.tabTerminal.ControlBox.CloseBox});
            this.tabTerminal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabTerminal.Location = new System.Drawing.Point(0, 0);
            this.tabTerminal.Name = "tabTerminal";
            this.tabTerminal.ReorderTabsEnabled = true;
            this.tabTerminal.SelectedTabFont = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.tabTerminal.SelectedTabIndex = 0;
            this.tabTerminal.Size = new System.Drawing.Size(150, 147);
            this.tabTerminal.TabFont = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tabTerminal.TabIndex = 0;
            this.tabTerminal.Text = "superTabControl1";
            this.tabTerminal.TabItemClose += new System.EventHandler<DevComponents.DotNetBar.SuperTabStripTabItemCloseEventArgs>(this.tabTerminal_TabItemClose);
            this.tabTerminal.SelectedTabChanged += new System.EventHandler<DevComponents.DotNetBar.SuperTabStripSelectedTabChangedEventArgs>(this.tabTerminal_SelectedTabChanged);
            this.tabTerminal.Enter += new System.EventHandler(this.tabTerminal_Enter);
            this.tabTerminal.Leave += new System.EventHandler(this.tabTerminal_Leave);
            // 
            // contextMenuBar1
            // 
            this.contextMenuBar1.AntiAlias = true;
            this.contextMenuBar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.cmTabPopup});
            this.contextMenuBar1.Location = new System.Drawing.Point(24, 130);
            this.contextMenuBar1.Name = "contextMenuBar1";
            this.contextMenuBar1.Size = new System.Drawing.Size(106, 27);
            this.contextMenuBar1.Stretch = true;
            this.contextMenuBar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.contextMenuBar1.TabIndex = 1;
            this.contextMenuBar1.TabStop = false;
            this.contextMenuBar1.Text = "contextMenuBar1";
            // 
            // cmTabPopup
            // 
            this.cmTabPopup.AutoExpandOnClick = true;
            this.cmTabPopup.Name = "cmTabPopup";
            this.cmTabPopup.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.mnuReName,
            this.mnuCloseOther,
            this.mnuReConnect,
            this.mnuDisConnect,
            this.mnuLinkConnect,
            this.mnuNewTab,
            this.mnuModifyDevice,
            this.mnuRestoreCfgCmd,
            this.mnuChangeStatus,
            this.mnuSaveTerminalLog});
            this.cmTabPopup.Text = "buttonItem1";
            // 
            // mnuReName
            // 
            this.mnuReName.Name = "mnuReName";
            this.mnuReName.Text = "이름 변경";
            this.mnuReName.Click += new System.EventHandler(this.mnuReName_Click);
            // 
            // mnuCloseOther
            // 
            this.mnuCloseOther.Name = "mnuCloseOther";
            this.mnuCloseOther.Text = "현재연결만 남기고 모두 닫기";
            this.mnuCloseOther.Visible = false;
            this.mnuCloseOther.Click += new System.EventHandler(this.mnuCloseOther_Click);
            // 
            // mnuReConnect
            // 
            this.mnuReConnect.BeginGroup = true;
            this.mnuReConnect.Name = "mnuReConnect";
            this.mnuReConnect.Text = "재 연결";
            this.mnuReConnect.Click += new System.EventHandler(this.mnuReConnect_Click);
            // 
            // mnuDisConnect
            // 
            this.mnuDisConnect.Name = "mnuDisConnect";
            this.mnuDisConnect.Text = "연결 종료";
            this.mnuDisConnect.Click += new System.EventHandler(this.mnuDisConnect_Click);
            // 
            // mnuLinkConnect
            // 
            this.mnuLinkConnect.Name = "mnuLinkConnect";
            this.mnuLinkConnect.Text = "링크 장비 연결";
            this.mnuLinkConnect.Click += new System.EventHandler(this.mnuLinkConnect_Click);
            // 
            // mnuNewTab
            // 
            this.mnuNewTab.BeginGroup = true;
            this.mnuNewTab.Name = "mnuNewTab";
            this.mnuNewTab.Text = "새 탭으로 연결";
            this.mnuNewTab.Click += new System.EventHandler(this.mnuNewTab_Click);
            // 
            // mnuModifyDevice
            // 
            this.mnuModifyDevice.BeginGroup = true;
            this.mnuModifyDevice.Name = "mnuModifyDevice";
            this.mnuModifyDevice.Text = "장비 속성";
            this.mnuModifyDevice.Click += new System.EventHandler(this.mnuModifyDevice_Click);
            // 
            // mnuRestoreCfgCmd
            // 
            this.mnuRestoreCfgCmd.BeginGroup = true;
            this.mnuRestoreCfgCmd.Name = "mnuRestoreCfgCmd";
            this.mnuRestoreCfgCmd.Text = "복원 명령 실행";
            this.mnuRestoreCfgCmd.Click += new System.EventHandler(this.mnuRestoreCfgCmd_Click);
            // 
            // mnuChangeStatus
            // 
            this.mnuChangeStatus.BeginGroup = true;
            this.mnuChangeStatus.Name = "mnuChangeStatus";
            this.mnuChangeStatus.Text = "탭으로 분리";
            this.mnuChangeStatus.Click += new System.EventHandler(this.mnuChangeStatus_Click);
            // 
            // mnuSaveTerminalLog
            // 
            this.mnuSaveTerminalLog.BeginGroup = true;
            this.mnuSaveTerminalLog.Name = "mnuSaveTerminalLog";
            this.mnuSaveTerminalLog.Text = "결과저장";
            this.mnuSaveTerminalLog.Click += new System.EventHandler(this.mnuSaveTerminalLog_Click);
            // 
            // imlTab
            // 
            this.imlTab.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imlTab.ImageSize = new System.Drawing.Size(9, 9);
            this.imlTab.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // TerminalPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.contextMenuBar1);
            this.Controls.Add(this.tabTerminal);
            this.Name = "TerminalPanel";
            this.Size = new System.Drawing.Size(150, 147);
            ((System.ComponentModel.ISupportInitialize)(this.tabTerminal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.SuperTabControl tabTerminal;
        private DevComponents.DotNetBar.ContextMenuBar contextMenuBar1;
        private DevComponents.DotNetBar.ButtonItem cmTabPopup;
        private DevComponents.DotNetBar.ButtonItem mnuReConnect;
        private DevComponents.DotNetBar.ButtonItem mnuDisConnect;
        private DevComponents.DotNetBar.ButtonItem mnuNewTab;
        private DevComponents.DotNetBar.ButtonItem mnuModifyDevice;
        private System.Windows.Forms.ImageList imlTab;
        private DevComponents.DotNetBar.ButtonItem mnuReName;
        private DevComponents.DotNetBar.ButtonItem mnuCloseOther;
        private DevComponents.DotNetBar.ButtonItem mnuChangeStatus;
        private DevComponents.DotNetBar.ButtonItem mnuSaveTerminalLog;
        private DevComponents.DotNetBar.ButtonItem mnuRestoreCfgCmd;
        private DevComponents.DotNetBar.ButtonItem mnuLinkConnect;





    }
}
