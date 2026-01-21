using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using ACPS.CommonConfigCompareClass;

namespace RACTCommonClass
{
    #region DeviceInfo 클래스입니다.
    /// <summary>
    ///  장비 정보 클래스입니다.
    /// </summary>
    [Serializable]
    public class DeviceInfo : ICloneableEx<DeviceInfo>
    {
        #region [basic generate part :: Create, ICloneable]
        /// <summary>
        /// 기본 생성자입니다. 
        /// </summary>
        public DeviceInfo()
        {
            m_TerminalConnectInfo = new TerminalConnectInfo();
        }
        /// <summary>
        /// 복사 생성자입니다. 		
        /// </summary>
        /// <param name="aDeviceInfo"></param>
        public DeviceInfo(DeviceInfo aDeviceInfo)
        {
            CopyTo(aDeviceInfo, this, false);
        }
        ///<summary>
        /// ICloneable interface 구현 얖은복사(Shallow Copy)를 수행한 객체 복사본을 리턴합니다.
        ///</summary>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        /// <summary>
        /// 객체의 Compact한 복사본을 리턴합니다. 참조 Object는 null로 대체한 객체를 반환하는 것으로
        /// 리모팅 통신시 필요없는 깊은복사 대상 Object를 Null로 대체해 Compact시킵니다.
        /// </summary>
        /// <returns></returns>
        public DeviceInfo CompactClone()
        {
            DeviceInfo tDeviceInfo = new DeviceInfo();
            CopyTo(this, tDeviceInfo, true);
            return tDeviceInfo;
        }
        /// <summary>
        /// ICloneable Interface Overload 깊은복사(Deep Copy)를 수행한 객체 복사본을 리턴합니다.
        /// </summary>
        /// <returns></returns>
        public DeviceInfo DeepClone()
        {
            DeviceInfo tDeviceInfo = new DeviceInfo();
            CopyTo(this, tDeviceInfo, false);
            return tDeviceInfo;
        }
        /// <summary>
        /// 개체 복사를 처리합니다.
        /// </summary>
        /// <param name="aSource">원본 개체입니다.</param>
        /// <param name="aDest">대상 개체입니다.</param>
        /// <param name="aIsCompactClone">Compact 복사 여부입니다.</param>
        private void CopyTo(DeviceInfo aSource, DeviceInfo aDest, bool aIsCompactClone)
        {
            aDest.DeviceID = aSource.DeviceID;
            aDest.Name = aSource.Name;
            aDest.ModelID = aSource.ModelID;
            //2010-11-10 hanjiyeon
            aDest.ORG1Code = aSource.ORG1Code;
            aDest.BranchCode = aSource.BranchCode;
            aDest.CenterCode = aSource.CenterCode;
            aDest.IPAddress = aSource.IPAddress;
            aDest.TelnetID1 = aSource.TelnetID1;
            aDest.TelnetPwd1 = aSource.TelnetPwd1;
            aDest.TelnetID2 = aSource.TelnetID2;
            aDest.TelnetPwd2 = aSource.TelnetPwd2;
            aDest.ApplyDate = aSource.ApplyDate;
            aDest.GroupID = aSource.GroupID;
            aDest.ORG1Name = aSource.ORG1Name;
            aDest.ORG2Name = aSource.ORG2Name;
            aDest.CenterName = aSource.CenterName;
            aDest.TpoName = aSource.TpoName;
            // 2013-01-11 - shinyn - 모델명을 가져옵니다.
            aDest.ModelName = aSource.ModelName;
            // 2013-01-18 - shinyn - 장비 타입을 가져옵니다.
            aDest.DeviceType = aSource.DeviceType;

            // 2013-05-02- shinyn - Prompt값을 매핑합니다.
            aDest.WAIT = aSource.WAIT;
            aDest.USERID = aSource.USERID;
            aDest.PWD = aSource.PWD;
            aDest.USERID2 = aSource.USERID2;
            aDest.PWD2 = aSource.PWD2;

            //2013-08-08 - shinyn - MoreString,MoreMark값을 매핑합니다.
            aDest.MoreString = aSource.MoreString;
            aDest.MoreMark = aSource.MoreMark;

            //2013-08-14 - shinyn - 사용자 장비에 대한 사용자 이름과 계정입니다.
            aDest.UsrName = aSource.UsrName;
            aDest.Account = aSource.Account;
            aDest.UsrID = aSource.UsrID;

            //2015-09-24 - Gunny - 패널 이름을 저장 합니다.
            aDest.TerminalName = aSource.TerminalName;


            aDest.TerminalConnectInfo = new TerminalConnectInfo(aSource.TerminalConnectInfo);

            //데몬에는 필요없는 정보들
            if (!aIsCompactClone)
            {
                aDest.DevicePartCode = aSource.DevicePartCode;
                aDest.InputFlag = aSource.InputFlag;
                aDest.TpoName = aSource.TpoName;
                aDest.DeviceNumber = aSource.DeviceNumber;
                aDest.Version = aSource.Version;
                aDest.DeviceGroupName = aSource.DeviceGroupName;
                aDest.Description = aSource.Description;
                aDest.ORG1Code = aSource.ORG1Code;
                aDest.DeviceGroupName = aSource.DeviceGroupName;
            }

            aDest.SSHTunnelIP = aSource.SSHTunnelIP;
            aDest.SSHTunnelPort = aSource.SSHTunnelPort;

            //2019-04-25 Console Mode 연결시 IsRegistered 값 복사 안되어 추가
            aDest.IsRegistered = aSource.IsRegistered;

        }
        #endregion //[basic generate part :: Create, ICloneable]

