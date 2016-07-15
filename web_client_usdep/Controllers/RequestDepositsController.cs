using major_fansyspr;
using major_fansyspr.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using client_data;
using client_data.IdentityModels;
using client_data.Models;

namespace web_client_usdep.Controllers
{
    [Authorize]
    public class RequestDepositsController : Controller
    {
        private ClientContext db = new ClientContext();
        private FansySprContext fansy_db = new FansySprContext();
        private ApplicationUserManager _userManager;

        public RequestDepositsController()
        {
        }

        public RequestDepositsController(ApplicationUserManager userManager)
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

        // GET: RequestDeposits
        public ActionResult Index()
        {
            
            return View();
        }

        // GET: RequestDeposits/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestDeposits requestDeposit = db.RequestDeposits.Find(id);
            if (requestDeposit == null)
            {
                return HttpNotFound();
            }
            return View(requestDeposit);
        }

        // GET: RequestDeposits/Create
        public ActionResult Create()
        {
            string user = User.Identity.GetUserId();
            ApplicationUser user_user = UserManager.FindById(user);
            Client _client = fansy_db.Client.Where(o => o.Fansy_ID == user_user.ClientFansyId).FirstOrDefault();
            //List<Dogovor> _dogovor = fansy_db.Dogovor.Where(o => o.Client_Id == user_user.ClientFansyId).ToList();

            ViewBag.Client = _client;
            ViewBag.Portfolio = GetPortfolio(user_user.ClientFansyId);
            ViewBag.DepositCurrency = GetDepositCurrency();
            ViewBag.ValueTypes = GetValueTypes();
            ViewBag.ContributionType = GetContributionType();
            ViewBag.PeriodPayment = GetPeriodPayment();
            ViewBag.KoId = GetKoId();
            ViewBag.ContributionDogType = GetContributionDogType();            
            ViewBag.RubricaOut = GetCODE("RUBRICA_OUT", "DEPOSIT_DOG");
            ViewBag.List = Enumerable.Empty<SelectListItem>();


            RequestDeposits _requestDeposits = new RequestDeposits();
            _requestDeposits.ClientId = _client.Fansy_ID;
            _requestDeposits.Request = true;
            _requestDeposits.AppUserId = user_user.Id;

            return View(_requestDeposits);
        }

        // POST: RequestDeposits/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RequestDeposits requestDeposit)
        {
            string user = User.Identity.GetUserId();
            var user_user = UserManager.FindById(user);
            Client _client = fansy_db.Client.Where(o => o.Fansy_ID == user_user.ClientFansyId).FirstOrDefault();
            //List<Dogovor> _dogovor = fansy_db.Dogovor.Where(o => o.Client_Id == user_user.ClientFansyId).ToList();
            //RequestDeposits _reqdepo = requestDeposit;
            requestDeposit.RequestDate = null;
            requestDeposit.RequestNum = String.Empty;
            requestDeposit.RequestStatus = -1;
            requestDeposit.RequestDescription = String.Empty;
            //requestDeposit.AgreementContractDate = null;
            //requestDeposit.ChangesDogDate = null;
            //requestDeposit.TerminationDogDate = null;
            requestDeposit.StatusObrobotki = fansy_db.OD_USR_TABS.Where(r=>r.CODE == "STATUS_TYPE" && r.DOP == "CREATED").Select(r=>r.NUM).FirstOrDefault();
            requestDeposit.Request = true;
            if (ModelState.IsValid)
            {
                db.RequestDeposits.Add(requestDeposit);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            List<SelectListItem> _resualt = new List<SelectListItem>();
            
                _resualt = fansy_db.BanksAccount
                .Where(r => r.CLIENT_ID == requestDeposit.PortfolioId)
                .Select(x =>
                new SelectListItem
                {                    
                    Value = x.ACC_ID.ToString(),
                    Text = x.ACNT,
                    Selected = (x.ACC_ID == requestDeposit.AccountReturn)
                }).ToList();

            ViewBag.List = _resualt;



            ViewBag.Portfolio = GetPortfolio(user_user.ClientFansyId);
            ViewBag.DepositCurrency = GetDepositCurrency();
            ViewBag.ValueTypes = GetValueTypes();
            ViewBag.Client = _client;
            ViewBag.ContributionType = GetContributionType();
            ViewBag.PeriodPayment = GetPeriodPayment();
            ViewBag.KoId = GetKoId();
            ViewBag.ContributionDogType = GetContributionDogType();
            ViewBag.RubricaOut = GetCODE("RUBRICA_OUT", "DEPOSIT_DOG");

            return View(requestDeposit);
        }

        // GET: RequestDeposits/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestDeposits requestDeposit = db.RequestDeposits.Find(id);
            if (requestDeposit == null)
            {
                return HttpNotFound();
            }
            return View(requestDeposit);
        }

