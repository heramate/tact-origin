using System;
using System.Collections.Generic;
using System.IO; //Directory
using System.Linq;
using System.Windows.Forms;

namespace TACTKeepAliveServerTester
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);//***
            Application.Run(new ServerController());
        }

        //[출처] [C#] UnhandledException 처리(http://blog.naver.com/PostView.nhn?blogId=sosozl&logNo=220464612901)
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string message = "프로그램에 문제가 있어 비 정상적으로 종료 되었습니다.\n"+
                                "아래 내용을 개발사에게 전달 바랍니다.\nskbsm@daims.com\n\n\n";
            //string[] call_stacks;
 
            Exception exc = (Exception)e.ExceptionObject;

            string[] call_stacks = exc.StackTrace.Split(new string[] { "\r\n" },
                                    StringSplitOptions.RemoveEmptyEntries);
 
            message += "● Error:\n    " + exc.Message + "\n\n";
            message += "● Location:\n";;
 
            foreach(string line in call_stacks)
            {
                if (line.Contains(".cs"))
                {
                    message += line + "\n\n";
                }
            }

            //string ExceptionLogFilePath = Application.StartupPath + "\\ExceptionLog";
            //if (!Directory.Exists(ExceptionLogFilePath))
            //{
            //    //로그 저장 폴더가 없으면 생성합니다.
            //    Directory.CreateDirectory(ExceptionLogFilePath);
            //}

            //ExceptionLogFilePath += string.Format("\\UnhandledException_{0}.log", DateTime.Now.ToString("yyyyMMddHHmmss"));
            //System.IO.File.WriteAllText(ExceptionLogFilePath, message);
            System.IO.File.WriteAllText(string.Format("_UnhandledException_{0}.log", DateTime.Now.ToString("yyyyMMddHHmmss")), message);

            //GlobalClass.PrintLogException("▼미처리(Unhandled) 예외 발견: \r\n", message);
            //MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
 
            //Environment.Exit(101); //오류 보고 다이얼로그 표시하지 않고 종료 시키기 for WINDOWS 7
        }

    }
}
