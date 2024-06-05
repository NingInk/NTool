using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace NTool.Extensions
{
    public static class StringExtension
    {
        public static string LeftJustifying(this string source, int number, char c = ' ')
        {
            return source
                + new string(
                    c,
                    number - System.Text.Encoding.GetEncoding("gb2312").GetBytes(source).Length
                );
        }

        public static string RightJustifying(this string source, int number, char c = ' ')
        {
            return new string(
                    c,
                    number - System.Text.Encoding.GetEncoding("gb2312").GetBytes(source).Length
                ) + source;
        }

        /// <summary>
        /// 截取两者之间的字符
        /// </summary>
        /// <param name="source">源字符</param>
        /// <param name="startStr">开始字符</param>
        /// <param name="endStr">结束字符</param>
        /// <returns>截取之后的字符</returns>
        public static string CutOutBetweenByRegex(
            this string source,
            string startStr,
            string endStr
        )
        {
            var rg = new Regex(
                "(?<=(" + startStr + "))[.\\s\\S]*?(?=(" + endStr + "))",
                RegexOptions.Multiline | RegexOptions.Singleline
            );
            return rg.Match(source).Value;
        }

        public static string CutOutBetween(this string source, string startStr, string endStr)
        {
            var result = string.Empty;
            try
            {
                var startIndex = source.IndexOf(startStr, StringComparison.Ordinal);
                if (startIndex == -1)
                    return result;
                var tmpStr = source.Substring(startIndex + startStr.Length);
                var endIndex = tmpStr.IndexOf(endStr, StringComparison.Ordinal);
                if (endIndex == -1)
                    return result;
                result = tmpStr.Remove(endIndex);
            }
            catch (Exception)
            {
                // ignored
            }

            return result;
        }

        /// <summary>
        /// 添加富文本标记（颜色）
        /// </summary>
        /// <param name="str"></param>
        /// <param name="col"> cyan 青色 grey 灰色 magenta 洋红 red红 green绿 blue 蓝 yellow 黄 white 白 black 黑 </param>
        /// <returns></returns>
        public static string Color(this string str, Color col)
        {
            return str.Color(ColorUtility.ToHtmlStringRGB(col));
        }

        public static string Color(this string str, string html)
        {
            return $"<color=#{html}>{str}</color>";
        }

        /// <summary>
        /// 添加富文本标记（颜色）
        /// cyan 青色 grey 灰色 magenta 洋红 red红 green绿 blue 蓝 yellow 黄 white 白 black 黑
        /// </summary>
        /// <param name="str"></param>
        /// <param name="r">Red</param>
        /// <param name="g">Green</param>
        /// <param name="b">Blue</param>
        /// <param name="a">Alpha</param>
        /// <returns></returns>
        public static string Color(
            this string str,
            float r = 0,
            float g = 1,
            float b = 0,
            float a = 1
        )
        {
            return str.Color(new Color(r, g, b, a));
        }

        public static bool IsNullOrEmpty(this string selfStr)
        {
            return string.IsNullOrEmpty(selfStr);
        }

        public static bool IsNotNullAndEmpty(this string selfStr)
        {
            return !string.IsNullOrEmpty(selfStr);
        }

        public static bool IsTrimNullOrEmpty(this string selfStr)
        {
            return selfStr == null || string.IsNullOrEmpty(selfStr.Trim());
        }

        /// <summary>
        /// Check Whether string trim is null or empty
        /// </summary>
        /// <param name="selfStr"></param>
        /// <returns></returns>
        public static bool IsTrimNotNullAndEmpty(this string selfStr)
        {
            return selfStr != null && !string.IsNullOrEmpty(selfStr.Trim());
        }

        /// <summary>
        /// 缓存
        /// </summary>
        private static readonly char[] mCachedSplitCharArray = { '.' };

        public static string[] Split(this string selfStr, char splitSymbol)
        {
            mCachedSplitCharArray[0] = splitSymbol;
            return selfStr.Split(mCachedSplitCharArray);
        }

        public static string FillFormat(this string selfStr, params object[] args)
        {
            return string.Format(selfStr, args);
        }

        public static StringBuilder Builder(this string selfStr)
        {
            return new StringBuilder(selfStr);
        }

        /// <summary>
        /// 添加到最前
        /// </summary>
        /// <param name="self">源</param>
        /// <param name="prefixString">前缀内容</param>
        public static StringBuilder AddPrefix(this StringBuilder self, string prefixString)
        {
            self.Insert(0, prefixString);
            return self;
        }

        public static int ToInt(this string selfStr, int defaultValue = 0)
        {
            return int.TryParse(selfStr, out var result) ? result : defaultValue;
        }

        public static DateTime ToDateTime(
            this string selfStr,
            DateTime defaultValue = default(DateTime)
        )
        {
            return DateTime.TryParse(selfStr, out var retValue) ? retValue : defaultValue;
        }

        public static float ToFloat(this string selfStr, float defaultValue = 0)
        {
            return float.TryParse(selfStr, out var retValue) ? retValue : defaultValue;
        }

        public static bool HasChinese(this string input)
        {
            return Regex.IsMatch(input, @"[\u4e00-\u9fa5]");
        }

        public static bool HasSpace(this string input)
        {
            return input.Contains(" ");
        }

        public static string RemoveString(this string str, params string[] targets)
        {
            return targets.Aggregate(str, (current, t) => current.Replace(t, string.Empty));
        }

        public static string StringJoinStringJoin(this IEnumerable<string> self, string separator)
        {
            return string.Join(separator, self);
        }
    }
}
