using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RACTServer
{
    public partial class MainTest : Form
    {
        public MainTest()
        {
            InitializeComponent();
        }

        RACTServer m_Server = null;
        private void button1_Click(object sender, EventArgs e)
        {
            m_Server = new RACTServer(Application.StartupPath);
            if (!m_Server.Start())
            {
                MessageBox.Show("에러 발생");
            }
            button1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (m_Server != null)
            {
                m_Server.Stop();
            }
            m_Server = null;
            button1.Enabled = true;
        }

        private void MainTest_Load(object sender, EventArgs e)
        {
            button1_Click(null, null);
        }

        private void MainTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_Server != null)
            {
                m_Server.Stop();
            }
        }
    }
}