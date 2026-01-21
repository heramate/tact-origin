using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;

namespace RACTCommonClass
{
    [Serializable]
    public class SerialConfig
    {
        /// <summary>
        /// Port Name 입니다.
        /// </summary>
        private string m_PortName = "";

        /// <summary>
        /// BaudRate 입니다.
        /// </summary>
        private int m_BaudRate = 9600;

        /// <summary>
        /// DataBits 입니다.
        /// </summary>
        private int m_DataBits = 8;

        /// <summary>
        /// Parity 입니다.
        /// </summary>
        private Parity m_Parity = Parity.None;

        /// <summary>
        /// StopBits 입니다.
        /// </summary>
        private StopBits m_StopBits = StopBits.One;

        /// <summary>
        /// Handshake 입니다.
        /// </summary>
        private Handshake m_Handshake = Handshake.None;

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public SerialConfig() { }

        /// <summary>
        /// 복사 생성자 입니다.
        /// </summary>
        /// <param name="aSerialConfig"></param>
        public SerialConfig(SerialConfig aSerialConfig)
        {
            m_BaudRate = aSerialConfig.m_BaudRate;
            m_DataBits = aSerialConfig.m_DataBits;
            m_Handshake = aSerialConfig.m_Handshake;
            m_Parity = aSerialConfig.m_Parity;
            m_PortName = aSerialConfig.m_PortName;
            m_StopBits = aSerialConfig.m_StopBits;
        }


        /// <summary>
        /// Handshake 가져오거나 설정 합니다.
        /// </summary>
        public Handshake Handshake
        {
            get { return m_Handshake; }
            set { m_Handshake = value; }
        }

        /// <summary>
        /// StopBits 가져오거나 설정 합니다.
        /// </summary>
        public StopBits StopBits
        {
            get { return m_StopBits; }
            set { m_StopBits = value; }
        }


        /// <summary>
        /// Parity 가져오거나 설정 합니다.
        /// </summary>
        public Parity Parity
        {
            get { return m_Parity; }
            set { m_Parity = value; }
        }

        /// <summary>
        /// DataBits 가져오거나 설정 합니다.
        /// </summary>
        public int DataBits
        {
            get { return m_DataBits; }
            set { m_DataBits = value; }
        }


        /// <summary>
        /// BaudRate 가져오거나 설정 합니다.
        /// </summary>
        public int BaudRate
        {
            get { return m_BaudRate; }
            set { m_BaudRate = value; }
        }


        /// <summary>
        /// Port Name 가져오거나 설정 합니다.
        /// </summary>
        public string PortName
        {
            get { return m_PortName; }
            set { m_PortName = value; }
        }
    }
}
