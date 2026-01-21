using System;
using System.Collections.Generic;
using System.Text;

namespace RACTClient
{
    public interface IOptionPanal
    {
        /// <summary>
        /// 컨트롤을 초기화 합니다.
        /// </summary>
        void InitializeControl();
        /// <summary>
        /// 옵션을 저장 합니다.
        /// </summary>
        bool SaveOption();
    }
}
