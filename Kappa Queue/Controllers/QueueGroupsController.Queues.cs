using KappaQueueCommon.Models.Queues;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace KappaQueue.Controllers
{
    public partial class QueueGroupsController : ControllerBase
    {
        /// <summary>
        /// Получение всех очередей в группе
        /// </summary>
        /// <response code="200">В теле возвращен список очередей группы</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя нет прав на просмотр очередей в группе</response>
        [HttpGet("{id:int}/queues")]
        [ProducesResponseType(typeof(List<Queue>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Produces("application/json")]
        [Authorize(Roles = "manager,admin,ticketer")]
        public ActionResult<List<Queue>> GetQueues(int id)
        {
            QueueGroup group = _db.QueueGroups.Include(qg => qg.Queues).ThenInclude(q => q.QueueNodes).ThenInclude(qn => qn.Position).FirstOrDefault(q => q.Id == id);
            return Ok(group?.Queues);
        }

        /// <summary>
        /// Получение группы очередей с идентификатором id
        /// </summary>
        /// <response code="200">Возвращена информация по очереди в группе</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя нет прав на просмотр очереди группы</response>
        [HttpGet("{id:int}/queues/{queueId:int}")]
        [ProducesResponseType(typeof(Queue), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Produces("application/json")]
        [Authorize(Roles = "manager,admin,ticketer")]
        public ActionResult<Queue> GetQueue(int id, int queueId)
        {
            QueueGroup group = _db.QueueGroups.Include(qg => qg.Queues).ThenInclude(q => q.QueueNodes).ThenInclude(qn => qn.Position).FirstOrDefault(q => q.Id == id);
            return Ok(group?.Queues.FirstOrDefault(q => q.Id == queueId));
        }

        /// <summary>
        /// Привязка очереди к группе очередей
        /// </summary>
        /// <response code="200">Возвращена информация по новому наполнению группы</response>
        /// <response code="400">Отсутствует группа или очереди с заданными идентификаторами</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя нет прав на привязку очереди к группе очередей</response>
        [HttpPost("{id:int}/queues/{queueId:int}")]
        [ProducesResponseType(typeof(List<Queue>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize(Roles = "manager,admin")]
        public ActionResult<List<Queue>> AddQueueToGroup(int id, int queueId)
        {
            QueueGroup group = _db.QueueGroups.Include(qg => qg.Queues).ThenInclude(q => q.QueueNodes).ThenInclude(qn => qn.Position).FirstOrDefault(q => q.Id == id);
            Queue queue = _db.Queues.Include(q => q.QueueNodes).ThenInclude(qn => qn.Position).FirstOrDefault(q => q.Id == queueId);

            if (group == null || queue == null)
            {
                return BadRequest("Отсутствует требуемая группа или очередь");
            }

            if (queue.QueueGroupId == id)
                return Ok(group.Queues);

            group.Queues.Add(queue);
            _db.SaveChanges();
            return Ok(group.Queues);
        }
        
        /// <summary>
        /// Удаление очереди из группы
        /// </summary>
        /// <response code="200">Из группы удалена очередь, возвращена информация по всем группам очередей</response>
        /// <response code="400">Очереди или группы с заданным идентификатором не существует, либо попытка удалить из базовой группы</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя нет прав на удаление очереди из групп очередей</response>
        [HttpDelete("{id:int}/queues/{queueId:int}")]
        [ProducesResponseType(typeof(List<QueueGroup>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Produces("application/json")]
        [Authorize(Roles = "manager,admin")]
        public ActionResult<List<QueueGroup>> DeleteQueueFromGroup(int id, int queueId)
        {           
            if (id == 1)
            {
                return BadRequest("Невозможно удалить очередь из базовой группы. Назначьте ей другую группу");
            }

            QueueGroup group = _db.QueueGroups.Include(qg => qg.Queues).ThenInclude(q => q.QueueNodes).ThenInclude(qn => qn.Position).FirstOrDefault(q => q.Id == id);
            QueueGroup mainGroup = _db.QueueGroups.FirstOrDefault(q => q.Id == 1);
            Queue queue = _db.Queues.Include(q => q.QueueNodes).ThenInclude(qn => qn.Position).FirstOrDefault(q => q.Id == queueId);

            if (queue == null || group == null)
            {
                return BadRequest("Отсутствует требуемая очередь или группа");
            }

            mainGroup.Queues.Add(queue);
            _db.SaveChanges();
            return Ok(group.Queues);
        }
    }
}
