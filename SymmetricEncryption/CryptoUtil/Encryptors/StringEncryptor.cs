using CryptoUtil.Algorithms;
using CryptoUtil.Settings;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CryptoUtil.Encryptors
{
    public class StringEncryptor : IEncryptor<string, string>
    {
        private readonly IAlgorithmFactory algorithmFactory;
        private readonly ICryptoSettings settings;

        public StringEncryptor(IAlgorithmFactory algorithmFactory, ICryptoSettings settings)
        {
            this.settings = settings;
            this.algorithmFactory = algorithmFactory;
        }

        public string Encrypt(string input, string password)
        {
            byte[] iv = new byte[settings.IvSize];
            byte[] salt = new byte[settings.SaltSize];
            
            // Generate random iv and salt
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(iv);
                rng.GetBytes(salt);
            }

            // Generate key for decryption
            byte[] key = new Rfc2898DeriveBytes(password, salt, 100).GetBytes(settings.KeySize);

            byte[] encyptedInput;
            using (var algorithm = algorithmFactory.Build())
            {
                algorithm.IV = iv;
                algorithm.Key = key;

                var encryptor = algorithm.CreateEncryptor();

                using (MemoryStream outputStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(outputStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter writer = new StreamWriter(cryptoStream))
                        {
                            writer.Write(input);
                        }

                        encyptedInput = outputStream.ToArray();
                    }
                }
            }

            // Concat iv + salt + encrypted input
            var output = iv.Concat(salt.Concat(encyptedInput)).ToArray();
            return Convert.ToBase64String(output);
        }
    }
}
