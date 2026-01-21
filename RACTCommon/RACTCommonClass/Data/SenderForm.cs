using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using RACTCommonClass;
using DevComponents.DotNetBar;

namespace RACTCommonClass
{
   /// <summary>
	/// SenderForm에 대한 요약 설명입니다.
	/// </summary>
    public partial class SenderForm : Office2007Form, ISenderObject
	{


		public SenderForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// 사용 중인 모든 리소스를 정리합니다.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			try
			{
				Console.WriteLine("**** MRE Disposed!! ****");
				m_MRE.Close();
			}
			catch {}

			try
			{
				//임시 스래드를 중지 합니다.
				if(m_SenderThread != null)
				{
					if(m_SenderThread.IsAlive)
					{
						try
						{
							m_SenderThread.Abort();
						}
						catch {}
					}
				}
			}
			catch {}

			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
        /// <summary>
        /// 임시 스래드 입니다.
        /// </summary>
        protected Thread m_SenderThread = null;
        /// <summary>
        /// 결과 저장 객체 입니다.
        /// </summary>
        protected ResultCommunicationData m_Result = null;

        protected CommandResultItem m_WorkResult = null;

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
            FormEnable(false);
            m_SenderThread = new Thread(vThreadMethod);
            m_SenderThread.Start();
        }

        public virtual void ResultReceiver(CommandResultItem vWorkResult)
        {
            if (vWorkResult != null)
            {
                //m_Result = vResult;				
                m_WorkResult = vWorkResult;
            }
            try
            {
                m_MRE.Set();
            }
            catch { }
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

        public virtual void FormEnable(bool vEnabled) { }
    }
}
