using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MessagePack;

namespace RACTCommonClass
{
    public interface ISenderObject
    {
        void ResultReceiver(CommandResultItem vResult);
        void ResultReceiver(ResultCommunicationData vResult);
    }

    [Serializable]
    [MessagePackObject]
    public class SenderObject : IDisposable, ISenderObject
    {
        protected Thread m_SenderThread = null;
        [Key(0)] protected CommandResultItem m_WorkResult = null;
        [Key(1)] protected ResultCommunicationData m_Result = null;
        protected ManualResetEvent m_MRE = new ManualResetEvent(false);

        public void StartSendThread(ThreadStart vThreadMethod)
        {
            m_SenderThread = new Thread(vThreadMethod);
            m_SenderThread.Start();
        }

        public virtual void ResultReceiver(ResultCommunicationData vResult)
        {
            if (vResult != null) m_Result = vResult;
            try { m_MRE.Set(); } catch { }
        }

        public virtual void ResultReceiver(CommandResultItem vResult)
        {
            if (vResult != null) m_WorkResult = vResult;
            try { m_MRE.Set(); } catch { }
        }

        public virtual void Wait() { m_MRE.WaitOne(); }
        public virtual void Wait(int aTimeOut) { m_MRE.WaitOne(aTimeOut); }

        public CommandResultItem WorkResult { get { return m_WorkResult; } set { m_WorkResult = value; } }
        public ResultCommunicationData Result { get { return m_Result; } set { m_Result = value; } }

        public void Dispose()
        {
            try { m_MRE.Close(); } catch { }
            if (m_SenderThread != null && m_SenderThread.IsAlive)
            {
                try { m_SenderThread.Join(500); if (m_SenderThread.IsAlive) m_SenderThread.Interrupt(); } catch { }
            }
            m_SenderThread = null;
        }
    }
}
