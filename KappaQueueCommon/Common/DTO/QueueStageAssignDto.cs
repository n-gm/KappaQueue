namespace KappaQueueCommon.Common.DTO
{
    public class QueueStageAssignDto
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
        /// <summary>
        /// Освобождать после обследования
        /// </summary>
        public bool FreeAfterStage { get; set; }
    }
}
