using System.Security.Cryptography;

namespace CryptoUtil.Algorithms
{
    public class GenericAlgorithmFactory<T> : IAlgorithmFactory where T : SymmetricAlgorithm, new()
    {
        public SymmetricAlgorithm Build()
        {
            return new T();
        }
    }
}
