using KappaQueueCommon.Common.DTO;
using KappaQueueCommon.Models.Clients;
using KappaQueueCommon.Models.Context;
using KappaQueueCommon.Models.Positions;
using KappaQueueCommon.Models.Rooms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace KappaQueueCommon.Models.Users
{
    /// <summary>
    /// Модель для пользователя
    /// </summary>
    [Table("users")]
    [Index("Username", IsUnique = true, Name = "IX_Users_Username")]
    public class User : QueueEntity
    {
        [Column("password")]
        [MaxLength(64)]
        [Required]
        [JsonIgnore]
        [BindRequired]
        [BindProperty]
        public string _password { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        /// <example>1</example>
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Key]
        [JsonPropertyName("id")]
        [BindNever]
        public uint Id { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        /// <example>Иван</example>
        [Column("first_name")]
        [Required]
        [MaxLength(32)]
        [BindRequired]
        [BindProperty]
        public string FirstName { get; set; }

        /// <summary>
        /// Отчество пользователя
        /// </summary>
        /// <example>Иванович</example>
        [Column("middle_name")]
        [MaxLength(32)]
        [BindProperty]
        public string MiddleName { get; set; }

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        /// <example>Иванов</example>
        [Column("last_name")]
        [MaxLength(32)]
        [BindProperty]
        [BindRequired]
        public string LastName { get; set; }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        /// <example>Ivanov</example>
        [Column("username")]
        [MaxLength(32)]
        [BindProperty]
        [BindRequired]
        public string Username { get; set; }
        
        [BindNever]
        public UserStatus Status { get; set; }

        /// <summary>
        /// Установка пароля пользователя
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        [BindNever]
        public string Password { 
            set
            {
                _password = BCrypt.Net.BCrypt.HashPassword(value); 
            } 
        }

        /// <summary>
        /// Признак блокировки пользователя
        /// </summary>
        /// <example>false</example>
        [Column("blocked")]
        [BindNever]
        public bool Blocked { get; set; }

        /// <summary>
        /// Список ролей пользователя
        /// </summary>
        [JsonPropertyName("roles")]
        public List<UserRole> Roles { get; set; } = new List<UserRole>();

        /// <summary>
        /// Список привязанных кабинетов к пользователю
        /// </summary>
        [JsonPropertyName("rooms")]
        [BindNever]
        public List<Room> Rooms { get; set; } = new List<Room>();

        /// <summary>
        /// Список должностей сотрудника
        /// </summary>
        [BindNever]
        public List<Position> Positions { get; set; } = new List<Position>();

        [JsonIgnore]
        [BindNever]
        public List<ClientStageAssignement> Assignements { get; set; } = new List<ClientStageAssignement>();

        [JsonIgnore]
        [BindNever]
        public List<UserJournal> Journal { get; set; } = new List<UserJournal>();

        public User()
        {
            Blocked = false;
        }

        public User(UserAddDto addUser)
        {
            Username = addUser.Username;
            Password = addUser.Password;
            FirstName = addUser.FirstName;
            MiddleName = addUser.MiddleName;
            LastName = addUser.LastName;
            Blocked = false;
        }

        public void AssignData(UserChangeDto data)
        {
        //    Username = data.Username;
            if (!string.IsNullOrWhiteSpace(data.Password))
                Password = data.Password;
            FirstName = data.FirstName;
            MiddleName = data.MiddleName;
            LastName = data.LastName;
            Blocked = data.Blocked;
        }

        /// <summary>
        /// Получить полное имя пользователя без сокращений
        /// </summary>
        /// <returns></returns>
        public string FullName()
        {
            return FirstName + " " + MiddleName ?? "" + " " + LastName;
        }

        /// <summary>
        /// Получить краткое имя пользователя
        /// </summary>
        /// <returns></returns>
        public string ShortName()
        {
            return FirstName[0] + ". " + MiddleName != null ? MiddleName[0] + ". " : "" + LastName;
        }

        /// <summary>
        /// Получить информацию для инициализации данных в БД
        /// </summary>
        /// <returns></returns>
        public static User[] Seed()
        {     
            User[] users = new User[] {
                new User { Id = 1, FirstName = "Администратор", LastName = "Администратор", Username = "admin", Password = "admin", EntityGuid = Guid.NewGuid() }
            };            

            return users;
        }

        public static void AfterSeed(QueueDBContext context)
        {
            User user = context.Users.FirstOrDefault(u => u.Id == 1);

            UserRole adminRole = context.UserRoles.FirstOrDefault(ur => ur.Id == 1);

            user.Roles.Add(adminRole);
            user.Status = new UserStatus { StateId = 1, UserId = user.Id, LastUpdateTime = DateTime.Now };

            context.SaveChanges();
        }

        /// <summary>
        /// Проверка пароля
        /// </summary>
        /// <param name="password">Пароль для проверки</param>
        /// <returns></returns>
        public bool CheckPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, _password);
        }

        public static List<User> GetUsers(QueueDBContext context)
        {
            return context.Users
                        .Include(u => u.Roles)
                            .ThenInclude(ur => ur.UserRights)
                        .Include(u => u.Rooms)
                        .Include(u => u.Positions)
                        .Include(u => u.Status)
                        .ToList();
        }

        public static User GetUser(int id, QueueDBContext context)
        {
            return context.Users
                    .Include(u => u.Roles)
                        .ThenInclude(ur => ur.UserRights)
                    .Include(u => u.Rooms)
                    .Include(u => u.Positions)
                    .Include(u => u.Status)
                    .FirstOrDefault(u => u.Id == id);
        }
    }
}
