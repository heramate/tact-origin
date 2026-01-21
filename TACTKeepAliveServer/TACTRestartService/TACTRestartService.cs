using System;
using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
using System.Diagnostics;
//using System.Linq;
using System.ServiceProcess;
using System.Windows.Forms;//Application
using System.Text;

namespace TACTRestartService
{
    public partial class TACTRestartService : ServiceBase
    {
        private TACTServiceManager m_ServiceManager = null;
        public TACTRestartService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (m_ServiceManager != null)
            {
                m_ServiceManager.Dispose();
                m_ServiceManager = null;
            }
            m_ServiceManager = new TACTServiceManager(Application.StartupPath);
            m_ServiceManager.Start();
        }

        protected override void OnStop()
        {
            if (m_ServiceManager != null)
            {
                m_ServiceManager.Dispose();
                m_ServiceManager = null;
            }
        }
    }
}
