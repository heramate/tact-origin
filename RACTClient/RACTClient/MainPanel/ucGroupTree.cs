using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using RACTCommonClass;
using System.Threading;

namespace RACTClient
{
    public partial class ucGroupTree : SenderControl, IMainPanel
    {
        /// <summary>
        ///  장비 연결 이벤트 입니다.
        /// </summary>
        public event ConnectDeviceHandler OnConnectDeviceEvent;
        /// <summary>
        /// 그룹 수정 이벤트 입니다.
        /// </summary>
        public event ModifyGroupHandler OnModifyGroupEvent;
        /// <summary>
        /// 장비 수정 이벤트 입니다.
        /// </summary>
        public event ModifyDeviceHandler OnModifyDeviceEvent;


        /// <summary>
        /// 2013-01-18 - shinyn - 수동장비 수정 이벤트 입니다.
        /// </summary>
        public event ModifyUsrDeviceHandler OnModifyUsrDeviceEvent;

        /// <summary>
        /// 그룹 장비 연결 이벤트 입니다.
        /// </summary>
        public event ConnectGroupDevice OnConnectGroupDeviceEvent;

        /// <summary>
        /// 2013-08-14 - shinyn - 사용자 장비 공유 이벤트 입니다.
        /// </summary>
        public event ShareDeviceHandler OnShareDeviceEvent;

        /// <summary>
        /// 2013-08-14 - shinyn - 선택한 그룹의 사용자 장비 목록을 추가하는 이벤트입니다.
        /// </summary>
        public event AddShareDeviceHandler OnAddShareDeviceEvent;

        /// <summary>
        /// 2013-09-09 - shinyn - 선택한 사용자 정보를 표시하기위해 사용자 정보를 추가하는 이벤트입니다.
        /// </summary>
        public event SelectUserInfoHandler OnSelectUserInfoEvent;



        /// <summary>
        /// 트리 타입 입니다.
        /// </summary>
        private E_TreeType m_TreeType = E_TreeType.SystemGroup;


        /// <summary>
        /// 2013-05-02 - shinyn - 선택된 장비정보입니다.
        /// </summary>
        private DeviceInfo m_SelectedDeviceInfo = null;

        /// <summary>
        /// 2013-08-13- shinyn - 트리에서 제외할 그룹 정보입니다.
        /// </summary>
        private GroupInfo m_DeleteGroupInfo = null;

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public ucGroupTree()
        {
            InitializeComponent();
            imageList1.Images.Add((Image)global::RACTClient.Properties.Resources.그룹);
            imageList1.Images.Add((Image)global::RACTClient.Properties.Resources.장비);
        }

        public E_TreeType TreeType
        {
            get { return m_TreeType; }
            set { m_TreeType = value; }
        }

        /// <summary>
        /// 그룹 정보 변경 이벤트 처리 합니다.
        /// </summary>
        /// <param name="aValue1"></param>
        /// <param name="aValue2"></param>
        void OnGroupInfoChangeEvent_Process(GroupInfo aValue1, E_WorkType aValue2)
        {
            RefreshTree();
        }

        private void treeViewEx1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // 2013-05-02- shinyn - 접근권한 그룹인 경우 선택된 노드정보를 매핑합니다.
            if (m_TreeType == E_TreeType.SystemGroup)
            {
                if (treeViewEx1.SelectedNode == null)
                {
                    // 2013-05-03- shinyn - 선택된 노드 정보를 매핑합니다.
                    AppGlobal.m_SelectedSystemNode = null;
                }
                else
                {
                    // 2013-05-03- shinyn - 선택된 노드 정보를 매핑합니다.
                    AppGlobal.m_SelectedSystemNode = (TreeNodeEx)treeViewEx1.SelectedNode;
                }
                return;
            }

