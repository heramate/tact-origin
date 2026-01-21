using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Threading; //Thread
//using MKLibrary.MKData;
using RACTCommonClass;
using System.Net;//IPAddress
using System.Diagnostics;//StackTrace
using System.Text;//StringBuilder

namespace TACTRestartService
{
    public static class GlobalClass
    {
        /// <summary>
        /// 서버의 버전 정보 입니다.
        /// </summary>
        //public const string c_Version = "1.0.0.1";
        /// <summary>
        /// 서버 시작 위치입니다.
        /// </summary>
        public static string m_StartupPath = string.Empty;
        /// <summary>
        /// 서버 환경 설정 XML 파일명
        /// </summary>
        public const string c_SystemConfigFileName = @"\SystemConfig.xml";
        /// <summary>
        /// 서버 환경 설정 정보입니다.
        /// </summary>
        public static SystemConfig m_SystemInfo = null;

        /// <summary>
        /// 감시대상 서비스 정보 설정 XML 파일명
        /// </summary>
        public const string c_ServiceListFileName = @"\ServiceList.xml";

        /// <summary>
        /// 파일로그 저장 프로세서 입니다.
        /// </summary>
        public static FileLogProcess m_LogProcess = null;
        /// <summary>
        /// 파일로그에 모든 정보를 출력할지 여부 (GlobalClass.PrintLogXXX() 에서 플래그값 사용)
        /// </summary>
        public static bool m_IsFileLogDetailYN = false;
        /// <summary>
        /// 서버기동 정지 여부 입니다.
        /// </summary>
        public static bool m_IsServerStop = false;

        /// <summary>
        /// 쓰레드를 강제 종료합니다.
        /// </summary>
        /// <param name="aThread"></param>
        public static void StopThread(Thread aThread)
        {
            if (aThread == null) return;

            //종료대기
            aThread.Join(100);
            if (aThread.IsAlive)
            {
                try
                {
                    //강제종료
                    aThread.Abort();
                }
                catch (Exception e)
                {
                    Thread.ResetAbort();
                }
            }
            aThread = null;
        }
        

        public static void PrintLogError(string aLogMessage, bool aIsFile = true) { PrintLog(E_FileLogType.Error, aLogMessage, aIsFile); }
        public static void PrintLogInfo(string aLogMessage, bool aIsFile = true) { PrintLog(E_FileLogType.Info, aLogMessage, aIsFile); }
        public static void PrintLogException(string aMsgTitle, Exception e, bool aIsFile = true)
        {
            PrintLog(E_FileLogType.Error, aMsgTitle + (e == null ? string.Empty : e.ToString()), aIsFile);
        }
        public static void PrintLog(E_FileLogType aLogType, string aLogMessage, bool aIsFile = true)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine(aLogMessage);
#endif
            if (m_IsFileLogDetailYN || aIsFile)
            {
                if (GlobalClass.m_LogProcess == null) return;
                GlobalClass.m_LogProcess.PrintLog(aLogType, aLogMessage);
            }
        }


    } // End of class GlobalClass


    public static class Util
    {
        public static string QuotedStr(string aValue, string ends = "\'")
        {
            return ends + aValue + ends;
        }

        public static string DateTimeToDBValue(DateTime aValue)
        {
            return aValue == DateTime.MinValue ? "null" : Util.QuotedStr(aValue.ToString("yyyy-MM-dd HH:mm:ss"));
        }


        public static string QuotedStrOrNull(string aValue, string ends = "\'")
        {
            return (string.IsNullOrEmpty(aValue) ? "null" : ends + aValue + ends);
        }

        public static bool IsValidIPAddress(string aIPString)
        {
            if (aIPString == null || aIPString.Length < 1) return false;

            IPAddress ipAddr;
            return IPAddress.TryParse(aIPString, out ipAddr);
        }

        public static void Assert(bool aCondition, string aMessage)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(aCondition, aMessage);
#endif
            if (!aCondition) GlobalClass.PrintLogError(aMessage);
        }

    } // End of class Util
}
