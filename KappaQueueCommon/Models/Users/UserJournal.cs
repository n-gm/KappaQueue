using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace KappaQueueCommon.Models.Users
{
    [Table("user_journal")]
    [Index("UserId", "OpTime", IsUnique = false )]
    public class UserJournal
    {
        [Column("user_id")]
        [Required]
        public uint UserId { get; set; }

        [JsonIgnore]
        [ForeignKey("UserId")]
        public User User { get; set; }

        [Column("state_id")]
        [Required]
        public byte StateId { get; set; }

        [ForeignKey("StateId")]
        public UserState UserState { get; set; }

        [Column("oper_time")]
        [Required]
        public DateTime OpTime { get; set; } = DateTime.Now;
    }
}
