﻿using System.IO;

namespace InstagramPhotos.Utility.IO
{
    public static class StreamHelper
    {
        public static byte[] GetByteArrayFromStream(Stream input)
        {
            if (input is MemoryStream)
            {
                return ((MemoryStream) input).ToArray();
            }
            // Jon Skeet's accepted answer  
            return ReadFully(input);
        }

        public static byte[] ReadFully(Stream input)
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
    }
}