using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using RACTClient;

namespace RACTOneTerminal
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

            //2017.06.21 - NoSeungPil - RCCS 로그인 기능추가
            //if (args.Length != 4)
            if (!((args.Length == 1) || (args.Length == 4) || (args.Length == 5) || (args.Length == 7)))
            {
                MessageBox.Show("인자가 잘 못 되었습니다.");
                return;
            }

            //if (args[1].Trim().Length == 0 || args[2].Trim().Length == 0 || args[3].Trim().Length == 0)
            foreach (string arg in args)
            {
                if (arg.Trim().Length == 0)
                {
                    MessageBox.Show("인자가 잘 못 되었습니다.");
                    return;
                }
            }

            string programArg = "";
            string[] programArgs;
            if (args.Length > 0)
            {
                if (args.Length == 1)
                {
                    string tmpArg = null;
                    tmpArg = args[0];
                    MessageBox.Show("인자가 값 확인 :" + tmpArg);
                    if (tmpArg.ToLower().Contains("skbtactone://"))
                    {
                        tmpArg = Regex.Replace(tmpArg, "skbtactone://", "", RegexOptions.IgnoreCase).TrimEnd('/');
                
                        programArg = tmpArg.Replace("%20", " ");
                        MessageBox.Show("인자가 처리 후 값 확인 :" + programArg);
                    }
                    else
                    {
                        MessageBox.Show("인자가 잘 못 되었습니다.");
                        return;
                    }
                }
                else
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (i == 0)
                        {
                            programArg = args[i];
                        }
                        else
                        {
                            programArg = programArg + " " + args[i];
                        }
                    }
                }
            }

            programArgs = programArg.Split(' ');

            try
            {
                //if (AppGlobal.s_FileLogProcessor != null) AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "RACTClient 업데이트 확인.");

                DirectoryInfo directory = new DirectoryInfo(Application.StartupPath + "\\Tmp");

                ModUpdate update = new ModUpdate(Application.StartupPath);
                update.GetAppInfo();
                /* RACTOneTerminal은 업데이트 만 체크하도록 
                update.GetUpdateAppInfo(ModUpdate.E_ExeType.Install);
                // 업데이트 필요여부 확인
                if (update.IsUpdate(ModUpdate.E_ExeType.Install) == true)
                {
                    //Directory.Delete("Tmp", true);
                    foreach (FileInfo file in directory.GetFiles())
                    {
                        file.Delete();
                    }

                    //TACTClient or RACTOneTerminal를 실행 할 수 있어 각 해당 프로세스의 Program에서 ProgramName을 Write해준다 
                    update.iniWriteValue("Program", "ProgramName", "RACTOneTerminal.exe");
                    if (args.Length >= 4)
                        update.iniWriteValue("Program", "ProgramArgs", programArgs);


                    //로그기록 사용시 자원해제하지 않으면 프로세스가 정상종료 되지 않음
                    //AppGlobal.s_FileLogProcessor.Stop();
                    System.Diagnostics.Process.Start(update.exeInstallName);
                    return;
                }
                */
                update.GetUpdateAppInfo(ModUpdate.E_ExeType.Update);
                //MessageBox.Show("업데이트 확인 1 :");
                if (update.IsUpdate(ModUpdate.E_ExeType.Update) == true)
                {
                    //Directory.Delete("Tmp", true);
                    foreach (FileInfo file in directory.GetFiles())
                    {
                        file.Delete();
                    }

                    update.iniWriteValue("Program", "ProgramName", "TACTOneTerminal.exe");
                    if (programArgs.Length >= 4)
                        update.iniWriteValue("Program", "ProgramArgs", programArg);

                   
                    //MessageBox.Show("업데이트 확인 2 :");
                    System.Diagnostics.Process.Start(Application.StartupPath + "\\" + update.exeUpdateName);

                    //AppGlobal.s_FileLogProcessor.Stop();
                    return;
                }
                else
                {
                    //MessageBox.Show("업데이트 확인 3 :");
                    foreach (FileInfo file in directory.GetFiles())
                    {
                        file.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                //if (AppGlobal.s_FileLogProcessor != null)
                //{
                //    AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, "RACTClient 업데이트 실패." + ex.ToString());
                //    AppGlobal.s_FileLogProcessor.Stop();
                //}
                System.Diagnostics.Debug.WriteLine("업데이트 오류" + ex.ToString());
            }


            if (programArgs.Length == 7)
            {
                //RCCS 로그인의 경우
                if (programArgs[4] == "1")
                {
                    AppGlobal.s_ConnectionMode = int.Parse(programArgs[4]);
                    AppGlobal.s_RCCSIP = args[5];
                    if (!int.TryParse(programArgs[6], out AppGlobal.s_RCCSPort))
                    {
                        MessageBox.Show("인자(RCCS Port)를 확인해 주십시요.");
                        return;
                    }
                }
                else if (programArgs[4] == "2")
                {
                    AppGlobal.s_ConnectionMode = int.Parse(programArgs[4]);
                    AppGlobal.s_RPCSIP = programArgs[5];
                    if (!int.TryParse(args[6], out AppGlobal.s_RPCSPort))
                    {
                        MessageBox.Show("인자(RPCS Port)를 확인해 주십시요.");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("인자(접속구분)를 확인해 주십시요.");
                    return;
                }
            }
            else if (programArgs.Length == 5)
            {
                AppGlobal.s_ConnectionMode = int.Parse(programArgs[4]);
            }

            //RPCS 접속 시도시 사용자로 하여금 한번더 체크하도록
            if (AppGlobal.s_ConnectionMode == 3)
            {
                if (MessageBox.Show("RPCS(무선) 장비 접속시 LTE망 과금이 발생합니다.\r\n접속 하시겠습니까?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Application.Run(new ucOneTerminal(programArgs[0], programArgs[1], programArgs[2], programArgs[3]));
                }
            }
            else
                Application.Run(new ucOneTerminal(programArgs[0], programArgs[1], programArgs[2], programArgs[3]));
    
        }
    }
}