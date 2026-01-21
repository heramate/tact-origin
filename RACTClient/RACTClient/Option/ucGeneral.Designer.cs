namespace RACTClient
{
    partial class ucGeneral
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtLogPath = new System.Windows.Forms.TextBox();
            this.btnLogOpenPath = new MKLibrary.Controls.MKButton(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtScriptPath = new System.Windows.Forms.TextBox();
            this.btnScriptPath = new MKLibrary.Controls.MKButton(this.components);
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rdoTerminal = new System.Windows.Forms.RadioButton();
            this.rdoBatchRegister = new System.Windows.Forms.RadioButton();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.autoSaveSwitch = new DevComponents.DotNetBar.Controls.SwitchButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.txtLogPath);
            this.groupBox2.Controls.Add(this.btnLogOpenPath);
            this.groupBox2.Location = new System.Drawing.Point(15, 70);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(342, 52);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "로그 저장 위치";
            // 
            // txtLogPath
            // 
            this.txtLogPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLogPath.Location = new System.Drawing.Point(6, 20);
            this.txtLogPath.Name = "txtLogPath";
            this.txtLogPath.Size = new System.Drawing.Size(299, 21);
            this.txtLogPath.TabIndex = 3;
            // 
            // btnLogOpenPath
            // 
            this.btnLogOpenPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogOpenPath.BackgroundImageDisable = null;
            this.btnLogOpenPath.BackgroundImageHover = null;
            this.btnLogOpenPath.BackgroundImageNormal = null;
            this.btnLogOpenPath.BackgroundImageSelect = null;
            this.btnLogOpenPath.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnLogOpenPath.BorderEdgeRadius = 3;
            this.btnLogOpenPath.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnLogOpenPath.ButtonImageCenter = null;
            this.btnLogOpenPath.ButtonImageLeft = null;
            this.btnLogOpenPath.ButtonImageRight = null;
            this.btnLogOpenPath.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnLogOpenPath, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnLogOpenPath.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnLogOpenPath.ColorDepthFocus = 2;
            this.btnLogOpenPath.ColorDepthHover = 2;
            this.btnLogOpenPath.ColorDepthShadow = 2;
            this.btnLogOpenPath.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnLogOpenPath.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnLogOpenPath.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnLogOpenPath.IconIndexDisable = -1;
            this.btnLogOpenPath.IconIndexHover = -1;
            this.btnLogOpenPath.IconIndexNormal = -1;
            this.btnLogOpenPath.IconIndexSelect = -1;
            this.btnLogOpenPath.Image = null;
            this.btnLogOpenPath.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnLogOpenPath.ImageIndent = 0;
            this.btnLogOpenPath.ImageList = null;
            this.btnLogOpenPath.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnLogOpenPath.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnLogOpenPath.Location = new System.Drawing.Point(313, 20);
            this.btnLogOpenPath.Name = "btnLogOpenPath";
            this.btnLogOpenPath.Size = new System.Drawing.Size(23, 21);
            this.btnLogOpenPath.TabIndex = 2;
            this.btnLogOpenPath.Text = "...";
            this.btnLogOpenPath.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnLogOpenPath.TextIndent = 0;
            this.btnLogOpenPath.Click += new System.EventHandler(this.btnLogOpenPath_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtScriptPath);
            this.groupBox1.Controls.Add(this.btnScriptPath);
            this.groupBox1.Location = new System.Drawing.Point(15, 128);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(342, 58);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "스크립트 저장 위치";
            // 
            // txtScriptPath
            // 
            this.txtScriptPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtScriptPath.Location = new System.Drawing.Point(6, 24);
            this.txtScriptPath.Name = "txtScriptPath";
            this.txtScriptPath.Size = new System.Drawing.Size(299, 21);
            this.txtScriptPath.TabIndex = 3;
            // 
            // btnScriptPath
            // 
            this.btnScriptPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScriptPath.BackgroundImageDisable = null;
            this.btnScriptPath.BackgroundImageHover = null;
            this.btnScriptPath.BackgroundImageNormal = null;
            this.btnScriptPath.BackgroundImageSelect = null;
            this.btnScriptPath.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnScriptPath.BorderEdgeRadius = 3;
            this.btnScriptPath.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnScriptPath.ButtonImageCenter = null;
            this.btnScriptPath.ButtonImageLeft = null;
            this.btnScriptPath.ButtonImageRight = null;
            this.btnScriptPath.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnScriptPath, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnScriptPath.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnScriptPath.ColorDepthFocus = 2;
            this.btnScriptPath.ColorDepthHover = 2;
            this.btnScriptPath.ColorDepthShadow = 2;
            this.btnScriptPath.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnScriptPath.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnScriptPath.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnScriptPath.IconIndexDisable = -1;
            this.btnScriptPath.IconIndexHover = -1;
            this.btnScriptPath.IconIndexNormal = -1;
            this.btnScriptPath.IconIndexSelect = -1;
            this.btnScriptPath.Image = null;
            this.btnScriptPath.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnScriptPath.ImageIndent = 0;
            this.btnScriptPath.ImageList = null;
            this.btnScriptPath.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnScriptPath.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnScriptPath.Location = new System.Drawing.Point(313, 24);
            this.btnScriptPath.Name = "btnScriptPath";
            this.btnScriptPath.Size = new System.Drawing.Size(23, 23);
            this.btnScriptPath.TabIndex = 2;
            this.btnScriptPath.Text = "...";
            this.btnScriptPath.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnScriptPath.TextIndent = 0;
            this.btnScriptPath.Click += new System.EventHandler(this.btnScriptPath_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.rdoTerminal);
            this.groupBox3.Controls.Add(this.rdoBatchRegister);
            this.groupBox3.Location = new System.Drawing.Point(15, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(342, 52);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "메인 화면 선택";
            // 
            // rdoTerminal
            // 
            this.rdoTerminal.AutoSize = true;
            this.rdoTerminal.Location = new System.Drawing.Point(192, 24);
            this.rdoTerminal.Name = "rdoTerminal";
            this.rdoTerminal.Size = new System.Drawing.Size(59, 16);
            this.rdoTerminal.TabIndex = 0;
            this.rdoTerminal.Text = "터미널";
            this.rdoTerminal.UseVisualStyleBackColor = true;
            // 
            // rdoBatchRegister
            // 
            this.rdoBatchRegister.AutoSize = true;
            this.rdoBatchRegister.Checked = true;
            this.rdoBatchRegister.Location = new System.Drawing.Point(52, 24);
            this.rdoBatchRegister.Name = "rdoBatchRegister";
            this.rdoBatchRegister.Size = new System.Drawing.Size(103, 16);
            this.rdoBatchRegister.TabIndex = 0;
            this.rdoBatchRegister.TabStop = true;
            this.rdoBatchRegister.Text = "장비 일괄 등록";
            this.rdoBatchRegister.UseVisualStyleBackColor = true;
            // 
            // autoSaveSwitch
            // 
            this.autoSaveSwitch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // 
            // 
            this.autoSaveSwitch.BackgroundStyle.Class = "";
            this.autoSaveSwitch.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.autoSaveSwitch.Location = new System.Drawing.Point(202, 18);
            this.autoSaveSwitch.Name = "autoSaveSwitch";
            this.autoSaveSwitch.OffText = "꺼짐";
            this.autoSaveSwitch.OnText = "자동저장";
            this.autoSaveSwitch.Size = new System.Drawing.Size(80, 17);
            this.autoSaveSwitch.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.autoSaveSwitch.SwitchWidth = 15;
            this.autoSaveSwitch.TabIndex = 22;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.autoSaveSwitch);
            this.groupBox4.Location = new System.Drawing.Point(15, 193);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(342, 46);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "자동 저장 선택";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 12);
            this.label1.TabIndex = 23;
            this.label1.Text = "터미널 내용 자동 저장 설정";
            // 
            // ucGeneral
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Name = "ucGeneral";
            this.Size = new System.Drawing.Size(374, 372);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private MKLibrary.Controls.MKButton btnLogOpenPath;
        private System.Windows.Forms.GroupBox groupBox1;
        private MKLibrary.Controls.MKButton btnScriptPath;
        private System.Windows.Forms.TextBox txtLogPath;
        private System.Windows.Forms.TextBox txtScriptPath;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rdoTerminal;
        private System.Windows.Forms.RadioButton rdoBatchRegister;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private DevComponents.DotNetBar.Controls.SwitchButton autoSaveSwitch;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label1;
    }
}
