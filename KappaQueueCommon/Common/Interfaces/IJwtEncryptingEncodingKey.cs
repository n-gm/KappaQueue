using Microsoft.IdentityModel.Tokens;

namespace KappaQueueCommon.Common.Interfaces
{
    public interface IJwtEncryptingEncodingKey
    {
        string SigningAlgorithm { get; }

        string EncryptingAlgorithm { get; }

        SecurityKey GetKey();
    }
}
