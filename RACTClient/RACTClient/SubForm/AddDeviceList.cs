using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RACTClient
{
    public partial class AddDeviceList : BaseForm
    {

        private E_AddDeviceType m_AddDevceType;

        public E_AddDeviceType AddDeviceType
        {
            get { return m_AddDevceType; }
        }


        public AddDeviceList()
        {
            InitializeComponent();
            InitializeControl();
        }

        public void InitializeControl()
        {
            AddButton(E_ButtonType.Close, E_ButtonSide.Right, "닫기");
            AddButton(E_ButtonType.OK, E_ButtonSide.Right, "확인");
        }

        protected override void ButtonProcess(E_ButtonType aButtonType)
        {
            if (aButtonType == E_ButtonType.OK)
            {
                if (rdoAddUserGroup.Checked)
                {
                    m_AddDevceType = E_AddDeviceType.AddUserGroup;
                }
                else if (rdoSaveDeviceList.Checked)
                {
                    m_AddDevceType = E_AddDeviceType.SaveDeviceList;
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    }
}