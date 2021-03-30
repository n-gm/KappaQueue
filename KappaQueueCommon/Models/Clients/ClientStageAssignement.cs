using KappaQueueCommon.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KappaQueueCommon.Models.Clients
{
    [Table("client_stages_assignement")]
    public class ClientStageAssignement
    {
        [Column("client_id")]
        [Required]
        public int ClientId { get; set; }
        [Column("position_id")]
        [Required]
        public int PositionId { get; set; }
        [Column("priority")]
        [Required]
        public byte Priority { get; set; }
        [Column("user_id")]
        [Required]
        public int UserId { get; set; }
        [Column("assign_time")]
        [Required]
        public DateTime AssignTime { get; set; } = DateTime.Now;
        public ClientStage ClientStage { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
