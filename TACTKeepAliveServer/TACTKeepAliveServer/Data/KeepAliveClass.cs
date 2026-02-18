using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.ComponentModel;
using System.Buffers;

namespace TACT.KeepAliveServer
{
    [Serializable]
    public class KeepAliveMsg
    {
        public DateTime RecvDateTime { get; set; } = DateTime.Now;
        public string? RecvIPAddress { get; set; }
        public int RecvPort { get; set; }
        public DateTime TimestampWaiting { get; set; }
        public string? SendIPAddress { get; set; }
        public int SendPort { get; set; }
        public DateTime SentDateTime { get; set; }
        public int SentCount { get; set; }

        public string? ModelName { get; set; }
        public string? DeviceIP { get; set; }
        public string? SerialNumber { get; set; }
        public string? USIM { get; set; }
        public string? IMEI { get; set; }
        public string? LTEModuleName { get; set; }
        public string? SSHServerDomain { get; set; }
        public int SSHPort { get; set; }
        public E_SSHTunnelCreateOption SSHTunnelCreateOption { get; set; }
        public int SSHTunnelPort { get; set; }
        public string? SSHUserID { get; set; }
        public string? SSHPassword { get; set; }
        public int NeId { get; set; }

        public KeepAliveMsg() { }

        public KeepAliveMsg(string deviceIP, E_SSHTunnelCreateOption option, int tunnelPort = 0)
        {
            DeviceIP = deviceIP;
            SSHTunnelCreateOption = option;
            SSHTunnelPort = tunnelPort;
        }

        // 레거시 호환용 생성자
        public KeepAliveMsg(byte[] data, DateTime recvTime, string recvIP, int recvPort)
        {
            RecvDateTime = recvTime;
            RecvIPAddress = recvIP;
            RecvPort = recvPort;
            Decode(data);
        }

        public bool IsFullMessage() => !string.IsNullOrEmpty(IMEI) || !string.IsNullOrEmpty(SSHServerDomain);

        public void CopyFrom(KeepAliveMsg source, bool copyNetworkInfo, bool copyDeviceIdent, bool copySshInfo)
        {
            if (copyNetworkInfo)
            {
                RecvIPAddress = source.RecvIPAddress;
                RecvPort = source.RecvPort;
            }
            if (copyDeviceIdent)
            {
                DeviceIP = source.DeviceIP;
                USIM = source.USIM;
                IMEI = source.IMEI;
                SerialNumber = source.SerialNumber;
                ModelName = source.ModelName;
                NeId = source.NeId;
            }
            if (copySshInfo)
            {
                SSHServerDomain = source.SSHServerDomain;
                SSHPort = source.SSHPort;
                SSHUserID = source.SSHUserID;
                SSHPassword = source.SSHPassword;
                SSHTunnelPort = source.SSHTunnelPort;
            }
        }

        public void Decode(ReadOnlySpan<byte> data)
        {
            if (data.Length < 4) return;

            int cursor = 0;
            if (data.Slice(0, 4).SequenceEqual("FACT"u8))
            {
                cursor = 4;
            }

            while (cursor + 2 <= data.Length)
            {
                byte type = data[cursor++];
                byte length = data[cursor++];

                if (cursor + length > data.Length) break;

                var valueSpan = data.Slice(cursor, length);
                ProcessTlvItem((E_KeepAliveType)type, valueSpan);
                cursor += length;
            }
        }

        private void ProcessTlvItem(E_KeepAliveType type, ReadOnlySpan<byte> value)
        {
            switch (type)
            {
                case E_KeepAliveType.ModelName: ModelName = Encoding.ASCII.GetString(value); break;
                case E_KeepAliveType.SerialNumber: SerialNumber = Encoding.ASCII.GetString(value); break;
                case E_KeepAliveType.USIM: USIM = Encoding.ASCII.GetString(value); break;
                case E_KeepAliveType.IMEI: IMEI = Encoding.ASCII.GetString(value); break;
                case E_KeepAliveType.SSHServerDomain: SSHServerDomain = Encoding.ASCII.GetString(value); break;
                case E_KeepAliveType.SSHUserID: SSHUserID = Encoding.ASCII.GetString(value); break;
                case E_KeepAliveType.SSHPassword: SSHPassword = Encoding.ASCII.GetString(value); break;
                case E_KeepAliveType.LTEModuleName: LTEModuleName = Encoding.ASCII.GetString(value); break;
                case E_KeepAliveType.IPv4Address: if (value.Length == 4) DeviceIP = new IPAddress(value).ToString(); break;
                case E_KeepAliveType.SSHPort: if (value.Length == 2) SSHPort = (value[0] << 8 | value[1]); break;
                case E_KeepAliveType.SSHTunnelPort: if (value.Length == 2) SSHTunnelPort = (value[0] << 8 | value[1]); break;
                case E_KeepAliveType.SSHTunnelCreateOption: if (value.Length > 0) SSHTunnelCreateOption = (E_SSHTunnelCreateOption)value[0]; break;
            }
        }

        public string ToString(string prefix)
        {
            return $"{prefix} DeviceIP={DeviceIP}, USIM={USIM}, Model={ModelName}";
        }
    }

    public class KeepAliveCollection : List<KeepAliveMsg>
    {
        public enum E_FindKeyType { DeviceIP, USIM }

        public KeepAliveMsg? Find(string aKey, E_FindKeyType aKeyType)
        {
            foreach (KeepAliveMsg info in this)
            {
                if (aKeyType == E_FindKeyType.DeviceIP && info.DeviceIP == aKey) return info;
                if (aKeyType == E_FindKeyType.USIM && info.USIM == aKey) return info;
            }
            return null;
        }
    }

    public class KeepAlivePacket
    {
        public byte[]? Packet { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;
        public string SenderAddress { get; set; } = string.Empty;
        public int PortNumber { get; set; }
    }
}
