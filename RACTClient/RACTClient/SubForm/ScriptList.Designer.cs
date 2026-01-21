namespace RACTClient
{
    partial class ScriptList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScriptList));
            this.panel1 = new System.Windows.Forms.Panel();
            this.contextMenuBar1 = new DevComponents.DotNetBar.ContextMenuBar();
            this.ctmPopup = new DevComponents.DotNetBar.ButtonItem();
            this.mnuAdd = new DevComponents.DotNetBar.ButtonItem();
            this.mnuModify = new DevComponents.DotNetBar.ButtonItem();
            this.mnuDelete = new DevComponents.DotNetBar.ButtonItem();
            this.fgShortenCommand = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.label1 = new System.Windows.Forms.Label();
            this.cboCommandGroup = new MKLibrary.Controls.MKComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnGroup = new MKLibrary.Controls.MKButton(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fgShortenCommand)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.contextMenuBar1);
            this.panel1.Controls.Add(this.fgShortenCommand);
            this.panel1.Location = new System.Drawing.Point(8, 72);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(558, 252);
            this.panel1.TabIndex = 1;
            // 
            // contextMenuBar1
            // 
            this.contextMenuBar1.AntiAlias = true;
            this.contextMenuBar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.ctmPopup});
            this.contextMenuBar1.Location = new System.Drawing.Point(180, 154);
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
            this.mnuAdd.Click += new System.EventHandler(this.mnuAdd_Click);
            // 
            // mnuModify
            // 
            this.mnuModify.Name = "mnuModify";
            this.mnuModify.Text = "수정";
            this.mnuModify.Click += new System.EventHandler(this.mnuModify_Click);
            // 
            // mnuDelete
            // 
            this.mnuDelete.Name = "mnuDelete";
            this.mnuDelete.Text = "삭제";
            this.mnuDelete.Click += new System.EventHandler(this.mnuDelete_Click);
            // 
            // fgShortenCommand
            // 
            this.fgShortenCommand.AllowEditing = false;
            this.fgShortenCommand.BackColor = System.Drawing.SystemColors.Window;
            this.fgShortenCommand.ColumnInfo = resources.GetString("fgShortenCommand.ColumnInfo");
            this.fgShortenCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fgShortenCommand.ForeColor = System.Drawing.SystemColors.WindowText;
            this.fgShortenCommand.Location = new System.Drawing.Point(0, 0);
            this.fgShortenCommand.Name = "fgShortenCommand";
            this.fgShortenCommand.Rows.Count = 1;
            this.fgShortenCommand.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
            this.fgShortenCommand.Size = new System.Drawing.Size(558, 252);
            this.fgShortenCommand.Styles = ((C1.Win.C1FlexGrid.CellStyleCollection)(new C1.Win.C1FlexGrid.CellStyleCollection("")));
            this.fgShortenCommand.TabIndex = 0;
            this.fgShortenCommand.MouseDown += new System.Windows.Forms.MouseEventHandler(this.fgShortenCommand_MouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "스크립트 그룹";
            // 
            // cboCommandGroup
            // 
            this.cboCommandGroup.BackColor = System.Drawing.SystemColors.Window;
            this.cboCommandGroup.BackColorSelected = System.Drawing.Color.Orange;
            this.cboCommandGroup.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(201)))), ((int)(((byte)(185)))));
            this.cboCommandGroup.BorderEdgeRadius = 3;
            this.cboCommandGroup.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.cboCommandGroup.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.Fixed;
            this.cboCommandGroup.BoxBorderColor = System.Drawing.Color.Gray;
            this.cboCommandGroup.ColorBoxSize = new System.Drawing.Size(12, 8);
            this.cboCommandGroup.ComboBoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCommandGroup.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboCommandGroup.ForeColor = System.Drawing.Color.Black;
            this.cboCommandGroup.ImageList = null;
            this.cboCommandGroup.ItemHeight = 14;
            this.cboCommandGroup.Location = new System.Drawing.Point(14, 24);
            this.cboCommandGroup.MaxDorpDownWidth = 500;
            this.cboCommandGroup.Name = "cboCommandGroup";
            this.cboCommandGroup.SelectedIndex = -1;
            this.cboCommandGroup.ShowColorBox = false;
            this.cboCommandGroup.Size = new System.Drawing.Size(198, 21);
            this.cboCommandGroup.TabIndex = 171;
            this.cboCommandGroup.SelectedIndexChanged += new System.EventHandler(this.cboCommandGroup_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "스크립트 리스트";
            // 
            // btnGroup
            // 
            this.btnGroup.BackgroundImageDisable = null;
            this.btnGroup.BackgroundImageHover = null;
            this.btnGroup.BackgroundImageNormal = null;
            this.btnGroup.BackgroundImageSelect = null;
            this.btnGroup.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnGroup.BorderEdgeRadius = 3;
            this.btnGroup.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnGroup.ButtonImageCenter = null;
            this.btnGroup.ButtonImageLeft = null;
            this.btnGroup.ButtonImageRight = null;
            this.btnGroup.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnGroup, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnGroup.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnGroup.ColorDepthFocus = 2;
            this.btnGroup.ColorDepthHover = 2;
            this.btnGroup.ColorDepthShadow = 2;
            this.btnGroup.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnGroup.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnGroup.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnGroup.IconIndexDisable = -1;
            this.btnGroup.IconIndexHover = -1;
            this.btnGroup.IconIndexNormal = -1;
            this.btnGroup.IconIndexSelect = -1;
            this.btnGroup.Image = null;
            this.btnGroup.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnGroup.ImageIndent = 0;
            this.btnGroup.ImageList = null;
            this.btnGroup.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnGroup.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnGroup.Location = new System.Drawing.Point(218, 24);
            this.btnGroup.Name = "btnGroup";
            this.btnGroup.Size = new System.Drawing.Size(69, 20);
            this.btnGroup.TabIndex = 172;
            this.btnGroup.Text = "수정";
            this.btnGroup.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnGroup.TextIndent = 0;
            this.btnGroup.Click += new System.EventHandler(this.btnGroup_Click);
            // 
            // ScriptList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 375);
            this.Controls.Add(this.btnGroup);
            this.Controls.Add(this.cboCommandGroup);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Name = "ScriptList";
            this.Text = "스크립트";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.cboCommandGroup, 0);
            this.Controls.SetChildIndex(this.btnGroup, 0);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fgShortenCommand)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private C1.Win.C1FlexGrid.C1FlexGrid fgShortenCommand;
        private DevComponents.DotNetBar.ContextMenuBar contextMenuBar1;
        private DevComponents.DotNetBar.ButtonItem ctmPopup;
        private DevComponents.DotNetBar.ButtonItem mnuAdd;
        private DevComponents.DotNetBar.ButtonItem mnuModify;
        private DevComponents.DotNetBar.ButtonItem mnuDelete;
        private System.Windows.Forms.Label label1;
        private MKLibrary.Controls.MKComboBox cboCommandGroup;
        private System.Windows.Forms.Label label2;
        private MKLibrary.Controls.MKButton btnGroup;
    }
}