namespace KappaQueueCommon.Common.Classes
{
    public enum ClientStateEnum : byte
    {
        /// <summary>
        /// Создан, но не активирован
        /// </summary>
        NOT_ACTIVATED = 1,
        /// <summary>
        /// Техническое ожидаение после выдачи талона или завершения обследования
        /// </summary>
        WAITING = 2,
        /// <summary>
        /// Свободен
        /// </summary>
        FREE = 3,
        /// <summary>
        /// Вызван
        /// </summary>
        CALLED = 4,
        /// <summary>
        /// Обслуживается
        /// </summary>
        SERVICING = 5,
        /// <summary>
        /// Прием полностью закончен
        /// </summary>
        SERVICED = 6,
        /// <summary>
        /// Прохождение очереди приостановлено
        /// </summary>
        SUSPENDED = 255
    }
}
