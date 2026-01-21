using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RACTCommonClass;
using C1.Win.C1FlexGrid;

namespace RACTClient
{
    public partial class ViewReservedString : BaseForm
    {
        public ViewReservedString()
        {
            InitializeComponent();
        }

        public void InitializeControl()
        {
            AddButton(E_ButtonType.Close, E_ButtonSide.Right, "닫기");
            AppGlobal.InitializeGridStyle(fgReservedString);

            DisplayReservedCommand();
        }

        /// <summary>
        /// 설명을 표시합니다.
        /// </summary>
        private void DisplayReservedCommand()
        {
            List<ReservedString> tCommandList = TelnetReservedString.GetTelnetReservedString();

            fgReservedString.Rows.Count = tCommandList.Count + 1;
            int tIndex = 1;

            foreach (ReservedString tCommandInfo in tCommandList)
            {
                fgReservedString[tIndex, 0] = tIndex;
                fgReservedString[tIndex, 1] = tCommandInfo.Command;
                fgReservedString[tIndex, 2] = tCommandInfo.Description;
                fgReservedString.Rows[tIndex].Height = 20;

                tIndex++;
            }

            fgReservedString.AutoSizeCol(1);
        }

        private void fgReservedString_KeyDown(object sender, KeyEventArgs e)
        {
            GridClipBoard(fgReservedString, e.Control, e.KeyCode);
        }

        private void fgReservedString_MouseDown(object sender, MouseEventArgs e)
        {
            HitTestInfo tHitTest = fgReservedString.HitTest(e.X, e.Y);

            if (tHitTest.Row < 1 || tHitTest.Column != 1)
            {
                mnuCopy.Enabled = false;
                return;
            }
            else
            {
                mnuCopy.Enabled = true;
            }
            if (e.Button == MouseButtons.Right)
            {
                fgReservedString.Select(tHitTest.Row, tHitTest.Column);
                ctmPopup.Popup(MousePosition);
            }
        }

        /// <summary>
        /// 그리드 내용을 클립 보드에 복사하거나 클립 보드의 내용을 그리드에 복사 합니다.
        /// </summary>
        public void GridClipBoard(C1FlexGrid vGrid, bool isControl, Keys vKey)
        {
            if (!isControl) return;

            try
            {
                switch (vKey)
                {
                    case Keys.C:
                        string tClipString = "";
                        if (vGrid.RowSel > 0)
                        {
                            if (vGrid.ColSel == 1)
                            {
                                Clipboard.SetDataObject(vGrid[vGrid.RowSel, vGrid.ColSel].ToString());
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        private void mnuCopy_Click(object sender, EventArgs e)
        {
            GridClipBoard(fgReservedString, true, Keys.C);
        }


        protected override void ButtonProcess(E_ButtonType aButtonType)
        {
            this.Close();
        }
    }
}