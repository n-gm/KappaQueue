using KappaQueueCommon.Models.Positions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Column("out_of_order")]
        [Required]
        public bool OutOfOrder { get; set; } = false;

        [Column("free_after_stage")]
        [Required]
        public bool FreeAfterStage { get; set; } = true;

        [Column("start_time")]
        public DateTime? StartTime { get; set; } = null;
        
        [Column("end_time")]
        public DateTime? EndTime { get; set; } = null;
        
        [ForeignKey("PositionId")]
        public Position Position { get; set; }

        public List<ClientStageAssignement> Assignements { get; set; }
    }
}
