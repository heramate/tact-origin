using System;
using System.Collections.Generic;//List
using System.Collections;
//using System.Linq;
using System.Text;
using MKLibrary.MKData; //MKConvert

namespace TACT.KeepAliveServer
{
    [Serializable]
    public class TlvCollection //: MarshalByRefObject, IDictionary
    {
        /// <summary>
        /// TLV중 Type값 바이트 수
        /// </summary>
        public static int TypeBytes = 1;
        /// <summary>
        /// TLV중 Length값 바이트 수
        /// </summary>
        public static int LengthBytes = 1;
        /// <summary>
        /// TLV패킷 마지막 값
        /// </summary>
        public static byte EndofData = 0;
        /// <summary>
        /// TLV패킷 시작 값(prefix)
        /// </summary>
        public static int ValuePrefixBytes = 4;
        /// <summary>
        /// 패킷 전체 ByteArray값
        /// </summary>
        public byte[] RawData { get; private set; }

        /// <summary>
        /// Type,Value 세트 목록 (ByteArray의 해석결과)
        /// </summary>
        private Hashtable m_TlvDatas = new Hashtable(); // {type, value}

        public TlvCollection(byte[] aRawData)
        {
            RawData = aRawData;
            Decode(RawData, ValuePrefixBytes);
        }

        public TlvCollection(int aTypeBytes, int aLengthBytes, byte aEndofData) 
        {
            TypeBytes = aTypeBytes;
            LengthBytes = aLengthBytes;
            EndofData = aEndofData;
        }

        public TlvCollection(int aTypeBytes, int aLengthBytes, byte aEndofData, int aValuePrefixBytes, byte[] aRawData)
        {
            TypeBytes = aTypeBytes;
            LengthBytes = aLengthBytes;
            EndofData = aEndofData;
            ValuePrefixBytes = aValuePrefixBytes;
            RawData = aRawData;

            Decode(RawData, ValuePrefixBytes);
        }

        /// <summary>
        /// byte array 를 해석해서 m_TlvDatas 해쉬테이블에 담는다.
        /// </summary>
        /// <param name="aByteData">해석할 byte array값</param>
        /// <param name="aStartIndex">꺼낼 시작인덱스(recursive 기준값)</param>
        public void Decode(byte[] aByteData, int aStartIndex)
        {
            // 초기화
            if (aStartIndex <= ValuePrefixBytes) m_TlvDatas.Clear();

            if (aByteData == null) return;
            if (aByteData.Length <= aStartIndex) return;

            //string hexStr;
            byte[] temp = null;
            byte[] value = null;
            int type = 0, length = 0;

            if (aByteData.Length <= aStartIndex + TypeBytes) return;
            temp = new byte[TypeBytes];
            Array.Copy(aByteData, aStartIndex + 0, temp, 0, TypeBytes);
            System.Diagnostics.Debug.Assert(TypeBytes == 1, "Type값 길이는 1bytes라는 가정");
            type = (int)temp[0];

            // EndOfData 
            if (type == (int)EndofData) return;

            temp = new byte[LengthBytes];
            Array.Copy(aByteData, aStartIndex + TypeBytes, temp, 0, LengthBytes);
            length = (int)temp[0];

            System.Diagnostics.Debug.Assert(length >= 0);
            if (length > 0)
            {
                value = new byte[length];
                Array.Copy(aByteData, aStartIndex + TypeBytes + LengthBytes, value, 0, length);
            }

            m_TlvDatas.Add(type, value);
            string byteStr = string.Empty;
            switch(type)
            {
                case 5://ip
                case 6: case 8: case 10: //short
                    foreach(byte val in value) {
                        byteStr += string.Format("{0} ", val);
                    }
                    break;
                default: //string
                    byteStr = (value != null ? Encoding.ASCII.GetString(value) : "");
                    break;
            }
            GlobalClass.PrintLogInfo(string.Format("[Tlv.Decode] {0} | {1} | {2}", type, length, byteStr), true);


            // recursive
            Decode(aByteData, aStartIndex + TypeBytes + LengthBytes + length);
        }

        #region IDictionary 인터페이스 정의
        public void Add(object key, object value)
        {
            m_TlvDatas.Add(key, value);
        }

        public bool Contains(object key)
        {
            return m_TlvDatas.Contains(key);
        }

        public void Clear()
        {
            m_TlvDatas.Clear();
        }
        public ICollection Keys
        {
            get { return m_TlvDatas.Keys; }
        }

        public void Remove(object key)
        {
            m_TlvDatas.Remove(key);
        }

        public ICollection Values
        {
            get { return m_TlvDatas.Values; }
        }

        public object this[object key]
        {
            get { return m_TlvDatas[key]; }
            set { m_TlvDatas[key] = value; }
        }

        #endregion IDictionary 인터페이스 정의
    } // class TlvCollection
}
