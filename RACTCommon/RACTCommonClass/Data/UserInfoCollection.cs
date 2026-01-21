using System;
using System.Collections.Generic;
using System.Text;
using ACPS.CommonConfigCompareClass;

namespace RACTCommonClass
{
    [Serializable]
    public class UserInfoCollection : GenericListMarshalByRef<UserInfo>, ICloneableEx<UserInfoCollection>
    {
        #region [basic generate part :: Create, ICloneable]
        /// <summary>
        ///  기본 생성자 입니다.
        /// </summary>
        public UserInfoCollection()
        {
        }
        /// <summary>
        /// 복사 생성자 입니다.
        /// </summary>
        /// <param name="aDeviceInfoCollection"></param>
        public UserInfoCollection(UserInfoCollection aDeviceInfoCollection)
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
        public UserInfoCollection CompactClone()
        {
            UserInfoCollection tCollection = new UserInfoCollection();
            CopyTo(this, tCollection, true);
            return tCollection;
        }
        /// <summary>
        /// ICloneable Interface Overload 깊은복사(Deep Copy)를 수행한 객체 복사본을 리턴합니다.
        /// </summary>
        /// <returns></returns>
        public UserInfoCollection DeepClone()
        {
            UserInfoCollection tCollection = new UserInfoCollection();
            CopyTo(this, tCollection, false);
            return tCollection;
        }
        /// <summary>
        /// 맴버 대입을 처리합니다.
        /// </summary>
        private void CopyTo(UserInfoCollection aSource, UserInfoCollection aDest, bool aIsCompactClone)
        {
            if (aDest != null && aDest.Count > 0)
                aDest.Clear();

            foreach (UserInfo tDeviceInfo in aSource)
            {
                if (aIsCompactClone)
                    aDest.Add((UserInfo)tDeviceInfo.CompactClone());
                else
                    aDest.Add((UserInfo)tDeviceInfo.DeepClone());
            }
        }
        #endregion //[basic generate part :: Create, ICloneable]

        #region [property part]
        /// <summary>
        /// 해당 ID의 요소를 가져오거나 설정합니다.
        /// </summary>
        /// <param name="aID">가져오거나 설정할 요소 ID입니다.</param>
        /// <returns></returns>
        public override UserInfo this[int aID]
        {
            get
            {
                UserInfo tDeviceInfo = null;
                lock (base.InnerList.SyncRoot)
                {
                    foreach (UserInfo tmpDeviceInfo in base.InnerList)
                    {
                        if (tmpDeviceInfo.ClientID == aID)
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
                UserInfo tDeviceInfo = null;
                lock (base.InnerList.SyncRoot)
                {
                    for (int idx = 0; idx < base.InnerList.Count; idx++)
                    {
                        tDeviceInfo = base.InnerList[idx] as UserInfo;
                        if (tDeviceInfo.ClientID == aID)
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
                foreach (UserInfo tDeviceInfo in base.InnerList)
                {
                    if (tDeviceInfo.ClientID == aID)
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

      

        #endregion //[public member part]


    }
       
}
