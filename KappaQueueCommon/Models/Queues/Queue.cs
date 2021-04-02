using KappaQueueCommon.Common.DTO;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace KappaQueueCommon.Models.Queues
{
    /// <summary>
    /// Класс очереди
    /// </summary>
    [Table("queues")]
    [Index("Name", IsUnique = true)]
    [Index("Prefix", IsUnique = true)]
    public class Queue : QueueEntity
    {
        public Queue()
        {

        }

        public Queue(QueueAddDto addQueue)
        {
            AssignData(addQueue);
        }

        public void AssignData(QueueAddDto data)
        {
            Name = data.Name;
            Prefix = data.Prefix.ToUpper();
            OutOfOrder = data.OutOfOrder;
            Description = data.Description;
        }
        /// <summary>
        /// Идентификатор очереди
        /// </summary>
        [Column("id")]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public uint Id { get; set; }

        /// <summary>
        /// Наименование очереди
        /// </summary>
        [Column("name")]
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

        /// <summary>
        /// Префикс очереди (то, что печатается в талоне)
        /// </summary>
        [Column("prefix")]
        [MaxLength(3)]
        public string Prefix { get; set; }

        /// <summary>
        /// Признак вызова клиента без очереди
        /// </summary>
        [Column("out_of_order")]
        [Required]
        public bool OutOfOrder { get; set; } = false;

        [Column("queue_group_id")]
        [Required]
        [JsonIgnore]
        public uint QueueGroupId { get; set; }

        [ForeignKey("QueueGroupId")]
        [JsonIgnore]
        public QueueGroup QueuesGroup { get; set; } 
        /// <summary>
        /// Описание очереди
        /// </summary>
        [Column("description")]
        [MaxLength(128)]
        public string Description { get; set; }

        public List<QueueStage> QueueNodes { get; set; } = new List<QueueStage>();
    }
}
