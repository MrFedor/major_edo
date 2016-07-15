using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using major_data.Models;
using System.Threading.Tasks;
using major_data.IdentityModels;
using Microsoft.AspNet.Identity.Owin;
using major_data;

namespace major_web.Controllers
{
    public class SecshondeportamentsController : Controller
    {
        private ApplicationUserManager _userManager;
        private UserContext db = new UserContext();

        public SecshondeportamentsController()
        {
        }

        public SecshondeportamentsController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: Secshondeportaments
        public ActionResult Index()
        {
            return View(db.Secshondeportament.OrderBy(o=>o.Name).ToList());
        }

        // GET: Secshondeportaments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Secshondeportament secshondeportament = db.Secshondeportament.Find(id);
            if (secshondeportament == null)
            {
                return HttpNotFound();
            }
            return View(secshondeportament);
        }

        // GET: Secshondeportaments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Secshondeportaments/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Podpisant")] Secshondeportament secshondeportament)
        {
            if (ModelState.IsValid)
            {
                db.Secshondeportament.Add(secshondeportament);                
                db.SaveChanges();

                var _secdep = db.Secshondeportament.Find(secshondeportament.Id);
                foreach (var item in db.Users.ToList())
                {
                    foreach (var _enumper in db.Enumpermission.ToList())
                    {
                        db.Permission.Add(new Permission { AppUser = item, EnumpermissionName = _enumper.Name, SecshondeportamentId = _secdep.Id, IsChecked = false });
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(secshondeportament);
        }

        // GET: Secshondeportaments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Secshondeportament secshondeportament = db.Secshondeportament.Find(id);
            if (secshondeportament == null)
            {
                return HttpNotFound();
            }
            return View(secshondeportament);
        }

        // POST: Secshondeportaments/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Podpisant")] Secshondeportament secshondeportament)
        {
            if (ModelState.IsValid)
            {
                db.Entry(secshondeportament).State = EntityState.Modified;               
                foreach (var item in db.Users.ToList())
                {
                    var _per = db.Permission.Where(o=>o.AppUser.Id == item.Id && o.SecshondeportamentId == secshondeportament.Id).FirstOrDefault();
                    if (_per == null)
                    {
                        db.Permission.Add(new Permission { AppUser = item, EnumpermissionName = "Reading", SecshondeportamentId = secshondeportament.Id, IsChecked =false });
                        db.Permission.Add(new Permission { AppUser = item, EnumpermissionName = "Signature", SecshondeportamentId = secshondeportament.Id, IsChecked = false });
                        db.Permission.Add(new Permission { AppUser = item, EnumpermissionName = "ToSignature", SecshondeportamentId = secshondeportament.Id, IsChecked = false });
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(secshondeportament);
        }

        // GET: Secshondeportaments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Secshondeportament secshondeportament = db.Secshondeportament.Find(id);
            if (secshondeportament == null)
            {
                return HttpNotFound();
            }
            return View(secshondeportament);
        }

        // POST: Secshondeportaments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Secshondeportament secshondeportament = db.Secshondeportament.Find(id);
            db.Secshondeportament.Remove(secshondeportament);
            db.Permission.RemoveRange(db.Permission.Where(o => o.SecshondeportamentId.Equals(secshondeportament.Id)).ToList());
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        


        public async Task<ActionResult> DetailSecshondeportament(int? id)
        {           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Secshondeportament secshondeportament = await db.Secshondeportament.Include(o => o.AppUsers).Where(o => o.Id == id).FirstOrDefaultAsync();
            if (secshondeportament == null)
            {
                return HttpNotFound();
            }
            if (secshondeportament.Podpisant != null)
            {
                var _user = await UserManager.FindByIdAsync(secshondeportament.Podpisant);
                ViewBag.Cert = _user.FirstName;
            }
            else
            {
                ViewBag.Cert = String.Empty;
            }
            

            return PartialView("DetailSecshondeportament", secshondeportament);
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
