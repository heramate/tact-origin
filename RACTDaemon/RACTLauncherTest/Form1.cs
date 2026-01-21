using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RACTDaemonLauncher;
using System.Diagnostics;

namespace RACTLauncherTest
{
    public partial class Form1 : Form
    {
        DaemonLauncher m_Launcher = new DaemonLauncher();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            if (!m_Launcher.Start())
            {
                button1.Enabled = true;
                MessageBox.Show("데몬런처 실행이 실패했습니다.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_Launcher.Stop();

            button1.Enabled = true;
        }
    }
}