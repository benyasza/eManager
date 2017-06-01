using Fodraszat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fodraszat.Models.RequestObjects;
using Newtonsoft.Json;
using Fodraszat.Models.RequestObjects.DataObjects;
using Fodraszat.Services;
using Fodraszat.Models.ResponseObjects;

namespace Fodraszat.Controllers
{
    public class StatisticsController : Controller
    {
        private HairDressEntities db = new HairDressEntities();

        public ActionResult Index()
        {
            var hairdressers = db.HairDressers?.Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name }).ToList() ?? new List<SelectListItem>();
            hairdressers.Insert(0, new SelectListItem { Text = "Fodrász", Value = "" });
            ViewData.Add("HairDressers", hairdressers);

            return View();
        }
        [HttpPost]
        public ActionResult Index(StatisticsRequest stat)
        {
            var hairdressers = db.HairDressers?.Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name }).ToList() ?? new List<SelectListItem>();
            hairdressers.Insert(0, new SelectListItem { Text = "Fodrász", Value = "" });
            ViewData.Add("HairDressers", hairdressers);

            var startDate = stat.StartDate ?? DateTime.MinValue;
            var endDate = stat.EndDate ?? DateTime.Now;
            var hairDresserId = stat.Hairdresser;

            string searchString = "";
            if (hairDresserId > 0)
            {
                searchString = $"{{\"HairDresserId\":{hairDresserId},";
            }

            List<Invoices> invoices = db.Invoices
                ?.Where(x => 
                x.Date >= startDate &&
                x.Date <= endDate &&
                x.Jobs.Contains(searchString))?.ToList() ?? new List<Invoices>();

            if (!invoices?.Any() ?? true)
            {
                return View();
            }

            List<StatisticsDetails> detailsList = new List<StatisticsDetails>();

            foreach (var invoice in invoices)
            {
                var jobObjects = JsonConvert.DeserializeObject<IEnumerable<JobObject>>(invoice.Jobs);

                if ((jobObjects?.Any() ?? false) && hairDresserId > 0)
                {
                    jobObjects = jobObjects.Where(t => t.HairDresserId == hairDresserId);
                }

                if ((jobObjects?.Any() ?? false))
                {
                    foreach (var jobObject in jobObjects)
                    {
                        var job = DataService.GetJobById(jobObject.JobId);

                        var statisticsDetails = new StatisticsDetails
                        {
                            HairDresser = DataService.GetHairdresserById(jobObject.HairDresserId)?.Name ?? string.Empty,
                            Date = invoice.Date,
                            Job = job.Name,
                            Price = jobObject.Price,
                            Overhead = job.Overhead,
                            Discount = jobObject.Discount,
                            Salary = InvoiceService.GetSalary(jobObject, job)
                        };

                        detailsList.Add(statisticsDetails);
                    }
                }
            }

            StatisticsResponse response = new StatisticsResponse
            {
                StatisticsDetails = detailsList
            };

            response.TotalPrice = detailsList.Sum(t => t.Price);
            response.TotalOverhead = detailsList.Sum(t => t.Overhead);
            response.TotalSalary = detailsList.Sum(t => t.Salary);

            ViewData.Add("Stats", response);

            return View();
        }
    }
}