using Microsoft.IdentityModel.Tokens;

namespace KappaQueue.Common.Interfaces
{
    public interface IJwtSigningEncodingKey
    {
        string SigningAlgorithm { get; }

        SecurityKey GetKey();
    }
}
