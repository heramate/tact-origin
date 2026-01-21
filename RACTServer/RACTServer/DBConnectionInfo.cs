using System;
using System.Collections.Generic;
using System.Text;

namespace RACTServer
{
    /// <summary>
    ///  DB 연결 정보 클래스입니다.
    /// </summary>
    [Serializable]
    public class DBConnectionInfo 
    {
        /// <summary>
        /// 기본 생성자입니다. 
        /// </summary>
        public DBConnectionInfo()
        {
        }
        /// <summary>
        /// 복사 생성자입니다. 		
        /// </summary>
        /// <param name="aDBConnectionInfo"></param>
        public DBConnectionInfo(DBConnectionInfo aDBConnectionInfo)
        {
            CopyTo(aDBConnectionInfo, this, false);
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
        public DBConnectionInfo CompactClone()
        {
            DBConnectionInfo tDBConnectionInfo = new DBConnectionInfo();
            CopyTo(this, tDBConnectionInfo, true);
            return tDBConnectionInfo;
        }
        /// <summary>
        /// ICloneable Interface Overload 깊은복사(Deep Copy)를 수행한 객체 복사본을 리턴합니다.
        /// </summary>
        /// <returns></returns>
        public DBConnectionInfo DeepClone()
        {
            DBConnectionInfo tDBConnectionInfo = new DBConnectionInfo();
            CopyTo(this, tDBConnectionInfo, false);
            return tDBConnectionInfo;
        }
        /// <summary>
        /// 개체 복사를 처리합니다.
        /// </summary>
        /// <param name="aSource">원본 개체입니다.</param>
        /// <param name="aDest">대상 개체입니다.</param>
        /// <param name="aIsCompactClone">Compact 복사 여부입니다.</param>
        private void CopyTo(DBConnectionInfo aSource, DBConnectionInfo aDest, bool aIsCompactClone)
        {
            aDest.DBServerIP = aSource.DBServerIP;
            aDest.DBName = aSource.DBName;
            aDest.UserID = aSource.UserID;
            aDest.Password = aSource.Password;
            aDest.DBConnectionCount = aSource.DBConnectionCount;
        }

        /// <summary>
        /// DB Server IPAddress 입니다.
        /// </summary>
        private string m_DBServerIP = string.Empty;
        /// <summary>
        /// DB Server IPAddress 속성을 가져오거나 설정합니다.
        /// </summary>
        public string DBServerIP
        {
            get { return m_DBServerIP; }
            set { m_DBServerIP = value; }
        }

        /// <summary>
        /// DB 명 입니다.
        /// </summary>
        private string m_DBName = string.Empty;
        /// <summary>
        /// DB 명 속성을 가져오거나 설정합니다.
        /// </summary>
        public string DBName
        {
            get { return m_DBName; }
            set { m_DBName = value; }
        }

        /// <summary>
        /// DB 사용자 계정 입니다.
        /// </summary>
        private string m_UserID = string.Empty;
        /// <summary>
        /// DB 사용자 계정 속성을 가져오거나 설정합니다.
        /// </summary>
        public string UserID
        {
            get { return m_UserID; }
            set { m_UserID = value; }
        }

        /// <summary>
        /// DB 사용자 패스워드 입니다.
        /// </summary>
        private string m_Password = string.Empty;
        /// <summary>
        /// DB 사용자 패스워드 속성을 가져오거나 설정합니다.
        /// </summary>
        public string Password
        {
            get { return m_Password; }
            set { m_Password = value; }
        }

        /// <summary>
        /// DB 연결 개수 입니다.
        /// </summary>
        private int m_DBConnectionCount = 3;
        /// <summary>
        /// DB 연결 개수 속성을 가져오거나 설정합니다.
        /// </summary>
        public int DBConnectionCount
        {
            get { return m_DBConnectionCount; }
            set { m_DBConnectionCount = value; }
        }

    }
}
