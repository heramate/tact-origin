using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using RACTCommonClass;
using System.Threading;

namespace RACTClient
{
    public partial class ucBatchRegisterPanel : SenderControl
    {
        #region[속성입니다.]
        private DeviceInfoCollection m_SelectedDeviceList;
        /// <summary>
        /// 작업 타입 입니다.
        /// </summary>
        private E_WorkType m_WorkType = E_WorkType.Add;

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
        /// 선택된 장비목록 속성을 가져오거나 설정합니다.
        /// </summary>
        public DeviceInfoCollection Devicelist
        {
            get { return m_SelectedDeviceList; }
            set { m_SelectedDeviceList = value; }
        }
        private E_ConnectionProtocol m_CurrentProtocol;
        /// <summary>
        /// 현재 설정된 프로토콜 속성을 가져오거나 설정합니다.
        /// </summary>
        public E_ConnectionProtocol CurrentProtocol
        {
            get { return m_CurrentProtocol; }
            set { m_CurrentProtocol = value; }
        }
        #endregion
        #region[생성자 입니다.]
        public ucBatchRegisterPanel()
        {
            InitializeComponent();
        }
        #endregion

        /// <summary>
        /// Initializes the control.
        /// </summary>
        public void initializeControl()
        {
            AppGlobal.InitializeComboBoxStyle(cboProtocol);
            AppGlobal.InitializeComboBoxStyle(cboGroup);
            AppGlobal.InitializeButtonStyle(btnNewGroup);
            AppGlobal.InitializeGridStyle(fgDeviceList);
            AppGlobal.s_DataSyncProcssor.OnGroupInfoChangeEvent 
                += new HandlerArgument2<GroupInfo, E_WorkType>(s_DataSyncProcssor_OnGroupInfoChangeEvent);

            pnlSerialPort.InitializeControl();

            m_SelectedDeviceList = new DeviceInfoCollection();
            initializeProtocolList();
            nudPort.Value = AppGlobal.s_ClientOption.ConnectionInfo.TelnetPort;
            InitializeGroupList();
        }

