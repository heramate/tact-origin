using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using RACTCommonClass;

namespace RACTCommonClass
{
    /// <summary>
    /// 명령 전송 관련 인터페이스 입니다.
    /// </summary>
    public interface ISenderObject
    {
        //2010-11-15 hanjiyeon ResultReceiver 객체 변경 - 데몬->클라이언트로 데이터 직접전송.
        /// <summary>
        /// 결과 받기 메소드 입니다.
        /// </summary>
        /// <param name="vResult">결과 입니다.</param>
        void ResultReceiver(CommandResultItem vResult);

        void ResultReceiver(ResultCommunicationData vResult);

    }

    /// <summary>
    /// SenderObject에 대한 요약 설명입니다.
    /// </summary>
    public class SenderObject : IDisposable, ISenderObject
    {
        /// <summary>
        /// 임시 스래드 입니다.
        /// </summary>
        protected Thread m_SenderThread = null;
        /// <summary>
        /// 결과 저장 객체 입니다.
        /// </summary>
        protected CommandResultItem m_WorkResult = null;

        protected ResultCommunicationData m_Result = null;

        /// <summary>
        /// 사용자 지정 이벤트 객체 입니다.
        /// </summary>
        protected ManualResetEvent m_MRE = new ManualResetEvent(false);

        /// <summary>
        /// 전송 스래드를 시작 합니다.
        /// </summary>
        /// <param name="vThreadMethod"></param>
        public void StartSendThread(ThreadStart vThreadMethod)
        {
            m_SenderThread = new Thread(vThreadMethod);
            m_SenderThread.Start();
        }

        public virtual void ResultReceiver(ResultCommunicationData vResult)
        {
            if (vResult != null)
            {
                m_Result = vResult;
            }
            try
            {
                m_MRE.Set();
            }
            catch { }
        }

        public virtual void ResultReceiver(CommandResultItem vWorkResult)
        {
            if (vWorkResult != null)
            {
                m_WorkResult = vWorkResult;
            }
            try
            {
                m_MRE.Set();
            }
            catch { }
        }

        public void Dispose()
        {
            try
            {
                Console.WriteLine("**** MRE Disposed!! ****");
                m_MRE.Close();
            }
            catch { }

            try
            {
                //임시 스래드를 중지 합니다.
                if (m_SenderThread != null)
                {
                    if (m_SenderThread.IsAlive)
                    {
                        try
                        {
                            m_SenderThread.Abort();
                        }
                        catch { }
                    }
                }
            }
            catch { }
        }
    }
}
