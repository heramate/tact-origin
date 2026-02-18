using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace TACT.KeepAliveServer
{
    public class SSHTunnelManager : IDisposable
    {
        public ushort PortRangeMin { get; }
        public ushort PortRangeMax { get; }
        
        private readonly ConcurrentDictionary<ushort, TunnelInfo> _tunnelPortInfos = new();
        private readonly CancellationTokenSource _cts = new();
        private Task _monitorTask;

        public SSHTunnelManager(ushort portRangeMin, ushort portRangeMax)
        {
            PortRangeMin = Math.Min(portRangeMin, portRangeMax);
            PortRangeMax = Math.Max(portRangeMin, portRangeMax);
        }

        public void Start()
        {
            _monitorTask = Task.Run(() => MonitorPortsAsync(_cts.Token));
            Log.Information("SSHTunnelManager started. Monitoring ports {Min}-{Max}", PortRangeMin, PortRangeMax);
        }

        private async Task MonitorPortsAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    // 로직 구현 (포트 상태 체크 등)
                    await Task.Delay(1500, ct);
                }
                catch (OperationCanceledException) { break; }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error in Tunnel Monitoring Task");
                }
            }
        }

        public int CreateTunnelPort(string deviceIp)
        {
            var availablePort = GetAvailablePort();
            if (availablePort == 0) return 0;

            var info = new TunnelInfo(availablePort, deviceIp, E_TunnelState.Closed);
            if (_tunnelPortInfos.TryAdd(availablePort, info))
            {
                Log.Information("Allocated tunnel port {Port} for {DeviceIP}", availablePort, deviceIp);
                return availablePort;
            }
            return 0;
        }

        private ushort GetAvailablePort()
        {
            for (ushort p = PortRangeMin; p <= PortRangeMax; p++)
            {
                if (!_tunnelPortInfos.ContainsKey(p)) return p;
            }
            return 0;
        }

        public void Stop()
        {
            _cts.Cancel();
            Log.Information("SSHTunnelManager stopping...");
        }

        public void Dispose()
        {
            Stop();
            _cts.Dispose();
        }
    }
}
