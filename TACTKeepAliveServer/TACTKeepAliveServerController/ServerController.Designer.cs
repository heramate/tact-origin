namespace TACTKeepAliveServerTester
{
    partial class ServerController
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
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtDBLoginPwd = new MKLibrary.Controls.MKTextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.txtDBLoginID = new MKLibrary.Controls.MKTextBox();
            this.txtServerIP = new MKLibrary.Controls.MKIPAddress(this.components);
            this.txtServerId = new MKLibrary.Controls.MKTextBox();
            this.txtDBName = new MKLibrary.Controls.MKTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtServerPort = new MKLibrary.Controls.MKTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.grbDBInfo = new System.Windows.Forms.GroupBox();
            this.txtDBServerIP = new MKLibrary.Controls.MKTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.grbServerInfo = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtTunnelRequestTimeout = new MKLibrary.Controls.MKTextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.chkPortListLogYN = new System.Windows.Forms.CheckBox();
            this.label30 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.txtDaemonRequestTimeout = new MKLibrary.Controls.MKTextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.chkBase64 = new System.Windows.Forms.CheckBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtSSHTunnelPortRange = new MKLibrary.Controls.MKTextBox();
            this.txtTunnelTimeout = new MKLibrary.Controls.MKTextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label29 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.btnTunnelPort = new System.Windows.Forms.Button();
            this.label24 = new System.Windows.Forms.Label();
            this.tunnelPortList = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.btnDevListUpdate = new System.Windows.Forms.Button();
            this.txtReqDevList = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.txtSSHServerIP = new MKLibrary.Controls.MKTextBox();
            this.mkTextBox2 = new MKLibrary.Controls.MKTextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txtSSHUserID = new MKLibrary.Controls.MKTextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.mkTextBox5 = new MKLibrary.Controls.MKTextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.mkTextBox7 = new MKLibrary.Controls.MKTextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.txtSSHPassword = new MKLibrary.Controls.MKTextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtSSHServerPort = new MKLibrary.Controls.MKTextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.ipTestDevice = new MKLibrary.Controls.MKTextBox();
            this.btnDevListClear = new System.Windows.Forms.Button();
            this.label33 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.grbDBInfo.SuspendLayout();
            this.grbServerInfo.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Enabled = false;
            this.label8.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label8.Location = new System.Drawing.Point(10, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(297, 12);
            this.label8.TabIndex = 13;
            this.label8.Text = "※설정변경은 KAMServerConfig.xml을 수정후 재기동";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(338, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 12);
            this.label7.TabIndex = 3;
            this.label7.Text = "서버ID :";
            // 
            // txtDBLoginPwd
            // 
            this.txtDBLoginPwd.BackColorPattern = System.Drawing.Color.White;
            this.txtDBLoginPwd.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.txtDBLoginPwd.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtDBLoginPwd.BorderEdgeRadius = 3;
            this.txtDBLoginPwd.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.txtDBLoginPwd.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.txtDBLoginPwd.isLockHeight = true;
            this.txtDBLoginPwd.Lines = new string[0];
            this.txtDBLoginPwd.Location = new System.Drawing.Point(298, 70);
            this.txtDBLoginPwd.MaxLength = 2147483647;
            this.txtDBLoginPwd.Name = "txtDBLoginPwd";
            this.txtDBLoginPwd.PasswordChar = '\0';
            this.txtDBLoginPwd.Size = new System.Drawing.Size(140, 21);
            this.txtDBLoginPwd.TabIndex = 6;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(699, 455);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(84, 32);
            this.btnStart.TabIndex = 9;
            this.btnStart.Text = "시작";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // txtDBLoginID
            // 
            this.txtDBLoginID.BackColorPattern = System.Drawing.Color.White;
            this.txtDBLoginID.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.txtDBLoginID.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtDBLoginID.BorderEdgeRadius = 3;
            this.txtDBLoginID.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.txtDBLoginID.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.txtDBLoginID.isLockHeight = true;
            this.txtDBLoginID.Lines = new string[0];
            this.txtDBLoginID.Location = new System.Drawing.Point(78, 70);
            this.txtDBLoginID.MaxLength = 2147483647;
            this.txtDBLoginID.Name = "txtDBLoginID";
            this.txtDBLoginID.PasswordChar = '\0';
            this.txtDBLoginID.Size = new System.Drawing.Size(137, 21);
            this.txtDBLoginID.TabIndex = 5;
            // 
            // txtServerIP
            // 
            this.txtServerIP.BackColor = System.Drawing.SystemColors.Window;
            this.txtServerIP.BackColorPattern = System.Drawing.Color.White;
            this.txtServerIP.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.txtServerIP.BorderColor = System.Drawing.Color.DimGray;
            this.txtServerIP.BorderEdgeRadius = 3;
            this.txtServerIP.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.txtServerIP.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.Fixed;
            this.txtServerIP.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.txtServerIP.Location = new System.Drawing.Point(60, 19);
            this.txtServerIP.Name = "txtServerIP";
            this.txtServerIP.ParentIPControl = null;
            this.txtServerIP.Size = new System.Drawing.Size(151, 20);
            this.txtServerIP.TabIndex = 5;
            // 
            // txtServerId
            // 
            this.txtServerId.BackColorPattern = System.Drawing.Color.White;
            this.txtServerId.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.txtServerId.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtServerId.BorderEdgeRadius = 3;
            this.txtServerId.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.txtServerId.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.txtServerId.isLockHeight = true;
            this.txtServerId.Lines = new string[0];
            this.txtServerId.Location = new System.Drawing.Point(388, 20);
            this.txtServerId.MaxLength = 2147483647;
            this.txtServerId.Name = "txtServerId";
            this.txtServerId.PasswordChar = '\0';
            this.txtServerId.Size = new System.Drawing.Size(52, 21);
            this.txtServerId.TabIndex = 4;
            // 
            // txtDBName
            // 
            this.txtDBName.BackColorPattern = System.Drawing.Color.White;
            this.txtDBName.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.txtDBName.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtDBName.BorderEdgeRadius = 3;
            this.txtDBName.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.txtDBName.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.txtDBName.isLockHeight = true;
            this.txtDBName.Lines = new string[0];
            this.txtDBName.Location = new System.Drawing.Point(298, 30);
            this.txtDBName.MaxLength = 2147483647;
            this.txtDBName.Name = "txtDBName";
            this.txtDBName.PasswordChar = '\0';
            this.txtDBName.Size = new System.Drawing.Size(140, 21);
            this.txtDBName.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(224, 74);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "Login 암호 :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "서버 IP :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "Login 계정 :";
            // 
            // txtServerPort
            // 
            this.txtServerPort.BackColorPattern = System.Drawing.Color.White;
            this.txtServerPort.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.txtServerPort.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtServerPort.BorderEdgeRadius = 3;
            this.txtServerPort.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.txtServerPort.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.txtServerPort.isLockHeight = true;
            this.txtServerPort.Lines = new string[0];
            this.txtServerPort.Location = new System.Drawing.Point(280, 20);
            this.txtServerPort.MaxLength = 2147483647;
            this.txtServerPort.Name = "txtServerPort";
            this.txtServerPort.PasswordChar = '\0';
            this.txtServerPort.Size = new System.Drawing.Size(49, 21);
            this.txtServerPort.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(219, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "접속Port :";
            // 
            // grbDBInfo
            // 
            this.grbDBInfo.Controls.Add(this.txtDBServerIP);
            this.grbDBInfo.Controls.Add(this.txtDBLoginPwd);
            this.grbDBInfo.Controls.Add(this.txtDBLoginID);
            this.grbDBInfo.Controls.Add(this.txtDBName);
            this.grbDBInfo.Controls.Add(this.label6);
            this.grbDBInfo.Controls.Add(this.label5);
            this.grbDBInfo.Controls.Add(this.label4);
            this.grbDBInfo.Controls.Add(this.label3);
            this.grbDBInfo.Location = new System.Drawing.Point(12, 99);
            this.grbDBInfo.Name = "grbDBInfo";
            this.grbDBInfo.Size = new System.Drawing.Size(458, 105);
            this.grbDBInfo.TabIndex = 12;
            this.grbDBInfo.TabStop = false;
            this.grbDBInfo.Text = "DB 정보";
            // 
            // txtDBServerIP
            // 
            this.txtDBServerIP.BackColorPattern = System.Drawing.Color.White;
            this.txtDBServerIP.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.txtDBServerIP.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtDBServerIP.BorderEdgeRadius = 3;
            this.txtDBServerIP.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.txtDBServerIP.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.txtDBServerIP.isLockHeight = true;
            this.txtDBServerIP.Lines = new string[0];
            this.txtDBServerIP.Location = new System.Drawing.Point(78, 30);
            this.txtDBServerIP.MaxLength = 2147483647;
            this.txtDBServerIP.Name = "txtDBServerIP";
            this.txtDBServerIP.PasswordChar = '\0';
            this.txtDBServerIP.Size = new System.Drawing.Size(137, 21);
            this.txtDBServerIP.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(259, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "DB명:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "DB서버 IP :";
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(792, 455);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(84, 32);
            this.btnStop.TabIndex = 10;
            this.btnStop.Text = "중지";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // grbServerInfo
            // 
            this.grbServerInfo.Controls.Add(this.txtServerIP);
            this.grbServerInfo.Controls.Add(this.txtServerId);
            this.grbServerInfo.Controls.Add(this.label7);
            this.grbServerInfo.Controls.Add(this.txtServerPort);
            this.grbServerInfo.Controls.Add(this.label2);
            this.grbServerInfo.Controls.Add(this.label1);
            this.grbServerInfo.Location = new System.Drawing.Point(12, 35);
            this.grbServerInfo.Name = "grbServerInfo";
            this.grbServerInfo.Size = new System.Drawing.Size(458, 58);
            this.grbServerInfo.TabIndex = 11;
            this.grbServerInfo.TabStop = false;
            this.grbServerInfo.Text = "KeepAlive 서버정보";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label34);
            this.groupBox1.Controls.Add(this.label33);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.txtTunnelRequestTimeout);
            this.groupBox1.Controls.Add(this.label31);
            this.groupBox1.Controls.Add(this.label32);
            this.groupBox1.Controls.Add(this.chkPortListLogYN);
            this.groupBox1.Controls.Add(this.label30);
            this.groupBox1.Controls.Add(this.label27);
            this.groupBox1.Controls.Add(this.txtDaemonRequestTimeout);
            this.groupBox1.Controls.Add(this.label26);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.chkBase64);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.txtSSHTunnelPortRange);
            this.groupBox1.Controls.Add(this.txtTunnelTimeout);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Location = new System.Drawing.Point(12, 307);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(458, 193);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SSH터널 && KeepAlive 설정";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(256, 119);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(176, 12);
            this.label17.TabIndex = 34;
            this.label17.Text = "초 (설정값이 0이면 no timeout)";
            // 
            // txtTunnelRequestTimeout
            // 
            this.txtTunnelRequestTimeout.BackColorPattern = System.Drawing.Color.White;
            this.txtTunnelRequestTimeout.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.txtTunnelRequestTimeout.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtTunnelRequestTimeout.BorderEdgeRadius = 3;
            this.txtTunnelRequestTimeout.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.txtTunnelRequestTimeout.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.txtTunnelRequestTimeout.isLockHeight = true;
            this.txtTunnelRequestTimeout.Lines = new string[0];
            this.txtTunnelRequestTimeout.Location = new System.Drawing.Point(188, 114);
            this.txtTunnelRequestTimeout.MaxLength = 2147483647;
            this.txtTunnelRequestTimeout.Name = "txtTunnelRequestTimeout";
            this.txtTunnelRequestTimeout.PasswordChar = '\0';
            this.txtTunnelRequestTimeout.Size = new System.Drawing.Size(67, 21);
            this.txtTunnelRequestTimeout.TabIndex = 33;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(6, 119);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(180, 12);
            this.label31.TabIndex = 32;
            this.label31.Text = "터널Open/Close 응답Timeout :";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(45, 169);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(141, 12);
            this.label32.TabIndex = 28;
            this.label32.Text = "파일로그에 상세히 출력 :";
            // 
            // chkPortListLogYN
            // 
            this.chkPortListLogYN.AutoSize = true;
            this.chkPortListLogYN.Location = new System.Drawing.Point(188, 168);
            this.chkPortListLogYN.Name = "chkPortListLogYN";
            this.chkPortListLogYN.Size = new System.Drawing.Size(15, 14);
            this.chkPortListLogYN.TabIndex = 27;
            this.chkPortListLogYN.UseVisualStyleBackColor = true;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(339, 33);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(113, 12);
            this.label30.TabIndex = 31;
            this.label30.Text = "입력예: 40101,40300";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(256, 90);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(176, 12);
            this.label27.TabIndex = 28;
            this.label27.Text = "초 (설정값이 0이면 no timeout)";
            // 
            // txtDaemonRequestTimeout
            // 
            this.txtDaemonRequestTimeout.BackColorPattern = System.Drawing.Color.White;
            this.txtDaemonRequestTimeout.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.txtDaemonRequestTimeout.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtDaemonRequestTimeout.BorderEdgeRadius = 3;
            this.txtDaemonRequestTimeout.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.txtDaemonRequestTimeout.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.txtDaemonRequestTimeout.isLockHeight = true;
            this.txtDaemonRequestTimeout.Lines = new string[0];
            this.txtDaemonRequestTimeout.Location = new System.Drawing.Point(188, 85);
            this.txtDaemonRequestTimeout.MaxLength = 2147483647;
            this.txtDaemonRequestTimeout.Name = "txtDaemonRequestTimeout";
            this.txtDaemonRequestTimeout.PasswordChar = '\0';
            this.txtDaemonRequestTimeout.Size = new System.Drawing.Size(67, 21);
            this.txtDaemonRequestTimeout.TabIndex = 27;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(51, 90);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(135, 12);
            this.label26.TabIndex = 26;
            this.label26.Text = "데몬요청처리 Timeout :";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(64, 148);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(122, 12);
            this.label20.TabIndex = 25;
            this.label20.Text = "Base64 인코딩 적용 :";
            // 
            // chkBase64
            // 
            this.chkBase64.AutoSize = true;
            this.chkBase64.Location = new System.Drawing.Point(188, 146);
            this.chkBase64.Name = "chkBase64";
            this.chkBase64.Size = new System.Drawing.Size(15, 14);
            this.chkBase64.TabIndex = 24;
            this.chkBase64.UseVisualStyleBackColor = true;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(256, 61);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(176, 12);
            this.label18.TabIndex = 22;
            this.label18.Text = "초 (설정값이 0이면 no timeout)";
            // 
            // txtSSHTunnelPortRange
            // 
            this.txtSSHTunnelPortRange.BackColorPattern = System.Drawing.Color.White;
            this.txtSSHTunnelPortRange.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.txtSSHTunnelPortRange.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtSSHTunnelPortRange.BorderEdgeRadius = 3;
            this.txtSSHTunnelPortRange.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.txtSSHTunnelPortRange.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.txtSSHTunnelPortRange.isLockHeight = true;
            this.txtSSHTunnelPortRange.Lines = new string[0];
            this.txtSSHTunnelPortRange.Location = new System.Drawing.Point(188, 28);
            this.txtSSHTunnelPortRange.MaxLength = 2147483647;
            this.txtSSHTunnelPortRange.Name = "txtSSHTunnelPortRange";
            this.txtSSHTunnelPortRange.PasswordChar = '\0';
            this.txtSSHTunnelPortRange.Size = new System.Drawing.Size(148, 21);
            this.txtSSHTunnelPortRange.TabIndex = 5;
            // 
            // txtTunnelTimeout
            // 
            this.txtTunnelTimeout.BackColorPattern = System.Drawing.Color.White;
            this.txtTunnelTimeout.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.txtTunnelTimeout.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtTunnelTimeout.BorderEdgeRadius = 3;
            this.txtTunnelTimeout.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.txtTunnelTimeout.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.txtTunnelTimeout.isLockHeight = true;
            this.txtTunnelTimeout.Lines = new string[0];
            this.txtTunnelTimeout.Location = new System.Drawing.Point(188, 57);
            this.txtTunnelTimeout.MaxLength = 2147483647;
            this.txtTunnelTimeout.Name = "txtTunnelTimeout";
            this.txtTunnelTimeout.PasswordChar = '\0';
            this.txtTunnelTimeout.Size = new System.Drawing.Size(67, 21);
            this.txtTunnelTimeout.TabIndex = 20;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(35, 61);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(151, 12);
            this.label16.TabIndex = 19;
            this.label16.Text = "터널 미사용 판단Timeout :";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(73, 31);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(113, 12);
            this.label10.TabIndex = 0;
            this.label10.Text = "터널 할당 포트범위:";
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(642, 379);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(76, 27);
            this.btnTest.TabIndex = 14;
            this.btnTest.Text = "장비추가";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(485, 365);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(201, 12);
            this.label15.TabIndex = 10;
            this.label15.Text = "[테스트] 데몬의 장비접속요청 추가:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label29);
            this.groupBox2.Controls.Add(this.label28);
            this.groupBox2.Controls.Add(this.btnTunnelPort);
            this.groupBox2.Controls.Add(this.label24);
            this.groupBox2.Controls.Add(this.tunnelPortList);
            this.groupBox2.Controls.Add(this.label22);
            this.groupBox2.Controls.Add(this.btnDevListUpdate);
            this.groupBox2.Controls.Add(this.txtReqDevList);
            this.groupBox2.Location = new System.Drawing.Point(481, 35);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(395, 315);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "터널포트 모니터";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(11, 38);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(276, 12);
            this.label29.TabIndex = 31;
            this.label29.Text = "장비IP        ┃요청옵션┃할당포트┃발송대기시작";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(12, 165);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(320, 12);
            this.label28.TabIndex = 30;
            this.label28.Text = "장비IP        ┃포트   ┃터널상태  ┃세션수┃상태변경시각";
            // 
            // btnTunnelPort
            // 
            this.btnTunnelPort.Location = new System.Drawing.Point(297, 265);
            this.btnTunnelPort.Name = "btnTunnelPort";
            this.btnTunnelPort.Size = new System.Drawing.Size(92, 29);
            this.btnTunnelPort.TabIndex = 29;
            this.btnTunnelPort.Text = "목록갱신";
            this.btnTunnelPort.UseVisualStyleBackColor = true;
            this.btnTunnelPort.Click += new System.EventHandler(this.btnTunnelPort_Click);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(12, 144);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(119, 12);
            this.label24.TabIndex = 28;
            this.label24.Text = "▼ 터널포트 상태감시";
            // 
            // tunnelPortList
            // 
            this.tunnelPortList.Location = new System.Drawing.Point(13, 180);
            this.tunnelPortList.Multiline = true;
            this.tunnelPortList.Name = "tunnelPortList";
            this.tunnelPortList.Size = new System.Drawing.Size(376, 84);
            this.tunnelPortList.TabIndex = 27;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(11, 19);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(208, 12);
            this.label22.TabIndex = 26;
            this.label22.Text = "▼ 장비에 보낼 요청(KAM수신대기중)";
            // 
            // btnDevListUpdate
            // 
            this.btnDevListUpdate.Location = new System.Drawing.Point(297, 123);
            this.btnDevListUpdate.Name = "btnDevListUpdate";
            this.btnDevListUpdate.Size = new System.Drawing.Size(92, 29);
            this.btnDevListUpdate.TabIndex = 20;
            this.btnDevListUpdate.Text = "목록갱신";
            this.btnDevListUpdate.UseVisualStyleBackColor = true;
            this.btnDevListUpdate.Click += new System.EventHandler(this.btnDevListUpdate_Click);
            // 
            // txtReqDevList
            // 
            this.txtReqDevList.Location = new System.Drawing.Point(13, 53);
            this.txtReqDevList.Multiline = true;
            this.txtReqDevList.Name = "txtReqDevList";
            this.txtReqDevList.Size = new System.Drawing.Size(376, 64);
            this.txtReqDevList.TabIndex = 19;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(366, 12);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(516, 12);
            this.label14.TabIndex = 18;
            this.label14.Text = "데몬요청수신→(KAM수신대기)→장비에 터널요청→터널포트Open대기→데몬에 터널포트 응답";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBox1);
            this.groupBox3.Controls.Add(this.txtSSHServerIP);
            this.groupBox3.Controls.Add(this.mkTextBox2);
            this.groupBox3.Controls.Add(this.label19);
            this.groupBox3.Controls.Add(this.txtSSHUserID);
            this.groupBox3.Controls.Add(this.label21);
            this.groupBox3.Controls.Add(this.mkTextBox5);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.mkTextBox7);
            this.groupBox3.Controls.Add(this.label23);
            this.groupBox3.Controls.Add(this.txtSSHPassword);
            this.groupBox3.Controls.Add(this.label25);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.txtSSHServerPort);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Location = new System.Drawing.Point(12, 215);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(458, 86);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "SSH 서버정보";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(288, 135);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(89, 16);
            this.checkBox1.TabIndex = 24;
            this.checkBox1.Text = "Base64적용";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // txtSSHServerIP
            // 
            this.txtSSHServerIP.BackColorPattern = System.Drawing.Color.White;
            this.txtSSHServerIP.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.txtSSHServerIP.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtSSHServerIP.BorderEdgeRadius = 3;
            this.txtSSHServerIP.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.txtSSHServerIP.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.txtSSHServerIP.isLockHeight = true;
            this.txtSSHServerIP.Lines = new string[0];
            this.txtSSHServerIP.Location = new System.Drawing.Point(80, 23);
            this.txtSSHServerIP.MaxLength = 2147483647;
            this.txtSSHServerIP.Name = "txtSSHServerIP";
            this.txtSSHServerIP.PasswordChar = '\0';
            this.txtSSHServerIP.Size = new System.Drawing.Size(137, 21);
            this.txtSSHServerIP.TabIndex = 8;
            // 
            // mkTextBox2
            // 
            this.mkTextBox2.BackColorPattern = System.Drawing.Color.White;
            this.mkTextBox2.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.mkTextBox2.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.mkTextBox2.BorderEdgeRadius = 3;
            this.mkTextBox2.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.mkTextBox2.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.mkTextBox2.isLockHeight = true;
            this.mkTextBox2.Lines = new string[0];
            this.mkTextBox2.Location = new System.Drawing.Point(166, 159);
            this.mkTextBox2.MaxLength = 2147483647;
            this.mkTextBox2.Name = "mkTextBox2";
            this.mkTextBox2.PasswordChar = '\0';
            this.mkTextBox2.Size = new System.Drawing.Size(67, 21);
            this.mkTextBox2.TabIndex = 23;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(296, 180);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(27, 12);
            this.label19.TabIndex = 22;
            this.label19.Text = "(분)";
            // 
            // txtSSHUserID
            // 
            this.txtSSHUserID.BackColorPattern = System.Drawing.Color.White;
            this.txtSSHUserID.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.txtSSHUserID.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtSSHUserID.BorderEdgeRadius = 3;
            this.txtSSHUserID.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.txtSSHUserID.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.txtSSHUserID.isLockHeight = true;
            this.txtSSHUserID.Lines = new string[0];
            this.txtSSHUserID.Location = new System.Drawing.Point(80, 54);
            this.txtSSHUserID.MaxLength = 2147483647;
            this.txtSSHUserID.Name = "txtSSHUserID";
            this.txtSSHUserID.PasswordChar = '\0';
            this.txtSSHUserID.Size = new System.Drawing.Size(137, 21);
            this.txtSSHUserID.TabIndex = 9;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(10, 163);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(151, 12);
            this.label21.TabIndex = 21;
            this.label21.Text = "터널당 최대 세션(텔넷) 수:";
            // 
            // mkTextBox5
            // 
            this.mkTextBox5.BackColorPattern = System.Drawing.Color.White;
            this.mkTextBox5.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.mkTextBox5.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.mkTextBox5.BorderEdgeRadius = 3;
            this.mkTextBox5.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.mkTextBox5.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.mkTextBox5.isLockHeight = true;
            this.mkTextBox5.Lines = new string[0];
            this.mkTextBox5.Location = new System.Drawing.Point(166, 108);
            this.mkTextBox5.MaxLength = 2147483647;
            this.mkTextBox5.Name = "mkTextBox5";
            this.mkTextBox5.PasswordChar = '\0';
            this.mkTextBox5.Size = new System.Drawing.Size(175, 21);
            this.mkTextBox5.TabIndex = 5;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(36, 58);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(24, 12);
            this.label13.TabIndex = 8;
            this.label13.Text = "ID :";
            // 
            // mkTextBox7
            // 
            this.mkTextBox7.BackColorPattern = System.Drawing.Color.White;
            this.mkTextBox7.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.mkTextBox7.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.mkTextBox7.BorderEdgeRadius = 3;
            this.mkTextBox7.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.mkTextBox7.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.mkTextBox7.isLockHeight = true;
            this.mkTextBox7.Lines = new string[0];
            this.mkTextBox7.Location = new System.Drawing.Point(166, 132);
            this.mkTextBox7.MaxLength = 2147483647;
            this.mkTextBox7.Name = "mkTextBox7";
            this.mkTextBox7.PasswordChar = '\0';
            this.mkTextBox7.Size = new System.Drawing.Size(67, 21);
            this.mkTextBox7.TabIndex = 20;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(38, 136);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(123, 12);
            this.label23.TabIndex = 19;
            this.label23.Text = "터널 미사용 Timeout:";
            // 
            // txtSSHPassword
            // 
            this.txtSSHPassword.BackColorPattern = System.Drawing.Color.White;
            this.txtSSHPassword.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.txtSSHPassword.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtSSHPassword.BorderEdgeRadius = 3;
            this.txtSSHPassword.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.txtSSHPassword.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.txtSSHPassword.isLockHeight = true;
            this.txtSSHPassword.Lines = new string[0];
            this.txtSSHPassword.Location = new System.Drawing.Point(300, 49);
            this.txtSSHPassword.MaxLength = 2147483647;
            this.txtSSHPassword.Name = "txtSSHPassword";
            this.txtSSHPassword.PasswordChar = '\0';
            this.txtSSHPassword.Size = new System.Drawing.Size(140, 21);
            this.txtSSHPassword.TabIndex = 6;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(48, 111);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(113, 12);
            this.label25.TabIndex = 0;
            this.label25.Text = "터널 할당 포트범위:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(10, 32);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(52, 12);
            this.label12.TabIndex = 0;
            this.label12.Text = "서버 IP :";
            // 
            // txtSSHServerPort
            // 
            this.txtSSHServerPort.BackColorPattern = System.Drawing.Color.White;
            this.txtSSHServerPort.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.txtSSHServerPort.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtSSHServerPort.BorderEdgeRadius = 3;
            this.txtSSHServerPort.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.txtSSHServerPort.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.txtSSHServerPort.isLockHeight = true;
            this.txtSSHServerPort.Lines = new string[0];
            this.txtSSHServerPort.Location = new System.Drawing.Point(300, 20);
            this.txtSSHServerPort.MaxLength = 2147483647;
            this.txtSSHServerPort.Name = "txtSSHServerPort";
            this.txtSSHServerPort.PasswordChar = '\0';
            this.txtSSHServerPort.Size = new System.Drawing.Size(140, 21);
            this.txtSSHServerPort.TabIndex = 4;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(237, 24);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(61, 12);
            this.label11.TabIndex = 0;
            this.label11.Text = "접속포트 :";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(228, 53);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 12);
            this.label9.TabIndex = 0;
            this.label9.Text = "Password :";
            // 
            // ipTestDevice
            // 
            this.ipTestDevice.BackColorPattern = System.Drawing.Color.White;
            this.ipTestDevice.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.ipTestDevice.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ipTestDevice.BorderEdgeRadius = 3;
            this.ipTestDevice.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.ipTestDevice.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.ipTestDevice.isLockHeight = true;
            this.ipTestDevice.Lines = new string[] {
        "100.64.131.75"};
            this.ipTestDevice.Location = new System.Drawing.Point(487, 380);
            this.ipTestDevice.MaxLength = 2147483647;
            this.ipTestDevice.Name = "ipTestDevice";
            this.ipTestDevice.PasswordChar = '\0';
            this.ipTestDevice.Size = new System.Drawing.Size(149, 21);
            this.ipTestDevice.TabIndex = 25;
            this.ipTestDevice.Text = "100.64.131.75";
            // 
            // btnDevListClear
            // 
            this.btnDevListClear.Location = new System.Drawing.Point(724, 379);
            this.btnDevListClear.Name = "btnDevListClear";
            this.btnDevListClear.Size = new System.Drawing.Size(152, 29);
            this.btnDevListClear.TabIndex = 26;
            this.btnDevListClear.Text = "요청&&감시터널 초기화";
            this.btnDevListClear.UseVisualStyleBackColor = true;
            this.btnDevListClear.Click += new System.EventHandler(this.btnDevListClear_Click);
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(209, 169);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(143, 12);
            this.label33.TabIndex = 35;
            this.label33.Text = "(디버깅 메시지까지 출력)";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(209, 148);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(194, 12);
            this.label34.TabIndex = 36;
            this.label34.Text = "(장비측과 KAM통신규약대로 설정)";
            // 
            // ServerController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(916, 512);
            this.Controls.Add(this.btnDevListClear);
            this.Controls.Add(this.ipTestDevice);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.grbDBInfo);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.grbServerInfo);
            this.Name = "ServerController";
            this.Text = "KeepAliveServerController";
            this.grbDBInfo.ResumeLayout(false);
            this.grbDBInfo.PerformLayout();
            this.grbServerInfo.ResumeLayout(false);
            this.grbServerInfo.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private MKLibrary.Controls.MKTextBox txtDBLoginPwd;
        private System.Windows.Forms.Button btnStart;
        private MKLibrary.Controls.MKTextBox txtDBLoginID;
        private MKLibrary.Controls.MKIPAddress txtServerIP;
        private MKLibrary.Controls.MKTextBox txtServerId;
        private MKLibrary.Controls.MKTextBox txtDBName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private MKLibrary.Controls.MKTextBox txtServerPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox grbDBInfo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.GroupBox grbServerInfo;
        private System.Windows.Forms.GroupBox groupBox1;
        private MKLibrary.Controls.MKTextBox txtSSHTunnelPortRange;
        private System.Windows.Forms.Label label10;
        private MKLibrary.Controls.MKTextBox txtDBServerIP;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnDevListUpdate;
        private System.Windows.Forms.TextBox txtReqDevList;
        private System.Windows.Forms.Label label14;
        private MKLibrary.Controls.MKTextBox txtTunnelTimeout;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.CheckBox chkBase64;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBox1;
        private MKLibrary.Controls.MKTextBox txtSSHServerIP;
        private MKLibrary.Controls.MKTextBox mkTextBox2;
        private System.Windows.Forms.Label label19;
        private MKLibrary.Controls.MKTextBox txtSSHUserID;
        private System.Windows.Forms.Label label21;
        private MKLibrary.Controls.MKTextBox mkTextBox5;
        private System.Windows.Forms.Label label13;
        private MKLibrary.Controls.MKTextBox mkTextBox7;
        private System.Windows.Forms.Label label23;
        private MKLibrary.Controls.MKTextBox txtSSHPassword;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label12;
        private MKLibrary.Controls.MKTextBox txtSSHServerPort;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox tunnelPortList;
        private MKLibrary.Controls.MKTextBox ipTestDevice;
        private System.Windows.Forms.Label label27;
        private MKLibrary.Controls.MKTextBox txtDaemonRequestTimeout;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Button btnTunnelPort;
        private System.Windows.Forms.Button btnDevListClear;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.CheckBox chkPortListLogYN;
        private System.Windows.Forms.Label label17;
        private MKLibrary.Controls.MKTextBox txtTunnelRequestTimeout;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.Label label33;
    }
}

