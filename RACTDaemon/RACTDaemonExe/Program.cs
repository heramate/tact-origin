using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RACTDaemonExe
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main(string[] arg)
        {
            try
            {
                //string aServerlIP, int aServerPort, string aServerChannelName,string aLocallIP, int aLocalPort
                if (arg.Length == 5)
                {
                    Application.Run(new Form1(arg[0].Trim(), int.Parse(arg[1].Trim()), arg[2], arg[3], int.Parse(arg[4])));
                }
                else
                {
                    MessageBox.Show("Daemon 시작에 필요한 파라미터 인자가 올바르지 않습니다. Daemon을 시작할 수 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}