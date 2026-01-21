namespace RACTClient
{
    partial class ucDeviceSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucDeviceSearch));
            this.txtIPAddress = new MKLibrary.Controls.MKIPAddress(this.components);
            this.cboDevicePart = new MKLibrary.Controls.MKComboBox();
            this.cboDeviceModel = new MKLibrary.Controls.MKComboBox();
            this.btnDeviceSearch = new MKLibrary.Controls.MKButton(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.fgDeviceList = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.txtDeviceName = new System.Windows.Forms.TextBox();
            this.bar1 = new DevComponents.DotNetBar.Bar();
            this.ctmPopup = new DevComponents.DotNetBar.ButtonItem();
            this.mnuQuickConnect = new DevComponents.DotNetBar.ButtonItem();
            this.btnSearchIPList = new MKLibrary.Controls.MKButton(this.components);
            this.lblArea = new System.Windows.Forms.Label();
            this.lblSelectDeviceCount = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cboIPType = new MKLibrary.Controls.MKComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.fgDeviceList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).BeginInit();
            this.SuspendLayout();
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
            this.txtIPAddress.Location = new System.Drawing.Point(71, 44);
            this.txtIPAddress.Name = "txtIPAddress";
            this.txtIPAddress.ParentIPControl = null;
            this.txtIPAddress.Size = new System.Drawing.Size(151, 20);
            this.txtIPAddress.TabIndex = 16;
            // 
            // cboDevicePart
            // 
            this.cboDevicePart.BackColor = System.Drawing.SystemColors.Window;
            this.cboDevicePart.BackColorSelected = System.Drawing.Color.Orange;
            this.cboDevicePart.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(201)))), ((int)(((byte)(185)))));
            this.cboDevicePart.BorderEdgeRadius = 3;
            this.cboDevicePart.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.cboDevicePart.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.Fixed;
            this.cboDevicePart.BoxBorderColor = System.Drawing.Color.Gray;
            this.cboDevicePart.ColorBoxSize = new System.Drawing.Size(12, 8);
            this.cboDevicePart.ComboBoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDevicePart.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboDevicePart.ForeColor = System.Drawing.Color.Black;
            this.cboDevicePart.ImageList = null;
            this.cboDevicePart.ItemHeight = 14;
            this.cboDevicePart.Location = new System.Drawing.Point(311, 16);
            this.cboDevicePart.MaxDorpDownWidth = 500;
            this.cboDevicePart.Name = "cboDevicePart";
            this.cboDevicePart.SelectedIndex = -1;
            this.cboDevicePart.ShowColorBox = false;
            this.cboDevicePart.Size = new System.Drawing.Size(151, 21);
            this.cboDevicePart.TabIndex = 170;
            this.cboDevicePart.SelectedIndexChanged += new System.EventHandler(this.cboDevicePart_SelectedIndexChanged);
            // 
            // cboDeviceModel
            // 
            this.cboDeviceModel.BackColor = System.Drawing.SystemColors.Window;
            this.cboDeviceModel.BackColorSelected = System.Drawing.Color.Orange;
            this.cboDeviceModel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(201)))), ((int)(((byte)(185)))));
            this.cboDeviceModel.BorderEdgeRadius = 3;
            this.cboDeviceModel.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.cboDeviceModel.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.Fixed;
            this.cboDeviceModel.BoxBorderColor = System.Drawing.Color.Gray;
            this.cboDeviceModel.ColorBoxSize = new System.Drawing.Size(12, 8);
            this.cboDeviceModel.ComboBoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDeviceModel.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboDeviceModel.ForeColor = System.Drawing.Color.Black;
            this.cboDeviceModel.ImageList = null;
            this.cboDeviceModel.ItemHeight = 14;
            this.cboDeviceModel.Location = new System.Drawing.Point(568, 16);
            this.cboDeviceModel.MaxDorpDownWidth = 500;
            this.cboDeviceModel.Name = "cboDeviceModel";
            this.cboDeviceModel.SelectedIndex = -1;
            this.cboDeviceModel.ShowColorBox = false;
            this.cboDeviceModel.Size = new System.Drawing.Size(146, 21);
            this.cboDeviceModel.TabIndex = 170;
            this.cboDeviceModel.SelectedIndexChanged += new System.EventHandler(this.cboDeviceModel_SelectedIndexChanged);
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
            this.btnDeviceSearch.Location = new System.Drawing.Point(496, 44);
            this.btnDeviceSearch.Name = "btnDeviceSearch";
            this.btnDeviceSearch.Size = new System.Drawing.Size(75, 23);
            this.btnDeviceSearch.TabIndex = 175;
            this.btnDeviceSearch.Text = "장비 검색";
            this.btnDeviceSearch.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnDeviceSearch.TextIndent = 0;
            this.btnDeviceSearch.Click += new System.EventHandler(this.btnDeviceSearch_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(245, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 176;
            this.label1.Text = "장비 분류 :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(485, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 176;
            this.label2.Text = "장비 모델 :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 12);
            this.label3.TabIndex = 176;
            this.label3.Text = "장비 IP :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(245, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 12);
            this.label4.TabIndex = 176;
            this.label4.Text = "장비명(TID) :";
            // 
            // fgDeviceList
            // 
            this.fgDeviceList.AllowEditing = false;
            this.fgDeviceList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fgDeviceList.BackColor = System.Drawing.SystemColors.Window;
            this.fgDeviceList.ColumnInfo = resources.GetString("fgDeviceList.ColumnInfo");
            this.fgDeviceList.ForeColor = System.Drawing.SystemColors.WindowText;
            this.fgDeviceList.Location = new System.Drawing.Point(3, 75);
            this.fgDeviceList.Name = "fgDeviceList";
            this.fgDeviceList.Rows.Count = 1;
            this.fgDeviceList.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
            this.fgDeviceList.Size = new System.Drawing.Size(908, 499);
            this.fgDeviceList.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("fgDeviceList.Styles"));
            this.fgDeviceList.TabIndex = 0;
            this.fgDeviceList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.fgDeviceList_MouseDown);
            // 
            // txtDeviceName
            // 
            this.txtDeviceName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtDeviceName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtDeviceName.Location = new System.Drawing.Point(329, 43);
            this.txtDeviceName.Name = "txtDeviceName";
            this.txtDeviceName.Size = new System.Drawing.Size(145, 21);
            this.txtDeviceName.TabIndex = 177;
            // 
            // bar1
            // 
            this.bar1.AntiAlias = true;
            this.bar1.BarType = DevComponents.DotNetBar.eBarType.MenuBar;
            this.bar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.ctmPopup});
            this.bar1.Location = new System.Drawing.Point(426, 263);
            this.bar1.Name = "bar1";
            this.bar1.Size = new System.Drawing.Size(75, 25);
            this.bar1.Stretch = true;
            this.bar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.bar1.TabIndex = 178;
            this.bar1.TabStop = false;
            this.bar1.Text = "bar1";
            // 
            // ctmPopup
            // 
            this.ctmPopup.Name = "ctmPopup";
            this.ctmPopup.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.mnuQuickConnect});
            this.ctmPopup.Text = "buttonItem1";
            this.ctmPopup.Visible = false;
            // 
            // mnuQuickConnect
            // 
            this.mnuQuickConnect.Name = "mnuQuickConnect";
            this.mnuQuickConnect.Text = "빠른 연결";
            this.mnuQuickConnect.Click += new System.EventHandler(this.mnuQuickConnect_Click);
            // 
            // btnSearchIPList
            // 
            this.btnSearchIPList.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnSearchIPList.BackgroundImageDisable = null;
            this.btnSearchIPList.BackgroundImageHover = null;
            this.btnSearchIPList.BackgroundImageNormal = null;
            this.btnSearchIPList.BackgroundImageSelect = null;
            this.btnSearchIPList.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnSearchIPList.BorderEdgeRadius = 3;
            this.btnSearchIPList.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnSearchIPList.ButtonImageCenter = null;
            this.btnSearchIPList.ButtonImageLeft = null;
            this.btnSearchIPList.ButtonImageRight = null;
            this.btnSearchIPList.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnSearchIPList, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnSearchIPList.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnSearchIPList.ColorDepthFocus = 2;
            this.btnSearchIPList.ColorDepthHover = 2;
            this.btnSearchIPList.ColorDepthShadow = 2;
            this.btnSearchIPList.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnSearchIPList.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnSearchIPList.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnSearchIPList.IconIndexDisable = -1;
            this.btnSearchIPList.IconIndexHover = -1;
            this.btnSearchIPList.IconIndexNormal = -1;
            this.btnSearchIPList.IconIndexSelect = -1;
            this.btnSearchIPList.Image = null;
            this.btnSearchIPList.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnSearchIPList.ImageIndent = 0;
            this.btnSearchIPList.ImageList = null;
            this.btnSearchIPList.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnSearchIPList.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnSearchIPList.Location = new System.Drawing.Point(704, 44);
            this.btnSearchIPList.Name = "btnSearchIPList";
            this.btnSearchIPList.Size = new System.Drawing.Size(123, 23);
            this.btnSearchIPList.TabIndex = 179;
            this.btnSearchIPList.Text = "아이피 일괄 검색";
            this.btnSearchIPList.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnSearchIPList.TextIndent = 0;
            this.btnSearchIPList.Click += new System.EventHandler(this.btnSearchIPList_Click);
            // 
            // lblArea
            // 
            this.lblArea.AutoSize = true;
            this.lblArea.Location = new System.Drawing.Point(736, 20);
            this.lblArea.Name = "lblArea";
            this.lblArea.Size = new System.Drawing.Size(65, 12);
            this.lblArea.TabIndex = 180;
            this.lblArea.Text = "검색지역 : ";
            // 
            // lblSelectDeviceCount
            // 
            this.lblSelectDeviceCount.AutoSize = true;
            this.lblSelectDeviceCount.Location = new System.Drawing.Point(577, 48);
            this.lblSelectDeviceCount.Name = "lblSelectDeviceCount";
            this.lblSelectDeviceCount.Size = new System.Drawing.Size(93, 12);
            this.lblSelectDeviceCount.TabIndex = 181;
            this.lblSelectDeviceCount.Text = "조회 장비대수 : ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 12);
            this.label5.TabIndex = 183;
            this.label5.Text = "IP 타입 :";
            // 
            // cboIPType
            // 
            this.cboIPType.BackColor = System.Drawing.SystemColors.Window;
            this.cboIPType.BackColorSelected = System.Drawing.Color.Orange;
            this.cboIPType.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(201)))), ((int)(((byte)(185)))));
            this.cboIPType.BorderEdgeRadius = 3;
            this.cboIPType.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.cboIPType.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.Fixed;
            this.cboIPType.BoxBorderColor = System.Drawing.Color.Gray;
            this.cboIPType.ColorBoxSize = new System.Drawing.Size(12, 8);
            this.cboIPType.ComboBoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboIPType.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboIPType.ForeColor = System.Drawing.Color.Black;
            this.cboIPType.ImageList = null;
            this.cboIPType.ItemHeight = 14;
            this.cboIPType.Location = new System.Drawing.Point(71, 16);
            this.cboIPType.MaxDorpDownWidth = 500;
            this.cboIPType.Name = "cboIPType";
            this.cboIPType.SelectedIndex = -1;
            this.cboIPType.ShowColorBox = false;
            this.cboIPType.Size = new System.Drawing.Size(151, 21);
            this.cboIPType.TabIndex = 182;
            this.cboIPType.SelectedIndexChanged += new System.EventHandler(this.cboIPType_SelectedIndexChanged);
            // 
            // ucDeviceSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cboIPType);
            this.Controls.Add(this.lblSelectDeviceCount);
            this.Controls.Add(this.lblArea);
            this.Controls.Add(this.btnSearchIPList);
            this.Controls.Add(this.fgDeviceList);
            this.Controls.Add(this.bar1);
            this.Controls.Add(this.txtDeviceName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDeviceSearch);
            this.Controls.Add(this.cboDeviceModel);
            this.Controls.Add(this.cboDevicePart);
            this.Controls.Add(this.txtIPAddress);
            this.Name = "ucDeviceSearch";
            this.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.Size = new System.Drawing.Size(914, 571);
            ((System.ComponentModel.ISupportInitialize)(this.fgDeviceList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MKLibrary.Controls.MKIPAddress txtIPAddress;
        private MKLibrary.Controls.MKComboBox cboDevicePart;
        private MKLibrary.Controls.MKComboBox cboDeviceModel;
        private MKLibrary.Controls.MKButton btnDeviceSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        // 2013-04-22- shinyn- 더블클릭시 추가되도록 하는 기능 추가
        public C1.Win.C1FlexGrid.C1FlexGrid fgDeviceList;
        private System.Windows.Forms.TextBox txtDeviceName;
        private DevComponents.DotNetBar.Bar bar1;
        private DevComponents.DotNetBar.ButtonItem ctmPopup;
        private DevComponents.DotNetBar.ButtonItem mnuQuickConnect;
        private MKLibrary.Controls.MKButton btnSearchIPList;
        private System.Windows.Forms.Label lblArea;
        private System.Windows.Forms.Label lblSelectDeviceCount;
        private System.Windows.Forms.Label label5;
        private MKLibrary.Controls.MKComboBox cboIPType;
    }
}
