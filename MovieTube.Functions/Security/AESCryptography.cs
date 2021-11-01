using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MovieTube.Functions.Security
{
    public class AESCryptography
    {
        private static string AesIV128 = Common.AES_IV;  //16 chars = 128 bits

        private static string AesKey128 = Common.AES_KEY;  //16 chars = 128 bits

        public static string Encrypt(string decrypted)
        {
            // AesCryptoServiceProvider
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 128;
            aes.IV = UTF8Encoding.UTF8.GetBytes(AesIV128);
            aes.Key = UTF8Encoding.UTF8.GetBytes(AesKey128);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            byte[] data = Encoding.UTF8.GetBytes(decrypted);
            using (ICryptoTransform encrypt = aes.CreateEncryptor())
            {
                byte[] dest = encrypt.TransformFinalBlock(data, 0, data.Length);
                return Convert.ToBase64String(dest);
            }
        }

        public static string Decrypt(string encrypted)
        {
            // AesCryptoServiceProvider
            string plaintext = null;
            using (AesManaged aes = new AesManaged())
            {
                byte[] cipherText = Convert.FromBase64String(encrypted);
                byte[] aesIV = UTF8Encoding.UTF8.GetBytes(AesIV128);
                byte[] aesKey = UTF8Encoding.UTF8.GetBytes(AesKey128);
                ICryptoTransform decryptor = aes.CreateDecryptor(aesKey, aesIV);
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;
        }
    }
}
