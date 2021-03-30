using System.ComponentModel.DataAnnotations;

namespace KappaQueueCommon.Common.DTO
{
    public class QueueAddDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        [MaxLength(3)]
        public string Prefix { get; set; }
        public bool OutOfOrder { get; set; }
    }
}
