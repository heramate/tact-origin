using System;
using System.Collections.Generic;
using System.Text;

namespace RACTTerminal
{
    public class Chars
    {
        public struct RACT_CharSet
        {
            public RACT_CharSet(System.Int32 p1, System.Int16 p2)
            {
                Location = p1;
                UnicodeNo = p2;
            }

            public System.Int32 Location;
            public System.Int16 UnicodeNo;
        }

        public Chars.Sets Set;

        public Chars(Chars.Sets p1)
        {
            this.Set = p1;
        }

        public static System.Char Get(System.Char CurChar, Chars.Sets GL, Chars.Sets GR)
        {
            RACT_CharSet[] CurSet;

            if (System.Convert.ToInt32(CurChar) < 128)
            {
                switch (GL)
                {
                    case Chars.Sets.ASCII:
                        CurSet = Chars.ASCII;
                        break;

                    case Chars.Sets.DECSG:
                        CurSet = Chars.DECSG;
                        break;

                    case Chars.Sets.NRCUK:
                        CurSet = Chars.NRCUK;
                        break;

                    case Chars.Sets.NRCFinnish:
                        CurSet = Chars.NRCFinnish;
                        break;

                    case Chars.Sets.NRCFrench:
                        CurSet = Chars.NRCFrench;
                        break;

                    case Chars.Sets.NRCFrenchCanadian:
                        CurSet = Chars.NRCFrenchCanadian;
                        break;

                    case Chars.Sets.NRCGerman:
                        CurSet = Chars.NRCGerman;
                        break;

                    case Chars.Sets.NRCItalian:
                        CurSet = Chars.NRCItalian;
                        break;

                    case Chars.Sets.NRCNorDanish:
                        CurSet = Chars.NRCNorDanish;
                        break;

                    case Chars.Sets.NRCPortuguese:
                        CurSet = Chars.NRCPortuguese;
                        break;

                    case Chars.Sets.NRCSpanish:
                        CurSet = Chars.NRCSpanish;
                        break;

                    case Chars.Sets.NRCSwedish:
                        CurSet = Chars.NRCSwedish;
                        break;

                    case Chars.Sets.NRCSwiss:
                        CurSet = Chars.NRCSwiss;
                        break;

                    default:
                        CurSet = Chars.ASCII;
                        break;
                }
            }
            else
            {
                switch (GR)
                {
                    case Chars.Sets.ISOLatin1S:
                        CurSet = Chars.ISOLatin1S;
                        break;

                    case Chars.Sets.DECS:
                        CurSet = Chars.DECS;
                        break;

                    default:
                        CurSet = Chars.DECS;
                        break;
                }
            }

            for (System.Int32 i = 0; i < CurSet.Length; i++)
            {
                if (CurSet[i].Location == System.Convert.ToInt32(CurChar))
                {
                    System.Byte[] CurBytes = System.BitConverter.GetBytes(CurSet[i].UnicodeNo);
                    System.Char[] NewChars = System.Text.Encoding.Unicode.GetChars(CurBytes);

                    return NewChars[0];
                }

            }

            return CurChar;
        }

        public static RACT_CharSet[] DECSG =
		{
			new RACT_CharSet (0x5F, 0x0020), // Blank 
			//            new uc_CharSet (0x60, 0x25C6), // Filled Diamond 
			new RACT_CharSet (0x61, 0x0000), // Pi over upsidedown Pi ?  
			new RACT_CharSet (0x62, 0x2409), // HT symbol 
			new RACT_CharSet (0x63, 0x240C), // LF Symbol  
			new RACT_CharSet (0x64, 0x240D), // CR Symbol  
			new RACT_CharSet (0x65, 0x240A), // LF Symbol  
			new RACT_CharSet (0x66, 0x00B0), // Degree  
			new RACT_CharSet (0x67, 0x00B1), // Plus over Minus  
			new RACT_CharSet (0x68, 0x2424), // NL Symbol  
			new RACT_CharSet (0x69, 0x240B), // VT Symbol 
			//            new uc_CharSet (0x6A, 0x2518), // Bottom Right Box 
			//            new uc_CharSet (0x6B, 0x2510), // Top Right Box
			//            new uc_CharSet (0x6C, 0x250C), // TopLeft Box
			//            new uc_CharSet (0x6D, 0x2514), // Bottom Left Box
			//            new uc_CharSet (0x6E, 0x253C), // Cross Piece
			new RACT_CharSet (0x6F, 0x23BA), // Scan Line 1
			new RACT_CharSet (0x70, 0x25BB), // Scan Line 3
			//            new uc_CharSet (0x71, 0x2500), // Horizontal Line (scan line 5 as well?)
			new RACT_CharSet (0x72, 0x23BC), // Scan Line 7 
			new RACT_CharSet (0x73, 0x23BD), // Scan Line 9 
			//            new uc_CharSet (0x74, 0x251C), // Left Tee Piece
			//            new uc_CharSet (0x75, 0x2524), // Right Tee Piece
			//            new uc_CharSet (0x76, 0x2534), // Bottom Tee Piece
			//            new uc_CharSet (0x77, 0x252C), // Top Tee Piece
			//            new uc_CharSet (0x78, 0x2502), // Vertical Line
			new RACT_CharSet (0x79, 0x2264), // Less than or equal  
			new RACT_CharSet (0x7A, 0x2265), // Greater than or equal 
			new RACT_CharSet (0x7B, 0x03A0), // Capital Pi
			new RACT_CharSet (0x7C, 0x2260), // Not Equal 
			new RACT_CharSet (0x7D, 0x00A3), // Pound Sign 
			new RACT_CharSet (0x7E, 0x00B7), // Middle Dot 
			};

