using System;
using System.Collections.Generic;
using System.Text;
using MKLibrary.MKNetwork;
using MKLibrary.MKLog;
using System.Collections;
using MKLibrary.MKData;
using System.Windows.Forms;
using System.IO;
using RACTDaemonProcess;//DaemonConfig

namespace RACTDaemonLauncher
{
    public static class AppGlobal
    {
        /// <summary>
        /// 시스템 정보 입니다.
        /// 2019.01.25 KwonTaeSuk 환경설정파일 정리(DaemonLauncherConfig.xml, DaemonProcessConfig.xml)
        /// </summary>
        public static DaemonConfig s_SystemInfo = null;

        /// <summary>
        /// Remote 통신을 위한 Gateway입니다.
        /// </summary>
        public static MKRemote m_RemoteGateway = null;
        /// <summary>
        /// 사용자 Login 및 System Log를 저장할 파일입니다.
        /// </summary>
        public static MKFileLog s_FileLog = null;

        /// <summary>
        /// 서버 System정보를 가져오기 합니다.
        /// </summary>
        /// <returns>서비 System정보 가져오기의 성공 여부 입니다.</returns>
        public static bool LoadSystemInfo()
        {
            ArrayList tSystemInfos = null;
            MKXML tXML = new MKXML();
            E_XmlError tErrorString;

            AppGlobal.s_FileLog.PrintLogEnter(string.Format("환경 정보 파일({0})을 로드 합니다. ", DaemonConfig.s_DaemonConfigFileName));
            FileInfo tFileInfo = new FileInfo(Application.StartupPath + "\\" + DaemonConfig.s_DaemonConfigFileName);
            if (!tFileInfo.Exists)
            {
                MKXML.ObjectToXML(tFileInfo.FullName, new DaemonConfig());
                AppGlobal.s_FileLog.PrintLogEnter(string.Format("환경정보 파일({0})이 없어 새로 생성하였습니다", tFileInfo.FullName));
                return false;
            }

            try
            {
                tSystemInfos = MKXML.ObjectFromXML(Application.StartupPath + "\\" + DaemonConfig.s_DaemonConfigFileName, typeof(DaemonConfig), out tErrorString);
                if (tSystemInfos == null) return false;
                if (tSystemInfos.Count == 0) return false;
                AppGlobal.s_SystemInfo = (DaemonConfig)tSystemInfos[0];

                return true;
            }
            catch (Exception ex)
            {
                AppGlobal.s_FileLog.PrintLogEnter(ex.ToString());
                return false;
            }
        }
        /// <summary>
        /// 환경 정보를 저장 합니다.
        /// </summary>
        public static void SaveConfig(HealthCheckProcessCollection aHealthCheckProcessCollection)
        {
            MKXML tXML = new MKXML();

            IniFile tIni = new IniFile(Application.StartupPath + "\\RACTDaemonProcessList.ini");
            foreach (HealthCheckProcess tProcessInfo in aHealthCheckProcessCollection.Values)
            {
                tIni.WriteValue(tProcessInfo.HealthCheckItem.Key, "Auto Start", Convert.ToInt32(tProcessInfo.HealthCheckItem.AutoStart).ToString());
                tIni.WriteValue(tProcessInfo.HealthCheckItem.Key, "Process ID", tProcessInfo.HealthCheckItem.ProcessID.ToString());
            }

            //MKXML.ObjectToXML(Application.StartupPath + "\\"+DaemonLauncherConfig.s_DaemonLauncherConfigFileName, AppGlobal.s_SystemInfo);
        }

        /// <summary>
        /// 로그파일 및 디버그 메시지를 출력합니다.
        /// </summary>
        /// <param name="aLogMessage">출력할 로그메시지</param>
        /// <param name="aIsPrintDebugMsg">디버그창에 출력 여부</param>
        public static void PrintLogEnter(string aLogMessage, bool aIsPrintDebugMsg = false)
        {
            if (AppGlobal.s_FileLog != null) {
                AppGlobal.s_FileLog.PrintLogEnter(aLogMessage);
            }
            if (aIsPrintDebugMsg) {
                System.Diagnostics.Debug.WriteLine(aLogMessage);
            }
        }
    }
}
