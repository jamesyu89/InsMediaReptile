using YGCFDDecrypt;

namespace InstagramPhotos.Utility.YIGUO
{
    public static class Functions
    {
        public static string SQLConnectDecrypt(string EncryptStr)
        {
            return Crypto.DecryptString(EncryptStr);
        }
    }
}
