using System;
using System.Collections.Concurrent;
using Serilog;

namespace TACT.KeepAliveServer
{
    public class KeepAliveCommProcess : IDisposable
    {
        private readonly int _listenPort;
        private readonly ConcurrentDictionary<string, KeepAliveMsg> _sendReply = new();

        public KeepAliveCommProcess(int listenPort)
        {
            _listenPort = listenPort;
        }

        public void Start()
        {
            Log.Information("KeepAliveCommProcess started (Modernized stub). UDP reception is handled by Service.");
        }

        public void Stop()
        {
            Log.Information("KeepAliveCommProcess stopped.");
        }

        public void Dispose() => Stop();

        public void AddSendReply(KeepAliveMsg msg)
        {
            if (msg.DeviceIP != null)
                _sendReply[msg.DeviceIP] = msg;
        }
    }
}