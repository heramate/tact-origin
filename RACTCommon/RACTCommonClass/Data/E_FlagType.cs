using System;
using System.Collections.Generic;
using System.Text;

namespace RACTCommonClass
{
    [Serializable]
    public enum E_FlagType
    {
        /// <summary>
        /// FORMS에서 연동된 장비 입니다.
        /// </summary>
        FORMS = 0,
        /// <summary>
        /// 사용자가 입력한 장비 입니다.
        /// </summary>
        User,
    }
}
