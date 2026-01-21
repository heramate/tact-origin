using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RACTClient
{
    public partial class SearchIPList : BaseForm
    {

        private StringBuilder m_IPList = new StringBuilder();

        private string m_IPString = string.Empty;

        public string IPString
        {
            get { return m_IPString; }
        }

        public SearchIPList()
        {
            InitializeComponent();
            InitializeControl();
        }

        public void InitializeControl()
        {
            AddButton(E_ButtonType.Accept, E_ButtonSide.Right, "확인");
            AddButton(E_ButtonType.Cancel, E_ButtonSide.Right, "닫기");

            InitializeColumn();
        }

        private void InitializeColumn()
        {
            fgCommand.Cols["IP"].Caption = "IP";
            fgCommand.Cols["IP"].AllowEditing = true;
            fgCommand.Cols["IP"].Width = 238;


            fgCommand.AllowAddNew = true;
            fgCommand.AllowDelete = true;
            fgCommand.AllowEditing = true;
            fgCommand.AllowResizing = C1.Win.C1FlexGrid.AllowResizingEnum.Columns;
            fgCommand.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None;
        }

        protected override void ButtonProcess(E_ButtonType aButtonType)
        {
            try
            {
                if (aButtonType == E_ButtonType.Accept)
                {
                    
                    for (int i = 1; i < fgCommand.Rows.Count; i++)
                    {
                        if (i == 1)
                        {
                            if (fgCommand.Rows[i]["IP"] == null)
                            {
                                m_IPList.Append(" ");
                            }
                            else
                            {
                                m_IPList.Append(fgCommand.Rows[i]["IP"].ToString());
                            }
                        }
                        else
                        {
                            if (fgCommand.Rows[i]["IP"] != null)
                            {
                                m_IPList.Append("," + fgCommand.Rows[i]["IP"].ToString());
                            }
                            
                        }
                    }
                    m_IPString = m_IPList.ToString();

                    m_IPList.Remove(0, m_IPList.Length);
                    m_IPList = null;

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }

                if (aButtonType == E_ButtonType.Cancel)
                {
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message.ToString());
            }

            
        }

        private void fgCommand_KeyDown(object sender, KeyEventArgs e)
        {
            AppGlobal.GridClipBoard(fgCommand, e.Control, e.KeyCode, false);
        }

    }
}