using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        /// <param name="control">호출하는 컨트롤 (this)</param>
        /// <param name="action">실행할 작업</param>
        public static void SafeInvoke(this Control control, Action action)
        {
            // 앞서 만든 UIThreadDispatcher를 호출
            UIThreadDispatcher.Run(control, action);
        }

        /// <summary>
        /// [신규] UI 스레드에서 안전하게 동기(Blocking)로 실행합니다.
        /// 이 메서드는 작업이 완료될 때까지 리턴하지 않습니다.
        /// </summary>
        public static void RunSync(this Control control, Action action)
        {
            UIThreadDispatcher.RunSync(control, action);
        }
    }
}
