using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace RACTTerminal
{
    /// <summary>
    /// 소켓 객체 입니다.
    /// </summary>
    public class SocketObject
    {

        /// <summary>
        /// Socket 입니다.
        /// </summary>
        private Socket m_Socket;

        /// <summary>
        /// Buffer 입니다.
        /// </summary>
        private byte[] m_Buffer;

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public SocketObject()
        {
            this.m_Buffer = new Byte[8192];
        }

        /// <summary>
        /// Buffer 가져오거나 설정 합니다.
        /// </summary>
        public byte[] Buffer
        {
            get { return m_Buffer; }
            set { m_Buffer = value; }
        }

        /// <summary>
        /// Socket 가져오거나 설정 합니다.
        /// </summary>
        public Socket Socket
        {
            get { return m_Socket; }
            set { m_Socket = value; }
        }
    }
}


         