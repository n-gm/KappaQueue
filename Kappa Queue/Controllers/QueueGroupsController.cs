using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KappaQueueCommon.Common.DTO;
using KappaQueueCommon.Models.Context;
using KappaQueueCommon.Models.Queues;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KappaQueue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueueGroupsController : ControllerBase
    {
        private readonly QueueDBContext _db;

        /// <summary>
        /// Конструктор контроллера
        /// </summary>
        /// <param name="context">Контекст БД</param>
        public QueueGroupsController(QueueDBContext context)
        {
            _db = context;
        }

        /// <summary>
        /// Получение всех групп очередей
        /// </summary>
        /// <response code="200">В теле возвращен список очередей</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя нет прав на просмотр групп очередей</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<QueueGroup>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Produces("application/json")]
        [Authorize(Roles = "manager,admin,ticketer")]
        public ActionResult<List<QueueGroup>> GetQueues()
        {
            return Ok(_db.QueueGroups.Include(qg => qg.Queues).ThenInclude(q => q.QueueNodes).ThenInclude(qn => qn.Position).ToList());
        }

        /// <summary>
        /// Получение группы очередей с идентификатором id
        /// </summary>
        /// <response code="200">Возвращена информация по очереди</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя нет прав на просмотр очереди</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(QueueGroup), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Produces("application/json")]
        [Authorize(Roles = "manager,admin")]
        public ActionResult<QueueGroup> GetQueue(int id)
        {
            return Ok(_db.QueueGroups.Include(qg => qg.Queues).ThenInclude(q => q.QueueNodes).ThenInclude(qn => qn.Position).FirstOrDefault(q => q.Id == id));
        }

        /// <summary>
        /// Создание группы очередей
        /// </summary>
        /// <response code="200">Возвращена информация по созданной группе очередей</response>
        /// <response code="400">Неверное наименование или префикс группы очередей</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя нет прав на создание групп очередей</response>
        [HttpPost]
        [ProducesResponseType(typeof(QueueGroup), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize(Roles = "manager,admin")]
        public ActionResult<QueueGroup> AddQueue([FromBody] QueueGroupAddDto addQueueGroup)
        {
            if (string.IsNullOrEmpty(addQueueGroup.Name) || string.IsNullOrEmpty(addQueueGroup.Prefix))
            {
                return BadRequest("Нельзя создавать очередь с пустым именем или префиксом");
            }

            QueueGroup group = new QueueGroup(addQueueGroup);

            if (_db.Queues.FirstOrDefault(q => q.Prefix == group.Prefix) != null
                || _db.QueueGroups.FirstOrDefault(qg => qg.Prefix.Equals(group.Prefix) || qg.Name.Equals(group.Name)) != null)
            {
                return BadRequest("Данное имя группы или префикс уже используется");
            }

            _db.QueueGroups.Add(group);
            _db.SaveChanges();
            return Ok(group);
        }

        /// <summary>
        /// Изменение группы очередй
        /// </summary>
        /// <response code="200">Возвращена информация по созданной группе очередей</response>
        /// <response code="400">Неверное наименование или префикс группы очередей, неверный id группы очередей</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя нет прав на изменение группы очередей</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(QueueGroup), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize(Roles = "manager,admin")]
        public ActionResult<QueueGroup> ChangeQueueGroup(int id, [FromBody] QueueGroupAddDto changeQueueGroup)
        {
            if (string.IsNullOrEmpty(changeQueueGroup.Name) || string.IsNullOrEmpty(changeQueueGroup.Prefix))
            {
                return BadRequest("Нельзя создавать очередь с пустым именем или префиксом");
            }

            if (_db.Queues.FirstOrDefault(q => q.Prefix == changeQueueGroup.Prefix) != null
                || _db.QueueGroups.FirstOrDefault(qg => qg.Prefix.Equals(changeQueueGroup.Prefix) || qg.Name.Equals(changeQueueGroup.Name)) != null)
            {
                return BadRequest("Данное имя очереди или префикс уже используется");
            }

            QueueGroup group = _db.QueueGroups.Include(qg => qg.Queues).ThenInclude(q => q.QueueNodes).ThenInclude(qn => qn.Position).FirstOrDefault(qg => qg.Id == id);

            if (group == null)
            {
                return BadRequest("Отсутствует группа очередей с идентификатором " + id.ToString());
            }

            group.AssignData(changeQueueGroup);
            _db.SaveChanges();
            return Ok(group);
        }

        /// <summary>
        /// Удаление группы очередей
        /// </summary>
        /// <response code="200">Группа очередей удалена, возвращена информация по всем группам очередей</response>
        /// <response code="400">Невозможно удалить группу очередей с идентификатором 1</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя нет прав на удаление групп очередей</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(List<QueueGroup>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Produces("application/json")]
        [Authorize(Roles = "manager,admin")]
        public ActionResult<List<QueueGroup>> DeletePosition(int id)
        {
            if (id == 1)
            {
                return BadRequest("Невозможно удалить системную группу очередей");
            }

            QueueGroup group = _db.QueueGroups.FirstOrDefault(qg => qg.Id == id);

            if (group != null)
            {
                string query = "UPDATE queues SET queue_group_id = 1 WHERE queue_group_id = {0}";
                _db.Database.ExecuteSqlRaw(query, id);

                _db.QueueGroups.Remove(group);
                _db.SaveChanges();
            }
            return Ok(_db.QueueGroups.Include(qg => qg.Queues).ThenInclude(q => q.QueueNodes).ThenInclude(qn => qn.Position).ToList());
        }
    }
}