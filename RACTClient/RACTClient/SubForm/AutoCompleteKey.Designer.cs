namespace RACTClient.SubForm
{
    partial class AutoCompleteKey
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.atkListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // atkListBox
            // 
            this.atkListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.atkListBox.FormattingEnabled = true;
            this.atkListBox.HorizontalScrollbar = true;
            this.atkListBox.ItemHeight = 12;
            this.atkListBox.Location = new System.Drawing.Point(1, 5);
            this.atkListBox.Name = "atkListBox";
            this.atkListBox.Size = new System.Drawing.Size(317, 132);
            this.atkListBox.TabIndex = 0;
            // 
            // AutoCompleteKey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(321, 142);
            this.Controls.Add(this.atkListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AutoCompleteKey";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "명령어 자동완성";
            this.Load += new System.EventHandler(this.AutoCompleteKey_OnLoad);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox atkListBox;



    }
}