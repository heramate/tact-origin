using System;
using System.Collections.Generic;
using System.Text;
using Serilog;

namespace TACT.KeepAliveServer
{
    public class TlvCollection
    {
        public static int TypeBytes = 1;
        public static int LengthBytes = 1;
        public static byte EndofData = 0;
        public static int ValuePrefixBytes = 4;

        public byte[]? RawData { get; private set; }
        private readonly Dictionary<byte, byte[]> _tlvDatas = new();

        public TlvCollection(byte[] rawData)
        {
            RawData = rawData;
            Decode(rawData.AsSpan(ValuePrefixBytes));
        }

        public void Decode(ReadOnlySpan<byte> data)
        {
            _tlvDatas.Clear();
            int cursor = 0;

            while (cursor + TypeBytes + LengthBytes <= data.Length)
            {
                byte type = data[cursor++];
                if (type == EndofData) break;

                byte length = data[cursor++];
                if (cursor + length > data.Length) break;

                var value = data.Slice(cursor, length).ToArray();
                _tlvDatas[type] = value;
                
                Log.Debug("[Tlv.Decode] Type: {Type}, Length: {Length}", type, length);
                cursor += length;
            }
        }

        public byte[]? this[byte key]
        {
            get => _tlvDatas.TryGetValue(key, out var val) ? val : null;
            set => _tlvDatas[key] = value ?? Array.Empty<byte>();
        }

        public bool Contains(byte key) => _tlvDatas.ContainsKey(key);
        public void Clear() => _tlvDatas.Clear();
        public IEnumerable<byte> Keys => _tlvDatas.Keys;
    }
}
