using System;
using Serilog;

namespace TACT.KeepAliveServer
{
    public class DBLogProcess : IDisposable
    {
        public DBLogProcess(object pool)
        {
            // 레거시 생성자 호환성 유지
        }

        public void Start()
        {
            Log.Information("DBLogProcess started (Modernized stub).");
        }

        public void Stop()
        {
            Log.Information("DBLogProcess stopped.");
        }

        public void Dispose() => Stop();

        public void PrintLog(E_FileLogType type, string msg)
        {
            switch (type)
            {
                case E_FileLogType.Error: Log.Error(msg); break;
                case E_FileLogType.Warning: Log.Warning(msg); break;
                default: Log.Information(msg); break;
            }
        }
    }
}
