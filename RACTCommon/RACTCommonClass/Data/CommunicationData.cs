using System;
using System.Collections.Generic;
using System.Text;

namespace RACTCommonClass
{
    #region CommunicationData 클래스입니다.
    /// <summary>
    ///  통신 데이터 클래스입니다.
    /// </summary>
    [Serializable]
    public class CommunicationData
    {
        /// <summary>
        /// 기본 생성자입니다. 
        /// </summary>
        public CommunicationData()
        {
        }
        /// <summary>
        /// 복사 생성자입니다. 		
        /// </summary>
        /// <param name="aCommunicationData"></param>
        public CommunicationData(CommunicationData aCommunicationData)
        {
            CopyTo(aCommunicationData, this, false);
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
        public CommunicationData CompactClone()
        {
            CommunicationData tCommunicationData = new CommunicationData();
            CopyTo(this, tCommunicationData, true);
            return tCommunicationData;
        }
        /// <summary>
        /// ICloneable Interface Overload 깊은복사(Deep Copy)를 수행한 객체 복사본을 리턴합니다.
        /// </summary>
        /// <returns></returns>
        public CommunicationData DeepClone()
        {
            CommunicationData tCommunicationData = new CommunicationData();
            CopyTo(this, tCommunicationData, false);
            return tCommunicationData;
        }
        /// <summary>
        /// 개체 복사를 처리합니다.
        /// </summary>
        /// <param name="aSource">원본 개체입니다.</param>
        /// <param name="aDest">대상 개체입니다.</param>
        /// <param name="aIsCompactClone">Compact 복사 여부입니다.</param>
        private void CopyTo(CommunicationData aSource, CommunicationData aDest, bool aIsCompactClone)
        {
            aDest.JobID = aSource.JobID;
            aDest.ClientID = aSource.ClientID;
            aDest.RequestTime = aSource.RequestTime;
            aDest.UserData = aSource.UserData;
            aDest.OwnerKey = aSource.OwnerKey;
            aDest.CommType = aSource.CommType;
        }
    #endregion //[basic generate part :: Create, ICloneable]

        #region [property part]

        /// <summary>
        /// 통신 타입 입니다.
        /// </summary>
        private E_CommunicationType m_CommType = E_CommunicationType.UnKnown;
        /// <summary>
        /// 통신 타입 속성을 가져오거나 설정합니다.
        /// </summary>
        public E_CommunicationType CommType
        {
            get { return m_CommType; }
            set { m_CommType = value; }
        }

        /// <summary>
        /// 작업에 대한 고유 ID 입니다.
        /// </summary>
        private long m_JobID = 0;
        /// <summary>
        /// 작업에 대한 고유 ID 속성을 가져오거나 설정합니다.
        /// </summary>
        public long JobID
        {
            get { return m_JobID; }
            set { m_JobID = value; }
        }

        /// <summary>
        /// 메시지의 발송 하거나, 송신 할 클라이언트의 고유 ID 입니다.
        /// </summary>
        private int m_ClientID = 0;
        /// <summary>
        /// 메시지의 발송 하거나, 송신 할 클라이언트의 고유 ID 속성을 가져오거나 설정합니다.
        /// </summary>
        public int ClientID
        {
            get { return m_ClientID; }
            set { m_ClientID = value; }
        }

        /// <summary>
        /// 메시지가 생성된 시간 입니다.
        /// </summary>
        private DateTime m_RequestTime = DateTime.Now;
        /// <summary>
        /// 메시지가 생성된 시간 속성을 가져오거나 설정합니다.
        /// </summary>
        public DateTime RequestTime
        {
            get { return m_RequestTime; }
            set { m_RequestTime = value; }
        }

        /// <summary>
        /// 사용자가 임의로 저장한 데이터 입니다.
        /// </summary>
        private object m_UserData = null;
        /// <summary>
        /// 사용자가 임의로 저장한 데이터 속성을 가져오거나 설정합니다.
        /// </summary>
        public object UserData
        {
            get { return m_UserData; }
            set { m_UserData = value; }
        }

        /// <summary>
        /// 메시지의 주인의 키 입니다.
        /// </summary>
        private int m_OwnerKey = 0;
        /// <summary>
        /// 메시지의 주인의 키 속성을 가져오거나 설정합니다.
        /// </summary>
        public int OwnerKey
        {
            get { return m_OwnerKey; }
            set { m_OwnerKey = value; }
        }


        /// <summary>
        /// 통신 해더 정보를 복사 합니다.
        /// </summary>
        /// <param name="aCommunicationData"></param>
        public void SetCommunicationInfo(CommunicationData aCommunicationData)
        {
            CopyTo(aCommunicationData, this, false);
        }

        /// <summary>
        /// Gunny 2015 TACT 고도화
        /// 제한 명령어 True / False 값 입니다.
        /// </summary>
        private bool m_IsLimitCmd = false;

        /// <summary>
        /// 제한 명령어 True / False 값 가져오거나 설정 합니다.
        /// </summary>
        public bool IsLimitCmd
        {
            get { return m_IsLimitCmd; }
            set { m_IsLimitCmd = value; }
        }
    }

