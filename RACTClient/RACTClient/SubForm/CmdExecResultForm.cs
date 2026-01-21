using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using C1.Win.C1FlexGrid;

using System.Security.Permissions;
using System.Security.AccessControl;
using System.Security.Principal;

namespace RACTClient
{
    public partial class CmdExecResultForm : Form
    {

        Dictionary<string, string> resultCmd = new Dictionary<string, string>();

        private StringBuilder allResult = new StringBuilder();
        private StringBuilder sbResult = new StringBuilder();

        public CmdExecResultForm()
        {
            InitializeComponent();
        }              
        

        /// <summary>
        /// 2015-09-24 - gunny - Log 파일열기  열기 실행
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClick_OpenLogFile(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog tOpenFileDialog = new OpenFileDialog();

                tOpenFileDialog.Filter = "LOG Files(*.clog)|*.clog";
                
                string tDirPath = Application.StartupPath + @"\Log\AutoSaveLogs\";
                tOpenFileDialog.InitialDirectory = tDirPath;
                

                if (tOpenFileDialog.ShowDialog(AppGlobal.s_ClientMainForm) != DialogResult.OK) return;

                string tFilePath = tOpenFileDialog.FileName;
                
                txtFileOpen.Text  = tFilePath;

                rdoResultAll.Checked = true;
                //grdCmdList.Rows.Count = 1;
                rtxtResult.Text = "";
                allResult.Length = 0;

                FileRead(tFilePath);

                

                //tabNotePads.Selected = true;
            }
            catch (Exception ex)
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, ex.Message.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void FileRead(string filePath)
        {

            FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read);

            StreamReader st = new StreamReader(fs, System.Text.Encoding.UTF8);

            try
            {
                st.BaseStream.Seek(0, SeekOrigin.Begin);

                bool isAppendText = false;
                string cmd = "";
                sbResult.Length = 0;

                resultCmd = new Dictionary<string, string>();
                while (st.Peek() > -1)
                {

                    string temp = st.ReadLine();
                    temp = temp.Replace("\0", "");

                    if (temp.Contains("|&|"))
                    {
                        if (cmd != "")
                        {
                            cmd = cmd.Replace("|&|", "");
                            cmd = cmd.Replace("||", "");
                            int i = 0;
                            string tempCmd = cmd;
                            while (resultCmd.ContainsKey(tempCmd))
                            {
                                tempCmd = cmd + "(" + i + ")";
                                i++;
                            }

                            resultCmd.Add(tempCmd, "< 명령어 : " + tempCmd + " > \r\n" +sbResult.ToString());


                            cmd = "";
                        }
                        cmd = temp;
                        isAppendText = true;
                        sbResult = new StringBuilder();
                    }
                    else
                    {
                        if (isAppendText && temp != "")
                        {
                            sbResult.Append(temp + "\n");
                        }
                    }

                    allResult.Append(temp + "\n");

                }
                if (cmd != "")
                {
                    cmd = cmd.Replace("|&|", "");
                    cmd = cmd.Replace("||", "");
                    int i = 0;
                    string tempCmd = cmd;

                    while (resultCmd.ContainsKey(tempCmd))
                    {
                        tempCmd = cmd + "(" + i + ")";
                        i++;
                    }
                    allResult = allResult.Replace("|&|", "\n< 명령어 : ");
                    allResult = allResult.Replace("||", " > \r\n");

                    resultCmd.Add(tempCmd, "< 명령어 : " + tempCmd + " > \r\n" + sbResult.ToString());
                }


                rtxtResult.Text = allResult.ToString();

                st.Close();

                fs.Close();

                //2015-10-30 명령어 bold 처리
                SetStyleCommandLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("FileRead : " + ex.ToString());

                if (st != null)
                    st.Dispose();

                if (fs != null)
                    fs.Dispose();
            }

        }

        //2015-10-30 명령어 bold 처리
        /// <summary>
        /// 명령어 라인을 Bold 처리합니다.
        /// </summary>
        private void SetStyleCommandLine()
        {
            try
            {
                string tCmdTextStart = "< 명령어 : ";
                int tCmdTextStartIdx = 0;
                
                string tCmdTextEnd = " > ";
                int tCmdTextEndIdx = 0;

                int tFindPos = 0;
                string tCmdText = "";

                while (tFindPos > -1) //더이상 찾는 명령어가 없으면 -1 이 되므로 조건문을 이와 같이 정함.
                {
                    tCmdTextStartIdx = rtxtResult.Text.IndexOf(tCmdTextStart, tFindPos);
                    tCmdTextEndIdx = rtxtResult.Text.IndexOf(tCmdTextEnd, tFindPos);

                    tCmdText = rtxtResult.Text.Substring(tCmdTextStartIdx, tCmdTextEndIdx - tCmdTextStartIdx + 2); 
                    
                    rtxtResult.Find(tCmdText, tFindPos, RichTextBoxFinds.WholeWord);
                    rtxtResult.SelectionFont = new Font("", 10, FontStyle.Bold);

                    //다음 명령어 텍스트 위치 찾기.                    
                    tFindPos += tCmdText.Length;
                    tFindPos = rtxtResult.Text.IndexOf(tCmdTextStart, tFindPos);                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void OnClick_checkAll(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                grdCmdList.Rows.Count = 1;
                rtxtResult.Text = allResult.ToString();
            }
        }

        private void OnClick_SeclctCmd(object sender, EventArgs e)
        {
            try
            {
                rtxtResult.Text = "";
                grdCmdList.Rows.Count = 1;
                int i = 1;
                foreach (string key in resultCmd.Keys)
                {
                    grdCmdList.Rows.Add();
                    grdCmdList[i, "colCommand"] = key;
                    grdCmdList.Rows[i].UserData = resultCmd[key];

                    i++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("OnClick_SeclctCmd : " + ex.ToString());
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSaveResult_Click(object sender, EventArgs e)
        {
            Console.WriteLine("결과저장");

            WriteSaveResult();
        }

        public void WriteSaveResult()
        {
            try
            {
                SaveFileDialog tOpenDialog = new SaveFileDialog();
                tOpenDialog.DefaultExt = "tacts";
                tOpenDialog.Filter = "TACT Log Files (*.log)|*.log|All Files (*.*)|*.*";
               

                if (rtxtResult.Text.Equals(""))
                {
                    AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, " 저장할 내용이 없습니다.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }


                if (tOpenDialog.ShowDialog(AppGlobal.s_ClientMainForm) == DialogResult.OK)
                {
                    // 2015-04-16 - 신윤남 - 저장된 터미널 로그를 저장합니다.
                    string tString = "";
                    File.AppendAllText(tOpenDialog.FileName, rtxtResult.Text);

                 
                    AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "파일을 저장 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                

            }
            catch (Exception e)
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, e.Message.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "파일을 저장 실패 했습니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void OnSelChangeGrid2(object sender, RowColEventArgs e)
        {
            Console.WriteLine("OnSelChangeGrid : " +sender.ToString());
            if (grdCmdList.Rows.Count > 1)
            {
                rtxtResult.Text = "";
            }
            for ( int i = 1 ; i < grdCmdList.Rows.Count ; i++ ){
                CheckEnum result = grdCmdList.GetCellCheck(i, 0);
                if (result == CheckEnum.Checked)
                {
                    rtxtResult.Text = rtxtResult.Text + grdCmdList.Rows[i].UserData.ToString()+"\n";
                }
               
            }

            //2015-10-30 명령어 bold 처리
            if (rtxtResult.Text.Length > 0)
            {
                SetStyleCommandLine();
            }            
        }
    }
}
