using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using RACTCommonClass;

namespace RACTClient
{
    /// <summary>
    /// 2013-01-11 - shinyn - UserControl에서 SenderControl로 변환해서 서버에 정보를 요청한다.
    /// </summary>
    public partial class ucMainBatchRegisterBasePanel : SenderControl
    {
        E_ProcessStep m_ProcessStep;


        public ucMainBatchRegisterBasePanel()
        {
            InitializeComponent();
        }

        public void initializeControl()
        {
            AppGlobal.InitializeButtonStyle(btn1);
            AppGlobal.InitializeButtonStyle(btn2);

            ucBatchDeviceSelectPanel.initializeControl();
            ucBatchRegisterPanel.initializeControl();
            ((ClientMain)AppGlobal.s_ClientMainForm).ucSearchDevice1.OnRegisterDeviceList += new DeviceRegisterHandler(ucSearchDevice1_OnRegisterDeviceList);

            onPrevious();
        }

        void ucSearchDevice1_OnRegisterDeviceList(DeviceInfoCollection aCollection)
        {
            ucBatchRegisterPanel.initializeControl();
            ucBatchRegisterPanel.Devicelist = aCollection;
            ucBatchDeviceSelectPanel.Devicelist = aCollection;
            ucBatchRegisterPanel.DisplayList();
            setControlStatus();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            onPrevious();
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            setControlStatus();
        }

