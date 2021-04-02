using KappaQueueCommon.Common.DTO;
using KappaQueueCommon.Models.Positions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KappaQueueCommon.Models.Queues
{
    [Table("queue_stages")]
    public class QueueStage
    {
        public QueueStage()
        {

        }

        public QueueStage(QueueNodeAssignDto data)
        {
            AssignData(data);
        }

        public void AssignData(QueueNodeAssignDto data)
        {
            OutOfOrder = data.OutOfOrder;
            PositionId = data.PositionId;
            Priority = data.Priority;
        }

        /// <summary>
        /// Идентификатор элемента очереди
        /// </summary>
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// Приоритет при вызове к специалисту. 0 - самый высокий, 255 - самый низкий
        /// </summary>
        [Column("priority")]
        [Required]
        public byte Priority { get; set; } = 0;

        /// <summary>
        /// Признак вызова к специалисту без очереди
        /// </summary>
        [Column("out_of_order")]
        [Required]
        public bool OutOfOrder { get; set; } = false;

        /// <summary>
        /// Признак того, что клиент освобождается при завершении этапа
        /// </summary>
        [Column("free_after_stage")]
        [Required]
        public bool FreeAfterStage { get; set; } = true;
        
        /// <summary>
        /// Идентификатор очереди
        /// </summary>
        [Column("queue_id")]
        [Required]
        public uint QueueId { get; set; }

        /// <summary>
        /// Идентификатор должности
        /// </summary>
        [Column("position_id")]
        [Required]
        public uint PositionId { get; set; }
        /// <summary>
        /// Очередь, к которой привязан этап
        /// </summary>
        [ForeignKey("QueueId")]
        public Queue Queue { get; set; }

        /// <summary>
        /// Должность, привязанная к очереди
        /// </summary>
        [ForeignKey("PositionId")]
        public Position Position { get; set; }
    }
}
