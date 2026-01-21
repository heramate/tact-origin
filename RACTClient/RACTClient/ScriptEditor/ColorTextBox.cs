using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace RACTClient
{
    public partial class ColorTextBox : RichTextBox
    {
        /// <summary>
        /// 색상 입니다.
        /// </summary>
        private SyntaxColor[] m_SyntaxColors;

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public ColorTextBox()
        {
            InitializeComponent();

            SyntaxColors = new SyntaxColor[] 
          {
            new SyntaxColor("[Comment]", Color.Green),
            new SyntaxColor("[String]", Color.Purple),
            new SyntaxColor("using", Color.Blue),
            new SyntaxColor("new", Color.Blue),
            new SyntaxColor("typeof", Color.Blue),
            new SyntaxColor("int", Color.Blue),
            new SyntaxColor("float", Color.Blue),
            new SyntaxColor("double", Color.Blue),
            new SyntaxColor("string", Color.Blue),
            new SyntaxColor("for", Color.Blue),
            new SyntaxColor("foreach", Color.Blue),
            new SyntaxColor("if", Color.Blue),
            new SyntaxColor("break", Color.Blue),
            new SyntaxColor("continue", Color.Blue),
            new SyntaxColor("try", Color.Blue),
            new SyntaxColor("return", Color.Blue),
            new SyntaxColor("catch", Color.Blue),
            new SyntaxColor("throw", Color.Blue),
            new SyntaxColor("void", Color.Blue),
            new SyntaxColor("var", Color.Blue),
            new SyntaxColor("as", Color.Blue),
            new SyntaxColor("is", Color.Blue),
            new SyntaxColor("in", Color.Blue),
            new SyntaxColor("this", Color.Blue),
            new SyntaxColor("true", Color.Blue),
            new SyntaxColor("false", Color.Blue),
                new SyntaxColor("For", Color.Blue),
                new SyntaxColor("Next", Color.Blue),
                new SyntaxColor("To", Color.Blue),
                new SyntaxColor("Sub", Color.Blue),
                new SyntaxColor("End", Color.Blue),

          };
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                string[] tSplit = value.Split(

                base.Text = value;
            }
        }

        /// <summary>
        /// Syntax Color 구조체 입니다.
        /// </summary>
        public struct SyntaxColor
        {
            /// <summary>
            /// 문자 입니다.
            /// </summary>
            private String m_SyntaxString;
            /// <summary>
            /// 색상 입니다.
            /// </summary>
            private Color m_Color;

            /// <summary>
            /// 문자열 변환 합니다.
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return SyntaxString + " " + Color.ToString(); // GetType().Name;
            }
            /// <summary>
            /// 기본 생성자 입니다.
            /// </summary>
            /// <param name="aString"></param>
            /// <param name="aColor"></param>
            public SyntaxColor(String aString, Color aColor)
            {
                this.m_SyntaxString = aString;
                this.m_Color = aColor;
            }

            public String SyntaxString
            {
                get { return m_SyntaxString; }
                set { m_SyntaxString = value; }
            }
            public Color Color
            {
                get { return m_Color; }
                set { m_Color = value; }
            }
        };
    }
}
