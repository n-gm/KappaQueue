using KappaQueueCommon.Common.DTO;
using KappaQueueCommon.Models.Positions;
using KappaQueueCommon.Models.Queues;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KappaQueue.Controllers
{
    public partial class QueuesController : ControllerBase
    {
        /// <summary>
        /// Получение списка этапов очереди
        /// </summary>
        /// <response code="200">В теле возвращен список очередей</response>
        /// <response code="400">Передан неверный идентификатор очереди</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя нет прав на просмотр должностей очереди</response>
        [HttpGet("{id:int}/nodes")]
        [ProducesResponseType(typeof(List<QueueNode>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Produces("application/json")]
        [Authorize(Roles = "manager,admin,ticketer")]
        public ActionResult<List<QueueNode>> GetQueueNodes(int id)
        {
            try
            {
                return Ok(_db.Queues
                            .Include(q => q.QueueNodes)
                            .ThenInclude(qn => qn.Position)
                            .First(q => q.Id == id)
                            .QueueNodes.ToList());
            } catch (Exception e)
            {
                return BadRequest("Не найдена очередь с идентификатором " + id.ToString());
            }
        }

        /// <summary>
        /// Добавление должности к очереди
        /// </summary>
        /// <response code="200">В теле возвращен список должностей в очереди</response>
        /// <response code="400">Передан неверный идентификатор очереди или должности</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя нет прав на добавление должности в очередь</response>
        [HttpPost("{id:int}/nodes")]
        [ProducesResponseType(typeof(List<QueueNode>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize(Roles = "manager,admin")]
        public ActionResult<List<QueueNode>> AddQueueNode(int id, [FromBody] QueueNodeAssignDto addNode)
        {
            Queue queue = _db.Queues
                            .Include(q => q.QueueNodes)
                            .FirstOrDefault(q => q.Id == id);
            
            if (queue == null)
            {
                return BadRequest("Не найдена очередь с идентификатором " + id.ToString());
            }

            Position position = _db.Positions.FirstOrDefault(p => p.Id == addNode.PositionId);

            if (position == null)
            {
                return BadRequest("Отсутствует должность с идентификатором " + id.ToString());
            }

            QueueNode node = new QueueNode(addNode);

            queue.QueueNodes.Add(node);

            _db.SaveChanges();

            return Ok(queue.QueueNodes);
        }

        /// <summary>
        /// Изменение привязки должности в очереди
        /// </summary>
        /// <response code="200">Возвращена информация по должностям очереди</response>
        /// <response code="400">Неверное наименование или префикс очереди, неверный id очереди</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя нет прав на изменение очереди</response>
        [HttpPut("{id:int}/nodes/{positionId:int}")]
        [ProducesResponseType(typeof(QueueNode), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize(Roles = "manager,admin")]
        public ActionResult<QueueNode> ChangeQueueNode(int id, int positionId, [FromBody] QueueNodeAssignDto changeQueueNode)
        {

            QueueNode queueNode = _db.QueueNodes.Include(qn => qn.Position).FirstOrDefault(qn => qn.Id == id && qn.PositionId == positionId);

            if (queueNode == null)
            {
                return BadRequest("Отсутствует очередь с идентификатором " + id.ToString() + " с должностью с идентификатором " + positionId.ToString());
            }

            queueNode.AssignData(changeQueueNode);
            _db.SaveChanges();
            return Ok(queueNode);
        }

        /// <summary>
        /// Удаление должности из очереди
        /// </summary>
        /// <response code="200">Должность удалена, возвращена информация по всем должностям очереди</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя нет прав на удаление должности из очереди</response>
        [HttpDelete("{id:int}/nodes/{positionId:int}")]
        [ProducesResponseType(typeof(List<Queue>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Produces("application/json")]
        [Authorize(Roles = "manager,admin")]
        public ActionResult<List<QueueNode>> DeletePosition(int id, int positionId)
        {
            Queue queue = _db.Queues.Include(q => q.QueueNodes).FirstOrDefault(p => p.Id == id);
            queue.QueueNodes.Remove(queue.QueueNodes.FirstOrDefault(qn => qn.Id == positionId));
            _db.SaveChanges();
            return Ok(queue.QueueNodes);
        }
    }
}
