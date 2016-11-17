using major_data;
using major_data.IdentityModels;
using major_data.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace major_web.Controllers
{
    public class RuleSystemsController : Controller
    {
        private ApplicationUserManager _userManager;
        private UserContext db = new UserContext();

        public RuleSystemsController()
        {
        }

        public RuleSystemsController(ApplicationUserManager userManager)
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

        // GET: RuleSystems
        public ActionResult Index()
        {
            var ruleSystem = db.RuleSystem.Include(r => r.AssetType).Include(r => r.Department).Include(r => r.Dogovor.Client).Include(r => r.Fond).Include(r => r.Secshondeportament);
            var _to_list = db.Users.OrderBy(o=>o.FirstName).ToList();
            ViewBag.AddUserList = new SelectList(_to_list, "Id", "FirstName");
            return View(ruleSystem.ToList());
        }

        public ActionResult SearchRuleSystems(string AssetType, string Department, string Client, string Fond, string Secshondeportament, string id_user)
        {
            var _rule = db.RuleSystem
                .Include(o => o.AssetType)
                .Include(o => o.Department)
                .Include(o => o.Dogovor.Client)
                .Include(o => o.Fond)
                .Include(o => o.Secshondeportament)
                .Include(o => o.RuleUsers)
                .ToList();

            if (!String.IsNullOrEmpty(AssetType))
            {
                _rule = _rule.Where(o => o.AssetType.Name.ToLower().Contains(AssetType.ToLower())).ToList();
            }
            if (!String.IsNullOrEmpty(Department))
            {
                _rule = _rule.Where(o => o.Department.Name.ToLower().Contains(Department.ToLower())).ToList();
            }
            if (!String.IsNullOrEmpty(Client))
            {
                _rule = _rule.Where(o => o.Dogovor.Client.Name.ToLower().Contains(Client.ToLower())).ToList();
            }
            if (!String.IsNullOrEmpty(Fond))
            {
                _rule = _rule.Where(o => o.Fond != null && o.Fond.Name.ToLower().Contains(Fond.ToLower())).ToList();
            }
            if (!String.IsNullOrEmpty(Secshondeportament))
            {
                _rule = _rule.Where(o => o.Secshondeportament.Name.ToLower().Contains(Secshondeportament.ToLower())).ToList();
            }
            if (!String.IsNullOrEmpty(id_user))
            {
                //var _us_rule = db.RuleUser.Where(o => o.AppUserId == id_user).ToList();
                _rule = _rule.Where(o => o.RuleUsers.Where(v=>v.AppUserId == id_user).Select(p=>p.AppUserId).AsEnumerable().Contains(id_user)).ToList();
            }

            return PartialView("SearchRuleSystems", _rule);
        }



        // GET: RuleSystems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RuleSystem ruleSystem = db.RuleSystem.Find(id);
            if (ruleSystem == null)
            {
                return HttpNotFound();
            }
            return View(ruleSystem);
        }

        // GET: RuleSystems/Create
        public ActionResult Create()
        {
            RuleSystem _rule = new RuleSystem();
            _rule.Type = 3;
            _rule.StartDate = DateTime.Now;
            _rule.UseRule = true;
            _rule.NumberRule = 0;

            ViewBag.AssetTypeId = new SelectList(db.AssetType.OrderBy(o=>o.Name).ToList(), "Id", "Name");
            ViewBag.DepartmentId = new SelectList(db.Department.OrderBy(o => o.Name).ToList(), "Id", "Name");
            ViewBag.DogovorId = new SelectList(db.Dogovor, "Id", "DogovorNum");
            ViewBag.FondId = new SelectList(db.Fond.OrderBy(o => o.Name).ToList(), "Id", "Name");
            ViewBag.SecshondeportamentId = new SelectList(db.Secshondeportament.OrderBy(o => o.Name).ToList(), "Id", "Name");

            ViewBag.AppUserList = new MultiSelectList(db.Users.OrderBy(o => o.FirstName).ToList(), "Id", "UserName");
            return View(_rule);
        }

        // POST: RuleSystems/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RuleSystem ruleSystem, string[] AppUsers)
        {
            if (ModelState.IsValid)
            {
                if (AppUsers != null)
                {
                    foreach (string item in AppUsers)
                    {
                        ruleSystem.RuleUsers.Add(new RuleUser { AppUserId = item, NotifFile = false });
                    }
                }

                db.RuleSystem.Add(ruleSystem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssetTypeId = new SelectList(db.AssetType, "Id", "Name", ruleSystem.AssetTypeId);
            ViewBag.DepartmentId = new SelectList(db.Department, "Id", "Name", ruleSystem.DepartmentId);
            ViewBag.DogovorId = new SelectList(db.Dogovor, "Id", "DogovorNum", ruleSystem.DogovorId);
            ViewBag.FondId = new SelectList(db.Fond, "Id", "Name", ruleSystem.FondId);
            ViewBag.SecshondeportamentId = new SelectList(db.Secshondeportament, "Id", "Name", ruleSystem.SecshondeportamentId);

            ViewBag.AppUserList = new MultiSelectList(db.Users, "Id", "UserName");
            return View(ruleSystem);
        }

        private IEnumerable<SelectListItem> GetDogovorIdClient(int? id)
        {
            var _GetDogovorIdClient = db.Dogovor
                .Include(r => r.Client)
                .OrderBy(r => r.Client.Name)
                .Select(x =>
                new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Client.Name,
                    Selected = (x.Id == id)
                }).AsEnumerable();

            return _GetDogovorIdClient;
        }

        // GET: RuleSystems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RuleSystem ruleSystem = db.RuleSystem.Find(id);
            if (ruleSystem == null)
            {
                return HttpNotFound();
            }
            ViewBag.AssetTypeId = new SelectList(db.AssetType, "Id", "Name", ruleSystem.AssetTypeId);
            ViewBag.DepartmentId = new SelectList(db.Department, "Id", "Name", ruleSystem.DepartmentId);
            ViewBag.DogovorId = GetDogovorIdClient(ruleSystem.DogovorId);

            //ViewBag.DogovorId = new SelectList(db.Dogovor, "Id", "DogovorNum", ruleSystem.DogovorId);
            ViewBag.FondId = new SelectList(db.Fond.OrderBy(r => r.Name), "Id", "Name", ruleSystem.FondId);
            ViewBag.SecshondeportamentId = new SelectList(db.Secshondeportament, "Id", "Name", ruleSystem.SecshondeportamentId);
            var _list_user_in = db.Users.Include("Secshondeportament").Where(o => o.RuleUsers.Where(p => p.RuleSystemId == id).Select(k => k.RuleSystemId).FirstOrDefault() == id).ToList();
            ViewBag.ListUsers = _list_user_in;
            var _to_list = db.Users.ToList().Except(_list_user_in).ToList();
            ViewBag.AddUserList = new SelectList(_to_list, "Id", "FirstName");

            //var _arr = ruleSystem.RuleUsers.Where(o => o.RuleSystem == ruleSystem).Select(p => p.AppUser).ToArray();
            //ViewBag.AppUserList = new MultiSelectList(db.Users.OrderBy(o => o.UserName), "Id", "UserName", ruleSystem.RuleUsers.Where(o => o.RuleSystem == ruleSystem).Select(p=>p.AppUser).ToArray());
            return View(ruleSystem);
        }

        // POST: RuleSystems/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RuleSystem ruleSystem, string[] AppUsers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ruleSystem).State = EntityState.Modified;
                db.Entry(ruleSystem).Property(o => o.DateLastFolder).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AssetTypeId = new SelectList(db.AssetType, "Id", "Name", ruleSystem.AssetTypeId);
            ViewBag.DepartmentId = new SelectList(db.Department, "Id", "Name", ruleSystem.DepartmentId);
            ViewBag.DogovorId = GetDogovorIdClient(ruleSystem.DogovorId);
            //ViewBag.DogovorId = new SelectList(db.Dogovor, "Id", "DogovorNum", ruleSystem.DogovorId);
            ViewBag.FondId = new SelectList(db.Fond.OrderBy(r => r.Name), "Id", "Name", ruleSystem.FondId);
            ViewBag.SecshondeportamentId = new SelectList(db.Secshondeportament, "Id", "Name", ruleSystem.SecshondeportamentId);
            ViewBag.ListUsers = db.Users.Include("Secshondeportament").Where(o => o.RuleUsers.Where(p => p.RuleSystemId == ruleSystem.Id).Select(k => k.RuleSystemId).FirstOrDefault() == ruleSystem.Id).ToList();
            //ViewBag.AppUserList = new MultiSelectList(db.Users.OrderBy(o => o.UserName), "Id", "UserName", ruleSystem.RuleUsers.Select(o=>o.RuleSystem == ruleSystem).ToArray());
            return View(ruleSystem);
        }

        // GET: RuleSystems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RuleSystem ruleSystem = db.RuleSystem.Find(id);
            if (ruleSystem == null)
            {
                return HttpNotFound();
            }
            return View(ruleSystem);
        }

        // POST: RuleSystems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RuleSystem ruleSystem = db.RuleSystem.Find(id);
            db.RuleSystem.Remove(ruleSystem);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

       

        public async System.Threading.Tasks.Task<ActionResult> RemoveUser(int id, string id_user)
        {
            RuleUser ruleSystem = db.RuleUser.Where(o => o.AppUserId == id_user && o.RuleSystemId == id).FirstOrDefault();
            db.RuleUser.Remove(ruleSystem);
            await db.SaveChangesAsync();
            var _users = db.Users
                .Include(o => o.Secshondeportament)
                .Where(o => o.RuleUsers.Where(p => p.RuleSystemId == id).Select(k => k.RuleSystemId).FirstOrDefault() == id)                
                .ToList();
            ViewBag.Check = db.RuleUser.Where(p => p.RuleSystemId == id).ToList();
            return PartialView("ListUserForRule", _users);
        }
        public async System.Threading.Tasks.Task<ActionResult> AddUser(int id, string id_user)
        {
            db.RuleUser.Add(new RuleUser { AppUserId = id_user, RuleSystemId = id, NotifFile = false });
            await db.SaveChangesAsync();
            var _users = db.Users
                .Include(o => o.Secshondeportament)
                .Where(o => o.RuleUsers.Where(p => p.RuleSystemId == id).Select(k => k.RuleSystemId).FirstOrDefault() == id)
                .ToList();
            ViewBag.Check = db.RuleUser.Where(p => p.RuleSystemId == id).ToList();
            return PartialView("ListUserForRule", _users);
        }

        public async System.Threading.Tasks.Task<string> UpdateUser(int id, string id_user, bool checkbox_rule)
        {
            RuleUser ruleSystem = db.RuleUser.Where(o => o.AppUserId == id_user && o.RuleSystemId == id).FirstOrDefault();
            ruleSystem.NotifFile = checkbox_rule;

            string db_save = String.Empty;
            try
            {
                await db.SaveChangesAsync();
                db_save = "Данные сохранены";
            }
            catch (Exception ex)
            {
                db_save = String.Format("Ошибка: {0}", ex.Message);
            }
            

            return db_save;
        }

        public ActionResult ListUserForRule(int? id)
        {
            var _list_user_in = db.Users
                .Include(o => o.Secshondeportament)
                .Where(o => o.RuleUsers.Where(p => p.RuleSystemId == id).Select(k => k.RuleSystemId).FirstOrDefault() == id)
                .ToList();
            ViewBag.Check = db.RuleUser.Where(p => p.RuleSystemId == id).ToList();
            return PartialView("ListUserForRule", _list_user_in);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
