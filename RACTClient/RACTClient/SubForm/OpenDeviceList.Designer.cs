namespace RACTClient
{
    partial class OpenDeviceList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpenDeviceList));
            this.fgDeviceList = new C1.Win.C1FlexGrid.C1FlexGrid();
            ((System.ComponentModel.ISupportInitialize)(this.fgDeviceList)).BeginInit();
            this.SuspendLayout();
            // 
            // fgDeviceList
            // 
            this.fgDeviceList.BackColor = System.Drawing.SystemColors.Window;
            this.fgDeviceList.ColumnInfo = resources.GetString("fgDeviceList.ColumnInfo");
            this.fgDeviceList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fgDeviceList.ForeColor = System.Drawing.SystemColors.WindowText;
            this.fgDeviceList.Location = new System.Drawing.Point(0, 0);
            this.fgDeviceList.Name = "fgDeviceList";
            this.fgDeviceList.Rows.Count = 1;
            this.fgDeviceList.Size = new System.Drawing.Size(908, 504);
            this.fgDeviceList.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("fgDeviceList.Styles"));
            this.fgDeviceList.TabIndex = 1;
            // 
            // OpenDeviceList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(908, 543);
            this.Controls.Add(this.fgDeviceList);
            this.DoubleBuffered = true;
            this.Name = "OpenDeviceList";
            this.Text = "목록열기";
            this.Controls.SetChildIndex(this.fgDeviceList, 0);
            ((System.ComponentModel.ISupportInitialize)(this.fgDeviceList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private C1.Win.C1FlexGrid.C1FlexGrid fgDeviceList;

    }
}