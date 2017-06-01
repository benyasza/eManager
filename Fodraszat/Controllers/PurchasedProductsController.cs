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
    public class PurchasedProductsController : Controller
    {
        private HairDressEntities db = new HairDressEntities();

        // GET: PurchasedProducts
        public ActionResult Index()
        {
            return View(db.PurchasedProducts.ToList());
        }

        // GET: PurchasedProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchasedProducts purchasedProducts = db.PurchasedProducts.Find(id);
            if (purchasedProducts == null)
            {
                return HttpNotFound();
            }
            return View(purchasedProducts);
        }

        // GET: PurchasedProducts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PurchasedProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,UnitPrice")] PurchasedProducts purchasedProducts)
        {
            if (ModelState.IsValid)
            {
                db.PurchasedProducts.Add(purchasedProducts);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(purchasedProducts);
        }

        // GET: PurchasedProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchasedProducts purchasedProducts = db.PurchasedProducts.Find(id);
            if (purchasedProducts == null)
            {
                return HttpNotFound();
            }
            return View(purchasedProducts);
        }

        // POST: PurchasedProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,UnitPrice")] PurchasedProducts purchasedProducts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchasedProducts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(purchasedProducts);
        }

        // GET: PurchasedProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchasedProducts purchasedProducts = db.PurchasedProducts.Find(id);
            if (purchasedProducts == null)
            {
                return HttpNotFound();
            }
            return View(purchasedProducts);
        }

        // POST: PurchasedProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PurchasedProducts purchasedProducts = db.PurchasedProducts.Find(id);
            db.PurchasedProducts.Remove(purchasedProducts);
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
