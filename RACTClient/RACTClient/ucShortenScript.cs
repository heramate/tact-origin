using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace RACTClient
{
    public partial class ucShortenScript : UserControl
    {
        public ucShortenScript()
        {
            InitializeComponent();
            initializeControl();
        }

        private void initializeControl()
        {
            AppGlobal.InitializeButtonStyle(btnSetting);
            AppGlobal.InitializeGridStyle(grdShortenScript);
        }
    }
}
