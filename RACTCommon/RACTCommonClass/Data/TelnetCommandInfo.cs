using System;
using System.Collections.Generic;
using System.Text;
using RACTTerminal;

namespace RACTCommonClass
{
    [Serializable]
    public class TelnetCommandInfo
    {

        /// <summary>
        /// DeviceInfo 입니다.
        /// </summary>
        private DeviceInfo m_DeviceInfo;
        /// <summary>
        /// 전송할 명령 입니다.
        /// </summary>
        private string m_Command;
        /// <summary>
        /// Command 타입 입니다.
        /// </summary>
        private E_TelnetWorkType m_WorkTyp = E_TelnetWorkType.Connect;
        /// <summary>
        /// 사용자 ID 입니다.
        /// </summary>
        private int m_UserID;

        /// <summary>
        /// KeyMap 입니다.
        /// </summary>
        private KeyInfo m_KeyInfo;

        /// <summary>
        /// Session ID 입니다.
        /// </summary>
        private int m_SessionID;

        /// <summary>
        /// Sender 입니다.
        /// </summary>
        private ITelnetEmulator m_Sender;

        /// <summary>
        /// Sender 가져오거나 설정 합니다.
        /// </summary>
        public ITelnetEmulator Sender
        {
            get { return m_Sender; }
            set { m_Sender = value; }
        }

        /// <summary>
        /// Session ID 가져오거나 설정 합니다.
        /// </summary>
        public int SessionID
        {
            get { return m_SessionID; }
            set { m_SessionID = value; }
        }

        /// <summary>
        /// KeyMap 가져오거나 설정 합니다.
        /// </summary>
        public KeyInfo KeyInfo
        {
            get { return m_KeyInfo; }
            set { m_KeyInfo = value; }
        }


        /// <summary>
        /// 사용자 ID 가져오거나 설정 합니다.
        /// </summary>
        public int UserID
        {
            get { return m_UserID; }
            set { m_UserID = value; }
        }

        /// <summary>
        /// DeviceInfo 가져오거나 설정 합니다.
        /// </summary>
        public DeviceInfo DeviceInfo
        {
            get { return m_DeviceInfo; }
            set { m_DeviceInfo = value; }
        }

        /// <summary>
        /// Command 타입 가져오거나 설정 합니다.
        /// </summary>
        public E_TelnetWorkType WorkTyp
        {
            get { return m_WorkTyp; }
            set { m_WorkTyp = value; }
        }

        /// <summary>
        /// 전송할 명령 가져오거나 설정 합니다.
        /// </summary>
        public string Command
        {
            get { return m_Command; }
            set { m_Command = value; }
        }

      
    }
    /// <summary>
    /// Telnet WorkType 입니다.
    /// </summary>
    [Serializable]
    public enum E_TelnetWorkType
    {
        Connect,
        Disconnect,
        Execute

    }
}
