using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RACTCommonClass;
using MKLibrary.Controls;
using System.IO;
using System.Reflection.Emit;
using MSScriptControl;

namespace RACTClient
{
    public partial class ModifyScript : BaseForm
    {
        /// <summary>
        /// 작업 타입 입니다.
        /// </summary>
        private E_WorkType m_WorkType;
        /// <summary>
        /// 단축 명령 입니다.
        /// </summary>
        private Script m_CommandInfo;
        /// <summary>
        /// 원본 데이터 입니다.
        /// </summary>
        private Script m_OldCommandInfo;
        /// <summary>
        /// 2020-10-05 TACT기능개선 스크립트명령기능 오류 수정
        /// </summary>
        public event ReturnHandlerArgument1<bool> OnDulplicate;

        public E_WorkType WorkType
        {
            get { return m_WorkType; }
            //set { m_WorkType = value; }
        }

        public Script CommandInfo
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
        public ModifyScript(ScriptGroupInfo aGroupInfo)
            : this(E_WorkType.Add, new Script())
        {
            m_CommandInfo.GroupID = aGroupInfo.ID;
        }

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public ModifyScript(E_WorkType aWorkType, Script aCommandInfo)
        {
            InitializeComponent();
            m_WorkType = aWorkType;
            m_CommandInfo = new Script(aCommandInfo);
        }

        /// <summary>
        /// 컨트롤을 초기화 합니다.
        /// </summary>
        public void InitializeControl()
        {
            AddButton(E_ButtonType.Close, E_ButtonSide.Right, "닫기");
            AddButton(E_ButtonType.OK, E_ButtonSide.Right, "확인");
            AppGlobal.InitializeButtonStyle(btnAddGroup);

            groupBox1.Visible = AppGlobal.s_RACTClientMode == E_RACTClientMode.Online;
            EventProcessor.OnScriptGroupInfoChange += new HandlerArgument2<ScriptGroupInfo, E_WorkType>(OnChangeCommandGroup);
            DisplayBaseData();
        }

        /// <summary>
        /// 명령 정보를 표시 합니다.
        /// </summary>
        private void DisplayBaseData()
        {
            DisplayCommandGroup();
            txtName.Text = m_CommandInfo.Name;
            txtDescription.Text = m_CommandInfo.Description;
            txtScript.Text = m_CommandInfo.RawScript;
        }

