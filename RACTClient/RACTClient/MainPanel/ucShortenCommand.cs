using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using RACTCommonClass;
using C1.Win.C1FlexGrid;
using DevComponents.DotNetBar;

namespace RACTClient
{
    
    public partial class ucShortenCommand : UserControl,IMainPanel
    {
        /// <summary>
        /// 단축 명령 전송 이벤트 입니다.
        /// </summary>
        public static event HandlerArgument1<ShortenCommandInfo> OnSendShortenCommand;

        public ucShortenCommand()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 컨트롤을 초기화 합니다.
        /// </summary>
        public void InitializeControl()
        {
            AppGlobal.InitializeButtonStyle(btnSetting);
            AppGlobal.s_DataSyncProcssor.OnShortenCommandGroupInfoChangeEvent += new HandlerArgument2<ShortenCommandGroupInfo, E_WorkType>(s_DataSyncProcssor_OnShortenCommandGroupInfoChangeEvent);
            AppGlobal.s_DataSyncProcssor.OnShortenCommandInfoChangeEvent += new HandlerArgument2<ShortenCommandInfo, E_WorkType>(s_DataSyncProcssor_OnShortenCommandInfoChangeEvent);

            DisplayShortenCommand();
        }

        private void DisplayShortenCommand()
        {
            ButtonItem tGroupButton;
            ButtonItem tCommandButton;
            itemPanel1.Items.Clear();
            foreach (ShortenCommandGroupInfo tGroupInfo in AppGlobal.s_ShortenCommandList)
            {
               tGroupButton = ProcessCommandGroup(tGroupInfo, E_WorkType.Add);
                
                foreach (ShortenCommandInfo tCommand in tGroupInfo.ShortenCommandList)
                {
                    ProcessCommmand(tCommand, E_WorkType.Add, tGroupButton);
                }
            }
        }

        void tCommandButton_Click(object sender, EventArgs e)
        {
            if (OnSendShortenCommand != null) OnSendShortenCommand((ShortenCommandInfo)((ButtonItem)sender).Tag);
        }

        void s_DataSyncProcssor_OnShortenCommandInfoChangeEvent(ShortenCommandInfo aValue1, E_WorkType aValue2)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument2<ShortenCommandInfo, E_WorkType>(s_DataSyncProcssor_OnShortenCommandInfoChangeEvent), new object[] { aValue1, aValue2 });
                return;
            }

            for (int i = 0; i < itemPanel1.Items.Count; i++)
            {
                if (((ShortenCommandGroupInfo)itemPanel1.Items[i].Tag).ID == aValue1.GroupID)
                {
                    ProcessCommmand(aValue1, aValue2,(ButtonItem)itemPanel1.Items[i]);
                    break;
                }
            }
         
        }

        void s_DataSyncProcssor_OnShortenCommandGroupInfoChangeEvent(ShortenCommandGroupInfo aValue1, E_WorkType aValue2)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument2<ShortenCommandGroupInfo, E_WorkType>(s_DataSyncProcssor_OnShortenCommandGroupInfoChangeEvent), new object[] { aValue1, aValue2 });
                return;
            }
            ProcessCommandGroup(aValue1, aValue2);
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            ShortenCommandList tCommandForm = new ShortenCommandList();
            tCommandForm.InitializeControl();
            tCommandForm.ShowDialog(this);
        }

        private void ProcessCommmand(ShortenCommandInfo aCommand, E_WorkType aWorkType,ButtonItem aParentButton)
        {
            ButtonItem tCommandButton;
            if (aWorkType == E_WorkType.Add)
            {
                tCommandButton = new ButtonItem();
                tCommandButton.Name = aCommand.Name;
                tCommandButton.Text = aCommand.Name;
                tCommandButton.Tooltip = aCommand.Description;
                tCommandButton.Click += new EventHandler(tCommandButton_Click);
                tCommandButton.Tag = aCommand;

                aParentButton.SubItems.AddRange(new BaseItem[] { tCommandButton });
            }
            else
            {
                for (int i = 0; i < aParentButton.SubItems.Count; i++)
                {
                    if (aWorkType == E_WorkType.Modify)
                    {
                        tCommandButton =(ButtonItem)aParentButton.SubItems[i];
						//2020-10-05 TACT기능개선 단축명령기능 오류 수정
                        if (((ShortenCommandInfo)tCommandButton.Tag).ID == aCommand.ID)
                        {
                            tCommandButton.Name = aCommand.Name;
                            tCommandButton.Text = aCommand.Name;
                            tCommandButton.Tooltip = aCommand.Description;
                            tCommandButton.Click += new EventHandler(tCommandButton_Click);
                            tCommandButton.Tag = aCommand;
                        }
                    }
                    else
                    {
						//2020-10-05 TACT기능개선 단축명령기능 오류 수정
                        tCommandButton =(ButtonItem)aParentButton.SubItems[i];
                        if (((ShortenCommandInfo)tCommandButton.Tag).ID == aCommand.ID)
                        {
                            aParentButton.SubItems.RemoveAt(i);
                        }
                    }
                }
            }
            ProcessCommandGroup(AppGlobal.s_ShortenCommandList[((ShortenCommandGroupInfo)aParentButton.Tag).ID],E_WorkType.Modify);
        }
        private ButtonItem ProcessCommandGroup(ShortenCommandGroupInfo aGroupInfo, E_WorkType aWorkType)
        {
            ButtonItem tGroupButton = null;
            switch (aWorkType)
            {
                case E_WorkType.Add:
                    tGroupButton = new ButtonItem();
                    tGroupButton.Name = aGroupInfo.Name + "(" + aGroupInfo.ShortenCommandList.Count + ")";
                    tGroupButton.Text = aGroupInfo.Name + "(" + aGroupInfo.ShortenCommandList.Count + ")";
                    tGroupButton.Tooltip = aGroupInfo.Description;
                    tGroupButton.Tag = aGroupInfo;
                    this.itemPanel1.Items.AddRange(new BaseItem[] { tGroupButton });
                    break;

                default:
                    for (int i = 0; i < itemPanel1.Items.Count; i++)
                    {
                        if (((ShortenCommandGroupInfo)itemPanel1.Items[i].Tag).ID == aGroupInfo.ID)
                        {
                            if (aWorkType == E_WorkType.Modify)
                            {
                                tGroupButton = (ButtonItem)itemPanel1.Items[i];
                                tGroupButton.Name = aGroupInfo.Name + "(" + aGroupInfo.ShortenCommandList.Count + ")";
                                tGroupButton.Text = aGroupInfo.Name + "(" + aGroupInfo.ShortenCommandList.Count + ")";
                                tGroupButton.Tooltip = aGroupInfo.Description;
                                tGroupButton.Tag = aGroupInfo;
                            }
                            else
                            {
                                itemPanel1.Items.RemoveAt(i);
                            }
                            break;
                        }
                    }
                    break;
            }

            itemPanel1.Invalidate();

            return tGroupButton;
        }

     
    }
}
