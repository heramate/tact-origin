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
    public partial class ModifyGroupInfo : BaseForm
    {
        /// <summary>
        /// 그룹 정보 입니다.
        /// </summary>
        private GroupInfo m_GroupInfo = null;
        /// <summary>
        /// 장비정보 속성을 가져오거나 설정합니다.
        /// </summary>
        public GroupInfo GroupInfo
        {
            get { return m_GroupInfo; }
            set { m_GroupInfo = value; }
        }	

        /// <summary>
        /// 작업 타입 입니다.
        /// </summary>
        private E_WorkType m_WorkType = E_WorkType.Add;
        
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public ModifyGroupInfo()
        {
            InitializeComponent();
            m_GroupInfo = new GroupInfo();
            DisplayData();
        }

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public ModifyGroupInfo(E_WorkType aWorkType,GroupInfo aGroupInfo)
        {
            InitializeComponent();
            m_WorkType = aWorkType;
            if (m_WorkType == E_WorkType.Add)
            {
                m_GroupInfo = new GroupInfo();
            }
            else
            {
                m_GroupInfo = aGroupInfo;
            }
            DisplayData();
        }

        /// <summary>
        /// 컨트롤을 초기화 합니다.
        /// </summary>
        public void initializeControl()
        {
            // 2013-08-13- shinyn - 상위그룹 리스트 표시
            trvGroup.InitializeControl();
            AddButton(E_ButtonType.Close, E_ButtonSide.Right, "닫기");
            AddButton(E_ButtonType.OK, E_ButtonSide.Right, "저장");
        }

        /// <summary>
        /// 그룹 정보를 표시 합니다.
        /// </summary>
        private void DisplayData()
        {
            if (m_WorkType == E_WorkType.Modify)
            {
                trvGroup.InitializeControl(m_GroupInfo);
            }
            else
            {
                trvGroup.InitializeControl();
            }
            

            
            txtName.Text = m_GroupInfo.Name;
            txtDescription.Text = m_GroupInfo.Description;
        }

        /// <summary>
        /// 화면의 Button을 클릭한 이벤트를 처리하는 가상 함수입니다.
        /// </summary>
        /// <param name="aButtonType">버튼 타입</param>
        protected override void ButtonProcess(E_ButtonType aButtonType)
        {
            switch (aButtonType)
            {
                case E_ButtonType.OK:
                    this.DialogResult = DialogResult.OK;
                    SaveGroupInfo();
                    Close();
                    break;
                case E_ButtonType.Close:
                    this.DialogResult = DialogResult.Cancel;
                    Close();
                    break;
            }
        }

        private void SaveGroupInfo()
        {
            if (txtName.Text.Length == 0)
            {
                AppGlobal.ShowMessageBox(this, "그룹 이름을 입력하세요", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2013-08-13 - shinyn - 상위그룹 선택한 내역을 저장하도록 한다.
            TreeNode tNode = trvGroup.treeViewEx1.SelectedNode;
            if (tNode == null)
            {
                AppGlobal.ShowMessageBox(this, "상위 그룹을 선택하세요", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (tNode.Text != "ROOT")
            {
                GroupInfo tGroupInfo = (GroupInfo)tNode.Tag;

                m_GroupInfo.UP_ID = tGroupInfo.ID;
                m_GroupInfo.TOP_ID = tGroupInfo.TOP_ID;
            }
            else if(tNode.Text == "ROOT" && m_GroupInfo.ID != m_GroupInfo.TOP_ID)
            {
                m_GroupInfo.UP_ID = "";
                m_GroupInfo.TOP_ID = m_GroupInfo.ID;
            }
            
            m_GroupInfo.Name = txtName.Text;
            m_GroupInfo.Description = txtDescription.Text;

            RequestCommunicationData tRequestData = null;
            GroupRequestInfo tGrupRequestInfo = new GroupRequestInfo();
            tGrupRequestInfo.WorkType = m_WorkType;
            tGrupRequestInfo.GroupInfo = m_GroupInfo;
            tGrupRequestInfo.UserID = AppGlobal.s_LoginResult.UserID;

            tRequestData = AppGlobal.MakeDefaultRequestData();
            tRequestData.CommType = E_CommunicationType.RequestGroupInfo;

            tRequestData.RequestData = tGrupRequestInfo;

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

            EventProcessor.Run((GroupInfo)m_Result.ResultData, m_WorkType);
            m_GroupInfo = (GroupInfo)m_Result.ResultData;

            if (m_WorkType == E_WorkType.Add)
            {
                AppGlobal.ShowMessageBox(this, "그룹을 추가 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                AppGlobal.ShowMessageBox(this, "그룹을 수정 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}