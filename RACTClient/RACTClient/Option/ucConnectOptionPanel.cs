using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using RACTCommonClass;

using MKLibrary.MKNetwork;

namespace RACTClient
{
    public partial class ucConnectOptionPanel : UserControl,IOptionPanal
    {
        public ucConnectOptionPanel()
        {
            InitializeComponent();
        }

        private E_ConnectionProtocol m_Protocol;
        /// <summary>
        /// 설정된 Protocol 속성을 가져오거나 설정합니다.
        /// </summary>
        public E_ConnectionProtocol Protocol
        {
            get { return m_Protocol; }
            set { m_Protocol = value; }
        }


        /// <summary>
        /// 컨트롤을 초기화 합니다.
        /// </summary>
        public void InitializeControl()
        {
            AppGlobal.InitializeComboBoxStyle(cboDefaultProtocol);

            foreach (string tType in Enum.GetNames(typeof(E_ConnectionProtocol)))
            {
                cboDefaultProtocol.Items.Add(tType);
                cboDefaultProtocol.Items[cboDefaultProtocol.Items.Count - 1].Tag = Enum.Parse(typeof(E_ConnectionProtocol), tType);
            }

            for (int i = 0; i < cboDefaultProtocol.Items.Count; i++)
            {
                if ((E_ConnectionProtocol)cboDefaultProtocol.Items[i].Tag == AppGlobal.s_ClientOption.ConnectionInfo.ConnectionProtocol)
                {
                    cboDefaultProtocol.SelectedIndex = i;
                    break;
                }
            }

            // 2019-01-23 추가 - 터미널창의 커맨드 텍스트 유지 여부
            if (AppGlobal.s_ClientOption.IsTerminalTextClear)
            {
                rdoFill.Checked = true;
            }
            else
            {
                rdoErase.Checked = true;
            }
			// 2019-11-10 개선사항 (명령어 단위 전송 지연 기능 )
            sendDelay.Value = AppGlobal.s_ClientOption.SendDelay;
        }

        /// <summary>
        /// 옵션을 저장 합니다.
        /// </summary>
        public bool SaveOption()
        {
            AppGlobal.s_ClientOption.ConnectionInfo.ConnectionProtocol= (E_ConnectionProtocol)cboDefaultProtocol.Items[cboDefaultProtocol.SelectedIndex].Tag;

            // 2019-01-23 추가 - 터미널창의 커맨드 텍스트 유지 여부
            AppGlobal.s_ClientOption.IsTerminalTextClear = rdoFill.Checked;
			// 2019-11-10 개선사항 (명령어 단위 전송 지연 기능 )
            AppGlobal.s_ClientOption.SendDelay = Convert.ToInt32(sendDelay.Value);

            return true;
        }

    }
}
