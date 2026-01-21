using System;
using System.Collections.Generic;
using System.Text;

namespace RACTTerminal
{
    /// <summary>
    /// ParserEventArgs 입니다.
    /// </summary>
    public struct ParserEventArgs
    {
        /// <summary>
        /// Action 입니다.
        /// </summary>
        private E_Actions m_Action;
        /// <summary>
        /// Char 입니다.
        /// </summary>
        private Char m_CurChar;
        /// <summary>
        /// Sequence 입니다.
        /// </summary>
        private string m_CurSequence;
        /// <summary>
        /// Params 입니다.
        /// </summary>
        private Params m_CurParams;

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public ParserEventArgs(E_Actions aActions, Char aChar,string aString,Params aParams)
        {
            m_Action = aActions;
            m_CurChar = aChar;
            m_CurSequence = aString;
            m_CurParams = aParams;
        }
        /// <summary>
        /// Action 입니다.
        /// </summary>
        public E_Actions Action
        {
            get { return m_Action; }
            set { m_Action = value; }
        }
        /// <summary>
        /// Char 입니다.
        /// </summary>
        public Char CurChar
        {
            get { return m_CurChar; }
            set { m_CurChar = value; }
        }
        /// <summary>
        /// Sequence 입니다.
        /// </summary>
        public string CurSequence
        {
            get { return m_CurSequence; }
            set { m_CurSequence = value; }
        }
        /// <summary>
        /// Params 입니다.
        /// </summary>
        public Params CurParams
        {
            get { return m_CurParams; }
            set { m_CurParams = value; }
        }
    }
}
