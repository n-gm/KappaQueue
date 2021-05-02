using KappaQueueCommon.Common.References;
using Microsoft.AspNetCore.Mvc;

namespace KappaQueue.Controllers
{
    [Route("auth")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AuthFormController : Controller
    {
        
        public IActionResult Index()
        {
            
            ViewBag.ShowHeader = '0';
     
            if (HttpContext.User != null
                && HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToRoute(new { controller = "UsersForm", action = "Index" });
                /*
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
                                                */
            }

            return View();
        }
    }
}