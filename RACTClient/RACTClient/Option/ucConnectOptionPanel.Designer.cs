using MKLibrary.Controls;
namespace RACTClient
{
    partial class ucConnectOptionPanel
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
            this.cboDefaultProtocol = new MKLibrary.Controls.MKComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoErase = new System.Windows.Forms.RadioButton();
            this.rdoFill = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.sendDelay = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "기본 프로토콜 :";
            // 
            // cboDefaultProtocol
            // 
            this.cboDefaultProtocol.BackColor = System.Drawing.SystemColors.Control;
            this.cboDefaultProtocol.BackColorSelected = System.Drawing.Color.Orange;
            this.cboDefaultProtocol.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.cboDefaultProtocol.BorderEdgeRadius = 3;
            this.cboDefaultProtocol.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.cboDefaultProtocol.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.None;
            this.cboDefaultProtocol.BoxBorderColor = System.Drawing.Color.Gray;
            this.cboDefaultProtocol.ColorBoxSize = new System.Drawing.Size(12, 8);
            this.cboDefaultProtocol.ComboBoxStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cboDefaultProtocol.ImageList = null;
            this.cboDefaultProtocol.ItemHeight = 14;
            this.cboDefaultProtocol.Location = new System.Drawing.Point(130, 14);
            this.cboDefaultProtocol.MaxDorpDownWidth = 500;
            this.cboDefaultProtocol.Name = "cboDefaultProtocol";
            this.cboDefaultProtocol.SelectedIndex = -1;
            this.cboDefaultProtocol.ShowColorBox = false;
            this.cboDefaultProtocol.Size = new System.Drawing.Size(170, 21);
            this.cboDefaultProtocol.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoErase);
            this.groupBox1.Controls.Add(this.rdoFill);
            this.groupBox1.Location = new System.Drawing.Point(20, 52);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 51);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "커맨드 텍스트 유지 타입";
            // 
            // rdoErase
            // 
            this.rdoErase.AutoSize = true;
            this.rdoErase.Location = new System.Drawing.Point(144, 20);
            this.rdoErase.Name = "rdoErase";
            this.rdoErase.Size = new System.Drawing.Size(115, 16);
            this.rdoErase.TabIndex = 2;
            this.rdoErase.Text = "기존 텍스트 삭제";
            this.rdoErase.UseVisualStyleBackColor = true;
            // 
            // rdoFill
            // 
            this.rdoFill.AutoSize = true;
            this.rdoFill.Checked = true;
            this.rdoFill.Location = new System.Drawing.Point(23, 20);
            this.rdoFill.Name = "rdoFill";
            this.rdoFill.Size = new System.Drawing.Size(115, 16);
            this.rdoFill.TabIndex = 2;
            this.rdoFill.TabStop = true;
            this.rdoFill.Text = "기존 텍스트 유지";
            this.rdoFill.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.sendDelay);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(20, 109);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(280, 63);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "명령어 라인 처리 속도";
            // 
            // sendDelay
            // 
            this.sendDelay.Location = new System.Drawing.Point(92, 24);
            this.sendDelay.Maximum = new decimal(new int[] {
            6000,
            0,
            0,
            0});
            this.sendDelay.Name = "sendDelay";
            this.sendDelay.Size = new System.Drawing.Size(73, 21);
            this.sendDelay.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(171, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "milliseconds";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "전송 지연 :";
            // 
            // ucConnectOptionPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cboDefaultProtocol);
            this.Controls.Add(this.label1);
            this.Name = "ucConnectOptionPanel";
            this.Size = new System.Drawing.Size(332, 213);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        internal MKComboBox cboDefaultProtocol;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoErase;
        private System.Windows.Forms.RadioButton rdoFill;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown sendDelay;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}
