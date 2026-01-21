using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace RACTCommonClass
{







    /// <summary>
    /// 압축 데이터 클래스 입니다.
    /// </summary>
    [Serializable]
    public class CompressData : IDisposable
    {
        /// <summary>
        /// 압축 전 크기를 저장 합니다.
        /// </summary>
        int m_OriginalSize = 0;
        /// <summary>
        /// 압축 된 메모리 스트림 입니다.
        /// </summary>
        Stream m_Stream = null;

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public CompressData(int aOriginalSize, Stream aStream)
        {
            m_OriginalSize = aOriginalSize;
            m_Stream = aStream;
        }

        public void Dispose()
        {
            m_Stream = null;
        }

        /// <summary>
        /// 실제 크기를 반환 합니다.
        /// </summary>
        public int OriginalSize
        {
            get { return m_OriginalSize; }
        }

        /// <summary>
        /// 메모리 스트림을 반환 합니다.
        /// </summary>
        public Stream Stream
        {
            get { return m_Stream; }
        }
    }   
}
