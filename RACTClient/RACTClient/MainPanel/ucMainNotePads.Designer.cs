namespace RACTClient
{
    partial class ucMainNotePads
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
            this.tabNotePads = new DevComponents.DotNetBar.SuperTabControl();
            this.contextMenuBar1 = new DevComponents.DotNetBar.ContextMenuBar();
            this.cmTabPopup = new DevComponents.DotNetBar.ButtonItem();
            this.mnuCloseOther = new DevComponents.DotNetBar.ButtonItem();
            this.mnuChangeStatus = new DevComponents.DotNetBar.ButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.tabNotePads)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabNotePads
            // 
            this.tabNotePads.BackColor = System.Drawing.Color.White;
            this.tabNotePads.CloseButtonOnTabsVisible = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tabNotePads.ControlBox.CloseBox.Name = "";
            // 
            // 
            // 
            this.tabNotePads.ControlBox.MenuBox.Name = "";
            this.tabNotePads.ControlBox.Name = "";
            this.tabNotePads.ControlBox.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.tabNotePads.ControlBox.MenuBox,
            this.tabNotePads.ControlBox.CloseBox});
            this.tabNotePads.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabNotePads.Location = new System.Drawing.Point(0, 0);
            this.tabNotePads.Name = "tabNotePads";
            this.tabNotePads.ReorderTabsEnabled = true;
            this.tabNotePads.SelectedTabFont = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.tabNotePads.SelectedTabIndex = 0;
            this.tabNotePads.Size = new System.Drawing.Size(150, 147);
            this.tabNotePads.TabFont = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tabNotePads.TabIndex = 0;
            this.tabNotePads.Text = "superTabControl1";
            this.tabNotePads.TabItemClose += new System.EventHandler<DevComponents.DotNetBar.SuperTabStripTabItemCloseEventArgs>(this.tabNotePads_TabItemClose);
            this.tabNotePads.SelectedTabChanged += new System.EventHandler<DevComponents.DotNetBar.SuperTabStripSelectedTabChangedEventArgs>(this.tabNotePads_SelectedTabChanged);
            // 
            // contextMenuBar1
            // 
            this.contextMenuBar1.AntiAlias = true;
            this.contextMenuBar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.cmTabPopup});
            this.contextMenuBar1.Location = new System.Drawing.Point(25, 130);
            this.contextMenuBar1.Name = "contextMenuBar1";
            this.contextMenuBar1.Size = new System.Drawing.Size(106, 27);
            this.contextMenuBar1.Stretch = true;
            this.contextMenuBar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.contextMenuBar1.TabIndex = 2;
            this.contextMenuBar1.TabStop = false;
            this.contextMenuBar1.Text = "contextMenuBar1";
            // 
            // cmTabPopup
            // 
            this.cmTabPopup.AutoExpandOnClick = true;
            this.cmTabPopup.Name = "cmTabPopup";
            this.cmTabPopup.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.mnuCloseOther,
            this.mnuChangeStatus});
            this.cmTabPopup.Text = "buttonItem1";
            // 
            // mnuCloseOther
            // 
            this.mnuCloseOther.Name = "mnuCloseOther";
            this.mnuCloseOther.Text = "현재노트만 남기고 모두 닫기";
            this.mnuCloseOther.Click += new System.EventHandler(this.mnuCloseOther_Click);
            // 
            // mnuChangeStatus
            // 
            this.mnuChangeStatus.BeginGroup = true;
            this.mnuChangeStatus.Name = "mnuChangeStatus";
            this.mnuChangeStatus.Text = "탭으로 분리";
            this.mnuChangeStatus.Click += new System.EventHandler(this.mnuChangeStatus_Click);
            // 
            // ucMainNotePads
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.contextMenuBar1);
            this.Controls.Add(this.tabNotePads);
            this.Name = "ucMainNotePads";
            this.Size = new System.Drawing.Size(150, 147);
            ((System.ComponentModel.ISupportInitialize)(this.tabNotePads)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.SuperTabControl tabNotePads;
        private DevComponents.DotNetBar.ContextMenuBar contextMenuBar1;
        private DevComponents.DotNetBar.ButtonItem cmTabPopup;
        private DevComponents.DotNetBar.ButtonItem mnuCloseOther;
        private DevComponents.DotNetBar.ButtonItem mnuChangeStatus;

    }
}
