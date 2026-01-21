using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using RACTCommonClass;
using DevComponents.DotNetBar;
using RACTTerminal;
using RACTSerialProcess;
using System.Threading;
using MKLibrary.MKProcess;
using System.IO;

namespace RACTClient
{
    public partial class ucNotePad : SenderControl
    {
        private DevComponents.DotNetBar.ButtonItem cmPopUP;
        private DevComponents.DotNetBar.ContextMenuBar contextMenuBar1;
        private DevComponents.DotNetBar.ButtonItem mnuUnDo2;
        private DevComponents.DotNetBar.ButtonItem mnuCut2;
        private DevComponents.DotNetBar.ButtonItem mnuCopy2;
        private DevComponents.DotNetBar.ButtonItem mnuPaste2;



        public ucNotePad()
        {
            InitializeComponent();
            
            // 
            // contextMenuBar1
            // 
            this.contextMenuBar1.AntiAlias = true;
            this.contextMenuBar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.cmPopUP});
            this.contextMenuBar1.Location = new System.Drawing.Point(64, 27);
            this.contextMenuBar1.Name = "contextMenuBar1";
            this.contextMenuBar1.Size = new System.Drawing.Size(75, 25);
            this.contextMenuBar1.Stretch = true;
            this.contextMenuBar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.contextMenuBar1.TabIndex = 0;
            this.contextMenuBar1.TabStop = false;
            this.contextMenuBar1.Text = "contextMenuBar1";

            this.mnuCopy2 = new DevComponents.DotNetBar.ButtonItem();
            this.mnuCut2 = new DevComponents.DotNetBar.ButtonItem();
            this.mnuPaste2 = new DevComponents.DotNetBar.ButtonItem();
            this.mnuUnDo2 = new DevComponents.DotNetBar.ButtonItem();

            

