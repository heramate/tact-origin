using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace RACTTerminal
{

    /// <summary>
    /// 문자 속성 구조체 입니다.
    /// </summary>
    public struct CharAttribStruct
    {
        public Boolean IsBold;
        public Boolean IsDim;
        public Boolean IsUnderscored;
        public Boolean IsBlinking;
        public Boolean IsInverse;
        public Boolean IsPrimaryFont;
        public Boolean IsAlternateFont;
        public Boolean UseAltColor;
        public Color AltColor;
        public Boolean UseAltBGColor;
        public Color AltBGColor;
        public Chars GL;
        public Chars GR;
        public Chars GS;
        public Boolean IsDECSG;

        public CharAttribStruct(
            Boolean p1,
            Boolean p2,
            Boolean p3,
            Boolean p4,
            Boolean p5,
            Boolean p6,
            Boolean p7,
            Boolean p12,
            Color p13,
            Boolean p14,
            Color p15,
            Chars p16,
            Chars p17,
            Chars p18,
            Boolean p19)
        {
            IsBold = p1;
            IsDim = p2;
            IsUnderscored = p3;
            IsBlinking = p4;
            IsInverse = p5;
            IsPrimaryFont = p6;
            IsAlternateFont = p7;
            UseAltColor = p12;
            AltColor = p13;
            UseAltBGColor = p14;
            AltBGColor = p15;
            GL = p16;
            GR = p17;
            GS = p18;
            IsDECSG = p19;
        }

    }    
}
