using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using RACTDaemonLauncher;

namespace RACTDaemonService
{
    public partial class RactDaemon : ServiceBase
    {
        private DaemonLauncher m_DaemonLauncher;
        public RactDaemon()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            m_DaemonLauncher = new DaemonLauncher();
            m_DaemonLauncher.Start();
        }

        protected override void OnStop()
        {
            if (m_DaemonLauncher != null)
            {
                m_DaemonLauncher.Stop();
            }
        }
    }
}
