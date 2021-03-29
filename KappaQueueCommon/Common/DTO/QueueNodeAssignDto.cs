using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KappaQueueCommon.Common.DTO
{
    public class QueueNodeAssignDto
    {
        /// <summary>
        /// Идентификатор должности
        /// </summary>
        public int PositionId { get; set; }
        /// <summary>
        /// Приоритет этапа
        /// </summary>
        public byte Priority { get; set; }
        /// <summary>
        /// Этап вне очереди
        /// </summary>
        public bool OutOfOrder { get; set; }
    }
}
