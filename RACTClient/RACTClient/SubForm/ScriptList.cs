using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RACTCommonClass;
using C1.Win.C1FlexGrid;
using MKLibrary.Controls;

namespace RACTClient
{
    public partial class ScriptList : BaseForm
    {
        /// <summary>
        ///  화면 모드 입니다.
        /// </summary>
        private E_ScriptFormMode m_Mode = E_ScriptFormMode.Edit;
        public ScriptList():this(E_ScriptFormMode.Edit)
        {
        }

        public ScriptList(E_ScriptFormMode aMode)
        {
            InitializeComponent();

            m_Mode = aMode;
        }

        public void InitializeControl()
        {

            AppGlobal.InitializeButtonStyle(btnGroup);
            AppGlobal.InitializeGridStyle(fgShortenCommand);


            if (m_Mode == E_ScriptFormMode.Edit)
            {
                AddButton(E_ButtonType.Delete, E_ButtonSide.Right, "삭제");
                AddButton(E_ButtonType.Modify, E_ButtonSide.Right, "수정");
                AddButton(E_ButtonType.Add, E_ButtonSide.Right, "추가");
            }
            else
            {
                AddButton(E_ButtonType.Close, E_ButtonSide.Right, "닫기");
                AddButton(E_ButtonType.Select, E_ButtonSide.Right, "선택");
            }

            AppGlobal.s_DataSyncProcssor.OnScriptGroupInfoChangeEvent += new HandlerArgument2<ScriptGroupInfo, E_WorkType>(s_DataSyncProcssor_OnScriptGroupInfoChangeEvent);
            AppGlobal.s_DataSyncProcssor.OnScriptChangeEvent += new HandlerArgument2<Script, E_WorkType>(s_DataSyncProcssor_OnScriptChangeEvent);

            DisplayCommandGroup();
        }

