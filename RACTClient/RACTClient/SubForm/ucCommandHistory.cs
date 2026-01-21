using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using RACTCommonClass;

namespace RACTClient
{
    public partial class ucCommandHistory : UserControl,IDropDownChild
    {
        public ucCommandHistory()
        {
            InitializeComponent();
        }

      

        public event EventHandler DropDownCanceled;

        public event EventHandler DropDownSelected;



        internal void InitializeControl(TelnetCommandHistoryInfoCollection aCollection)
        {
            AppGlobal.InitializeGridStyle(fgCommand);

            fgCommand.Rows.Count = 1;
            TelnetCommandHistoryInfo tHistory;
            for (int i = 0; i < aCollection.Count; i++)
            {
                tHistory = aCollection[i] as TelnetCommandHistoryInfo;
                fgCommand.Rows.Count++;
                fgCommand[i + 1, 0] = i + 1;
                fgCommand[i + 1, 1] = tHistory.Time.ToString("yyyy-MM-dd HH:mm:ss");
                fgCommand[i + 1, 2] = tHistory.Command;
            }
        }
    }
}
