using System;
using System.Collections.Generic;
using System.Text;
using ACPS.CommonConfigCompareClass;

namespace RACTCommonClass
{
    [Serializable]
    public class ShortenCommandInfo : ICloneableEx<ShortenCommandInfo>
    {
        /// <summary>
        /// ID 입니다.
        /// </summary>
        private int m_ID;

        /// <summary>
        /// Name 입니다.
        /// </summary>
        private string m_Name;

        /// <summary>
        /// Command 입니다.
        /// </summary>
        private string m_Command;

        /// <summary>
        /// 설명 입니다.
        /// </summary>
        private string m_Description;
        /// <summary>
        /// 그룹 ID 입니다.
        /// </summary>
        private int m_GroupID;

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public ShortenCommandInfo() { }
         /// <summary>
        /// 복사 생성자입니다. 		
        /// </summary>
        /// <param name="aShortenCommandInfo"></param>
        public ShortenCommandInfo(ShortenCommandInfo aShortenCommandInfo)
        {
            CopyTo(aShortenCommandInfo, this, false);
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
        public ShortenCommandInfo CompactClone()
        {
            ShortenCommandInfo tShortenCommandInfo = new ShortenCommandInfo();
            CopyTo(this, tShortenCommandInfo, true);
            return tShortenCommandInfo;
        }
        /// <summary>
        /// ICloneable Interface Overload 깊은복사(Deep Copy)를 수행한 객체 복사본을 리턴합니다.
        /// </summary>
        /// <returns></returns>
        public ShortenCommandInfo DeepClone()
        {
            ShortenCommandInfo tShortenCommandInfo = new ShortenCommandInfo();
            CopyTo(this, tShortenCommandInfo, false);
            return tShortenCommandInfo;
        }
        /// <summary>
        /// 개체 복사를 처리합니다.
        /// </summary>
        /// <param name="aSource">원본 개체입니다.</param>
        /// <param name="aDest">대상 개체입니다.</param>
        /// <param name="aIsCompactClone">Compact 복사 여부입니다.</param>
        private void CopyTo(ShortenCommandInfo aSource, ShortenCommandInfo aDest, bool aIsCompactClone)
        {
            aDest.ID = aSource.ID;
            aDest.Name = aSource.Name;
            aDest.GroupID = aSource.GroupID;
            aDest.Command = aSource.Command;
            aDest.Description = aSource.Description;
        }

        /// <summary>
        /// 그룹 ID 가져오거나 설정 합니다.
        /// </summary>
        public int GroupID
        {
            get { return m_GroupID; }
            set { m_GroupID = value; }
        }



        /// <summary>
        /// 설명 가져오거나 설정 합니다.
        /// </summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }

        /// <summary>
        /// Command 가져오거나 설정 합니다.
        /// </summary>
        public string Command
        {
            get { return m_Command; }
            set { m_Command = value; }
        }

       

        /// <summary>
        /// Name 가져오거나 설정 합니다.
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        /// <summary>
        /// ID 가져오거나 설정 합니다.
        /// </summary>
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

    }

