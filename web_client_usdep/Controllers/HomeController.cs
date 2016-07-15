using major_fansyspr;
using major_fansyspr.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using client_data.IdentityModels;
using client_data;
using client_data.Models;

namespace web_client_usdep.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ClientContext db = new ClientContext();
        private FansySprContext fansy_db = new FansySprContext();
        private ApplicationUserManager _userManager;

        public HomeController()
        {
        }

        public HomeController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index()
        {
            string user = User.Identity.GetUserId();
            ApplicationUser user_user = UserManager.FindById(user);

            IEnumerable<ListCODEResult> _rubric = fansy_db.OD_USR_TABS.Where(r => r.CODE.Contains("RUBRICA_OUT")).Select(o =>
            new ListCODEResult
            {
                num = o.NUM,
                name = o.NAME,
                dop = o.DOP,
                alt = o.ALT
            }).AsEnumerable();



            IEnumerable<ListCODEResult> _status = fansy_db.OD_USR_TABS.Where(r => r.CODE.Contains("STATUS_TYPE")).Select(o =>
            new ListCODEResult
            {
                num = o.NUM,
                name = o.NAME,
                dop = o.DOP,
                alt = o.ALT
            }).AsEnumerable();
            

            IEnumerable<Dogovor> _dog = fansy_db.Dogovor.Where(r => r.Client_Id == user_user.ClientFansyId).AsEnumerable();
            List<RequestDeposits> _tmp = db.RequestDeposits.OrderBy(o => o.OutDate).Where(o => o.AppUserId == user_user.Id).ToList();
            List<ListIndexResult> _listIndexResult = new List<ListIndexResult>();
            foreach (RequestDeposits item in _tmp)
            {
                _listIndexResult.Add(new ListIndexResult
                {
                    Id = item.Id,
                    outNumber = item.OutNumber,
                    outDate = item.OutDate,
                    rubricaOut = _rubric.Where(r => r.num == item.RubricaOut).Select(r => r.name).FirstOrDefault(),
                    fond = _dog.Where(r => r.Fansy_ID == item.PortfolioId).Select(r => r.Name).FirstOrDefault(),
                    statusObrobotki = _status.Where(r => r.num == item.StatusObrobotki).Select(r => r.name).FirstOrDefault()
                });
            }

            ViewBag.History = _listIndexResult;

            return View();
        }

        
    }
}