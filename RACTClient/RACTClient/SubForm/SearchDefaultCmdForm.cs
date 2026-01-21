using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RACTCommonClass;
using System.Collections;

namespace RACTClient
{
    public partial class SearchDefaultCmdForm : Form
    {
        private int beforeMouse = 0;
        public int modelID;
        public delegate void SendDefaultCommand (string cmd);
        public event SendDefaultCommand SendCmd;


        public SearchDefaultCmdForm()
        {
            InitializeComponent();
        }



        /// <summary>
        /// Gunny [기본명령어 리스트를 가져옴] 현재 명령어 받음 
        /// </summary>
        private bool GetDefaultCmd(String lineCmd)
        {
            bool result = false;

            grdCmdList.Rows.Count = 1;

            if (AppGlobal.s_DefaultCmdInfoList.Contains(modelID))
            {
                DefaultCmdInfo tDefaultCmdInfo = AppGlobal.s_DefaultCmdInfoList[modelID];

                ArrayList DefaultCmdList = tDefaultCmdInfo.Command;
                ArrayList DefaultDescriptionList = tDefaultCmdInfo.Description;

                //Console.WriteLine("모델 ID : " + modelID + " 명령어 " + lineCmd);

                int matchCnt = 0;
                for (int i = 0; i < DefaultCmdList.Count; i++)
                {
                    string cmd = (string)DefaultCmdList[i];

                    if (cmd.Contains(lineCmd))
                    {
                        
                        string desc = (string)DefaultDescriptionList[i];

                        grdCmdList.Rows.Add();
                        grdCmdList[matchCnt + 1, "colETC"] = desc;
                        grdCmdList[matchCnt + 1, "colCmd"] = cmd;
                        grdCmdList.Rows[matchCnt + 1].UserData = desc;
                        matchCnt++;

                    }
                }
                
            }
            return result;
        }

        private void OnClickSearchDefaultCmd(object sender, EventArgs e)
        {
            GetDefaultCmd(txtSearch.Text.ToString());
        }

        private void OnLoadForm(object sender, EventArgs e)
        {
            if (txtSearch.Text.Length > 0)
            {
                GetDefaultCmd(txtSearch.Text.ToString());
            }
        }

        private void ExitForm(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnSelectItemByMouse(object sender, MouseEventArgs e)
        {
            int rowNum = grdCmdList.RowSel;
            if (rowNum > 0)
            {
                string DefaultCmd = (string)grdCmdList[rowNum, "colCmd"];
                SendCmd(DefaultCmd);
                this.Close();
            }
        }


        private void MouseMove(object sender, MouseEventArgs e)
        {
            string tip;
            if (grdCmdList.MouseRow == beforeMouse)
                return;
            if (grdCmdList.MouseRow > 0)
            {
                tip = (string)grdCmdList.Rows[grdCmdList.MouseRow].UserData;
                toolTip1.SetToolTip(grdCmdList, tip);
                beforeMouse = grdCmdList.MouseRow;
            }
        }

    }
}
