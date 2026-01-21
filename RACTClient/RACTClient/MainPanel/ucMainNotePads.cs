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


namespace RACTClient
{

    public delegate void NotedPadTabChangeHandler (E_NotePadStatus aStatus, string aNotePadName);

    /// <summary>
    /// 2013-01-17 - shinyn - 메모장 모음 컨트롤입니다.
    /// </summary>
    public partial class ucMainNotePads : SenderControl, IMainPanel
    {

        /// <summary>
        /// 활성화 중인 메모장 목록입니다.
        /// </summary>
        private List<ucNotePad> m_NotePadList = new List<ucNotePad>();

        public List<ucNotePad> NotePadList
        {
            get { return m_NotePadList; }
        }

        /// <summary>
        /// 현재 선택된 메모장입니다.
        /// </summary>
        private ucNotePad m_SelectedNotePad = null;


        /// <summary>
        /// 최근에 닫히 메모장입니다.
        /// </summary>
        private ucNotePad m_CloseNotePad = null;

        public event NotedPadTabChangeHandler OnNotePadTabChangeEvent;

        public ucMainNotePads()
        {
            InitializeComponent();
        }

        public void InitializeControl()
        {

        }

        public void NewNotePad()
        {
            ucNotePad tNotePad = MakeNotePad();
            tNotePad.NotePadStatus = E_NotePadStatus.New;
            bool bSuccess = AddNotePad(tNotePad, "");    
        }

        public void OpenNotePad(string aFilePath)
        {
            ucNotePad tNotePad = MakeNotePad();
            tNotePad.NotePadStatus = E_NotePadStatus.Open;
            bool bSuccess = AddNotePad(tNotePad, aFilePath);
            if(bSuccess) tNotePad.OpenFile(aFilePath);       
        }


        internal bool AddNotePad(ucNotePad aNotePad, string aFilePath)
        {
            int tCount = 0;
            

            if (AppGlobal.s_ClientOption.NotePadWindowsPopupType == E_DefaultNotePadPopupType.Tab)
            {
                if (!CheckNotePadCount(aNotePad, out tCount)) return false;

                SuperTabItem tTabItem = MakeTabPanel(aNotePad, tCount, aFilePath);
                tabNotePads.Tabs.AddRange(new BaseItem[] { tTabItem });
                tabNotePads.ReorderTabsEnabled = true;
                tabNotePads.SelectedTab = tTabItem;
            }
            else
            {
                NotePadWindows tForm = new NotePadWindows();

                aNotePad.Dock = DockStyle.Fill;
                tForm.Controls.Add(aNotePad);
                tForm.Size = new Size(AppGlobal.s_ClientOption.PopupSizeWidth, AppGlobal.s_ClientOption.PopupSizeHeight);

                if (aNotePad.FilePath != "")
                {
                    string[] tFileName = aNotePad.FilePath.Split('\\');
                    tForm.Text = string.Format("{0} - 메모장", tFileName[tFileName.GetLength(0) - 1]);
                }
                else
                {
                    tForm.Text = "제목없음 - 메모장";
                }

                tForm.MaximizeBox = false;
                tForm.BringToFront();
                tForm.Show();
            }

            

            return true;
        }

        private SuperTabItem MakeTabPanel(ucNotePad aUcNotePad, int aCount, string aFileParth)
        {
            SuperTabControlPanel tTabPanel;
            SuperTabItem tTabItem;

            tTabItem = new SuperTabItem();
            tTabPanel = new SuperTabControlPanel();
            tTabItem.MouseUp += new MouseEventHandler(TabItem_MouseUp);
            tTabItem.AttachedControl = tTabPanel;
            
            tabNotePads.Controls.Add(tTabPanel);
            tTabPanel.Controls.Add(aUcNotePad);

            tTabItem.AttachedControl = tTabPanel;
            tTabItem.GlobalItem = false;

            if(aFileParth != "")
            {
                string[] aSplitFileName = aFileParth.Split('\\');
                tTabItem.Name = string.Format("{0} - 메모장", aSplitFileName[aSplitFileName.GetLength(0) - 1]);
            }
            else
            {
                tTabItem.Name = string.Format("제목없음{0} - 메모장", m_NotePadList.Count);
            }

            tTabItem.Text = tTabItem.Name;
            aUcNotePad.Name = tTabItem.Name;
            tTabPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            tTabPanel.Location = new System.Drawing.Point(0, 26);
            tTabPanel.Name = "superTabControlPanel1";
            tTabPanel.Size = new System.Drawing.Size(150, 124);
            tTabPanel.TabIndex = 1;
            tTabPanel.TabItem = tTabItem;

            aUcNotePad.Name = tTabItem.Text;

            return tTabItem;
        }

