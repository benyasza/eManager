using Fodraszat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fodraszat.Controllers
{
    public class StatisticsController : Controller
    {
        private HairDressEntities db = new HairDressEntities();

        public ActionResult Index()
        {
            var hairdressers = db.HairDressers?.Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name }).ToList() ?? new List<SelectListItem>();
            hairdressers.Insert(0, new SelectListItem { Text = "Válassz", Value = "" });
            ViewData.Add("HairDressers", hairdressers);

            return View();
        }
        [HttpPost]
        public ActionResult Index(Statistics stat)
        {
            var StartDate = stat.StartDate ?? DateTime.MinValue;
            var EndDate = stat.EndDate ?? DateTime.Now;
            //var HairDresser = stat.Hairdressers ?? 0;

            List<Invoices> statistics = db.Invoices.Where(x => x.Date >= StartDate && x.Date <= EndDate && !string.IsNullOrEmpty(x.HairDresser)).ToList();

            if (!string.IsNullOrEmpty(stat.Hairdressers))
            {
                statistics = statistics.Where(x => x.HairDresser == stat.Hairdressers).ToList();
            }

            statistics = statistics.Take(400).ToList();

            ViewData.Add("Stats", statistics);

            var hairdressers = db.HairDressers?.Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name }).ToList() ?? new List<SelectListItem>();
            hairdressers.Insert(0, new SelectListItem { Text = "Válassz", Value = "" });
            ViewData.Add("HairDressers", hairdressers);

            return View();
        }
    }
}