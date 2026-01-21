using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using ACPS.CommonConfigCompareClass;

namespace RACTCommonClass
{
    #region ModelInfo 클래스입니다.
    /// <summary>
    ///  모델 정보 클래스입니다.
    /// </summary>
    [Serializable]
    public class ModelInfo : ICloneableEx<ModelInfo>
    {
        #region [basic generate part :: Create, ICloneable]
        /// <summary>
        /// 기본 생성자입니다. 
        /// </summary>
        public ModelInfo()
        {
        }
        /// <summary>
        /// 복사 생성자입니다. 		
        /// </summary>
        /// <param name="aModelInfo"></param>
        public ModelInfo(ModelInfo aModelInfo)
        {
            CopyTo(aModelInfo, this, false);
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
        public ModelInfo CompactClone()
        {
            ModelInfo tModelInfo = new ModelInfo();
            CopyTo(this, tModelInfo, true);
            return tModelInfo;
        }
        /// <summary>
        /// ICloneable Interface Overload 깊은복사(Deep Copy)를 수행한 객체 복사본을 리턴합니다.
        /// </summary>
        /// <returns></returns>
        public ModelInfo DeepClone()
        {
            ModelInfo tModelInfo = new ModelInfo();
            CopyTo(this, tModelInfo, false);
            return tModelInfo;
        }
        /// <summary>
        /// 개체 복사를 처리합니다.
        /// </summary>
        /// <param name="aSource">원본 개체입니다.</param>
        /// <param name="aDest">대상 개체입니다.</param>
        /// <param name="aIsCompactClone">Compact 복사 여부입니다.</param>
        private void CopyTo(ModelInfo aSource, ModelInfo aDest, bool aIsCompactClone)
        {
            aDest.ModelID = aSource.ModelID;
            aDest.ModelName = aSource.ModelName;
            aDest.PortCount = aSource.PortCount;
            aDest.ModelTypeCode = aSource.ModelTypeCode;
            aDest.ModelTypeName = aSource.ModelTypeName;
            aDest.ViewOrder = aSource.ViewOrder;
            aDest.MoreString = aSource.MoreString;
            aDest.MoreMark = aSource.MoreMark;
            aDest.SlotCount = aSource.SlotCount;
            aDest.Divergence = aSource.Divergence;

            aDest.IpTypeCd = aSource.IpTypeCd;

            // 2013-05-02- shinyn - 객체복사
            aDest.CfgRestoreCommands = aSource.CfgRestoreCommands;
            aDest.DefaultConnectionCommadSet = aSource.DefaultConnectionCommadSet;

        }
        #endregion //[basic generate part :: Create, ICloneable]

        #region [property part]

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
        /// 모델 이름 입니다.
        /// </summary>
        private string m_ModelName = string.Empty;
        /// <summary>
        /// 모델 이름 속성을 가져오거나 설정합니다.
        /// </summary>
        public string ModelName
        {
            get { return m_ModelName; }
            set { m_ModelName = value; }
        }

        /// <summary>
        /// 포트 개수 입니다.
        /// </summary>
        private int m_PortCount = 0;
        /// <summary>
        /// 포트 개수 속성을 가져오거나 설정합니다.
        /// </summary>
        public int PortCount
        {
            get { return m_PortCount; }
            set { m_PortCount = value; }
        }

        /// <summary>
        /// 모델 타입 코드 입니다.
        /// </summary>
        private int m_ModelTypeCode = 0;
        /// <summary>
        /// 모델 타입 코드 속성을 가져오거나 설정합니다.
        /// </summary>
        public int ModelTypeCode
        {
            get { return m_ModelTypeCode; }
            set { m_ModelTypeCode = value; }
        }

        /// <summary>
        /// 모델 타입 이름 입니다.
        /// </summary>
        private string m_ModelTypeName = string.Empty;
        /// <summary>
        /// 모델 타입 이름 속성을 가져오거나 설정합니다.
        /// </summary>
        public string ModelTypeName
        {
            get { return m_ModelTypeName; }
            set { m_ModelTypeName = value; }
        }

        /// <summary>
        /// 표시 순서 입니다.
        /// </summary>
        private int m_ViewOrder = 0;
        /// <summary>
        /// 표시 순서 속성을 가져오거나 설정합니다.
        /// </summary>
        public int ViewOrder
        {
            get { return m_ViewOrder; }
            set { m_ViewOrder = value; }
        }

        /// <summary>
        /// MoreString 입니다.
        /// </summary>
        private string m_MoreString = string.Empty;
        /// <summary>
        /// MoreString 속성을 가져오거나 설정합니다.
        /// </summary>
        public string MoreString
        {
            get { return m_MoreString; }
            set { m_MoreString = value; }
        }

        /// <summary>
        /// MoreMark 입니다.
        /// </summary>
        private string m_MoreMark = string.Empty;
        /// <summary>
        /// MoreMark 속성을 가져오거나 설정합니다.
        /// </summary>
        public string MoreMark
        {
            get { return m_MoreMark; }
            set { m_MoreMark = value; }
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


        /// <summary>
        /// 슬롯개수 입니다.
        /// </summary>
        private int m_SlotCount = 0;
        /// <summary>
        /// 슬롯개수 속성을 가져오거나 설정합니다.
        /// </summary>
        public int SlotCount
        {
            get { return m_SlotCount; }
            set { m_SlotCount = value; }
        }

        /// <summary>
        /// 분기수(G/E-PON) 입니다.
        /// </summary>
        private int m_Divergence = 0;
        /// <summary>
        /// 분기수(G/E-PON) 속성을 가져오거나 설정합니다.
        /// </summary>
        public int Divergence
        {
            get { return m_Divergence; }
            set { m_Divergence = value; }
        }

        /// <summary>
        /// IP망 타입 구분 입니다.
        /// </summary>
        private int m_IpTypeCd = 1;
        /// <summary>
        /// IP망 타입 속성을 가져오거나 설정합니다.
        /// </summary>
        public int IpTypeCd
        {
            get { return m_IpTypeCd; }
            set { m_IpTypeCd = value; }
        }

        /// <summary>
        /// 2013-01-11 - shinyn - Config 복원명령을 모델별로 가지고 있는다.
        /// </summary>
        private CfgRestoreCommandCollection m_CfgRestoreCommands = new CfgRestoreCommandCollection();

        public CfgRestoreCommandCollection CfgRestoreCommands
        {
            get { return m_CfgRestoreCommands; }
            set { m_CfgRestoreCommands = value; }
        }

        /// <summary>
        /// 2013-05-02 - shinyn - 기본접속 명령을 미리 받아와서 넣어넣고 불러올때 사용합니다.
        /// </summary>
        private FACT_DefaultConnectionCommandSet m_DefaultConnectionCommandSet = new FACT_DefaultConnectionCommandSet();

        public FACT_DefaultConnectionCommandSet DefaultConnectionCommadSet
        {
            get { return m_DefaultConnectionCommandSet; }
            set { m_DefaultConnectionCommandSet = value; }
        }


        #endregion //[property part]
    }
    #endregion //ModelInfo 클래스입니다.

    #region ModelInfoCollection 클래스입니다.
    /// <summary>
    /// 모델 정보 목록 클래스 입니다.             
    /// </summary>
    [Serializable]
    public class ModelInfoCollection : GenericListMarshalByRef<ModelInfo>, ICloneableEx<ModelInfoCollection>
    {
        #region [basic generate part :: Create, ICloneable]
        /// <summary>
        ///  기본 생성자 입니다.
        /// </summary>
        public ModelInfoCollection()
        {
        }
        /// <summary>
        /// 복사 생성자 입니다.
        /// </summary>
        /// <param name="aModelInfoCollection"></param>
        public ModelInfoCollection(ModelInfoCollection aModelInfoCollection)
        {
            CopyTo(aModelInfoCollection, this, false);
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
        public ModelInfoCollection CompactClone()
        {
            ModelInfoCollection tCollection = new ModelInfoCollection();
            CopyTo(this, tCollection, true);
            return tCollection;
        }
        /// <summary>
        /// ICloneable Interface Overload 깊은복사(Deep Copy)를 수행한 객체 복사본을 리턴합니다.
        /// </summary>
        /// <returns></returns>
        public ModelInfoCollection DeepClone()
        {
            ModelInfoCollection tCollection = new ModelInfoCollection();
            CopyTo(this, tCollection, false);
            return tCollection;
        }
        /// <summary>
        /// 맴버 대입을 처리합니다.
        /// </summary>
        private void CopyTo(ModelInfoCollection aSource, ModelInfoCollection aDest, bool aIsCompactClone)
        {
            if (aDest != null && aDest.Count > 0)
                aDest.Clear();

            foreach (ModelInfo tModelInfo in aSource)
            {
                if (aIsCompactClone)
                    aDest.Add((ModelInfo)tModelInfo.CompactClone());
                else
                    aDest.Add((ModelInfo)tModelInfo.DeepClone());
            }
        }
        #endregion //[basic generate part :: Create, ICloneable]

        #region [property part]
        /// <summary>
        /// 해당 ID의 요소를 가져오거나 설정합니다.
        /// </summary>
        /// <param name="aID">가져오거나 설정할 요소 ID입니다.</param>
        /// <returns></returns>
        public override ModelInfo this[int aID]
        {
            get
            {
                ModelInfo tModelInfo = null;
                lock (base.InnerList.SyncRoot)
                {
                    foreach (ModelInfo tmpModelInfo in base.InnerList)
                    {
                        if (tmpModelInfo.ModelID == aID)
                        {
                            tModelInfo = tmpModelInfo;
                            break;
                        }
                    }
                }
                return tModelInfo;
            }
            set
            {
                ModelInfo tModelInfo = null;
                lock (base.InnerList.SyncRoot)
                {
                    for (int idx = 0; idx < base.InnerList.Count; idx++)
                    {
                        tModelInfo = base.InnerList[idx] as ModelInfo;
                        if (tModelInfo.ModelID == aID)
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
                foreach (ModelInfo tModelInfo in base.InnerList)
                {
                    if (tModelInfo.ModelID == aID)
                    {
                        base.Remove(tModelInfo);
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
                foreach (ModelInfo tModelInfo in base.InnerList)
                {
                    if (tModelInfo.ModelID == aModelID)
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
    #endregion //ModelInfoCollection 클래스입니다.
}
