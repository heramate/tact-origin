using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace RACTTerminal
{
    /// <summary>
    /// Key Map 입니다.
    /// </summary>
    public class KeyMap
    {
        public ArrayList m_Elements = new ArrayList();

        public KeyMap()
        {
            this.SetToDefault();
        }

        public void SetToDefault()
        {
            m_Elements.Clear();



            m_Elements.Add(new KeyInfo(15, false, "Shift", "\x1B[Z", Mode.s_Any, 0)); //ShTab
            m_Elements.Add(new KeyInfo(28, false, "Key", "\x0D", Mode.s_Any, 0)); //Return
            m_Elements.Add(new KeyInfo(28, true, "Key", "\x0D", Mode.s_Any, 0)); //Return
            m_Elements.Add(new KeyInfo(15, false, "Key", "\t", Mode.s_Any, 0)); //Return
            m_Elements.Add(new KeyInfo(59, false, "Key", "\x1BOP", Mode.s_Any, 0)); //F1->PF1
            m_Elements.Add(new KeyInfo(60, false, "Key", "\x1BOQ", Mode.s_Any, 0)); //F2->PF2
            m_Elements.Add(new KeyInfo(61, false, "Key", "\x1BOR", Mode.s_Any, 0)); //F3->PF3
            m_Elements.Add(new KeyInfo(62, false, "Key", "\x1BOS", Mode.s_Any, 0)); //F4->PF4
            m_Elements.Add(new KeyInfo(63, false, "Key", "\x1B[15~", Mode.s_Any, 0)); //F5
            m_Elements.Add(new KeyInfo(64, false, "Key", "\x1B[17~", Mode.s_Any, 0)); //F6
            m_Elements.Add(new KeyInfo(65, false, "Key", "\x1B[18~", Mode.s_Any, 0)); //F7
            m_Elements.Add(new KeyInfo(66, false, "Key", "\x1B[19~", Mode.s_Any, 0)); //F8
            m_Elements.Add(new KeyInfo(67, false, "Key", "\x1B[20~", Mode.s_Any, 0)); //F9
            m_Elements.Add(new KeyInfo(68, false, "Key", "\x1B[21~", Mode.s_Any, 0)); //F10
            m_Elements.Add(new KeyInfo(87, false, "Key", "\x1B[23~", Mode.s_Any, 0)); //F11
            m_Elements.Add(new KeyInfo(88, false, "Key", "\x1B[24~", Mode.s_Any, 0)); //F12
            m_Elements.Add(new KeyInfo(61, false, "Shift", "\x1B[25~", Mode.s_Any, 0)); //ShF3 ->F13
            m_Elements.Add(new KeyInfo(62, false, "Shift", "\x1B[26~", Mode.s_Any, 0)); //ShF4 ->F14
            m_Elements.Add(new KeyInfo(63, false, "Shift", "\x1B[28~", Mode.s_Any, 0)); //ShF5 ->F15
            m_Elements.Add(new KeyInfo(64, false, "Shift", "\x1B[29~", Mode.s_Any, 0)); //ShF6 ->F16
            m_Elements.Add(new KeyInfo(65, false, "Shift", "\x1B[31~", Mode.s_Any, 0)); //ShF7 ->F17
            m_Elements.Add(new KeyInfo(66, false, "Shift", "\x1B[32~", Mode.s_Any, 0)); //ShF8 ->F18
            m_Elements.Add(new KeyInfo(67, false, "Shift", "\x1B[33~", Mode.s_Any, 0)); //ShF9 ->F19
            m_Elements.Add(new KeyInfo(68, false, "Shift", "\x1B[34~", Mode.s_Any, 0)); //ShF10->F20
            m_Elements.Add(new KeyInfo(87, false, "Shift", "\x1B[28~", Mode.s_Any, 0)); //ShF11->Help
            m_Elements.Add(new KeyInfo(88, false, "Shift", "\x1B[29~", Mode.s_Any, 0)); //ShF12->Do
            m_Elements.Add(new KeyInfo(71, true, "Key", "\x1B[1~", Mode.s_Any, 0)); //Home
            m_Elements.Add(new KeyInfo(82, true, "Key", "\x1B[2~", Mode.s_Any, 0)); //Insert
            m_Elements.Add(new KeyInfo(83, true, "Key", "\x1B[3~", Mode.s_Any, 0)); //Delete
            m_Elements.Add(new KeyInfo(79, true, "Key", "\x1B[4~", Mode.s_Any, 0)); //End
            m_Elements.Add(new KeyInfo(73, true, "Key", "\x1B[5~", Mode.s_Any, 0)); //PageUp
            m_Elements.Add(new KeyInfo(81, true, "Key", "\x1B[6~", Mode.s_Any, 0)); //PageDown
            m_Elements.Add(new KeyInfo(72, true, "Key", "\x1B[A", Mode.s_CursorAppln, 0)); //CursorUp
            m_Elements.Add(new KeyInfo(80, true, "Key", "\x1B[B", Mode.s_CursorAppln, 0)); //CursorDown
            m_Elements.Add(new KeyInfo(77, true, "Key", "\x1B[C", Mode.s_CursorAppln, 0)); //CursorKeyRight
            m_Elements.Add(new KeyInfo(75, true, "Key", "\x1B[D", Mode.s_CursorAppln, 0)); //CursorKeyLeft
            m_Elements.Add(new KeyInfo(72, true, "Key", "\x1BOA", Mode.s_CursorAppln, 1)); //CursorUp
            m_Elements.Add(new KeyInfo(80, true, "Key", "\x1BOB", Mode.s_CursorAppln, 1)); //CursorDown
            m_Elements.Add(new KeyInfo(77, true, "Key", "\x1BOC", Mode.s_CursorAppln, 1)); //CursorKeyRight
            m_Elements.Add(new KeyInfo(75, true, "Key", "\x1BOD", Mode.s_CursorAppln, 1)); //CursorKeyLeft
            m_Elements.Add(new KeyInfo(82, false, "Key", "\x1BOp", Mode.s_KeypadAppln, 1)); //Keypad0
            m_Elements.Add(new KeyInfo(79, false, "Key", "\x1BOq", Mode.s_KeypadAppln, 1)); //Keypad1
            m_Elements.Add(new KeyInfo(80, false, "Key", "\x1BOr", Mode.s_KeypadAppln, 1)); //Keypad2
            m_Elements.Add(new KeyInfo(81, false, "Key", "\x1BOs", Mode.s_KeypadAppln, 1)); //Keypad3
            m_Elements.Add(new KeyInfo(75, false, "Key", "\x1BOt", Mode.s_KeypadAppln, 1)); //Keypad4
            m_Elements.Add(new KeyInfo(76, false, "Key", "\x1BOu", Mode.s_KeypadAppln, 1)); //Keypad5
            m_Elements.Add(new KeyInfo(77, false, "Key", "\x1BOv", Mode.s_KeypadAppln, 1)); //Keypad6
            m_Elements.Add(new KeyInfo(71, false, "Key", "\x1BOw", Mode.s_KeypadAppln, 1)); //Keypad7
            m_Elements.Add(new KeyInfo(72, false, "Key", "\x1BOx", Mode.s_KeypadAppln, 1)); //Keypad8
            m_Elements.Add(new KeyInfo(73, false, "Key", "\x1BOy", Mode.s_KeypadAppln, 1)); //Keypad9
            m_Elements.Add(new KeyInfo(74, false, "Key", "\x1BOm", Mode.s_KeypadAppln, 1)); //Keypad-
            m_Elements.Add(new KeyInfo(78, false, "Key", "\x1BOl", Mode.s_KeypadAppln, 1)); //Keypad+ (use instead of comma)
            m_Elements.Add(new KeyInfo(83, false, "Key", "\x1BOn", Mode.s_KeypadAppln, 1)); //Keypad.
            m_Elements.Add(new KeyInfo(28, true, "Key", "\x1BOM", Mode.s_KeypadAppln, 1)); //Keypad Enter
            m_Elements.Add(new KeyInfo(03, false, "Ctrl", "\x00", Mode.s_Any, 0)); //Ctrl2->Null
            m_Elements.Add(new KeyInfo(57, false, "Ctrl", "\x00", Mode.s_Any, 0)); //CtrlSpaceBar->Null
            m_Elements.Add(new KeyInfo(04, false, "Ctrl", "\x1B", Mode.s_Any, 0)); //Ctrl3->Escape
            m_Elements.Add(new KeyInfo(05, false, "Ctrl", "\x1C", Mode.s_Any, 0)); //Ctrl4->FS
            m_Elements.Add(new KeyInfo(06, false, "Ctrl", "\x1D", Mode.s_Any, 0)); //Ctrl5->GS
            m_Elements.Add(new KeyInfo(07, false, "Ctrl", "\x1E", Mode.s_Any, 0)); //Ctrl6->RS
            m_Elements.Add(new KeyInfo(08, false, "Ctrl", "\x1F", Mode.s_Any, 0)); //Ctrl7->US
            m_Elements.Add(new KeyInfo(09, false, "Ctrl", "\x7F", Mode.s_Any, 0)); //Ctrl8->DEL
            m_Elements.Add(new KeyInfo(14, false, "Key", "\x08", Mode.s_Any, 0)); //BackSpace->DEL
           // m_Elements.Add(new KeyInfo(53, false, "Shift", "?", Mode.s_Any, 0)); //BackSpace->DEL
           // m_Elements.Add(new KeyInfo(14, false, "Ctrl", "\x08", Mode.s_Any, 0)); //Ctrl + BackSpace->DEL
        }

        /// <summary>
        /// 해당 문자를 찾습니다.
        /// </summary>
        public KeyInfo Find(UInt16 aScanCode, Boolean aExtendFlag, string aModifier, UInt32 aModeFlags)
        {
            //string tOutstring = "";
            KeyInfo tElement;
            for (int i = 0; i < m_Elements.Count; i++)
            {
                 tElement = (KeyInfo)m_Elements[i];

                if (tElement.ScanCode == aScanCode &&
                    tElement.ExtendFlag == aExtendFlag &&
                    tElement.Modifier == aModifier &&
                    (tElement.Flag == Mode.s_Any ||
                    ((tElement.Flag & aModeFlags) == tElement.Flag && tElement.FlagValue == 1) ||
                    ((tElement.Flag & aModeFlags) == 0 && tElement.FlagValue == 0)))
                {
                    return tElement;
                }
            }

            return null;
        }
    }
}
