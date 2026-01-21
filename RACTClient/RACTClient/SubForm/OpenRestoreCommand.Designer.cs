namespace RACTClient
{
    partial class OpenRestoreCommand
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenRestoreCommand));
            this.fgCommandList = new C1.Win.C1FlexGrid.C1FlexGrid();
            ((System.ComponentModel.ISupportInitialize)(this.fgCommandList)).BeginInit();
            this.SuspendLayout();
            // 
            // fgCommandList
            // 
            this.fgCommandList.BackColor = System.Drawing.SystemColors.Window;
            this.fgCommandList.ColumnInfo = resources.GetString("fgCommandList.ColumnInfo");
            this.fgCommandList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fgCommandList.ForeColor = System.Drawing.SystemColors.WindowText;
            this.fgCommandList.Location = new System.Drawing.Point(0, 0);
            this.fgCommandList.Name = "fgCommandList";
            this.fgCommandList.Rows.Count = 1;
            this.fgCommandList.Size = new System.Drawing.Size(558, 336);
            this.fgCommandList.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("fgCommandList.Styles"));
            this.fgCommandList.TabIndex = 2;
            this.fgCommandList.CellButtonClick += new C1.Win.C1FlexGrid.RowColEventHandler(this.fgCommandList_CellButtonClick);
            // 
            // OpenRestoreCommand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 375);
            this.Controls.Add(this.fgCommandList);
            this.DoubleBuffered = true;
            this.Name = "OpenRestoreCommand";
            this.Text = "최근Config바이너리파일목록";
            this.Controls.SetChildIndex(this.fgCommandList, 0);
            ((System.ComponentModel.ISupportInitialize)(this.fgCommandList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private C1.Win.C1FlexGrid.C1FlexGrid fgCommandList;
    }
}