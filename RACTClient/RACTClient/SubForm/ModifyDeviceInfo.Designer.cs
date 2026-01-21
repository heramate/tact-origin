namespace RACTClient
{
    partial class ModifyDeviceInfo
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
            this.txtDisplayName = new MKLibrary.Controls.MKTextBox();
            this.txtModelName = new MKLibrary.Controls.MKTextBox();
            this.btnDeviceSearch = new MKLibrary.Controls.MKButton(this.components);
            this.mkLabel1 = new MKLibrary.Controls.MKLabel();
            this.mkLabel2 = new MKLibrary.Controls.MKLabel();
            this.mkLabel5 = new MKLibrary.Controls.MKLabel();
            this.mkLabel6 = new MKLibrary.Controls.MKLabel();
            this.mkLabel7 = new MKLibrary.Controls.MKLabel();
            this.cboDeviceGroup = new MKLibrary.Controls.MKComboBox();
            this.ipDevice = new MKLibrary.Controls.MKIPAddress(this.components);
            this.superTabControl1 = new DevComponents.DotNetBar.SuperTabControl();
            this.superTabControlPanel2 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.pnlSerialOption = new RACTClient.ucSerialConnectOption();
            this.mkLabel3 = new MKLibrary.Controls.MKLabel();
            this.nudPort = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.cboProtocol = new MKLibrary.Controls.MKComboBox();
            this.superTabItem2 = new DevComponents.DotNetBar.SuperTabItem();
            this.superTabControlPanel1 = new DevComponents.DotNetBar.SuperTabControlPanel();
            this.txtLocation = new System.Windows.Forms.RichTextBox();
            this.superTabItem1 = new DevComponents.DotNetBar.SuperTabItem();
            ((System.ComponentModel.ISupportInitialize)(this.superTabControl1)).BeginInit();
            this.superTabControl1.SuspendLayout();
            this.superTabControlPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).BeginInit();
            this.superTabControlPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtDisplayName
            // 
            this.txtDisplayName.BackColorPattern = System.Drawing.Color.White;
            this.txtDisplayName.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.txtDisplayName.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtDisplayName.BorderEdgeRadius = 3;
            this.txtDisplayName.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.txtDisplayName.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.txtDisplayName.isLockHeight = true;
            this.txtDisplayName.Lines = new string[0];
            this.txtDisplayName.Location = new System.Drawing.Point(83, 65);
            this.txtDisplayName.MaxLength = 2147483647;
            this.txtDisplayName.Name = "txtDisplayName";
            this.txtDisplayName.PasswordChar = '\0';
            this.txtDisplayName.ReadOnly = true;
            this.txtDisplayName.Size = new System.Drawing.Size(217, 21);
            this.txtDisplayName.TabIndex = 14;
            // 
            // txtModelName
            // 
            this.txtModelName.BackColorPattern = System.Drawing.Color.White;
            this.txtModelName.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.txtModelName.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.txtModelName.BorderEdgeRadius = 3;
            this.txtModelName.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.txtModelName.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.txtModelName.isLockHeight = true;
            this.txtModelName.Lines = new string[0];
            this.txtModelName.Location = new System.Drawing.Point(83, 39);
            this.txtModelName.MaxLength = 2147483647;
            this.txtModelName.Name = "txtModelName";
            this.txtModelName.PasswordChar = '\0';
            this.txtModelName.ReadOnly = true;
            this.txtModelName.Size = new System.Drawing.Size(217, 21);
            this.txtModelName.TabIndex = 14;
            // 
            // btnDeviceSearch
            // 
            this.btnDeviceSearch.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnDeviceSearch.BackgroundImageDisable = null;
            this.btnDeviceSearch.BackgroundImageHover = null;
            this.btnDeviceSearch.BackgroundImageNormal = null;
            this.btnDeviceSearch.BackgroundImageSelect = null;
            this.btnDeviceSearch.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnDeviceSearch.BorderEdgeRadius = 3;
            this.btnDeviceSearch.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnDeviceSearch.ButtonImageCenter = null;
            this.btnDeviceSearch.ButtonImageLeft = null;
            this.btnDeviceSearch.ButtonImageRight = null;
            this.btnDeviceSearch.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnDeviceSearch, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnDeviceSearch.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnDeviceSearch.ColorDepthFocus = 2;
            this.btnDeviceSearch.ColorDepthHover = 2;
            this.btnDeviceSearch.ColorDepthShadow = 2;
            this.btnDeviceSearch.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnDeviceSearch.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnDeviceSearch.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnDeviceSearch.IconIndexDisable = -1;
            this.btnDeviceSearch.IconIndexHover = -1;
            this.btnDeviceSearch.IconIndexNormal = -1;
            this.btnDeviceSearch.IconIndexSelect = -1;
            this.btnDeviceSearch.Image = null;
            this.btnDeviceSearch.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnDeviceSearch.ImageIndent = 0;
            this.btnDeviceSearch.ImageList = null;
            this.btnDeviceSearch.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnDeviceSearch.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnDeviceSearch.Location = new System.Drawing.Point(240, 11);
            this.btnDeviceSearch.Name = "btnDeviceSearch";
            this.btnDeviceSearch.Size = new System.Drawing.Size(60, 23);
            this.btnDeviceSearch.TabIndex = 9;
            this.btnDeviceSearch.Text = "장비검색";
            this.btnDeviceSearch.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnDeviceSearch.TextIndent = 0;
            this.btnDeviceSearch.Click += new System.EventHandler(this.btnDeviceSearch_Click);
            // 
            // mkLabel1
            // 
            this.mkLabel1.BackColor = System.Drawing.Color.Transparent;
            this.mkLabel1.BackColorPattern = System.Drawing.Color.White;
            this.mkLabel1.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.mkLabel1.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.mkLabel1.BorderEdgeRadius = 3;
            this.mkLabel1.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.mkLabel1.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.None;
            this.mkLabel1.CaptionLabel = false;
            this.mkLabel1.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.mkLabel1.Image = null;
            this.mkLabel1.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.mkLabel1.ImageIndent = 0;
            this.mkLabel1.ImageIndex = -1;
            this.mkLabel1.ImageList = null;
            this.mkLabel1.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.mkLabel1.InternalGap = new MKLibrary.MKObject.MKRectangleGap(0, 0, 0, 0);
            this.mkLabel1.Location = new System.Drawing.Point(13, 12);
            this.mkLabel1.Name = "mkLabel1";
            this.mkLabel1.Size = new System.Drawing.Size(69, 21);
            this.mkLabel1.TabIndex = 16;
            this.mkLabel1.Text = "장비 IP :";
            this.mkLabel1.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.mkLabel1.TextIndent = 0;
            // 
            // mkLabel2
            // 
            this.mkLabel2.BackColor = System.Drawing.Color.Transparent;
            this.mkLabel2.BackColorPattern = System.Drawing.Color.White;
            this.mkLabel2.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.mkLabel2.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.mkLabel2.BorderEdgeRadius = 3;
            this.mkLabel2.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.mkLabel2.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.None;
            this.mkLabel2.CaptionLabel = false;
            this.mkLabel2.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.mkLabel2.Image = null;
            this.mkLabel2.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.mkLabel2.ImageIndent = 0;
            this.mkLabel2.ImageIndex = -1;
            this.mkLabel2.ImageList = null;
            this.mkLabel2.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.mkLabel2.InternalGap = new MKLibrary.MKObject.MKRectangleGap(0, 0, 0, 0);
            this.mkLabel2.Location = new System.Drawing.Point(13, 97);
            this.mkLabel2.Name = "mkLabel2";
            this.mkLabel2.Size = new System.Drawing.Size(69, 21);
            this.mkLabel2.TabIndex = 17;
            this.mkLabel2.Text = "장비그룹 :";
            this.mkLabel2.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.mkLabel2.TextIndent = 0;
            // 
            // mkLabel5
            // 
            this.mkLabel5.BackColor = System.Drawing.Color.Transparent;
            this.mkLabel5.BackColorPattern = System.Drawing.Color.White;
            this.mkLabel5.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.mkLabel5.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.mkLabel5.BorderEdgeRadius = 3;
            this.mkLabel5.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.mkLabel5.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.None;
            this.mkLabel5.CaptionLabel = false;
            this.mkLabel5.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.mkLabel5.Image = null;
            this.mkLabel5.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.mkLabel5.ImageIndent = 0;
            this.mkLabel5.ImageIndex = -1;
            this.mkLabel5.ImageList = null;
            this.mkLabel5.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.mkLabel5.InternalGap = new MKLibrary.MKObject.MKRectangleGap(0, 0, 0, 0);
            this.mkLabel5.Location = new System.Drawing.Point(13, 39);
            this.mkLabel5.Name = "mkLabel5";
            this.mkLabel5.Size = new System.Drawing.Size(69, 21);
            this.mkLabel5.TabIndex = 20;
            this.mkLabel5.Text = "모델명 :";
            this.mkLabel5.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.mkLabel5.TextIndent = 0;
            // 
            // mkLabel6
            // 
            this.mkLabel6.BackColor = System.Drawing.Color.Transparent;
            this.mkLabel6.BackColorPattern = System.Drawing.Color.White;
            this.mkLabel6.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.mkLabel6.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.mkLabel6.BorderEdgeRadius = 3;
            this.mkLabel6.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.mkLabel6.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.None;
            this.mkLabel6.CaptionLabel = false;
            this.mkLabel6.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.mkLabel6.Image = null;
            this.mkLabel6.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.mkLabel6.ImageIndent = 0;
            this.mkLabel6.ImageIndex = -1;
            this.mkLabel6.ImageList = null;
            this.mkLabel6.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.mkLabel6.InternalGap = new MKLibrary.MKObject.MKRectangleGap(0, 0, 0, 0);
            this.mkLabel6.Location = new System.Drawing.Point(13, 66);
            this.mkLabel6.Name = "mkLabel6";
            this.mkLabel6.Size = new System.Drawing.Size(69, 21);
            this.mkLabel6.TabIndex = 17;
            this.mkLabel6.Text = "표시이름 :";
            this.mkLabel6.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.mkLabel6.TextIndent = 0;
            // 
            // mkLabel7
            // 
            this.mkLabel7.BackColor = System.Drawing.Color.Transparent;
            this.mkLabel7.BackColorPattern = System.Drawing.Color.White;
            this.mkLabel7.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.mkLabel7.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.mkLabel7.BorderEdgeRadius = 3;
            this.mkLabel7.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.mkLabel7.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.None;
            this.mkLabel7.CaptionLabel = false;
            this.mkLabel7.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.mkLabel7.Image = null;
            this.mkLabel7.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.mkLabel7.ImageIndent = 0;
            this.mkLabel7.ImageIndex = -1;
            this.mkLabel7.ImageList = null;
            this.mkLabel7.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.mkLabel7.InternalGap = new MKLibrary.MKObject.MKRectangleGap(0, 0, 0, 0);
            this.mkLabel7.Location = new System.Drawing.Point(13, 128);
            this.mkLabel7.Name = "mkLabel7";
            this.mkLabel7.Size = new System.Drawing.Size(69, 21);
            this.mkLabel7.TabIndex = 20;
            this.mkLabel7.Text = "위치 :";
            this.mkLabel7.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.mkLabel7.TextIndent = 0;
            // 
            // cboDeviceGroup
            // 
            this.cboDeviceGroup.BackColor = System.Drawing.SystemColors.Control;
            this.cboDeviceGroup.BackColorSelected = System.Drawing.Color.Orange;
            this.cboDeviceGroup.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.cboDeviceGroup.BorderEdgeRadius = 3;
            this.cboDeviceGroup.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.cboDeviceGroup.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.None;
            this.cboDeviceGroup.BoxBorderColor = System.Drawing.Color.Gray;
            this.cboDeviceGroup.ColorBoxSize = new System.Drawing.Size(12, 8);
            this.cboDeviceGroup.ComboBoxStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cboDeviceGroup.ImageList = null;
            this.cboDeviceGroup.ItemHeight = 14;
            this.cboDeviceGroup.Location = new System.Drawing.Point(83, 96);
            this.cboDeviceGroup.MaxDorpDownWidth = 500;
            this.cboDeviceGroup.Name = "cboDeviceGroup";
            this.cboDeviceGroup.SelectedIndex = -1;
            this.cboDeviceGroup.ShowColorBox = false;
            this.cboDeviceGroup.Size = new System.Drawing.Size(217, 21);
            this.cboDeviceGroup.TabIndex = 24;
            // 
            // ipDevice
            // 
            this.ipDevice.BackColor = System.Drawing.Color.White;
            this.ipDevice.BackColorPattern = System.Drawing.Color.White;
            this.ipDevice.BackGroundStyle = MKLibrary.MKDrawing.E_BackgroundStyle.Color;
            this.ipDevice.BorderColor = System.Drawing.Color.DimGray;
            this.ipDevice.BorderEdgeRadius = 3;
            this.ipDevice.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.ipDevice.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.Fixed;
            this.ipDevice.Enabled = false;
            this.ipDevice.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.ipDevice.Location = new System.Drawing.Point(83, 12);
            this.ipDevice.Name = "ipDevice";
            this.ipDevice.ParentIPControl = null;
            this.ipDevice.Size = new System.Drawing.Size(151, 20);
            this.ipDevice.TabIndex = 21;
            // 
            // superTabControl1
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.superTabControl1.ControlBox.CloseBox.Name = "";
            // 
            // 
            // 
            this.superTabControl1.ControlBox.MenuBox.Name = "";
            this.superTabControl1.ControlBox.Name = "";
            this.superTabControl1.ControlBox.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.superTabControl1.ControlBox.MenuBox,
            this.superTabControl1.ControlBox.CloseBox});
            this.superTabControl1.Controls.Add(this.superTabControlPanel2);
            this.superTabControl1.Controls.Add(this.superTabControlPanel1);
            this.superTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControl1.Location = new System.Drawing.Point(5, 5);
            this.superTabControl1.Name = "superTabControl1";
            this.superTabControl1.ReorderTabsEnabled = true;
            this.superTabControl1.SelectedTabFont = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold);
            this.superTabControl1.SelectedTabIndex = 0;
            this.superTabControl1.Size = new System.Drawing.Size(325, 239);
            this.superTabControl1.TabFont = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.superTabControl1.TabIndex = 22;
            this.superTabControl1.Tabs.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.superTabItem1,
            this.superTabItem2});
            this.superTabControl1.Text = "superTabControl1";
            // 
            // superTabControlPanel2
            // 
            this.superTabControlPanel2.Controls.Add(this.pnlSerialOption);
            this.superTabControlPanel2.Controls.Add(this.mkLabel3);
            this.superTabControlPanel2.Controls.Add(this.nudPort);
            this.superTabControlPanel2.Controls.Add(this.label1);
            this.superTabControlPanel2.Controls.Add(this.cboProtocol);
            this.superTabControlPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel2.Location = new System.Drawing.Point(0, 28);
            this.superTabControlPanel2.Name = "superTabControlPanel2";
            this.superTabControlPanel2.Size = new System.Drawing.Size(325, 211);
            this.superTabControlPanel2.TabIndex = 0;
            this.superTabControlPanel2.TabItem = this.superTabItem2;
            // 
            // pnlSerialOption
            // 
            this.pnlSerialOption.BackColor = System.Drawing.Color.Transparent;
            this.pnlSerialOption.Location = new System.Drawing.Point(20, 37);
            this.pnlSerialOption.Name = "pnlSerialOption";
            this.pnlSerialOption.Size = new System.Drawing.Size(288, 137);
            this.pnlSerialOption.TabIndex = 188;
            this.pnlSerialOption.Load += new System.EventHandler(this.pnlSerialOption_Load);
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
            this.mkLabel3.Location = new System.Drawing.Point(21, 42);
            this.mkLabel3.Name = "mkLabel3";
            this.mkLabel3.Size = new System.Drawing.Size(69, 21);
            this.mkLabel3.TabIndex = 190;
            this.mkLabel3.Text = "Port :";
            this.mkLabel3.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.mkLabel3.TextIndent = 0;
            // 
            // nudPort
            // 
            this.nudPort.Location = new System.Drawing.Point(113, 42);
            this.nudPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudPort.Name = "nudPort";
            this.nudPort.Size = new System.Drawing.Size(151, 21);
            this.nudPort.TabIndex = 189;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(21, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 12);
            this.label1.TabIndex = 187;
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
            this.cboProtocol.Location = new System.Drawing.Point(113, 12);
            this.cboProtocol.MaxDorpDownWidth = 500;
            this.cboProtocol.Name = "cboProtocol";
            this.cboProtocol.SelectedIndex = -1;
            this.cboProtocol.ShowColorBox = false;
            this.cboProtocol.Size = new System.Drawing.Size(151, 21);
            this.cboProtocol.TabIndex = 185;
            this.cboProtocol.SelectedIndexChanged += new System.EventHandler(this.cboProtocol_SelectedIndexChanged);
            // 
            // superTabItem2
            // 
            this.superTabItem2.AttachedControl = this.superTabControlPanel2;
            this.superTabItem2.GlobalItem = false;
            this.superTabItem2.Name = "superTabItem2";
            this.superTabItem2.Text = "접속정보";
            // 
            // superTabControlPanel1
            // 
            this.superTabControlPanel1.Controls.Add(this.txtLocation);
            this.superTabControlPanel1.Controls.Add(this.mkLabel1);
            this.superTabControlPanel1.Controls.Add(this.mkLabel6);
            this.superTabControlPanel1.Controls.Add(this.cboDeviceGroup);
            this.superTabControlPanel1.Controls.Add(this.ipDevice);
            this.superTabControlPanel1.Controls.Add(this.mkLabel7);
            this.superTabControlPanel1.Controls.Add(this.txtDisplayName);
            this.superTabControlPanel1.Controls.Add(this.mkLabel2);
            this.superTabControlPanel1.Controls.Add(this.mkLabel5);
            this.superTabControlPanel1.Controls.Add(this.txtModelName);
            this.superTabControlPanel1.Controls.Add(this.btnDeviceSearch);
            this.superTabControlPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.superTabControlPanel1.Location = new System.Drawing.Point(0, 28);
            this.superTabControlPanel1.Name = "superTabControlPanel1";
            this.superTabControlPanel1.Size = new System.Drawing.Size(325, 211);
            this.superTabControlPanel1.TabIndex = 1;
            this.superTabControlPanel1.TabItem = this.superTabItem1;
            // 
            // txtLocation
            // 
            this.txtLocation.Location = new System.Drawing.Point(83, 128);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(217, 72);
            this.txtLocation.TabIndex = 25;
            this.txtLocation.Text = "";
            // 
            // superTabItem1
            // 
            this.superTabItem1.AttachedControl = this.superTabControlPanel1;
            this.superTabItem1.GlobalItem = false;
            this.superTabItem1.Name = "superTabItem1";
            this.superTabItem1.Text = "장비 정보";
            // 
            // ModifyDeviceInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 288);
            this.Controls.Add(this.superTabControl1);
            this.DoubleBuffered = true;
            this.Name = "ModifyDeviceInfo";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "장비 관리";
            this.Controls.SetChildIndex(this.superTabControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.superTabControl1)).EndInit();
            this.superTabControl1.ResumeLayout(false);
            this.superTabControlPanel2.ResumeLayout(false);
            this.superTabControlPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).EndInit();
            this.superTabControlPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private MKLibrary.Controls.MKTextBox txtDisplayName;
        private MKLibrary.Controls.MKTextBox txtModelName;
        private MKLibrary.Controls.MKButton btnDeviceSearch;
        private MKLibrary.Controls.MKLabel mkLabel1;
        private MKLibrary.Controls.MKLabel mkLabel2;
        private MKLibrary.Controls.MKLabel mkLabel5;
        private MKLibrary.Controls.MKLabel mkLabel6;
        private MKLibrary.Controls.MKLabel mkLabel7;
        private MKLibrary.Controls.MKIPAddress ipDevice;
        private MKLibrary.Controls.MKComboBox cboDeviceGroup;
        private DevComponents.DotNetBar.SuperTabControl superTabControl1;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel2;
        private DevComponents.DotNetBar.SuperTabItem superTabItem2;
        private DevComponents.DotNetBar.SuperTabControlPanel superTabControlPanel1;
        private DevComponents.DotNetBar.SuperTabItem superTabItem1;
        private MKLibrary.Controls.MKLabel mkLabel3;
        private System.Windows.Forms.NumericUpDown nudPort;
        private ucSerialConnectOption pnlSerialOption;
        private System.Windows.Forms.Label label1;
        private MKLibrary.Controls.MKComboBox cboProtocol;
        private System.Windows.Forms.RichTextBox txtLocation;
    }
}