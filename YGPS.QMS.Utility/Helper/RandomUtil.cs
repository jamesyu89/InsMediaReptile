using System;
using System.Linq;

namespace InstagramPhotos.Utility.Helper
{
    /// <summary>
    /// 随机数(和/或字母)帮助类
    /// </summary>
    public class RandomUtil
    {
        /// <summary>
        /// 根据输入的指定长度获取随机数字
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string CreateNumString(int len)
        {
            var result = string.Empty;
            if (len < 1)
            {
                return result;
            }

            var list = Enumerable.Range(0, 10).Select(x => x.ToString()).ToList();

            var cnt = list.Count;
            for (int i = 0; i < len; i++)
            {
                var random = new Random(Guid.NewGuid().GetHashCode());
                var index = random.Next(cnt);
                result = string.Format("{0}{1}", result, list[index]);
            }

            return result;
        }

        /// <summary>
        /// 根据输入的指定长度获取随机大写字母组合
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string CreateLetterString(int len)
        {
            var result = string.Empty;
            if (len < 1)
            {
                return result;
            }

            var upperNum = 65; //大写字母开始数字
            var list = Enumerable.Range(upperNum, 26).Select(x => ((char)x).ToString()).ToList();
            var cnt = list.Count;
            for (int i = 0; i < len; i++)
            {
                var random = new Random(Guid.NewGuid().GetHashCode());
                var index = random.Next(cnt);
                result = string.Format("{0}{1}", result, list[index]);
            }

            //0替换为H O替换为Z
            result = result.Replace("0", "H").Replace("O", "Z");

            return result;
        }

        /// <summary>
        /// 根据输入的指定长度获取随机数字和大写字母组合
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string CreateMultString(int len)
        {
            var result = string.Empty;
            if (len < 1)
            {
                return result;
            }
            var list = Enumerable.Range(0, 10).Select(x => x.ToString()).ToList();
            var upperNum = 65; //大写字母开始数字
            list.AddRange(Enumerable.Range(upperNum, 26).Select(x => ((char)x).ToString()).ToArray());
            var cnt = list.Count;
            for (int i = 0; i < len; i++)
            {
                var random = new Random(Guid.NewGuid().GetHashCode());
                var index = random.Next(cnt);
                result = string.Format("{0}{1}", result, list[index]);
            }

            //0替换为H O替换为Z
            result = result.Replace("0", "H").Replace("O", "Z");

            return result;
        }
    }
}
