using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Fodraszat.Models;

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
                var grouppedList = db.Invoices.GroupBy(t => t.InvoiceNr);

                foreach (var invoice in grouppedList)
                {
                    list.Add(new InvoiceListItem
                    {
                        InvoiceNumber = invoice.Key,
                        Date = invoice.First().Date,
                        Client = invoice.First().Client,
                        Price = invoice.Sum(t => t.Price)
                    });
                }
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

            IEnumerable<Invoices> invoices = db.Invoices.Where(t => t.InvoiceNr == invoiceNr.Value);
            if (!invoices?.Any() ?? true)
            {
                return HttpNotFound();
            }

            var jobs = invoices
                .Where(t => !(string.IsNullOrEmpty(t.Job)))
                ?.Select(s => new InvoiceDetailsRow
                {
                    HairDresser = GetHairDresserName(s.HairDresser),
                    Job = GetJobName(s.Job),
                    Price = s.Price
                });

            var materials = invoices
                .Where(t => !(string.IsNullOrEmpty(t.Material)))
                ?.Select(s => new InvoiceDetailsRow
                {
                    Material = GetMaterialName(s.Material),
                    Price = s.Price
                });

            InvoiceDetails details = new InvoiceDetails
            {
                InvoiceNumber = invoiceNr.Value,
                TotalCost = invoices.Sum(t => t.Price),
                Jobs = jobs,
                Materials = materials
            };

            return View(details);
        }

        private string GetHairDresserName(string id)
        {
            int recordId;
            int.TryParse(id, out recordId);

            if (recordId < 0)
            {
                return string.Empty;
            }

            return db.HairDressers.Find(recordId)?.Name ?? string.Empty;
        }

        private string GetJobName(string id)
        {
            int recordId;
            int.TryParse(id, out recordId);

            if (recordId < 0)
            {
                return string.Empty;
            }

            return db.Jobs.Find(recordId)?.Name ?? string.Empty;
        }

        private string GetMaterialName(string id)
        {
            int recordId;
            int.TryParse(id, out recordId);

            if (recordId < 0)
            {
                return string.Empty;
            }

            return db.Materials.Find(recordId)?.MaterialName ?? string.Empty;
        }

        // GET: Invoices/Create
        public ActionResult Create()
        {
            var materials = db.Materials?.Select(t => new ExtendedSelectListItem { Value = t.Id.ToString(), Text = t.MaterialName, Price = t.Price })
                .ToList() ?? new List<ExtendedSelectListItem>();
            materials.Insert(0, new ExtendedSelectListItem { Text ="Válassz", Value ="", Price = 0 });
            ViewData.Add("Materials", materials);

            var jobs = db.Jobs?.Select(t => new ExtendedSelectListItem { Value = t.Id.ToString(), Text = t.Name, Price = t.Cost })
                .ToList() ?? new List<ExtendedSelectListItem>();
            jobs.Insert(0, new ExtendedSelectListItem { Text = "Válassz", Value = "", Price = 0 });
            ViewData.Add("Jobs", jobs);


            var hairdressers = db.HairDressers?.Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name }).ToList() ?? new List<SelectListItem>();
            hairdressers.Insert(0, new SelectListItem { Text = "Válassz", Value = "" });
            ViewData.Add("HairDressers", hairdressers);
            return View();

        }

        // POST: Invoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InvoiceRequest request)
        {
            if (ModelState.IsValid)
            {
                var jobs = request.SelectedJobs.Split('|');
                var hairDressers = request.SelectedHairDressers.Split('|');

                int count = Math.Min(jobs.Count(), hairDressers.Count());

                var rand = new Random(DateTime.Now.Millisecond);
                int invoiceNr = rand.Next(10000000, 99999999);

                int price = 0;
                int id;

                // store records with matching job-hairdresser pairs, without material
                for (int i = 0; i < count; i++)
                {
                    int.TryParse(jobs[i], out id);

                    if (id < 0)
                    {
                        break;
                    }

                    price = db.Jobs.Where(t => t.Id == id)?.FirstOrDefault()?.Cost ?? 0;

                    var invoice = new Invoices
                    {
                        InvoiceNr = invoiceNr,
                        HairDresser = hairDressers[i],
                        Job = jobs[i],
                        Material = string.Empty,
                        Date = DateTime.Now,
                        Price = price,
                        Client = request.Client ?? string.Empty
                    };

                    db.Invoices.Add(invoice);
                }

                var materials = request.SelectedMaterials?.Split('|');

                // store seleted materials
                if (materials?.Any() ?? false)
                {
                    foreach (var material in materials)
                    {
                        int.TryParse(material, out id);

                        if (id < 0)
                        {
                            break;
                        }

                        price = db.Materials.Where(t => t.Id == id)?.FirstOrDefault()?.Cost ?? 0;

                        db.Invoices.Add(new Invoices
                        {
                            InvoiceNr = invoiceNr,
                            HairDresser = string.Empty,
                            Job = string.Empty,
                            Material = material,
                            Date = DateTime.Now,
                            Price = price,
                            Client = request.Client ?? string.Empty
                        });
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(request);
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
            Invoices invoices = db.Invoices.Find(id);
            db.Invoices.Remove(invoices);
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
