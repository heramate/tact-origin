namespace RACTClient
{
    partial class ViewReservedString
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewReservedString));
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.contextMenuBar1 = new DevComponents.DotNetBar.ContextMenuBar();
            this.ctmPopup = new DevComponents.DotNetBar.ButtonItem();
            this.mnuCopy = new DevComponents.DotNetBar.ButtonItem();
            this.fgReservedString = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fgReservedString)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(304, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "Telnet 명령 작성시 사용할 수 있는 예약어 정보 입니다.";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.contextMenuBar1);
            this.panel1.Controls.Add(this.fgReservedString);
            this.panel1.Location = new System.Drawing.Point(15, 37);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(563, 454);
            this.panel1.TabIndex = 2;
            // 
            // contextMenuBar1
            // 
            this.contextMenuBar1.AntiAlias = true;
            this.contextMenuBar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.ctmPopup});
            this.contextMenuBar1.Location = new System.Drawing.Point(158, 233);
            this.contextMenuBar1.Name = "contextMenuBar1";
            this.contextMenuBar1.Size = new System.Drawing.Size(75, 25);
            this.contextMenuBar1.Stretch = true;
            this.contextMenuBar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.contextMenuBar1.TabIndex = 1;
            this.contextMenuBar1.TabStop = false;
            this.contextMenuBar1.Text = "contextMenuBar1";
            // 
            // ctmPopup
            // 
            this.ctmPopup.AutoExpandOnClick = true;
            this.ctmPopup.Name = "ctmPopup";
            this.ctmPopup.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.mnuCopy});
            this.ctmPopup.Text = "buttonItem1";
            // 
            // mnuCopy
            // 
            this.mnuCopy.Name = "mnuCopy";
            this.mnuCopy.Text = "복사";
            this.mnuCopy.Click += new System.EventHandler(this.mnuCopy_Click);
            // 
            // fgReservedString
            // 
            this.fgReservedString.BackColor = System.Drawing.SystemColors.Window;
            this.fgReservedString.ColumnInfo = resources.GetString("fgReservedString.ColumnInfo");
            this.fgReservedString.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fgReservedString.ForeColor = System.Drawing.SystemColors.WindowText;
            this.fgReservedString.Location = new System.Drawing.Point(0, 0);
            this.fgReservedString.Name = "fgReservedString";
            this.fgReservedString.Rows.Count = 1;
            this.fgReservedString.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
            this.fgReservedString.Size = new System.Drawing.Size(563, 454);
            this.fgReservedString.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("fgReservedString.Styles"));
            this.fgReservedString.TabIndex = 0;
            this.fgReservedString.MouseDown += new System.Windows.Forms.MouseEventHandler(this.fgReservedString_MouseDown);
            this.fgReservedString.KeyDown += new System.Windows.Forms.KeyEventHandler(this.fgReservedString_KeyDown);
            // 
            // ViewReservedString
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 545);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Name = "ViewReservedString";
            this.Text = "예약어 선택";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fgReservedString)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private C1.Win.C1FlexGrid.C1FlexGrid fgReservedString;
        private DevComponents.DotNetBar.ContextMenuBar contextMenuBar1;
        private DevComponents.DotNetBar.ButtonItem ctmPopup;
        private DevComponents.DotNetBar.ButtonItem mnuCopy;
    }
}