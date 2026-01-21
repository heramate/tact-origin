using MKLibrary.Controls;
namespace RACTClient
{
    partial class ucBatchRegisterPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucBatchRegisterPanel));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cboProtocol = new MKLibrary.Controls.MKComboBox();
            this.cboGroup = new MKLibrary.Controls.MKComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grbSerialPortInfo = new System.Windows.Forms.GroupBox();
            this.grbTelnet = new System.Windows.Forms.GroupBox();
            this.nudPort = new System.Windows.Forms.NumericUpDown();
            this.grbGeneralInfo = new System.Windows.Forms.GroupBox();
            this.btnNewGroup = new MKLibrary.Controls.MKButton(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.fgDeviceList = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlSerialPort = new RACTClient.ucSerialPortPanel();
            this.panel1.SuspendLayout();
            this.grbSerialPortInfo.SuspendLayout();
            this.grbTelnet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).BeginInit();
            this.grbGeneralInfo.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fgDeviceList)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Protocol :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "Port :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "장비 그룹 :";
            // 
            // cboProtocol
            // 
            this.cboProtocol.BackColor = System.Drawing.SystemColors.Window;
            this.cboProtocol.BackColorSelected = System.Drawing.Color.Orange;
            this.cboProtocol.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.cboProtocol.BorderEdgeRadius = 3;
            this.cboProtocol.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.cboProtocol.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.None;
            this.cboProtocol.BoxBorderColor = System.Drawing.Color.Gray;
            this.cboProtocol.ColorBoxSize = new System.Drawing.Size(12, 8);
            this.cboProtocol.ComboBoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProtocol.ImageList = null;
            this.cboProtocol.ItemHeight = 14;
            this.cboProtocol.Location = new System.Drawing.Point(100, 25);
            this.cboProtocol.MaxDorpDownWidth = 500;
            this.cboProtocol.Name = "cboProtocol";
            this.cboProtocol.SelectedIndex = -1;
            this.cboProtocol.ShowColorBox = false;
            this.cboProtocol.Size = new System.Drawing.Size(156, 21);
            this.cboProtocol.TabIndex = 3;
            this.cboProtocol.SelectedIndexChanged += new System.EventHandler(this.cboProtocol_SelectedIndexChanged);
            // 
            // cboGroup
            // 
            this.cboGroup.BackColor = System.Drawing.SystemColors.Window;
            this.cboGroup.BackColorSelected = System.Drawing.Color.Orange;
            this.cboGroup.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.cboGroup.BorderEdgeRadius = 3;
            this.cboGroup.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.cboGroup.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.None;
            this.cboGroup.BoxBorderColor = System.Drawing.Color.Gray;
            this.cboGroup.ColorBoxSize = new System.Drawing.Size(12, 8);
            this.cboGroup.ComboBoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGroup.ImageList = null;
            this.cboGroup.ItemHeight = 14;
            this.cboGroup.Location = new System.Drawing.Point(100, 54);
            this.cboGroup.MaxDorpDownWidth = 500;
            this.cboGroup.Name = "cboGroup";
            this.cboGroup.SelectedIndex = -1;
            this.cboGroup.ShowColorBox = false;
            this.cboGroup.Size = new System.Drawing.Size(156, 21);
            this.cboGroup.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.grbTelnet);
            this.panel1.Controls.Add(this.grbSerialPortInfo);
            this.panel1.Controls.Add(this.grbGeneralInfo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 273);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(714, 200);
            this.panel1.TabIndex = 8;
            // 
            // grbSerialPortInfo
            // 
            this.grbSerialPortInfo.Controls.Add(this.pnlSerialPort);
            this.grbSerialPortInfo.Location = new System.Drawing.Point(285, 13);
            this.grbSerialPortInfo.Name = "grbSerialPortInfo";
            this.grbSerialPortInfo.Size = new System.Drawing.Size(280, 178);
            this.grbSerialPortInfo.TabIndex = 10;
            this.grbSerialPortInfo.TabStop = false;
            this.grbSerialPortInfo.Text = "SerialPort 정보";
            // 
            // grbTelnet
            // 
            this.grbTelnet.Controls.Add(this.label2);
            this.grbTelnet.Controls.Add(this.nudPort);
            this.grbTelnet.Location = new System.Drawing.Point(286, 13);
            this.grbTelnet.Name = "grbTelnet";
            this.grbTelnet.Size = new System.Drawing.Size(279, 66);
            this.grbTelnet.TabIndex = 9;
            this.grbTelnet.TabStop = false;
            this.grbTelnet.Text = "Telnet 정보";
            // 
            // nudPort
            // 
            this.nudPort.Location = new System.Drawing.Point(65, 25);
            this.nudPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudPort.Name = "nudPort";
            this.nudPort.Size = new System.Drawing.Size(197, 21);
            this.nudPort.TabIndex = 16;
            // 
            // grbGeneralInfo
            // 
            this.grbGeneralInfo.Controls.Add(this.btnNewGroup);
            this.grbGeneralInfo.Controls.Add(this.label1);
            this.grbGeneralInfo.Controls.Add(this.cboProtocol);
            this.grbGeneralInfo.Controls.Add(this.label3);
            this.grbGeneralInfo.Controls.Add(this.cboGroup);
            this.grbGeneralInfo.Location = new System.Drawing.Point(11, 13);
            this.grbGeneralInfo.Name = "grbGeneralInfo";
            this.grbGeneralInfo.Size = new System.Drawing.Size(266, 115);
            this.grbGeneralInfo.TabIndex = 8;
            this.grbGeneralInfo.TabStop = false;
            this.grbGeneralInfo.Text = "기본 정보";
            // 
            // btnNewGroup
            // 
            this.btnNewGroup.BackgroundImageDisable = null;
            this.btnNewGroup.BackgroundImageHover = null;
            this.btnNewGroup.BackgroundImageNormal = null;
            this.btnNewGroup.BackgroundImageSelect = null;
            this.btnNewGroup.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnNewGroup.BorderEdgeRadius = 3;
            this.btnNewGroup.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnNewGroup.ButtonImageCenter = null;
            this.btnNewGroup.ButtonImageLeft = null;
            this.btnNewGroup.ButtonImageRight = null;
            this.btnNewGroup.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnNewGroup, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnNewGroup.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnNewGroup.ColorDepthFocus = 2;
            this.btnNewGroup.ColorDepthHover = 2;
            this.btnNewGroup.ColorDepthShadow = 2;
            this.btnNewGroup.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnNewGroup.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnNewGroup.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnNewGroup.IconIndexDisable = -1;
            this.btnNewGroup.IconIndexHover = -1;
            this.btnNewGroup.IconIndexNormal = -1;
            this.btnNewGroup.IconIndexSelect = -1;
            this.btnNewGroup.Image = null;
            this.btnNewGroup.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnNewGroup.ImageIndent = 0;
            this.btnNewGroup.ImageList = null;
            this.btnNewGroup.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnNewGroup.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnNewGroup.Location = new System.Drawing.Point(182, 81);
            this.btnNewGroup.Name = "btnNewGroup";
            this.btnNewGroup.Size = new System.Drawing.Size(74, 23);
            this.btnNewGroup.TabIndex = 17;
            this.btnNewGroup.Text = "새 그룹";
            this.btnNewGroup.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnNewGroup.TextIndent = 0;
            this.btnNewGroup.Click += new System.EventHandler(this.btnNewGroup_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.fgDeviceList);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.panel2.Size = new System.Drawing.Size(714, 267);
            this.panel2.TabIndex = 9;
            // 
            // fgDeviceList
            // 
            this.fgDeviceList.BackColor = System.Drawing.SystemColors.Window;
            this.fgDeviceList.ColumnInfo = resources.GetString("fgDeviceList.ColumnInfo");
            this.fgDeviceList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fgDeviceList.EditOptions = ((C1.Win.C1FlexGrid.EditFlags)((C1.Win.C1FlexGrid.EditFlags.AutoSearch | C1.Win.C1FlexGrid.EditFlags.MultiCheck)));
            this.fgDeviceList.ForeColor = System.Drawing.SystemColors.WindowText;
            this.fgDeviceList.Location = new System.Drawing.Point(3, 34);
            this.fgDeviceList.Name = "fgDeviceList";
            this.fgDeviceList.Rows.Count = 1;
            this.fgDeviceList.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
            this.fgDeviceList.Size = new System.Drawing.Size(708, 233);
            this.fgDeviceList.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("fgDeviceList.Styles"));
            this.fgDeviceList.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.label4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(3, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(708, 34);
            this.panel3.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "장비 목록 :";
            // 
            // pnlSerialPort
            // 
            this.pnlSerialPort.BackColor = System.Drawing.Color.White;
            this.pnlSerialPort.Location = new System.Drawing.Point(1, 15);
            this.pnlSerialPort.Name = "pnlSerialPort";
            this.pnlSerialPort.Size = new System.Drawing.Size(273, 155);
            this.pnlSerialPort.TabIndex = 0;
            // 
            // ucBatchRegisterPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "ucBatchRegisterPanel";
            this.Size = new System.Drawing.Size(714, 473);
            this.panel1.ResumeLayout(false);
            this.grbSerialPortInfo.ResumeLayout(false);
            this.grbTelnet.ResumeLayout(false);
            this.grbTelnet.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).EndInit();
            this.grbGeneralInfo.ResumeLayout(false);
            this.grbGeneralInfo.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fgDeviceList)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private MKComboBox cboProtocol;
        private MKComboBox cboGroup;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private C1.Win.C1FlexGrid.C1FlexGrid fgDeviceList;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox grbGeneralInfo;
        private System.Windows.Forms.NumericUpDown nudPort;
        private MKButton btnNewGroup;
        private System.Windows.Forms.GroupBox grbTelnet;
        private System.Windows.Forms.GroupBox grbSerialPortInfo;
        private ucSerialPortPanel pnlSerialPort;
    }
}
