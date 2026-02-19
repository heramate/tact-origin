using System;
using System.Collections.Generic;
using Rebex.Net;
using RACTCommonClass;

namespace RACTClient.Connectivity
{
    /// <summary>
    /// Jump Host에 대한 SSH 연결을 관리하고 공유하는 세션 풀입니다.
    /// </summary>
    public class SshSessionPool
    {
        private static readonly SshSessionPool _instance = new SshSessionPool();
        public static SshSessionPool Instance => _instance;

        // 세션 단절 시 UI 계층에 알리기 위한 이벤트
        public event Action<string> OnSessionDisconnected;

        private readonly Dictionary<string, SharedSshSession> _sessions = new Dictionary<string, SharedSshSession>();
        private readonly object _lock = new object();

        private SshSessionPool() { }

        public string GetKey(DeviceInfo info) => 
            $"{info.IPAddress}:{info.TerminalConnectInfo.TelnetPort}:{info.UserId}";

        public Ssh GetOrCreateSession(DeviceInfo jumpHost)
        {
            string key = GetKey(jumpHost);
            lock (_lock)
            {
                if (_sessions.TryGetValue(key, out var sharedSession) && sharedSession.Client.IsConnected)
                {
                    sharedSession.RefCount++;
                    return sharedSession.Client;
                }

                // 새 세션 생성
                var client = new Ssh();
                client.Disconnected += (s, e) => HandleUnexpectedDisconnection(key);

                client.Connect(jumpHost.IPAddress, jumpHost.TerminalConnectInfo.TelnetPort);
                client.Login(jumpHost.UserId, jumpHost.Password);

                _sessions[key] = new SharedSshSession { Client = client, RefCount = 1 };
                return client;
            }
        }

        public void ReleaseSession(DeviceInfo jumpHost)
        {
            string key = GetKey(jumpHost);
            lock (_lock)
            {
                if (_sessions.TryGetValue(key, out var sharedSession))
                {
                    sharedSession.RefCount--;
                    if (sharedSession.RefCount <= 0)
                    {
                        sharedSession.Client.Disconnect();
                        sharedSession.Client.Dispose();
                        _sessions.Remove(key);
                    }
                }
            }
        }

        private void HandleUnexpectedDisconnection(string key)
        {
            lock (_lock)
            {
                if (_sessions.ContainsKey(key))
                {
                    _sessions.Remove(key);
                    OnSessionDisconnected?.Invoke(key);
                }
            }
        }

        private class SharedSshSession
        {
            public Ssh Client { get; set; }
            public int RefCount { get; set; }
        }
    }
}
