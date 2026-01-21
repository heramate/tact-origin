namespace RACTServerServiceManager
{
    partial class ucConnectionDataPanel
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
            this.grbServerInfo = new System.Windows.Forms.GroupBox();
            this.txtServerIP = new MKLibrary.Controls.MKIPAddress(this.components);
            this.txtServerPort = new MKLibrary.Controls.MKTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.grbDBInfo = new System.Windows.Forms.GroupBox();
            this.txtDBPassword = new MKLibrary.Controls.MKTextBox();
            this.txtDBID = new MKLibrary.Controls.MKTextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDBName = new MKLibrary.Controls.MKTextBox();
            this.txtDBIP = new MKLibrary.Controls.MKTextBox();
            this.grbServiceStatus = new System.Windows.Forms.GroupBox();
            this.btnStop = new MKLibrary.Controls.MKButton(this.components);
            this.btnStart = new MKLibrary.Controls.MKButton(this.components);
            this.grbServerInfo.SuspendLayout();
            this.grbDBInfo.SuspendLayout();
            this.grbServiceStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // grbServerInfo
            // 
            this.grbServerInfo.Controls.Add(this.txtServerIP);
            this.grbServerInfo.Controls.Add(this.txtServerPort);
            this.grbServerInfo.Controls.Add(this.label2);
            this.grbServerInfo.Controls.Add(this.label1);
            this.grbServerInfo.Location = new System.Drawing.Point(15, 15);
            this.grbServerInfo.Name = "grbServerInfo";
            this.grbServerInfo.Size = new System.Drawing.Size(378, 100);
            this.grbServerInfo.TabIndex = 0;
            this.grbServerInfo.TabStop = false;
            this.grbServerInfo.Text = "서버 정보";
            // 
            // txtServerIP
            // 
            this.txtServerIP.BackColor = System.Drawing.SystemColors.Window;
            this.txtServerIP.BackColorPattern = System.Drawing.Color.White;
            this.txtServerIP.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.txtServerIP.BorderColor = System.Drawing.Color.DimGray;
            this.txtServerIP.BorderEdgeRadius = 3;
            this.txtServerIP.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.txtServerIP.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.Fixed;
            this.txtServerIP.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.txtServerIP.Location = new System.Drawing.Point(128, 23);
            this.txtServerIP.Name = "txtServerIP";
            this.txtServerIP.ParentIPControl = null;
            this.txtServerIP.Size = new System.Drawing.Size(151, 20);
            this.txtServerIP.TabIndex = 3;
            // 
            // txtServerPort
            // 
            this.txtServerPort.BackColorPattern = System.Drawing.Color.White;
            this.txtServerPort.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.txtServerPort.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtServerPort.BorderEdgeRadius = 3;
            this.txtServerPort.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.txtServerPort.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.txtServerPort.isLockHeight = true;
            this.txtServerPort.Lines = new string[0];
            this.txtServerPort.Location = new System.Drawing.Point(128, 54);
            this.txtServerPort.MaxLength = 2147483647;
            this.txtServerPort.Name = "txtServerPort";
            this.txtServerPort.PasswordChar = '\0';
            this.txtServerPort.Size = new System.Drawing.Size(151, 21);
            this.txtServerPort.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "서버 포트 :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "서버 IP :";
            // 
            // grbDBInfo
            // 
            this.grbDBInfo.Controls.Add(this.txtDBPassword);
            this.grbDBInfo.Controls.Add(this.txtDBID);
            this.grbDBInfo.Controls.Add(this.label6);
            this.grbDBInfo.Controls.Add(this.label5);
            this.grbDBInfo.Controls.Add(this.label4);
            this.grbDBInfo.Controls.Add(this.label3);
            this.grbDBInfo.Controls.Add(this.txtDBName);
            this.grbDBInfo.Controls.Add(this.txtDBIP);
            this.grbDBInfo.Location = new System.Drawing.Point(15, 121);
            this.grbDBInfo.Name = "grbDBInfo";
            this.grbDBInfo.Size = new System.Drawing.Size(378, 155);
            this.grbDBInfo.TabIndex = 0;
            this.grbDBInfo.TabStop = false;
            this.grbDBInfo.Text = "DB 정보";
            // 
            // txtDBPassword
            // 
            this.txtDBPassword.BackColorPattern = System.Drawing.Color.White;
            this.txtDBPassword.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.txtDBPassword.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtDBPassword.BorderEdgeRadius = 3;
            this.txtDBPassword.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.txtDBPassword.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.txtDBPassword.isLockHeight = true;
            this.txtDBPassword.Lines = new string[0];
            this.txtDBPassword.Location = new System.Drawing.Point(128, 111);
            this.txtDBPassword.MaxLength = 2147483647;
            this.txtDBPassword.Name = "txtDBPassword";
            this.txtDBPassword.PasswordChar = '*';
            this.txtDBPassword.Size = new System.Drawing.Size(225, 21);
            this.txtDBPassword.TabIndex = 2;
            // 
            // txtDBID
            // 
            this.txtDBID.BackColorPattern = System.Drawing.Color.White;
            this.txtDBID.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.txtDBID.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtDBID.BorderEdgeRadius = 3;
            this.txtDBID.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.txtDBID.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.txtDBID.isLockHeight = true;
            this.txtDBID.Lines = new string[0];
            this.txtDBID.Location = new System.Drawing.Point(128, 84);
            this.txtDBID.MaxLength = 2147483647;
            this.txtDBID.Name = "txtDBID";
            this.txtDBID.PasswordChar = '\0';
            this.txtDBID.Size = new System.Drawing.Size(225, 21);
            this.txtDBID.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 115);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "Password :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "DB ID :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "DB 이름 :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "DB 서버 IP :";
            // 
            // txtDBName
            // 
            this.txtDBName.BackColorPattern = System.Drawing.Color.White;
            this.txtDBName.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.txtDBName.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtDBName.BorderEdgeRadius = 3;
            this.txtDBName.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.txtDBName.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.txtDBName.isLockHeight = true;
            this.txtDBName.Lines = new string[0];
            this.txtDBName.Location = new System.Drawing.Point(128, 57);
            this.txtDBName.MaxLength = 2147483647;
            this.txtDBName.Name = "txtDBName";
            this.txtDBName.PasswordChar = '\0';
            this.txtDBName.Size = new System.Drawing.Size(225, 21);
            this.txtDBName.TabIndex = 2;
            // 
            // txtDBIP
            // 
            this.txtDBIP.BackColorPattern = System.Drawing.Color.White;
            this.txtDBIP.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.txtDBIP.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtDBIP.BorderEdgeRadius = 3;
            this.txtDBIP.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.txtDBIP.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.txtDBIP.isLockHeight = true;
            this.txtDBIP.Lines = new string[0];
            this.txtDBIP.Location = new System.Drawing.Point(128, 30);
            this.txtDBIP.MaxLength = 2147483647;
            this.txtDBIP.Name = "txtDBIP";
            this.txtDBIP.PasswordChar = '\0';
            this.txtDBIP.Size = new System.Drawing.Size(225, 21);
            this.txtDBIP.TabIndex = 2;
            // 
            // grbServiceStatus
            // 
            this.grbServiceStatus.Controls.Add(this.btnStop);
            this.grbServiceStatus.Controls.Add(this.btnStart);
            this.grbServiceStatus.Location = new System.Drawing.Point(15, 282);
            this.grbServiceStatus.Name = "grbServiceStatus";
            this.grbServiceStatus.Size = new System.Drawing.Size(378, 66);
            this.grbServiceStatus.TabIndex = 0;
            this.grbServiceStatus.TabStop = false;
            this.grbServiceStatus.Text = "서비스 상태";
            // 
            // btnStop
            // 
            this.btnStop.BackgroundImageDisable = null;
            this.btnStop.BackgroundImageHover = null;
            this.btnStop.BackgroundImageNormal = null;
            this.btnStop.BackgroundImageSelect = null;
            this.btnStop.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnStop.BorderEdgeRadius = 3;
            this.btnStop.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnStop.ButtonImageCenter = null;
            this.btnStop.ButtonImageLeft = null;
            this.btnStop.ButtonImageRight = null;
            this.btnStop.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnStop, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnStop.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnStop.ColorDepthFocus = 2;
            this.btnStop.ColorDepthHover = 2;
            this.btnStop.ColorDepthShadow = 2;
            this.btnStop.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnStop.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnStop.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnStop.IconIndexDisable = -1;
            this.btnStop.IconIndexHover = -1;
            this.btnStop.IconIndexNormal = -1;
            this.btnStop.IconIndexSelect = -1;
            this.btnStop.Image = null;
            this.btnStop.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnStop.ImageIndent = 0;
            this.btnStop.ImageList = null;
            this.btnStop.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnStop.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnStop.Location = new System.Drawing.Point(101, 26);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(72, 25);
            this.btnStop.TabIndex = 0;
            this.btnStop.Text = "중지";
            this.btnStop.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnStop.TextIndent = 0;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackgroundImageDisable = null;
            this.btnStart.BackgroundImageHover = null;
            this.btnStart.BackgroundImageNormal = null;
            this.btnStart.BackgroundImageSelect = null;
            this.btnStart.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnStart.BorderEdgeRadius = 3;
            this.btnStart.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnStart.ButtonImageCenter = null;
            this.btnStart.ButtonImageLeft = null;
            this.btnStart.ButtonImageRight = null;
            this.btnStart.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnStart, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnStart.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnStart.ColorDepthFocus = 2;
            this.btnStart.ColorDepthHover = 2;
            this.btnStart.ColorDepthShadow = 2;
            this.btnStart.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnStart.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnStart.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnStart.IconIndexDisable = -1;
            this.btnStart.IconIndexHover = -1;
            this.btnStart.IconIndexNormal = -1;
            this.btnStart.IconIndexSelect = -1;
            this.btnStart.Image = null;
            this.btnStart.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnStart.ImageIndent = 0;
            this.btnStart.ImageList = null;
            this.btnStart.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnStart.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnStart.Location = new System.Drawing.Point(23, 26);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(72, 25);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "시작";
            this.btnStart.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnStart.TextIndent = 0;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // ucConnectionDataPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grbServiceStatus);
            this.Controls.Add(this.grbDBInfo);
            this.Controls.Add(this.grbServerInfo);
            this.Name = "ucConnectionDataPanel";
            this.Size = new System.Drawing.Size(408, 366);
            this.grbServerInfo.ResumeLayout(false);
            this.grbServerInfo.PerformLayout();
            this.grbDBInfo.ResumeLayout(false);
            this.grbDBInfo.PerformLayout();
            this.grbServiceStatus.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grbServerInfo;
        private System.Windows.Forms.GroupBox grbDBInfo;
        private System.Windows.Forms.GroupBox grbServiceStatus;
        private MKLibrary.Controls.MKTextBox txtServerPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private MKLibrary.Controls.MKTextBox txtDBID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private MKLibrary.Controls.MKTextBox txtDBName;
        private MKLibrary.Controls.MKTextBox txtDBIP;
        private MKLibrary.Controls.MKButton btnStop;
        private MKLibrary.Controls.MKButton btnStart;
        private MKLibrary.Controls.MKTextBox txtDBPassword;
        private MKLibrary.Controls.MKIPAddress txtServerIP;
    }
}
