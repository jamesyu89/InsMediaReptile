using System;

namespace InstagramPhotos.Utility.Text
{
    public class TextHelper
    {
        /// <summary>
        ///     从地址里查找省市区
        /// </summary>
        /// <param name="address">输入地址</param>
        /// <param name="province">输出省</param>
        /// <param name="city">输出市</param>
        /// <param name="district">输出区，直辖市为空</param>
        public static void HandleAddress(string address, out string province, out string city, out string district)
        {
            province = string.Empty;
            city = string.Empty;
            district = string.Empty;

            string[] zhixiashi = {"北京", "上海", "天津", "重庆"};

            //关注字符串
            string str_province = address.Substring(0, 9);
            string str_city = string.Empty;
            string str_dist = string.Empty;

            //直辖市
            foreach (string zhi in zhixiashi)
            {
                if (str_province.StartsWith(zhi))
                {
                    province = zhi;
                    str_city = address.Replace(zhi + "市", string.Empty);
                }
            }
            //一般省，自治区
            if (string.IsNullOrEmpty(province))
            {
                string[] split_prov = {"省", "自治区"};
                province = str_province.Split(split_prov, StringSplitOptions.RemoveEmptyEntries)[0];

                str_city = address.Replace(province + "省", string.Empty).Replace(province + "自治区", string.Empty);
            }


            //市部分
            string[] split_city = {"市", "地区", "自治州", "区"};
            int min_idx_city = 99;
            string endfix_city = string.Empty;
            foreach (string sp_city in split_city)
            {
                int idx = str_city.IndexOf(sp_city);
                if (idx > 1 && idx < min_idx_city)
                {
                    min_idx_city = idx;
                    endfix_city = sp_city;
                }
            }
            if (min_idx_city < 10)
            {
                city = str_city.Substring(0, min_idx_city);
                str_dist = str_city.Replace(city + endfix_city, string.Empty);
            }


            //区部分
            string[] split_dist = {"市", "自治县", "地区", "县", "区", "镇", "旗", "街道"};
            int min_idx_dist = 99;
            string endfix_dist = string.Empty;
            foreach (string sp_dist in split_dist)
            {
                int idx = str_dist.IndexOf(sp_dist);
                if (idx > 1 && idx < min_idx_dist)
                {
                    min_idx_dist = idx;
                    endfix_dist = sp_dist;
                }
            }
            if (min_idx_dist < 10)
            {
                district = str_dist.Substring(0, min_idx_dist);
            }
        }


        /// <summary>
        ///     从地址里查找省市区 wayne版，看不懂上面那些
        /// </summary>
        /// <param name="address">输入地址</param>
        /// <param name="province">输出省</param>
        /// <param name="city">输出市</param>
        /// <param name="district">输出区，直辖市为空</param>
        public static void PraseAddress(string address, out string province, out string city, out string district)
        {
            province = string.Empty;
            city = string.Empty;
            district = string.Empty;

            string[] zhixiashi = {"北京", "上海", "天津", "重庆"};

            //关注字符串
            string str_province = address.Substring(0, address.Length > 8 ? 9 : address.Length);
            string str_city = string.Empty;
            string str_dist = string.Empty;

            //直辖市
            foreach (string zhi in zhixiashi)
            {
                if (str_province.StartsWith(zhi))
                {
                    province = zhi;
                    str_city = address.Replace(zhi + "市", string.Empty);
                }
            }
            //一般省，自治区
            if (string.IsNullOrEmpty(province))
            {
                string[] split_prov = {"省", "自治区"};
                province = str_province.Split(split_prov, StringSplitOptions.RemoveEmptyEntries)[0];

                str_city = address.Replace(province + "省", string.Empty).Replace(province + "自治区", string.Empty);
            }


            //市部分
            string[] split_city = {"市", "地区", "自治州", "区"};
            int min_idx_city = 99;
            string endfix_city = string.Empty;
            foreach (string sp_city in split_city)
            {
                int idx = str_city.IndexOf(sp_city);
                if (idx > 1 && idx < min_idx_city)
                {
                    min_idx_city = idx;
                    endfix_city = sp_city;
                }
            }
            if (min_idx_city < 10)
            {
                city = str_city.Substring(0, min_idx_city);
                str_dist = str_city.Replace(city + endfix_city, string.Empty);
            }


            //区部分
            string[] split_dist = {"市", "自治县", "地区", "县", "区", "镇", "旗", "街道"};
            int min_idx_dist = 99;
            string endfix_dist = string.Empty;
            foreach (string sp_dist in split_dist)
            {
                int idx = str_dist.IndexOf(sp_dist);
                if (idx > 1 && idx < min_idx_dist)
                {
                    min_idx_dist = idx;
                    endfix_dist = sp_dist;
                }
            }
            if (min_idx_dist < 10)
            {
                district = str_dist.Substring(0, min_idx_dist);
            }
        }
    }
}