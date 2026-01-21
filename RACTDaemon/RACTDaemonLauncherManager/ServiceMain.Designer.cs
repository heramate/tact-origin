namespace RACTDaemonLauncherManager
{
    partial class ServiceMain
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
            if (m_DisposeFlag)
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServiceMain));
            this.pnlLauncherService = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtServiceName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtLauncherChannelName = new System.Windows.Forms.TextBox();
            this.txtLauncherIP = new System.Windows.Forms.TextBox();
            this.txtLauncherPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnServiceStop = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.btnServiceStart = new System.Windows.Forms.Button();
            this.lblServiceStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.picServiceStatus = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.fgServerList = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.pnlServer = new System.Windows.Forms.GroupBox();
            this.noiAMAS = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuStart = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuStop = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.닫기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pnlLauncherProcess = new System.Windows.Forms.GroupBox();
            this.btnLauncherStop = new System.Windows.Forms.Button();
            this.btnLauncherStart = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.pnlLauncherService.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picServiceStatus)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fgServerList)).BeginInit();
            this.pnlServer.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.pnlLauncherProcess.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLauncherService
            // 
            this.pnlLauncherService.Controls.Add(this.label13);
            this.pnlLauncherService.Controls.Add(this.txtServiceName);
            this.pnlLauncherService.Controls.Add(this.label6);
            this.pnlLauncherService.Controls.Add(this.txtLauncherChannelName);
            this.pnlLauncherService.Controls.Add(this.txtLauncherIP);
            this.pnlLauncherService.Controls.Add(this.txtLauncherPort);
            this.pnlLauncherService.Controls.Add(this.label2);
            this.pnlLauncherService.Controls.Add(this.btnServiceStop);
            this.pnlLauncherService.Controls.Add(this.label8);
            this.pnlLauncherService.Controls.Add(this.btnServiceStart);
            this.pnlLauncherService.Controls.Add(this.lblServiceStatus);
            this.pnlLauncherService.Controls.Add(this.label1);
            this.pnlLauncherService.Controls.Add(this.picServiceStatus);
            this.pnlLauncherService.Location = new System.Drawing.Point(15, 60);
            this.pnlLauncherService.Name = "pnlLauncherService";
            this.pnlLauncherService.Size = new System.Drawing.Size(390, 127);
            this.pnlLauncherService.TabIndex = 0;
            this.pnlLauncherService.TabStop = false;
            this.pnlLauncherService.Text = "Daemon Launcher Service";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(84, 50);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(61, 12);
            this.label13.TabIndex = 10;
            this.label13.Text = "서비스명 :";
            // 
            // txtServiceName
            // 
            this.txtServiceName.Location = new System.Drawing.Point(149, 45);
            this.txtServiceName.Name = "txtServiceName";
            this.txtServiceName.ReadOnly = true;
            this.txtServiceName.Size = new System.Drawing.Size(154, 21);
            this.txtServiceName.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(250, 77);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 12);
            this.label6.TabIndex = 8;
            this.label6.Text = "ChannelName :";
            // 
            // txtLauncherChannelName
            // 
            this.txtLauncherChannelName.Location = new System.Drawing.Point(252, 93);
            this.txtLauncherChannelName.Name = "txtLauncherChannelName";
            this.txtLauncherChannelName.ReadOnly = true;
            this.txtLauncherChannelName.Size = new System.Drawing.Size(127, 21);
            this.txtLauncherChannelName.TabIndex = 7;
            // 
            // txtLauncherIP
            // 
            this.txtLauncherIP.Location = new System.Drawing.Point(87, 93);
            this.txtLauncherIP.Name = "txtLauncherIP";
            this.txtLauncherIP.ReadOnly = true;
            this.txtLauncherIP.Size = new System.Drawing.Size(100, 21);
            this.txtLauncherIP.TabIndex = 4;
            this.txtLauncherIP.Text = "333.333.333.333";
            // 
            // txtLauncherPort
            // 
            this.txtLauncherPort.Location = new System.Drawing.Point(194, 93);
            this.txtLauncherPort.Name = "txtLauncherPort";
            this.txtLauncherPort.ReadOnly = true;
            this.txtLauncherPort.Size = new System.Drawing.Size(49, 21);
            this.txtLauncherPort.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(85, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "Launcher IP:";
            // 
            // btnServiceStop
            // 
            this.btnServiceStop.Image = global::RACTDaemonLauncherManager.Properties.Resources.Delet_16;
            this.btnServiceStop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnServiceStop.Location = new System.Drawing.Point(308, 15);
            this.btnServiceStop.Name = "btnServiceStop";
            this.btnServiceStop.Size = new System.Drawing.Size(76, 23);
            this.btnServiceStop.TabIndex = 3;
            this.btnServiceStop.Text = "중지";
            this.btnServiceStop.UseVisualStyleBackColor = true;
            this.btnServiceStop.Click += new System.EventHandler(this.btnServiceStop_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(192, 77);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 12);
            this.label8.TabIndex = 2;
            this.label8.Text = "Port :";
            // 
            // btnServiceStart
            // 
            this.btnServiceStart.Image = global::RACTDaemonLauncherManager.Properties.Resources.Bind_16_1;
            this.btnServiceStart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnServiceStart.Location = new System.Drawing.Point(231, 15);
            this.btnServiceStart.Name = "btnServiceStart";
            this.btnServiceStart.Size = new System.Drawing.Size(75, 23);
            this.btnServiceStart.TabIndex = 2;
            this.btnServiceStart.Text = "시작";
            this.btnServiceStart.UseVisualStyleBackColor = true;
            this.btnServiceStart.Click += new System.EventHandler(this.btnServiceStart_Click);
            // 
            // lblServiceStatus
            // 
            this.lblServiceStatus.AutoSize = true;
            this.lblServiceStatus.Location = new System.Drawing.Point(165, 20);
            this.lblServiceStatus.Name = "lblServiceStatus";
            this.lblServiceStatus.Size = new System.Drawing.Size(0, 12);
            this.lblServiceStatus.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(84, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "서비스상태 :";
            // 
            // picServiceStatus
            // 
            this.picServiceStatus.Image = global::RACTDaemonLauncherManager.Properties.Resources.ServiceStarted;
            this.picServiceStatus.Location = new System.Drawing.Point(12, 43);
            this.picServiceStatus.Name = "picServiceStatus";
            this.picServiceStatus.Size = new System.Drawing.Size(57, 68);
            this.picServiceStatus.TabIndex = 0;
            this.picServiceStatus.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.fgServerList);
            this.panel1.Location = new System.Drawing.Point(6, 21);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(378, 145);
            this.panel1.TabIndex = 1;
            // 
            // fgServerList
            // 
            this.fgServerList.BackColor = System.Drawing.SystemColors.Window;
            this.fgServerList.ColumnInfo = resources.GetString("fgServerList.ColumnInfo");
            this.fgServerList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fgServerList.ForeColor = System.Drawing.SystemColors.WindowText;
            this.fgServerList.Location = new System.Drawing.Point(0, 0);
            this.fgServerList.Name = "fgServerList";
            this.fgServerList.Rows.Count = 1;
            this.fgServerList.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
            this.fgServerList.Size = new System.Drawing.Size(378, 145);
            this.fgServerList.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("fgServerList.Styles"));
            this.fgServerList.TabIndex = 0;
            // 
            // pnlServer
            // 
            this.pnlServer.Controls.Add(this.panel1);
            this.pnlServer.Location = new System.Drawing.Point(15, 268);
            this.pnlServer.Name = "pnlServer";
            this.pnlServer.Size = new System.Drawing.Size(390, 176);
            this.pnlServer.TabIndex = 2;
            this.pnlServer.TabStop = false;
            this.pnlServer.Text = "RACT Daemon 실행 목록";
            // 
            // noiAMAS
            // 
            this.noiAMAS.ContextMenuStrip = this.contextMenuStrip1;
            this.noiAMAS.Icon = ((System.Drawing.Icon)(resources.GetObject("noiAMAS.Icon")));
            this.noiAMAS.Text = "RACT Daemon Launcher";
            this.noiAMAS.Visible = true;
            this.noiAMAS.DoubleClick += new System.EventHandler(this.noiAMAS_DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuStart,
            this.mnuStop,
            this.mnuInfo,
            this.닫기ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(127, 92);
            // 
            // mnuStart
            // 
            this.mnuStart.Image = global::RACTDaemonLauncherManager.Properties.Resources.Bind_16_1;
            this.mnuStart.Name = "mnuStart";
            this.mnuStart.Size = new System.Drawing.Size(126, 22);
            this.mnuStart.Text = "시작";
            // 
            // mnuStop
            // 
            this.mnuStop.Image = global::RACTDaemonLauncherManager.Properties.Resources.Delet_16;
            this.mnuStop.Name = "mnuStop";
            this.mnuStop.Size = new System.Drawing.Size(126, 22);
            this.mnuStop.Text = "중지";
            // 
            // mnuInfo
            // 
            this.mnuInfo.Image = global::RACTDaemonLauncherManager.Properties.Resources.server_32;
            this.mnuInfo.Name = "mnuInfo";
            this.mnuInfo.Size = new System.Drawing.Size(126, 22);
            this.mnuInfo.Text = "서버 정보";
            this.mnuInfo.Click += new System.EventHandler(this.mnuInfo_Click);
            // 
            // 닫기ToolStripMenuItem
            // 
            this.닫기ToolStripMenuItem.Name = "닫기ToolStripMenuItem";
            this.닫기ToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.닫기ToolStripMenuItem.Text = "닫기";
            this.닫기ToolStripMenuItem.Click += new System.EventHandler(this.닫기ToolStripMenuItem_Click);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(61, 4);
            // 
            // pnlLauncherProcess
            // 
            this.pnlLauncherProcess.Controls.Add(this.btnLauncherStop);
            this.pnlLauncherProcess.Controls.Add(this.btnLauncherStart);
            this.pnlLauncherProcess.Controls.Add(this.label5);
            this.pnlLauncherProcess.Location = new System.Drawing.Point(15, 194);
            this.pnlLauncherProcess.Name = "pnlLauncherProcess";
            this.pnlLauncherProcess.Size = new System.Drawing.Size(390, 68);
            this.pnlLauncherProcess.TabIndex = 7;
            this.pnlLauncherProcess.TabStop = false;
            this.pnlLauncherProcess.Text = "Daemon Launcher Process";
            // 
            // btnLauncherStop
            // 
            this.btnLauncherStop.Image = global::RACTDaemonLauncherManager.Properties.Resources.Delet_16;
            this.btnLauncherStop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLauncherStop.Location = new System.Drawing.Point(291, 36);
            this.btnLauncherStop.Name = "btnLauncherStop";
            this.btnLauncherStop.Size = new System.Drawing.Size(89, 23);
            this.btnLauncherStop.TabIndex = 9;
            this.btnLauncherStop.Text = " 런처중지";
            this.btnLauncherStop.UseVisualStyleBackColor = true;
            this.btnLauncherStop.Click += new System.EventHandler(this.btnLauncherStop_Click);
            // 
            // btnLauncherStart
            // 
            this.btnLauncherStart.Image = global::RACTDaemonLauncherManager.Properties.Resources.Bind_16_1;
            this.btnLauncherStart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLauncherStart.Location = new System.Drawing.Point(201, 36);
            this.btnLauncherStart.Name = "btnLauncherStart";
            this.btnLauncherStart.Size = new System.Drawing.Size(88, 23);
            this.btnLauncherStart.TabIndex = 8;
            this.btnLauncherStart.Text = " 런처시작";
            this.btnLauncherStart.UseVisualStyleBackColor = true;
            this.btnLauncherStart.Click += new System.EventHandler(this.btnLauncherStart_Click);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(5, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(262, 14);
            this.label5.TabIndex = 7;
            this.label5.Text = "서비스 실행 없이 데몬런처를 바로 실행합니다.";
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(15, 13);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(390, 43);
            this.label12.TabIndex = 8;
            this.label12.Text = "▼ 운영 실행단계: \r\nRACS_Daemon_Launcher(서비스) → RACTDaemonLauncher(DLL) \r\n→ RACTDaemonEx" +
    "e(프로세스) → RACTDaemonProcess (실 데몬)";
            // 
            // ServiceMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(420, 459);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.pnlLauncherService);
            this.Controls.Add(this.pnlServer);
            this.Controls.Add(this.pnlLauncherProcess);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ServiceMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TACT Daemon Manager (Last updated: 2020.10.08)";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ServiceMain_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnlLauncherService.ResumeLayout(false);
            this.pnlLauncherService.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picServiceStatus)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fgServerList)).EndInit();
            this.pnlServer.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.pnlLauncherProcess.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox pnlLauncherService;
        private System.Windows.Forms.PictureBox picServiceStatus;
        private System.Windows.Forms.Label lblServiceStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private C1.Win.C1FlexGrid.C1FlexGrid fgServerList;
        private System.Windows.Forms.GroupBox pnlServer;
        private System.Windows.Forms.Button btnServiceStop;
        private System.Windows.Forms.Button btnServiceStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NotifyIcon noiAMAS;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuStart;
        private System.Windows.Forms.ToolStripMenuItem mnuStop;
        private System.Windows.Forms.ToolStripMenuItem mnuInfo;
        private System.Windows.Forms.TextBox txtLauncherPort;
        private System.Windows.Forms.TextBox txtLauncherIP;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ToolStripMenuItem 닫기ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtLauncherChannelName;
        private System.Windows.Forms.GroupBox pnlLauncherProcess;
        private System.Windows.Forms.Button btnLauncherStop;
        private System.Windows.Forms.Button btnLauncherStart;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtServiceName;
    }
}

