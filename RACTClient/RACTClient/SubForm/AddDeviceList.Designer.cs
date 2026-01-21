namespace RACTClient
{
    partial class AddDeviceList
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoSaveDeviceList = new System.Windows.Forms.RadioButton();
            this.rdoAddUserGroup = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoSaveDeviceList);
            this.groupBox1.Controls.Add(this.rdoAddUserGroup);
            this.groupBox1.Location = new System.Drawing.Point(12, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(483, 73);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "장비 등록 형태";
            // 
            // rdoSaveDeviceList
            // 
            this.rdoSaveDeviceList.AutoSize = true;
            this.rdoSaveDeviceList.Location = new System.Drawing.Point(307, 33);
            this.rdoSaveDeviceList.Name = "rdoSaveDeviceList";
            this.rdoSaveDeviceList.Size = new System.Drawing.Size(131, 16);
            this.rdoSaveDeviceList.TabIndex = 5;
            this.rdoSaveDeviceList.Text = "장비 목록 파일 저장";
            this.rdoSaveDeviceList.UseVisualStyleBackColor = true;
            // 
            // rdoAddUserGroup
            // 
            this.rdoAddUserGroup.AutoSize = true;
            this.rdoAddUserGroup.Checked = true;
            this.rdoAddUserGroup.Location = new System.Drawing.Point(54, 33);
            this.rdoAddUserGroup.Name = "rdoAddUserGroup";
            this.rdoAddUserGroup.Size = new System.Drawing.Size(115, 16);
            this.rdoAddUserGroup.TabIndex = 4;
            this.rdoAddUserGroup.TabStop = true;
            this.rdoAddUserGroup.Text = "사용자 장비 등록";
            this.rdoAddUserGroup.UseVisualStyleBackColor = true;
            // 
            // AddDeviceList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(578, 129);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.Name = "AddDeviceList";
            this.Text = "장비등록";
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoSaveDeviceList;
        private System.Windows.Forms.RadioButton rdoAddUserGroup;
    }
}