namespace RACTClient
{
    partial class ucShortenCommand
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
            this.pnlEXShortenCommand = new DevComponents.DotNetBar.PanelEx();
            this.btnSetting = new MKLibrary.Controls.MKButton(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.itemPanel1 = new DevComponents.DotNetBar.ItemPanel();
            this.pnlEXShortenCommand.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlEXShortenCommand
            // 
            this.pnlEXShortenCommand.CanvasColor = System.Drawing.SystemColors.Control;
            this.pnlEXShortenCommand.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.pnlEXShortenCommand.Controls.Add(this.btnSetting);
            this.pnlEXShortenCommand.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEXShortenCommand.Location = new System.Drawing.Point(0, 0);
            this.pnlEXShortenCommand.Name = "pnlEXShortenCommand";
            this.pnlEXShortenCommand.Size = new System.Drawing.Size(199, 21);
            this.pnlEXShortenCommand.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.pnlEXShortenCommand.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.pnlEXShortenCommand.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.pnlEXShortenCommand.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.pnlEXShortenCommand.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.pnlEXShortenCommand.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.pnlEXShortenCommand.Style.GradientAngle = 90;
            this.pnlEXShortenCommand.TabIndex = 3;
            this.pnlEXShortenCommand.Text = "단축 명령";
            // 
            // btnSetting
            // 
            this.btnSetting.BackgroundImageDisable = null;
            this.btnSetting.BackgroundImageHover = null;
            this.btnSetting.BackgroundImageNormal = null;
            this.btnSetting.BackgroundImageSelect = null;
            this.btnSetting.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnSetting.BorderEdgeRadius = 3;
            this.btnSetting.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnSetting.ButtonImageCenter = null;
            this.btnSetting.ButtonImageLeft = null;
            this.btnSetting.ButtonImageRight = null;
            this.btnSetting.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnSetting, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnSetting.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnSetting.ColorDepthFocus = 2;
            this.btnSetting.ColorDepthHover = 2;
            this.btnSetting.ColorDepthShadow = 2;
            this.btnSetting.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnSetting.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnSetting.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.btnSetting.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnSetting.IconIndexDisable = -1;
            this.btnSetting.IconIndexHover = -1;
            this.btnSetting.IconIndexNormal = -1;
            this.btnSetting.IconIndexSelect = -1;
            this.btnSetting.Image = null;
            this.btnSetting.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnSetting.ImageIndent = 0;
            this.btnSetting.ImageList = null;
            this.btnSetting.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnSetting.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnSetting.Location = new System.Drawing.Point(160, 1);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(37, 18);
            this.btnSetting.TabIndex = 1;
            this.btnSetting.Text = "설정";
            this.btnSetting.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnSetting.TextIndent = 0;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.itemPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 21);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(199, 192);
            this.panel1.TabIndex = 4;
            // 
            // itemPanel1
            // 
            // 
            // 
            // 
            this.itemPanel1.BackgroundStyle.Class = "ItemPanel";
            this.itemPanel1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.itemPanel1.ContainerControlProcessDialogKey = true;
            this.itemPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itemPanel1.LayoutOrientation = DevComponents.DotNetBar.eOrientation.Vertical;
            this.itemPanel1.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F";
            this.itemPanel1.Location = new System.Drawing.Point(0, 0);
            this.itemPanel1.Name = "itemPanel1";
            this.itemPanel1.Size = new System.Drawing.Size(199, 192);
            this.itemPanel1.TabIndex = 0;
            this.itemPanel1.Text = "itemPanel1";
            // 
            // ucShortenCommand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlEXShortenCommand);
            this.Name = "ucShortenCommand";
            this.Size = new System.Drawing.Size(199, 213);
            this.pnlEXShortenCommand.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx pnlEXShortenCommand;
        private MKLibrary.Controls.MKButton btnSetting;
        private System.Windows.Forms.Panel panel1;
        private DevComponents.DotNetBar.ItemPanel itemPanel1;
    }
}
