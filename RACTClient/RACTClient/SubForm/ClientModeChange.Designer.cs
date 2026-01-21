namespace RACTClient
{
    partial class ClientModeChange
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rdoConsole = new System.Windows.Forms.RadioButton();
            this.rdoOnline = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(214, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "TACT Client 실행 모드를 변경 합니다.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(477, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "Online 모드는 시설정보에 등록된 장비 접속 및 스크립트 단축 명령이 사용 가능 합니다.";
            // 
            // rdoConsole
            // 
            this.rdoConsole.AutoSize = true;
            this.rdoConsole.Location = new System.Drawing.Point(307, 33);
            this.rdoConsole.Name = "rdoConsole";
            this.rdoConsole.Size = new System.Drawing.Size(106, 16);
            this.rdoConsole.TabIndex = 5;
            this.rdoConsole.Text = "Console Mode";
            this.rdoConsole.UseVisualStyleBackColor = true;
            // 
            // rdoOnline
            // 
            this.rdoOnline.AutoSize = true;
            this.rdoOnline.Checked = true;
            this.rdoOnline.Location = new System.Drawing.Point(54, 33);
            this.rdoOnline.Name = "rdoOnline";
            this.rdoOnline.Size = new System.Drawing.Size(95, 16);
            this.rdoOnline.TabIndex = 4;
            this.rdoOnline.TabStop = true;
            this.rdoOnline.Text = "Online Mode";
            this.rdoOnline.UseVisualStyleBackColor = true;
            this.rdoOnline.CheckedChanged += new System.EventHandler(this.rdoOnline_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoConsole);
            this.groupBox1.Controls.Add(this.rdoOnline);
            this.groupBox1.Location = new System.Drawing.Point(12, 64);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(483, 73);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "모드 선택";
            // 
            // ClientModeChange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 184);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ClientModeChange";
            this.Text = "ClientModeChange";
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rdoConsole;
        private System.Windows.Forms.RadioButton rdoOnline;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}