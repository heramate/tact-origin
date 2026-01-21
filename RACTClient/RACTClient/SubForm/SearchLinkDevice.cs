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
    public partial class SearchLinkDevice : SenderForm
    {

        private DeviceInfoCollection m_DisplayDeviceList = null;

        private DeviceInfoCollection m_SelectedDevieList = new DeviceInfoCollection();

        public DeviceInfoCollection SelectedDeviceList
        {
            get { return m_SelectedDevieList; }
        }

        public SearchLinkDevice()
        {
            InitializeComponent();
            
        }
        
        private void btnDeviceSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtIPAddress.IPAddress == "")
                {
                    MessageBox.Show("아이피를 입력해주세요");
                    return;
                }

                StartSendThread(new ThreadStart(DeviceSearch));
            }
            catch (Exception ex)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "SearchLinkDevice : btnSearch_Click : " + ex.Message.ToString());
            }
            
        }

        private void DeviceSearch()
        {
            RequestCommunicationData tReqestData = null;
            tReqestData = AppGlobal.MakeDefaultRequestData();
            tReqestData.CommType = E_CommunicationType.RequestSearchDeviceForType;

            // 0:장비구분 1:아이피주소 2:사용자아이디 
            string[] tArrRequest = new string[3];

            // 1:일반장비 2:사용자등록장비
            if (rdoNEGroup.Checked == true)
            {
                tArrRequest[0] = "1";
            }
            else
            {
                tArrRequest[0] = "2";
            }

            tArrRequest[1] = txtIPAddress.IPAddress;
            tArrRequest[2] = AppGlobal.s_LoginResult.UserID.ToString();
            

            tReqestData.RequestData = tArrRequest;
            m_Result = null;
            m_MRE.Reset();

            AppGlobal.SendRequestData(this, tReqestData);
            m_MRE.WaitOne(AppGlobal.s_RequestTimeOut);
            
        }

        public override void ResultReceiver(ResultCommunicationData vResult)
        {
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
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, " SearchLinkDevice : ResultReceive Error : Result null ");
                return;
            }

            object tResult = m_Result.ResultData;

            if (tResult.GetType().Equals(typeof(CompressData))) 
            {
                DeviceInfoCollection tDeviceInfos = (DeviceInfoCollection)AppGlobal.DecompressObject((CompressData)m_Result.ResultData);

                if (tDeviceInfos.Count < 1)
                {
                    MessageBox.Show("검색된 장비리스트가 없습니다. 다시 검색해주세요");
                }
                DisplayList(tDeviceInfos);
            }
        }

        public void DisplayList(DeviceInfoCollection aDeviceList)
        {
            this.Enabled = true;
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
                    tRowIndex++;
                }
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

        private void AddRow(int aRowIndex, DeviceInfo aDeviceInfo)
        {
            fgDeviceList[aRowIndex, "Team"] = aDeviceInfo.ORG2Name;
            fgDeviceList[aRowIndex, "CenterName"] = aDeviceInfo.CenterName;
            fgDeviceList[aRowIndex, "ModelName"] = aDeviceInfo.ModelName;
            fgDeviceList[aRowIndex, "DeviceName"] = aDeviceInfo.Name;			//장비이름
            fgDeviceList[aRowIndex, "IPAddress"] = aDeviceInfo.IPAddress;		//장비주소          
            fgDeviceList.Rows[aRowIndex].UserData = aDeviceInfo;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (fgDeviceList.RowSel < 1)
            {
                MessageBox.Show("장비를 선택해주세요.");
                return;
            }

            DeviceInfo tDveviceInfo = (DeviceInfo)fgDeviceList.Rows[fgDeviceList.RowSel].UserData;
            m_SelectedDevieList.Clear();
            m_SelectedDevieList.Add(tDveviceInfo);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}