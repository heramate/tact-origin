namespace RACTClient
{
    partial class ucCommandLine
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
            this.components = new System.ComponentModel.Container();
            RACTTerminal.Mode mode2 = new RACTTerminal.Mode();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.btnEdit = new MKLibrary.Controls.MKButton(this.components);
            this.switchButton1 = new DevComponents.DotNetBar.Controls.SwitchButton();
            this.contextMenuBar1 = new DevComponents.DotNetBar.ContextMenuBar();
            this.ctmPopup = new DevComponents.DotNetBar.ButtonItem();
            this.mcSmallTerminal1 = new RACTClient.MCSmallTerminal();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAll = new MKLibrary.Controls.MKButton(this.components);
            this.panelEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.btnAll);
            this.panelEx1.Controls.Add(this.label1);
            this.panelEx1.Controls.Add(this.btnEdit);
            this.panelEx1.Controls.Add(this.switchButton1);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(435, 21);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 2;
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.BackgroundImageDisable = null;
            this.btnEdit.BackgroundImageHover = null;
            this.btnEdit.BackgroundImageNormal = null;
            this.btnEdit.BackgroundImageSelect = null;
            this.btnEdit.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnEdit.BorderEdgeRadius = 3;
            this.btnEdit.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnEdit.ButtonImageCenter = null;
            this.btnEdit.ButtonImageLeft = null;
            this.btnEdit.ButtonImageRight = null;
            this.btnEdit.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnEdit, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnEdit.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnEdit.ColorDepthFocus = 2;
            this.btnEdit.ColorDepthHover = 2;
            this.btnEdit.ColorDepthShadow = 2;
            this.btnEdit.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnEdit.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnEdit.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnEdit.IconIndexDisable = -1;
            this.btnEdit.IconIndexHover = -1;
            this.btnEdit.IconIndexNormal = -1;
            this.btnEdit.IconIndexSelect = -1;
            this.btnEdit.Image = null;
            this.btnEdit.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnEdit.ImageIndent = 0;
            this.btnEdit.ImageList = null;
            this.btnEdit.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnEdit.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnEdit.Location = new System.Drawing.Point(328, 2);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(105, 18);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "모든 터미널 전송";
            this.btnEdit.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnEdit.TextIndent = 0;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // switchButton1
            // 
            this.switchButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.switchButton1.BackgroundStyle.Class = "";
            this.switchButton1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.switchButton1.Location = new System.Drawing.Point(179, 2);
            this.switchButton1.Name = "switchButton1";
            this.switchButton1.Size = new System.Drawing.Size(65, 17);
            this.switchButton1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.switchButton1.TabIndex = 0;
            this.switchButton1.ValueChanged += new System.EventHandler(this.switchButton1_ValueChanged);
            // 
            // contextMenuBar1
            // 
            this.contextMenuBar1.AntiAlias = true;
            this.contextMenuBar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.ctmPopup});
            this.contextMenuBar1.Location = new System.Drawing.Point(251, 183);
            this.contextMenuBar1.Name = "contextMenuBar1";
            this.contextMenuBar1.Size = new System.Drawing.Size(75, 27);
            this.contextMenuBar1.Stretch = true;
            this.contextMenuBar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.contextMenuBar1.TabIndex = 0;
            this.contextMenuBar1.TabStop = false;
            this.contextMenuBar1.Text = "contextMenuBar1";
            // 
            // ctmPopup
            // 
            this.ctmPopup.AutoExpandOnClick = true;
            this.ctmPopup.Name = "ctmPopup";
            this.ctmPopup.Text = "buttonItem1";
            // 
            // mcSmallTerminal1
            // 
            this.mcSmallTerminal1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mcSmallTerminal1.Font = new System.Drawing.Font("Courier New", 8F);
            this.mcSmallTerminal1.ForeColor = System.Drawing.Color.GreenYellow;
            this.mcSmallTerminal1.Location = new System.Drawing.Point(0, 21);
            this.mcSmallTerminal1.Modes = mode2;
            this.mcSmallTerminal1.Name = "mcSmallTerminal1";
            this.mcSmallTerminal1.Prompt = ">";
            this.mcSmallTerminal1.Size = new System.Drawing.Size(435, 346);
            this.mcSmallTerminal1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "명령 실행 창";
            // 
            // btnAll
            // 
            this.btnAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAll.BackgroundImageDisable = null;
            this.btnAll.BackgroundImageHover = null;
            this.btnAll.BackgroundImageNormal = null;
            this.btnAll.BackgroundImageSelect = null;
            this.btnAll.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnAll.BorderEdgeRadius = 3;
            this.btnAll.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnAll.ButtonImageCenter = null;
            this.btnAll.ButtonImageLeft = null;
            this.btnAll.ButtonImageRight = null;
            this.btnAll.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnAll, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnAll.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnAll.ColorDepthFocus = 2;
            this.btnAll.ColorDepthHover = 2;
            this.btnAll.ColorDepthShadow = 2;
            this.btnAll.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnAll.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnAll.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnAll.IconIndexDisable = -1;
            this.btnAll.IconIndexHover = -1;
            this.btnAll.IconIndexNormal = -1;
            this.btnAll.IconIndexSelect = -1;
            this.btnAll.Image = null;
            this.btnAll.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnAll.ImageIndent = 0;
            this.btnAll.ImageList = null;
            this.btnAll.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnAll.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnAll.Location = new System.Drawing.Point(251, 2);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(71, 18);
            this.btnAll.TabIndex = 3;
            this.btnAll.Text = "전체 선택";
            this.btnAll.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnAll.TextIndent = 0;
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // ucCommandLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mcSmallTerminal1);
            this.Controls.Add(this.contextMenuBar1);
            this.Controls.Add(this.panelEx1);
            this.Name = "ucCommandLine";
            this.Size = new System.Drawing.Size(435, 367);
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.contextMenuBar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.Controls.SwitchButton switchButton1;
        private DevComponents.DotNetBar.ContextMenuBar contextMenuBar1;
        private MKLibrary.Controls.MKButton btnEdit;
        private DevComponents.DotNetBar.ButtonItem ctmPopup;
        private DevComponents.DotNetBar.ButtonItem buttonItem2;
        private DevComponents.DotNetBar.CheckBoxItem checkBoxItem1;
        private MCSmallTerminal mcSmallTerminal1;
        private System.Windows.Forms.Label label1;
        private MKLibrary.Controls.MKButton btnAll;
    }
}
