using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Reflection;
using RACTSerialProcess;
using System.IO.Ports;
using RACTCommonClass;

namespace RACTClient
{
    public partial class ucSerialConnectOption : UserControl
    {
        public ucSerialConnectOption()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 컨트롤을 초기화 합니다.
        /// </summary>
        public void Initialize()
        {
            AppGlobal.InitializeComboBoxStyle(cboBaudRate);
            AppGlobal.InitializeComboBoxStyle(cboDataBit);
            AppGlobal.InitializeComboBoxStyle(cboParity);
            AppGlobal.InitializeComboBoxStyle(cboPort);
            AppGlobal.InitializeComboBoxStyle(cboStopBit);

            int i = 0;

            //포트 목록을 초기화 합니다.
            cboPort.Items.Clear();
            string[] tPorts = SerialPort.GetPortNames();

            if (tPorts.Length > 0)
            {
                for (i = 0; i < tPorts.Length; i++)
                {
                    cboPort.Items.Add(tPorts[i]);
                    if (tPorts[i].Equals(AppGlobal.s_ClientOption.ConnectionInfo.SerialConfig.PortName))
                    {
                        cboPort.SelectedIndex = i;
                    }
                }

                if(cboPort.SelectedIndex < 0)
                {
                    cboPort.SelectedIndex = 0;
                }
            }

            //통신 속도를 초기화 합니다.
            Int32[] tBaudRates = { 100, 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 38400, 56000, 57600, 115200, 128000, 256000, 0 };

            cboBaudRate.Items.Clear();
            for (i = 0; i < tBaudRates.Length; i++)
            {
                cboBaudRate.Items.Add(tBaudRates[i].ToString());
                if (tBaudRates[i] == AppGlobal.s_ClientOption.ConnectionInfo.SerialConfig.BaudRate)
                {
                    cboBaudRate.SelectedIndex = i;
                }
            }
            

            //데이터 비트 목록을 초기화 합니다.
            Int32[] tDataBit = { 5,6,7,8 };
            cboDataBit.Items.Clear();
            for (i = 0; i < tDataBit.Length; i++)
            {
                cboDataBit.Items.Add(tDataBit[i].ToString());
                if (tDataBit[i] == AppGlobal.s_ClientOption.ConnectionInfo.SerialConfig.DataBits)
                {
                    cboDataBit.SelectedIndex = i;
                }
            }
        
            //패리티 목록을 초기화 합니다.
            cboParity.Items.Clear();
            foreach (string tParity in Enum.GetNames(typeof(Parity)))
            {
                cboParity.Items.Add(tParity);
                if (tParity.Equals(AppGlobal.s_ClientOption.ConnectionInfo.SerialConfig.Parity.ToString()))
                {
                    cboParity.SelectedIndex = (int)AppGlobal.s_ClientOption.ConnectionInfo.SerialConfig.Parity;
                }
            }

            //스탑 비트 목록을 초기화 합니다.
            cboStopBit.Items.Clear();
            foreach (string tParity in Enum.GetNames(typeof(StopBits)))
            {
                cboStopBit.Items.Add(tParity);
                if (tParity.Equals(AppGlobal.s_ClientOption.ConnectionInfo.SerialConfig.StopBits.ToString()))
                {
                    cboStopBit.SelectedIndex = (int)AppGlobal.s_ClientOption.ConnectionInfo.SerialConfig.StopBits;
                }
            }
        }

        public SerialConfig SerialConfig
        {
            get
            {
                SerialConfig tConfig = new SerialConfig();
                tConfig.BaudRate = int.Parse(cboBaudRate.Text);
                tConfig.DataBits = int.Parse(cboDataBit.Text);
                tConfig.Parity = (Parity)cboParity.SelectedIndex;
                tConfig.StopBits =(StopBits)cboStopBit.SelectedIndex;
                tConfig.PortName = cboPort.Text;
                return tConfig;
            }

            set
            {
                for (int i = 0; i < cboBaudRate.Items.Count; i++)
                {
                    if (cboBaudRate.Items[i].Text.Equals(value.BaudRate.ToString()))
                    {
                        cboBaudRate.SelectedIndex = i;
                        break;
                    }
                }

                for (int i = 0; i < cboDataBit.Items.Count; i++)
                {
                    if (cboDataBit.Items[i].Text.Equals(value.DataBits.ToString()))
                    {
                        cboDataBit.SelectedIndex = i;
                        break;
                    }
                }

                for (int i = 0; i < cboParity.Items.Count; i++)
                {
                    if (cboParity.Items[i].Text.Equals(value.Parity.ToString()))
                    {
                        cboParity.SelectedIndex = i;
                        break;
                    }
                }

                for (int i = 0; i < cboPort.Items.Count; i++)
                {
                    if (cboPort.Items[i].Text.Equals(value.PortName.ToString()))
                    {
                        cboPort.SelectedIndex = i;
                        break;
                    }
                }

                for (int i = 0; i < cboStopBit.Items.Count; i++)
                {
                    if (cboStopBit.Items[i].Text.Equals(value.StopBits.ToString()))
                    {
                        cboStopBit.SelectedIndex = i;
                        break;
                    }
                }
            }
        }


        }
}
