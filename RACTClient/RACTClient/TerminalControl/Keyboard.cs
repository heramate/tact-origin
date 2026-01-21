using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using RACTTerminal;

namespace RACTClient
{
    public class Keyboard
    {
        /// <summary>
        /// 키보드 이벤트 입니다.
        /// </summary>
        public event KeyboardEventHandler OnKeyboardEvent;
        /// <summary>
        /// 컨트롤키 이벤트 입니다.
        /// </summary>
        public event ControlKeyboardEventHandler OnControlKeyBoardEvent;
        /// <summary>
        /// 마지막 키 눌렀는지 여부 입니다.
        /// </summary>
        private bool m_LastKeyDownSent = false;
        /// <summary>
        /// Alt 키 누름 여부 입니다.
        /// </summary>
        private bool m_AltIsDown = false;
        /// <summary>
        /// Shift 키 누름 여부 입니다.
        /// </summary>
        private bool m_ShiftIsDown = false;
        /// <summary>
        /// Ctrl 키 누름 여부 입니다.
        /// </summary>
        private bool m_CtrlIsDown = false;

        public bool CtrlIsDown
        {
            get { return m_CtrlIsDown; }
            set { m_CtrlIsDown = value; }
        }
        /// <summary>
        /// 부모 입니다.
        /// </summary>
        private MCTerminalEmulator m_Parent;
        /// <summary>
        /// Key Map 입니다.
        /// </summary>
        private KeyMap KeyMap = new KeyMap();

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        /// <param name="aParent"></param>
        public Keyboard(MCTerminalEmulator aParent)
        {
            this.m_Parent = aParent;
        }
        /// <summary>
        /// 키보드 Down 처리 입니다.
        /// </summary>
        /// <param name="KeyMess"></param>
        public void KeyDown(System.Windows.Forms.Message aKeyMessage)
        {
            Byte[] tLPBytes;
            Byte[] tWPBytes;
            UInt16 tKeyValue = 0;
            UInt16 tRepeatCount = 0;
            Byte tScanCode = 0;
            Byte tAnsiChar = 0;
            UInt16 tUniChar = 0;
            Byte tFlags = 0;


            tLPBytes = BitConverter.GetBytes(aKeyMessage.LParam.ToInt64());
            tWPBytes = BitConverter.GetBytes(aKeyMessage.WParam.ToInt64());
            tRepeatCount = System.BitConverter.ToUInt16(tLPBytes, 0);
            tScanCode = tLPBytes[2];
            tFlags = tLPBytes[3];


            if (aKeyMessage.Msg == WMCodes.WM_SYSKEYDOWN || aKeyMessage.Msg == WMCodes.WM_KEYDOWN)
            {
                tKeyValue = System.BitConverter.ToUInt16(tWPBytes, 0);

                switch (tKeyValue)
                {
                    case 16:  // Shift Keys
                    case 160: // L Shift Key
                    case 161: // R Shift Keys
                        this.m_ShiftIsDown = true;
                        return;

                    case 17:  // Ctrl Keys
                    case 162: // L Ctrl Key
                    case 163: // R Ctrl Key
                        this.m_CtrlIsDown = true;
                        return;

                    case 18:  // Alt Keys (Menu)
                    case 164: // L Alt Key
                    case 165: // R Ctrl Key
                        this.m_AltIsDown = true;
                        return;
                }

                string tModifier = "Key";

                if (m_ShiftIsDown)
                {
                    tModifier = "Shift";
                }
                else if (m_CtrlIsDown)
                {
                    tModifier = "Ctrl";
                }
                else if (m_AltIsDown)
                {
                    tModifier = "Alt";
                }


                KeyInfo tControlKey = KeyMap.Find(tScanCode, Convert.ToBoolean(tFlags & 0x01), tModifier, m_Parent.Modes.Flags);

                this.m_LastKeyDownSent = false;

                if (tControlKey != null)
                {
                    this.m_LastKeyDownSent = true;
                    OnControlKeyBoardEvent(this, tControlKey);
                }

            }
            else if (aKeyMessage.Msg == WMCodes.WM_SYSKEYUP || aKeyMessage.Msg == WMCodes.WM_KEYUP)
            {
                tKeyValue = System.BitConverter.ToUInt16(tWPBytes, 0);

                switch (tKeyValue)
                {
                    case 16:  // Shift Keys
                    case 160: // L Shift Key
                    case 161: // R Shift Keys
                        this.m_ShiftIsDown = false;
                        break;

                    case 17:  // Ctrl Keys
                    case 162: // L Ctrl Key
                    case 163: // R Ctrl Key
                        this.m_CtrlIsDown = false;
                        break;

                    case 18:  // Alt Keys (Menu)
                    case 164: // L Alt Key
                    case 165: // R Ctrl Key
                        this.m_AltIsDown = false;
                        break;
                    case 37: //Left Keys
                    case 38: // UP Keys
                    case 39: //Right Keys
                    case 40://Down Keys
                        tAnsiChar = tWPBytes[0];
                        tUniChar = System.BitConverter.ToUInt16(tWPBytes, 0);

                        KeyInfo tControlKey = KeyMap.Find(tScanCode, Convert.ToBoolean(tFlags & 0x01), "Key", m_Parent.Modes.Flags);

                        this.m_LastKeyDownSent = false;

                        if (tControlKey != null)
                        {
                            this.m_LastKeyDownSent = true;
                            m_Parent.CheckPrompt();
                            if (m_Parent.TerminalStatus == E_TerminalStatus.Recording)
                            {
                                m_Parent.SaveWaitScript();
                                m_Parent.m_ScriptGenerator.AddSend(new TerminalScriptKeyInfo(System.Convert.ToChar(tAnsiChar).ToString(), E_TerminalScriptKeyType.Normal));
                            }
                            OnControlKeyBoardEvent(this, tControlKey);
                        }
                        break;
                    default:
                        break;
                }
            }
            else if (aKeyMessage.Msg == WMCodes.WM_SYSCHAR || aKeyMessage.Msg == WMCodes.WM_CHAR)
            {
                tAnsiChar = tWPBytes[0];
                tUniChar = System.BitConverter.ToUInt16(tWPBytes, 0);


                if (this.m_LastKeyDownSent == false)
                {
                    m_Parent.CheckPrompt();
                    if (m_Parent.TerminalStatus == E_TerminalStatus.Recording)
                    {
                        m_Parent.SaveWaitScript();
                        m_Parent.m_ScriptGenerator.AddSend(new TerminalScriptKeyInfo(System.Convert.ToChar(tAnsiChar).ToString(), E_TerminalScriptKeyType.Normal));
                    }
                    OnKeyboardEvent(this, System.Convert.ToChar(tAnsiChar).ToString());
                }
            }
        }
        /// <summary>
        /// Tab을 전송 합니다.
        /// </summary>
        public void SendTab()
        {
            OnKeyboardEvent(this, "\t");
        }
    }
}
