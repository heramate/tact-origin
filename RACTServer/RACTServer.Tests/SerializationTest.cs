using Microsoft.VisualStudio.TestTools.UnitTesting;
using RACTServer;

namespace RACTServer.Tests
{
    [TestClass]
    public class SerializationTest
    {
        [TestMethod]
        public void VerifyHighSpeedSerialization()
        {
            // 벤치마크 및 무결성 검증 실행
            SerializationBenchmark.RunVerification();
            
            // Note: 실제 Assert는 RunVerification 내부의 로직 성공 여부에 따라 
            // 추가할 수 있으나, 현재는 예외 없이 실행되는지를 검증합니다.
        }
    }
}
