namespace RACTClient
{
    partial class FontFrom
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFont = new System.Windows.Forms.TextBox();
            this.txtStyle = new System.Windows.Forms.TextBox();
            this.txtSize = new System.Windows.Forms.TextBox();
            this.lstFont = new MKLibrary.Controls.MKListBox(this.components);
            this.lstStyle = new MKLibrary.Controls.MKListBox(this.components);
            this.lstSize = new MKLibrary.Controls.MKListBox(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "글꼴";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(159, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "글꼴 스타일:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(306, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "크기:";
            // 
            // txtFont
            // 
            this.txtFont.Location = new System.Drawing.Point(14, 24);
            this.txtFont.Name = "txtFont";
            this.txtFont.Size = new System.Drawing.Size(141, 21);
            this.txtFont.TabIndex = 2;
            // 
            // txtStyle
            // 
            this.txtStyle.Location = new System.Drawing.Point(161, 24);
            this.txtStyle.Name = "txtStyle";
            this.txtStyle.Size = new System.Drawing.Size(141, 21);
            this.txtStyle.TabIndex = 2;
            // 
            // txtSize
            // 
            this.txtSize.Location = new System.Drawing.Point(308, 24);
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(88, 21);
            this.txtSize.TabIndex = 2;
            // 
            // lstFont
            // 
            this.lstFont.BackColor = System.Drawing.SystemColors.Window;
            this.lstFont.BackColorSelected = System.Drawing.Color.Orange;
            this.lstFont.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lstFont.BorderEdgeRadius = 3;
            this.lstFont.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.lstFont.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.Flat;
            this.lstFont.BoxBorderColor = System.Drawing.Color.Gray;
            this.lstFont.ColorBoxSize = new System.Drawing.Size(12, 8);
            this.lstFont.ImageList = null;
            this.lstFont.ItemHeight = 13;
            this.lstFont.Location = new System.Drawing.Point(14, 51);
            this.lstFont.Name = "lstFont";
            this.lstFont.SelectedIndex = -1;
            this.lstFont.ShowColorBox = false;
            this.lstFont.Size = new System.Drawing.Size(141, 119);
            this.lstFont.TabIndex = 3;
            this.lstFont.SelectedIndexChanged += new System.EventHandler(this.lstFont_SelectedIndexChanged);
            // 
            // lstStyle
            // 
            this.lstStyle.BackColor = System.Drawing.SystemColors.Window;
            this.lstStyle.BackColorSelected = System.Drawing.Color.Orange;
            this.lstStyle.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lstStyle.BorderEdgeRadius = 3;
            this.lstStyle.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.lstStyle.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.Flat;
            this.lstStyle.BoxBorderColor = System.Drawing.Color.Gray;
            this.lstStyle.ColorBoxSize = new System.Drawing.Size(12, 8);
            this.lstStyle.ImageList = null;
            this.lstStyle.ItemHeight = 13;
            this.lstStyle.Location = new System.Drawing.Point(161, 51);
            this.lstStyle.Name = "lstStyle";
            this.lstStyle.SelectedIndex = -1;
            this.lstStyle.ShowColorBox = false;
            this.lstStyle.Size = new System.Drawing.Size(141, 119);
            this.lstStyle.TabIndex = 3;
            this.lstStyle.SelectedIndexChanged += new System.EventHandler(this.lstStyle_SelectedIndexChanged);
            // 
            // lstSize
            // 
            this.lstSize.BackColor = System.Drawing.SystemColors.Window;
            this.lstSize.BackColorSelected = System.Drawing.Color.Orange;
            this.lstSize.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lstSize.BorderEdgeRadius = 3;
            this.lstSize.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.lstSize.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.Flat;
            this.lstSize.BoxBorderColor = System.Drawing.Color.Gray;
            this.lstSize.ColorBoxSize = new System.Drawing.Size(12, 8);
            this.lstSize.ImageList = null;
            this.lstSize.ItemHeight = 13;
            this.lstSize.Location = new System.Drawing.Point(308, 51);
            this.lstSize.Name = "lstSize";
            this.lstSize.SelectedIndex = -1;
            this.lstSize.ShowColorBox = false;
            this.lstSize.Size = new System.Drawing.Size(88, 119);
            this.lstSize.TabIndex = 3;
            this.lstSize.SelectedIndexChanged += new System.EventHandler(this.lstSize_SelectedIndexChanged);
            // 
            // FontFrom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 226);
            this.Controls.Add(this.lstSize);
            this.Controls.Add(this.lstStyle);
            this.Controls.Add(this.lstFont);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFont);
            this.Controls.Add(this.txtStyle);
            this.Controls.Add(this.txtSize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Name = "FontFrom";
            this.Text = "글꼴";
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtSize, 0);
            this.Controls.SetChildIndex(this.txtStyle, 0);
            this.Controls.SetChildIndex(this.txtFont, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.lstFont, 0);
            this.Controls.SetChildIndex(this.lstStyle, 0);
            this.Controls.SetChildIndex(this.lstSize, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFont;
        private System.Windows.Forms.TextBox txtStyle;
        private System.Windows.Forms.TextBox txtSize;
        private MKLibrary.Controls.MKListBox lstFont;
        private MKLibrary.Controls.MKListBox lstStyle;
        private MKLibrary.Controls.MKListBox lstSize;
    }
}