        void s_DataSyncProcssor_OnScriptChangeEvent(Script aValue1, E_WorkType aValue2)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument2<Script, E_WorkType>(s_DataSyncProcssor_OnScriptChangeEvent), new object[] { aValue1, aValue2 });
                return;
            }

            if (aValue2 == E_WorkType.Add)
            {
                if (((ScriptGroupInfo)cboCommandGroup.Items[cboCommandGroup.SelectedIndex].Tag).ID == aValue1.GroupID)
                {
                    DisplayGrid(fgShortenCommand.Rows.Add(), aValue1);
                }
            }
            else if (aValue2 == E_WorkType.Modify)
            {
                for (int i = 1; i < fgShortenCommand.Rows.Count; i++)
                {
                    if (((Script)fgShortenCommand.Rows[i].UserData).ID == aValue1.ID)
                    {
                        if (((ScriptGroupInfo)cboCommandGroup.Items[cboCommandGroup.SelectedIndex].Tag).ID != aValue1.GroupID)
                        {
                            fgShortenCommand.Rows.Remove(i);
                        }
                        else
                        {
                            DisplayGrid(fgShortenCommand.Rows[i], aValue1);
                        }
                        break;
                    }
                }
            }
            else
            {
                for (int i = 1; i < fgShortenCommand.Rows.Count; i++)
                {
                    if (((Script)fgShortenCommand.Rows[i].UserData).ID == aValue1.ID)
                    {
                        fgShortenCommand.Rows.Remove(i);
                        break;
                    }
                }
            }
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
                MKListItem tItem = new MKListItem(aValue1.Name);
                tItem.Tag = aValue1;

                cboCommandGroup.Items.Add(tItem);
                if (cboCommandGroup.Items.Count == 1)
                {
                    cboCommandGroup.SelectedIndex = 0;
                }
            }
            else if (aValue2 == E_WorkType.Modify)
            {
                ScriptGroupInfo tTag;
                for (int i = 0; i < cboCommandGroup.Items.Count; i++)
                {
                    tTag = (ScriptGroupInfo)cboCommandGroup.Items[i].Tag;
                    if (tTag.ID == aValue1.ID)
                    {
                        cboCommandGroup.Items[i].Text = aValue1.Name;
                        cboCommandGroup.Items[i].Tag = aValue1;
                    }
                }
            }
            else
            {
                ScriptGroupInfo tTag;
                for (int i = 0; i < cboCommandGroup.Items.Count; i++)
                {
                    tTag = (ScriptGroupInfo)cboCommandGroup.Items[i].Tag;
                    if (tTag.ID == aValue1.ID)
                    {
                        cboCommandGroup.Items.RemoveAt(i);
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
                case E_ButtonType.Select:
                    SelectScript();
                    DialogResult = DialogResult.OK;
                    this.Close();
                    break;
            }
        }
        /// <summary>
        /// 선택된 스크립트를 가져오기 합니다.
        /// </summary>
        public Script SelectedScript
        {
            get { return m_SelectedScript; }
        }
        /// <summary>
        /// 선택된 스크립트 입니다.
        /// </summary>
        private Script m_SelectedScript;
        /// <summary>
        /// 선택 처리 합니다.
        /// </summary>
        private void SelectScript()
        {
            if (fgShortenCommand.RowSel < 1) return;
            m_SelectedScript = fgShortenCommand.Rows[fgShortenCommand.RowSel].UserData as Script;
           
        }
        /// <summary>
        /// 명령 추가 창을 표시 합니다.
        /// </summary>
        private void AddCommand()
        {
            if(cboCommandGroup.SelectedIndex < 0)
            {
                AppGlobal.ShowMessageBox(this,"스크립트 그룹을 등록하세요",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }

            ModifyScript tAddForm = new ModifyScript((ScriptGroupInfo)cboCommandGroup.Items[cboCommandGroup.SelectedIndex].Tag);
            tAddForm.OnDulplicate += new ReturnHandlerArgument1<bool>(ModifyScript_OnDulplicatet);
            tAddForm.InitializeControl();
            tAddForm.ShowDialog(this);
        }
        /// <summary>
        /// 명령 수정창을 표시 합니다.
        /// </summary>
        private void ModifyCommand()
        {
            if (fgShortenCommand.RowSel < 1) return;
            Script tCommand = fgShortenCommand.Rows[fgShortenCommand.RowSel].UserData as Script;
            ModifyScript tAddForm = new ModifyScript(E_WorkType.Modify, tCommand);
            tAddForm.OnDulplicate += new ReturnHandlerArgument1<bool>(ModifyScript_OnDulplicatet);
            tAddForm.InitializeControl();
            tAddForm.ShowDialog(this);
        }
		//2020-10-05 TACT기능개선 스크립트명령기능 오류 수정
        bool ModifyScript_OnDulplicatet(object aValue1)
        {
            bool bValue = false;
            Script tCommand;
            ModifyScript tModifyScript = aValue1 as ModifyScript;

            for (int i = 1; i < fgShortenCommand.Rows.Count; i++)
            {

                tCommand = fgShortenCommand.Rows[i].UserData as Script;
                if (tModifyScript.WorkType == E_WorkType.Modify)
                {
                    if (tCommand.ID == tModifyScript.CommandInfo.ID)
                        continue;
                }
                if (tCommand.Name.Equals(tModifyScript.DPLabelName))
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
            if (fgShortenCommand.RowSel < 1) return;
            if (AppGlobal.ShowMessageBox(this, "해당 명령을 삭제 하시겠습니까?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Script tCommand = fgShortenCommand.Rows[fgShortenCommand.RowSel].UserData as Script;

                RequestCommunicationData tRequestData = null;
                ScriptRequestInfo tShortenCommandRequestInfo = new ScriptRequestInfo();
                tShortenCommandRequestInfo.WorkType = E_WorkType.Delete;
                tShortenCommandRequestInfo.ScriptInfo = tCommand;
                tShortenCommandRequestInfo.UserID = AppGlobal.s_LoginResult.UserID;

                tRequestData = AppGlobal.MakeDefaultRequestData();
                tRequestData.CommType = E_CommunicationType.RequestScriptInfo;

                tRequestData.RequestData = tShortenCommandRequestInfo;

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
                EventProcessor.Run((Script)m_Result.ResultData, E_WorkType.Delete);
            }
        }

        /// <summary>
        /// 단축 명령어를 표시 합니다.
        /// </summary>
        private void DisplayShortenCommand(ScriptCollection aList)
        {
            fgShortenCommand.Rows.Count = 1;
            foreach (Script tCommand in aList)
            {
                DisplayGrid(fgShortenCommand.Rows.Add(), tCommand);
            }
        }
        /// <summary>
        /// 그룹을 표시 합니다.
        /// </summary>
        private void DisplayCommandGroup()
        {
            foreach (ScriptGroupInfo tCommand in AppGlobal.s_ScriptList)
            {
                s_DataSyncProcssor_OnScriptGroupInfoChangeEvent(tCommand, E_WorkType.Add);
            }
        }

        /// <summary>
        /// 그리드에 정보를 표시 합니다.
        /// </summary>
        /// <param name="aRow"></param>
        /// <param name="aCommand"></param>
        private void DisplayGrid(Row aRow, Script aCommand)
        {
            aRow[ 0] = aRow.Index;
            aRow[ 1] = aCommand.Name;
            aRow[ 2] = aCommand.Description;
            aRow.UserData = aCommand;
        }

        private void fgShortenCommand_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                mnuDelete.Enabled = true;
                mnuModify.Enabled = true;

                if (fgShortenCommand.RowSel < 1)
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

        private void btnGroup_Click(object sender, EventArgs e)
        {
            ScriptGroupList tForm = new ScriptGroupList();
            tForm.InitializeControl();
            tForm.ShowDialog(this);
        }

        private void cboCommandGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            fgShortenCommand.Rows.Count =1;
            if(cboCommandGroup.SelectedIndex < 0) return;


            ScriptGroupInfo tGroupInfo = (ScriptGroupInfo)cboCommandGroup.Items[cboCommandGroup.SelectedIndex].Tag;
            DisplayShortenCommand(AppGlobal.s_ScriptList[tGroupInfo.ID].ScriptList);
        }
    }

    public enum E_ScriptFormMode
    {
        Edit,
        Select
    }
}