            //2013-09-09 -shinyn - 선택된 사용자 노드를 반환합니다.
            if (m_TreeType == E_TreeType.DisplayUserGroupList)
            {
                if (treeViewEx1.SelectedNode == null)
                {
                    AppGlobal.m_SelectedUserNode = null;
                }
                else
                {
                    AppGlobal.m_SelectedUserNode = (TreeNodeEx)treeViewEx1.SelectedNode;

                    UserInfo tUserInfo = (UserInfo)treeViewEx1.SelectedNode.Tag;

                    if (OnSelectUserInfoEvent != null) OnSelectUserInfoEvent(tUserInfo);
                }
            }

        }

        private void treeViewEx1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            // 2013-08-13- shinyn - 사용자그룹 조회인 경우에는 이벤트 발생안하도록 처리
            if (m_TreeType == E_TreeType.DisplayUserGroup) return;


            // 2013-08-14- shinyn - 사용자 공유 장비리스트를 조회하기 위해 더블클릭
            if (m_TreeType == E_TreeType.UserGroupList)
            {
                TreeNodeEx tSelectNode = (TreeNodeEx)((TreeViewEx)sender).SelectedNode;

                if (tSelectNode != null && tSelectNode.Tag is GroupInfo)
                {
                    GroupInfo tGroupInfo = (GroupInfo)tSelectNode.Tag;

                    if (OnAddShareDeviceEvent != null) OnAddShareDeviceEvent(tGroupInfo.DeviceList);
                }

                return;
            }

