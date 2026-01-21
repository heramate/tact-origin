using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Forms;


namespace AutoUpdate
{
    static class Program
    {
        
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        static _Log log;
        [STAThread]
        static void Main()
        {
            log = new _Log("AutoUpdate", Application.StartupPath + "\\Log");
            log.setDebugState(true);            

            ModUpdate update = new ModUpdate(Application.StartupPath);

            try
            {
                log.SetLog("Update Main : Config(CLParam) Load ");
                update.GetAppInfo();           // 현재 프로그램 환경정보 로드

                log.SetLog("Update Main : Config(Update Info) Load ");
                update.GetUpdateAppInfo(ModUpdate.E_ExeType.Update);

                if (update.IsUpdate(ModUpdate.E_ExeType.Update) == true)  // 업데이트 할 필요가 있으면
                {
                    log.SetLog("Update Main : Update Exist ");
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new frmMain(update));
                }
                else  // 업데이트 할 필요가 없으면.
                {
                    log.SetLog("Update Main : Update Not Exist, Try To Strat TACTClient.exe Program ");
                    if (IsAdministrator() == false)
                    {
                        try
                        {
                            ProcessStartInfo procInfo = new ProcessStartInfo();
                            procInfo.UseShellExecute = true;
                            procInfo.FileName = update.exeFileName;
                            procInfo.Arguments = update.exeFileArgs;
                            procInfo.WorkingDirectory = Environment.CurrentDirectory;
                            procInfo.Verb = "runas";
                            Process.Start(procInfo);

                        }
                        catch (Exception ex)
                        {
                            log.SetLog("Update Main : Error " + ex.Message);
                            //MessageBox.Show(ex.Message.ToString());
                        }
                    }else
                        System.Diagnostics.Process.Start(Application.StartupPath + "\\" +update.exeFileName, update.exeFileArgs);
                }
            }
            catch (Exception e)
            {
                log.SetLog("Update Main:" + e.Message);
                //MessageBox.Show(e.Message, "업데이트 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();

            if (identity != null)
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            return false;
        }
    }
}