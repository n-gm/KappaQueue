using KappaQueueCommon.Models.Positions;
using KappaQueueCommon.Models.Queues;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KappaQueueCommon.Models.Clients
{
    [Table("client_stages")]
    public class ClientStage
    {
        [Column("client_id")]
        [Required]
        public uint ClientId { get; set; }
        [Column("position_id")]
        [Required]
        public uint PositionId { get; set; }
        [Column("priority")]
        [Required]
        public byte Priority { get; set; } = 0;

        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        [Column("start_time")]
        public DateTime? StartTime { get; set; } = null;
        
        [Column("end_time")]
        public DateTime? EndTime { get; set; } = null;
        
        [ForeignKey("PositionId")]
        public Position Position { get; set; }

        public List<ClientStageAssignement> Assignements { get; set; }
    }
}
