using System;
using System.ComponentModel;//MemberInfo
using System.Reflection;//DescriptionAttribute


namespace RACTCommonClass
{
    /// <summary>
    /// enum 관련 공용 클래스 추가
    /// 2019.09.25
    /// </summary>
    public static class EnumUtil
    {
        /// <summary>
        /// GetDescription() 메소드 사용을 위한 enum 정의 샘플
        /// </summary>
        //public enum FriendlyColorsEnum
        //{
        //    [Description("Blanched Almond Color")]
        //    BlanchedAlmond = 1,
        //    [Description("Dark Sea Green Color")]
        //    DarkSeaGreen = 2,
        //    [Description("Deep Sky Blue Color")]
        //    DeepSkyBlue = 3,
        //    [Description("Rosy Brown Color")]
        //    RosyBrown = 4
        //}

        /// <summary>
        /// enum 아이템 표시명 가져오기
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public static string GetDescription(Enum en)
        {
            Type type = en.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("파라미터가 Enum 타입이 아닙니다.", en.ToString());
            }

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                //해당 text 값을 배열로 추출해 옵니다.
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }
    }
}
