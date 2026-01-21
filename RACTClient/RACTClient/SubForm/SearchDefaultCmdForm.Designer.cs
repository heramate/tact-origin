namespace RACTClient
{
    partial class SearchDefaultCmdForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchDefaultCmdForm));
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnSearch = new MKLibrary.Controls.MKButton(this.components);
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnClose = new MKLibrary.Controls.MKButton(this.components);
            this.pnlBody = new System.Windows.Forms.Panel();
            this.pnlCmdGrid = new System.Windows.Forms.Panel();
            this.grdCmdList = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnlTop.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.pnlCmdGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdCmdList)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.btnSearch);
            this.pnlTop.Controls.Add(this.txtSearch);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(547, 40);
            this.pnlTop.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackgroundImageDisable = null;
            this.btnSearch.BackgroundImageHover = null;
            this.btnSearch.BackgroundImageNormal = null;
            this.btnSearch.BackgroundImageSelect = null;
            this.btnSearch.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnSearch.BorderEdgeRadius = 3;
            this.btnSearch.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnSearch.ButtonImageCenter = null;
            this.btnSearch.ButtonImageLeft = null;
            this.btnSearch.ButtonImageRight = null;
            this.btnSearch.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnSearch, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnSearch.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnSearch.ColorDepthFocus = 2;
            this.btnSearch.ColorDepthHover = 2;
            this.btnSearch.ColorDepthShadow = 2;
            this.btnSearch.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnSearch.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnSearch.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnSearch.IconIndexDisable = -1;
            this.btnSearch.IconIndexHover = -1;
            this.btnSearch.IconIndexNormal = -1;
            this.btnSearch.IconIndexSelect = -1;
            this.btnSearch.Image = null;
            this.btnSearch.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnSearch.ImageIndent = 0;
            this.btnSearch.ImageList = null;
            this.btnSearch.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnSearch.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnSearch.Location = new System.Drawing.Point(469, 9);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(66, 22);
            this.btnSearch.TabIndex = 179;
            this.btnSearch.Text = "검색";
            this.btnSearch.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnSearch.TextIndent = 0;
            this.btnSearch.Click += new System.EventHandler(this.OnClickSearchDefaultCmd);
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.BackColor = System.Drawing.Color.White;
            this.txtSearch.Location = new System.Drawing.Point(16, 10);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(447, 21);
            this.txtSearch.TabIndex = 178;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnClose);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 355);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(547, 33);
            this.pnlBottom.TabIndex = 1;
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
            this.btnClose.Location = new System.Drawing.Point(469, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(66, 22);
            this.btnClose.TabIndex = 180;
            this.btnClose.Text = "닫기";
            this.btnClose.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnClose.TextIndent = 0;
            this.btnClose.Click += new System.EventHandler(this.ExitForm);
            // 
            // pnlBody
            // 
            this.pnlBody.Controls.Add(this.pnlCmdGrid);
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Location = new System.Drawing.Point(0, 40);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Size = new System.Drawing.Size(547, 315);
            this.pnlBody.TabIndex = 2;
            // 
            // pnlCmdGrid
            // 
            this.pnlCmdGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlCmdGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCmdGrid.Controls.Add(this.grdCmdList);
            this.pnlCmdGrid.Location = new System.Drawing.Point(16, 8);
            this.pnlCmdGrid.Name = "pnlCmdGrid";
            this.pnlCmdGrid.Size = new System.Drawing.Size(518, 297);
            this.pnlCmdGrid.TabIndex = 0;
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
            this.grdCmdList.Size = new System.Drawing.Size(516, 295);
            this.grdCmdList.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("grdCmdList.Styles"));
            this.grdCmdList.TabIndex = 185;
            this.grdCmdList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OnSelectItemByMouse);
            this.grdCmdList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MouseMove);
            // 
            // toolTip1
            // 
            this.toolTip1.BackColor = System.Drawing.Color.BlanchedAlmond;
            this.toolTip1.ShowAlways = true;
            // 
            // SearchDefaultCmdForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(547, 388);
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.KeyPreview = true;
            this.Name = "SearchDefaultCmdForm";
            this.Text = "기본 명령 조회";
            this.Load += new System.EventHandler(this.OnLoadForm);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.pnlBody.ResumeLayout(false);
            this.pnlCmdGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdCmdList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Panel pnlBody;
        private MKLibrary.Controls.MKButton btnSearch;
        private MKLibrary.Controls.MKButton btnClose;
        private System.Windows.Forms.Panel pnlCmdGrid;
        private C1.Win.C1FlexGrid.C1FlexGrid grdCmdList;
        public System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}