using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using major_data;
using major_data.Models;

namespace major_web.Controllers
{
    public class FondsController : Controller
    {
        private UserContext db = new UserContext();

        // GET: Fonds
        public ActionResult Index()
        {
            return View(db.Fond.ToList());
        }

        // GET: Fonds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fond fond = db.Fond.Find(id);
            if (fond == null)
            {
                return HttpNotFound();
            }
            return View(fond);
        }

        // GET: Fonds/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Fonds/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,RegNum,LicNumber,NameFolderFoPath")] Fond fond)
        {
            if (ModelState.IsValid)
            {
                db.Fond.Add(fond);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fond);
        }

        // GET: Fonds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fond fond = db.Fond.Find(id);
            if (fond == null)
            {
                return HttpNotFound();
            }
            return View(fond);
        }

        // POST: Fonds/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,RegNum,LicNumber,NameFolderFoPath")] Fond fond)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fond).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fond);
        }

        // GET: Fonds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fond fond = db.Fond.Find(id);
            if (fond == null)
            {
                return HttpNotFound();
            }
            return View(fond);
        }

        // POST: Fonds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fond fond = db.Fond.Find(id);
            db.Fond.Remove(fond);
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
