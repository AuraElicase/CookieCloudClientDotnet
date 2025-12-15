using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CookieCloudClientDotnet.Infrastructure.Security
{
    internal static class AES
    {
        internal static string Decrypt(string encryptedBase64, string passphrase)
        {
            byte[] encryptedData = Convert.FromBase64String(encryptedBase64);
            // Check for "Salted__" prefix
            byte[] saltHeader = encryptedData.Take(8).ToArray();
            if (!Encoding.ASCII.GetString(saltHeader).Equals("Salted__"))
                throw new Exception("Invalid OpenSSL salt header.");

            byte[] salt = encryptedData.Skip(8).Take(8).ToArray();
            byte[] keyIv = Tools.BinaryFunc.BytesToKey(Encoding.UTF8.GetBytes(passphrase), salt, 32 + 16);
            byte[] key = keyIv.Take(32).ToArray();
            byte[] iv = keyIv.Skip(32).Take(16).ToArray();

            byte[] ciphertext = encryptedData.Skip(16).ToArray();

            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.None;

                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                {
                    byte[] decrypted = decryptor.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
                    return Encoding.UTF8.GetString(Tools.BinaryFunc.Unpad(decrypted));
                }
            }
        }
    }
}
