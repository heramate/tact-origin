namespace RACTClient
{
    partial class ucMainBatchRegisterBasePanel
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
            this.lblArrow = new System.Windows.Forms.Label();
            this.lblBatchRegister = new System.Windows.Forms.Label();
            this.lblSelectDevice = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btn2 = new MKLibrary.Controls.MKButton(this.components);
            this.btn1 = new MKLibrary.Controls.MKButton(this.components);
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.ucBatchDeviceSelectPanel = new RACTClient.ucBatchDeviceSelectPanel();
            this.ucBatchRegisterPanel = new RACTClient.ucBatchRegisterPanel();
            this.panel3.SuspendLayout();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblArrow
            // 
            this.lblArrow.AutoSize = true;
            this.lblArrow.Location = new System.Drawing.Point(73, 5);
            this.lblArrow.Name = "lblArrow";
            this.lblArrow.Size = new System.Drawing.Size(17, 12);
            this.lblArrow.TabIndex = 1;
            this.lblArrow.Text = "▶";
            // 
            // lblBatchRegister
            // 
            this.lblBatchRegister.AutoSize = true;
            this.lblBatchRegister.Location = new System.Drawing.Point(93, 5);
            this.lblBatchRegister.Name = "lblBatchRegister";
            this.lblBatchRegister.Size = new System.Drawing.Size(77, 12);
            this.lblBatchRegister.TabIndex = 0;
            this.lblBatchRegister.Text = "일괄장비등록";
            // 
            // lblSelectDevice
            // 
            this.lblSelectDevice.AutoSize = true;
            this.lblSelectDevice.Location = new System.Drawing.Point(10, 5);
            this.lblSelectDevice.Name = "lblSelectDevice";
            this.lblSelectDevice.Size = new System.Drawing.Size(53, 12);
            this.lblSelectDevice.TabIndex = 0;
            this.lblSelectDevice.Text = "장비선택";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.panel3.Controls.Add(this.btn2);
            this.panel3.Controls.Add(this.btn1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 555);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(845, 39);
            this.panel3.TabIndex = 7;
            // 
            // btn2
            // 
            this.btn2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn2.BackgroundImageDisable = null;
            this.btn2.BackgroundImageHover = null;
            this.btn2.BackgroundImageNormal = null;
            this.btn2.BackgroundImageSelect = null;
            this.btn2.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btn2.BorderEdgeRadius = 3;
            this.btn2.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btn2.ButtonImageCenter = null;
            this.btn2.ButtonImageLeft = null;
            this.btn2.ButtonImageRight = null;
            this.btn2.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btn2, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btn2.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btn2.ColorDepthFocus = 2;
            this.btn2.ColorDepthHover = 2;
            this.btn2.ColorDepthShadow = 2;
            this.btn2.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btn2.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btn2.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btn2.IconIndexDisable = -1;
            this.btn2.IconIndexHover = -1;
            this.btn2.IconIndexNormal = -1;
            this.btn2.IconIndexSelect = -1;
            this.btn2.Image = null;
            this.btn2.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btn2.ImageIndent = 0;
            this.btn2.ImageList = null;
            this.btn2.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btn2.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btn2.Location = new System.Drawing.Point(754, 8);
            this.btn2.Name = "btn2";
            this.btn2.Size = new System.Drawing.Size(75, 23);
            this.btn2.TabIndex = 0;
            this.btn2.Text = "다음>>";
            this.btn2.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btn2.TextIndent = 0;
            this.btn2.Click += new System.EventHandler(this.btn2_Click);
            // 
            // btn1
            // 
            this.btn1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn1.BackgroundImageDisable = null;
            this.btn1.BackgroundImageHover = null;
            this.btn1.BackgroundImageNormal = null;
            this.btn1.BackgroundImageSelect = null;
            this.btn1.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btn1.BorderEdgeRadius = 3;
            this.btn1.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btn1.ButtonImageCenter = null;
            this.btn1.ButtonImageLeft = null;
            this.btn1.ButtonImageRight = null;
            this.btn1.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btn1, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btn1.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btn1.ColorDepthFocus = 2;
            this.btn1.ColorDepthHover = 2;
            this.btn1.ColorDepthShadow = 2;
            this.btn1.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btn1.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btn1.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btn1.IconIndexDisable = -1;
            this.btn1.IconIndexHover = -1;
            this.btn1.IconIndexNormal = -1;
            this.btn1.IconIndexSelect = -1;
            this.btn1.Image = null;
            this.btn1.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btn1.ImageIndent = 0;
            this.btn1.ImageList = null;
            this.btn1.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btn1.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btn1.Location = new System.Drawing.Point(675, 8);
            this.btn1.Name = "btn1";
            this.btn1.Size = new System.Drawing.Size(75, 23);
            this.btn1.TabIndex = 0;
            this.btn1.Text = "<<이전";
            this.btn1.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btn1.TextIndent = 0;
            this.btn1.Visible = false;
            this.btn1.Click += new System.EventHandler(this.btn1_Click);
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.lblArrow);
            this.panelEx1.Controls.Add(this.lblBatchRegister);
            this.panelEx1.Controls.Add(this.lblSelectDevice);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(845, 22);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 10;
            // 
            // ucBatchDeviceSelectPanel
            // 
            this.ucBatchDeviceSelectPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.ucBatchDeviceSelectPanel.Devicelist = null;
            this.ucBatchDeviceSelectPanel.DeviceList = null;
            this.ucBatchDeviceSelectPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucBatchDeviceSelectPanel.Location = new System.Drawing.Point(0, 22);
            this.ucBatchDeviceSelectPanel.Name = "ucBatchDeviceSelectPanel";
            this.ucBatchDeviceSelectPanel.Size = new System.Drawing.Size(845, 533);
            this.ucBatchDeviceSelectPanel.TabIndex = 9;
            // 
            // ucBatchRegisterPanel
            // 
            this.ucBatchRegisterPanel.BackColor = System.Drawing.Color.White;
            this.ucBatchRegisterPanel.CurrentProtocol = RACTCommonClass.E_ConnectionProtocol.TELNET;
            this.ucBatchRegisterPanel.Devicelist = null;
            this.ucBatchRegisterPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucBatchRegisterPanel.GroupID = "-1";
            this.ucBatchRegisterPanel.Location = new System.Drawing.Point(0, 22);
            this.ucBatchRegisterPanel.Name = "ucBatchRegisterPanel";
            this.ucBatchRegisterPanel.Size = new System.Drawing.Size(845, 533);
            this.ucBatchRegisterPanel.TabIndex = 8;
            this.ucBatchRegisterPanel.Visible = false;
            // 
            // ucMainBatchRegisterBasePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ucBatchDeviceSelectPanel);
            this.Controls.Add(this.ucBatchRegisterPanel);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panelEx1);
            this.Name = "ucMainBatchRegisterBasePanel";
            this.Size = new System.Drawing.Size(845, 594);
            this.panel3.ResumeLayout(false);
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblArrow;
        private System.Windows.Forms.Label lblBatchRegister;
        private System.Windows.Forms.Label lblSelectDevice;
        private System.Windows.Forms.Panel panel3;
        private MKLibrary.Controls.MKButton btn2;
        private MKLibrary.Controls.MKButton btn1;
        private ucBatchRegisterPanel ucBatchRegisterPanel;
        private ucBatchDeviceSelectPanel ucBatchDeviceSelectPanel;
        private DevComponents.DotNetBar.PanelEx panelEx1;
    }
}
