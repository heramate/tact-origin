using System;
using System.Collections.Concurrent;
using System.Threading;
using RACTCommonClass;

namespace RACTClient
{
    public class CommandExecuteLogProcess : ISenderObject, IDisposable
    {
        /// <summary>
        /// 로그 목록을 관리하는 차단 컬렉션입니다.
        /// </summary>
        private readonly BlockingCollection<DBExecuteCommandLogInfo> m_LogCollection = new BlockingCollection<DBExecuteCommandLogInfo>();

        /// <summary>
        /// 취소 토큰 소스입니다.
        /// </summary>
        private readonly CancellationTokenSource m_Cts = new CancellationTokenSource();

        /// <summary>
        /// 로그 처리 스레드입니다.
        /// </summary>
        private Thread m_LogProcessThread = null;

        /// <summary>
        /// 기본 생성자입니다.
        /// </summary>
        public CommandExecuteLogProcess()
        {
        }

        /// <summary>
        /// 로그 처리 프로세서를 시작합니다.
        /// </summary>
        public void Start()
        {
            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "터미널 로그 처리 프로세서를 시작합니다.");

            m_LogProcessThread = new Thread(ProcessLog)
            {
                IsBackground = true,
                Name = "CommandLogProcessThread"
            };
            m_LogProcessThread.Start();
        }

        /// <summary>
        /// 종료 처리합니다.
        /// </summary>
        public void Stop()
        {
            AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "터미널 로그 처리 프로세서를 종료합니다.");

            if (m_Disposed) return;

            try
            {
                if (!m_LogCollection.IsAddingCompleted)
                {
                    m_LogCollection.CompleteAdding();
                }
            }
            catch (ObjectDisposedException)
            {
            }

            try
            {
                if (!m_Cts.IsCancellationRequested)
                {
                    m_Cts.Cancel();
                }
            }
            catch (ObjectDisposedException)
            {
            }

            Dispose();
        }

        /// <summary>
        /// 로그를 처리합니다. 소비자 루프입니다.
        /// </summary>
        private void ProcessLog()
        {
            try
            {
                // CompleteAdding()이 호출되고 컬렉션이 비워질 때까지 차단하며 대기합니다.
                foreach (var tLog in m_LogCollection.GetConsumingEnumerable(m_Cts.Token))
                {
                    try
                    {
                        if (tLog == null || string.IsNullOrWhiteSpace(tLog.Command)) continue;

                        if (AppGlobal.s_RACTClientMode == E_RACTClientMode.Online)
                        {
                            var tRequestData = AppGlobal.MakeDefaultRequestData();
                            tRequestData.CommType = E_CommunicationType.RequestSaveExcuteCommand;
                            tRequestData.RequestData = tLog;

                            // Fire-and-forget 방식으로 전송합니다.
                            AppGlobal.SendRequestData(this, tRequestData);
                        }

                        // 로컬 파일 로그를 기록합니다.
                        CommandWriter.WriteCommandLog(tLog);
                    }
                    catch (Exception ex)
                    {
                        AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, $"개별 로그 처리 중 오류 발생: {ex.Message}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "로그 처리 스레드가 취소되었습니다.");
            }
            catch (ObjectDisposedException)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Infomation, "로그 처리 컬렉션이 정리되었습니다.");
            }
            catch (Exception ex)
            {
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, $"로그 처리 루프 오류: {ex.Message}");
            }
        }

        /// <summary>
        /// 터미널 실행 로그를 큐에 추가합니다.
        /// </summary>
        public void AddTerminalExecuteLog(DBExecuteCommandLogInfo aLog)
        {
            if (aLog == null) return;

            try
            {
                if (!m_LogCollection.IsAddingCompleted)
                {
                    m_LogCollection.Add(aLog);
                }
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is ObjectDisposedException)
            {
                // Stop/Dispose 이후 Add 경합 시 발생할 수 있습니다.
            }
        }

        #region ISenderObject 구현

        public void ResultReceiver(CommandResultItem vResult)
        {
            // 로그 전송 결과에 대한 후속 처리가 필요 없는 경우 빈 메서드를 유지합니다.
        }

        public void ResultReceiver(ResultCommunicationData vResult)
        {
            // 로그 전송 결과에 대한 후속 처리가 필요 없는 경우 빈 메서드를 유지합니다.
        }

        #endregion

        #region IDisposable 구현

        private bool m_Disposed = false;

        public void Dispose()
        {
            if (m_Disposed) return;

            try
            {
                if (!m_Cts.IsCancellationRequested)
                {
                    m_Cts.Cancel();
                }

                // 스레드가 종료될 때까지 잠시 대기합니다.
                if (m_LogProcessThread != null && m_LogProcessThread.IsAlive)
                {
                    if (!m_LogProcessThread.Join(1000))
                    {
                        AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Warning, "로그 처리 스레드가 1초 내에 종료되지 않았습니다.");
                    }
                }

                m_LogCollection.Dispose();
                m_Cts.Dispose();
            }
            catch
            {
            }

            m_Disposed = true;
        }

        #endregion
    }
}
