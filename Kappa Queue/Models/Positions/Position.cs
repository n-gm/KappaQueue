using KappaQueue.Common.DTO;
using KappaQueue.Models.Queues;
using KappaQueue.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace KappaQueue.Models.Positions
{
    [Table("positions")]
    public class Position : QueueEntity
    {
        public Position()
        {

        }

        public Position(PositionAddDto addPosition)
        {
            AssignData(addPosition);
        }

        public void AssignData(PositionAddDto data)
        {
            Name = data.Name;
            Description = data.Description;
        }

        [Column("id")]
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("name")]
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

        [Column("description")]
        [MaxLength(256)]
        public string Description { get; set; }

        [JsonIgnore]
        public List<QueueNode> QueueNodes { get; set; } = new List<QueueNode>();
        
        [JsonIgnore]
        public List<User> Users { get; set; } = new List<User>();
    }
}