        #region [property part]


        /// <summary>
        /// FACT 등록 장비인지 여부 입니다.
        /// </summary>
        private bool m_IsRegistered = true;

        /// <summary>
        /// FACT 등록 장비인지 여부 가져오거나 설정 합니다.
        /// </summary>
        public bool IsRegistered
        {
            get { return m_IsRegistered; }
            set { m_IsRegistered = value; }
        }


        /// <summary>
        /// 터미널 접속 정보 입니다.
        /// </summary>
        private TerminalConnectInfo m_TerminalConnectInfo;

        /// <summary>
        /// 터미널 접속 정보 가져오거나 설정 합니다.
        /// </summary>
        public TerminalConnectInfo TerminalConnectInfo
        {
            get { return m_TerminalConnectInfo; }
            set { m_TerminalConnectInfo = value; }
        }

        /// <summary>
        /// 장비 ID 입니다.
        /// </summary>
        private int m_DeviceID = 0;
        /// <summary>
        /// 장비 ID 속성을 가져오거나 설정합니다.
        /// </summary>
        public int DeviceID
        {
            get { return m_DeviceID; }
            set { m_DeviceID = value; }
        }

        /// <summary>
        /// 장비 이름 입니다.
        /// </summary>
        private string m_Name = string.Empty;
        /// <summary>
        /// 장비 이름 속성을 가져오거나 설정합니다.
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        /// <summary>
        /// 모델 ID 입니다.
        /// </summary>
        private int m_ModelID = 0;
        /// <summary>
        /// 모델 ID 속성을 가져오거나 설정합니다.
        /// </summary>
        public int ModelID
        {
            get { return m_ModelID; }
            set { m_ModelID = value; }
        }

        /// <summary>
        /// 2013-01-11 - shinyn - 모델 이름입니다. 추가
        /// </summary>
        private string m_ModelName = string.Empty;

        public string ModelName
        {
            get { return m_ModelName; }
            set { m_ModelName = value; }
        }

        /// <summary>
        /// 지점 코드 입니다.	
        /// </summary>
        private string m_BranchCode = string.Empty;
        /// <summary>
        /// 지점 코드 속성을 가져오거나 설정합니다.
        /// </summary>
        public string BranchCode
        {
            get { return m_BranchCode; }
            set { m_BranchCode = value; }
        }

        /// <summary>
        /// 센터 코드 입니다.
        /// </summary>
        private string m_CenterCode = string.Empty;
        /// <summary>
        /// 센터 코드 속성을 가져오거나 설정합니다.
        /// </summary>
        public string CenterCode
        {
            get { return m_CenterCode; }
            set { m_CenterCode = value; }
        }

        /// <summary>
        /// 사업장 주소 입니다.
        /// </summary>
        private string m_TpoName = string.Empty;
        /// <summary>
        /// 사업장 주소 속성을 가져오거나 설정합니다.
        /// </summary>
        public string TpoName
        {
            get { return m_TpoName; }
            set { m_TpoName = value; }
        }

        /// <summary>
        /// 장비의 IP 주소 입니다.
        /// </summary>
        private string m_IPAddress = string.Empty;
        /// <summary>
        /// 장비의 IP 주소 속성을 가져오거나 설정합니다.
        /// </summary>
        public string IPAddress
        {
            get { return m_IPAddress; }
            set { m_IPAddress = value; }
        }

        private string m_TerminalName = string.Empty;

        public string TerminalName
        {
            get { return m_TerminalName; }
            set { m_TerminalName = value; }
        }

        /// <summary>
        /// 장비 코드 입니다.
        /// </summary>
        private int m_DevicePartCode = 0;
        /// <summary>
        /// 장비 코드 속성을 가져오거나 설정합니다.
        /// </summary>
        public int DevicePartCode
        {
            get { return m_DevicePartCode; }
            set { m_DevicePartCode = value; }
        }

        /// <summary>
        /// 장비 연동 구분 입니다.
        /// </summary>
        private E_FlagType m_InputFlag = E_FlagType.FORMS;
        /// <summary>
        /// 장비 연동 구분 속성을 가져오거나 설정합니다.
        /// </summary>
        public E_FlagType InputFlag
        {
            get { return m_InputFlag; }
            set { m_InputFlag = value; }
        }

        /// <summary>
        /// 텔넷 1차 사용자 계정 입니다.
        /// </summary>
        private string m_TelnetID1 = string.Empty;
        /// <summary>
        /// 텔넷 1차 사용자 계정 속성을 가져오거나 설정합니다.
        /// </summary>
        public string TelnetID1
        {
            get { return m_TelnetID1; }
            set { m_TelnetID1 = value; }
        }

