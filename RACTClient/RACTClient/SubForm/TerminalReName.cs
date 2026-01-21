using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RACTClient
{
    public partial class TerminalReName : BaseForm
    {
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public TerminalReName():this("")
        {
        }
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        /// <param name="aName">탭 이름 입니다.</param>
        public TerminalReName(string aName)
        {
            InitializeComponent();
            txtName.Text = aName;
        }

        public void InitializeContro()
        {
            AddButton(E_ButtonType.Close, E_ButtonSide.Right, "닫기");
            AddButton(E_ButtonType.OK, E_ButtonSide.Right, "확인");
        }
        /// <summary>
        /// 이름을 가져오기 합니다.
        /// </summary>
        public string GetNewTabName
        {
            get { return txtName.Text.Trim(); }
        }
        protected override void ButtonProcess(E_ButtonType aButtonType)
        {
            if (aButtonType == E_ButtonType.OK)
            {
                if (txtName.Text.Trim().Length == 0)
                {
                    AppGlobal.ShowMessageBox(this, "새로운 탭 이름을 입력하세요.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtName.Focus();
                    return;
                }
                DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                this.Close();
            }
            
        }
    }
}