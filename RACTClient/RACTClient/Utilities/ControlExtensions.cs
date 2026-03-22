using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RACTClient.Utilities
{
    public static class ControlExtensions
    {
        /// <summary>
        /// UI 스레드에서 안전하게 비동기(Fire-and-forget)로 실행합니다. (UIThreadDispatcher 래퍼)
        /// 사용법: this.SafeInvoke(() => { ... });
        /// </summary>
        public static void SafeInvoke(this Control control, Action action)
        {
            UIThreadDispatcher.Run(control, action);
        }

        /// <summary>
        /// UI 스레드에서 안전하게 동기(Blocking)로 실행합니다.
        /// 이 메서드는 작업이 완료될 때까지 리턴하지 않습니다.
        /// </summary>
        public static void RunSync(this Control control, Action action)
        {
            UIThreadDispatcher.RunSync(control, action);
        }

        /// <summary>
        /// ResultReceiver 후처리처럼 UI 스레드에서만 실행되어야 하는 작업을 안전하게 전달합니다.
        /// </summary>
        public static void RunResultHandlerOnUi(this Control control, Action action)
        {
            if (control == null || control.IsDisposed || control.Disposing)
            {
                return;
            }

            if (!control.InvokeRequired)
            {
                action();
                return;
            }

            _ = control.InvokeAsync(() =>
            {
                if (!control.IsDisposed && !control.Disposing)
                {
                    action();
                }
            });
        }

        /// <summary>
        /// 현재 스레드가 UI 스레드가 아니면 UI 스레드로 작업을 전달하고 false를 반환합니다.
        /// true를 반환하는 경우에만 호출부가 계속 실행하면 됩니다.
        /// </summary>
        public static bool EnsureUiThread(this Control control, Action action)
        {
            if (control == null || action == null || control.IsDisposed || control.Disposing)
            {
                return false;
            }

            if (!control.InvokeRequired)
            {
                return true;
            }

            if (control.IsHandleCreated)
            {
                control.BeginInvoke(action);
            }

            return false;
        }

        /// <summary>
        /// Control.BeginInvoke를 Task로 감싸 UI 작업 완료를 await할 수 있게 합니다.
        /// </summary>
        public static Task InvokeAsync(this Control control, Action action)
        {
            var tcs = new TaskCompletionSource<bool>();
            control.BeginInvoke(new Action(() =>
            {
                try
                {
                    action();
                    tcs.SetResult(true);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }));
            return tcs.Task;
        }
    }
}
