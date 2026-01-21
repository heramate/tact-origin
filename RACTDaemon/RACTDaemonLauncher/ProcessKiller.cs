using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace RACTDaemonLauncher
{
   public class ProcessKiller
    {
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_CLOSE = 0xF060;

        [DllImport("user32.dll")]
        public static extern int FindWindow(
            string lpClassName, // class name
            string lpWindowName // window name
        );

        [DllImport("user32.dll")]
        public static extern int SendMessage(
            int hWnd, // handle to destination window
            uint Msg, // message
            int wParam, // first message parameter
            int lParam // second message parameter
        );

       public static void KILL()
       {
           int iHandle=FindWindow("Form1" ,"RACTDaemonExec");
           int j=SendMessage(iHandle, WM_SYSCOMMAND,SC_CLOSE, 0);
       }
    }
}
