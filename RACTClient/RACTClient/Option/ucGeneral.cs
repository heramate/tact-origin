using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RACTClient
{
    public partial class ucGeneral : UserControl, IOptionPanal
    {
        public ucGeneral()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 초기화 합니다.
        /// </summary>
        public void InitializeControl()
        {
            AppGlobal.InitializeButtonStyle(btnLogOpenPath);
            AppGlobal.InitializeButtonStyle(btnScriptPath);

            if (AppGlobal.s_ClientOption.DefaultMainControl == E_ClientDefaultMainControlType.BatchRegister)
            {
                rdoBatchRegister.Checked = true;
            }
            else
            {
                rdoTerminal.Checked = true;
            }
			// 2019-11-10 개선사항 (로그 저장 경로 개선)
            txtLogPath.Text = AppGlobal.s_ClientOption.LogPath;
            txtScriptPath.Text = AppGlobal.s_ClientOption.ScriptSavePath;
			// 2019-11-10 개선사항 (로그 자동저장 설장값 옵션으로 지원 기능 추가 )
            autoSaveSwitch.Value = AppGlobal.s_ClientOption.IsAutoSaveLog;
        }

        public bool SaveOption()
        {
            if (txtScriptPath.Text.Trim().Length == 0)
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "스크립트 위치를 설정 하세요", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (txtLogPath.Text.Trim().Length == 0)
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "로그 저장 위치를 설정 하세요", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
			// 2019-11-10 개선사항 (로그 저장 경로 개선)
            AppGlobal.s_ClientOption.LogPath = txtLogPath.Text;
            AppGlobal.s_ClientOption.ScriptSavePath = txtScriptPath.Text;

            AppGlobal.s_ClientOption.DefaultMainControl = rdoBatchRegister.Checked ? E_ClientDefaultMainControlType.BatchRegister : E_ClientDefaultMainControlType.Terminal;
			// 2019-11-10 개선사항 (로그 자동저장 설장값 옵션으로 지원 기능 추가 )
            AppGlobal.s_ClientOption.IsAutoSaveLog = autoSaveSwitch.Value; 

            return true;
        }

        private void btnLogOpenPath_Click(object sender, EventArgs e)
        {
			// 2019-11-10 개선사항 (로그 저장 경로 개선)
            folderBrowserDialog1.SelectedPath = AppGlobal.s_ClientOption.LogPath;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                if (folderBrowserDialog1.SelectedPath.EndsWith("Log"))
                    txtLogPath.Text = folderBrowserDialog1.SelectedPath + "\\";
                else
                    txtLogPath.Text = folderBrowserDialog1.SelectedPath + "\\Log\\";
            }
        }

        private void btnScriptPath_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = AppGlobal.s_ClientOption.ScriptSavePath;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtScriptPath.Text = folderBrowserDialog1.SelectedPath + "\\";
            }
        }
    }
}
