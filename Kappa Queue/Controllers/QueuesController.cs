using System.Collections.Generic;
using System.Linq;
using KappaQueueCommon.Common.DTO;
using KappaQueueCommon.Common.References;
using KappaQueueCommon.Models.Context;
using KappaQueueCommon.Models.Queues;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KappaQueue.Controllers
{
    /// <summary>
    /// Очереди
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public partial class QueuesController : ControllerBase
    {
        private readonly QueueDBContext _db;

        /// <summary>
        /// Конструктор контроллера
        /// </summary>
        /// <param name="context">Контекст БД</param>
        public QueuesController(QueueDBContext context)
        {
            _db = context;
        }

        /// <summary>
        /// Получение всех очередей
        /// </summary>
        /// <response code="200">В теле возвращен список очередей</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя нет прав на просмотр очередей</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<Queue>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Produces("application/json")]
        [Authorize(Roles = RightsRef.ALL_QUEUES + "," + RightsRef.GET_QUEUES)]
        public ActionResult<List<Queue>> GetQueues()
        {
            return Ok(_db.Queues.Include(q => q.QueueNodes).ThenInclude(qn => qn.Position).ToList());
        }

        /// <summary>
        /// Получение очереди с идентификатором id
        /// </summary>
        /// <response code="200">Возвращена информация по очереди</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя нет прав на просмотр очереди</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(List<Queue>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Produces("application/json")]
        [Authorize(Roles = RightsRef.ALL_QUEUES + "," + RightsRef.GET_QUEUE)]
        public ActionResult<List<Queue>> GetQueue(int id)
        {
            return Ok(_db.Queues.Include(q => q.QueueNodes).ThenInclude(qn => qn.Position).FirstOrDefault(q => q.Id == id));
        }

        /// <summary>
        /// Создание очереди
        /// </summary>
        /// <response code="200">Возвращена информация по созданной очереди</response>
        /// <response code="400">Неверное наименование или префикс очереди</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя нет прав на создание очереди</response>
        [HttpPost]
        [ProducesResponseType(typeof(Queue), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize(Roles = RightsRef.ALL_QUEUES + "," + RightsRef.CREATE_QUEUE)]
        public ActionResult<Queue> AddQueue([FromBody] QueueAddDto addQueue)
        {
            if (string.IsNullOrEmpty(addQueue.Name) || string.IsNullOrEmpty(addQueue.Prefix))
            {
                return BadRequest("Нельзя создавать очередь с пустым именем или префиксом");
            }

            Queue queue = new Queue(addQueue);            

            if (_db.Queues.FirstOrDefault(q => q.Name == queue.Name || q.Prefix == queue.Prefix) != null
                || _db.QueueGroups.FirstOrDefault(qg => qg.Prefix.Equals(queue.Prefix)) != null)
            {
                return BadRequest("Данное имя очереди или префикс уже используется");
            }

            _db.Queues.Add(queue);
            _db.SaveChanges();
            return Ok(queue);
        }

        /// <summary>
        /// Изменение очереди
        /// </summary>
        /// <response code="200">Возвращена информация по измененной очереди</response>
        /// <response code="400">Неверное наименование или префикс очереди, неверный id очереди</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя нет прав на изменение очереди</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(Queue), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize(Roles = RightsRef.ALL_QUEUES + "," + RightsRef.CHANGE_QUEUE)]
        public ActionResult<Queue> ChangePosition(int id, [FromBody] QueueAddDto changeQueue)
        {
            if (string.IsNullOrEmpty(changeQueue.Name) || string.IsNullOrEmpty(changeQueue.Prefix))
            {
                return BadRequest("Нельзя создавать очередь с пустым именем или префиксом");
            }

            if (_db.Queues.FirstOrDefault(q => q.Name == changeQueue.Name || q.Prefix == changeQueue.Prefix) != null)
            {
                return BadRequest("Данное имя очереди или префикс уже используется");
            }

            Queue queue = _db.Queues.Include(q => q.QueueNodes).ThenInclude(qn => qn.Position).FirstOrDefault(p => p.Id == id);
            
            if (queue == null)
            {
                return BadRequest("Отсутствует очередь с идентификатором " + id.ToString());
            }
            
            queue.AssignData(changeQueue);
            _db.SaveChanges();
            return Ok(queue);
        }

        /// <summary>
        /// Удаление очереди
        /// </summary>
        /// <response code="200">Очередь удалена, возвращена информация по всем очередям</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя нет прав на удаление очереди</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(List<Queue>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Produces("application/json")]
        [Authorize(Roles = RightsRef.ALL_QUEUES + "," + RightsRef.DELETE_QUEUE)]
        public ActionResult<List<Queue>> DeletePosition(int id)
        {
            Queue queue = _db.Queues.FirstOrDefault(p => p.Id == id);
            _db.Queues.Remove(queue);
            _db.SaveChanges();
            return Ok(_db.Queues.Include(q => q.QueueNodes).ThenInclude(qn => qn.Position).ToList());
        }
    }
}