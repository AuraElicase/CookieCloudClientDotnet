using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CookieCloudClientDotnet.Infrastructure.Tools
{
    internal static class BinaryFunc
    {
        internal static byte[] BytesToKey(byte[] data, byte[] salt, int outputLength = 48)
        {
            if (salt.Length != 8)
                throw new ArgumentException("Salt must be exactly 8 bytes long.");

            // Append salt to data
            byte[] dataWithSalt = new byte[data.Length + salt.Length];
            Buffer.BlockCopy(data, 0, dataWithSalt, 0, data.Length);
            Buffer.BlockCopy(salt, 0, dataWithSalt, data.Length, salt.Length);
            byte[] key = MD5.HashData(dataWithSalt);
            byte[] finalKey = new byte[key.Length];
            Buffer.BlockCopy(key, 0, finalKey, 0, key.Length);

            while (finalKey.Length < outputLength)
            {
                byte[] newData = new byte[key.Length + dataWithSalt.Length];
                Buffer.BlockCopy(key, 0, newData, 0, key.Length);
                Buffer.BlockCopy(dataWithSalt, 0, newData, key.Length, dataWithSalt.Length);

                key = MD5.HashData(newData);

                byte[] extendedKey = new byte[finalKey.Length + key.Length];
                Buffer.BlockCopy(finalKey, 0, extendedKey, 0, finalKey.Length);
                Buffer.BlockCopy(key, 0, extendedKey, finalKey.Length, key.Length);

                finalKey = extendedKey;
            }

            // Trim the final key to desired output length
            byte[] result = new byte[outputLength];
            Buffer.BlockCopy(finalKey, 0, result, 0, outputLength);
            return result;
        }

        internal static byte[] Unpad(byte[] data)
        {
            int paddingLength = data[^1]; // last byte
            if (paddingLength <= 0 || paddingLength > 16)
                throw new Exception("Invalid PKCS7 padding.");

            return data.Take(data.Length - paddingLength).ToArray();
        }
    }
}
