using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace RACTTerminal
{
    public class Params
    {
        /// <summary>
        /// Char 리스트 입니다.
        /// </summary>
        private ArrayList m_Elements = new ArrayList();
        
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public Params(){}

        /// <summary>
        /// 갯수를 가져오기 합니다.
        /// </summary>
        /// <returns></returns>
        public Int32 Count()
        {
            return m_Elements.Count;
        }
        /// <summary>
        /// Clear 합니다.
        /// </summary>
        public void Clear()
        {
            m_Elements.Clear();
        }
        /// <summary>
        /// 문자를 더합니다.
        /// </summary>
        /// <param name="aCurChar">현재 문자 입니다.</param>
        public void Add(Char aCurChar)
        {
            if (this.Count() < 1)
            {
                this.m_Elements.Add("0");
            }

            if (aCurChar == ';')
            {
                this.m_Elements.Add("0");
            }
            else
            {
                int tIndex = this.m_Elements.Count - 1;
                this.m_Elements[tIndex] = ((string)this.m_Elements[tIndex] + aCurChar.ToString());
            }
        }
        /// <summary>
        /// Char 리스트를 가져오거나 설정 합니다.
        /// </summary>
        public ArrayList Elements
        {
            get { return m_Elements; }
            set { m_Elements = value; }
        }
    }
		   
}
