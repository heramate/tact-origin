using System;
using System.Collections.Generic;
using System.Text;

namespace RACTCommonClass
{
    /// <summary>
    /// 로그 분류입니다.
    /// </summary>
    public enum E_FileLogType
    {
        /// <summary>
        /// 오류를 기록합니다.
        /// </summary>		
        Error,
        /// <summary>
        /// 정보를 기록합니다.
        /// </summary>
        Infomation,
        Info,
        /// <summary>
        /// 경고 내용을 기록합니다.
        /// </summary>
        Warning,
    }
}
