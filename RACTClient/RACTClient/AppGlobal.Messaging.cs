using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RACTCommonClass;
using MKLibrary.MKNetwork;

namespace RACTClient
{
    /// <summary>
    /// .NET 10 고성능 서버 통신 검증을 위한 경량화된 AppGlobal 클래스
    /// </summary>
    public static class AppGlobal
    {
        public static MKRemote s_RemoteGateway = null;
        public static PipelineClient s_PipelineClient = null;
        public static string s_ServerIP = "127.0.0.1";
        public static int s_ServerPort = 43210;
        
        public static UserInfo s_UserInfo = null;
        public static readonly int s_RequestTimeOut = 5000;

        private static readonly Dictionary<int, ISenderObject> s_SenderList = new Dictionary<int, ISenderObject>();
        private static readonly BlockingCollection<CommunicationData> s_RequestQueue = new BlockingCollection<CommunicationData>();

        static AppGlobal()
        {
            // 통신 처리 스레드 시작
            Task.Run(() => ProcessRequestQueue());
        }

        public static void AddSender(ISenderObject sender)
        {
            lock (s_SenderList)
            {
                int key = sender.GetHashCode();
                if (!s_SenderList.ContainsKey(key)) s_SenderList.Add(key, sender);
            }
        }

        public static async Task<bool> TryServerConnectAsync(string ip, int port)
        {
            s_ServerIP = ip;
            s_ServerPort = port;
            
            // 1. 기존 MKRemote 연결 (하위 호환)
            // 2. 고성능 Pipeline 채널 연결
            s_PipelineClient = new PipelineClient(ip, port + 12); // 서버 고성능 포트 54322
            s_PipelineClient.PacketReceived += OnPipelinePacketReceived;
            
            return await s_PipelineClient.ConnectAsync();
        }

        private static void OnPipelinePacketReceived(byte[] packet)
        {
            var result = ObjectConverter.GetObject<ResultCommunicationData>(packet);
            if (result == null) return;

            lock (s_SenderList)
            {
                if (s_SenderList.TryGetValue(result.OwnerKey, out var sender))
                {
                    sender.ResultReceiver(result);
                }
            }
        }

        private static void ProcessRequestQueue()
        {
            foreach (var request in s_RequestQueue.GetConsumingEnumerable())
            {
                if (s_PipelineClient != null)
                {
                    byte[] data = ObjectConverter.GetBytes(request);
                    _ = s_PipelineClient.SendAsync(data);
                }
            }
        }

        public static async Task<ResultCommunicationData> SendRequestAsync(CommunicationData request)
        {
            var tcs = new TaskCompletionSource<ResultCommunicationData>();
            var sender = new AsyncSender(tcs);
            AddSender(sender);
            request.OwnerKey = sender.GetHashCode();
            s_RequestQueue.Add(request);

            using (var cts = new CancellationTokenSource(s_RequestTimeOut))
            {
                cts.Token.Register(() => tcs.TrySetException(new TimeoutException("서버 응답 시간 초과")));
                return await tcs.Task;
            }
        }

        private class AsyncSender : ISenderObject
        {
            private readonly TaskCompletionSource<ResultCommunicationData> _tcs;
            public AsyncSender(TaskCompletionSource<ResultCommunicationData> tcs) => _tcs = tcs;
            public void ResultReceiver(ResultCommunicationData result) => _tcs.TrySetResult(result);
            public void ResultReceiver(CommandResultItem result) { }
        }
    }
}