        public static RACT_CharSet[] DECS =
		{
			new RACT_CharSet (0xA8, 0x0020), // Currency Sign
			new RACT_CharSet (0xD7, 0x0152), // latin small ligature OE 
			new RACT_CharSet (0xDD, 0x0178), // Capital Y with diaeresis
			new RACT_CharSet (0xF7, 0x0153), // latin small ligature oe 
			new RACT_CharSet (0xFD, 0x00FF), // Lowercase y with diaeresis
			};

        public static RACT_CharSet[] ASCII = // same as Basic Latin
		{
			new RACT_CharSet (0x00, 0x0000), //
			};

        public static RACT_CharSet[] NRCUK = // UK National Replacement
		{
			new RACT_CharSet (0x23, 0x00A3), //
			};

        public static RACT_CharSet[] NRCFinnish = // Finnish National Replacement
		{
			new RACT_CharSet (0x5B, 0x00C4), // A with diearesis
			new RACT_CharSet (0x5C, 0x00D6), // O with diearesis
			new RACT_CharSet (0x5D, 0x00C5), // A with hollow dot above
			new RACT_CharSet (0x5E, 0x00DC), // U with diearesis
			new RACT_CharSet (0x60, 0x00E9), // e with accute accent
			new RACT_CharSet (0x7B, 0x00E4), // a with diearesis
			new RACT_CharSet (0x7C, 0x00F6), // o with diearesis
			new RACT_CharSet (0x7D, 0x00E5), // a with hollow dot above
			new RACT_CharSet (0x7E, 0x00FC), // u with diearesis
			};

        public static RACT_CharSet[] NRCFrench = // French National Replacement
		{
			new RACT_CharSet (0x23, 0x00A3), // Pound Sign
			new RACT_CharSet (0x40, 0x00E0), // a with grav accent
			new RACT_CharSet (0x5B, 0x00B0), // Degree Symbol
			new RACT_CharSet (0x5C, 0x00E7), // little cedila
			new RACT_CharSet (0x5D, 0x00A7), // funny s (technical term)
			new RACT_CharSet (0x7B, 0x00E9), // e with accute accent
			new RACT_CharSet (0x7C, 0x00F9), // u with grav accent
			new RACT_CharSet (0x7D, 0x00E8), // e with grav accent
			new RACT_CharSet (0x7E, 0x00A8), // diearesis
			};

        public static RACT_CharSet[] NRCFrenchCanadian = // French Canadian National Replacement
		{
			new RACT_CharSet (0x40, 0x00E0), // a with grav accent
			new RACT_CharSet (0x5B, 0x00E2), // a with circumflex
			new RACT_CharSet (0x5C, 0x00E7), // little cedila
			new RACT_CharSet (0x5D, 0x00EA), // e with circumflex
			new RACT_CharSet (0x5E, 0x00CE), // i with circumflex
			new RACT_CharSet (0x60, 0x00F4), // o with circumflex
			new RACT_CharSet (0x7B, 0x00E9), // e with accute accent
			new RACT_CharSet (0x7C, 0x00F9), // u with grav accent
			new RACT_CharSet (0x7D, 0x00E8), // e with grav accent
			new RACT_CharSet (0x7E, 0x00FB), // u with circumflex
			};

        public static RACT_CharSet[] NRCGerman = // German National Replacement
		{
			new RACT_CharSet (0x40, 0x00A7), // funny s
			new RACT_CharSet (0x5B, 0x00C4), // A with diearesis
			new RACT_CharSet (0x5C, 0x00D6), // O with diearesis
			new RACT_CharSet (0x5D, 0x00DC), // U with diearesis
			new RACT_CharSet (0x7B, 0x00E4), // a with diearesis
			new RACT_CharSet (0x7C, 0x00F6), // o with diearesis
			new RACT_CharSet (0x7D, 0x00FC), // u with diearesis
			new RACT_CharSet (0x7E, 0x00DF), // funny B
			};

