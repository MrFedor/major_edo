using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace major_web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AppUserController : Controller
    {
        // GET: AppUser
        public ActionResult Index()
        {
            return View();
        }
    }
}