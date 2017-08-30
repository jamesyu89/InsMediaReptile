using System;
using System.Collections.Generic;

namespace InstagramPhotos.Utility.Text.Encoding
{
    public class GB2312Helper
    {
        private static readonly System.Text.Encoding GB2312 = System.Text.Encoding.GetEncoding("GB2312");

        /// <summary>
        ///     265313 327332 303367对应汉字：邓宗明
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string GetGB2312ByOct(string p)
        {
            string input = p.Replace(" ", string.Empty);
            var bytes = new List<byte>();

            for (int i = 0; i < input.Length; i += 3)
            {
                bytes.Add(Convert.ToByte(Convert.ToInt32(input.Substring(i, 3), 8)));
            }

            return new String(GB2312.GetChars(bytes.ToArray()));
        }
    }
}