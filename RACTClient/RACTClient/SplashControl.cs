using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Management;
using System.Threading;
using RACTCommonClass;

namespace RACTClient
{
    public partial class SplashControl : UserControl
    {

        /// <summary>
        /// 종료 이벤트 입니다.
        /// </summary>
        public event DefaultHandler OnExit;
        /// <summary>
        /// 로그인 처리 스래드 입니다.
        /// </summary>
        private Thread m_LoginThread = null;     

        public SplashControl()
        {            
            InitializeComponent();

            lblDisplay.Text = "";
        }

        public void InitalizeControl()
        {

            if (AppGlobal.s_IsModeChangeConnect)
            {
                ChangeVisable(true);
                txtID.Text = AppGlobal.s_UserAccount;
                txtPW.Text = AppGlobal.s_Password;
                txtServerIP.Text = AppGlobal.s_ServerIP;
                rdoCS.Checked = true;

                rdoConsole.Enabled = false;

                pbStatus.Visible = false;
                lblDisplay.Text = "";
                btnLogin.Enabled = true;
                txtID.Enabled = true;
                txtPW.Enabled = true;
                txtServerIP.Enabled = true;
            }
            else
            {
                //ChangeVisable(false);
                ChangeVisable(true);
                txtServerIP.Text = AppGlobal.s_ClientOption.ServerIP;
            }

            //txtServerIP.Text = "127.0.0.1";

            //txtServerIP.Text = "118.217.79.48";

            txtServerIP.Text = "118.217.79.41";
            //txtServerIP.Text = "118.217.79.36";
            //txtServerIP.Text = "118.217.79.15";


        }

        private void ChangeVisable(bool aVisable)
        {
            if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Console)
            {
                aVisable = true;
            }
            label1.Visible = aVisable;
            label2.Visible = aVisable;
            //label3.Visible = aVisable;
            label4.Visible = aVisable;

            txtID.Visible = aVisable;
            txtPW.Visible = aVisable;
            //txtServerIP.Visible = aVisable;
            chkSaveID.Visible = aVisable;
            chkSavePW.Visible = aVisable;
            btnLogin.Visible = aVisable;
            butCancel.Visible = aVisable;
            rdoConsole.Visible = aVisable;
            rdoCS.Visible = aVisable;

            if (AppGlobal.m_DirectConnect)
            { 
                rdoConsole.Checked = true;
                rdoCS.Visible = false;
            }

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (AppGlobal.m_DirectConnect)
                AppGlobal.s_RACTClientMode = E_RACTClientMode.Console;
            else
                AppGlobal.s_RACTClientMode = rdoCS.Checked ? E_RACTClientMode.Online : E_RACTClientMode.Console;

