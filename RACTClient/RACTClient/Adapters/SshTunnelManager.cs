using Rebex.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

public static class SshTunnelManager
{
    private class TunnelSession
    {
        public Ssh SshClient;
        public SshTunnel SocksServer;
        public int ReferenceCount; // 탭에서 이 세션을 참조하는 수
        public int LocalPort;      // 할당된 로컬 SOCKS5 포트
        public int ChannelCount;   // SSH 세션 내 오픈된 채널(세션) 수
    }

    // Key: "Host:Port:User", Value: 해당 점프 서버로 연결된 세션들의 리스트(풀)
    private static readonly Dictionary<string, List<TunnelSession>> _sessionPool = new Dictionary<string, List<TunnelSession>>();
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    // SSH 서버 설정에 따른 채널 제한 (보통 10개이나 안전하게 8개로 설정)
    private const int MAX_CHANNELS_PER_SESSION = 8;

    public static string CreateKey(List<string> hosts, int port, string user)
        => (hosts != null && hosts.Count > 0) ? $"{hosts[0]}:{port}:{user}" : null;

    /// <summary>
    /// 터널을 획득하거나 새로 생성합니다. (I/O 분리형 최종 버전)
    /// </summary>
    public static async Task<(int Port, string Key)> GetOrCreateTunnelAsync(List<string> hosts, int port, string user, string pwd)
    {
        string key = CreateKey(hosts, port, user);
        if (string.IsNullOrEmpty(key)) throw new ArgumentException("잘못된 점프 서버 정보입니다.");

        // [단계 1] 기존 풀에서 여유가 있는 세션이 있는지 확인 (세마포어 짧게 점유)
        await _semaphore.WaitAsync();
        try
        {
            if (!_sessionPool.ContainsKey(key)) _sessionPool[key] = new List<TunnelSession>();

            var existingSession = _sessionPool[key].FirstOrDefault(s =>
                s.SshClient.IsConnected && s.ChannelCount < MAX_CHANNELS_PER_SESSION);

            if (existingSession != null)
            {
                existingSession.ReferenceCount++;
                existingSession.ChannelCount++;
                return (existingSession.LocalPort, key);
            }
        }
        finally { _semaphore.Release(); }

        // [단계 2] 새로운 SSH 연결 시도 (세마포어 밖에서 수행 - 핵심!)
        // 여기서 발생하는 지연(서버 풀 상태 등)은 다른 탭의 자원 해제나 조회를 방해하지 않습니다.
        foreach (var host in hosts)
        {
            try
            {
                var ssh = new Ssh { Timeout = 5000 }; // 5초 타임아웃

                await Task.Run(() => {
                    ssh.Connect(host, port);
                    ssh.Login(user, pwd);
                });

                var socks = ssh.StartSocksServer("127.0.0.1", 0); // 다이나믹 포트 할당
                int allocatedPort = ((IPEndPoint)socks.LocalEndPoint).Port;

                var newSession = new TunnelSession
                {
                    SshClient = ssh,
                    SocksServer = socks,
                    ReferenceCount = 1,
                    ChannelCount = 1,
                    LocalPort = allocatedPort
                };

                // [단계 3] 새로 만든 세션을 풀에 추가 (다시 세마포어 점유)
                await _semaphore.WaitAsync();
                try
                {
                    _sessionPool[key].Add(newSession);
                }
                finally { _semaphore.Release(); }

                return (allocatedPort, key);
            }
            catch (Exception ex)
            {
                // 개별 접속 실패 로그 기록 (기존 세션은 건드리지 않음)
                System.Diagnostics.Debug.WriteLine($"[Tunnel] Connect failed for {host}: {ex.Message}");
            }
        }

        throw new Exception("서버 연결 수 초과 또는 네트워크 오류로 인해 터널을 생성할 수 없습니다.");
    }

    /// <summary>
    /// 점유 중인 채널 및 세션을 안전하게 해제합니다.
    /// </summary>
    public static async Task ReleaseTunnelAsync(string key, int localPort)
    {
        await _semaphore.WaitAsync();
        try
        {
            if (string.IsNullOrEmpty(key) || !_sessionPool.TryGetValue(key, out var pool)) return;

            var session = pool.FirstOrDefault(s => s.LocalPort == localPort);
            if (session != null)
            {
                session.ReferenceCount--;
                session.ChannelCount--;

                // 참조하는 탭이 없고 채널도 사용 중이지 않을 때만 실제 종료
                if (session.ReferenceCount <= 0)
                {
                    try { session.SocksServer?.Close(); } catch { }
                    try { session.SshClient?.Dispose(); } catch { }
                    pool.Remove(session);
                    if (pool.Count == 0) _sessionPool.Remove(key);
                }
            }
        }
        finally { _semaphore.Release(); }
    }
}