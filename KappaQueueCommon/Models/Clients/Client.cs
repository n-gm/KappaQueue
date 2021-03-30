using KappaQueueCommon.Common.Classes;
using KappaQueueCommon.Models.Queues;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KappaQueueCommon.Models.Clients
{
    [Table("clients")]
    [Index("CreateTime", "StateId", IsUnique = false)]
    public class Client : QueueEntity
    {
        [Column("id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("queue_id")]
        [Required]
        public int QueueId { get; set; }

        [Column("number")]
        [Required]
        public int Number { get; set; }

        [Column("state_id")]
        [Required]
        public byte StateId { get; set; } = (byte)ClientStateEnum.WAITING;

        [ForeignKey("StateId")]
        public ClientState State { get; set; }

        [Column("out_of_order")]
        [Required]
        public bool OutOfOrder { get; set; } = false;

        [Column("create_time")]
        [Required]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        [Column("end_time")]
        public DateTime? EndTime { get; set; } = null;

        [ForeignKey("QueueId")]
        public Queue Queue { get; set; }


        public List<ClientStage> ClientStages { get; set; }
    }
}
