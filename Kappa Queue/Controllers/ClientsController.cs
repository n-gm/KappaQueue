using System;
using System.Collections.Generic;
using System.Linq;
using KappaQueueCommon.Common.Classes;
using KappaQueueCommon.Common.DTO;
using KappaQueueCommon.Common.References;
using KappaQueueCommon.Models.Clients;
using KappaQueueCommon.Models.Context;
using KappaQueueCommon.Models.Queues;
using KappaQueueCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KappaQueue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly QueueDBContext _db;
        private readonly ITicketEnumerator _ticketEnumerator;

        public ClientsController(QueueDBContext context, ITicketEnumerator ticketEnumerator)
        {
            _db = context;
            _ticketEnumerator = ticketEnumerator;
        }

        /// <summary>
        /// Получить информацию по всем клиентам
        /// </summary>
        /// <returns>Список всех клиентов</returns>
        /// <param name="startTime">Дата, с которой возвращать выборку</param>
        /// <response code="200">Возвращен список всех клиентов в системе с учетом фильтрации</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя недостаточно прав для просмотра всех клиентов</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<Client>), 200)]
        [ProducesResponseType(403)]
        [Authorize(Roles = RightsRef.ALL_CLIENTS + "," + RightsRef.GET_CLIENTS)]
        public ActionResult<List<Client>> GetClients(DateTime? startTime)
        {
            return Ok(_db.Clients
                            .Include(c => c.ClientStages)
                                .ThenInclude(cs => cs.Position)
                            .Include(c => c.Queue)
                                .ThenInclude(q => q.QueuesGroup)
                            .Include(c => c.State)
                            .Where(c => c.CreateTime >= (startTime ?? DateTime.MinValue)));
        }

        /// <summary>
        /// Получить информацию по клиенту
        /// </summary>
        /// <returns>Список всех пользователей</returns>
        /// <param name="id">Идентификатор клиента</param>
        /// <response code="200">Возвращен список всех клиентов</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя недостаточно прав для просмотра клиента</response>
        [HttpGet("{id:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Client), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Authorize(Roles = RightsRef.ALL_CLIENTS + "," + RightsRef.GET_CLIENT)]
        public ActionResult<Client> GetClient(int id)
        {
            return Ok(_db.Clients
                            .Include(c => c.ClientStages)
                                .ThenInclude(cs => cs.Position)
                            .Include(c => c.Queue)
                                .ThenInclude(q => q.QueuesGroup)
                            .Include(c => c.State)
                            .FirstOrDefault(c => c.Id == id));
        }

        /// <summary>
        /// Создать клиента
        /// </summary>
        /// <returns>Список всех пользователей</returns>
        /// <param name="addClient">Структура для создания клиента</param>
        /// <response code="200">Возвращен созданные клиент</response>
        /// <response code="400">Отсутствует очередь с требуемым идентификатором</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя недостаточно прав для создание клиента</response>
        /// <response code="500">В процессе работы с БД возникла ошибка, клиент не создан</response>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(Client), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        [Authorize(Roles = RightsRef.ALL_CLIENTS + "," + RightsRef.CREATE_CLIENT)]
        public ActionResult<Client> CreateClient([FromBody] ClientAddDto addClient) 
        {
            Client client = new Client(addClient);

            Queue queue = _db.Queues.Include(q => q.QueuesGroup).FirstOrDefault(q => q.Id == client.QueueId);

            if (queue == null)
            {
                return BadRequest("Отсутствует очередь с идентификатором " + client.QueueId.ToString());
            }

            client.Prefix = queue.Prefix ?? queue.QueuesGroup.Prefix;
            client.Number = _ticketEnumerator.GetTicketNumber(client.Prefix);

            _db.Clients.Add(client);

            using (_db.Database.BeginTransaction())
            {
                try
                {
                    _db.SaveChanges();

                    if (addClient.Stages?.Count > 0)
                    {
                        foreach (QueueStageAssignDto stage in addClient.Stages)
                            client.AssignStages(stage);
                    }
                    else
                    {
                        foreach (QueueStage stage in _db.QueueStages.Where(qs => qs.QueueId == client.QueueId))
                            client.AssignQueueStage(stage);
                    }

                    _db.SaveChanges();
                    _db.Database.CommitTransaction();

                    return Ok(client);
                } catch (Exception e)
                {
                    _db.Database.RollbackTransaction();                    
                    return StatusCode(500, "В процессе выдачи талона возникла ошибка: {"+e.Message+"}, талон не выдан");
                }
            }
        }

        /// <summary>
        /// Изменить параметры клиента
        /// </summary>
        /// <returns>Список всех пользователей</returns>
        /// <param name="id">Идентификатор клиента</param>
        /// <param name="changeClient">JSON с новыми параметрами клиента</param>
        /// <param name="outOfOrder">Признак "Вне очереди" для клиента</param>
        /// <param name="stateId">Состояние клиента</param>
        /// <response code="200">Возвращен список всех клиентов</response>
        /// <response code="400">Пользователь с заданным идентификатором не существует</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя недостаточно прав для изменения клиента</response>
        [HttpPut("{id:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Client), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Authorize(Roles = RightsRef.ALL_CLIENTS + "," + RightsRef.CHANGE_CLIENT)]
        public ActionResult<Client> ChangeClient(int id,
                                              byte? stateId,
                                              bool? outOfOrder,
                                              [FromBody] ClientChangeDto changeClient)
        {
            Client client = _db.Clients.FirstOrDefault(c => c.Id == id);

            if (client == null)
                return BadRequest("Не найден клиент с идентификатором " + id.ToString());

            client.StateId = changeClient?.StateId ?? stateId ?? client.StateId;
            client.OutOfOrder = changeClient?.OutOfOrder ?? outOfOrder ?? client.OutOfOrder;

            if (client.StateId == (byte)ClientStateEnum.SERVICED)
            {
                client.EndTime ??= DateTime.Now;
            } else
            {
                client.EndTime = null;
            }

            _db.SaveChanges();

            return Ok(client);
        }

        /// <summary>
        /// Удалить клиента
        /// </summary>
        /// <returns>Список всех пользователей</returns>
        /// <param name="id">Идентификатор клиента</param>
        /// <response code="200">Возвращен список всех клиентов</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя недостаточно прав для просмотра клиента</response>
        [HttpDelete("{id:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<Client>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Authorize(Roles = RightsRef.ALL_CLIENTS + "," + RightsRef.DELETE_CLIENT)]
        public ActionResult<List<Client>> DeleteClient(int id)
        {
            Client client = _db.Clients.FirstOrDefault(c => c.Id == id);

            if (client != null)
            {
                _db.Clients.Remove(client);
            }

            return Ok(_db.Clients
                            .Include(c => c.ClientStages)
                                .ThenInclude(cs => cs.Position)
                            .Include(c => c.Queue)
                                .ThenInclude(q => q.QueuesGroup)
                            .Include(c => c.State)
                            .Where(c => c.CreateTime >= DateTime.Today));
        }
    }
}