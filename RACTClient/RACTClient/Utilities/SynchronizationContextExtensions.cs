using System;
using System.Threading;
using System.Threading.Tasks;

namespace RACTClient.Utilities.Extensions
{
    public static class SynchronizationContextExtensions
    {
        /// <summary>
        /// Action을 SynchronizationContext에서 비동기로 실행하고 완료를 기다립니다.
        /// </summary>
        public static Task PostAsync(this SynchronizationContext context, Action action)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (action == null) throw new ArgumentNullException(nameof(action));

            var tcs = new TaskCompletionSource<bool>();
            context.Post(_ =>
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
            }, null);
            return tcs.Task;
        }
    }
}
