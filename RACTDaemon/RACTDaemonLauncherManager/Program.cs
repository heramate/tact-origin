using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace RACTDaemonLauncherManager
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool tIsCreatedNew;
            Mutex tMutex = new Mutex(true, "WIA_DIO_COM", out tIsCreatedNew);
            if (tIsCreatedNew)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new ServiceMain());
                tMutex.ReleaseMutex();
            }
            else
            {
                MessageBox.Show("RACT Daemon Service Manager가 실행 중 입니다.");
            }
        }
    }
}