namespace SSHTerminalControlTest
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
            this.terminalEmulator1 = new WalburySoftware.TerminalEmulator();
            this.txtIPAddress = new System.Windows.Forms.TextBox();
            this.txtConnect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // terminalEmulator1
            // 
            this.terminalEmulator1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(160)))));
            this.terminalEmulator1.Columns = 87;
            this.terminalEmulator1.ConnectionType = WalburySoftware.TerminalEmulator.ConnectionTypes.Telnet;
            this.terminalEmulator1.Font = new System.Drawing.Font("Courier New", 8F);
            this.terminalEmulator1.Hostname = null;
            this.terminalEmulator1.Location = new System.Drawing.Point(-3, 53);
            this.terminalEmulator1.Name = "terminalEmulator1";
            this.terminalEmulator1.Password = null;
            this.terminalEmulator1.Port = 0;
            this.terminalEmulator1.Rows = 24;
            this.terminalEmulator1.Size = new System.Drawing.Size(616, 315);
            this.terminalEmulator1.TabIndex = 0;
            this.terminalEmulator1.Text = "terminalEmulator1";
            this.terminalEmulator1.Username = null;
            // 
            // txtIPAddress
            // 
            this.txtIPAddress.Location = new System.Drawing.Point(12, 12);
            this.txtIPAddress.Name = "txtIPAddress";
            this.txtIPAddress.Size = new System.Drawing.Size(121, 21);
            this.txtIPAddress.TabIndex = 3;
            this.txtIPAddress.Text = "10.30.6.160";
            // 
            // txtConnect
            // 
            this.txtConnect.Location = new System.Drawing.Point(538, 12);
            this.txtConnect.Name = "txtConnect";
            this.txtConnect.Size = new System.Drawing.Size(75, 23);
            this.txtConnect.TabIndex = 4;
            this.txtConnect.Text = "연결";
            this.txtConnect.UseVisualStyleBackColor = true;
            this.txtConnect.Click += new System.EventHandler(this.txtConnect_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(625, 380);
            this.Controls.Add(this.txtConnect);
            this.Controls.Add(this.txtIPAddress);
            this.Controls.Add(this.terminalEmulator1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private WalburySoftware.TerminalEmulator terminalEmulator1;
        private System.Windows.Forms.TextBox txtIPAddress;
        private System.Windows.Forms.Button txtConnect;
    }
}

