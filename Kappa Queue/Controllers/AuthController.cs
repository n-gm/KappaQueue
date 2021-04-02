﻿using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using KappaQueueCommon.Common.DTO;
using KappaQueueCommon.Models.Context;
using KappaQueueCommon.Models.Users;
using KappaQueueCommon.Utils;
using KappaQueueEvents.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KappaQueue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly QueueDBContext _db;
        private readonly IEventHandler _handler; 

        public AuthController(QueueDBContext context, IEventHandler eventHandler)
        {
            _db = context;
        }
        
        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="username">Имя пользователя</param>
        /// <param name="password">Пароль</param>
        /// <response code="200">Успешная авторизация, в теле ответа вернулся JWT-токен</response>
        /// <response code="401">В случае неверного логина/пароля возвращается ошибка 401</response>        
        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(401)]
        [AllowAnonymous]

        public ActionResult<string> Auth(string username,
                                        string password)
        {
            User user = _db.Users.Include(ur => ur.Roles).FirstOrDefault(u => u.Username.Equals(username) && !u.Blocked);

            if (user?.CheckPassword(password) ?? false)
            {
                return Ok(AuthUtils.CreateEncryptedToken(user));
            }
            else
            {
                return Unauthorized();
            }
        }

        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="node">Информация для авторизации</param>
        /// <response code="200">Успешная авторизация, в теле ответа вернулся JWT-токен</response>
        /// <response code="401">В случае неверного логина/пароля возвращается ошибка 401</response>
        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(401)]
        [Consumes("application/json")]
        [AllowAnonymous]

        public ActionResult<string> Auth([FromBody] AuthorizationNode node)
        {
            return Auth(node.Username, node.Password);
        }
        
        /// <summary>
        /// Обновление токена
        /// </summary>
        /// <response code="200">В теле ответа вернулся обновленный JWT-токен</response>
        /// <response code="401">В случае отсутствия аутентификации возвращается ошибка</response>
        /// <response code="402">В случае отсутствия пользователя или необходимого ключа в токене возвращается ошибка запроса</response>
        [HttpGet("refresh-token")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(401)]
        [Authorize(Roles = "admin, manager, ticketer, performer")]
        public ActionResult<string> RefreshToken()
        {
            Claim sid = HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(JwtRegisteredClaimNames.Sid));
            if (sid == null)
                return BadRequest();

            int userId = int.Parse(sid.Value);

            User user = _db.Users.FirstOrDefault(u => u.Id == userId && !u.Blocked);

            if (user == null)
                return BadRequest();

            return Ok(AuthUtils.CreateEncryptedToken(user));
        }
    }
}