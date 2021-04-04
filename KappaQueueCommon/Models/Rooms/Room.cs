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
                
        [ForeignKey("RoomTypeId")]        
        public RoomType RoomType { get; set; }

        [Column("room_type_id")]
        [Required]
        public byte RoomTypeId { get; set; }

        [Column("number")]
        [Required]
        public int Number { get; set; }

        [Column("blocked")]
        [Required]
        public bool Blocked { get; set; } = false;

        [JsonIgnore]
        public List<User> Users { get; set; } = new List<User>();
    }
}
