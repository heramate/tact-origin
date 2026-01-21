using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using RACTCommonClass;

namespace RACTClient
{
    public enum E_SerialPort
    {
        TELNET,
        SERIAL_PORT
    }

    public partial class ucSerialPortPanel : UserControl,IOptionPanal
    {
        public ucSerialPortPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 컨트롤을 초기화 합니다.
        /// </summary>
        public void InitializeControl()
        {
            pnlSerialOption.Initialize();
        }

        /// <summary>
        /// 옵션을 저장 합니다.
        /// </summary>
        public bool SaveOption()
        {
            AppGlobal.s_ClientOption.ConnectionInfo.SerialConfig = pnlSerialOption.SerialConfig;
            return true;
        }

        /// <summary>
        /// 컨트롤에서 가져온 SerialPort 정보를 반환합니다.
        /// </summary>
        /// <returns></returns>
        public SerialConfig GetSerialPortInfo()
        {
            return pnlSerialOption.SerialConfig;
        }

    }
}
