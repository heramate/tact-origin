using System;
using System.Collections.Generic;
using System.Text;

namespace RACTClient
{
    public class DevicePartInfo
    {
        private string m_Name = "";
        private int m_ViewOrder = 0;
        /// <summary>
        /// 장비분류 코드
        /// </summary>
        private int m_Code = 0;
        //2024-05-13  추가 : 장비분류 코드 제어를 위해 추가(1 = IPTV, 2 = CATV, 3 = BOTH).
        /// <summary>
        /// 장비분류 코드
        /// </summary>
        private int m_IPTyep = 1;
        public DevicePartInfo(string aName, int aViewOrder)
        {
            m_Name = aName;
            m_ViewOrder = aViewOrder;
        }

        public DevicePartInfo(string aName, int aViewOrder, int aCode)
        {
            m_Name = aName;
            m_ViewOrder = aViewOrder;
            m_Code = aCode;
        }

        public DevicePartInfo(string aName, int aViewOrder, int aCode, int aIPTyep)
        {
            m_Name = aName;
            m_ViewOrder = aViewOrder;
            m_Code = aCode;
            m_IPTyep = aIPTyep;
        }

        /// <summary>
        /// 장비분류코드를 가져오거나 설정합니다.
        /// </summary>
        public int Code
        {
            get { return m_Code; }
            set { m_Code = value; }
        }

        public string Name
        {
            get { return m_Name; }
        }
        public int ViewOrder
        {
            get { return m_ViewOrder; }
        }
        public int IPTyep
        {
            get { return m_IPTyep; }
            set { m_IPTyep = value; }
        }
    }
}