            if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online)
            {
                if (txtServerIP.Text.Length == 0)
                {
                    AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "서버 IP를 입력 하세요", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (txtID.Text.Length == 0)
                {
                    AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "ID를 입력 하세요", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (txtPW.Text.Length == 0)
                {
                    AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "PW를 입력 하세요", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            //AppGlobal.s_ServerIP = txtServerIP.Text;
            AppGlobal.s_UserAccount = txtID.Text;
            AppGlobal.s_Password = txtPW.Text;

            txtID.Enabled = false;
            txtPW.Enabled = false;
            txtServerIP.Enabled = false;
            rdoConsole.Enabled = false;
            rdoCS.Enabled = false;

            btnLogin.Enabled = false;
            pbStatus.Visible = true;
            StartLogin();
        }

        /// <summary>
        /// 로그인을 처리하는 부분입니다.
        /// </summary>			
        public void StartLogin()
        {
        
            try
            {
                IPHostEntry tIPhe = Dns.Resolve(SystemInformation.ComputerName);
                IPAddress tIP = tIPhe.AddressList[0];

                AppGlobal.s_ClientIP = "";
                for (int i = 0; i < tIPhe.AddressList.Length; i++)
                {
                    AppGlobal.s_ClientIP += tIPhe.AddressList[i].ToString() + "/";
                }

                AppGlobal.s_ClientIP = AppGlobal.s_ClientIP.Trim('/');

                GetMACAddress();

                SaveLoginInfo();

            }
            catch (Exception ex)
            {
                
            }
            m_LoginThread = new Thread(new ThreadStart(ProcessLogin));
            m_LoginThread.Start();
        }

        /// <summary>
        /// 로그인 정보를 저장 합니다.
        /// </summary>
        private void SaveLoginInfo()
        {
            if (chkSaveID.Checked)
            {
                Kernel32.WriteProfileString("RACT", "ID_SAVE", "1");
                Kernel32.WriteProfileString("RACT", "ID", txtID.Text);
            }
            else
            {
                Kernel32.WriteProfileString("RACT", "ID_SAVE", "0");
                Kernel32.WriteProfileString("RACT", "ID", "");
            }

            if (chkSavePW.Checked)
            {
                Kernel32.WriteProfileString("RACT", "PW_SAVE", "1");
                Kernel32.WriteProfileString("RACT", "PW", txtPW.Text);

            }
            else
            {
                Kernel32.WriteProfileString("RACT", "PW_SAVE", "0");
                Kernel32.WriteProfileString("RACT", "PW", "");
            }

        }

        private void GetMACAddress()
        {
            ManagementObjectSearcher query = new ManagementObjectSearcher
                ("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled='TRUE'");
            ManagementObjectCollection queryCol = query.Get();

            string tMACAddress = "";

            foreach (ManagementObject mo in queryCol)
            {
                tMACAddress = (string)mo["MACAddress"];
            }

            AppGlobal.s_MacAddress = tMACAddress;
        }

        /// <summary>
        /// 로그인을 처리 합니다.
        /// </summary>
        private void ProcessLogin()
        {
            EventProcessor.LoginStarting(true);
        }

        /// <summary>
        /// 프로그램 초기화 정보를 표시합니다.
        /// </summary>
        /// <param name="vString">표시할 정보 입니다.</param>
        public void ShowInitInfo(string aString)
        {
            this.Invoke(new HandlerArgument1<string>(DisplayInitInfo), new object[] { aString });
        }

        public void ShowConsoleModeMessage()
        {
            if (AppGlobal.s_IsModeChangeConnect)
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "서버에 접속 할 수 없습니다.\n 기존 모드를 유지 합니다.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if(OnExit != null) OnExit();
                return;
            }
            else
            {
                //콘솔 모드 접속 해야함
                if (AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "서버에 접속 할 수 없습니다.\n콘솔 모드로 실행 하시겠습니까?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    ChangeVisable(true);
                    txtID.Enabled = true;
                    txtPW.Enabled = true;
                    chkSaveID.Enabled = true;
                    btnLogin.Enabled = true;
                    lblDisplay.Visible = false;
                    pbStatus.Visible = false;

                    rdoConsole.Enabled = true;
                    rdoCS.Enabled = true;

                    return;
                }
                else
                {

                }
                rdoConsole.Checked = true;
                btnLogin_Click(null, null);
            }
        }

        private void DisplayInitInfo(string aString)
        {
            lblDisplay.Text = aString;
            this.Refresh();
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            if (AppGlobal.s_IsModeChangeConnect)
            {
                if (OnExit != null) OnExit();
                return;
            }
            EventProcessor.LoginStarting(false);
        }

        private void SplashControl_Load(object sender, EventArgs e)
        {
            StringBuilder tStringBuilder = new StringBuilder(Kernel32.MAX_PATH);
            if (Kernel32.GetProfileInt("RACT", "ID_SAVE", 0) == 1)
            {
                chkSaveID.Checked = true;
                Kernel32.GetProfileString("RACT", "ID", "", tStringBuilder, Kernel32.MAX_PATH);
                txtID.Text = tStringBuilder.ToString();
            }

            if (Kernel32.GetProfileInt("RACT", "PW_SAVE", 0) == 1)
            {
                Kernel32.GetProfileString("RACT", "PW", "", tStringBuilder, Kernel32.MAX_PATH);
                txtPW.Text = tStringBuilder.ToString();
            }
        }

        /// <summary>
        /// 서버 IP를 가져오기 합니다.
        /// </summary>
        public string ServerIP
        {
            get { return txtServerIP.Text; }
            set { txtServerIP.Text = value;}
        }

        /// <summary>
        /// User ID 를 가져오기 합니다.
        /// </summary>
        public string UserID
        {
            get { return txtID.Text; }
            set { txtID.Text = value; }
        }
        /// <summary>
        /// User PW 를 가져오기 합니다.
        /// </summary>
        public string UserPW
        {
            get { return txtPW.Text; }
            set { txtPW.Text = value; }
        }
        

        internal void StopProgress()
        {
            pbStatus.Visible = false;
        }

        internal void ReStartInit()
        {
            
        }

        private void pbStatus_Click(object sender, EventArgs e)
        {

        }

        private void txtPW_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                btnLogin_Click(null, EventArgs.Empty);
            }
        }

        private void rdoCS_CheckedChanged(object sender, EventArgs e)
        {
            ChangeClientMode();
        }

        private void rdoConsole_CheckedChanged(object sender, EventArgs e)
        {
            ChangeClientMode();
        }

        private void ChangeClientMode()
        {
            txtID.Enabled = rdoCS.Checked;
            txtPW.Enabled = rdoCS.Checked;
            txtServerIP.Enabled = rdoCS.Checked;
        }

        private void txtPW_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtID_TextChanged(object sender, EventArgs e)
        {

        }

        internal void SetLoginInfo(string aIPAddress, string aID, string aPass)
        {
            txtID.Text = aID;
            txtPW.Text = aPass;
            txtServerIP.Text = aIPAddress;
            btnLogin_Click(null, null);
        }
    }
}
