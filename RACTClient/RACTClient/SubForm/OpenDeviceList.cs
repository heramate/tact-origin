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

    /// <summary>
    /// 2013-01-11 - shinyn - 오픈한 장비리스트 폼 추가
    /// </summary>
    public partial class OpenDeviceList : BaseForm
    {

        private string m_FilePath = string.Empty;

        public string FilePath
        {
            set { m_FilePath = value; }
            get { return m_FilePath; }
        }

        private int m_RemainTerminal = 0;

        public int RemailTerminal
        {
            get { return m_RemainTerminal; }
            set { m_RemainTerminal = value; }
        }

        private DeviceInfoCollection m_DeviceInfos = new DeviceInfoCollection();

        public DeviceInfoCollection DeviceInfos
        {
            get { return m_DeviceInfos; }
        }

        public OpenDeviceList()
        {
            InitializeComponent();
            InitializeControl();
        }

        public void OpenFileDeviceList(DeviceInfoCollection aDeviceInfos)
        {
            try
            {
                if (aDeviceInfos == null) return;

                DisplayList(aDeviceInfos);

            }
            catch (Exception ex)
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, ex.Message.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DisplayList(DeviceInfoCollection aDeviceInfos)
        {
            int tRowIndex = 1;
            try
            {
                fgDeviceList.Redraw = false;
                fgDeviceList.Rows.Count = 1;
                fgDeviceList.Rows.Count = aDeviceInfos.Count + 1;

                foreach (DeviceInfo tDeviceInfo in aDeviceInfos)
                {
                    AddRow(tRowIndex, tDeviceInfo);
                    if (tDeviceInfo.InputFlag == E_FlagType.User)
                    {
                        fgDeviceList.Rows[tRowIndex].StyleNew.ForeColor = Color.FromArgb(255, 113, 50);
                    }
                    tRowIndex++;
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

        private void AddRow(int aRowIndex, DeviceInfo aDeviceInfo)
        {
            fgDeviceList[aRowIndex, "ORG1"] = aDeviceInfo.ORG1Name;
            fgDeviceList[aRowIndex, "Team"] = aDeviceInfo.ORG2Name;
            fgDeviceList[aRowIndex, "CenterName"] = aDeviceInfo.CenterName;
            fgDeviceList[aRowIndex, "TPOName"] = aDeviceInfo.TpoName;
            fgDeviceList[aRowIndex, "ModelName"] = aDeviceInfo.ModelName;
            fgDeviceList[aRowIndex, "DeviceGroup"] = aDeviceInfo.DeviceGroupName;
            fgDeviceList[aRowIndex, "DeviceNumber"] = aDeviceInfo.DeviceNumber;
            fgDeviceList[aRowIndex, "DeviceName"] = aDeviceInfo.Name;			//장비이름
            fgDeviceList[aRowIndex, "IPAddress"] = aDeviceInfo.IPAddress;		//장비주소          
            fgDeviceList.Rows[aRowIndex].UserData = aDeviceInfo;
        }

        public void InitializeControl()
        {
            AddButton(E_ButtonType.Close, E_ButtonSide.Right, "닫기");
            AddButton(E_ButtonType.OK, E_ButtonSide.Right, "확인");
        }

        protected override void ButtonProcess(E_ButtonType aButtonType)
        {
            if (aButtonType == E_ButtonType.OK)
            {
                int i = 0;
                int tCount = 0;

                for (i = 1; i < fgDeviceList.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(fgDeviceList.Rows[i]["CheckDevice"]))
                    {
                        DeviceInfo tDeviceInfo = (DeviceInfo)fgDeviceList.Rows[i].UserData;

                        m_DeviceInfos.Add(tDeviceInfo);

                        tCount++;
                    }
                }

                if (tCount == 0)
                {
                    AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "장비목록을 선택해주세요.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (tCount > RemailTerminal)
                {
                    AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, string.Format("열수있는 터미널 갯수는 {0}입니다. 다시 선택해주세요.",RemailTerminal), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                     


                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();

            }
        }
    }
}