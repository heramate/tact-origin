using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace RACTClient
{
    public partial class SelectTargetTerminal : BaseForm
    {
        /// <summary>
        /// 터미널 목록 입니다.
        /// </summary>
        private List<ITactTerminal> m_TerminalList = null;
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public SelectTargetTerminal()
        {
            InitializeComponent();
            initializeControl();
        }
       
        /// <summary>
        /// 컨트롤을 초기화 합니다.
        /// </summary>
        private void initializeControl()
        {
            AddButton(E_ButtonType.Close, E_ButtonSide.Right, "닫기");
            AddButton(E_ButtonType.OK, E_ButtonSide.Right, "확인");

           

        }
        /// <summary>
        /// 터미널 목록을 가져오거나 설정 합니다.
        /// </summary>
        public List<ITactTerminal> TerminalList
        {
            get { return m_TerminalList; }
            set
            {
                m_TerminalList = value;

                lstTerminal.Items.Clear();

                ListViewItem tTempButton = null;
                if (m_TerminalList != null)
                {
                    foreach (ITactTerminal tEmulator in m_TerminalList)
                    {
                        if (tEmulator.IsConnected)
                        {
                            tTempButton = new ListViewItem();
                            tTempButton.Text = string.Concat(tEmulator.Name, " (",tEmulator.ToolTip,")")  ;
                            tTempButton.Tag = tEmulator;
                            tTempButton.ToolTipText = tEmulator.ToolTip;
                            lstTerminal.Items.Add(tTempButton);
                        }
                    }
                }
            }
        }
        public ITactTerminal SelectedTerminal
        {
            get{return (ITactTerminal)lstTerminal.SelectedItems[0].Tag;}
        }

        /// <summary>
        /// 버튼 처리를 합니다.
        /// </summary>
        /// <param name="aButtonType"></param>
        protected override void ButtonProcess(E_ButtonType aButtonType)
        {
            if (aButtonType == E_ButtonType.OK)
            {
                if (lstTerminal.SelectedItems.Count == 0)
                {
                    AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "대상 터미널을 선택 하세요.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DialogResult = DialogResult.OK;
            }

            Close();
        }
    }
}