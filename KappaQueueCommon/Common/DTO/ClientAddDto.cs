using System.Collections.Generic;

namespace KappaQueueCommon.Common.DTO
{
    /// <summary>
    /// Класс для создание пользователя
    /// </summary>
    public class ClientAddDto
    {
        /// <summary>
        /// Идентификатор очереди
        /// </summary>
        public uint QueueId { get; set; }
        /// <summary>
        /// Клиент вне очереди
        /// </summary>
        public bool OutOfOrder { get; set; } = false;
        /// <summary>
        /// Список привязок должностей к очереди в случае кастомного набора
        /// </summary>
        public List<QueueStageAssignDto> Stages { get; set; } = new List<QueueStageAssignDto>();
    }
}
