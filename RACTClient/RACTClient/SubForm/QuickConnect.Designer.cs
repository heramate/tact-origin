namespace RACTClient
{
    partial class QuickConnect
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
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cboProtocol = new MKLibrary.Controls.MKComboBox();
            this.txtIPAddress = new MKLibrary.Controls.MKIPAddress(this.components);
            this.pnlSerialOption = new RACTClient.ucSerialConnectOption();
            this.mkLabel3 = new MKLibrary.Controls.MKLabel();
            this.nudPort = new System.Windows.Forms.NumericUpDown();
            this.txtPassword = new MKLibrary.Controls.MKTextBox();
            this.txtID = new MKLibrary.Controls.MKTextBox();
            this.lblPassword = new MKLibrary.Controls.MKLabel();
            this.lblID = new MKLibrary.Controls.MKLabel();
            this.mkLabel8 = new MKLibrary.Controls.MKLabel();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 12);
            this.label3.TabIndex = 179;
            this.label3.Text = "장비 IP :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 12);
            this.label1.TabIndex = 180;
            this.label1.Text = "프로토콜 :";
            // 
            // cboProtocol
            // 
            this.cboProtocol.BackColor = System.Drawing.SystemColors.Window;
            this.cboProtocol.BackColorSelected = System.Drawing.Color.Orange;
            this.cboProtocol.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(201)))), ((int)(((byte)(185)))));
            this.cboProtocol.BorderEdgeRadius = 3;
            this.cboProtocol.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.cboProtocol.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.Fixed;
            this.cboProtocol.BoxBorderColor = System.Drawing.Color.Gray;
            this.cboProtocol.ColorBoxSize = new System.Drawing.Size(12, 8);
            this.cboProtocol.ComboBoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProtocol.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboProtocol.ForeColor = System.Drawing.Color.Black;
            this.cboProtocol.ImageList = null;
            this.cboProtocol.ItemHeight = 14;
            this.cboProtocol.Location = new System.Drawing.Point(104, 13);
            this.cboProtocol.MaxDorpDownWidth = 500;
            this.cboProtocol.Name = "cboProtocol";
            this.cboProtocol.SelectedIndex = -1;
            this.cboProtocol.ShowColorBox = false;
            this.cboProtocol.Size = new System.Drawing.Size(151, 21);
            this.cboProtocol.TabIndex = 178;
            this.cboProtocol.SelectedIndexChanged += new System.EventHandler(this.cboProtocol_SelectedIndexChanged);
            // 
            // txtIPAddress
            // 
            this.txtIPAddress.BackColor = System.Drawing.SystemColors.Window;
            this.txtIPAddress.BackColorPattern = System.Drawing.Color.White;
            this.txtIPAddress.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.txtIPAddress.BorderColor = System.Drawing.Color.DimGray;
            this.txtIPAddress.BorderEdgeRadius = 3;
            this.txtIPAddress.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.txtIPAddress.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.Fixed;
            this.txtIPAddress.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.txtIPAddress.Location = new System.Drawing.Point(104, 40);
            this.txtIPAddress.Name = "txtIPAddress";
            this.txtIPAddress.ParentIPControl = null;
            this.txtIPAddress.Size = new System.Drawing.Size(151, 20);
            this.txtIPAddress.TabIndex = 177;
            // 
            // pnlSerialOption
            // 
            this.pnlSerialOption.Location = new System.Drawing.Point(11, 36);
            this.pnlSerialOption.Name = "pnlSerialOption";
            this.pnlSerialOption.Size = new System.Drawing.Size(253, 163);
            this.pnlSerialOption.TabIndex = 181;
            // 
            // mkLabel3
            // 
            this.mkLabel3.BackColor = System.Drawing.Color.Transparent;
            this.mkLabel3.BackColorPattern = System.Drawing.Color.White;
            this.mkLabel3.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.mkLabel3.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.mkLabel3.BorderEdgeRadius = 3;
            this.mkLabel3.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.mkLabel3.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.None;
            this.mkLabel3.CaptionLabel = false;
            this.mkLabel3.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.mkLabel3.Image = null;
            this.mkLabel3.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.mkLabel3.ImageIndent = 0;
            this.mkLabel3.ImageIndex = -1;
            this.mkLabel3.ImageList = null;
            this.mkLabel3.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.mkLabel3.InternalGap = new MKLibrary.MKObject.MKRectangleGap(0, 0, 0, 0);
            this.mkLabel3.Location = new System.Drawing.Point(12, 66);
            this.mkLabel3.Name = "mkLabel3";
            this.mkLabel3.Size = new System.Drawing.Size(69, 21);
            this.mkLabel3.TabIndex = 183;
            this.mkLabel3.Text = "Port :";
            this.mkLabel3.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.mkLabel3.TextIndent = 0;
            // 
            // nudPort
            // 
            this.nudPort.Location = new System.Drawing.Point(104, 66);
            this.nudPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudPort.Name = "nudPort";
            this.nudPort.Size = new System.Drawing.Size(151, 21);
            this.nudPort.TabIndex = 182;
            // 
            // txtPassword
            // 
            this.txtPassword.BackColorPattern = System.Drawing.Color.White;
            this.txtPassword.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.txtPassword.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtPassword.BorderEdgeRadius = 3;
            this.txtPassword.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.txtPassword.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.txtPassword.isLockHeight = true;
            this.txtPassword.Lines = new string[0];
            this.txtPassword.Location = new System.Drawing.Point(104, 120);
            this.txtPassword.MaxLength = 25;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(151, 21);
            this.txtPassword.TabIndex = 185;
            // 
            // txtID
            // 
            this.txtID.BackColorPattern = System.Drawing.Color.White;
            this.txtID.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.txtID.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtID.BorderEdgeRadius = 3;
            this.txtID.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.txtID.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.txtID.isLockHeight = true;
            this.txtID.Lines = new string[0];
            this.txtID.Location = new System.Drawing.Point(104, 93);
            this.txtID.MaxLength = 25;
            this.txtID.Name = "txtID";
            this.txtID.PasswordChar = '\0';
            this.txtID.Size = new System.Drawing.Size(151, 21);
            this.txtID.TabIndex = 184;
            // 
            // lblPassword
            // 
            this.lblPassword.BackColor = System.Drawing.Color.Transparent;
            this.lblPassword.BackColorPattern = System.Drawing.Color.White;
            this.lblPassword.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.lblPassword.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblPassword.BorderEdgeRadius = 3;
            this.lblPassword.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.lblPassword.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.None;
            this.lblPassword.CaptionLabel = false;
            this.lblPassword.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.lblPassword.Image = null;
            this.lblPassword.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.lblPassword.ImageIndent = 0;
            this.lblPassword.ImageIndex = -1;
            this.lblPassword.ImageList = null;
            this.lblPassword.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.lblPassword.InternalGap = new MKLibrary.MKObject.MKRectangleGap(0, 0, 0, 0);
            this.lblPassword.Location = new System.Drawing.Point(12, 120);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(82, 21);
            this.lblPassword.TabIndex = 187;
            this.lblPassword.Text = "Password :";
            this.lblPassword.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.lblPassword.TextIndent = 0;
            // 
            // lblID
            // 
            this.lblID.BackColor = System.Drawing.Color.Transparent;
            this.lblID.BackColorPattern = System.Drawing.Color.White;
            this.lblID.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.lblID.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblID.BorderEdgeRadius = 3;
            this.lblID.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.lblID.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.None;
            this.lblID.CaptionLabel = false;
            this.lblID.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.lblID.Image = null;
            this.lblID.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.lblID.ImageIndent = 0;
            this.lblID.ImageIndex = -1;
            this.lblID.ImageList = null;
            this.lblID.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.lblID.InternalGap = new MKLibrary.MKObject.MKRectangleGap(0, 0, 0, 0);
            this.lblID.Location = new System.Drawing.Point(11, 93);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(69, 21);
            this.lblID.TabIndex = 186;
            this.lblID.Text = "ID :";
            this.lblID.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.lblID.TextIndent = 0;
            // 
            // mkLabel8
            // 
            this.mkLabel8.BackColor = System.Drawing.Color.Transparent;
            this.mkLabel8.BackColorPattern = System.Drawing.Color.White;
            this.mkLabel8.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.mkLabel8.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.mkLabel8.BorderEdgeRadius = 3;
            this.mkLabel8.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.mkLabel8.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.None;
            this.mkLabel8.CaptionLabel = false;
            this.mkLabel8.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.mkLabel8.Image = null;
            this.mkLabel8.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.mkLabel8.ImageIndent = 0;
            this.mkLabel8.ImageIndex = -1;
            this.mkLabel8.ImageList = null;
            this.mkLabel8.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.mkLabel8.InternalGap = new MKLibrary.MKObject.MKRectangleGap(0, 0, 0, 0);
            this.mkLabel8.Location = new System.Drawing.Point(12, 120);
            this.mkLabel8.Name = "mkLabel8";
            this.mkLabel8.Size = new System.Drawing.Size(82, 21);
            this.mkLabel8.TabIndex = 187;
            this.mkLabel8.Text = "Password1 :";
            this.mkLabel8.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.mkLabel8.TextIndent = 0;
            // 
            // QuickConnect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 239);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtID);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblID);
            this.Controls.Add(this.mkLabel3);
            this.Controls.Add(this.nudPort);
            this.Controls.Add(this.pnlSerialOption);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboProtocol);
            this.Controls.Add(this.txtIPAddress);
            this.DoubleBuffered = true;
            this.Name = "QuickConnect";
            this.Text = "빠른 연결";
            this.Controls.SetChildIndex(this.txtIPAddress, 0);
            this.Controls.SetChildIndex(this.cboProtocol, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.pnlSerialOption, 0);
            this.Controls.SetChildIndex(this.nudPort, 0);
            this.Controls.SetChildIndex(this.mkLabel3, 0);
            this.Controls.SetChildIndex(this.lblID, 0);
            this.Controls.SetChildIndex(this.lblPassword, 0);
            this.Controls.SetChildIndex(this.txtID, 0);
            this.Controls.SetChildIndex(this.txtPassword, 0);
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private MKLibrary.Controls.MKComboBox cboProtocol;
        private MKLibrary.Controls.MKIPAddress txtIPAddress;
        private ucSerialConnectOption pnlSerialOption;
        private MKLibrary.Controls.MKLabel mkLabel3;
        private System.Windows.Forms.NumericUpDown nudPort;
        private MKLibrary.Controls.MKTextBox txtPassword;
        private MKLibrary.Controls.MKTextBox txtID;
        private MKLibrary.Controls.MKLabel lblPassword;
        private MKLibrary.Controls.MKLabel lblID;
        private MKLibrary.Controls.MKLabel mkLabel8;
    }
}