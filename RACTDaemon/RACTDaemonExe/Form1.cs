using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RACTDaemonProcess;
using RACTDaemonLauncher;

namespace RACTDaemonExe
{
    public partial class Form1 : Form
    {
        private DaemonProcess m_DaemonProcess ;
        public Form1()
        {
            InitializeComponent();
        }


        public Form1(string aServerlIP, int aServerPort, string aServerChannelName, string aLocallIP, int aLocalPort)
        {
            InitializeComponent();
            m_DaemonProcess = new DaemonProcess(aServerlIP, aServerPort, aServerChannelName, aLocallIP, aLocalPort);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!m_DaemonProcess.StartDaemon())
            {
                MessageBox.Show("[RACTDaemonExe] DaemonProcess start Error");
            }
            button1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            m_DaemonProcess.Stop();
            button1.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button1_Click(null, null);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_DaemonProcess.SendDaemonLogOut();
        }
    }
}