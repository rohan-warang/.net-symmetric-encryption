using System.Security.Cryptography;

namespace CryptoUtil.Algorithms
{
    public interface IAlgorithmFactory
    {
        SymmetricAlgorithm Build();
    }
}
