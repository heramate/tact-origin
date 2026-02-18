using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Serilog;

namespace TACT.KeepAliveServer
{
    public class DaemonCommProcess : IDisposable
    {
        private readonly ConcurrentQueue<RequestCommunicationData> _requestQueue = new();
        
        public DaemonCommProcess()
        {
        }

        public bool Start()
        {
            Log.Information("DaemonCommProcess started (Stub). Waiting for daemon requests via modern channels.");
            return true;
        }

        public void Stop()
        {
            Log.Information("DaemonCommProcess stopping...");
        }

        public void Dispose() => Stop();

        public void AddResult(RequestCommunicationData req, string tunnelIp, int tunnelPort, ErrorInfo errorInfo)
        {
            Log.Information("Result for Client {ClientID}: Tunnel={IP}:{Port}, Error={Error}", 
                req.ClientID, tunnelIp, tunnelPort, errorInfo.Message);
        }
    }

    public class RequestCommunicationData
    {
        public int ClientID { get; set; }
        public DateTime RequestTime { get; set; } = DateTime.Now;
        public object RequestData { get; set; }
        public string CommType { get; set; } = string.Empty;
    }

    public class ErrorInfo
    {
        public string ErrorType { get; set; } = "None";
        public string Message { get; set; } = string.Empty;

        public ErrorInfo(string type, string message)
        {
            ErrorType = type;
            Message = message;
        }

        public static ErrorInfo NoError = new ErrorInfo("None", "");
    }
}
