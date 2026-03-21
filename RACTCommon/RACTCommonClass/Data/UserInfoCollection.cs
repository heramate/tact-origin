using ACPS.CommonConfigCompareClass;
using System.Collections.Concurrent;
using System.Linq;

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

        private readonly ConcurrentDictionary<int, UserInfo> m_ClientMap = new ConcurrentDictionary<int, UserInfo>();

        public override void Add(UserInfo item)
        {
            if (item != null)
            {
                m_ClientMap[item.ClientID] = item;
                base.Add(item);
            }
        }

        public override void Remove(UserInfo item)
        {
            if (item != null)
            {
                UserInfo removed;
                m_ClientMap.TryRemove(item.ClientID, out removed);
                base.Remove(item);
            }
        }

        public override void Clear()
        {
            m_ClientMap.Clear();
            base.Clear();
        }

        /// <summary>
        /// 해당 ID의 요소를 가져오거나 설정합니다. (O(1) 검색 최적화)
        /// </summary>
        /// <param name="aID">가져오거나 설정할 요소 ID입니다.</param>
        /// <returns></returns>
        public override UserInfo this[int aID]
        {
            get
            {
                UserInfo tUserInfo;
                if (m_ClientMap.TryGetValue(aID, out tUserInfo))
                {
                    return tUserInfo;
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    m_ClientMap[aID] = value;
                    // base list 동기화는 필요시 처리 (여기서는 주로 검색 용도로 사용됨)
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
            UserInfo removed;
            if (m_ClientMap.TryRemove(aID, out removed))
            {
                base.Remove(removed);
            }
        }

        /// <summary>
        /// 해당 장비가 포함되어 있는지 여부를 반환합니다.
        /// </summary>
        /// <param name="aID"></param>
        /// <returns></returns>
        public bool Contains(int aID)
        {
            return m_ClientMap.ContainsKey(aID);
        }

        /// <summary>
        /// 안전한 순회를 위해 전체 목록을 복사하여 반환합니다.
        /// </summary>
        /// <returns></returns>
        public List<UserInfo> ToList()
        {
            return m_ClientMap.Values.ToList();
        }

      

        #endregion //[public member part]


    }
       
}
