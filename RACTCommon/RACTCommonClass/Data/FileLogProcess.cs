using System;
using System.Collections.Generic;
using System.Text;
using MKLibrary.MKLog;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace RACTCommonClass
{
    public class FileLogProcess
    {
        /// <summary>
        /// 파일을 저장할 로그 입니다.
        /// </summary>
        private MKFileLog m_FileLog = null;
        /// <summary>
        /// 로그가 저장될 비차단 큐 입니다.
        /// </summary>
        private BlockingCollection<FileLogInfo> m_LogQueue = null;
        /// <summary>
        /// 로그를 처리할 비동기 태스크 입니다.
        /// </summary>
        private Task m_LogProcessTask = null;
        /// <summary>
        /// 비동기 작업을 제어할 토큰입니다.
        /// </summary>
        private CancellationTokenSource _cts = null;

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public FileLogProcess(string aFilePath, string aFileName)
        {
            m_LogQueue = new BlockingCollection<FileLogInfo>();
            m_FileLog = new MKFileLog(aFilePath, aFileName, true, true);
        }

        /// <summary>
        /// 시작 합니다.
        /// </summary>
        public void Start()
        {
            _cts = new CancellationTokenSource();
            m_LogProcessTask = Task.Run(() => ProcessLogAsync(_cts.Token));
        }

        /// <summary>
        /// 종료 합니다.
        /// </summary>
        public void Stop()
        {
            try
            {
                if (_cts != null)
                {
                    _cts.Cancel();
                }

                if (m_LogQueue != null)
                {
                    m_LogQueue.CompleteAdding();
                }

                // 남은 로그가 처리될 때까지 최대 3초 대기
                if (m_LogProcessTask != null)
                {
                    m_LogProcessTask.Wait(3000);
                }

                if (m_FileLog != null)
                {
                    m_FileLog.Dispose();
                }
            }
            catch { }
        }

        /// <summary>
        /// 로그를 저장 합니다.
        /// </summary>
        public void PrintLog(E_FileLogType aLogType, string aLogMessage)
        {
            if (m_LogQueue != null && !m_LogQueue.IsAddingCompleted)
            {
                m_LogQueue.Add(new FileLogInfo(aLogType, aLogMessage));
            }
        }

        /// <summary>
        /// 로그를 저장 합니다.
        /// </summary>
        public void PrintLog(string aLogMessage)
        {
            PrintLog(E_FileLogType.Infomation, aLogMessage);
        }

        /// <summary>
        /// 로그를 저장 합니다.
        /// </summary>
        public void PrintLog(FileLogInfo aLogInfo)
        {
            if (m_LogQueue != null && !m_LogQueue.IsAddingCompleted)
            {
                m_LogQueue.Add(aLogInfo);
            }
        }

        /// <summary>
        /// 로그를 비동기 일괄 처리 합니다.
        /// </summary>
        private async Task ProcessLogAsync(CancellationToken cancellationToken)
        {
            List<FileLogInfo> batchLogs = new List<FileLogInfo>();

            while (!m_LogQueue.IsCompleted)
            {
                try
                {
                    // 로그가 들어올 때까지 대기하거나 일괄 처리를 위해 가져옴
                    if (m_LogQueue.TryTake(out FileLogInfo logInfo, 100, cancellationToken))
                    {
                        batchLogs.Add(logInfo);

                        // 최대 100개까지 일괄 수집
                        while (batchLogs.Count < 100 && m_LogQueue.TryTake(out FileLogInfo nextLog))
                        {
                            batchLogs.Add(nextLog);
                        }
                    }

                    if (batchLogs.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (var log in batchLogs)
                        {
                            sb.AppendLine(string.Format("[{0}] {1} {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), log.LogType.ToString(), log.Message));
                        }

                        // 파일에 비동기로 기록 (내부 MKFileLog가 동기라면 Task.Run으로 래핑하여 I/O 차단 방지)
                        await Task.Run(() => {
                            m_FileLog.PrintLogEnter(sb.ToString().TrimEnd());
                        });

                        batchLogs.Clear();
                    }
                }
                catch (OperationCanceledException) { break; }
                catch (Exception)
                {
                    // 로그 기록 중 발생한 에러는 무시하거나 최소화
                }
            }
        }
    }
}
