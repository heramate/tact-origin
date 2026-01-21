using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RACTClient
{
    /// <summary>
    /// 텔넷 찾기에 사용할 핸들러 입니다.
    /// </summary>
    /// <param name="aString">찾을 내용 입니다.</param>
    public delegate void TelnetStringFindHandler(TelnetStringFindHandlerArgs aStringArgs);

    public partial class TelnetFindForm : BaseForm
    {
        /// <summary>
        /// 텔넷 찾기 이벤트 입니다.
        /// </summary>
        public event TelnetStringFindHandler OnTelnetStringFind;

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public TelnetFindForm()
        {
            InitializeComponent();
            InitializeControl();
           
        }
       
        /// <summary>
        /// 컨트롤을 초기화 합니다.
        /// </summary>
        private void InitializeControl()
        {
            AddButton(E_ButtonType.Close, E_ButtonSide.Right, "닫기");
            AddButton(E_ButtonType.OK, E_ButtonSide.Right, "다음 찾기");
           
        }

        /// <summary>
        /// 버튼 처리 입니다.
        /// </summary>
        /// <param name="aButtonType"></param>
        protected override void ButtonProcess(E_ButtonType aButtonType)
        {
            switch (aButtonType)
            {
                case E_ButtonType.OK:
                    if (txtNote.Text.Length == 0) return;
                    if (OnTelnetStringFind != null) OnTelnetStringFind(new TelnetStringFindHandlerArgs(txtNote.Text,cboMatchCase.Checked));
                    break;
                case E_ButtonType.Close:
                    this.Close();
                    break;
            }
        }

        private void TelnetFindForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void TelnetFindForm_Load(object sender, EventArgs e)
        {
            txtNote.Focus();
        }

        private void TelnetFindForm_Activated(object sender, EventArgs e)
        {
            txtNote.Focus();
        }

        private void txtNote_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Return)
            {
                ButtonProcess(E_ButtonType.OK);
            }
        }
    }
}