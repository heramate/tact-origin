using System;
using System.Collections.Generic;
using System.Text;
using ACPS.CommonConfigCompareClass;

namespace TelnetProcessor
{
    public class TelnetSessionCollection  : GenericListMarshalByRef<TelnetSessionInfo>
    {
        /// <summary>
        ///  기본 생성자 입니다.
        /// </summary>
        public TelnetSessionCollection()
        {
        }
        /// <summary>
        /// 복사 생성자 입니다.
        /// </summary>
        /// <param name="aDeviceInfoCollection"></param>
        public TelnetSessionCollection(TelnetSessionCollection aDeviceInfoCollection)
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
        public TelnetSessionCollection CompactClone()
        {
            TelnetSessionCollection tCollection = new TelnetSessionCollection();
            CopyTo(this, tCollection, true);
            return tCollection;
        }
        /// <summary>
        /// ICloneable Interface Overload 깊은복사(Deep Copy)를 수행한 객체 복사본을 리턴합니다.
        /// </summary>
        /// <returns></returns>
        public TelnetSessionCollection DeepClone()
        {
            TelnetSessionCollection tCollection = new TelnetSessionCollection();
            CopyTo(this, tCollection, false);
            return tCollection;
        }
        /// <summary>
        /// 맴버 대입을 처리합니다.
        /// </summary>
        private void CopyTo(TelnetSessionCollection aSource, TelnetSessionCollection aDest, bool aIsCompactClone)
        {
            if (aDest != null && aDest.Count > 0)
                aDest.Clear();

            foreach (TelnetSessionInfo tDeviceInfo in aSource)
            {
                if (aIsCompactClone)
                    aDest.Add((TelnetSessionInfo)tDeviceInfo);
                else
                    aDest.Add((TelnetSessionInfo)tDeviceInfo);
            }
        }
        /// <summary>
        /// 해당 ID의 요소를 가져오거나 설정합니다.
        /// </summary>
        /// <param name="aID">가져오거나 설정할 요소 ID입니다.</param>
        /// <returns></returns>
        public override TelnetSessionInfo this[int aSessionID]
        {
            get
            {
                TelnetSessionInfo tDeviceInfo = null;
                lock (base.InnerList.SyncRoot)
                {
                    foreach (TelnetSessionInfo tmpDeviceInfo in base.InnerList)
                    {
                        if (tmpDeviceInfo.ConnectionSessionID == aSessionID)
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
                TelnetSessionInfo tDeviceInfo = null;
                lock (base.InnerList.SyncRoot)
                {
                    for (int idx = 0; idx < base.InnerList.Count; idx++)
                    {
                        tDeviceInfo = base.InnerList[idx] as TelnetSessionInfo;
                        if (tDeviceInfo.ConnectionSessionID == aSessionID)
                        {
                            base.InnerList[idx] = value;
                            break;
                        }
                    }
                }
            }
        }

      
        /// <summary>
        /// 해당 ID의 요소를 제거합니다.
        /// </summary>
        /// <param name="aID"></param>
        public void Remove(int aID)
        {
            lock (base.InnerList.SyncRoot)
            {
                foreach (TelnetSessionInfo tDeviceInfo in base.InnerList)
                {
                    if (tDeviceInfo.ConnectionSessionID == aID)
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

    }
}
