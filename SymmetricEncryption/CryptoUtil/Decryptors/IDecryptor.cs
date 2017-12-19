
namespace CryptoUtil.Decryptors
{
    public interface IDecryptor<in Tin, out Tout>
    {
        Tout Encrypt(Tin input, string password);
    }
}