        void s_DataSyncProcssor_OnGroupInfoChangeEvent(GroupInfo aGroupInfo, E_WorkType aWorkType)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new HandlerArgument2<GroupInfo, E_WorkType>(s_DataSyncProcssor_OnGroupInfoChangeEvent), aGroupInfo, aWorkType);
                    return;
                }

                switch (aWorkType)
                {
                    case E_WorkType.Add:
                        cboGroup.Items.Add(aGroupInfo.Name);
                        cboGroup.Items[cboGroup.Items.Count - 1].Tag = aGroupInfo;
                        break;
                    case E_WorkType.Delete:
                        // shinyn - 2012-12-13 - NE Group ID int -> string 수정 'B' PON(Biz) -> FOMs연동 값에 따른 수정
                        string tGroupID;
                        for (int i = cboGroup.Items.Count - 1; i > -1; i--)
                        {
                            tGroupID = ((GroupInfo)cboGroup.Items[i].Tag).ID;

                            if (aGroupInfo.ID == tGroupID)
                            {
                                cboGroup.Items.RemoveAt(i);
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// 프로토콜 목록을 초기화 합니다.
        /// </summary>
        private void initializeProtocolList()
        {
            cboProtocol.Items.Clear();
            foreach (string tType in Enum.GetNames(typeof(E_ConnectionProtocol)))
            {
                cboProtocol.Items.Add(tType);
                cboProtocol.Items[cboProtocol.Items.Count - 1].Tag = Enum.Parse(typeof(E_ConnectionProtocol), tType);
            }

            for (int i = 0; i < cboProtocol.Items.Count; i++)
            {
                if ((E_ConnectionProtocol)cboProtocol.Items[i].Tag == AppGlobal.s_ClientOption.ConnectionInfo.ConnectionProtocol)
                {
                    cboProtocol.SelectedIndex = i;
                    break;
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

            cboGroup.Items.Clear();
            foreach (GroupInfo tGroupInfo in AppGlobal.s_GroupInfoList.InnerList.Values)
            {
                cboGroup.Items.Add(tGroupInfo.Name);
                cboGroup.Items[cboGroup.Items.Count - 1].Tag = tGroupInfo;
            }
            cboGroup.SelectedIndex = 0;
        }

        /// <summary>
        /// 장비를 그리드에 표시합니다.
        /// </summary>
        public void DisplayList()
        {
            int tRowIndex = 1;
            try
            {
                fgDeviceList.Redraw = false;
                fgDeviceList.Rows.Count = 1;
                fgDeviceList.Rows.Count = m_SelectedDeviceList.Count + 1;

                foreach (DeviceInfo tDeviceInfo in m_SelectedDeviceList)
                {
                    AddRow(tRowIndex, tDeviceInfo);
                    if (tDeviceInfo.InputFlag == E_FlagType.User)
                    {
                        fgDeviceList.Rows[tRowIndex].StyleNew.ForeColor = Color.FromArgb(255, 113, 50);
                    }
                    tRowIndex++;
                }

                fgDeviceList.RowSel = 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(">>>>>>>>>>>>>>>> :" + ex.ToString());
            }
            finally
            {
                fgDeviceList.Redraw = true;
            }
        }

        /// <summary>
        /// Grid에 새로운 Row를 추가합니다.
        /// </summary>
        /// <param name="aRowIndex">추가할 Row Index</param>
        /// <param name="aDeviceInfo">추가할 장비 정보</param>
        private void AddRow(int aRowIndex, DeviceInfo aDeviceInfo)
        {
            ModelInfo tModelInfo = AppGlobal.s_ModelInfoList[aDeviceInfo.ModelID];
            fgDeviceList[aRowIndex, "ORG1"] = AppGlobal.GetORG1Name(aDeviceInfo.ORG1Code);
            fgDeviceList[aRowIndex, "Team"] = AppGlobal.GetBranchName(aDeviceInfo.BranchCode);
            fgDeviceList[aRowIndex, "CenterName"] = AppGlobal.GetCenterName(aDeviceInfo.CenterCode);
            fgDeviceList[aRowIndex, "TPOName"] = aDeviceInfo.TpoName;
            fgDeviceList[aRowIndex, "ModelName"] = tModelInfo.ModelName;
            fgDeviceList[aRowIndex, "DeviceGroup"] = aDeviceInfo.DeviceGroupName;
            fgDeviceList[aRowIndex, "DeviceNumber"] = aDeviceInfo.DeviceNumber;
            fgDeviceList[aRowIndex, "DeviceName"] = aDeviceInfo.Name;			//장비이름
            fgDeviceList[aRowIndex, "IPAddress"] = aDeviceInfo.IPAddress;		//장비주소          
            fgDeviceList.Rows[aRowIndex].UserData = aDeviceInfo;
        }

        /// <summary>
        /// 장비 정보를 저장하는 함수입니다.
        /// </summary>
        public bool SaveDeviceInfo()
        {
            if (cboGroup.Items.Count == 0)
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "그룹을 등록하세요", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;

            }
            DeviceInfoCollection tDeviceInfoList = m_SelectedDeviceList;
            RequestCommunicationData tRequestData = null;
            DeviceCollectionRequestInfo tDeviceRequestInfo = new DeviceCollectionRequestInfo();
            tDeviceRequestInfo.WorkType = m_WorkType;
            tDeviceRequestInfo.DeviceInfoList = tDeviceInfoList;
            tDeviceRequestInfo.UserID = AppGlobal.s_LoginResult.UserID;

            //새로운 장비 정보를 저장합니다.
            tDeviceRequestInfo.GroupID = ((GroupInfo)cboGroup.Items[cboGroup.SelectedIndex].Tag).ID;

            tDeviceRequestInfo.ConnectionInfo = new TerminalConnectInfo();
            tDeviceRequestInfo.ConnectionInfo.ConnectionProtocol = (E_ConnectionProtocol)cboProtocol.Items[cboProtocol.SelectedIndex].Tag;
            if (m_CurrentProtocol == E_ConnectionProtocol.TELNET)
            {
                tDeviceRequestInfo.ConnectionInfo.TelnetPort = (int)nudPort.Value;
                tDeviceRequestInfo.ConnectionInfo.SerialConfig = AppGlobal.s_ClientOption.ConnectionInfo.SerialConfig;
            }
            else
            {
                tDeviceRequestInfo.ConnectionInfo.TelnetPort = (int)AppGlobal.s_ClientOption.ConnectionInfo.TelnetPort;
                tDeviceRequestInfo.ConnectionInfo.SerialConfig = pnlSerialPort.GetSerialPortInfo();
            }

            tRequestData = AppGlobal.MakeDefaultRequestData();
            tRequestData.CommType = E_CommunicationType.RequestBatchRegisteration;

            tRequestData.RequestData = tDeviceRequestInfo;

            m_Result = null;
            m_MRE.Reset();

            AppGlobal.SendRequestData(this, tRequestData);
            m_MRE.WaitOne();

            return CheckedResult();
        }

        /// <summary>
        /// 요청 결과를 확인하는 함수입니다.
        /// </summary>
        private bool CheckedResult()
        {
            if (m_Result == null) return false;
            if (m_Result.Error.Error != E_ErrorType.NoError)
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, m_Result.Error.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            EventProcessor.Run((DeviceInfoCollection)m_Result.ResultData, m_WorkType);
            AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "장비 정보를 추가 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return true;
        }

        private void btnNewGroup_Click(object sender, EventArgs e)
        {
            try
            {
                ModifyGroupInfo tNewGroupForm = new ModifyGroupInfo();
                tNewGroupForm.initializeControl();

                // shinyn - 2012-12-13 - NE Group ID int -> string 수정 'B' PON(Biz) -> FOMs연동 값에 따른 수정
                if (tNewGroupForm.ShowDialog() == DialogResult.OK)
                {
                    m_GroupID = tNewGroupForm.GroupInfo.ID;
                    InitializeGroupList();

                    if (m_GroupID == "-1")
                    {
                        cboGroup.SelectedIndex = 0;
                    }
                    else
                    {
                        // shinyn - 2012-12-13 - NE Group ID int -> string 수정 'B' PON(Biz) -> FOMs연동 값에 따른 수정
                        string tGroupID;
                        for (int i = 0; i < cboGroup.Items.Count; i++)
                        {
                            tGroupID = ((GroupInfo)cboGroup.Items[i].Tag).ID;

                            if (tGroupID == m_GroupID)
                            {
                                cboGroup.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(ex.ToString());
            }
        }

        private void cboProtocol_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProtocol.Items.Count == 0 || cboProtocol.SelectedIndex < 0)
            {
                return;
            }
            else if ((E_ConnectionProtocol)cboProtocol.Items[cboProtocol.SelectedIndex].Tag == E_ConnectionProtocol.TELNET)
            {
                m_CurrentProtocol = E_ConnectionProtocol.TELNET;
                nudPort.Value = 23;
                grbTelnet.Visible = true;
                grbSerialPortInfo.Visible = false;
            }
            else if ((E_ConnectionProtocol)cboProtocol.Items[cboProtocol.SelectedIndex].Tag == E_ConnectionProtocol.SSHTelnet)
            {
                // 2013-03-06 - shinyn - SSH텔넷기능인 경우 분기처리 추가
                m_CurrentProtocol = E_ConnectionProtocol.SSHTelnet;
                nudPort.Value = 22;
                grbTelnet.Visible = true;
                grbSerialPortInfo.Visible = false;
            }
            else
            {
                m_CurrentProtocol = E_ConnectionProtocol.SERIAL_PORT;
                grbTelnet.Visible = false;
                grbSerialPortInfo.Visible = true;
            }
        }
    }
}
