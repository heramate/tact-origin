using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;

namespace TACTRestartService
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new TACTRestartService() 
            };
            ServiceBase.Run(ServicesToRun);
        }

        //▼바로실행 테스트
        //public static TACTServiceManager m_ServiceManager = null;
        //static void Main()
        //{
        //    m_ServiceManager = new TACTServiceManager(Application.StartupPath);
        //    m_ServiceManager.Start();
        //}
    }
}
