using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using KappaQueueCommon.Common.References;
using KappaQueueCommon.Models.Context;
using KappaQueueCommon.Models.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KappaQueue.Controllers
{
    [Route("/users")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class UsersFormController : Controller
    {
        private readonly QueueDBContext _db;

        /// <summary>
        /// Контроллер для работы с пользователями       
        /// </summary>
        /// <param name="context"></param>
        public UsersFormController(QueueDBContext context)
        {
            _db = context;
        }

        [Authorize(Roles = "allUsers")]
        public IActionResult Index()
        {
            if (HttpContext.User != null)
            {

                ViewData["UsersVisible"] = HttpContext.User.IsInRole(RightsRef.ALL_USERS)
                                                ? ""
                                                : "hidden";
                ViewData["RoomsVisible"] = HttpContext.User.IsInRole(RightsRef.ALL_USERS)
                                                ? ""
                                                : "hidden";
                ViewData["PositionsVisible"] = HttpContext.User.IsInRole(RightsRef.ALL_POSITIONS)
                                                ? ""
                                                : "hidden";
                ViewData["QueuesVisible"] = HttpContext.User.IsInRole(RightsRef.ALL_QUEUES)
                                                ? ""
                                                : "hidden";
                ViewData["ReportsVisible"] = HttpContext.User.IsInRole(RightsRef.ALL_USERS)
                                                ? ""
                                                : "hidden";
            }

            ViewBag.ShowHeader = '1';
            List<User> users = KappaQueueCommon.Models.Users.User.GetUsers(_db);
            return View(users);
        }

        /// <summary>
        /// Карточка пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <returns></returns>
        [Authorize(Roles = RightsRef.CREATE_USER)]
        [Route("card")]
        public IActionResult Card(int id)
        {
            if (HttpContext.User != null)
            {

                ViewData["UsersVisible"] = HttpContext.User.IsInRole(RightsRef.ALL_USERS)
                                                ? ""
                                                : "hidden";
                ViewData["RoomsVisible"] = HttpContext.User.IsInRole(RightsRef.ALL_USERS)
                                                ? ""
                                                : "hidden";
                ViewData["PositionsVisible"] = HttpContext.User.IsInRole(RightsRef.ALL_POSITIONS)
                                                ? ""
                                                : "hidden";
                ViewData["QueuesVisible"] = HttpContext.User.IsInRole(RightsRef.ALL_QUEUES)
                                                ? ""
                                                : "hidden";
                ViewData["ReportsVisible"] = HttpContext.User.IsInRole(RightsRef.ALL_USERS)
                                                ? ""
                                                : "hidden";
            }

            User user = KappaQueueCommon.Models.Users.User.GetUser(id, _db);
            return View(user);
        }
    }
}