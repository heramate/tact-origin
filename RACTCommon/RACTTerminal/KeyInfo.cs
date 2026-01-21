using System;
using System.Collections.Generic;
using System.Text;

namespace RACTTerminal
{
    /// <summary>
    /// 키 정보 입니다.
    /// </summary>
    [Serializable]
    public class KeyInfo
    {
        /// <summary>
        /// Scan Code 입니다.
        /// </summary>
        private UInt16 m_ScanCode;
        /// <summary>
        /// Extend Flag 입니다.
        /// </summary>
        private Boolean m_ExtendFlag;
        /// <summary>
        /// Modifier 입니다.
        /// </summary>
        private string m_Modifier;
        /// <summary>
        /// Out String 입니다.
        /// </summary>
        private string m_Outstring;
        /// <summary>
        /// Flag 입니다.
        /// </summary>
        private UInt32 m_Flag;
        /// <summary>
        /// Flag Value 입니다.
        /// </summary>
        private UInt32 m_FlagValue;
       
        /// <summary>
        /// 기본 성자 입니다.
        /// </summary>
        public KeyInfo(UInt16 aScanCode, Boolean aExtendFlag, string aModifier, string aOutstring, UInt32 aFlag, UInt32 aFlagValue)
        {
            m_ScanCode = aScanCode;
            m_ExtendFlag = aExtendFlag;
            m_Modifier = aModifier;
            m_Outstring = aOutstring;
            m_Flag = aFlag;
            m_FlagValue = aFlagValue;
        }

        public UInt16 ScanCode
        {
            get { return m_ScanCode; }
            set { m_ScanCode = value; }
        }

        public Boolean ExtendFlag
        {
            get { return m_ExtendFlag; }
            set { m_ExtendFlag = value; }
        }

        public string Modifier
        {
            get { return m_Modifier; }
            set { m_Modifier = value; }
        }


        public string Outstring
        {
            get { return m_Outstring; }
            set { m_Outstring = value; }
        }


        public UInt32 Flag
        {
            get { return m_Flag; }
            set { m_Flag = value; }
        }


        public UInt32 FlagValue
        {
            get { return m_FlagValue; }
            set { m_FlagValue = value; }
        }
    }
}