            // 
            // cmPopUP
            // 
            this.cmPopUP.AutoExpandOnClick = true;
            this.cmPopUP.Name = "cmPopUP";
            this.cmPopUP.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.mnuCopy2,
            this.mnuPaste2,
            this.mnuCut2,
            this.mnuUnDo2 
            });
            this.cmPopUP.Text = "buttonItem1";


            // 
            // mnuCopy
            // 
            this.mnuCopy2.Name = "mnuCopy";
            this.mnuCopy2.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlC);
            this.mnuCopy2.Text = "복사(&C)";
            this.mnuCopy2.Click += new System.EventHandler(this.mnuCopy2_Click);

            // 
            // mnuUnDo
            // 
            this.mnuUnDo2.Name = "mnuUnDo";
            this.mnuUnDo2.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlZ);
            this.mnuUnDo2.Text = "실행취소(&U)";
            this.mnuUnDo2.Click += new System.EventHandler(this.mnuUnDo2_Click);
            // 
            // mnuCut
            // 
            this.mnuCut2.BeginGroup = true;
            this.mnuCut2.Name = "mnuCut";
            this.mnuCut2.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlX);
            this.mnuCut2.Text = "잘라내기(&T)";
            this.mnuCut2.Click += new System.EventHandler(this.mnuCut2_Click);
            // 
            // mnuPaste
            // 
            this.mnuPaste2.Name = "mnuPaste";
            this.mnuPaste2.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlV);
            this.mnuPaste2.Text = "붙여넣기(&P)";
            this.mnuPaste2.Click += new System.EventHandler(this.mnuPaste2_Click);
            

        }

        private E_NotePadStatus m_NotePadStatus;

        public E_NotePadStatus NotePadStatus 
        {
            get { return m_NotePadStatus; }
            set { m_NotePadStatus = value; }
        }

        private string m_FilePath = string.Empty;

        public string FilePath
        {
            get { return m_FilePath; }
            set { m_FilePath = value; }
        }

        public void OpenFile(string aFilePath)
        {
            FilePath = aFilePath;
            txtNotePad.LoadFile(aFilePath, RichTextBoxStreamType.PlainText);

            
        }


        public void OpenFile()
        {
            OpenFileDialog tOpenFileDialog = new OpenFileDialog();

            tOpenFileDialog.Filter = "TXT Files(*.txt)|*.txt";

            if (tOpenFileDialog.ShowDialog(AppGlobal.s_ClientMainForm) != DialogResult.OK) return;

            FilePath = tOpenFileDialog.FileName;

            txtNotePad.LoadFile(FilePath, RichTextBoxStreamType.PlainText);

            
        }

        private void SaveFile()
        {
            string tSaveFilePath = string.Empty;

            // 파일명이 없으면 새로 저장
            if (FilePath == "")
            {
                SaveFileDialog tSaveFileDialog = new SaveFileDialog();

                tSaveFileDialog.Filter = "TXT Files(*.txt)|*.txt";

                if (tSaveFileDialog.ShowDialog(AppGlobal.s_ClientMainForm) != DialogResult.OK) return;

                tSaveFilePath = tSaveFileDialog.FileName;

                if (File.Exists(tSaveFilePath))
                {
                    File.Delete(tSaveFilePath);
                }
            }
            else
            {
                tSaveFilePath = FilePath;
            }

            
            // 2013-02-24 -shinyn - 파일이 있는 경우 삭제후 새로 만들기
            if (File.Exists(tSaveFilePath))
            {
                File.Delete(tSaveFilePath);
            }

            FileStream tFS = new FileStream(tSaveFilePath, FileMode.CreateNew, FileAccess.Write, FileShare.None);

            StreamWriter swFromFileStreamUTF8Buffer = new StreamWriter(tFS, System.Text.Encoding.UTF8, 512);
            swFromFileStreamUTF8Buffer.Write(txtNotePad.Text);
            swFromFileStreamUTF8Buffer.Flush();
            swFromFileStreamUTF8Buffer.Close();

            FilePath = tSaveFilePath;

            // 2013-02-24 - shinyn - 메모장 팝업 여부에 따른 제목 수정
            if (this.Parent.GetType().Name == "SuperTabControlPanel")
            {
                // 2013.02.-22 메모장 오류 수정
                SuperTabControlPanel tPanel = (SuperTabControlPanel)this.Parent;
                string[] tFileName = FilePath.Split('\\');
                tPanel.Text = string.Format("{0} - 메모장", tFileName[tFileName.GetLength(0) - 1]);
            }
            else if (this.Parent.GetType().Name == "NotePadWindows")
            {
                NotePadWindows tNotePadWindow = (NotePadWindows)this.Parent;
                string[] tFileName = FilePath.Split('\\');
                tNotePadWindow.Text = string.Format("{0} - 메모장", tFileName[tFileName.GetLength(0) - 1]);

            }

        }

        private void SaveAsFile()
        {
            SaveFileDialog tSaveFileDialog = new SaveFileDialog();

            tSaveFileDialog.Filter = "TXT Files(*.txt)|*.txt";

            if (tSaveFileDialog.ShowDialog(AppGlobal.s_ClientMainForm) != DialogResult.OK) return;

            string tSaveFilePath = tSaveFileDialog.FileName;

            if (File.Exists(tSaveFilePath))
            {
                File.Delete(tSaveFilePath);
            }

            FileStream tFS = new FileStream(tSaveFilePath, FileMode.CreateNew, FileAccess.Write, FileShare.None);

            StreamWriter swFromFileStreamUTF8Buffer = new StreamWriter(tFS, System.Text.Encoding.UTF8, 512);
            swFromFileStreamUTF8Buffer.Write(txtNotePad.Text);
            swFromFileStreamUTF8Buffer.Flush();
            swFromFileStreamUTF8Buffer.Close();

            FilePath = tSaveFilePath;

            // 2013-02-24 - shinyn - 메모장 팝업 타입에 따라 메모장 텍스트 수정
            if (this.Parent.GetType().Name == "SuperTabControlPanel")
            {
                SuperTabControlPanel tPanel = (SuperTabControlPanel)this.Parent;
                string[] tFileName = FilePath.Split('\\');
                tPanel.Text = string.Format("{0} - 메모장", tFileName[tFileName.GetLength(0) - 1]);

            }
            else if (this.Parent.GetType().Name == "NotePadWindows")
            {
                NotePadWindows tNotePadWindow = (NotePadWindows)this.Parent;
                string[] tFileName = FilePath.Split('\\');
                tNotePadWindow.Text = string.Format("{0} - 메모장", tFileName[tFileName.GetLength(0) - 1]);
            }
        }

        private void mnuNew_Click(object sender, EventArgs e)
        {
            if (AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm,
                "새 메모를 열면 기존 내용은 사라집니다.\r\n새 메모를 실행하시겠습니까?",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            txtNotePad.Text = "";
            FilePath = "";

            
        }

        private void mnuOpen_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void mnuUnDo_Click(object sender, EventArgs e)
        {
            txtNotePad.Undo();
        }

        private void mnuCut_Click(object sender, EventArgs e)
        {
            txtNotePad.Cut();
        }

        private void mnuCopy_Click(object sender, EventArgs e)
        {
            txtNotePad.Copy();
        }

        private void mnuPaste_Click(object sender, EventArgs e)
        {
            txtNotePad.Paste();
        }

        private void mnuUnDo2_Click(object sender, EventArgs e)
        {
            txtNotePad.Undo();
        }

        private void mnuCut2_Click(object sender, EventArgs e)
        {
            txtNotePad.Cut();
        }

        private void mnuCopy2_Click(object sender, EventArgs e)
        {
            txtNotePad.Copy();
        }

        private void mnuPaste2_Click(object sender, EventArgs e)
        {
            txtNotePad.Paste();
        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void mnuSaveAs_Click(object sender, EventArgs e)
        {
            SaveAsFile();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            
        }

        private void txtNotePad_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Font tFont = new Font(FontFamily.GenericMonospace, 8.5F);
                Graphics tGraphics = this.CreateGraphics();

                cmPopUP.Popup(MousePosition);
            }
        }
    }
}
