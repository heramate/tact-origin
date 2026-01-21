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

namespace RACTClient.SubForm
{
    public partial class AutoCompleteKey : Form
    {
        public string keyText;
        public int modelID;
        public delegate void SetAutoCompleteCmd (string cmd);
        public event SetAutoCompleteCmd SetAutoCompleteKey;

        public AutoCompleteKey()
        {
            InitializeComponent();
        }

        private void AutoCompleteKey_OnLoad(object sender, EventArgs e)
        {
            this.atkListBox.MouseLeave += new EventHandler(lbResult_MouseLeave);
            this.atkListBox.KeyUp += new KeyEventHandler(lbResult_KeyUp);
            this.atkListBox.Click += new EventHandler(lbResult_Click);

            this.atkListBox.Items.Clear();

            if (AppGlobal.s_AutoCompleteCmdList.Contains(modelID))
            {
                AutoCompleteCmdInfo tAutoCompleteCmdInfo = AppGlobal.s_AutoCompleteCmdList[modelID];

                ArrayList AutoCompleteCmdList = tAutoCompleteCmdInfo.Command;

                for (int i = 0; i < AutoCompleteCmdList.Count; i++)
                {
                    string cmd = (string)AutoCompleteCmdList[i];
                    if (cmd.StartsWith(keyText))
                    {
                        atkListBox.Items.Add(cmd);
                    }
                    if (i == 99)
                        break;
                }
            }

        }


        //2015-08-22 hanjiyeon 추가 Testcode
        /// <summary>
        /// listbox - 마우스 클릭 시의 처리입니다. (선택된 아이템을 표시하고 focus를 설정합니다.)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbResult_Click(object sender, EventArgs e)
        {
            if (atkListBox.SelectedIndex >= 0)
            {
                if (atkListBox.SelectedItem != null)
                {
                    string str = atkListBox.SelectedItem.ToString();
                    SetAutoCompleteKey(str);
                    this.Close();
                }
            }
        }

        //2015-08-22 hanjiyeon 추가 Testcode
        /// <summary>
        /// listbox - 키보드 key up 시의 처리입니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbResult_KeyUp(object sender, KeyEventArgs e)
        {
            //Enter 키 입력 후 선택된 아이템을 표시 (focus를 설정하지 않으면 focus가 터미널로 오지 않음.)


            if (e.KeyCode == Keys.Enter)
            {
                if (atkListBox.SelectedItem != null)
                {
                    string str = atkListBox.SelectedItem.ToString();
                    SetAutoCompleteKey(str);
                    this.Close();
                }

            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }

        }


        /// <summary>
        /// 마우스 위치를 벗어나면 자동완성문자열 창 숨기기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbResult_MouseLeave(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
