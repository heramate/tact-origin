using System;
using System.Collections.Generic;
using System.Text;

namespace RACTCommonClass
{
    public enum E_ConnectError
    {
        /// <summary>
        /// 연결 성공입니다.
        /// </summary>
        NoError,
        /// <summary>
        /// 연결은 가능하나 서버에 접속할수없음.
        /// </summary>
        ServerNoRun,
        /// <summary>
        /// 네트워크 연결이 되지 않음.
        /// </summary>
        LinkFail,
        LocalFail
    }
}
