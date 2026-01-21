namespace RACTClient
{
    partial class ucHighlightColor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucHighlightColor));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnBackColor = new DevComponents.DotNetBar.ColorPickerButton();
            this.lblBackColor = new MKLibrary.Controls.MKLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnFontColor = new DevComponents.DotNetBar.ColorPickerButton();
            this.lblFontColor = new MKLibrary.Controls.MKLabel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblPreView = new System.Windows.Forms.RichTextBox();
            this.btnDefault = new MKLibrary.Controls.MKButton(this.components);
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblFont = new MKLibrary.Controls.MKLabel();
            this.btnFont = new MKLibrary.Controls.MKButton(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.btnBackColor);
            this.groupBox1.Controls.Add(this.lblBackColor);
            this.groupBox1.Location = new System.Drawing.Point(17, 76);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(305, 50);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "배경색";
            // 
            // btnBackColor
            // 
            this.btnBackColor.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnBackColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBackColor.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnBackColor.Image = ((System.Drawing.Image)(resources.GetObject("btnBackColor.Image")));
            this.btnBackColor.Location = new System.Drawing.Point(230, 20);
            this.btnBackColor.Name = "btnBackColor";
            this.btnBackColor.SelectedColorImageRectangle = new System.Drawing.Rectangle(2, 2, 12, 12);
            this.btnBackColor.Size = new System.Drawing.Size(54, 21);
            this.btnBackColor.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnBackColor.TabIndex = 3;
            this.btnBackColor.SelectedColorChanged += new System.EventHandler(this.btnBackColor_SelectedColorChanged);
            // 
            // lblBackColor
            // 
            this.lblBackColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBackColor.BackColor = System.Drawing.Color.Black;
            this.lblBackColor.BackColorPattern = System.Drawing.Color.White;
            this.lblBackColor.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.lblBackColor.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblBackColor.BorderEdgeRadius = 3;
            this.lblBackColor.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Round;
            this.lblBackColor.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.None;
            this.lblBackColor.CaptionLabel = false;
            this.lblBackColor.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.lblBackColor.Image = null;
            this.lblBackColor.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.lblBackColor.ImageIndent = 0;
            this.lblBackColor.ImageIndex = -1;
            this.lblBackColor.ImageList = null;
            this.lblBackColor.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.lblBackColor.InternalGap = new MKLibrary.MKObject.MKRectangleGap(0, 0, 0, 0);
            this.lblBackColor.Location = new System.Drawing.Point(9, 20);
            this.lblBackColor.Name = "lblBackColor";
            this.lblBackColor.Size = new System.Drawing.Size(207, 21);
            this.lblBackColor.TabIndex = 1;
            this.lblBackColor.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.lblBackColor.TextIndent = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.btnFontColor);
            this.groupBox2.Controls.Add(this.lblFontColor);
            this.groupBox2.Location = new System.Drawing.Point(17, 132);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(305, 50);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "전경색";
            // 
            // btnFontColor
            // 
            this.btnFontColor.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnFontColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFontColor.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.btnFontColor.Image = ((System.Drawing.Image)(resources.GetObject("btnFontColor.Image")));
            this.btnFontColor.Location = new System.Drawing.Point(230, 20);
            this.btnFontColor.Name = "btnFontColor";
            this.btnFontColor.SelectedColorImageRectangle = new System.Drawing.Rectangle(2, 2, 12, 12);
            this.btnFontColor.Size = new System.Drawing.Size(54, 21);
            this.btnFontColor.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.btnFontColor.TabIndex = 3;
            this.btnFontColor.SelectedColorChanged += new System.EventHandler(this.btnFontColor_SelectedColorChanged);
            // 
            // lblFontColor
            // 
            this.lblFontColor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFontColor.BackColor = System.Drawing.Color.White;
            this.lblFontColor.BackColorPattern = System.Drawing.Color.White;
            this.lblFontColor.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.lblFontColor.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblFontColor.BorderEdgeRadius = 3;
            this.lblFontColor.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Round;
            this.lblFontColor.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.None;
            this.lblFontColor.CaptionLabel = false;
            this.lblFontColor.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.lblFontColor.Image = null;
            this.lblFontColor.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.lblFontColor.ImageIndent = 0;
            this.lblFontColor.ImageIndex = -1;
            this.lblFontColor.ImageList = null;
            this.lblFontColor.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.lblFontColor.InternalGap = new MKLibrary.MKObject.MKRectangleGap(0, 0, 0, 0);
            this.lblFontColor.Location = new System.Drawing.Point(9, 20);
            this.lblFontColor.Name = "lblFontColor";
            this.lblFontColor.Size = new System.Drawing.Size(207, 21);
            this.lblFontColor.TabIndex = 1;
            this.lblFontColor.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.lblFontColor.TextIndent = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.lblPreView);
            this.groupBox3.Controls.Add(this.btnDefault);
            this.groupBox3.Location = new System.Drawing.Point(17, 191);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(305, 212);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "미리보기";
            // 
            // lblPreView
            // 
            this.lblPreView.Location = new System.Drawing.Point(9, 24);
            this.lblPreView.Name = "lblPreView";
            this.lblPreView.ReadOnly = true;
            this.lblPreView.Size = new System.Drawing.Size(207, 168);
            this.lblPreView.TabIndex = 4;
            this.lblPreView.Text = "";
            // 
            // btnDefault
            // 
            this.btnDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDefault.BackgroundImageDisable = null;
            this.btnDefault.BackgroundImageHover = null;
            this.btnDefault.BackgroundImageNormal = null;
            this.btnDefault.BackgroundImageSelect = null;
            this.btnDefault.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnDefault.BorderEdgeRadius = 3;
            this.btnDefault.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnDefault.ButtonImageCenter = null;
            this.btnDefault.ButtonImageLeft = null;
            this.btnDefault.ButtonImageRight = null;
            this.btnDefault.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnDefault, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnDefault.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnDefault.ColorDepthFocus = 2;
            this.btnDefault.ColorDepthHover = 2;
            this.btnDefault.ColorDepthShadow = 2;
            this.btnDefault.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnDefault.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnDefault.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnDefault.IconIndexDisable = -1;
            this.btnDefault.IconIndexHover = -1;
            this.btnDefault.IconIndexNormal = -1;
            this.btnDefault.IconIndexSelect = -1;
            this.btnDefault.Image = null;
            this.btnDefault.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnDefault.ImageIndent = 0;
            this.btnDefault.ImageList = null;
            this.btnDefault.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnDefault.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnDefault.Location = new System.Drawing.Point(230, 24);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(54, 23);
            this.btnDefault.TabIndex = 3;
            this.btnDefault.Text = "초기화";
            this.btnDefault.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnDefault.TextIndent = 0;
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.lblFont);
            this.groupBox4.Controls.Add(this.btnFont);
            this.groupBox4.Location = new System.Drawing.Point(17, 20);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(305, 50);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "글꼴";
            // 
            // lblFont
            // 
            this.lblFont.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFont.BackColor = System.Drawing.Color.Transparent;
            this.lblFont.BackColorPattern = System.Drawing.Color.White;
            this.lblFont.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.lblFont.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblFont.BorderEdgeRadius = 3;
            this.lblFont.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Round;
            this.lblFont.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.None;
            this.lblFont.CaptionLabel = false;
            this.lblFont.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.lblFont.Image = null;
            this.lblFont.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.lblFont.ImageIndent = 0;
            this.lblFont.ImageIndex = -1;
            this.lblFont.ImageList = null;
            this.lblFont.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.lblFont.InternalGap = new MKLibrary.MKObject.MKRectangleGap(0, 0, 0, 0);
            this.lblFont.Location = new System.Drawing.Point(9, 20);
            this.lblFont.Name = "lblFont";
            this.lblFont.Size = new System.Drawing.Size(207, 21);
            this.lblFont.TabIndex = 1;
            this.lblFont.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.lblFont.TextIndent = 0;
            // 
            // btnFont
            // 
            this.btnFont.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFont.BackgroundImageDisable = null;
            this.btnFont.BackgroundImageHover = null;
            this.btnFont.BackgroundImageNormal = null;
            this.btnFont.BackgroundImageSelect = null;
            this.btnFont.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnFont.BorderEdgeRadius = 3;
            this.btnFont.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnFont.ButtonImageCenter = null;
            this.btnFont.ButtonImageLeft = null;
            this.btnFont.ButtonImageRight = null;
            this.btnFont.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnFont, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnFont.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnFont.ColorDepthFocus = 2;
            this.btnFont.ColorDepthHover = 2;
            this.btnFont.ColorDepthShadow = 2;
            this.btnFont.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnFont.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnFont.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnFont.IconIndexDisable = -1;
            this.btnFont.IconIndexHover = -1;
            this.btnFont.IconIndexNormal = -1;
            this.btnFont.IconIndexSelect = -1;
            this.btnFont.Image = null;
            this.btnFont.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnFont.ImageIndent = 0;
            this.btnFont.ImageList = null;
            this.btnFont.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnFont.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnFont.Location = new System.Drawing.Point(230, 18);
            this.btnFont.Name = "btnFont";
            this.btnFont.Size = new System.Drawing.Size(54, 23);
            this.btnFont.TabIndex = 3;
            this.btnFont.Text = "...";
            this.btnFont.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnFont.TextIndent = 0;
            this.btnFont.Click += new System.EventHandler(this.btnFont_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(75, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(194, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "중요 장비 강조 FONT 설정 입니다.";
            // 
            // ucHighlightColor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Name = "ucHighlightColor";
            this.Size = new System.Drawing.Size(345, 406);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private MKLibrary.Controls.MKLabel lblBackColor;
        private System.Windows.Forms.GroupBox groupBox2;
        private MKLibrary.Controls.MKLabel lblFontColor;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private MKLibrary.Controls.MKButton btnDefault;
        private System.Windows.Forms.RichTextBox lblPreView;
        private DevComponents.DotNetBar.ColorPickerButton btnBackColor;
        private DevComponents.DotNetBar.ColorPickerButton btnFontColor;
        private System.Windows.Forms.GroupBox groupBox4;
        private MKLibrary.Controls.MKLabel lblFont;
        private MKLibrary.Controls.MKButton btnFont;
        private System.Windows.Forms.Label label1;
    }
}
