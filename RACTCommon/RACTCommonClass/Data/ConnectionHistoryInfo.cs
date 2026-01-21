using System;
using System.Collections.Generic;
using System.Text;

namespace RACTCommonClass
{
    /// <summary>
    /// 장비 접속 기록 정보 입니다.
    /// </summary>
    [Serializable]
    public class DeviceConnectionHistoryInfo
    {
        /// <summary>
        /// DateTime 입니다.
        /// </summary>
        private DateTime m_ConnectionTime;

        /// <summary>
        /// User ID 입니다.
        /// </summary>
        private int m_UserID;

        /// <summary>
        /// ConnectionType 입니다.
        /// </summary>
        private E_DeviceConnectType m_ConnectionType;

        /// <summary>
        /// Description 입니다.
        /// </summary>
        private string m_Description;

        /// <summary>
        /// Disconnection Time 입니다.
        /// </summary>
        private DateTime m_EndTime;

        /// <summary>
        /// ID 입니다.
        /// </summary>
        private int m_ID;
        /// <summary>
        /// Device Name 입니다.
        /// </summary>
        private string m_DeviceName;

        /// <summary>
        /// IP Address 입니다.
        /// </summary>
        private string m_IPAddress;
        
        /// <summary>
        /// Device ID 입니다.
        /// </summary>
        private int m_DeviceID;

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public DeviceConnectionHistoryInfo() { }

        
        /// <summary>
        /// Device ID 속성을 가져오거나 설정합니다.
        /// </summary>
        public int DeviceID
        {
            get { return m_DeviceID; }
            set { m_DeviceID = value; }
        }	

        

        /// <summary>
        /// Device Name 속성을 가져오거나 설정합니다.
        /// </summary>
        public string DeviceName
        {
            get { return m_DeviceName; }
            set { m_DeviceName = value; }
        }	


        /// <summary>
        /// IP Address 가져오거나 설정 합니다.
        /// </summary>
        public string IPAddress
        {
            get { return m_IPAddress; }
            set { m_IPAddress = value; }
        }

        /// <summary>
        /// ID 가져오거나 설정 합니다.
        /// </summary>
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }
        /// <summary>
        /// Disconnection Time 가져오거나 설정 합니다.
        /// </summary>
        public DateTime EndTime
        {
            get { return m_EndTime; }
            set { m_EndTime = value; }
        }

        /// <summary>
        /// Description 가져오거나 설정 합니다.
        /// </summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }

        /// <summary>
        /// ConnectionType 가져오거나 설정 합니다.
        /// </summary>
        public E_DeviceConnectType ConnectionType
        {
            get { return m_ConnectionType; }
            set { m_ConnectionType = value; }
        }

        /// <summary>
        /// User ID 가져오거나 설정 합니다.
        /// </summary>
        public int UserID
        {
            get { return m_UserID; }
            set { m_UserID = value; }
        }

        /// <summary>
        /// DateTime 가져오거나 설정 합니다.
        /// </summary>
        public DateTime ConnectionTime
        {
            get { return m_ConnectionTime; }
            set { m_ConnectionTime = value; }
        }
    }
    [Serializable]
    public class ConnectionHistoryInfoCollection : List<DeviceConnectionHistoryInfo>
    {
    }
}
