using System;
using System.Collections.Generic;
using System.Text;

namespace RACTSerialProcess
{
    public interface ISerialEmulator
    {
        /// <summary>
        /// 시리얼 포트를 가져오기 합니다.
        /// </summary>
        string ComPort { get;}
        /// <summary>
        /// 결과를 표시 합니다.
        /// </summary>
        /// <param name="aResult"></param>
        void DisplayResult(SerialCommandResultInfo aResult);
    }

   
}
