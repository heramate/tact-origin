using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace RACTClient
{
    public partial class ucDesignPanel : UserControl
    {
        public ucDesignPanel()
        {
            InitializeComponent();
            base.Font = new System.Drawing.Font("맑은 고딕", 9F);
        }

        private int m_BoarderSize = 1;
        /// <summary>
        /// 테두리 크기 속성을 가져오거나 설정합니다.
        /// </summary>
        public int BoarderSize
        {
            get { return m_BoarderSize; }
            set
            {
                m_BoarderSize = value;
                Invalidate();
            }
        }
        private bool m_IsResizable = false;
        /// <summary>
        /// 높이를 사용자가 설정 가능하지 여부 속성을 가져오거나 설정합니다.
        /// </summary>
        public bool IsResizable
        {
            get { return m_IsResizable; }
            set { m_IsResizable = value; }
        }

        private E_DesignType m_DesignType = E_DesignType.LabelArea;
        /// <summary>
        /// 디자인 타입 속성을 가져오거나 설정합니다.
        /// </summary>
        public E_DesignType DesignType
        {
            get { return m_DesignType; }
            set
            {
                m_DesignType = value;
                switch (m_DesignType)
                {
                    case E_DesignType.LabelArea:
                        //case E_DesignType.LabelArea2:
                        m_BorderColor = Color.FromArgb(158, 201, 210);
                        m_BackGroundColor = Color.FromArgb(214, 233, 240);
                        if (!IsResizable)
                        {
                            if (m_DesignType == E_DesignType.LabelArea)
                            {
                                this.Height = 34;
                            }
                            //else if (m_DesignType == E_DesignType.LabelArea2)
                            //{
                            //    this.Height = 34;
                            //}
                        }

                        break;
                    case E_DesignType.InputArea:
                        //case E_DesignType.InputArea2:
                        {
                            if (!IsResizable)
                            {
                                if (m_DesignType == E_DesignType.InputArea)
                                {
                                    this.Height = 34;
                                }
                                //else if (m_DesignType == E_DesignType.InputArea2)
                                //{
                                //    this.Height = 34;
                                //}
                            }
                            m_BorderColor = Color.FromArgb(158, 201, 210);
                            m_BackGroundColor = Color.FromArgb(231, 242, 246);
                            Text = "";
                        }
                        break;
                    case E_DesignType.LabelAreaAlarm:
                        m_BorderColor = Color.FromArgb(199, 199, 199);
                        m_BackGroundColor = Color.FromArgb(226, 226, 226);
                        if (!IsResizable)
                        {
                            this.Height = 34;
                        }

                        break;
                    case E_DesignType.InputAreaAlarm:
                        {
                            if (!IsResizable)
                            {
                                this.Height = 34;
                            }
                            m_BorderColor = Color.FromArgb(199, 199, 199);
                            m_BackGroundColor = Color.FromArgb(238, 238, 238);
                            Text = "";
                        }
                        break;
                    default:
                        break;
                }
                Invalidate();
            }
        }

        [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public new string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new HandlerArgument1<string>(SetText), new object[] { value });
                }
                else
                    SetText(value);
            }
        }
        private void SetText(string aText)
        {
            base.Text = aText;
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            if (!IsResizable)
            {
                if (m_DesignType == E_DesignType.InputArea || m_DesignType == E_DesignType.LabelArea)
                {
                    this.Height = 34;
                }
                //else if (m_DesignType == E_DesignType.InputArea2 || m_DesignType == E_DesignType.LabelArea2)
                //{
                //    this.Height = 34;
                //}
            }
        }

        private Color m_BorderColor = Color.FromArgb(158, 201, 210);
        private Color m_BackGroundColor = Color.FromArgb(214, 233, 240);
        private Color m_ForeColor = Color.Black;

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            SolidBrush tBrushBackGround = new SolidBrush(m_BackGroundColor);
            e.Graphics.FillRectangle(tBrushBackGround, ClientRectangle);
            tBrushBackGround.Dispose();

            Pen tPen = new Pen(m_BorderColor);
            tPen.Width = (float)m_BoarderSize;
            tPen.DashStyle = DashStyle.Solid;

            Rectangle tRect = this.ClientRectangle;
            tRect.Width -= 1;
            tRect.Height -= 1;

            e.Graphics.DrawRectangle(tPen, tRect);

            tPen.Dispose();

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            StringFormat tStringFormat = new StringFormat();
            tStringFormat.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Show;

            SolidBrush tBrush = new SolidBrush(m_ForeColor);

            RectangleF tRect = tRect = new RectangleF(this.ClientRectangle.X + 5, this.ClientRectangle.Y, this.ClientRectangle.Width, this.ClientRectangle.Height);

            //float tTxtHeight = (int)e.Graphics.MeasureString(m_Text, base.Font).Height;

            //tRect.Height += (tRect.Height - tTxtHeight) / 2;

            tStringFormat.Alignment = StringAlignment.Near;
            tStringFormat.LineAlignment = StringAlignment.Center;

            e.Graphics.DrawString(Text, base.Font, tBrush, tRect, tStringFormat);
            tStringFormat.Dispose();
            tBrush.Dispose();
        }
    }



    public enum E_DesignType
    {
        /// <summary>
        /// 레이블 영역(Height 가 34)
        /// </summary>
        LabelArea,
        /// <summary>
        /// 입력 컨트롤 영역(Height 가 34)
        /// </summary>
        InputArea,
        /// <summary>
        /// Alarm 레이블 영역(Height 가 34)
        /// </summary>
        LabelAreaAlarm,
        /// <summary>
        /// Alarm 입력 컨트롤 영역(Height 가 34)
        /// </summary>
        InputAreaAlarm,
    }
}