        public static RACT_CharSet[] NRCItalian = // Italian National Replacement
		{
			new RACT_CharSet (0x23, 0x00A3), // pound sign
			new RACT_CharSet (0x40, 0x00A7), // funny s
			new RACT_CharSet (0x5B, 0x00B0), // Degree Symbol
			new RACT_CharSet (0x5C, 0x00E7), // little cedilla
			new RACT_CharSet (0x5D, 0x00E9), // e with accute accent
			new RACT_CharSet (0x60, 0x00F9), // u with grav accent
			new RACT_CharSet (0x7B, 0x00E0), // a with grav accent
			new RACT_CharSet (0x7C, 0x00F2), // o with grav accent
			new RACT_CharSet (0x7D, 0x00E8), // e with grav accent
			new RACT_CharSet (0x7E, 0x00CC), // I with grav accent
			};

        public static RACT_CharSet[] NRCNorDanish = // Norwegian Danish National Replacement
		{
			new RACT_CharSet (0x5B, 0x00C6), // AE ligature
			new RACT_CharSet (0x5C, 0x00D8), // O with strikethough
			new RACT_CharSet (0x5D, 0x00D8), // O with strikethough
			new RACT_CharSet (0x5D, 0x00C5), // A with hollow dot above
			new RACT_CharSet (0x7B, 0x00E6), // ae ligature
			new RACT_CharSet (0x7C, 0x00F8), // o with strikethough
			new RACT_CharSet (0x7D, 0x00F8), // o with strikethough
			new RACT_CharSet (0x7D, 0x00E5), // a with hollow dot above
			};

        public static RACT_CharSet[] NRCPortuguese = // Portuguese National Replacement
		{
			new RACT_CharSet (0x5B, 0x00C3), // A with tilde
			new RACT_CharSet (0x5C, 0x00C7), // big cedilla
			new RACT_CharSet (0x5D, 0x00D5), // O with tilde
			new RACT_CharSet (0x7B, 0x00E3), // a with tilde
			new RACT_CharSet (0x7C, 0x00E7), // little cedilla
			new RACT_CharSet (0x7D, 0x00F6), // o with tilde
			};

        public static RACT_CharSet[] NRCSpanish = // Spanish National Replacement
		{
			new RACT_CharSet (0x23, 0x00A3), // pound sign
			new RACT_CharSet (0x40, 0x00A7), // funny s
			new RACT_CharSet (0x5B, 0x00A1), // I with dot
			new RACT_CharSet (0x5C, 0x00D1), // N with tilde
			new RACT_CharSet (0x5D, 0x00BF), // Upside down question mark
			new RACT_CharSet (0x7B, 0x0060), // back single quote
			new RACT_CharSet (0x7C, 0x00B0), // Degree Symbol
			new RACT_CharSet (0x7D, 0x00F1), // n with tilde
			new RACT_CharSet (0x7E, 0x00E7), // small cedilla
			};

        public static RACT_CharSet[] NRCSwedish = // Swedish National Replacement
		{
			new RACT_CharSet (0x40, 0x00C9), // E with acute
			new RACT_CharSet (0x5B, 0x00C4), // A with diearesis
			new RACT_CharSet (0x5C, 0x00D6), // O with diearesis
			new RACT_CharSet (0x5D, 0x00C5), // A with hollow dot above
			new RACT_CharSet (0x5E, 0x00DC), // U with diearesis
			new RACT_CharSet (0x60, 0x00E9), // e with accute accent
			new RACT_CharSet (0x7B, 0x00E4), // a with diearesis
			new RACT_CharSet (0x7C, 0x00F6), // o with diearesis
			new RACT_CharSet (0x7D, 0x00E5), // a with hollow dot above
			new RACT_CharSet (0x7E, 0x00FC), // u with diearesis
			};

        public static RACT_CharSet[] NRCSwiss = // Swiss National Replacement
		{
			new RACT_CharSet (0x23, 0x00F9), // u with grav accent
			new RACT_CharSet (0x40, 0x00E0), // a with grav accent
			new RACT_CharSet (0x5B, 0x00E9), // e with accute accent
			new RACT_CharSet (0x5C, 0x00E7), // small cedilla
			new RACT_CharSet (0x5D, 0x00EA), // e with circumflex
			new RACT_CharSet (0x5E, 0x00CE), // i with circumflex
			new RACT_CharSet (0x5F, 0x00E8), // e with grav accent
			new RACT_CharSet (0x60, 0x00F4), // o with circumflex
			new RACT_CharSet (0x7B, 0x00E4), // a with diearesis
			new RACT_CharSet (0x7C, 0x00F6), // o with diearesis
			new RACT_CharSet (0x7D, 0x00FC), // u with diearesis
			new RACT_CharSet (0x7E, 0x00FB), // u with circumflex
			};

        public static RACT_CharSet[] ISOLatin1S = // Same as Latin-1 Supplemental
		{
			new RACT_CharSet (0x00, 0x0000) //
		};

        public enum Sets
        {
            None,
            DECSG,
            DECTECH,
            DECS,
            ASCII,
            ISOLatin1S,
            NRCUK,
            NRCFinnish,
            NRCFrench,
            NRCFrenchCanadian,
            NRCGerman,
            NRCItalian,
            NRCNorDanish,
            NRCPortuguese,
            NRCSpanish,
            NRCSwedish,
            NRCSwiss
        }
    }
}
