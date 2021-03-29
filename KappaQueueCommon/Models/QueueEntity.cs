using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace KappaQueueCommon.Models
{
    [Index("EntityGuid", IsUnique = true)]
    public abstract class QueueEntity
    {
        [Column("guid")]
        [JsonPropertyName("guid")]
        public Guid EntityGuid { get; set; } = Guid.NewGuid();
    }
}
