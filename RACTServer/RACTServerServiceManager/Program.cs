using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RACTServerServiceManager
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ServiceManagerMainForm tServiceManagerMainForm = new ServiceManagerMainForm();
            tServiceManagerMainForm.InitializeControl();
            Application.Run(tServiceManagerMainForm);
        }
    }
}