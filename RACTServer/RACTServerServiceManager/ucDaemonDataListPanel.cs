using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using RACTCommonClass;

namespace RACTServerServiceManager
{
    public delegate void DisaplayHandler();
    public partial class ucDaemonDataListPanel : UserControl
    {
        public ucDaemonDataListPanel()
        {
            InitializeComponent();
        }

        internal void InitializeControl()
        {
        }

        public void SetGridRowCount(int aCount)
        {
            fgDaemonStatusList.Rows.Count = aCount;
        }

        /// <summary>
        /// Daemon 정보를 Display 합니다.
        /// </summary>
        public void DisplayList()
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new DisaplayHandler(DisplayList));
                    return;
                }

                int tRowIndex = 1;
                fgDaemonStatusList.Rows.Count = ServiceManagerGlobal.s_RunningDaemonList.Count + 1;

                foreach (DaemonProcessInfo tDaemonProcess in ServiceManagerGlobal.s_RunningDaemonList)
                {
                    fgDaemonStatusList[tRowIndex, "IP"] = tDaemonProcess.IP;
                    fgDaemonStatusList[tRowIndex, "Port"] = tDaemonProcess.Port;
                    fgDaemonStatusList[tRowIndex, "UserCount"] = tDaemonProcess.ConnectUsercount;
                    fgDaemonStatusList[tRowIndex, "SessionCount"] = tDaemonProcess.TelnetSessionCount;
                    fgDaemonStatusList.Rows[tRowIndex].UserData = tDaemonProcess;
                    tRowIndex++;
                }
            }
            catch(Exception ex)
            {
            }
        }
    }
}
