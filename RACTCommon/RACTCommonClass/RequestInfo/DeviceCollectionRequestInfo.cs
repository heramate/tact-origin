using System;
using System.Collections.Generic;
using System.Text;

namespace RACTCommonClass
{
    [Serializable]
    public class DeviceCollectionRequestInfo
    {
        /// <summary>
        /// 사용자 ID입니다.
        /// </summary>
        private int m_UserID;

        /// <summary>
        /// 작업타입 입니다.
        /// </summary>
        private E_WorkType m_WorkType;

        /// <summary>
        /// 장비 정보 입니다,.
        /// </summary>
        private DeviceInfoCollection m_DeviceInfoList;

        // shinyn - 2012-12-13 - NE Group ID int -> string 수정 'B' PON(Biz) -> FOMs연동 값에 따른 수정
        /// <summary>
        /// 그룹 ID 입니다.
        /// </summary>
        private string m_GroupID = "0";

        private TerminalConnectInfo m_ConnectionInfo;
        /// <summary>
        /// 해당 장비에 설정할 Serial Port 속성입니다.
        /// </summary>
        private SerialConfig m_SerialConfig;
        /// <summary>
        /// SerialPort 속성을 가져오거나 설정합니다.
        /// </summary>
        public SerialConfig SerialConfig
        {
            get { return m_SerialConfig; }
            set { m_SerialConfig = value; }
        }
        public TerminalConnectInfo ConnectionInfo
        {
            get { return m_ConnectionInfo; }
            set { m_ConnectionInfo = value; }
        }

        // shinyn - 2012-12-13 - NE Group ID int -> string 수정 'B' PON(Biz) -> FOMs연동 값에 따른 수정
        /// <summary>
        /// 그룹 ID 속성을 가져오거나 설정합니다.
        /// </summary>
        /// 
        public string GroupID
        {
            get { return m_GroupID; }
            set { m_GroupID = value; }
        }
        /// <summary>
        /// 장비정보 속성을 가져오거나 설정합니다.
        /// </summary>
        public DeviceInfoCollection DeviceInfoList
        {
            get { return m_DeviceInfoList; }
            set { m_DeviceInfoList = value; }
        }	

        /// <summary>
        /// 작업타입 속성을 가져오거나 설정합니다.
        /// </summary>
        public E_WorkType WorkType
        {
            get { return m_WorkType; }
            set { m_WorkType = value; }
        }	

        /// <summary>
        /// User ID 속성을 가져오거나 설정합니다.
        /// </summary>
        public int UserID
        {
            get { return m_UserID; }
            set { m_UserID = value; }
        }	
    }
}