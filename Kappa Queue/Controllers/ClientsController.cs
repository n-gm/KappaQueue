using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KappaQueueCommon.Models.Clients;
using KappaQueueCommon.Models.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KappaQueue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly QueueDBContext _db;

        public ClientsController(QueueDBContext context)
        {
            _db = context;
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
        [Authorize(Roles = "manager,admin,ticketer")]
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
        /// Получить информацию по всем пользователям
        /// </summary>
        /// <returns>Список всех пользователей</returns>
        /// <param name="startTime">Дата, с которой возвращать выборку</param>
        /// <response code="200">Возвращен список всех пользователей в системе</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя недостаточно прав для просмотра всех пользователей</response>
        [HttpGet("{id:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<Client>), 200)]
        [ProducesResponseType(403)]
        [Authorize(Roles = "manager,admin,ticketer")]
        public ActionResult<List<Client>> GetClient(int id)
        {
            return Ok(_db.Clients
                            .Include(c => c.ClientStages)
                                .ThenInclude(cs => cs.Position)
                            .Include(c => c.Queue)
                                .ThenInclude(q => q.QueuesGroup)
                            .Include(c => c.State)
                            .Where(c => c.Id == id));
        }
    }
}