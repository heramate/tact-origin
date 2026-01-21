using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RACTCommonClass;

namespace RACTClient
{
    public partial class DeviceSearch : BaseForm
    {
        private DeviceInfo m_SelectedDeviceInfo;
        /// <summary>
        /// 선택된 장비 정보 속성을 가져오거나 설정합니다.
        /// </summary>
        public DeviceInfo SelectedDeviceInfo
        {
            get { return m_SelectedDeviceInfo; }
            set { m_SelectedDeviceInfo = value; }
        }	

        public DeviceSearch()
        {
            InitializeComponent();
        }

        public void InitializeControl()
        {
            AddButton(E_ButtonType.Close, E_ButtonSide.Right, "닫기");
            AddButton(E_ButtonType.OK, E_ButtonSide.Right, "확인");
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
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
                    m_SelectedDeviceInfo = ucDeviceSearch1.getSelectedDeviceInfo();
                    DialogResult = DialogResult.OK;
                    Close();
                    break;
                case E_ButtonType.Close:
                    DialogResult = DialogResult.Cancel;
                    Close();
                    break;
            }
        }
    }
}