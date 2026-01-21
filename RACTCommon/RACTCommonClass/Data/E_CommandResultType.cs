using System;
using System.Collections.Generic;
using System.Text;

namespace RACTCommonClass
{
    /// <summary>
    /// 명령 결과 타입 열거형입니다.
    /// </summary>
    [Serializable]
    public enum E_CommandResultType
    {
        /// <summary>
        /// 명령 셋에 대한 결과 입니다.
        /// </summary>
        CommndSet,
        /// <summary>
        /// 작업그룹의 전체 결과를 보냅니다.
        /// </summary>
        CommandGroup,
        /// <summary>
        /// 명령 별로 결과를 보냅니다.
        /// </summary>
        CommandLine,
        /// <summary>
        /// 실시간으로 결과를 보냅니다.
        /// </summary>
        Realtime,
        /// <summary>
        /// 대기 결과 입니다.
        /// </summary>
        Waiting,
        /// <summary>
        /// 재부팅 핑결과 입니다.
        /// </summary>
        RebootPing,
    }
}