    #region ShortenCommandInfoCollection 클래스입니다.
    /// <summary>
    /// 장비 정보 목록 클래스 입니다.             
    /// </summary>
    [Serializable]
    public class ShortenCommandInfoCollection : GenericListMarshalByRef<ShortenCommandInfo>, ICloneableEx<ShortenCommandInfoCollection>
    {
        #region [basic generate part :: Create, ICloneable]
        /// <summary>
        ///  기본 생성자 입니다.
        /// </summary>
        public ShortenCommandInfoCollection()
        {
        }
        /// <summary>
        /// 복사 생성자 입니다.
        /// </summary>
        /// <param name="aShortenCommandInfoCollection"></param>
        public ShortenCommandInfoCollection(ShortenCommandInfoCollection aShortenCommandInfoCollection)
        {
            CopyTo(aShortenCommandInfoCollection, this, false);
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
        public ShortenCommandInfoCollection CompactClone()
        {
            ShortenCommandInfoCollection tCollection = new ShortenCommandInfoCollection();
            CopyTo(this, tCollection, true);
            return tCollection;
        }
        /// <summary>
        /// ICloneable Interface Overload 깊은복사(Deep Copy)를 수행한 객체 복사본을 리턴합니다.
        /// </summary>
        /// <returns></returns>
        public ShortenCommandInfoCollection DeepClone()
        {
            ShortenCommandInfoCollection tCollection = new ShortenCommandInfoCollection();
            CopyTo(this, tCollection, false);
            return tCollection;
        }
        /// <summary>
        /// 맴버 대입을 처리합니다.
        /// </summary>
        private void CopyTo(ShortenCommandInfoCollection aSource, ShortenCommandInfoCollection aDest, bool aIsCompactClone)
        {
            if (aDest != null && aDest.Count > 0)
                aDest.Clear();

            foreach (ShortenCommandInfo tShortenCommandInfo in aSource)
            {
                if (aIsCompactClone)
                    aDest.Add((ShortenCommandInfo)tShortenCommandInfo.CompactClone());
                else
                    aDest.Add((ShortenCommandInfo)tShortenCommandInfo.DeepClone());
            }
        }
        #endregion //[basic generate part :: Create, ICloneable]

        #region [property part]
        /// <summary>
        /// 해당 ID의 요소를 가져오거나 설정합니다.
        /// </summary>
        /// <param name="aID">가져오거나 설정할 요소 ID입니다.</param>
        /// <returns></returns>
        public override ShortenCommandInfo this[int aID]
        {
            get
            {
                ShortenCommandInfo tShortenCommandInfo = null;
                lock (base.InnerList.SyncRoot)
                {
                    foreach (ShortenCommandInfo tmpShortenCommandInfo in base.InnerList)
                    {
                        if (tmpShortenCommandInfo.ID == aID)
                        {
                            tShortenCommandInfo = tmpShortenCommandInfo;
                            break;
                        }
                    }
                }
                return tShortenCommandInfo;
            }
            set
            {
                ShortenCommandInfo tShortenCommandInfo = null;
                lock (base.InnerList.SyncRoot)
                {
                    for (int idx = 0; idx < base.InnerList.Count; idx++)
                    {
                        tShortenCommandInfo = base.InnerList[idx] as ShortenCommandInfo;
                        if (tShortenCommandInfo.ID == aID)
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
                foreach (ShortenCommandInfo tShortenCommandInfo in base.InnerList)
                {
                    if (tShortenCommandInfo.ID == aID)
                    {
                        base.Remove(tShortenCommandInfo);
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
        #endregion //[public member part]

        #region [public member part]


        #endregion //[public member part]
    }
    #endregion //ShortenCommandInfoCollection 클래스입니다.

    [Serializable]
    public class ShortenCommandGroupInfo : ICloneableEx<ShortenCommandGroupInfo>
    {

        /// <summary>
        /// ID 입니다.
        /// </summary>
        private int m_ID;

        /// <summary>
        /// Name 입니다.
        /// </summary>
        private string m_Name;

        /// <summary>
        /// 설명 입니다.
        /// </summary>
        private string m_Description;
        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public ShortenCommandGroupInfo()
        {
        }
           /// <summary>
        /// 복사 생성자입니다. 		
        /// </summary>
        /// <param name="aShortenCommandInfo"></param>
        public ShortenCommandGroupInfo(ShortenCommandGroupInfo aShortenCommandInfo)
        {
            CopyTo(aShortenCommandInfo, this, false);
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
        public ShortenCommandGroupInfo CompactClone()
        {
            ShortenCommandGroupInfo tShortenCommandInfo = new ShortenCommandGroupInfo();
            CopyTo(this, tShortenCommandInfo, true);
            return tShortenCommandInfo;
        }
        /// <summary>
        /// ICloneable Interface Overload 깊은복사(Deep Copy)를 수행한 객체 복사본을 리턴합니다.
        /// </summary>
        /// <returns></returns>
        public ShortenCommandGroupInfo DeepClone()
        {
            ShortenCommandGroupInfo tShortenCommandInfo = new ShortenCommandGroupInfo();
            CopyTo(this, tShortenCommandInfo, false);
            return tShortenCommandInfo;
        }
        /// <summary>
        /// 개체 복사를 처리합니다.
        /// </summary>
        /// <param name="aSource">원본 개체입니다.</param>
        /// <param name="aDest">대상 개체입니다.</param>
        /// <param name="aIsCompactClone">Compact 복사 여부입니다.</param>
        private void CopyTo(ShortenCommandGroupInfo aSource, ShortenCommandGroupInfo aDest, bool aIsCompactClone)
        {
            aDest.ID = aSource.ID;
            aDest.Name = aSource.Name;
            aDest.Description = aSource.Description;
            aDest.ShortenCommandList = aSource.ShortenCommandList;
        }

        /// <summary>
        /// 단축 명령 목록 입니다.
        /// </summary>
        private ShortenCommandInfoCollection m_ShortenCommandList = new ShortenCommandInfoCollection();

        /// <summary>
        /// 단축 명령 목록 가져오거나 설정 합니다.
        /// </summary>
        public ShortenCommandInfoCollection ShortenCommandList
        {
            get { return m_ShortenCommandList; }
            set { m_ShortenCommandList = value; }
        }


        /// <summary>
        /// 설명 가져오거나 설정 합니다.
        /// </summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }

        /// <summary>
        /// Name 가져오거나 설정 합니다.
        /// </summary>
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }


        /// <summary>
        /// ID 가져오거나 설정 합니다.
        /// </summary>
        public int ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }


    }

    [Serializable]
    public class ShortenCommandGroupInfoCollection : GenericListMarshalByRef<ShortenCommandGroupInfo>, ICloneableEx<ShortenCommandGroupInfoCollection>
    {
        #region [basic generate part :: Create, ICloneable]
        /// <summary>
        ///  기본 생성자 입니다.
        /// </summary>
        public ShortenCommandGroupInfoCollection()
        {
        }
        /// <summary>
        /// 복사 생성자 입니다.
        /// </summary>
        /// <param name="aShortenCommandInfoCollection"></param>
        public ShortenCommandGroupInfoCollection(ShortenCommandGroupInfoCollection aShortenCommandInfoCollection)
        {
            CopyTo(aShortenCommandInfoCollection, this, false);
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
        public ShortenCommandGroupInfoCollection CompactClone()
        {
            ShortenCommandGroupInfoCollection tCollection = new ShortenCommandGroupInfoCollection();
            CopyTo(this, tCollection, true);
            return tCollection;
        }
        /// <summary>
        /// ICloneable Interface Overload 깊은복사(Deep Copy)를 수행한 객체 복사본을 리턴합니다.
        /// </summary>
        /// <returns></returns>
        public ShortenCommandGroupInfoCollection DeepClone()
        {
            ShortenCommandGroupInfoCollection tCollection = new ShortenCommandGroupInfoCollection();
            CopyTo(this, tCollection, false);
            return tCollection;
        }
        /// <summary>
        /// 맴버 대입을 처리합니다.
        /// </summary>
        private void CopyTo(ShortenCommandGroupInfoCollection aSource, ShortenCommandGroupInfoCollection aDest, bool aIsCompactClone)
        {
            if (aDest != null && aDest.Count > 0)
                aDest.Clear();

            foreach (ShortenCommandGroupInfo tShortenCommandInfo in aSource)
            {
                if (aIsCompactClone)
                    aDest.Add((ShortenCommandGroupInfo)tShortenCommandInfo.CompactClone());
                else
                    aDest.Add((ShortenCommandGroupInfo)tShortenCommandInfo.DeepClone());
            }
        }
        #endregion //[basic generate part :: Create, ICloneable]

        #region [property part]
        /// <summary>
        /// 해당 ID의 요소를 가져오거나 설정합니다.
        /// </summary>
        /// <param name="aID">가져오거나 설정할 요소 ID입니다.</param>
        /// <returns></returns>
        public override ShortenCommandGroupInfo this[int aID]
        {
            get
            {
                ShortenCommandGroupInfo tShortenCommandInfo = null;
                lock (base.InnerList.SyncRoot)
                {
                    foreach (ShortenCommandGroupInfo tmpShortenCommandInfo in base.InnerList)
                    {
                        if (tmpShortenCommandInfo.ID == aID)
                        {
                            tShortenCommandInfo = tmpShortenCommandInfo;
                            break;
                        }
                    }
                }
                return tShortenCommandInfo;
            }
            set
            {
                ShortenCommandGroupInfo tShortenCommandInfo = null;
                lock (base.InnerList.SyncRoot)
                {
                    for (int idx = 0; idx < base.InnerList.Count; idx++)
                    {
                        tShortenCommandInfo = base.InnerList[idx] as ShortenCommandGroupInfo;
                        if (tShortenCommandInfo.ID == aID)
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
                foreach (ShortenCommandGroupInfo tShortenCommandInfo in base.InnerList)
                {
                    if (tShortenCommandInfo.ID == aID)
                    {
                        base.Remove(tShortenCommandInfo);
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
        #endregion //[public member part]

        #region [public member part]


        #endregion //[public member part]
    }
}
