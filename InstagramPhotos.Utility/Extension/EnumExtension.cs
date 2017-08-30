using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramPhotos.Utility.Extension
{
    /// <summary>
    /// 
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        ///获取描述信息 
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum enumValue)
        {
            try
            {
                string str = enumValue.ToString();
                System.Reflection.FieldInfo field = enumValue.GetType().GetField(str);
                object[] objs = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if (objs.Length == 0) return str;
                System.ComponentModel.DescriptionAttribute da = (System.ComponentModel.DescriptionAttribute)objs[0];
                return da.Description;
            }
            catch (Exception)
            {
                return "无描述";
            }
        }
    }
}
