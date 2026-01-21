using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RACTCommonClass;
using System.Threading;
using C1.Win.C1FlexGrid;

namespace RACTClient 
{
    public partial class ModifyShareGroupInfo : SenderForm
    {

        private GroupInfo m_GroupInfo = new GroupInfo();

        public GroupInfo vGroupInfo
        {
            get { return m_GroupInfo; }
            set { m_GroupInfo = value; }
        }


        private E_WorkType m_WorkType = E_WorkType.Add;

        private DeviceInfoCollection m_SelectDeviceList = new DeviceInfoCollection();

        private DeviceInfoCollection m_SelectedDeviceList = new DeviceInfoCollection();

        public ModifyShareGroupInfo()
        {
            InitializeComponent();
            m_GroupInfo = new GroupInfo();
            DisplayData();
        }

        public void InitializeControl()
        {
            trvGroup.InitializeControl();
            trvUserGroup.InitializeControl();

            // 2013-09-09 - shinyn - 로딩하는 것을 숨깁니다.
            ShowLoadingProgress(false);

            AppGlobal.InitializeGridStyle(fgDeviceList1);

            AppGlobal.InitializeGridStyle(fgDeviceList2);

            cboSearch.Items.Clear();
            cboSearch.Items.Add("이름");
            cboSearch.Items.Add("계정");
            cboSearch.SelectedIndex = 0;

            lblSelectDeviceCount.Visible = false;
            lblCheckDeviceCount.Visible = false;

            
        }

        public ModifyShareGroupInfo(E_WorkType aWorkType)
        {
            InitializeComponent();
            m_WorkType = aWorkType;
            DisplayData();   
        }

        private void DisplayData()
        {
            trvGroup.InitializeControl();
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            StartSendThread(new ThreadStart(RequestRactUserList));
        }

        private void RequestRactUserList()
        {
            AppGlobal.s_UserInfoList.Clear();

            RequestCommunicationData tRequestData = null;

            // 2013-08-14 - shinyn - 사용자 장비 공류 리스트 검색 조건입니다.
            // 0: 검색조건[1:이름2:계정] 1:검색명 2:제외할 사용자 아이디
            string[] tArrRequest = new string[3];

            
            if (cboSearch.Text == "이름")
            {
                tArrRequest[0] = "1";
            }
            else
            {
                tArrRequest[0] = "2";
            }


            ShowLoadingProgress(true);

            tArrRequest[1] = txtSearch.Text;
            tArrRequest[2] = AppGlobal.s_LoginResult.UserID.ToString();

            tRequestData = AppGlobal.MakeDefaultRequestData();
            tRequestData.CommType = E_CommunicationType.RequestRactUserList;

            tRequestData.RequestData = tArrRequest;

            m_Result = null;
            m_MRE.Reset();

            AppGlobal.SendRequestData(this, tRequestData);
            m_MRE.WaitOne(AppGlobal.s_RequestTimeOut);
            
        }

        public override void ResultReceiver(ResultCommunicationData vResult)
        {
            // 2013-08-14- shinyn - 결과값 올때까지 계속 실행한다.
            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument1<ResultCommunicationData>(ResultReceiver), vResult);
                return;
            }

            base.ResultReceiver(vResult);

