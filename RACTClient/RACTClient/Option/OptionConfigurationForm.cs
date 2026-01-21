using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RACTClient
{
    public partial class OptionConfigurationForm : BaseForm
    {
        /// <summary>
        /// 클라이언
        /// </summary>
        public event DefaultHandler OnClientOptionChangeEvent = null;
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public OptionConfigurationForm()
        {
            InitializeComponent();
            initializeControl();

        }
        /// <summary>
        /// 컨트롤을 초기화 합니다.
        /// </summary>
        private void initializeControl()
        {
            //각 패널을 초기 화합니다.
            foreach (Control tControl in pnlSubPanl.Controls)
            {
                if (tControl is IOptionPanal)
                {
                    ((IOptionPanal)tControl).InitializeControl();
                }
            }
            SetTreeView();

            AddButton(E_ButtonType.Close, E_ButtonSide.Right, "닫기");
            AddButton(E_ButtonType.OK, E_ButtonSide.Right, "확인");
        }

        /// <summary>
        /// 버튼 클릭 처리 입니다.
        /// </summary>
        /// <param name="aButtonType">눌러진 버튼 타입 입니다.</param>
        protected override void ButtonProcess(E_ButtonType aButtonType)
        {
            switch (aButtonType)
            {
                case E_ButtonType.OK:
                    // 확인 처리
                    bool tIsOK = true;
                    foreach (Control tControl in pnlSubPanl.Controls)
                    {
                        if (tControl is IOptionPanal)
                        {
                            if (!((IOptionPanal)tControl).SaveOption())
                            {
                                tIsOK = false;
                                break;
                            }
                        }
                    }
                    if (tIsOK)
                    {
                        AppGlobal.MakeClientOption();
                        if (OnClientOptionChangeEvent != null)
                        {
                            OnClientOptionChangeEvent();
                        }
                        AppGlobal.ShowMessageBox(AppGlobal.s_ClientMainForm,"연결 속성을 설정하였습니다.", MessageBoxButtons.OK, MessageBoxIcon.None);
                        Close();
                    }
                    break;
                case E_ButtonType.Close:
                    Close();
                    break;
            }
        }

        /// <summary>
        /// 트리 목록을 초기화 합니다.
        /// </summary>
        private void SetTreeView()
        {
            TreeNode tTreeNode;
            TreeNode tSubNode;
            tTreeNode = new TreeNode("일반",0,0);
            tTreeNode.Tag = pnlGeneral;
            trvOptionConfiguration.Nodes.Add(tTreeNode);

            tTreeNode = new TreeNode("연결", 0, 0);
            tTreeNode.Tag = pnlConnectOption;
            trvOptionConfiguration.Nodes.Add(tTreeNode);
            trvOptionConfiguration.SelectedNode = tTreeNode;

            tSubNode = new TreeNode("TELNET", 1, 1);
            tSubNode.Tag = pnlTelnet;
            tTreeNode.Nodes.Add(tSubNode);

            tSubNode = new TreeNode("Serial Port", 1, 1);
            tSubNode.Tag = pnlSerialPort;
            tTreeNode.Nodes.Add(tSubNode);

            tTreeNode = new TreeNode("터미널", 0, 0);
            tTreeNode.Tag = pnlTerminalPopupType;
            trvOptionConfiguration.Nodes.Add(tTreeNode);

			// 2019-11-10 개선사항 (중요 장비 강조 정책 옵션 기능)
            tSubNode = new TreeNode("기본 글꼴 & 색상", 1, 1);
            tSubNode.Tag = pnlTerminalColor;
            tTreeNode.Nodes.Add(tSubNode);
			// 2019-11-10 개선사항 (중요 장비 강조 정책 옵션 기능)
            tSubNode = new TreeNode("강조 글꼴 & 색상 ", 1, 1);
            tSubNode.Tag = pnlHighlightColor;
            tTreeNode.Nodes.Add(tSubNode);

            tSubNode = new TreeNode("레이아웃", 1, 1);
            tSubNode.Tag = pnlTerminalLayout;
            tTreeNode.Nodes.Add(tSubNode);

            trvOptionConfiguration.ExpandAll();
            trvOptionConfiguration.SelectedNode = trvOptionConfiguration.Nodes[0];
        }

        /// <summary>
        /// 트리 노드 선택을 변경 합니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trvOptionConfiguration_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode tNode = trvOptionConfiguration.SelectedNode;

            if (tNode == null || tNode.Tag == null) return;

            ((Control)tNode.Tag).BringToFront();

            this.Invalidate();
        }
    }
}

