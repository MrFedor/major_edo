using major_fansyspr;
using major_fansyspr.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using client_data.IdentityModels;
using client_data;
using client_data.Models;
using System.Reflection;

namespace web_client_usdep.Controllers
{
    [Authorize]
    public class DogovorDepositsController : Controller
    {
        private ClientContext db = new ClientContext();
        private FansySprContext fansy_db = new FansySprContext();
        private ApplicationUserManager _userManager;

        public DogovorDepositsController()
        { }

        public DogovorDepositsController(ApplicationUserManager userManager)
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

        // GET: DogovorDeposit
        public ActionResult Create(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var requestDeposit = db.RequestDeposits.Where(x => x.Id == id).FirstOrDefault();
            if (requestDeposit == null)
            {
                return HttpNotFound();
            }
            
            DogovorDeposits dogovorDeposit = new DogovorDeposits();
            foreach (PropertyInfo item in requestDeposit.GetType().GetProperties())
            {
                dogovorDeposit.GetType().GetProperty(item.Name).SetValue(dogovorDeposit, item.GetValue(requestDeposit));                
            }


            Client _client = fansy_db.Client.Where(o => o.Fansy_ID == requestDeposit.ClientId).FirstOrDefault();
            //List<Dogovor> _dogovor = fansy_db.Dogovor.Where(o => o.Client_Id == requestDeposit.ClientId).ToList();
            ViewBag.Client = _client;
            ViewBag.Portfolio = GetPortfolio(requestDeposit.ClientId);
            ViewBag.DepositCurrency = GetDepositCurrency(requestDeposit.DepositCurrency);
            ViewBag.SettlementCurrency = GetDepositCurrency(requestDeposit.SettlementCurrency);
            ViewBag.ValueTypes = GetValueTypes();
            ViewBag.ContributionType = GetContributionType();
            ViewBag.PeriodPayment = GetPeriodPayment();
            ViewBag.KoId = GetKoId(requestDeposit.KoId);
            ViewBag.FilialId = GetFilialId(requestDeposit.FilialId);
            ViewBag.ContributionDogType = GetContributionDogType();
            ViewBag.RubricaOut = GetCODE("RUBRICA_OUT", "DEPOSIT_DOG");
            ViewBag.AccountReturn = GetAccountReturn(requestDeposit.AccountReturn);
            ViewBag.KoAccountOpen = fansy_db.Banks.Where(r => r.CLIENT_ID == requestDeposit.KoAccountOpen).Select(r => r.BANKS_NAME).FirstOrDefault();
            dogovorDeposit.OutNumber = String.Empty;
            dogovorDeposit.OutDate = DateTime.Now.Date;
            dogovorDeposit.RequestId = requestDeposit.Id;


            return View(dogovorDeposit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DogovorDeposits dogovorDeposit, HttpPostedFileBase upload)
        {
            string user = User.Identity.GetUserId();
            var user_user = UserManager.FindById(user);
            Client _client = fansy_db.Client.Where(o => o.Fansy_ID == user_user.ClientFansyId).FirstOrDefault();
            //List<Dogovor> _dogovor = fansy_db.Dogovor.Where(o => o.Client_Id == user_user.ClientFansyId).ToList();
            DogovorDeposits _dogdepo = dogovorDeposit;
            _dogdepo.RequestDate = null;
            _dogdepo.RequestNum = String.Empty;
            _dogdepo.RequestStatus = -1;
            _dogdepo.RequestDescription = String.Empty;
            _dogdepo.StatusObrobotki = fansy_db.OD_USR_TABS.Where(r => r.CODE == "STATUS_TYPE" && r.DOP == "CREATED").Select(r => r.NUM).FirstOrDefault();
            if (ModelState.IsValid)
            {
                db.RequestDeposits.Add(_dogdepo);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Client = _client;
            ViewBag.Portfolio = GetPortfolio(dogovorDeposit.ClientId);
            ViewBag.DepositCurrency = GetDepositCurrency(dogovorDeposit.DepositCurrency);
            ViewBag.SettlementCurrency = GetDepositCurrency(dogovorDeposit.SettlementCurrency);
            ViewBag.ValueTypes = GetValueTypes();
            ViewBag.ContributionType = GetContributionType();
            ViewBag.PeriodPayment = GetPeriodPayment();
            ViewBag.KoId = GetKoId(dogovorDeposit.KoId);
            ViewBag.FilialId = GetFilialId(dogovorDeposit.FilialId);
            ViewBag.ContributionDogType = GetContributionDogType();
            ViewBag.RubricaOut = GetCODE("RUBRICA_OUT", "DEPOSIT_DOG");
            ViewBag.AccountReturn = GetAccountReturn(dogovorDeposit.AccountReturn);
            ViewBag.KoAccountOpen = fansy_db.Banks.Where(r => r.CLIENT_ID == dogovorDeposit.KoAccountOpen).Select(r => r.BANKS_NAME).FirstOrDefault();            

            return View(_dogdepo);
        }



        private List<ListCODEResult> GetCODE(string _str, string _select)
        {
            List<ListCODEResult> _od = new List<ListCODEResult>();
            _od = fansy_db.OD_USR_TABS.Where(r => r.CODE.Contains(_str)).Select(o =>
            new ListCODEResult
            {
                num = o.NUM,
                name = o.NAME,
                dop = o.DOP,
                alt = o.ALT,
                select = (_select.Equals(o.DOP))
            }
            ).ToList();

            return _od;
        }

        private IEnumerable<SelectListItem> GetPortfolio(int? client_id)
        {
            var _Departments = fansy_db.Dogovor
                .Where(o => o.Client_Id == client_id)
                .Select(x =>
                new SelectListItem
                {
                    Value = x.Fansy_ID.ToString(),
                    Text = x.Name
                }).AsEnumerable();

            return new SelectList(_Departments, "Value", "Text");
        }

        private IEnumerable<SelectListItem> GetDepositCurrency(int id)
        {
            var _Departments = fansy_db.OD_USR_TABS
                .Where(o => o.CODE.Contains("CURRENCY") && o.NUM == id)
                .Select(x =>
                new SelectListItem
                {
                    Value = x.NUM.ToString(),
                    Text = x.NAME
                }).AsEnumerable();

            return new SelectList(_Departments, "Value", "Text");
        }


        private IEnumerable<SelectListItem> GetPeriodPayment()
        {
            var _Departments = fansy_db.OD_USR_TABS
                .Where(o => o.CODE.Contains("PERCENT_MODE"))
                .Select(x =>
                new SelectListItem
                {
                    Value = x.NUM.ToString(),
                    Text = x.NAME
                }).AsEnumerable();

            return new SelectList(_Departments, "Value", "Text");
        }

        private IEnumerable<SelectListItem> GetKoId(int? id)
        {
            var _Departments = fansy_db.Banks
                .Where(r => r.CLIENT_ID == id)
                .Select(x =>
                new SelectListItem
                {
                    Value = x.CLIENT_ID.ToString(),
                    Text = x.BIC + " | " + x.BANKS_NAME                
                }).AsEnumerable();

            return new SelectList(_Departments, "Value", "Text");
        }
        
        private IEnumerable<SelectListItem> GetFilialId(int? id)
        {
            var _Departments = fansy_db.Banks
                .Where(r => r.CLIENT_ID == id)
                .Select(x =>
                new SelectListItem
                {
                    Value = x.CLIENT_ID.ToString(),
                    Text = x.BANKS_NAME
                }).AsEnumerable();

            return new SelectList(_Departments, "Value", "Text");
        }

        private IEnumerable<SelectListItem> GetAccountReturn(int? id)
        {
            var _Departments = fansy_db.BanksAccount
                .Where(r => r.ACC_ID == id)
                .Select(x =>
                new SelectListItem
                {
                    Value = x.ACC_ID.ToString(),
                    Text = x.ACNT
                }).AsEnumerable();

            return new SelectList(_Departments, "Value", "Text");
        }

        private IEnumerable<SelectListItem> GetContributionDogType()
        {
            var _Departments = fansy_db.OD_USR_TABS
                .Where(o => o.CODE.Contains("D_SHARE_TYPE"))
                .Select(x =>
                new SelectListItem
                {
                    Value = x.NUM.ToString(),
                    Text = x.NAME
                }).AsEnumerable();

            return new SelectList(_Departments, "Value", "Text");
        }
        private IEnumerable<SelectListItem> GetContributionType()
        {
            var _Departments = fansy_db.OD_USR_TABS
                .Where(o => o.CODE.Contains("PERCENT_TYPE"))
                .Select(x =>
                new SelectListItem
                {
                    Value = x.NUM.ToString(),
                    Text = x.NAME
                }).AsEnumerable();

            return new SelectList(_Departments, "Value", "Text");
        }

        private IEnumerable<SelectListItem> GetValueTypes()
        {
            var _Departments = fansy_db.OD_USR_TABS
                .Where(o => o.CODE.Contains("VALUE_TYPES"))
                .Select(x =>
                new SelectListItem
                {
                    Value = x.NUM.ToString(),
                    Text = x.NAME,
                    Selected = (x.NAME == "Денежные средства")
                }).AsEnumerable();

            return new SelectList(_Departments, "Value", "Text");
        }

    }
}