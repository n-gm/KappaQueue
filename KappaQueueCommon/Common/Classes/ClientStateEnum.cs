using System;
using System.Collections.Generic;
using System.Text;

namespace KappaQueueCommon.Common.Classes
{
    public enum ClientStateEnum : byte
    {
        /// <summary>
        /// Техническое ожидаение после выдачи талона или завершения обследования
        /// </summary>
        WAITING = 1,
        /// <summary>
        /// Свободен
        /// </summary>
        FREE = 2,
        /// <summary>
        /// Вызван
        /// </summary>
        CALLED = 3,
        /// <summary>
        /// Обслуживается
        /// </summary>
        SERVICING = 4,
        /// <summary>
        /// Прием полностью закончен
        /// </summary>
        SERVICED = 5,
        /// <summary>
        /// Прохождение очереди приостановлено
        /// </summary>
        SUSPENDED = 255
    }
}
