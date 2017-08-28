using System;
using System.Collections.Generic;

namespace InstagramPhotos.Utility.Utility
{
    public static class BitMapUtility
    {
        public static bool Has(this int bitList, int bit)
        {
            return (bitList & bit) == bit;
        }

        public static List<int> ToBitList(this int bitList)
        {
            var result = new List<int>();

            for (int i = 0; Math.Pow(2, i) <= bitList; i++)
            {
                if ((bitList.Has((int)Math.Pow(2, i))))
                {
                    result.Add((int)Math.Pow(2, i));
                }
            }

            return result;
        }
    }
}
