using System;
using System.Collections.Generic;
using System.Text;

namespace RACTCommonClass
{
    /// <summary>
    /// 로그인 결과에 대한 열거형입니다.
    /// </summary>
    [Serializable]
    public enum E_LoginResult
    {
        /// <summary>
        /// 정상적으로 로그인되었음을 나타냅니다.
        /// </summary>
        Success,
        /// <summary>
        /// 잘못된 ID임을 나타냅니다.
        /// </summary>
        IncorrectID,
        /// <summary>
        /// 잘못된 Password임을 나타냅니다.
        /// </summary>
        IncorrectPassword,
        /// <summary>
        /// 이미 로그인 되어있음을 나타냅니다.
        /// </summary>
        AlreadyLogin,
        /// <summary>
        /// 알수없는 오류가 발생하였음을 나타냅니다.
        /// </summary>
        UnknownError,
        /// <summary>
        /// Server와 Client 버전이 일치하지 않습니다.
        /// </summary>
        VersionUnmatch,
        /// <summary>
        /// RACT 사용 권한이 없습니다.
        /// </summary>
        NotAuthentication,
        /// <summary>
        /// 접속 제한 날짜가 지났습니다.
        /// </summary>
        UnUsedLimit,
    }
}
