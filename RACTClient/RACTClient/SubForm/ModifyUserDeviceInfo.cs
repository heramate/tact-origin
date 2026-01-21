using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RACTCommonClass;

namespace RACTClient
{

    /// <summary>
    /// 2013-01-18 - shinyn - 수동 장비관리 폼 추가
    /// </summary>
    public partial class ModifyUserDeviceInfo : BaseForm
    {
        DeviceInfo m_DeviceInfo = new DeviceInfo();

        DeviceInfo m_OldDeviceInfo;

        private string m_GroupID = "-1";

        public string GroupID
        {
            get { return m_GroupID; }
            set { m_GroupID = value; }
        }

        private E_ConnectionProtocol m_Protocol;

        public E_ConnectionProtocol Protocol
        {
            get { return m_Protocol; }
            set { m_Protocol = value; }
        }

        private E_WorkType m_WorkType = E_WorkType.Add;

        public ModifyUserDeviceInfo()
        {
            InitializeComponent();
        }

        public ModifyUserDeviceInfo(string aGroupID)
        {
            m_GroupID = aGroupID;
            InitializeComponent();

            txtWAIT.Text = "n:|e:|:";
            txtUSERID.Text = "d:|>|#";
            txtPWD.Text = ">|#";
            txtUSERID2.Text = "d:|>|#";
            txtPWD2.Text = "#|>";
            txtMoreString.Text = "--More--";
            txtMoreMark.Text = "${SPACE}";

        }

        public ModifyUserDeviceInfo(DeviceInfo aDeviceInfo, E_WorkType aWorkType)
        {
            m_DeviceInfo = new DeviceInfo(aDeviceInfo);
            m_WorkType = aWorkType;
            m_GroupID = aDeviceInfo.GroupID;
            m_Protocol = m_DeviceInfo.TerminalConnectInfo.ConnectionProtocol;
            InitializeComponent();
        }

        public void InitializeControl()
        {
            AppGlobal.InitializeComboBoxStyle(cboDeviceGroup);
            AppGlobal.InitializeComboBoxStyle(cboProtocol);

            AddButton(E_ButtonType.Close, E_ButtonSide.Right, "닫기");
            AddButton(E_ButtonType.OK, E_ButtonSide.Right, "저장");

            m_DeviceInfo.DeviceType = E_DeviceType.UserNeGroup;

            InitializeProtocolList();
            pnlSerialOption.Initialize();
            InitializeGroupList();
            DisplayBaseData();
        }

        private void InitializeProtocolList()
        {
            foreach (string tType in Enum.GetNames(typeof(E_ConnectionProtocol)))
            {
                cboProtocol.Items.Add(tType);
                cboProtocol.Items[cboProtocol.Items.Count - 1].Tag = Enum.Parse(typeof(E_ConnectionProtocol), tType);
                if (tType.Equals(AppGlobal.s_ClientOption.ConnectionInfo.ConnectionProtocol.ToString())) ;
                {
                    cboProtocol.SelectedIndex = cboProtocol.Items.Count -1;
                }

            }
        }

        private void InitializeGroupList()
        {
            if (AppGlobal.s_GroupInfoList == null) return;

            foreach (GroupInfo tGroupInfo in AppGlobal.s_GroupInfoList.InnerList.Values)
            {
                cboDeviceGroup.Items.Add(tGroupInfo.Name);
                cboDeviceGroup.Items[cboDeviceGroup.Items.Count - 1].Tag = tGroupInfo.ID;
            }
        }

