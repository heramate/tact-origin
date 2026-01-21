namespace RACTClient
{
    partial class ucTelnetPanel
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
            this.txtConnectionPort = new MKLibrary.Controls.MKTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "연결 Port :";
            // 
            // txtConnectionPort
            // 
            this.txtConnectionPort.BackColorPattern = System.Drawing.Color.White;
            this.txtConnectionPort.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.txtConnectionPort.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtConnectionPort.BorderEdgeRadius = 3;
            this.txtConnectionPort.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.txtConnectionPort.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.txtConnectionPort.isLockHeight = true;
            this.txtConnectionPort.Lines = new string[0];
            this.txtConnectionPort.Location = new System.Drawing.Point(130, 14);
            this.txtConnectionPort.MaxLength = 2147483647;
            this.txtConnectionPort.Name = "txtConnectionPort";
            this.txtConnectionPort.PasswordChar = '\0';
            this.txtConnectionPort.Size = new System.Drawing.Size(170, 21);
            this.txtConnectionPort.TabIndex = 1;
            // 
            // ucTelnetPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.Controls.Add(this.txtConnectionPort);
            this.Controls.Add(this.label1);
            this.Name = "ucTelnetPanel";
            this.Size = new System.Drawing.Size(350, 150);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        internal MKLibrary.Controls.MKTextBox txtConnectionPort;
    }
}