        /// <summary>
        /// 텔넷 1차 사용자 암호 입니다.
        /// </summary>
        private string m_TelnetPwd1 = string.Empty;
        /// <summary>
        /// 텔넷 1차 사용자 암호 속성을 가져오거나 설정합니다.
        /// </summary>
        public string TelnetPwd1
        {
            get { return m_TelnetPwd1; }
            set { m_TelnetPwd1 = value; }
        }

        /// <summary>
        /// 텔넷 2차 사용자 계정 입니다.
        /// </summary>
        private string m_TelnetID2 = string.Empty;
        /// <summary>
        /// 텔넷 2차 사용자 계정 속성을 가져오거나 설정합니다.
        /// </summary>
        public string TelnetID2
        {
            get { return m_TelnetID2; }
            set { m_TelnetID2 = value; }
        }

        /// <summary>
        /// 텔넷 2차 사용자 암호 입니다.
        /// </summary>
        private string m_TelnetPwd2 = string.Empty;
        /// <summary>
        /// 텔넷 2차 사용자 암호 속성을 가져오거나 설정합니다.
        /// </summary>
        public string TelnetPwd2
        {
            get { return m_TelnetPwd2; }
            set { m_TelnetPwd2 = value; }
        }

        /// <summary>
        /// OS Version 입니다.
        /// </summary>
        private string m_Version = string.Empty;
        /// <summary>
        /// OS Version 속성을 가져오거나 설정합니다.
        /// </summary>
        public string Version
        {
            get { return m_Version; }
            set { m_Version = value; }
        }

        /// <summary>
        /// 장비번호 입니다.
        /// </summary>
        private string m_DeviceNumber = string.Empty;
        /// <summary>
        /// 장비번호 속성을 가져오거나 설정합니다.
        /// </summary>
        public string DeviceNumber
        {
            get { return m_DeviceNumber; }
            set { m_DeviceNumber = value; }
        }

        // shinyn - 2012-12-13 - NE Group ID int -> string 수정 'B' PON(Biz) -> FOMs연동 값에 따른 수정
        /// <summary>
        /// 그룹 ID 입니다.
        /// </summary>
        private string m_GroupID = "0";

        // shinyn - 2012-12-13 - NE Group ID int -> string 수정 'B' PON(Biz) -> FOMs연동 값에 따른 수정
        /// <summary>
        /// 그룹 ID 속성을 가져오거나 설정합니다.
        /// </summary>
        public string GroupID
        {
            get { return m_GroupID; }
            set { m_GroupID = value; }
        }

        /// <summary>
        /// 적용 날짜 정보 입니다.
        /// </summary>
        private DateTime m_ApplyDate = DateTime.Now;
        /// <summary>
        /// 적용 날짜 정보 속성을 가져오거나 설정합니다.
        /// </summary>
        public DateTime ApplyDate
        {
            get { return m_ApplyDate; }
            set { m_ApplyDate = value; }
        }

        /// <summary>
        /// 장비 그룹명 입니다.
        /// </summary>
        private string m_DeviceGroupName = string.Empty;
        /// <summary>
        /// 장비 그룹명 속성을 가져오거나 설정합니다.
        /// </summary>
        public string DeviceGroupName
        {
            get { return m_DeviceGroupName; }
            set { m_DeviceGroupName = value; }
        }

        /// <summary>
        /// 장비의 설명 입니다.
        /// </summary>
        private string m_Description = string.Empty;
        /// <summary>
        /// 장비의 설명 속성을 가져오거나 설정합니다.
        /// </summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }

        /// <summary>
        /// 조직코드 입니다.
        /// </summary>
        private string m_ORG1Code = string.Empty;
        /// <summary>
        /// 조직코드 속성을 가져오거나 설정합니다.
        /// </summary>
        public string ORG1Code
        {
            get { return m_ORG1Code; }
            set { m_ORG1Code = value; }
        }


        /// <summary>
        /// 조직 이름 입니다.
        /// </summary>
        private string m_ORG1Name = string.Empty;

        /// <summary>
        /// 조직 이름 가져오거나 설정 합니다.
        /// </summary>
        public string ORG1Name
        {
            get { return m_ORG1Name; }
            set { m_ORG1Name = value; }
        }


        /// <summary>
        /// ORG2 Code 입니다.
        /// </summary>
        private string m_ORG2Code = string.Empty;

        /// <summary>
        /// ORG2 Code 가져오거나 설정 합니다.
        /// </summary>
        public string ORG2Code
        {
            get { return m_ORG2Code; }
            set { m_ORG2Code = value; }
        }


        /// <summary>
        /// ORG2 Name 입니다.
        /// </summary>
        private string m_ORG2Name = string.Empty;

        /// <summary>
        /// ORG2 Name 가져오거나 설정 합니다.
        /// </summary>
        public string ORG2Name
        {
            get { return m_ORG2Name; }
            set { m_ORG2Name = value; }
        }


        /// <summary>
        /// Center Name 입니다.
        /// </summary>
        private string m_CenterName = string.Empty;

        /// <summary>
        /// Center Name 가져오거나 설정 합니다.
        /// </summary>
        public string CenterName
        {
            get { return m_CenterName; }
            set { m_CenterName = value; }
        }



