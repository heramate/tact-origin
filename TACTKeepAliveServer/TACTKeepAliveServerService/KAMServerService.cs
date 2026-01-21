using System;
using System.ComponentModel;
using System.IO;//DirectoryInfo
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;
using TACT.KeepAliveServer;

namespace TACTKeepAliveServerService
{
    public partial class TACTKeepAliveServerService : ServiceBase
    {
        KeepAliveServer m_Server = null;
        public TACTKeepAliveServerService()
		{
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            /// 2019.10.02 KwonTaeSuk [KAMServer장비접속개선] 처리되지 않은 예외로 종료될때 로그 출력(UnhandledException_{0}.log)
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
                
            if (m_Server != null)
            {
                m_Server.Stop();
                m_Server = null;
            }

            m_Server = new KeepAliveServer(Application.StartupPath);
            m_Server.Start();
        }

        protected override void OnStop()
        {
            if (m_Server != null)
            {
                m_Server.Stop();
            }
        }

        //[출처] [C#] UnhandledException 처리(http://blog.naver.com/PostView.nhn?blogId=sosozl&logNo=220464612901)
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string message = "TACTKeepAliveServer 프로그램에 문제가 있어 비 정상적으로 종료 되었습니다.\n" +
                                "아래 내용을 전달 바랍니다.\nskbsm@daims.com\n\n\n";
            //string[] call_stacks;

            Exception exc = (Exception)e.ExceptionObject;

            string[] call_stacks = exc.StackTrace.Split(new string[] { "\r\n" },
                                    StringSplitOptions.RemoveEmptyEntries);

            message += "● Error:\n    " + exc.Message + "\n\n";
            message += "● Location:\n"; ;

            foreach (string line in call_stacks)
            {
                if (line.Contains(".cs"))
                {
                    message += line + "\n\n";
                }
            }

            String logFilePath = Application.StartupPath + "\\ExceptionLog";
            DirectoryInfo di = new DirectoryInfo(logFilePath);
            if (!di.Exists) {
                di.Create();
            }
            System.IO.File.WriteAllText(string.Format(logFilePath + "\\UnhandledException_{0}.log", DateTime.Now.ToString("yyyyMMddHHmmss")), message);
            //GlobalClass.PrintLogException("▼미처리(Unhandled) 예외 발견: \r\n", message);
            //MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            //Environment.Exit(101); //오류 보고 다이얼로그 표시하지 않고 종료 시키기 for WINDOWS 7
        }
    }
}
