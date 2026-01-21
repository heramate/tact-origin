using System;
using System.Collections.Generic;
using System.Text;

namespace RACTTerminal
{
    public class TabStops
    {
        /// <summary>
        /// 컬럼 입니다.
        /// </summary>
        private bool[] m_Columns;
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public TabStops()
        {
            m_Columns = new System.Boolean[256];

            m_Columns[8] = true;
            m_Columns[16] = true;
            m_Columns[24] = true;
            m_Columns[32] = true;
            m_Columns[40] = true;
            m_Columns[48] = true;
            m_Columns[56] = true;
            m_Columns[64] = true;
            m_Columns[72] = true;
            m_Columns[80] = true;
            m_Columns[88] = true;
            m_Columns[96] = true;
            m_Columns[104] = true;
            m_Columns[112] = true;
            m_Columns[120] = true;
            m_Columns[128] = true;
        }

        /// <summary>
        /// 컬럼을 가져 오거나 설정 합니다.
        /// </summary>
        public bool[] Columns
        {
            get { return m_Columns; }
            set { m_Columns = value; }
        }
    }
}
