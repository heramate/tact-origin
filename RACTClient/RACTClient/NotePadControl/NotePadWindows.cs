using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;


namespace RACTClient
{
    public partial class NotePadWindows : Office2007Form
    {
        public NotePadWindows()
        {
            InitializeComponent();
        }

        private bool m_IsClose = true;

        private void NotePadWindows_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_IsClose)
            {

            }
        }

        private void NotePadWindows_Resize(object sender, EventArgs e)
        {
            if (!m_IsClose) return;

            if(this.WindowState == FormWindowState.Minimized)
            {
                m_IsClose = false;
                //((ClientMain)AppGlobal.s_ClientMainForm).AddNotePadTab(
                this.Close();
            }
                
        
        }

        internal void AddNotePadControl(ucNotePad aNotePad)
        {
            this.Controls.Add(aNotePad);
        }

        private void NotePadWindows_MinimumSizeChanged(object sender, EventArgs e)
        {
            ucNotePad tNotePad = (ucNotePad)this.Controls[0];
            if(tNotePad.FilePath != "")
            {
                string[] tFileName = tNotePad.FilePath.Split('\\');

                this.Text = string.Format("{0} - 메모장", tFileName[tFileName.GetLength(0) - 1]);
            }
            else
            {
                this.Text = "제목없음 - 메모장";
            }
        }


        
    }
}