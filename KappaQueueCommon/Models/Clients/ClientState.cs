using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KappaQueueCommon.Models.Clients
{
    [Table("client_states")]
    public class ClientState
    {
        [Column("id")]
        [Required]
        public byte Id { get; set; }

        [Column("name")]
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }

        [Column("description")]
        [MaxLength(128)]
        public string Description { get; set; }

        public List<Client> Clients { get; set; }
        public static ClientState[] Seed()
        {
            return new ClientState[]
            {
                new ClientState { Id = 1, Name = "Не активирован", Description = "Не активирован"},
                new ClientState { Id = 2, Name = "Ожидание", Description = "Ожидание"},
                new ClientState { Id = 3, Name = "Свободен", Description = "Свободен"},
                new ClientState { Id = 4, Name = "Приглашен", Description = "Приглашен к специалисту" },
                new ClientState { Id = 5, Name = "Занят", Description = "Занят"},
                new ClientState { Id = 6, Name = "Завершено", Description = "Все этапы пройдены"},
                new ClientState { Id = 255, Name = "Приостановлено", Description = "Приостановлено"}
            };
        }
    }
}
