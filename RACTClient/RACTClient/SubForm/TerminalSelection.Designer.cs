namespace RACTClient
{
    partial class TerminalSelection
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
            this.components = new System.ComponentModel.Container();
            this.lstTerminal = new MKLibrary.Controls.MKCheckedListBox(this.components);
            this.SuspendLayout();
            // 
            // lstTerminal
            // 
            this.lstTerminal.BackColor = System.Drawing.SystemColors.Window;
            this.lstTerminal.BackColorSelected = System.Drawing.Color.Orange;
            this.lstTerminal.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lstTerminal.BorderEdgeRadius = 3;
            this.lstTerminal.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.lstTerminal.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.Flat;
            this.lstTerminal.BoxBorderColor = System.Drawing.Color.Gray;
            this.lstTerminal.CheckOnClick = false;
            this.lstTerminal.ColorBoxSize = new System.Drawing.Size(12, 8);
            this.lstTerminal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstTerminal.ImageList = null;
            this.lstTerminal.ItemHeight = 16;
            this.lstTerminal.Location = new System.Drawing.Point(0, 0);
            this.lstTerminal.Name = "lstTerminal";
            this.lstTerminal.SelectedIndex = -1;
            this.lstTerminal.ShowColorBox = false;
            this.lstTerminal.Size = new System.Drawing.Size(150, 150);
            this.lstTerminal.TabIndex = 0;
            // 
            // TerminalSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lstTerminal);
            this.Name = "TerminalSelection";
            this.ResumeLayout(false);

        }

        #endregion

        public MKLibrary.Controls.MKCheckedListBox lstTerminal;

    }
}
