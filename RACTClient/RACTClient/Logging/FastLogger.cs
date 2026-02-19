using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using RACTCommonClass;

namespace RACTClient.Logging
{
    /// <summary>
    /// BlockingCollection을 사용한 고성능 비동기 파일 로거입니다.
    /// </summary>
    public class FastLogger : IDisposable
    {
        private readonly BlockingCollection<LogEntry> _logQueue = new BlockingCollection<LogEntry>(new ConcurrentQueue<LogEntry>());
        private readonly string _logDirectory;
        private readonly string _prefix;
        private bool _isDisposed = false;

        public FastLogger(string directory, string prefix)
        {
            _logDirectory = directory;
            _prefix = prefix;
            if (!Directory.Exists(_logDirectory)) Directory.CreateDirectory(_logDirectory);
            
            // 로그 처리 루프 시작
            Task.Run(() => ProcessLogs());
        }

        public void Start() { /* 인터페이스 호환용 */ }
        public void Stop() => Dispose();

        public void PrintLog(E_FileLogType type, string message) => Enqueue(type, message);
        public void PrintLog(string message) => Enqueue(E_FileLogType.Infomation, message);

        private void Enqueue(E_FileLogType type, string message)
        {
            if (_isDisposed || _logQueue.IsAddingCompleted) return;
            _logQueue.Add(new LogEntry { Type = type, Message = message, Timestamp = DateTime.Now });
        }

        private void ProcessLogs()
        {
            foreach (var entry in _logQueue.GetConsumingEnumerable())
            {
                try
                {
                    string fileName = $"{_prefix}_{entry.Timestamp:yyyyMMdd}.log";
                    string filePath = Path.Combine(_logDirectory, fileName);
                    
                    using (var sw = new StreamWriter(filePath, true, Encoding.UTF8, 8192))
                    {
                        sw.WriteLine($"[{entry.Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{entry.Type}] {entry.Message}");
                        
                        // 대기 중인 로그가 더 있다면 한 번에 배치 쓰기
                        while (_logQueue.TryTake(out var nextEntry, 0))
                        {
                            sw.WriteLine($"[{nextEntry.Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{nextEntry.Type}] {nextEntry.Message}");
                        }
                    }
                }
                catch { /* 로깅 실패가 서비스에 영향을 주지 않도록 처리 */ }
            }
        }

        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;
            _logQueue.CompleteAdding();
        }

        private struct LogEntry {
            public E_FileLogType Type;
            public string Message;
            public DateTime Timestamp;
        }
    }
}
