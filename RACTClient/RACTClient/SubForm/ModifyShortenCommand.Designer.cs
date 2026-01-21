namespace RACTClient
{
    partial class ModifyShortenCommand
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtCommand = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnReservedString = new MKLibrary.Controls.MKButton(this.components);
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboGroup = new MKLibrary.Controls.MKComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "화면 표시 명칭(Label)";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(18, 34);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(179, 21);
            this.txtName.TabIndex = 1;
            // 
            // txtCommand
            // 
            this.txtCommand.Location = new System.Drawing.Point(18, 87);
            this.txtCommand.Name = "txtCommand";
            this.txtCommand.Size = new System.Drawing.Size(383, 21);
            this.txtCommand.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "전송할 문자열";
            // 
            // btnReservedString
            // 
            this.btnReservedString.BackgroundImageDisable = null;
            this.btnReservedString.BackgroundImageHover = null;
            this.btnReservedString.BackgroundImageNormal = null;
            this.btnReservedString.BackgroundImageSelect = null;
            this.btnReservedString.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnReservedString.BorderEdgeRadius = 3;
            this.btnReservedString.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnReservedString.ButtonImageCenter = null;
            this.btnReservedString.ButtonImageLeft = null;
            this.btnReservedString.ButtonImageRight = null;
            this.btnReservedString.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnReservedString, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnReservedString.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnReservedString.ColorDepthFocus = 2;
            this.btnReservedString.ColorDepthHover = 2;
            this.btnReservedString.ColorDepthShadow = 2;
            this.btnReservedString.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnReservedString.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnReservedString.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnReservedString.IconIndexDisable = -1;
            this.btnReservedString.IconIndexHover = -1;
            this.btnReservedString.IconIndexNormal = -1;
            this.btnReservedString.IconIndexSelect = -1;
            this.btnReservedString.Image = null;
            this.btnReservedString.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnReservedString.ImageIndent = 0;
            this.btnReservedString.ImageList = null;
            this.btnReservedString.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnReservedString.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnReservedString.Location = new System.Drawing.Point(314, 60);
            this.btnReservedString.Name = "btnReservedString";
            this.btnReservedString.Size = new System.Drawing.Size(87, 23);
            this.btnReservedString.TabIndex = 4;
            this.btnReservedString.Text = "예약어 검색";
            this.btnReservedString.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnReservedString.TextIndent = 0;
            this.btnReservedString.Visible = false;
            this.btnReservedString.Click += new System.EventHandler(this.btnReservedString_Click);
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(18, 139);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(383, 21);
            this.txtDescription.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "설명";
            // 
            // cboGroup
            // 
            this.cboGroup.BackColor = System.Drawing.SystemColors.Window;
            this.cboGroup.BackColorSelected = System.Drawing.Color.Orange;
            this.cboGroup.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(201)))), ((int)(((byte)(185)))));
            this.cboGroup.BorderEdgeRadius = 3;
            this.cboGroup.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.cboGroup.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.Fixed;
            this.cboGroup.BoxBorderColor = System.Drawing.Color.Gray;
            this.cboGroup.ColorBoxSize = new System.Drawing.Size(12, 8);
            this.cboGroup.ComboBoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGroup.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboGroup.ForeColor = System.Drawing.Color.Black;
            this.cboGroup.ImageList = null;
            this.cboGroup.ItemHeight = 14;
            this.cboGroup.Location = new System.Drawing.Point(203, 34);
            this.cboGroup.MaxDorpDownWidth = 500;
            this.cboGroup.Name = "cboGroup";
            this.cboGroup.SelectedIndex = -1;
            this.cboGroup.ShowColorBox = false;
            this.cboGroup.Size = new System.Drawing.Size(198, 21);
            this.cboGroup.TabIndex = 174;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(201, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 12);
            this.label4.TabIndex = 173;
            this.label4.Text = "단축 명령 그룹";
            // 
            // ModifyShortenCommand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 216);
            this.Controls.Add(this.cboGroup);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnReservedString);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.txtCommand);
            this.Name = "ModifyShortenCommand";
            this.Text = "단축 명령";
            this.Controls.SetChildIndex(this.txtCommand, 0);
            this.Controls.SetChildIndex(this.txtDescription, 0);
            this.Controls.SetChildIndex(this.txtName, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.btnReservedString, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.cboGroup, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtCommand;
        private System.Windows.Forms.Label label2;
        private MKLibrary.Controls.MKButton btnReservedString;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label3;
        private MKLibrary.Controls.MKComboBox cboGroup;
        private System.Windows.Forms.Label label4;
    }
}