using System;
using System.Collections.Generic;
using System.Text;

namespace RACTClient
{
    public class TelnetStringFindHandlerArgs
    {

        /// <summary>
        /// 찾을 문자 입니다. 입니다.
        /// </summary>
        private string m_FindString;

        /// <summary>
        /// 대/소문자 구분 입니다.
        /// </summary>
        private bool m_IsMatchCase;

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        /// <param name="aFindString">찾을 문자 입니다.</param>
        /// <param name="aIsMatchCase">대소문자 구분 여부 입니다.</param>
        public TelnetStringFindHandlerArgs(string aFindString, bool aIsMatchCase)
        {
            m_FindString = aFindString;
            m_IsMatchCase = aIsMatchCase; 
        }

        /// <summary>
        /// 대/소문자 구분 가져오거나 설정 합니다.
        /// </summary>
        public bool IsMatchCase
        {
            get { return m_IsMatchCase; }
            set { m_IsMatchCase = value; }
        }


        /// <summary>
        /// 찾을 문자 입니다. 가져오거나 설정 합니다.
        /// </summary>
        public string FindString
        {
            get { return m_FindString; }
            set { m_FindString = value; }
        }

    }
}
