namespace RACTClient
{
    partial class DeviceSearch
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
            this.ucDeviceSearch1 = new RACTClient.ucDeviceSearch();
            this.SuspendLayout();
            // 
            // ucDeviceSearch1
            // 
            this.ucDeviceSearch1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.ucDeviceSearch1.DeviceName = null;
            this.ucDeviceSearch1.DevicePartCode = null;
            this.ucDeviceSearch1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucDeviceSearch1.IPAddress = null;
            this.ucDeviceSearch1.Location = new System.Drawing.Point(0, 0);
            this.ucDeviceSearch1.ModelInfo = null;
            this.ucDeviceSearch1.Name = "ucDeviceSearch1";
            this.ucDeviceSearch1.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.ucDeviceSearch1.Size = new System.Drawing.Size(908, 572);
            this.ucDeviceSearch1.TabIndex = 0;
            // 
            // DeviceSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(908, 611);
            this.Controls.Add(this.ucDeviceSearch1);
            this.Name = "DeviceSearch";
            this.Text = "장비 검색";
            this.Controls.SetChildIndex(this.ucDeviceSearch1, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private ucDeviceSearch ucDeviceSearch1;
    }
}