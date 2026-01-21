namespace RACTClient
{
    partial class ConnectionHistorySearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectionHistorySearch));
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpStartTime = new System.Windows.Forms.DateTimePicker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.fgSearchList = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.dtpEndTime = new System.Windows.Forms.DateTimePicker();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.btnSearch = new MKLibrary.Controls.MKButton(this.components);
            this.contextMenuBar1 = new DevComponents.DotNetBar.ContextMenuBar();
            this.cmPopup = new DevComponents.DotNetBar.ButtonItem();
            this.mnuCommandHistory = new DevComponents.DotNetBar.ButtonItem();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fgSearchList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.CustomFormat = "yyyy-MM-dd";
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartDate.Location = new System.Drawing.Point(12, 30);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(103, 21);
            this.dtpStartDate.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "시작 날짜";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(227, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "종료 날짜";
            // 
            // dtpStartTime
            // 
            this.dtpStartTime.CustomFormat = "HH:mm:ss";
            this.dtpStartTime.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.dtpStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartTime.Location = new System.Drawing.Point(121, 30);
            this.dtpStartTime.Name = "dtpStartTime";
            this.dtpStartTime.ShowUpDown = true;
            this.dtpStartTime.Size = new System.Drawing.Size(103, 21);
            this.dtpStartTime.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.fgSearchList);
            this.panel1.Location = new System.Drawing.Point(12, 57);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(873, 483);
            this.panel1.TabIndex = 3;
            // 
            // fgSearchList
            // 
            this.fgSearchList.AllowEditing = false;
            this.fgSearchList.BackColor = System.Drawing.SystemColors.Window;
            this.fgSearchList.ColumnInfo = resources.GetString("fgSearchList.ColumnInfo");
            this.fgSearchList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fgSearchList.ForeColor = System.Drawing.SystemColors.WindowText;
            this.fgSearchList.Location = new System.Drawing.Point(0, 0);
            this.fgSearchList.Name = "fgSearchList";
            this.fgSearchList.Rows.Count = 1;
            this.fgSearchList.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
            this.fgSearchList.Size = new System.Drawing.Size(873, 483);
            this.fgSearchList.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("fgSearchList.Styles"));
            this.fgSearchList.TabIndex = 0;
            this.fgSearchList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.fgSearchList_MouseDown);
            // 
            // dtpEndTime
            // 
            this.dtpEndTime.CustomFormat = "HH:mm:ss";
            this.dtpEndTime.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.dtpEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndTime.Location = new System.Drawing.Point(339, 30);
            this.dtpEndTime.Name = "dtpEndTime";
            this.dtpEndTime.ShowUpDown = true;
            this.dtpEndTime.Size = new System.Drawing.Size(103, 21);
            this.dtpEndTime.TabIndex = 1;
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.CustomFormat = "yyyy-MM-dd";
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndDate.Location = new System.Drawing.Point(230, 30);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(103, 21);
            this.dtpEndDate.TabIndex = 1;
            // 
            // btnSearch
            // 
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
            this.btnSearch.Location = new System.Drawing.Point(448, 31);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(69, 20);
            this.btnSearch.TabIndex = 173;
            this.btnSearch.Text = "검색";
            this.btnSearch.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnSearch.TextIndent = 0;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // contextMenuBar1
            // 
            this.contextMenuBar1.AntiAlias = true;
            this.contextMenuBar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.cmPopup});
            this.contextMenuBar1.Location = new System.Drawing.Point(7, 344);
            this.contextMenuBar1.Name = "contextMenuBar1";
            this.contextMenuBar1.Size = new System.Drawing.Size(75, 25);
            this.contextMenuBar1.Stretch = true;
            this.contextMenuBar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.contextMenuBar1.TabIndex = 174;
            this.contextMenuBar1.TabStop = false;
            this.contextMenuBar1.Text = "contextMenuBar1";
            // 
            // cmPopup
            // 
            this.cmPopup.AutoExpandOnClick = true;
            this.cmPopup.Name = "cmPopup";
            this.cmPopup.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.mnuCommandHistory});
            this.cmPopup.Text = "buttonItem1";
            // 
            // mnuCommandHistory
            // 
            this.mnuCommandHistory.Name = "mnuCommandHistory";
            this.mnuCommandHistory.Text = "명령어 기록 보기";
            this.mnuCommandHistory.Click += new System.EventHandler(this.mnuCommandHistory_Click);
            // 
            // ConnectionHistorySearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 585);
            this.Controls.Add(this.contextMenuBar1);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.dtpStartDate);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dtpEndDate);
            this.Controls.Add(this.dtpStartTime);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtpEndTime);
            this.Controls.Add(this.label2);
            this.Name = "ConnectionHistorySearch";
            this.Text = "접속 기록 검색";
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.dtpEndTime, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.dtpStartTime, 0);
            this.Controls.SetChildIndex(this.dtpEndDate, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.dtpStartDate, 0);
            this.Controls.SetChildIndex(this.btnSearch, 0);
            this.Controls.SetChildIndex(this.contextMenuBar1, 0);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fgSearchList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpStartTime;
        private System.Windows.Forms.Panel panel1;
        private C1.Win.C1FlexGrid.C1FlexGrid fgSearchList;
        private System.Windows.Forms.DateTimePicker dtpEndTime;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private MKLibrary.Controls.MKButton btnSearch;
        private DevComponents.DotNetBar.ContextMenuBar contextMenuBar1;
        private DevComponents.DotNetBar.ButtonItem cmPopup;
        private DevComponents.DotNetBar.ButtonItem mnuCommandHistory;
    }
}