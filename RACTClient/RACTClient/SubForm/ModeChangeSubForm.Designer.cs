namespace RACTClient
{
    partial class ModeChangeSubForm
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
            this.m_SplashControl = new RACTClient.SplashControl();
            this.SuspendLayout();
            // 
            // m_SplashControl
            // 
            this.m_SplashControl.BackColor = System.Drawing.Color.DarkSalmon;
            this.m_SplashControl.BackgroundImage = global::RACTClient.Properties.Resources.login;
            this.m_SplashControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_SplashControl.Location = new System.Drawing.Point(0, 0);
            this.m_SplashControl.Name = "m_SplashControl";
            this.m_SplashControl.ServerIP = "10.30.5.140";
            this.m_SplashControl.Size = new System.Drawing.Size(570, 294);
            this.m_SplashControl.TabIndex = 0;
            this.m_SplashControl.UserID = "suwon1";
            this.m_SplashControl.UserPW = "suwon2";
            this.m_SplashControl.OnExit += new RACTClient.DefaultHandler(this.splashControl1_OnExit);
            // 
            // ModeChangeSubForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 294);
            this.Controls.Add(this.m_SplashControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ModeChangeSubForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ModeChangeSubForm";
            this.ResumeLayout(false);

        }

        #endregion

        public SplashControl m_SplashControl;

    }
}