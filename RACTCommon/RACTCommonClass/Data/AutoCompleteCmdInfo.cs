using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using ACPS.CommonConfigCompareClass;

namespace RACTCommonClass
{
    #region AutoCompleteCmdInfo 클래스입니다.
    /// <summary>
    ///  모델 정보 클래스입니다.
    /// </summary>
    [Serializable]
    public class AutoCompleteCmdInfo : ICloneableEx<AutoCompleteCmdInfo>
    {
        #region [basic generate part :: Create, ICloneable]
        /// <summary>
        /// 기본 생성자입니다. 
        /// </summary>
        public AutoCompleteCmdInfo()
        {
        }
        /// <summary>
        /// 복사 생성자입니다. 		
        /// </summary>
        /// <param name="aAutoCompleteCmdInfo"></param>
        public AutoCompleteCmdInfo(AutoCompleteCmdInfo aAutoCompleteCmdInfo)
        {
            CopyTo(aAutoCompleteCmdInfo, this, false);
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
        public AutoCompleteCmdInfo CompactClone()
        {
            AutoCompleteCmdInfo tAutoCompleteCmdInfo = new AutoCompleteCmdInfo();
            CopyTo(this, tAutoCompleteCmdInfo, true);
            return tAutoCompleteCmdInfo;
        }
        /// <summary>
        /// ICloneable Interface Overload 깊은복사(Deep Copy)를 수행한 객체 복사본을 리턴합니다.
        /// </summary>
        /// <returns></returns>
        public AutoCompleteCmdInfo DeepClone()
        {
            AutoCompleteCmdInfo tAutoCompleteCmdInfo = new AutoCompleteCmdInfo();
            CopyTo(this, tAutoCompleteCmdInfo, false);
            return tAutoCompleteCmdInfo;
        }
        /// <summary>
        /// 개체 복사를 처리합니다.
        /// </summary>
        /// <param name="aSource">원본 개체입니다.</param>
        /// <param name="aDest">대상 개체입니다.</param>
        /// <param name="aIsCompactClone">Compact 복사 여부입니다.</param>
        private void CopyTo(AutoCompleteCmdInfo aSource, AutoCompleteCmdInfo aDest, bool aIsCompactClone)
        {
            aDest.EmbargoID = aSource.EmbargoID;
            aDest.ModelID = aSource.ModelID;
            aDest.Command = aSource.Command;
            aDest.Description = aSource.Description;
        }
        #endregion //[basic generate part :: Create, ICloneable]

        #region [property part]

        /// <summary>
        /// 제한명령어 시퀀스 입니다.
        /// </summary>
        private int m_EmbargoID = 0;
        /// <summary>
        /// 제한명령어 시퀀스 가져오거나 설정합니다.
        /// </summary>
        public int EmbargoID
        {
            get { return m_EmbargoID; }
            set { m_EmbargoID = value; }
        }

        /// <summary>
        /// 모델의 고유 ID 입니다.
        /// </summary>
        private int m_ModelID = 0;
        /// <summary>
        /// 모델의 고유 ID 속성을 가져오거나 설정합니다.
        /// </summary>
        public int ModelID
        {
            get { return m_ModelID; }
            set { m_ModelID = value; }
        }


        /// <summary>
        /// 장비 모델별 기본명령어 입니다.
        /// </summary>
        private ArrayList m_Command = new ArrayList();


        /// <summary>
        /// 장비 모델별 기본명령어를 가져오거나 설정합니다.
        /// </summary>
        public ArrayList Command
        {
            get { return m_Command; }
            set { m_Command = value; }
        }


        /// <summary>
        /// 장비 모델별 기본명령어 설명 입니다.
        /// </summary>
        private ArrayList m_Description = new ArrayList();


        /// <summary>
        /// 장비 모델별 기본명령어 설명을 가져오거나 설정합니다.
        /// </summary>
        public ArrayList Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }

        #endregion //[property part]
    }
    #endregion //AutoCompleteCmdInfo 클래스입니다.

