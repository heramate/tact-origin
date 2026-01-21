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
    public partial class ConnectionHistorySearch : BaseForm
    {
        public ConnectionHistorySearch()
        {
            InitializeComponent();

        }

        public void InitializeControl()
        {
            AppGlobal.InitializeButtonStyle(btnSearch);
            AppGlobal.InitializeGridStyle(fgSearchList);

            AddButton(E_ButtonType.Close, E_ButtonSide.Right, "닫기");

            dtpStartDate.Value = DateTime.Now.AddDays(-7);
            dtpStartTime.Value = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));

            dtpEndDate.Value = DateTime.Now;
            dtpEndTime.Value = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
        }

        protected override void ButtonProcess(E_ButtonType aButtonType)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            RequestCommunicationData tRequestData = null;

            ConnectionHistoryRequestInfo tConnectionHistoryInfo = new ConnectionHistoryRequestInfo();
            tConnectionHistoryInfo.StartTime = DateTime.Parse(dtpStartDate.Value.ToString("yyyy-MM-dd") + " " + dtpStartTime.Value.ToString("HH:mm:ss"));
            tConnectionHistoryInfo.EndTime =  DateTime.Parse(dtpEndDate.Value.ToString("yyyy-MM-dd") + " " + dtpEndTime.Value.ToString("HH:mm:ss"));
            tConnectionHistoryInfo.UserID = AppGlobal.s_LoginResult.UserID;

            tRequestData = AppGlobal.MakeDefaultRequestData();
            tRequestData.CommType = E_CommunicationType.RequestConnectionHistory;

            tRequestData.RequestData = tConnectionHistoryInfo;

            m_Result = null;
            m_MRE.Reset();

            AppGlobal.SendRequestData(this, tRequestData);
            m_MRE.WaitOne(10000);

            CheckedResult();
        }

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

            DisplayConnectioHistory((ConnectionHistoryInfoCollection)m_Result.ResultData);

           // this.Close();
        }

        private void DisplayConnectioHistory(ConnectionHistoryInfoCollection aCollection)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument1<ConnectionHistoryInfoCollection>(DisplayConnectioHistory), new object[] { aCollection });
                return;
            }

            fgSearchList.Rows.Count = 1;

            try
            {
              //  fgSearchList.Visible = false;
                DeviceConnectionHistoryInfo tHistoryInfo;
                for (int i = 0; i < aCollection.Count; i++)
                {
                    tHistoryInfo = aCollection[i];
                    fgSearchList.Rows.Count++;
                    fgSearchList[i+1, 0] = i + 1;
                    fgSearchList[i+ 1, 1] =tHistoryInfo.DeviceName;
                    fgSearchList[i+1, 2] = tHistoryInfo.IPAddress;
                    fgSearchList[i+1, 3] = tHistoryInfo.ConnectionTime.ToString("yyyy-MM-dd HH:mm:ss");
                    if (tHistoryInfo.ConnectionType == E_DeviceConnectType.DisConnection)
                    {
                        fgSearchList[i+1, 4] = tHistoryInfo.EndTime.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    fgSearchList.Rows[i+1].UserData = tHistoryInfo;
                }
            }
            catch (Exception ex)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, ex.ToString());
            }

            //fgSearchList.Visible = true;
        }

        private void fgSearchList_MouseDown(object sender, MouseEventArgs e)
        {
            C1.Win.C1FlexGrid.HitTestInfo tyHitTestInfo = fgSearchList.HitTest(e.X, e.Y);
            if (e.Button == MouseButtons.Right)
            {
                if (tyHitTestInfo.Row > 0)
                {
                    fgSearchList.Select(tyHitTestInfo.Row,tyHitTestInfo.Column);
                    if (fgSearchList.RowSel > 0)
                    {
                        cmPopup.Popup(MousePosition);
                    }
                }
            }
        }

        private void mnuCommandHistory_Click(object sender, EventArgs e)
        {
            RequestCommunicationData tRequestData = null;

            DeviceConnectionHistoryInfo tConnectionHistoryInfo = fgSearchList.Rows[fgSearchList.RowSel].UserData as DeviceConnectionHistoryInfo;

            TelnetCommandHistoryRequestInfo tCommandRequestInfo= new TelnetCommandHistoryRequestInfo();
            tCommandRequestInfo.ConnectionLogID = tConnectionHistoryInfo.ID;

            tRequestData = AppGlobal.MakeDefaultRequestData();
            tRequestData.CommType = E_CommunicationType.RequestCommandHistory;

            tRequestData.RequestData = tCommandRequestInfo;

            m_Result = null;
            m_MRE.Reset();

            AppGlobal.SendRequestData(this, tRequestData);
            m_MRE.WaitOne(10000);

            CheckedCommandResult();
        }

        private void CheckedCommandResult()
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

            DisplayCommandConnectioHistory((TelnetCommandHistoryInfoCollection)m_Result.ResultData);
        }
        /// <summary>
        /// 팝업 창 입니다.
        /// </summary>
        private MKDropDown m_DetailsInfo;
        /// <summary>
        /// 명령 리스트 입니다.
        /// </summary>
        private ucCommandHistory m_CommandHistory;

        private void DisplayCommandConnectioHistory(TelnetCommandHistoryInfoCollection aCollection)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument1<TelnetCommandHistoryInfoCollection>(DisplayCommandConnectioHistory), new object[] { aCollection });
                return;
            }
            m_CommandHistory = new ucCommandHistory();
            m_CommandHistory.InitializeControl(aCollection);

            m_DetailsInfo = new MKDropDown(m_CommandHistory);
            Point tPoint = new Point(Control.MousePosition.X, Control.MousePosition.Y);
            m_DetailsInfo.Show(tPoint, ToolStripDropDownDirection.Default);
        }
    }
}