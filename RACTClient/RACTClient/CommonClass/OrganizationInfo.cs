using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using RACTCommonClass;

namespace RACTClient
{
    /// <summary>
    /// 조직정보 클래스 입니다.
    /// </summary>
    public class OrganizationInfo : MarshalByRefObject
    {
        /// <summary>
        /// 정보센터 코드 별 센터명입니다.
        /// </summary>
        private Hashtable m_CenterNameByCode = null;
        /// <summary>
        /// 운영 팀코드 별 팀 명 (구.지사 코드 별 지사명)입니다.
        /// </summary>
        private Hashtable m_BranchNameByCode = null;
        /// <summary>
        /// 네트워크 실코드 별 실명 입니다.
        /// </summary>
        private Hashtable m_ORG1NameByCode = null;

        /// <summary>
        /// 전국 조직 정보 입니다.
        /// </summary>
        private FACTGroupInfo m_AllGroupInfo = null;

        /// <summary>
        /// 전체 조직 정보 클래스의 기본생성자 입니다.
        /// </summary>
        public OrganizationInfo() { }

        /// <summary>
        /// 정보센터코드 별 정보센터명을 가져오거나 설정합니다.
        /// </summary>
        public Hashtable CenterNameByCode
        {
            get { return m_CenterNameByCode; }
            set { m_CenterNameByCode = value; }
        }

        /// <summary>
        /// 지사코드 별 지사명을 가져오거나 설정합니다.
        /// </summary>
        public Hashtable BranchNameByCode
        {
            get { return m_BranchNameByCode; }
            set { m_BranchNameByCode = value; }
        }

        /// <summary>
        /// 실코드 별 실명을 가져오거나 설정합니다.
        /// </summary>
        public Hashtable ORG1NameByCode
        {
            get { return m_ORG1NameByCode; }
            set { m_ORG1NameByCode = value; }
        }

        /// <summary>
        /// 전국 조직 정보를 가져오거나 설정합니다.
        /// </summary>
        public FACTGroupInfo AllGroupInfo
        {
            get { return m_AllGroupInfo; }
            set { m_AllGroupInfo = value; }
        }
    }
}
