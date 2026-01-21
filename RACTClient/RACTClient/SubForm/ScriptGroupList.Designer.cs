namespace RACTClient
{
    partial class ScriptGroupList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScriptGroupList));
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.contextMenuBar1 = new DevComponents.DotNetBar.ContextMenuBar();
            this.ctmPopup = new DevComponents.DotNetBar.ButtonItem();
            this.mnuAdd = new DevComponents.DotNetBar.ButtonItem();
            this.mnuModify = new DevComponents.DotNetBar.ButtonItem();
            this.mnuDelete = new DevComponents.DotNetBar.ButtonItem();
            this.fgCommandGroup = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fgCommandGroup)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "스크립트 그룹 리스트";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.contextMenuBar1);
            this.panel1.Controls.Add(this.fgCommandGroup);
            this.panel1.Location = new System.Drawing.Point(15, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(551, 298);
            this.panel1.TabIndex = 2;
            // 
            // contextMenuBar1
            // 
            this.contextMenuBar1.AntiAlias = true;
            this.contextMenuBar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.ctmPopup});
            this.contextMenuBar1.Location = new System.Drawing.Point(212, 111);
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
            this.mnuAdd,
            this.mnuModify,
            this.mnuDelete});
            this.ctmPopup.Text = "buttonItem1";
            // 
            // mnuAdd
            // 
            this.mnuAdd.Name = "mnuAdd";
            this.mnuAdd.Text = "추가";
            // 
            // mnuModify
            // 
            this.mnuModify.Name = "mnuModify";
            this.mnuModify.Text = "수정";
            // 
            // mnuDelete
            // 
            this.mnuDelete.Name = "mnuDelete";
            this.mnuDelete.Text = "삭제";
            // 
            // fgCommandGroup
            // 
            this.fgCommandGroup.AllowEditing = false;
            this.fgCommandGroup.BackColor = System.Drawing.SystemColors.Window;
            this.fgCommandGroup.ColumnInfo = resources.GetString("fgCommandGroup.ColumnInfo");
            this.fgCommandGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fgCommandGroup.ForeColor = System.Drawing.SystemColors.WindowText;
            this.fgCommandGroup.Location = new System.Drawing.Point(0, 0);
            this.fgCommandGroup.Name = "fgCommandGroup";
            this.fgCommandGroup.Rows.Count = 1;
            this.fgCommandGroup.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
            this.fgCommandGroup.Size = new System.Drawing.Size(551, 298);
            this.fgCommandGroup.Styles = ((C1.Win.C1FlexGrid.CellStyleCollection)(new C1.Win.C1FlexGrid.CellStyleCollection("")));
            this.fgCommandGroup.TabIndex = 0;
            // 
            // ScriptGroupList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 375);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Name = "ScriptGroupList";
            this.Text = "스크립트 그룹";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fgCommandGroup)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private C1.Win.C1FlexGrid.C1FlexGrid fgCommandGroup;
        private DevComponents.DotNetBar.ContextMenuBar contextMenuBar1;
        private DevComponents.DotNetBar.ButtonItem ctmPopup;
        private DevComponents.DotNetBar.ButtonItem mnuAdd;
        private DevComponents.DotNetBar.ButtonItem mnuModify;
        private DevComponents.DotNetBar.ButtonItem mnuDelete;
    }
}