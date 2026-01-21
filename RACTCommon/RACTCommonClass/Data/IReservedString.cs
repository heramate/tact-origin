using System;
using System.Collections.Generic;
using System.Text;
using RACTCommonClass;

namespace RACTCommonClass
{
    public interface IReservedString
    {
        /// <summary>
        /// 예약어를 처리 합니다.
        /// </summary>
        /// <param name="aDeviceInfo">장비 정보 입니다.</param>
        /// <returns>처리된 결과 입니다.</returns>
        string Process(string aSoruceString, DeviceInfo aDeviceInfo);
        /// <summary>
        /// 예약어를 처리 합니다.
        /// </summary>
        /// <param name="aSourceString">원본 문자 입니다.</param>
        /// <param name="aCheckString">확인할 문자 입니다.</param>
        /// <param name="aIsMatch">비교 타입 입니다.</param>
        /// <returns>참인지 여부 입니다.</returns>
        bool Process(string aSourceString, string aCheckString, bool aIsMatch);


    }
}
