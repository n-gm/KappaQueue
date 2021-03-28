using System;
using System.Text.Json.Serialization;

namespace KappaQueue.Common.DTO
{
    /// <summary>
    /// Создание пользователя
    /// </summary>
    public class UserAddDto
    {
        private Guid _guid = Guid.NewGuid();
        /// <summary>
        /// Логин пользователя
        /// </summary>
        /// <example>Username</example>
        [JsonPropertyName("username")]
        public string Username { get; set; }
        /// <summary>
        /// Пароль пользователя
        /// </summary>
        /// <example>Password</example>
        public string Password { get; set; }
        /// <summary>
        /// Имя пользователя
        /// </summary>
        /// <example>Иван</example>
        public string FirstName { get; set; }
        /// <summary>
        /// Отчество пользователя
        /// </summary>
        /// <example>Иванович</example>
        public string MiddleName { get; set; }
        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        /// <example>Иванов</example>
        public string LastName { get; set; }
        /// <summary>
        /// Guid пользователя
        /// </summary>
        [JsonIgnore]
        public Guid Guid { get => _guid; }
    }
}
