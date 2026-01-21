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
    /// 2013-01-11 - shinyn - CFG 복원명령리스트 폼 추가
    /// </summary>
    public partial class OpenRestoreCommand : BaseForm
    {

        private CfgSaveInfoCollection m_CfgSaveInfos = new CfgSaveInfoCollection();

        public CfgSaveInfoCollection CfgSaveInfos
        {
            set { m_CfgSaveInfos = value; }
            get { return m_CfgSaveInfos; }
        }

        public OpenRestoreCommand()
        {
            InitializeComponent();
            InitializeControl();
        }


        public void OpenOnlineRestoreList(CfgSaveInfoCollection aCfgSaveInfos)
        {
            try
            {
                if (aCfgSaveInfos == null) return;

                DisplayOnlineList(aCfgSaveInfos);
            }
            catch (Exception ex)
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, ex.Message.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        
        /// <summary>
        /// 온라인 경우 CfgRestoreScript와 FullFileName이 없다.
        /// </summary>
        /// <param name="aCfgSaveInfos"></param>
        private void DisplayOnlineList(CfgSaveInfoCollection aCfgSaveInfos)
        {
            int tRowIndex = 1;
            try
            {
                fgCommandList.Redraw = false;
                fgCommandList.Rows.Count = 1;
                fgCommandList.Rows.Count = aCfgSaveInfos.Count + 1;

                foreach (CfgSaveInfo tCfgSaveInfo in aCfgSaveInfos)
                {
                    AddOnlineRow(tRowIndex, tCfgSaveInfo);
                    
                    tRowIndex++;
                }

                fgCommandList.RowSel = 1;
            }
            catch (Exception ex)
            {
                //AppGlobal.m_FileLog.PrintLogEnter("InitializeDevice : " + ex.ToString());
                Console.WriteLine(">>>>>>>>>>>>>>>> :" + ex.ToString());
            }
            finally
            {
                fgCommandList.Redraw = true;
                //  m_IsDeviceInitialized = true;
            }
        }

        private void AddOnlineRow(int aRowIndex, CfgSaveInfo aCfgSaveInfo)
        {
            fgCommandList[aRowIndex, "StTime"] = aCfgSaveInfo.StTime.ToShortDateString() + " " +
                                                 aCfgSaveInfo.StTime.ToShortTimeString();

            string tFullFileName = aCfgSaveInfo.FileName;
            if (aCfgSaveInfo.FileExtend != "")
            {
                tFullFileName = tFullFileName + "." + aCfgSaveInfo.FileExtend;
            }

            fgCommandList[aRowIndex, "FullFileName"] = tFullFileName;
            fgCommandList.Rows[aRowIndex].UserData = aCfgSaveInfo;
        }

        public void OpenConsoleRestoreList(CfgSaveInfoCollection aCfgSaveInfos)
        {
            try
            {
                if (aCfgSaveInfos == null) return;

                DisplayConsoleList(aCfgSaveInfos);
            }
            catch (Exception ex)
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, ex.Message.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 콘솔인 경우 CfgRestoreScript와 FullFileName이 있다.
        /// </summary>
        /// <param name="aCfgSaveInfos"></param>
        private void DisplayConsoleList(CfgSaveInfoCollection aCfgSaveInfos)
        {
            int tRowIndex = 1;
            try
            {
                fgCommandList.Redraw = false;
                fgCommandList.Rows.Count = 1;

                if (aCfgSaveInfos[0].StTime == null) return;

                fgCommandList.Rows.Count = aCfgSaveInfos.Count + 1;

                foreach (CfgSaveInfo tCfgSaveInfo in aCfgSaveInfos)
                {
                    AddConsoleRow(tRowIndex, tCfgSaveInfo);

                    tRowIndex++;
                }

                fgCommandList.RowSel = 1;
            }
            catch (Exception ex)
            {
                //AppGlobal.m_FileLog.PrintLogEnter("InitializeDevice : " + ex.ToString());
                Console.WriteLine(">>>>>>>>>>>>>>>> :" + ex.ToString());
            }
            finally
            {
                fgCommandList.Redraw = true;
                //  m_IsDeviceInitialized = true;
            }
        }

        private void AddConsoleRow(int aRowIndex, CfgSaveInfo aCfgSaveInfo)
        {
            fgCommandList[aRowIndex, "StTime"] = aCfgSaveInfo.StTime.ToShortDateString() + " " +
                                                 aCfgSaveInfo.StTime.ToShortTimeString();

            fgCommandList[aRowIndex, "FullFileName"] = aCfgSaveInfo.FullFileName;
            fgCommandList.Rows[aRowIndex].UserData = aCfgSaveInfo;
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

                for (i = 1; i < fgCommandList.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(fgCommandList.Rows[i]["CheckCommand"]))
                    {
                        CfgSaveInfo tCfgSsaveInfo = (CfgSaveInfo)fgCommandList.Rows[i].UserData;
                        m_CfgSaveInfos.Add(tCfgSsaveInfo);
                        tCount++;
                    }


                }

                if (tCount == 0)
                {
                    AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "명령 목록을 선택해주세요.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void fgCommandList_CellButtonClick(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            C1.Win.C1FlexGrid.Row tRow = fgCommandList.Rows[e.Row];

            int tSelectRow = 0;

            if (Convert.ToBoolean(tRow["CheckCommand"]) == true)
            {
                tSelectRow = tRow.Index;
            }

            for (int i = 1; i < fgCommandList.Rows.Count; i++)
            {
                if (tSelectRow != i) fgCommandList.Rows[i]["CheckCommand"] = false;
            }
        }
    }
}