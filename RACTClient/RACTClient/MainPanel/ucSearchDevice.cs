using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using RACTCommonClass;
using C1.Win.C1FlexGrid;
using System.Threading;

namespace RACTClient
{
    public partial class ucSearchDevice : SenderControl, IMainPanel
    {
        /// <summary>
        ///  장비 연결 이벤트 입니다.
        /// </summary>
        public event ConnectDeviceHandler OnConnectDeviceEvent;
        /// <summary>
        /// 장비 수정 이벤트 입니다.
        /// </summary>
        public event ModifyDeviceHandler OnModifyDeviceEvent;
        /// <summary>
        /// 장비 등록 이벤트 입니다.
        /// </summary>
        public event DeviceRegisterHandler OnRegisterDeviceList;

        // 2013-05-02 - shinyn - 장비접속 오류를 해결하기 위해 데몬정보와 장비정보를 함께 보내 접속하도록 수정한다.
        private DeviceInfoCollection m_SelectedDeviceList = new DeviceInfoCollection();


        /// <summary>
        /// 검색 타입 입니다.
        /// </summary>
        private E_SearchType m_SearchType;
        /// <summary>
        /// 검색 문자열 입니다.
        /// </summary>
        private string m_SearchText;
        /// <summary>
        /// 검색 문자열 속성을 가져오거나 설정합니다.
        /// </summary>
        public string SearchText
        {
            get { return m_SearchText; }
            set { m_SearchText = value; }
        }

        public ucSearchDevice()
        {
            InitializeComponent();
        }

        private void cboDevicePart_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 컨트롤을 초기화 합니다.
        /// </summary>
        public void InitializeControl()
        {
            cboSearch.Items.Clear();

            // 사업장->TID로 변경
            // 국소명으로 검색하는 기능 추가
            cboSearch.Items.Add("장비IP");
            cboSearch.Items[cboSearch.Items.Count - 1].Tag = E_SearchType.IP;
            cboSearch.Items.Add("TID");
            cboSearch.Items[cboSearch.Items.Count - 1].Tag = E_SearchType.Place;
            cboSearch.Items.Add("모델");
            cboSearch.Items[cboSearch.Items.Count - 1].Tag = E_SearchType.Model;
            cboSearch.Items.Add("국소명");
            cboSearch.Items[cboSearch.Items.Count - 1].Tag = E_SearchType.TpoName;
            cboSearch.SelectedIndex = 1;

            AppGlobal.InitializeButtonStyle(btnSearch);
            AppGlobal.InitializeGridStyle(grdDeviceList);
        }

        /// <summary>
        /// 검색 버튼을 클릭했을 때 실행하는 함수입니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string RTSearchText;

            m_SearchType = (E_SearchType)cboSearch.Items[cboSearch.SelectedIndex].Tag;
            m_SearchText = txtSearch.Text;

            //E_SearchType.IP일때 IP 유효성 체크
            if (m_SearchType == E_SearchType.IP)
            {
                if (AppGlobal.IsValidIPv4(m_SearchText, out RTSearchText))
                    ;// m_SearchText = RTSearchText;
                else
                {
                    AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "유효한 IP를 입력하세요.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }


            if (m_SearchText.Trim().Length == 0)
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "검색할 문자열을 입력하세요.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            AppGlobal.ShowLoadingProgress(true);
            txtSearch.AutoCompleteCustomSource.Add(m_SearchText);
            this.Enabled = false;
            StartSendThread(new ThreadStart(DisplayDevice));
        }


        /// <summary>
        /// 조건이 일치하는 장비를 그리드에 표시합니다.
        /// </summary>
        private void DisplayDevice()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DefaultHandler(DisplayDevice));
                return;
            }
            RequestCommunicationData tRequestData = null;
            DeviceSearchInfo tSearchInfo = new DeviceSearchInfo();


            TreeNodeEx tSelectNode = (TreeNodeEx)((ClientMain)AppGlobal.s_ClientMainForm).trvSystemGroup.treeViewEx1.SelectedNode;


            if (tSelectNode != null)
            {
                tSearchInfo.SelectFACTGroupInfo = (FACTGroupInfo)tSelectNode.Tag;
            }


            // 검색 조건 추가 : 국소명으로 조회하는 기능 추가(TpoName)
            // Place : DetailLoc로 조회하는 기능 추가
            // 조회후 결과를 표시할때 DetailLoc, TpoName 을 그리드에 보이도록 수정
            if (m_SearchType == E_SearchType.IP)
            {
                tSearchInfo.DeviceIPAddress = txtSearch.Text;
            }
            else if (m_SearchType == E_SearchType.Model)
            {
                tSearchInfo.ModelName = txtSearch.Text;
            }
            else if (m_SearchType == E_SearchType.Place)
            {
                tSearchInfo.DeviceName = txtSearch.Text;
            }
            else if (m_SearchType == E_SearchType.TpoName)
            {
                tSearchInfo.TpoName = txtSearch.Text;
            }