        void TabItem_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                SuperTabItem tBaseItem = (SuperTabItem)sender;
                SuperTabControlPanel tPanel = (SuperTabControlPanel)tBaseItem.AttachedControl;
                ucNotePad tNotePad = (ucNotePad)tPanel.Controls[0];
                m_SelectedNotePad = tNotePad;
                ShowContextMenu(cmTabPopup);
            }
        }

        private bool CheckNotePadCount(ucNotePad aNotePad, out int tCount)
        {
            tCount = 0;



            if (tabNotePads.Controls.Count > 5)
            {
                AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm, "최대 노트패드 개수는 5개 입니다.", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            tCount = m_NotePadList.Count;

            return true;
        }

        public ucNotePad MakeNotePad()
        {
            ucNotePad tNotePad = new ucNotePad();
            tNotePad.Dock = DockStyle.Fill;
            m_NotePadList.Add(tNotePad);

            return tNotePad;
        }

        


        private void ShowContextMenu(ButtonItem aPopup)
        {
            aPopup.Popup(MousePosition);
        }

       
        /// <summary>
        /// 현재 노트패드만 남기고 모두 닫기 기능
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuCloseOther_Click(object sender, EventArgs e)
        {
            if (m_SelectedNotePad == null) return;

            for (int i = tabNotePads.Tabs.Count - 1; i > -1; i--)
            {
                if (((ucNotePad)((SuperTabControlPanel)((SuperTabItem)tabNotePads.Tabs[i]).AttachedControl).Controls[0]) != m_SelectedNotePad)
                {
                    ((SuperTabItem)tabNotePads.Tabs[i]).Close();
                }
            }
            this.Invalidate();
        }

        /// <summary>
        /// 탭 분리하는 기능
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuChangeStatus_Click(object sender, EventArgs e)
        {
            if (m_SelectedNotePad == null) return;

            NotePadWindows tForm = new NotePadWindows();

            m_SelectedNotePad.Dock = DockStyle.Fill;
            tForm.Controls.Add(m_SelectedNotePad);
            tForm.Size = new Size(AppGlobal.s_ClientOption.PopupSizeWidth, AppGlobal.s_ClientOption.PopupSizeHeight);

            if(m_SelectedNotePad.FilePath != "")
            {
                string[] tFileName = m_SelectedNotePad.FilePath.Split('\\');
                tForm.Text = string.Format("{0} - 메모장", tFileName[tFileName.GetLength(0)-1]);
            }
            else
            {
                tForm.Text = "제목없음 - 메모장";
            }

            
            tabNotePads.SelectedTab.Close();

            m_NotePadList.Remove(m_SelectedNotePad);
            tForm.MaximizeBox = false;
            tForm.BringToFront();
            tForm.Show();
        }

        private void tabNotePads_TabItemClose(object sender, SuperTabStripTabItemCloseEventArgs e)
        {
            if (((SuperTabItem)(e.Tab)).AttachedControl.Controls.Count == 0) return;

            m_CloseNotePad = ((SuperTabItem)e.Tab).AttachedControl.Controls[0] as ucNotePad;

            m_NotePadList.Remove(m_CloseNotePad);
        }

        private void tabNotePads_SelectedTabChanged(object sender, SuperTabStripSelectedTabChangedEventArgs e)
        {
            if (tabNotePads.SelectedTabIndex < 0)
            {
                m_SelectedNotePad = null;
                return;
            }

            if (((SuperTabControlPanel)tabNotePads.SelectedTab.AttachedControl).Controls.Count == 0) return;

            ((SuperTabControlPanel)tabNotePads.SelectedTab.AttachedControl).Controls[0].Focus();
            ucNotePad tNotePad = (ucNotePad)((SuperTabControlPanel)tabNotePads.SelectedTab.AttachedControl).Controls[0];
            m_SelectedNotePad = tNotePad;
        }
    }
}
