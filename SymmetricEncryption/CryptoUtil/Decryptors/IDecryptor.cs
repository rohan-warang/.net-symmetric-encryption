
namespace CryptoUtil.Decryptors
{
    public interface IDecryptor<in Tin, out Tout>
    {
        Tout Decrypt(Tin input, string password);
    }
}
