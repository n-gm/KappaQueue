using System;
using System.Text.Json.Serialization;

namespace KappaQueueCommon.Common.DTO
{
    /// <summary>
    /// Создание пользователя
    /// </summary>
    public class UserAddDto : UserChangeDto
    {
        /// <summary>
        /// Логин пользователя
        /// </summary>
        /// <example>Username</example>
        [JsonPropertyName("username")]
        public string Username { get; set; }      
    }
}
