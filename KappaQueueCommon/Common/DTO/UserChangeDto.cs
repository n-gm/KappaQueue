namespace KappaQueueCommon.Common.DTO
{
    public class UserChangeDto
    {
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
        /// Признак блокировки пользователя
        /// </summary>
        public bool Blocked { get; set; } = false;
    }
}
