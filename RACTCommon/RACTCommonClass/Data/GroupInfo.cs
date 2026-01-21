using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using ACPS.CommonConfigCompareClass;

namespace RACTCommonClass
{
    [Serializable]
    public class GroupInfo
    {
        // shinyn - 2012-12-13 - NE Group ID int -> string 수정 'B' PON(Biz) -> FOMs연동 값에 따른 수정
        /// <summary>
        /// ID 입니다.
        /// </summary>
        private string m_ID;
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
        /// TOP GROUP ID 입니다.
        /// </summary>
        private string m_TOP_ID = string.Empty;

        /// <summary>
        /// UP GROUP ID 입니다.
        /// </summary>
        private string m_UP_ID = string.Empty;

        /// <summary>
        /// GROUP LEVEL 입니다.
        /// </summary>
        private int m_LEVEL = 0;

        /// <summary>
        /// GROUP의 순서입니다.
        /// </summary>
        private int m_SEQ_ID = 0;


        /// <summary>
        /// 그룹의 장비 수입니다.
        /// </summary>
        private int m_DEVICE_COUNT = 0;

        /// <summary>
        /// 그룹에 속한 장비 목록 입니다.
        /// </summary>
        private DeviceInfoCollection m_DeviceList = new DeviceInfoCollection();

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public GroupInfo() { }

        /// <summary>
        /// 복사 생성자 입니다.
        /// </summary>
        public GroupInfo(GroupInfo aGroupInfo)
        {
            if (aGroupInfo == null) return;
            m_ID = aGroupInfo.ID;
            m_Name = aGroupInfo.Name;
            m_UserID = aGroupInfo.UserID;
            m_DeviceList = aGroupInfo.m_DeviceList;
            m_Description = aGroupInfo.Description;
            m_TOP_ID = aGroupInfo.TOP_ID;
            m_UP_ID = aGroupInfo.UP_ID;
            m_LEVEL = aGroupInfo.LEVEL;
            m_SEQ_ID = aGroupInfo.SEQ_ID;
            m_DEVICE_COUNT = aGroupInfo.DEVICE_COUNT;
        }

        /// <summary>
        /// 그룹에 속한 장비 목록 가져오거나 설정 합니다.
        /// </summary>
        public DeviceInfoCollection DeviceList
        {
            get { return m_DeviceList; }
            set { m_DeviceList = value; }
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

        // shinyn - 2012-12-13 - NE Group ID int -> string 수정 'B' PON(Biz) -> FOMs연동 값에 따른 수정
        /// <summary>
        /// ID 가져오거나 설정 합니다.
        /// </summary>
        public string ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        /// <summary>
        /// 2013-08-13- shinyn -  TOP GROUP ID 입니다.
        /// </summary>

        public string TOP_ID
        {
            get { return m_TOP_ID; }
            set { m_TOP_ID = value; }
        }

        /// <summary>
        ///2013-08-13- shinyn -  UP GROUP ID 입니다.
        /// </summary>

        public string UP_ID
        {
            get { return m_UP_ID; }
            set { m_UP_ID = value; }
        }

        /// <summary>
        /// 2013-08-13- shinyn - GROUP LEVEL 입니다.
        /// </summary>
        public int LEVEL
        {
            get { return m_LEVEL; }
            set { m_LEVEL = value; }
        }

        /// <summary>
        /// 2013-08-13- shinyn - GROUP의 순서입니다.
        /// </summary>
        /// 
        public int SEQ_ID
        {
            get { return m_SEQ_ID; }
            set { m_SEQ_ID = value; }
        }

        /// <summary>
        /// 2013-08-13- shinyn - GROUP의 장비수입니다.
        /// </summary>
        public int DEVICE_COUNT
        {
            get { return m_DEVICE_COUNT; }
            set { m_DEVICE_COUNT = value; }
        }
    }

    [Serializable]
    public class GroupInfoCollection
    {
        // shinyn - 2012-12-13 - NE Group ID int -> string 수정 'B' PON(Biz) -> FOMs연동 값에 따른 수정
        private Dictionary<string, GroupInfo> m_List = new Dictionary<string, GroupInfo>();


        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public GroupInfoCollection() { }

        // shinyn - 2012-12-13 - NE Group ID int -> string 수정 'B' PON(Biz) -> FOMs연동 값에 따른 수정
        public bool ContainsKey(string aGroupID)
        {
            return m_List.ContainsKey(aGroupID);
        }

        // shinyn - 2012-12-13 - NE Group ID int -> string 수정 'B' PON(Biz) -> FOMs연동 값에 따른 수정
        public GroupInfo this[string aKey]
        {
            get { return m_List[aKey]; }
            set { m_List[aKey] = value; }
        }

        // shinyn - 2012-12-13 - NE Group ID int -> string 수정 'B' PON(Biz) -> FOMs연동 값에 따른 수정
        /// <summary>
        /// 추가 합니다.
        /// </summary>
        /// <param name="aKey"></param>
        /// <param name="aGroupInfo"></param>
        public void Add(string aKey, GroupInfo aGroupInfo)
        {
            m_List.Add(aKey, aGroupInfo);
        }

        public int Count
        {
            get { return m_List.Count; }
        }

        // shinyn - 2012-12-13 - NE Group ID int -> string 수정 'B' PON(Biz) -> FOMs연동 값에 따른 수정
        public int GetCountByGroup(string aKey)
        {
            return m_List[aKey].DeviceList.Count;
        }

        // shinyn - 2012-12-13 - NE Group ID int -> string 수정 'B' PON(Biz) -> FOMs연동 값에 따른 수정
        public Dictionary<string, GroupInfo> InnerList
        {
            get { return m_List; }
        }
        // shinyn - 2012-12-13 - NE Group ID int -> string 수정 'B' PON(Biz) -> FOMs연동 값에 따른 수정
        public void Remove(string aKey)
        {
            m_List.Remove(aKey);
        }
    }
}
