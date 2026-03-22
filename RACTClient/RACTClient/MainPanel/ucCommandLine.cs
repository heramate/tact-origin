using DevComponents.DotNetBar;
using RACTClient.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RACTClient
{
    public partial class ucCommandLine : UserControl, IMainPanel
    {
        /// <summary>
        /// 명령 리스트 입니다.
        /// </summary>
        private TerminalSelection m_TerminalList = new TerminalSelection();
        /// <summary>
        /// 팝업 창 입니다.
        /// </summary>
        private MKDropDown m_DetailsInfo;


        public ucCommandLine()
        {
            InitializeComponent();
        }

        public void InitializeControl()
        {
            AppGlobal.InitializeButtonStyle(btnEdit);
            m_DetailsInfo = new MKDropDown(m_TerminalList);
            m_TerminalList.lstTerminal.ItemCheck += new ItemCheckEventHandler(lstTerminal_ItemCheck);
            m_DetailsInfo.Closed += new ToolStripDropDownClosedEventHandler(m_DetailsInfo_Closed);
            btnEdit.Enabled = false;
            mcSmallTerminal1.Status(switchButton1.Value);
        }

        void m_DetailsInfo_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            ChangeSendTerminalCount();
        }

        void lstTerminal_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // ChangeSendTerminalCount();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {

            if (m_TerminalList.lstTerminal.Items.Count == 0) return;

            int tVisibleItemCount = Math.Min(m_TerminalList.lstTerminal.Items.Count, 12);
            int tPopupWidth = btnEdit.Width;
            for (int i = 0; i < m_TerminalList.lstTerminal.Items.Count; i++)
            {
                int tItemWidth = TextRenderer.MeasureText(m_TerminalList.lstTerminal.Items[i].Text, m_TerminalList.lstTerminal.Font).Width + 48;
                tPopupWidth = Math.Max(tPopupWidth, tItemWidth);
            }
            int tPopupHeight = ((tVisibleItemCount + 1) * m_TerminalList.lstTerminal.ItemHeight) + SystemInformation.HorizontalScrollBarHeight + 4;
            Size tPopupSize = new Size(tPopupWidth, tPopupHeight);

            m_TerminalList.Size = tPopupSize;
            m_DetailsInfo.Size = tPopupSize;

            Point tPoint = new Point(Control.MousePosition.X, Control.MousePosition.Y);
            m_DetailsInfo.Show(tPoint, ToolStripDropDownDirection.Default);
        }





        /// <summary>
        /// 전송할 터미널을 수정 합니다.
        /// </summary>
        /// <param name="aTerMinalName"></param>
        private void AddTerminal(string aTerMinalName)
        {
            // 1. Count를 사용하여 리스트 항목 개수만큼 반복
            int itemCount = m_TerminalList.lstTerminal.Items.Count;
            for (int i = 0; i < itemCount; i++)
            {
                // 2. 인덱서로 접근하여 텍스트 비교
                // lstTerminal.Items[i]가 ListViewItem 타입인지, 혹은 별도 객체인지 확인이 필요합니다.
                // 일반적인 ListView라면 아래처럼 접근 가능합니다.
                if (m_TerminalList.lstTerminal.Items[i].Text.Equals(aTerMinalName))
                {
                    return; // 중복 발견 시 메서드 종료
                }
            }

            m_TerminalList.lstTerminal.Items.Add(aTerMinalName);
        }
        /// <summary>
        /// 전송할 터미널을 삭제 합니다.
        /// </summary>
        /// <param name="aTerMinalName"></param>
        private void RemoveTerminal(string aTerMinalName)
        {

            for (int i = m_TerminalList.lstTerminal.Items.Count - 1; i >= 0; i--)
            {
                if (m_TerminalList.lstTerminal.Items[i].Text.Equals(aTerMinalName))
                {
                    m_TerminalList.lstTerminal.Items.RemoveAt(i);
                    ChangeSendTerminalCount();
                    break;
                }
            }
        }

        void tCheckBox_CheckedChanged(object sender, CheckBoxChangeEventArgs e)
        {
            ChangeSendTerminalCount();

        }



        private List<string> m_CheckList = new List<string>();

        private void ChangeSendTerminalCount()
        {
            int tCount = 0;
            m_CheckList.Clear();
            for (int i = m_TerminalList.lstTerminal.Items.Count - 1; i >= 0; i--)
            {
                if (m_TerminalList.lstTerminal.GetItemChecked(i))
                {
                    m_CheckList.Add(m_TerminalList.lstTerminal.Items[i].Text);
                    tCount++;
                }
            }

            switchButton1.Value = true;
            if (tCount == m_TerminalList.lstTerminal.Items.Count)
            {
                btnEdit.Text = "모든 터미널 전송";
            }
            else if (tCount == 0)
            {
                switchButton1.Value = false;
                btnEdit.Text = "0개 터미널 전송";
            }
            else
            {
                btnEdit.Text = string.Format("{0}개 터미널 전송", tCount);
            }
            mcSmallTerminal1.SetReceiveList(m_CheckList);


        }


        private void btnAll_Click(object sender, EventArgs e)
        {
            try
            {
                int tCount = 0;

                if (AppGlobal.ShowMessageBox(null, "전체 터미널을 선택하시겠습니까?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    m_CheckList.Clear();
                    for (int i = m_TerminalList.lstTerminal.Items.Count - 1; i >= 0; i--)
                    {
                        m_TerminalList.lstTerminal.SetItemChecked(i, true);
                        m_CheckList.Add(m_TerminalList.lstTerminal.Items[i].Text);
                        tCount++;
                    }

                    switchButton1.Value = true;
                    if (tCount == m_TerminalList.lstTerminal.Items.Count)
                    {
                        btnEdit.Text = "모든 터미널 전송";
                    }
                    else if (tCount == 0)
                    {
                        switchButton1.Value = false;
                        btnEdit.Text = "0개 터미널 전송";
                    }
                    else
                    {
                        btnEdit.Text = string.Format("{0}개 터미널 전송", tCount);
                    }
                    mcSmallTerminal1.SetReceiveList(m_CheckList);
                }
            }
            catch (Exception ex)
            {
                AppGlobal.s_FileLogProcessor.PrintLog("CommandLine : btnAll_Click Error " + ex.Message.ToString());
            }
        }

        internal void TerminalChange(E_TerminalStatus aWorkType, string aTerminalName)
        {
            if (!this.EnsureUiThread(() => TerminalChange(aWorkType, aTerminalName)))
            {
                return;
            }

            try
            {
                switch (aWorkType)
                {
                    case E_TerminalStatus.Add:
                        AddTerminal(aTerminalName);
                        break;
                    case E_TerminalStatus.Delete:
                    case E_TerminalStatus.Disconnected:
                        RemoveTerminal(aTerminalName);
                        break;
                    case E_TerminalStatus.Connection:
                        bool TmepFlag = true;
                        for (int i = m_TerminalList.lstTerminal.Items.Count - 1; i >= 0; i--)
                        {
                            if (m_TerminalList.lstTerminal.Items[i].Text.Equals(aTerminalName))
                            {
                                TmepFlag = false;
                            }
                        }

                        if (TmepFlag)
                            AddTerminal(aTerminalName);
                        break;
                }
                ChangeSendTerminalCount();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        private void switchButton1_ValueChanged(object sender, EventArgs e)
        {
            btnEdit.Enabled = switchButton1.Value;
            btnAll.Enabled = switchButton1.Value;
            mcSmallTerminal1.Status(switchButton1.Value);
        }

        private void mcSmallTerminal1_Load(object sender, EventArgs e)
        {

        }

        public void sTerminalApplyOption()
        {
            mcSmallTerminal1.ApplyOption();
        }


    }
}
