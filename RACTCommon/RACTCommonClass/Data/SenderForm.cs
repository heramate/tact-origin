using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace RACTCommonClass
{
    public partial class SenderForm : Form, ISenderObject
    {
        protected ResultCommunicationData m_Result = null;
        protected CommandResultItem m_WorkResult = null;
        protected ManualResetEvent m_MRE = new ManualResetEvent(false);

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
    }
}
