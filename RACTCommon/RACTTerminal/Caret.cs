using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace RACTTerminal
{
    public class Caret
    {
        /// <summary>
        /// 위치 입니다.
        /// </summary>
        public Point Pos = new Point(0,0);
        /// <summary>
        /// 색상입니다.
        /// </summary>
        private Color m_Color = Color.FromArgb(255, 181, 106);
        /// <summary>
        /// Bitmap 입니다.
        /// </summary>
        private Bitmap m_Bitmap = null;
        /// <summary>
        /// 그래픽 객체 입니다.
        /// </summary>
        private Graphics m_Buffer = null;
        /// <summary>
        /// Off 여부 입니다.
        /// </summary>
        private Boolean m_IsOff = false;
        /// <summary>
        /// EOL 여부 입니다.
        /// </summary>
        private Boolean m_EOL = false;

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public Caret(){}

        /// <summary>
        /// Color를 가져오거나 설정 합니다.
        /// </summary>
        public Color Color
        {
            get { return m_Color; }
            set { m_Color = value; }
        }
        /// <summary>
        /// Bitmap을 가져오거나 설정 합니다.
        /// </summary>
        public Bitmap Bitmap
        {
            get { return m_Bitmap; }
            set { m_Bitmap = value; }
        }
        /// <summary>
        /// Graphics Buffer를 가져오거나 설정 합니다.
        /// </summary>
        public Graphics Buffer
        {
            get { return m_Buffer; }
            set { m_Buffer = value; }
        }
        public Boolean IsOff
        {
            get { return m_IsOff; }
            set { m_IsOff = value; }
        }
        /// <summary>
        /// EOL을 가져 오거나 설정 합니다.
        /// </summary>
        public Boolean EOL
        {
            get { return m_EOL; }
            set { m_EOL = value; }
        }
    }
}
