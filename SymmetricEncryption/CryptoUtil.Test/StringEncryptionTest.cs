using CryptoUtil.Algorithms;
using CryptoUtil.Decryptors;
using CryptoUtil.Encryptors;
using CryptoUtil.Settings;
using NUnit.Framework;
using System.Security.Cryptography;

namespace CryptoUtil.Test
{
    [TestFixture]
    public class StringEncryptionTest
    {
        private IEncryptor<string, string> encryptor;
        private IDecryptor<string, string> decryptor;

        private class Settings : ICryptoSettings
        {
            public int SaltSize => 8;
            public int KeySize => 32;
            public int IvSize => 16;
        }

        [SetUp]
        public void Setup()
        {
            IAlgorithmFactory algorithmFactory = new GenericAlgorithmFactory<AesManaged>();
            ICryptoSettings settings = new Settings();

            encryptor = new StringEncryptor(algorithmFactory, settings);
            decryptor = new StringDecryptor(algorithmFactory, settings);
        }

        [Test]
        public void EncryptSameInputProducesDifferentOutputs()
        {
            const string inputString = "Test String";
            const string password = "P@s5w0rd";

            var encryptedString1 = encryptor.Encrypt(inputString, password);
            var encryptedString2 = encryptor.Encrypt(inputString, password);

            Assert.AreNotEqual(encryptedString1, encryptedString2);
        }

        [Test]
        public void DecryptedOutputStringMatchesInputString()
        {
            const string inputString = "Test String";
            const string password = "P@s5w0rd";

            var encryptedString = encryptor.Encrypt(inputString, password);
            var decryptedString = decryptor.Decrypt(encryptedString, password);

            Assert.AreEqual(decryptedString, inputString);
        }
    }
}
