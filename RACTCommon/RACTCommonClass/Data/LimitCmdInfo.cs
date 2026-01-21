using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using ACPS.CommonConfigCompareClass;

namespace RACTCommonClass
{
    #region EmbagoInfo 클래스입니다.
    [Serializable]
    public class EmbagoInfo : ICloneableEx<EmbagoInfo>
    {
                /// <summary>
        /// 기본 생성자입니다. 
        /// </summary>
        public EmbagoInfo()
        {
        }

        /// <summary>
        /// 복사 생성자입니다. 		
        /// </summary>
        /// <param name="aLimitCmdInfo"></param>
        public EmbagoInfo(EmbagoInfo aEmbagoInfo)
        {
            CopyTo(aEmbagoInfo, this, false);
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
        public EmbagoInfo CompactClone()
        {
            EmbagoInfo tEmbagoInfo = new EmbagoInfo();
            CopyTo(this, tEmbagoInfo, true);
            return tEmbagoInfo;
        }
        /// <summary>
        /// ICloneable Interface Overload 깊은복사(Deep Copy)를 수행한 객체 복사본을 리턴합니다.
        /// </summary>
        /// <returns></returns>
        public EmbagoInfo DeepClone()
        {
            EmbagoInfo tEmbagoInfo = new EmbagoInfo();
            CopyTo(this, tEmbagoInfo, false);
            return tEmbagoInfo;
        }
        /// <summary>
        /// 개체 복사를 처리합니다.
        /// </summary>
        /// <param name="aSource">원본 개체입니다.</param>
        /// <param name="aDest">대상 개체입니다.</param>
        /// <param name="aIsCompactClone">Compact 복사 여부입니다.</param>
        private void CopyTo(EmbagoInfo aSource, EmbagoInfo aDest, bool aIsCompactClone)
        {
            aDest.Embargo = aSource.Embargo;
            aDest.EmbargoEnble = aSource.EmbargoEnble;
        }

        #region [property part]

        private String m_Embargo = String.Empty;
        public String Embargo
        {
            get { return m_Embargo; }
            set { m_Embargo = value; }
        }

        private bool m_EmbargoEnble = false;
        public bool EmbargoEnble
        {
            get { return m_EmbargoEnble; }
            set { m_EmbargoEnble = value; }
        }

        #endregion
    }


    #endregion

