using System;
using System.Collections.Generic;
using System.Text;
using MKLibrary.MKLog;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using MessagePack;

namespace RACTCommonClass
{
    /// <summary>
    /// .NET 10 기반 고성능 비동기 파일 로그 프로세스
    /// </summary>
    public class FileLogProcess
    {
        private MKFileLog m_FileLog = null;
        private BlockingCollection<FileLogInfo> m_LogQueue = null;
        private Task m_LogProcessTask = null;
        private CancellationTokenSource _cts = null;

        public FileLogProcess(string aFilePath, string aFileName)
        {
            m_LogQueue = new BlockingCollection<FileLogInfo>();
            m_FileLog = new MKFileLog(aFilePath, aFileName, true, true);
        }

        public void Start()
        {
            _cts = new CancellationTokenSource();
            m_LogProcessTask = Task.Run(() => ProcessLogAsync(_cts.Token));
        }

        public void Stop()
        {
            try
            {
                _cts?.Cancel();
                m_LogQueue?.CompleteAdding();
                m_LogProcessTask?.Wait(3000);
                m_FileLog?.Dispose();
            }
            catch { }
        }

        public void PrintLog(E_FileLogType aLogType, string aLogMessage)
        {
            if (m_LogQueue != null && !m_LogQueue.IsAddingCompleted)
                m_LogQueue.Add(new FileLogInfo(aLogType, aLogMessage));
        }

        public void PrintLog(string aLogMessage) => PrintLog(E_FileLogType.Infomation, aLogMessage);

        public void PrintLog(FileLogInfo aLogInfo)
        {
            if (m_LogQueue != null && !m_LogQueue.IsAddingCompleted)
                m_LogQueue.Add(aLogInfo);
        }

        private async Task ProcessLogAsync(CancellationToken cancellationToken)
        {
            List<FileLogInfo> batchLogs = new List<FileLogInfo>();
            while (!m_LogQueue.IsCompleted)
            {
                try
                {
                    if (m_LogQueue.TryTake(out FileLogInfo logInfo, 100, cancellationToken))
                    {
                        batchLogs.Add(logInfo);
                        while (batchLogs.Count < 100 && m_LogQueue.TryTake(out FileLogInfo nextLog)) batchLogs.Add(nextLog);
                    }

                    if (batchLogs.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (var log in batchLogs)
                            sb.AppendLine(string.Format("[{0}] {1} {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), log.LogType.ToString(), log.Message));

                        await Task.Run(() => m_FileLog.PrintLogEnter(sb.ToString().TrimEnd()));
                        batchLogs.Clear();
                    }
                }
                catch (OperationCanceledException) { break; }
                catch { }
            }
        }
    }
}
