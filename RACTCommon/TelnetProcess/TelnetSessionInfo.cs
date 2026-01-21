using System;
using System.Collections.Generic;
using System.Text;
using RACTCommonClass;
using System.Threading;
using RACTTerminal;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace TelnetProcessor
{
    /// <summary>
    /// 세션 종료 핸들러 입니다.
    /// </summary>
    /// <param name="aKey"></param>
    public delegate void SessionDisConnectHandler (int aKey);
    /// <summary>
    /// 텔넷 명령 로그를 저장할 핸들러 입니다.
    /// </summary>
    /// <param name="aSessionKey"></param>
    /// <param name="aCommand"></param>
    public delegate void TelnetCommandSaveHandler(DBExecuteCommandLogInfo aLogInfo);
    /// <summary>
    /// 텔넷 세션 클래스 입니다.
    /// </summary>
    public class TelnetSessionInfo
    {

        /// <summary>
        /// 누적 명령어 입니다.
        /// </summary>
        private String fullCommand = "";

        /// <summary>
        /// 텔넷 에뮬레이터입니다.
        /// </summary>
        private ITelnetEmulator aSender = null;
        /// <summary>
        /// 텔넷 결과 이벤트 입니다.
        /// </summary>
        public event TelnetResultHandler OnTelnetResultEvent;
        /// <summary>
        /// 종료 이벤트 입니다
        /// </summary>
        public event SessionDisConnectHandler OnSessionDisConnectEvent;
        /// <summary>
        /// 텔넷로그를 저장할 이벤트 입니다.
        /// </summary>
       // public event TelnetCommandSaveHandler OnTelnetCommandSaveEvent;
        /// <summary>
        /// Telnet 객체 입니다.
        /// </summary>
        private TerminalServer m_Telnet;
        /// <summary>
        /// 실행할 명령이 저장 됩니다.
        /// </summary>
        private Queue<RequestCommunicationData> m_RequestQueue;
        /// <summary>
        /// 명령 결과가 저장됩니다.
        /// </summary>
        private Queue<TelnetCommandResultInfo> m_ResultQueue;
        /// <summary>
        /// 결과 대기 입니다.
        /// </summary>
        private ManualResetEvent m_ResultMRE = new ManualResetEvent(false);
        /// <summary>
        /// 요청 대기 입니다.
        /// </summary>
        private ManualResetEvent m_RequestMRE = new ManualResetEvent(false);
        /// <summary>
        /// 장비 정보 입니다.
        /// </summary>
        private DeviceInfo m_DeviceInfo;
        /// <summary>
        /// 실행 여부 입니다.
        /// </summary>
        private bool m_IsRun;
        /// <summary>
        /// 요청 처리용 스레드 입니다.
        /// </summary>
        private Thread m_RequestProcessThread;
        /// <summary>
        /// 결과 처리용 스레드 입니다.
        /// </summary>
        private Thread m_ResultProcessThread;
        /// <summary>
        /// Client ID 입니다.
        /// </summary>
        private int m_ClientID;
        /// <summary>
        /// 명령 요청 정보 입니다.
        /// </summary>
        private RequestCommunicationData m_RequestData;
        /// <summary>
        /// Connection Session ID 입니다.
        /// </summary>
        private int m_ConnectionSessionID;
        /// <summary>
        /// 타임아웃 시간 입니다.
        /// </summary>
        //private int m_ConnectTimeOut = 5000;
        private int m_ConnectTimeOut = 15000; // 2015-04-30 - 신윤남 - 로그인이 안될 경우 수동으로 로그인하는 기능에 사용합니다.
        /// <summary>
        /// Sender 입니다.
        /// </summary>
        private ITelnetEmulator m_Sender;
        /// <summary>
        /// 텔넷 명령 정보 입니다.
        /// </summary>
        private TelnetCommandInfo m_TelnetCommandInfo = null;
        /// <summary>
        /// 타임 아웃 체크 스레드 입니다.
        /// </summary>
        private Thread m_TimeOutCheckThread = null;
        /// <summary>
        /// 마지막 명령 전송 시간 입니다.
        /// </summary>
        private DateTime m_LastCommandSendTime = DateTime.Now;

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public TelnetSessionInfo(int aClientID,DeviceInfo aDeviceInfo )
        {
            m_IsRun = true;

            m_ClientID = aClientID;
            m_DeviceInfo = aDeviceInfo;

            m_ResultQueue = new Queue<TelnetCommandResultInfo>();
            m_RequestQueue = new Queue<RequestCommunicationData>();


            m_RequestProcessThread = new Thread(new ThreadStart(ProcessRequest));
            m_RequestProcessThread.Name = "요청 처리용 스레드 입니다.";
            m_RequestProcessThread.Start();

            m_ResultProcessThread = new Thread(new ThreadStart(ProcessResult));
            m_ResultProcessThread.Name = "결과 처리용 스레드 입니다.";
            m_ResultProcessThread.Start();

            m_TimeOutCheckThread = new Thread(new ThreadStart(ConnectionTimeOutCheck));
            m_TimeOutCheckThread.Start();

            m_Telnet = new TerminalServer();
            m_Telnet.OnConnected += new ConnectHandler(m_Telnet_OnConnected);
            m_Telnet.OnReceivedData += new ReceivedDataHandler(m_Telnet_OnReceivedData);
            m_Telnet.OnDisconnected += new DisConnectHandler(m_Telnet_OnDisconnected);
        }
        private void ConnectionTimeOutCheck()
        {
            while (m_IsRun)
            {
                if (((TimeSpan)DateTime.Now.Subtract(m_LastCommandSendTime)).TotalMilliseconds > m_ConnectTimeOut && m_SessionStatus == E_SessionStatus.TryConnect)
                {
                    m_Telnet_OnDisconnected();
                    break;
                }
                Thread.Sleep(500);
            }
        }
        /// <summary>
        /// 접속 종료 이벤트 입니다.
        /// </summary>
        void m_Telnet_OnDisconnected()
        {
            TelnetCommandResultInfo tResult = new TelnetCommandResultInfo(m_RequestData);
            if (m_SessionStatus == E_SessionStatus.Disconnect) return;

            m_SessionStatus = E_SessionStatus.Disconnect;
            tResult.ReslutType = E_TelnetReslutType.DisConnected;
            if (m_Sender != null)
            {
                m_Sender.DisplayResult(tResult);
            }
            else
            {
                lock (m_ResultQueue)
                {
                    m_ResultQueue.Enqueue(tResult);
                    m_ResultMRE.Set();
                }
            }
//            Thread.Sleep(100);
            if (OnSessionDisConnectEvent != null) OnSessionDisConnectEvent(m_ConnectionSessionID);
        }

        /// <summary>
        /// 장비로 받은 결과 처리 입니다.
        /// </summary>
        /// <param name="aReceivedData"></param>
        void m_Telnet_OnReceivedData(string aReceivedData)
        {
            TelnetCommandResultInfo tResult = new TelnetCommandResultInfo(m_RequestData);

            if (m_SessionStatus == E_SessionStatus.TryConnect)
            {
                m_SessionStatus = E_SessionStatus.Connect;
            }
            tResult.ResultData = aReceivedData;
            tResult.SessionID = m_ConnectionSessionID;
            if (m_Sender != null)
            {
                m_Sender.DisplayResult(tResult);
            }
            else
            {
                lock (m_ResultQueue)
                {
                    m_ResultQueue.Enqueue(tResult);
                    m_ResultMRE.Set();
                }
            }
        }
        
        /// <summary>
        /// 연결 처리 결과 처리 입니다.
        /// </summary>
        /// <param name="aIsConnect"></param>
        void m_Telnet_OnConnected(bool aIsConnect)
        {
            TelnetCommandResultInfo tResult = new TelnetCommandResultInfo(m_RequestData);
            tResult.ResultData = string.Empty;
            tResult.ReslutType = E_TelnetReslutType.TryConnect;
            lock (m_ResultQueue)
            {
                m_ResultQueue.Enqueue(tResult);
                m_ResultMRE.Set();
            }
        }

        /// <summary>
        /// 명령을 추가 합니다.
        /// </summary>
        /// <param name="tCommandInfo"></param>
        internal void AddCommand(RequestCommunicationData tCommandInfo)
        {
            lock (m_RequestQueue)
            {
                m_RequestQueue.Enqueue(tCommandInfo);
                m_RequestMRE.Set();
            }
        }

        /// <summary>
        /// 명령 처리 합니다.
        /// </summary>
        private void ProcessRequest()
        {

            RequestCommunicationData tCommand = null;
            while (m_IsRun)
            {
                if (m_RequestQueue.Count == 0)
                {
                    m_RequestMRE.Reset();
                    m_RequestMRE.WaitOne();
                }

                lock (m_RequestQueue)
                {
                    tCommand = m_RequestQueue.Dequeue();
                }

                if (tCommand != null)
                {
                    SendCommand(tCommand);   
                    tCommand = null;
                }
            }
        }

        /// <summary>
        /// 결과 처리 합니다.
        /// </summary>
        private void ProcessResult()
        {
            TelnetCommandResultInfo tResult = null;

            while (m_IsRun)
            {

                if (m_ResultQueue.Count == 0)
                {
                    m_ResultMRE.Reset();
                    m_ResultMRE.WaitOne();
                }
                
                lock (m_ResultQueue)
                {
                    if (m_ResultQueue.Count > 0)
                    {
                        tResult = m_ResultQueue.Dequeue();
                    }
                }


                if (tResult != null)
                {
                    tResult.SessionID = m_ConnectionSessionID;
                    if (OnTelnetResultEvent != null)
                    {
                        OnTelnetResultEvent(tResult);
                    }
                    tResult = null;
                }

            }
        }


        /// <summary>
        /// 장비로 받은 결과 처리 입니다.
        /// </summary>
        /// <param name="aReceivedData"></param>
        void MakeResultTest(string aReceivedData)
        {
            TelnetCommandResultInfo tResult = new TelnetCommandResultInfo(m_RequestData);

            if (m_SessionStatus == E_SessionStatus.TryConnect)
            {
                m_SessionStatus = E_SessionStatus.Connect;
            }
            tResult.ResultData = aReceivedData;
            tResult.SessionID = m_ConnectionSessionID;
            tResult.IsLimitCmd = true;

            if (m_Sender != null)
            {
                m_Sender.DisplayResult(tResult);
            }
            else
            {
                lock (m_ResultQueue)
                {
                    m_ResultQueue.Enqueue(tResult);
                    m_ResultMRE.Set();
                }
            }
        }
        
        /// <summary>
        /// 장비에 명령을 전송 합니다.
        /// </summary>
        /// <param name="aCommandInfo"></param>
        private void SendCommand(RequestCommunicationData aCommandInfo) 
        {
            m_RequestData = aCommandInfo;
            m_TelnetCommandInfo = (TelnetCommandInfo)m_RequestData.RequestData;
            m_LastCommandSendTime = DateTime.Now;


                //if (m_TelnetCommandInfo.Command.Contains("\r"))
                //    {
                //        if (m_TelnetCommandInfo.KeyInfo != null && m_TelnetCommandInfo.KeyInfo.ScanCode.Equals(28))
                //            {
                //                // 엔터키 입력 이벤트
                //                if (fullCommand.ToLower().Contains("show system") || m_TelnetCommandInfo.Command.ToLower().Contains("show system"))
                //                {
                //                    //m_Telnet.SendLimitResult("제한 명령어 입력");
                //                    MakeResultTest("제한 명령어 입력");
                //                    return;
                //                }
                //                fullCommand = "";
                //                Console.Write("Enter Click : " + m_TelnetCommandInfo.Command);
                //            }
                //    }
                //else if (m_TelnetCommandInfo.KeyInfo != null && m_TelnetCommandInfo.KeyInfo.ScanCode.Equals(14))
                //    {
                //        //BackSpace 입력
                //        if (!fullCommand.Equals(""))
                //        {
                //            Console.Write("BackSpace Click : " + m_TelnetCommandInfo.Command);
                //            fullCommand = fullCommand.Remove(fullCommand.Length - 1);
                //            Console.Write("fullCommand : " + fullCommand);
                //        }
                        
                        
                //    }               
                //else
                //    {
                //        fullCommand += m_TelnetCommandInfo.Command;
                //        Console.Write(fullCommand);
                       
                //    }
            

            //Gunny m_TelnetCommandInfo.Command  누적시키면서 enter 확인.
            //if 제한명령어일 경우.. MakeResultTest ... else m_TelnetCommandInfo.Command에 enter전의 명령어를 전송.
            //클라이언트에 result 객체 만들어서 전송.


            //2013-01-28 -- SSH 명령어 전송
            switch (m_TelnetCommandInfo.DeviceInfo.TerminalConnectInfo.ConnectionProtocol)
            {
                case E_ConnectionProtocol.SSHTelnet:
                    if (m_TelnetCommandInfo.KeyInfo != null)
                    {
                        m_Telnet.SendSSHCommand(m_TelnetCommandInfo.KeyInfo.Outstring);
                    }
                    else
                    {
                        m_Telnet.SendSSHCommand(m_TelnetCommandInfo.Command);
                    }
                    break;
                case E_ConnectionProtocol.TELNET:
                    if (m_TelnetCommandInfo.KeyInfo != null)
                    {
                        m_Telnet.SendCommand(m_TelnetCommandInfo.KeyInfo.Outstring);
                    }
                    else
                    {
                        m_Telnet.SendCommand(m_TelnetCommandInfo.Command);
                    }
                    break;
                case E_ConnectionProtocol.SERIAL_PORT:
                    if (m_TelnetCommandInfo.KeyInfo != null)
                    {
                        m_Telnet.SendCommand(m_TelnetCommandInfo.KeyInfo.Outstring);
                    }
                    else
                    {
                        m_Telnet.SendCommand(m_TelnetCommandInfo.Command);
                    }
                    break;
            }


            
        }
      
        /// <summary>
        /// 장비에 접속 합니다.
        /// </summary>
        /// <param name="tRequestData"></param>
        internal bool Connect(RequestCommunicationData tRequestData)
        {
            m_RequestData = tRequestData;
            TelnetCommandInfo tTelnetCommandInfo = (TelnetCommandInfo)m_RequestData.RequestData;

            
            bool tRet = false;

            // 2013-03-06 - shinyn - SSH기능인경우 분기처리 추가
            if (tTelnetCommandInfo.DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.SSHTelnet)
            {
                tRet = m_Telnet.ConnectSSH2(tTelnetCommandInfo.DeviceInfo.IPAddress, tTelnetCommandInfo.DeviceInfo.TerminalConnectInfo.TelnetPort, 
                                            tTelnetCommandInfo.DeviceInfo.TelnetID1,
                                            tTelnetCommandInfo.DeviceInfo.TelnetPwd1);
            }
            else if (tTelnetCommandInfo.DeviceInfo.TerminalConnectInfo.ConnectionProtocol == E_ConnectionProtocol.TELNET)
            {
                tRet = m_Telnet.Connect(tTelnetCommandInfo.DeviceInfo.IPAddress, tTelnetCommandInfo.DeviceInfo.TerminalConnectInfo.TelnetPort);
            }
            return tRet;
        }

       

        /// <summary>
        /// 종료 처리 합니다.
        /// </summary>
        internal void Dispose()
        {
            m_IsRun = false;

            if (m_Telnet != null) m_Telnet.Dispose();
            lock(m_RequestQueue)
            {
                m_RequestQueue.Clear();
            }
            lock (m_ResultQueue)
            {
                m_ResultQueue.Clear();
            }
            try
            {
                if (m_RequestProcessThread != null && m_RequestProcessThread.IsAlive)
                {
                    m_RequestProcessThread.Abort();
                }
            }
            catch { }

            try
            {
                if (m_ResultProcessThread != null && m_ResultProcessThread.IsAlive)
                {
                    m_ResultProcessThread.Abort();
                }
            }
            catch { }
        }

        /// <summary>
        /// Connection Session ID 가져오거나 설정 합니다.
        /// </summary>
        public int ConnectionSessionID
        {
            get { return m_ConnectionSessionID; }
            set { m_ConnectionSessionID = value; }
        }
        /// <summary>
        /// Sender 가져오거나 설정 합니다.
        /// </summary>
        public ITelnetEmulator Sender
        {
            get { return m_Sender; }
            set { m_Sender = value; }
        }

        /// <summary>
        /// ClientID 가져오거나 설정 합니다.
        /// </summary>
        public int ClientID
        {
            get { return m_ClientID; }
            set { m_ClientID = value; }
        }

        /// <summary>
        /// 세션 상태 입니다.
        /// </summary>
        private E_SessionStatus m_SessionStatus = E_SessionStatus.TryConnect;

        private enum E_SessionStatus
        {
            TryConnect,
            Connect,
            Disconnect
        }

        internal void Disconnect(RequestCommunicationData aRequestCommunicationData)
        {
            TelnetCommandResultInfo tResult = new TelnetCommandResultInfo(aRequestCommunicationData);
            m_SessionStatus = E_SessionStatus.Disconnect;
            tResult.ReslutType = E_TelnetReslutType.DisConnected;
            if (m_Sender != null)
            {
                 m_Sender.DisplayResult(tResult);
            }
            else
            {
                lock (m_ResultQueue)
                {
                    m_ResultQueue.Enqueue(tResult);
                    m_ResultMRE.Set();
                }
            }
            if (OnSessionDisConnectEvent != null) OnSessionDisConnectEvent(m_ConnectionSessionID);
            
        }

        public void Disconnect()
        {
            TelnetCommandResultInfo tResult = new TelnetCommandResultInfo();
            m_SessionStatus = E_SessionStatus.Disconnect;
            tResult.ReslutType = E_TelnetReslutType.DisConnected;
            if (m_Sender != null)
            {
                m_Sender.DisplayResult(tResult);
            }
            else
            {
                lock (m_ResultQueue)
                {
                    m_ResultQueue.Enqueue(tResult);
                    m_ResultMRE.Set();
                }
            }
            if (OnSessionDisConnectEvent != null) OnSessionDisConnectEvent(m_ConnectionSessionID);
        }
    }
}
