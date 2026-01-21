using System;
using System.Collections.Generic;
using System.Text;
using System.Collections; //BitArray
using System.Runtime.InteropServices;//Marshal
using System.ComponentModel;
using System.Net;//[DefaultValue()]

namespace TACT.KeepAliveServer
{
    /*
    KeepAliveMsg Format
    1)수신
     모델명 (MIN)
     RPCS 관리IPv4 (MIN)
     USIM Serial No(=ICCID) (FULL)
     IMEI (FULL)
    2)송신(reply) : 응답안하다가 장비접속 요청이 있으면 응답을 통해 SSH터널을 뚫도록 요청 
     모델명 (FULL)
     RPCS 관리IPv4 (FULL)
     SSH 터널 생성요청 플래그
     SSH 접속 도메인(IP) (SSH)
     SSH 터널용 PORT (SSH)
     SSH 접속 PORT (SSH)
     SSH 접속 계정 (SSH)
     SSH 접속 PWD (SSH)
    */

    [Serializable]
    public class KeepAliveMsg //: MarshalByRefObject, ICloneable , ICollection
    {
        #region 메시지 데이터 : [Type] 설명 -------------------------------------------

        /// <summary>
        /// KAM수신시각
        /// </summary>
        [DefaultValue("")]//DateTime.MinValue
        public DateTime RecvDateTime { get; set; }

        /// <summary>
        /// KeepAlive 수신IP (Cat.M1)
        /// </summary>
        [DefaultValue("")]

        public string RecvIPAddress { get; set; }
        /// <summary>
        /// KeepAlive 수신포트 (Cat.M1)
        /// </summary>
        [DefaultValue(0)]
        public int RecvPort { get; set; }

        /// <summary>
        /// [발송KAM정보] KAM발송 대기시각
        /// </summary>
        [DefaultValue("")]//DateTime.MinValue
        public DateTime TimestampWaiting { get; set; }


        /// <summary>
        /// KeepAlive 발송IP (Cat.M1)
        /// </summary>
        [DefaultValue("")]
        public string SendIPAddress { get; set; }

        /// <summary>
        /// KeepAlive 발송포트 (Cat.M1)
        /// </summary>
        [DefaultValue(0)]
        public int SendPort { get; set; }

        /// <summary>
        /// KAM발송한 시각
        /// </summary>
        [DefaultValue("")]//DateTime.MinValue
        public DateTime SentDateTime { get; set; }

        /// <summary>
        /// [SEND] 요청KAM 발송한 횟수
        /// </summary>
        [DefaultValue(0)]
        public int SentCount { get; set; }


        //public static string c_EmptyString = string.Empty;
        /// <summary>
        /// [1] 장치 모델명 (ex: V108)
        /// </summary>
        [DefaultValue("")]
        public string ModelName { get; set; }

        /// <summary>
        /// [5] 장비IPv4 (RPCS장비구분 키)
        /// </summary>
        [DefaultValue("")]
        public string DeviceIP { get; set; }

        /// <summary>
        /// [2] LTE chipset serial number (예: SIM-TI2B1)
        /// </summary>
        [DefaultValue("")]
        public string SerialNumber { get; set; }


        /// <summary>
        /// [3] LTE USIM 정보 (예: 1808300090201)
        /// </summary>
        [DefaultValue("")]
        public string USIM { get; set; }

        /// <summary>
        /// [4] LTE IMEI 정보  (예: 358777070000329)
        /// </summary>
        [DefaultValue("")]
        public string IMEI { get; set; }

        /// <summary>
        /// [9] LTE 모듈명
        /// </summary>
        [DefaultValue("")]
        public string LTEModuleName { get; set; }

        /// <summary>
        /// [7] SSH서버 접속 도메인 또는 IP  (예: catm1.skbroadband.com)
        /// </summary>
        [DefaultValue("")]
        public string SSHServerDomain { get; set; }
        
        /// <summary>
        /// [6] SSH서버 접속 포트  (예: 40002)
        /// </summary>
        [DefaultValue(0)]
        public int SSHPort { get; set; }


        /// <summary>
        /// [8] SSH터널 생성 요청 플래그
        /// </summary>
        //private E_SSHTunnelCreateOption m_SSHTunnelCreateOption = E_SSHTunnelCreateOption.Unknown;
        [DefaultValue(E_SSHTunnelCreateOption.Unknown)]
        public E_SSHTunnelCreateOption SSHTunnelCreateOption { get; set; }

