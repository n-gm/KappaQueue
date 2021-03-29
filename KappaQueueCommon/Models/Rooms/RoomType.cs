using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace KappaQueueCommon.Models.Rooms
{
    [Table("room_types")]
    public class RoomType
    {
        [Column("room_type_id")]
        [Required]
        [Key]
        public byte Id { get; set; }
        
        [Column("name")]
        [MaxLength(32)]
        [Required]
        public string Name { get; set; }
        
        [Column("prefix")]
        [MaxLength(3)]
        public string Prefix { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [JsonIgnore]
        public List<Room> Rooms { get; set; } = new List<Room>();
    }
}
