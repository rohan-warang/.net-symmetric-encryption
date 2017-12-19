
namespace CryptoUtil.Settings
{
    public interface ICryptoSettings
    {
        int SaltSize { get; }
        int KeyIterations { get; }
        int KeySize { get; }      
        int IvSize { get; }
    }
}
