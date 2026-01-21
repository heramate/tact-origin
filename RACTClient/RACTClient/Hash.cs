using System;
using System.Collections.Generic;
using System.Web;

using System.Security.Cryptography; // sha256 password 관련 함수.
using System.Text;


namespace RACTClient
{


    /// <summary>
    /// Summary description for Hash
    /// </summary>
    public class Hash
    {
        public Hash() { }

        public enum HashType : int
        {
            MD5,
            SHA1,
            SHA256,
            SHA512
        }

        public enum TextCoding : int
        {
            ASCII,
            BigEndianUnicode,
            Unicode,
            UTF32,
            UTF7,
            UTF8,
            IBM037 = 37,
            IBM437 = 437,
            IBM500 = 500,
            ASMO_708 = 708,
            DOS_720 = 720,
            ibm737 = 737,
            ibm775 = 775,
            ibm850 = 850,
            ibm852 = 852,
            IBM855 = 855,
            ibm857 = 857,
            IBM00858 = 858,
            IBM860 = 860,
            ibm861 = 861,
            DOS_862 = 862,
            IBM863 = 863,
            IBM864 = 864,
            IBM865 = 865,
            cp866 = 866,
            ibm869 = 869,
            IBM870 = 870,
            windows_874 = 874,
            cp875 = 875,
            shift_jis = 932,
            gb2312 = 936,
            ks_c_5601_1987 = 949,
            big5 = 950,
            IBM1026 = 1026,
            IBM01047 = 1047,
            IBM01140 = 1140,
            IBM01141 = 1141,
            IBM01142 = 1142,
            IBM01143 = 1143,
            IBM01144 = 1144,
            IBM01145 = 1145,
            IBM01146 = 1146,
            IBM01147 = 1147,
            IBM01148 = 1148,
            IBM01149 = 1149,
            utf_16 = 1200,
            unicodeFFFE = 1201,
            windows_1250 = 1250,
            windows_1251 = 1251,
            windows_1252 = 1252,
            windows_1253 = 1253,
            windows_1254 = 1254,
            windows_1255 = 1255,
            windows_1256 = 1256,
            windows_1257 = 1257,
            windows_1258 = 1258,
            조합 = 1361,
            macintosh = 10000,
            x_mac_japanese = 10001,
            x_mac_chinesetrad = 10002,
            x_mac_korean = 10003,
            x_mac_arabic = 10004,
            x_mac_hebrew = 10005,
            x_mac_greek = 10006,
            x_mac_cyrillic = 10007,
            x_mac_chinesesimp = 10008,
            x_mac_romanian = 10010,
            x_mac_ukrainian = 10017,
            x_mac_thai = 10021,
            x_mac_ce = 10029,
            x_mac_icelandic = 10079,
            x_mac_turkish = 10081,
            x_mac_croatian = 10082,
            utf_32 = 12000,
            utf_32BE = 12001,
            x_Chinese_CNS = 20000,
            x_cp20001 = 20001,
            x_Chinese_Eten = 20002,
            x_cp20003 = 20003,
            x_cp20004 = 20004,
            x_cp20005 = 20005,
            x_IA5 = 20105,
            x_IA5_German = 20106,
            x_IA5_Swedish = 20107,
            x_IA5_Norwegian = 20108,
            us_ascii = 20127,
            x_cp20261 = 20261,
            x_cp20269 = 20269,
            IBM273 = 20273,
            IBM277 = 20277,
            IBM278 = 20278,
            IBM280 = 20280,
            IBM284 = 20284,
            IBM285 = 20285,
            IBM290 = 20290,
            IBM297 = 20297,
            IBM420 = 20420,
            IBM423 = 20423,
            IBM424 = 20424,
            x_EBCDIC_KoreanExt = 20833,
            IBM_Thai = 20838,
            koi8_r = 20866,
            IBM871 = 20871,
            IBM880 = 20880,
            IBM905 = 20905,
            IBM00924 = 20924,
            EUC_JP = 20932,
            x_cp20936 = 20936,
            x_cp20949 = 20949,
            cp1025 = 21025,
            koi8_u = 21866,
            iso_8859_1 = 28591,
            iso_8859_2 = 28592,
            iso_8859_3 = 28593,
            iso_8859_4 = 28594,
            iso_8859_5 = 28595,
            iso_8859_6 = 28596,
            iso_8859_7 = 28597,
            iso_8859_8 = 28598,
            iso_8859_9 = 28599,
            iso_8859_13 = 28603,
            iso_8859_15 = 28605,
            x_Europa = 29001,
            iso_8859_8_i = 38598,
            iso_20220_jp = 50220,
            csISO2022JP = 50221,
            iso_20222_jp = 50222,
            iso_2022_kr = 50225,
            x_cp50227 = 50227,
            euc_jp = 51932,
            EUC_CN = 51936,
            euc_kr = 51949,
            hz_gb_2312 = 52936,
            GB18030 = 54936,
            x_iscii_de = 57002,
            x_iscii_be = 57003,
            x_iscii_ta = 57004,
            x_iscii_te = 57005,
            x_iscii_as = 57006,
            x_iscii_or = 57007,
            x_iscii_ka = 57008,
            x_iscii_ma = 57009,
            x_iscii_gu = 57010,
            x_iscii_pa = 57011,
            utf_7 = 65000,
            utf_8 = 65001
        }

