using System;

namespace InstagramPhotos.Utility.Utility
{
    public class MathHelper
    {
        /// <summary>
        /// 四舍五入 如 :    1.5四舍五入后为2     1.4四舍五入后为1
        /// </summary>
        /// <param name="d">原数据</param>
        /// <param name="i">保留几位小数</param>
        /// <returns></returns>
        public static double Round(double d, int i)
        {


            if (d >= 0)
            {
                d += 5 * Math.Pow(10, -(i + 1));//求指定次数的指定次幂
            }
            else
            {
                d += 5 * Math.Pow(10, -(i + 1));
            }

            string str = d.ToString("F2");
            if (str.IndexOf('.') < 0)
            {
                return d;
            }

            string[] strs = str.Split('.');
            int idot = str.IndexOf('.');
            string prestr = strs[0];
            string poststr = strs[1];
            if (poststr.Length > i)
            {
                poststr = str.Substring(idot + 1, i);//截取需要位数
            }
            if (poststr.Length <= 2)
            {
                poststr = poststr + "0";
            }
            string strd = prestr + "." + poststr;
            d = double.Parse(strd);//将字符串转换为双精度实数
            return d;
        }

        /// <summary>
        /// 四舍五入 如 :    1.5四舍五入后为2     1.4四舍五入后为1
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static int RoundInt(double d)
        {
            return Convert.ToInt32(Round(d, 0));
        }
    }
}
