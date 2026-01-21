using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace RACTClient
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public class RACT
    {
        /// <summary>
        /// 전송 문자 발생 이벤트 입니다.
        /// </summary>
        public event HandlerArgument1<string> OnScriptSend;
        /// <summary>
        /// 대기 문자 발생 이벤트 입니다.
        /// </summary>
        public event HandlerArgument1<string> OnWaitForString;

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public RACT() { }
        /// <summary>
        /// 전송 문자를 처리 합니다.
        /// </summary>
        /// <param name="aObject"></param>
        public void Send(object aObject)
        {
            // 2013-05-02 - shinyn - 스크립트 로그 저장하지 않음
            //AppGlobal.s_FileLogProcessor.PrintLog("Send 스크립트 객체  -- "+aObject.ToString() +" -- ");
            if (OnScriptSend != null)
            {
                OnScriptSend(aObject.ToString());
            }
        }
        /// <summary>
        /// 수신 문자를 처리 합니다.
        /// </summary>
        /// <param name="aObject"></param>
        public void WaitForString(object aObject)
        {
            // 2013-05-02 - shinyn - 스크립트 로그 저장하지 않음
            //AppGlobal.s_FileLogProcessor.PrintLog("Wait 스크립트 객체  -- " + aObject.ToString() + " -- ");
            if (OnWaitForString != null)
            {
                OnWaitForString(aObject.ToString());
            }
        }
    }
}