            if (((TreeViewEx)sender).SelectedNode != null && ((TreeViewEx)sender).SelectedNode.Tag is DeviceInfo)
            {
                mnuConnectDevice_Click(null, null);
            }
        }

        private void treeViewEx1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            if (m_TreeType == E_TreeType.SystemGroup) return;

            // 2013-08-13- shinyn - 사용자그룹 조회인 경우에는 이벤트 발생안하도록 처리
            if (m_TreeType == E_TreeType.DisplayUserGroup) return;
            if (m_TreeType == E_TreeType.DisplayUserGroupList) return;
            if (m_TreeType == E_TreeType.UserGroupList) return;

            mnuTL1Connect.Visible = false;

            if (treeViewEx1.SelectedNode == null)
            {
                mnuAddDevice.Visible = false;
                mnuAddUsrDevice.Visible = false;
                mnuAddGroup.Visible = true;
                mnuConnectGroupDevice.Visible = false;
                mnuDeleteGroup.Visible = false;
                mnuModifyGroup.Visible = false;

                // 2013-08-14-shinyn - 사용자 장비 공유 추가
                mnuShareDevice.Visible = false;

                mnuConnectDevice.Visible = false;
                mnuProperiesDevice.Visible = false;
                mnuDeleteDevice.Visible = false;
            }
            else
            {
                TreeNodeEx tSelectNode = (TreeNodeEx)treeViewEx1.SelectedNode;

                if (tSelectNode.Tag is GroupInfo)
                {
                    mnuAddDevice.Visible = true;
                    // 2013-01-18 - shinyn - 수동장비등록 추가
                    mnuAddUsrDevice.Visible = true;

                    mnuAddGroup.Visible = true;
                    mnuConnectGroupDevice.Visible = true;
                    mnuDeleteGroup.Visible = true;
                    mnuModifyGroup.Visible = true;
                    // 2013-08-14- shinyn - 사용자 장비 공유 추가
                    mnuShareDevice.Visible = true;

                    mnuConnectDevice.Visible = false;
                    mnuProperiesDevice.Visible = false;
                    mnuDeleteDevice.Visible = false;
                }
                else
                {
                    mnuAddDevice.Visible = false;
                    // 2013-01-18 - shinyn - 수동장비등록 추가
                    mnuAddUsrDevice.Visible = false;
                    mnuAddGroup.Visible = false;
                    mnuConnectGroupDevice.Visible = false;
                    mnuDeleteGroup.Visible = false;
                    mnuModifyGroup.Visible = false;

                    // 2013-08-14 - shinyn - 사용자 장비 공유 추가
                    mnuShareDevice.Visible = false;

                    mnuConnectDevice.Visible = true;
                    mnuProperiesDevice.Visible = true;
                    mnuDeleteDevice.Visible = true;

                    //2015-10-28 hanjiyeon TL1 접속 버튼 추가.
                    if (tSelectNode.Tag is DeviceInfo)
                    {
                        DeviceInfo tDI = tSelectNode.Tag as DeviceInfo;

                        if (AppGlobal.IsAlLuDevice(tDI.ModelID))                    
                        {
                            mnuTL1Connect.Visible = true;
                        }                        
                    }
                }
            }
            buttonItem1.Popup(MousePosition);
        }


        private void mnuConnectDevice_Click(object sender, EventArgs e)
        {
            if (treeViewEx1.SelectedNode == null) return;

            TreeNodeEx tSelectNode = (TreeNodeEx)treeViewEx1.SelectedNode;

            // 2013-05-02- shinyn - 접속오류를 해결하기 위해 데몬정보와 장비정보를 함께 보내서 로그인하도록 수정한다.
            // if (OnConnectDeviceEvent != null) OnConnectDeviceEvent((DeviceInfo)tSelectNode.Tag);
            m_SelectedDeviceInfo = (DeviceInfo)tSelectNode.Tag;

            StartSendThread(new ThreadStart(ConnectDevice));
        }
                
        /// <summary>
        /// TL1 연결 버튼 클릭 시의 처리입니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuTL1Connect_Click(object sender, EventArgs e)
        {
            if (treeViewEx1.SelectedNode == null) return;

            TreeNodeEx tSelectNode = (TreeNodeEx)treeViewEx1.SelectedNode;                        
            m_SelectedDeviceInfo = (DeviceInfo)tSelectNode.Tag;
            m_SelectedDeviceInfo.TerminalConnectInfo.TelnetPort = 1023;

            StartSendThread(new ThreadStart(ConnectDevice_TL1));
        }

        private void ConnectDevice_TL1()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DefaultHandler(ConnectDevice_TL1));
                return;
            }

            RequestCommunicationData tRequestData = null;

            tRequestData = AppGlobal.MakeDefaultRequestData();
            tRequestData.CommType = E_CommunicationType.RequestDaemonInfoList;
            tRequestData.RequestData = 1;

            m_Result = null;
            m_MRE.Reset();

            AppGlobal.SendRequestData(this, tRequestData);
            m_MRE.WaitOne(AppGlobal.s_RequestTimeOut * 10);
        }

        /// <summary>
        /// 2013-05-02- shinyn - 장비접속하기위한 결과값 받기
        /// </summary>
        private void ConnectDevice()
        {
            RequestCommunicationData tRequestData = AppGlobal.MakeDefaultRequestData();
            tRequestData.CommType = E_CommunicationType.RequestDaemonInfoList;
            tRequestData.RequestData = 1;

            AppGlobal.SendRequestData(this, tRequestData);
            m_MRE.WaitOne(AppGlobal.s_RequestTimeOut * 10);
        }

        /// <summary>
        /// 2013-05-02- shinyn - 장비접속하기위한 결과값 받기
        /// </summary>
        /// <param name="vResult"></param>
        public override void ResultReceiver(ResultCommunicationData vResult)
        {
            base.ResultReceiver(vResult);

            if (this.InvokeRequired)
            {
                this.Invoke(new HandlerArgument1<ResultCommunicationData>(ResultReceiver), vResult);
                return;
            }


            // 2013-05-02-shinyn - 결과값이 널인 경우 로그에 저장한다.
            if (m_Result == null || m_Result.ResultData == null)
            {
                this.Enabled = true;
                AppGlobal.ShowLoadingProgress(false);
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, " ucGroupTree : ResultReceive Error : Result null ");
                return;
            }

            object tResult = m_Result.ResultData;

            if (tResult.GetType().Equals(typeof(CompressData)))
            {
            }
            else
            {
                if (m_Result.ResultData.GetType().Equals(typeof(List<DaemonProcessInfo>)))
                {
                    List<DaemonProcessInfo> tDaemonList = m_Result.ResultData as List<DaemonProcessInfo>;

                    if (OnConnectDeviceEvent != null) OnConnectDeviceEvent(m_SelectedDeviceInfo, tDaemonList[0]);
                }
            }
        }

        /// <summary>
        /// 그룹 추가 메뉴 기능을 수행하는 함수입니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuAddGroup_Click(object sender, EventArgs e)
        {
            if (OnModifyGroupEvent != null)
            {
                if (OnModifyGroupEvent != null) OnModifyGroupEvent(E_WorkType.Add, null);
            }
        }

        /// <summary>
        /// 그룹 수정 메뉴 기능을 수행하는 함수입니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuModifyGroup_Click(object sender, EventArgs e)
        {
            if (treeViewEx1.SelectedNode == null) return;

            TreeNodeEx tSelectNode = (TreeNodeEx)treeViewEx1.SelectedNode;

            if (OnModifyGroupEvent != null) OnModifyGroupEvent(E_WorkType.Modify, (GroupInfo)tSelectNode.Tag);
        }

        /// <summary>
        /// 그룹 삭제 메뉴 기능을 수행하는 함수입니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuDeleteGroup_Click(object sender, EventArgs e)
        {
            if (treeViewEx1.SelectedNode == null) return;

            TreeNodeEx tSelectNode = (TreeNodeEx)treeViewEx1.SelectedNode;
            if (OnModifyGroupEvent != null) OnModifyGroupEvent(E_WorkType.Delete, (GroupInfo)tSelectNode.Tag);
        }

        /// <summary>
        /// 장비 추가 메뉴 기능을 수행하는 함수입니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuAddDevice_Click(object sender, EventArgs e)
        {
            if (treeViewEx1.SelectedNode == null) return;

            TreeNodeEx tSelectNode = (TreeNodeEx)treeViewEx1.SelectedNode;

            if (tSelectNode.Tag is GroupInfo)
            {
                if (OnModifyDeviceEvent != null)
                {
                    OnModifyDeviceEvent(E_WorkType.Add, tSelectNode.Tag);
                }
            }
        }

        /// <summary>
        /// 2013-01-18 - shinyn - 수동장비 등록 메뉴 기능을 수행하는 함수 추가
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuAddUsrDevice_Click(object sender, EventArgs e)
        {
            if (treeViewEx1.SelectedNode == null) return;

            TreeNodeEx tSelectNode = (TreeNodeEx)treeViewEx1.SelectedNode;

            if (tSelectNode.Tag is GroupInfo)
            {
                if (OnModifyUsrDeviceEvent != null)
                {
                    OnModifyUsrDeviceEvent(E_WorkType.Add, tSelectNode.Tag);
                }


            }
        }

        /// <summary>
        /// 장비 속성 메뉴 기능을 수행하는 함수입니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuPropertiesDevice_Click(object sender, EventArgs e)
        {
            if (treeViewEx1.SelectedNode == null) return;

            TreeNodeEx tSelectNode = (TreeNodeEx)treeViewEx1.SelectedNode;

            if (tSelectNode.Tag is DeviceInfo)
            {


                // 2013-01-18 - shinyn - 일반, 수동 장비 속성을 보여줍니다.
                DeviceInfo tDeviceInfo = (DeviceInfo)tSelectNode.Tag;

                switch (tDeviceInfo.DeviceType)
                {
                    case E_DeviceType.NeGroup:
                        if (OnModifyDeviceEvent != null)
                        {
                            OnModifyDeviceEvent(E_WorkType.Modify, tSelectNode.Tag);
                        }
                        break;
                    case E_DeviceType.UserNeGroup:
                        if (OnModifyUsrDeviceEvent != null)
                        {
                            OnModifyUsrDeviceEvent(E_WorkType.Modify, tSelectNode.Tag);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// 장비 삭제 기능을 수행하는 함수입니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuDeleteDevice_Click(object sender, EventArgs e)
        {
            if (treeViewEx1.SelectedNode == null) return;

            TreeNodeEx tSelectNode = (TreeNodeEx)treeViewEx1.SelectedNode;

            if (tSelectNode.Tag is DeviceInfo)
            {
                if (OnModifyDeviceEvent != null)
                {
                    OnModifyDeviceEvent(E_WorkType.Delete, tSelectNode.Tag);
                }
            }
        }

        /// <summary>
        /// 컨트롤을 초기 화합니다.
        /// </summary>
        public void InitializeControl()
        {
            RefreshTree();


            // P20130723-20 - 사용자그룹 트리뷰에서 체크박스 기능을 넣어 수정 
            /*
            if (m_TreeType == E_TreeType.SystemGroup)
            {
                treeViewEx1.CheckBoxes = true;
                if (treeViewEx1.Nodes.Count > 0)
                {
                    treeViewEx1.Nodes[0].Checked = true;
                }
            }
            */
            if (m_TreeType == E_TreeType.UserGroup)
            {
                AppGlobal.s_DataSyncProcssor.OnGroupInfoChangeEvent += new HandlerArgument2<GroupInfo, E_WorkType>(OnGroupInfoChangeEvent_Process);
                AppGlobal.s_DataSyncProcssor.OnDeviceInfoChangeEvent += new HandlerArgument2<DeviceInfo, E_WorkType>(OnDeviceInfoChangeEvent);
                AppGlobal.s_DataSyncProcssor.OnDeviceInfoListChangeEvent += new HandlerArgument2<DeviceInfoCollection, E_WorkType>(s_DataSyncProcssor_OnDeviceInfoListChangeEvent);
            }
        }


        /// <summary>
        /// 컨트롤을 초기 화합니다.
        /// </summary>
        public void InitializeControl(GroupInfo aDeleteGroupInfo)
        {
            m_DeleteGroupInfo = aDeleteGroupInfo;
            InitializeControl();
        }

        void s_DataSyncProcssor_OnDeviceInfoListChangeEvent(DeviceInfoCollection aValue1, E_WorkType aValue2)
        {
            // TODO 바뀐것만 새로고침
            RefreshTree();
        }

        /// <summary>
        /// 그룹 트리의 노드 정보를 새로고침 하는 함수입니다.
        /// </summary>
        public void RefreshTree()
        {
            AppGlobal.InitializeGroupTreeView(treeViewEx1, m_TreeType, m_DeleteGroupInfo);
        }

        /// <summary>
        /// 트리에서 장비 정보가 변경되었을 때 수행하는 함수 입니다.
        /// </summary>
        /// <param name="aValue1"></param>
        /// <param name="aValue2"></param>
        void OnDeviceInfoChangeEvent(DeviceInfo aDeviceInfo, E_WorkType aWorkType)
        {
            // 2013-08-13- shinyn - 사용자그룹 조회인 경우에는 이벤트 발생안하도록 처리
            if (m_TreeType == E_TreeType.DisplayUserGroup) return;
            if (m_TreeType == E_TreeType.DisplayUserGroupList) return;

            RefreshTree();
        }

        private void mnuConnectGroupDevice_Click(object sender, EventArgs e)
        {
            if (treeViewEx1.SelectedNode == null) return;

            TreeNodeEx tSelectNode = (TreeNodeEx)treeViewEx1.SelectedNode;

            if (tSelectNode.Tag is GroupInfo)
            {
                if (OnConnectGroupDeviceEvent != null) OnConnectGroupDeviceEvent((GroupInfo)tSelectNode.Tag);

            }
        }

        /// <summary>
        /// 2013-08-14- shinyn - 사용자 장비를 조회하여, 장비리스트를 공유한다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuShareDevice_Click(object sender, EventArgs e)
        {
            TreeNodeEx tGroupNode = (TreeNodeEx)treeViewEx1.SelectedNode;

            if (tGroupNode.Tag is GroupInfo)
            {
                GroupInfo tGroupInfo = (GroupInfo)tGroupNode.Tag;

                if (OnShareDeviceEvent != null) OnShareDeviceEvent(tGroupInfo);


            }
        }


    }
}
