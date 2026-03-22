using System;
using System.IO;
using MessagePack;
using MessagePack.Resolvers;

namespace RACTCommonClass
{
    /// <summary>
    /// .NET 10 및 Framework 4.8 호환 고성능 MessagePack 직렬화기
    /// BinaryFormatter를 대체하여 보안성과 성능을 확보합니다.
    /// </summary>
    public class ObjectConverter
    {
        private static MessagePackSerializerOptions _options;

        static ObjectConverter()
        {
            // MessagePack v2.x 표준 설정
            _options = MessagePackSerializerOptions.Standard
                .WithResolver(CompositeResolver.Create(
                    NativeGuidResolver.Instance,
                    NativeDecimalResolver.Instance,
                    StandardResolver.Instance
                ))
                .WithSecurity(MessagePackSecurity.UntrustedData);
        }

        /// <summary>
        /// 개체를 바이트 배열로 변환합니다.
        /// </summary>
        public static byte[] GetBytes(object aValue)
        {
            if (aValue == null) return null;
            try
            {
                return MessagePackSerializer.Serialize(aValue, _options);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("MsgPack Serialize Error: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 바이트 배열을 개체로 변환합니다.
        /// </summary>
        public static object GetObject(byte[] aBytes)
        {
            if (aBytes == null || aBytes.Length == 0) return null;
            try
            {
                // object 타입으로 역직렬화 시 Typeless 지원이 필요할 수 있으나 우선 기본 역직렬화 시도
                return MessagePackSerializer.Deserialize<object>(aBytes, _options);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("MsgPack Deserialize Error: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 제네릭 방식으로 바이트 배열을 개체로 변환합니다. (추천 방식)
        /// </summary>
        public static T GetObject<T>(byte[] aBytes)
        {
            if (aBytes == null || aBytes.Length == 0) return default;
            try
            {
                return MessagePackSerializer.Deserialize<T>(aBytes, _options);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MsgPack Deserialize<{typeof(T).Name}> Error: " + ex.Message);
                return default;
            }
        }
    }
}
