using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using ACPS.CommonConfigCompareClass;

namespace RACTCommonClass
{
    [Serializable]
    public class DaemonProcessInfo:ICloneableEx<DaemonProcessInfo>
    {
        /// <summary>
        /// 접속된 사용자 수 입니다.
        /// </summary>
        private int m_ConnectUsercount;

        /// <summary>
        /// 연결된 Telnet Session 수 입니다.
        /// </summary>
        private int m_TelnetSessionCount;

        /// <summary>
        /// Daemon ID 입니다.
        /// </summary>
        private int m_DaemonID;
        /// <summary>
        /// 데이터 큐(데이터 형식=  byte[])  입니다.
        /// </summary>
        private Queue m_DataQueue = new Queue();
        /// <summary>
        /// IP 입니다.
        /// </summary>
        private string m_IP;

        /// <summary>
        /// Port 입니다.
        /// </summary>
        private int m_Port;
        /// <summary>
        /// Channel Name 입니다.
        /// </summary>
        private string m_ChannelName;
        /// <summary>
        /// 클라이언트 라이프타임 입니다.
        /// </summary>
        private DateTime m_LifeTime;

        /// <summary>
        /// 기본 생성자입니다. 
        /// </summary>
        public DaemonProcessInfo()
        {
            m_DaemonID = this.GetHashCode();
        }
        /// <summary>
        /// 복사 생성자입니다. 		
        /// </summary>
        /// <param name="aDeviceInfo"></param>
        public DaemonProcessInfo(DaemonProcessInfo aDeviceInfo)
        {
            CopyTo(aDeviceInfo, this, false);
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
        public DaemonProcessInfo CompactClone()
        {
            DaemonProcessInfo tDeviceInfo = new DaemonProcessInfo();
            CopyTo(this, tDeviceInfo, true);
            return tDeviceInfo;
        }
        /// <summary>
        /// ICloneable Interface Overload 깊은복사(Deep Copy)를 수행한 객체 복사본을 리턴합니다.
        /// </summary>
        /// <returns></returns>
        public DaemonProcessInfo DeepClone()
        {
            DaemonProcessInfo tDeviceInfo = new DaemonProcessInfo();
            CopyTo(this, tDeviceInfo, false);
            return tDeviceInfo;
        }
        /// <summary>
        /// 개체 복사를 처리합니다.
        /// </summary>
        /// <param name="aSource">원본 개체입니다.</param>
        /// <param name="aDest">대상 개체입니다.</param>
        /// <param name="aIsCompactClone">Compact 복사 여부입니다.</param>
        private void CopyTo(DaemonProcessInfo aSource, DaemonProcessInfo aDest, bool aIsCompactClone)
        {

            aDest.DaemonID = aSource.DaemonID;
            aDest.ChannelName = aSource.ChannelName;
            aDest.ConnectUsercount = aSource.m_ConnectUsercount;
            aDest.TelnetSessionCount = aSource.TelnetSessionCount;
            aDest.IP = aSource.IP;
            aDest.Port = aSource.Port;
            aDest.LifeTime = aSource.LifeTime;
        }


        /// <summary>
        /// Temp Session Count 입니다.
        /// (KwonTaeSuk, 2018.12, 설명 추가)
        /// - 서버가 할당했으나 데몬의 실제 세션은 아직 아닌 상태
        /// - 다음 데몬상태 업데이트시 TelnetSessionCount에 포함되고 TempSessionCount는 0으로 리셋된다.
        /// </summary>
        private int m_TempSessionCount = 0;
        /// <summary>
        /// Temp Session Count 속성을 가져오거나 설정합니다.
        /// </summary>
        public int TempSessionCount
        {
            get { return m_TempSessionCount; }
            set { m_TempSessionCount = value; }
        }	

        /// <summary>
        /// 클라이언트 라이프타임 속성을 가져오거나 설정합니다.
        /// </summary>
        public DateTime LifeTime
        {
            get { return m_LifeTime; }
            set { m_LifeTime = value; }
        }
        /// <summary>
        /// Channel Name 가져오거나 설정 합니다.
        /// </summary>
        public string ChannelName
        {
            get { return m_ChannelName; }
            set { m_ChannelName = value; }
        }
        /// <summary>
        /// Port 가져오거나 설정 합니다.
        /// </summary>
        public int Port
        {
            get { return m_Port; }
            set { m_Port = value; }
        }

        /// <summary>
        /// IP 가져오거나 설정 합니다.
        /// </summary>
        public string IP
        {
            get { return m_IP; }
            set { m_IP = value; }
        }

        /// <summary>
        /// 데이터 큐 속성을 가져오거나 설정합니다.
        /// </summary>
        public Queue DataQueue
        {
            get { return m_DataQueue; }
            set { m_DataQueue = value; }
        }
        /// <summary>
        /// Daemon ID 가져오기 합니다.
        /// </summary>
        public int DaemonID
        {
            get { return m_DaemonID; }
            set { m_DaemonID = value; }
        }

        /// <summary>
        /// 연결된 Telnet Session 수 가져오거나 설정 합니다.
        /// </summary>
        public int TelnetSessionCount
        {
            get { return m_TelnetSessionCount; }
            set { m_TelnetSessionCount = value; }
        }


        /// <summary>
        /// 접속된 사용자 수 가져오거나 설정 합니다.
        /// </summary>
        public int ConnectUsercount
        {
            get { return m_ConnectUsercount; }
            set { m_ConnectUsercount = value; }
        }
    }

