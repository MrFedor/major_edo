using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using major_data.Models;
using major_data;

namespace major_web.Controllers
{
    public class EnumpermissionsController : Controller
    {
        private UserContext db = new UserContext();

        // GET: Enumpermissions
        public ActionResult Index()
        {
            return View(db.Enumpermission.ToList());
        }

        // GET: Enumpermissions/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enumpermission enumpermission = db.Enumpermission.Find(id);
            if (enumpermission == null)
            {
                return HttpNotFound();
            }
            return View(enumpermission);
        }

        // GET: Enumpermissions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Enumpermissions/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description")] Enumpermission enumpermission)
        {
            if (ModelState.IsValid)
            {
                db.Enumpermission.Add(enumpermission);
                foreach (var item in db.Users.ToList())
                {
                    foreach (var _enumper in db.Secshondeportament.Select(o=>o.Id).ToList())
                    {
                        db.Permission.Add(new Permission { AppUser = item, EnumpermissionName = enumpermission.Name, SecshondeportamentId = _enumper, IsChecked =false });
                    }                    
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(enumpermission);
        }

        // GET: Enumpermissions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enumpermission enumpermission = db.Enumpermission.Find(id);
            if (enumpermission == null)
            {
                return HttpNotFound();
            }
            return View(enumpermission);
        }

        // POST: Enumpermissions/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name,Description")]Enumpermission enumpermission)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enumpermission).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(enumpermission);
        }

        // GET: Enumpermissions/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enumpermission enumpermission = db.Enumpermission.Find(id);
            if (enumpermission == null)
            {
                return HttpNotFound();
            }
            return View(enumpermission);
        }

        // POST: Enumpermissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Enumpermission enumpermission = db.Enumpermission.Find(id);
            db.Enumpermission.Remove(enumpermission);
            var _per = db.Permission.Where(o => o.EnumpermissionName.Equals(enumpermission.Name)).ToList();
            db.Permission.RemoveRange(_per);
            db.SaveChanges();
            return RedirectToAction("Index");
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