        /// <summary>
        /// 위치 정보를 가져오기 합니다.
        /// 2013-01-18 - shinyn - 수동장비에 대한 위치정보 부분을 수정
        /// </summary>
        public string Location
        {
            get
            {
                return m_DeviceType == E_DeviceType.NeGroup ?
                       string.Concat(m_ORG1Name.Trim(), ">", m_ORG2Name.Trim(), ">", m_CenterName.Trim(), ">", m_TpoName.Trim()) :
                       m_TpoName.Trim();
            }
        }


        /// <summary>
        /// 2013-01-18 - shinyn - NE등록장비인지, RACT_USR_NE_NE등록장비인지 구분 
        /// </summary>
        private E_DeviceType m_DeviceType = E_DeviceType.NeGroup;

        public E_DeviceType DeviceType
        {
            get { return m_DeviceType; }
            set { m_DeviceType = value; }
        }


        /// <summary>
        /// 2013-01-11 - shinyn - 장비정보에 해당하는 CFG복원명령리스트를 객체 추가함
        /// </summary>
        private CfgSaveInfoCollection m_CfgSaveInfos = new CfgSaveInfoCollection();

        public CfgSaveInfoCollection CfgSaveInfos
        {
            get { return m_CfgSaveInfos; }
            set { m_CfgSaveInfos = value; }
        }

        /// <summary>
        /// 2013-05-02 - shinyn - 수동장비인 경우 기본접속 명령을 지정한것에서 가져온다.
        /// </summary>
        private string m_WAIT = string.Empty;

        public string WAIT
        {
            get { return m_WAIT; }
            set { m_WAIT = value; }
        }

        /// <summary>
        /// 2013-05-02 - shinyn - 수동장비인 경우 기본접속 명령을 지정한것에서 가져온다.
        /// </summary>
        private string m_USERID = string.Empty;

        public string USERID
        {
            get { return m_USERID; }
            set { m_USERID = value; }
        }

        /// <summary>
        /// 2013-05-02 - shinyn - 수동장비인 경우 기본접속 명령을 지정한것에서 가져온다.
        /// </summary>
        private string m_PWD = string.Empty;

        public string PWD
        {
            get { return m_PWD; }
            set { m_PWD = value; }
        }

        /// <summary>
        /// 2013-05-02 - shinyn - 수동장비인 경우 기본접속 명령을 지정한것에서 가져온다.
        /// </summary>
        private string m_USERID2 = string.Empty;

        public string USERID2
        {
            get { return m_USERID2; }
            set { m_USERID2 = value; }
        }

        /// <summary>
        /// 2013-05-02 - shinyn - 수동장비인 경우 기본접속 명령을 지정한것에서 가져온다. 
        /// </summary>
        private string m_PWD2 = string.Empty;

        public string PWD2
        {
            get { return m_PWD2; }
            set { m_PWD2 = value; }
        }

        // 수동장비의 MoreString --More-- -- more --
        private string m_MoreString = string.Empty;

        public string MoreString
        {
            get { return m_MoreString; }
            set { m_MoreString = value; }
        }

        // 수동장비의 MoreString온경우 실행되는 예약어 ${ENTER},${SPACE}
        private string m_MoreMark = string.Empty;

        public string MoreMark
        {
            get { return m_MoreMark; }
            set { m_MoreMark = value; }
        }

        /// <summary>
        /// 2013-08-14 - shinyn - 사용자 그룹의 장비목록인 경우 사용자 이름입니다.
        /// </summary>
        private string m_UsrName = string.Empty;

        /// <summary>
        /// 2013-08-14 - shinyn - 사용자 그룹의 장비목록인 경우 사용자 이름을 설정하거나 가져옵니다.
        /// </summary>
        public string UsrName
        {
            get { return m_UsrName; }
            set { m_UsrName = value; }
        }

        /// <summary>
        /// 2013-08-14 - shinyn - 사용자 그룹의 장비목록인 경우 사용자 계정입니다.
        /// </summary>
        private string m_Account = string.Empty;

        /// <summary>
        /// 2013-08-14 - shinyn - 사용자 그룹의 장비목록인 경우 사용자 계정을 설정하거나 가져옵니다.
        /// </summary>
        public string Account
        {
            get { return m_Account; }
            set { m_Account = value; }
        }

        /// <summary>
        /// 2013-08-14 - shinyn - 사용자그룹의 장비목록인 경우 사용자 아이디입니다.
        /// </summary>
        private int m_UsrID = 0;

        /// <summary>
        /// 2013-08-14 - shinyn - 사용자그룹의 장비목록인 경우 사용자 아이디를 설정하거나 가져옵니다.
        /// </summary>
        public int UsrID
        {
            get { return m_UsrID; }
            set { m_UsrID = value; }
        }

        /// <summary>
        /// 2018.10 [c-RPCS무선접속] 장비에 접속할 SSH터널의 접속 IP
        /// </summary>
        private string m_SSHTunnelIP = string.Empty;
        /// <summary>
        /// 2018.10 [c-RPCS무선접속] 장비에 접속할 SSH터널의 접속 포트
        /// </summary>
        private int m_SSHTunnelPort = 0;

        public string SSHTunnelIP
        {
            get { return m_SSHTunnelIP; }
            set { m_SSHTunnelIP = value; }
        }
        public int SSHTunnelPort
        {
            get { return m_SSHTunnelPort; }
            set { m_SSHTunnelPort = value; }
        }