    /// <summary>
    /// DaemonProcessInfo 목록 클래스 입니다.             
    /// </summary>
    [Serializable]
    public class DaemonProcessInfoCollection : GenericListMarshalByRef<DaemonProcessInfo>, ICloneableEx<DaemonProcessInfoCollection>
    {
        /// <summary>
        ///  기본 생성자 입니다.
        /// </summary>
        public DaemonProcessInfoCollection()
        {
        }
        /// <summary>
        /// 복사 생성자 입니다.
        /// </summary>
        /// <param name="aDeviceInfoCollection"></param>
        public DaemonProcessInfoCollection(DaemonProcessInfoCollection aDaemonProcessInfoCollection)
        {
            CopyTo(aDaemonProcessInfoCollection, this, false);
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
        public DaemonProcessInfoCollection CompactClone()
        {
            DaemonProcessInfoCollection tCollection = new DaemonProcessInfoCollection();
            CopyTo(this, tCollection, true);
            return tCollection;
        }
        /// <summary>
        /// ICloneable Interface Overload 깊은복사(Deep Copy)를 수행한 객체 복사본을 리턴합니다.
        /// </summary>
        /// <returns></returns>
        public DaemonProcessInfoCollection DeepClone()
        {
            DaemonProcessInfoCollection tCollection = new DaemonProcessInfoCollection();
            CopyTo(this, tCollection, false);
            return tCollection;
        }
        /// <summary>
        /// 맴버 대입을 처리합니다.
        /// </summary>
        private void CopyTo(DaemonProcessInfoCollection aSource, DaemonProcessInfoCollection aDest, bool aIsCompactClone)
        {
            if (aDest != null && aDest.Count > 0)
                aDest.Clear();

            foreach (DaemonProcessInfo tDeviceInfo in aSource)
            {
                if (aIsCompactClone)
                    aDest.Add((DaemonProcessInfo)tDeviceInfo.CompactClone());
                else
                    aDest.Add((DaemonProcessInfo)tDeviceInfo.DeepClone());
            }
        }

        /// <summary>
        /// 해당 ID의 요소를 가져오거나 설정합니다.
        /// </summary>
        /// <param name="aID">가져오거나 설정할 요소 ID입니다.</param>
        /// <returns></returns>
        public override DaemonProcessInfo this[int aID]
        {
            get
            {
                DaemonProcessInfo tDeviceInfo = null;
                lock (base.InnerList.SyncRoot)
                {
                    foreach (DaemonProcessInfo tmpDeviceInfo in base.InnerList)
                    {
                        if (tmpDeviceInfo.DaemonID == aID)
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
                DaemonProcessInfo tDeviceInfo = null;
                lock (base.InnerList.SyncRoot)
                {
                    for (int idx = 0; idx < base.InnerList.Count; idx++)
                    {
                        tDeviceInfo = base.InnerList[idx] as DaemonProcessInfo;
                        if (tDeviceInfo.DaemonID == aID)
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
                foreach (DaemonProcessInfo tDeviceInfo in base.InnerList)
                {
                    if (tDeviceInfo.DaemonID == aID)
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
