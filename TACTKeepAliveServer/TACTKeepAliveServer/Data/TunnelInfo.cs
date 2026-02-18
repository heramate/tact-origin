using System;

namespace TACT.KeepAliveServer
{
    public class TunnelInfo
    {
        public string DeviceIP { get; set; } = string.Empty;
        public ushort TunnelPort { get; set; }
        public string TunnelIP { get; set; } = "127.0.0.1";
        public E_TunnelState TunnelState { get; set; } = E_TunnelState.Closed;

        public DateTime TimeStampClosed { get; set; } = DateTime.MinValue;
        public DateTime TimeStampWaitingOpen { get; set; } = DateTime.MinValue;
        public DateTime TimeStampOpened { get; set; } = DateTime.MinValue;
        public DateTime TimeStampConnected { get; set; } = DateTime.MinValue;
        public DateTime TimeStampWaitingClose { get; set; } = DateTime.MinValue;

        public TunnelInfo(ushort tunnelPort, string deviceIp, E_TunnelState state)
        {
            TunnelPort = tunnelPort;
            DeviceIP = deviceIp;
            UpdateState(state);
        }

        public TunnelInfo(ushort tunnelPort, E_TunnelState state)
        {
            TunnelPort = tunnelPort;
            UpdateState(state);
        }

        public void UpdateState(E_TunnelState state)
        {
            TunnelState = state;
            var now = DateTime.Now;
            switch (state)
            {
                case E_TunnelState.Closed: TimeStampClosed = now; break;
                case E_TunnelState.WaitingOpen: TimeStampWaitingOpen = now; break;
                case E_TunnelState.Opened: TimeStampOpened = now; break;
                case E_TunnelState.Connected: TimeStampConnected = now; break;
                case E_TunnelState.WaitingClose: TimeStampWaitingClose = now; break;
            }
        }
    }
}
