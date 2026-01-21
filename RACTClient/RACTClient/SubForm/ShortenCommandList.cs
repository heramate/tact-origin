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
    public partial class ShortenCommandList : BaseForm
    {
        public ShortenCommandList()
        {
            InitializeComponent();
        }

        public void InitializeControl()
        {

            AppGlobal.InitializeButtonStyle(btnGroup);
            AppGlobal.InitializeGridStyle(fgShortenCommand);

            AddButton(E_ButtonType.Delete, E_ButtonSide.Right, "삭제");
            AddButton(E_ButtonType.Modify, E_ButtonSide.Right, "수정");
            AddButton(E_ButtonType.Add, E_ButtonSide.Right, "추가");
            

            AppGlobal.s_DataSyncProcssor.OnShortenCommandInfoChangeEvent += new HandlerArgument2<ShortenCommandInfo, E_WorkType>(OnChangeCommand);
            AppGlobal.s_DataSyncProcssor.OnShortenCommandGroupInfoChangeEvent += new HandlerArgument2<ShortenCommandGroupInfo, E_WorkType>(OnChangeCommandGroup);

            

            DisplayCommandGroup();
        }

        void OnChangeCommandGroup(ShortenCommandGroupInfo aValue1, E_WorkType aValue2)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument2<ShortenCommandGroupInfo, E_WorkType>(OnChangeCommandGroup), new object[] { aValue1, aValue2 });
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
                ShortenCommandGroupInfo tTag;
                for (int i = 0; i < cboCommandGroup.Items.Count; i++)
                {
                    tTag = (ShortenCommandGroupInfo)cboCommandGroup.Items[i].Tag;
                    if (tTag.ID == aValue1.ID)
                    {
                        cboCommandGroup.Items[i].Text = aValue1.Name;
                        cboCommandGroup.Items[i].Tag = aValue1;
                    }
                }
            }
            else
            {
                ShortenCommandGroupInfo tTag;
                for (int i = 0; i < cboCommandGroup.Items.Count; i++)
                {
                    tTag = (ShortenCommandGroupInfo)cboCommandGroup.Items[i].Tag;
                    if (tTag.ID == aValue1.ID)
                    {
                        cboCommandGroup.Items.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        void OnChangeCommand(ShortenCommandInfo aValue1, E_WorkType aValue2)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument2<ShortenCommandInfo, E_WorkType>(OnChangeCommand),new object[]{aValue1,aValue2});
                return;
            }

            if (aValue2 == E_WorkType.Add)
            {
                if (((ShortenCommandGroupInfo)cboCommandGroup.Items[cboCommandGroup.SelectedIndex].Tag).ID == aValue1.GroupID)
                {
                    DisplayGrid(fgShortenCommand.Rows.Add(), aValue1);
                }
            }
            else if (aValue2 == E_WorkType.Modify)
            {
                for (int i = 1; i < fgShortenCommand.Rows.Count; i++)
                {
                    if (((ShortenCommandInfo)fgShortenCommand.Rows[i].UserData).ID == aValue1.ID)
                    {
                        if (((ShortenCommandGroupInfo)cboCommandGroup.Items[cboCommandGroup.SelectedIndex].Tag).ID != aValue1.GroupID)
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
                    if (((ShortenCommandInfo)fgShortenCommand.Rows[i].UserData).ID == aValue1.ID)
                    {
                        fgShortenCommand.Rows.Remove(i);
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
            if(cboCommandGroup.SelectedIndex < 0)
            {
                AppGlobal.ShowMessageBox(this,"단축명령 그룹을 등록하세요",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }

            ModifyShortenCommand tAddForm = new ModifyShortenCommand((ShortenCommandGroupInfo)cboCommandGroup.Items[cboCommandGroup.SelectedIndex].Tag);
            tAddForm.OnDulplicate += new ReturnHandlerArgument1<bool>(ModifyShortenCommand_OnDulplicate);
            tAddForm.InitializeControl();
            tAddForm.ShowDialog(this);
        }
        /// <summary>
        /// 명령 수정창을 표시 합니다.
        /// </summary>
        private void ModifyCommand()
        {
            if (fgShortenCommand.RowSel < 1) return;
            ShortenCommandInfo tCommand = fgShortenCommand.Rows[fgShortenCommand.RowSel].UserData as ShortenCommandInfo;
            ModifyShortenCommand tAddForm = new ModifyShortenCommand(E_WorkType.Modify,tCommand);
            tAddForm.OnDulplicate += new ReturnHandlerArgument1<bool>(ModifyShortenCommand_OnDulplicate);

            tAddForm.InitializeControl();
            tAddForm.ShowDialog(this);
        }
		//2020-10-05 TACT기능개선 단축명령기능 오류 수정
        bool ModifyShortenCommand_OnDulplicate(object aValue1)
        {
            bool bValue = false;
            ShortenCommandInfo tCommand;
            ModifyShortenCommand tModifyShortenCommand = aValue1 as ModifyShortenCommand;

            for(int i=1; i< fgShortenCommand.Rows.Count; i++)
            {
                tCommand = fgShortenCommand.Rows[i].UserData as ShortenCommandInfo;

                if (tModifyShortenCommand.WorkType == E_WorkType.Modify)
                {
                    if (tCommand.ID == tModifyShortenCommand.CommandInfo.ID)
                        continue;
                }

                if (tCommand.Name.Equals(tModifyShortenCommand.DPLabelName))
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
                ShortenCommandInfo tCommand = fgShortenCommand.Rows[fgShortenCommand.RowSel].UserData as ShortenCommandInfo;

                RequestCommunicationData tRequestData = null;
                ShortenCommandRequestInfo tShortenCommandRequestInfo = new ShortenCommandRequestInfo();
                tShortenCommandRequestInfo.WorkType = E_WorkType.Delete;
                tShortenCommandRequestInfo.ShortenCommandInfo = tCommand;
                tShortenCommandRequestInfo.UserID = AppGlobal.s_LoginResult.UserID;

                tRequestData = AppGlobal.MakeDefaultRequestData();
                tRequestData.CommType = E_CommunicationType.RequestShortenCommand;

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
                EventProcessor.Run((ShortenCommandInfo)m_Result.ResultData, E_WorkType.Delete);
            }
        }

        /// <summary>
        /// 단축 명령어를 표시 합니다.
        /// </summary>
        private void DisplayShortenCommand(ShortenCommandInfoCollection aList)
        {
            fgShortenCommand.Rows.Count = 1;
            foreach (ShortenCommandInfo tCommand in aList)
            {
                DisplayGrid(fgShortenCommand.Rows.Add(), tCommand);
            }
        }
        /// <summary>
        /// 그룹을 표시 합니다.
        /// </summary>
        private void DisplayCommandGroup()
        {
            foreach (ShortenCommandGroupInfo tCommand in AppGlobal.s_ShortenCommandList)
            {
                OnChangeCommandGroup(tCommand, E_WorkType.Add);
            }
        }

        /// <summary>
        /// 그리드에 정보를 표시 합니다.
        /// </summary>
        /// <param name="aRow"></param>
        /// <param name="aCommand"></param>
        private void DisplayGrid(Row aRow, ShortenCommandInfo aCommand)
        {
            aRow[ 0] = aRow.Index;
            aRow[ 1] = aCommand.Name;
            aRow[ 2] = aCommand.Command;
            aRow[ 3] = aCommand.Description;
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
            ShortenCommandGroupList tForm = new ShortenCommandGroupList();
            tForm.InitializeControl();
            tForm.ShowDialog(this);
        }

        private void cboCommandGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            fgShortenCommand.Rows.Count =1;
            if(cboCommandGroup.SelectedIndex < 0) return;


            ShortenCommandGroupInfo tGroupInfo = (ShortenCommandGroupInfo)cboCommandGroup.Items[cboCommandGroup.SelectedIndex].Tag;
            DisplayShortenCommand(AppGlobal.s_ShortenCommandList[tGroupInfo.ID].ShortenCommandList);
        }
    }
}