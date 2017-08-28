using System;

namespace InstagramPhotos.Utility.Helper
{
    /// <summary>
    /// 数字处理帮助类
    /// </summary>
    public class NumberUtil
    {
        /// <summary>
        /// 标准四舍五入方法
        /// </summary>
        /// <param name="input"></param>
        /// <param name="decimals">保留小数位数 默认2位小数</param>
        /// <returns></returns>
        public static decimal PowerRound(decimal input, int decimals = 2)
        {
            var result = Math.Round(input, decimals, MidpointRounding.AwayFromZero);

            //Math.Round(3.144, 2, MidpointRounding.AwayFromZero); //3.14.  四舍
            //Math.Round(3.1451, 2, MidpointRounding.AwayFromZero); //3.15  四舍五入
            //Math.Round(3.145, 2, MidpointRounding.AwayFromZero); //3.15. 四舍五入
            //Math.Round(3.135, 2, MidpointRounding.AwayFromZero);  //3.14  四舍五入
            //Math.Round(3.146, 2, MidpointRounding.AwayFromZero); //3.15. 六入

            return result;
        }
    }
}
