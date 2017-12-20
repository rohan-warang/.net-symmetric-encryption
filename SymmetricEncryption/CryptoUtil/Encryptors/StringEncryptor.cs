using CryptoUtil.Algorithms;
using CryptoUtil.CryptoByteGenerators;
using CryptoUtil.KeyGenerators;
using CryptoUtil.Settings;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CryptoUtil.Encryptors
{
    public class StringEncryptor : IEncryptor<string, string>
    {
        private readonly ICryptoSettings cryptoSettings;
        private readonly IAlgorithmFactory algorithmFactory;
        private readonly IKeyGenerator keyGenerator;
        private readonly ICryptoByteGenerator cryptoByteGenerator;

        public StringEncryptor(
            ICryptoSettings cryptoSettings,
            IAlgorithmFactory algorithmFactory,
            IKeyGenerator keyGenerator,
            ICryptoByteGenerator cryptoByteGenerator)
        {
            this.cryptoSettings = cryptoSettings;
            this.algorithmFactory = algorithmFactory;
            this.keyGenerator = keyGenerator;
            this.cryptoByteGenerator = cryptoByteGenerator;
        }

        public string Encrypt(string input, string password)
        {
            byte[] iv = cryptoByteGenerator.Generate(cryptoSettings.IvSize);
            byte[] salt = cryptoByteGenerator.Generate(cryptoSettings.SaltSize);
            byte[] key = keyGenerator.Generate(password, salt);
            byte[] encyptedInput;

            using (var algorithm = algorithmFactory.Build())
            {
                var encryptor = algorithm.CreateEncryptor(key, iv);

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
            return Encoding.UTF8.GetString(output);
        }
    }
}
