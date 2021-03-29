using KappaQueueCommon.Common.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KappaQueueCommon.Models.Queues
{
    [Table("queue_groups")]
    [Index("Name", IsUnique = true)]
    [Index("Prefix", IsUnique = true)]
    public class QueueGroup : QueueEntity
    {
        public QueueGroup()
        {

        }

        public QueueGroup(QueueGroupAddDto data)
        {
            AssignData(data);
        }

        public void AssignData(QueueGroupAddDto data)
        {
            Name = data.Name;
            Prefix = data.Prefix;
            Description = data.Description;
        }

        [Key]
        [Required]
        [Column("group_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("name")]
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }

        [Column("prefix")]
        [Required]
        [MaxLength(3)]
        public string Prefix { get; set; }

        [Column("description")]
        [MaxLength(256)]
        public string Description { get; set; }

        public List<Queue> Queues { get; set; }

        public static QueueGroup[] Seed()
        {
            QueueGroup[] groups = new QueueGroup[] {
                new QueueGroup{ Id = 1, Name = "Общее", Prefix = "О", Description = "Общая группа очередей" }
            };

            return groups;
        }
    }
}