    #region RequestCommunicationData 클래스입니다.
    /// <summary>
    ///  요청 데이터 클래스입니다.
    /// </summary>
    [Serializable]
    public class RequestCommunicationData : CommunicationData
    {
        /// <summary>
        /// 기본 생성자입니다. 
        /// </summary>
        public RequestCommunicationData()
        {
        }
        /// <summary>
        /// 복사 생성자입니다. 		
        /// </summary>
        /// <param name="aRequestCommunicationData"></param>
        public RequestCommunicationData(RequestCommunicationData aRequestCommunicationData)
        {
            CopyTo(aRequestCommunicationData, this, false);
        }
        /// <summary>
        /// 복사 생성자 입니다.
        /// </summary>
        /// <param name="aCommunicationData"></param>
        public RequestCommunicationData(CommunicationData aCommunicationData) : base(aCommunicationData) { }

        ///<summary>
        /// ICloneable interface 구현 얖은복사(Shallow Copy)를 수행한 객체 복사본을 리턴합니다.
        ///</summary>
        public new object Clone()
        {
            return this.MemberwiseClone();
        }
        /// <summary>
        /// 객체의 Compact한 복사본을 리턴합니다. 참조 Object는 null로 대체한 객체를 반환하는 것으로
        /// 리모팅 통신시 필요없는 깊은복사 대상 Object를 Null로 대체해 Compact시킵니다.
        /// </summary>
        /// <returns></returns>
        public new RequestCommunicationData CompactClone()
        {
            RequestCommunicationData tRequestCommunicationData = new RequestCommunicationData((CommunicationData)this);
            CopyTo(this, tRequestCommunicationData, true);
            return tRequestCommunicationData;
        }
        /// <summary>
        /// ICloneable Interface Overload 깊은복사(Deep Copy)를 수행한 객체 복사본을 리턴합니다.
        /// </summary>
        /// <returns></returns>
        public new RequestCommunicationData DeepClone()
        {
            RequestCommunicationData tRequestCommunicationData = new RequestCommunicationData((CommunicationData)this);
            CopyTo(this, tRequestCommunicationData, false);
            return tRequestCommunicationData;
        }
        /// <summary>
        /// 개체 복사를 처리합니다.
        /// </summary>
        /// <param name="aSource">원본 개체입니다.</param>
        /// <param name="aDest">대상 개체입니다.</param>
        /// <param name="aIsCompactClone">Compact 복사 여부입니다.</param>
        private void CopyTo(RequestCommunicationData aSource, RequestCommunicationData aDest, bool aIsCompactClone)
        {
            aDest.RequestData = aSource.RequestData;
        }

        /// <summary>
        /// 요청 데이터 입니다.
        /// </summary>
        private object m_RequestData = null;
        /// <summary>
        /// 요청 데이터 속성을 가져오거나 설정합니다.
        /// </summary>
        public object RequestData
        {
            get { return m_RequestData; }
            set { m_RequestData = value; }
        }

    }

