using System;
using System.Threading;

namespace RACTClient.Helpers
{
    /// <summary>
    /// 백그라운드 스레드에서 UI 컨트롤에 안전하게 접근하기 위한 동기화 도우미입니다.
    /// </summary>
    public static class UiContext
    {
        private static SynchronizationContext _context;

        /// <summary>
        /// 현재 스레드의 컨텍스트를 UI 컨텍스트로 캡처합니다. 메인 UI 스레드에서 호출해야 합니다.
        /// </summary>
        public static void Initialize()
        {
            if (_context == null)
                _context = SynchronizationContext.Current;
        }

        /// <summary>
        /// [비동기] 작업을 UI 스레드 메시지 큐에 던지고 즉시 리턴합니다. (BeginInvoke 방식)
        /// </summary>
        public static void RunAsync(Action action)
        {
            if (_context == null || _context == SynchronizationContext.Current)
            {
                SafeExecute(action);
            }
            else
            {
                _context.Post(_ => SafeExecute(action), null);
            }
        }

        /// <summary>
        /// [동기] 작업을 UI 스레드에서 실행하고 완료될 때까지 대기합니다. (Invoke 방식)
        /// </summary>
        public static void RunSync(Action action)
        {
            if (_context == null || _context == SynchronizationContext.Current)
            {
                SafeExecute(action);
            }
            else
            {
                _context.Send(_ => SafeExecute(action), null);
            }
        }

        private static void SafeExecute(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"UiContext Error: {ex.Message}");
            }
        }
    }
}
