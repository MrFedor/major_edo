using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using major_data.Models;

namespace major_edo.Controllers
{
    public class ContragentController : Controller
    {
        private UserContext db = new UserContext();

        // GET: Contragent
        public ActionResult Index()
        {
            return View(db.m_Contragent.ToList());
        }

        // GET: Contragent/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            m_Contragent m_Contragent = db.m_Contragent.Find(id);
            if (m_Contragent == null)
            {
                return HttpNotFound();
            }
            return View(m_Contragent);
        }

        // GET: Contragent/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contragent/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Inn")] m_Contragent m_Contragent)
        {
            if (ModelState.IsValid)
            {
                db.m_Contragent.Add(m_Contragent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(m_Contragent);
        }

        // GET: Contragent/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            m_Contragent m_Contragent = db.m_Contragent.Find(id);
            if (m_Contragent == null)
            {
                return HttpNotFound();
            }
            return View(m_Contragent);
        }

        // POST: Contragent/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Inn")] m_Contragent m_Contragent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(m_Contragent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(m_Contragent);
        }

        // GET: Contragent/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            m_Contragent m_Contragent = db.m_Contragent.Find(id);
            if (m_Contragent == null)
            {
                return HttpNotFound();
            }
            return View(m_Contragent);
        }

        // POST: Contragent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            m_Contragent m_Contragent = db.m_Contragent.Find(id);
            db.m_Contragent.Remove(m_Contragent);
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
