namespace RACTClient
{
    partial class ucCommandLine
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
            this.mkTextBox1 = new MKLibrary.Controls.MKTextBox();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.SuspendLayout();
            // 
            // mkTextBox1
            // 
            this.mkTextBox1.BackColorPattern = System.Drawing.Color.White;
            this.mkTextBox1.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.mkTextBox1.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.mkTextBox1.BorderEdgeRadius = 3;
            this.mkTextBox1.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.mkTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mkTextBox1.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.mkTextBox1.isLockHeight = true;
            this.mkTextBox1.Lines = new string[0];
            this.mkTextBox1.Location = new System.Drawing.Point(0, 21);
            this.mkTextBox1.MaxLength = 2147483647;
            this.mkTextBox1.MultiLine = true;
            this.mkTextBox1.Name = "mkTextBox1";
            this.mkTextBox1.PasswordChar = '\0';
            this.mkTextBox1.Size = new System.Drawing.Size(435, 346);
            this.mkTextBox1.TabIndex = 3;
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(435, 21);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 2;
            this.panelEx1.Text = "명령 실행 창";
            // 
            // ucCommandLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mkTextBox1);
            this.Controls.Add(this.panelEx1);
            this.Name = "ucCommandLine";
            this.Size = new System.Drawing.Size(435, 367);
            this.ResumeLayout(false);

        }

        #endregion

        private MKLibrary.Controls.MKTextBox mkTextBox1;
        private DevComponents.DotNetBar.PanelEx panelEx1;
    }
}
