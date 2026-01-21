namespace RACTServerServiceManager
{
    partial class ucDaemonDataListPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucDaemonDataListPanel));
            this.grbRunningDaemonList = new System.Windows.Forms.GroupBox();
            this.fgDaemonStatusList = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.grbRunningDaemonList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fgDaemonStatusList)).BeginInit();
            this.SuspendLayout();
            // 
            // grbRunningDaemonList
            // 
            this.grbRunningDaemonList.Controls.Add(this.fgDaemonStatusList);
            this.grbRunningDaemonList.Location = new System.Drawing.Point(15, 15);
            this.grbRunningDaemonList.Name = "grbRunningDaemonList";
            this.grbRunningDaemonList.Padding = new System.Windows.Forms.Padding(10, 15, 10, 10);
            this.grbRunningDaemonList.Size = new System.Drawing.Size(378, 333);
            this.grbRunningDaemonList.TabIndex = 0;
            this.grbRunningDaemonList.TabStop = false;
            this.grbRunningDaemonList.Text = "현재 실행중인 데몬 정보";
            // 
            // fgDaemonStatusList
            // 
            this.fgDaemonStatusList.BackColor = System.Drawing.SystemColors.Window;
            this.fgDaemonStatusList.ColumnInfo = resources.GetString("fgDaemonStatusList.ColumnInfo");
            this.fgDaemonStatusList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fgDaemonStatusList.EditOptions = ((C1.Win.C1FlexGrid.EditFlags)((C1.Win.C1FlexGrid.EditFlags.AutoSearch | C1.Win.C1FlexGrid.EditFlags.MultiCheck)));
            this.fgDaemonStatusList.ForeColor = System.Drawing.SystemColors.WindowText;
            this.fgDaemonStatusList.Location = new System.Drawing.Point(10, 29);
            this.fgDaemonStatusList.Name = "fgDaemonStatusList";
            this.fgDaemonStatusList.Rows.Count = 1;
            this.fgDaemonStatusList.Size = new System.Drawing.Size(358, 294);
            this.fgDaemonStatusList.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("fgDaemonStatusList.Styles"));
            this.fgDaemonStatusList.TabIndex = 0;
            // 
            // ucDaemonDataListPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grbRunningDaemonList);
            this.Name = "ucDaemonDataListPanel";
            this.Size = new System.Drawing.Size(407, 384);
            this.grbRunningDaemonList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fgDaemonStatusList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grbRunningDaemonList;
        private C1.Win.C1FlexGrid.C1FlexGrid fgDaemonStatusList;
    }
}
