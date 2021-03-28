using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KappaQueue.Common.DTO
{
    /// <summary>
    /// Класс для создания должности
    /// </summary>
    public class PositionAddDto
    {
        /// <summary>
        /// Наименование должности
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Описание должности
        /// </summary>
        public string Description { get; set; }
    }
}
