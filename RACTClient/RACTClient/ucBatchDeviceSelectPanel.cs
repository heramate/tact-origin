using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using C1.Win.C1FlexGrid;
using RACTCommonClass;
// 2013-05-02 - shinyn - 장비접속오류를 해결하기 위해 데몬정보와 장비정보를 함께 보내서 접속한다.
using System.Threading;


namespace RACTClient
{
    // 2013-05-02 - shinyn - 장비접속오류를 해결하기 위해 데몬정보와 장비정보를 함께 보내서 접속한다.
    //public partial class ucBatchDeviceSelectPanel : UserControl
    public partial class ucBatchDeviceSelectPanel : SenderControl
    {
        /// <summary>
        ///  장비 연결 이벤트 입니다.
        /// </summary>
        public event ConnectDeviceHandler OnConnectDeviceEvent;
        private DeviceInfoCollection m_SelectedDeviceList;

        /// <summary>
        /// 2013-05-02 - shinyn - 선택된 장비로 접속합니다.
        /// </summary>
        private DeviceInfo m_SelectedDeviceInfo = null;
        /// <summary>
        /// 선택된 장비목록 속성을 가져오거나 설정합니다.
        /// </summary>
        public DeviceInfoCollection Devicelist
        {
            get { return m_SelectedDeviceList; }
            set { m_SelectedDeviceList = value; }
        }	

        /// <summary>
        /// 선택된 장비 목록 속성을 가져오거나 설정합니다.
        /// </summary>
        public DeviceInfoCollection DeviceList
        {
            get { return m_SelectedDeviceList; }
            set { m_SelectedDeviceList = value; }
        }


        public ucBatchDeviceSelectPanel()
        {
            InitializeComponent();

            ucDeviceSearch.fgDeviceList.DoubleClick += new EventHandler(fgDeviceSearchList_DoubleClick);
        }

        internal void initializeControl()
        {
            AppGlobal.InitializeButtonStyle(btnAdd);
            AppGlobal.InitializeButtonStyle(btnAddAll);
            AppGlobal.InitializeButtonStyle(btnRemove);
            AppGlobal.InitializeButtonStyle(btnRemoveAll);

            AppGlobal.InitializeGridStyle(fgDeviceList);
            m_SelectedDeviceList = new DeviceInfoCollection();
            ucDeviceSearch.InitializeControl();
            ucDeviceSearch.setLineSelectMode(C1.Win.C1FlexGrid.SelectionModeEnum.ListBox);
            fgDeviceList.Rows.Count = 1;

            lblCheckDeviceCount.Text = "";
        }

