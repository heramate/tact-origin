using System;
using System.Collections.Generic;
using System.Text;
using ACPS.CommonConfigCompareClass;

namespace RACTCommonClass
{
    #region GroupInfo 클래스입니다.
    /// <summary>
    ///  그룹 정보 클래스입니다.
    /// </summary>
    [Serializable]
    public class FACTGroupInfo : ICloneableEx<FACTGroupInfo>
    {
        #region [basic generate part :: Create, ICloneable]
        /// <summary>
        /// 기본 생성자입니다. 
        /// </summary>
        public FACTGroupInfo()
        {
        }
        /// <summary>
        /// 복사 생성자입니다. 		
        /// </summary>
        /// <param name="aGroupInfo"></param>
        public FACTGroupInfo(FACTGroupInfo aGroupInfo)
        {
            CopyTo(aGroupInfo, this, false);
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
        public FACTGroupInfo CompactClone()
        {
            FACTGroupInfo tGroupInfo = new FACTGroupInfo();
            CopyTo(this, tGroupInfo, true);
            return tGroupInfo;
        }
        /// <summary>
        /// ICloneable Interface Overload 깊은복사(Deep Copy)를 수행한 객체 복사본을 리턴합니다.
        /// </summary>
        /// <returns></returns>
        public FACTGroupInfo DeepClone()
        {
            FACTGroupInfo tGroupInfo = new FACTGroupInfo();
            CopyTo(this, tGroupInfo, false);
            return tGroupInfo;
        }
        /// <summary>
        /// 개체 복사를 처리합니다.
        /// </summary>
        /// <param name="aSource">원본 개체입니다.</param>
        /// <param name="aDest">대상 개체입니다.</param>
        /// <param name="aIsCompactClone">Compact 복사 여부입니다.</param>
        private void CopyTo(FACTGroupInfo aSource, FACTGroupInfo aDest, bool aIsCompactClone)
        {
            aDest.BranchCode = aSource.BranchCode;
            aDest.BranchName = aSource.BranchName;
            aDest.CenterCode = aSource.CenterCode;
            aDest.CenterName = aSource.CenterName;
            aDest.ORG1Code = aSource.ORG1Code;
            aDest.ORG1Name = aSource.ORG1Name; //2010-11-10 hanjiyeon 수정.
            aDest.DeviceCount = aSource.DeviceCount;

            if (aIsCompactClone)
            {
                if (aSource.ParentGroup != null) aDest.ParentGroup = aSource.ParentGroup.CompactClone();
                if (aSource.SubGroups != null) aDest.SubGroups = aSource.SubGroups.CompactClone();
            }
            else
            {
                if (aSource.ParentGroup != null) aDest.ParentGroup = aSource.ParentGroup.DeepClone();
                if (aSource.SubGroups != null) aDest.SubGroups = aSource.SubGroups.DeepClone();
            }
        }
        #endregion //[basic generate part :: Create, ICloneable]

        #region [property part]

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
        /// 지점 이름 입니다.
        /// </summary>
        private string m_BranchName = string.Empty;
        /// <summary>
        /// 지점 이름 속성을 가져오거나 설정합니다.
        /// </summary>
        public string BranchName
        {
            get { return m_BranchName; }
            set { m_BranchName = value; }
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
        /// 센터 이름 입니다.
        /// </summary>
        private string m_CenterName = string.Empty;
        /// <summary>
        /// 센터 이름 속성을 가져오거나 설정합니다.
        /// </summary>
        public string CenterName
        {
            get { return m_CenterName; }
            set { m_CenterName = value; }
        }

        /// <summary>
        /// 부모 그룹 정보 입니다.
        /// </summary>
        private FACTGroupInfo m_ParentGroup = null;
        /// <summary>
        /// 부모 그룹 정보 속성을 가져오거나 설정합니다.
        /// </summary>
        public FACTGroupInfo ParentGroup
        {
            get { return m_ParentGroup; }
            set { m_ParentGroup = value; }
        }


        /// <summary>
        /// Device Count 입니다.
        /// </summary>
        private int m_DeviceCount;
        /// <summary>
        /// Device Count 속성을 가져오거나 설정합니다.
        /// </summary>
        public int DeviceCount
        {
            get 
            {

                return m_DeviceCount;
                

               
            }
            set { m_DeviceCount = value; }
        }

        public int GetToatalDeviceCount()
        {
            return GetDeviceCount(this);
        }

        private int GetDeviceCount(FACTGroupInfo aGroupInfo)
        {
            int tTotalCount = 0;
            try
            {

                if (aGroupInfo.SubGroups == null) return aGroupInfo.DeviceCount;
                foreach (FACTGroupInfo tSubGroup in aGroupInfo.SubGroups)
                {
                    if (tSubGroup.SubGroups != null)
                    {
                        tTotalCount += GetDeviceCount(tSubGroup);
                    }
                    else
                    {
                        tTotalCount += tSubGroup.DeviceCount;
                        System.Diagnostics.Debug.WriteLine(tSubGroup.DeviceCount);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            return tTotalCount;
        }



        


        /// <summary>
        /// 하위 그룹 목록 입니다.
        /// </summary>
        private FACTGroupInfoCollection m_SubGroups = null;
        /// <summary>
        /// 하위 그룹 목록 속성을 가져오거나 설정합니다.
        /// </summary>
        public FACTGroupInfoCollection SubGroups
        {
            get { return m_SubGroups; }
            set { m_SubGroups = value; }
        }

        //2010-10-11 hanjiyeon 추가 - 조직변경
        /// <summary>
        /// 실 단위의 조직 코드입니다. (실>팀>센터)
        /// </summary>
        private string m_ORG1Code = "";
        /// <summary>
        /// 실 단위의 조직 코드를 가져오거나 설정합니다. (실>팀>센터)
        /// </summary>
        public string ORG1Code
        {
            get { return m_ORG1Code; }
            set { m_ORG1Code = value; }
        }

        /// <summary>
        /// 실 단위의 조직 이름입니다. (실>팀>센터)
        /// </summary>
        private string m_ORG1Name = "";
        /// <summary>
        /// 실 단위의 조직명을 가져오거나 설정합니다. (실>팀>센터)
        /// </summary>
        public string ORG1Name
        {
            get { return m_ORG1Name; }
            set { m_ORG1Name = value; }
        }

        #endregion //[property part]
    }
    #endregion //GroupInfo 클래스입니다.

    #region GroupInfoCollection 클래스입니다.
    /// <summary>
    /// 그룹 정보 목록 클래스 입니다.             
    /// </summary>
    [Serializable]
    public class FACTGroupInfoCollection : GenericListMarshalByRef<FACTGroupInfo>, ICloneableEx<FACTGroupInfoCollection>
    {
        /// <summary>
        /// 부모 그룹 입니다.
        /// </summary>
        //private GroupInfo m_ParentGroup = null;

        #region [basic generate part :: Create, ICloneable]
        /// <summary>
        ///  기본 생성자 입니다.
        /// </summary>
        public FACTGroupInfoCollection()
        {
        }
        /// <summary>
        /// 복사 생성자 입니다.
        /// </summary>
        /// <param name="aGroupInfoCollection"></param>
        public FACTGroupInfoCollection(/*GroupInfo aGroupInfo, */FACTGroupInfoCollection aGroupInfoCollection)
        {
            //m_ParentGroup = aGroupInfo;
            CopyTo(aGroupInfoCollection, this, false);
        }
        ///// <summary>
        ///// 확장 생성자 입니다.
        ///// </summary>
        ///// <param name="aGroupInfo"></param>
        //public GroupInfoCollection(/*GroupInfo aGroupInfo*/)
        //{
        //    //m_ParentGroup = aGroupInfo;
        //}
        /////<summary>
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
        public FACTGroupInfoCollection CompactClone()
        {
            FACTGroupInfoCollection tCollection = new FACTGroupInfoCollection();
            CopyTo(this, tCollection, true);
            return tCollection;
        }
        /// <summary>
        /// ICloneable Interface Overload 깊은복사(Deep Copy)를 수행한 객체 복사본을 리턴합니다.
        /// </summary>
        /// <returns></returns>
        public FACTGroupInfoCollection DeepClone()
        {
            FACTGroupInfoCollection tCollection = new FACTGroupInfoCollection();
            CopyTo(this, tCollection, false);
            return tCollection;
        }
        /// <summary>
        /// 맴버 대입을 처리합니다.
        /// </summary>
        private void CopyTo(FACTGroupInfoCollection aSource, FACTGroupInfoCollection aDest, bool aIsCompactClone)
        {
            if (aDest != null && aDest.Count > 0)
                aDest.Clear();

            foreach (FACTGroupInfo tGroupInfo in aSource)
            {
                if (aIsCompactClone)
                    aDest.Add((FACTGroupInfo)tGroupInfo.CompactClone());
                else
                    aDest.Add((FACTGroupInfo)tGroupInfo.DeepClone());
            }
        }
        #endregion //[basic generate part :: Create, ICloneable]

        #region [public member part]

        /// <summary>
        /// GroupInfo 정보를 추가합니다.
        /// </summary>
        /// <param name="aGrouopInfo"></param>
        /// <returns></returns>
        public new int Add(FACTGroupInfo aGrouopInfo)
        {
            //aGrouopInfo.ParentGroup = m_ParentGroup;            
            //return m_ParentGroup.SubGroups.Add(aGrouopInfo);
            return base.InnerList.Add(aGrouopInfo);
        }

        #endregion //[public member part]


    }
    #endregion //GroupInfoCollection 클래스입니다.
}
