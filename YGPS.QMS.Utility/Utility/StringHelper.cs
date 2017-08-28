using System;

namespace InstagramPhotos.Utility.Utility
{
    public static class StringHelper
    {
        public static string GetRandomString(int len, string codes = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890")
        {
            var arr = new char[len];
            var s = Guid.NewGuid().ToString("N");
            var l = Convert.ToInt32(s.Substring(0, 5), 16);
            var random = new Random(l);
            for (int i = 0; i < len; i++)
            {
                arr[i] = codes[random.Next(0, codes.Length)];

            }
            return new string(arr);
        }

        public static string GetLengthStr(int len)
        {
            if (len < 10)
            {
                return "00" + len;
            }
            if (len >= 10 && len < 100)
            {
                return "0" + len;
            }
            if (len >= 100 && len < 1000)
            {
                return len.ToString();
            }
            throw new Exception("长度超长或为负数的异常");
        }

    }
}
