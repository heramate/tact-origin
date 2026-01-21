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
    public partial class ModifyShortenCommandGroup : BaseForm
    {        /// <summary>
        /// 작업 타입 입니다.
        /// </summary>
        private E_WorkType m_WorkType;
        /// <summary>
        /// 단축 명령 입니다.
        /// </summary>
        private ShortenCommandGroupInfo m_CommandGroupInfo;

        /// <summary>
        /// 2020-10-05 TACT기능개선 단축명령기능 오류 수정
        /// </summary>
        public event ReturnHandlerArgument1<bool> OnDulplicate;

        public E_WorkType WorkType
        {
            get { return m_WorkType; }
            //set { m_WorkType = value; }
        }

        public ShortenCommandGroupInfo CommandGroupInfo
        {
            get { return m_CommandGroupInfo; }
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
        public ModifyShortenCommandGroup():this(E_WorkType.Add,new ShortenCommandGroupInfo())
        {
        }

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public ModifyShortenCommandGroup(E_WorkType aWorkType, ShortenCommandGroupInfo aCommandGroupInfo)
        {
            InitializeComponent();
            m_WorkType = aWorkType;
            m_CommandGroupInfo = new ShortenCommandGroupInfo(aCommandGroupInfo);
        }

        /// <summary>
        /// 컨트롤을 초기화 합니다.
        /// </summary>
        public void InitializeControl()
        {
            AddButton(E_ButtonType.Close, E_ButtonSide.Right, "닫기");
            AddButton(E_ButtonType.OK, E_ButtonSide.Right, "확인");

            DisplayBaseData();
        }

        /// <summary>
        /// 명령 정보를 표시 합니다.
        /// </summary>
        private void DisplayBaseData()
        {
            txtName.Text = m_CommandGroupInfo.Name;
            txtDescription.Text = m_CommandGroupInfo.Description;
        }


        protected override void ButtonProcess(E_ButtonType aButtonType)
        {
            switch (aButtonType)
            {
                case E_ButtonType.Close:
                    this.Close();
                    break;
                case E_ButtonType.OK:
                    SaveShortenCommandGroup();
                    break;
            }
        }
        /// <summary>
        /// 명령어 저장 처리합니다.
        /// </summary>
        private void SaveShortenCommandGroup()
        {

            if (txtName.Text.Trim().Length == 0)
            {
                AppGlobal.ShowMessageBox(this, "단축 명령 그룹 이름을 입력 하세요.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
			//2020-10-05 TACT기능개선 단축명령기능 오류 수정
            if (OnDulplicate(this))
            {
                AppGlobal.ShowMessageBox(this, "단축 명령 그룹 이름이 중복 됩니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
         
            m_CommandGroupInfo.Name = txtName.Text.Trim();
            m_CommandGroupInfo.Description = txtDescription.Text;

            RequestCommunicationData tRequestData = null;
            ShortenCommandGroupRequestInfo tShortenCommandRequestInfo = new ShortenCommandGroupRequestInfo();
            tShortenCommandRequestInfo.WorkType = m_WorkType;
            tShortenCommandRequestInfo.ShortenCommandGroupInfo = m_CommandGroupInfo;
            tShortenCommandRequestInfo.UserID = AppGlobal.s_LoginResult.UserID;

            tRequestData = AppGlobal.MakeDefaultRequestData();
            tRequestData.CommType = E_CommunicationType.RequestShortenCommandGroup;

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

            EventProcessor.Run((ShortenCommandGroupInfo)m_Result.ResultData, m_WorkType);

            switch (m_WorkType)
            {

                case E_WorkType.Add:
                    AppGlobal.ShowMessageBox(this, "단축 명령 그룹을 추가 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case E_WorkType.Modify:
                    AppGlobal.ShowMessageBox(this, "단축 명령 을을 수정 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }

            this.Close();
        }


    }
}