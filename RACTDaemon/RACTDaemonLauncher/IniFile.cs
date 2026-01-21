using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace RACTDaemonLauncher
{
    public class IniFile
    {
        public string m_Path;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public IniFile(string INIPath)
        {
            m_Path = INIPath;
        }

        public void WriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.m_Path);
        }

        public string ReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(Section, Key, string.Empty, temp, 255, this.m_Path);
            return temp.ToString();
        }
    }
}
