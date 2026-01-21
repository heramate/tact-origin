using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace RACTClient
{
    /// <summary>
    /// 스크롤 값 변경시 발생 합니다.
    /// </summary>
    /// <param name="aType"></param>
    /// <param name="aValue"></param>
    public delegate void ChangeScrollValue ( int aValue);
    /// <summary>
    /// 스크롤바 입니다.
    /// </summary>
    public class VertScrollBar : VScrollBar
    {
        public event ChangeScrollValue OnChangeScrollValue;
        public event DefaultHandler OnMoveMaxValue;
        public event DefaultHandler OnMoveMinValue;
        protected int valSmallChange = 10;
        protected int valLargeChange = 5;
        protected int valViewable = 1;
        protected int valMax = 1;
        private int m_OldValue = 0;
        private int m_NewValue = 0;

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public VertScrollBar()
        {
            this.SetStyle(ControlStyles.Selectable, false);
            this.Maximum = 0;
            this.Minimum = -1;
            this.Scroll += new ScrollEventHandler(VertScrollBar_Scroll);
            this.ValueChanged += new EventHandler(VertScrollBar_ValueChanged);
        }

        void VertScrollBar_ValueChanged(object sender, EventArgs e)
        {
            if (!this.Visible) return;
            //System.Diagnostics.Debug.WriteLine(string.Concat(" 값변경 old " ,this.m_OldValue , " value " , this.Value));
            int tChangeValue = 0;
          

            tChangeValue = Value - m_OldValue;

            if (OnChangeScrollValue != null) OnChangeScrollValue(tChangeValue);
            m_OldValue = Value;
            if (Value == Maximum)
            {
                IsChangeValue = false;   
                if (OnMoveMaxValue != null) OnMoveMaxValue();
            }

            if (Value == Minimum)
            {
                if (OnMoveMinValue != null) OnMoveMinValue();
            }
        }
        /// <summary>
        /// 스크롤을 지정한 라인으로 표시 합니다.
        /// </summary>
        /// <param name="aLine"></param>
        public void MoveLine(int aLine)
        {
            if (aLine < Minimum)
            {
                aLine = Minimum;
            }

            if (aLine > Maximum)
            {
                aLine = Maximum;
            }

            Value = aLine;
        }

        void VertScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            if (!this.Visible) return;
            m_IsChangeValue = true;
            m_OldValue = e.OldValue;
            m_NewValue = e.NewValue;
            m_IsChangeValue = true;
        }

        /// <summary>
        /// 스크롤 변경 여부 입니다.
        /// </summary>
        private bool m_IsChangeValue;

        /// <summary>
        /// 스크롤 변경 여부 가져오거나 설정 합니다.
        /// </summary>
        public bool IsChangeValue
        {
            get { return m_IsChangeValue; }
            set { m_IsChangeValue = value; }
        }

        public int OldValue
        {
            get { return m_OldValue; }
            set { m_OldValue = value; }
        }
        public int ChangeValue
        {
            get
            {
                if (OldValue > NewValue)
                {
                    return (OldValue - NewValue) * -1;
                }
                else
                {
                    return NewValue - OldValue;
                }
            }

        }
        public int NewValue
        {
            get { return m_NewValue; }
        }
		
		public int ValSmallChange
		{
			get {return valSmallChange;}
			set
			{
				valSmallChange=value;
				this.SmallChange=valSmallChange;
			}
		}

		public int ValLargeChange
		{
			get {return valLargeChange;}
			set {valLargeChange=value;}
		}

		public int ViewableCount
		{
			get {return valViewable;}
			set
			{
				valViewable=value;
				Recalc();
			}
		}

		public int MaximumCount
		{
			get {return valMax;}
			set
			{
				valMax=value;
				this.LargeChange=this.Height;
				Recalc();
			}
		}

		public void sb2_ValueChanged(object sender, System.EventArgs e)
		{
			//form.tbValue2.Text=this.Value.ToString();
		}

		public void sb2_Layout(object sender, System.Windows.Forms.LayoutEventArgs e)
		{
			
			this.LargeChange=this.Height;
			Recalc();
		}

		protected void Recalc()
		{
			this.Maximum=MaximumCount;
			this.LargeChange=ViewableCount;
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 8469)
			{
				switch((uint)m.WParam)
				{
					case 2:			// page up
						if (this.Value - this.ValLargeChange > 0)
						{
							this.Value-=this.ValLargeChange;
						}
						else
						{
							this.Value=0;
						}
						break;

					case 3:			// page down
						if (this.Value + this.LargeChange + this.ValLargeChange < this.Maximum)
						{
							this.Value+=this.ValLargeChange;
						}
						else
						{
							this.Value=this.Maximum - this.LargeChange;
						}
						break;

					default:
						base.WndProc (ref m);
						break;
				}
			}
			else
			{
				base.WndProc (ref m);
			}
		}

	}
    
}
