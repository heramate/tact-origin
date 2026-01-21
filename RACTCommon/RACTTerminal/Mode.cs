using System;
using System.Collections.Generic;
using System.Text;

namespace RACTTerminal
{
    public class Mode
    {
        public static UInt32 s_Locked = 1;           // Unlocked           = off 
        public static UInt32 s_BackSpace = 2;           // Delete             = off 
        public static UInt32 s_NewLine = 4;           // Line Feed          = off 
        public static UInt32 s_Repeat = 8;           // No Repeat          = off 
        public static UInt32 s_AutoWrap = 16;          // No AutoWrap        = off 
        public static UInt32 s_CursorAppln = 32;          // Std Cursor Codes   = off 
        public static UInt32 s_KeypadAppln = 64;          // Std Numeric Codes  = off 
        public static UInt32 s_DataProcessing = 128;         // Typewriter         = off 
        public static UInt32 s_PositionReports = 256;         // CharacterCodes     = off
        public static UInt32 s_LocalEchoOff = 512;         // LocalEchoOn        = off
        public static UInt32 s_OriginRelative = 1024;        // OriginAbsolute     = off
        public static UInt32 s_LightBackground = 2048;        // DarkBackground     = off
        public static UInt32 s_National = 4096;        // Multinational      = off
        public static UInt32 s_Any = 0x80000000;  // Any Flags

        public UInt32 Flags;

        public Mode(System.UInt32 InitialFlags)
        {
            this.Flags = InitialFlags;
        }

        public Mode()
        {
            this.Flags = 0;
        }
    }
}
