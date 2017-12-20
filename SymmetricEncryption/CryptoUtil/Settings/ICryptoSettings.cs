
namespace CryptoUtil.Settings
{
    public interface ICryptoSettings
    {
        int IvSize { get; }
        int SaltSize { get; }
        int KeySize { get; }
    }
}
