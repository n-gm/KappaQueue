using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace KappaQueueCommon.Models.Users
{
    [Table("roles")]
    [Index("Code", IsUnique = true, Name = "IX_Roles_Code")]
    public class UserRole
    {
        [Column("role_id")]
        [Required]
        [Key]
        public byte Id {get;set;}

        [Column("name")]
        [MaxLength(32)]
        [Required]
        public string Name { get; set; }

        [Column("code")]
        [MaxLength(32)]
        [Required]
        public string Code { get; set; }

        [Column("description")]
        [MaxLength(128)]
        public string Description { get; set; }

        [JsonIgnore]
        public List<User> Users { get; set; } = new List<User>();

        public static UserRole[] Seed()
        {
            UserRole[] roles = new UserRole[]
            {
                new UserRole { Id = 1, Name = "Администратор", Description = "Администратор", Code = "admin"},
                new UserRole { Id = 2, Name = "Управляющий", Description = "Управляющий", Code = "manager"},
                new UserRole { Id = 3, Name = "Выдача талона", Description = "Выдача талона", Code = "ticketer"},
                new UserRole { Id = 4, Name = "Исполнитель", Description = "Исполнитель", Code = "performer"},
                new UserRole { Id = 5, Name = "Терминал", Description = "Терминал", Code = "terminal"}
            };

            return roles;
        }
    }
}
