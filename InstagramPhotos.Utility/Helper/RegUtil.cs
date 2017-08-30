using System.Text.RegularExpressions;

namespace InstagramPhotos.Utility.Helper
{
    public class RegUtil
    {
        public static Regex regNum = new Regex(@"^(-?\d+)(\.\d+)?$");
        public static Regex regPosNum = new Regex(@"^[0-9]*[1-9][0-9]*$");
        public static Regex regUrl = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
        public static Regex regEmail = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        private static Regex regHTML = new Regex(@"<([^<>]|<([^<>]|<[^<>]*>)*>)*>", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// 是否是数字 正负 整数，小数等
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumber(string input)
        {
            bool flag = false;
            if (string.IsNullOrEmpty(input) == false)
            {
                flag = regNum.IsMatch(input.Trim());
            }
            return flag;
        }

        /// <summary>
        /// 是否是正整数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsPositiveNumber(string input)
        {
            bool flag = false;
            if (string.IsNullOrEmpty(input) == false)
            {
                flag = regPosNum.IsMatch(input.Trim());
            }
            return flag;
        }

        public static bool IsUrl(string input)
        {
            bool flag = false;
            if (string.IsNullOrEmpty(input) == false)
            {
                flag = regUrl.IsMatch(input.Trim());
            }
            return flag;
        }

        public static bool IsEmail(string input)
        {
            bool flag = false;
            if (string.IsNullOrEmpty(input) == false)
            {
                flag = regEmail.IsMatch(input.Trim());
            }
            return flag;
        }

        public static string ClearHTML(string input)
        {
            return regHTML.Replace(input, string.Empty);
        }

    }
}
