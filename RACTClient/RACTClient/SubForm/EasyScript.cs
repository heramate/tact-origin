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
    public partial class EasyScript : BaseForm
    {
        private bool m_IsCommandPopupAllow = false;

        private bool m_IsCommandEditing = true;

        private int m_SelectedRow = 0;

        private bool m_IsPermit = true;

        public bool IsCommandEditing
        {
            get { return m_IsCommandEditing; }
            set { m_IsCommandEditing = value; }
        }

        public EasyScript()
        {
            InitializeComponent();
        }

        public void InitializeControl()
        {
            AddButton(E_ButtonType.Close, E_ButtonSide.Right, "닫기");

            InitializeColumn();
        }

        protected override void ButtonProcess(E_ButtonType aButtonType)
        {
            if (aButtonType == E_ButtonType.Close)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void InitializeColumn()
        {
            fgCommand.Cols["No"].Caption = "순서";
            fgCommand.Cols["No"].AllowEditing = false;
            fgCommand.Cols["No"].Visible = false;
            fgCommand.Cols["No"].Width = 50;

            fgCommand.Cols["Command"].Caption = "명령어";
            fgCommand.Cols["Command"].AllowEditing = true;
            fgCommand.Cols["Command"].Width = 431;
            
            fgCommand.Cols["Prompt"].Caption = "프롬프트";
            fgCommand.Cols["Prompt"].AllowEditing = true;
            fgCommand.Cols["Prompt"].Width = 100;

            fgCommand.AllowAddNew = true;
            fgCommand.AllowDelete = true;
            fgCommand.AllowEditing = true;
            fgCommand.AllowResizing = C1.Win.C1FlexGrid.AllowResizingEnum.Columns;
            fgCommand.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
        }

        private void fgCommandList_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                m_IsCommandEditing = true;

                if (fgCommand.Rows.Count < 3) return;

                if (fgCommand.Rows.Count == 1)
                {
                    AddNewLine();
                }
                else
                {
                    if (fgCommand.Rows.Count < 3)
                    {
                        fgCommand.RemoveItem(fgCommand.RowSel);
                        ResetIndex(1);
                        fgCommand.Select(1, fgCommand.Cols["Command"].Index);
                        fgCommand.AutoSizeCol(1);
                        InitializeColumn();
                        ResetIndex(1);
                    }
                    else
                    {
                        fgCommand.RemoveItem(fgCommand.RowSel);
                        ResetIndex(1);
                        fgCommand.Select(fgCommand.RowSel, fgCommand.Cols["Command"].Index);
                        fgCommand.AutoSizeCol(1);
                        InitializeColumn();
                        ResetIndex(1);
                    }

                    
                }
            }

            //[2008.09.05]mjjoe 명령어 중간 삽입 기능 추가
            if (e.KeyCode == Keys.Insert)
            {
                AddNewLine(m_SelectedRow);
            }
        }

        private void AddNewLine()
        {
            int tRow = fgCommand.Rows.Count;
            fgCommand.Rows.Count++;
            fgCommand[tRow, "No"] = tRow.ToString();
                                   
            fgCommand[tRow, "Command"] = "";
            fgCommand[tRow, "Prompt"] = "";

            fgCommand.Select(tRow - 1, fgCommand.Cols["Command"].Index);

            InitializeColumn();
        }

        private void AddNewLine(int Rowindex)
        {
            int tRow = Rowindex;

            if (tRow <= 0) return;	// 선택한 Row가 없을 경우는 그냥 종료 한다. 

            object[] Rowitems = new object[fgCommand.Cols.Count];

            Rowitems[0] = tRow.ToString();

            fgCommand.AddItem(Rowitems, tRow, 0);

            ResetIndex(tRow);
            fgCommand.Select(tRow, 1);

            InitializeColumn();
            ResetIndex(1);
        }

        private void ResetIndex(int tStartRow)
        {
            for (int i = tStartRow; i < fgCommand.Rows.Count; i++)
            {
                fgCommand[i, "No"] = i.ToString();
            }
        }

        private void fgCommand_ChangeEdit(object sender, EventArgs e)
        {
            //if (fgCommand[fgCommand.RowSel, "Command"] == null) return;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {

                if (fgCommand.Rows.Count < 2) return;

                int i = 0;
                int j = 1;
                /*
                for (i = 1; i <= fgCommand.Rows.Count - 1; i++)
                {
                    for (j = 1; j <= 2; j++)
                    {
                        if (Convert.ToString(fgCommand.Rows[i][j]) == "")
                        {
                            switch (j)
                            {
                                case 1:
                                    fgCommand.Rows.Move(0, i);
                                    MessageBox.Show("명령어를 입력해주세요");
                                    break;
                                case 2:
                                    fgCommand.Rows.Move(0, i);
                                    MessageBox.Show("프롬프트를 입력해주세요");
                                    break;
                            }
                            return;
                        }
                    }
                }
                */
                txtScript.Clear();

                StringBuilder tScript = new StringBuilder();


                tScript.Append("Sub Main\r\n");
    

                // 아스키 코드값 " : 34
                char tCode = (char)34;

                for (i = 1; i <=  fgCommand.Rows.Count - 1; i++)
                {
                    for (j = 1; j <= 2; j++)
                    {
                        // Command는 TACT.Send Script이다.
                        if (j == 1 && Convert.ToString(fgCommand.Rows[i][j]) != "")
                        {
                            tScript.Append("TACT.Send ");
                            // " : 22추가 
                            tScript.Append(tCode);
                            tScript.Append(Convert.ToString(fgCommand.Rows[i][j]));
                            tScript.Append(tCode);
                            tScript.Append("&char(13)\r\n");
                        }
                        // Prompt는 TACT.WaitForString Script이다
                        if (j == 2 && Convert.ToString(fgCommand.Rows[i][j]) != "")
                        {
                            tScript.Append("TACT.WaitForString ");
                            tScript.Append(tCode);
                            tScript.Append(Convert.ToString(fgCommand.Rows[i][j]));
                            tScript.Append(tCode);
                            //tScript.Append("&chr(13)\r\n");
                        }
                    }
                }

                tScript.Append("End Sub");

                txtScript.Text = tScript.ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("EasyScript btnCreate_Click :" + ex.Message.ToString());   
            }

        }

        private void fgCommand_SelChange(object sender, EventArgs e)
        {
            m_SelectedRow = fgCommand.RowSel;


        }

        private void fgCommand_KeyDown(object sender, KeyEventArgs e)
        {
            if (!m_IsPermit)
            {
                if (AppGlobal.GridClipBoard(fgCommand, e.Control, e.KeyCode, false) == E_ClipboardProcessType.Paste)
                {
                    fgCommand.AutoSizeCol(1);
                    ResetIndex(1);
                }
            }

            AppGlobal.GridClipBoard(fgCommand, e.Control, e.KeyCode, false);
        }

        private void fgCommand_AfterEdit(object sender, C1.Win.C1FlexGrid.RowColEventArgs e)
        {
            InitializeColumn();
            ResetIndex(1);
        }

    }
}