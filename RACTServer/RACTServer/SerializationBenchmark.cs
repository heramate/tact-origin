using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RACTCommonClass;

namespace RACTServer
{
    public static class SerializationBenchmark
    {
        public static void RunVerification()
        {
            Console.WriteLine("=== 고속 역직렬화(MessagePack) 성능 검증 시작 ===");

            // 1. 테스트 데이터 준비 (5,000개 장비 정보)
            int count = 5000;
            var deviceList = new DeviceInfoCollection();
            for (int i = 0; i < count; i++)
            {
                deviceList.Add(new DeviceInfo
                {
                    DeviceID = i,
                    Name = $"Test_Device_{i}",
                    IPAddress = $"192.168.0.{i % 255}",
                    ModelName = "L3_Switch_HighPerf",
                    BranchCode = "BR_001",
                    CenterCode = "CTR_99"
                });
            }

            // 2. 직렬화 테스트
            Stopwatch sw = Stopwatch.StartNew();
            byte[] serializedData = ObjectConverter.GetBytes(deviceList);
            sw.Stop();
            long serializeTime = sw.ElapsedMilliseconds;
            long dataSize = serializedData?.Length ?? 0;

            // 3. 역직렬화 테스트 (핵심)
            sw.Restart();
            var deserializedList = ObjectConverter.GetObject<DeviceInfoCollection>(serializedData);
            sw.Stop();
            long deserializeTime = sw.ElapsedMilliseconds;

            // 4. 결과 출력
            Console.WriteLine($"대상 객체 수: {count:N0} 개");
            Console.WriteLine($"데이터 크기: {dataSize / 1024.0 / 1024.0:F2} MB");
            Console.WriteLine($"직렬화 시간: {serializeTime} ms");
            Console.WriteLine($"역직렬화 시간: {deserializeTime} ms");
            
            // 무결성 검증
            bool isSuccess = deserializedList != null && deserializedList.Count == count;
            Console.WriteLine($"무결성 검증: {(isSuccess ? "PASS" : "FAIL")}");
            
            if (isSuccess)
            {
                Console.WriteLine($"평균 역직렬화 속도: {count / (deserializeTime / 1000.0 + 0.001):N0} objects/sec");
            }
            Console.WriteLine("================================================");
        }
    }
}
