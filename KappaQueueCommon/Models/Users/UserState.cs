using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KappaQueueCommon.Models.Users
{
    [Table("user_states")]
    public class UserState : QueueEntity
    {
        [Column("state_id")]
        [Required]
        [Key]
        public byte StateId { get; set; }

        [Column("name")]
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

        [Column("description")]
        [MaxLength(256)]
        public string Description { get; set; }

        [JsonIgnore]
        public List<UserJournal> Users { get; set; } = new List<UserJournal>();

        public static UserState[] Seed()
        {
            return new UserState[]
            {
                new UserState { StateId = 1, Name = "Свободен", Description = "Пользователь свободен" },
                new UserState { StateId = 2, Name = "Ожидание", Description = "Ожидание приглашенного клиента" },
                new UserState { StateId = 3, Name = "Прием", Description = "Пользователь ведет прием клиента" },
                new UserState { StateId = 4, Name = "Приостановлен", Description = "Прием приостановлен" }
            };
        }
    }
}
