using System;
using System.Collections.Generic;//Dictionary<>
using System.Net; //IPEndPoint
using System.Text; //Thread
using System.Threading;


namespace TACT.KeepAliveServer
{
    /// - KAM수신시마다 DB정보(LTE_NE)와 병합하여 업데이트된다.
    /// - Keep-Alive의 송신자(sender) IP,포트 정보는 KAM전송(reply)시 사용된다.
    /// - 장비에 요청시 해당 송신처로 우선 전송시도하고 실패시 다음 KAM까지 기다렸다가 전송하기 위해 저장
    /// - 
    /// - LTE세션 타임아웃된 정보는 자동으로 삭제하는 스레드 포함

    /// <summary>
    /// 장비별 최신 KAM정보 유지관리
    /// ├ 다음 KAM수신을 기다리지 않고 즉시 최근 수신KAM의 발신처로 SEND하여 접속을 하기 위함
    ///   (근거: KAM수신로그 분석 결과 최대 41시간 동안 유지된 발신처 리모트주소도 있음)
    /// ├ KAM수신시 업데이트(DB의 최신값과 수신KAM을 병합한 결과를 저장)
    /// ├ 장비에 터널 요청(SEND)시 필요한 발신처IP/포트(LTE세션)를 관리한다.
    /// └ 발송할 Keep-Alive 메시지 생성시 필요한 값을 관리한다.
    /// 
    /// </summary>
    class KeepAliveManager : IDisposable
    {
        private Dictionary<string, KeepAliveMsg> m_DeviceKeepAliveInfos = null;

        public KeepAliveManager() 
        {
            m_DeviceKeepAliveInfos = new Dictionary<string, KeepAliveMsg>();
        }

        public void Dispose()
        {
            if (m_DeviceKeepAliveInfos != null)
            {
                m_DeviceKeepAliveInfos.Clear();
                m_DeviceKeepAliveInfos = null;
            }
        }

        public int GetCount()
        {
            return m_DeviceKeepAliveInfos.Count;
        }


        /// <summary>
        /// 장비의 최신 수신KAM정보(발신처) 업데이트
        /// </summary>
        /// <param name="aDeviceIP"></param>
        /// <param name="aKAM"></param>
        public void Add(KeepAliveMsg aKAM)
        {
            Util.Assert(!string.IsNullOrEmpty(aKAM.DeviceIP), "[KeepAliveManager.Add] 장비IP가 없음");

            lock (m_DeviceKeepAliveInfos)
            {
                m_DeviceKeepAliveInfos[aKAM.DeviceIP] = aKAM; // Add or Update
            }
        }

        public string _ToString(string title = null)
        {
            if (m_DeviceKeepAliveInfos.Count == 0)
            {
                return "[KeepAliveManager] 수신내역이 없습니다.";
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(title == null ? "[KeepAliveManager] 장비별 최신KAM 정보▼" : title);
            int iCount = 0;
            KeepAliveMsg keepAliveMsg = null;
            foreach (string deviceIP in m_DeviceKeepAliveInfos.Keys)
            {
                keepAliveMsg = m_DeviceKeepAliveInfos[deviceIP];
                if (keepAliveMsg == null) continue;
                sb.AppendLine(keepAliveMsg.ToString(string.Format("[{0:2}]", ++iCount)));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 전송(reply) KeepAlive 메시지 데이터를 생성한다.
        /// (메시지포맷은 TlvDef.s_SendStructureFull/s_SendStructureMin 에 정의)
        /// </summary>
        /// <param name="aReplyMsg">송신KAM 정보</param>
        /// <returns>새로 생성한 KAM</returns>
        public KeepAliveMsg CreateKeepAliveReplyMessage(string aDeviceIP, E_SSHTunnelCreateOption aTunnelOption, int aTunnelPort = 0)
        {
            Util.Assert(!string.IsNullOrEmpty(aDeviceIP), string.Format("[KeepAliveManager.CreateKeepAliveReplyMessage] 장비IP가 없습니다. 장비IP{0}, 터널옵션={1}", aDeviceIP, aTunnelOption));
            if (string.IsNullOrEmpty(aDeviceIP)) return null;
            
            /// 수신KAM이 있는지 조회한다. (전송할 리모트주소 및 메시지데이터 조회)
            KeepAliveMsg lastRecvMsg = null;
            lock (m_DeviceKeepAliveInfos)
            {
                if (!m_DeviceKeepAliveInfos.TryGetValue(aDeviceIP, out lastRecvMsg)) return null;
            }
            if (lastRecvMsg == null) return null;

            /// 보낼KAM을 생성한다.
            KeepAliveMsg replyMsg = new KeepAliveMsg(aDeviceIP, aTunnelOption);
            lock (m_DeviceKeepAliveInfos)
            {
                // 공통항목
                replyMsg.SSHTunnelCreateOption = aTunnelOption;
                replyMsg.DeviceIP = lastRecvMsg.DeviceIP;
                replyMsg.USIM = lastRecvMsg.USIM;

                switch (aTunnelOption)
                {
                    case E_SSHTunnelCreateOption.Open:
                    case E_SSHTunnelCreateOption.Close:
                        replyMsg.CopyFrom(lastRecvMsg, true, true, true);
                        replyMsg.SSHServerDomain = GlobalClass.m_SystemInfo.SSHServerIP;//catm1.skbroadband.com
                        replyMsg.SSHPort = GlobalClass.m_SystemInfo.SSHServerPort;
                        replyMsg.SSHUserID = GlobalClass.m_SystemInfo.SSHUserID;
                        replyMsg.SSHPassword = GlobalClass.m_SystemInfo.SSHPassword;
                        replyMsg.SSHTunnelPort = aTunnelPort;
                        break;

                    case E_SSHTunnelCreateOption.Unknown:
                        replyMsg.CopyFrom(lastRecvMsg, false, true, true);
                        break;

                    default:
                        Util.Assert(false, string.Format("[KAM데이터관리자] 처리되지 않은 터널링 옵션입니다. SSHTunnelCreateOption={0}", aTunnelOption.ToString()));
                        break;
                }
            } // End of lock

            return replyMsg;
        }

        public void ClearAll()
        {
            lock (m_DeviceKeepAliveInfos)
            {
                m_DeviceKeepAliveInfos.Clear();
            }
        }

    } // End of class KeepAliveSenderManager
}
