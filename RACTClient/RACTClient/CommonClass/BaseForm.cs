using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RACTCommonClass;
using MKLibrary.Controls;
using DevComponents.DotNetBar;
using System.Threading;

namespace RACTClient
{
    public partial class BaseForm : SenderForm
    {
        /// <summary>
        /// 버튼 목록 입니다.
        /// </summary>
        private List<ButtonInfo> m_ButtonList = new List<ButtonInfo>();

        /// <summary>
        /// 기본 생성자 입니다.
        /// </summary>
        public BaseForm()
        {
            InitializeComponent(); 
        }
        /// <summary>
        /// 버튼을 추가 합니다.(먼저 Add한 객체가 먼저 표시됨)
        /// </summary>
        /// <param name="aButtonType">버튼 타입 입니다.</param>
        /// <param name="aButtonSide">버튼 위치 입니다.</param>
        /// <param name="aButtonText">버튼 텍스트 입니다.</param>
        public void AddButton(E_ButtonType aButtonType, E_ButtonSide aButtonSide, string aButtonText)
        {
            AddButton(new ButtonInfo(aButtonType, aButtonSide, aButtonText));
        }

        /// <summary>
        /// 버튼을 추가 합니다. (먼저 Add한 객체가 먼저 표시됨)
        /// </summary>
        /// <param name="aButtonInfo">버튼 정보 입니다.</param>
        public void AddButton(ButtonInfo aButtonInfo)
        {
            aButtonInfo.Button.Click += new EventHandler(Button_Click);
            AppGlobal.InitializeButtonStyle(aButtonInfo.Button);
            pnlButton.Controls.Add(aButtonInfo.Button);
            m_ButtonList.Add(aButtonInfo);
            InitializeButtonControl();
        }
        /// <summary>
        /// 버튼 클릭 이벤트 입니다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Button_Click(object sender, EventArgs e)
        {
            ButtonProcess((E_ButtonType)((MKButton)sender).Tag);
        }

        /// <summary>
        /// 화면의 Button을 클릭한 이벤트를 처리하는 가상 함수입니다.
        /// </summary>
        /// <param name="aButtonType">버튼 타입</param>
        protected virtual void ButtonProcess(E_ButtonType aButtonType) { }

       
        /// <summary>
        /// 버튼을 초기화 합니다.
        /// </summary>
        private void InitializeButtonControl()
        {
            this.SuspendLayout();
            int tTop = pnlButton.Height / 2 - ButtonInfo.s_ButtonHeight / 2;
            int tLeftLocation = 5;
            int tRightLocation = this.Width - 23;

            Graphics tGraphics = null;
            SizeF tFontSize =  SizeF.Empty;
            int tButtonGap = 5;
            foreach (ButtonInfo tButtonInfo in m_ButtonList)
            {
                tGraphics = tButtonInfo.Button.CreateGraphics();
                tFontSize = tGraphics.MeasureString(tButtonInfo.Button.Text, tButtonInfo.Button.Font);

                if (tFontSize.Width > ButtonInfo.s_ButtonWidth)
                {
                    tButtonInfo.Button.Width =(int) (tFontSize.Width - ButtonInfo.s_ButtonWidth) + 2;
                }

                
                switch (tButtonInfo.Side)
                {
                    case E_ButtonSide.Left:
                        tButtonInfo.Button.Location = new Point(tLeftLocation + tButtonGap, tTop);
                        tLeftLocation = tButtonInfo.Button.Location.X + tButtonInfo.Button.Width;
                        break;
                    case E_ButtonSide.Right:
                        tButtonInfo.Button.Location = new Point(tRightLocation - tButtonGap - tButtonInfo.Button.Width, tTop);
                        tRightLocation = tButtonInfo.Button.Location.X;
                        break;
                }

            }
            this.ResumeLayout(false);
        }


    }
}