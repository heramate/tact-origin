namespace RACTServerTest
{
    partial class Form1
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
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.grbDBInfo = new System.Windows.Forms.GroupBox();
            this.txtDBServerIP = new MKLibrary.Controls.MKTextBox();
            this.txtDBLoginPwd = new MKLibrary.Controls.MKTextBox();
            this.txtDBLoginID = new MKLibrary.Controls.MKTextBox();
            this.txtDBName = new MKLibrary.Controls.MKTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.grbServerInfo = new System.Windows.Forms.GroupBox();
            this.txtServerIP = new MKLibrary.Controls.MKIPAddress(this.components);
            this.txtServerId = new MKLibrary.Controls.MKTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtServerPort = new MKLibrary.Controls.MKTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.grbDBInfo.SuspendLayout();
            this.grbServerInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(246, 259);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(79, 28);
            this.button2.TabIndex = 2;
            this.button2.Text = "중지";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(154, 259);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(84, 28);
            this.button1.TabIndex = 1;
            this.button1.Text = "서버실행";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
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
            this.grbDBInfo.Location = new System.Drawing.Point(12, 122);
            this.grbDBInfo.Name = "grbDBInfo";
            this.grbDBInfo.Size = new System.Drawing.Size(458, 118);
            this.grbDBInfo.TabIndex = 4;
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
            this.txtDBServerIP.Location = new System.Drawing.Point(83, 30);
            this.txtDBServerIP.MaxLength = 2147483647;
            this.txtDBServerIP.Name = "txtDBServerIP";
            this.txtDBServerIP.PasswordChar = '\0';
            this.txtDBServerIP.Size = new System.Drawing.Size(137, 21);
            this.txtDBServerIP.TabIndex = 7;
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
            this.txtDBLoginPwd.Location = new System.Drawing.Point(305, 70);
            this.txtDBLoginPwd.MaxLength = 2147483647;
            this.txtDBLoginPwd.Name = "txtDBLoginPwd";
            this.txtDBLoginPwd.PasswordChar = '\0';
            this.txtDBLoginPwd.Size = new System.Drawing.Size(140, 21);
            this.txtDBLoginPwd.TabIndex = 6;
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
            this.txtDBLoginID.Location = new System.Drawing.Point(83, 70);
            this.txtDBLoginID.MaxLength = 2147483647;
            this.txtDBLoginID.Name = "txtDBLoginID";
            this.txtDBLoginID.PasswordChar = '\0';
            this.txtDBLoginID.Size = new System.Drawing.Size(137, 21);
            this.txtDBLoginID.TabIndex = 5;
            this.txtDBLoginID.TextChanged += new System.EventHandler(this.txtDBLoginID_TextChanged);
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
            this.txtDBName.Location = new System.Drawing.Point(305, 30);
            this.txtDBName.MaxLength = 2147483647;
            this.txtDBName.Name = "txtDBName";
            this.txtDBName.PasswordChar = '\0';
            this.txtDBName.Size = new System.Drawing.Size(140, 21);
            this.txtDBName.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(226, 74);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "Login 암호 :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "Login 계정 :";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(261, 34);
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
            // grbServerInfo
            // 
            this.grbServerInfo.Controls.Add(this.txtServerIP);
            this.grbServerInfo.Controls.Add(this.txtServerId);
            this.grbServerInfo.Controls.Add(this.label7);
            this.grbServerInfo.Controls.Add(this.txtServerPort);
            this.grbServerInfo.Controls.Add(this.label2);
            this.grbServerInfo.Controls.Add(this.label1);
            this.grbServerInfo.Location = new System.Drawing.Point(12, 28);
            this.grbServerInfo.Name = "grbServerInfo";
            this.grbServerInfo.Size = new System.Drawing.Size(458, 88);
            this.grbServerInfo.TabIndex = 3;
            this.grbServerInfo.TabStop = false;
            this.grbServerInfo.Text = "서버 정보";
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
            this.txtServerIP.Location = new System.Drawing.Point(83, 19);
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
            this.txtServerId.Location = new System.Drawing.Point(83, 53);
            this.txtServerId.MaxLength = 2147483647;
            this.txtServerId.Name = "txtServerId";
            this.txtServerId.PasswordChar = '\0';
            this.txtServerId.Size = new System.Drawing.Size(113, 21);
            this.txtServerId.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(28, 57);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 12);
            this.label7.TabIndex = 3;
            this.label7.Text = "서버ID :";
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
            this.txtServerPort.Location = new System.Drawing.Point(317, 20);
            this.txtServerPort.MaxLength = 2147483647;
            this.txtServerPort.Name = "txtServerPort";
            this.txtServerPort.PasswordChar = '\0';
            this.txtServerPort.Size = new System.Drawing.Size(79, 21);
            this.txtServerPort.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(251, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "접속Port :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "서버 IP :";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Enabled = false;
            this.label8.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label8.Location = new System.Drawing.Point(225, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(245, 12);
            this.label8.TabIndex = 5;
            this.label8.Text = "※환경설정 변경은 System.xml을 수정할 것";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 306);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.grbDBInfo);
            this.Controls.Add(this.grbServerInfo);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "TACTServer 실행테스트(no service)";
            this.Load += new System.EventHandler(this.MainTest_Load);
            this.grbDBInfo.ResumeLayout(false);
            this.grbDBInfo.PerformLayout();
            this.grbServerInfo.ResumeLayout(false);
            this.grbServerInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox grbDBInfo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox grbServerInfo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private MKLibrary.Controls.MKIPAddress txtServerIP;
        private MKLibrary.Controls.MKTextBox txtServerPort;
        private MKLibrary.Controls.MKTextBox txtDBName;
        private MKLibrary.Controls.MKTextBox txtDBLoginPwd;
        private MKLibrary.Controls.MKTextBox txtDBLoginID;
        private MKLibrary.Controls.MKTextBox txtServerId;
        private MKLibrary.Controls.MKTextBox txtDBServerIP;
        private System.Windows.Forms.Label label8;
    }
}

