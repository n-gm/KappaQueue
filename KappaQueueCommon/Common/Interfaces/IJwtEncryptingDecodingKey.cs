using Microsoft.IdentityModel.Tokens;

namespace KappaQueueCommon.Common.Interfaces
{
    public interface IJwtEncryptingDecodingKey
    {
        SecurityKey GetKey();
    }
}
