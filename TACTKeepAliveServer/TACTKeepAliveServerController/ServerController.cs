using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TACT.KeepAliveServer;
using RACTCommonClass;




namespace TACTKeepAliveServerTester
{
    public partial class ServerController : Form
    {
        KeepAliveServer m_Server = null;

        public ServerController()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            btnStop.Enabled = true;

            if (m_Server == null) {
                m_Server = new KeepAliveServer(Application.StartupPath);
            }

            if (!m_Server.Start())
            {
                MessageBox.Show("서버실행 실패");
                btnStart.Enabled = true;
            }

            if (GlobalClass.m_SystemInfo == null)
            {
                MessageBox.Show("환경설정값이 로드되지 않았습니다.");
                btnStart.Enabled = true;
                return;
            }
            else
            {
                txtDBServerIP.Text = GlobalClass.m_SystemInfo.DBServerIP;
                txtDBName.Text = GlobalClass.m_SystemInfo.DBName;
                txtDBLoginID.Text = GlobalClass.m_SystemInfo.UserID;
                txtDBLoginPwd.Text = GlobalClass.m_SystemInfo.Password;

                txtServerIP.IPAddress = GlobalClass.m_SystemInfo.ServerIP;
                txtServerPort.Text = GlobalClass.m_SystemInfo.ServerPort.ToString();
                txtServerId.Text = GlobalClass.m_SystemInfo.ServerID.ToString();

                txtSSHServerIP.Text = GlobalClass.m_SystemInfo.SSHServerIP;
                txtSSHServerPort.Text = GlobalClass.m_SystemInfo.SSHServerPort.ToString();
                txtSSHTunnelPortRange.Text = GlobalClass.m_SystemInfo.SSHTunnelPortRange;
                txtSSHUserID.Text = GlobalClass.m_SystemInfo.SSHUserID;
                txtSSHPassword.Text = GlobalClass.m_SystemInfo.SSHPassword;

                txtTunnelTimeout.Text = GlobalClass.m_SystemInfo.SSHTunnelUseTimeoutSeconds.ToString();
                chkBase64.Checked = GlobalClass.m_SystemInfo.KeepAliveBase64Encode;

                txtDaemonRequestTimeout.Text = GlobalClass.m_SystemInfo.DaemonRequestTimeoutSeconds.ToString();

                txtTunnelRequestTimeout.Text = GlobalClass.m_SystemInfo.TunnelRequestTimeoutSeconds.ToString();

                chkPortListLogYN.Checked = GlobalClass.m_SystemInfo.FileLogDetailYN;
            }

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (m_Server == null) return;
            try
            {
                if (m_Server != null)
                {
                    m_Server.Stop();
                    m_Server = null;
                }
                btnStop.Enabled = false;
                btnStart.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (m_Server == null) return;
            if (ipTestDevice.Text.Length < 1)
            {
                MessageBox.Show("먼저 SSH요청을 추가할 장비IP를 입력하세요.");
                return;
            }
            m_Server._Test1(ipTestDevice.Text);
            btnDevListUpdate_Click(null, null);
            btnTunnelPort_Click(null, null);
        }

        private void btnDevListUpdate_Click(object sender, EventArgs e)
        {
            txtReqDevList.Text = "";
            if (m_Server == null) return;
            ArrayList replyList = m_Server.GetKeepAliveWatingList();
            StringBuilder sb = new StringBuilder();

            KeepAliveMsg replyMsg = null;
            for (int i = 0; i < replyList.Count; i++)
            {
                replyMsg = (KeepAliveMsg)replyList[i];
                if (replyMsg == null) continue;
                //장비IP        ┃요청옵션 ┃할당포트 ┃발송대기시작
                sb.Append(replyMsg.DeviceIP 
                        + " ┃" + replyMsg.SSHTunnelCreateOption.ToString() 
                        + " ┃" + replyMsg.SSHTunnelPort
                        + " ┃" + (replyMsg.TimestampWaiting == DateTime.MinValue ? "-" : replyMsg.TimestampWaiting.ToString("HH:mm:ss")) + "\r\n");
            }
            txtReqDevList.Text = sb.ToString();
        }

        private void btnTunnelPort_Click(object sender, EventArgs e)
        {
            tunnelPortList.Text = "";
            if (m_Server == null) return;
            ArrayList list = m_Server.GetTunnelPortInfoList();
            StringBuilder sb = new StringBuilder();

            DateTime stateChangeTime = DateTime.MinValue;
            TunnelInfo tunnelInfo = null;
            for (int i = 0; i < list.Count; i++)
            {
                tunnelInfo = (TunnelInfo)list[i];
                if (tunnelInfo == null) continue;

                switch (tunnelInfo.TunnelState)
                {
                    case E_TunnelState.Closed:
                        stateChangeTime = tunnelInfo.TimeStampClosed;
                        break;
                    case E_TunnelState.WaitingOpen:
                        stateChangeTime = tunnelInfo.TimeStampWaitingOpen;
                        break;
                    case E_TunnelState.Opened:
                        stateChangeTime = tunnelInfo.TimeStampOpened;
                        break;
                    case E_TunnelState.Connected:
                        stateChangeTime = tunnelInfo.TimeStampConnected;
                        break;
                    case E_TunnelState.WaitingClose:
                        stateChangeTime = tunnelInfo.TimeStampWaitingClose;
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false, string.Format("처리되지 않은 TunnelState", tunnelInfo.TunnelState));
                        break;
                }
                //string dueTimeSpan = ((TimeSpan)DateTime.Now.Subtract(stateChangeTime)).ToString("mm:ss");

                //sb.Append("장비IP \t 터널포트 \t 터널상태 \t 연결세션수 \t 상태설정시각(초과시각)");
                sb.Append(string.Format("{0} ┃{1} ┃{2} ┃{3} ┃{4}\r\n", 
                                        tunnelInfo.DeviceIP, 
                                        tunnelInfo.TunnelPort, 
                                        tunnelInfo.TunnelState.ToString(), 
                                        tunnelInfo.GetTunnelConnectCount(),
                                        stateChangeTime == DateTime.MinValue ? "-" : stateChangeTime.ToString("HH:mm:ss"))
                        );
            }
            tunnelPortList.Text = sb.ToString();
        }

        private void btnDevListClear_Click(object sender, EventArgs e)
        {
            if (m_Server == null) return;
            m_Server.ClearAllRequest();
            btnDevListUpdate_Click(null, null);
            btnTunnelPort_Click(null, null);
        }

    }
}
