using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;

namespace RACTClient
{
    public partial class SelectTargetTerminal : BaseForm
    {
        /// <summary>
        /// 터미널 목록 입니다.
        /// </summary>
        private List<ITactTerminal> m_TerminalList = null;
        /// <summary>
        /// 리스트 row 높이 조정용 이미지 리스트 입니다.
        /// </summary>
        private ImageList m_RowSpaceImageList = null;
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public SelectTargetTerminal()
        {
            InitializeComponent();
            initializeControl();
        }

        /// <summary>
        /// 컨트롤을 초기화 합니다.
        /// </summary>
        private void initializeControl()
        {
            AddButton(E_ButtonType.Close, E_ButtonSide.Right, "닫기");
            AddButton(E_ButtonType.OK, E_ButtonSide.Right, "확인");

            m_RowSpaceImageList = new ImageList();
            m_RowSpaceImageList.ColorDepth = ColorDepth.Depth32Bit;
            m_RowSpaceImageList.ImageSize = new Size(1, 24);
            m_RowSpaceImageList.Images.Add(new Bitmap(1, 24));
            lstTerminal.SmallImageList = m_RowSpaceImageList;
            lstTerminal.View = View.Details;
            lstTerminal.HeaderStyle = ColumnHeaderStyle.None;
            lstTerminal.Scrollable = true;
            lstTerminal.HideSelection = false;
            lstTerminal.FullRowSelect = true;
            lstTerminal.Columns.Add(string.Empty);
            lstTerminal.KeyDown += new KeyEventHandler(lstTerminal_KeyDown);
            lstTerminal.Resize += new EventHandler(lstTerminal_Resize);


        }

        private void SelectFirstTerminal()
        {
            if (lstTerminal.Items.Count == 0)
            {
                return;
            }

            lstTerminal.SelectedIndices.Clear();
            lstTerminal.Items[0].Selected = true;
            lstTerminal.Items[0].Focused = true;
            lstTerminal.EnsureVisible(0);
        }

        private void lstTerminal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            e.Handled = true;
            e.SuppressKeyPress = true;
            ButtonProcess(E_ButtonType.OK);
        }

        private void lstTerminal_Resize(object sender, EventArgs e)
        {
            AdjustListItemWidth();
        }

        private void AdjustListItemWidth()
        {
            if (!lstTerminal.IsHandleCreated)
            {
                return;
            }

            if (lstTerminal.Columns.Count == 0)
            {
                lstTerminal.Columns.Add(string.Empty);
            }

            int tItemWidth = Math.Max(200, lstTerminal.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 4);
            lstTerminal.Columns[0].Width = tItemWidth;
        }
        /// <summary>
        /// 터미널 목록을 가져오거나 설정 합니다.
        /// </summary>
        public List<ITactTerminal> TerminalList
        {
            get { return m_TerminalList; }
            set
            {
                m_TerminalList = value;

                lstTerminal.Items.Clear();

                ListViewItem tTempButton = null;
                if (m_TerminalList != null)
                {
                    foreach (ITactTerminal tEmulator in m_TerminalList)
                    {
                        if (tEmulator.IsConnected)
                        {
                            tTempButton = new ListViewItem();
                            tTempButton.Text = string.Concat(tEmulator.Name, " (", tEmulator.ToolTip, ")");
                            tTempButton.ImageIndex = 0;
                            tTempButton.Tag = tEmulator;
                            tTempButton.ToolTipText = tEmulator.ToolTip;
                            lstTerminal.Items.Add(tTempButton);
                        }
                    }
                }

                SelectFirstTerminal();
                AdjustListItemWidth();
            }
        }
        public ITactTerminal SelectedTerminal
        {
            get { return (ITactTerminal)lstTerminal.SelectedItems[0].Tag; }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            SelectFirstTerminal();
            AdjustListItemWidth();
            lstTerminal.Focus();
        }

        /// <summary>
        /// 버튼 처리를 합니다.
        /// </summary>
        /// <param name="aButtonType"></param>
        protected override void ButtonProcess(E_ButtonType aButtonType)
        {
            if (aButtonType == E_ButtonType.OK)
            {
                if (lstTerminal.SelectedItems.Count == 0)
                {
                    AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "대상 터미널을 선택 하세요.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DialogResult = DialogResult.OK;
            }

            Close();
        }
    }
}