        /// <summary>
        /// [10] SSH터널링 포트
        /// </summary>
        [DefaultValue(0)]
        public int SSHTunnelPort { get; set; }

        /// <summary>
        /// [11] SSH서버 접속 계정
        /// </summary>
        [DefaultValue("")]
        public string SSHUserID { get; set; }

        /// <summary>
        /// [12] SSH서버 접속 패스워드
        /// </summary>
        [DefaultValue("")]
        public string SSHPassword { get; set; }

        /// <summary>
        /// [DB값] 장비ID (NE_NE.NeID)
        /// </summary>
        [DefaultValue(0)]
        public int NeId { get; set; }



        #endregion 메시지 데이터 : [Type] 설명 ---------------------------------
        public string ToString(string title)
        {
            /// Receive(Min)
            if (!IsFullMessage())
            {
                return string.Format("{0}: 장비IP={1}, USIM={2}, 수신주소={3}:{4}, 수신시각={5}, 발송대기시작={6}"
                                  ,title, DeviceIP, USIM
                                  ,RecvIPAddress, RecvPort
                                  ,Util.DateTimeToLogValue(RecvDateTime)
                                  ,Util.DateTimeToLogValue(this.TimestampWaiting));
            }
            else
            {
                /// Send(Full)
                if (SSHTunnelPort > 0)
                {
                    const string c_formatToStringSend =
                    "{0}: 장비IP={1}, SSHTunnelPort={2}, SSHTunnelCreateOption={3}, USIM={4}, 장치모델명={5},  " +
                    "SSHServerDomain={6}, SSHPort={7}, SSHUserID={8}, [12]SSHPassword={9},  " +
                    "발송주소={10}:{11}, 발송시각={12}, 최근KAM수신주소={13}:{14}, 최근KAM수신시각 = {15}\r\n";
                    return string.Format(c_formatToStringSend, title,
                                      DeviceIP, SSHTunnelPort, SSHTunnelCreateOption, USIM, ModelName,
                                      SSHServerDomain, SSHPort, SSHUserID, SSHPassword,
                                      SendIPAddress, SendPort, Util.DateTimeToLogValue(SentDateTime),
                                      RecvIPAddress, RecvPort, Util.DateTimeToLogValue(RecvDateTime));
                }
                /// Receive(Full)
                else
                {
                    const string c_formatToStringRecv =
                    "{0}: 장비IP={1}, USIM={2}, 장치모델명={3}, SerialNumber={4}, IMEI={5}, LTE모듈명={6}, " +
                    "수신주소(from)={7}:{8}, 수신시각 = {9}\r\n";
                    return string.Format(c_formatToStringRecv, title,
                                      DeviceIP, USIM, ModelName, SerialNumber, IMEI, LTEModuleName,
                                      RecvIPAddress, RecvPort, Util.DateTimeToLogValue(RecvDateTime));
                }
            }
        }

        /// <summary>
        /// 기본생성자 
        /// </summary>
        public KeepAliveMsg() { }

        public KeepAliveMsg(string aDeviceIP, E_SSHTunnelCreateOption aOption)
        {
            this.DeviceIP = aDeviceIP;
            this.SSHTunnelCreateOption = aOption;
        }

        public KeepAliveMsg(string aDeviceIP, E_SSHTunnelCreateOption aOption, int aTunnelPort)
        {
            this.DeviceIP = aDeviceIP;
            this.SSHTunnelCreateOption = aOption;
            this.SSHTunnelPort = aTunnelPort;
        }

        /// <summary>
        /// 복사 생성자(DeepCopy)
        /// </summary>
        /// <param name="aSource"></param>
        public KeepAliveMsg(KeepAliveMsg aSource)
        {
            Copy(aSource, true);
        }



        /// <summary>
        /// Keep-Alive 메시지 구성이 min인지 full인지 구분(class TlvDef)
        /// </summary>
        /// <returns></returns>
        public bool IsFullMessage()
        {
            /// TODO: KAM연동포맷이 바뀔경우 수정이 필요할 수 있음
            return !string.IsNullOrEmpty(this.IMEI) || !string.IsNullOrEmpty(this.SSHServerDomain);
        }

