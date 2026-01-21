namespace RACTClient
{
    partial class ucGroupTree
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
            this.treeViewEx1 = new RACTClient.TreeViewEx();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuBar1 = new DevComponents.DotNetBar.ContextMenuBar();
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            this.mnuConnectGroupDevice = new DevComponents.DotNetBar.ButtonItem();
            this.mnuAddGroup = new DevComponents.DotNetBar.ButtonItem();
            this.mnuModifyGroup = new DevComponents.DotNetBar.ButtonItem();
            this.mnuDeleteGroup = new DevComponents.DotNetBar.ButtonItem();
            this.mnuConnectDevice = new DevComponents.DotNetBar.ButtonItem();
            this.mnuTL1Connect = new DevComponents.DotNetBar.ButtonItem();
            this.mnuAddDevice = new DevComponents.DotNetBar.ButtonItem();
            this.mnuAddUsrDevice = new DevComponents.DotNetBar.ButtonItem();
            this.mnuProperiesDevice = new DevComponents.DotNetBar.ButtonItem();
            this.mnuDeleteDevice = new DevComponents.DotNetBar.ButtonItem();
            this.mnuShareDevice = new DevComponents.DotNetBar.ButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // treeViewEx1
            // 
            this.treeViewEx1.CheckedImageIndex = 0;
            this.treeViewEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewEx1.ImageIndex = 0;
            this.treeViewEx1.ImageList = this.imageList1;
            this.treeViewEx1.IndeterminateImageIndex = 0;
            this.treeViewEx1.Location = new System.Drawing.Point(0, 0);
            this.treeViewEx1.Name = "treeViewEx1";
            this.treeViewEx1.SelectedImageIndex = 0;
            this.treeViewEx1.Size = new System.Drawing.Size(150, 405);
            this.treeViewEx1.TabIndex = 0;
            this.treeViewEx1.UncheckedImageIndex = 0;
            this.treeViewEx1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeViewEx1_MouseDoubleClick);
            this.treeViewEx1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewEx1_AfterSelect);
            this.treeViewEx1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeViewEx1_MouseDown);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // contextMenuBar1
            // 
            this.contextMenuBar1.AntiAlias = true;
            this.contextMenuBar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItem1});
            this.contextMenuBar1.Location = new System.Drawing.Point(31, 358);
            this.contextMenuBar1.Name = "contextMenuBar1";
            this.contextMenuBar1.Size = new System.Drawing.Size(75, 27);
            this.contextMenuBar1.Stretch = true;
            this.contextMenuBar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.contextMenuBar1.TabIndex = 2;
            this.contextMenuBar1.TabStop = false;
            this.contextMenuBar1.Text = "contextMenuBar1";
            // 
            // buttonItem1
            // 
            this.buttonItem1.AutoExpandOnClick = true;
            this.buttonItem1.Name = "buttonItem1";
            this.buttonItem1.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.mnuConnectGroupDevice,
            this.mnuAddGroup,
            this.mnuModifyGroup,
            this.mnuDeleteGroup,
            this.mnuConnectDevice,
            this.mnuTL1Connect,
            this.mnuAddDevice,
            this.mnuShareDevice,
            this.mnuAddUsrDevice,
            this.mnuProperiesDevice,
            this.mnuDeleteDevice});
            this.buttonItem1.Text = "buttonItem1";
            // 
            // mnuConnectGroupDevice
            // 
            this.mnuConnectGroupDevice.Name = "mnuConnectGroupDevice";
            this.mnuConnectGroupDevice.Text = "그룹 장비 연결";
            this.mnuConnectGroupDevice.Click += new System.EventHandler(this.mnuConnectGroupDevice_Click);
            // 
            // mnuAddGroup
            // 
            this.mnuAddGroup.BeginGroup = true;
            this.mnuAddGroup.Name = "mnuAddGroup";
            this.mnuAddGroup.Text = "새 그룹 추가";
            this.mnuAddGroup.Click += new System.EventHandler(this.mnuAddGroup_Click);
            // 
            // mnuModifyGroup
            // 
            this.mnuModifyGroup.Name = "mnuModifyGroup";
            this.mnuModifyGroup.Text = "그룹 수정";
            this.mnuModifyGroup.Click += new System.EventHandler(this.mnuModifyGroup_Click);
            // 
            // mnuDeleteGroup
            // 
            this.mnuDeleteGroup.Name = "mnuDeleteGroup";
            this.mnuDeleteGroup.Text = "그룹 삭제";
            this.mnuDeleteGroup.Click += new System.EventHandler(this.mnuDeleteGroup_Click);
            // 
            // mnuConnectDevice
            // 
            this.mnuConnectDevice.Name = "mnuConnectDevice";
            this.mnuConnectDevice.Text = "연결";
            this.mnuConnectDevice.Click += new System.EventHandler(this.mnuConnectDevice_Click);
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
            this.mnuAddDevice.Text = "장비 등록";
            this.mnuAddDevice.Click += new System.EventHandler(this.mnuAddDevice_Click);
            // 
            // mnuAddUsrDevice
            // 
            this.mnuAddUsrDevice.Name = "mnuAddUsrDevice";
            this.mnuAddUsrDevice.Text = "수동 장비 등록";
            this.mnuAddUsrDevice.Click += new System.EventHandler(this.mnuAddUsrDevice_Click);
            // 
            // mnuProperiesDevice
            // 
            this.mnuProperiesDevice.Name = "mnuProperiesDevice";
            this.mnuProperiesDevice.Text = "장비 속성";
            this.mnuProperiesDevice.Click += new System.EventHandler(this.mnuPropertiesDevice_Click);
            // 
            // mnuDeleteDevice
            // 
            this.mnuDeleteDevice.Name = "mnuDeleteDevice";
            this.mnuDeleteDevice.Text = "장비 삭제";
            this.mnuDeleteDevice.Click += new System.EventHandler(this.mnuDeleteDevice_Click);
            // 
            // mnuShareDevice
            // 
            this.mnuShareDevice.Name = "mnuShareDevice";
            this.mnuShareDevice.Text = "사용자 장비 공유";
            this.mnuShareDevice.Click += new System.EventHandler(this.mnuShareDevice_Click);
            // 
            // ucGroupTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.contextMenuBar1);
            this.Controls.Add(this.treeViewEx1);
            this.Name = "ucGroupTree";
            this.Size = new System.Drawing.Size(150, 405);
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.ContextMenuBar contextMenuBar1;
        private DevComponents.DotNetBar.ButtonItem buttonItem1;
        private DevComponents.DotNetBar.ButtonItem mnuConnectDevice;
        private DevComponents.DotNetBar.ButtonItem mnuConnectGroupDevice;
        private DevComponents.DotNetBar.ButtonItem mnuAddDevice;
        private DevComponents.DotNetBar.ButtonItem mnuProperiesDevice;
        private DevComponents.DotNetBar.ButtonItem mnuDeleteDevice;
        private DevComponents.DotNetBar.ButtonItem mnuAddGroup;
        private DevComponents.DotNetBar.ButtonItem mnuModifyGroup;
        private DevComponents.DotNetBar.ButtonItem mnuDeleteGroup;
        private System.Windows.Forms.ImageList imageList1;
        public TreeViewEx treeViewEx1;
        private DevComponents.DotNetBar.ButtonItem mnuAddUsrDevice;
        private DevComponents.DotNetBar.ButtonItem mnuShareDevice;
        private DevComponents.DotNetBar.ButtonItem mnuTL1Connect;

    }
}