    #region LimitCmdInfo 클래스입니다.
    /// <summary>
    ///  모델 정보 클래스입니다.
    /// </summary>
    [Serializable]
    public class LimitCmdInfo : ICloneableEx<LimitCmdInfo>
    {
        #region [basic generate part :: Create, ICloneable]
        /// <summary>
        /// 기본 생성자입니다. 
        /// </summary>
        public LimitCmdInfo()
        {
        }
        /// <summary>
        /// 복사 생성자입니다. 		
        /// </summary>
        /// <param name="aLimitCmdInfo"></param>
        public LimitCmdInfo(LimitCmdInfo aLimitCmdInfo)
        {
            CopyTo(aLimitCmdInfo, this, false);
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
        public LimitCmdInfo CompactClone()
        {
            LimitCmdInfo tLimitCmdInfo = new LimitCmdInfo();
            CopyTo(this, tLimitCmdInfo, true);
            return tLimitCmdInfo;
        }
        /// <summary>
        /// ICloneable Interface Overload 깊은복사(Deep Copy)를 수행한 객체 복사본을 리턴합니다.
        /// </summary>
        /// <returns></returns>
        public LimitCmdInfo DeepClone()
        {
            LimitCmdInfo tLimitCmdInfo = new LimitCmdInfo();
            CopyTo(this, tLimitCmdInfo, false);
            return tLimitCmdInfo;
        }
        /// <summary>
        /// 개체 복사를 처리합니다.
        /// </summary>
        /// <param name="aSource">원본 개체입니다.</param>
        /// <param name="aDest">대상 개체입니다.</param>
        /// <param name="aIsCompactClone">Compact 복사 여부입니다.</param>
        private void CopyTo(LimitCmdInfo aSource, LimitCmdInfo aDest, bool aIsCompactClone)
        {
            aDest.EmbargoID = aSource.EmbargoID;
            aDest.ModelID = aSource.ModelID;
            aDest.EmbagoCmd = aSource.EmbagoCmd;
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
        /// 장비 모델별 사용 가능한 점검 명령어 제한 기능 입니다.
        /// </summary>
        private ArrayList m_EmbagoCmd = new ArrayList();
        /// <summary>
        /// 장비 모델별 사용 가능한 점검 명령어 제한 기능 속성을 가져오거나 설정합니다.
        /// </summary>
        public ArrayList EmbagoCmd
        {
            get { return m_EmbagoCmd; }
            set { m_EmbagoCmd = value; }
        }

        #endregion //[property part]
    }
    #endregion //LimitCmdInfo 클래스입니다.

    #region LimitCmdInfoCollection 클래스입니다.
    /// <summary>
    /// 모델 정보 목록 클래스 입니다.             
    /// </summary>
    [Serializable]
    public class LimitCmdInfoCollection : GenericListMarshalByRef<LimitCmdInfo>, ICloneableEx<LimitCmdInfoCollection>
    {
        #region [basic generate part :: Create, ICloneable]
        /// <summary>
        ///  기본 생성자 입니다.
        /// </summary>
        public LimitCmdInfoCollection()
        {
        }
        /// <summary>
        /// 복사 생성자 입니다.
        /// </summary>
        /// <param name="aLimitCmdInfoCollection"></param>
        public LimitCmdInfoCollection(LimitCmdInfoCollection aLimitCmdInfoCollection)
        {
            CopyTo(aLimitCmdInfoCollection, this, false);
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
        public LimitCmdInfoCollection CompactClone()
        {
            LimitCmdInfoCollection tCollection = new LimitCmdInfoCollection();
            CopyTo(this, tCollection, true);
            return tCollection;
        }
        /// <summary>
        /// ICloneable Interface Overload 깊은복사(Deep Copy)를 수행한 객체 복사본을 리턴합니다.
        /// </summary>
        /// <returns></returns>
        public LimitCmdInfoCollection DeepClone()
        {
            LimitCmdInfoCollection tCollection = new LimitCmdInfoCollection();
            CopyTo(this, tCollection, false);
            return tCollection;
        }
        /// <summary>
        /// 맴버 대입을 처리합니다.
        /// </summary>
        private void CopyTo(LimitCmdInfoCollection aSource, LimitCmdInfoCollection aDest, bool aIsCompactClone)
        {
            if (aDest != null && aDest.Count > 0)
                aDest.Clear();

            foreach (LimitCmdInfo tLimitCmdInfo in aSource)
            {
                if (aIsCompactClone)
                    aDest.Add((LimitCmdInfo)tLimitCmdInfo.CompactClone());
                else
                    aDest.Add((LimitCmdInfo)tLimitCmdInfo.DeepClone());
            }
        }
        #endregion //[basic generate part :: Create, ICloneable]

        #region [property part]
        /// <summary>
        /// 해당 ID의 요소를 가져오거나 설정합니다.
        /// </summary>
        /// <param name="aID">가져오거나 설정할 요소 ID입니다.</param>
        /// <returns></returns>
        public override LimitCmdInfo this[int aID]
        {
            get
            {
                LimitCmdInfo tLimitCmdInfo = null;
                lock (base.InnerList.SyncRoot)
                {
                    foreach (LimitCmdInfo tmpLimitCmdInfo in base.InnerList)
                    {
                        if (tmpLimitCmdInfo.ModelID == aID)
                        {
                            tLimitCmdInfo = tmpLimitCmdInfo;
                            break;
                        }
                    }
                }
                return tLimitCmdInfo;
            }
            set
            {
                LimitCmdInfo tLimitCmdInfo = null;
                lock (base.InnerList.SyncRoot)
                {
                    for (int idx = 0; idx < base.InnerList.Count; idx++)
                    {
                        tLimitCmdInfo = base.InnerList[idx] as LimitCmdInfo;
                        if (tLimitCmdInfo.ModelID == aID)
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
                foreach (LimitCmdInfo tLimitCmdInfo in base.InnerList)
                {
                    if (tLimitCmdInfo.ModelID == aID)
                    {
                        base.Remove(tLimitCmdInfo);
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
                foreach (LimitCmdInfo tLimitCmdInfo in base.InnerList)
                {
                    if (tLimitCmdInfo.ModelID == aModelID)
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
    #endregion //LimitCmdInfoCollection 클래스입니다.
}
