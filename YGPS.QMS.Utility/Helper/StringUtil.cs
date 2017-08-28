using System;
using System.Text.RegularExpressions;

namespace InstagramPhotos.Utility.Helper
{
    /// <summary>
    /// 常用字符串帮助类
    /// </summary>
    public class StringUtil
    {
        #region 字段

        private static readonly Regex regNum = new Regex(@"^\d+$", RegexOptions.CultureInvariant);

        private static readonly Regex regMobilePhoneNo = new Regex(@"^((\d{3})|(\d{3}-))?1[1,2,3,4,5,6,7,8,9]\d{9}$", RegexOptions.CultureInvariant);

        private static readonly Regex regEmail = new Regex(@"^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$", RegexOptions.CultureInvariant);

        #endregion

        #region 方法

        /// <summary>
        /// 验证输入字符串是否全部由数字构成
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumber(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            return regNum.IsMatch(input);
        }

        /// <summary>
        /// 验证邮箱格式是否正确
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEmail(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            return regEmail.IsMatch(input);
        }

        /// <summary>
        /// 验证是否手机号码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsMobilePhoneNo(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            return regMobilePhoneNo.IsMatch(input);
        }

        /// <summary>
        /// 验证是否是中国大陆合法的身份证号码
        /// </summary>
        /// <param name="IdNumber">身份证号码</param>
        /// <returns>是否符合身份证号码格式</returns>
        public static bool IsIDCard(string IdNumber)
        {
            if (IdNumber.Length == 18)
            {
                bool check = CheckIDCard18(IdNumber);
                return check;
            }

            if (IdNumber.Length == 15)
            {
                bool check = CheckIDCard15(IdNumber);
                return check;
            }

            return false;

        }

        /// <summary>
        /// 是否是IP地址
        /// </summary>
        /// <param name="ipAddress">IP地址 形如：1.1.1.1</param>
        /// <returns></returns>
        public static bool IsIpAddress(string ipAddress)
        {
            var flag = false;

            if (string.IsNullOrEmpty(ipAddress) || ipAddress.Length < 7 || ipAddress.Length > 15)
            {
                return flag;
            }

            var regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";
            var regex = new Regex(regformat, RegexOptions.IgnoreCase);

            flag = regex.IsMatch(ipAddress);

            return flag;
        }

        /// <summary>
        /// 字符串截取，返回指定长度的字符串 一个汉字算作两个字符长度
        /// </summary>
        /// <param name="sourceStr">字符串</param>
        /// <param name="len">截取的字符串长度</param>
        /// <param name="isdot">末尾是否使用...</param>
        /// <returns></returns>
        public static String CutString(string sourceStr, int len, bool isdot = false)
        {
            if (sourceStr == null)
            {
                return string.Empty;
            }
            string tsourceStr = Convert.ToString(sourceStr);
            if (GetStrLen(tsourceStr) <= len)
            {
                return tsourceStr;
            }

            //tsourceStr = System.Text.RegularExpressions.Regex.Replace(tsourceStr, "<[^>]*>", "");
            string result = ""; //最终返回的结果
            //int byteLen = System.Text.Encoding.Default.GetByteCount(tsourceStr);  //单字节字符长度
            int byteLen = System.Text.Encoding.GetEncoding("gb2312").GetByteCount(tsourceStr);  //单字节字符长度
            int charLen = tsourceStr.Length; //把字符平等对待时的字符串长度
            int byteCount = 0;  //记录读取进度{中文按两单位计算}
            int pos = 0;    //记录截取位置{中文按两单位计算}
            if (byteLen > len)
            {
                for (int i = 0; i < charLen; i++)
                {
                    if (Convert.ToInt32(tsourceStr.ToCharArray()[i]) > 255)  //遇中文字符计数加2
                        byteCount += 2;
                    else         //按英文字符计算加1
                        byteCount += 1;
                    // if (byteCount >= len)   //到达指定长度时，记录指针位置并停止
                    if (byteCount > len)   //到达指定长度时，记录指针位置并停止
                    {
                        pos = i;
                        break;
                    }
                }
                if (isdot)
                {
                    result = tsourceStr.Substring(0, pos) + "...";
                }
                else
                {
                    result = tsourceStr.Substring(0, pos);
                }

            }
            else
                result = tsourceStr;

            return result;
        }

        /// <summary>
        /// 替换.操作符为对应的md5字符串
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ReplaceDotString(string input)
        {
            input = input ?? string.Empty;
            return input.Replace(".", "5058f1af8388633f609cadb75a75dc9d");
        }

        /// <summary>
        /// 替换5058f1af8388633f609cadb75a75dc9d为.操作符
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string RecoverDotString(string input)
        {
            input = input ?? string.Empty;
            return input.Replace("5058f1af8388633f609cadb75a75dc9d", ".");
        }

        #endregion

        #region 辅助方法

        private static bool CheckIDCard18(string Id)
        {
            long n = 0;
            if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = Id.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
            {
                return false;//校验码验证
            }
            return true;//符合GB11643-1999标准
        }

        private static bool CheckIDCard15(string Id)
        {
            long n = 0;
            if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            return true;//符合15位身份证标准
        }

        /// <summary>
        /// 获取字符串长度 一个汉字算两个长度
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static int GetStrLen(string input)
        {
            if (input == null)
            {
                input = string.Empty;
            }
            input = Regex.Replace(input, "[\u4e00-\u9fa5]", "aa", RegexOptions.IgnoreCase);
            return input.Length;
        }

        #endregion

    }
}
