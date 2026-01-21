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
    /// 
    /// </summary>
    public partial class ModifyDeviceInfo : BaseForm
    {
        /// <summary>
        /// 장비 정보 입니다.
        /// </summary>
        DeviceInfo m_DeviceInfo = null;

        // shinyn - 2012-12-13 - NE Group ID int -> string 수정 'B' PON(Biz) -> FOMs연동 값에 따른 수정
        /// <summary>
        /// 장비가 소속된 그룹 ID 입니다.
        /// </summary>
        private string m_GroupID = "-1";
        // shinyn - 2012-12-13 - NE Group ID int -> string 수정 'B' PON(Biz) -> FOMs연동 값에 따른 수정
        /// <summary>
        /// 그룹 ID 속성을 가져오거나 설정합니다.
        /// </summary>
        public string GroupID
        {
            get { return m_GroupID; }
            set { m_GroupID = value; }
        }

        /// <summary>
        /// 장비 접속 프로토콜입니다.
        /// </summary>
        private E_ConnectionProtocol m_Protocol;
        /// <summary>
        /// 프로토콜 속성을 가져오거나 설정합니다.
        /// </summary>
        public E_ConnectionProtocol Protocol
        {
            get { return m_Protocol; }
            set { m_Protocol = value; }
        }	

        /// <summary>
        /// 작업 타입 입니다.
        /// </summary>
        private E_WorkType m_WorkType = E_WorkType.Add;

        public ModifyDeviceInfo()
        {
            InitializeComponent();
        }

        // shinyn - 2012-12-13 - NE Group ID int -> string 수정 'B' PON(Biz) -> FOMs연동 값에 따른 수정
        public ModifyDeviceInfo(string aGroupID)
        {
            m_GroupID = aGroupID;
            InitializeComponent();
        }

        // shinyn - 2012-12-13 - NE Group ID int -> string 수정 'B' PON(Biz) -> FOMs연동 값에 따른 수정
        public ModifyDeviceInfo(DeviceInfo aDeviceInfo, E_WorkType aWorkType)
        {
            m_DeviceInfo = new DeviceInfo(aDeviceInfo);
            m_WorkType = aWorkType;
            m_GroupID = aDeviceInfo.GroupID;
            m_Protocol = m_DeviceInfo.TerminalConnectInfo.ConnectionProtocol;
            InitializeComponent();
        }

        /// <summary>
        /// 컨트롤을 초기화 합니다.
        /// </summary>
        public void InitializeControl()
        {
            AppGlobal.InitializeButtonStyle(btnDeviceSearch);
            AppGlobal.InitializeComboBoxStyle(cboDeviceGroup);
            AppGlobal.InitializeComboBoxStyle(cboProtocol);

            AddButton(E_ButtonType.Close, E_ButtonSide.Right, "닫기");
            AddButton(E_ButtonType.OK, E_ButtonSide.Right, "저장");

            if (m_WorkType.Equals(E_WorkType.Modify))
            {
                btnDeviceSearch.Visible = false;
            }
            else
            {
                btnDeviceSearch.Visible = true;
            }

            initializeProtocolList();
            pnlSerialOption.Initialize();
            InitializeGroupList();
            displayBaseData();

        }

        /// <summary>
        /// 화면의 Button을 클릭한 이벤트를 처리하는 가상 함수입니다.
        /// </summary>
        /// <param name="aButtonType">버튼 타입</param>
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
        DeviceInfo m_OldDeviceInfo;
        /// <summary>
        /// 장비 정보를 저장하는 함수입니다.
        /// </summary>
        private void SaveDeviceInfo()
        {
            if (cboDeviceGroup.SelectedIndex < 0)
            {
                AppGlobal.ShowMessageBox(this, "그룹을 선택 하세요.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2013-01-11 - shinyn - 바로 저장 클릭 했을때 오류 수정
            if (m_DeviceInfo == null)
            {
                AppGlobal.ShowMessageBox(this, "검색후 저장해주세요.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            m_OldDeviceInfo = m_DeviceInfo.DeepClone();
            // 새로운 장비 정보를 저장합니다.

            // shinyn - 2012-12-13 - NE Group ID int -> string 수정 'B' PON(Biz) -> FOMs연동 값에 따른 수정
            m_DeviceInfo.GroupID = cboDeviceGroup.Items[cboDeviceGroup.SelectedIndex].Tag.ToString();

            m_DeviceInfo.TerminalConnectInfo.TelnetPort = (int)nudPort.Value;
            m_DeviceInfo.TerminalConnectInfo.ConnectionProtocol = (E_ConnectionProtocol)cboProtocol.Items[cboProtocol.SelectedIndex].Tag;
            m_DeviceInfo.TerminalConnectInfo.IPAddress = m_DeviceInfo.IPAddress;
            m_DeviceInfo.TerminalConnectInfo.SerialConfig = pnlSerialOption.SerialConfig;
            m_DeviceInfo.TerminalConnectInfo.TelnetPort =(int)nudPort.Value;
            // 2013-01-18 - shinyn - NE에 등록된 장비 저장 요청
            m_DeviceInfo.DeviceType = E_DeviceType.NeGroup;

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

            CheckedResult();
            this.Close();
        }

        /// <summary>
        /// 요청 결과를 확인하는 함수입니다.
        /// </summary>
        private void CheckedResult()
        {
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

                case E_WorkType.Add :
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

        /// <summary>
        /// 프로토콜 목록을 초기화 합니다.
        /// </summary>
        private void initializeProtocolList()
        {
            foreach (string tType in Enum.GetNames(typeof(E_ConnectionProtocol)))
            {
                cboProtocol.Items.Add(tType);
                cboProtocol.Items[cboProtocol.Items.Count - 1].Tag = Enum.Parse(typeof(E_ConnectionProtocol), tType);
                if(tType.Equals(AppGlobal.s_ClientOption.ConnectionInfo.ConnectionProtocol.ToString()))
                {
                    cboProtocol.SelectedIndex = cboProtocol.Items.Count -1;
                }
            }
        }

        /// <summary>
        /// 그룹 목록을 초기화 합니다.
        /// </summary>
        /// <param name="vTV">그룹 목록을 초기화 할 콤보박스 입니다.</param>
        private void InitializeGroupList()
        {
            if (AppGlobal.s_GroupInfoList == null) return;

            foreach (GroupInfo tGroupInfo in AppGlobal.s_GroupInfoList.InnerList.Values)
            {
                cboDeviceGroup.Items.Add(tGroupInfo.Name);
                cboDeviceGroup.Items[cboDeviceGroup.Items.Count-1].Tag = tGroupInfo.ID;
            }
        }

        /// <summary>
        /// 기본 정보를 표시합니다.
        /// </summary>
        private void displayBaseData()
        {
            if (m_DeviceInfo == null)
            {
                nudPort.Value = AppGlobal.s_ClientOption.ConnectionInfo.TelnetPort;
               
            }
            else
            {
                ipDevice.IPAddress = m_DeviceInfo.IPAddress;
                txtDisplayName.Text = m_DeviceInfo.Name;
                // 2013-01-18 - shinyn - 모델명 가져오는 부분 수정
                txtModelName.Text = m_DeviceInfo.ModelName;
                txtModelName.Tag = m_DeviceInfo.ModelID;
                txtLocation.Text = m_DeviceInfo.Location;
                nudPort.Value = m_DeviceInfo.TerminalConnectInfo.TelnetPort;


                for (int i = 0; i < cboProtocol.Items.Count; i++)
                {

                    E_ConnectionProtocol tConnectionProtocol = (E_ConnectionProtocol)cboProtocol.Items[i].Tag;

                    if (tConnectionProtocol == m_DeviceInfo.TerminalConnectInfo.ConnectionProtocol)
                    {
                        switch (tConnectionProtocol)
                        {
                            case E_ConnectionProtocol.TELNET:
                                nudPort.Value = m_DeviceInfo.TerminalConnectInfo.TelnetPort;
                                break;
                            case E_ConnectionProtocol.SSHTelnet:
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


                pnlSerialOption.SerialConfig = m_DeviceInfo.TerminalConnectInfo.SerialConfig;

            }

            // shinyn - 2012-12-13 - NE Group ID int -> string 수정 'B' PON(Biz) -> FOMs연동 값에 따른 수정
            if (m_GroupID == "-1")
            {
                cboDeviceGroup.SelectedIndex = 0;
            }
            else
            {
                // shinyn - 2012-12-13 - NE Group ID int -> string 수정 'B' PON(Biz) -> FOMs연동 값에 따른 수정
                string tGroupID;
                for (int i = 0; i < cboDeviceGroup.Items.Count; i++)
                {
                    // shinyn - 2012-12-13 - NE Group ID int -> string 수정 'B' PON(Biz) -> FOMs연동 값에 따른 수정
                    tGroupID = cboDeviceGroup.Items[i].Tag.ToString();

                    if (tGroupID == m_GroupID)
                    {
                        cboDeviceGroup.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private void btnDeviceSearch_Click(object sender, EventArgs e)
        {
            DeviceSearch tSearch = new DeviceSearch();
            tSearch.InitializeControl();

            if (tSearch.ShowDialog(this) == DialogResult.OK)
            {
                m_DeviceInfo = tSearch.SelectedDeviceInfo;
                displayBaseData();
            }
        }

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

        private void pnlSerialOption_Load(object sender, EventArgs e)
        {

        }
    }

    public enum E_DisplayType
    {
        ADD, MODIFY, DELETE
    }
}