        private void DisplayBaseData()
        {
            if (m_DeviceInfo == null)
            {
                nudPort.Value = AppGlobal.s_ClientOption.ConnectionInfo.TelnetPort;
            }
            else
            {
                ipDevice.IPAddress = m_DeviceInfo.IPAddress;
                txtDisplayName.Text = m_DeviceInfo.Name;
                txtModelName.Text = m_DeviceInfo.ModelName;
                txtLocation.Text = m_DeviceInfo.Location;
                txtTelnetID1.Text = m_DeviceInfo.TelnetID1;
                txtPassword1.Text = m_DeviceInfo.TelnetPwd1;
                txtTelnetID2.Text = m_DeviceInfo.TelnetID2;
                txtPassword2.Text = m_DeviceInfo.TelnetPwd2;
                // 2013-05-02 - shinyn - 기본접속 정보를 조회하도록 한다.
                txtWAIT.Text = m_DeviceInfo.WAIT;
                txtUSERID.Text = m_DeviceInfo.USERID;
                txtPWD.Text = m_DeviceInfo.PWD;
                txtUSERID2.Text = m_DeviceInfo.USERID2;
                txtPWD2.Text = m_DeviceInfo.PWD2;
                // 2013-08-09 - shinyn -MoreString, MoreMark 표시
                txtMoreString.Text = m_DeviceInfo.MoreString;
                txtMoreMark.Text = m_DeviceInfo.MoreMark;

                nudPort.Value = m_DeviceInfo.TerminalConnectInfo.TelnetPort;


                
                for(int i = 0; i < cboProtocol.Items.Count; i ++)
                {

                    E_ConnectionProtocol tConnectionProtocol = (E_ConnectionProtocol)cboProtocol.Items[i].Tag;

                    if (tConnectionProtocol == m_DeviceInfo.TerminalConnectInfo.ConnectionProtocol)
                    {
                        switch(tConnectionProtocol)
                        {
                            case E_ConnectionProtocol.TELNET:
                                nudPort.Value = m_DeviceInfo.TerminalConnectInfo.TelnetPort;
                                break;
                            case E_ConnectionProtocol.SSHTelnet:
                                // 2013-03-06 - shinyn - SSH 기능인 경우 분기처리 추가
                                nudPort.Value = m_DeviceInfo.TerminalConnectInfo.TelnetPort;
                                break;
                            case E_ConnectionProtocol.SERIAL_PORT:
                                pnlSerialOption.SerialConfig = m_DeviceInfo.TerminalConnectInfo.SerialConfig;
                                break;
                        }

                        cboProtocol.SelectedIndex = i;
                        break;
                    }
                }

                //pnlSerialOption.SerialConfig = m_DeviceInfo.TerminalConnectInfo.SerialConfig;


                if (m_GroupID == "-1")
                {
                    cboDeviceGroup.SelectedIndex = 0;
                }
                else
                {
                    string tGroupID = string.Empty;

                    for(int i=0;i<cboDeviceGroup.Items.Count;i++)
                    {
                        tGroupID = cboDeviceGroup.Items[i].Tag.ToString();
                        if(tGroupID == m_GroupID)
                        {
                            cboDeviceGroup.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
        }

        private void cboProtocol_SelectedIndexChanged(object sender, EventArgs e)
        {

            
            if (cboProtocol.Items.Count == 0 || cboProtocol.SelectedIndex < 0)
            {
                return;
            }

            E_ConnectionProtocol tConnectionProtocol = (E_ConnectionProtocol)cboProtocol.Items[cboProtocol.SelectedIndex].Tag;

            switch(tConnectionProtocol)
            {
                case E_ConnectionProtocol.TELNET:
                    pnlSerialOption.Visible = false;
                    nudPort.Value = 23;
                    break;
                case E_ConnectionProtocol.SSHTelnet:
                    // 2013-03-06 - shinyn - SSH텔넷기능인 경우 분기처리 추가
                    pnlSerialOption.Visible = false;
                    nudPort.Value = 22;
                    break;
                case E_ConnectionProtocol.SERIAL_PORT:
                    pnlSerialOption.Visible = true;
                    pnlSerialOption.BringToFront();
                    break;
            }
        }

        protected override void ButtonProcess(E_ButtonType aButtonType)
        {
            switch (aButtonType)
            {
                case E_ButtonType.OK:
                    SaveDeviceInfo();
                    break;
                case E_ButtonType.Close:
                    Close();
                    break;
            }
        }

        private void SaveDeviceInfo()
        {
            if (cboDeviceGroup.SelectedIndex < 0)
            {
                AppGlobal.ShowMessageBox(this, "그룹을 선택 하세요.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ipDevice.IPAddress == "")
            {
                AppGlobal.ShowMessageBox(this, "아이피를 입력해주세요.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtLocation.Text.Trim().Length == 0)
            {
                AppGlobal.ShowMessageBox(this, "위치를 입력해주세요.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtTelnetID1.Text.Trim().Length == 0)
            {
                AppGlobal.ShowMessageBox(this, "TelnetID_1을 입력해주세요.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtPassword1.Text.Trim().Length == 0)
            {
                AppGlobal.ShowMessageBox(this, "Password_1을 입력해주세요.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            m_OldDeviceInfo = m_DeviceInfo.DeepClone();
            
            m_DeviceInfo.GroupID = cboDeviceGroup.Items[cboDeviceGroup.SelectedIndex].Tag.ToString();

            m_DeviceInfo.TerminalConnectInfo.TelnetPort = (int)nudPort.Value;
            m_DeviceInfo.TerminalConnectInfo.ConnectionProtocol = (E_ConnectionProtocol)cboProtocol.Items[cboProtocol.SelectedIndex].Tag;
            m_DeviceInfo.TerminalConnectInfo.IPAddress = ipDevice.IPAddress;
            m_DeviceInfo.TerminalConnectInfo.SerialConfig = pnlSerialOption.SerialConfig;

            m_DeviceInfo.IPAddress = ipDevice.IPAddress;
            m_DeviceInfo.Name = txtDisplayName.Text;
            m_DeviceInfo.ModelName = txtModelName.Text;
            m_DeviceInfo.TelnetID1 = txtTelnetID1.Text;
            m_DeviceInfo.TelnetPwd1 = txtPassword1.Text;
            m_DeviceInfo.TelnetID2 = txtTelnetID2.Text;
            m_DeviceInfo.TelnetPwd2 = txtPassword2.Text;
            m_DeviceInfo.Name = txtDisplayName.Text;
            m_DeviceInfo.TpoName = txtLocation.Text;


            // 2013-05-02 - shinyn - 프롬프트값을 매핑합니다.
            m_DeviceInfo.WAIT = txtWAIT.Text;
            m_DeviceInfo.USERID = txtUSERID.Text;
            m_DeviceInfo.PWD = txtPWD.Text;
            m_DeviceInfo.USERID2 = txtUSERID2.Text;
            m_DeviceInfo.PWD2 = txtPWD2.Text;

            // MoreString, MoreMark 매핑합니다.
            m_DeviceInfo.MoreString = txtMoreString.Text;
            m_DeviceInfo.MoreMark = txtMoreMark.Text;

            m_DeviceInfo.DeviceType = E_DeviceType.UserNeGroup;

            RequestCommunicationData tRequestData = null;
            DeviceRequestInfo tDeviceRequestInfo = new DeviceRequestInfo();

            tDeviceRequestInfo.WorkType = m_WorkType;
            tDeviceRequestInfo.DeviceInfo = m_DeviceInfo;
            tDeviceRequestInfo.UserID = AppGlobal.s_LoginResult.UserID;

            tRequestData = AppGlobal.MakeDefaultRequestData();
            tRequestData.CommType = E_CommunicationType.RequestDeviceInfo;

            tRequestData.RequestData = tDeviceRequestInfo;

            m_Result = null;
            m_MRE.Reset();

            AppGlobal.SendRequestData(this, tRequestData);
            m_MRE.WaitOne(AppGlobal.s_RequestTimeOut);

            if (m_Result == null)
            {
                AppGlobal.ShowMessageBox(this, "알 수 없는 에러가 발생했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (m_Result.Error.Error != E_ErrorType.NoError)
            {
                AppGlobal.ShowMessageBox(this, m_Result.Error.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            
            if (m_WorkType == E_WorkType.Modify)
            {
                EventProcessor.Run(m_OldDeviceInfo, E_WorkType.Delete);
            }
            

            EventProcessor.Run((DeviceInfo)m_Result.ResultData, m_WorkType);

            switch (m_WorkType)
            {

                case E_WorkType.Add:
                    AppGlobal.ShowMessageBox(this, "장비 정보를 추가 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case E_WorkType.Modify:
                    AppGlobal.ShowMessageBox(this, "장비 정보를 수정 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case E_WorkType.Delete:
                    AppGlobal.ShowMessageBox(this, "장비 정보를 삭제 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }
            this.Close();

        }

        private void ipDevice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                txtModelName.Focus();
                txtModelName.BringToFront();
            }
        }

        private void txtModelName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                txtDisplayName.Focus();
                txtDisplayName.BringToFront();
            }
        }

        private void cboDeviceGroup_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                txtLocation.Focus();
                txtLocation.BringToFront();
            }
        }

        private void txtTelnetID1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                txtPassword1.Focus();
                txtPassword1.BringToFront();
            }
        }

        private void txtTelnetID2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                txtPassword2.Focus();
                txtPassword2.BringToFront();
            }
        }

        private void txtPassword2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                if (AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm,
                                            "저장하시겠습니까?",
                                            MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Question) != DialogResult.Yes) return;

                SaveDeviceInfo();
            }
        }





    }
}