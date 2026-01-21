using System;
using System.Collections.Generic;
using System.Text;

namespace RACTClient
{
    public enum E_ScriptWorkType
    {
        /// <summary>
        /// 기본 상태 입니다.
        /// </summary>
        None,
        /// <summary>
        /// 실행 입니다.
        /// </summary>
        Run,
        /// <summary>
        /// 실행 취소 입니다.
        /// </summary>
        RunCancel,
        /// <summary>
        /// 레코딩 입니다.
        /// </summary>
        Rec,
        /// <summary>
        /// 레코딩 취소 입니다.
        /// </summary>
        RecCancel,
        /// <summary>
        /// 저장 입니다.
        /// </summary>
        Save,
        /// <summary>
        /// 로그 저장 입니다.
        /// </summary>
        RecLog
    }
}
