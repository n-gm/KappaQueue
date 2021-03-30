using KappaQueueCommon.Common.DTO;
using KappaQueueCommon.Models.Clients;
using KappaQueueCommon.Models.Queues;
using KappaQueueCommon.Models.Users;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security;
using System.Text.Json.Serialization;

namespace KappaQueueCommon.Models.Positions
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
        public List<QueueStage> QueueNodes { get; set; } = new List<QueueStage>();
        
        [JsonIgnore]
        public List<User> Users { get; set; } = new List<User>();

        [JsonIgnore]
        public List<ClientStage> ClientStages { get; set; } = new List<ClientStage>();
    }
}