    /// <summary>
    ///  결과 데이터 클래스입니다.
    /// </summary>
    [Serializable]
    public class ResultCommunicationData : CommunicationData
    {
        /// <summary>
        /// 기본 생성자입니다. 
        /// </summary>
        public ResultCommunicationData()
        {
        }
        /// <summary>
        /// 복사 생성자입니다. 		
        /// </summary>
        /// <param name="aResultCommunicationData"></param>
        public ResultCommunicationData(ResultCommunicationData aResultCommunicationData)
        {
            CopyTo(aResultCommunicationData, this, false);
        }
        /// <summary>
        /// 복사 생성자 입니다.
        /// </summary>
        /// <param name="aCommunicationData"></param>
        public ResultCommunicationData(CommunicationData aCommunicationData) : base(aCommunicationData) { }
        ///<summary>
        /// ICloneable interface 구현 얖은복사(Shallow Copy)를 수행한 객체 복사본을 리턴합니다.
        ///</summary>
        public new object Clone()
        {
            return this.MemberwiseClone();
        }
        /// <summary>
        /// 객체의 Compact한 복사본을 리턴합니다. 참조 Object는 null로 대체한 객체를 반환하는 것으로
        /// 리모팅 통신시 필요없는 깊은복사 대상 Object를 Null로 대체해 Compact시킵니다.
        /// </summary>
        /// <returns></returns>
        public new ResultCommunicationData CompactClone()
        {
            ResultCommunicationData tResultCommunicationData = new ResultCommunicationData((CommunicationData)this);
            CopyTo(this, tResultCommunicationData, true);
            return tResultCommunicationData;
        }
        /// <summary>
        /// ICloneable Interface Overload 깊은복사(Deep Copy)를 수행한 객체 복사본을 리턴합니다.
        /// </summary>
        /// <returns></returns>
        public new ResultCommunicationData DeepClone()
        {
            ResultCommunicationData tResultCommunicationData = new ResultCommunicationData((CommunicationData)this);
            CopyTo(this, tResultCommunicationData, false);
            return tResultCommunicationData;
        }
        /// <summary>
        /// 개체 복사를 처리합니다.
        /// </summary>
        /// <param name="aSource">원본 개체입니다.</param>
        /// <param name="aDest">대상 개체입니다.</param>
        /// <param name="aIsCompactClone">Compact 복사 여부입니다.</param>
        private void CopyTo(ResultCommunicationData aSource, ResultCommunicationData aDest, bool aIsCompactClone)
        {
            if (aIsCompactClone)
            {
                if (aSource.Error != null) aDest.Error = aSource.Error.CompactClone();
            }
            else
            {
                if (aSource.Error != null) aDest.Error = aSource.Error.DeepClone();
            }
            aDest.ResultData = aSource.ResultData;
            aDest.IsCompressed = aSource.IsCompressed;
            aDest.UserData = aSource.UserData;
        }

        /// <summary>
        /// 결과 데이터 입니다.
        /// </summary>
        private object m_ResultData = null;
        /// <summary>
        /// 결과 데이터 속성을 가져오거나 설정합니다.
        /// </summary>
        public object ResultData
        {
            get { return m_ResultData; }
            set { m_ResultData = value; }
        }

        /// <summary>
        /// 오류 정보 입니다.
        /// </summary>
        private ErrorInfo m_Error = new ErrorInfo();
        /// <summary>
        /// 오류 정보 속성을 가져오거나 설정합니다.
        /// </summary>
        public ErrorInfo Error
        {
            get { return m_Error; }
            set { m_Error = value; }
        }

        /// <summary>
        /// 압축 여부 입니다.
        /// </summary>
        private bool m_IsCompressed = false;
        /// <summary>
        /// 압축 여부 속성을 가져오거나 설정합니다.
        /// </summary>
        public bool IsCompressed
        {
            get { return m_IsCompressed; }
            set { m_IsCompressed = value; }
        }

