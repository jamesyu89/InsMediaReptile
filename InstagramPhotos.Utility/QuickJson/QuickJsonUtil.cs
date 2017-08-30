namespace InstagramPhotos.Utility.QuickJson
{
    public class QuickJsonUtil
    {
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
                var builder = new JsonBuilder();
                json = builder.ConvertToJsonString(jsonObject);
            }
            catch
            {
                json = string.Empty;
            }
            return json;
        }
    }
}
