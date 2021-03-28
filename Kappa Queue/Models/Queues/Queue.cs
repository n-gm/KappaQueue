using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KappaQueue.Models.Queues
{
    /// <summary>
    /// Класс очереди
    /// </summary>
    [Table("queues")]
    public class Queue : QueueEntity
    {
        /// <summary>
        /// Идентификатор очереди
        /// </summary>
        [Column("id")]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

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
        [Required]
        [MaxLength(3)]
        public string Prefix { get; set; }

        /// <summary>
        /// Признак вызова клиента без очереди
        /// </summary>
        [Column("out_of_order")]
        [Required]
        public bool OutOfOrder { get; set; } = false;

        /// <summary>
        /// Описание очереди
        /// </summary>
        [Column("description")]
        [MaxLength(128)]
        public string Description { get; set; }

        public List<QueueNode> QueueNodes { get; set; } = new List<QueueNode>();
    }
}
