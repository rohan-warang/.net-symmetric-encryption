using CryptoUtil.Algorithms;
using CryptoUtil.Settings;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace CryptoUtil.Decryptors
{
    public class StringDecryptor : IDecryptor<string, string>
    {
        private readonly IAlgorithmFactory algorithmFactory;
        private readonly ICryptoSettings settings;

        public StringDecryptor(IAlgorithmFactory algorithmFactory, ICryptoSettings settings)
        {
            this.settings = settings;
            this.algorithmFactory = algorithmFactory;
        }

        public string Decrypt(string input, string password)
        {
            byte[] inputBytes = Convert.FromBase64String(input);

            // Split iv, salt and encrypted data from the input
            byte[] iv = inputBytes.Take(settings.IvSize).ToArray();
            byte[] salt = inputBytes.Skip(settings.IvSize).Take(settings.SaltSize).ToArray(); 
            byte[] encyptedInput = inputBytes.Skip(settings.IvSize + settings.SaltSize).ToArray();

            // Generate key for decryption
            byte[] key = new Rfc2898DeriveBytes(password, salt, 100).GetBytes(settings.KeySize);

            using (var algorithm = algorithmFactory.Build())
            {
                algorithm.IV = iv;
                algorithm.Key = key;

                var decryptor = algorithm.CreateDecryptor();

                using (var outputStream = new MemoryStream(encyptedInput))
                {
                    using (var cryptoStream = new CryptoStream(outputStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var reader = new StreamReader(cryptoStream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
