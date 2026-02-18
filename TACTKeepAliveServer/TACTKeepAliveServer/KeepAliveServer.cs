using System;
using Serilog;

namespace TACT.KeepAliveServer
{
    public class KeepAliveServer
    {
        private readonly string _startupPath;

        public KeepAliveServer(string startupPath)
        {
            _startupPath = startupPath;
            GlobalClass.StartupPath = startupPath;
        }

        public bool Start()
        {
            try
            {
                Log.Information("■ Starting TACT KeepAlive Server Components...");
                
                // .NET 9에서는 로깅과 DB 초기화가 Program.cs에서 수행됨
                // 여기서는 레거시 서비스 스타터를 관리하거나 필요한 경우에만 사용
                
                return true;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Failed to start KeepAliveServer components");
                return false;
            }
        }

        public void Stop()
        {
            Log.Information("Stopping TACT KeepAlive Server Components...");
            GlobalClass.IsServerStop = true;
        }
    }
}
