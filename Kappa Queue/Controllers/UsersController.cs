using System.Collections.Generic;
using System.Linq;
using KappaQueue.Common.DTO;
using KappaQueue.Models.Context;
using KappaQueue.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        public UsersController(QueueDBContext context)
        {
            _db = context;
        }

        /// <summary>
        /// Получить информацию о пользователе
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <response code="200">Возвращен пользователь с идентификатором id</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя недостаточно прав для просмотра пользователя</response>
        [HttpGet("{id:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Authorize(Roles = "manager,admin")]
        public ActionResult<User> GetUser(int id)
        {
            User user = _db.Users.Include(u => u.Roles).Include(u => u.Rooms).FirstOrDefault(u => u.Id == id);

            return Ok(user);
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
        [ProducesResponseType(403)]
        [Authorize(Roles = "manager,admin")]
        public ActionResult<List<User>> GetUsers()
        {
            return Ok(_db.Users.Include(u => u.Roles).Include(u => u.Rooms).Include(u => u.Positions).ToList());
        }
        
        /// <summary>
        /// Создать пользователя
        /// </summary>
        /// <returns>Список всех пользователей</returns>
        /// <response code="200">Пользователь успешно создан, в ответ возвращена информация по пользователю</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя недостаточно прав для создания других пользователей</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(User), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Consumes("application/json")]
        [Authorize(Roles = "manager,admin")]
        public ActionResult<User> AddUser([FromBody]UserAddDto addUser)
        {
            if (!HttpContext.User.IsInRole("manager") &&
                !HttpContext.User.IsInRole("admin"))
                return Forbid();

            if (_db.Users.FirstOrDefault(u => u.Username.Equals(addUser.Username)) != null)
                return BadRequest("Пользователь с заданным логином уже существует");

            User user = new User(addUser);
            _db.Users.Add(user);
            _db.SaveChanges();

            return Ok(user);
        }
        
    }
}