        /// <summary>
        /// 보낼KAM 생성을 위한 복사 메서드
        /// </summary>
        /// <param name="aSource"></param>
        /// <param name="isFullReply"></param>
        /// <param name="isRecvAddrCopy"></param>
        /// <param name="isSendAddrCopy"></param>
        public void CopyFrom(KeepAliveMsg aSource, bool isFullReply, bool isRecvAddrCopy, bool isSendAddrCopy)
        {
            //TlvDef.s_SendStructureMin
            this.DeviceIP = aSource.DeviceIP;
            this.USIM = aSource.USIM;

            //TlvDef.s_SendStructureFull 참고
            if (isFullReply)
            {
                this.ModelName = aSource.ModelName;
                this.DeviceIP = aSource.DeviceIP;
                this.USIM = aSource.USIM;
            }

            /// 보낼KAM의 RecvXXX는 최근 수신KAM 정보
            this.RecvDateTime = aSource.RecvDateTime;
            this.RecvIPAddress = aSource.RecvIPAddress;
            this.RecvPort = aSource.RecvPort;

            /// 보낼 리모트 주소
            this.SendIPAddress = aSource.RecvIPAddress;
            this.SendPort = aSource.RecvPort;
            //this.SendDateTime = DateTime.Now; //발신시각은 Send시 직접 업데이트
        }



        /// <summary>
        /// 속성값 복사
        /// </summary>
        /// <param name="aSource"></param>
        /// <param name="aIsDeepCopy"></param>
        public void Copy(KeepAliveMsg aSource, bool aIsDeepCopy)
        {
            if (aIsDeepCopy)
            {
                this.RecvDateTime = aSource.RecvDateTime;
                this.RecvIPAddress = aSource.RecvIPAddress;
                this.RecvPort = aSource.RecvPort;
                this.SendIPAddress = aSource.SendIPAddress;
                this.SendPort = aSource.SendPort;
                this.TimestampWaiting = aSource.TimestampWaiting;
            }

            this.DeviceIP = aSource.DeviceIP;
            this.USIM = aSource.USIM;
            this.LTEModuleName = aSource.LTEModuleName;
            this.IMEI = aSource.IMEI;
            this.ModelName = aSource.ModelName;
            this.SerialNumber = aSource.SerialNumber;
            this.NeId = aSource.NeId;

            if (aIsDeepCopy)
            {
                this.SSHPassword = aSource.SSHPassword;
                this.SSHPort = aSource.SSHPort;
                this.SSHServerDomain = aSource.SSHServerDomain;
                this.SSHTunnelCreateOption = aSource.SSHTunnelCreateOption;
                this.SSHTunnelPort = aSource.SSHTunnelPort;
                this.SSHUserID = aSource.SSHUserID;

                //this.SendCount = aSource.SendCount;
            }
        }

        public KeepAliveMsg(byte[] byteDatas, DateTime aRecvTime, string aRecvIPAddr, int aRecvPort)
        {
            TlvCollection tlvData = new TlvCollection(byteDatas);
            RecvIPAddress = aRecvIPAddr;
            RecvPort = aRecvPort;
            RecvDateTime = aRecvTime;

            Decode(tlvData);
        }

        public KeepAliveMsg(byte[] byteDatas)
        {
            TlvCollection tlvData = new TlvCollection(byteDatas);
            Decode(tlvData);
        }

        public KeepAliveMsg(TlvCollection tlvList)
        {
            Decode(tlvList);
        }

