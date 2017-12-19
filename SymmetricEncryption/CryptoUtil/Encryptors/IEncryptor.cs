
namespace CryptoUtil.Encryptors
{
    public interface IEncryptor<in Tin, out Tout>
    {
        Tout Encrypt(Tin input, string password);
    }
}
