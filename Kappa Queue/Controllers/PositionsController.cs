using System.Collections.Generic;
using System.Linq;
using KappaQueueCommon.Common.DTO;
using KappaQueueCommon.Common.References;
using KappaQueueCommon.Models.Context;
using KappaQueueCommon.Models.Positions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KappaQueue.Controllers
{
    /// <summary>
    /// Должности
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PositionsController : ControllerBase
    {
        private readonly QueueDBContext _db;
        public PositionsController(QueueDBContext context)
        {
            _db = context;
        }

        /// <summary>
        /// Получение всех должностей в системе
        /// </summary>
        /// <response code="200">В теле возвращен список должностей</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя нет прав на просмотр должностей</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<Position>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Produces("application/json")]
        [Authorize(Roles = RightsRef.ALL_POSITIONS + "," + RightsRef.GET_POSITIONS)]
        public ActionResult<List<Position>> GetPositions()
        {
            return Ok(_db.Positions.ToList());
        }

        /// <summary>
        /// Получение должности с идентификатором id
        /// </summary>
        /// <response code="200">Возвращена информация по должности</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя нет прав на просмотр должностей</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Position), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Produces("application/json")]
        [Authorize(Roles = RightsRef.ALL_POSITIONS + "," + RightsRef.GET_POSITION)]
        public ActionResult<List<Position>> GetPosition(int id)
        {
            return Ok(_db.Positions.FirstOrDefault(p => p.Id == id));
        }

        /// <summary>
        /// Создание должности
        /// </summary>
        /// <response code="200">Возвращена информация по созданной должности</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя нет прав на создание должностей</response>
        [HttpPost]
        [ProducesResponseType(typeof(Position), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize(Roles = RightsRef.ALL_POSITIONS + "," + RightsRef.CREATE_POSITION)]
        public ActionResult<Position> AddPosition([FromBody] PositionAddDto addPosition)
        {
            Position position = new Position(addPosition);
            _db.Positions.Add(position);
            _db.SaveChanges();
            return Ok(position);
        }

        /// <summary>
        /// Изменение должности
        /// </summary>
        /// <response code="200">Возвращена информация по созданной должности</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя нет прав на изменение должностей</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(Position), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Produces("application/json")]
        [Consumes("application/json")]
        [Authorize(Roles = RightsRef.ALL_POSITIONS + "," + RightsRef.CHANGE_POSITION)]
        public ActionResult<Position> ChangePosition(int id, [FromBody] PositionAddDto changePosition)
        {
            Position position = _db.Positions.FirstOrDefault(p => p.Id == id);
            position.AssignData(changePosition);
            _db.SaveChanges();
            return Ok(position);
        }

        /// <summary>
        /// Удаление должности
        /// </summary>
        /// <response code="200">Должность удалена, возвращена информация по всем должностям</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя нет прав на удаление должностей</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(List<Position>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Produces("application/json")]
        [Authorize(Roles = RightsRef.ALL_POSITIONS + "," + RightsRef.DELETE_POSITION)]
        public ActionResult<List<Position>> DeletePosition(int id)
        {
            Position position = _db.Positions.FirstOrDefault(p => p.Id == id);
            if (position != null)
            {
                _db.Positions.Remove(position);
                _db.SaveChanges();
            }
            return Ok(_db.Positions.ToList());
        }
    }
}