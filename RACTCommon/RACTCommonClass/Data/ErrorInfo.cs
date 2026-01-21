using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace RACTCommonClass
{
    #region ErrorInfo 클래스입니다.
    /// <summary>
    ///  오류 정보 클래스입니다.
    /// </summary>
    [Serializable]
    [DefaultProperty("Error")]
    public class ErrorInfo 
    {
        #region [basic generate part :: Create, ICloneable]
        /// <summary>
        /// 기본 생성자입니다. 
        /// </summary>
        public ErrorInfo()
        {
        }
        /// <summary>
        /// 복사 생성자입니다. 		
        /// </summary>
        /// <param name="aErrorInfo"></param>
        public ErrorInfo(ErrorInfo aErrorInfo)
        {
            CopyTo(aErrorInfo, this, false);
        }
        /// <summary>
        /// 확장 생성자입니다.
        /// </summary>
        /// <param name="aErrorType"></param>
        /// <param name="aErrorString"></param>
        public ErrorInfo(E_ErrorType aErrorType, string aErrorString)
        {
            m_Error = aErrorType;
            m_ErrorString = aErrorString;
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
        public ErrorInfo CompactClone()
        {
            ErrorInfo tErrorInfo = new ErrorInfo();
            CopyTo(this, tErrorInfo, true);
            return tErrorInfo;
        }
        /// <summary>
        /// ICloneable Interface Overload 깊은복사(Deep Copy)를 수행한 객체 복사본을 리턴합니다.
        /// </summary>
        /// <returns></returns>
        public ErrorInfo DeepClone()
        {
            ErrorInfo tErrorInfo = new ErrorInfo();
            CopyTo(this, tErrorInfo, false);
            return tErrorInfo;
        }
        /// <summary>
        /// 개체 복사를 처리합니다.
        /// </summary>
        /// <param name="aSource">원본 개체입니다.</param>
        /// <param name="aDest">대상 개체입니다.</param>
        /// <param name="aIsCompactClone">Compact 복사 여부입니다.</param>
        private void CopyTo(ErrorInfo aSource, ErrorInfo aDest, bool aIsCompactClone)
        {
            aDest.Error = aSource.Error;
            aDest.ErrorString = aSource.ErrorString;
            aDest.SourceErrorString = aSource.SourceErrorString;
        }
        #endregion //[basic generate part :: Create, ICloneable]

        #region [property part]

        /// <summary>
        /// 오류 구분 입니다.
        /// </summary>
        private E_ErrorType m_Error = E_ErrorType.NoError;
        /// <summary>
        /// 오류 구분 속성을 가져오거나 설정합니다.
        /// </summary>
        public E_ErrorType Error
        {
            get { return m_Error; }
            set { m_Error = value; }
        }

        /// <summary>
        /// 오류 문자열 입니다.
        /// </summary>
        private string m_ErrorString = string.Empty;
        /// <summary>
        /// 오류 문자열 속성을 가져오거나 설정합니다.
        /// </summary>
        public string ErrorString
        {
            get { return m_ErrorString; }
            set { m_ErrorString = value; }
        }

        /// <summary>
        /// 시스템에서 제공하는 오류 문자열 입니다.
        /// </summary>
        private string m_SourceErrorString = string.Empty;
        /// <summary>
        /// 시스템에서 제공하는 오류 문자열 속성을 가져오거나 설정합니다.
        /// </summary>
        public string SourceErrorString
        {
            get { return m_SourceErrorString; }
            set { m_SourceErrorString = value; }
        }

        #endregion //[property part]
    }
    #endregion //ErrorInfo 클래스입니다.
}
