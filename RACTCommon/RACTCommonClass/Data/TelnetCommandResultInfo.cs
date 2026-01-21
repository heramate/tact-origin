using System;
using System.Collections.Generic;
using System.Text;

namespace RACTCommonClass
{
    [Serializable]
    public class TelnetCommandResultInfo :ResultCommunicationData
    {

        /// <summary>
        /// 결과 타입 입니다.
        /// </summary>
        private E_TelnetReslutType m_ReslutType = E_TelnetReslutType.Data;

        /// <summary>
        /// Telnet Connected Session ID 입니다.
        /// </summary>
        private int m_SessionID;

        /// <summary>
        /// Telnet Connected Session ID 가져오거나 설정 합니다.
        /// </summary>
        public int SessionID
        {
            get { return m_SessionID; }
            set { m_SessionID = value; }
        }

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        /// <param name="aRequestInfo"></param>
        public TelnetCommandResultInfo(RequestCommunicationData aRequestInfo)
            : base(aRequestInfo)
        {
        }
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public TelnetCommandResultInfo() { }

        /// <summary>
        /// 결과 타입 가져오거나 설정 합니다.
        /// </summary>
        public E_TelnetReslutType ReslutType
        {
            get { return m_ReslutType; }
            set { m_ReslutType = value; }
        }

       

    }

    /// <summary>
    /// 결과 타입 종류 입니다.
    /// </summary>
    public enum E_TelnetReslutType
    {
        /// <summary>
        /// 연결 중입니다.
        /// </summary>
        TryConnect,
        /// <summary>
        /// 연결 결과 입니다.
        /// </summary>
        Connected,
        /// <summary>
        /// 종료 결과 입니다.
        /// </summary>
        DisConnected,
        /// <summary>
        /// 데이타 입니다.
        /// </summary>
        Data
    }
}
