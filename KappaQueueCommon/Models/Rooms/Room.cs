using KappaQueueCommon.Models.Users;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace KappaQueueCommon.Models.Rooms
{
    [Table("rooms")]
    public class Room
    {
        [Column("room_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ushort Id { get; set; }

        [Column("room_type")]
        [ForeignKey("RoomTypeId")]
        [Required]
        public RoomType RoomType { get; set; }

        public byte RoomTypeId { get; set; }

        [Column("number")]
        [Required]
        public int Number { get; set; }

        [Column("blocked")]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public bool Blocked { get; set; }

        [JsonIgnore]
        public List<User> Users { get; set; } = new List<User>();
    }
}
