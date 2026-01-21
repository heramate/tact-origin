using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SSHTerminalControlTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void txtConnect_Click(object sender, EventArgs e)
        {
            terminalEmulator1.ConnectSSH2(txtIPAddress.Text, 22, "admin", "vertex25");
        }
    }
}