using Newtonsoft.Json;

namespace KappaQueue.Common.DTO
{
    /// <summary>
    /// Класс для хранения информации для авторизации
    /// </summary>
    public class AuthorizationNode
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
