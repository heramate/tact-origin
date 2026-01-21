using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;

namespace RACTServerService
{
    public partial class RACTServerService : ServiceBase
    {
        RACTServer.RACTServer m_Server = null;
        public RACTServerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (m_Server != null)
            {
                m_Server.Stop();

                m_Server = null;
            }

            m_Server = new RACTServer.RACTServer(Application.StartupPath,true);
            m_Server.Start();
        }

        protected override void OnStop()
        {
            if (m_Server != null)
            {
                m_Server.Stop();
            }
        }
    }
}
