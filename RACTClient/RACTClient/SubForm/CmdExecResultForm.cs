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

                sbResult.Length = 0;
                resultCmd = new Dictionary<string, string>();
                allResult.Length = 0;

                List<string> rawLines = new List<string>();
                while (st.Peek() > -1)
                {
                    rawLines.Add((st.ReadLine() ?? string.Empty).Replace("\0", string.Empty));
                }

                List<string> normalizedLines = NormalizeLogLines(rawLines);
                string currentCommand = string.Empty;
                sbResult = new StringBuilder();

                foreach (string line in normalizedLines)
                {
                    if (IsCommandMarker(line))
                    {
                        if (!string.IsNullOrWhiteSpace(currentCommand))
                        {
                            AddCommandResult(currentCommand, sbResult.ToString());
                        }

                        currentCommand = ExtractCommandFromMarker(line);
                        sbResult.Clear();
                        allResult.AppendLine("< 명령어 : " + currentCommand + " > ");
                        continue;
                    }

                    if (!string.IsNullOrWhiteSpace(currentCommand) && !string.IsNullOrEmpty(line))
                    {
                        sbResult.AppendLine(line);
                    }

                    allResult.AppendLine(line);
                }

                if (!string.IsNullOrWhiteSpace(currentCommand))
                {
                    AddCommandResult(currentCommand, sbResult.ToString());
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

        private List<string> NormalizeLogLines(IEnumerable<string> rawLines)
        {
            List<string> normalized = new List<string>();
            string pendingCommandMarker = null;

            foreach (string rawLine in rawLines)
            {
                string line = rawLine ?? string.Empty;

                if (pendingCommandMarker != null)
                {
                    if (line.Trim().Equals("||"))
                    {
                        normalized.Add(pendingCommandMarker + "||");
                        pendingCommandMarker = null;
                        continue;
                    }

                    normalized.Add(pendingCommandMarker);
                    pendingCommandMarker = null;
                }

                if (line.Contains("|&|") && !line.Contains("||"))
                {
                    pendingCommandMarker = line;
                    continue;
                }

                normalized.Add(line);
            }

            if (pendingCommandMarker != null)
            {
                normalized.Add(pendingCommandMarker + "||");
            }

            return normalized;
        }

        private bool IsCommandMarker(string line)
        {
            return !string.IsNullOrWhiteSpace(line) && line.Contains("|&|");
        }

        private string ExtractCommandFromMarker(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return string.Empty;
            }

            int markerStart = line.IndexOf("|&|", StringComparison.Ordinal);
            if (markerStart >= 0)
            {
                line = line.Substring(markerStart + 3);
            }

            int markerEnd = line.IndexOf("||", StringComparison.Ordinal);
            if (markerEnd >= 0)
            {
                line = line.Substring(0, markerEnd);
            }

            return line.Replace("\r", string.Empty)
                       .Replace("\n", string.Empty)
                       .Trim();
        }

        private void AddCommandResult(string command, string rawResult)
        {
            string cleanedCommand = ExtractCommandFromMarker("|&|" + command + "||");
            string cleanedResult = NormalizeCommandResult(cleanedCommand, rawResult);
            string displayCommand = cleanedCommand;
            int duplicateIndex = 0;

            while (resultCmd.ContainsKey(displayCommand))
            {
                displayCommand = cleanedCommand + "(" + duplicateIndex + ")";
                duplicateIndex++;
            }

            resultCmd.Add(displayCommand, "< 명령어 : " + displayCommand + " > \r\n" + cleanedResult);
        }

        private string NormalizeCommandResult(string command, string rawResult)
        {
            var lines = rawResult.Replace("\r\n", "\n")
                                 .Replace('\r', '\n')
                                 .Split(new[] { '\n' }, StringSplitOptions.None)
                                 .ToList();

            while (lines.Count > 0 && string.IsNullOrWhiteSpace(lines[0]))
            {
                lines.RemoveAt(0);
            }

            while (lines.Count > 0 && IsEchoLine(lines[0], command))
            {
                lines.RemoveAt(0);

                while (lines.Count > 0 && string.IsNullOrWhiteSpace(lines[0]))
                {
                    lines.RemoveAt(0);
                }
            }

            return string.Join("\r\n", lines);
        }

        private bool IsEchoLine(string line, string command)
        {
            string trimmedLine = (line ?? string.Empty).Trim();
            string trimmedCommand = (command ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(trimmedLine) || string.IsNullOrWhiteSpace(trimmedCommand))
            {
                return false;
            }

            if (trimmedLine.Equals(trimmedCommand, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (!trimmedLine.EndsWith(trimmedCommand, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            string prefix = trimmedLine.Substring(0, trimmedLine.Length - trimmedCommand.Length).TrimEnd();
            if (string.IsNullOrEmpty(prefix))
            {
                return false;
            }

            char lastChar = prefix[prefix.Length - 1];
            return lastChar == '#' || lastChar == '>' || lastChar == '$' || lastChar == '%';
        }

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