        #endregion //[property part]


    }
    #endregion //DeviceInfo 클래스입니다.

    #region DeviceInfoCollection 클래스입니다.
    /// <summary>
    /// 장비 정보 목록 클래스 입니다.             
    /// </summary>
    [Serializable]
    public class DeviceInfoCollection : GenericListMarshalByRef<DeviceInfo>, ICloneableEx<DeviceInfoCollection>
    {
        #region [basic generate part :: Create, ICloneable]
        /// <summary>
        ///  기본 생성자 입니다.
        /// </summary>
        public DeviceInfoCollection()
        {
        }
        /// <summary>
        /// 복사 생성자 입니다.
        /// </summary>
        /// <param name="aDeviceInfoCollection"></param>
        public DeviceInfoCollection(DeviceInfoCollection aDeviceInfoCollection)
        {
            CopyTo(aDeviceInfoCollection, this, false);
        }
        ///<summary>
        /// ICloneable interface 구현 얖은복사(Shallow Copy)를 수행한 객체 복사본을 리턴합니다.
        ///</summary>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        /// <summary>
        /// 객체의 Compact한 복사본을 리턴합니다. 참조 Object는 null로 대체한 객체를 반환하는 것으로
        /// 리모팅 통신시 필요없는 깊은복사 대상 Object를 Null로 대체해 Compact시킵니다.
        /// </summary>
        /// <returns></returns>
        public DeviceInfoCollection CompactClone()
        {
            DeviceInfoCollection tCollection = new DeviceInfoCollection();
            CopyTo(this, tCollection, true);
            return tCollection;
        }
        /// <summary>
        /// ICloneable Interface Overload 깊은복사(Deep Copy)를 수행한 객체 복사본을 리턴합니다.
        /// </summary>
        /// <returns></returns>
        public DeviceInfoCollection DeepClone()
        {
            DeviceInfoCollection tCollection = new DeviceInfoCollection();
            CopyTo(this, tCollection, false);
            return tCollection;
        }
        /// <summary>
        /// 맴버 대입을 처리합니다.
        /// </summary>
        private void CopyTo(DeviceInfoCollection aSource, DeviceInfoCollection aDest, bool aIsCompactClone)
        {
            if (aDest != null && aDest.Count > 0)
                aDest.Clear();

            foreach (DeviceInfo tDeviceInfo in aSource)
            {
                if (aIsCompactClone)
                    aDest.Add((DeviceInfo)tDeviceInfo.CompactClone());
                else
                    aDest.Add((DeviceInfo)tDeviceInfo.DeepClone());
            }
        }
        #endregion //[basic generate part :: Create, ICloneable]

