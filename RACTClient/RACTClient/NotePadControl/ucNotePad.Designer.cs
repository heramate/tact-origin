namespace RACTClient
{
    partial class ucNotePad
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
            this.mnuCopy = new DevComponents.DotNetBar.ButtonItem();
            this.mnuCut = new DevComponents.DotNetBar.ButtonItem();
            this.mnuPaste = new DevComponents.DotNetBar.ButtonItem();
            this.mnuUnDo = new DevComponents.DotNetBar.ButtonItem();
            this.cmPopUP = new DevComponents.DotNetBar.ButtonItem();
            this.contextMenuBar1 = new DevComponents.DotNetBar.ContextMenuBar();
            this.bar1 = new DevComponents.DotNetBar.Bar();
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            this.mnuNew = new DevComponents.DotNetBar.ButtonItem();
            this.mnuOpen = new DevComponents.DotNetBar.ButtonItem();
            this.mnuSave = new DevComponents.DotNetBar.ButtonItem();
            this.mnuSaveAs = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem2 = new DevComponents.DotNetBar.ButtonItem();
            this.txtNotePad = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).BeginInit();
            this.SuspendLayout();

            this.buttonItem2.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.mnuCopy,
            this.mnuPaste,
            this.mnuCut,
            this.mnuUnDo                
            });

            // 
            // mnuCopy
            // 
            this.mnuCopy.Name = "mnuCopy";
            this.mnuCopy.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlC);
            this.mnuCopy.Text = "복사(&C)";
            this.mnuCopy.Click += new System.EventHandler(this.mnuCopy_Click);

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
            // mnuPaste
            // 
            this.mnuPaste.Name = "mnuPaste";
            this.mnuPaste.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlV);
            this.mnuPaste.Text = "붙여넣기(&P)";
            this.mnuPaste.Click += new System.EventHandler(this.mnuPaste_Click);
            // 
            // cmPopUP
            // 
            this.cmPopUP.Name = "cmPopUP";
            // 
            // contextMenuBar1
            // 
            this.contextMenuBar1.DockSide = DevComponents.DotNetBar.eDockSide.Top;
            this.contextMenuBar1.Location = new System.Drawing.Point(0, 0);
            this.contextMenuBar1.Name = "contextMenuBar1";
            this.contextMenuBar1.Size = new System.Drawing.Size(0, 0);
            this.contextMenuBar1.TabIndex = 0;
            this.contextMenuBar1.TabStop = false;
            // 
            // bar1
            // 
            this.bar1.AccessibleDescription = "DotNetBar Bar (barMenu)";
            this.bar1.AccessibleName = "DotNetBar Bar";
            this.bar1.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuBar;
            this.bar1.AntiAlias = true;
            this.bar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.bar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItem1,
            this.buttonItem2});
            this.bar1.Location = new System.Drawing.Point(0, 0);
            this.bar1.Name = "bar1";
            this.bar1.Size = new System.Drawing.Size(533, 26);
            this.bar1.Stretch = true;
            this.bar1.TabIndex = 3;
            this.bar1.TabStop = false;
            this.bar1.Text = "bar1";
            // 
            // buttonItem1
            // 
            this.buttonItem1.Name = "buttonItem1";
            this.buttonItem1.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.mnuNew,
            this.mnuOpen,
            this.mnuSave,
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
            // mnuSave
            // 
            this.mnuSave.BeginGroup = true;
            this.mnuSave.Name = "mnuSave";
            this.mnuSave.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlS);
            this.mnuSave.Text = "파일로 저장(&S)";
            this.mnuSave.Click += new System.EventHandler(this.mnuSave_Click);
            // 
            // mnuSaveAs
            // 
            this.mnuSaveAs.Name = "mnuSaveAs";
            this.mnuSaveAs.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlA);
            this.mnuSaveAs.Text = "다른 이름으로 저장(&A)";
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
            // txtNotePad
            // 
            this.txtNotePad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNotePad.Location = new System.Drawing.Point(0, 26);
            this.txtNotePad.Name = "txtNotePad";
            this.txtNotePad.Size = new System.Drawing.Size(533, 466);
            this.txtNotePad.TabIndex = 4;
            this.txtNotePad.Text = "";
            this.txtNotePad.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtNotePad_MouseDown);
            // 
            // ucNotePad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtNotePad);
            this.Controls.Add(this.bar1);
            this.Name = "ucNotePad";
            this.Size = new System.Drawing.Size(533, 492);
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Bar bar1;
        private DevComponents.DotNetBar.ButtonItem buttonItem1;
        private DevComponents.DotNetBar.ButtonItem mnuNew;
        private DevComponents.DotNetBar.ButtonItem mnuOpen;
        private DevComponents.DotNetBar.ButtonItem mnuSave;
        private DevComponents.DotNetBar.ButtonItem buttonItem2;
        private DevComponents.DotNetBar.ButtonItem mnuUnDo;
        private DevComponents.DotNetBar.ButtonItem mnuCut;
        private DevComponents.DotNetBar.ButtonItem mnuCopy;
        private DevComponents.DotNetBar.ButtonItem mnuPaste;
        
        private System.Windows.Forms.RichTextBox txtNotePad;
        private DevComponents.DotNetBar.ButtonItem mnuSaveAs;
    }
}
