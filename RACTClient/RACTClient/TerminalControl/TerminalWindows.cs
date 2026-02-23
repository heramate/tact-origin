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
    public partial class TerminalWindows : Office2007Form
    {
        /// <summary>
        /// 닫기 여부 입니다.
        /// </summary>
        private bool m_IsClose = true;
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public TerminalWindows()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 폼 닫기 처리 입니다.
        /// </summary>
        private void TerminalWindows_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_IsClose)
            {
                ((MCTerminalEmulator)this.Controls[0]).Disconnect();
            }
        }
        /// <summary>
        /// 폼 최소화 입니다.
        /// </summary>
        private void TerminalWindows_MinimumSizeChanged(object sender, EventArgs e)
        {
           
        }

        private void TerminalWindows_Resize(object sender, EventArgs e)
        {
            if (!m_IsClose) return;
            if (this.WindowState == FormWindowState.Minimized)
            {
                m_IsClose = false;
                ((ClientMain)AppGlobal.s_ClientMainForm).AddTerminalTab((ITactTerminal)this.Controls[0]);
                this.Close();

            }
        }

        internal void AddTerminalControl(ITactTerminal tEmulator)
        {
            this.Controls.Add((Control)tEmulator);
            tEmulator.OnTerminalStatusChange += new HandlerArgument2<object, E_TerminalStatus>(tEmulator_OnTerminalStatusChange);
        }

        void tEmulator_OnTerminalStatusChange(object aValue1, E_TerminalStatus aValue2)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument2<object, E_TerminalStatus>(tEmulator_OnTerminalStatusChange), aValue1, aValue2);
                return;
            }

            this.Text = this.Controls[0].Name + " [ " +aValue2.ToString() +" ]";
        }
    }
}