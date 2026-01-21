using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace RACTClient
{
    public partial class BatchCmdForm : Form
    {

        public delegate void SendBatchCommand(string cmd , decimal CycleTime);
        public event SendBatchCommand SendBatchCommandFunction;
        private int iStartIdx = 0;
        private int iStopIdx = 0;
        /// <summary>
        /// 부모 입니다.
        /// </summary>
        private MCTerminalEmulator m_Parent;

        public BatchCmdForm(MCTerminalEmulator aMCTerminalControl)
        {
            InitializeComponent();
            m_Parent = aMCTerminalControl;
        }

        private void OnClick_Btn_Submit(object sender, EventArgs e)
        {
            string limitCmd = m_Parent.IsLimitCmdByBatch(rTxtBatchCmd.Text);
            if (limitCmd == "")
            {
                if (AppGlobal.ShowMessageBox(this, "배치를 실행하시겠습니까?", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    this.Close();
                    SendBatchCommandFunction(rTxtBatchCmd.Text, nmSendTerm.Value);
                }
            }else{
                Regex regex = new Regex(limitCmd);

                MatchCollection mc = regex.Matches(rTxtBatchCmd.Text);
                int iCursorPosition = rTxtBatchCmd.SelectionStart;

                foreach (Match m in mc)
                {
                    iStartIdx = m.Index;
                    iStopIdx = m.Length;

                    rTxtBatchCmd.Select(iStartIdx, iStopIdx);
                    rTxtBatchCmd.SelectionColor = Color.Red;
                    rTxtBatchCmd.SelectionStart = iCursorPosition;
                    rTxtBatchCmd.SelectionColor = Color.Black;
                }
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnKeyDownEvent(object sender, KeyEventArgs e)
        {
          /*if (iStartIdx > 0 || iStopIdx > 0)
            {
                rTxtBatchCmd.Select(iStartIdx, iStopIdx);
                rTxtBatchCmd.SelectionColor = Color.Black;
                iStartIdx = 0;
                iStopIdx = 0;
            }*/

            //2015-10-30 hanjiyeon 수정. (전체 선택 후 입력 시 기존 문자열이 삭제되지 않아 처리 코드를 변경함.)
                
                rTxtBatchCmd.SelectionColor = Color.Black;
                
                
            
        }

    }
}
