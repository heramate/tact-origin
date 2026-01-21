using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RACTClient
{
    public partial class ucTelnetPanel : UserControl, IOptionPanal
    {
        public ucTelnetPanel()
        {
            InitializeComponent();
        }

        private int m_PortNumber;
        /// <summary>
        /// 해당 연결의 Port 번호 속성을 가져오거나 설정합니다.
        /// </summary>
        public int PortNumber
        {
            get { return m_PortNumber; }
            set { m_PortNumber = value; }
        }

        /// <summary>
        /// 컨트롤을 초기화 합니다.
        /// </summary>
        public void InitializeControl()
        {           
            txtConnectionPort.Text = AppGlobal.s_ClientOption.ConnectionInfo.TelnetPort.ToString();
        }

        /// <summary>
        /// 옵션을 저장 합니다.
        /// </summary>
        public bool SaveOption()
        {
            if (txtConnectionPort.Text.Length == 0)
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "Telnet Port를 입력 하세요", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            int tPort = 0;
            if (!int.TryParse(txtConnectionPort.Text, out tPort))
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "Telnet Port를 확인 하세요", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            AppGlobal.s_ClientOption.ConnectionInfo.TelnetPort= tPort;

            return true;
        }

    }
}
