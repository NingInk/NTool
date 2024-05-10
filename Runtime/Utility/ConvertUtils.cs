using System.Linq;

namespace NTool.Utility
{
    public class ConvertUtils
    {
        private const int Gb = 1024 * 1024 * 1024,
            Mb = 1024 * 1024,
            Kb = 1024;

        public static string ByteConvert(int size)
        {
            var str = string.Empty;
            if (size > Gb)
            {
                str += size / Gb + " GB  ";
                size %= Gb;
            }

            if (size > Mb)
            {
                str += size / Mb + " Mb  ";
                size %= Mb;
            }

            if (size > Kb)
            {
                str += size / Kb + " Kb  ";
                size %= Kb;
            }

            return str + size + " Byte";
        }

        /// <summary>
        /// 数字转汉字   （整数不大于12位,小数包含点在内不大于14位）
        /// </summary>
        /// <param name="numerals">数字</param>
        /// <param name="isUpper">是否使用大些中文</param>
        /// <returns></returns>
        public static string NumberToChina(float numerals, bool isUpper = false) =>
            isUpper
                ? NumberToChinaUpper(numerals.ToString("0.00"))
                : NumberToChina(numerals.ToString("0.00"));

        #region 数字转汉字

        private static string NumberToChina(string numerals)
        {
            if (numerals.Contains('.')) //只有整数部分
            {
                return numerals.Length > 12 ? string.Empty : IntPart(numerals);
            }
            else
            {
                if (numerals.Length > 14)
                    return string.Empty;
                else
                    return IntPart(numerals.Split('.')[0]) + DecimalPart(numerals.Split('.')[1]);
            }
        }

        private static string IntPart(string integerPart) //整数部分处理
        {
            if (string.IsNullOrEmpty(integerPart))
                return "";
            var units = new[] { "", "十", "百", "千", "万", "十", "百", "千", "亿", "十", "百", "千" }; //12个量词
            var sourceReverse = string.Join("", integerPart.Reverse());
            var tmp = string.Empty;
            for (var i = integerPart.Length - 1; i >= 0; i--)
            {
                tmp += TransitionChar(sourceReverse[i], i) + units[i]; //数字转汉字，并加上量词
            }

            return tmp.Replace("零千", "零")
                .Replace("零百", "零")
                .Replace("零十", "零")
                .Replace("零零零#万", "零")
                .Replace("零零零", "零")
                .Replace("零零", "零")
                .Replace("零#", "")
                .Replace("#", "");
        }

        private static string DecimalPart(string decimalsPart) //小数部分处理
        {
            if (string.IsNullOrEmpty(decimalsPart))
                return "";
            decimalsPart = decimalsPart.TrimEnd('0');
            var lens = decimalsPart.Length;
            if (lens == 0 || lens > 2)
                return "";
            var tmp = "点";
            for (var i = 0; i < lens; i++)
                tmp += TransitionChar(decimalsPart[i]);
            return tmp;
        }

        private static string TransitionChar(char c, int index = -1)
        {
            switch (c)
            {
                case '0':
                    if (index == 0 || index == 4 || index == 8)
                        return "#";
                    else
                        return "零";
                case '1':
                    return "一";
                case '2':
                    return "二";
                case '3':
                    return "三";
                case '4':
                    return "四";
                case '5':
                    return "五";
                case '6':
                    return "六";
                case '7':
                    return "七";
                case '8':
                    return "八";
                case '9':
                    return "九";
                default:
                    return c.ToString();
            }
        }

        #endregion

        #region 数字转大写汉字

        /// <summary>
        /// 数字转汉字   （整数不大于12位,小数包含点在内不大于14位）
        /// </summary>
        /// <param name="numerals"></param>
        /// <returns></returns>
        private static string NumberToChinaUpper(string numerals)
        {
            if (numerals.Contains(".")) //只有整数部分
            {
                return numerals.Length > 12 ? "" : Part_Int(numerals);
            }
            else
            {
                if (numerals.Length > 14)
                    return string.Empty;
                else
                    return Part_Int(numerals.Split('.')[0]) + Part_Decimal(numerals.Split('.')[1]);
            }
        }

        static string Part_Int(string integerPart) //整数部分处理
        {
            if (string.IsNullOrEmpty(integerPart))
                return "";
            var units = new[] { "", "拾", "佰", "仟", "万", "拾", "佰", "仟", "亿", "拾", "佰", "仟" }; //12个量词
            var sourceReverse = string.Join("", integerPart.Reverse());
            var tmp = "";
            for (var i = integerPart.Length - 1; i >= 0; i--)
            {
                tmp += Transition_Char(sourceReverse[i], i) + units[i]; //数字转汉字，并加上量词
            }

            return tmp.Replace("零仟", "零")
                .Replace("零佰", "零")
                .Replace("零拾", "零")
                .Replace("零零零#万", "零")
                .Replace("零零零", "零")
                .Replace("零零", "零")
                .Replace("零#", "")
                .Replace("#", "");
        }

        private static string Part_Decimal(string decimalsPart) //小数部分处理
        {
            if (string.IsNullOrEmpty(decimalsPart))
                return "";
            decimalsPart = decimalsPart.TrimEnd('0');
            var lens = decimalsPart.Length;
            if (lens == 0 || lens > 2)
                return "";
            var tmp = "点";
            for (var i = 0; i < lens; i++)
                tmp += Transition_Char(decimalsPart[i]);
            return tmp;
        }

        private static string Transition_Char(char c, int index = -1) //数字转汉字
        {
            string tmp;
            switch (c)
            {
                case '0':
                    if (index == 0 || index == 4 || index == 8)
                        tmp = "#";
                    else
                        tmp = "零";
                    break;
                case '1':
                    tmp = "壹";
                    break;
                case '2':
                    tmp = "贰";
                    break;
                case '3':
                    tmp = "叁";
                    break;
                case '4':
                    tmp = "肆";
                    break;
                case '5':
                    tmp = "伍";
                    break;
                case '6':
                    tmp = "陆";
                    break;
                case '7':
                    tmp = "柒";
                    break;
                case '8':
                    tmp = "捌";
                    break;
                case '9':
                    tmp = "玖";
                    break;
                default:
                    tmp = c.ToString();
                    break;
            }

            return tmp;
        }

        #endregion
    }
}