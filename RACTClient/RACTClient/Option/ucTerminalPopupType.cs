using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RACTClient
{
    public partial class ucTerminalPopupType : UserControl, IOptionPanal
    {
        public ucTerminalPopupType()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 컨트롤을 초기화 합니다.
        /// </summary>
        public void InitializeControl()
        {
            if (AppGlobal.s_ClientOption.TerminalPopupType == E_TerminalPopupType.None || AppGlobal.s_ClientOption.TerminalPopupType == E_TerminalPopupType.PopUp)
            {
                rdoPopup.Checked = true;
            }
            else
            {
                rdoCopyPaste.Checked = true;
            }

            if (AppGlobal.s_ClientOption.ShortenCommandTaget == E_ShortenCommandTagret.ActiveTerminal)
            {
                rdoActiveTerminal.Checked = true;
            }
            else
            {
                rdoAllTerminal.Checked = true;
            }


            if (AppGlobal.s_ClientOption.IsUseTerminalAutoLogin)
            {
                rdoAutoLoginUse.Checked = true;
            }
            else
            {
                rdoAutoLoginNotUse.Checked = true;
            }

            // More문자 올경우 자동 스크롤 되는 기능을 사용여부
            if (AppGlobal.s_ClientOption.IsUseTerminalAutoMoreString)
            {
                rdoAutoMoreStringUse.Checked = true;
            }
            else
            {
                rdoAutoMoreStringNotUse.Checked = true;
            }

            // 2014-07-09 - 신윤남 - 세션종료시 터미널 종료 기능 사용여부
            if (AppGlobal.s_ClientOption.IsUseTerminalClose)
            {
                rdoTerminalCloseY.Checked = true;
            }
            else
            {
                rdoTerminalCloseN.Checked = true;
            }

        }

        /// <summary>
        /// 옵션을 저장 합니다.
        /// </summary>
        public bool SaveOption()
        {
            AppGlobal.s_ClientOption.TerminalPopupType = rdoPopup.Checked ? E_TerminalPopupType.PopUp : E_TerminalPopupType.CopyPaste;
            AppGlobal.s_ClientOption.ShortenCommandTaget = rdoActiveTerminal.Checked ? E_ShortenCommandTagret.ActiveTerminal : E_ShortenCommandTagret.AllTerminal;
            AppGlobal.s_ClientOption.IsUseTerminalAutoLogin = rdoAutoLoginUse.Checked;
            AppGlobal.s_ClientOption.IsUseTerminalAutoMoreString = rdoAutoMoreStringUse.Checked;
            // 2014-07-08 세션 종료시 터미널 닫히는 옵션 처리
            AppGlobal.s_ClientOption.IsUseTerminalClose = rdoTerminalCloseY.Checked;
            return true;
        }


        internal void ChangeMode(bool aVisable)
        {
            groupBox2.Visible = aVisable;
        }
    }
}
