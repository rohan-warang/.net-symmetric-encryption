using CryptoUtil.Algorithms;
using CryptoUtil.KeyGenerators;
using CryptoUtil.Settings;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CryptoUtil.Decryptors
{
    public class StringDecryptor : IDecryptor<string, string>
    {
        private readonly ICryptoSettings cryptoSettings;
        private readonly IAlgorithmFactory algorithmFactory;
        private readonly IKeyGenerator keyGenerator;

        public StringDecryptor(
            ICryptoSettings cryptoSettings,
            IAlgorithmFactory algorithmFactory,
            IKeyGenerator keyGenerator)
        {
            this.cryptoSettings = cryptoSettings;
            this.algorithmFactory = algorithmFactory;
            this.keyGenerator = keyGenerator;
        }

        public string Encrypt(string input, string password)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] iv = inputBytes.Take(cryptoSettings.IvSize).ToArray();
            byte[] salt = inputBytes.Skip(cryptoSettings.IvSize).Take(cryptoSettings.SaltSize).ToArray(); 
            byte[] key = keyGenerator.Generate(password, salt);
            byte[] encyptedInput = inputBytes.Skip(cryptoSettings.IvSize + cryptoSettings.SaltSize).ToArray();

            using (var algorithm = algorithmFactory.Build())
            {
                var decryptor = algorithm.CreateDecryptor(key, iv);

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
