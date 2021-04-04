using KappaQueueCommon.Common.Classes;
using KappaQueueCommon.Common.DTO;
using KappaQueueCommon.Models.Queues;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KappaQueueCommon.Models.Clients
{
    [Table("clients")]
    [Index("CreateTime", "StateId", IsUnique = false)]
    public class Client : QueueEntity
    {

        public Client()
        {

        }

        public Client(ClientAddDto client)
        {
            QueueId = client.QueueId;
            OutOfOrder = client.OutOfOrder;         
        }

        public void AssignStages(QueueStageAssignDto stage)
        {
            ClientStage clientStage = new ClientStage()
            {
                ClientId = Id,
                PositionId = stage.PositionId,
                Priority = stage.Priority,
                OutOfOrder = stage.OutOfOrder,
                FreeAfterStage = stage.FreeAfterStage
            };

            ClientStages.Add(clientStage);
        }

        public void AssignQueueStage(QueueStage stage)
        {
            ClientStage clientStage = new ClientStage()
            {
                ClientId = Id,
                PositionId = stage.PositionId,
                Priority = stage.Priority,
                OutOfOrder = stage.OutOfOrder,
                FreeAfterStage = stage.FreeAfterStage
            };
            
            ClientStages.Add(clientStage);
        }

        [Column("id")]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint Id { get; set; }

        [Column("queue_id")]
        [Required]
        public uint QueueId { get; set; }

        [Column("prefix")]
        [Required]
        [MaxLength(3)]
        public string Prefix { get; set; }

        [Column("number")]
        [Required]
        public int Number { get; set; }

        [Column("state_id")]
        [Required]
        public byte StateId { get; set; } = (byte)ClientStateEnum.NOT_ACTIVATED;

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


        public List<ClientStage> ClientStages { get; set; } = new List<ClientStage>();
    }
}
