using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RACTServer;

namespace RACTServerTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        RACTServer.RACTServer m_Server = null;

        /// <summary>
        /// 서버실행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = true;

            try
            {
                m_Server = new RACTServer.RACTServer(Application.StartupPath);
                if (!m_Server.Start())
                {
                    MessageBox.Show("서버실행 실패");
                    button1.Enabled = true;
                    button2.Enabled = false;
                    return;
                }

                txtDBServerIP.Text = GlobalClass.m_SystemInfo.DBServerIP;
                txtDBName.Text = GlobalClass.m_SystemInfo.DBName;
                txtDBLoginID.Text = GlobalClass.m_SystemInfo.UserID;
                txtDBLoginPwd.Text = GlobalClass.m_SystemInfo.Password;

                txtServerIP.IPAddress = GlobalClass.m_SystemInfo.ServerIP;
                txtServerPort.Text = GlobalClass.m_SystemInfo.ServerPort.ToString();
                txtServerId.Text = GlobalClass.m_SystemInfo.ServerID.ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                button1.Enabled = true;
                button2.Enabled = false;
            }
        }

        /// <summary>
        /// 중지
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = false;

            if (m_Server != null)
            {
                m_Server.Stop();
                button1.Enabled = false;
                button2.Enabled = true;
            }
            //this.Close();
        }

        private void MainTest_Load(object sender, EventArgs e)
        {
            //button1_Click(null, null);
        }

        private void MainTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_Server != null)
            {
                m_Server.Stop();
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void txtDBLoginID_TextChanged(object sender, EventArgs e)
        {

        }
    }
}