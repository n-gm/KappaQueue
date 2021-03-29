using System.Text.Json.Serialization;

namespace KappaQueueCommon.Common.DTO
{
    /// <summary>
    /// Класс для хранения информации для авторизации
    /// </summary>
    public class AuthorizationNode
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        [JsonPropertyName("username")]
        public string Username { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
