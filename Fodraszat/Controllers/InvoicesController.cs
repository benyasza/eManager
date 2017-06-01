using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Fodraszat.Models;
using Newtonsoft.Json;
using Fodraszat.Models.InvoiceEntities;
using Fodraszat.Services;
using Fodraszat.Models.RequestObjects;
using Fodraszat.Models.RequestObjects.DataObjects;
using System;

namespace Fodraszat.Controllers
{
    public class InvoicesController : Controller
    {
        private HairDressEntities db = new HairDressEntities();

        // GET: Invoices
        public ActionResult Index()
        {
            List<InvoiceListItem> list = new List<InvoiceListItem>();

            if (db.Invoices?.Any() ?? false)
            {
                list = db.Invoices.Select(t => new InvoiceListItem
                {
                    InvoiceNumber = t.InvoiceNr,
                    Date = t.Date,
                    Client = t.Client,
                    TotalPrice = t.TotalPrice,
                    PriceWithDiscount = t.PriceWithDiscount
                }).ToList();
            }

            return View(list);
        }

        // GET: Invoices/Details/5
        public ActionResult Details(int? invoiceNr)
        {
            if (invoiceNr == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var invoice = db.Invoices.First(t => t.InvoiceNr == invoiceNr.Value);
            if (invoice == null)
            {
                return HttpNotFound();
            }

            //var jobs = JsonConvert.DeserializeObject<IEnumerable<JobObject>>(invoice.Jobs)
            //    ?.Select(t => new JobEntity { Name = GetJobName(t.Id.ToString()), Price = t.Price, Discount = t.Discount });
            //var materials = JsonConvert.DeserializeObject<IEnumerable<MaterialObject>>(invoice.Materials)
            //    ?.Select(t => new MaterialEntity { Name = GetMaterialName(t.Id.ToString()), Price = t.Price, Quantity = t.Quantity });
            //var products = JsonConvert.DeserializeObject<IEnumerable<PurchasedProductObject>>(invoice.PurchasedProducts)
            //    ?.Select(t => new PurchasedProductEntity { Name = GetProductName(t.Id.ToString()), Price = t.Price, Quantity = t.Quantity });

            //InvoiceDetails details = new InvoiceDetails
            //{
            //    InvoiceNumber = invoiceNr.Value,
            //    TotalCost = invoice.TotalPrice,
            //    Jobs = jobs,
            //    Materials = materials,
            //    Products = products
            //};

            InvoiceDetails details = new InvoiceDetails();

            return View(details);
        }

        // GET: Invoices/Create
        public ActionResult Create()
        {
            var materials = db.Materials
                ?.Select(t => new ExtendedSelectListItem { Value = t.Id.ToString(), Text = t.Name, Price = t.UnitPrice })
                .ToList() ?? new List<ExtendedSelectListItem>();
            materials.Insert(0, new ExtendedSelectListItem { Text = "Válassz", Value = "", Price = 0 });
            ViewData.Add("Materials", materials);

            var jobs = db.Jobs
                ?.Select(t => new ExtendedSelectListItem { Value = t.Id.ToString(), Text = t.Name, Price = t.Price })
                .ToList() ?? new List<ExtendedSelectListItem>();
            jobs.Insert(0, new ExtendedSelectListItem { Text = "Válassz", Value = "", Price = 0 });
            ViewData.Add("Jobs", jobs);

            var products = db.PurchasedProducts
                ?.Select(t => new ExtendedSelectListItem { Value = t.Id.ToString(), Text = t.Name, Price = t.UnitPrice })
                .ToList() ?? new List<ExtendedSelectListItem>();
            products.Insert(0, new ExtendedSelectListItem { Text = "Válassz", Value = "", Price = 0 });
            ViewData.Add("Products", products);


            var hairdressers = db.HairDressers?.Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name }).ToList() ?? new List<SelectListItem>();
            hairdressers.Insert(0, new SelectListItem { Text = "Válassz", Value = "" });
            ViewData.Add("HairDressers", hairdressers);

            return View();

        }

        // POST: Invoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(InvoiceRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var rand = new Random(DateTime.Now.Millisecond);
            int invoiceNr = rand.Next(10000000, 99999999);

            Invoices invoice = new Invoices
            {
                InvoiceNr = invoiceNr,
                Client = request.Client,
                Date = DateTime.Now
            };

            List<JobObject> jobs = new List<JobObject>(); 
            foreach (var item in request.Jobs)
            {
                
                var job = InvoiceService.UpdateJob(item);
                jobs.Add(job);
            }

            List<MaterialObject> materials = new List<MaterialObject>();
            foreach (var item in request.Materials)
            {

                var material = InvoiceService.UpdateMaterial(item);
                materials.Add(material);
            }

            List<PurchasedProductObject> purchases = new List<PurchasedProductObject>();
            foreach (var item in request.Products)
            {

                var purchase = InvoiceService.UpdatePurchase(item);
                purchases.Add(purchase);
            }

            invoice.Jobs = JsonConvert.SerializeObject(jobs);
            invoice.Materials = JsonConvert.SerializeObject(materials);
            invoice.PurchasedProducts = JsonConvert.SerializeObject(purchases);

            int totalprice = 0;
            totalprice = jobs.Sum(x => x.Price);
            totalprice += materials.Sum(x => x.Price);
            totalprice += purchases.Sum(x => x.Price);
            invoice.TotalPrice = totalprice;

            int totalPriceWithDiscount =  InvoiceService.GetJobTotalPriceWithDiscount(jobs);
            totalPriceWithDiscount += materials.Sum(x => x.Price);
            totalPriceWithDiscount += purchases.Sum(x => x.Price);
            invoice.PriceWithDiscount = totalPriceWithDiscount;

            db.Invoices.Add(invoice);
            db.SaveChanges();

            return Json(Url.Action("Index", "Invoices"));
        }

        // GET: Invoices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoices invoices = db.Invoices.Find(id);
            if (invoices == null)
            {
                return HttpNotFound();
            }
            return View(invoices);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HairDresser,InvoiceNr,Job,Material,Date")] Invoices invoices)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoices).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(invoices);
        }

        // GET: Invoices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoices invoices = db.Invoices.Find(id);
            if (invoices == null)
            {
                return HttpNotFound();
            }
            return View(invoices);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Invoices invoices = db.Invoices.Find(id);
            //db.Invoices.Remove(invoices);
            //db.SaveChanges();
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
