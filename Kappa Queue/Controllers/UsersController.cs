using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Claims;
using KappaQueueCommon.Common.DTO;
using KappaQueueCommon.Common.References;
using KappaQueueCommon.Models.Context;
using KappaQueueCommon.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace KappaQueue.Controllers
{
    /// <summary>
    /// Управление пользователями
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public partial class UsersController : ControllerBase
    {
        private readonly QueueDBContext _db;

        /// <summary>
        /// Контроллер для работы с пользователями       
        /// </summary>
        /// <param name="context"></param>
        public UsersController(QueueDBContext context)
        {
            _db = context;
        }

        /// <summary>
        /// Получить информацию по всем пользователям
        /// </summary>
        /// <returns>Список всех пользователей</returns>
        /// <response code="200">Возвращен список всех пользователей в системе</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя недостаточно прав для просмотра всех пользователей</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<User>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Authorize(Roles = RightsRef.ALL_USERS + "," + RightsRef.GET_USERS)]
        public ActionResult<List<User>> GetUsers()
        {
            return Ok(_db.Users
                        .Include(u => u.Roles)
                            .ThenInclude(ur => ur.UserRights)
                        .Include(u => u.Rooms)
                        .Include(u => u.Positions)
                        .Include(u => u.Status)
                        .ToList());
        }

        /// <summary>
        /// Получить информацию о пользователе
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <response code="200">Возвращен пользователь с идентификатором id</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя недостаточно прав для просмотра пользователя</response>
        /// <response code="404">Пользователь с запрошенным идентификатором не найден</response>
        [HttpGet("{id:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        [Authorize(Roles = RightsRef.ALL_USERS + "," + RightsRef.GET_USER)]
        public ActionResult<User> GetUser(int id)
        {
            User user = _db.Users
                            .Include(u => u.Roles)
                                .ThenInclude(ur => ur.UserRights)
                            .Include(u => u.Rooms)
                            .Include(u => u.Positions)
                            .Include(u => u.Status)
                            .FirstOrDefault(u => u.Id == id);

            if (user != null)
                return Ok(user);
            else
                return NotFound();

        }

        /// <summary>
        /// Создать пользователя
        /// </summary>
        /// <returns>Список всех пользователей</returns>
        /// <response code="200">Пользователь успешно создан, в ответ возвращена информация по пользователю</response>
        /// <response code="400">Пользователь с заданными логином уже существует</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя недостаточно прав для создания других пользователей</response>
        /// <response code="415">В заголовке неверно указано поле Content-type, либо в теле сообщения содержится не JSON</response>
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(415)]
        [Authorize(Roles = RightsRef.ALL_USERS + "," + RightsRef.CREATE_USER)]
        public ActionResult<User> AddUser([FromBody]UserAddDto addUser)
        {
            if (_db.Users.FirstOrDefault(u => u.Username.Equals(addUser.Username)) != null)
                return BadRequest("Пользователь с заданным логином уже существует");

            User user = new User(addUser);
            if (ModelState.IsValid)
            {
                _db.Users.Add(user);
                _db.SaveChanges();

                UserStatus status = new UserStatus(user);
                user.Status = status;
                _db.SaveChanges();

                return Ok(user);
            } else
            {
                return BadRequest("");
            }
        }

        /// <summary>
        /// Изменить данные пользователя
        /// </summary>
        /// <returns>Список всех пользователей</returns>
        /// <response code="200">Пользователь успешно создан, в ответ возвращена информация по пользователю</response>
        /// <response code="401">Пользователь не аутентифицирован или его sid в токене не совпадает с идентификатором пользователя</response>
        /// <response code="403">У пользователя недостаточно прав для удаления пользователей</response>
        [HttpPut("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Consumes("application/json")]
        [Authorize]
        public ActionResult<User> ChangeUser(int id, [FromBody]UserChangeDto changeUser)
        {
            Claim sid = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(JwtRegisteredClaimNames.Sid));
            if (sid == null)
            {
                return Unauthorized();
            }

            if (sid.Value.Equals(id.ToString())
                || HttpContext.User.IsInRole(RightsRef.CHANGE_USER))
            {
                User user = _db.Users.FirstOrDefault(u => u.Id == id);
                user.AssignData(changeUser);
                _db.SaveChanges();

                return Ok(user);
            } else
            {
                return Forbid();
            }
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <returns>Список всех пользователей</returns>
        /// <response code="200">Пользователь успешно создан, в ответ возвращена информация по пользователю</response>
        /// <response code="400">Пользователь попытался удалить системного пользователя</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя недостаточно прав для удаления пользователей</response>
        [HttpDelete("{id:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Authorize(Roles = RightsRef.ALL_USERS + "," + RightsRef.DELETE_USER)]
        public ActionResult<User> DeleteUser(int id)
        {
            if (id == 1)
            {
                return BadRequest("Нельзя удалять системного пользователя");
            }

            User user = _db.Users.FirstOrDefault(u => u.Id == id);
            user.Blocked = true;
            _db.SaveChanges();

            return Ok(user);
        }
    }
}