using System.IO;

namespace InstagramPhotos.Utility.Extension
{
    public static class StreamExtension
    {
        public static byte[] ToBytes(this Stream target)
        {
            byte[] content;

            if (target == null)
                content = new byte[0];
            else if (target is MemoryStream)
                content = ((MemoryStream)target).ToArray();
            else
            {
                content = new byte[target.Length];
                target.Position = 0;
                target.Read(content, 0, (int)target.Length);
            }
            return content;
        }
    }
}
