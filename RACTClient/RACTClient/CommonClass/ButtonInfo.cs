using System;
using System.Collections.Generic;
using System.Text;
using MKLibrary.Controls;

namespace RACTClient
{
    /// <summary>
    /// 버튼 정보 입니다.
    /// </summary>
    public class ButtonInfo
    {
        /// <summary>
        /// 버튼 크기 입니다.
        /// </summary>
        public const int s_ButtonHeight = 23;
        /// <summary>
        /// 버튼 넓이 입니다.
        /// </summary>
        public const int s_ButtonWidth = 75;
        /// <summary>
        /// 버튼 타입 입니다.
        /// </summary>
        private E_ButtonType m_Type = E_ButtonType.Cancel;
        /// <summary>
        /// 버튼 위치 입니다.
        /// </summary>
        private E_ButtonSide m_Side = E_ButtonSide.Left;
        /// <summary>
        /// 버튼 입니다.
        /// </summary>
        private MKButton m_Button = new MKButton();

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        /// <param name="aType"></param>
        /// <param name="aSide"></param>
        /// <param name="aText"></param>
        public ButtonInfo(E_ButtonType aType, E_ButtonSide aSide, string aText)
        {
            m_Type = aType;
            m_Side = aSide;
            m_Button.Text= aText;
            m_Button.Tag = aType;

            m_Button.Size = new System.Drawing.Size(s_ButtonWidth, s_ButtonHeight);
            m_Button.TextAlignment = MKLibrary.MKDrawing.E_Alignment.MiddleCenter;
        }
        /// <summary>
        /// 버튼 타입을 가져오거나 설정 합니다.
        /// </summary>
        public E_ButtonType Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }
        /// <summary>
        /// 버튼 위치를 가져오거나 설정 합니다.
        /// </summary>
        public E_ButtonSide Side
        {
            get { return m_Side; }
            set { m_Side = value; }
        }
        /// <summary>
        /// 버튼을 가져오거나 설정 합니다.
        /// </summary>
        public MKButton Button
        {
            get { return m_Button; }
            set { m_Button = value; }
        }
    }
}