        public void Decode(TlvCollection tlvList)
        {
            //ClearKeepAliveTypes();
            ICollection keyList = tlvList.Keys;
            foreach (int dataType in keyList)
            {
                byte[] value = (byte[])tlvList[dataType];
                if (value == null || value.Length == 0)
                {
                    GlobalClass.PrintLogError(string.Format("[KeepAliveClass.Decode] dataType={0}, value is null || lengh == 0", ((E_KeepAliveType)dataType).ToString()), true);
                    continue;
                }

                try {
                    E_KeepAliveType keepAliveType = (E_KeepAliveType)dataType;
                } catch (Exception ex) {
                    GlobalClass.PrintLogException(string.Format("[KeepAliveClass.Decode] E_KeepAliveType정의 값이 아닙니다.{0}", dataType), ex);
                    continue;
                }

                //m_TypeList.Add((E_KeepAliveType)dataType);
                //GlobalClass.PrintLogInfo("- KeepAliveMsg.Decode() - (3)E_KeepAliveType = " + ((E_KeepAliveType)dataType).ToString());
                switch ((E_KeepAliveType)dataType)
                {
                    //// End of data
                    case E_KeepAliveType.EndOfData:
                        break;

                    // 모델명, 가변길이 (string format, without null termination)
                    case E_KeepAliveType.ModelName:
                       ModelName = (value == null || value.Length < 1 ? string.Empty : Encoding.ASCII.GetString(value));
                        break;
                    // Serial number, 가변길이 (string format, without null termination)
                    case E_KeepAliveType.SerialNumber:
                       SerialNumber = (value == null || value.Length < 1 ? string.Empty : Encoding.ASCII.GetString(value));
                        break;
                    // USIM 정보, 가변길이 (string format, without null termination)
                    case E_KeepAliveType.USIM:
                       USIM = (value == null || value.Length < 1 ? string.Empty : Encoding.ASCII.GetString(value));
                        break;
                    // IMEI 정보, 가변길이 (string format, without null termination)
                    case E_KeepAliveType.IMEI:
                       IMEI = (value == null || value.Length < 1 ? string.Empty : Encoding.ASCII.GetString(value));
                        break;
                    // SSH 서버 도메인(or IP), 가변길이 (string format, without null termination)
                    case E_KeepAliveType.SSHServerDomain:
                       SSHServerDomain = (value == null || value.Length < 1 ? string.Empty : Encoding.ASCII.GetString(value));
                        break;
                    // SSH 서버 접속 계정, 가변길이 (string format, without null termination)
                    case E_KeepAliveType.SSHUserID:
                       SSHUserID = (value == null || value.Length < 1 ? string.Empty : Encoding.ASCII.GetString(value));
                        break;
                    // SSH 서버 접속 패스워드, 가변길이 (string format, without null termination)
                    case E_KeepAliveType.SSHPassword:
                       SSHPassword = (value == null || value.Length < 1 ? string.Empty : Encoding.ASCII.GetString(value));
                        break;
                    // LTE Cat.M1 모듈명
                    case E_KeepAliveType.LTEModuleName:
                        LTEModuleName = (value == null || value.Length < 1 ? string.Empty : Encoding.ASCII.GetString(value));
                        break;

                    // 관리 IPv4 정보, 4 bytes (htonl)
                    case E_KeepAliveType.IPv4Address:
                        DeviceIP = string.Empty;

                        if (value == null) GlobalClass.PrintLogError("[KeepAliveClass.Decode] IP주소 값이 null입니다.");    
                        else if (value.Length == 4)
                        {
                            try
                            {
                                IPAddress address = new IPAddress(value);
                                DeviceIP = address.ToString();
                            }
                            catch (Exception e)
                            {
                                GlobalClass.PrintLogException("[KeepAliveClass.Decode] DeviceIP변환중 예외발생", e);
                            }
                        }
                        else GlobalClass.PrintLogError(string.Format("[KeepAliveClass.Decode] IP주소 길이가 4자리가 아닙니다.{0}", value.Length));
                        break;

                    // SSH 서버 접속 포트, 2 bytes (htons)
                    case E_KeepAliveType.SSHPort:
                        if (value != null && value.Length == 2)//ushort
                        {
                            //string byteStr = Util.ByteNumbersToString(value, ' ');
                            //GlobalClass.PrintLogError(byteStr);
                            //if (BitConverter.IsLittleEndian) Array.Reverse(value);
                            //m_SSHPort = (int)BitConverter.ToUInt16(value, 0);
                           SSHPort = (value[0] << 8 | value[1] << 0);
                        }
                        break;

                    // SSH remote port, 2 bytes (htons)
                    case E_KeepAliveType.SSHTunnelPort:
                        if (value != null && value.Length == 2)//ushort
                        {
                            //m_SSHTunnelPort = BitConverter.ToUInt16(value, 0);
                           SSHTunnelPort = (value[0] << 8 | value[1] << 0);
                        }
                        break;

                    // SSH Tunnel 생성요청 여부, 1 bytes (0xFF: class, 0x01: create)
                    case E_KeepAliveType.SSHTunnelCreateOption:
                        if (value != null && value.Length > 0)
                        {
                           SSHTunnelCreateOption = (E_SSHTunnelCreateOption)value[0];
                        }
                        break;
                    default:
                        //m_TypeList.Remove((E_KeepAliveType)dataType);
                        GlobalClass.PrintLogError(string.Format("[KeepAliveClass.Decode] 정의되지 않은 E_KeepAliveType 타입값입니다. {0}", dataType));
                        //System.Diagnostics.Debug.Assert(false, string.Format("정의되지 않은 E_KeepAliveType 타입값입니다. {0}", dataType));
                        break;
                } // End of switch

            } // End of foreach
        }

