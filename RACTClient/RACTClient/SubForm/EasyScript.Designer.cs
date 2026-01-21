namespace RACTClient
{
    partial class EasyScript
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EasyScript));
            this.fgCommand = new C1.Win.C1FlexGrid.C1FlexGrid();
            this.txtScript = new System.Windows.Forms.RichTextBox();
            this.btnCreate = new MKLibrary.Controls.MKButton(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.fgCommand)).BeginInit();
            this.SuspendLayout();
            // 
            // fgCommand
            // 
            this.fgCommand.BackColor = System.Drawing.SystemColors.Window;
            this.fgCommand.ColumnInfo = resources.GetString("fgCommand.ColumnInfo");
            this.fgCommand.ForeColor = System.Drawing.SystemColors.WindowText;
            this.fgCommand.Location = new System.Drawing.Point(0, 72);
            this.fgCommand.Name = "fgCommand";
            this.fgCommand.Rows.Count = 1;
            this.fgCommand.Size = new System.Drawing.Size(610, 253);
            this.fgCommand.Styles = new C1.Win.C1FlexGrid.CellStyleCollection(resources.GetString("fgCommand.Styles"));
            this.fgCommand.TabIndex = 2;
            this.fgCommand.ChangeEdit += new System.EventHandler(this.fgCommand_ChangeEdit);
            this.fgCommand.AfterEdit += new C1.Win.C1FlexGrid.RowColEventHandler(this.fgCommand_AfterEdit);
            this.fgCommand.KeyDown += new System.Windows.Forms.KeyEventHandler(this.fgCommand_KeyDown);
            this.fgCommand.SelChange += new System.EventHandler(this.fgCommand_SelChange);
            this.fgCommand.KeyUp += new System.Windows.Forms.KeyEventHandler(this.fgCommandList_KeyUp);
            // 
            // txtScript
            // 
            this.txtScript.Location = new System.Drawing.Point(0, 331);
            this.txtScript.Name = "txtScript";
            this.txtScript.Size = new System.Drawing.Size(610, 296);
            this.txtScript.TabIndex = 3;
            this.txtScript.Text = "";
            // 
            // btnCreate
            // 
            this.btnCreate.BackgroundImageDisable = null;
            this.btnCreate.BackgroundImageHover = null;
            this.btnCreate.BackgroundImageNormal = null;
            this.btnCreate.BackgroundImageSelect = null;
            this.btnCreate.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.btnCreate.BorderEdgeRadius = 3;
            this.btnCreate.BorderEdgeStyle = MKLibrary.MKDrawing.E_EdgeStyle.Rectangular;
            this.btnCreate.ButtonImageCenter = null;
            this.btnCreate.ButtonImageLeft = null;
            this.btnCreate.ButtonImageRight = null;
            this.btnCreate.ButtonImages = new MKLibrary.Controls.MKThreeImageCollection(this.btnCreate, new MKLibrary.Controls.MKImage[] {
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1),
            new MKLibrary.Controls.MKImage(-1, -1, -1, -1)});
            this.btnCreate.ButtonStyle = MKLibrary.MKObject.E_ButtonStyle.Fixed;
            this.btnCreate.ColorDepthFocus = 2;
            this.btnCreate.ColorDepthHover = 2;
            this.btnCreate.ColorDepthShadow = 2;
            this.btnCreate.ControlColor = MKLibrary.MKDrawing.E_ControlColor.LightGray;
            this.btnCreate.ControlColorInfo = new MKLibrary.MKDrawing.ControlColorInfo(System.Drawing.Color.WhiteSmoke, System.Drawing.Color.LightGray, System.Drawing.Color.White, System.Drawing.Color.Gainsboro, System.Drawing.Color.Silver, System.Drawing.Color.White, System.Drawing.SystemColors.Control, System.Drawing.SystemColors.Control, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.Color.Black, System.Drawing.SystemColors.ControlDark, System.Drawing.Color.Khaki, System.Drawing.Color.Orange, System.Drawing.Color.LightBlue, System.Drawing.Color.CornflowerBlue);
            this.btnCreate.GradationStyle = MKLibrary.MKDrawing.E_GradationStyle.TopToBottom;
            this.btnCreate.IconIndexDisable = -1;
            this.btnCreate.IconIndexHover = -1;
            this.btnCreate.IconIndexNormal = -1;
            this.btnCreate.IconIndexSelect = -1;
            this.btnCreate.Image = null;
            this.btnCreate.ImageAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleLeft;
            this.btnCreate.ImageIndent = 0;
            this.btnCreate.ImageList = null;
            this.btnCreate.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.btnCreate.InternalGap = new MKLibrary.MKObject.MKRectangleGap(2, 4, 2, 4);
            this.btnCreate.Location = new System.Drawing.Point(519, 42);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(89, 26);
            this.btnCreate.TabIndex = 173;
            this.btnCreate.Text = "스크립트 생성";
            this.btnCreate.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
            this.btnCreate.TextIndent = 0;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(503, 12);
            this.label4.TabIndex = 174;
            this.label4.Text = "* 아래 명령어와 프롬프트를 입력후 스크립트 생성 클릭하면, 하단에 스크립트가 생성됩니다.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(206, 12);
            this.label1.TabIndex = 175;
            this.label1.Text = "* INSERT : 열추가 DELETE : 열삭제";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(535, 12);
            this.label2.TabIndex = 176;
            this.label2.Text = "*프롬프트 용도 : #이 아닌 다른 프롬프트로 오는 경우 사용, #인 경우 프롬프트 입력안해도 됩니다.";
            // 
            // EasyScript
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 667);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.txtScript);
            this.Controls.Add(this.fgCommand);
            this.DoubleBuffered = true;
            this.Name = "EasyScript";
            this.Text = "EasyScript";
            this.Controls.SetChildIndex(this.fgCommand, 0);
            this.Controls.SetChildIndex(this.txtScript, 0);
            this.Controls.SetChildIndex(this.btnCreate, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.fgCommand)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private C1.Win.C1FlexGrid.C1FlexGrid fgCommand;
        private System.Windows.Forms.RichTextBox txtScript;
        private MKLibrary.Controls.MKButton btnCreate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}