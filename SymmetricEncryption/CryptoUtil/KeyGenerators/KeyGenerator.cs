using CryptoUtil.Settings;
using System.Security.Cryptography;

namespace CryptoUtil.KeyGenerators
{
    public class KeyGenerator : IKeyGenerator
    {
        ICryptoSettings cryptoSettings;

        public KeyGenerator(ICryptoSettings cryptoSettings)
        {
            this.cryptoSettings = cryptoSettings;
        }

        public byte[] Generate(string password, byte[] salt)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, cryptoSettings.KeyIterations);
            return pbkdf2.GetBytes(cryptoSettings.KeySize);
        }
    }
}
