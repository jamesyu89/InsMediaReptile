using System.IO;
using System.Text;

namespace InstagramPhotos.Utility.IO
{
    public static class FileOperationHelper
    {
        public static string ReadToEnd(string path)
        {
            FileStream fin;
            if (!File.Exists(path)) fin = File.Create(path);
            else fin = new FileStream(path, FileMode.Open, FileAccess.Read);
            var brin = new StreamReader(fin, Encoding.Default);
            string s = brin.ReadToEnd();
            brin.Close();
            return s;
        }

        public static string ReadFirstLine(string path)
        {
            FileStream fin;
            if (!File.Exists(path)) fin = File.Create(path);
            else fin = new FileStream(path, FileMode.Open, FileAccess.Read);
            var brin = new StreamReader(fin, Encoding.Default);
            string s = brin.ReadLine();
            brin.Close();
            return s;
        }

        public static void Save(string str, string path)
        {
            var fout = new FileStream(path, FileMode.Create, FileAccess.Write);
            var brout = new StreamWriter(fout, Encoding.Default);
            brout.Write(str);
            brout.Close();
        }

        public static void Save(byte[] bytes, string path, string filename)
        {
            string FullPath = path + filename;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var fs = new FileStream(FullPath, FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
        }
    }
}