        /// <summary>
        /// 보낼 byte array메시지를 만든다.
        /// </summary>
        /// <param name="aTypeList"></param>
        /// <param name="aIsApplyBase64"></param>
        /// <returns></returns>
        public byte[] Encode(E_KeepAliveType [] aTypeList, bool aIsApplyBase64)
        {
            try
            {
                List<byte> byteList = new List<byte>();
                //fixed data
                byteList.AddRange(_EncodingStringToGetBytes("FACT"));

                foreach (E_KeepAliveType aType in aTypeList)
                {
                    _AddToByteList(aType, ref byteList);
                }

                byte[] byteDatas = byteList.ToArray();
                if (aIsApplyBase64)
                {
                    GlobalClass.PrintLogInfo("[KeepAliveMsg.Encode]Base64적용 전▶" + Encoding.ASCII.GetString(byteList.ToArray()), true);
                    //byteDatas = Routrek.Toolkit.Base64.Encode(tKAPacket.Packet);
                    string str = Convert.ToBase64String(byteDatas);
                    byteDatas = _EncodingStringToGetBytes(str);
                }
                GlobalClass.PrintLogInfo("[KeepAliveMsg.Encode]Encode결과▶" + Encoding.ASCII.GetString(byteDatas), true);
                return byteDatas;
            }
            catch (Exception e)
            {
                GlobalClass.PrintLogException("[KeepAliveMsg.Encode] 패킷 인코딩중 오류 발생(KeepAliveMsg to byte[])", e);
            }
            return null;
        }

