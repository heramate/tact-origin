using System;
using System.Collections.Generic;
using System.Text;
using RACTCommonClass;
using System.Threading;
using System.IO.Ports;

namespace RACTSerialProcess
{
    /// <summary>
    /// 시리얼 세션 정보 입니다.
    /// </summary>
    public class SerialSessionInfo
    {
        /// <summary>
        /// 수신 큐 입니다.
        /// </summary>
        private Queue<SerialCommandResultInfo> m_ReceivedQueue;
        /// <summary>
        /// 수신 대기 입니다.
        /// </summary>
        private ManualResetEvent m_ReceivedMRE = new ManualResetEvent(false);
        /// <summary>
        /// 전송 큐 입니다.
        /// </summary>
        private Queue<SerialCommandRequestInfo> m_RequestQueue ;
        /// <summary>
        /// 전송 대기 입니다.
        /// </summary>
        private ManualResetEvent m_RequestMRE = new ManualResetEvent(false);
        /// <summary>
        /// 시리얼 객체 입니다.
        /// </summary>
        private SerialPort m_Serial = null;
        /// <summary>
        /// Serial Config 입니다.
        /// </summary>
        private SerialConfig m_SerialConfig;
        /// <summary>
        /// 결과 처리용 스레드 입니다.
        /// </summary>
        private Thread m_ResultProcessThread = null;
        /// <summary>
        /// 요청 처리용 스레드 입니다.
        /// </summary>
        private Thread m_RequestProcessThread = null;
        /// <summary>
        /// 실행 여부 입니다.
        /// </summary>
        private bool m_IsRun;
        /// <summary>
        /// 요청 emulator 입니다.
        /// </summary>
        private ISerialEmulator m_Sender = null;

        /// <summary>
        /// Session ID 입니다.
        /// </summary>
        private int m_SessionID;

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public SerialSessionInfo(ISerialEmulator aSender, SerialConfig aConfig)
        {
            m_IsRun = true;
            m_SessionID = this.GetHashCode();

            m_Sender = aSender;
            m_Serial = new SerialPort();
            m_SerialConfig = aConfig;

            m_Serial.PortName = m_SerialConfig.PortName;
            m_Serial.BaudRate = m_SerialConfig.BaudRate;
            m_Serial.Parity = m_SerialConfig.Parity;
            m_Serial.DataBits = m_SerialConfig.DataBits;
            m_Serial.StopBits = m_SerialConfig.StopBits;
            m_Serial.Handshake = m_SerialConfig.Handshake;

            m_Serial.ReadTimeout = 500;
            m_Serial.WriteTimeout = 500;
            m_Serial.DataReceived += new SerialDataReceivedEventHandler(Serial_DataReceived);

            m_ReceivedQueue = new Queue<SerialCommandResultInfo>();

            m_RequestQueue = new Queue<SerialCommandRequestInfo>();

            m_ResultProcessThread = new Thread(new ThreadStart(ProcessResultData));
            m_ResultProcessThread.Start();

            m_RequestProcessThread = new Thread(new ThreadStart(ProcessRequestData));
            m_RequestProcessThread.Start();
        }

        /// <summary>
        /// 시리얼에서 데이터 받음 처리 합니다.
        /// </summary>
        void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] tReadBuffer = new byte[m_Serial.ReadBufferSize + 1];
            int tReceiveCount = 0;

            if (m_Serial.IsOpen)
            {
                try
                {
                    tReceiveCount = m_Serial.Read(tReadBuffer, 0, m_Serial.ReadBufferSize);
                    lock (m_ReceivedQueue)
                    {
                        m_ReceivedQueue.Enqueue(new SerialCommandResultInfo(m_Serial.PortName, System.Text.Encoding.ASCII.GetString(tReadBuffer, 0, tReceiveCount), m_SessionID));
                        m_ReceivedMRE.Set();
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// 결과를 처리 합니다.
        /// </summary>
        private void ProcessResultData()
        {
            SerialCommandResultInfo tSerialCommandResultInfo = null;
            while (m_IsRun)
            {
                if (m_ReceivedQueue.Count == 0)
                {
                    m_ReceivedMRE.Reset();
                    m_ReceivedMRE.WaitOne();
                }
                lock (m_ReceivedQueue)
                {
                    tSerialCommandResultInfo = m_ReceivedQueue.Dequeue();
                }

                if (tSerialCommandResultInfo != null)
                {
                    m_Sender.DisplayResult(tSerialCommandResultInfo);
                    tSerialCommandResultInfo = null;
                }

            }
        }
        /// <summary>
        /// 명령 요청 처리 합니다.
        /// </summary>
        private void ProcessRequestData()
        {
            SerialCommandRequestInfo tSerialCommandResultInfo = null;
            while (m_IsRun)
            {
                if (m_RequestQueue.Count == 0)
                {
                    m_RequestMRE.Reset();
                    m_RequestMRE.WaitOne();
                }

                lock (m_RequestQueue)
                {
                    tSerialCommandResultInfo = m_RequestQueue.Dequeue();
                }

                if (tSerialCommandResultInfo != null)
                {
                    m_Serial.Write(tSerialCommandResultInfo.RequestData);
                    tSerialCommandResultInfo = null;
                }
            }
        }
        /// <summary>
        /// 접속 처리 합니다.
        /// </summary>
        /// <returns></returns>
        internal bool Connect()
        {
            try
            {
                m_Serial.Open();
                lock (m_RequestQueue)
                {
                    m_RequestQueue.Enqueue(new SerialCommandRequestInfo("COM", "\n"));
                    m_RequestMRE.Set();
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 명령을 전송 합니다.
        /// </summary>
        /// <param name="aCommand"></param>
        public void SendData(SerialCommandRequestInfo aCommand)
        {
            lock (m_RequestQueue)
            {
                m_RequestQueue.Enqueue(aCommand);
                m_RequestMRE.Set();
            }
        }
    
        /// <summary>
        /// 종료 처리 합니다.
        /// </summary>
        internal bool DisConnect()
        {
            try
            {
                m_IsRun = false;
                m_ReceivedQueue.Clear();
                try
                {
                    if (m_RequestProcessThread != null && m_RequestProcessThread.IsAlive)
                    {
                        m_RequestProcessThread.Join(10);
                        m_RequestProcessThread.Abort();
                        m_RequestProcessThread = null;
                    }
                }
                catch { }

                m_RequestQueue.Clear();

                try
                {
                    if (m_ResultProcessThread != null && m_ResultProcessThread.IsAlive)
                    {
                        m_ResultProcessThread.Join(10);
                        m_ResultProcessThread.Abort();
                        m_ResultProcessThread = null;
                    }
                }
                catch { }

                m_Serial.Close();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// 종료 처리 합니다.
        /// </summary>
        internal void Stop()
        {
            m_IsRun = false;
            DisConnect();
        }

        /// <summary>
        /// Session ID 가져오거나 설정 합니다.
        /// </summary>
        public int SessionID
        {
            get { return m_SessionID; }
            set { m_SessionID = value; }
        }
        /// <summary>
        /// Serial Config 가져오거나 설정 합니다.
        /// </summary>
        public SerialConfig SerialConfig
        {
            get { return m_SerialConfig; }
            set { m_SerialConfig = value; }
        }

        /// <summary>
        /// Sender 가져오거나 설정 합니다.
        /// </summary>
        public ISerialEmulator Sender
        {
            get { return m_Sender; }
            set { m_Sender = value; }
        }

    }
}