        /// <summary>
        /// 그룹을 표시 합니다.
        /// </summary>
        private void DisplayCommandGroup()
        {
            foreach (ScriptGroupInfo tCommand in AppGlobal.s_ScriptList)
            {
                OnChangeCommandGroup(tCommand, E_WorkType.Add);
            }
           
        }
        void OnChangeCommandGroup(ScriptGroupInfo aValue1, E_WorkType aValue2)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument2<ScriptGroupInfo, E_WorkType>(OnChangeCommandGroup), new object[] { aValue1, aValue2 });
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
                ScriptGroupInfo tTag;
                for (int i = 0; i < cboGroup.Items.Count; i++)
                {
                    tTag = (ScriptGroupInfo)cboGroup.Items[i].Tag;
                    if (tTag.ID == aValue1.ID)
                    {
                        cboGroup.Items[i].Text = aValue1.Name;
                        cboGroup.Items[i].Tag = aValue1;
                    }
                }
            }
            else
            {
                ScriptGroupInfo tTag;
                for (int i = 0; i < cboGroup.Items.Count; i++)
                {
                    tTag = (ScriptGroupInfo)cboGroup.Items[i].Tag;
                    if (tTag.ID == aValue1.ID)
                    {
                        cboGroup.Items.RemoveAt(i);
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
                case E_ButtonType.OK:
                    SaveScript();
                    break;
            }
        }
        /// <summary>
        /// 명령어 저장 처리합니다.
        /// </summary>
        private void SaveScript()
        {
            if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online)
            {
                if (txtName.Text.Trim().Length == 0)
                {
                    AppGlobal.ShowMessageBox(this, "스크립트 이름을 입력 하세요.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
				//2020-10-05 TACT기능개선 스크립트명령기능 오류 수정
                if (OnDulplicate(this))
                {
                    AppGlobal.ShowMessageBox(this, "스크립트 명령 이름이 중복 됩니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cboGroup.SelectedIndex < 0)
                {
                    AppGlobal.ShowMessageBox(this, "스크립트 그룹을 선택 하세요.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                m_OldCommandInfo = m_CommandInfo.DeepClone();

                //if (!m_CommandInfo.Compile(cssEditor1.EditText))
                //{
                //    AppGlobal.ShowMessageBox(this, "스크립트가 잘못 되었습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}


                m_CommandInfo.Name = txtName.Text.Trim();
                m_CommandInfo.GroupID = ((ScriptGroupInfo)cboGroup.Items[cboGroup.SelectedIndex].Tag).ID;
                m_CommandInfo.Description = txtDescription.Text;
                m_CommandInfo.RawScript = txtScript.Text;

                RequestCommunicationData tRequestData = null;
                ScriptRequestInfo tShortenCommandRequestInfo = new ScriptRequestInfo();
                tShortenCommandRequestInfo.WorkType = m_WorkType;
                tShortenCommandRequestInfo.ScriptInfo = m_CommandInfo;
                tShortenCommandRequestInfo.UserID = AppGlobal.s_LoginResult.UserID;

                tRequestData = AppGlobal.MakeDefaultRequestData();
                tRequestData.CommType = E_CommunicationType.RequestScriptInfo;

                tRequestData.RequestData = tShortenCommandRequestInfo;

                m_Result = null;
                m_MRE.Reset();

                AppGlobal.SendRequestData(this, tRequestData);
                m_MRE.WaitOne(AppGlobal.s_RequestTimeOut);

                CheckedResult();
            }
            else
            {
                SaveFile();
            }
        }

        private void SaveFile()
        {
            try
            {
                SaveFileDialog tOpenDialog = new SaveFileDialog();
                tOpenDialog.DefaultExt = "tacts";
                tOpenDialog.Filter = "TACT Script Files (*.tacts)|*.tacts|All Files (*.*)|*.*";

                if (tOpenDialog.ShowDialog(AppGlobal.s_ClientMainForm) == DialogResult.OK)
                {
                    File.WriteAllText(tOpenDialog.FileName, txtScript.Text);

                    AppGlobal.ShowMessageBox(this, "스크립트 파일을 저장 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch
            {
                AppGlobal.ShowMessageBox(this, "스크립트 파일 저장 실패 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
				//2020-10-05 TACT기능개선 스크립트명령기능 오류 수정
                EventProcessor.Run((Script)m_Result.ResultData, E_WorkType.Modify);
                //EventProcessor.Run(m_OldCommandInfo, E_WorkType.Delete);
                //EventProcessor.Run((Script)m_Result.ResultData, E_WorkType.Add);
            }
            else
            {
                EventProcessor.Run((Script)m_Result.ResultData, m_WorkType);
            }

            switch (m_WorkType)
            {

                case E_WorkType.Add:
                    AppGlobal.ShowMessageBox(this, "스크립트를 추가 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;

                case E_WorkType.Modify:
                    AppGlobal.ShowMessageBox(this, "스크립트를 수정 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }

            this.Close();
        }

        private void mnuPaste_Click(object sender, EventArgs e)
        {
            txtScript.Paste();
        }

      

        private void mnuNew_Click(object sender, EventArgs e)
        {
            if (!txtScript.Text.Equals(m_CommandInfo.RawScript))
            {
                if (AppGlobal.ShowMessageBox(this, "스크립트가 변경 되었습니다.\r\n계속 하시겠습니까?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
            }
            txtScript.Text="";
        }

        private void mnuOpen_Click(object sender, EventArgs e)
        {
            string tScriptText = "";
            if (AppGlobal.ShowScriptOpenDialog(out tScriptText) == DialogResult.OK)
            {
                txtScript.Text = tScriptText;
            }
        }

       
        


        private void mnuRun_Click(object sender, EventArgs e)
        {

            try
            {
                MSScriptControl.ScriptControl m_Script = new ScriptControlClass();
                m_Script.Language = "VBScript";
                Object[] oParams = new Object[0] { };
                RACT tObjectClass = new RACT();

                m_Script.ExecuteStatement(txtScript.Text);
                m_Script.AddObject("TACT", tObjectClass, true);
                string tString = (string)m_Script.Run("Main", ref oParams);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cssEditor1_EditTextChanged(object sender, EventArgs e)
        {
           
        }

        private void mnuBreakPoint_Click(object sender, EventArgs e)
        {

        }

        private void mnuCopy_Click(object sender, EventArgs e)
        {
            txtScript.Copy();
        }

        private void mnuUnDo_Click(object sender, EventArgs e)
        {
            txtScript.Undo();
        }

        private void mnuCut_Click(object sender, EventArgs e)
        {
            txtScript.Cut();
        }

        private void mnuSaveAs_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void btnAddGroup_Click(object sender, EventArgs e)
        {
            ModifyScriptGroup tGroup = new ModifyScriptGroup();
            tGroup.InitializeControl();
            tGroup.ShowDialog();
        }

        // 2013-04-19 - shinyn - 간편 스크립트 생성
        private void btnEasyScript_Click(object sender, EventArgs e)
        {
            try
            {
                EasyScript tEasyScript = new EasyScript();
                tEasyScript.InitializeControl();
                tEasyScript.ShowDialog();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ModifyScript btnEasyScript_Click : "+ex.Message.ToString());
            }

        }
        
    }
}