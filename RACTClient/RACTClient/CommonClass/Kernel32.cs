using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace RACTClient
{
    internal class Kernel32
    {
        public static int MAX_PATH = 256;

        [DllImport("kernel32.dll", EntryPoint = "GetProfileStringA")]
        public static extern int GetProfileString(string lpszSection, string lpszKeyName, string lpszDefault, StringBuilder lpszReturn, int nSize);

        [DllImport("kernel32.dll", EntryPoint = "WriteProfileStringA")]
        public static extern int WriteProfileString(string lpszSection, string lpszKeyName, string lpszString);

        [DllImport("kernel32.dll", EntryPoint = "GetProfileIntA")]
        public static extern int GetProfileInt(string lpszSection, string lpszKeyName, int nDefault);

        [DllImport("kernel32.dll", EntryPoint = "WriteProfileIntA")]
        public static extern bool WriteProfileInt(string lpszSection, string lpszKeyName, int nValue);
    }
}
