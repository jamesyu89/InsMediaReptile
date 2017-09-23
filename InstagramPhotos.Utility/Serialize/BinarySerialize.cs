using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace InstagramPhotos.Utility.Serialize
{
    public class BinaryHelper
    {
        /// <summary>
        /// 将对象序列化为byte[]
        /// 使用IFormatter的Serialize序列化
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <returns>序列化获取的二进制流</returns>
        public static byte[] FormatterObjectBytes(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            byte[] buff;
            try
            {
                using (var ms = new MemoryStream())
                {
                    IFormatter iFormatter = new BinaryFormatter();
                    iFormatter.Serialize(ms, obj);
                    buff = ms.GetBuffer();
                }
            }
            catch (Exception er)
            {
                throw new Exception(er.Message);
            }
            return buff;
        }


        /// <summary>
        /// 将对象转为二进制文件，并保存到指定的文件中
        /// </summary>
        /// <param name="name">文件路径</param>
        /// <param name="obj">待存的对象</param>
        /// <returns></returns>
        public static bool BinaryFileSave(string name, object obj)
        {
            Stream flstr = null;
            BinaryWriter binaryWriter = null;
            try
            {
                flstr = new FileStream(name, FileMode.Create);
                binaryWriter = new BinaryWriter(flstr);
                var buff = FormatterObjectBytes(obj);
                binaryWriter.Write(buff);
            }
            catch (Exception er)
            {
                throw new Exception(er.Message);
            }
            finally
            {
                if (binaryWriter != null) binaryWriter.Close();
                if (flstr != null) flstr.Close();
            }
            return true;
        }

        /// <summary>
        /// 将byte[]反序列化为对象
        /// 使用IFormatter的Deserialize发序列化
        /// </summary>
        /// <param name="buff">传入的byte[]</param>
        /// <returns></returns>
        public static object FormatterByteObject(byte[] buff)
        {
            if (buff == null)
                throw new ArgumentNullException("buff");
            object obj;
            try
            {
                using (var ms = new MemoryStream())
                {
                    IFormatter iFormatter = new BinaryFormatter();
                    obj = iFormatter.Deserialize(ms);
                }
            }
            catch (Exception er)
            {
                throw new Exception(er.Message);
            }
            return obj;
        }


        /// <summary>
        /// 将对象序列化为byte[]
        /// 使用Marshal的StructureToPtr序列化
        /// </summary>
        /// <param name="obj">需序列化的对象</param>
        /// <returns>序列化后的byte[]</returns>
        public static byte[] MarshalObjectByte(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");
            byte[] buff;
            try
            {
                buff = new byte[Marshal.SizeOf(obj)];
                var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(buff, 0);
                Marshal.StructureToPtr(obj, ptr, true);
            }
            catch (Exception er)
            {
                throw new Exception(er.Message);
            }
            return buff;
        }

        /// <summary>
        /// 将byte[]序列化为对象
        /// </summary>
        /// <param name="buff">被转换的二进制流</param>
        /// <param name="type">转换成的类名</param>
        /// <returns></returns>
        public static object MarshalByteObject(byte[] buff, Type type)
        {
            if (buff == null)
                throw new ArgumentNullException("buff");
            if (type == null)
                throw new ArgumentNullException("type");
            try
            {
                var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(buff, 0);
                return Marshal.PtrToStructure(ptr, type);
            }
            catch (Exception er)
            {
                throw new Exception(er.Message);
            }
        }


        /// <summary>
        /// 将文件转换为byte数组
        /// </summary>
        /// <param name="path">文件地址</param>
        /// <returns>转换后的byte[]</returns>
        public static byte[] FileObjectBytes(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");
            if (!File.Exists(path)) return new byte[0];
            try
            {
                var fi = new FileInfo(path);
                var buff = new byte[fi.Length];
                var fs = fi.OpenRead();
                fs.Read(buff, 0, Convert.ToInt32(fs.Length));
                fs.Close();
                return buff;
            }
            catch (Exception er)
            {
                throw new Exception(er.Message);
            }
        }


        /// <summary>
        /// 将byte[]转换为文件并保存到指定的地址
        /// </summary>
        /// <param name="buff">需反序列化的byte[]</param>
        /// <param name="savePath">文件保存的路径</param>
        /// <returns>是否成功</returns>
        public static string FileByteObject(byte[] buff, string savePath)
        {
            if (buff == null)
                throw new ArgumentNullException("buff");
            if (savePath == null)
                throw new ArgumentNullException("savePath");
            if (File.Exists(savePath)) return "文件名重复";
            try
            {
                var fs = new FileStream(savePath, FileMode.CreateNew);
                var bw = new BinaryWriter(fs);
                bw.Write(buff, 0, buff.Length);
                bw.Close();
                fs.Close();
            }
            catch (Exception er)
            {
                throw new Exception(er.Message);
            }
            return "保存成功";
        }


        /// <summary>
        /// 将图片序列化为二进制流
        /// </summary>
        /// <param name="imgPath">图片路径</param>
        /// <returns>序列化后的二进制流</returns>
        public static byte[] SetImgToBytes(string imgPath)
        {
            if (string.IsNullOrEmpty(imgPath))
                throw new ArgumentNullException(imgPath);
            try
            {
                byte[] byteData;
                using (var file = new FileStream(imgPath, FileMode.Open, FileAccess.Read))
                {
                    byteData = new byte[file.Length];
                    file.Read(byteData, 0, byteData.Length);
                    file.Close();
                }
                return byteData;
            }
            catch (Exception er)
            {

                throw new Exception(er.Message);
            }
        }



    }
}