        #endregion //[property part]
    }
    #endregion //ResultCommunicationData 클래스입니다.


    #region CommandItemBase 클래스입니다.
    /// <summary>
    ///  
    /// </summary>
    [Serializable]
    public class CommandItemBase 
    {
        #region [basic generate part :: Create, ICloneable]
        /// <summary>
        /// 기본 생성자입니다. 
        /// </summary>
        public CommandItemBase()
        {
        }
        /// <summary>
        /// 복사 생성자입니다. 		
        /// </summary>
        /// <param name="aCommandItemBase"></param>
        public CommandItemBase(CommandItemBase aCommandItemBase)
        {
            CopyTo(aCommandItemBase, this, false);
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
        public CommandItemBase CompactClone()
        {
            CommandItemBase tCommandItemBase = new CommandItemBase();
            CopyTo(this, tCommandItemBase, true);
            return tCommandItemBase;
        }
        /// <summary>
        /// ICloneable Interface Overload 깊은복사(Deep Copy)를 수행한 객체 복사본을 리턴합니다.
        /// </summary>
        /// <returns></returns>
        public CommandItemBase DeepClone()
        {
            CommandItemBase tCommandItemBase = new CommandItemBase();
            CopyTo(this, tCommandItemBase, false);
            return tCommandItemBase;
        }
        /// <summary>
        /// 개체 복사를 처리합니다.
        /// </summary>
        /// <param name="aSource">원본 개체입니다.</param>
        /// <param name="aDest">대상 개체입니다.</param>
        /// <param name="aIsCompactClone">Compact 복사 여부입니다.</param>
        private void CopyTo(CommandItemBase aSource, CommandItemBase aDest, bool aIsCompactClone)
        {
            aDest.JobID = aSource.JobID;
            aDest.CommandResultType = aSource.CommandResultType;
            aDest.ClientID = aSource.ClientID;
            aDest.OwnerKey = aSource.OwnerKey;
        }
        #endregion //[basic generate part :: Create, ICloneable]

        #region [property part]

        /// <summary>
        /// 작업의 고유 ID 입니다.
        /// </summary>
        private long m_JobID = 0;
        /// <summary>
        /// 작업의 고유 ID 속성을 가져오거나 설정합니다.
        /// </summary>
        public long JobID
        {
            get { return m_JobID; }
            set { m_JobID = value; }
        }


        /// <summary>
        /// 작업 결과 반환 타입 입니다.
        /// </summary>
        private E_CommandResultType m_CommandResultType = E_CommandResultType.CommandGroup;
        /// <summary>
        /// 작업 결과 반환 타입 속성을 가져오거나 설정합니다.
        /// </summary>
        public E_CommandResultType CommandResultType
        {
            get { return m_CommandResultType; }
            set { m_CommandResultType = value; }
        }

        /// <summary>
        /// 사용자의 클라이언트 ID 입니다.
        /// </summary>
        private int m_ClientID = 0;
        /// <summary>
        /// 사용자의 클라이언트 ID 속성을 가져오거나 설정합니다.
        /// </summary>
        public int ClientID
        {
            get { return m_ClientID; }
            set { m_ClientID = value; }
        }
        /// <summary>
        /// Owner Key 입니다.
        /// </summary>
        private int m_OwnerKey = 0;
        /// <summary>
        /// Owner Key 속성을 가져오거나 설정합니다.
        /// </summary>
        public int OwnerKey
        {
            get { return m_OwnerKey; }
            set { m_OwnerKey = value; }
        }

        #endregion //[property part]
    }
    #endregion //CommandItemBase 클래스입니다.

