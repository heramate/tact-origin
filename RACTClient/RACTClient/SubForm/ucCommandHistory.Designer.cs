namespace RACTClient
{
    partial class ucCommandHistory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucCommandHistory));
            this.panel1 = new System.Windows.Forms.Panel();
            this.fgCommand = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fgCommand)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.fgCommand);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(432, 150);
            this.panel1.TabIndex = 0;
            // 
            // fgCommand
            // 
            this.fgCommand.BackColor = System.Drawing.SystemColors.Window;
            this.fgCommand.ColumnInfo = resources.GetString("fgCommand.ColumnInfo");
            this.fgCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fgCommand.ForeColor = System.Drawing.SystemColors.WindowText;
            this.fgCommand.Location = new System.Drawing.Point(0, 0);
            this.fgCommand.Name = "fgCommand";
            this.fgCommand.Rows.Count = 1;
            this.fgCommand.Size = new System.Drawing.Size(432, 150);
            this.fgCommand.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("fgCommand.Styles"));
            this.fgCommand.TabIndex = 0;
            // 
            // ucCommandHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "ucCommandHistory";
            this.Size = new System.Drawing.Size(432, 150);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fgCommand)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private C1.Win.C1FlexGrid.C1FlexGrid fgCommand;
    }
}
