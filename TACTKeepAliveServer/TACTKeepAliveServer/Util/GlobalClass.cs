using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Threading; //Thread
using MKLibrary.MKData;
using RACTCommonClass;
using System.Net;//IPAddress
using System.Diagnostics;//StackTrace
using System.Text;//StringBuilder

namespace TACT.KeepAliveServer
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
        /// 서버 환경 설정이 저장된 XML 파일명입니다.
        /// </summary>
        public const string c_SystemConfigFileName = @"\KAMServerConfig.xml";
        /// <summary>
        /// 서버 환경 설정 정보입니다.
        /// </summary>
        public static SystemConfig m_SystemInfo = null;
        /// <summary>
        /// 파일로그에 모든 정보를 출력할지 여부 (GlobalClass.PrintLogXXX() 에서 플래그값 사용)
        /// </summary>
        public static bool m_FileLogDetailYN = false; //=SystemConfig.FileLogDetailYN

        /// <summary>
        /// 파일로그 저장 프로세서 입니다.
        /// </summary>
        public static FileLogProcess m_LogProcess = null;

        /// <summary>
        /// DB로그 저장 프로세서 입니다.
        /// </summary>
        public static DBLogProcess m_DBLogProcess = null;

        /// <summary>
        /// 서버기동 정지 여부 입니다.
        /// </summary>
        public static bool m_IsServerStop = false;
        

        /// <summary>
        /// RPC장비와 KeepAlive통신할 프로세서 입니다.
        /// </summary>
        public static KeepAliveCommProcess m_KeepAliveCommThread = null;
        /// <summary>
        /// RPC장비와 KeepAlive통신할 프로세서 입니다.
        /// </summary>
        public static DaemonCommProcess m_DaemonCommProcess = null;
        /// <summary>
        /// SSH터널 관리자
        /// </summary>
        public static SSHTunnelManager m_TunnelManager = null;
        


        /// <summary>
        /// LTE Cat.M1 장비 정보
        /// </summary>
        //public static KeepAliveCollection m_KeepAliveInfoCollection = new KeepAliveCollection();


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


        public static void PrintLogError(string aLogMessage, bool aDetailLogYN = false) { PrintLog(E_FileLogType.Error, aLogMessage, aDetailLogYN); }
        public static void PrintLogInfo(string aLogMessage, bool aDetailLogYN = false) { PrintLog(E_FileLogType.Info, aLogMessage, aDetailLogYN); }
        public static void PrintLogException(string aMsgTitle, Exception e, bool aDetailLogYN = false)
        {
            PrintLog(E_FileLogType.Error, aMsgTitle + (e == null ? string.Empty : e.ToString()), aDetailLogYN);
        }
        public static void PrintLog(E_FileLogType aLogType, string aLogMessage, bool aDetailLogYN = false)
        {
            if (GlobalClass.m_LogProcess == null) return;
            if (!aDetailLogYN || m_FileLogDetailYN)
            {
                GlobalClass.m_LogProcess.PrintLog(aLogType, aLogMessage);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(aLogMessage);
            }
        }


    } // End of class GlobalClass


    public static class Util
    {
        public static string c_DateTimeFormatMDHMS = "MM/dd HH:mm:ss";
        public static string c_DateTimeFormatYMDHMS = "yyyy-MM-dd HH:mm:ss";


        public static string QuotedStr(string aValue, string ends = "\'")
        {
            return ends + aValue + ends;
        }

        public static string DateTimeToDBValue(DateTime aValue)
        {
            return aValue == DateTime.MinValue ? "null" : Util.QuotedStr(aValue.ToString(c_DateTimeFormatYMDHMS));
        }

        public static string DateTimeToLogValue(DateTime aValue)
        {
            return aValue == DateTime.MinValue || aValue == DateTime.MaxValue ? "" : Util.QuotedStr(aValue.ToString(c_DateTimeFormatMDHMS));
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
            System.Diagnostics.Debug.Assert(aCondition, aMessage);
            if (!aCondition) GlobalClass.PrintLogError(aMessage);
        }

        /// <summary>
        /// [디버깅] byte array 각 값을 두 자리 숫자로 표시
        /// </summary>
        /// <param name="aBytes"></param>
        /// <param name="aNoDataString"></param>
        public static void PrintByteDatas(byte[] aBytes, string aNoDataString = "<none>")
        {
            if ((aBytes == null) || (aBytes.Length == 0))
            {
                System.Diagnostics.Debug.WriteLine(aNoDataString);
            }
            else
            {
                for (int i = 0; i < aBytes.Length; i++)
                {
                    System.Diagnostics.Debug.Write(string.Format("{0:00} ", aBytes[i]));//Console.Write("{0:X2} ", aBytes[i]);
                }
                System.Diagnostics.Debug.WriteLine(string.Empty);
            }
        }

        public static string ByteNumbersToString(byte[] aByteValues, char aSeperator)
        {
            string byteStr = string.Empty;
            if (aByteValues == null || aByteValues.Length == 0) return byteStr;
            foreach (byte byteValue in aByteValues)
            {
                byteStr += string.Format("{0:00}{1}", byteValue, aSeperator);
            }
            return byteStr;
        }

        #region Base64 인코딩/디코딩 ------------------------------------------------
        public static byte[] Base64Encode(string EncodingText, System.Text.Encoding oEncoding = null)
        {
            if (oEncoding == null)
                oEncoding = System.Text.Encoding.ASCII;
 
            byte[] arr = oEncoding.GetBytes(EncodingText);
            //return System.Convert.ToBase64String(arr);
            return arr;
        }
 
        public static string Base64Decode(string DecodingText, System.Text.Encoding oEncoding = null)
        {
            if (oEncoding == null)
                oEncoding = System.Text.Encoding.ASCII;

            byte[] arr = System.Convert.FromBase64String(DecodingText);
            string str = System.Convert.ToBase64String(arr);
            return oEncoding.GetString(arr);
        }
        #endregion Base64 인코딩/디코딩 ---------------------------------------------


    } // End of class Util
}
