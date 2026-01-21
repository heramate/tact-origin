using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace AutoInstall
{
    class _Log
    {
        static public string fPath;
        static public string tag;
        static public bool DebugState = false;

        public _Log()
        {
            tag = "";
        }
        public _Log(string AppPath)
        {
            tag = "";
            fPath = AppPath;

            DirectoryInfo dir = new DirectoryInfo(fPath);
            // 폴더가 존재하지 않으면
            if (dir.Exists == false)
            {
                // 새로 생성합니다.
                dir.Create();
            }
        }

        public _Log(string Tag, string AppPath)
        {
            tag = Tag;
            fPath = AppPath;

            DirectoryInfo dir = new DirectoryInfo(fPath);
            // 폴더가 존재하지 않으면
            if (dir.Exists == false)
            {
                // 새로 생성합니다.
                dir.Create();
            }
        }

        public void setDebugState(bool vbool)
        {
            DebugState = vbool;
        }

        public bool getDebugState()
        {
            return DebugState;
        }

        private static string gHMSTime
        {
            get
            {
                string tmp = string.Format("{0:D2}:{1:D2}:{2:D2}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                return tmp;
            }
        }

        private static string gYMNTime
        {
            get
            {
                string tmp = string.Format("{0:D2}{1:D2}{2:D2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                return tmp;
            }
        }

        public void GetLog()
        {
            Process process = new Process();
            //string path = Application.StartupPath + "\\Log_" + gYMNTime + ".txt";
            string path = fPath + "\\" + tag + "_Log_" + gYMNTime + ".txt";

            process.StartInfo.FileName = path;
            process.StartInfo.Arguments = @"\" + tag + "_Log_" + gYMNTime + ".txt";
            process.Start();
        }

        public void SetLog(string logstring)
        {
            if (DebugState)
            {
                string tmp = gYMNTime;
                //string path = Application.StartupPath + "\\Log_" + gYMNTime + ".txt";
                string path = fPath + "\\" + tag + "_Log_" + gYMNTime + ".txt";

                FileInfo fileinfo = new FileInfo(path);
                try
                {
                    FileStream filesavestream = File.Open(path, FileMode.Append);
                    StreamWriter sw = new StreamWriter(filesavestream);

                    string log = string.Format("[{0}] {1}", gHMSTime, logstring);
                    sw.WriteLine(log);

                    sw.Close();
                }
                catch (Exception ex)
                {
                    SetLog("Exception" + ex.Message);
                }
            }
        }

        public void dellog()
        {

        }
    }
}
