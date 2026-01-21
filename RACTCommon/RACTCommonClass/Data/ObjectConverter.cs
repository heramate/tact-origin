using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.IO.Compression;

namespace RACTCommonClass
{
    #region 객체 변경 클래스 입니다 --------------------------------------------------------
    /// <summary>
    /// 객체 변경 클래스 입니다.
    /// </summary>
    public class ObjectConverter
    {
        /// <summary>
        /// 문자열을 압축하는 게 효율적인 문자열 길이 입니다.
        /// </summary>
        public const int c_CompressableStringLength = 2000;

        /// <summary>
        /// 객체를 Byte배열로 변경합니다.
        /// </summary>
        /// <param name="aObject">Byte배열로 변경할 객체 입니다.</param>
        /// <returns>변경된 Byte배열 입니다.</returns>
        public static byte[] GetBytes(object aObject)
        {
            if (aObject == null) return null;

            BinaryFormatter tBF = new BinaryFormatter();
            tBF.AssemblyFormat = FormatterAssemblyStyle.Simple;
            tBF.TypeFormat = FormatterTypeStyle.TypesWhenNeeded;
            MemoryStream tMS = new MemoryStream();

            tBF.Serialize(tMS, aObject);
            return tMS.ToArray();
        }

        /// <summary>
        /// Byte배열을 객체로 변경합니다.
        /// </summary>
        /// <param name="aObject">변경할 Byte배열 입니다.</param>
        /// <returns>변경된 객체 입니다.</returns>
        public static object GetObject(byte[] aObject)
        {
            if (aObject == null) return null;
            if (aObject.Length == 0) return null;

            BinaryFormatter tBF = new BinaryFormatter();
            MemoryStream tMS = new MemoryStream(aObject);

            return tBF.Deserialize(tMS);
        }

        /// <summary>
        /// 객체를 압축데이터로 변환합니다.
        /// </summary>
        /// <param name="aObject"></param>
        /// <returns></returns>
        public static byte[] GetCompressData(object aObject)
        {
            byte[] tBytes = GetBytes(aObject);
            MemoryStream tMS = new MemoryStream();
            GZipStream tGZS = new GZipStream(tMS, CompressionMode.Compress, true);
            tGZS.Write(tBytes, 0, tBytes.Length);
            tGZS.Close();
            CompressData tCompressData = new CompressData(tBytes.Length, tMS);
            return GetBytes(tCompressData);
        }

        /// <summary>
        /// 압축 데이터를 해제하고 객체로 반환합니다.
        /// </summary>
        /// <param name="aObject"></param>
        /// <returns></returns>
        public static object GetDecompressData(byte[] aObject)
        {
            object tObject = GetObject(aObject);
            if (tObject.GetType() == typeof(CompressData))
            {
                return GetDecompressData((CompressData)tObject);
            }
            else
                return tObject;
        }

        /// <summary>
        /// 압축 데이터 클래스를 해제하고 객체로 반환합니다.
        /// </summary>
        /// <param name="aCompressData"></param>
        /// <returns></returns>
        private static object GetDecompressData(CompressData aCompressData)
        {
            GZipStream tGZipStream = null;
            int tOffset = 0;
            int tBytesRead = 0;
            byte[] tBytes = new byte[aCompressData.OriginalSize + 100];

            aCompressData.Stream.Position = 0;
            tGZipStream = new GZipStream(aCompressData.Stream, CompressionMode.Decompress);
            while (true)
            {
                tBytesRead = tGZipStream.Read(tBytes, tOffset, 100);
                if (tBytesRead == 0) break;
                tOffset += tBytesRead;
            }
            object tObject = ObjectConverter.GetObject(tBytes);
            tGZipStream.Close();
            aCompressData.Dispose();
            return tObject;
        }

        /// <summary>
        /// 문자열을 압축합니다.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string CompressString(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            MemoryStream ms = new MemoryStream();
            using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true))
            {
                zip.Write(buffer, 0, buffer.Length);
            }

            ms.Position = 0;

            byte[] compressed = new byte[ms.Length];
            ms.Read(compressed, 0, compressed.Length);

            byte[] gzBuffer = new byte[compressed.Length + 4];
            System.Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
            System.Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
            return Convert.ToBase64String(gzBuffer);
        }

        /// <summary>
        /// 압축된 문자열을 해제합니다.
        /// </summary>
        /// <param name="compressedText"></param>
        /// <returns></returns>
        public static string DecompressString(string compressedText)
        {
            byte[] gzBuffer = Convert.FromBase64String(compressedText);
            using (MemoryStream ms = new MemoryStream())
            {
                int msgLength = BitConverter.ToInt32(gzBuffer, 0);
                ms.Write(gzBuffer, 4, gzBuffer.Length - 4);

                byte[] buffer = new byte[msgLength];

                ms.Position = 0;
                using (GZipStream zip = new GZipStream(ms, CompressionMode.Decompress))
                {
                    zip.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }

    }


    #endregion //객체 변경 클래스 입니다 --------------------------------------------------------
}
