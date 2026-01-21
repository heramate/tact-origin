using System;
using System.Collections.Generic;
using System.Text;

namespace RACTClient
{
    public class TelnetStringFind
    {
        /// <summary>
        /// Fine List 입니다.
        /// </summary>
        private List<StringFindInfo> m_FindList;
        /// <summary>
        /// 찾을 문자 원형 입니다.
        /// </summary>
        private string m_String = "";

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        /// <param name="aString"></param>
        public TelnetStringFind(string aString)
        {
            m_String = aString;
            m_FindList = new List<StringFindInfo>();
            for (int i = 0; i < aString.Length; i++)
            {
                m_FindList.Add(new StringFindInfo(aString.Substring(i, 1)));
            }
        }

        /// <summary>
        /// 전체 목록을 찾았는지 여부 입니다.
        /// </summary>
        public bool IsMatch
        {
            get
            {
                foreach (StringFindInfo tInfo in m_FindList)
                {
                    if (!tInfo.IsMatch) return false;
                }
                return true;
            }
        }
        public int GetCheckIndex
        {
            get
            {
                int tIndex = 0;
                for (int i = 0; i < FindList.Count; i++)
                {
                    if (!FindList[i].IsMatch)
                    {
                        tIndex = i;
                        break;
                    }
                }
                return tIndex;
            }

        }
        /// <summary>
        /// Fine List 가져오거나 설정 합니다.
        /// </summary>
        public List<StringFindInfo> FindList
        {
            get { return m_FindList; }
            set { m_FindList = value; }
        }
    }
    public class StringFindInfo
    {
        /// <summary>
        /// 찾을 문자 입니다.
        /// </summary>
        private string m_FindText;

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        /// <param name="aString"></param>
        public StringFindInfo(string aString)
        {
            m_FindText = aString;
        }

        /// <summary>
        /// Row 입니다.
        /// </summary>
        private int m_Row = -1;

        /// <summary>
        /// Col 입니다.
        /// </summary>
        private int m_Col = -1;

        /// <summary>
        /// Col 가져오거나 설정 합니다.
        /// </summary>
        public int Col
        {
            get { return m_Col; }
            set { m_Col = value; }
        }


        /// <summary>
        /// Row 가져오거나 설정 합니다.
        /// </summary>
        public int Row
        {
            get { return m_Row; }
            set { m_Row = value; }
        }

        /// <summary>
        /// 찾았는지 여부를 가져오기 합니다.
        /// </summary>
        public bool IsMatch
        {
            get { return Row > -1 && Col > -1; }
        }

        /// <summary>
        /// 찾을 문자 가져오거나 설정 합니다.
        /// </summary>
        public string FindText
        {
            get { return m_FindText; }
            set { m_FindText = value; }
        }
    }
}
