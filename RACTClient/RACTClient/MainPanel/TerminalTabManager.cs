using System;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using RACTClient.Models;
using RACTTerminal;
using RACTCommonClass;

namespace RACTClient
{
    /// <summary>
    /// SuperTabControl을 사용한 터미널 탭 UI 관리를 담당하는 클래스입니다.
    /// </summary>
    public class TerminalTabManager
    {
        private readonly SuperTabControl _tabControl;

        public TerminalTabManager(SuperTabControl tabControl)
        {
            _tabControl = tabControl ?? throw new ArgumentNullException(nameof(tabControl));
        }

        /// <summary>
        /// 터미널을 위한 새로운 탭을 생성하고 추가합니다.
        /// </summary>
        public SuperTabItem AddTerminalTab(ITactTerminal terminal, int duplicateCount, MouseEventHandler tabMouseUpHandler)
        {
            var tabPanel = new SuperTabControlPanel();
            var tabItem = new SuperTabItem();

            if (tabMouseUpHandler != null)
            {
                tabItem.MouseUp += tabMouseUpHandler;
            }

            _tabControl.Controls.Add(tabPanel);
            tabPanel.Controls.Add(terminal.UIControl);
            tabItem.AttachedControl = tabPanel;
            tabItem.GlobalItem = false;

            string tabName = GenerateTabName(terminal, duplicateCount);
            tabItem.Name = tabName;
            tabItem.Text = tabName;
            
            terminal.DeviceInfo.TerminalName = tabName;
            terminal.UIControl.Name = tabName;
            
            tabItem.Image = (Image)global::RACTClient.Properties.Resources.TryConnect;
            tabItem.Tag = terminal.DeviceInfo;
            tabItem.Tooltip = terminal.ToolTip;
            
            tabPanel.Dock = DockStyle.Fill;
            tabPanel.TabItem = tabItem;

            _tabControl.Tabs.Add(tabItem);
            _tabControl.SelectedTab = tabItem;

            return tabItem;
        }

        /// <summary>
        /// 탭 이름을 기반으로 표시용 명칭을 생성합니다.
        /// </summary>
        private string GenerateTabName(ITactTerminal terminal, int count)
        {
            var info = terminal.DeviceInfo;
            string baseName;

            if (info.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SERIAL_PORT)
            {
                baseName = "Serial-" + info.TerminalConnectInfo.SerialConfig.PortName;
            }
            else
            {
                baseName = AppGlobal.s_ClientOption.TerminalDisplayNameType == E_TerminalDisplayNameType.IPAddress 
                    ? info.IPAddress 
                    : info.Name;
            }

            return count > 0 ? $"{baseName}({count})" : baseName;
        }

        /// <summary>
        /// 현재 선택된 탭에서 터미널 인스턴스를 추출합니다.
        /// </summary>
        public ITactTerminal GetActiveTerminal()
        {
            if (_tabControl.SelectedTabIndex < 0) return null;

            var panel = _tabControl.SelectedTab.AttachedControl as SuperTabControlPanel;
            if (panel == null || panel.Controls.Count == 0) return null;

            return panel.Controls[0] as ITactTerminal;
        }

        /// <summary>
        /// 특정 터미널이 포함된 탭을 활성화합니다.
        /// </summary>
        public void ActivateTerminalTab(ITactTerminal terminal)
        {
            if (terminal?.Parent is SuperTabControlPanel panel && panel.TabItem != null)
            {
                _tabControl.SelectedTab = panel.TabItem;
            }
        }

        /// <summary>
        /// 선택된 탭의 이름을 변경합니다.
        /// </summary>
        public void RenameSelectedTab(string newName)
        {
            if (_tabControl.SelectedTab != null)
            {
                _tabControl.SelectedTab.Text = newName;
            }
        }
    }
}
