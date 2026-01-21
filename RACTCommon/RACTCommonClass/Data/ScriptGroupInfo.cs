using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using ACPS.CommonConfigCompareClass;
using RACTTerminal;

namespace RACTCommonClass
{
    [Serializable]
    public class ScriptGroupInfo : ICloneableEx<ScriptGroupInfo>
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
        /// Description 입니다.
        /// </summary>
        private string m_Description;
        /// <summary>
        /// User ID 입니다.
        /// </summary>
        private int m_UserID;
        /// <summary>
        /// 그룹에 속한 스크립트 목록 입니다.
        /// </summary>
        private ScriptCollection m_ScriptList = new ScriptCollection();

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public ScriptGroupInfo() { }

        /// <summary>
        /// 복사 생성자 입니다.
        /// </summary>
        public ScriptGroupInfo(ScriptGroupInfo aGroupInfo)
        {
            if (aGroupInfo == null) return;
            m_ID = aGroupInfo.ID;
            m_Name = aGroupInfo.Name;
            m_UserID = aGroupInfo.UserID;
            m_ScriptList = aGroupInfo.m_ScriptList;
            m_Description = aGroupInfo.Description;
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
        public ScriptGroupInfo CompactClone()
        {
            ScriptGroupInfo tShortenCommandInfo = new ScriptGroupInfo();
            CopyTo(this, tShortenCommandInfo, true);
            return tShortenCommandInfo;
        }
        /// <summary>
        /// ICloneable Interface Overload 깊은복사(Deep Copy)를 수행한 객체 복사본을 리턴합니다.
        /// </summary>
        /// <returns></returns>
        public ScriptGroupInfo DeepClone()
        {
            ScriptGroupInfo tShortenCommandInfo = new ScriptGroupInfo();
            CopyTo(this, tShortenCommandInfo, false);
            return tShortenCommandInfo;
        }
        /// <summary>
        /// 개체 복사를 처리합니다.
        /// </summary>
        /// <param name="aSource">원본 개체입니다.</param>
        /// <param name="aDest">대상 개체입니다.</param>
        /// <param name="aIsCompactClone">Compact 복사 여부입니다.</param>
        private void CopyTo(ScriptGroupInfo aSource, ScriptGroupInfo aDest, bool aIsCompactClone)
        {
            aDest.ID = aSource.ID;
            aDest.Description = aSource.Description;
            aDest.ScriptList = aSource.ScriptList;
            aDest.Name = aSource.Name;
            aDest.UserID = aSource.UserID;
        }

        /// <summary>
        /// 그룹에 속한 스크립트 목록 가져오거나 설정 합니다.
        /// </summary>
        public ScriptCollection ScriptList
        {
            get { return m_ScriptList; }
            set { m_ScriptList = value; }
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
        /// Description 가져오거나 설정 합니다.
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
    public class ScriptGroupInfoCollection : GenericListMarshalByRef<ScriptGroupInfo>, ICloneableEx<ScriptGroupInfoCollection>
    {
        #region [basic generate part :: Create, ICloneable]
        /// <summary>
        ///  기본 생성자 입니다.
        /// </summary>
        public ScriptGroupInfoCollection()
        {
        }
        /// <summary>
        /// 복사 생성자 입니다.
        /// </summary>
        /// <param name="aShortenCommandInfoCollection"></param>
        public ScriptGroupInfoCollection(ScriptGroupInfoCollection aShortenCommandInfoCollection)
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
        public ScriptGroupInfoCollection CompactClone()
        {
            ScriptGroupInfoCollection tCollection = new ScriptGroupInfoCollection();
            CopyTo(this, tCollection, true);
            return tCollection;
        }
        /// <summary>
        /// ICloneable Interface Overload 깊은복사(Deep Copy)를 수행한 객체 복사본을 리턴합니다.
        /// </summary>
        /// <returns></returns>
        public ScriptGroupInfoCollection DeepClone()
        {
            ScriptGroupInfoCollection tCollection = new ScriptGroupInfoCollection();
            CopyTo(this, tCollection, false);
            return tCollection;
        }
        /// <summary>
        /// 맴버 대입을 처리합니다.
        /// </summary>
        private void CopyTo(ScriptGroupInfoCollection aSource, ScriptGroupInfoCollection aDest, bool aIsCompactClone)
        {
            if (aDest != null && aDest.Count > 0)
                aDest.Clear();

            foreach (ScriptGroupInfo tShortenCommandInfo in aSource)
            {
                if (aIsCompactClone)
                    aDest.Add((ScriptGroupInfo)tShortenCommandInfo.CompactClone());
                else
                    aDest.Add((ScriptGroupInfo)tShortenCommandInfo.DeepClone());
            }
        }
        #endregion //[basic generate part :: Create, ICloneable]

        #region [property part]
        /// <summary>
        /// 해당 ID의 요소를 가져오거나 설정합니다.
        /// </summary>
        /// <param name="aID">가져오거나 설정할 요소 ID입니다.</param>
        /// <returns></returns>
        public override ScriptGroupInfo this[int aID]
        {
            get
            {
                ScriptGroupInfo tShortenCommandInfo = null;
                lock (base.InnerList.SyncRoot)
                {
                    foreach (ScriptGroupInfo tmpShortenCommandInfo in base.InnerList)
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
                ScriptGroupInfo tShortenCommandInfo = null;
                lock (base.InnerList.SyncRoot)
                {
                    for (int idx = 0; idx < base.InnerList.Count; idx++)
                    {
                        tShortenCommandInfo = base.InnerList[idx] as ScriptGroupInfo;
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
                foreach (ScriptGroupInfo tShortenCommandInfo in base.InnerList)
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

        public ScriptGroupInfo GetScriptGroupInfo(int aScriptID)
        {
            foreach (ScriptGroupInfo tShortenCommandInfo in base.InnerList)
            {
                foreach (Script tScript in tShortenCommandInfo.ScriptList)
                {
                    if (tScript.ID == aScriptID)
                    {
                        return tShortenCommandInfo;
                    }
                }
            }
            return null;
        }

    }
}
