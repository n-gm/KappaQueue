using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace KappaQueue.Models
{
    [Index("EntityGuid", IsUnique = true)]
    public abstract class QueueEntity
    {
        [Column("guid")]
        [JsonPropertyName("guid")]
        public Guid EntityGuid { get; set; } = Guid.NewGuid();
    }
}
