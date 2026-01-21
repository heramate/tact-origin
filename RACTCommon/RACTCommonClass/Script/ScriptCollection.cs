using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using ACPS.CommonConfigCompareClass;

namespace RACTCommonClass
{
    [Serializable]
    public class ScriptCollection  : GenericListMarshalByRef<Script>, ICloneableEx<ScriptCollection>
    {
        #region [basic generate part :: Create, ICloneable]
        /// <summary>
        ///  기본 생성자 입니다.
        /// </summary>
        public ScriptCollection()
        {
        }
        /// <summary>
        /// 복사 생성자 입니다.
        /// </summary>
        /// <param name="aShortenCommandInfoCollection"></param>
        public ScriptCollection(ScriptCollection aShortenCommandInfoCollection)
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
        public ScriptCollection CompactClone()
        {
            ScriptCollection tCollection = new ScriptCollection();
            CopyTo(this, tCollection, true);
            return tCollection;
        }
        /// <summary>
        /// ICloneable Interface Overload 깊은복사(Deep Copy)를 수행한 객체 복사본을 리턴합니다.
        /// </summary>
        /// <returns></returns>
        public ScriptCollection DeepClone()
        {
            ScriptCollection tCollection = new ScriptCollection();
            CopyTo(this, tCollection, false);
            return tCollection;
        }
        /// <summary>
        /// 맴버 대입을 처리합니다.
        /// </summary>
        private void CopyTo(ScriptCollection aSource, ScriptCollection aDest, bool aIsCompactClone)
        {
            if (aDest != null && aDest.Count > 0)
                aDest.Clear();

            foreach (Script tShortenCommandInfo in aSource)
            {
                if (aIsCompactClone)
                    aDest.Add((Script)tShortenCommandInfo.CompactClone());
                else
                    aDest.Add((Script)tShortenCommandInfo.DeepClone());
            }
        }
        #endregion //[basic generate part :: Create, ICloneable]

        #region [property part]
        /// <summary>
        /// 해당 ID의 요소를 가져오거나 설정합니다.
        /// </summary>
        /// <param name="aID">가져오거나 설정할 요소 ID입니다.</param>
        /// <returns></returns>
        public override Script this[int aID]
        {
            get
            {
                Script tShortenCommandInfo = null;
                lock (base.InnerList.SyncRoot)
                {
                    foreach (Script tmpShortenCommandInfo in base.InnerList)
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
                Script tShortenCommandInfo = null;
                lock (base.InnerList.SyncRoot)
                {
                    for (int idx = 0; idx < base.InnerList.Count; idx++)
                    {
                        tShortenCommandInfo = base.InnerList[idx] as Script;
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
                foreach (Script tShortenCommandInfo in base.InnerList)
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