    #region CommandResultItem 클래스입니다.
    /// <summary>
    ///  명령 결과 아이템 클래스입니다.
    /// </summary>
    [Serializable]
    public class CommandResultItem : CommandItemBase
    {
        #region [basic generate part :: Create, ICloneable]
        /// <summary>
        /// 기본 생성자입니다. 
        /// </summary>
        public CommandResultItem()
        {
        }
        /// <summary>
        /// 복사 생성자입니다. 		
        /// </summary>
        /// <param name="aCommandResultItem"></param>
        public CommandResultItem(CommandResultItem aCommandResultItem)
        {
            CopyTo(aCommandResultItem, this, false);
        }
        /// <summary>
        /// 복사 생성자입니다. 		
        /// </summary>
        /// <param name="aCommandItem"></param>
        public CommandResultItem(CommandItemBase aCommandItem) : base(aCommandItem) { }
        /// <summary>
        /// 확장 생성자 입니다.
        /// </summary>
        /// <param name="aJobID"></param>
        /// <param name="aCommandResultType"></param>
        /// <param name="aCommandResult"></param>
        public CommandResultItem(long aJobID, E_CommandResultType aCommandResultType, object aCommandResult)
        {
            base.JobID = aJobID;
            base.CommandResultType = aCommandResultType;
            m_CommandResult = aCommandResult;
        }
        /// <summary>
        /// 확장 생성자 입니다.
        /// </summary>
        /// <param name="aJobID"></param>
        /// <param name="aBackupJobID"></param>
        /// <param name="aCommandResultType"></param>
        /// <param name="aCommandResult"></param>
        public CommandResultItem(long aJobID, long aBackupJobID, E_CommandResultType aCommandResultType, object aCommandResult)
        {
            base.JobID = aJobID;
            base.CommandResultType = aCommandResultType;
            m_CommandResult = aCommandResult;
        }
        ///<summary>
        /// ICloneable interface 구현 얖은복사(Shallow Copy)를 수행한 객체 복사본을 리턴합니다.
        ///</summary>
        public new object Clone()
        {
            return this.MemberwiseClone();
        }
        /// <summary>
        /// 객체의 Compact한 복사본을 리턴합니다. 참조 Object는 null로 대체한 객체를 반환하는 것으로
        /// 리모팅 통신시 필요없는 깊은복사 대상 Object를 Null로 대체해 Compact시킵니다.
        /// </summary>
        /// <returns></returns>
        public new CommandResultItem CompactClone()
        {
            CommandResultItem tCommandResultItem = new CommandResultItem((CommandItemBase)this);
            CopyTo(this, tCommandResultItem, true);
            return tCommandResultItem;
        }
        /// <summary>
        /// ICloneable Interface Overload 깊은복사(Deep Copy)를 수행한 객체 복사본을 리턴합니다.
        /// </summary>
        /// <returns></returns>
        public new CommandResultItem DeepClone()
        {
            CommandResultItem tCommandResultItem = new CommandResultItem((CommandItemBase)this);
            CopyTo(this, tCommandResultItem, false);
            return tCommandResultItem;
        }
        /// <summary>
        /// 개체 복사를 처리합니다.
        /// </summary>
        /// <param name="aSource">원본 개체입니다.</param>
        /// <param name="aDest">대상 개체입니다.</param>
        /// <param name="aIsCompactClone">Compact 복사 여부입니다.</param>
        private void CopyTo(CommandResultItem aSource, CommandResultItem aDest, bool aIsCompactClone)
        {
            aDest.CommandResult = aSource.CommandResult;
            aDest.UserID = aSource.UserID;
        }
        #endregion //[basic generate part :: Create, ICloneable]

        #region [property part]

     
        /// <summary>
        /// 작업 결과 입니다.
        /// </summary>
        private object m_CommandResult = null;
        /// <summary>
        /// 작업 결과 속성을 가져오거나 설정합니다.
        /// </summary>
        public object CommandResult
        {
            get { return m_CommandResult; }
            set { m_CommandResult = value; }
        }

     
        /// <summary>
        /// 작업자 ID 입니다.
        /// </summary>
        private int m_UserID = 0;
        /// <summary>
        /// 작업자 ID 속성을 가져오거나 설정합니다.
        /// </summary>
        public int UserID
        {
            get { return m_UserID; }
            set { m_UserID = value; }
        }

    
        #endregion //[property part]
    }
    #endregion //CommandResultItem 클래스입니다.
}