        /// <summary>
        /// 컨트롤의 상태를 결정합니다.
        /// </summary>
        public void setControlStatus()
        {
            // 다음 버튼을 클릭했을 때 실행합니다.
             if (m_ProcessStep == E_ProcessStep.Select)
            {
                if (ucBatchDeviceSelectPanel.Devicelist == null || ucBatchDeviceSelectPanel.Devicelist.Count == 0)
                {
                    AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "장비를 선택하세요.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                onNext();

                ucBatchDeviceSelectPanel.Visible = false;
                ucBatchRegisterPanel.Visible = true;

                btn1.Visible = true;
                btn2.Text = "등록";
                if (ucBatchDeviceSelectPanel.Devicelist.Count == 0)
                {
                    btn2.Enabled = false;
                }

                lblSelectDevice.Font = new System.Drawing.Font("굴림", 9.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                lblSelectDevice.ForeColor = Color.LightGray;
                lblBatchRegister.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
                lblBatchRegister.ForeColor = Color.Black;

                m_ProcessStep = E_ProcessStep.Register;
            }
            // 등록 버튼을 클릭했을 때 실행합니다.
            else
            {
                if (onRegistration())
                {
                    ucBatchDeviceSelectPanel.initializeControl();
                    ucBatchRegisterPanel.initializeControl();
                    onPrevious();
                }
            }
        }

        /// <summary>
        /// 다음 버튼의 기능을 수행하는 함수입니다.
        /// </summary>
        private void onNext()
        {
            ucBatchRegisterPanel.initializeControl();
            ucBatchRegisterPanel.Devicelist = ucBatchDeviceSelectPanel.Devicelist;
            ucBatchRegisterPanel.DisplayList();
            m_ProcessStep = E_ProcessStep.Register;
        }

        /// <summary>
        /// 이전 버튼의 기능을 수행하는 함수입니다.
        /// </summary>
        private void onPrevious()
        {
            ucBatchDeviceSelectPanel.Visible = true;
            ucBatchRegisterPanel.Visible = false;

            btn1.Visible = false;
            btn2.Text = "다음>>";
            if (ucBatchDeviceSelectPanel.Devicelist.Count == 0)
            {
                btn2.Enabled = true;
            }

            lblSelectDevice.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            lblSelectDevice.ForeColor = Color.Black;
            lblBatchRegister.Font = new System.Drawing.Font("굴림", 9.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            lblBatchRegister.ForeColor = Color.LightGray;

            m_ProcessStep = E_ProcessStep.Select;
        }

        /// <summary>
        /// 등록 버튼의 기능을 수행하는 함수입니다. -
        /// 2013-01-11 - shinyn - 등록시 장비등록할것인지 파일등록 할 것인지 선택하도록 한다. 메소드 수정
        /// 
        /// </summary>
        private bool onRegistration()
        {
            AddDeviceList tAddDeviceList = new AddDeviceList();

            if (tAddDeviceList.ShowDialog() != DialogResult.OK) return false;

            E_AddDeviceType tAddDeviceType = tAddDeviceList.AddDeviceType;

            bool tRet = true;

            switch (tAddDeviceType)
            {
                case E_AddDeviceType.AddUserGroup:
                    tRet = ucBatchRegisterPanel.SaveDeviceInfo();
                    break;
                case E_AddDeviceType.SaveDeviceList:
                    try
                    {

                        if (!(ucBatchDeviceSelectPanel.Devicelist.Count > 0))
                        {
                            AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "선택된 장비목록이 없습니다. 다시 선택해주세요.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }

                        // 2013-01-11 - shinyn - 장비목록에 대한 복원명령어를 가져온다.

                        RequestCommunicationData tRequestData = null;
                        CfgRestoreCommandRequestInfoCollection tRequestInfos = new CfgRestoreCommandRequestInfoCollection();

                        foreach(DeviceInfo tDeviceInfo in ucBatchDeviceSelectPanel.Devicelist)
                        {
                            CfgRestoreCommandRequestInfo tRequestInfo = new CfgRestoreCommandRequestInfo();

                            tRequestInfo.CommandPart = E_CommandPart.ConfigBRRestore;
                            tRequestInfo.ModelID = tDeviceInfo.ModelID;
                            tRequestInfo.IPAddress = tDeviceInfo.IPAddress;

                            tRequestInfos.Add(tRequestInfo);
                        }

                        tRequestData = AppGlobal.MakeDefaultRequestData();
                        tRequestData.CommType = E_CommunicationType.RequestDevicesCfgRestoreCommand;

                        tRequestData.RequestData = tRequestInfos;

                        m_Result = null;
                        m_MRE.Reset();

                        AppGlobal.SendRequestData(this, tRequestData);
                        m_MRE.WaitOne(AppGlobal.s_RequestTimeOut+5000);

                        DeviceCfgSaveInfoCollection tDeviceCfgSaveInfos = new DeviceCfgSaveInfoCollection();

                        if (m_Result == null)
                        {
                            //타임 아웃 처리 콘솔 모드로 변경 해야 하나?
                            AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "타임 아웃 발생했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        else
                        {
                            if (m_Result.Error.Error != E_ErrorType.NoError)
                            {
                                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "오류 발생:" + m_Result.Error.ErrorString, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false ;
                            }

                            tDeviceCfgSaveInfos = (DeviceCfgSaveInfoCollection)m_Result.ResultData;

                            foreach (DeviceCfgSaveInfo tDeviceCfgSaveInfo in tDeviceCfgSaveInfos)
                            {
                                SetTelnetReservedString(tDeviceCfgSaveInfo.CfgSaveInfoCollection);
                            }
                        }

                        tRet = AppGlobal.SaveDeviceList(AppGlobal.s_LoginResult.UserInfo, ucBatchDeviceSelectPanel.Devicelist.DeepClone(), tDeviceCfgSaveInfos.DeepClone());

                        if (tRet) AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "저장하였습니다.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, ex.Message.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    break;
            }


            return tRet;
        }

        /// <summary>
        /// 2013-01-11 - shinyn - 텔넷 예약어 매칭처리
        /// </summary>
        /// <param name="tSelectCfgSaveInfos"></param>
        private void SetTelnetReservedString(CfgSaveInfoCollection tSelectCfgSaveInfos)
        {
            string tConfigFileName = string.Empty;

            foreach (CfgSaveInfo tCfgSaveInfo in tSelectCfgSaveInfos)
            {
                foreach (CfgRestoreCommand tCfgRestoreCommand in tCfgSaveInfo.CfgRestoreCommands)
                {
                    if (tCfgRestoreCommand.Cmd != "")
                    {
                        if (tCfgRestoreCommand.Cmd.IndexOf(TelnetReservedString.c_FTPIP) > -1)
                        {
                            tCfgRestoreCommand.Cmd = tCfgRestoreCommand.Cmd.Replace(TelnetReservedString.c_FTPIP, tCfgSaveInfo.FTPServerIP);
                        }
                        else if (tCfgRestoreCommand.Cmd.IndexOf(TelnetReservedString.c_FTPUSER) > -1)
                        {
                            tCfgRestoreCommand.Cmd = tCfgRestoreCommand.Cmd.Replace(TelnetReservedString.c_FTPUSER, tCfgSaveInfo.CenterFTPID);
                        }
                        else if (tCfgRestoreCommand.Cmd.IndexOf(TelnetReservedString.c_FTPPASSEORD) > -1)
                        {
                            tCfgRestoreCommand.Cmd = tCfgRestoreCommand.Cmd.Replace(TelnetReservedString.c_FTPPASSEORD, tCfgSaveInfo.CenterFTPPW);
                        }
                        else if (tCfgRestoreCommand.Cmd.IndexOf(TelnetReservedString.c_CONFIGFILENAME) > -1)
                        {
                            tConfigFileName = tCfgSaveInfo.FileName;
                            tCfgRestoreCommand.Cmd = tCfgRestoreCommand.Cmd.Replace(TelnetReservedString.c_CONFIGFILENAME, tConfigFileName);
                        }
                        else if (tCfgRestoreCommand.Cmd.IndexOf(TelnetReservedString.c_CONFIGFILENAMEEXT) > -1)
                        {
                            tConfigFileName = tCfgSaveInfo.FileName;
                            if (tCfgSaveInfo.FileExtend != "")
                            {
                                tConfigFileName += "." + tCfgSaveInfo.FileExtend;
                            }
                            tCfgRestoreCommand.Cmd = tCfgRestoreCommand.Cmd.Replace(TelnetReservedString.c_CONFIGFILENAMEEXT, tConfigFileName);
                        }
                        else if (tCfgRestoreCommand.Cmd.IndexOf(TelnetReservedString.c_CONFIGFILENAME16) > -1)
                        {
                            tConfigFileName = tCfgSaveInfo.FileName;
                            tCfgRestoreCommand.Cmd = tCfgRestoreCommand.Cmd.Replace(TelnetReservedString.c_CONFIGFILENAME16, tConfigFileName);
                        }
                        else if (tCfgRestoreCommand.Cmd.IndexOf(TelnetReservedString.c_CONFIGFILENAMEEXT16) > -1)
                        {
                            tConfigFileName = tCfgSaveInfo.FileName;
                            if (tCfgSaveInfo.FileExtend != "")
                            {
                                tConfigFileName += "." + tCfgSaveInfo.FileExtend;
                            }
                            tCfgRestoreCommand.Cmd = tCfgRestoreCommand.Cmd.Replace(TelnetReservedString.c_CONFIGFILENAMEEXT16, tConfigFileName);
                        }
                        else if (tCfgRestoreCommand.Cmd.IndexOf(TelnetReservedString.c_CONFIGFULLFILENAME) > -1)
                        {
                            tConfigFileName = tCfgSaveInfo.FileName;
                            if (tCfgSaveInfo.FileExtend != "")
                            {
                                tConfigFileName += "." + tCfgSaveInfo.FileExtend;
                            }
                            tCfgRestoreCommand.Cmd = tCfgRestoreCommand.Cmd.Replace(TelnetReservedString.c_CONFIGFULLFILENAME, tConfigFileName);
                        }
                    }                    
                }
            }
        }

        enum E_ProcessStep
        {
            Select, Register
        }


        /// <summary>
        /// 2013-01-11 - shinyn - R14-01 - 목록저장 버튼 추가
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butSaveList_Click(object sender, EventArgs e)
        {
            
        }
    }
}
