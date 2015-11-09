using System.Security.Cryptography;
using System.Text;

namespace X.Web.Util
{
    public sealed class RsaCryption
    {
        /// <summary>
        /// 加密成16进制数
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="content"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string Encrypt(RSAParameters publicKey, string content, int size = 1024)
        {
            var rsa = new RSACryptoServiceProvider(size);
            rsa.ImportParameters(publicKey);
            var cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);
            return StringConvert.Bytes2Hex(cipherbytes);
        }

        /// <summary>
        /// 加密成base64字符串
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="content"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string Encrypt2Base64(RSAParameters publicKey, string content, int size = 1024)
        {
            var rsa = new RSACryptoServiceProvider(size);
            rsa.ImportParameters(publicKey);
            var cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);
            return StringConvert.Bytes2Base64(cipherbytes);
        }

        /// <summary>
        /// 从16进制数解密
        /// </summary>
        /// <param name="privateKey"></param>
        /// <param name="content"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string Decrypt(RSAParameters privateKey, string content, int size = 1024)
        {
            var rsa = new RSACryptoServiceProvider(size);
            rsa.ImportParameters(privateKey);
            var cipherbytes = rsa.Decrypt(StringConvert.Hex2Bytes(content), false);
            return Encoding.UTF8.GetString(cipherbytes);
        }

        /// <summary>
        /// 从base64字符串解密
        /// </summary>
        /// <param name="privateKey"></param>
        /// <param name="content"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string DecryptFromBase64(RSAParameters privateKey, string content, int size = 1024)
        {
            var rsa = new RSACryptoServiceProvider(size);
            rsa.ImportParameters(privateKey);
            var cipherbytes = rsa.Decrypt(StringConvert.Base64ToBytes(content), false);
            return Encoding.UTF8.GetString(cipherbytes);
        }
    }
}