        private void _AddToByteList(E_KeepAliveType aType, ref List<byte> aByteList)
        {
            /// KAM연동포맷 기본값 유무 확인
            Util.Assert(!string.IsNullOrEmpty(USIM), "KAM연동포맷(Min/Full KAM) 기본값인 USIM값이 없습니다. 확인필요!");
            Util.Assert(!string.IsNullOrEmpty(DeviceIP), "KAM연동포맷(Min/Full KAM) 기본값인 장비IP값이 없습니다. 확인필요!");

            byte[] value = null;
            int length = 0;

            switch (aType)  
            {
                // End of data
                case E_KeepAliveType.EndOfData:
                    aByteList.AddRange(new byte[] { 0, 0 });
                    break;

                // [1] 모델명, 가변길이 (string format, without null termination)
                case E_KeepAliveType.ModelName:
                    value = _EncodingStringToGetBytes(ModelName);
                    break;
                // [2] Cat.M1 Serial number, 가변길이 (string format, without null termination)
                case E_KeepAliveType.SerialNumber:
                    value = _EncodingStringToGetBytes(SerialNumber);
                    break;
                // [3] Cat.M1 USIM 정보, 가변길이 (string format, without null termination)
                case E_KeepAliveType.USIM:
                    value = _EncodingStringToGetBytes(USIM);
                    break;
                // [4] IMEI 정보, 가변길이 (string format, without null termination)
                case E_KeepAliveType.IMEI:
                    value = _EncodingStringToGetBytes(IMEI);
                    break;
                // [5] 장비 IPv4 정보, 4 bytes (htonl)
                case E_KeepAliveType.IPv4Address:
                    string[] ipValues = DeviceIP.Split('.');
                    if (ipValues != null && ipValues.Length == 4) {
                        value = new byte[] { Convert.ToByte(ipValues[0]), Convert.ToByte(ipValues[1]), Convert.ToByte(ipValues[2]), Convert.ToByte(ipValues[3]) };
                    }
                    break;
                // [7] SSH 서버 도메인(or IP), 가변길이 (string format, without null termination)
                case E_KeepAliveType.SSHServerDomain:
                    value = _EncodingStringToGetBytes(SSHServerDomain);
                    break;
                // [6] SSH 서버 접속 포트, 2 bytes (htons)
                case E_KeepAliveType.SSHPort: 
                    value = BitConverter.GetBytes((ushort)SSHPort);
                    Array.Reverse(value); // to little endian
                    break;
                // [8] SSH Tunnel 생성요청 여부, 1 bytes (0xFF: class, 0x01: create)
                case E_KeepAliveType.SSHTunnelCreateOption:
                    value = new byte[] { (byte)SSHTunnelCreateOption };
                    break;
                // [9] Cat.M1 모듈명
                case E_KeepAliveType.LTEModuleName:
                    value = _EncodingStringToGetBytes(LTEModuleName);
                    break;
                // [10] SSH터널용 리모트 포트, 2 bytes (htons)
                case E_KeepAliveType.SSHTunnelPort:
                    value = BitConverter.GetBytes((ushort)SSHTunnelPort);
                    Array.Reverse(value); // to little endian
                    break;
                // [11] SSH 서버 접속 계정, 가변길이 (string format, without null termination)
                case E_KeepAliveType.SSHUserID:
                    value = _EncodingStringToGetBytes(SSHUserID);
                    break;
                // [12] SSH 서버 접속 패스워드, 가변길이 (string format, without null termination)
                case E_KeepAliveType.SSHPassword:
                    value = _EncodingStringToGetBytes(SSHPassword);
                    break;

                default:
                    GlobalClass.PrintLogError(string.Format("[KeepAliveClass._AddToByteList] 지원하지 않는 E_KeepAliveType 타입값입니다. {0}", aType.ToString()));
                    return;
            }

            length = (value != null ? value.Length : 0);
            aByteList.Add((byte)aType);   // Type
            aByteList.Add((byte)length);  // Length
            if (length > 0)
            {
                aByteList.AddRange(value);    // Value
            }
            System.Diagnostics.Debug.Write(string.Format("[메시지항목추가] Type={0}, Length={1}, Value={2}", (byte)aType, length, value != null ? Encoding.ASCII.GetString(value) : ""));
        }

        private byte[] _EncodingStringToGetBytes(string aValue)
        {
            try
            {
                // Null 경우 처리
                if (string.IsNullOrEmpty(aValue)) {
                    return new byte[0];
                }
                else {
                    return Encoding.ASCII.GetBytes(aValue);
                } 
            }
            catch (Exception e)
            {
                GlobalClass.PrintLogException("[KeepAliveClass._EncodingStringToGetBytes] _EncodingStringToGetBytes(string) 예외발생!", e);
                return new byte[0];
            }
        }
        

        /// <summary>
        /// 보낼 응답(Reply) 메시지를 만든다.
        /// </summary>
        /// <param name="aInfo"></param>
        /// <param name="aIsFullMsg"></param>
        /// <returns></returns>
        //public static KeepAliveMsg createReply(KeepAliveMsg aInfo, bool aIsFullMsg)
        //{
        //    KeepAliveMsg replyMsg = new KeepAliveMsg(aInfo.USIM, aInfo.DeviceIP);
        //    replyMsg.ServerDateTime = DateTime.Now;

        //    if (aIsFullMsg)
        //    {
        //        replyMsg.Copy(aInfo, false);

        //        replyMsg.SSHServerDomain = GlobalClass.m_SystemInfo.SSHServerIP;//catm1.skbroadband.com
        //        replyMsg.SSHPort = GlobalClass.m_SystemInfo.SSHServerPort;
        //        replyMsg.SSHUserID = GlobalClass.m_SystemInfo.SSHUserID;
        //        replyMsg.SSHPassword = GlobalClass.m_SystemInfo.SSHPassword;
        //        //replyMsg.SSHTunnelPort = 0; // 포트정보는 장비에 요청 직전에 설정(터널링포트 점유 예방)
        //    }
        //    return replyMsg;
        //}
    } // End of class (KeepAliveMsg)


    /// <summary>
    /// 장비IP별 KeepAliveMsg
    /// </summary>
    public class KeepAliveCollection : List<KeepAliveMsg>
    {
        public enum E_FindKeyType
        {
            DeviceIP,
            USIM
        }

