using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using RACTCommonClass;

namespace RACTClient
{
    /// <summary>
    /// 모델 정렬을 위한 비교 클래스 입니다.
    /// </summary>
    public class ModelSortCompare : IComparer
    {
        /// <summary>
        /// 지정한 객체를 비교하여 비교 결과를 반환 합니다.
        /// </summary>
        /// <param name="x">비교 소스 입니다.</param>
        /// <param name="y">비교 대상 입니다.</param>
        /// <returns>비교 결과 입니다.</returns>
        public int Compare(object x, object y)
        {
            return ((ModelInfo)x).ModelName.CompareTo(((ModelInfo)y).ModelName);
        }
    }
}