        #region [property part]
        /// <summary>
        /// 해당 ID의 요소를 가져오거나 설정합니다.
        /// </summary>
        /// <param name="aID">가져오거나 설정할 요소 ID입니다.</param>
        /// <returns></returns>
        public override DeviceInfo this[int aID]
        {
            get
            {
                DeviceInfo tDeviceInfo = null;
                lock (base.InnerList.SyncRoot)
                {
                    foreach (DeviceInfo tmpDeviceInfo in base.InnerList)
                    {
                        if (tmpDeviceInfo.DeviceID == aID)
                        {
                            tDeviceInfo = tmpDeviceInfo;
                            break;
                        }
                    }
                }
                return tDeviceInfo;
            }
            set
            {
                DeviceInfo tDeviceInfo = null;
                lock (base.InnerList.SyncRoot)
                {
                    for (int idx = 0; idx < base.InnerList.Count; idx++)
                    {
                        tDeviceInfo = base.InnerList[idx] as DeviceInfo;
                        if (tDeviceInfo.DeviceID == aID)
                        {
                            base.InnerList[idx] = value;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 해당 IP의 장비를 가져오거나 설정합니다.
        /// </summary>
        /// <param name="aIPAddress"></param>
        /// <returns></returns>
        public DeviceInfo this[string aIPAddress]
        {
            get
            {
                DeviceInfo tDeviceInfo = null;
                lock (base.InnerList.SyncRoot)
                {
                    foreach (DeviceInfo tmpDeviceInfo in base.InnerList)
                    {
                        if (tmpDeviceInfo.IPAddress == aIPAddress)
                        {
                            tDeviceInfo = tmpDeviceInfo;
                            break;
                        }
                    }
                }
                return tDeviceInfo;
            }
            set
            {
                DeviceInfo tDeviceInfo = null;
                lock (base.InnerList.SyncRoot)
                {
                    for (int idx = 0; idx < base.InnerList.Count; idx++)
                    {
                        tDeviceInfo = base.InnerList[idx] as DeviceInfo;
                        if (tDeviceInfo.IPAddress == aIPAddress)
                        {
                            base.InnerList[idx] = value;
                            break;
                        }
                    }
                }
            }
        }
        #endregion //[property part]

        #region [public member part]
        /// <summary>
        /// 해당 ID의 요소를 제거합니다.
        /// </summary>
        /// <param name="aID"></param>
        public void Remove(int aID)
        {
            lock (base.InnerList.SyncRoot)
            {
                foreach (DeviceInfo tDeviceInfo in base.InnerList)
                {
                    if (tDeviceInfo.DeviceID == aID)
                    {
                        base.Remove(tDeviceInfo);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 2013-01-18 - shinyn - 장비객체를 보내서 같으면 삭제한다.
        /// 동일아이디 존재할 수 있다.
        /// </summary>
        /// <param name="aDeviceInfo"></param>
        public void Remove(DeviceInfo aDeviceInfo)
        {
            lock (base.InnerList.SyncRoot)
            {
                foreach (DeviceInfo tDeviceInfo in base.InnerList)
                {
                    if (tDeviceInfo == aDeviceInfo)
                    {
                        base.Remove(tDeviceInfo);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 해당 장비가 포함되어 있는지 여부를 반환합니다.
        /// </summary>
        /// <param name="aID"></param>
        /// <returns></returns>
        public bool Contains(int aID)
        {
            return this[aID] != null;
        }

        /// <summary>
        /// 해당 IP의 장비가 포함되어 있는지 여부를 반환합니다.
        /// </summary>
        /// <param name="aIPAddress"></param>
        /// <returns></returns>
        public bool Contains(string aIPAddress)
        {
            return this[aIPAddress] != null;
        }

        #endregion //[public member part]

        #region [public member part]

        /// <summary>
        /// 현재 장비 목록의 장비 ID 목록을 반환합니다.
        /// </summary>
        /// <returns></returns>
        public DeviceIDList GetIDs()
        {
            DeviceIDList tDeviceIDList = new DeviceIDList();
            foreach (DeviceInfo tDeviceInfo in base.InnerList)
            {
                tDeviceIDList.Add(tDeviceInfo.DeviceID);
            }
            return tDeviceIDList;
        }


        /// <summary>
        /// 장비분류에 따른 장비 대수를 얻어옵니다.
        /// </summary>
        /// <param name="aDevicePartList"></param>
        /// <returns></returns>
        public int GetCountByGroup(ArrayList aDevicePartList)
        {
            int tCount = 0;
            bool bDevicePartDevice = false;
            if (aDevicePartList.Count == 1 && (int)aDevicePartList[0] == 0) return base.InnerList.Count;

            foreach (DeviceInfo tDI in base.InnerList)
            {
                bDevicePartDevice = false;
                foreach (int tValue in aDevicePartList)
                {
                    if (tDI.DevicePartCode == tValue)
                    {
                        bDevicePartDevice = true;
                        break;
                    }
                }

                if (bDevicePartDevice)
                {
                    tCount++;
                }
            }
            return tCount;
        }

        //2010-11-10 hanjiyeon 수정
        /// <summary>
        /// 선택된 장비 분류에 속하는 장비정보들을 반환합니다.
        /// </summary>
        /// <param name="aDeviceInfoCollection"></param>
        /// <param name="aDevicePartList"></param>
        private DeviceInfoCollection GetDeviceInfosByDevicePartList(DeviceInfoCollection aDeviceInfoCollection, ArrayList aDevicePartList)
        {
            DeviceInfoCollection tDeviceInfoCollection = null;// aDeviceInfoCollection.DeepClone();

            bool bDevicePartDevice = false;

            //if (aDevicePartList.Count == 1 && aDevicePartList[0].ToString() == "")
            if (aDevicePartList.Count == 1 && (int)aDevicePartList[0] < 1)
            {
                tDeviceInfoCollection = aDeviceInfoCollection.DeepClone();
                return tDeviceInfoCollection;
            }

            tDeviceInfoCollection = new DeviceInfoCollection();

            //2010-11-16 hanjiyeon 부등호 수정.
            foreach (DeviceInfo tDI in aDeviceInfoCollection)
            {
                bDevicePartDevice = false;
                foreach (int tValue in aDevicePartList)
                {
                    if (tDI.DevicePartCode == tValue)
                    {
                        bDevicePartDevice = true;
                        break;
                    }
                }

                if (bDevicePartDevice)
                {
                    tDeviceInfoCollection.Add(tDI);
                    //tDeviceInfoCollection.Remove(tDI.DeviceID);
                }
            }

            return tDeviceInfoCollection;
        }

        /// <summary>
        /// 해당 장비분류코드와 일치하는 장비 목록을 반환하는 함수 입니다.
        /// </summary>
        /// <param name="aDevicePartCode">장비 분류 코드</param>
        /// <returns>해당 장비분류코드와 일치하는 장비 목록</returns>
        public DeviceInfoCollection GetMatchListByDevicePart(int aDevicePartCode)
        {
            DeviceInfoCollection tDeviceList = new DeviceInfoCollection();

            foreach (DeviceInfo tDeviceInfo in this)
            {
                if (aDevicePartCode == tDeviceInfo.DevicePartCode)
                {
                    tDeviceList.Add(tDeviceInfo);
                }
            }
            return tDeviceList;
        }

        /// <summary>
        /// 해당 모델ID와 일치하는 장비 목록을 반환하는 함수 입니다.
        /// </summary>
        /// <param name="aDevicePartCode">장비 모델 ID</param>
        /// <returns>해당 모델ID와 일치하는 장비 목록</returns>
        public DeviceInfoCollection GetMatchListByDeviceModel(int aModelID)
        {
            DeviceInfoCollection tDeviceList = new DeviceInfoCollection();

            foreach (DeviceInfo tDeviceInfo in this)
            {
                if (aModelID == tDeviceInfo.ModelID)
                {
                    tDeviceList.Add(tDeviceInfo);
                }
            }
            return tDeviceList;
        }

        /// <summary>
        /// 해당 IP와 일치하는 장비 목록을 반환하는 함수 입니다.
        /// </summary>
        /// <param name="aDevicePartCode">장비 IP</param>
        /// <returns>해당 IP와 일치하는 장비 목록</returns>
        public DeviceInfoCollection GetMatchListByIPAddress(string aIPAddress)
        {
            DeviceInfoCollection tDeviceList = new DeviceInfoCollection();

            foreach (DeviceInfo tDeviceInfo in this)
            {
                if (aIPAddress == tDeviceInfo.IPAddress)
                {
                    tDeviceList.Add(tDeviceInfo);
                }
            }
            return tDeviceList;
        }

        /// <summary>
        /// 해당 장비 이름과 일치하는 장비 목록을 반환하는 함수 입니다.
        /// </summary>
        /// <param name="aDevicePartCode">장비 이름</param>
        /// <returns>해당 장비 이름과 일치하는 장비 목록</returns>
        public DeviceInfoCollection GetMatchListByDeviceName(string aDeviceName)
        {
            DeviceInfoCollection tDeviceList = new DeviceInfoCollection();

            foreach (DeviceInfo tDeviceInfo in this)
            {
                if (tDeviceInfo.Name.ToLowerInvariant().Contains(aDeviceName.ToLowerInvariant()))
                {
                    tDeviceList.Add(tDeviceInfo);
                }
            }
            return tDeviceList;
        }
        #endregion //[public member part]D
    }
    #endregion //DeviceInfoCollection 클래스입니다.

    #region 장비 ID 목록 클래스 입니다.

    /// <summary>
    /// 장비 ID목록 클래스 입니다.
    /// </summary>
    [Serializable]
    public class DeviceIDList : MarshalByRefObject, ICloneable, ICollection
    {
        /// <summary>
        /// 장비 ID목록이저장될 배열 입니다.
        /// </summary>
        private ArrayList m_IDs = new ArrayList();

        /// <summary>
        /// 장비 ID목록 클래스의 기본 생성자 입니다.
        /// </summary>
        public DeviceIDList() { }

        /// <summary>
        /// 장비 ID목록 클래스의 기본 입니다.
        /// </summary>
        /// <param name="vIDs">장비 ID목록 배열입니다.</param>
        public DeviceIDList(DeviceIDList vIDs)
        {
            if (vIDs != null)
            {
                for (int i = 0; i < vIDs.Count; i++)
                {
                    m_IDs.Add(vIDs[i]);
                }
            }
        }

        /// <summary>
        /// 장비 ID를 추가합니다.
        /// </summary>
        /// <param name="vID">추가할 장비의 ID입니다.</param>
        /// <returns>장비의 ID가 추가된 위치 입니다.</returns>
        public int Add(int vID)
        {
            return m_IDs.Add(vID);
        }

        /// <summary>
        /// 지정한 장비 ID목록을 추가합니다.
        /// </summary>
        /// <param name="vIDs">장비 ID목록을 </param>
        public void AddRange(ICollection vIDs)
        {
            m_IDs.AddRange(vIDs);
        }

        /// <summary>
        /// 지정한 위치에 장비 ID를 추가합니다.
        /// </summary>
        /// <param name="vIndex">장비 ID를 추가할 위치 입니다.</param>
        /// <param name="vID">장비의 ID입니다.</param>
        public void Insert(int vIndex, int vID)
        {
            m_IDs.Insert(vIndex, vID);
        }

        /// <summary>
        /// 지정한 장비 ID를 삭제합니다.
        /// </summary>
        /// <param name="vID">삭제할 장비 ID입니다.</param>
        public void Remove(int vID)
        {
            m_IDs.Remove(vID);
        }

        /// <summary>
        /// 지정한 위치의 장비 ID를 삭제합니다.
        /// </summary>
        /// <param name="vIndex">삭제할 장비ID의 위치 입니다.</param>
        public void RemoveAt(int vIndex)
        {
            m_IDs.RemoveAt(vIndex);
        }

        /// <summary>
        /// 지정한 장비ID가 포함되어있는지를 확인합니다.
        /// </summary>
        /// <param name="vID">포함 여부를 확인할 장비 ID입니다.</param>
        /// <returns>장비의 ID가 포함되었는지의 여부 입니다.</returns>
        public bool Contains(int vID)
        {
            return m_IDs.Contains(vID);
        }

        /// <summary>
        /// 지정한 장비의 ID위치를 확인 합니다.
        /// </summary>
        /// <param name="vID">위치를 확인할 장비의 ID입니다.</param>
        /// <returns>장비 ID의 위치 입니다.</returns>
        public int IndexOf(int vID)
        {
            return m_IDs.IndexOf(vID);
        }

        /// <summary>
        /// 지정한 장비의 ID위치를 확인 합니다.
        /// </summary>
        /// <param name="vID">위치를 확인할 장비의 ID입니다.</param>
        /// <param name="vStartIndex">장비 ID 위치를 찾기 시작할 위치 입니다.</param>
        /// <returns>장비 ID의 위치 입니다.</returns>
        public int IndexOf(int vID, int vStartIndex)
        {
            return m_IDs.IndexOf(vID, vStartIndex);
        }

        /// <summary>
        /// 모든 장비 ID를 삭제 합니다.
        /// </summary>
        public void Clear()
        {
            m_IDs.Clear();
        }

        /// <summary>
        /// 전체 장비 ID개수를 가져옵니다.
        /// </summary>
        public int Count
        {
            get { return m_IDs.Count; }
        }

        /// <summary>
        /// 장비 ID의 배열 요소를 가져옵니다.
        /// </summary>
        /// <returns>장비 ID의 배열 요소입니다.</returns>
        public ICollection ToArray()
        {
            return (ICollection)m_IDs;
        }

        /// <summary>
        /// 장비 ID를 가져오거나 설정합니다.
        /// </summary>
        public int this[int vIndex]
        {
            get { return (int)m_IDs[vIndex]; }
            set { m_IDs[vIndex] = value; }
        }

        /// <summary>
        /// 장비 ID목록의 복사본을 생성합니다.
        /// </summary>
        /// <returns>장비 ID목록의 복사본입니다.</returns>
        public DeviceIDList Clone()
        {
            return new DeviceIDList(this);
        }

        public IEnumerator GetEnumerator()
        {
            return m_IDs.GetEnumerator();
        }

        #region ICloneable 멤버
        object System.ICloneable.Clone()
        {
            return new DeviceIDList(this);
        }
        #endregion

        #region ICollection 멤버를 정의합니다.

        /// <summary>
        /// 대상 배열의 지정한 인덱스에서 시작하는 요소 범위를 호환되는 Array에 복사합니다.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        void ICollection.CopyTo(Array array, int arrayIndex)
        {
            m_IDs.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// 포함된 요소의 수를 가져옵니다.
        /// </summary>
        int ICollection.Count
        {
            get { return m_IDs.Count; }
        }

        /// <summary>
        /// 액세스를 동기화하는 데 사용할 수 있는 개체를 가져옵니다.
        /// </summary>
        object ICollection.SyncRoot
        {
            get { return m_IDs.SyncRoot; }
        }

        /// <summary>
        /// 액세스가 동기화되어 스레드로부터 안전하게 보호되는지 여부를 나타내는 값을 가져옵니다.
        /// </summary>
        bool ICollection.IsSynchronized
        {
            get { return m_IDs.IsSynchronized; }
        }

        #endregion //ICollection 멤버를 정의합니다.
    }
    #endregion //장비 ID 목록 클래스 입니다.

    #region DeviceInfoCollection 클래스입니다.
    /// <summary>
    /// 장비 목록 클래스 입니다.             
    /// </summary>
    [Serializable]
    public class DeviceCollection : GenericListMarshalByRef<DeviceInfo>, ICloneableEx<DeviceCollection>
    {
        #region [basic generate part :: Create, ICloneable]
        /// <summary>
        ///  기본 생성자 입니다.
        /// </summary>
        public DeviceCollection()
        {
        }
        /// <summary>
        /// 복사 생성자 입니다.
        /// </summary>
        /// <param name="aDeviceCollection"></param>
        public DeviceCollection(DeviceCollection aDeviceCollection)
        {
            CopyTo(aDeviceCollection, this, false);
        }
        ///<summary>
        /// ICloneable interface 구현 얖은복사(Shallow Copy)를 수행한 객체 복사본을 리턴합니다.
        ///</summary>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        /// <summary>
        /// 객체의 Compact한 복사본을 리턴합니다. 참조 Object는 null로 대체한 객체를 반환하는 것으로
        /// 리모팅 통신시 필요없는 깊은복사 대상 Object를 Null로 대체해 Compact시킵니다.
        /// </summary>
        /// <returns></returns>
        public DeviceCollection CompactClone()
        {
            DeviceCollection tCollection = new DeviceCollection();
            CopyTo(this, tCollection, true);
            return tCollection;
        }
        /// <summary>
        /// ICloneable Interface Overload 깊은복사(Deep Copy)를 수행한 객체 복사본을 리턴합니다.
        /// </summary>
        /// <returns></returns>
        public DeviceCollection DeepClone()
        {
            DeviceCollection tCollection = new DeviceCollection();
            CopyTo(this, tCollection, false);
            return tCollection;
        }
        /// <summary>
        /// 맴버 대입을 처리합니다.
        /// </summary>
        private void CopyTo(DeviceCollection aSource, DeviceCollection aDest, bool aIsCompactClone)
        {
            if (aDest != null && aDest.Count > 0)
                aDest.Clear();

            foreach (DeviceInfo tDeviceInfo in aSource)
            {
                if (aIsCompactClone)
                    aDest.Add((DeviceInfo)tDeviceInfo.CompactClone());
                else
                    aDest.Add((DeviceInfo)tDeviceInfo.DeepClone());
            }
        }



        #endregion //[basic generate part :: Create, ICloneable]
    }
    #endregion //DeviceInfoCollection 클래스입니다.
}
