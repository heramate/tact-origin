using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RACTCommonClass;
using RACTSerialProcess;

namespace RACTClient
{
    public partial class QuickConnect : BaseForm
    {
        /// <summary>
        /// 연결 정보 입니다.
        /// </summary>
        private TerminalConnectInfo m_ConnectionInfo;

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public QuickConnect()
        {
            InitializeComponent();
        }

	
        protected override void ButtonProcess(E_ButtonType aButtonType)
        {
            switch (aButtonType)
            {
                case E_ButtonType.OK:
                    // 2013-01-28 - SSH 연결 수정
                    E_ConnectionProtocol tConnectionProtocol = (E_ConnectionProtocol)cboProtocol.SelectedIndex;

                    if (tConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
                    {
                        if (txtID.Text == "")
                        {
                            AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "아이디를 입력해주세요.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        if (txtPassword.Text == "")
                        {
                            AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "비밀번호를 입력해주세요.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        
                    }




                    DialogResult = DialogResult.OK;
                    break;
            }
            this.Close();
        }
        public void InitializeControl()
        {
            AddButton(E_ButtonType.Cancel, E_ButtonSide.Right, "취소");
            AddButton(E_ButtonType.OK, E_ButtonSide.Right, "연결");

            pnlSerialOption.Initialize();

            nudPort.Value = AppGlobal.s_ClientOption.ConnectionInfo.TelnetPort;

            foreach (string tType in Enum.GetNames(typeof(E_ConnectionProtocol)))
            {
                cboProtocol.Items.Add(tType);
                cboProtocol.Items[cboProtocol.Items.Count - 1].Tag = Enum.Parse(typeof(E_ConnectionProtocol), tType);
            }
            cboProtocol.SelectedIndex = (int)AppGlobal.s_ClientOption.ConnectionInfo.ConnectionProtocol;
        }


        /// <summary>
        /// 2013-01-28 - SSH 빠른 연결 기능 추가
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboProtocol_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (cboProtocol.Items.Count == 0 || cboProtocol.SelectedIndex < 0)
            {
                return;
            }

            E_ConnectionProtocol tConnectionProtocol = (E_ConnectionProtocol)cboProtocol.Items[cboProtocol.SelectedIndex].Tag;

            switch (tConnectionProtocol)
            {
                case E_ConnectionProtocol.TELNET:
                    pnlSerialOption.Visible = false;
                    nudPort.Value = 23;
                    lblID.Visible = false;
                    txtID.Visible = false;
                    lblPassword.Visible = false;
                    txtPassword.Visible = false;
                    this.Size = new Size(275, 166);
                    break;
                case E_ConnectionProtocol.SSHTelnet:
                    // 2013-03-06 - shinyn - SSH텔넷인경우 분기처리 추가
                    pnlSerialOption.Visible = false;
                    nudPort.Value = 22;
                    lblID.Visible = true;
                    txtID.Visible = true;
                    lblPassword.Visible = true;
                    txtPassword.Visible = true;
                    this.Size = new Size(275, 266);
                    break;
                case E_ConnectionProtocol.SERIAL_PORT:
                    this.Size = new Size(275, 302);
                    pnlSerialOption.Visible = true;
                    lblID.Visible = false;
                    txtID.Visible = false;
                    lblPassword.Visible = false;
                    txtPassword.Visible = false;
                    pnlSerialOption.BringToFront();
                    break;
            }
        }
        

        /// <summary>
        /// Serial 연결 정보 가져오기 합니다.
        /// </summary>
        public TerminalConnectInfo QuickConnectInfo
        {
            get
            {
                m_ConnectionInfo = new TerminalConnectInfo();
                m_ConnectionInfo.ConnectionProtocol = (E_ConnectionProtocol)cboProtocol.SelectedIndex;
                m_ConnectionInfo.IPAddress = txtIPAddress.IPAddress;
                m_ConnectionInfo.TelnetPort = (int)nudPort.Value;
                m_ConnectionInfo.SerialConfig = pnlSerialOption.SerialConfig;
                m_ConnectionInfo.ID = txtID.Text;
                m_ConnectionInfo.Password = txtPassword.Text;

                return m_ConnectionInfo;
            }
        }

    }
}