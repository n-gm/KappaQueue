using KappaQueueCommon.Common.DTO;
using KappaQueueCommon.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace KappaQueue.Controllers
{
    /// <summary>
    /// Управление пользователями
    /// </summary>
    public partial class UsersController : ControllerBase
    {
        /// <summary>
        /// Получить информацию о ролях пользователя
        /// </summary>
        /// <response code="200">Возвращен список ролей пользователя</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя недостаточно прав для просмотра ролей пользователя</response>
        [HttpGet("{id}/roles")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<UserRole>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Authorize(Roles = "manager,admin")]
        public ActionResult<List<UserRole>> GetUserRoles(int id)
        {
            User user = _db.Users.Include(u => u.Roles).FirstOrDefault(u => u.Id == id);

            return Ok(user.Roles);
        }

        /// <summary>
        /// Добавить роли пользователю
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <param name="addRoles">Список добавляемых идентификаторов ролей</param>
        /// <response code="200">Роли добавлены, в тело запроса возвращен актуальный список ролей пользователя</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя недостаточно прав для добавления ролей</response>
        [HttpPost("{id}/roles")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<UserRole>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Authorize(Roles = "manager,admin")]
        public ActionResult<List<UserRole>> AddUserRoles(int id, [FromBody] UserRoleAddDto addRoles)
        {
            User user = _db.Users.Include(u => u.Roles).FirstOrDefault(u => u.Id == id);

            foreach (byte roleId in addRoles.Roles)
            {
                if (user.Roles.FirstOrDefault(u => u.Id == roleId) == null)
                    user.Roles.Add(_db.UserRoles.FirstOrDefault(ur => ur.Id == roleId));
            }

            _db.SaveChanges();

            return Ok(user.Roles);
        }

        /// <summary>
        /// Изменить роли пользователя - недостающие роли добавляются, лишние - удаляются
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <param name="addRoles">Список добавляемых идентификаторов ролей</param>
        /// <response code="200">Роли изменены, в тело запроса возвращен актуальный список ролей пользователя</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя недостаточно прав для изменения ролей</response>
        [HttpPut("{id}/roles")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<UserRole>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Authorize(Roles = "manager,admin")]
        public ActionResult<List<UserRole>> ChangeUserRoles(int id, [FromBody] UserRoleAddDto addRoles)
        {
            User user = _db.Users.Include(u => u.Roles).FirstOrDefault(u => u.Id == id);

            user.Roles.Clear();

            foreach (byte roleId in addRoles.Roles)
            {
                user.Roles.Add(_db.UserRoles.FirstOrDefault(ur => ur.Id == roleId));
            }

            _db.SaveChanges();

            return Ok(user.Roles);
        }

        /// <summary>
        /// Удалить роль пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <param name="roleId">Идентификатор удаляемой роли</param>
        /// <response code="200">Роль удалена, в тело запроса возвращен актуальный список ролей пользователя</response>
        /// <response code="401">Пользователь не аутентифицирован</response>
        /// <response code="403">У пользователя недостаточно прав для удаления ролей</response>
        [HttpDelete("{id}/roles/{roleId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<UserRole>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [Authorize(Roles = "manager,admin")]
        public ActionResult<List<UserRole>> ChangeUserRoles(int id, byte roleId)
        {
            User user = _db.Users.Include(u => u.Roles).FirstOrDefault(u => u.Id == id);

            user.Roles.Remove(user.Roles.FirstOrDefault(r => r.Id == roleId));

            _db.SaveChanges();

            return Ok(user.Roles);
        }
    }
}
