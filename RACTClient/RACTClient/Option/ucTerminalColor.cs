using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RACTClient
{

    public partial class ucTerminalColor : UserControl,IOptionPanal
    {
        public ucTerminalColor()
        {
            InitializeComponent();
        }

        public void InitializeControl()
        {
   
            AppGlobal.InitializeButtonStyle(btnDefault);
            AppGlobal.InitializeButtonStyle(btnFont);

            btnBackColor.SelectedColor = AppGlobal.s_ClientOption.TerminalBackGroundColor;
            btnFontColor.SelectedColor = AppGlobal.s_ClientOption.TerminalFontColor;

            lblBackColor.BackColor = AppGlobal.s_ClientOption.TerminalBackGroundColor;
            lblFontColor.BackColor = AppGlobal.s_ClientOption.TerminalFontColor;

            lblPreView.BackColor = lblBackColor.BackColor;
            lblPreView.ForeColor = lblFontColor.BackColor;

            lblFont.Text = string.Concat("'", AppGlobal.s_ClientOption.TerminalFontName, "', ", AppGlobal.s_ClientOption.TerminalFontSize);
            lblFont.Tag = new System.Drawing.Font(AppGlobal.s_ClientOption.TerminalFontName, AppGlobal.s_ClientOption.TerminalFontSize, AppGlobal.s_ClientOption.TerminalFontStyle, System.Drawing.GraphicsUnit.Point, ((byte)(0))); ;
            lblPreView.Text = "SWITCH# \r\nSWITCH# \r\nSWITCH# \r\nSWITCH# \r\nSWITCH# \r\nSWITCH# sh run\r\nBuilding configuration...\r\n\r\nCurrent configuration:";
            lblPreView.Font = (Font)lblFont.Tag;

        }

        public bool SaveOption()
        {
            AppGlobal.s_ClientOption.TerminalBackGroundColor = lblBackColor.BackColor;
            AppGlobal.s_ClientOption.TerminalFontColor = lblFontColor.BackColor;
            AppGlobal.s_ClientOption.TerminalFontName = ((Font)lblFont.Tag).FontFamily.Name;
            AppGlobal.s_ClientOption.TerminalFontSize = ((Font)lblFont.Tag).Size;
            AppGlobal.s_ClientOption.TerminalFontStyle = ((Font)lblFont.Tag).Style;
            return true;
        }

        private void btnBackColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                lblBackColor.BackColor = colorDialog1.Color;
                lblPreView.BackColor = lblBackColor.BackColor;
            }
        }

        private void btnFontColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                lblFontColor.BackColor = colorDialog1.Color;
                lblPreView.ForeColor = lblFontColor.BackColor;
            }
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
           lblBackColor.BackColor= Color.Black;
           lblFontColor.BackColor = Color.GreenYellow;
           Font tFont = new System.Drawing.Font("±¼¸²Ã¼", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           lblFont.Tag = tFont;

           lblFont.Text = string.Concat("'", tFont.FontFamily.Name, "', ", tFont.Size);

           lblPreView.BackColor = lblBackColor.BackColor;
           lblPreView.ForeColor = lblFontColor.BackColor;
           lblPreView.Font = (Font)lblFont.Tag;
        }

        private void btnBackColor_SelectedColorChanged(object sender, EventArgs e)
        {
            lblBackColor.BackColor = btnBackColor.SelectedColor;
            lblPreView.BackColor = btnBackColor.SelectedColor;
        }

        private void btnFontColor_SelectedColorChanged(object sender, EventArgs e)
        {
            lblFontColor.BackColor = btnFontColor.SelectedColor;
            lblPreView.ForeColor = btnFontColor.SelectedColor;
        }

        private void btnFont_Click(object sender, EventArgs e)
        {



            FontFrom tFontDialog = new FontFrom();
            tFontDialog.initializeControl();
            
            if (tFontDialog.ShowDialog() == DialogResult.OK)
            {
                lblFont.Text = string.Concat("'", tFontDialog.Font.FontFamily.Name, "', ", tFontDialog.Font.Size);
                lblFont.Tag = tFontDialog.Font;
                lblPreView.Font = tFontDialog.Font;
            }


            //FontDialog tFontDialog = new FontDialog();
            //tFontDialog.Font = (Font)lblFont.Tag; ;
            //if (tFontDialog.ShowDialog() == DialogResult.OK)
            //{
            //    lblFont.Text = string.Concat("'", tFontDialog.Font.FontFamily.Name, "', ", tFontDialog.Font.Size);
            //    lblFont.Tag = tFontDialog.Font;
            //    lblPreView.Font = tFontDialog.Font;
            //}
        }
    }
}
