namespace RACTClient
{
    partial class CmdExecResultForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CmdExecResultForm));
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnFileOpen = new MKLibrary.Controls.MKButton(this.components);
            this.txtFileOpen = new System.Windows.Forms.TextBox();
            this.lblFileOpen = new System.Windows.Forms.Label();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnClose = new MKLibrary.Controls.MKButton(this.components);
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.rdoResultByCmd = new System.Windows.Forms.RadioButton();
            this.gBoxResultByCmd = new System.Windows.Forms.GroupBox();
            this.pnlResultByCmd = new System.Windows.Forms.Panel();
            this.grdCmdList = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.btnSaveResult = new MKLibrary.Controls.MKButton(this.components);
            this.rdoResultAll = new System.Windows.Forms.RadioButton();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.pnlBody = new System.Windows.Forms.Panel();
            this.pnlResult = new System.Windows.Forms.Panel();
            this.rtxtResult = new System.Windows.Forms.RichTextBox();
            this.lblResult = new System.Windows.Forms.Label();
            this.pnlTop.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.gBoxResultByCmd.SuspendLayout();
            this.pnlResultByCmd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCmdList)).BeginInit();
            this.pnlBody.SuspendLayout();
            this.pnlResult.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.btnFileOpen);
            this.pnlTop.Controls.Add(this.txtFileOpen);
            this.pnlTop.Controls.Add(this.lblFileOpen);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(782, 52);
            this.pnlTop.TabIndex = 178;
            // 
            // btnFileOpen
            // 
            this.btnFileOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFileOpen.BackgroundImageDisable = null;
            this.btnFileOpen.BackgroundImageHover = null;
            this.btnFileOpen.BackgroundImageNormal = null;
            this.btnFileOpen.BackgroundImageSelect = null;
            this.btnFileOpen.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnFileOpen.BorderEdgeRadius = 3;
            this.btnFileOpen.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnFileOpen.ButtonImageCenter = null;
            this.btnFileOpen.ButtonImageLeft = null;
            this.btnFileOpen.ButtonImageRight = null;
            this.btnFileOpen.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnFileOpen, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnFileOpen.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnFileOpen.ColorDepthFocus = 2;
            this.btnFileOpen.ColorDepthHover = 2;
            this.btnFileOpen.ColorDepthShadow = 2;
            this.btnFileOpen.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnFileOpen.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnFileOpen.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnFileOpen.IconIndexDisable = -1;
            this.btnFileOpen.IconIndexHover = -1;
            this.btnFileOpen.IconIndexNormal = -1;
            this.btnFileOpen.IconIndexSelect = -1;
            this.btnFileOpen.Image = null;
            this.btnFileOpen.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnFileOpen.ImageIndent = 0;
            this.btnFileOpen.ImageList = null;
            this.btnFileOpen.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnFileOpen.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnFileOpen.Location = new System.Drawing.Point(697, 15);
            this.btnFileOpen.Name = "btnFileOpen";
            this.btnFileOpen.Size = new System.Drawing.Size(69, 22);
            this.btnFileOpen.TabIndex = 177;
            this.btnFileOpen.Text = "열기";
            this.btnFileOpen.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnFileOpen.TextIndent = 0;
            this.btnFileOpen.Click += new System.EventHandler(this.OnClick_OpenLogFile);
            // 
            // txtFileOpen
            // 
            this.txtFileOpen.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFileOpen.Location = new System.Drawing.Point(107, 16);
            this.txtFileOpen.Name = "txtFileOpen";
            this.txtFileOpen.ReadOnly = true;
            this.txtFileOpen.Size = new System.Drawing.Size(584, 21);
            this.txtFileOpen.TabIndex = 176;
            // 
            // lblFileOpen
            // 
            this.lblFileOpen.AutoSize = true;
            this.lblFileOpen.Location = new System.Drawing.Point(20, 19);
            this.lblFileOpen.Name = "lblFileOpen";
            this.lblFileOpen.Size = new System.Drawing.Size(81, 12);
            this.lblFileOpen.TabIndex = 175;
            this.lblFileOpen.Text = "▶ 파일 선택 :";
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnClose);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 447);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(782, 44);
            this.pnlBottom.TabIndex = 182;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
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
            this.btnClose.Location = new System.Drawing.Point(699, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(68, 22);
            this.btnClose.TabIndex = 184;
            this.btnClose.Text = "닫기";
            this.btnClose.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnClose.TextIndent = 0;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.rdoResultByCmd);
            this.pnlLeft.Controls.Add(this.gBoxResultByCmd);
            this.pnlLeft.Controls.Add(this.rdoResultAll);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Location = new System.Drawing.Point(0, 52);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(344, 395);
            this.pnlLeft.TabIndex = 183;
            // 
            // rdoResultByCmd
            // 
            this.rdoResultByCmd.AutoSize = true;
            this.rdoResultByCmd.Location = new System.Drawing.Point(22, 36);
            this.rdoResultByCmd.Name = "rdoResultByCmd";
            this.rdoResultByCmd.Size = new System.Drawing.Size(103, 16);
            this.rdoResultByCmd.TabIndex = 179;
            this.rdoResultByCmd.Text = "명령어 별 결과";
            this.rdoResultByCmd.UseVisualStyleBackColor = true;
            this.rdoResultByCmd.CheckedChanged += new System.EventHandler(this.OnClick_SeclctCmd);
            // 
            // gBoxResultByCmd
            // 
            this.gBoxResultByCmd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.gBoxResultByCmd.Controls.Add(this.pnlResultByCmd);
            this.gBoxResultByCmd.Controls.Add(this.btnSaveResult);
            this.gBoxResultByCmd.Location = new System.Drawing.Point(11, 38);
            this.gBoxResultByCmd.Name = "gBoxResultByCmd";
            this.gBoxResultByCmd.Size = new System.Drawing.Size(324, 351);
            this.gBoxResultByCmd.TabIndex = 180;
            this.gBoxResultByCmd.TabStop = false;
            this.gBoxResultByCmd.Text = "                          ";
            // 
            // pnlResultByCmd
            // 
            this.pnlResultByCmd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlResultByCmd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlResultByCmd.Controls.Add(this.grdCmdList);
            this.pnlResultByCmd.Location = new System.Drawing.Point(6, 20);
            this.pnlResultByCmd.Name = "pnlResultByCmd";
            this.pnlResultByCmd.Size = new System.Drawing.Size(312, 296);
            this.pnlResultByCmd.TabIndex = 184;
            // 
            // grdCmdList
            // 
            this.grdCmdList.BackColor = System.Drawing.SystemColors.Window;
            this.grdCmdList.ColumnInfo = resources.GetString("grdCmdList.ColumnInfo");
            this.grdCmdList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdCmdList.ForeColor = System.Drawing.SystemColors.WindowText;
            this.grdCmdList.Location = new System.Drawing.Point(0, 0);
            this.grdCmdList.Name = "grdCmdList";
            this.grdCmdList.Rows.Count = 1;
            this.grdCmdList.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.ListBox;
            this.grdCmdList.Size = new System.Drawing.Size(310, 294);
            this.grdCmdList.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("grdCmdList.Styles"));
            this.grdCmdList.TabIndex = 184;
            this.grdCmdList.CellChanged += new C1.Win.C1FlexGrid.RowColEventHandler(this.OnSelChangeGrid2);
            // 
            // btnSaveResult
            // 
            this.btnSaveResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveResult.BackgroundImageDisable = null;
            this.btnSaveResult.BackgroundImageHover = null;
            this.btnSaveResult.BackgroundImageNormal = null;
            this.btnSaveResult.BackgroundImageSelect = null;
            this.btnSaveResult.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnSaveResult.BorderEdgeRadius = 3;
            this.btnSaveResult.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnSaveResult.ButtonImageCenter = null;
            this.btnSaveResult.ButtonImageLeft = null;
            this.btnSaveResult.ButtonImageRight = null;
            this.btnSaveResult.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnSaveResult, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnSaveResult.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnSaveResult.ColorDepthFocus = 2;
            this.btnSaveResult.ColorDepthHover = 2;
            this.btnSaveResult.ColorDepthShadow = 2;
            this.btnSaveResult.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnSaveResult.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnSaveResult.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnSaveResult.IconIndexDisable = -1;
            this.btnSaveResult.IconIndexHover = -1;
            this.btnSaveResult.IconIndexNormal = -1;
            this.btnSaveResult.IconIndexSelect = -1;
            this.btnSaveResult.Image = null;
            this.btnSaveResult.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnSaveResult.ImageIndent = 0;
            this.btnSaveResult.ImageList = null;
            this.btnSaveResult.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnSaveResult.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnSaveResult.Location = new System.Drawing.Point(172, 322);
            this.btnSaveResult.Name = "btnSaveResult";
            this.btnSaveResult.Size = new System.Drawing.Size(146, 22);
            this.btnSaveResult.TabIndex = 182;
            this.btnSaveResult.Text = "선택 결과 저장";
            this.btnSaveResult.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnSaveResult.TextIndent = 0;
            this.btnSaveResult.Click += new System.EventHandler(this.btnSaveResult_Click);
            // 
            // rdoResultAll
            // 
            this.rdoResultAll.AutoSize = true;
            this.rdoResultAll.Checked = true;
            this.rdoResultAll.Location = new System.Drawing.Point(22, 13);
            this.rdoResultAll.Name = "rdoResultAll";
            this.rdoResultAll.Size = new System.Drawing.Size(75, 16);
            this.rdoResultAll.TabIndex = 178;
            this.rdoResultAll.TabStop = true;
            this.rdoResultAll.Text = "전체 결과";
            this.rdoResultAll.UseVisualStyleBackColor = true;
            this.rdoResultAll.CheckedChanged += new System.EventHandler(this.OnClick_checkAll);
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.splitter1.Location = new System.Drawing.Point(344, 52);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 395);
            this.splitter1.TabIndex = 184;
            this.splitter1.TabStop = false;
            // 
            // pnlBody
            // 
            this.pnlBody.Controls.Add(this.pnlResult);
            this.pnlBody.Controls.Add(this.lblResult);
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Location = new System.Drawing.Point(347, 52);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Size = new System.Drawing.Size(435, 395);
            this.pnlBody.TabIndex = 185;
            // 
            // pnlResult
            // 
            this.pnlResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlResult.Controls.Add(this.rtxtResult);
            this.pnlResult.Location = new System.Drawing.Point(17, 35);
            this.pnlResult.Name = "pnlResult";
            this.pnlResult.Size = new System.Drawing.Size(403, 354);
            this.pnlResult.TabIndex = 184;
            // 
            // rtxtResult
            // 
            this.rtxtResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxtResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtxtResult.Location = new System.Drawing.Point(0, 0);
            this.rtxtResult.Name = "rtxtResult";
            this.rtxtResult.Size = new System.Drawing.Size(401, 352);
            this.rtxtResult.TabIndex = 178;
            this.rtxtResult.Text = "";
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(15, 13);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(85, 12);
            this.lblResult.TabIndex = 176;
            this.lblResult.Text = "명령 실행 결과";
            // 
            // CmdExecResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 491);
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.Name = "CmdExecResultForm";
            this.Text = "명령 실행 결과 조회";
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.pnlLeft.ResumeLayout(false);
            this.pnlLeft.PerformLayout();
            this.gBoxResultByCmd.ResumeLayout(false);
            this.pnlResultByCmd.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdCmdList)).EndInit();
            this.pnlBody.ResumeLayout(false);
            this.pnlBody.PerformLayout();
            this.pnlResult.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private MKLibrary.Controls.MKButton btnFileOpen;
        private System.Windows.Forms.TextBox txtFileOpen;
        private System.Windows.Forms.Label lblFileOpen;
        private System.Windows.Forms.Panel pnlBottom;
        private MKLibrary.Controls.MKButton btnClose;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.RadioButton rdoResultByCmd;
        private System.Windows.Forms.GroupBox gBoxResultByCmd;
        private System.Windows.Forms.Panel pnlResultByCmd;
        private C1.Win.C1FlexGrid.C1FlexGrid grdCmdList;
        private MKLibrary.Controls.MKButton btnSaveResult;
        private System.Windows.Forms.RadioButton rdoResultAll;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.Panel pnlResult;
        private System.Windows.Forms.RichTextBox rtxtResult;
        private System.Windows.Forms.Label lblResult;

    }
}