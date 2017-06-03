using Fodraszat.Models;
using Fodraszat.Models.InventoryEntities;
using Fodraszat.Models.RequestObjects;
using Fodraszat.Models.RequestObjects.DataObjects;
using Fodraszat.Models.ResponseObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fodraszat.Controllers
{
    public class InventoryController : Controller
    {
        private HairDressEntities db = new HairDressEntities();

        // GET: Inventory
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(InventoryRequest request)
        {
            var startDate = request?.StartDate ?? DateTime.MinValue;
            var endDate = request?.EndDate ?? DateTime.Now;

            List<Invoices> invoices = db.Invoices
                ?.Where(x =>
                x.Date >= startDate &&
                x.Date <= endDate)?.ToList() ?? new List<Invoices>();

            if (!invoices?.Any() ?? true)
            {
                return View();
            }

            var materials = new List<MaterialObject>();
            var products = new List<PurchasedProductObject>();

            foreach (var invoice in invoices)
            {
                var materialObjects = JsonConvert.DeserializeObject<IEnumerable<MaterialObject>>(invoice.Materials);
                var productObjects = JsonConvert.DeserializeObject<IEnumerable<PurchasedProductObject>>(invoice.PurchasedProducts);

                if (materialObjects?.Any() ?? false)
                {
                    materials.AddRange(materialObjects);
                }

                if (productObjects?.Any() ?? false)
                {
                    products.AddRange(productObjects);
                }
            }

            var response = new InventoryResponse();

            response.Materials = materials
                .GroupBy(t => t.Id)
                .Select(s => new InventoryMaterial(s.Key, s));

            response.Products = products
                .GroupBy(t => t.Id)
                .Select(s => new InventoryProduct(s.Key, s));

            ViewData.Add("Inventory", response);

            return View();
        }
    }
}