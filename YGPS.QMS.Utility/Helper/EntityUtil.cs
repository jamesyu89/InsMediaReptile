using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace InstagramPhotos.Utility.Helper
{
    public class EntityUtil
    {
        /// <summary>
        /// （深）克隆对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static T Clone<T>(T t) where T : new()
        {
            if (t == null)
            {
                return t;
            }

            using (var memStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
                binaryFormatter.Serialize(memStream, t);
                memStream.Seek(0, SeekOrigin.Begin);

                return (T)binaryFormatter.Deserialize(memStream);
            }
        }
    }
}
