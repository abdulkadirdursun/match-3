using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Match3.SaveSystem
{
    public static class Cryptography
    {
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("32characterherepiyhcvb5asdjfbnej");

        public static string Encrypt(string json)
        {
            using var aes = Aes.Create();
            aes.Key = Key;
            aes.GenerateIV();

            using var ms = new MemoryStream();
            ms.Write(aes.IV, 0, aes.IV.Length);

            using (var crypto = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            using (var sw = new StreamWriter(crypto))
            {
                sw.Write(json);
            }

            return Convert.ToBase64String(ms.ToArray());
        }

        public static string Decrypt(string cipherText)
        {
            byte[] data = Convert.FromBase64String(cipherText);
            using var aes = Aes.Create();
            aes.Key = Key;

            byte[] iv = new byte[aes.IV.Length];
            Array.Copy(data, 0, iv, 0, iv.Length);
            aes.IV = iv;

            using var ms = new MemoryStream(data, iv.Length, data.Length - iv.Length);
            using var crypto = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using var sr = new StreamReader(crypto);
            return sr.ReadToEnd();
        }
    }
}