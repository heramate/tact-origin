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
    public partial class ucDeviceSearch : SenderControl
    {
       

        private DevicePartInfo m_DevicePartInfo;
        private ModelInfo m_ModelInfo;
        private string m_IPAddress;
        private string m_DeviceName;
        // 2013-04-22 - shinyn -검색할 아이피리스트입니다.
        private string m_IPList = string.Empty;
        private DeviceInfoCollection m_DisplayDeviceList;

        // 2013-05-02 - shinyn - 선택된 접속 장비리스트입니다.
        private DeviceInfoCollection m_SelecteDeviceList = new DeviceInfoCollection();

        

        /// <summary>
        /// 장비 이름 속성을 가져오거나 설정합니다.
        /// </summary>
        public string DeviceName
        {
            get { return m_DeviceName; }
            set { m_DeviceName = value; }
        }	

        /// <summary>
        /// IP주소 속성을 가져오거나 설정합니다.
        /// </summary>
        public string IPAddress
        {
            get { return m_IPAddress; }
            set { m_IPAddress = value; }
        }	

        // 검색 조건 중, 장비 모델 콤보박스의 초기화 유무입니다.
        bool m_IsModelListInitialize = false;
        /// <summary>
        /// 모델 정보 속성을 가져오거나 설정합니다.
        /// </summary>
        public ModelInfo ModelInfo
        {
            get { return m_ModelInfo; }
            set { m_ModelInfo = value; }
        }	

        /// <summary>
        /// 장비 분류 속성을 가져오거나 설정합니다.
        /// </summary>
        public DevicePartInfo DevicePartCode
        {
            get { return m_DevicePartInfo; }
            set { m_DevicePartInfo = value; }
        }	

        public ucDeviceSearch()
        {
            InitializeComponent();
            InitializeControl();
        }

        public void InitializeControl()
        {
            AppGlobal.InitializeButtonStyle(btnDeviceSearch);
            AppGlobal.InitializeGridStyle(fgDeviceList);
            //cboDevicePart.Enabled = false;
            cboIPType.Enabled = false;
            AppGlobal.InitializeIPTypeComboBox(cboIPType);
            
            cboIPType.Enabled = true;
            cboIPType.SelectedIndex = 0;
            /*
            cboDevicePart.Enabled = false;
            AppGlobal.InitializeDevicePartComboBox(false, cboDevicePart);
            cboDevicePart.Enabled = true;
            cboDevicePart.SelectedIndex = 0;
            */
            //2013-05-03-shinyn-검색지역을 Comment추가 합니다.
            lblArea.Text = "";
            lblSelectDeviceCount.Text = "";
        }

        private void btnDeviceSearch_Click(object sender, EventArgs e)
        {
           

            if (cboDeviceModel.SelectedIndex < 1 &&
                cboDevicePart.SelectedIndex < 1 &&
                 txtIPAddress.IPAddress.Length == 0 &&
                 txtDeviceName.Text.Trim().Length == 0)
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "최소 하나 이상 검색 조건을 선택 하세요.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            m_SearchInfo = new DeviceSearchInfo();

            m_SearchInfo.DeviceIPAddress = txtIPAddress.IPAddress;
            m_SearchInfo.DeviceName = txtDeviceName.Text.Trim();
            m_SearchInfo.UserID = AppGlobal.s_LoginResult.UserID;

            m_SearchInfo.IPTyep = (AppGlobal.m_ViewIPType == E_IpType.IPTV) ? 1 : (AppGlobal.m_ViewIPType == E_IpType.CATV) ? 2 : 3;

            if (cboDeviceModel.SelectedIndex > 0)
            {
                m_SearchInfo.DeviceModel = ((ModelInfo)cboDeviceModel.Items[cboDeviceModel.SelectedIndex].Tag).ModelID;
            }

            if (cboDevicePart.SelectedIndex > 0)
            {
                m_SearchInfo.DevicePart = ((DevicePartInfo)cboDevicePart.Items[cboDevicePart.SelectedIndex].Tag).Code;
            }


            // 2013-05-03- shinyn - 왼쪽 접근권한그룹 선택후 조회시 권한그룹에 따른 조회가 되도록 한다.
            if (AppGlobal.m_SelectedSystemNode != null)
            {
                object tGroupInfo = AppGlobal.m_SelectedSystemNode.Tag;

                if (tGroupInfo == null)
                {
                    lblArea.Text = "접근권한그룹 선택지역 : 전국";
                }
                else
                {
                    if (tGroupInfo.GetType().Equals(typeof(FACTGroupInfo)))
                    {
                        FACTGroupInfo tFACTGroupInfo = (FACTGroupInfo)tGroupInfo;
                        m_SearchInfo.SelectFACTGroupInfo = tFACTGroupInfo;

                        if (tFACTGroupInfo.ORG1Name != "")
                        {
                            lblArea.Text = "접근권한그룹 선택지역 : " + tFACTGroupInfo.ORG1Name;
                        }

                        if (tFACTGroupInfo.BranchName != "")
                        {
                            lblArea.Text += " > " + tFACTGroupInfo.BranchName;
                        }

                        if (tFACTGroupInfo.CenterName != "")
                        {
                            lblArea.Text += " > " + tFACTGroupInfo.CenterName;
                        }
                    }
                }
                
            }

            this.Enabled = false;
            AppGlobal.ShowLoadingProgress(true);
            StartSendThread(new ThreadStart(SearchDevice));

        }

        private DeviceSearchInfo m_SearchInfo;

        private void SearchDevice()
        {
            RequestCommunicationData tRequestData = null;
           
            tRequestData = AppGlobal.MakeDefaultRequestData();
            tRequestData.CommType = E_CommunicationType.RequestFACTSearchDevice;

            tRequestData.RequestData = m_SearchInfo;

            m_Result = null;
            m_MRE.Reset();

            AppGlobal.SendRequestData(this, tRequestData);
            // 2013-05-02- shinyn - 기다리는 시간 타임아웃을 지정한다.
            //m_MRE.WaitOne();
            m_MRE.WaitOne(AppGlobal.s_RequestTimeOut);
        }

        public override void ResultReceiver(ResultCommunicationData vResult)
        {
            // 2013-05-02- shinyn - 결과값 올때까지 계속 실행한다.
            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument1<ResultCommunicationData>(ResultReceiver), vResult);
                return;
            }

            base.ResultReceiver(vResult);

            // 2013-05-02-shinyn - 결과값이 널인 경우 로그에 저장한다.
            if (m_Result == null || m_Result.ResultData == null)
            {
                this.Enabled = true;
                AppGlobal.ShowLoadingProgress(false);
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning," ucDeviceSearch : ResultReceive Error : Result null ");
                return;
            }


            // 2013-05-02- shinyn - 디바이스 정보인지 데몬요청한것인지 분기처리
            object tResult = m_Result.ResultData;
            if (tResult.GetType().Equals(typeof(CompressData)))
            {
                DisplayList((DeviceInfoCollection)AppGlobal.DecompressObject((CompressData)m_Result.ResultData));
            }
            else
            {
                if (m_Result.ResultData.GetType().Equals(typeof(List<DaemonProcessInfo>)))
                {
                    List<DaemonProcessInfo> tDamonList = m_Result.ResultData as List<DaemonProcessInfo>;

                    int i = 0;
                    foreach (DeviceInfo tDeviceInfo in m_SelecteDeviceList)
                    {
                        ((ClientMain)AppGlobal.s_ClientMainForm).ucSearchDevice1_OnConnectDeviceEvent(tDeviceInfo, tDamonList[i]);
                        i++;
                    }
                }
            }
        }

        /// <summary>
        /// 검색 결과가 존재하지 않는지 확인하는 함수 입니다.
        /// </summary>
        /// <param name="aCount">겸색 결과 수</param>
        /// <returns>검색 결과의 존재 여부</returns>
        public bool CheckEmptyResult(int aCount)
        {
            if (aCount == 0)
            {
                fgDeviceList.Rows.Count = 1;
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm,"검색 조건과 일치하는 장비가 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 표시되고 있는 장비의 개수 입니다.
        /// </summary>
        private int m_DisplayDeviceCount = 0;

        private void cboDevicePart_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cboDevicePart.Enabled) return;

            m_DisplayDeviceList = null;
            fgDeviceList.Rows.Count = 1;

            if (cboDevicePart.SelectedIndex < 1)
            {
                cboDeviceModel.Enabled = false;
            }
            else
            {
                cboDeviceModel.Enabled = true;
            }
            m_IsModelListInitialize = false;
            AppGlobal.InitializeDeviceModelComboBox(cboDeviceModel, cboDevicePart.Text);
            cboDeviceModel.SelectedIndex = 0;
            m_IsModelListInitialize = true;

            txtIPAddress.IPAddress = "";
            txtDeviceName.Text = "";
        }

        /// <summary>
        /// 장비 모델 선택시의 처리 입니다.
        /// </summary>
        private void cboDeviceModel_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (!m_IsModelListInitialize) return;
        }

        /// <summary>
        /// 장비를 그리드에 표시합니다.
        /// </summary>
        public void DisplayList(DeviceInfoCollection aDeviceList)
        {

            AppGlobal.ShowLoadingProgress(false);
            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument1<DeviceInfoCollection>(DisplayList), aDeviceList);
                return;
            }
            this.Enabled = true;
            int tRowIndex = 1;
            m_DisplayDeviceList = aDeviceList;
            try
            {
                fgDeviceList.Redraw = false;
                fgDeviceList.Rows.Count = 1;
                fgDeviceList.Rows.Count = aDeviceList.Count + 1;

                foreach (DeviceInfo tDeviceInfo in aDeviceList)
                {
                    AddRow(tRowIndex, tDeviceInfo);
                    if (tDeviceInfo.InputFlag == E_FlagType.User)
                    {
                        fgDeviceList.Rows[tRowIndex].StyleNew.ForeColor = Color.FromArgb(255, 113, 50);
                    }
                    tRowIndex++;
                }

                lblSelectDeviceCount.Text = "조회 장비대수 : " + aDeviceList.Count.ToString();

            }
            catch (Exception ex)
            {
                //AppGlobal.m_FileLog.PrintLogEnter("InitializeDevice : " + ex.ToString());
                Console.WriteLine(">>>>>>>>>>>>>>>> :" + ex.ToString());
            }
            finally
            {
                fgDeviceList.Redraw = true;
                //  m_IsDeviceInitialized = true;
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

        public DeviceInfo getSelectedDeviceInfo()
        {
            if (fgDeviceList.RowSel < 1) return null;

            return (DeviceInfo)fgDeviceList.Rows[fgDeviceList.RowSel].UserData;
        }

        /// <summary>
        /// 테이블의 선택 모드를 설정합니다.
        /// </summary>
        /// <param name="aMode">선택모드 입니다.</param>
        public void setLineSelectMode(SelectionModeEnum aMode)
        {
            fgDeviceList.SelectionMode = aMode;
            ctmPopup.Visible = aMode == SelectionModeEnum.ListBox;
        }

        /// <summary>
        /// 선택된 행 목록을 반환합니다
        /// </summary>
        /// <returns></returns>
        public RowCollection getSelectedRows()
        {
            return fgDeviceList.Rows.Selected;
        }

        /// <summary>
        /// 현재 표시된 장비 목록을 반환합니다.
        /// </summary>
        /// <returns></returns>
        public DeviceInfoCollection getDisplayDeviceList()
        {
            return m_DisplayDeviceList;
        }

        private void fgDeviceList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //HitTestInfo tHitInfo = fgDeviceList.HitTest(e.X, e.Y);
                //if (tHitInfo.Row > 0)
                //{
                //   fgDeviceList.Select(tHitInfo.Row, tHitInfo.Column);
                //}
                if (fgDeviceList.RowSel > 0)
                {
                    ctmPopup.Popup(MousePosition);
                }

            }
        }

        /// <summary>
        /// 2013-05-02 -shinyn - 여러개의 장비를 접속합니다.
        /// </summary>
        private void ConnectDevice()
        {
            // 2013-05-02 - shinyn - 데몬 정보받고, 연결하면 연결이 제대로 되지 않아, 데몬정보리스트를 전부 받고, 터미널 연결하는것으로 바꾼다.
            RequestCommunicationData tRequestData = null;

            tRequestData = AppGlobal.MakeDefaultRequestData();
            tRequestData.CommType = E_CommunicationType.RequestDaemonInfoList;
            m_SelecteDeviceList.Clear();
            for (int i = 0; i < fgDeviceList.Rows.Selected.Count; i++)
            {
                m_SelecteDeviceList.Add((DeviceInfo)fgDeviceList.Rows.Selected[i].UserData);
            }
            tRequestData.RequestData = m_SelecteDeviceList.Count;
            m_Result = null;
            m_MRE.Reset();

            AppGlobal.SendRequestData(this, tRequestData);
            m_MRE.WaitOne(AppGlobal.s_RequestTimeOut * 10);
        }

        private void mnuQuickConnect_Click(object sender, EventArgs e)
        {
            if (fgDeviceList.Rows.Selected == null || fgDeviceList.Rows.Selected.Count == 0)
            {
                return;
            }

            StartSendThread(new ThreadStart(ConnectDevice));
        }



        private void btnSearchIPList_Click(object sender, EventArgs e)
        {
            try
            {
                SearchIPList tSearchIPList = new SearchIPList();
                DialogResult tResult = tSearchIPList.ShowDialog();
                if (tResult != DialogResult.OK) return;

                int tIPTyep = (AppGlobal.m_ViewIPType == E_IpType.IPTV) ? 1 : (AppGlobal.m_ViewIPType == E_IpType.CATV) ? 2 : 3;

                string tIPList = tIPTyep +"|"+ tSearchIPList.IPString;
                

                if (tSearchIPList != null) tSearchIPList.Dispose();



                m_IPList = tIPList;

                if (m_IPList == "")
                {
                    MessageBox.Show("아이피리스트가 없습니다. 다시 입력해주세요");
                    return;
                }

                this.Enabled = false;
                AppGlobal.ShowLoadingProgress(true);
                StartSendThread(new ThreadStart(SearchDeviceIPList));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message.ToString());
            }
        }

        private void SearchDeviceIPList()
        {
            RequestCommunicationData tRequestData = null;

            tRequestData = AppGlobal.MakeDefaultRequestData();
            tRequestData.CommType = E_CommunicationType.RequestFACTIPSearchDevice;

            tRequestData.RequestData = m_IPList;

            m_Result = null;
            m_MRE.Reset();

            AppGlobal.SendRequestData(this, tRequestData);
            m_MRE.WaitOne();
        }

        private void cboIPType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cboIPType.Enabled) return;
            //m_DisplayDeviceList = null;
            fgDeviceList.Rows.Count = 1;

            if (cboDevicePart.SelectedIndex < 1)
            {
                cboDeviceModel.Enabled = false;
            }
            else
            {
                cboDeviceModel.Enabled = true;
            }
            AppGlobal.m_ViewIPType = ((E_IpType)cboIPType.Items[cboIPType.SelectedIndex].Tag);
            //m_IsModelListInitialize = false;
            AppGlobal.InitializeDevicePartComboBox(false, cboDevicePart);
            cboDevicePart.SelectedIndex = 0;
            //AppGlobal.InitializeDeviceModelComboBox(cboDeviceModel, cboDevicePart.Text);
            ////cboDeviceModel.SelectedIndex = 0;
            // m_IsModelListInitialize = true;

            txtIPAddress.IPAddress = "";
            txtDeviceName.Text = "";
        }
    }
}
