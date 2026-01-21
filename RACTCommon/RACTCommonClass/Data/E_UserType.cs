using System;
using System.Collections.Generic;
using System.Text;

namespace RACTCommonClass
{
    [Serializable]
    public enum E_UserType
    {
        /// <summary>
        /// 총괄관리자 (다산) -2
        /// </summary>
        Supervisor_Dasan = -2,
        /// <summary>
        /// 총괄관리자 = -1
        /// </summary>
        Supervisor = -1,
        /// <summary>
        /// 전체 관리자 = 0
        /// </summary>
        Admin_All = 0,
        /// <summary>
        /// 지역 관리자 = 1
        /// </summary>
        Admin_Area = 1,
        /// <summary>
        /// 지역 운용자 = 2
        /// </summary>
        Operator_Area = 2,
        /// <summary>
        /// 유지보수사 = 9
        /// </summary>
        Bp_Area = 9,
    }
}
