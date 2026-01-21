using System;
using System.Collections.Generic;
using System.Text;

namespace RACTTerminal
{
    /// <summary>
    /// 협상 분석 이벤트 핸들러 입니다.
    /// </summary>
    public delegate void NegotiateParserEventHandler(object Sender, NegotiateParserEventArgs e);
    /// <summary>
    /// 키보드 이벤트 핸들러 입니다.
    /// </summary>
    public delegate void KeyboardEventHandler(object aSender, string aString);
    /// <summary>
    /// 키보드 이벤트 핸들러 입니다.
    /// </summary>
    public delegate void ControlKeyboardEventHandler(object aSender, KeyInfo aKeyMap);
    /// <summary>
    /// 새로고침 핸들러 입니다.
    /// </summary>
    public delegate void RefreshEventHandler();
    public delegate void RxdTextEventHandler(string sReceived);
    public delegate void CaretOffEventHandler();
    public delegate void CaretOnEventHandler();
    public delegate void ParserEventHandler(object Sender, ParserEventArgs e);
}
