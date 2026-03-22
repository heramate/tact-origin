using RACTCommonClass;
using Rebex.Net;
using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace RACTClient.Models
{
    public class TargetInfo
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public E_ConnectionProtocol Protocol { get; set; } // "SSH", "TELNET", "SERIAL"
        public string Username { get; set; }
        public string Password { get; set; }
        public bool UseBastion { get; set; } = true;

        // Serial Settings
        public string PortName { get; set; } // e.g., "COM1"
        public int BaudRate { get; set; } = 9600;
        public int DataBits { get; set; } = 8;
        public Parity Parity { get; set; } = Parity.None;
        public StopBits StopBits { get; set; } = StopBits.One;
        public Handshake Handshake { get; set; } = Handshake.None;
    }

    public class BastionHostInfo
    {
        public string Host { get; set; }
        public int Port { get; set; } = 22; // 기본값 22
    }

    public class BastionConfig
    {
        // 여러 서버 정보를 담을 수 있도록 수정
        public List<BastionHostInfo> Hosts { get; set; } = new List<BastionHostInfo>();
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class SessionContext : IDisposable
    {
        private readonly SshTunnel _forwarding;
        public object TerminalSession { get; private set; }

        /// <summary>
        /// 세션 고유 ID
        /// </summary>
        public int SessionID { get; set; }

        public SessionContext(SshTunnel forwarding, object session)
        {
            _forwarding = forwarding;
            TerminalSession = session;
        }

        public void Dispose()
        {
            if (TerminalSession is Ssh ssh)
            {
                if (ssh.IsConnected) ssh.Disconnect();
                ssh.Dispose();
            }
            else if (TerminalSession is IDisposable disposable)
            {
                disposable.Dispose();
            }

            if (_forwarding != null)
            {
                _forwarding.Close();
                if (_forwarding is IDisposable disposableForwarding)
                {
                    disposableForwarding.Dispose();
                }
            }
        }
    }
}
