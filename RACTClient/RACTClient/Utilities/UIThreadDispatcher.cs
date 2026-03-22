using RACTCommonClass;
using System;
using System.Threading;
using System.Windows.Forms;

namespace RACTClient.Utilities
{
    public static class UIThreadDispatcher
    {
        private static SynchronizationContext _uiContext;

        /// <summary>
        /// 프로그램 시작 시 메인 UI 스레드에서 한 번 호출해야 합니다.
        /// </summary>
        public static void Initialize()
        {
            if (_uiContext == null)
            {
                _uiContext = SynchronizationContext.Current;

                if (_uiContext == null)
                {
                    _uiContext = new SynchronizationContext();
                }
            }
        }

        public static void Run(Control target, Action action)
        {
            if (target == null || target.IsDisposed || target.Disposing)
            {
                return;
            }

            if (!target.InvokeRequired)
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    LogError(ex, false);
                }
                return;
            }

            if (_uiContext == null)
            {
                _uiContext = SynchronizationContext.Current;
            }

            if (_uiContext == null)
            {
                return;
            }

            _uiContext.Post(_ =>
            {
                if (target == null || target.IsDisposed || target.Disposing)
                {
                    return;
                }

                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    LogError(ex, false);
                }
            }, null);
        }

        /// <summary>
        /// UI 스레드에서 작업을 동기적으로 실행합니다.
        /// </summary>
        public static void RunSync(Control target, Action action)
        {
            if (target == null || target.IsDisposed || target.Disposing)
            {
                return;
            }

            try
            {
                if (target.InvokeRequired)
                {
                    target.Invoke((MethodInvoker)delegate
                    {
                        if (!target.IsDisposed && !target.Disposing)
                        {
                            action();
                        }
                    });
                }
                else
                {
                    action();
                }
            }
            catch (Exception ex)
            {
                LogError(ex, true);
            }
        }

        private static void LogError(Exception ex, bool sync)
        {
            if (AppGlobal.s_FileLogProcessor != null)
            {
                string prefix = sync ? "UI Sync Dispatch Error: " : "UI Dispatch Error: ";
                AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error, prefix + ex.Message);
            }
        }
    }
}
