using RACTCommonClass;
namespace RACTClient
{
    partial class OptionConfigurationForm : BaseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionConfigurationForm));
            this.pnlConnectOption = new RACTClient.ucConnectOptionPanel();
            this.trvOptionConfiguration = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.pnlTelnet = new RACTClient.ucTelnetPanel();
            this.pnlSerialPort = new RACTClient.ucSerialPortPanel();
            this.pnlTerminalPopupType = new RACTClient.ucTerminalPopupType();
            this.pnlSubPanl = new System.Windows.Forms.Panel();
            this.pnlGeneral = new RACTClient.ucGeneral();
            this.pnlHighlightColor = new RACTClient.ucHighlightColor();
            this.pnlTerminalLayout = new RACTClient.ucTerminalLayout();
            this.pnlTerminalColor = new RACTClient.ucTerminalColor();
            this.pnlSubPanl.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlConnectOption
            // 
            this.pnlConnectOption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.pnlConnectOption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlConnectOption.Location = new System.Drawing.Point(0, 0);
            this.pnlConnectOption.Name = "pnlConnectOption";
            this.pnlConnectOption.Protocol = RACTCommonClass.E_ConnectionProtocol.TELNET;
            this.pnlConnectOption.Size = new System.Drawing.Size(363, 378);
            this.pnlConnectOption.TabIndex = 1;
            // 
            // trvOptionConfiguration
            // 
            this.trvOptionConfiguration.Dock = System.Windows.Forms.DockStyle.Left;
            this.trvOptionConfiguration.ImageIndex = 0;
            this.trvOptionConfiguration.ImageList = this.imageList1;
            this.trvOptionConfiguration.Location = new System.Drawing.Point(10, 10);
            this.trvOptionConfiguration.Margin = new System.Windows.Forms.Padding(0);
            this.trvOptionConfiguration.Name = "trvOptionConfiguration";
            this.trvOptionConfiguration.SelectedImageIndex = 0;
            this.trvOptionConfiguration.Size = new System.Drawing.Size(158, 378);
            this.trvOptionConfiguration.TabIndex = 2;
            this.trvOptionConfiguration.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvOptionConfiguration_AfterSelect);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "옵션설정_소분류.png");
            this.imageList1.Images.SetKeyName(1, "옵션설정_대분류.png");
            // 
            // pnlTelnet
            // 
            this.pnlTelnet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.pnlTelnet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTelnet.Location = new System.Drawing.Point(0, 0);
            this.pnlTelnet.Name = "pnlTelnet";
            this.pnlTelnet.PortNumber = 0;
            this.pnlTelnet.Size = new System.Drawing.Size(363, 378);
            this.pnlTelnet.TabIndex = 3;
            // 
            // pnlSerialPort
            // 
            this.pnlSerialPort.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.pnlSerialPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSerialPort.Location = new System.Drawing.Point(0, 0);
            this.pnlSerialPort.Name = "pnlSerialPort";
            this.pnlSerialPort.Size = new System.Drawing.Size(363, 378);
            this.pnlSerialPort.TabIndex = 4;
            // 
            // pnlTerminalPopupType
            // 
            this.pnlTerminalPopupType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.pnlTerminalPopupType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTerminalPopupType.Location = new System.Drawing.Point(0, 0);
            this.pnlTerminalPopupType.Name = "pnlTerminalPopupType";
            this.pnlTerminalPopupType.Size = new System.Drawing.Size(363, 378);
            this.pnlTerminalPopupType.TabIndex = 5;
            // 
            // pnlSubPanl
            // 
            this.pnlSubPanl.Controls.Add(this.pnlGeneral);
            this.pnlSubPanl.Controls.Add(this.pnlHighlightColor);
            this.pnlSubPanl.Controls.Add(this.pnlTerminalLayout);
            this.pnlSubPanl.Controls.Add(this.pnlTerminalColor);
            this.pnlSubPanl.Controls.Add(this.pnlTerminalPopupType);
            this.pnlSubPanl.Controls.Add(this.pnlTelnet);
            this.pnlSubPanl.Controls.Add(this.pnlSerialPort);
            this.pnlSubPanl.Controls.Add(this.pnlConnectOption);
            this.pnlSubPanl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSubPanl.Location = new System.Drawing.Point(168, 10);
            this.pnlSubPanl.Name = "pnlSubPanl";
            this.pnlSubPanl.Size = new System.Drawing.Size(363, 378);
            this.pnlSubPanl.TabIndex = 6;
            // 
            // pnlGeneral
            // 
            this.pnlGeneral.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.pnlGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGeneral.Location = new System.Drawing.Point(0, 0);
            this.pnlGeneral.Name = "pnlGeneral";
            this.pnlGeneral.Size = new System.Drawing.Size(363, 378);
            this.pnlGeneral.TabIndex = 8;
            // 
            // pnlHighlightColor
            // 
            this.pnlHighlightColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.pnlHighlightColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHighlightColor.Location = new System.Drawing.Point(0, 0);
            this.pnlHighlightColor.Name = "pnlHighlightColor";
            this.pnlHighlightColor.Size = new System.Drawing.Size(363, 378);
            this.pnlHighlightColor.TabIndex = 7;
            // 
            // pnlTerminalLayout
            // 
            this.pnlTerminalLayout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.pnlTerminalLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTerminalLayout.Location = new System.Drawing.Point(0, 0);
            this.pnlTerminalLayout.Name = "pnlTerminalLayout";
            this.pnlTerminalLayout.Size = new System.Drawing.Size(363, 378);
            this.pnlTerminalLayout.TabIndex = 7;
            // 
            // pnlTerminalColor
            // 
            this.pnlTerminalColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.pnlTerminalColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTerminalColor.Location = new System.Drawing.Point(0, 0);
            this.pnlTerminalColor.Name = "pnlTerminalColor";
            this.pnlTerminalColor.Size = new System.Drawing.Size(363, 378);
            this.pnlTerminalColor.TabIndex = 7;
            // 
            // OptionConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.ClientSize = new System.Drawing.Size(541, 437);
            this.Controls.Add(this.pnlSubPanl);
            this.Controls.Add(this.trvOptionConfiguration);
            this.Name = "OptionConfigurationForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "옵션 설정";
            this.Controls.SetChildIndex(this.trvOptionConfiguration, 0);
            this.Controls.SetChildIndex(this.pnlSubPanl, 0);
            this.pnlSubPanl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ucConnectOptionPanel pnlConnectOption;
        private System.Windows.Forms.TreeView trvOptionConfiguration;
        private ucTelnetPanel pnlTelnet;
        private ucSerialPortPanel pnlSerialPort;
        private ucTerminalPopupType pnlTerminalPopupType;
        private System.Windows.Forms.Panel pnlSubPanl;
        private ucTerminalColor pnlTerminalColor;
        private ucGeneral pnlGeneral;
        private System.Windows.Forms.ImageList imageList1;
        private ucTerminalLayout pnlTerminalLayout;
        private ucHighlightColor pnlHighlightColor;
    }
}
