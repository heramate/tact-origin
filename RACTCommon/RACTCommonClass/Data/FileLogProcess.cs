using System;
using System.Collections.Generic;
using System.Text;
using MKLibrary.MKLog;
using System.Threading;

namespace RACTCommonClass
{
    public class FileLogProcess
    {
        /// <summary>
        /// 파일을 저장할 로그 입니다.
        /// </summary>
        private MKFileLog m_FileLog = null;
        /// <summary>
        /// 로그가 저장될 큐 입니다.
        /// </summary>
        private Queue<FileLogInfo> m_LogQueue = null;
        /// <summary>
        /// 로그를 처리할 스레드 입니다.
        /// </summary>
        private Thread m_LogProcessThread = null;
        /// <summary>
        /// 실행 여부 입니다.
        /// </summary>
        private bool m_IsRun;

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public FileLogProcess(string aFilePath,string aFileName)
        {
            m_LogQueue = new Queue<FileLogInfo>();
            m_FileLog = new MKFileLog(aFilePath, aFileName, true, true);
        }

        /// <summary>
        /// 시작 합니다.
        /// </summary>
        public void Start()
        {
            m_IsRun = true;
            m_LogProcessThread = new Thread(new ThreadStart(ProcessLog));
            m_LogProcessThread.Start();
        }

        /// <summary>
        /// 종료 합니다.
        /// </summary>
        public void Stop()
        {
            m_IsRun = false;
            try
            {
                lock (m_LogQueue)
                {
                    m_LogQueue.Clear();
                }
                if (m_LogProcessThread != null && m_LogProcessThread.IsAlive)
                {
                    try
                    {
                        m_LogProcessThread.Abort();
                    }
                    catch { }
                    m_LogProcessThread = null;
                }
                m_FileLog.Dispose();
            }
            catch { }
        }

        /// <summary>
        /// 로그를 저장 합니다.
        /// </summary>
        /// <param name="aLogInfo"></param>
        public void PrintLog(E_FileLogType aLogType, string aLogMessage)
        {
            lock (m_LogQueue)
            {
                m_LogQueue.Enqueue(new FileLogInfo(aLogType, aLogMessage));
            }
        }
        /// <summary>
        /// 로그를 저장 합니다.
        /// </summary>
        /// <param name="aLogInfo"></param>
        public void PrintLog(string aLogMessage)
        {
            lock (m_LogQueue)
            {
                m_LogQueue.Enqueue(new FileLogInfo(E_FileLogType.Infomation, aLogMessage));
            }
        }

        /// <summary>
        /// 로그를 저장 합니다.
        /// </summary>
        /// <param name="aLogInfo"></param>
        public void PrintLog(FileLogInfo aLogInfo)
        {
            lock (m_LogQueue)
            {
                m_LogQueue.Enqueue(aLogInfo);
            }
        }



        /// <summary>
        /// 로그를 처리 합니다.
        /// </summary>
        private void ProcessLog()
        {
            FileLogInfo tLogInfo = null;

            // 2013-04-26 - shinyn - 로그저장시 Exception발생시 Exception로그 저장하도록 한다.
            while (m_IsRun)
            {
                try
                {
                    lock (m_LogQueue)
                    {
                        if (m_LogQueue.Count > 0)
                        {
                            tLogInfo = m_LogQueue.Dequeue();
                        }
                    }
                    if (tLogInfo != null)
                    {
                        m_FileLog.PrintLogEnter(string.Concat(tLogInfo.LogType.ToString(), " ", tLogInfo.Message));
                        tLogInfo = null;
                    }
                }
                catch (Exception ex)
                {
                }
                finally
                {
                }
                
                Thread.Sleep(1);
            }
        }
    }
}
