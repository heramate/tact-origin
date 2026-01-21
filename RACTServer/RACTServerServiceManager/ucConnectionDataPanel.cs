using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using RACTServer;
using System.Threading;
using RACTCommonClass;
using System.Collections;
using MKLibrary.MKNetwork;

namespace RACTServerServiceManager
{
    public delegate void EnableChangeHandler(bool aEnable);
    public partial class ucConnectionDataPanel : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ucConnectionDataPanel"/> class.
        /// </summary>
        public ucConnectionDataPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes the control.
        /// </summary>
        public void InitializeControl()
        {
            ServiceManagerGlobal.InitializeButtonStyle(btnStart);
            ServiceManagerGlobal.InitializeButtonStyle(btnStop);
        }

        internal void DisplayBaseData()
        {
            txtServerIP.IPAddress = ServiceManagerGlobal.m_SystemInfo.ServerIP;
            txtServerPort.Text =  ServiceManagerGlobal.m_SystemInfo.ServerPort.ToString();

            txtDBIP.Text = ServiceManagerGlobal.m_DBConnectionInfo.DBServerIP;
            txtDBName.Text = ServiceManagerGlobal.m_DBConnectionInfo.DBName;
            txtDBID.Text = ServiceManagerGlobal.m_DBConnectionInfo.UserID;
            txtDBPassword.Text = ServiceManagerGlobal.m_DBConnectionInfo.Password;

        }

        public void EnableButton(bool aEnable)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EnableChangeHandler(EnableButton), aEnable);
                return;
            }

            btnStart.Enabled = aEnable;
            btnStop.Enabled = !aEnable;
            grbDBInfo.Enabled = aEnable;
            grbServerInfo.Enabled = aEnable;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopService();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartService();
        }

        /// <summary>
        /// 서비스를 시작합니다.
        /// </summary>
        public void StartService()
        {
            if (ServiceManagerGlobal.m_SystemInfo == null)
            {
                MessageBox.Show(this, "환경설정(System.xml) 정보가 없습니다.", "RACT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //SystemConfig tSystemConfig = new SystemConfig();

            //if (txtServerIP.IPAddress.Length == 0 || txtServerPort.Text.Trim().Length == 0
            //    || txtDBIP.Text.Trim().Length == 0 || txtDBName.Text.Trim().Length == 0
            //    || txtDBPassword.Text.Trim().Length == 0 || txtDBID.Text.Trim().Length == 0)
            //{
            //    MessageBox.Show(this, "접속 정보를 모두 입력해주세요.", "RACT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}

            //tSystemConfig.ServerIP = txtServerIP.IPAddress;
            //tSystemConfig.ServerPort = Convert.ToInt32(txtServerPort.Text);

            //tSystemConfig.DBServerIP = txtDBIP.Text;
            //tSystemConfig.DBName = txtDBName.Text;
            //tSystemConfig.Password = txtDBPassword.Text;
            //tSystemConfig.UserID = txtDBID.Text;

            // System 정보 저장
            //ServiceManagerGlobal.SetSystemInfo(tSystemConfig);
            ServiceManagerGlobal.s_MainForm.StartService();
            ServiceManagerGlobal.s_IsRun = true;
        }

        /// <summary>
        /// 서비스를 정지합니다.
        /// </summary>
        public void StopService()
        {
            ServiceManagerGlobal.s_MainForm.StopService();
            ServiceManagerGlobal.s_ServerRemoteGateway = null;
            ServiceManagerGlobal.s_IsServerConnected = false;
            EnableButton(true);
        }
    }
}