    #region AutoCompleteCmdInfoCollection 클래스입니다.
    /// <summary>
    /// 모델 정보 목록 클래스 입니다.             
    /// </summary>
    [Serializable]
    public class AutoCompleteCmdInfoCollection : GenericListMarshalByRef<AutoCompleteCmdInfo>, ICloneableEx<AutoCompleteCmdInfoCollection>
    {
        #region [basic generate part :: Create, ICloneable]
        /// <summary>
        ///  기본 생성자 입니다.
        /// </summary>
        public AutoCompleteCmdInfoCollection()
        {
        }
        /// <summary>
        /// 복사 생성자 입니다.
        /// </summary>
        /// <param name="aAutoCompleteCmdInfoCollection"></param>
        public AutoCompleteCmdInfoCollection(AutoCompleteCmdInfoCollection aAutoCompleteCmdInfoCollection)
        {
            CopyTo(aAutoCompleteCmdInfoCollection, this, false);
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
        public AutoCompleteCmdInfoCollection CompactClone()
        {
            AutoCompleteCmdInfoCollection tCollection = new AutoCompleteCmdInfoCollection();
            CopyTo(this, tCollection, true);
            return tCollection;
        }
        /// <summary>
        /// ICloneable Interface Overload 깊은복사(Deep Copy)를 수행한 객체 복사본을 리턴합니다.
        /// </summary>
        /// <returns></returns>
        public AutoCompleteCmdInfoCollection DeepClone()
        {
            AutoCompleteCmdInfoCollection tCollection = new AutoCompleteCmdInfoCollection();
            CopyTo(this, tCollection, false);
            return tCollection;
        }
        /// <summary>
        /// 맴버 대입을 처리합니다.
        /// </summary>
        private void CopyTo(AutoCompleteCmdInfoCollection aSource, AutoCompleteCmdInfoCollection aDest, bool aIsCompactClone)
        {
            if (aDest != null && aDest.Count > 0)
                aDest.Clear();

            foreach (AutoCompleteCmdInfo tAutoCompleteCmdInfo in aSource)
            {
                if (aIsCompactClone)
                    aDest.Add((AutoCompleteCmdInfo)tAutoCompleteCmdInfo.CompactClone());
                else
                    aDest.Add((AutoCompleteCmdInfo)tAutoCompleteCmdInfo.DeepClone());
            }
        }
        #endregion //[basic generate part :: Create, ICloneable]

        #region [property part]
        /// <summary>
        /// 해당 ID의 요소를 가져오거나 설정합니다.
        /// </summary>
        /// <param name="aID">가져오거나 설정할 요소 ID입니다.</param>
        /// <returns></returns>
        public override AutoCompleteCmdInfo this[int aID]
        {
            get
            {
                AutoCompleteCmdInfo tAutoCompleteCmdInfo = null;
                lock (base.InnerList.SyncRoot)
                {
                    foreach (AutoCompleteCmdInfo tmpAutoCompleteCmdInfo in base.InnerList)
                    {
                        if (tmpAutoCompleteCmdInfo.ModelID == aID)
                        {
                            tAutoCompleteCmdInfo = tmpAutoCompleteCmdInfo;
                            break;
                        }
                    }
                }
                return tAutoCompleteCmdInfo;
            }
            set
            {
                AutoCompleteCmdInfo tAutoCompleteCmdInfo = null;
                lock (base.InnerList.SyncRoot)
                {
                    for (int idx = 0; idx < base.InnerList.Count; idx++)
                    {
                        tAutoCompleteCmdInfo = base.InnerList[idx] as AutoCompleteCmdInfo;
                        if (tAutoCompleteCmdInfo.ModelID == aID)
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
                foreach (AutoCompleteCmdInfo tAutoCompleteCmdInfo in base.InnerList)
                {
                    if (tAutoCompleteCmdInfo.ModelID == aID)
                    {
                        base.Remove(tAutoCompleteCmdInfo);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 모델 ID가 포함되었는 여부를 가져오기 합니다.
        /// </summary>
        /// <param name="aModelID"></param>
        public bool Contains(int aModelID)
        {
            bool tResult = false;
            lock (base.InnerList.SyncRoot)
            {
                foreach (AutoCompleteCmdInfo tAutoCompleteCmdInfo in base.InnerList)
                {
                    if (tAutoCompleteCmdInfo.ModelID == aModelID)
                    {
                        tResult = true;
                        break;
                    }
                }
            }
            return tResult;
        }

        #endregion //[public member part]
    }
    #endregion //AutoCompleteCmdInfoCollection 클래스입니다.
}
