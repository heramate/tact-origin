using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using ACPS.CommonConfigCompareClass;

namespace RACTCommonClass
{
   
    /// <summary>
    ///  사용자 정보 클래스입니다.
    /// </summary>
    [Serializable]
    public class UserInfo : ICloneableEx<UserInfo>
    {
        /// <summary>
        /// 기본 생성자입니다. 
        /// </summary>
        public UserInfo()
        {
            this.ClientID = this.GetHashCode();
        }
        /// <summary>
        /// 복사 생성자입니다. 		
        /// </summary>
        /// <param name="aUserInfo"></param>
        public UserInfo(UserInfo aUserInfo)
        {
            this.ClientID = this.GetHashCode();
            CopyTo(aUserInfo, this, false);
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
        public UserInfo CompactClone()
        {
            UserInfo tUserInfo = new UserInfo();
            CopyTo(this, tUserInfo, true);
            return tUserInfo;
        }
        /// <summary>
        /// ICloneable Interface Overload 깊은복사(Deep Copy)를 수행한 객체 복사본을 리턴합니다.
        /// </summary>
        /// <returns></returns>
        public UserInfo DeepClone()
        {
            UserInfo tUserInfo = new UserInfo();
            CopyTo(this, tUserInfo, false);
            return tUserInfo;
        }
        /// <summary>
        /// 개체 복사를 처리합니다.
        /// </summary>
        /// <param name="aSource">원본 개체입니다.</param>
        /// <param name="aDest">대상 개체입니다.</param>
        /// <param name="aIsCompactClone">Compact 복사 여부입니다.</param>
        private void CopyTo(UserInfo aSource, UserInfo aDest, bool aIsCompactClone)
        {
            aDest.ClientID = aSource.ClientID;
            aDest.UserID = aSource.UserID;
            aDest.Account = aSource.Account;
            aDest.IPAddress = aSource.IPAddress;
            aDest.MacIPAddress = aSource.MacIPAddress;
            aDest.LastLoginTime = aSource.LastLoginTime;
            aDest.BranchCode = aSource.BranchCode;
            aDest.CenterCode = aSource.CenterCode;
            aDest.DevicePartCode = aSource.DevicePartCode;
            aDest.DevicePartCodes = aSource.DevicePartCodes;
            aDest.UserType = aSource.UserType;
            aDest.IsSupervisor = aSource.IsSupervisor;
            aDest.FACTORGType = aSource.FACTORGType;
            aDest.ORG1Code = aSource.ORG1Code;
            aDest.Centers = (ArrayList)aSource.Centers.Clone();
            aDest.ReceivedDeviceCount = aSource.ReceivedDeviceCount;
            aDest.LifeTime = aSource.LifeTime;
            // 2013-08-14 - shinyn - 사용자에 대한 사용자 그룹정보
            aDest.GroupInfos = aSource.GroupInfos;

            //2015-10-30 제한명령어 - 사용자 권한 적용.
            aDest.LimitedCmdUser = aSource.LimitedCmdUser;
        }

        //2015-10-30 추가. 제한명령어 사용자 권한 적용.
        /// <summary>
        /// 제한명령어 적용 여부 입니다. (true :제한, false : 제한 안함)
        /// </summary>
        private bool m_LimitedCmdUser = false;
        /// <summary>
        /// 제한명령어 적용 여부를 가져오거나 설정합니다. (true :제한, false : 제한 안함)
        /// </summary>
        public bool LimitedCmdUser
        {
            get { return m_LimitedCmdUser; }
            set { m_LimitedCmdUser = value; }
        }

        /// <summary>
        /// Client의 고유 식별자 입니다.
        /// </summary>
        private int m_ClientID = 0;
        /// <summary>
        /// Client의 고유 식별자 속성을 가져오거나 설정합니다.
        /// </summary>
        public int ClientID
        {
            get { return m_ClientID; }
            set { m_ClientID = value; }
        }

        /// <summary>
        /// 사용자의 고유 ID 입니다.
        /// </summary>
        private int m_UserID = 0;
        /// <summary>
        /// 사용자의 고유 ID 속성을 가져오거나 설정합니다.
        /// </summary>
        public int UserID
        {
            get { return m_UserID; }
            set { m_UserID = value; }
        }

        /// <summary>
        /// 사용자의 계정 입니다.
        /// </summary>
        private string m_Account = string.Empty;
        /// <summary>
        /// 사용자의 계정 속성을 가져오거나 설정합니다.
        /// </summary>
        public string Account
        {
            get { return m_Account; }
            set { m_Account = value; }
        }

        /// <summary>
        /// 사용자가 접속한 IP Address 입니다.
        /// </summary>
        private string m_IPAddress = string.Empty;
        /// <summary>
        /// 사용자가 접속한 IP Address 속성을 가져오거나 설정합니다.
        /// </summary>
        public string IPAddress
        {
            get { return m_IPAddress; }
            set { m_IPAddress = value; }
        }

        /// <summary>
        /// 사용자가 접속한 Mac IP Address 입니다.
        /// </summary>
        private string m_MacIPAddress = string.Empty;
        /// <summary>
        /// 사용자가 접속한 Mac IP Address 속성을 가져오거나 설정합니다.
        /// </summary>
        public string MacIPAddress
        {
            get { return m_MacIPAddress; }
            set { m_MacIPAddress = value; }
        }

        /// <summary>
        /// 사용자가 서버에 마지막으로 접속한 시간 입니다.
        /// </summary>
        private DateTime m_LastLoginTime = DateTime.Now;
        /// <summary>
        /// 사용자가 서버에 마지막으로 접속한 시간 속성을 가져오거나 설정합니다.
        /// </summary>
        public DateTime LastLoginTime
        {
            get { return m_LastLoginTime; }
            set { m_LastLoginTime = value; }
        }

        /// <summary>
        /// 사용자가 소속된 지사코드 입니다.
        /// </summary>
        private string m_BranchCode = string.Empty;
        /// <summary>
        /// 사용자가 소속된 지사코드 속성을 가져오거나 설정합니다.
        /// </summary>
        public string BranchCode
        {
            get { return m_BranchCode; }
            set { m_BranchCode = value; }
        }

        /// <summary>
        /// 사용자가 소속된 센터코드 입니다.
        /// </summary>
        private string m_CenterCode = string.Empty;
        /// <summary>
        /// 사용자가 소속된 센터코드 속성을 가져오거나 설정합니다.
        /// </summary>
        public string CenterCode
        {
            get { return m_CenterCode; }
            set { m_CenterCode = value; }
        }

        /// <summary>
        /// 데이터 큐(데이터 형식=  byte[])  입니다.
        /// </summary>
        private Queue m_DataQueue = new Queue();
        /// <summary>
        /// 데이터 큐 속성을 가져오거나 설정합니다.
        /// </summary>
        public Queue DataQueue
        {
            get { return m_DataQueue; }
            set { m_DataQueue = value; }
        }

        /// <summary>
        /// 클라이언트 라이프타임 입니다.
        /// </summary>
        private DateTime m_LifeTime;
        /// <summary>
        /// 클라이언트 라이프타임 속성을 가져오거나 설정합니다.
        /// </summary>
        public DateTime LifeTime
        {
            get { return m_LifeTime; }
            set { m_LifeTime = value; }
        }

        /// <summary>
        /// 장비 분류 코드 입니다. (2개 이상일 때)
        /// </summary>
        private string m_DevicePartCodes = string.Empty;
        /// <summary>
        /// 장비 분류 코드 속성을 가져오거나 설정합니다.
        /// </summary>
        public string DevicePartCodes
        {
            get { return m_DevicePartCodes; }
            set { m_DevicePartCodes = value; }
        }

        /// <summary>
        /// 장비 분류 코드 입니다. (1 개일 때)
        /// </summary>
        private int m_DevicePartCode = -1;
        /// <summary>
        /// 장비 분류 코드 속성을 가져오거나 설정합니다.
        /// </summary>
        public int DevicePartCode
        {
            get { return m_DevicePartCode; }
            set { m_DevicePartCode = value; }
        }


        /// <summary>
        /// 사용자 구분 입니다.
        /// </summary>
        private E_UserType m_UserType = E_UserType.Admin_All;
        /// <summary>
        /// 사용자 구분 속성을 가져오거나 설정합니다.
        /// </summary>
        public E_UserType UserType
        {
            get { return m_UserType; }
            set { m_UserType = value; }
        }

        /// <summary>
        /// 총괄관리자 권한 입니다.
        /// </summary>
        private bool m_IsSupervisor = false;
        /// <summary>
        /// 총괄관리자 권한 속성을 가져오거나 설정합니다.
        /// </summary>
        public bool IsSupervisor
        {
            get { return m_IsSupervisor; }
            set { m_IsSupervisor = value; }
        }


        /// <summary>
        /// 전체 권한 여부 입니다.
        /// </summary>
        private bool m_IsViewAllBranch;
        /// <summary>
        /// 전체 권한 여부 속성을 가져오거나 설정합니다.
        /// </summary>
        public bool IsViewAllBranch
        {
            get { return m_IsViewAllBranch; }
            set { m_IsViewAllBranch = value; }
        }	


        /// <summary>
        /// 사용자의 관리 센터 입니다.
        /// </summary>
        private ArrayList m_Centers = new ArrayList();
        /// <summary>
        /// 사용자의 관리 센터 속성을 가져오거나 설정합니다.
        /// </summary>
        public ArrayList Centers
        {
            get { return m_Centers; }
            set { m_Centers = value; }
        }

        /// <summary>
        /// FACT 조직 유형을 사용하는지의 여부 입니다.
        /// </summary>
        private bool m_FACTORGType = false;

        /// <summary>
        /// FACT 조직 유형을 사용하는지의 여부를 가져오거나 설정합니다.
        /// </summary>
        public bool FACTORGType
        {
            get { return m_FACTORGType; }
            set { m_FACTORGType = value; }
        }

        /// <summary>
        /// 실 단위의 조직 코드입니다. (실>팀>센터)
        /// </summary>
        private string m_ORG1Code = "";

        public string ORG1Code
        {
            get { return m_ORG1Code; }
            set { m_ORG1Code = value; }
        }

        /// <summary>
        /// 수신한 장비 대수 입니다. 입니다.
        /// </summary>
        private int m_ReceivedDeviceCount = 0;
        /// <summary>
        /// 수신한 장비 대수 입니다. 속성을 가져오거나 설정합니다.
        /// </summary>
        public int ReceivedDeviceCount
        {
            get { return m_ReceivedDeviceCount; }
            set { m_ReceivedDeviceCount = value; }
        }

        /// <summary>
        /// 2013-08-14- shinyn- 사용자 이름입니다.
        /// </summary>
        private string m_Name = string.Empty;

        /// <summary>
        /// 사용자 이름 속성을 가져오거나 설정합니다.
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }


        /// <summary>
        /// 2013-08-14 - shinyn - 사용자에 대한 그룹 정보를 설정합니다.
        /// </summary>
        private GroupInfoCollection m_GroupInfos = new GroupInfoCollection();

        public GroupInfoCollection GroupInfos
        {
            get { return m_GroupInfos; }
            set { m_GroupInfos = value; }
        }

        /// <summary>
        /// 센터 목록을 DB에 사용할 수 있는 목록으로 가져오기 합니다.
        /// </summary>
        public string GetCenterCode
        {

            //exec [SP_RACT_GET_DEVICEINFO] 'N00935'',''H10100''
            get
            {
                string tCenterList = "";

                for (int i = 0; i < m_Centers.Count; i++)
                {
                    tCenterList += string.Concat(m_Centers[i].ToString(), "''", "," ,"''");
                }

                if (tCenterList.Length > 0)
                {
                    tCenterList = tCenterList.Substring(0, tCenterList.Length - 5);
                }

                return tCenterList;
            }
        }

      
    }
}
