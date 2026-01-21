namespace RACTClient
{
    partial class ModifyScript
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboGroup = new MKLibrary.Controls.MKComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAddGroup = new MKLibrary.Controls.MKButton(this.components);
            this.txtScript = new System.Windows.Forms.RichTextBox();
            this.bar1 = new DevComponents.DotNetBar.Bar();
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            this.mnuNew = new DevComponents.DotNetBar.ButtonItem();
            this.mnuOpen = new DevComponents.DotNetBar.ButtonItem();
            this.mnuSaveAs = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem2 = new DevComponents.DotNetBar.ButtonItem();
            this.mnuUnDo = new DevComponents.DotNetBar.ButtonItem();
            this.mnuCut = new DevComponents.DotNetBar.ButtonItem();
            this.mnuCopy = new DevComponents.DotNetBar.ButtonItem();
            this.mnuPaste = new DevComponents.DotNetBar.ButtonItem();
            this.mnuDebug = new DevComponents.DotNetBar.ButtonItem();
            this.mnuRun = new DevComponents.DotNetBar.ButtonItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnEasyScript = new MKLibrary.Controls.MKButton(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "화면 표시 명칭(Label)";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(18, 41);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(179, 21);
            this.txtName.TabIndex = 1;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(18, 85);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(383, 21);
            this.txtDescription.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "설명";
            // 
            // cboGroup
            // 
            this.cboGroup.BackColor = System.Drawing.SystemColors.Window;
            this.cboGroup.BackColorSelected = System.Drawing.Color.Orange;
            this.cboGroup.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(201)))), ((int)(((byte)(185)))));
            this.cboGroup.BorderEdgeRadius = 3;
            this.cboGroup.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.cboGroup.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.Fixed;
            this.cboGroup.BoxBorderColor = System.Drawing.Color.Gray;
            this.cboGroup.ColorBoxSize = new System.Drawing.Size(12, 8);
            this.cboGroup.ComboBoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGroup.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboGroup.ForeColor = System.Drawing.Color.Black;
            this.cboGroup.ImageList = null;
            this.cboGroup.ItemHeight = 14;
            this.cboGroup.Location = new System.Drawing.Point(203, 41);
            this.cboGroup.MaxDorpDownWidth = 500;
            this.cboGroup.Name = "cboGroup";
            this.cboGroup.SelectedIndex = -1;
            this.cboGroup.ShowColorBox = false;
            this.cboGroup.Size = new System.Drawing.Size(198, 21);
            this.cboGroup.TabIndex = 174;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(201, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 12);
            this.label4.TabIndex = 173;
            this.label4.Text = "스크립트 그룹";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnEasyScript);
            this.groupBox1.Controls.Add(this.btnAddGroup);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtDescription);
            this.groupBox1.Controls.Add(this.cboGroup);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(5, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(773, 116);
            this.groupBox1.TabIndex = 176;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "스크립트 정보";
            // 
            // btnAddGroup
            // 
            this.btnAddGroup.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnAddGroup.BackgroundImageDisable = null;
            this.btnAddGroup.BackgroundImageHover = null;
            this.btnAddGroup.BackgroundImageNormal = null;
            this.btnAddGroup.BackgroundImageSelect = null;
            this.btnAddGroup.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnAddGroup.BorderEdgeRadius = 3;
            this.btnAddGroup.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnAddGroup.ButtonImageCenter = null;
            this.btnAddGroup.ButtonImageLeft = null;
            this.btnAddGroup.ButtonImageRight = null;
            this.btnAddGroup.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnAddGroup, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnAddGroup.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnAddGroup.ColorDepthFocus = 2;
            this.btnAddGroup.ColorDepthHover = 2;
            this.btnAddGroup.ColorDepthShadow = 2;
            this.btnAddGroup.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnAddGroup.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnAddGroup.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnAddGroup.IconIndexDisable = -1;
            this.btnAddGroup.IconIndexHover = -1;
            this.btnAddGroup.IconIndexNormal = -1;
            this.btnAddGroup.IconIndexSelect = -1;
            this.btnAddGroup.Image = null;
            this.btnAddGroup.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnAddGroup.ImageIndent = 0;
            this.btnAddGroup.ImageList = null;
            this.btnAddGroup.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnAddGroup.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnAddGroup.Location = new System.Drawing.Point(407, 41);
            this.btnAddGroup.Name = "btnAddGroup";
            this.btnAddGroup.Size = new System.Drawing.Size(70, 23);
            this.btnAddGroup.TabIndex = 175;
            this.btnAddGroup.Text = "그룹 추가";
            this.btnAddGroup.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnAddGroup.TextIndent = 0;
            this.btnAddGroup.Click += new System.EventHandler(this.btnAddGroup_Click);
            // 
            // txtScript
            // 
            this.txtScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtScript.Location = new System.Drawing.Point(0, 26);
            this.txtScript.Name = "txtScript";
            this.txtScript.Size = new System.Drawing.Size(773, 517);
            this.txtScript.TabIndex = 1;
            this.txtScript.Text = "";
            // 
            // bar1
            // 
            this.bar1.AccessibleDescription = "DotNetBar Bar (barMenu)";
            this.bar1.AccessibleName = "DotNetBar Bar";
            this.bar1.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuBar;
            this.bar1.AntiAlias = true;
            this.bar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.bar1.DockSide = DevComponents.DotNetBar.eDockSide.Document;
            this.bar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItem1,
            this.buttonItem2,
            this.mnuDebug});
            this.bar1.Location = new System.Drawing.Point(0, 0);
            this.bar1.Name = "bar1";
            this.bar1.Size = new System.Drawing.Size(773, 26);
            this.bar1.Stretch = true;
            this.bar1.TabIndex = 0;
            this.bar1.TabStop = false;
            this.bar1.Text = "bar1";
            // 
            // buttonItem1
            // 
            this.buttonItem1.Name = "buttonItem1";
            this.buttonItem1.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.mnuNew,
            this.mnuOpen,
            this.mnuSaveAs});
            this.buttonItem1.Text = "파일(&F)";
            // 
            // mnuNew
            // 
            this.mnuNew.Name = "mnuNew";
            this.mnuNew.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlN);
            this.mnuNew.Text = "새로 만들기(&N)";
            this.mnuNew.Click += new System.EventHandler(this.mnuNew_Click);
            // 
            // mnuOpen
            // 
            this.mnuOpen.Name = "mnuOpen";
            this.mnuOpen.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlO);
            this.mnuOpen.Text = "열기(&O)";
            this.mnuOpen.Click += new System.EventHandler(this.mnuOpen_Click);
            // 
            // mnuSaveAs
            // 
            this.mnuSaveAs.BeginGroup = true;
            this.mnuSaveAs.Name = "mnuSaveAs";
            this.mnuSaveAs.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlS);
            this.mnuSaveAs.Text = "파일로 저장(&A)";
            this.mnuSaveAs.Click += new System.EventHandler(this.mnuSaveAs_Click);
            // 
            // buttonItem2
            // 
            this.buttonItem2.Name = "buttonItem2";
            this.buttonItem2.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.mnuUnDo,
            this.mnuCut,
            this.mnuCopy,
            this.mnuPaste});
            this.buttonItem2.Text = "편집(&E)";
            // 
            // mnuUnDo
            // 
            this.mnuUnDo.Name = "mnuUnDo";
            this.mnuUnDo.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlZ);
            this.mnuUnDo.Text = "실행취소(&U)";
            this.mnuUnDo.Click += new System.EventHandler(this.mnuUnDo_Click);
            // 
            // mnuCut
            // 
            this.mnuCut.BeginGroup = true;
            this.mnuCut.Name = "mnuCut";
            this.mnuCut.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlX);
            this.mnuCut.Text = "잘라내기(&T)";
            this.mnuCut.Click += new System.EventHandler(this.mnuCut_Click);
            // 
            // mnuCopy
            // 
            this.mnuCopy.Name = "mnuCopy";
            this.mnuCopy.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlC);
            this.mnuCopy.Text = "복사(&C)";
            this.mnuCopy.Click += new System.EventHandler(this.mnuCopy_Click);
            // 
            // mnuPaste
            // 
            this.mnuPaste.Name = "mnuPaste";
            this.mnuPaste.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlV);
            this.mnuPaste.Text = "붙여넣기(&P)";
            this.mnuPaste.Click += new System.EventHandler(this.mnuPaste_Click);
            // 
            // mnuDebug
            // 
            this.mnuDebug.Name = "mnuDebug";
            this.mnuDebug.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.mnuRun});
            this.mnuDebug.Text = "컴파일";
            this.mnuDebug.Visible = false;
            // 
            // mnuRun
            // 
            this.mnuRun.Name = "mnuRun";
            this.mnuRun.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.F5);
            this.mnuRun.Text = "컴파일 실행";
            this.mnuRun.Click += new System.EventHandler(this.mnuRun_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtScript);
            this.panel1.Controls.Add(this.bar1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(5, 121);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(773, 543);
            this.panel1.TabIndex = 177;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(5);
            this.panel2.Size = new System.Drawing.Size(783, 669);
            this.panel2.TabIndex = 178;
            // 
            // btnEasyScript
            // 
            this.btnEasyScript.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnEasyScript.BackgroundImageDisable = null;
            this.btnEasyScript.BackgroundImageHover = null;
            this.btnEasyScript.BackgroundImageNormal = null;
            this.btnEasyScript.BackgroundImageSelect = null;
            this.btnEasyScript.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnEasyScript.BorderEdgeRadius = 3;
            this.btnEasyScript.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnEasyScript.ButtonImageCenter = null;
            this.btnEasyScript.ButtonImageLeft = null;
            this.btnEasyScript.ButtonImageRight = null;
            this.btnEasyScript.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnEasyScript, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnEasyScript.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnEasyScript.ColorDepthFocus = 2;
            this.btnEasyScript.ColorDepthHover = 2;
            this.btnEasyScript.ColorDepthShadow = 2;
            this.btnEasyScript.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnEasyScript.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnEasyScript.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnEasyScript.IconIndexDisable = -1;
            this.btnEasyScript.IconIndexHover = -1;
            this.btnEasyScript.IconIndexNormal = -1;
            this.btnEasyScript.IconIndexSelect = -1;
            this.btnEasyScript.Image = null;
            this.btnEasyScript.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnEasyScript.ImageIndent = 0;
            this.btnEasyScript.ImageList = null;
            this.btnEasyScript.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnEasyScript.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnEasyScript.Location = new System.Drawing.Point(407, 83);
            this.btnEasyScript.Name = "btnEasyScript";
            this.btnEasyScript.Size = new System.Drawing.Size(130, 23);
            this.btnEasyScript.TabIndex = 176;
            this.btnEasyScript.Text = "간편 스크립트 생성";
            this.btnEasyScript.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnEasyScript.TextIndent = 0;
            this.btnEasyScript.Click += new System.EventHandler(this.btnEasyScript_Click);
            // 
            // ModifyScript
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(783, 708);
            this.Controls.Add(this.panel2);
            this.DoubleBuffered = true;
            this.Name = "ModifyScript";
            this.Text = "스크립트";
            this.Controls.SetChildIndex(this.panel2, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label3;
        private MKLibrary.Controls.MKComboBox cboGroup;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private DevComponents.DotNetBar.Bar bar1;
        private DevComponents.DotNetBar.ButtonItem buttonItem1;
        private DevComponents.DotNetBar.ButtonItem mnuNew;
        private DevComponents.DotNetBar.ButtonItem mnuOpen;
        private DevComponents.DotNetBar.ButtonItem buttonItem2;
        private DevComponents.DotNetBar.ButtonItem mnuCopy;
        private DevComponents.DotNetBar.ButtonItem mnuPaste;
        private DevComponents.DotNetBar.ButtonItem mnuSaveAs;
        private DevComponents.DotNetBar.ButtonItem mnuUnDo;
        private DevComponents.DotNetBar.ButtonItem mnuCut;
        private DevComponents.DotNetBar.ButtonItem mnuDebug;
        private DevComponents.DotNetBar.ButtonItem mnuRun;
        private System.Windows.Forms.RichTextBox txtScript;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private MKLibrary.Controls.MKButton btnAddGroup;
        private MKLibrary.Controls.MKButton btnEasyScript;
    }
}