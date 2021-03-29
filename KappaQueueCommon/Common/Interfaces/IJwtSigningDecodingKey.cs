using Microsoft.IdentityModel.Tokens;

namespace KappaQueueCommon.Common.Interfaces
{
    public interface IJwtSigningDecodingKey
    {
        SecurityKey GetKey();
    }
}
