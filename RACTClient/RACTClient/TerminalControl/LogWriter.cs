using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace RACTClient
{
    public class LogWriter
    {

        private string m_FilePath = "";
        private string m_DirPath = "";

        public LogWriter()
        {
        }

        public LogWriter(string aDirPath, string aFilePath)
        {
            m_DirPath = aDirPath;
            m_FilePath = aFilePath;
        }

        public void Delete()
        {
            try
            {
                if (File.Exists(m_FilePath))
                {
                    File.Delete(m_FilePath);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public string GetAllText()
        {
            string tAllText = "";
            if (File.Exists(m_FilePath))
            {
                tAllText = File.ReadAllText(m_FilePath);
            }

            return tAllText;

        }

        /// <summary>
        /// 로그 기록
        /// </summary>
        /// <param name="str">로그내용
        public void Log(string str)
        {
            
            string temp;

            DirectoryInfo di = new DirectoryInfo(m_DirPath);
            FileInfo fi = new FileInfo(m_FilePath);

            try
            {
                if (di.Exists != true) Directory.CreateDirectory(m_DirPath);

                if (fi.Exists != true)
                {
                    using (StreamWriter sw = new StreamWriter(m_FilePath))
                    {
                        temp = str;
                        sw.WriteLine(temp);
                        sw.Close();
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(m_FilePath))
                    {
                        temp = str;

                        if (temp.Length == 1)
                        {
                            sw.Write(temp);
                            sw.Close();
                        }
                        else
                        {
                            sw.WriteLine(temp);
                            sw.Close();
                        }

                        
                    }
                }
            }
            catch (Exception e)
            {
            }
        }

    }
}
