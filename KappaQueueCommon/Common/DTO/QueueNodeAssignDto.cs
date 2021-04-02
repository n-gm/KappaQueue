namespace KappaQueueCommon.Common.DTO
{
    public class QueueNodeAssignDto
    {
        /// <summary>
        /// Идентификатор должности
        /// </summary>
        public uint PositionId { get; set; }
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