        /// <summary>
        /// 2013-05-02 - shinyn - 응답에 따라 실행한다.
        /// </summary>
        /// <param name="vResult"></param>
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
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, " ucBatchDeviceSelectPanel : ResultReceive Error : Result null ");
                return;
            }

            object tResult = m_Result.ResultData;

            if (tResult.GetType().Equals(typeof(CompressData)))
            {
            }
            else
            {
                if (m_Result.ResultData.GetType().Equals(typeof(List<DaemonProcessInfo>)))
                {
                    List<DaemonProcessInfo> tDaemonList = m_Result.ResultData as List<DaemonProcessInfo>;

                    OnConnectDeviceEvent(m_SelectedDeviceInfo, tDaemonList[0]);
                }
            }
        }

        /// <summary>
        /// 2013-05-02 - shinyn - 스레드를 실행하여 접속한다.
        /// </summary>
        private void ConnectDevice()
        {
            RequestCommunicationData tRequestData = null;
            tRequestData = AppGlobal.MakeDefaultRequestData();
            tRequestData.CommType = E_CommunicationType.RequestDaemonInfoList;
            tRequestData.RequestData = 1;
            m_Result = null;
            m_MRE.WaitOne(AppGlobal.s_RequestTimeOut * 10);

            
        }

        void ucDeviceSearch_OnConnectDeviceEvent(DeviceInfo aDeviceInfo)
        {
            if (OnConnectDeviceEvent != null)
            {
                // 2013-05-02- shinyn - 장비접속 오류를 해결하기 위해 데몬정보와 장비 정보를 함께 보내 접속하도록 수정한다.
                //OnConnectDeviceEvent(aDeviceInfo);

                m_SelectedDeviceInfo = aDeviceInfo;

                StartSendThread(new ThreadStart(ConnectDevice));
            }
        }

        /// <summary>
        /// 장비를 그리드에 표시합니다.
        /// </summary>
        public void DisplayList()
        {
            int tRowIndex = 1;
            try
            {
                //fgDeviceList.Redraw = false;


                //fgDeviceList.Rows.Count = 1;
                //fgDeviceList.Rows.Count = m_SelectedDeviceList.Count + 1;

                //foreach (DeviceInfo tDeviceInfo in m_SelectedDeviceList)
                //{
                //    AddRow(tRowIndex, tDeviceInfo);
                //    if (tDeviceInfo.InputFlag == E_FlagType.User)
                //    {
                //        fgDeviceList.Rows[tRowIndex].StyleNew.ForeColor = Color.FromArgb(255, 113, 50);
                //    }
                //    tRowIndex++;
                //}

                //fgDeviceList.RowSel = 1;

                // 2013-04-22 - shinyn - 전체 추가시 기존에 추가한것에 더해서 추가되도록 수정
                fgDeviceList.Redraw = false;

                // --> 수정
                tRowIndex = fgDeviceList.Rows.Count;
                
                foreach (DeviceInfo tDeviceInfo in m_SelectedDeviceList)
                {
                    if (IsAddRow(tDeviceInfo))
                    {
                        tRowIndex = fgDeviceList.Rows.Count;
                        fgDeviceList.Rows.Count = fgDeviceList.Rows.Count + 1;
                        AddRow(tRowIndex, tDeviceInfo);
                        if (tDeviceInfo.InputFlag == E_FlagType.User)
                        {
                            fgDeviceList.Rows[tRowIndex].StyleNew.ForeColor = Color.FromArgb(255, 113, 50);
                        }
                        //tRowIndex++;
                    }

                    
                }

                fgDeviceList.RowSel = 1;

                lblCheckDeviceCount.Text = "선택 장비대수 : " + m_SelectedDeviceList.Count.ToString();

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

        private bool IsAddRow(DeviceInfo aDeviceInfo)
        {

            for (int i = 1; i < fgDeviceList.Rows.Count; i++)
            {
                if (fgDeviceList.Rows[i][8].ToString() == aDeviceInfo.IPAddress)
                {
                    return false;
                }
            }
            return true;
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            RowCollection tSelectedRows = ucDeviceSearch.getSelectedRows();

            if (tSelectedRows == null || tSelectedRows.Count == 0) return;
            
            foreach (Row tRow in tSelectedRows)
            {
                // 2013-04-24 - 이미 추가되있으면 추가하지 않는다. 
                if (!m_SelectedDeviceList.Contains(tRow.UserData as DeviceInfo))
                {
                    m_SelectedDeviceList.Add(tRow.UserData as DeviceInfo);
                }
                
            }

            DisplayList();
        }


        private void btnAddAll_Click(object sender, EventArgs e)
        {
            m_SelectedDeviceList = ucDeviceSearch.getDisplayDeviceList();
            if (m_SelectedDeviceList == null) return;
            DisplayList();
        }

        // 2013-04-22 - shinyn - 더블클릭시 장비 추가되도록 수정
        private void fgDeviceSearchList_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                C1FlexGrid fgDeviceSearchList = (C1FlexGrid)sender;
                int tSelectRow = fgDeviceSearchList.RowSel;
                DeviceInfo tDeviceInfo = (DeviceInfo)fgDeviceSearchList.Rows[tSelectRow].UserData;

                // 2013-04-22 - shinyn - 전체 추가시 기존에 추가한것에 더해서 추가되도록 수정
                fgDeviceList.Redraw = false;

                int tRowIndex = 0;

                if (IsAddRow(tDeviceInfo))
                {
                    tRowIndex = fgDeviceList.Rows.Count;
                    fgDeviceList.Rows.Count = fgDeviceList.Rows.Count + 1;
                    AddRow(tRowIndex, tDeviceInfo);
                    if (tDeviceInfo.InputFlag == E_FlagType.User)
                    {
                        fgDeviceList.Rows[tRowIndex].StyleNew.ForeColor = Color.FromArgb(255, 113, 50);
                    }
                    m_SelectedDeviceList.Add(tDeviceInfo);
                }

                fgDeviceList.RowSel = 1;

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

        private void btnRemove_Click(object sender, EventArgs e)
        {
            RowCollection tSelectedRows = fgDeviceList.Rows.Selected;
            if (tSelectedRows == null || tSelectedRows.Count == 0) return;
            foreach (Row tRow in tSelectedRows)
            {
                m_SelectedDeviceList.Remove(((DeviceInfo)tRow.UserData).DeviceID);
                fgDeviceList.Rows.Remove(tRow);
            }
            fgDeviceList.Rows.Count = m_SelectedDeviceList.Count + 1;
        }

        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
            m_SelectedDeviceList = new DeviceInfoCollection();
            fgDeviceList.Rows.Count = 1;
        }
    }
}
