using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace KappaQueueCommon.Common.DTO
{
    public class AuthResponseDto
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("token_ttl")]
        public int TokenActiveTime { get; set; }
    }
}
