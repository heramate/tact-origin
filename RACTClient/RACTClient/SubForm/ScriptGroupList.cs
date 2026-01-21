using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RACTCommonClass;
using C1.Win.C1FlexGrid;

namespace RACTClient
{
    public partial class ScriptGroupList : BaseForm
    {
        public ScriptGroupList()
        {
            InitializeComponent();
        }


        public void InitializeControl()
        {
            AddButton(E_ButtonType.Delete, E_ButtonSide.Right, "삭제");
            AddButton(E_ButtonType.Modify, E_ButtonSide.Right, "수정");
            AddButton(E_ButtonType.Add, E_ButtonSide.Right, "추가");

            AppGlobal.InitializeGridStyle(fgCommandGroup);
            AppGlobal.s_DataSyncProcssor.OnScriptGroupInfoChangeEvent += new HandlerArgument2<ScriptGroupInfo, E_WorkType>(s_DataSyncProcssor_OnScriptGroupInfoChangeEvent);
            DisplayShortenCommand();
        }

        void s_DataSyncProcssor_OnScriptGroupInfoChangeEvent(ScriptGroupInfo aValue1, E_WorkType aValue2)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument2<ScriptGroupInfo, E_WorkType>(s_DataSyncProcssor_OnScriptGroupInfoChangeEvent), new object[] { aValue1, aValue2 });
                return;
            }

            if (aValue2 == E_WorkType.Add)
            {
                DisplayGrid(fgCommandGroup.Rows.Add(), aValue1);
            }
            else if (aValue2 == E_WorkType.Modify)
            {
                for (int i = 1; i < fgCommandGroup.Rows.Count; i++)
                {
                    if (((ScriptGroupInfo)fgCommandGroup.Rows[i].UserData).ID == aValue1.ID)
                    {
                        DisplayGrid(fgCommandGroup.Rows[i], aValue1);
                        break;
                    }
                }
            }
            else
            {
                for (int i = 1; i < fgCommandGroup.Rows.Count; i++)
                {
                    if (((ScriptGroupInfo)fgCommandGroup.Rows[i].UserData).ID == aValue1.ID)
                    {
                        fgCommandGroup.Rows.Remove(i);
                        break;
                    }
                }
            }
        }

       

        protected override void ButtonProcess(E_ButtonType aButtonType)
        {
            switch (aButtonType)
            {
                case E_ButtonType.Close:
                    this.Close();
                    break;
                case E_ButtonType.Delete:
                    DeleteCommand();
                    break;
                case E_ButtonType.Modify:
                    ModifyCommand();
                    break;
                case E_ButtonType.Add:
                    AddCommand();
                    break;
            }
        }
        /// <summary>
        /// 명령 추가 창을 표시 합니다.
        /// </summary>
        private void AddCommand()
        {
            ModifyScriptGroup tAddForm = new ModifyScriptGroup();
            tAddForm.OnDulplicate += new ReturnHandlerArgument1<bool>(ModifyScript_OnDulplicatet);
            tAddForm.InitializeControl();
            tAddForm.ShowDialog(this);
        }
        /// <summary>
        /// 명령 수정창을 표시 합니다.
        /// </summary>
        private void ModifyCommand()
        {
            if (fgCommandGroup.RowSel < 1) return;
            ScriptGroupInfo tCommand = fgCommandGroup.Rows[fgCommandGroup.RowSel].UserData as ScriptGroupInfo;
            ModifyScriptGroup tAddForm = new ModifyScriptGroup(E_WorkType.Modify, tCommand);
            tAddForm.OnDulplicate += new ReturnHandlerArgument1<bool>(ModifyScript_OnDulplicatet);
            tAddForm.InitializeControl();
            tAddForm.ShowDialog(this);
        }
		//2020-10-05 TACT기능개선 스크립트명령기능 오류 수정
        bool ModifyScript_OnDulplicatet(object aValue1)
        {
            bool bValue = false;
            ScriptGroupInfo tScriptGroupInfo;
            ModifyScriptGroup tModifyScriptGroup = aValue1 as ModifyScriptGroup;

            for (int i = 1; i < fgCommandGroup.Rows.Count; i++)
            {

                tScriptGroupInfo = fgCommandGroup.Rows[i].UserData as ScriptGroupInfo;
                if (tModifyScriptGroup.WorkType == E_WorkType.Modify)
                {
                    if (tScriptGroupInfo.ID == tModifyScriptGroup.ScriptGroupInfo.ID)
                        continue;
                }
                if (tScriptGroupInfo.Name.Equals(tModifyScriptGroup.DPLabelName))
                {
                    bValue = true;
                    break;
                }
            }
            return bValue;
        }


        /// <summary>
        /// 명령을 삭제 합니다.
        /// </summary>
        private void DeleteCommand()
        {
            if (fgCommandGroup.RowSel < 1) return;
            if (AppGlobal.ShowMessageBox(this, "해당 그룹을 삭제 하시겠습니까?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                ScriptGroupInfo tCommand = fgCommandGroup.Rows[fgCommandGroup.RowSel].UserData as ScriptGroupInfo;

                if (tCommand.ScriptList.Count > 0)
                {
                    AppGlobal.ShowMessageBox(this, "그룹 하위에 스크립트를 삭제 하세요.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                RequestCommunicationData tRequestData = null;
                ScriptGroupRequestInfo tScriptGroupRequestInfo = new ScriptGroupRequestInfo();
                tScriptGroupRequestInfo.WorkType = E_WorkType.Delete;
                tScriptGroupRequestInfo.ScriptGroupInfo = tCommand;
                tScriptGroupRequestInfo.UserID = AppGlobal.s_LoginResult.UserID;

                tRequestData = AppGlobal.MakeDefaultRequestData();
                tRequestData.CommType = E_CommunicationType.RequestScriptGroup;

                tRequestData.RequestData = tScriptGroupRequestInfo;

                m_Result = null;
                m_MRE.Reset();

                AppGlobal.SendRequestData(this, tRequestData);
                m_MRE.WaitOne(AppGlobal.s_RequestTimeOut);

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
                EventProcessor.Run((ScriptGroupInfo)m_Result.ResultData, E_WorkType.Delete);
            }
        }

        /// <summary>
        /// 단축 명령어를 표시 합니다.
        /// </summary>
        private void DisplayShortenCommand()
        {
            fgCommandGroup.Rows.Count = 1;
            foreach (ScriptGroupInfo tCommand in AppGlobal.s_ScriptList)
            {
                DisplayGrid(fgCommandGroup.Rows.Add(), tCommand);
            }
        }

        /// <summary>
        /// 그리드에 정보를 표시 합니다.
        /// </summary>
        /// <param name="aRow"></param>
        /// <param name="aCommand"></param>
        private void DisplayGrid(Row aRow, ScriptGroupInfo aCommand)
        {
            aRow[0] = aRow.Index;
            aRow[1] = aCommand.Name;
            aRow[2] = aCommand.ScriptList.Count;
            aRow[3] = aCommand.Description;
            aRow.UserData = aCommand;
        }

        private void fgShortenCommand_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                mnuDelete.Enabled = true;
                mnuModify.Enabled = true;

                if (fgCommandGroup.RowSel < 1)
                {
                    mnuDelete.Enabled = false;
                    mnuModify.Enabled = false;
                }
                ctmPopup.Popup(MousePosition);
            }
        }

        private void mnuAdd_Click(object sender, EventArgs e)
        {
            AddCommand();
        }

        private void mnuModify_Click(object sender, EventArgs e)
        {
            ModifyCommand();
        }

        private void mnuDelete_Click(object sender, EventArgs e)
        {
            DeleteCommand();
        }

       
    }
}