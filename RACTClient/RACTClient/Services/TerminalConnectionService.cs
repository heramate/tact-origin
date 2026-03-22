using RACTCommonClass;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace RACTClient.Services
{
    /// <summary>
    /// 장비 정보 조회 및 권한 체크 등 네트워크 통신 기반의 연결 프로세스를 비동기(async/await)로 관리하는 서비스입니다.
    /// </summary>
    public class TerminalConnectionService
    {
        private readonly ConcurrentDictionary<E_CommunicationType, TaskCompletionSource<ResultCommunicationData>> _pendingRequests
            = new ConcurrentDictionary<E_CommunicationType, TaskCompletionSource<ResultCommunicationData>>();

        /// <summary>
        /// 서버에 요청을 전송하고 결과를 비동기적으로 대기합니다.
        /// </summary>
        /// <param name="sender">요청을 보내는 객체 (ISenderObject)</param>
        /// <param name="requestData">요청 데이터</param>
        /// <param name="timeoutMs">타임아웃 (밀리초)</param>
        /// <returns>서버 응답 결과</returns>
        public async Task<ResultCommunicationData> SendRequestAsync(ISenderObject sender, RequestCommunicationData requestData, int timeoutMs = 15000)
        {
            var tcs = new TaskCompletionSource<ResultCommunicationData>();

            // 동일 권항 형식의 이전 요청이 있다면 취소(제거) 처리
            _pendingRequests.AddOrUpdate(requestData.CommType, tcs, (key, old) =>
            {
                old.TrySetCanceled();
                return tcs;
            });

            AppGlobal.SendRequestData(sender, requestData);

            using (var cts = new System.Threading.CancellationTokenSource(timeoutMs))
            {
                try
                {
                    var completedTask = await Task.WhenAny(tcs.Task, Task.Delay(timeoutMs, cts.Token));
                    if (completedTask == tcs.Task)
                    {
                        return await tcs.Task;
                    }
                    else
                    {
                        throw new TimeoutException($"통신 응답 대기 시간이 경과되었습니다. (CommType: {requestData.CommType}, Timeout: {timeoutMs}ms)");
                    }
                }
                finally
                {
                    _pendingRequests.TryRemove(requestData.CommType, out _);
                }
            }
        }


        /// <summary>
        /// 서버로부터 수신된 데이터를 해당 요청 대기자에게 전달합니다.
        /// </summary>
        /// <param name="result">수신된 결과 데이터</param>
        public bool HandleResult(ResultCommunicationData result)
        {
            if (result == null) return false;

            if (_pendingRequests.TryRemove(result.CommType, out var tcs))
            {
                return tcs.TrySetResult(result);
            }

            return false;
        }
    }
}
