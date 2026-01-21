namespace RACTClient
{
    partial class BatchCmdForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblFileOpen = new System.Windows.Forms.Label();
            this.nmSendTerm = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlCmd = new System.Windows.Forms.Panel();
            this.rTxtBatchCmd = new System.Windows.Forms.RichTextBox();
            this.btnClose = new MKLibrary.Controls.MKButton(this.components);
            this.btnExecCmd = new MKLibrary.Controls.MKButton(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.nmSendTerm)).BeginInit();
            this.pnlCmd.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblFileOpen
            // 
            this.lblFileOpen.AutoSize = true;
            this.lblFileOpen.Location = new System.Drawing.Point(14, 17);
            this.lblFileOpen.Name = "lblFileOpen";
            this.lblFileOpen.Size = new System.Drawing.Size(81, 12);
            this.lblFileOpen.TabIndex = 176;
            this.lblFileOpen.Text = "▶ 전송 간격 :";
            // 
            // nmSendTerm
            // 
            this.nmSendTerm.Location = new System.Drawing.Point(105, 13);
            this.nmSendTerm.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nmSendTerm.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.nmSendTerm.Name = "nmSendTerm";
            this.nmSendTerm.Size = new System.Drawing.Size(75, 21);
            this.nmSendTerm.TabIndex = 177;
            this.nmSendTerm.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(183, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 12);
            this.label1.TabIndex = 178;
            this.label1.Text = "초 (1초 ~ 60초)";
            // 
            // pnlCmd
            // 
            this.pnlCmd.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlCmd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCmd.Controls.Add(this.rTxtBatchCmd);
            this.pnlCmd.Location = new System.Drawing.Point(16, 51);
            this.pnlCmd.Name = "pnlCmd";
            this.pnlCmd.Size = new System.Drawing.Size(449, 259);
            this.pnlCmd.TabIndex = 180;
            // 
            // rTxtBatchCmd
            // 
            this.rTxtBatchCmd.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rTxtBatchCmd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rTxtBatchCmd.Location = new System.Drawing.Point(0, 0);
            this.rTxtBatchCmd.Name = "rTxtBatchCmd";
            this.rTxtBatchCmd.Size = new System.Drawing.Size(447, 257);
            this.rTxtBatchCmd.TabIndex = 0;
            this.rTxtBatchCmd.Text = "";
            this.rTxtBatchCmd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnKeyDownEvent);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackgroundImageDisable = null;
            this.btnClose.BackgroundImageHover = null;
            this.btnClose.BackgroundImageNormal = null;
            this.btnClose.BackgroundImageSelect = null;
            this.btnClose.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnClose.BorderEdgeRadius = 3;
            this.btnClose.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnClose.ButtonImageCenter = null;
            this.btnClose.ButtonImageLeft = null;
            this.btnClose.ButtonImageRight = null;
            this.btnClose.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnClose, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnClose.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnClose.ColorDepthFocus = 2;
            this.btnClose.ColorDepthHover = 2;
            this.btnClose.ColorDepthShadow = 2;
            this.btnClose.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnClose.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnClose.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnClose.IconIndexDisable = -1;
            this.btnClose.IconIndexHover = -1;
            this.btnClose.IconIndexNormal = -1;
            this.btnClose.IconIndexSelect = -1;
            this.btnClose.Image = null;
            this.btnClose.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnClose.ImageIndent = 0;
            this.btnClose.ImageList = null;
            this.btnClose.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnClose.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnClose.Location = new System.Drawing.Point(399, 321);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(66, 22);
            this.btnClose.TabIndex = 181;
            this.btnClose.Text = "취소";
            this.btnClose.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnClose.TextIndent = 0;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnExecCmd
            // 
            this.btnExecCmd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExecCmd.BackgroundImageDisable = null;
            this.btnExecCmd.BackgroundImageHover = null;
            this.btnExecCmd.BackgroundImageNormal = null;
            this.btnExecCmd.BackgroundImageSelect = null;
            this.btnExecCmd.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnExecCmd.BorderEdgeRadius = 3;
            this.btnExecCmd.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnExecCmd.ButtonImageCenter = null;
            this.btnExecCmd.ButtonImageLeft = null;
            this.btnExecCmd.ButtonImageRight = null;
            this.btnExecCmd.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnExecCmd, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnExecCmd.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnExecCmd.ColorDepthFocus = 2;
            this.btnExecCmd.ColorDepthHover = 2;
            this.btnExecCmd.ColorDepthShadow = 2;
            this.btnExecCmd.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnExecCmd.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnExecCmd.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnExecCmd.IconIndexDisable = -1;
            this.btnExecCmd.IconIndexHover = -1;
            this.btnExecCmd.IconIndexNormal = -1;
            this.btnExecCmd.IconIndexSelect = -1;
            this.btnExecCmd.Image = null;
            this.btnExecCmd.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnExecCmd.ImageIndent = 0;
            this.btnExecCmd.ImageList = null;
            this.btnExecCmd.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnExecCmd.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnExecCmd.Location = new System.Drawing.Point(317, 321);
            this.btnExecCmd.Name = "btnExecCmd";
            this.btnExecCmd.Size = new System.Drawing.Size(76, 22);
            this.btnExecCmd.TabIndex = 182;
            this.btnExecCmd.Text = "명령실행";
            this.btnExecCmd.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnExecCmd.TextIndent = 0;
            this.btnExecCmd.Click += new System.EventHandler(this.OnClick_Btn_Submit);
            // 
            // BatchCmdForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 355);
            this.Controls.Add(this.btnExecCmd);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pnlCmd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nmSendTerm);
            this.Controls.Add(this.lblFileOpen);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "BatchCmdForm";
            this.Text = "일괄 명령실행";
            this.Click += new System.EventHandler(this.OnClick_Btn_Submit);
            ((System.ComponentModel.ISupportInitialize)(this.nmSendTerm)).EndInit();
            this.pnlCmd.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFileOpen;
        private System.Windows.Forms.NumericUpDown nmSendTerm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlCmd;
        private System.Windows.Forms.RichTextBox rTxtBatchCmd;
        private MKLibrary.Controls.MKButton btnClose;
        private MKLibrary.Controls.MKButton btnExecCmd;
    }
}