            tSearchInfo.UserID = AppGlobal.s_LoginResult.UserID;

            tRequestData = AppGlobal.MakeDefaultRequestData();
            tRequestData.CommType = E_CommunicationType.RequestFACTSearchDevice;

            tRequestData.RequestData = tSearchInfo;

            m_Result = null;
            m_MRE.Reset();
            AppGlobal.SendRequestData(this, tRequestData);
            m_MRE.WaitOne(AppGlobal.s_RequestTimeOut * 10);


        }

        public override void ResultReceiver(ResultCommunicationData vResult)
        {
            base.ResultReceiver(vResult);

            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument1<ResultCommunicationData>(ResultReceiver), vResult);
                return;
            }


            // 2013-05-02-shinyn - 결과값이 널인 경우 로그에 저장한다.
            if (m_Result == null || m_Result.ResultData == null)
            {
                this.Enabled = true;
                AppGlobal.ShowLoadingProgress(false);
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, " ucSearchDevice : ResultReceive Error : Result null ");
                return;
            }

            object tResult = m_Result.ResultData;

            if (tResult.GetType().Equals(typeof(CompressData)))
            {
                DisplayList((DeviceInfoCollection)AppGlobal.DecompressObject((CompressData)m_Result.ResultData));
            }
            else
            {
                if (m_Result.ResultData.GetType().Equals(typeof(List<DaemonProcessInfo>)))
                {
                    List<DaemonProcessInfo> tDaemonList = m_Result.ResultData as List<DaemonProcessInfo>;

                    int i = 0;
                    foreach (DeviceInfo tDeviceInfo in m_SelectedDeviceList)
                    {
                        if (OnConnectDeviceEvent != null)
                        {
                            // 2013-05-02- shinyn - 장비접속 오류를 해결하기 위해, 데몬정보와 장비정보를 함께 보낸다.
                            OnConnectDeviceEvent(tDeviceInfo, tDaemonList[i]);
                        }
                        i++;
                    }
                }
            }
        }

        /// <summary>
        /// 장비를 그리드에 표시합니다.
        /// </summary>
        public void DisplayList(DeviceInfoCollection aDeviceList)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument1<DeviceInfoCollection>(DisplayList), aDeviceList);
                return;
            }


            this.Enabled = true;
            if (aDeviceList.Count == 0)
            {
                grdDeviceList.Rows.Count = 1;
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "검색 조건과 일치하는 장비가 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            int tRowIndex = 1;
            try
            {
                grdDeviceList.Redraw = false;
                grdDeviceList.Rows.Count = 1;



                grdDeviceList.Rows.Count = aDeviceList.Count + 1;

                // 2013-12-30 - yn shin - 검색 완료후 장비가 하나이면, 표시안되는 버그 수정
                if (aDeviceList.Count == 1)
                {

                    foreach (DeviceInfo tDeviceInfo in aDeviceList)
                    {
                        if (tDeviceInfo.DeviceID == 0)
                        {
                            grdDeviceList.Rows.Count = 1;
                        }
                        else
                        {
                            grdDeviceList.Rows.Count = 2;
                        }
                    }
                }


                foreach (DeviceInfo tDeviceInfo in aDeviceList)
                {
                    // 검색한 결과가 있을 때에만 조회
                    if (tDeviceInfo.DeviceID > 0)
                    {
                        AddRow(tRowIndex, tDeviceInfo);
                        if (tDeviceInfo.InputFlag == E_FlagType.User)
                        {
                            grdDeviceList.Rows[tRowIndex].StyleNew.ForeColor = Color.FromArgb(255, 113, 50);
                        }
                        tRowIndex++;
                    }
                }


                //grdDeviceList.RowSel = 1;
            }
            catch (Exception ex)
            {
                //AppGlobal.m_FileLog.PrintLogEnter("InitializeDevice : " + ex.ToString());
                Console.WriteLine(">>>>>>>>>>>>>>>> :" + ex.ToString());
            }
            finally
            {
                grdDeviceList.Redraw = true;
                AppGlobal.ShowLoadingProgress(false);
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
            grdDeviceList[aRowIndex, "colDeviceIP"] = aDeviceInfo.IPAddress;
            grdDeviceList[aRowIndex, "colModelName"] = tModelInfo != null ? tModelInfo.ModelName : "";        //장비주소
            grdDeviceList[aRowIndex, "colDeviceGroup"] = aDeviceInfo.DeviceGroupName;
            grdDeviceList[aRowIndex, "colDeviceName"] = aDeviceInfo.Name;			//장비이름

            // 국소명으로 조회한결과 표시
            grdDeviceList[aRowIndex, "colTpoName"] = aDeviceInfo.TpoName; // 국소명
            grdDeviceList.Rows[aRowIndex].UserData = aDeviceInfo;

            //grdDeviceList.Rows[aRowIndex].Visible = false;
        }

        private enum E_SearchType
        {
            IP = 0,
            Place = 1,
            Model = 2,
            TpoName = 3
        }

        private void grdDeviceList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //HitTestInfo tHitInfo =  grdDeviceList.HitTest(e.X, e.Y);
                //if (tHitInfo.Row > 0)
                //{
                //    grdDeviceList.Select(tHitInfo.Row, tHitInfo.Column);
                //}
                if (grdDeviceList.RowSel > 0)
                {
                    //2015-09-18 hanjiyeon 수정 - TL1 접속 
                    //old code
                    //ctmPopup.Popup(MousePosition);
                    //new code -
                    DeviceInfo tDI = (DeviceInfo)grdDeviceList.Rows[grdDeviceList.RowSel].UserData;

                    if (AppGlobal.IsAlLuDevice(tDI.ModelID))
                    {
                        ctmPopup.SubItems["mnuTL1Connect"].Visible = true;
                    }
                    else
                    {
                        ctmPopup.SubItems["mnuTL1Connect"].Visible = false;                        
                    }
                    //- new code

                    if (AppGlobal.IsRpcsDevice(tDI.ModelID))
                    {
                        //ctmPopup.SubItems["mnuQuickConnect"].Visible = false;
                        ctmPopup.SubItems["mnuRpcsConnect"].Visible = false;
                        ctmPopup.SubItems["mnuCatm1Connect"].Visible = true;
                    }
                    else
                    {
                        //ctmPopup.SubItems["mnuQuickConnect"].Visible = true;
                        ctmPopup.SubItems["mnuRpcsConnect"].Visible = false;
                        ctmPopup.SubItems["mnuCatm1Connect"].Visible = false;
                    }

                    ctmPopup.Popup(MousePosition);                    
                }
            }
        }

        private void ConnectDevice()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DefaultHandler(ConnectDevice));
                return;
            }

            RequestCommunicationData tRequestData = null;

            tRequestData = AppGlobal.MakeDefaultRequestData();
            tRequestData.CommType = E_CommunicationType.RequestDaemonInfoList;

            m_SelectedDeviceList.Clear();

            for (int i = 0; i < grdDeviceList.Rows.Selected.Count; i++)
            {
                m_SelectedDeviceList.Add((DeviceInfo)grdDeviceList.Rows.Selected[i].UserData);
            }

            tRequestData.RequestData = m_SelectedDeviceList.Count;

            m_Result = null;
            m_MRE.Reset();

            AppGlobal.SendRequestData(this, tRequestData);
            m_MRE.WaitOne(AppGlobal.s_RequestTimeOut * 10);
        }

        private void ConnectDevice_TL1()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DefaultHandler(ConnectDevice_TL1));
                return;
            }

            RequestCommunicationData tRequestData = null;

            tRequestData = AppGlobal.MakeDefaultRequestData();
            tRequestData.CommType = E_CommunicationType.RequestDaemonInfoList;

            m_SelectedDeviceList.Clear();

            for (int i = 0; i < grdDeviceList.Rows.Selected.Count; i++)
            {
                DeviceInfo tDI = ((DeviceInfo)grdDeviceList.Rows.Selected[i].UserData).DeepClone();                
                tDI.TerminalConnectInfo.TelnetPort = 1023;
                m_SelectedDeviceList.Add(tDI);
            }

            tRequestData.RequestData = m_SelectedDeviceList.Count;

            m_Result = null;
            m_MRE.Reset();

            AppGlobal.SendRequestData(this, tRequestData);
            m_MRE.WaitOne(AppGlobal.s_RequestTimeOut * 10);
        }


        private void mnuQuickConnect_Click(object sender, EventArgs e)
        {
            AppGlobal.s_ConnectionMode = 0;
            if (grdDeviceList.Rows.Selected == null || grdDeviceList.Rows.Selected.Count == 0)
            {
                return;
            }


            StartSendThread(new ThreadStart(ConnectDevice));
        }

        //2015-09-18 hanjiyeon 추가 - TL1 접속
        /// <summary>
        /// TL1 연결 버튼 클릭 시의 처리입니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuTL1Connect_Click(object sender, EventArgs e)
        {
            AppGlobal.s_ConnectionMode = 0;
            if (grdDeviceList.Rows.Selected == null || grdDeviceList.Rows.Selected.Count == 0)
            {
                return;
            }

            StartSendThread(new ThreadStart(ConnectDevice_TL1));
        }

        private void mnuAddDevice_Click(object sender, EventArgs e)
        {
            RowCollection tSelectedRows = grdDeviceList.Rows.Selected;
            if (tSelectedRows == null || tSelectedRows.Count == 0) return;
            if (tSelectedRows.Count > 1)
            {
                DeviceInfoCollection tDeviceList = new DeviceInfoCollection();
                foreach (Row tRow in tSelectedRows)
                {
                    tDeviceList.Add((DeviceInfo)tRow.UserData);

                }

                if (OnRegisterDeviceList != null)
                {
                    OnRegisterDeviceList(tDeviceList);
                }
            }
            else
            {
                if (OnModifyDeviceEvent != null)
                {
                    OnModifyDeviceEvent(E_WorkType.Add, (DeviceInfo)grdDeviceList.Rows[grdDeviceList.RowSel].UserData);
                }
            }
        }

        private void txtSearch_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {


        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(null, null);
            }

            // 터미널에 같이 복사,붙여넣기 안되도록 처리
            if (e.Control && e.KeyCode == Keys.C) return;
            if (e.Control && e.KeyCode == Keys.V) return;
        }

        private void mnuRpcsConnect_Click(object sender, EventArgs e)
        {
            if (grdDeviceList.Rows.Selected == null || grdDeviceList.Rows.Selected.Count == 0)
            {
                return;
            }
			AppGlobal.s_ConnectionMode = 2;
            StartSendThread(new ThreadStart(ConnectDevice_Rpcs));
        }

        private void ConnectDevice_Rpcs()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DefaultHandler(ConnectDevice_Rpcs));
                return;
            }

            RequestCommunicationData tRequestData = null;

            tRequestData = AppGlobal.MakeDefaultRequestData();
            tRequestData.CommType = E_CommunicationType.RequestDaemonInfoList;

            m_SelectedDeviceList.Clear();

            for (int i = 0; i < grdDeviceList.Rows.Selected.Count; i++)
            {
                DeviceInfo tDI = ((DeviceInfo)grdDeviceList.Rows.Selected[i].UserData).DeepClone();
                tDI.TerminalConnectInfo.SerialConfig.PortName = "RPCS";
                m_SelectedDeviceList.Add(tDI);
            }

            tRequestData.RequestData = m_SelectedDeviceList.Count;

            m_Result = null;
            m_MRE.Reset();

            AppGlobal.SendRequestData(this, tRequestData);
            m_MRE.WaitOne(AppGlobal.s_RequestTimeOut * 10);
        }
        
        private void mnuCatm1Connect_Click(object sender, EventArgs e)
        {
            if (grdDeviceList.Rows.Selected == null || grdDeviceList.Rows.Selected.Count == 0)
            {
                return;
            }
            if (MessageBox.Show("RPCS(무선) 장비 접속시 LTE망 과금이 발생합니다.\r\n접속 하시겠습니까?\r\n(접속 시도시 일정시간이 소용됩니다.)", "", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            AppGlobal.s_ConnectionMode = 3;
            StartSendThread(new ThreadStart(ConnectDevice_Catm1));
        }

        private void ConnectDevice_Catm1()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DefaultHandler(ConnectDevice_Catm1));
                return;
            }

            RequestCommunicationData tRequestData = null;

            tRequestData = AppGlobal.MakeDefaultRequestData();
            //tRequestData.CommType = E_CommunicationType.RequestDaemonInfoList;
            tRequestData.CommType = E_CommunicationType.RequestSSHDaemonInfoList;

            m_SelectedDeviceList.Clear();

            for (int i = 0; i < grdDeviceList.Rows.Selected.Count; i++)
            {
                DeviceInfo tDI = ((DeviceInfo)grdDeviceList.Rows.Selected[i].UserData).DeepClone();
                tDI.TerminalConnectInfo.SerialConfig.PortName = "RPCSLTE";
                m_SelectedDeviceList.Add(tDI);
            }

            //tRequestData.RequestData = m_SelectedDeviceList.Count;
            tRequestData.RequestData = m_SelectedDeviceList;

            m_Result = null;
            m_MRE.Reset();

            AppGlobal.SendRequestData(this, tRequestData);
            m_MRE.WaitOne(AppGlobal.s_RequestTimeOut * 10);
        }
        
    }
}