        // POST: RequestDeposits/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RequestDeposits requestDeposit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(requestDeposit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(requestDeposit);
        }

        // GET: RequestDeposits/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RequestDeposits requestDeposit = db.RequestDeposits.Find(id);
            if (requestDeposit == null)
            {
                return HttpNotFound();
            }
            return View(requestDeposit);
        }

        // POST: RequestDeposits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            RequestDeposits requestDeposit = db.RequestDeposits.Find(id);
            db.RequestDeposits.Remove(requestDeposit);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private List<ListCODEResult> GetCODE(string _str, string _select)
        {
            List<ListCODEResult> _od = new List<ListCODEResult>();
            _od = fansy_db.OD_USR_TABS.Where(r => r.CODE.Contains(_str)).OrderBy(r=>r.NUM).Select(o=>
            new ListCODEResult {
                num = o.NUM,
                name = o.NAME,
                dop = o.DOP,
                alt = o.ALT,
                select = (_select.Equals(o.DOP)) }
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
        
        private IEnumerable<SelectListItem> GetDepositCurrency()
        {
            var _Departments = fansy_db.OD_USR_TABS
                .Where(o => o.CODE.Contains("CURRENCY"))
                .OrderBy(x=>x.NUM)
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

        private IEnumerable<SelectListItem> GetKoId()
        {
            var _Departments = fansy_db.Banks
                .Where(r => r.OWNER_ID == 0)
                .Select(x =>
                new SelectListItem
                {
                    Value = x.CLIENT_ID.ToString(),
                    Text = x.BIC + " | " + x.BANKS_NAME
                }).AsEnumerable();

            return new SelectList(_Departments, "Value", "Text");
        }

        [HttpGet]
        public JsonResult GetAccountReturn(int? id)
        {
            List<ListJsonResult> _resualt = new List<ListJsonResult>();
            if (id != null)
            {
                _resualt = fansy_db.BanksAccount
                .Where(r => r.CLIENT_ID == id)
                .Select(x =>
                new ListJsonResult
                {
                    value = x.ACC_ID.ToString(),
                    text = x.ACNT
                }).ToList();
            }

            return Json(_resualt, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetKoAccountOpen(int? id)
        {
            ListJsonResult _resualt = new ListJsonResult();
            if (id != null)
            {
                BanksAccount _banksAccount = fansy_db.BanksAccount.Where(r => r.ACC_ID == id).FirstOrDefault();
                if (_banksAccount != null)
                {
                    _resualt = fansy_db.Banks
                        .Where(r => r.CLIENT_ID == _banksAccount.BANKS_ID)
                        .Select(x =>
                        new ListJsonResult
                        {
                            value = x.CLIENT_ID.ToString(),
                            text = x.BANKS_NAME
                        }).FirstOrDefault();
                }

            }

            return Json(_resualt, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetFilialId(int? id)
        {
            List<ListJsonResult> _resualt = new List<ListJsonResult>();
            if (id != null)
            {
                _resualt = fansy_db.Banks
                .Where(r => r.OWNER_ID == id)
                .Select(x =>
                new ListJsonResult
                {
                    value = x.CLIENT_ID.ToString(),
                    text = x.BANKS_NAME
                }).ToList();
            }

            return Json(_resualt, JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<SelectListItem> GetContributionDogType()
        {
            var _Departments = fansy_db.OD_USR_TABS
                .Where(o => o.CODE.Equals("D_SHARE_TYPE"))
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


        [HttpGet]
        public JsonResult GetNumDateDU(int? id)
        {
            CultureInfo provider = CultureInfo.CreateSpecificCulture("ru-RU");
            string _resualt = String.Empty;
            if (id != null)
            {
                var _dog_num_date = fansy_db.Dogovor.Where(o => o.Fansy_ID == id).FirstOrDefault();                                
                if (_dog_num_date != null)
                {
                    _resualt = String.Format("№ {0} от {1}", _dog_num_date.DogovorNum, String.Format("{0:dd'/'MM'/'yyyy}", _dog_num_date.DogovorDate));
                }
                else
                {
                    _resualt = "В Базе не найден Номер и дата Правил ДУ";
                }
            }


            return Json(_resualt, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetXml(Guid id)
        {
            RequestDeposits _req = new RequestDeposits();
            string filename = string.Format("{0:dd-MM-yyyy}.xml", DateTime.Now);

            _req = db.RequestDeposits.Include(p => p.PercentPeriods).Where(o => o.Id == id).FirstOrDefault();

            XmlSerializer xml = new XmlSerializer(_req.GetType());
            var stream = new System.IO.MemoryStream();
            xml.Serialize(stream, _req);
            stream.Position = 0;
            stream.Flush();
            if (_req.Request == true)
            {
                string _out = _req.OutNumber.Replace("/", "_");
                _out = _out.Replace("\\","_");
                _out = _out.Replace(" ", "_");
                string _fond = fansy_db.Dogovor.Where(r => r.Fansy_ID == _req.PortfolioId).Select(r => r.Name).FirstOrDefault();
                _fond = _fond.Replace('"','_');
                _fond = _fond.Replace("'", "_");
                _fond = _fond.Replace(" ", "_");
                filename = string.Format("Запрос_{0}_{1}.xml", _out, _fond);
            }
            else
            {
                string _out = _req.OutNumber.Replace("/", "_");
                _out = _out.Replace("\\", "_");
                _out = _out.Replace(" ", "_");
                string _fond = fansy_db.Dogovor.Where(r => r.Fansy_ID == _req.PortfolioId).Select(r => r.Name).FirstOrDefault();
                _fond = _fond.Replace('"', '_');
                _fond = _fond.Replace("'", "_");
                _fond = _fond.Replace(" ", "_");
                filename = string.Format("Договор_{0}_{1}.xml", _out, _fond);
            }
            

            return File(stream, "application/xml", filename);
        }
    }
}
