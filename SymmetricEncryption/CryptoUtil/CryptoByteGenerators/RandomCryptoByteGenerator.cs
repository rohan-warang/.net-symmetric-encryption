using System.Security.Cryptography;

namespace CryptoUtil.CryptoByteGenerators
{
    public class RandomCryptoByteGenerator : ICryptoByteGenerator
    {
        public byte[] Generate(int size)
        {
            var salt = new byte[size];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            return salt;
        }
    }
}
