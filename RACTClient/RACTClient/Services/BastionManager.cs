using RACTClient.Models;
using RACTCommonClass;
using Rebex.Net;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace RACTClient.Services
{
    public class BastionManager
    {
        private static readonly Lazy<BastionManager> _instance = new Lazy<BastionManager>(() => new BastionManager());
        public static BastionManager Instance => _instance.Value;

        private Ssh _bastionSession;
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
        private int _referenceCount = 0;

        // 외부에서 주입받을 설정 객체
        private BastionConfig _config;

        private Timer _keepAliveTimer;
        private string ERR_GATEWAY_FAILED = $"Unable to establish a secure tunnel.{Environment.NewLine}Please verify your network permissions.";

        private BastionManager() { }

        /// <summary>
        /// Bastion 설정을 초기화합니다. (App 시작 시 또는 최초 사용 전 호출)
        /// </summary>
        public void Initialize(BastionConfig config)
        {
            _config = config;
        }

        public async Task<SessionContext> CreateTunnelAsync(TargetInfo target)
        {
            if (_config == null)
                throw new InvalidOperationException("BastionManager가 초기화되지 않았습니다. Initialize(config)를 먼저 호출하세요.");

            await _lock.WaitAsync();
            try
            {
                // 1. Bastion 메인 세션 연결
                if (_bastionSession == null || !_bastionSession.IsConnected)
                {
                    await ConnectBastionWithHAAsync();
                }

                // 2. 터널 생성
                var tunnel = _bastionSession.StartOutgoingTunnel("127.0.0.1", 0, target.Host, target.Port);
                int localPort = ((IPEndPoint)tunnel.LocalEndPoint).Port;

                // 3. 타겟 장비 세션 생성 (TargetInfo 정보 사용)
                object sessionObj = null;
                if (target.Protocol == E_ConnectionProtocol.SSHTelnet)
                {
                    var ssh = new Ssh();

                    /*
                    ssh.LogWriter = new Rebex.FileLogWriter(@"C:\temp\ssh_log.txt", Rebex.LogLevel.Debug);

                    // 상세 로그 레벨 상향
                    ssh.LogWriter = new Rebex.FileLogWriter($@"C:\temp\ssh_target_{target.Host}.txt", Rebex.LogLevel.Verbose);

                    // 타임아웃 연장 (중계 서버를 거치므로 물리적 지연 고려)
                    ssh.Timeout = 15000;

                    // 보안 알고리즘 호환성 강제 (최신 서버 대응)
                    ssh.Settings.SshParameters.KeyExchangeAlgorithms = SshKeyExchangeAlgorithm.Any;
                    ssh.Settings.SshParameters.MacAlgorithms = SshMacAlgorithm.Any;
                    ssh.Settings.SshParameters.EncryptionAlgorithms = SshEncryptionAlgorithm.Any;

                    */

                    ssh.Settings.SshParameters.Compression = true;
                    await ssh.ConnectAsync("127.0.0.1", localPort);
                    await ssh.LoginAsync(target.Username, target.Password);
                    sessionObj = ssh;
                }
                else
                {
                    sessionObj = new Telnet("127.0.0.1", localPort);
                }

                Interlocked.Increment(ref _referenceCount);
                return new SessionContext(tunnel, sessionObj);
            }
            catch (Exception ex)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, $"Tunnel Creation Failed: {ex.Message}");
                throw;
            }
            finally
            {
                _lock.Release();
            }
        }

        private async Task ConnectBastionWithHAAsync()
        {
            Exception lastEx = null;

            // 주입받은 _config.Hosts를 순회하며 접속 시도
            foreach (var hostInfo in _config.Hosts)
            {
                try
                {
                    var ssh = new Ssh();
                    // 성능 및 안정성 핵심 설정
                    ssh.Settings.SshParameters.Compression = true;    // 대량 데이터(로그) 수신 시 성능 향상
                    ssh.Timeout = 5000;



                    await ssh.ConnectAsync(hostInfo.Host, hostInfo.Port);
                    await ssh.LoginAsync(_config.Username, _config.Password);

                    var interval = TimeSpan.FromMinutes(1);
                    _keepAliveTimer = new Timer(_ =>
                    {
                        try
                        {
                            if (ssh.IsConnected && ssh.Session != null)
                                ssh.Session.KeepAlive();   // 세션당 1번만 호출
                        }
                        catch { /* 로그 or 재연결 */ }
                    }, null, interval, interval);

                    _bastionSession = ssh;

                    AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, $"Bastion Connected: {hostInfo.Host}");
                    return;
                }
                catch (Exception ex)
                {
                    lastEx = ex;
                    _bastionSession?.Dispose();
                    _bastionSession = null;
                }
            }
            throw new Exception(ERR_GATEWAY_FAILED, lastEx);
        }

        public async Task ReleaseSessionAsync(SessionContext context)
        {
            if (context == null) return;

            await _lock.WaitAsync();
            try
            {
                context.Dispose();

                if (Interlocked.Decrement(ref _referenceCount) <= 0)
                {
                    if (_bastionSession != null)
                    {
                        if (_bastionSession.IsConnected) _bastionSession.Disconnect();
                        _bastionSession.Dispose();
                        _bastionSession = null;
                        AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "모든 터널 종료로 Bastion 메인 세션을 해제했습니다.");
                    }
                }
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}
