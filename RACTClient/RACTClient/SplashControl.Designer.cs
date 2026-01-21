namespace RACTClient
{
    partial class SplashControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashControl));
            this.label1 = new System.Windows.Forms.Label();
            this.txtID = new MKLibrary.Controls.MKTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPW = new MKLibrary.Controls.MKTextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.lblDisplay = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtServerIP = new MKLibrary.Controls.MKTextBox();
            this.pbStatus = new System.Windows.Forms.ProgressBar();
            this.label4 = new System.Windows.Forms.Label();
            this.rdoConsole = new System.Windows.Forms.RadioButton();
            this.rdoCS = new System.Windows.Forms.RadioButton();
            this.chkSaveID = new System.Windows.Forms.CheckBox();
            this.chkSavePW = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(418, 245);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "사용자 ID";
            // 
            // txtID
            // 
            this.txtID.BackColorPattern = System.Drawing.Color.White;
            this.txtID.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.txtID.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtID.BorderEdgeRadius = 3;
            this.txtID.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.txtID.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.txtID.isLockHeight = true;
            this.txtID.Lines = new string[0];
            this.txtID.Location = new System.Drawing.Point(496, 241);
            this.txtID.MaxLength = 2147483647;
            this.txtID.Name = "txtID";
            this.txtID.PasswordChar = '\0';
            this.txtID.Size = new System.Drawing.Size(102, 21);
            this.txtID.TabIndex = 1;
            this.txtID.TextChanged += new System.EventHandler(this.txtID_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(418, 273);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "사용자 PW";
            // 
            // txtPW
            // 
            this.txtPW.BackColorPattern = System.Drawing.Color.White;
            this.txtPW.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.txtPW.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtPW.BorderEdgeRadius = 3;
            this.txtPW.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.txtPW.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.txtPW.isLockHeight = true;
            this.txtPW.Lines = new string[0];
            this.txtPW.Location = new System.Drawing.Point(496, 268);
            this.txtPW.MaxLength = 2147483647;
            this.txtPW.Name = "txtPW";
            this.txtPW.PasswordChar = '*';
            this.txtPW.Size = new System.Drawing.Size(102, 21);
            this.txtPW.TabIndex = 2;
            this.txtPW.TextChanged += new System.EventHandler(this.txtPW_TextChanged);
            this.txtPW.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPW_KeyDown);
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.Transparent;
            this.btnLogin.BackgroundImage = global::RACTClient.Properties.Resources.b_login;
            this.btnLogin.Image = ((System.Drawing.Image)(resources.GetObject("btnLogin.Image")));
            this.btnLogin.Location = new System.Drawing.Point(501, 334);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(49, 22);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // butCancel
            // 
            this.butCancel.BackColor = System.Drawing.Color.Transparent;
            this.butCancel.Image = ((System.Drawing.Image)(resources.GetObject("butCancel.Image")));
            this.butCancel.Location = new System.Drawing.Point(562, 334);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(49, 22);
            this.butCancel.TabIndex = 4;
            this.butCancel.UseVisualStyleBackColor = false;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // lblDisplay
            // 
            this.lblDisplay.AutoSize = true;
            this.lblDisplay.BackColor = System.Drawing.Color.Transparent;
            this.lblDisplay.ForeColor = System.Drawing.Color.White;
            this.lblDisplay.Location = new System.Drawing.Point(29, 352);
            this.lblDisplay.Name = "lblDisplay";
            this.lblDisplay.Size = new System.Drawing.Size(101, 12);
            this.lblDisplay.TabIndex = 0;
            this.lblDisplay.Text = "상태 표시 하는 곳";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(29, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "서버 IP";
            this.label3.Visible = false;
            // 
            // txtServerIP
            // 
            this.txtServerIP.BackColorPattern = System.Drawing.Color.White;
            this.txtServerIP.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.txtServerIP.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtServerIP.BorderEdgeRadius = 3;
            this.txtServerIP.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.txtServerIP.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.txtServerIP.isLockHeight = true;
            this.txtServerIP.Lines = new string[] {
        "10.30.5.140"};
            this.txtServerIP.Location = new System.Drawing.Point(31, 39);
            this.txtServerIP.MaxLength = 2147483647;
            this.txtServerIP.Name = "txtServerIP";
            this.txtServerIP.PasswordChar = '\0';
            this.txtServerIP.Size = new System.Drawing.Size(182, 21);
            this.txtServerIP.TabIndex = 0;
            this.txtServerIP.Text = "10.30.5.140";
            this.txtServerIP.Visible = false;
            // 
            // pbStatus
            // 
            this.pbStatus.Location = new System.Drawing.Point(25, 385);
            this.pbStatus.MarqueeAnimationSpeed = 50;
            this.pbStatus.Name = "pbStatus";
            this.pbStatus.Size = new System.Drawing.Size(636, 13);
            this.pbStatus.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pbStatus.TabIndex = 10;
            this.pbStatus.Value = 50;
            this.pbStatus.Visible = false;
            this.pbStatus.Click += new System.EventHandler(this.pbStatus_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(418, 302);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "접근제어 실행 모드 ";
            // 
            // rdoConsole
            // 
            this.rdoConsole.AutoSize = true;
            this.rdoConsole.BackColor = System.Drawing.Color.Transparent;
            this.rdoConsole.ForeColor = System.Drawing.Color.White;
            this.rdoConsole.Location = new System.Drawing.Point(610, 300);
            this.rdoConsole.Name = "rdoConsole";
            this.rdoConsole.Size = new System.Drawing.Size(70, 16);
            this.rdoConsole.TabIndex = 6;
            this.rdoConsole.Text = "Console";
            this.rdoConsole.UseVisualStyleBackColor = false;
            this.rdoConsole.CheckedChanged += new System.EventHandler(this.rdoConsole_CheckedChanged);
            // 
            // rdoCS
            // 
            this.rdoCS.AutoSize = true;
            this.rdoCS.BackColor = System.Drawing.Color.Transparent;
            this.rdoCS.Checked = true;
            this.rdoCS.ForeColor = System.Drawing.Color.White;
            this.rdoCS.Location = new System.Drawing.Point(539, 300);
            this.rdoCS.Name = "rdoCS";
            this.rdoCS.Size = new System.Drawing.Size(59, 16);
            this.rdoCS.TabIndex = 5;
            this.rdoCS.TabStop = true;
            this.rdoCS.Text = "Online";
            this.rdoCS.UseVisualStyleBackColor = false;
            this.rdoCS.CheckedChanged += new System.EventHandler(this.rdoCS_CheckedChanged);
            // 
            // chkSaveID
            // 
            this.chkSaveID.AutoSize = true;
            this.chkSaveID.BackColor = System.Drawing.Color.Transparent;
            this.chkSaveID.ForeColor = System.Drawing.Color.White;
            this.chkSaveID.Location = new System.Drawing.Point(610, 247);
            this.chkSaveID.Name = "chkSaveID";
            this.chkSaveID.Size = new System.Drawing.Size(63, 16);
            this.chkSaveID.TabIndex = 11;
            this.chkSaveID.Text = "ID 저장";
            this.chkSaveID.UseVisualStyleBackColor = false;
            // 
            // chkSavePW
            // 
            this.chkSavePW.AutoSize = true;
            this.chkSavePW.BackColor = System.Drawing.Color.Transparent;
            this.chkSavePW.ForeColor = System.Drawing.Color.White;
            this.chkSavePW.Location = new System.Drawing.Point(610, 273);
            this.chkSavePW.Name = "chkSavePW";
            this.chkSavePW.Size = new System.Drawing.Size(70, 16);
            this.chkSavePW.TabIndex = 12;
            this.chkSavePW.Text = "PW 저장";
            this.chkSavePW.UseVisualStyleBackColor = false;
            // 
            // SplashControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.Controls.Add(this.chkSavePW);
            this.Controls.Add(this.chkSaveID);
            this.Controls.Add(this.txtPW);
            this.Controls.Add(this.txtID);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pbStatus);
            this.Controls.Add(this.rdoConsole);
            this.Controls.Add(this.rdoCS);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.txtServerIP);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblDisplay);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Name = "SplashControl";
            this.Size = new System.Drawing.Size(690, 410);
            this.Load += new System.EventHandler(this.SplashControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private MKLibrary.Controls.MKTextBox txtID;
        private System.Windows.Forms.Label label2;
        private MKLibrary.Controls.MKTextBox txtPW;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.Label lblDisplay;
        private System.Windows.Forms.Label label3;
        private MKLibrary.Controls.MKTextBox txtServerIP;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rdoConsole;
        private System.Windows.Forms.RadioButton rdoCS;
        private System.Windows.Forms.ProgressBar pbStatus;
        private System.Windows.Forms.CheckBox chkSaveID;
        private System.Windows.Forms.CheckBox chkSavePW;
    }
}
