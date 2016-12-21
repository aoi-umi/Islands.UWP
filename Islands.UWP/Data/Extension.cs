using System.Text;
using System.Text.RegularExpressions;

namespace Islands.UWP
{
    public static class StringExtension
    {
        //unicode字符串转字符
        public static string UnicodeDencode(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;
            return Regex.Unescape(str);
        }

        //字符转unicode字符串
        public static string UnicodeEncode(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return str;
            StringBuilder strResult = new StringBuilder();
            if (!string.IsNullOrEmpty(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    strResult.Append("\\u");
                    strResult.Append(((int)str[i]).ToString("x4"));
                }
            }
            return strResult.ToString();
        }

        //本地连接补全host
        public static string FixHost(this string str, string Host)
        {
            return Regex.Replace(str, @"(href=""(/[^\s]+)"")", "href=\"" + Host + "$2\"");
        }

        //不带tag的http连接添加tag
        public static string FixLinkTag(this string str)
        {
            return Regex.Replace(str, @"(?<!<a.*)((?:http|https)://[^\s<""]+)", "<a href=\"$1\">$1</a>");
        }

        //引用处理
        public static string FixRef(this string str)
        {
            return Regex.Replace(str, "(<Run Foreground=\"#789922\">)?(&gt;&gt;(no\\.)?(\\d+))(.*?</Run>)?", "<Hyperlink UnderlineStyle =\"None\" Foreground=\"#789922\">$2</Hyperlink>", RegexOptions.IgnoreCase);
        }

        public static string FixEntity(this string str)
        {
            return Regex.Replace(str, "xff.{2};", "&#$0");
        }

        public static string GetExt(this string str)
        {
            int index = str.LastIndexOf(".");
            return index >= 0 ? str.Substring(index) : string.Empty;
        }
    }
}
