using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RACTClient
{
    public partial class ucTerminalLayout : UserControl, IOptionPanal
    {
        public ucTerminalLayout()
        {
            InitializeComponent();
        }

        #region IOptionPanal 멤버

        public void InitializeControl()
        {
            rdoPopup.Checked = AppGlobal.s_ClientOption.TerminalWindowsPopupType == E_DefaultTerminalPopupType.Popup;
            rdoTab.Checked = AppGlobal.s_ClientOption.TerminalWindowsPopupType == E_DefaultTerminalPopupType.Tab;

            // 2013-01-17 - shinyn - 메모장 실행 팝업 형태 가져오기 추가
            rdoPopup2.Checked = AppGlobal.s_ClientOption.NotePadWindowsPopupType == E_DefaultNotePadPopupType.Popup;
            rdoTab2.Checked = AppGlobal.s_ClientOption.NotePadWindowsPopupType == E_DefaultNotePadPopupType.Tab;

            rdoIPAddress.Checked = AppGlobal.s_ClientOption.TerminalDisplayNameType == E_TerminalDisplayNameType.IPAddress;
            rdoTID.Checked = AppGlobal.s_ClientOption.TerminalDisplayNameType == E_TerminalDisplayNameType.TID;

            nudHeight.Value = AppGlobal.s_ClientOption.PopupSizeHeight;
            nudWidth.Value = AppGlobal.s_ClientOption.PopupSizeWidth;
            nudColumnCount.Value = AppGlobal.s_ClientOption.TerminalColumnCount;
        }

        public bool SaveOption()
        {
            try
            {
                AppGlobal.s_ClientOption.TerminalWindowsPopupType = rdoPopup.Checked ? E_DefaultTerminalPopupType.Popup : E_DefaultTerminalPopupType.Tab;
                // 2013-01-17 - shinyn - 메모장 실행 팝업 형태 저장 추가
                AppGlobal.s_ClientOption.NotePadWindowsPopupType = rdoPopup2.Checked ? E_DefaultNotePadPopupType.Popup : E_DefaultNotePadPopupType.Tab;
                AppGlobal.s_ClientOption.PopupSizeHeight = (int)nudWidth.Value;
                AppGlobal.s_ClientOption.PopupSizeWidth = (int)nudHeight.Value;
                AppGlobal.s_ClientOption.TerminalColumnCount = (int)nudColumnCount.Value;

                AppGlobal.s_ClientOption.TerminalDisplayNameType = rdoIPAddress.Checked ? E_TerminalDisplayNameType.IPAddress : E_TerminalDisplayNameType.TID;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }

        #endregion
    }
}
