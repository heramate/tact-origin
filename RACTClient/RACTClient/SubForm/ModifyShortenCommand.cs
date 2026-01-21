using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RACTCommonClass;
using MKLibrary.Controls;

namespace RACTClient
{
    public partial class ModifyShortenCommand : BaseForm
    {
        /// <summary>
        /// 작업 타입 입니다.
        /// </summary>
        private E_WorkType m_WorkType;
        /// <summary>
        /// 단축 명령 입니다.
        /// </summary>
        private ShortenCommandInfo m_CommandInfo;
        /// <summary>
        /// 원본 데이터 입니다.
        /// </summary>
        private ShortenCommandInfo m_OldCommandInfo;
        /// <summary>
        /// 2020-10-05 TACT기능개선 단축명령기능 오류 수정
        /// </summary>
        public event ReturnHandlerArgument1<bool>OnDulplicate;


        public E_WorkType WorkType
        {
            get { return m_WorkType; }
            //set { m_WorkType = value; }
        }

        public ShortenCommandInfo CommandInfo
        {
            get { return m_CommandInfo; }
            //set { m_WorkType = value; }
        }

        public String DPLabelName
        {
            get { return txtName.Text; }
            //set { m_WorkType = value; }
        }
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public ModifyShortenCommand(ShortenCommandGroupInfo aGroupInfo)
            : this(E_WorkType.Add, new ShortenCommandInfo())
        {
            m_CommandInfo.GroupID = aGroupInfo.ID;
        }

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public ModifyShortenCommand(E_WorkType aWorkType, ShortenCommandInfo aCommandInfo)
        {
            InitializeComponent();
            m_WorkType = aWorkType;
            m_CommandInfo = new ShortenCommandInfo(aCommandInfo);
        }

        /// <summary>
        /// 컨트롤을 초기화 합니다.
        /// </summary>
        public void InitializeControl()
        {
            AddButton(E_ButtonType.Close, E_ButtonSide.Right, "닫기");
            AddButton(E_ButtonType.OK, E_ButtonSide.Right, "확인");

            AppGlobal.InitializeButtonStyle(btnReservedString);
            DisplayBaseData();
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

                cboGroup.Items.Add(tItem);

                if (aValue1.ID == m_CommandInfo.GroupID)
                {
                    cboGroup.SelectedIndex = cboGroup.Items.Count - 1;
                }
            }
            else if (aValue2 == E_WorkType.Modify)
            {
                ShortenCommandGroupInfo tTag;
                for (int i = 0; i < cboGroup.Items.Count; i++)
                {
                    tTag = (ShortenCommandGroupInfo)cboGroup.Items[i].Tag;
                    if (tTag.ID == aValue1.ID)
                    {
                        cboGroup.Items[i].Text = aValue1.Name;
                        cboGroup.Items[i].Tag = aValue1;
                    }
                }
            }
            else
            {
                ShortenCommandGroupInfo tTag;
                for (int i = 0; i < cboGroup.Items.Count; i++)
                {
                    tTag = (ShortenCommandGroupInfo)cboGroup.Items[i].Tag;
                    if (tTag.ID == aValue1.ID)
                    {
                        cboGroup.Items.RemoveAt(i);
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// 명령 정보를 표시 합니다.
        /// </summary>
        private void DisplayBaseData()
        {
            DisplayCommandGroup();

            txtName.Text = m_CommandInfo.Name;
            txtCommand.Text = m_CommandInfo.Command;
            txtDescription.Text = m_CommandInfo.Description;
        }

        private void btnReservedString_Click(object sender, EventArgs e)
        {
            ViewReservedString tReservedForm = new ViewReservedString();
            tReservedForm.InitializeControl();
            tReservedForm.ShowDialog(this);
        }

        protected override void ButtonProcess(E_ButtonType aButtonType)
        {
            switch (aButtonType)
            {
                case E_ButtonType.Close:
                    this.Close();
                    break;
                case E_ButtonType.OK:
                    SaveShortenCommand();
                    break;
            }
        }
        /// <summary>
        /// 명령어 저장 처리합니다.
        /// </summary>
        private void SaveShortenCommand()
        {

            if (txtName.Text.Trim().Length == 0)
            {
                AppGlobal.ShowMessageBox(this, "단축 명령 이름을 입력 하세요.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
			//2020-10-05 TACT기능개선 단축명령기능 오류 수정
            if (OnDulplicate(this))
            {
                AppGlobal.ShowMessageBox(this, "단축 명령 이름이 중복 됩니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (txtCommand.Text.Length == 0)
            {
                AppGlobal.ShowMessageBox(this, "단축 명령을 입력 하세요.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            m_OldCommandInfo = m_CommandInfo.DeepClone();

            m_CommandInfo.Name = txtName.Text.Trim();
            m_CommandInfo.GroupID = ((ShortenCommandGroupInfo)cboGroup.Items[cboGroup.SelectedIndex].Tag).ID;
            m_CommandInfo.Command = txtCommand.Text;
            m_CommandInfo.Description = txtDescription.Text;

            RequestCommunicationData tRequestData = null;
            ShortenCommandRequestInfo tShortenCommandRequestInfo = new ShortenCommandRequestInfo();
            tShortenCommandRequestInfo.WorkType = m_WorkType;
            tShortenCommandRequestInfo.ShortenCommandInfo = m_CommandInfo;
            tShortenCommandRequestInfo.UserID = AppGlobal.s_LoginResult.UserID;

            tRequestData = AppGlobal.MakeDefaultRequestData();
            tRequestData.CommType = E_CommunicationType.RequestShortenCommand;

            tRequestData.RequestData = tShortenCommandRequestInfo;

            m_Result = null;
            m_MRE.Reset();

            AppGlobal.SendRequestData(this, tRequestData);
            m_MRE.WaitOne(AppGlobal.s_RequestTimeOut);

            CheckedResult();
        }
        /// <summary>
        /// 요청 결과를 확인하는 함수입니다.
        /// </summary>
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

            if (m_WorkType == E_WorkType.Modify)
            {
                //2020-10-05 TACT기능개선 단축명령기능 오류 수정
				EventProcessor.Run((ShortenCommandInfo)m_Result.ResultData, E_WorkType.Modify);
                /*
                EventProcessor.Run(m_OldCommandInfo, E_WorkType.Delete);
                EventProcessor.Run((ShortenCommandInfo)m_Result.ResultData, E_WorkType.Add);
                */
            }
            else
            {
                EventProcessor.Run((ShortenCommandInfo)m_Result.ResultData, m_WorkType);
            }

            switch (m_WorkType)
            {

                case E_WorkType.Add:
                    AppGlobal.ShowMessageBox(this, "단축 명령을 추가 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case E_WorkType.Modify:
                    AppGlobal.ShowMessageBox(this, "단축 명령을 수정 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }

            this.Close();
        }
        
    }
}