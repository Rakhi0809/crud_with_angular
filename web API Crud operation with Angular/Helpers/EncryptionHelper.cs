using System.Security.Cryptography;
using System.Text;
using System.IO;
using System;

namespace web_API_Crud_operation_with_Angular.Helpers
{
    public class EncryptionHelper
    {
        private readonly string _key; // Encryption Key

        public EncryptionHelper(string key)
        {
            if (string.IsNullOrEmpty(key) || key.Length != 32)
                throw new ArgumentException("Encryption key must be 32 characters long.");

            _key = key;
        }

        // Encrypt method
        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                throw new ArgumentNullException(nameof(plainText), "Input text cannot be null or empty.");

            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(_key);

            // Generate a random IV for each encryption operation
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }

            // Combine IV and encrypted text
            byte[] ivAndCipherText = new byte[aes.IV.Length + ms.ToArray().Length];
            Buffer.BlockCopy(aes.IV, 0, ivAndCipherText, 0, aes.IV.Length);
            Buffer.BlockCopy(ms.ToArray(), 0, ivAndCipherText, aes.IV.Length, ms.ToArray().Length);

            return Convert.ToBase64String(ivAndCipherText);
        }

        // Decrypt method
        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                throw new ArgumentNullException(nameof(cipherText), "Input text cannot be null or empty.");

            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(_key);

            // Extract the IV from the beginning of the cipherText
            byte[] iv = new byte[16]; // AES block size
            Buffer.BlockCopy(cipherBytes, 0, iv, 0, iv.Length);
            aes.IV = iv;

            // Extract the actual cipher text (after the IV)
            byte[] actualCipherText = new byte[cipherBytes.Length - iv.Length];
            Buffer.BlockCopy(cipherBytes, iv.Length, actualCipherText, 0, actualCipherText.Length);

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(actualCipherText);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
    }
}
