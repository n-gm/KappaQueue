using Microsoft.IdentityModel.Tokens;

namespace KappaQueue.Common.Interfaces
{
    public interface IJwtSigningDecodingKey
    {
        SecurityKey GetKey();
    }
}
