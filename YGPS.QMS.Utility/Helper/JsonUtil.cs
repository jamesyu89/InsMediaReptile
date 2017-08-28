using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace InstagramPhotos.Utility.Helper
{
    public class JsonUtil
    {
        #region 方法

        /// <summary>
        /// 对象转换成json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonObject">需要格式化的对象</param>
        /// <returns>Json字符串</returns>
        public static string Serialize<T>(T jsonObject)
        {
            string json = null;
            try
            {
                if (jsonObject == null)
                {
                    return json;
                }
                var serializer = new DataContractJsonSerializer(typeof(T));

                using (var ms = new MemoryStream()) //定义一个stream用来存发序列化之后的内容
                {
                    serializer.WriteObject(ms, jsonObject);
                    json = Encoding.UTF8.GetString(ms.GetBuffer()); //将stream读取成一个字符串形式的数据，并且返回
                    ms.Close();
                }
            }
            catch
            {
                json = string.Empty;
            }
            return json;
        }

        /// <summary>
        /// json字符串转换成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json">要转换成对象的json字符串</param>
        /// <returns></returns>
        public static T Deserialize<T>(string json)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            T obj = default(T);
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                obj = (T)serializer.ReadObject(ms);
                ms.Close();
            }
            return obj;
        }

        #endregion
    }
}