
namespace CryptoUtil.KeyGenerators
{
    public interface IKeyGenerator
    {
        byte[] Generate(string password, byte[] salt);
    }
}
