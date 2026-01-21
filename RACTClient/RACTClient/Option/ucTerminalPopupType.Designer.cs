namespace RACTClient
{
    partial class ucTerminalPopupType
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
            this.label1 = new System.Windows.Forms.Label();
            this.rdoPopup = new System.Windows.Forms.RadioButton();
            this.rdoCopyPaste = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rdoAllTerminal = new System.Windows.Forms.RadioButton();
            this.rdoActiveTerminal = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.rdoAutoLoginNotUse = new System.Windows.Forms.RadioButton();
            this.rdoAutoLoginUse = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.rdoAutoMoreStringNotUse = new System.Windows.Forms.RadioButton();
            this.rdoAutoMoreStringUse = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.rdoTerminalCloseN = new System.Windows.Forms.RadioButton();
            this.rdoTerminalCloseY = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(309, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "마우스 오른쪽 버튼 클릭시 사용할 이벤트를 선택하세요.";
            // 
            // rdoPopup
            // 
            this.rdoPopup.AutoSize = true;
            this.rdoPopup.Checked = true;
            this.rdoPopup.Location = new System.Drawing.Point(22, 38);
            this.rdoPopup.Name = "rdoPopup";
            this.rdoPopup.Size = new System.Drawing.Size(103, 16);
            this.rdoPopup.TabIndex = 1;
            this.rdoPopup.TabStop = true;
            this.rdoPopup.Text = "팝업 메뉴 표시";
            this.rdoPopup.UseVisualStyleBackColor = true;
            // 
            // rdoCopyPaste
            // 
            this.rdoCopyPaste.AutoSize = true;
            this.rdoCopyPaste.Location = new System.Drawing.Point(170, 39);
            this.rdoCopyPaste.Name = "rdoCopyPaste";
            this.rdoCopyPaste.Size = new System.Drawing.Size(111, 16);
            this.rdoCopyPaste.TabIndex = 2;
            this.rdoCopyPaste.Text = "복사 && 붙여넣기";
            this.rdoCopyPaste.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.rdoCopyPaste);
            this.groupBox1.Controls.Add(this.rdoPopup);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(330, 65);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "마우스";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.rdoAllTerminal);
            this.groupBox2.Controls.Add(this.rdoActiveTerminal);
            this.groupBox2.Location = new System.Drawing.Point(12, 84);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(330, 65);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "단축/스크립트 명령";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(263, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "단축/스크립트 명령을 수행할 창을 선택 하세요.";
            // 
            // rdoAllTerminal
            // 
            this.rdoAllTerminal.AutoSize = true;
            this.rdoAllTerminal.Location = new System.Drawing.Point(170, 42);
            this.rdoAllTerminal.Name = "rdoAllTerminal";
            this.rdoAllTerminal.Size = new System.Drawing.Size(87, 16);
            this.rdoAllTerminal.TabIndex = 2;
            this.rdoAllTerminal.Text = "전체 터미널";
            this.rdoAllTerminal.UseVisualStyleBackColor = true;
            // 
            // rdoActiveTerminal
            // 
            this.rdoActiveTerminal.AutoSize = true;
            this.rdoActiveTerminal.Checked = true;
            this.rdoActiveTerminal.Location = new System.Drawing.Point(22, 42);
            this.rdoActiveTerminal.Name = "rdoActiveTerminal";
            this.rdoActiveTerminal.Size = new System.Drawing.Size(111, 16);
            this.rdoActiveTerminal.TabIndex = 1;
            this.rdoActiveTerminal.TabStop = true;
            this.rdoActiveTerminal.Text = "활성화된 터미널";
            this.rdoActiveTerminal.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.rdoAutoLoginNotUse);
            this.groupBox3.Controls.Add(this.rdoAutoLoginUse);
            this.groupBox3.Location = new System.Drawing.Point(12, 158);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(330, 60);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "터미널 자동 로그인";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(245, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "터미널 자동 로그인 사용 여부를 선택하세요.";
            // 
            // rdoAutoLoginNotUse
            // 
            this.rdoAutoLoginNotUse.AutoSize = true;
            this.rdoAutoLoginNotUse.Location = new System.Drawing.Point(170, 38);
            this.rdoAutoLoginNotUse.Name = "rdoAutoLoginNotUse";
            this.rdoAutoLoginNotUse.Size = new System.Drawing.Size(71, 16);
            this.rdoAutoLoginNotUse.TabIndex = 2;
            this.rdoAutoLoginNotUse.Text = "사용안함";
            this.rdoAutoLoginNotUse.UseVisualStyleBackColor = true;
            // 
            // rdoAutoLoginUse
            // 
            this.rdoAutoLoginUse.AutoSize = true;
            this.rdoAutoLoginUse.Checked = true;
            this.rdoAutoLoginUse.Location = new System.Drawing.Point(22, 39);
            this.rdoAutoLoginUse.Name = "rdoAutoLoginUse";
            this.rdoAutoLoginUse.Size = new System.Drawing.Size(47, 16);
            this.rdoAutoLoginUse.TabIndex = 1;
            this.rdoAutoLoginUse.TabStop = true;
            this.rdoAutoLoginUse.Text = "사용";
            this.rdoAutoLoginUse.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.rdoAutoMoreStringNotUse);
            this.groupBox4.Controls.Add(this.rdoAutoMoreStringUse);
            this.groupBox4.Location = new System.Drawing.Point(12, 227);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(330, 64);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "터미널 More문자 자동 스크롤";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(286, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "모델에 More문자에 대한 입력이 되어 있어야됩니다.";
            // 
            // rdoAutoMoreStringNotUse
            // 
            this.rdoAutoMoreStringNotUse.AutoSize = true;
            this.rdoAutoMoreStringNotUse.Location = new System.Drawing.Point(170, 42);
            this.rdoAutoMoreStringNotUse.Name = "rdoAutoMoreStringNotUse";
            this.rdoAutoMoreStringNotUse.Size = new System.Drawing.Size(71, 16);
            this.rdoAutoMoreStringNotUse.TabIndex = 2;
            this.rdoAutoMoreStringNotUse.Text = "사용안함";
            this.rdoAutoMoreStringNotUse.UseVisualStyleBackColor = true;
            // 
            // rdoAutoMoreStringUse
            // 
            this.rdoAutoMoreStringUse.AutoSize = true;
            this.rdoAutoMoreStringUse.Checked = true;
            this.rdoAutoMoreStringUse.Location = new System.Drawing.Point(22, 43);
            this.rdoAutoMoreStringUse.Name = "rdoAutoMoreStringUse";
            this.rdoAutoMoreStringUse.Size = new System.Drawing.Size(47, 16);
            this.rdoAutoMoreStringUse.TabIndex = 1;
            this.rdoAutoMoreStringUse.TabStop = true;
            this.rdoAutoMoreStringUse.Text = "사용";
            this.rdoAutoMoreStringUse.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.rdoTerminalCloseN);
            this.groupBox5.Controls.Add(this.rdoTerminalCloseY);
            this.groupBox5.Location = new System.Drawing.Point(12, 299);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(330, 64);
            this.groupBox5.TabIndex = 5;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "터미널 창 종료";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(273, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "세션 종료시 터미널 창 닫히는 여부를 선택하세요.";
            // 
            // rdoTerminalCloseN
            // 
            this.rdoTerminalCloseN.AutoSize = true;
            this.rdoTerminalCloseN.Checked = true;
            this.rdoTerminalCloseN.Location = new System.Drawing.Point(170, 42);
            this.rdoTerminalCloseN.Name = "rdoTerminalCloseN";
            this.rdoTerminalCloseN.Size = new System.Drawing.Size(71, 16);
            this.rdoTerminalCloseN.TabIndex = 2;
            this.rdoTerminalCloseN.TabStop = true;
            this.rdoTerminalCloseN.Text = "사용안함";
            this.rdoTerminalCloseN.UseVisualStyleBackColor = true;
            // 
            // rdoTerminalCloseY
            // 
            this.rdoTerminalCloseY.AutoSize = true;
            this.rdoTerminalCloseY.Location = new System.Drawing.Point(22, 43);
            this.rdoTerminalCloseY.Name = "rdoTerminalCloseY";
            this.rdoTerminalCloseY.Size = new System.Drawing.Size(47, 16);
            this.rdoTerminalCloseY.TabIndex = 1;
            this.rdoTerminalCloseY.Text = "사용";
            this.rdoTerminalCloseY.UseVisualStyleBackColor = true;
            // 
            // ucTerminalPopupType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "ucTerminalPopupType";
            this.Size = new System.Drawing.Size(354, 424);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rdoPopup;
        private System.Windows.Forms.RadioButton rdoCopyPaste;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rdoAllTerminal;
        private System.Windows.Forms.RadioButton rdoActiveTerminal;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rdoAutoLoginNotUse;
        private System.Windows.Forms.RadioButton rdoAutoLoginUse;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rdoAutoMoreStringNotUse;
        private System.Windows.Forms.RadioButton rdoAutoMoreStringUse;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton rdoTerminalCloseN;
        private System.Windows.Forms.RadioButton rdoTerminalCloseY;
    }
}
