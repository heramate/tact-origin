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
    public partial class ModeChangeSubForm : Form
    {
        public ModeChangeSubForm()
        {
            InitializeComponent();
        }
        
        private void splashControl1_OnExit()
        {
            AppGlobal.s_IsModeChangeConnect = false;
            AppGlobal.s_RACTClientMode = E_RACTClientMode.Console;
            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "클라이언트 모드를 변경을 취소 했습니다.");
            this.Hide();
        }

        internal void InitializeControl()
        {
            m_SplashControl.InitalizeControl();
        }
    }
}