            if (m_Result == null)
            {
                AppGlobal.ShowMessageBox(this, "알 수 없는 에러가 발생했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ShowLoadingProgress(false);
                return;
            }

            if (m_Result.Error.Error != E_ErrorType.NoError)
            {
                AppGlobal.ShowMessageBox(this, m_Result.Error.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                ShowLoadingProgress(false);
                return;
            }


            object tResult = m_Result.ResultData;

            if (tResult.GetType().Equals(typeof(UserInfoCollection)))
            {
                AppGlobal.s_UserInfoList = (UserInfoCollection)m_Result.ResultData;

                
                trvGroup.InitializeControl();

                if (AppGlobal.s_UserInfoList.Count == 0)
                {
                    AppGlobal.ShowMessageBox(this, "검색된 결과가 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                ShowLoadingProgress(false);
            }
            else if(tResult.GetType().Equals(typeof(GroupInfo)))
            {
                //EventProcessor.Run((GroupInfo)m_Result.ResultData, m_WorkType);
                GroupInfo tResultGroupInfo = (GroupInfo)m_Result.ResultData;

                AppGlobal.ShowMessageBox(this, "사용자 공유 장비 목록을 추가 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ShowLoadingProgress(false);

                this.DialogResult = DialogResult.OK;
                Close();
            }
            
        }


        /// <summary>
        /// 2013-08-14 - shinyn - 사용자 공유 장비 리스트를 추가합니다.
        /// </summary>
        /// <param name="aDeviceInfos"></param>
        private void trvUserGroup_OnAddShareDeviceEvent(DeviceInfoCollection aDeviceInfos)
        {
            try
            {
                m_SelectDeviceList = (DeviceInfoCollection)aDeviceInfos;
                DisplayList(aDeviceInfos);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 2013-09-09 - shinyn - 선택한 사용자 정보를 표시합니다.
        /// </summary>
        /// <param name="aDeviceInfos"></param>
        private void trvGroup_OnSelectUserInfoEvent(UserInfo aUserInfo)
        {
            try
            {
                lblUserInfo.Text = "추가할 사용자 목록 : ";
                if (aUserInfo != null)
                {
                    lblUserInfo.Text += aUserInfo.Name + "[" + aUserInfo.Account + "]";
                }
            }
            catch (Exception ex)
            {
            }
        }


        private void DisplayList(DeviceInfoCollection aDeviceInfos)
        {

            int tRowIndex = 1;

            try
            {

                lblSelectDeviceCount.Visible = false;
                fgDeviceList1.Redraw = false;
                fgDeviceList1.Rows.Count = 1;
                fgDeviceList1.Rows.Count = aDeviceInfos.Count + 1;

                foreach (DeviceInfo tDeviceInfo in aDeviceInfos)
                {
                    AddRow(tRowIndex, tDeviceInfo);
                    if (tDeviceInfo.InputFlag == E_FlagType.User)
                    {
                        fgDeviceList1.Rows[tRowIndex].StyleNew.ForeColor = Color.FromArgb(255, 113, 50);
                    }
                    tRowIndex++;
                }

                fgDeviceList1.Redraw = true;
                lblSelectDeviceCount.Text = "조회 장비대수 : " + aDeviceInfos.Count.ToString();
                lblSelectDeviceCount.Visible = true;
            }
            catch (Exception ex)
            {
            }


        }

        private void AddRow(int aRowIndex, DeviceInfo aDeviceInfo)
        {
            string tModelName = string.Empty;
            string tDeviceType = string.Empty;
            if (aDeviceInfo.DeviceType == E_DeviceType.NeGroup)
            {
                ModelInfo tModelInfo = AppGlobal.s_ModelInfoList[aDeviceInfo.ModelID];
                tModelName = tModelInfo.ModelName;
                tDeviceType = "일반장비";
            }
            else if(aDeviceInfo.DeviceType == E_DeviceType.UserNeGroup)
            {
                tModelName = aDeviceInfo.ModelName;
                tDeviceType = "수동장비";
            }

            fgDeviceList1[aRowIndex, "UsrName"] = aDeviceInfo.UsrName + "[" + aDeviceInfo.Account + "]";
            fgDeviceList1[aRowIndex, "Location"] = aDeviceInfo.Location;
            fgDeviceList1[aRowIndex, "DeviceType"] = tDeviceType;
            fgDeviceList1[aRowIndex, "ModelName"] = tModelName;
            fgDeviceList1[aRowIndex, "DeviceName"] = aDeviceInfo.Name;
            fgDeviceList1[aRowIndex, "IPAddress"] = aDeviceInfo.IPAddress;
            fgDeviceList1.Rows[aRowIndex].UserData = aDeviceInfo;
        }

        private void RequestAddShareDevice()
        {

            RequestCommunicationData tRequestData = null;

            

            tRequestData = AppGlobal.MakeDefaultRequestData();
            tRequestData.CommType = E_CommunicationType.RequestAddShareDevice;

            // 2013-09-09 - shinyn - 사용자를 매핑합니다.

            UserInfo tUserInfo =(UserInfo)AppGlobal.m_SelectedUserNode.Tag;

            m_GroupInfo.UserID = tUserInfo.UserID;
            m_GroupInfo.Name = txtGroupName.Text;
            m_GroupInfo.Description = txtGroupDesc.Text;
            m_GroupInfo.DeviceList = m_SelectedDeviceList;


            tRequestData.RequestData = m_GroupInfo;

            m_Result = null;
            m_MRE.Reset();

            AppGlobal.SendRequestData(this, tRequestData);
            m_MRE.WaitOne(AppGlobal.s_RequestTimeOut*5);
        }


        private void SaveShareDeviceInfo()
        {
            try
            {
                if (m_SelectedDeviceList.Count == 0)
                {
                    AppGlobal.ShowMessageBox(this, "선택된 장비리스트가 없습니다. 선택해주세요.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (AppGlobal.m_SelectedUserNode == null)
                {
                    AppGlobal.ShowMessageBox(this, "사용자 장비를 등록할 사용자를 선택해주세요.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (txtGroupName.Text == "")
                {
                    AppGlobal.ShowMessageBox(this, "그룹 이름을 입력해주세요.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                UserInfo tUserInfo = (UserInfo)AppGlobal.m_SelectedUserNode.Tag;

                if (AppGlobal.ShowMessageBox(this, tUserInfo.Name + "[" + tUserInfo.Account + "] 사용자에게 선택된 장비리스트를 " + txtGroupName.Text + " 그룹명으로 추가하시겠습니까?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }
                StartSendThread(new ThreadStart(RequestAddShareDevice));
            }
            catch (Exception ex)
            {
            }
        }

        

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {

                RowCollection tSelectRows = fgDeviceList1.Rows.Selected;

                if (tSelectRows == null || tSelectRows.Count == 0) return;

                foreach(Row tRow in tSelectRows)
                {
                    if (!m_SelectedDeviceList.Contains(tRow.UserData as DeviceInfo))
                    {
                        m_SelectedDeviceList.Add(tRow.UserData as DeviceInfo);
                    }
                }

                DisplaySelectedList();
            }
            catch (Exception ex)
            {
            }

        }


        private void DisplaySelectedList()
        {
            int tRowIndex = 1;
            try
            {
                fgDeviceList2.Redraw = false;
                tRowIndex = fgDeviceList2.Rows.Count;

                foreach (DeviceInfo tDeviceInfo in m_SelectedDeviceList)
                {
                    if(IsAddRow(tDeviceInfo))
                    {
                        tRowIndex = fgDeviceList2.Rows.Count;
                        fgDeviceList2.Rows.Count = fgDeviceList2.Rows.Count + 1;
                        AddSelectedRow(tRowIndex, tDeviceInfo);
                        if (tDeviceInfo.InputFlag == E_FlagType.User)
                        {
                            fgDeviceList2.Rows[tRowIndex].StyleNew.ForeColor = Color.FromArgb(255, 113, 50);
                        }
                    }
                }

                fgDeviceList2.Redraw = true;

                fgDeviceList2.RowSel = 1;

                lblCheckDeviceCount.Text = "선택 장비대수 : " + m_SelectedDeviceList.Count.ToString();

                lblCheckDeviceCount.Visible = true;
            }
            catch (Exception ex)
            {
            }

        }

        private void AddSelectedRow(int aRowIndex, DeviceInfo aDeviceInfo)
        {
            string tModelName = string.Empty;
            string tDeviceType = string.Empty;
            if (aDeviceInfo.DeviceType == E_DeviceType.NeGroup)
            {
                ModelInfo tModelInfo = AppGlobal.s_ModelInfoList[aDeviceInfo.ModelID];
                tModelName = tModelInfo.ModelName;
                tDeviceType = "일반장비";
            }
            else if (aDeviceInfo.DeviceType == E_DeviceType.UserNeGroup)
            {
                tModelName = aDeviceInfo.ModelName;
                tDeviceType = "수동장비";
            }

            fgDeviceList2[aRowIndex, "UsrName"] = aDeviceInfo.UsrName + "[" + aDeviceInfo.Account + "]";
            fgDeviceList2[aRowIndex, "Location"] = aDeviceInfo.Location;
            fgDeviceList2[aRowIndex, "DeviceType"] = tDeviceType;
            fgDeviceList2[aRowIndex, "ModelName"] = tModelName;
            fgDeviceList2[aRowIndex, "DeviceName"] = aDeviceInfo.Name;
            fgDeviceList2[aRowIndex, "IPAddress"] = aDeviceInfo.IPAddress;
            fgDeviceList2.Rows[aRowIndex].UserData = aDeviceInfo;
        }

        private bool IsAddRow(DeviceInfo aDeivceInfo)
        {
            try
            {
                for (int i = 1; i < fgDeviceList2.Rows.Count; i++)
                {
                    DeviceInfo tCheckDeviceInfo = (DeviceInfo)fgDeviceList2.Rows[i].UserData;

                    if (tCheckDeviceInfo.DeviceType == aDeivceInfo.DeviceType &&
                        tCheckDeviceInfo.DeviceID == aDeivceInfo.DeviceID)
                    {
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
            }
            return true;
        }

        private void btnAddAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_SelectDeviceList == null) return;

                foreach (DeviceInfo tDeviceInfo in m_SelectDeviceList)
                {
                    if (!m_SelectedDeviceList.Contains(tDeviceInfo))
                    {
                        m_SelectedDeviceList.Add(tDeviceInfo);
                    }
                }
                DisplaySelectedList();

            }
            catch (Exception ex)
            {
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {

                RowCollection tSelectRows = fgDeviceList2.Rows.Selected;

                if (tSelectRows == null || tSelectRows.Count == 0) return;

                foreach (Row tRow in tSelectRows)
                {
                    DeviceInfo tCheckDeviceInfo = (DeviceInfo)tRow.UserData;

                    foreach (DeviceInfo tDeviceInfo in m_SelectedDeviceList)
                    {
                        if (tDeviceInfo.DeviceID == tCheckDeviceInfo.DeviceID &&
                            tDeviceInfo.DeviceType == tCheckDeviceInfo.DeviceType)
                        {
                            m_SelectedDeviceList.Remove(tCheckDeviceInfo);
                        }
                    }
                }

                DisplaySelectedList();
            }
            catch (Exception ex)
            {
            }
        }

        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
            try
            {
                m_SelectedDeviceList.Clear();
                fgDeviceList2.Rows.Count = 1;
            }
            catch (Exception ex)
            {
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {



            SaveShareDeviceInfo();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }


        /// <summary>
        /// 프로그래스를 표시를 처리 합니다.
        /// </summary>
        /// <param name="aVisable"></param>
        private void ShowLoadingProgress(bool aVisable)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument1<bool>(ShowLoadingProgress), aVisable);
                return;
            }

            progressBarX1.Visible = aVisable;
        }
    }
}