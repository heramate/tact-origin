using System;
using System.Collections.Generic;
using System.Text;

namespace RACTTerminal
{
    public enum E_NegotiateCommand
    {
        SE = 240,
        NOP = 241,
        DM = 242,
        BREAK = 243,
        IP = 244,
        AO = 245,
        AYT = 246,
        EC = 247,
        EL = 248,
        GA = 249,
        SB = 250,
        WILL = 251,
        WONT = 252,
        DO = 253,
        DONT = 254,
        IAC = 255,
    }

    public enum E_NegotiateOption
    {
        ECHO = 1,   // echo
        SGA = 3,   // suppress go ahead
        STATUS = 5,   // status
        TM = 6,   // timing mark
        TTYPE = 24,  // terminal type
        NAWS = 31,  // window size
        TSPEED = 32,  // terminal speed
        LFLOW = 33,  // remote flow control
        LINEMODE = 34,  // linemode
        ENVIRON = 36,  // environment variables
    }
    /// <summary>
    /// negotiate Action 열거형 입니다.
    /// </summary>
    public enum E_NegotiateActions
    {
        None = 0,
        SendUp = 1,
        Dispatch = 2,
        Collect = 4,
        NewCollect = 5,
        Param = 6,
        Execute = 7,
    }

    public enum E_Actions
    {
        None = 0,
        Dispatch = 1,
        Execute = 2,
        Ignore = 3,
        Collect = 4,
        NewCollect = 5,
        Param = 6,
        OscStart = 8,
        OscPut = 9,
        OscEnd = 10,
        Hook = 11,
        Unhook = 12,
        Put = 13,
        Print = 14
    }
}
