using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using MKLibrary.Controls;

namespace RACTClient
{
    public partial class FontFrom : BaseForm
    {
        public FontFrom()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 컨트롤을 초기화 합니다.
        /// </summary>
        public void initializeControl()
        {
            AddButton(E_ButtonType.Close, E_ButtonSide.Right, "취소");
            AddButton(E_ButtonType.OK, E_ButtonSide.Right, "확인");

            for (int i = 0; i < AppGlobal.s_FontList.Length; i++)
            {
                lstFont.Items.Add(AppGlobal.s_FontList[i]);
                lstFont.Items[lstFont.Items.Count - 1].Tag = AppGlobal.s_FontList[i];
                if (AppGlobal.s_ClientOption.TerminalFontName.Equals(AppGlobal.s_FontList[i]))
                {
                    lstFont.SelectedIndex = lstFont.Items.Count - 1;
                }
            }
            

            

            MKListItem tItem = new MKListItem("보통");
            tItem.Tag = FontStyle.Regular;
            lstStyle.Items.Add(tItem);
            

            tItem = new MKListItem("기울임꼴");
            tItem.Tag = FontStyle.Italic;
            lstStyle.Items.Add(tItem);

            tItem = new MKListItem("굵게");
            tItem.Tag = FontStyle.Bold;
            lstStyle.Items.Add(tItem);


            for (int i = 0; i < lstStyle.Items.Count; i++)
            {
                if ((FontStyle)lstStyle.Items[i].Tag == AppGlobal.s_ClientOption.TerminalFontStyle)
                {
                    lstStyle.SelectedIndex = i;
                    break;
                }
            }


            for (int i = 8; i < 20; i++)
            {
                lstSize.Items.Add(i.ToString());
                lstSize.Items[lstSize.Items.Count - 1].Tag = i;
                if ((int)AppGlobal.s_ClientOption.TerminalFontSize == i)
                {
                    lstSize.SelectedIndex = lstSize.Items.Count - 1;
                }
            }
        }

        private void lstFont_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFont.Text = lstFont.Items[lstFont.SelectedIndex].Text;
        }

        private void lstStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtStyle.Text = lstStyle.Items[lstStyle.SelectedIndex].Text;
        }

        private void lstSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSize.Text = lstSize.Items[lstSize.SelectedIndex].Text;
        }
        private bool CheckFont()
        {
            bool tIsExistFont = false;
            for (int i = 0; i < AppGlobal.s_FontList.Length; i++)
            {
                if (txtFont.Text.Equals(AppGlobal.s_FontList[i]))
                {
                    tIsExistFont = true;
                    break;
                }
            }


            bool tIsExistStyle = false;
            for (int i = 0; i < lstStyle.Items.Count; i++)
            {
                if (txtStyle.Text.Equals(lstStyle.Items[i].Text))
                {
                    tIsExistStyle = true;
                    break;
                }
            }


            bool tIsExistSize = false;

            for (int i = 0; i < lstSize.Items.Count; i++)
            {
                if (txtSize.Text.Equals(lstSize.Items[i].Text))
                {
                    tIsExistSize = true;
                    break;
                }
            }

            return tIsExistFont || tIsExistSize || tIsExistStyle;
        }

        /// <summary>
        /// 화면의 Button을 클릭한 이벤트를 처리하는 가상 함수입니다.
        /// </summary>
        /// <param name="aButtonType">버튼 타입</param>
        protected override void ButtonProcess(E_ButtonType aButtonType)
        {
            switch (aButtonType)
            {
                case E_ButtonType.OK:
                    if (!CheckFont())
                    {
                        AppGlobal.ShowMessageBox(this, "글꼴을 확인 하세요.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    this.Font = new Font(lstFont.Items[lstFont.SelectedIndex].Tag.ToString(), (int)lstSize.Items[lstSize.SelectedIndex].Tag, (FontStyle)lstStyle.Items[lstStyle.SelectedIndex].Tag);
                    DialogResult = DialogResult.OK;
                    break;
                case E_ButtonType.Close:
                    DialogResult = DialogResult.Cancel;
                    Close();
                    break;
            }
        }
    }
}