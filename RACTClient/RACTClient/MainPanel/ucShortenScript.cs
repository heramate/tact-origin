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

namespace RACTClient
{
    public partial class ucShortenScript : UserControl,IMainPanel
    {
         /// <summary>
        /// 단축 명령 전송 이벤트 입니다.
        /// </summary>
        public static event HandlerArgument1<Script> OnSendScript;

        public ucShortenScript()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 컨트롤을 초기화 합니다.
        /// </summary>
        public void InitializeControl()
        {
            AppGlobal.InitializeButtonStyle(btnSetting);
            AppGlobal.s_DataSyncProcssor.OnScriptGroupInfoChangeEvent += new HandlerArgument2<ScriptGroupInfo, E_WorkType>(s_DataSyncProcssor_OnScriptGroupInfoChangeEvent);
            AppGlobal.s_DataSyncProcssor.OnScriptChangeEvent += new HandlerArgument2<Script, E_WorkType>(s_DataSyncProcssor_OnScriptChangeEvent);
            DisplayScript();
        }

        void s_DataSyncProcssor_OnScriptChangeEvent(Script aValue1, E_WorkType aValue2)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument2<Script, E_WorkType>(s_DataSyncProcssor_OnScriptChangeEvent), new object[] { aValue1, aValue2 });
                return;
            }

            for (int i = 0; i < itemPanel1.Items.Count; i++)
            {
                if (((ScriptGroupInfo)itemPanel1.Items[i].Tag).ID == aValue1.GroupID)
                {
                    ProcessCommmand(aValue1, aValue2, (ButtonItem)itemPanel1.Items[i]);
                    break;
                }
            }
        }

        void s_DataSyncProcssor_OnScriptGroupInfoChangeEvent(ScriptGroupInfo aValue1, E_WorkType aValue2)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument2<ScriptGroupInfo, E_WorkType>(s_DataSyncProcssor_OnScriptGroupInfoChangeEvent), new object[] { aValue1, aValue2 });
                return;
            }
            ProcessScriptGroup(aValue1, aValue2);
        }

        private void DisplayScript()
        {
            ButtonItem tGroupButton;
            ButtonItem tCommandButton;
            itemPanel1.Items.Clear();
            foreach (ScriptGroupInfo tGroupInfo in AppGlobal.s_ScriptList.InnerList)
            {
               tGroupButton = ProcessScriptGroup(tGroupInfo, E_WorkType.Add);
                
                foreach (Script tCommand in tGroupInfo.ScriptList)
                {
                    ProcessCommmand(tCommand, E_WorkType.Add, tGroupButton);
                }
            }
        }

        void tCommandButton_Click(object sender, EventArgs e)
        {
            if (OnSendScript != null) OnSendScript((Script)((ButtonItem)sender).Tag);
        }

 
        private void btnSetting_Click(object sender, EventArgs e)
        {
            ScriptList tCommandForm = new ScriptList();
            tCommandForm.InitializeControl();
            tCommandForm.ShowDialog(this);
        }

        private void ProcessCommmand(Script aCommand, E_WorkType aWorkType,ButtonItem aParentButton)
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
						//2020-10-05 TACT기능개선 스크립트명령기능 오류 수정
                        if (((Script)tCommandButton.Tag).ID == aCommand.ID)
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
						//2020-10-05 TACT기능개선 스크립트명령기능 오류 수정
                        tCommandButton =(ButtonItem)aParentButton.SubItems[i];
                        if (((Script)tCommandButton.Tag).ID == aCommand.ID)
                        {
                            aParentButton.SubItems.RemoveAt(i);
                        }
                    }
                }
            }
            ProcessScriptGroup(AppGlobal.s_ScriptList[((ScriptGroupInfo)aParentButton.Tag).ID], E_WorkType.Modify);
        }
        private ButtonItem ProcessScriptGroup(ScriptGroupInfo aGroupInfo, E_WorkType aWorkType)
        {
            ButtonItem tGroupButton = null;
            switch (aWorkType)
            {
                case E_WorkType.Add:
                    tGroupButton = new ButtonItem();
                    tGroupButton.Name = aGroupInfo.Name + "(" + aGroupInfo.ScriptList.Count + ")";
                    tGroupButton.Text = aGroupInfo.Name + "(" + aGroupInfo.ScriptList.Count + ")";
                    tGroupButton.Tooltip = aGroupInfo.Description;
                    tGroupButton.Tag = aGroupInfo;
                    this.itemPanel1.Items.AddRange(new BaseItem[] { tGroupButton });
                    break;

                default:
                    for (int i = 0; i < itemPanel1.Items.Count; i++)
                    {
                        if (((ScriptGroupInfo)itemPanel1.Items[i].Tag).ID == aGroupInfo.ID)
                        {
                            if (aWorkType == E_WorkType.Modify)
                            {
                                tGroupButton = (ButtonItem)itemPanel1.Items[i];
                                tGroupButton.Name = aGroupInfo.Name + "(" + aGroupInfo.ScriptList.Count + ")";
                                tGroupButton.Text = aGroupInfo.Name + "(" + aGroupInfo.ScriptList.Count + ")";
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
