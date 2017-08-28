using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace InstagramPhotos.Utility.IO
{
    public class GZipHelper
    {
        /// <summary>
        ///     压缩字符窜
        /// </summary>
        /// <param name="unCompressedString">原始字符窜</param>
        /// <returns>压缩后的字符窜</returns>
        public static string ZipString(string unCompressedString)
        {
            byte[] bytData = Encoding.UTF8.GetBytes(unCompressedString);
            var ms = new MemoryStream();
            Stream s = new GZipStream(ms, CompressionMode.Compress);
            s.Write(bytData, 0, bytData.Length);
            s.Close();
            byte[] compressedData = ms.ToArray();
            return Convert.ToBase64String(compressedData, 0, compressedData.Length);
        }

        /// <summary>
        ///     解压缩字符窜
        /// </summary>
        /// <param name="unCompressedString">压缩后的字符窜</param>
        /// <returns>解压缩后的字符窜</returns>
        public static string UnzipString(string unCompressedString)
        {
            byte[] bytData = Convert.FromBase64String(unCompressedString);

            Stream s = new GZipStream(new MemoryStream(bytData), CompressionMode.Decompress);
            var sw = new StreamReader(s);
            string str = sw.ReadToEnd();
            sw.Close();
            s.Close();

            return str;
        }

        /// <summary>
        ///     压缩二进制数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] ZipData(byte[] data)
        {
            using (var ms = new MemoryStream())
            {
                using (Stream s = new GZipStream(ms, CompressionMode.Compress))
                {
                    s.Write(data, 0, data.Length);
                    byte[] compressedData = ms.ToArray();
                    return compressedData;
                }
            }
        }

        /// <summary>
        ///     解压缩二进制数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] UnzipData(byte[] data)
        {
            using (Stream s = new GZipStream(new MemoryStream(data), CompressionMode.Decompress))
            {
                byte[] result = ReadFully(s);
                return result;
            }
        }

        private static byte[] ReadFully(Stream input)
        {
            var buffer = new byte[16*1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        ///     用.net自带的Gzip对二进制数组进行压缩,压缩比率可能不是太好
        /// </summary>
        /// <param name="data">二进制数组</param>
        /// <returns>压缩后二进制数组</returns>
        public static byte[] Compress(byte[] data)
        {
            using (var ms = new MemoryStream())
            {
                Stream zipStream = new GZipStream(ms, CompressionMode.Compress, true);
                zipStream.Write(data, 0, data.Length);
                zipStream.Close();
                ms.Position = 0;
                var compressed_data = new byte[ms.Length];
                ms.Read(compressed_data, 0, int.Parse(ms.Length.ToString(CultureInfo.InvariantCulture)));
                return compressed_data;
            }
        }

        /// <summary>
        ///     对二进制数组进行解压缩
        /// </summary>
        /// <param name="data">二进制数组</param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] data)
        {
            using (var zipMs = new MemoryStream(data))
            {
                byte[] buffer = EtractBytesFormStream(zipMs, data.Length);

                return buffer;
            }
        }

        public static byte[] EtractBytesFormStream(MemoryStream zipMs, int dataBlock)
        {
            byte[] data = null;
            int totalBytesRead = 0;
            //Stream zipStream = null;
            using (Stream zipStream = new GZipStream(zipMs, CompressionMode.Decompress))
            {
                while (true)
                {
                    Array.Resize(ref data, totalBytesRead + dataBlock + 1);
                    int bytesRead = zipStream.Read(data, totalBytesRead, dataBlock);
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    totalBytesRead += bytesRead;
                }
                Array.Resize(ref data, totalBytesRead);
                return data;
            }
        }
    }
}