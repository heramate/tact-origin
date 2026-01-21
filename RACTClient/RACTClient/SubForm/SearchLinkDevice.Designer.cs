namespace RACTClient
{
    partial class SearchLinkDevice
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchLinkDevice));
            this.fgDeviceList = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.label3 = new System.Windows.Forms.Label();
            this.btnDeviceSearch = new MKLibrary.Controls.MKButton(this.components);
            this.txtIPAddress = new MKLibrary.Controls.MKIPAddress(this.components);
            this.btnOK = new MKLibrary.Controls.MKButton(this.components);
            this.btnCancel = new MKLibrary.Controls.MKButton(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoUserNeGroup = new System.Windows.Forms.RadioButton();
            this.rdoNEGroup = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.fgDeviceList)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
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
            this.fgDeviceList.Location = new System.Drawing.Point(12, 56);
            this.fgDeviceList.Name = "fgDeviceList";
            this.fgDeviceList.Rows.Count = 1;
            this.fgDeviceList.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row;
            this.fgDeviceList.Size = new System.Drawing.Size(541, 142);
            this.fgDeviceList.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("fgDeviceList.Styles"));
            this.fgDeviceList.TabIndex = 177;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(242, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 12);
            this.label3.TabIndex = 180;
            this.label3.Text = "장비 IP :";
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
            this.btnDeviceSearch.Location = new System.Drawing.Point(478, 27);
            this.btnDeviceSearch.Name = "btnDeviceSearch";
            this.btnDeviceSearch.Size = new System.Drawing.Size(75, 23);
            this.btnDeviceSearch.TabIndex = 179;
            this.btnDeviceSearch.Text = "장비 검색";
            this.btnDeviceSearch.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnDeviceSearch.TextIndent = 0;
            this.btnDeviceSearch.Click += new System.EventHandler(this.btnDeviceSearch_Click);
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
            this.txtIPAddress.Location = new System.Drawing.Point(308, 28);
            this.txtIPAddress.Name = "txtIPAddress";
            this.txtIPAddress.ParentIPControl = null;
            this.txtIPAddress.Size = new System.Drawing.Size(151, 20);
            this.txtIPAddress.TabIndex = 178;
            // 
            // btnOK
            // 
            this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnOK.BackgroundImageDisable = null;
            this.btnOK.BackgroundImageHover = null;
            this.btnOK.BackgroundImageNormal = null;
            this.btnOK.BackgroundImageSelect = null;
            this.btnOK.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnOK.BorderEdgeRadius = 3;
            this.btnOK.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnOK.ButtonImageCenter = null;
            this.btnOK.ButtonImageLeft = null;
            this.btnOK.ButtonImageRight = null;
            this.btnOK.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnOK, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnOK.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnOK.ColorDepthFocus = 2;
            this.btnOK.ColorDepthHover = 2;
            this.btnOK.ColorDepthShadow = 2;
            this.btnOK.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnOK.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnOK.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnOK.IconIndexDisable = -1;
            this.btnOK.IconIndexHover = -1;
            this.btnOK.IconIndexNormal = -1;
            this.btnOK.IconIndexSelect = -1;
            this.btnOK.Image = null;
            this.btnOK.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnOK.ImageIndent = 0;
            this.btnOK.ImageList = null;
            this.btnOK.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnOK.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnOK.Location = new System.Drawing.Point(397, 204);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 184;
            this.btnOK.Text = "확인";
            this.btnOK.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnOK.TextIndent = 0;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.BackgroundImageDisable = null;
            this.btnCancel.BackgroundImageHover = null;
            this.btnCancel.BackgroundImageNormal = null;
            this.btnCancel.BackgroundImageSelect = null;
            this.btnCancel.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnCancel.BorderEdgeRadius = 3;
            this.btnCancel.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnCancel.ButtonImageCenter = null;
            this.btnCancel.ButtonImageLeft = null;
            this.btnCancel.ButtonImageRight = null;
            this.btnCancel.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnCancel, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnCancel.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnCancel.ColorDepthFocus = 2;
            this.btnCancel.ColorDepthHover = 2;
            this.btnCancel.ColorDepthShadow = 2;
            this.btnCancel.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnCancel.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnCancel.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnCancel.IconIndexDisable = -1;
            this.btnCancel.IconIndexHover = -1;
            this.btnCancel.IconIndexNormal = -1;
            this.btnCancel.IconIndexSelect = -1;
            this.btnCancel.Image = null;
            this.btnCancel.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnCancel.ImageIndent = 0;
            this.btnCancel.ImageList = null;
            this.btnCancel.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnCancel.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnCancel.Location = new System.Drawing.Point(478, 204);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 185;
            this.btnCancel.Text = "닫기";
            this.btnCancel.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnCancel.TextIndent = 0;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoUserNeGroup);
            this.groupBox1.Controls.Add(this.rdoNEGroup);
            this.groupBox1.Location = new System.Drawing.Point(12, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(224, 45);
            this.groupBox1.TabIndex = 186;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "장비 구분";
            // 
            // rdoUserNeGroup
            // 
            this.rdoUserNeGroup.AutoSize = true;
            this.rdoUserNeGroup.Location = new System.Drawing.Point(87, 20);
            this.rdoUserNeGroup.Name = "rdoUserNeGroup";
            this.rdoUserNeGroup.Size = new System.Drawing.Size(71, 16);
            this.rdoUserNeGroup.TabIndex = 184;
            this.rdoUserNeGroup.Text = "수동장비";
            this.rdoUserNeGroup.UseVisualStyleBackColor = true;
            // 
            // rdoNEGroup
            // 
            this.rdoNEGroup.AutoSize = true;
            this.rdoNEGroup.Checked = true;
            this.rdoNEGroup.Location = new System.Drawing.Point(10, 20);
            this.rdoNEGroup.Name = "rdoNEGroup";
            this.rdoNEGroup.Size = new System.Drawing.Size(71, 16);
            this.rdoNEGroup.TabIndex = 183;
            this.rdoNEGroup.TabStop = true;
            this.rdoNEGroup.Text = "일반장비";
            this.rdoNEGroup.UseVisualStyleBackColor = true;
            // 
            // SearchLinkDevice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(567, 239);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.fgDeviceList);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnDeviceSearch);
            this.Controls.Add(this.txtIPAddress);
            this.DoubleBuffered = true;
            this.Name = "SearchLinkDevice";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "링크 장비 연결";
            ((System.ComponentModel.ISupportInitialize)(this.fgDeviceList)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public C1.Win.C1FlexGrid.C1FlexGrid fgDeviceList;
        private System.Windows.Forms.Label label3;
        private MKLibrary.Controls.MKButton btnDeviceSearch;
        private MKLibrary.Controls.MKIPAddress txtIPAddress;
        private MKLibrary.Controls.MKButton btnOK;
        private MKLibrary.Controls.MKButton btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoUserNeGroup;
        private System.Windows.Forms.RadioButton rdoNEGroup;
    }
}