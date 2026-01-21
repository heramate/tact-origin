using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using RACTCommonClass;


namespace RACTSerialProcess
{
    public class SerialProcess
    {
        /// <summary>
        /// 세션에서 발생한 결과가 저장될 큐 입니다.
        /// </summary>
        public Queue<SerialCommandResultInfo> m_SerialResultQueue = new Queue<SerialCommandResultInfo>();
        /// <summary>
        /// 전송 큐 입니다.
        /// </summary>
        private Queue<SerialCommandRequestInfo> m_RequestQueue;
        /// <summary>
        /// 세션 결과 처리용 스레드 입니다.
        /// </summary>
        private Thread m_SerialRequestProcessThread;
        /// <summary>
        /// 시리얼 세션이 저장 됩니다.
        /// </summary>
        private Dictionary<string, SerialSessionInfo> m_SerialSessionList = new Dictionary<string, SerialSessionInfo>();
        /// <summary>
        /// 실행 여부 입니다.
        /// </summary>
        private bool m_IsRun = false;

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public SerialProcess() { }

        /// <summary>
        /// 시리얼 프로세서를 시작합니다.
        /// </summary>
        public void Start()
        {
            m_IsRun = true;
            m_RequestQueue = new Queue<SerialCommandRequestInfo>();
            m_SerialRequestProcessThread = new Thread(new ThreadStart(ProcessRequestData));
            m_SerialRequestProcessThread.Start();

        }
        /// <summary>
        /// 세션 정보를 가져오기 합니다.
        /// </summary>
        /// <param name="aComport"></param>
        /// <returns></returns>
        public ISerialEmulator GetSession(string aComport)
        {
            if (m_SerialSessionList.ContainsKey(aComport))
            {
                return m_SerialSessionList[aComport].Sender;
            }
            return null;
        }
        /// <summary>
        /// 명령 요청을 처리 합니다.
        /// </summary>
        private void ProcessRequestData()
        {
            SerialCommandRequestInfo tSerialCommandRequestInfo = null;
            while (m_IsRun)
            {
                lock (m_RequestQueue)
                {
                    if (m_RequestQueue.Count > 0)
                    {
                        tSerialCommandRequestInfo = m_RequestQueue.Dequeue();
                    }
                }

                if (tSerialCommandRequestInfo != null)
                {
                    if (m_SerialSessionList.ContainsKey(tSerialCommandRequestInfo.ComPort))
                    {
                        m_SerialSessionList[tSerialCommandRequestInfo.ComPort].SendData(tSerialCommandRequestInfo);
                    }
                    tSerialCommandRequestInfo = null;
                }
                else
                {
                    Thread.Sleep(1);
                }
            }
        }

        /// <summary>
        /// 장비에 접속 합니다.
        /// </summary>
        public bool ConnectDevice(ISerialEmulator aSender, SerialConfig aConfig)
        {
            if (!m_SerialSessionList.ContainsKey(aSender.ComPort))
            {
                SerialSessionInfo tSessionInfo = new SerialSessionInfo(aSender, aConfig);
                if (tSessionInfo.Connect())
                {
                    m_SerialSessionList.Add(aSender.ComPort, tSessionInfo);
                    return true;
                }
            }
            return false;
        }

        void tSessionInfo_OnSerialResultEvent(SerialCommandResultInfo aResult)
        {
            lock (m_SerialResultQueue)
            {
                m_SerialResultQueue.Enqueue(aResult);
            }
        }
        /// <summary>
        /// 명령을 전송 합니다.
        /// </summary>
        public void SendRequest(ISerialEmulator aSender, string aText)
        {
            lock (m_RequestQueue)
            {
                m_RequestQueue.Enqueue(new SerialCommandRequestInfo(aSender.ComPort, aText));
            }
        }
        /// <summary>
        /// 종료 처리를 합니다.
        /// </summary>
        /// <param name="aSender"></param>
        public bool DisconnectDevice(ISerialEmulator aSender)
        {
            if (m_SerialSessionList.ContainsKey(aSender.ComPort))
            {
                if (m_SerialSessionList[aSender.ComPort].DisConnect())
                {
                    m_SerialSessionList.Remove(aSender.ComPort);
                    return true;
                }
            }
            return false;
        }

        public void Dispose()
        {
            m_IsRun = false;
            try
            {
                foreach (SerialSessionInfo tSession in m_SerialSessionList.Values)
                {
                    tSession.Stop();
                }

                if (m_SerialRequestProcessThread != null && m_SerialRequestProcessThread.IsAlive)
                {
                    m_SerialRequestProcessThread.Abort();
                    m_SerialRequestProcessThread = null;
                }
            }
            catch { }
        }
    }
}
