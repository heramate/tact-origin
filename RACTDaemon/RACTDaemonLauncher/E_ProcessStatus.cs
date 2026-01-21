using System;
using System.Collections.Generic;
using System.Text;

namespace RACTDaemonLauncher
{
    /// <summary>
    /// 서버 프로세스 상태 입니다.
    /// </summary>
    [Serializable]
    public enum E_ProcessStatus
    {
        /// <summary>
        /// 서버 프로세스가 활성화 되어있습니다.
        /// </summary>
        ProcessAlive = 0,
        /// <summary>
        /// 서버 프로세스가 종료 되었습니다.
        /// </summary>
        ProcessKill = 1,
        /// <summary>
        /// 서버 리모트 객체가 활성화 되었습니다.
        /// </summary>
        RemoteAlive = 2,
        /// <summary>
        /// 서버 리모트 객체가 종료 되었습니다.
        /// </summary>
        RemoteKill = 3,
        /// <summary>
        /// 서버에 로그인 성공 했습니다.
        /// </summary>
        LoginAlive = 4,
        /// <summary>
        /// 서버에 로그인 실패 했습니다.
        /// </summary>
        LoginKill = 5
    }
}
