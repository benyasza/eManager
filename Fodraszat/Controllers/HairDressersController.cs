using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Fodraszat.Models;

namespace Fodraszat.Controllers
{
    public class HairDressersController : Controller
    {
        private HairDressEntities db = new HairDressEntities();

        // GET: HairDressers
        public ActionResult Index()
        {
            return View(db.HairDressers.ToList());
        }

        // GET: HairDressers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HairDressers hairDressers = db.HairDressers.Find(id);
            if (hairDressers == null)
            {
                return HttpNotFound();
            }
            return View(hairDressers);
        }

        // GET: HairDressers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HairDressers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Jobtitle")] HairDressers hairDressers)
        {
            if (ModelState.IsValid)
            {
                db.HairDressers.Add(hairDressers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hairDressers);
        }

        // GET: HairDressers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HairDressers hairDressers = db.HairDressers.Find(id);
            if (hairDressers == null)
            {
                return HttpNotFound();
            }
            return View(hairDressers);
        }

        // POST: HairDressers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Jobtitle")] HairDressers hairDressers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hairDressers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hairDressers);
        }

        // GET: HairDressers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HairDressers hairDressers = db.HairDressers.Find(id);
            if (hairDressers == null)
            {
                return HttpNotFound();
            }
            return View(hairDressers);
        }

        // POST: HairDressers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HairDressers hairDressers = db.HairDressers.Find(id);
            db.HairDressers.Remove(hairDressers);
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
