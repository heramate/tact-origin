using System;
using System.Collections.Generic;
using System.Text;

namespace RACTTerminal
{
    public class CaretAttribs
    {
        public System.Drawing.Point Pos;
        public Chars.Sets G0Set;
        public Chars.Sets G1Set;
        public Chars.Sets G2Set;
        public Chars.Sets G3Set;
        public CharAttribStruct Attribs;

        public CaretAttribs(
            System.Drawing.Point p1,
            Chars.Sets p2,
            Chars.Sets p3,
            Chars.Sets p4,
            Chars.Sets p5,
            CharAttribStruct p6)
        {
            this.Pos = p1;
            this.G0Set = p2;
            this.G1Set = p3;
            this.G2Set = p4;
            this.G3Set = p5;
            this.Attribs = p6;
        }
    }
}
