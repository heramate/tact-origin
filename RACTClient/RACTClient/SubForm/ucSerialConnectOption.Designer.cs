namespace RACTClient
{
    partial class ucSerialConnectOption
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
            this.label1 = new System.Windows.Forms.Label();
            this.cboPort = new MKLibrary.Controls.MKComboBox();
            this.cboBaudRate = new MKLibrary.Controls.MKComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboDataBit = new MKLibrary.Controls.MKComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboParity = new MKLibrary.Controls.MKComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboStopBit = new MKLibrary.Controls.MKComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 12);
            this.label1.TabIndex = 182;
            this.label1.Text = "COM Port :";
            // 
            // cboPort
            // 
            this.cboPort.BackColor = System.Drawing.SystemColors.Window;
            this.cboPort.BackColorSelected = System.Drawing.Color.Orange;
            this.cboPort.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(201)))), ((int)(((byte)(185)))));
            this.cboPort.BorderEdgeRadius = 3;
            this.cboPort.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.cboPort.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.Fixed;
            this.cboPort.BoxBorderColor = System.Drawing.Color.Gray;
            this.cboPort.ColorBoxSize = new System.Drawing.Size(12, 8);
            this.cboPort.ComboBoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPort.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboPort.ForeColor = System.Drawing.Color.Black;
            this.cboPort.ImageList = null;
            this.cboPort.ItemHeight = 14;
            this.cboPort.Location = new System.Drawing.Point(93, 3);
            this.cboPort.MaxDorpDownWidth = 500;
            this.cboPort.Name = "cboPort";
            this.cboPort.SelectedIndex = -1;
            this.cboPort.ShowColorBox = false;
            this.cboPort.Size = new System.Drawing.Size(151, 21);
            this.cboPort.TabIndex = 181;
            // 
            // cboBaudRate
            // 
            this.cboBaudRate.BackColor = System.Drawing.SystemColors.Window;
            this.cboBaudRate.BackColorSelected = System.Drawing.Color.Orange;
            this.cboBaudRate.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(201)))), ((int)(((byte)(185)))));
            this.cboBaudRate.BorderEdgeRadius = 3;
            this.cboBaudRate.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.cboBaudRate.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.Fixed;
            this.cboBaudRate.BoxBorderColor = System.Drawing.Color.Gray;
            this.cboBaudRate.ColorBoxSize = new System.Drawing.Size(12, 8);
            this.cboBaudRate.ComboBoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBaudRate.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboBaudRate.ForeColor = System.Drawing.Color.Black;
            this.cboBaudRate.ImageList = null;
            this.cboBaudRate.ItemHeight = 14;
            this.cboBaudRate.Location = new System.Drawing.Point(93, 30);
            this.cboBaudRate.MaxDorpDownWidth = 500;
            this.cboBaudRate.Name = "cboBaudRate";
            this.cboBaudRate.SelectedIndex = -1;
            this.cboBaudRate.ShowColorBox = false;
            this.cboBaudRate.Size = new System.Drawing.Size(151, 21);
            this.cboBaudRate.TabIndex = 181;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 12);
            this.label2.TabIndex = 182;
            this.label2.Text = "Baud rate :";
            // 
            // cboDataBit
            // 
            this.cboDataBit.BackColor = System.Drawing.SystemColors.Window;
            this.cboDataBit.BackColorSelected = System.Drawing.Color.Orange;
            this.cboDataBit.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(201)))), ((int)(((byte)(185)))));
            this.cboDataBit.BorderEdgeRadius = 3;
            this.cboDataBit.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.cboDataBit.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.Fixed;
            this.cboDataBit.BoxBorderColor = System.Drawing.Color.Gray;
            this.cboDataBit.ColorBoxSize = new System.Drawing.Size(12, 8);
            this.cboDataBit.ComboBoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDataBit.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboDataBit.ForeColor = System.Drawing.Color.Black;
            this.cboDataBit.ImageList = null;
            this.cboDataBit.ItemHeight = 14;
            this.cboDataBit.Location = new System.Drawing.Point(93, 57);
            this.cboDataBit.MaxDorpDownWidth = 500;
            this.cboDataBit.Name = "cboDataBit";
            this.cboDataBit.SelectedIndex = -1;
            this.cboDataBit.ShowColorBox = false;
            this.cboDataBit.Size = new System.Drawing.Size(151, 21);
            this.cboDataBit.TabIndex = 181;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 12);
            this.label3.TabIndex = 182;
            this.label3.Text = "Data bits :";
            // 
            // cboParity
            // 
            this.cboParity.BackColor = System.Drawing.SystemColors.Window;
            this.cboParity.BackColorSelected = System.Drawing.Color.Orange;
            this.cboParity.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(201)))), ((int)(((byte)(185)))));
            this.cboParity.BorderEdgeRadius = 3;
            this.cboParity.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.cboParity.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.Fixed;
            this.cboParity.BoxBorderColor = System.Drawing.Color.Gray;
            this.cboParity.ColorBoxSize = new System.Drawing.Size(12, 8);
            this.cboParity.ComboBoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboParity.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboParity.ForeColor = System.Drawing.Color.Black;
            this.cboParity.ImageList = null;
            this.cboParity.ItemHeight = 14;
            this.cboParity.Location = new System.Drawing.Point(93, 84);
            this.cboParity.MaxDorpDownWidth = 500;
            this.cboParity.Name = "cboParity";
            this.cboParity.SelectedIndex = -1;
            this.cboParity.ShowColorBox = false;
            this.cboParity.Size = new System.Drawing.Size(151, 21);
            this.cboParity.TabIndex = 181;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 12);
            this.label4.TabIndex = 182;
            this.label4.Text = "Parity :";
            // 
            // cboStopBit
            // 
            this.cboStopBit.BackColor = System.Drawing.SystemColors.Window;
            this.cboStopBit.BackColorSelected = System.Drawing.Color.Orange;
            this.cboStopBit.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(201)))), ((int)(((byte)(185)))));
            this.cboStopBit.BorderEdgeRadius = 3;
            this.cboStopBit.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.cboStopBit.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.Fixed;
            this.cboStopBit.BoxBorderColor = System.Drawing.Color.Gray;
            this.cboStopBit.ColorBoxSize = new System.Drawing.Size(12, 8);
            this.cboStopBit.ComboBoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStopBit.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboStopBit.ForeColor = System.Drawing.Color.Black;
            this.cboStopBit.ImageList = null;
            this.cboStopBit.ItemHeight = 14;
            this.cboStopBit.Location = new System.Drawing.Point(93, 111);
            this.cboStopBit.MaxDorpDownWidth = 500;
            this.cboStopBit.Name = "cboStopBit";
            this.cboStopBit.SelectedIndex = -1;
            this.cboStopBit.ShowColorBox = false;
            this.cboStopBit.Size = new System.Drawing.Size(151, 21);
            this.cboStopBit.TabIndex = 181;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1, 115);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 12);
            this.label5.TabIndex = 182;
            this.label5.Text = "Sotp bits :";
            // 
            // ucSerialConnectOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboStopBit);
            this.Controls.Add(this.cboParity);
            this.Controls.Add(this.cboDataBit);
            this.Controls.Add(this.cboBaudRate);
            this.Controls.Add(this.cboPort);
            this.Name = "ucSerialConnectOption";
            this.Size = new System.Drawing.Size(246, 136);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private MKLibrary.Controls.MKComboBox cboPort;
        private MKLibrary.Controls.MKComboBox cboBaudRate;
        private System.Windows.Forms.Label label2;
        private MKLibrary.Controls.MKComboBox cboDataBit;
        private System.Windows.Forms.Label label3;
        private MKLibrary.Controls.MKComboBox cboParity;
        private System.Windows.Forms.Label label4;
        private MKLibrary.Controls.MKComboBox cboStopBit;
        private System.Windows.Forms.Label label5;
    }
}
