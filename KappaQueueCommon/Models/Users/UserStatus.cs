using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace KappaQueueCommon.Models.Users
{
    [Table("user_status")]
    public class UserStatus
    {
        public UserStatus()
        {

        }

        public UserStatus(User user)
        {
            UserId = user.Id;
            StateId = 1;
        }

        [Column("user_id")]
        [Required]
        [Key]
        public uint UserId { get; set; }
        
        [JsonIgnore]
        [ForeignKey("UserId")]
        public User User { get; set; }

        [Column("state_id")]
        [Required]
        public byte StateId { get; set; }

        [JsonIgnore]
        [ForeignKey("StateId")]
        public UserState UserState { get; set; }

        [Column("list_update_time")]
        [Required]
        public DateTime LastUpdateTime { get; set; } = DateTime.Now;
    }
}