        public static string GetHashPW(string text)
        {
            return Hash.GetHash(text, Hash.TextCoding.UTF8, Hash.HashType.SHA256);
        }

        public static string GetHash(string text)
        {
            return Hash.GetHash(text, Hash.TextCoding.UTF8, Hash.HashType.SHA256);
        }

        public static string GetHash(string text, TextCoding textCoding, HashType hashType)
        {
            string hashString;
            switch (hashType)
            {
                case HashType.MD5: hashString = GetMD5(text, textCoding); break;
                case HashType.SHA1: hashString = GetSHA1(text, textCoding); break;
                case HashType.SHA256: hashString = GetSHA256(text, textCoding); break;
                case HashType.SHA512: hashString = GetSHA512(text, textCoding); break;
                default: hashString = "Invalid Hash Type"; break;
            }
            return hashString;
        }

        public static bool CheckHash(string orgText, TextCoding textCoding, string hashString, HashType hashType)
        {
            string originalHash = GetHash(orgText, textCoding, hashType);
            return (originalHash == hashString);
        }

        private static string GetMD5(string text, TextCoding textCoding)
        {
            byte[] hashValue;
            byte[] message = GetBytes(text, textCoding);

            MD5 hashString = new MD5CryptoServiceProvider();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

        private static string GetSHA1(string text, TextCoding textCoding)
        {
            byte[] hashValue;
            byte[] message = GetBytes(text, textCoding);

            SHA1Managed hashString = new SHA1Managed();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

        private static string GetSHA256(string text, TextCoding textCoding)
        {
            byte[] hashValue;
            byte[] message = GetBytes(text, textCoding);

            SHA256Managed hashString = new SHA256Managed();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

        private static string GetSHA512(string text, TextCoding textCoding)
        {
            byte[] hashValue;
            byte[] message = GetBytes(text, textCoding);

            SHA512Managed hashString = new SHA512Managed();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

        private static byte[] GetBytes(string text, TextCoding textCoding)
        {
            byte[] byteValue;

            switch (textCoding)
            {
                case TextCoding.ASCII: byteValue = System.Text.Encoding.ASCII.GetBytes(text); break;
                case TextCoding.BigEndianUnicode: byteValue = System.Text.Encoding.BigEndianUnicode.GetBytes(text); break; // big endian 바이트 순서를 사용하는 UTF-16 형식에 대한 인코딩을 가져옵니다.
                case TextCoding.Unicode: byteValue = System.Text.Encoding.Unicode.GetBytes(text); break; // little endian 바이트 순서를 사용하는 UTF-16 형식에 대한 인코딩을 가져옵니다.
                case TextCoding.UTF32: byteValue = System.Text.Encoding.UTF32.GetBytes(text); break; // little endian 바이트 순서를 사용하는 UTF-32 형식에 대한 인코딩을 가져옵니다.
                case TextCoding.UTF7: byteValue = System.Text.Encoding.UTF7.GetBytes(text); break;
                case TextCoding.UTF8: byteValue = System.Text.Encoding.UTF8.GetBytes(text); break;
                default:
                    System.Text.Encoding textEncoding = System.Text.Encoding.GetEncoding((int)textCoding);
                    byteValue = textEncoding.GetBytes(text);
                    break;
            }

            return byteValue;
        }
    }
}
/* 코드페이지   이름    표시이름
37      IBM037              IBM EBCDIC(미국-캐나다)
437     IBM437              OEM 미국
500     IBM500              IBM EBCDIC(국제)
708     ASMO-708            아랍어(ASMO 708)
720     DOS-720             아랍어(DOS)
737     ibm737              그리스어(DOS)
775     ibm775              발트어(DOS)
850     ibm850              서유럽어(DOS)
852     ibm852              중앙 유럽어(DOS)
855     IBM855              OEM 키릴 자모
857     ibm857              터키어(DOS)
858     IBM00858            OEM 다국 라틴 문자 I
860     IBM860              포르투갈어(DOS)
861     ibm861              아이슬란드어(DOS)
862     DOS-862             히브리어(DOS)
863     IBM863              프랑스어(캐나다)(DOS)
864     IBM864              아랍어(864)
865     IBM865              북유럽어(DOS)
866     cp866               키릴 자모(DOS)
869     ibm869              현대 그리스어(DOS)
870     IBM870              IBM EBCDIC(다국 라틴 문자-2)
874     windows-874         태국어(Windows)
875     cp875               IBM EBCDIC(현대 그리스어)
932     shift_jis           일본어(Shift-JIS)
936     gb2312              중국어 간체(GB2312)
949     ks_c_5601-1987      한국어
950     big5                중국어 번체(Big5)
1026    IBM1026             IBM EBCDIC(터키어 라틴 문자-5)
1047    IBM01047            IBM 라틴어-1
1140    IBM01140            IBM EBCDIC(미국-캐나다-유럽)
1141    IBM01141            IBM EBCDIC(독일-유럽)
1142    IBM01142            IBM EBCDIC(덴마크-노르웨이-유럽)
1143    IBM01143            IBM EBCDIC(핀란드-스웨덴-유럽)
1144    IBM01144            IBM EBCDIC(이탈리아-유럽)
1145    IBM01145            IBM EBCDIC(스페인-유럽)
1146    IBM01146            IBM EBCDIC(영국-유럽)
1147    IBM01147            IBM EBCDIC(프랑스-유럽)
1148    IBM01148            IBM EBCDIC(국제-유럽)
1149    IBM01149            IBM EBCDIC(아이슬란드어-유럽)
1200    utf-16              Unicode
1201    unicodeFFFE         유니코드(Big endian)
1250    windows-1250        중앙 유럽어(Windows)
1251    windows-1251        키릴 자모(Windows)
1252    windows-1252        서유럽어(Windows)
1253    windows-1253        그리스어(Windows)
1254    windows-1254        터키어(Windows)
1255    windows-1255        히브리어(Windows)
1256    windows-1256        아랍어(Windows)
1257    windows-1257        발트어(Windows)
1258    windows-1258        베트남어(Windows)
1361    조합                한국어(조합)
10000   macintosh           서유럽어(Mac)
10001   x-mac-japanese      일본어(Mac)
10002   x-mac-chinesetrad   중국어 번체(Mac)
10003   x-mac-korean        한국어(Mac)
10004   x-mac-arabic        아랍어(Mac)
10005   x-mac-hebrew        히브리어(Mac)
10006   x-mac-greek         그리스어(Mac)
10007   x-mac-cyrillic      키릴 자모(Mac)
10008   x-mac-chinesesimp   중국어 간체(Mac)
10010   x-mac-romanian      루마니아어(Mac)
10017   x-mac-ukrainian     우크라이나어(Mac)
10021   x-mac-thai          태국어(Mac)
10029   x-mac-ce            중앙 유럽어(Mac)
10079   x-mac-icelandic     아이슬란드어(Mac)
10081   x-mac-turkish       터키어(Mac)
10082   x-mac-croatian      크로아티아어(Mac)
12000   utf-32              유니코드(UTF-32)
12001   utf-32BE            유니코드(UTF-32 Big endian)
20000   x-Chinese-CNS       중국어 번체(CNS)
20001   x-cp20001           TCA 대만
20002   x-Chinese-Eten      중국어 번체(Eten)
20003   x-cp20003           IIBM5550 대만
20004   x-cp20004           TeleText 대만
20005   x-cp20005           Wang 대만
20105   x-IA5               서유럽어(IA5)
20106   x-IA5-German        독일어(IA5)
20107   x-IA5-Swedish       스웨덴어(IA5)
20108   x-IA5-Norwegian     노르웨이어(IA5)
20127   us-ascii            US-ASCII
20261   x-cp20261           T.61
20269   x-cp20269           ISO-6937
20273   IBM273              IBM EBCDIC(독일)
20277   IBM277              IBM EBCDIC(덴마크-노르웨이)
20278   IBM278              IBM EBCDIC(핀란드-스웨덴)
20280   IBM280              IBM EBCDIC(이탈리아)
20284   IBM284              IBM EBCDIC(스페인)
20285   IBM285              IBM EBCDIC(영국)
20290   IBM290              IBM EBCDIC(일본어 가타카나)
20297   IBM297              IBM EBCDIC(프랑스)
20420   IBM420              IBM EBCDIC(아랍어)
20423   IBM423              IBM EBCDIC(그리스어)
20424   IBM424              IBM EBCDIC(히브리어)
20833   x-EBCDIC-KoreanExtended IBM EBCDIC(한국어 확장)
20838   IBM-Thai            IBM EBCDIC(태국어)
20866   koi8-r              키릴 자모(KOI8-R)
20871   IBM871              IBM EBCDIC(아이슬란드어)
20880   IBM880              IBM EBCDIC(키릴 자모 러시아어)
20905   IBM905              IBM EBCDIC(터키어)
20924   IBM00924            IBM 라틴어-1
20932   EUC-JP              일본어(JIS 0208-1990 및 0212-1990)
20936   x-cp20936           중국어 간체(GB2312-80)
20949   x-cp20949           한국어(완성)
21025   cp1025              IBM EBCDIC(키릴 자모 세르비아어-불가리아어)
21866   koi8-u              키릴 자모(KOI8-U)
28591   iso-8859-1          서유럽어(ISO)
28592   iso-8859-2          중앙 유럽어(ISO)
28593   iso-8859-3          라틴어 3(ISO)
28594   iso-8859-4          발트어(ISO)
28595   iso-8859-5          키릴 자모(ISO)
28596   iso-8859-6          아랍어(ISO)
28597   iso-8859-7          그리스어(ISO)
28598   iso-8859-8          히브리어(ISO-Visual)
28599   iso-8859-9          터키어(ISO)
28603   iso-8859-13         에스토니아어(ISO)
28605   iso-8859-15         라틴어 9(ISO)
29001   x-Europa            유로파
38598   iso-8859-8-i        히브리어(ISO-Logical)
50220   iso-2022-jp         일본어(JIS)
50221   csISO2022JP         일본어(JIS-Allow 1 byte Kana)
50222   iso-2022-jp         일본어(JIS-Allow 1 byte Kana - SO/SI)
50225   iso-2022-kr         한국어(ISO)
50227   x-cp50227           중국어 간체(ISO-2022)
51932   euc-jp              일본어(EUC)
51936   EUC-CN              중국어 간체(EUC)
51949   euc-kr              한국어(EUC)
52936   hz-gb-2312          중국어 간체(HZ)
54936   GB18030             중국어 간체(GB18030)
57002   x-iscii-de          ISCII 데바나가리어
57003   x-iscii-be          ISCII 벵골어
57004   x-iscii-ta          ISCII 타밀어
57005   x-iscii-te          ISCII 텔루구어
57006   x-iscii-as          ISCII 아샘어
57007   x-iscii-or          ISCII 오리야어
57008   x-iscii-ka          ISCII 카나다어
57009   x-iscii-ma          ISCII 말라얄람어
57010   x-iscii-gu          ISCII 구자라트어
57011   x-iscii-pa          ISCII 펀잡어
65000   utf-7               유니코드(UTF-7)
65001   utf-8               유니코드(UTF-8)
*/