        public KeepAliveCollection() { }
        public KeepAliveMsg this[string aUSIM]
        {
            get
            {
                return Find(aUSIM, E_FindKeyType.USIM);
            }
            //set
            //{
            //    this[aUSIM] = value;
            //}
        }

        /// <summary>
        /// 키값으로 메시지를 찾는다.
        /// </summary>
        /// <param name="aKey">검색어</param>
        /// <param name="aKeyType">검색키(장비IP,IMEI)</param>
        /// <returns></returns>
        public KeepAliveMsg Find(string aKey, E_FindKeyType aKeyType)
        {
            foreach (KeepAliveMsg info in this)
            {
                switch (aKeyType)
                {
                    // RPCS장비IP로 검색
                    case E_FindKeyType.DeviceIP:
                        if (info.DeviceIP.Equals(aKey)) return info;
                        break;
                    // LTE IMEI로 검색
                    case E_FindKeyType.USIM:
                        if (info.USIM.Equals(aKey)) return info;
                        break;
                    default:
                        Util.Assert(false, string.Format("[KeepAliveCollection] 처리되지 않은 검색키 타입입니다.{0}", aKeyType));
                        break;
                }
            }
            return null;
        }


    } // End of class KeepAliveCollection

    #region KeepAlive 패킷 임시 저장 클래스 입니다 --------------------------------------------
    /// <summary>
    /// KeepAlive 패킷을 저장하는 클래스 입니다.
    /// </summary>
    [Serializable]
    public class KeepAlivePacket
    {
        /// <summary>
        /// 패킷 데이터
        /// </summary>
        private byte[] m_Packet = null;
        /// <summary>
        /// 패킷을 받은 시각
        /// </summary>
        private DateTime m_Time = DateTime.Now;
        /// <summary>
        /// 패킷을 전송할 장비 IP주소
        /// </summary>
        private string m_SenderAddress = "";
        /// <summary>
        /// 패킷을 전송할 장비 포트
        /// </summary>
        private int m_PortNumber = 0;

        /// <summary>
        ///  패킷 클래스의 생성자 입니다;
        /// </summary>
        /// <param name="vPacket"> 패킷 데이터 입니다.</param>
        /// <param name="vSenderAddress"> 발신 장비 IP주소 입니다.</param>
        //public KeepAlivePacket(byte[] vPacket, string vSenderAddress)
        //{
        //    m_Packet = vPacket;
        //    m_SenderAddress = vSenderAddress;
        //}

        public KeepAlivePacket(byte[] vPacket, string vSenderAddress, int vPortNumber)
        {
            m_Packet = vPacket;
            m_SenderAddress = vSenderAddress;
            m_PortNumber = vPortNumber;
        }

        /// <summary>
        /// 패킷 클래스의 생성자 입니다.
        /// </summary>
        /// <param name="vPacket">패킷 데이터 입니다.</param>
        /// <param name="vTime">발생한 시간입니다.</param>
        /// <param name="vSenderAddress">발신 장비 IP주소 입니다.</param>
        public KeepAlivePacket(byte[] vPacket, DateTime vTime, string vSenderAddress, int vPortNumber)
        {
            m_Packet = vPacket;
            m_Time = vTime;
            m_SenderAddress = vSenderAddress;
            m_PortNumber = vPortNumber;
        }

        /// <summary>
        /// 패킷 데이터를 가져옵니다.
        /// </summary>
        public byte[] Packet
        {
            get { return m_Packet; }
            set { m_Packet = value; }
            //set {
            //    if (value == null || value.Length == 0) Packet = null;
            //    Packet = new byte[value.Length];
            //    Array.Copy(value, 0, Packet, 0, value.Length);
            //}
        }

        /// <summary>
        /// 발생시간을 가져옵니다.
        /// </summary>
        public DateTime Time
        {
            get { return m_Time; }
        }

        /// <summary>
        /// 발신 장비의 IP주소를 가져옵니다.
        /// </summary>
        public string SenderAddress
        {
            get { return m_SenderAddress; }
        }

        /// <summary>
        /// 발신 장비의 포트번호를 가져옵니다.
        /// </summary>
        public int PortNumber
        {
            get { return m_PortNumber; }
        }

    } // End of class KeepAlivePacket

    #endregion // KeepAlive패킷 임시 저장 클래스 입니다 --------------------------------------------
}
