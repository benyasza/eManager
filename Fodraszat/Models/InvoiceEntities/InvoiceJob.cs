using Fodraszat.Models.RequestObjects.DataObjects;
using Fodraszat.Services;

namespace Fodraszat.Models.InvoiceEntities
{
    public class InvoiceJob
    {
        public InvoiceJob(JobObject jobObject)
        {
            if (jobObject != null)
            {
                HairDresser = DataService.GetHairdresserById(jobObject.HairDresserId)?.Name ?? string.Empty;
                Job = DataService.GetJobById(jobObject.JobId)?.Name;
                Discount = jobObject.Discount;
                Price = InvoiceService.GetJobPriceWithDiscount(jobObject);
            }
        }

        public string HairDresser { get; set; }

        public string Job { get; set; }

        public int Discount { get; set; }

        public int Price { get; set; }
    }
}