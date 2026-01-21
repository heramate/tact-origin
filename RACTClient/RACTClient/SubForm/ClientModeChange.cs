using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RACTClient
{
    public partial class ClientModeChange : BaseForm
    {
        public ClientModeChange()
        {
            InitializeComponent();
        }

        public void InitializeControl()
        {
            AddButton(E_ButtonType.Close, E_ButtonSide.Right, "닫기");
            AddButton(E_ButtonType.OK, E_ButtonSide.Right, "확인");

            if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online)
            {
                rdoOnline.Checked = true;
            }
            else
            {
                rdoConsole.Checked = true;
            }
        }

        public E_RACTClientMode ClientMode
        {
            get
            {
                return rdoOnline.Checked ? E_RACTClientMode.Online : E_RACTClientMode.Console;
            }
        }

        protected override void ButtonProcess(E_ButtonType aButtonType)
        {
            if (aButtonType == E_ButtonType.OK)
            {
                if (AppGlobal.s_RACTClientMode != ClientMode)
                {
                    string tMessageString = "";
                    if (ClientMode == E_RACTClientMode.Console)
                    {
                        tMessageString = "Daemon과 연결된 Telnet Session은 종료 됩니다.\n";
                    }

                    tMessageString += ClientMode.ToString() + " 모드로 변경 하시겠습니까?";


                    if (AppGlobal.ShowMessageBox(this, tMessageString, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        AppGlobal.s_RACTClientMode = ClientMode;
                        DialogResult = DialogResult.Yes;
                    }

                }
            }
            this.Close();
        }

        private void rdoOnline_CheckedChanged(object sender, EventArgs e)
        {
            //txtID.Enabled = rdoOnline.Checked;
            //txtPW.Enabled = rdoOnline.Checked;
            //txtServerIP.Enabled = rdoOnline.Checked;

            //lblPW.Enabled = rdoOnline.Checked;
            //lblServerIP.Enabled = rdoOnline.Checked;
            //lblUserID.Enabled = rdoOnline.Checked;
        }
    }
}