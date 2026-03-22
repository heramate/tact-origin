using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using MessagePack;

namespace RACTCommonClass
{
    #region ErrorInfo 클래스입니다.
    /// <summary>
    ///  오류 정보 클래스입니다.
    /// </summary>
    [Serializable]
    [DefaultProperty("Error")]
    [MessagePackObject]
    public class ErrorInfo 
    {
        #region [basic generate part :: Create, ICloneable]
        public ErrorInfo() { }
        public ErrorInfo(ErrorInfo a) { CopyTo(a, this, false); }
        public ErrorInfo(E_ErrorType type, string str) { Error = type; ErrorString = str; }
        public object Clone() { return this.MemberwiseClone(); }
        public ErrorInfo CompactClone() { ErrorInfo t = new ErrorInfo(); CopyTo(this, t, true); return t; }
        public ErrorInfo DeepClone() { ErrorInfo t = new ErrorInfo(); CopyTo(this, t, false); return t; }
        private void CopyTo(ErrorInfo aSource, ErrorInfo aDest, bool aIsCompactClone)
        {
            aDest.Error = aSource.Error;
            aDest.ErrorString = aSource.ErrorString;
            aDest.SourceErrorString = aSource.SourceErrorString;
        }
        #endregion

        [Key(0)] public E_ErrorType Error { get; set; } = E_ErrorType.NoError;
        [Key(1)] public string ErrorString { get; set; } = string.Empty;
        [Key(2)] public string SourceErrorString { get; set; } = string.Empty;
    }
    #endregion
}
