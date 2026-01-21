using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RACTClient
{
    public partial class TerminalPopupType : BaseForm
    {
        public TerminalPopupType()
        {
            InitializeComponent();
            initializeControl();

        }
        /// <summary>
        /// 컨트롤을 초기화 합니다.
        /// </summary>
        private void initializeControl()
        {
            pnlTerminal.ChangeMode(false);
            AddButton(E_ButtonType.OK, E_ButtonSide.Right, "확인");
        }

        /// <summary>
        /// 버튼 처리를 합니다.
        /// </summary>
        /// <param name="aButtonType"></param>
        protected override void ButtonProcess(E_ButtonType aButtonType)
        {
            pnlTerminal.SaveOption();
            AppGlobal.MakeClientOption();
            Close();
        }
    }
}