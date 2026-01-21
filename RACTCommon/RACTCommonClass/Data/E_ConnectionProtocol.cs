using System;
using System.Collections.Generic;
using System.Text;

namespace RACTCommonClass
{
    /// <summary>
    /// 연결 프로토콜 타입입니다.
    /// </summary>
    [Serializable]
    public enum E_ConnectionProtocol
    {
        TELNET,
        SERIAL_PORT,
        /// <summary>
        /// 2013-01-22 - shinyn - SSH기능 추가
        /// </summary>
        SSHTelnet,
    }
}

