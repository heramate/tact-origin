using System;
using System.Collections.Generic;
using System.Text;

namespace RACTCommonClass
{
    [Serializable]
    public enum E_DBLogType
    {
        /// <summary>
        /// 시스템 로그 입니다.
        /// </summary>
        SystemLog = 0,
        /// <summary>
        /// 사용자 로그인 로그아웃 로그 입니다.
        /// </summary>
        LoginLog,
        /// <summary>
        /// 장비에 접속한 로그 입니다.
        /// </summary>
        DeviceConnectionLog,
        /// <summary>
        /// 명령을 실행한 로그 입니다.
        /// </summary>
        ExecuteCommandLog

    }
}
