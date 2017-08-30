using System.Text;

//using Org.BouncyCastle.Crypto;
//using Org.BouncyCastle.Crypto.Engines;
//using Org.BouncyCastle.Crypto.Paddings;
//using Org.BouncyCastle.Crypto.Parameters;

namespace InstagramPhotos.Utility.Security.DES
{
    public class DESUtil
    {
      //  /// <summary>
      //  ///     加密方法
      //  /// </summary>
      //  /// <param name="text">明文</param>
      //  /// <param name="key">密钥 BASE64</param>
      //  /// <returns></returns>
      //  public static String encrypt(String text, String key)
      //  {
      //      IBlockCipher engine = new DesEngine();
      //      BufferedBlockCipher cipher = new PaddedBufferedBlockCipher(engine);
      //      byte[] inputByteArray = Encoding.UTF8.GetBytes(text);
      //      //true表示加密
      //      cipher.Init(true, new KeyParameter(Convert.FromBase64String(key)));

      //      var cipherText = new byte[cipher.GetOutputSize(inputByteArray.Length)];
      //      int outputLen = cipher.ProcessBytes(inputByteArray, 0, inputByteArray.Length, cipherText, 0);
      //      //加密
      //      cipher.DoFinal(cipherText, outputLen);
      //      String decryptString = Convert.ToBase64String(cipherText);

      //      return decryptString;
      //  }

      //  /**
      //* 解密方法
      //*
      //* @param text    密文
      //* @param key     密钥 BASE64
      //* @param charset 字符集
      //* @return 明文
      //* @throws Exception
      //*/

      //  public static String decrypt(String text, byte[] key)
      //  {
      //      byte[] inputByteArray = Convert.FromBase64String(text);

      //      IBlockCipher engine = new DesEngine();
      //      var cipher = new BufferedBlockCipher(engine);
      //      //false表示解密
      //      cipher.Init(false, new KeyParameter(key));

      //      var cipherText = new byte[cipher.GetOutputSize(inputByteArray.Length)];
      //      int outputLen = cipher.ProcessBytes(inputByteArray, 0, inputByteArray.Length, cipherText, 0);
      //      //解密
      //      cipher.DoFinal(cipherText, outputLen);
      //      String decryptString = Encoding.UTF8.GetString(cipherText);
      //      decryptString = ReplaceLowOrderASCIICharacters(decryptString);

      //      return decryptString;
      //  }

        public static string ReplaceLowOrderASCIICharacters(string tmp)
        {
            var info = new StringBuilder();
            foreach (char cc in tmp)
            {
                int ss = cc;
                if (((ss >= 0) && (ss <= 8)) || ((ss >= 11) && (ss <= 12)) || ((ss >= 14) && (ss <= 32)))
                    info.AppendFormat(" ", ss); //&#x{0:X};
                else info.Append(cc);
            }
            return info.ToString();
        }
    }
}