using System;
using System.Collections.Generic;
using System.Text;

namespace RACTCommonClass
{
    /// <summary>
    /// 오류 타입 열거형 입니다.
    /// </summary>
    [Serializable]
    public enum E_ErrorType : int
    {
        None = 0,
        /// <summary>
        /// 오류가 발생하지 았았음을 나타냅니다.
        /// </summary>
        NoError,
        /// <summary>
        /// 알수없는 오류가 발생하였음을 나타냅니다.
        /// </summary>
        UnKnownError,
        /// <summary>
        /// 타임 아웃이 발생하였습니다.
        /// </summary>
        Timeout,
        /// <summary>
        /// 장비 접속 타임아웃이 발생하였습니다.
        /// </summary>
        ConnectionTimeout,
        /// <summary>
        /// 비지니스 로직 에러
        /// </summary>
        LogicError
    }
}
