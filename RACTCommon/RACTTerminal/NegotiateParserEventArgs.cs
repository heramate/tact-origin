using System;
using System.Collections.Generic;
using System.Text;

namespace RACTTerminal
{
    /// <summary>
    /// Negotiate Parse Event Args 입니다.
    /// </summary>
    public struct NegotiateParserEventArgs
    {
        /// <summary>
        /// Action 입니다.
        /// </summary>
        public E_NegotiateActions m_Action;
        /// <summary>
        /// 현재 Char 입니다.
        /// </summary>
        public Char m_CurChar;
        /// <summary>
        /// 현재 Sequence 입니다.
        /// </summary>
        public string m_CurSequence;
       
        public Params m_CurParams;
     

        public NegotiateParserEventArgs(E_NegotiateActions aActions,Char aCurChar,string aCurSequence,Params aCurParams)
        {
            m_Action = aActions;
            m_CurChar = aCurChar;
            m_CurSequence = aCurSequence;
            m_CurParams = aCurParams;
        }
        public Params CurParams
        {
            get { return m_CurParams; }
            set { m_CurParams = value; }
        }
        public string CurSequence
        {
            get { return m_CurSequence; }
            set { m_CurSequence = value; }
        }
        public Char CurChar
        {
            get { return m_CurChar; }
            set { m_CurChar = value; }
        }
        public E_NegotiateActions Action
        {
            get { return m_Action; }
            set { m_Action = value; }
        }
    }
}
