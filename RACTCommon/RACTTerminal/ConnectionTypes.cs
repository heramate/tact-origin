using System;
using System.Collections.Generic;
using System.Text;

namespace RACTTerminal
{
    /// <summary>
    /// 연결 타입 입니다.
    /// </summary>
    public enum ConnectionTypes
    {
        /// <summary>
        /// 로컬 텔넷 연결 입니다.
        /// </summary>
        LocalTelnet,
        /// <summary>
        /// 리모트 텔넷 연결 입니다.
        /// </summary>
        RemoteTelnet,
        /// <summary>
        /// 시리얼 연결 입니다.
        /// </summary>
        Serial
    }
}
