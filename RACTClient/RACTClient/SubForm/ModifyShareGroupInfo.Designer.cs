namespace RACTClient
{
    partial class ModifyShareGroupInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModifyShareGroupInfo));
            this.lblUserInfo = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.cboSearch = new MKLibrary.Controls.MKComboBox();
            this.btnSearch = new MKLibrary.Controls.MKButton(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.fgDeviceList1 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnAdd = new MKLibrary.Controls.MKButton(this.components);
            this.btnAddAll = new MKLibrary.Controls.MKButton(this.components);
            this.btnRemove = new MKLibrary.Controls.MKButton(this.components);
            this.btnRemoveAll = new MKLibrary.Controls.MKButton(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtGroupName = new System.Windows.Forms.TextBox();
            this.txtGroupDesc = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.fgDeviceList2 = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.lblSelectDeviceCount = new System.Windows.Forms.Label();
            this.lblCheckDeviceCount = new System.Windows.Forms.Label();
            this.btnSave = new MKLibrary.Controls.MKButton(this.components);
            this.btnClose = new MKLibrary.Controls.MKButton(this.components);
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.progressBarX1 = new DevComponents.DotNetBar.Controls.ProgressBarX();
            this.trvUserGroup = new RACTClient.ucGroupTree();
            this.trvGroup = new RACTClient.ucGroupTree();
            ((System.ComponentModel.ISupportInitialize)(this.fgDeviceList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fgDeviceList2)).BeginInit();
            this.SuspendLayout();
            // 
            // lblUserInfo
            // 
            this.lblUserInfo.AutoSize = true;
            this.lblUserInfo.Location = new System.Drawing.Point(713, 74);
            this.lblUserInfo.Name = "lblUserInfo";
            this.lblUserInfo.Size = new System.Drawing.Size(117, 12);
            this.lblUserInfo.TabIndex = 13;
            this.lblUserInfo.Text = "추가할 사용자 목록 :";
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtSearch.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtSearch.Location = new System.Drawing.Point(864, 6);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(116, 21);
            this.txtSearch.TabIndex = 176;
            // 
            // cboSearch
            // 
            this.cboSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboSearch.BackColor = System.Drawing.SystemColors.Window;
            this.cboSearch.BackColorSelected = System.Drawing.Color.Orange;
            this.cboSearch.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(201)))), ((int)(((byte)(201)))), ((int)(((byte)(185)))));
            this.cboSearch.BorderEdgeRadius = 3;
            this.cboSearch.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.cboSearch.BorderStyle = MKLibrary.MKDrawing.E_BorderStyle.Fixed;
            this.cboSearch.BoxBorderColor = System.Drawing.Color.Gray;
            this.cboSearch.ColorBoxSize = new System.Drawing.Size(12, 8);
            this.cboSearch.ComboBoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSearch.Font = new System.Drawing.Font("돋움", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cboSearch.ForeColor = System.Drawing.Color.Black;
            this.cboSearch.ImageList = null;
            this.cboSearch.ItemHeight = 14;
            this.cboSearch.Location = new System.Drawing.Point(780, 6);
            this.cboSearch.MaxDorpDownWidth = 500;
            this.cboSearch.Name = "cboSearch";
            this.cboSearch.SelectedIndex = -1;
            this.cboSearch.ShowColorBox = false;
            this.cboSearch.Size = new System.Drawing.Size(78, 21);
            this.cboSearch.TabIndex = 175;
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackgroundImageDisable = null;
            this.btnSearch.BackgroundImageHover = null;
            this.btnSearch.BackgroundImageNormal = null;
            this.btnSearch.BackgroundImageSelect = null;
            this.btnSearch.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnSearch.BorderEdgeRadius = 3;
            this.btnSearch.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnSearch.ButtonImageCenter = null;
            this.btnSearch.ButtonImageLeft = null;
            this.btnSearch.ButtonImageRight = null;
            this.btnSearch.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnSearch, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnSearch.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnSearch.ColorDepthFocus = 2;
            this.btnSearch.ColorDepthHover = 2;
            this.btnSearch.ColorDepthShadow = 2;
            this.btnSearch.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnSearch.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnSearch.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnSearch.IconIndexDisable = -1;
            this.btnSearch.IconIndexHover = -1;
            this.btnSearch.IconIndexNormal = -1;
            this.btnSearch.IconIndexSelect = -1;
            this.btnSearch.Image = null;
            this.btnSearch.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnSearch.ImageIndent = 0;
            this.btnSearch.ImageList = null;
            this.btnSearch.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnSearch.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnSearch.Location = new System.Drawing.Point(985, 6);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(55, 21);
            this.btnSearch.TabIndex = 174;
            this.btnSearch.Text = "검색";
            this.btnSearch.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnSearch.TextIndent = 0;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(713, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 177;
            this.label2.Text = "검색 조건 :";
            // 
            // fgDeviceList1
            // 
            this.fgDeviceList1.AllowEditing = false;
            this.fgDeviceList1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fgDeviceList1.BackColor = System.Drawing.SystemColors.Window;
            this.fgDeviceList1.ColumnInfo = resources.GetString("fgDeviceList1.ColumnInfo");
            this.fgDeviceList1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.fgDeviceList1.Location = new System.Drawing.Point(262, 28);
            this.fgDeviceList1.Name = "fgDeviceList1";
            this.fgDeviceList1.Rows.Count = 1;
            this.fgDeviceList1.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.ListBox;
            this.fgDeviceList1.Size = new System.Drawing.Size(420, 159);
            this.fgDeviceList1.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("fgDeviceList1.Styles"));
            this.fgDeviceList1.TabIndex = 178;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(235, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(19, 12);
            this.label3.TabIndex = 179;
            this.label3.Text = "->";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(260, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(133, 12);
            this.label4.TabIndex = 181;
            this.label4.Text = "사용자 장비 조회 목록 :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(260, 230);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(133, 12);
            this.label5.TabIndex = 182;
            this.label5.Text = "사용자 장비 선택 목록 :";
            // 
            // btnAdd
            // 
            this.btnAdd.BackgroundImageDisable = null;
            this.btnAdd.BackgroundImageHover = null;
            this.btnAdd.BackgroundImageNormal = null;
            this.btnAdd.BackgroundImageSelect = null;
            this.btnAdd.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnAdd.BorderEdgeRadius = 3;
            this.btnAdd.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnAdd.ButtonImageCenter = null;
            this.btnAdd.ButtonImageLeft = null;
            this.btnAdd.ButtonImageRight = null;
            this.btnAdd.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnAdd, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnAdd.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnAdd.ColorDepthFocus = 2;
            this.btnAdd.ColorDepthHover = 2;
            this.btnAdd.ColorDepthShadow = 2;
            this.btnAdd.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnAdd.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnAdd.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnAdd.IconIndexDisable = -1;
            this.btnAdd.IconIndexHover = -1;
            this.btnAdd.IconIndexNormal = -1;
            this.btnAdd.IconIndexSelect = -1;
            this.btnAdd.Image = null;
            this.btnAdd.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnAdd.ImageIndent = 0;
            this.btnAdd.ImageList = null;
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnAdd.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnAdd.Location = new System.Drawing.Point(261, 193);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(62, 23);
            this.btnAdd.TabIndex = 185;
            this.btnAdd.Text = "선택추가";
            this.btnAdd.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnAdd.TextIndent = 0;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnAddAll
            // 
            this.btnAddAll.BackgroundImageDisable = null;
            this.btnAddAll.BackgroundImageHover = null;
            this.btnAddAll.BackgroundImageNormal = null;
            this.btnAddAll.BackgroundImageSelect = null;
            this.btnAddAll.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnAddAll.BorderEdgeRadius = 3;
            this.btnAddAll.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnAddAll.ButtonImageCenter = null;
            this.btnAddAll.ButtonImageLeft = null;
            this.btnAddAll.ButtonImageRight = null;
            this.btnAddAll.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnAddAll, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnAddAll.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnAddAll.ColorDepthFocus = 2;
            this.btnAddAll.ColorDepthHover = 2;
            this.btnAddAll.ColorDepthShadow = 2;
            this.btnAddAll.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnAddAll.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnAddAll.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnAddAll.IconIndexDisable = -1;
            this.btnAddAll.IconIndexHover = -1;
            this.btnAddAll.IconIndexNormal = -1;
            this.btnAddAll.IconIndexSelect = -1;
            this.btnAddAll.Image = null;
            this.btnAddAll.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnAddAll.ImageIndent = 0;
            this.btnAddAll.ImageList = null;
            this.btnAddAll.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnAddAll.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnAddAll.Location = new System.Drawing.Point(331, 193);
            this.btnAddAll.Name = "btnAddAll";
            this.btnAddAll.Size = new System.Drawing.Size(62, 23);
            this.btnAddAll.TabIndex = 186;
            this.btnAddAll.Text = "전체추가";
            this.btnAddAll.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnAddAll.TextIndent = 0;
            this.btnAddAll.Click += new System.EventHandler(this.btnAddAll_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.BackgroundImageDisable = null;
            this.btnRemove.BackgroundImageHover = null;
            this.btnRemove.BackgroundImageNormal = null;
            this.btnRemove.BackgroundImageSelect = null;
            this.btnRemove.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnRemove.BorderEdgeRadius = 3;
            this.btnRemove.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnRemove.ButtonImageCenter = null;
            this.btnRemove.ButtonImageLeft = null;
            this.btnRemove.ButtonImageRight = null;
            this.btnRemove.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnRemove, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnRemove.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnRemove.ColorDepthFocus = 2;
            this.btnRemove.ColorDepthHover = 2;
            this.btnRemove.ColorDepthShadow = 2;
            this.btnRemove.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnRemove.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnRemove.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnRemove.IconIndexDisable = -1;
            this.btnRemove.IconIndexHover = -1;
            this.btnRemove.IconIndexNormal = -1;
            this.btnRemove.IconIndexSelect = -1;
            this.btnRemove.Image = null;
            this.btnRemove.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnRemove.ImageIndent = 0;
            this.btnRemove.ImageList = null;
            this.btnRemove.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnRemove.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnRemove.Location = new System.Drawing.Point(551, 192);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(62, 23);
            this.btnRemove.TabIndex = 183;
            this.btnRemove.Text = "선택삭제";
            this.btnRemove.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnRemove.TextIndent = 0;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnRemoveAll
            // 
            this.btnRemoveAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveAll.BackgroundImageDisable = null;
            this.btnRemoveAll.BackgroundImageHover = null;
            this.btnRemoveAll.BackgroundImageNormal = null;
            this.btnRemoveAll.BackgroundImageSelect = null;
            this.btnRemoveAll.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnRemoveAll.BorderEdgeRadius = 3;
            this.btnRemoveAll.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnRemoveAll.ButtonImageCenter = null;
            this.btnRemoveAll.ButtonImageLeft = null;
            this.btnRemoveAll.ButtonImageRight = null;
            this.btnRemoveAll.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnRemoveAll, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnRemoveAll.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnRemoveAll.ColorDepthFocus = 2;
            this.btnRemoveAll.ColorDepthHover = 2;
            this.btnRemoveAll.ColorDepthShadow = 2;
            this.btnRemoveAll.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnRemoveAll.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnRemoveAll.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnRemoveAll.IconIndexDisable = -1;
            this.btnRemoveAll.IconIndexHover = -1;
            this.btnRemoveAll.IconIndexNormal = -1;
            this.btnRemoveAll.IconIndexSelect = -1;
            this.btnRemoveAll.Image = null;
            this.btnRemoveAll.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnRemoveAll.ImageIndent = 0;
            this.btnRemoveAll.ImageList = null;
            this.btnRemoveAll.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnRemoveAll.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnRemoveAll.Location = new System.Drawing.Point(620, 192);
            this.btnRemoveAll.Name = "btnRemoveAll";
            this.btnRemoveAll.Size = new System.Drawing.Size(62, 23);
            this.btnRemoveAll.TabIndex = 184;
            this.btnRemoveAll.Text = "전체삭제";
            this.btnRemoveAll.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnRemoveAll.TextIndent = 0;
            this.btnRemoveAll.Click += new System.EventHandler(this.btnRemoveAll_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(713, 276);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 187;
            this.label6.Text = "그룹 이름 :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(713, 303);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 188;
            this.label7.Text = "그룹 설명 :";
            // 
            // txtGroupName
            // 
            this.txtGroupName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGroupName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtGroupName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtGroupName.Location = new System.Drawing.Point(784, 272);
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.Size = new System.Drawing.Size(250, 21);
            this.txtGroupName.TabIndex = 189;
            // 
            // txtGroupDesc
            // 
            this.txtGroupDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGroupDesc.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtGroupDesc.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtGroupDesc.Location = new System.Drawing.Point(784, 300);
            this.txtGroupDesc.Multiline = true;
            this.txtGroupDesc.Name = "txtGroupDesc";
            this.txtGroupDesc.Size = new System.Drawing.Size(250, 65);
            this.txtGroupDesc.TabIndex = 190;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(713, 35);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(239, 12);
            this.label8.TabIndex = 191;
            this.label8.Text = "* 추가할 사용자를 검색하여, 선택해주세요.";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(217, 12);
            this.label9.TabIndex = 192;
            this.label9.Text = "* 장비그룹을 더블클릭->장비목록 추가";
            // 
            // fgDeviceList2
            // 
            this.fgDeviceList2.AllowEditing = false;
            this.fgDeviceList2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fgDeviceList2.BackColor = System.Drawing.SystemColors.Window;
            this.fgDeviceList2.ColumnInfo = resources.GetString("fgDeviceList2.ColumnInfo");
            this.fgDeviceList2.ForeColor = System.Drawing.SystemColors.WindowText;
            this.fgDeviceList2.Location = new System.Drawing.Point(262, 245);
            this.fgDeviceList2.Name = "fgDeviceList2";
            this.fgDeviceList2.Rows.Count = 1;
            this.fgDeviceList2.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.ListBox;
            this.fgDeviceList2.Size = new System.Drawing.Size(420, 155);
            this.fgDeviceList2.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("fgDeviceList2.Styles"));
            this.fgDeviceList2.TabIndex = 193;
            // 
            // lblSelectDeviceCount
            // 
            this.lblSelectDeviceCount.AutoSize = true;
            this.lblSelectDeviceCount.Location = new System.Drawing.Point(574, 9);
            this.lblSelectDeviceCount.Name = "lblSelectDeviceCount";
            this.lblSelectDeviceCount.Size = new System.Drawing.Size(93, 12);
            this.lblSelectDeviceCount.TabIndex = 194;
            this.lblSelectDeviceCount.Text = "조회 장비대수 : ";
            // 
            // lblCheckDeviceCount
            // 
            this.lblCheckDeviceCount.AutoSize = true;
            this.lblCheckDeviceCount.Location = new System.Drawing.Point(563, 230);
            this.lblCheckDeviceCount.Name = "lblCheckDeviceCount";
            this.lblCheckDeviceCount.Size = new System.Drawing.Size(93, 12);
            this.lblCheckDeviceCount.TabIndex = 195;
            this.lblCheckDeviceCount.Text = "선택 장비대수 : ";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackgroundImageDisable = null;
            this.btnSave.BackgroundImageHover = null;
            this.btnSave.BackgroundImageNormal = null;
            this.btnSave.BackgroundImageSelect = null;
            this.btnSave.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnSave.BorderEdgeRadius = 3;
            this.btnSave.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnSave.ButtonImageCenter = null;
            this.btnSave.ButtonImageLeft = null;
            this.btnSave.ButtonImageRight = null;
            this.btnSave.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnSave, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnSave.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnSave.ColorDepthFocus = 2;
            this.btnSave.ColorDepthHover = 2;
            this.btnSave.ColorDepthShadow = 2;
            this.btnSave.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnSave.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnSave.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnSave.IconIndexDisable = -1;
            this.btnSave.IconIndexHover = -1;
            this.btnSave.IconIndexNormal = -1;
            this.btnSave.IconIndexSelect = -1;
            this.btnSave.Image = null;
            this.btnSave.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnSave.ImageIndent = 0;
            this.btnSave.ImageList = null;
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnSave.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnSave.Location = new System.Drawing.Point(893, 371);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(62, 23);
            this.btnSave.TabIndex = 196;
            this.btnSave.Text = "저장";
            this.btnSave.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnSave.TextIndent = 0;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackgroundImageDisable = null;
            this.btnClose.BackgroundImageHover = null;
            this.btnClose.BackgroundImageNormal = null;
            this.btnClose.BackgroundImageSelect = null;
            this.btnClose.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnClose.BorderEdgeRadius = 3;
            this.btnClose.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnClose.ButtonImageCenter = null;
            this.btnClose.ButtonImageLeft = null;
            this.btnClose.ButtonImageRight = null;
            this.btnClose.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnClose, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnClose.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnClose.ColorDepthFocus = 2;
            this.btnClose.ColorDepthHover = 2;
            this.btnClose.ColorDepthShadow = 2;
            this.btnClose.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnClose.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnClose.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnClose.IconIndexDisable = -1;
            this.btnClose.IconIndexHover = -1;
            this.btnClose.IconIndexNormal = -1;
            this.btnClose.IconIndexSelect = -1;
            this.btnClose.Image = null;
            this.btnClose.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnClose.ImageIndent = 0;
            this.btnClose.ImageList = null;
            this.btnClose.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnClose.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnClose.Location = new System.Drawing.Point(966, 371);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(62, 23);
            this.btnClose.TabIndex = 197;
            this.btnClose.Text = "닫기";
            this.btnClose.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnClose.TextIndent = 0;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(688, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(19, 12);
            this.label10.TabIndex = 199;
            this.label10.Text = "->";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(713, 53);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(235, 12);
            this.label11.TabIndex = 200;
            this.label11.Text = "* 추가할 그룹 이름, 설명을 입력해 주세요.";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 28);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(135, 12);
            this.label12.TabIndex = 201;
            this.label12.Text = "* 현재 사용자 장비 목록";
            // 
            // progressBarX1
            // 
            // 
            // 
            // 
            this.progressBarX1.BackgroundStyle.Class = "";
            this.progressBarX1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.progressBarX1.FocusCuesEnabled = false;
            this.progressBarX1.Location = new System.Drawing.Point(14, 406);
            this.progressBarX1.Name = "progressBarX1";
            this.progressBarX1.ProgressType = DevComponents.DotNetBar.eProgressItemType.Marquee;
            this.progressBarX1.Size = new System.Drawing.Size(1026, 25);
            this.progressBarX1.TabIndex = 202;
            this.progressBarX1.Text = "progressBarX1";
            this.progressBarX1.Visible = false;
            // 
            // trvUserGroup
            // 
            this.trvUserGroup.Location = new System.Drawing.Point(14, 43);
            this.trvUserGroup.Name = "trvUserGroup";
            this.trvUserGroup.Size = new System.Drawing.Size(215, 357);
            this.trvUserGroup.TabIndex = 198;
            this.trvUserGroup.TreeType = RACTClient.E_TreeType.UserGroupList;
            this.trvUserGroup.OnAddShareDeviceEvent += new RACTClient.AddShareDeviceHandler(this.trvUserGroup_OnAddShareDeviceEvent);
            // 
            // trvGroup
            // 
            this.trvGroup.Location = new System.Drawing.Point(715, 89);
            this.trvGroup.Name = "trvGroup";
            this.trvGroup.Size = new System.Drawing.Size(319, 177);
            this.trvGroup.TabIndex = 14;
            this.trvGroup.TreeType = RACTClient.E_TreeType.DisplayUserGroupList;
            this.trvGroup.OnSelectUserInfoEvent += new RACTClient.SelectUserInfoHandler(this.trvGroup_OnSelectUserInfoEvent);
            // 
            // ModifyShareGroupInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1046, 438);
            this.Controls.Add(this.progressBarX1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.trvUserGroup);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblCheckDeviceCount);
            this.Controls.Add(this.lblSelectDeviceCount);
            this.Controls.Add(this.fgDeviceList2);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtGroupDesc);
            this.Controls.Add(this.txtGroupName);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnAddAll);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnRemoveAll);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.fgDeviceList1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.cboSearch);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.trvGroup);
            this.Controls.Add(this.lblUserInfo);
            this.DoubleBuffered = true;
            this.Name = "ModifyShareGroupInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "사용자 장비 공유";
            ((System.ComponentModel.ISupportInitialize)(this.fgDeviceList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fgDeviceList2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ucGroupTree trvGroup;
        private System.Windows.Forms.Label lblUserInfo;
        private System.Windows.Forms.TextBox txtSearch;
        private MKLibrary.Controls.MKComboBox cboSearch;
        private MKLibrary.Controls.MKButton btnSearch;
        private System.Windows.Forms.Label label2;
        public C1.Win.C1FlexGrid.C1FlexGrid fgDeviceList1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private MKLibrary.Controls.MKButton btnAdd;
        private MKLibrary.Controls.MKButton btnAddAll;
        private MKLibrary.Controls.MKButton btnRemove;
        private MKLibrary.Controls.MKButton btnRemoveAll;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtGroupName;
        private System.Windows.Forms.TextBox txtGroupDesc;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        public C1.Win.C1FlexGrid.C1FlexGrid fgDeviceList2;
        private System.Windows.Forms.Label lblSelectDeviceCount;
        private System.Windows.Forms.Label lblCheckDeviceCount;
        private MKLibrary.Controls.MKButton btnSave;
        private MKLibrary.Controls.MKButton btnClose;
        private ucGroupTree trvUserGroup;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private DevComponents.DotNetBar.Controls.ProgressBarX progressBarX1;
    }
}