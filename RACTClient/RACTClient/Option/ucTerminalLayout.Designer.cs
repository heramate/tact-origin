namespace RACTClient
{
    partial class ucTerminalLayout
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
            this.rdoTab = new System.Windows.Forms.RadioButton();
            this.rdoPopup = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.nudHeight = new System.Windows.Forms.NumericUpDown();
            this.nudWidth = new System.Windows.Forms.NumericUpDown();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rdoTID = new System.Windows.Forms.RadioButton();
            this.rdoIPAddress = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rdoPopup2 = new System.Windows.Forms.RadioButton();
            this.rdoTab2 = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.nudColumnCount = new System.Windows.Forms.NumericUpDown();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudColumnCount)).BeginInit();
            this.SuspendLayout();
            // 
            // rdoTab
            // 
            this.rdoTab.AutoSize = true;
            this.rdoTab.Checked = true;
            this.rdoTab.Location = new System.Drawing.Point(76, 21);
            this.rdoTab.Name = "rdoTab";
            this.rdoTab.Size = new System.Drawing.Size(63, 16);
            this.rdoTab.TabIndex = 2;
            this.rdoTab.TabStop = true;
            this.rdoTab.Text = "탭 연결";
            this.rdoTab.UseVisualStyleBackColor = true;
            // 
            // rdoPopup
            // 
            this.rdoPopup.AutoSize = true;
            this.rdoPopup.Location = new System.Drawing.Point(158, 21);
            this.rdoPopup.Name = "rdoPopup";
            this.rdoPopup.Size = new System.Drawing.Size(75, 16);
            this.rdoPopup.TabIndex = 2;
            this.rdoPopup.Text = "새로운 창";
            this.rdoPopup.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoPopup);
            this.groupBox1.Controls.Add(this.rdoTab);
            this.groupBox1.Location = new System.Drawing.Point(17, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(297, 51);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "터미널 연결 타입";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.nudHeight);
            this.groupBox2.Controls.Add(this.nudWidth);
            this.groupBox2.Location = new System.Drawing.Point(17, 134);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(297, 51);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "창 크기";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(156, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "높이 : ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "너비 : ";
            // 
            // nudHeight
            // 
            this.nudHeight.Location = new System.Drawing.Point(204, 19);
            this.nudHeight.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudHeight.Name = "nudHeight";
            this.nudHeight.Size = new System.Drawing.Size(69, 21);
            this.nudHeight.TabIndex = 1;
            this.nudHeight.Value = new decimal(new int[] {
            28,
            0,
            0,
            0});
            // 
            // nudWidth
            // 
            this.nudWidth.Location = new System.Drawing.Point(69, 21);
            this.nudWidth.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudWidth.Name = "nudWidth";
            this.nudWidth.Size = new System.Drawing.Size(69, 21);
            this.nudWidth.TabIndex = 0;
            this.nudWidth.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rdoTID);
            this.groupBox3.Controls.Add(this.rdoIPAddress);
            this.groupBox3.Location = new System.Drawing.Point(17, 259);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(297, 69);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "장비 연결 표시 방법";
            // 
            // rdoTID
            // 
            this.rdoTID.AutoSize = true;
            this.rdoTID.Location = new System.Drawing.Point(158, 35);
            this.rdoTID.Name = "rdoTID";
            this.rdoTID.Size = new System.Drawing.Size(42, 16);
            this.rdoTID.TabIndex = 2;
            this.rdoTID.Text = "TID";
            this.rdoTID.UseVisualStyleBackColor = true;
            // 
            // rdoIPAddress
            // 
            this.rdoIPAddress.AutoSize = true;
            this.rdoIPAddress.Checked = true;
            this.rdoIPAddress.Location = new System.Drawing.Point(67, 35);
            this.rdoIPAddress.Name = "rdoIPAddress";
            this.rdoIPAddress.Size = new System.Drawing.Size(85, 16);
            this.rdoIPAddress.TabIndex = 2;
            this.rdoIPAddress.TabStop = true;
            this.rdoIPAddress.Text = "IP Address";
            this.rdoIPAddress.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rdoPopup2);
            this.groupBox4.Controls.Add(this.rdoTab2);
            this.groupBox4.Location = new System.Drawing.Point(17, 79);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(297, 49);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "메모장 실행 타입";
            // 
            // rdoPopup2
            // 
            this.rdoPopup2.AutoSize = true;
            this.rdoPopup2.Location = new System.Drawing.Point(158, 24);
            this.rdoPopup2.Name = "rdoPopup2";
            this.rdoPopup2.Size = new System.Drawing.Size(75, 16);
            this.rdoPopup2.TabIndex = 2;
            this.rdoPopup2.Text = "새로운 창";
            this.rdoPopup2.UseVisualStyleBackColor = true;
            // 
            // rdoTab2
            // 
            this.rdoTab2.AutoSize = true;
            this.rdoTab2.Checked = true;
            this.rdoTab2.Location = new System.Drawing.Point(76, 24);
            this.rdoTab2.Name = "rdoTab2";
            this.rdoTab2.Size = new System.Drawing.Size(63, 16);
            this.rdoTab2.TabIndex = 2;
            this.rdoTab2.TabStop = true;
            this.rdoTab2.Text = "탭 연결";
            this.rdoTab2.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.nudColumnCount);
            this.groupBox5.Location = new System.Drawing.Point(18, 194);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(297, 51);
            this.groupBox5.TabIndex = 5;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "터미널 컬럼수";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "컬럼수 : ";
            // 
            // nudColumnCount
            // 
            this.nudColumnCount.Location = new System.Drawing.Point(80, 21);
            this.nudColumnCount.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudColumnCount.Name = "nudColumnCount";
            this.nudColumnCount.Size = new System.Drawing.Size(69, 21);
            this.nudColumnCount.TabIndex = 0;
            this.nudColumnCount.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // ucTerminalLayout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Name = "ucTerminalLayout";
            this.Size = new System.Drawing.Size(345, 406);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudColumnCount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rdoTab;
        private System.Windows.Forms.RadioButton rdoPopup;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudHeight;
        private System.Windows.Forms.NumericUpDown nudWidth;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rdoTID;
        private System.Windows.Forms.RadioButton rdoIPAddress;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rdoPopup2;
        private System.Windows.Forms.RadioButton rdoTab2;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudColumnCount;
    }
}
