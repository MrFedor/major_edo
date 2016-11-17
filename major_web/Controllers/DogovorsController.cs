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
    public class DogovorsController : Controller
    {
        private UserContext db = new UserContext();

        // GET: Dogovors
        public ActionResult Index()
        {
            var dogovor = db.Dogovor.Include(d => d.Client);
            return View(dogovor.ToList());
        }

        // GET: Dogovors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dogovor dogovor = db.Dogovor.Find(id);
            if (dogovor == null)
            {
                return HttpNotFound();
            }
            return View(dogovor);
        }

        // GET: Dogovors/Create
        public ActionResult Create()
        {
            ViewBag.ClientId = new SelectList(db.Client, "Id", "Name");
            return View();
        }

        // POST: Dogovors/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DogovorNum,DogovorDate,ClientId")] Dogovor dogovor)
        {
            if (ModelState.IsValid)
            {
                db.Dogovor.Add(dogovor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientId = new SelectList(db.Client, "Id", "Name", dogovor.ClientId);
            return View(dogovor);
        }

        // GET: Dogovors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dogovor dogovor = db.Dogovor.Find(id);
            if (dogovor == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientId = new SelectList(db.Client, "Id", "Name", dogovor.ClientId);
            return View(dogovor);
        }

        // POST: Dogovors/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DogovorNum,DogovorDate,ClientId")] Dogovor dogovor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dogovor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClientId = new SelectList(db.Client, "Id", "Name", dogovor.ClientId);
            return View(dogovor);
        }

        // GET: Dogovors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dogovor dogovor = db.Dogovor.Find(id);
            if (dogovor == null)
            {
                return HttpNotFound();
            }
            return View(dogovor);
        }

        // POST: Dogovors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dogovor dogovor = db.Dogovor.Find(id);
            db.Dogovor.Remove(dogovor);
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
