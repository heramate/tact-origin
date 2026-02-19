using System;
using Rebex.Net;
using RACTCommonClass;

namespace RACTClient.Connectivity
{
    /// <summary>
    /// SshSessionPool을 사용하여 중계 서버 터널링을 관리하는 클래스입니다.
    /// </summary>
    public class SshTunnelManager : IDisposable
    {
        private Ssh _sharedSshClient;
        private ISshChannel _tunnel;
        private DeviceInfo _currentJumpHost;
        private bool _disposed = false;

        public int LocalBoundPort { get; private set; }
        public bool IsActive => _sharedSshClient != null && _sharedSshClient.IsConnected && _tunnel != null;

        public void OpenTunnel(DeviceInfo jumpHost, DeviceInfo targetDevice)
        {
            if (IsActive) CloseTunnel();

            _currentJumpHost = jumpHost;
            
            // 1. 풀에서 공유 세션을 가져옴
            _sharedSshClient = SshSessionPool.Instance.GetOrCreateSession(jumpHost);

            // 2. 공유된 연결 위에서 목적지 장비로의 터널 채널 생성
            _tunnel = _sharedSshClient.StartPortForwarding(
                0, 
                targetDevice.IPAddress, 
                targetDevice.TerminalConnectInfo.TelnetPort
            );

            this.LocalBoundPort = _tunnel.BoundPort;
        }

        public void CloseTunnel()
        {
            _tunnel?.Close();
            _tunnel = null;

            if (_currentJumpHost != null)
            {
                SshSessionPool.Instance.ReleaseSession(_currentJumpHost);
                _currentJumpHost = null;
                _sharedSshClient = null;
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                CloseTunnel();
                _disposed = true;
            }
        }
    }
}
