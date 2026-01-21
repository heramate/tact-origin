using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using RACTCommonClass;
using System.Windows.Forms;

namespace RACTClient
{
    /// <summary>
    /// LogWrite에 대한 요약 설명입니다.
    /// </summary>
    public class CommandWriter
    {

        public static void WriteCommandLog(DBExecuteCommandLogInfo aCommandLog)
        {
            try
            {
				// 2019-11-10 개선사항 (로그 저장 경로 개선)
                string tLogPath = AppGlobal.s_ClientOption.LogPath + "CommandLog\\";

                DirectoryInfo di = new DirectoryInfo(tLogPath);
                if (di.Exists != true) Directory.CreateDirectory(tLogPath);

                string tLogFile = "";

                
                if (aCommandLog.DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET)
                {
                    if (AppGlobal.s_ClientOption.TerminalDisplayNameType == E_TerminalDisplayNameType.IPAddress)
                    {
                        tLogFile = DateTime.Now.ToString("yyyy-MM-dd") + "_" + aCommandLog.DeviceInfo.IPAddress + "_" + aCommandLog.ConnectionLogID + ".log";
                    }
                    else
                    {
                        tLogFile = DateTime.Now.ToString("yyyy-MM-dd") + "_" + aCommandLog.DeviceInfo.Name + "_" + aCommandLog.ConnectionLogID + ".log";
                    }
                }
                else if (aCommandLog.DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
                {
                    // 2013-03-06 - shinyn - SSH텔넷인 경우 분기 추가
                    if (AppGlobal.s_ClientOption.TerminalDisplayNameType == E_TerminalDisplayNameType.IPAddress)
                    {
                        tLogFile = DateTime.Now.ToString("yyyy-MM-dd") + "_" + aCommandLog.DeviceInfo.IPAddress + "_" + aCommandLog.ConnectionLogID + ".log";
                    }
                    else
                    {
                        tLogFile = DateTime.Now.ToString("yyyy-MM-dd") + "_" + aCommandLog.DeviceInfo.Name + "_" + aCommandLog.ConnectionLogID + ".log";
                    }
                }
                else
                {
                    tLogFile = DateTime.Now.ToString("yyyy-MM-dd")+ "_"+aCommandLog.DeviceInfo.TerminalConnectInfo.SerialConfig.PortName + "_" + aCommandLog.ConnectionLogID + ".log";
                }

                if (!Directory.Exists(tLogPath))
                {
                    Directory.CreateDirectory(tLogPath);
                }
            
                FileStream tFileStream = new FileStream(tLogPath + tLogFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                StreamWriter tStreamWriter = new StreamWriter(tFileStream);
                tStreamWriter.Write("[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "]  " + aCommandLog.Command + Environment.NewLine);
                tStreamWriter.Close();
                tFileStream.Close();
            }
            catch { }
        }


      

    }
}