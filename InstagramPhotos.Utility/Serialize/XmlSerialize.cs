using System.IO;
using System.Xml.Serialization;

namespace InstagramPhotos.Utility.Serialize
{
    public class XmlSerialize
    {
        /// <summary>
        ///     反序列化XML为类实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlObj"></param>
        /// <returns></returns>
        public static T DeserializeXML<T>(string xmlObj)
        {
            var serializer = new XmlSerializer(typeof (T));
            using (var reader = new StringReader(xmlObj))
            {
                return (T) serializer.Deserialize(reader);
            }
        }

        /// <summary>
        ///     序列化类实例为XML
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeXML<T>(T obj)
        {
            using (var writer = new StringWriter())
            {
                new XmlSerializer(obj.GetType()).Serialize(writer, obj);
                return writer.ToString();
            }
        }
    }
}