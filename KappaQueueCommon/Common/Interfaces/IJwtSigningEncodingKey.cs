using Microsoft.IdentityModel.Tokens;

namespace KappaQueueCommon.Common.Interfaces
{
    public interface IJwtSigningEncodingKey
    {
        string SigningAlgorithm { get; }

        SecurityKey GetKey();
    }
}
