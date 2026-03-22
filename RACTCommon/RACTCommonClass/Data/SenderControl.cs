using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace RACTCommonClass
{
    public partial class SenderControl : UserControl, ISenderObject
    {
        protected ResultCommunicationData m_Result = null;
        protected CommandResultItem m_WorkResult = null;

        public virtual void ResultReceiver(ResultCommunicationData vResult)
        {
            if (vResult != null) m_Result = vResult;
        }

        public virtual void ResultReceiver(CommandResultItem vResult)
        {
            if (vResult != null) m_WorkResult = vResult;
        }
    }
}
