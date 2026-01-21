namespace RACTClient
{
    partial class SearchIPList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchIPList));
            this.fgCommand = new C1.Win.C1FlexGrid.C1FlexGrid();
            ((System.ComponentModel.ISupportInitialize)(this.fgCommand)).BeginInit();
            this.SuspendLayout();
            // 
            // fgCommand
            // 
            this.fgCommand.BackColor = System.Drawing.SystemColors.Window;
            this.fgCommand.ColumnInfo = "1,0,0,0,0,75,Columns:0{Width:238;Name:\"IP\";Caption:\"IP\";TextAlign:CenterCenter;Te" +
                "xtAlignFixed:CenterCenter;}\t";
            this.fgCommand.ForeColor = System.Drawing.SystemColors.WindowText;
            this.fgCommand.Location = new System.Drawing.Point(2, 2);
            this.fgCommand.Name = "fgCommand";
            this.fgCommand.Rows.Count = 1;
            this.fgCommand.Rows.MaxSize = 800;
            this.fgCommand.Size = new System.Drawing.Size(256, 409);
            this.fgCommand.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("fgCommand.Styles"));
            this.fgCommand.TabIndex = 3;
            this.fgCommand.KeyDown += new System.Windows.Forms.KeyEventHandler(this.fgCommand_KeyDown);
            // 
            // SearchIPList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(257, 456);
            this.Controls.Add(this.fgCommand);
            this.DoubleBuffered = true;
            this.Name = "SearchIPList";
            this.Text = "아이피 일괄 검색";
            this.Controls.SetChildIndex(this.fgCommand, 0);
            ((System.ComponentModel.ISupportInitialize)(this.fgCommand)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private C1.Win.C1FlexGrid.C1FlexGrid fgCommand;
    }
}