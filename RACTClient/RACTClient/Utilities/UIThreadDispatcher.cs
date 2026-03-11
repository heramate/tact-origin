using RACTCommonClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RACTClient.Utilities
{
    public static class UIThreadDispatcher
    {
        // UI 스레드의 컨텍스트를 저장할 정적 변수
        private static SynchronizationContext _uiContext;

        /// <summary>
        /// 초기화 메서드: 프로그램 시작 시(ClientMain 생성자 등) 메인 UI 스레드에서 한 번 호출해야 합니다.
        /// </summary>
        public static void Initialize()
        {
            if (_uiContext == null)
            {
                _uiContext = SynchronizationContext.Current;

                // 만약 null이라면(Console 모드 등) 새로 생성
                if (_uiContext == null) _uiContext = new SynchronizationContext();
            }
        }

        /// <summary>
        /// UI 스레드에서 안전하게 작업을 실행합니다. (Fire-and-forget 방식)
        /// Target Control의 생명주기를 자동으로 체크합니다.
        /// </summary>
        /// <param name="target">업데이트 대상 컨트롤 (this)</param>
        /// <param name="action">실행할 로직 (람다)</param>
        public static void Run(Control target, Action action)
        {
            // 1. 1차 방어: 호출 시점에 컨트롤이 이미 파기되었으면 중단
            if (target != null && (target.IsDisposed || target.Disposing)) return;

            // 2. 컨텍스트가 초기화되지 않았으면 현재 컨텍스트 사용 시도
            if (_uiContext == null) _uiContext = SynchronizationContext.Current;
            if (_uiContext == null) return; // 실행 불가

            // 3. 비동기 Post 실행
            _uiContext.Post((state) =>
            {
                // 4. 2차 방어: 콜백 실행 시점에 컨트롤 파기 여부 재확인
                if (target != null && (target.IsDisposed || target.Disposing)) return;

                try
                {
                    // 5. 실제 로직 실행
                    action();
                }
                catch (Exception ex)
                {
                    // 6. 중앙 집중식 예외 로깅 (AppGlobal 활용)
                    // [Source: AppGlobal.cs Line 34, 42 참조]
                    if (AppGlobal.s_FileLogProcessor != null)
                    {
                        AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error,
                            "UI Dispatch Error: " + ex.Message);
                    }
                }
            }, null);
        }

        /// <summary>
        /// UI 스레드에서 작업을 동기적(Blocking)으로 실행합니다.
        /// 작업이 완료될 때까지 호출한 스레드는 대기합니다.
        /// </summary>
        /// <param name="target">대상 컨트롤</param>
        /// <param name="action">실행할 작업</param>
        public static void RunSync(Control target, Action action)
        {
            // 1. 방어 코드: 컨트롤이 없거나 이미 파기된 경우 실행하지 않음
            if (target == null || target.IsDisposed || target.Disposing) return;

            try
            {
                // 2. UI 스레드 체크
                if (target.InvokeRequired)
                {
                    // 3. 다른 스레드인 경우: 동기 Invoke 호출 (완료될 때까지 대기)
                    target.Invoke((MethodInvoker)delegate
                    {
                        // 콜백 시점에 컨트롤 생명주기 재확인
                        if (!target.IsDisposed && !target.Disposing)
                        {
                            action();
                        }
                    });
                }
                else
                {
                    // 4. 이미 UI 스레드인 경우: 즉시 실행
                    action();
                }
            }
            catch (Exception ex)
            {
                // 5. 예외 로깅
                if (AppGlobal.s_FileLogProcessor != null)
                {
                    AppGlobal.s_FileLogProcessor.PrintLog(E_FileLogType.Error,
                        "UI Sync Dispatch Error: " + ex.Message);
                }
            }
        }

        public static Task InvokeAsync(this Control control, Action action)
        {
            var tcs = new TaskCompletionSource<bool>();
            control.BeginInvoke(new Action(() => {
                try { action(); tcs.SetResult(true); }
                catch (Exception ex) { tcs.SetException(ex); }
            }));
            return tcs.Task;
        }
    }
}
